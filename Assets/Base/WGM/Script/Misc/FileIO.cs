using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System;
using ICSharpCode.SharpZipLib.Zip;

public class FileIO {

	public static string Read(string path, int startLine = 0, int lines = 10000)
	{
		string[] str = File.ReadAllLines(path);
		string ret = "";

		int endLine = startLine + lines;
		endLine = Mathf.Min(endLine, str.Length);

		for(int i = startLine; i < endLine; i++) {
			ret += str[i] + "\r\n";
		}
		
		ret = ret.TrimStart('\r', '\n').TrimEnd('\r', '\n');

		return ret;
	}

	public static void Write(string path, string contents)
	{
		File.WriteAllText(path, contents, System.Text.Encoding.UTF8);
	}

	public static void CopyDirectory(string srcPath, string destPath)
	{
		try
		{
			if(!Directory.Exists(destPath)) {
				Directory.CreateDirectory(destPath);
			}

			DirectoryInfo dir = new DirectoryInfo(srcPath);
			FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
			foreach (FileSystemInfo i in fileinfo)
			{
				if (i is DirectoryInfo)     //判断是否文件夹
				{
						if (!Directory.Exists(destPath+"/"+i.Name))
						{
							Directory.CreateDirectory(destPath + "/" + i.Name);   //目标目录下不存在此文件夹即创建子文件夹
						}
						CopyDirectory(i.FullName, destPath + "/" + i.Name);    //递归调用复制子文件夹
				}
				else
				{
					
						File.Copy(i.FullName, destPath + "/" + i.Name,true);      //不是文件夹即复制文件，true表示可以覆盖同名文件
				}
			}
		}
		catch (System.Exception e)
		{
			throw e;
		}
	}

    public static void ClearDirectory(string srcPath)
    {
        if(!Directory.Exists(srcPath)) {
            return;
        }
        try {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
            foreach(FileSystemInfo i in fileinfo) {
                if(i is DirectoryInfo)     //判断是否文件夹
                {
                    DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                    subdir.Delete(true);
                } else {
                    File.Delete(i.FullName);
                }
            }
        } catch(System.Exception e) {
            throw e;
        }
    }

	public static void DeleteFileWithExt(string srcPath, string ext)
    {
        if(!Directory.Exists(srcPath)) {
            return;
        }
        try {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
            foreach(FileSystemInfo i in fileinfo) {
                if(i.Extension == ext) {
					File.Delete(i.FullName);
				}
            }
        } catch(System.Exception e) {
            throw e;
        }
    }

	public static bool UploadFileToFtp(string filePath, string ftpPath, string username, string password)
	{
		try {
			string serverFileName = ftpPath + Path.GetFileNameWithoutExtension(filePath) + DateTime.Now.ToString("-yyyy-MM-dd hh：mm：ss：fff") + Path.GetExtension(filePath);
			FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(serverFileName);
			ftp.Credentials = new NetworkCredential(username, password);
			ftp.KeepAlive = true;
			ftp.UseBinary = true;
			ftp.UsePassive = true;
			ftp.Method = WebRequestMethods.Ftp.UploadFile;
			FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			byte[] buffer = new byte[fs.Length];
			fs.Read(buffer, 0, buffer.Length);
			fs.Close();
			Stream ftpstream = ftp.GetRequestStream();
			ftpstream.Write(buffer, 0, buffer.Length);
			ftpstream.Close();
		} catch(Exception e) {
			Debug.Log("发送错误" + e.Message);
			return false;
		}

		return true;
	}

	public static bool UnZipMemory(byte[] bytes, string zipedFolder)
	{
		bool result = true;
		FileStream fs = null;
		ZipInputStream zipStream = null;
		ZipEntry ent = null;
		string fileName;

		if(bytes == null || bytes.Length <= 0)
			return false;

		if(!Directory.Exists(zipedFolder))
			Directory.CreateDirectory(zipedFolder);

		try {
			zipStream = new ZipInputStream(new MemoryStream(bytes));
			while((ent = zipStream.GetNextEntry()) != null) {
				if(!string.IsNullOrEmpty(ent.Name)) {
					fileName = Path.Combine(zipedFolder, ent.Name);
					if(fileName.EndsWith("/")) {
						Directory.CreateDirectory(fileName);
						continue;
					}

					fs = File.Create(fileName);
					int size = 2048;
					byte[] data = new byte[size];
					while(true) {
						size = zipStream.Read(data, 0, data.Length);
						if(size > 0)
							fs.Write(data, 0, size);
						else
							break;
					}
					fs.Close();
				}
			}
		} catch(Exception e) {
			result = false;
			Debug.LogError(e);
		} finally {
			if(fs != null) {
				fs.Close();
				fs.Dispose();
			}
			if(zipStream != null) {
				zipStream.Close();
				zipStream.Dispose();
			}
			if(ent != null) {
				ent = null;
			}
		}
		return result;
	}
}
