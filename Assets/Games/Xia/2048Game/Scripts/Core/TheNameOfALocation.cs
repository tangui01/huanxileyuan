//地图位置类
[System.Serializable]
public struct TheNameOfALocation
{ 
    //父类，其功能就是规划一个地图信息。
    public int RIndex { get; set; }

    public int CIndex { get; set; }
     
    public TheNameOfALocation(int rindex, int cindex):this()
    {
       this.RIndex = rindex;
       this.CIndex = cindex;
    }
} 
