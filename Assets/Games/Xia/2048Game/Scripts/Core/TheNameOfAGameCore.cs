using System;
using System.Collections.Generic;

public class TheNameOfAGameCore  //与平台无关 
{
    //游戏地图
    private int[,] map;
    /// <summary>
    /// 游戏 4*4  方格地图
    /// </summary>
    public int[,] Map
    {
        get { return map; }
    }

    /// <summary>
    /// 创建游戏核心类对象
    /// </summary>
    public TheNameOfAGameCore()
    {
        map = new int[4, 4];//初始化地图设置
        mergeArr = new int[4];//合并
        removeZeroArr = new int[4];//去零数组
        emptyLOCList = new List<TheNameOfALocation>(16);//创建16大小的集合
        originalMap = new int[4, 4];//原始数组
        random = new Random();//随机生成数字

        moveDataList = new List<TheNameOfAMoveData>();//移动之后的数据
        mergeLocationList = new List<TheNameOfALocation>();//合并之后的集合
    }

    #region 移动
    private int moveRow, moveColumn;
    private TheNameOfAMoveDirection direction;
    /// <summary>
    /// 地图是否发生改变
    /// </summary>
    public bool IsChange { get; set; }
    private int[,] originalMap;//合并前记录的数组
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="dir"></param>
    public void Move(TheNameOfAMoveDirection dir)
    {
        if (dir == TheNameOfAMoveDirection.None)
        {
            return;
        }
        IsChange = false;

        Array.Copy(map, originalMap, map.Length);

        MergeLocationList.Clear();
        MoveDataList.Clear();

        direction = dir;

        switch (dir)
        {
            case TheNameOfAMoveDirection.Up: MoveUp(); break;
            case TheNameOfAMoveDirection.Down: MoveDown(); break;
            case TheNameOfAMoveDirection.Left: MoveLeft(); break;
            case TheNameOfAMoveDirection.Right: MoveRight(); break;
        }

        IsChange = CheckMapChange();
    }

    //检查地图是否改变
    private bool CheckMapChange()
    {
        for (int r = 0; r < 4; r++)
            for (int c = 0; c < 4; c++)
                if (this.map[r, c] != this.originalMap[r, c])
                    return true;

        return false;
    }

    private List<TheNameOfAMoveData> moveDataList;
    /// <summary>
    /// 需要移动的方格信息
    /// </summary>
    public List<TheNameOfAMoveData> MoveDataList
    {
        get
        { return this.moveDataList; }
    }

    //创建需要移动的方格数据对象
    private void CreateMoveData()
    {
        int zeroCount = 0;
        for (int i = 0; i < 4; i++)
        {
            if (mergeArr[i]== 0)
            {
                zeroCount++;
            }
            else
            {
                if (zeroCount != 0)
                {
                    TheNameOfALocation original = new TheNameOfALocation();
                    TheNameOfALocation target = new TheNameOfALocation();
                    switch (direction)
                    {
                        case TheNameOfAMoveDirection.Up:
                            original = new TheNameOfALocation(i, moveColumn);
                            target = new TheNameOfALocation(i - zeroCount, moveColumn);
                            break;
                        case TheNameOfAMoveDirection.Down:
                            original = new TheNameOfALocation(3 - i, moveColumn);
                            target = new TheNameOfALocation(3 - i + zeroCount, moveColumn);
                            break;
                        case TheNameOfAMoveDirection.Left:
                            original = new TheNameOfALocation(moveRow, i);
                            target = new TheNameOfALocation(moveRow, i - zeroCount);
                            break;
                        case TheNameOfAMoveDirection.Right:
                            original = new TheNameOfALocation(moveRow, 3 - i);
                            target = new TheNameOfALocation(moveRow, 3 - i + zeroCount);
                            break;
                    }
                    MoveDataList.Add(new TheNameOfAMoveData() { originalLoc = original, targetLoc = target });
                }
            }
        }
    }

    private void MoveUp()
    {//{2,2,4,4}  =>  {4,8 ,0 ,0 }
        //将map中的数据逐列从上向下取出，生成一个新一维数组 
        for (int c = 0; c < 4; c++)
        { 
            for (int r = 0; r < 4; r++)
                mergeArr[r] = map[r, c];

            moveColumn = c;//列
            //创建需要移动的数据
            CreateMoveData();
            //将取出行数据合并 
            Merge();

            //将合并后的数据从上向下放回原行
            for (int r = 0; r < 4; r++)
                map[r, c] = mergeArr[r];
        }
    }

    //下移
    private void MoveDown()
    {
        //将map中的数据逐行从右向左取出，生成一个新一维数组 
        for (int c = 0; c < 4; c++)
        { 
            for (int r = 3; r >= 0; r--)
                mergeArr[3 - r] = map[r, c];

            moveColumn = c; 
            CreateMoveData();
            //将取出行数据合并 
            Merge();

            //将合并后的数据从右向左放回原行
            for (int r = 3; r >= 0; r--)
                map[r, c] = mergeArr[3 - r];
        }
    }

    //左移
    private void MoveLeft()
    {
        //逐位比较  如果非零且不同 则销毁
        //将map中的数据逐行从左向右取出，生成一个新一维数组 
        for (int r = 0; r < 4; r++)
        { 
            for (int c = 0; c < 4; c++)
                mergeArr[c] = map[r, c];

            moveRow = r;
            CreateMoveData();
            //将取出行数据合并 
            Merge();

            //将合并后的数据从左向右放回原行
            for (int c = 0; c < 4; c++)
                map[r, c] = mergeArr[c];
        }
    }

    //右移
    private void MoveRight()
    {
        //将map中的数据逐行从右向左取出，生成一个新一维数组 
        for (int r = 0; r < 4; r++)
        {
            for (int c = 3; c >= 0; c--)
                mergeArr[3 - c] = map[r, c];

            moveRow = r;

            CreateMoveData();

            //将取出行数据合并
            Merge();

            //将合并后的数据从右向左放回原行
            for (int c = 3; c >= 0; c--)
                map[r, c] = mergeArr[3 - c];
        }
    }

    #endregion

    #region 合并
    //合并时使用的临时数组
    private int[] mergeArr;
    //去0时使用的临时数组
    private int[] removeZeroArr;

    /// <summary>
    /// 成绩
    /// </summary>
    public int Score { get; set; }

    private void Merge()
    {
        //1.去中间的0 
        RemoveZero();

        //2.检查相邻的元素是否相同
        for (int i = 0; i < mergeArr.Length - 1; i++)
        {
            //3.是
            if (mergeArr[i] != 0 && mergeArr[i] == mergeArr[i + 1])
            {
                mergeArr[i] += mergeArr[i + 1];  //将后一个元素加到前一个元素上，
                mergeArr[i + 1] = 0;      //将后一个元素清0

                Score += mergeArr[i];//计分 

                //记录合并的位置
                LogMergeLocation(i);
            }
        }
        RemoveZero();
    }

    //去零
    private void RemoveZero()
    {
        //新数组，里面所有元素为0
        Array.Clear(removeZeroArr, 0, removeZeroArr.Length);
        //原数组中的数据放入新数组的位置
        int index = 0;
        //检查原数组
        for (int i = 0; i < mergeArr.Length; i++)
        {
            //元素不为0，放入新数组
            if (mergeArr[i] != 0)
                removeZeroArr[index++] = mergeArr[i]; //新数组索引向后加1
            
        }
        removeZeroArr.CopyTo(mergeArr, 0);
    }

    private List<TheNameOfALocation> mergeLocationList;
    /// <summary>
    /// 需要合并的方格位置
    /// </summary>
    public List<TheNameOfALocation> MergeLocationList
    {
        get
        { return this.mergeLocationList; }
    }

    //记录合并位置
    private void LogMergeLocation(int mergeIndex)
    {//2 2 2 2  =》  4 0 4 0
        switch (direction)
        {
            case TheNameOfAMoveDirection.Up:
                //如果向上移动 mergeArr是每列数据(从上到下)
                if (mergeIndex > 0 && mergeArr[mergeIndex - 1] == 0)
                    mergeLocationList.Add(new TheNameOfALocation(mergeIndex - 1, moveColumn));
                else
                    mergeLocationList.Add(new TheNameOfALocation(mergeIndex, moveColumn));
                break;

            case TheNameOfAMoveDirection.Down:
                if (mergeIndex > 0 && mergeArr[mergeIndex - 1] == 0)
                    mergeLocationList.Add(new TheNameOfALocation(4 - mergeIndex, moveColumn));
                else
                    mergeLocationList.Add(new TheNameOfALocation(3 - mergeIndex, moveColumn));//3-0  3-1  3-2
                break;

            case TheNameOfAMoveDirection.Left:
                if (mergeIndex > 0 && mergeArr[mergeIndex - 1] == 0)
                    mergeLocationList.Add(new TheNameOfALocation(moveRow, mergeIndex - 1));
                else
                    mergeLocationList.Add(new TheNameOfALocation(moveRow, mergeIndex));
                break;

            case TheNameOfAMoveDirection.Right:
                if (mergeIndex > 0 && mergeArr[mergeIndex - 1] == 0)
                    mergeLocationList.Add(new TheNameOfALocation(moveRow, 4 - mergeIndex));
                else
                    mergeLocationList.Add(new TheNameOfALocation(moveRow, 3 - mergeIndex));
                break;
        }
    }
    #endregion

    #region 生成新数
    private List<TheNameOfALocation> emptyLOCList;//空位置 
    //随机找空位
    private Random random;

    /// <summary>
    /// 生成新数字
    /// </summary>
    /// <param name="number">生成的数字</param>
    /// <param name="loc">生成的位置</param>
    public void GenerateNumber(out int? number, out TheNameOfALocation? loc)
    {
        number = null;
        loc = null;
        CalculateEmpty();

        //如果还有空位
        if (emptyLOCList.Count > 0)
        {
            //从空位数组中随机选一个位置对象
            int randIndex = random.Next(0, emptyLOCList.Count);
            loc = emptyLOCList[randIndex];

            //将该位置的值赋为2或4 ,2机率高，4机率低  
            number = random.Next(1, 11) <= 1 ? 4 : 2;

            map[loc.Value.RIndex, loc.Value.CIndex] = number.Value;
            //将该位置从列表中清空
            emptyLOCList.RemoveAt(randIndex);
        }
    }

    //计算所有的空位置
    public void CalculateEmpty()
    {
        //清空记录的空位
        emptyLOCList.Clear();
        //逐行逐列扫描游戏地图每一个单元格
        for (int r = 0; r < 4; r++)
            for (int c = 0; c < 4; c++)
                if (map[r, c] == 0)
                    emptyLOCList.Add(new TheNameOfALocation(r, c));
    }
    #endregion

    /// <summary>
    /// 判断游戏是否结束
    /// </summary>
    /// <returns>结束状态</returns>
    public bool IsOver()
    {
        //还有空位，则游戏不结束
        if (emptyLOCList.Count > 0)
            return false;

        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (map[r, c] == map[r, c + 1] || map[c, r] == map[c + 1, r])
                    return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 清空游戏地图
    /// </summary>
    public void ClearMap()
    {
        Array.Clear(map, 0, map.Length);
    }
}
