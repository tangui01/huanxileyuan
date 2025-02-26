using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using ZXing;
using ZXing.QrCode;
using UnityEngine.Video;
using ZXing.QrCode.Internal;

namespace WGM
{
	public class LibMisc
	{

		public static Sprite GetQrCode(string textForEncoding)
		{
			var writer = new BarcodeWriter {
				Format = BarcodeFormat.QR_CODE,
				Options = new QrCodeEncodingOptions {
					Height = 256,
					Width = 256,
					Margin = 0,
					DisableECI = true,
				}
			};
			Color32[] cols = writer.Write(textForEncoding);
			Texture2D tex = new Texture2D(256, 256);
			tex.SetPixels32(cols);
			tex.Apply();
			return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
		}

		/// <summary>
		/// 加载本地Sprite
		/// </summary>
		/// <param name="path">本地路径</param>
		/// <returns></returns>
		public static async Task<Sprite> LoadSpriteAsync(string path)
		{
			if(string.IsNullOrEmpty(path) || !File.Exists(path)) {
				return null;
			}

			using(FileStream stream = File.OpenRead(path)) {
				byte[] bytes = new byte[stream.Length];
				await stream.ReadAsync(bytes, 0, bytes.Length);
				Texture2D tex = new Texture2D(2, 2);
				tex.LoadImage(bytes);
				tex.Apply();
				return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
			}
		}

		/// <summary>
		/// 下载资源到本地
		/// </summary>
		/// <param name="path">本地存储路径</param>
		/// <param name="url">远程资源路径</param>
		/// <returns></returns>
		public static async Task<byte[]> DownloadResourceAsync(string path, string url, Action<float> progress = null)
		{
			var request = UnityWebRequest.Get(url);
			var op = request.SendWebRequest();
            float loadProgress = 0;
            int timeCount=0;
            while (!request.isDone) {
				progress?.Invoke(request.downloadProgress);
                if (loadProgress == request.downloadProgress)
                {
                    timeCount++;
                    if (timeCount > 2000)
                    {
                        Debug.LogError($"download {url} has an network error");
                        return null;
                    }
                }
                else {
                    loadProgress = request.downloadProgress;
                    timeCount = 0;
                }
               // await new WaitForSeconds(5);
                await new WaitForEndOfFrame();
            }

			await op;

			if(request.isNetworkError) {
				Debug.LogError($"download {url} has an network error");
				return null;
			} else if(request.isHttpError) {
				Debug.LogError($"download {url} has an http error");
				return null;
			}
			
			byte[] bytes = request.downloadHandler.data;
			Debug.Log("DownloadResource url = " + url + " datalen = " + request.downloadHandler.data?.Length);
            Debug.Log("url " + url);
            Debug.Log("dict path "+ path);
            FileInfo fi = new FileInfo(path);
			if(!fi.Directory.Exists) {
				fi.Directory.Create();
			}
			if(File.Exists(path)) {
				File.Delete(path);
			}
			using(FileStream stream = File.Open(path, FileMode.CreateNew)) {
				await stream.WriteAsync(bytes, 0, bytes.Length);
			}

			return bytes;
		}

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

		public static string Md5Sign(string input)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			var hash = MD5.Create().ComputeHash(bytes);
			string sign = "";
			for(int i = 0; i < hash.Length; i++) {
				sign += hash[i].ToString("x2");
			}
			return sign;
		}

		public static string MD5Stream(string filePath)
		{
			FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			md5.ComputeHash(fs);
			fs.Close();

			byte[] b = md5.Hash;
			md5.Clear();

			StringBuilder sb = new StringBuilder(32);
			for(int i = 0; i < b.Length; i++) {
				sb.Append(b[i].ToString("x2"));
			}

			string str = sb.ToString();
			return str;
		}

		public static long GetMilliseconds(DateTime dateTime)
		{
			return (long)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
		}

		public static string DesEncrypt(string encryptString)
		{
			byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
			byte[] rgbKey = Encoding.UTF8.GetBytes("huacaizn.com".Substring(0, 8));//转换为字节
			byte[] rgbIV = Keys;
			byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
			DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();//实例化数据加密标准
			MemoryStream mStream = new MemoryStream();//实例化内存流
													  //将数据流链接到加密转换的流
			CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
			cStream.Write(inputByteArray, 0, inputByteArray.Length);
			cStream.FlushFinalBlock();
			byte[] array = mStream.ToArray();
			return Convert.ToBase64String(array);
		}

		public static string DesDecrypt(string decryptString)
		{
			byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
			byte[] rgbKey = Encoding.UTF8.GetBytes("huacaizn.com".Substring(0, 8));
			byte[] rgbIV = Keys;
			byte[] inputByteArray = Convert.FromBase64String(decryptString);
			DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
			MemoryStream mStream = new MemoryStream();
			CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
			cStream.Write(inputByteArray, 0, inputByteArray.Length);
			cStream.FlushFinalBlock();
			return Encoding.UTF8.GetString(mStream.ToArray());
		}

		public static string GetCrc16(string str)
		{
			ushort polynomial = 0xA001;
			ushort[] table = new ushort[256];

			ushort value;
			ushort temp;
			for(ushort i = 0; i < table.Length; ++i) {
				value = 0;
				temp = i;
				for(byte j = 0; j < 8; ++j) {
					if(((value ^ temp) & 0x0001) != 0) {
						value = (ushort)((value >> 1) ^ polynomial);
					} else {
						value >>= 1;
					}
					temp >>= 1;
				}
				table[i] = value;
			}

			byte[] bytes = Encoding.UTF8.GetBytes(str);
			ushort crc = 0;
			for(int i = 0; i < bytes.Length; ++i) {
				byte index = (byte)(crc ^ bytes[i]);
				crc = (ushort)((crc >> 8) ^ table[index]);
			}
			string result = crc.ToString("X4");
			return result;
		}

		public static Sprite Url2Sprite(string url)
		{
			var writer = new BarcodeWriter {
				Format = BarcodeFormat.QR_CODE,
				Options = new QrCodeEncodingOptions {
					Height = 256,
					Width = 256,
					Margin = 0,
					ErrorCorrection = ErrorCorrectionLevel.H,
				}
			};
			Color32[] color32 = writer.Write(url);

			Texture2D tex = new Texture2D(writer.Options.Width, writer.Options.Height);
			tex.SetPixels32(color32);
			tex.Apply();
			return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
		}
	}
}