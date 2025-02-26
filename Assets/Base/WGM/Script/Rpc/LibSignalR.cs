using System;
using UnityEngine;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using BestHTTP;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using BestHTTP.Connections;

namespace WGM
{
	public class LibSignalR
	{
		readonly Uri URI;
		readonly HubConnection hub;

		public bool IsConnected { get; private set; }

        public LibSignalR(string url, string authUrl, string machineId, string apikey, Action<HubConnection> onMethod)
		{
			URI = new Uri(url);
			IsConnected = false;
			
			hub = new HubConnection(URI, new JsonProtocol(new NewtonEncoder()), new HubOptions { SkipNegotiation = true, ConnectTimeout = TimeSpan.FromSeconds(20) }) {
			//hub = new HubConnection(URI, new MessagePackProtocol(), new HubOptions { SkipNegotiation = true, ConnectTimeout = TimeSpan.FromSeconds(20) }) {
				AuthenticationProvider = new PreAuthAccessTokenAuthenticator(new Uri(authUrl), machineId, apikey),
			};
			hub.AuthenticationProvider.OnAuthenticationSucceded += OnAuthenticationSucceded;
			hub.AuthenticationProvider.OnAuthenticationFailed += OnAuthenticationFailed;
			hub.OnConnected += OnConnected;
			hub.OnReconnecting += OnReconnecting;
			hub.OnReconnected += OnReconnected;
			hub.OnMessage += OnMessage;
			hub.OnError += OnError;
			hub.OnClosed += OnClosed;
			
			onMethod?.Invoke(hub);
		}

		public async Task<bool> StartAsync()
		{
			if(IsConnected) {
				return true;
			}

			bool isComplete = false;
			bool isError = false;
			
			hub.Reset();
			hub.StartConnect();

			hub.OnConnected += OnConnected;
			hub.OnError += OnError;
			
			float time = 0;
			float timeOut = 30.0f;
			await new WaitUntil(() => { time += Time.unscaledDeltaTime; return isComplete || isError || time > timeOut; });

			hub.OnConnected -= OnConnected;
			hub.OnError -= OnError;

			if(time > timeOut) {
				Debug.LogError($"LibSignalR Start: timeout{time}");
				await StopAsync();
			}
			
			void OnConnected(HubConnection hub) {
				isComplete = true;
			}

			void OnError(HubConnection hub, string error) {
				Debug.LogError($"LibSignalR Start: {error}");
				isError = true;
			}

			return isComplete;
		}

		public async Task StopAsync()
		{
			bool isComplete = false;
			bool isError = false;
            
			hub.OnClosed += OnClosed;
			hub.OnError += OnError;

			hub.StartClose();

			float time = 0;
			float timeOut = 2.0f;
			await new WaitUntil(() => { time += Time.unscaledDeltaTime; return isComplete || isError || time > timeOut; });

			hub.OnClosed -= OnClosed;
			hub.OnError -= OnError;

			if(!isComplete) {
				hub.Reset();
			}

			void OnClosed(HubConnection hub) {
				isComplete = true;
			}

			void OnError(HubConnection hub, string error) {
				Debug.LogError($"LibSignalR Stop: {error}");
				isError = true;
			}

			IsConnected = false;
		}

        public async Task<T> CallAsync<T>(string methodName, params object[] args) where T : RespBase, new()
		{
			if(!IsConnected) {
				return new T().SetFailed("未连接", 404) as T;
			}

			T result = new T();
			bool isComplete = false;
			bool isError = false;
			string errMsg = "";

			hub.Invoke<T>(methodName, args)
				.OnSuccess(res => result = res)
				.OnError(err => { isError = true; errMsg = err.Message; })
				.OnComplete(ret => isComplete = true);

			float time = 0;
			float timeOut = 30.0f;
			await new WaitUntil(() => { time += Time.unscaledDeltaTime; return isComplete || isError || !IsConnected || time > timeOut; });

			if(isError) {
				return new T().SetFailed(errMsg, 400) as T;
			}

			if(!IsConnected) {
				return new T().SetFailed("未连接", 404) as T;
			}

			if(time >= timeOut) {
				return new T().SetFailed("请求超时", 408) as T;
			}

			return result;
		}

		private void OnAuthenticationSucceded(IAuthenticationProvider provider)
		{
			Debug.Log(string.Format("Pre-Authentication Succeded! Token: '{0}' \n", (hub.AuthenticationProvider as PreAuthAccessTokenAuthenticator).Token));
		}

		private void OnAuthenticationFailed(IAuthenticationProvider provider, string reason)
		{
			Debug.Log(string.Format("Authentication Failed! Reason: '{0}'\n", reason));
		}

		private void OnConnected(HubConnection hub)
		{
			IsConnected = true;
			Debug.Log("Hub Connected\n");
        }

		private void OnReconnecting(HubConnection hub, string msg)
		{
            Debug.Log(this.ToString() + "OnReconnecting()");
            IsConnected = false;
			Debug.Log($"Hub Reconnecting {msg}\n");
		}

		private void OnReconnected(HubConnection hub)
		{
			IsConnected = true;
			Debug.Log("Hub Reconnected\n");
		}

		private bool OnMessage(HubConnection hub, BestHTTP.SignalRCore.Messages.Message message)
		{
			return true;
		}

		private void OnClosed(HubConnection hub)
		{
            //UnityEngine.Debug.Log(this.ToString() + "OnClosed()");
            IsConnected = false;
			Debug.Log("Hub Closed\n");
		}

		private void OnError(HubConnection hub, string error)
		{
           // UnityEngine.Debug.Log(this.ToString() + "OnError()");
            IsConnected = false;
			Debug.Log("Hub Error: " + error + "\n");
		}

		public sealed class PreAuthAccessTokenAuthenticator : IAuthenticationProvider
		{
			public bool IsPreAuthRequired {
				get {
					try {
						var bytes = Convert.FromBase64String(Token.Split('.')[1]);
						//var pl = JsonConvert.DeserializeObject<JwtPayload>(Encoding.UTF8.GetString(bytes));
						  
						var pl = JsonUtility.FromJson<JwtPayload>(Encoding.UTF8.GetString(bytes));
						
						var dt = new DateTime(1970, 1, 1).AddSeconds(pl.exp);
						return DateTime.Now > dt;
					} catch {
						return true;
					}
				}
			}

			public class JwtPayload
			{
				public string nameid;
				public string unique_name;
				public string role;
				public int exp;
			}

			public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

			public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

			private readonly Uri authenticationUri;

			public string Token { get; private set; }

			private readonly string MachineId;

			private readonly string ApiKey;

			public PreAuthAccessTokenAuthenticator(Uri authUri, string machineId, string apikey)
			{
				authenticationUri = authUri;
				MachineId = machineId;
				ApiKey = apikey;
			}

			public void StartAuthentication()
			{
				var request = new HTTPRequest(authenticationUri, HTTPMethods.Post, OnAuthenticationRequestFinished);
				request.SetHeader("Content-Type", "application/json");
				request.RawData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { MachineId, ApiKey }));
				request.Send();
			}

			private void OnAuthenticationRequestFinished(HTTPRequest req, HTTPResponse resp)
			{
				switch(req.State) {
					case HTTPRequestStates.Finished:
						if(resp.IsSuccess) {
							JObject jo = JObject.Parse(resp.DataAsText);
							int code = (int)jo["code"];
							if(code == 200) {
								Token = (string)jo["data"];
								OnAuthenticationSucceded?.Invoke(this);
							} else {
								AuthenticationFailed((string)jo["message"]);
							}
						} else {
							AuthenticationFailed(string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
															resp.StatusCode,
															resp.Message,
															resp.DataAsText));
						}
						break;

					// The request finished with an unexpected error. The request's Exception property may contain more info about the error.
					case HTTPRequestStates.Error:
						AuthenticationFailed("Request Finished with Error! " + (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
						break;

					// The request aborted, initiated by the user.
					case HTTPRequestStates.Aborted:
						AuthenticationFailed("Request Aborted!");
						break;

					// Connecting to the server is timed out.
					case HTTPRequestStates.ConnectionTimedOut:
						AuthenticationFailed("Connection Timed Out!");
						break;

					// The request didn't finished in the given time.
					case HTTPRequestStates.TimedOut:
						AuthenticationFailed("Processing the request Timed Out!");
						break;
				}
			}

			private void AuthenticationFailed(string reason)
			{
				OnAuthenticationFailed?.Invoke(this, reason);
			}

			public void PrepareRequest(HTTPRequest request)
			{
				if(HTTPProtocolFactory.GetProtocolFromUri(request.CurrentUri) == SupportedProtocols.HTTP)
					request.Uri = PrepareUri(request.Uri);
			}

			public Uri PrepareUri(Uri uri)
			{
				if(!string.IsNullOrEmpty(Token)) {
					string query = string.IsNullOrEmpty(uri.Query) ? "?" : uri.Query + "&";
					UriBuilder uriBuilder = new UriBuilder(uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath, query + "access_token=" + this.Token);
					return uriBuilder.Uri;
				} else
					return uri;
			}

            public void Cancel()
            {
                throw new NotImplementedException();
            }
        }
	}
}
