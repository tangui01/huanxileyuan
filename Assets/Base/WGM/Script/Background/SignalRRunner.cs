using System;
using System.Threading.Tasks;
using UnityEngine;
using BestHTTP.SignalRCore;
using WGM;
using BestHTTP;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

public class SignalRRunner : MonoBehaviour
{
	public static SignalRRunner Instance { get; private set; }

	public Action<RespOnCoinIn> onCoinIn;
	public Action<RespOnAgencyBind> onAgencyBind;
	public Action<RespOnProperty> onProperty;
	public Action<RespRankingList> onRankingList;
	public Action onDisconnect;
	public Action onUploadFile;

    private LibSignalR mSignalR;
	private CancellationTokenSource mHeartBeatCts;

	void Awake()
	{
        if(Instance != null) {
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		StartAsync().WrapErrors();
	}

	async Task StartAsync()
	{
        await new WaitWhile(() => { return string.IsNullOrEmpty(LibWGM.machine.MachineId) || string.IsNullOrEmpty(LibWGM.machine.ApiKey); });
		
		mSignalR = new LibSignalR($"{BuildSceneBundle.apiUrlBase}hub/device", $"{BuildSceneBundle.apiUrlBase}api/auth/kidstoyauth",
			LibWGM.machine.MachineId, LibWGM.machine.ApiKey, RegistMethods);

		mHeartBeatCts = new CancellationTokenSource();
		HeartBeatMonitorAsync(mHeartBeatCts.Token).WrapErrors();
	}

	async Task HeartBeatMonitorAsync(CancellationToken stoppingToken)
	{
		int tryConnectTimes = 0;
		int heartBeatFailedTimes = 0;
		
		while(!stoppingToken.IsCancellationRequested) {
			await new WaitWhile(() => Application.internetReachability == NetworkReachability.NotReachable);

			try {
				if(!mSignalR.IsConnected) {
					if(!await mSignalR.StartAsync()) {
						tryConnectTimes++;
						await Task.Delay(TimeSpan.FromSeconds(Mathf.Clamp(Mathf.Pow(2, tryConnectTimes), 0, 300)), stoppingToken);
						continue;
					}
					tryConnectTimes = 0;
				}

				var resp = await HeartBeatAsync(new ReqHeartBeat {
					Time = DateTime.Now,
					OnlineCoinInCount = LibWGM.accountReport.OnlineCoinInCount,
					OfflineCoinInCount = LibWGM.accountReport.OfflineCoinInCount,
					OnlineTicketOutCount = LibWGM.accountReport.OnlineTicketOutCount,
					OfflineTicketOutCount = LibWGM.accountReport.OfflineTicketOutCount,
					OfflinePrize1OutCount = LibWGM.accountReport.OfflinePrize1OutCount,
					OfflinePrize2OutCount = LibWGM.accountReport.OfflinePrize2OutCount,
					OfflinePrize3OutCount = LibWGM.accountReport.OfflinePrize3OutCount,
				});
				if(resp.Code != 200) {
					Debug.LogError($"心跳监测:{resp.Message}");
					if(heartBeatFailedTimes++ > 3) {
						await mSignalR.StopAsync();
					}
					continue;
				}

				LibWGM.accountReport.OnlineCoinInCount = 0;
				LibWGM.accountReport.OfflineCoinInCount = 0;
				LibWGM.accountReport.OnlineTicketOutCount = 0;
				LibWGM.accountReport.OfflineTicketOutCount = 0;
				LibWGM.accountReport.OfflinePrize1OutCount = 0;
				LibWGM.accountReport.OfflinePrize2OutCount = 0;
				LibWGM.accountReport.OfflinePrize3OutCount = 0;

				heartBeatFailedTimes = 0;

				//时间不同步，则更新系统时间为服务器时间
				if(Mathf.Abs((resp.Time - DateTime.Now).Minutes) > 1) {
					LibUnityPlugin.SetDateTime(LibMisc.GetMilliseconds(resp.Time));
				}

				await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
			} catch(TaskCanceledException) {
				break;
			}
		}
	}

	private async void OnDestroy()
	{
		mHeartBeatCts?.Cancel();
		if(mSignalR != null) {
			await mSignalR.StopAsync();
		}
	}

	public bool IsConnected { get { return mSignalR != null ? mSignalR.IsConnected : false; } }

    private void RegistMethods(HubConnection hub)
	{
      
        hub.On<RespOnCoinIn>("OnCoinIn", p => OnCoinIn(p));        
        hub.On<RespOnAgencyBind>("OnAgencyBind", p => onAgencyBind?.Invoke(p));
		hub.On<RespOnProperty>("OnProperty", p => onProperty?.Invoke(p));
		hub.On<RespExtAction>("OnExtAction", p => OnExtAction(p));
		hub.On<RespRankingList>("OnRankingList", p => onRankingList?.Invoke(p));
		hub.On("OnDisconnect", () => onDisconnect?.Invoke());
	}

	async void OnCoinIn(RespOnCoinIn resp)
	{
        Debug.Log($"OnCoinIn Coins:{resp.Coins}");

		var ackResp = await Instance.ReceivedAckAsync(new ReqReceivedAck { MethodName = "OnCoinIn" });
		if(ackResp.Code != 200) {
			Debug.LogError($"ReceivedAck: {ackResp.Message}");
			return;
		}

		LibWGM.accountReport.OnlineCoinInCount += resp.Coins;

		onCoinIn?.Invoke(resp);
	}

	async void OnExtAction(RespExtAction resp)
	{
		if(resp.Code != 200) {
			Debug.Log($"OnExtAction: {resp.Message}");
			return;
		}

		var ackResp = await ReceivedAckAsync(new ReqReceivedAck { MethodName = "OnExtAction" });
		if(ackResp.Code != 200) {
			Debug.LogError($"ReceivedAck: {resp.Message}");
			return;
		}

        Debug.Log($"OnExtAction: {resp.Action}  data: {resp.Data}");
		switch(resp.Action) {
			case ExtActionCode.RefreshStore: break; 
			case ExtActionCode.Disconnect: break;
			case ExtActionCode.EnterBackground: break;
			case ExtActionCode.AutoTest: break;
			case ExtActionCode.Reboot: break;
			case ExtActionCode.UploadFile: onUploadFile?.Invoke(); break;
            case ExtActionCode.CoinAdjust: break;
       
            default:break;
		}
	}


	public async Task<RespBase> ReceivedAckAsync(ReqReceivedAck req)
    {
        return await mSignalR.CallAsync<RespBase>("ReceivedAck", req);
    }

    public async Task<RespConfig> ConfigAsync(ReqConfig req)
	{
		return await mSignalR.CallAsync<RespConfig>("Config", req);
	}

	public async Task<RespHeartBeat> HeartBeatAsync(ReqHeartBeat req)
	{
		return await mSignalR.CallAsync<RespHeartBeat>("HeartBeat", req);
	}

    public async Task<RespErrorReport> ErrorReportAsync(ReqErrorReport req)
    {
        return await mSignalR.CallAsync<RespErrorReport>("ErrorReport", req);
    }

	public async Task<RespRankingList> RankingListAsync(ReqRankingList req)
	{
		return await mSignalR.CallAsync<RespRankingList>("RankingList", req);
	}
}
