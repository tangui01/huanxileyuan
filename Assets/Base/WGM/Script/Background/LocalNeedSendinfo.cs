using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/****************************************************
    文件：LocalNeedSendinfo.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：本地需要发送信息（在未成功发送的情况下）
*****************************************************/
[Serializable]
public class LocalNeedSendinfo 
{
    /// <summary>
    /// 主键
    /// </summary>
    [SimpleSQL.PrimaryKey, SimpleSQL.AutoIncrement]
    public int LocalNeedSendinfoId { get; set; }
    /// <summary>
    /// 要上传的币数
    /// </summary>
    public int ULCoinIn { get; set; }
    /// <summary>
    /// 要上传的出礼数
    /// </summary>
    public int ULCoinOut { get; set; }

    public int AddCcode { get; set; }
    public int AddCcodeCur { get; set; }

    public LocalNeedSendinfo()
    {
        ULCoinIn = 0;
        ULCoinOut = 0;
        AddCcodeCur = 0;
        AddCcode = 0x10;

    }

    public bool EqualTo(LocalNeedSendinfo before)
    {
        var beforeMembers = GetType().GetProperties();
        var afterMembers = before.GetType().GetProperties();
        for(int i = 0; i < beforeMembers.Length; i++) {
            var beforeVal = beforeMembers[i].GetValue(this, null);
            var afterVal = afterMembers[i].GetValue(before, null);
            var beforeValue = beforeVal?.ToString();
            var afterValue = afterVal?.ToString();
            if(beforeValue != afterValue) {
                return false;
            }
        }

        return true;
    }
    public LocalNeedSendinfo Clone()
    {
        using(MemoryStream stream = new MemoryStream()) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(stream) as LocalNeedSendinfo;
        }
    }
}
