using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using WGM;

public class Driver3229 
{
    public static Queue<KeyPara.KeyState> keyStates=new Queue<KeyPara.KeyState>();
    /// <summary>
    /// ?0??????????
    /// </summary>
    public static int prizeCount;
    public class KeyPara
    {
        public struct KeyState
        {
            /// <summary>
            /// ?????
            /// </summary>
            public int no;
            /// <summary>
            /// ??¦Ë????
            /// </summary>
            public int type;//????
            /// <summary>
            /// ??????
            /// </summary>
            public AppKeyCode appKeyCode;
        

         
            public int state;//??

            public int last;
            /// <summary>
            /// 0 ???  1 down 2  up  3 long
            /// </summary>
            public int keyCodeState;//????????
            public int down;//????
            public int up;//????
            public int longDown;//????
            public  int baseVR;

        }
        public const int longTime = 100;
        public const int downTime = 10;
        public const int upTime = 10;
        public int longTimer;
        public int curTime;
        public KeyState keyState;

        public KeyPara(int port ,AppKeyCode appKey)
        {
            keyState.no = port;
            keyState.appKeyCode = appKey;
            keyState.type = (int)appKey;
        }
        public KeyPara(int port, int appKey)
        {
            keyState.no = port;
            keyState.appKeyCode = (AppKeyCode)appKey;
            keyState.type = (int)appKey;
        }

        public bool IsDown()
        {
            return curTime >= downTime;
        }
        public bool IsUp()
        {
            return curTime >= upTime;
        }

        public bool IsLong(int milliseconds)//????????
        {
            longTimer += milliseconds;
            if (longTimer >= longTime)
            { longTimer = 0; return true; }
            return false;
        }
    }
    public static List<List<KeyPara>> keyParas;


    public int milliseconds;
    public Thread mThreadDriver;
    public int keysMaxCount = 1024;
    /// <summary>
    /// ?¦Ï???¦Ë
    /// </summary>
    public int baseVR;
    /// <summary>
    /// playerProts ???????????? check ?????? high ????¦Ë??? milliseconds ???
    /// </summary>
    /// <param name="prots"></param>
    public Driver3229(int[] playerProts,bool high=true,int ms = 5)
    {
        baseVR = high ? 0 : 1;//????????


        milliseconds = ms;
        keyParas = new List<List<KeyPara>>();
        for (int i = 0; i < playerProts.Length; i++)
        {
            keyParas.Add(new List<KeyPara>());
            for (int n = 0; n < 8; n++)
            {
                keyParas[i].Add(new KeyPara(playerProts[i],(AppKeyCode)n));
                keyParas[i][n].keyState.baseVR= high ? 0 : 1;
            }
        }

      
        mThreadDriver = new Thread(Update);
        mThreadDriver.Start();
    }
    public void Abort()
    {
        mThreadDriver?.Abort();
   
    }
    int a;
    public void Update()
    {
        while (true)
        {
            Thread.Sleep(milliseconds);
            a++;
            for (int i = 0; i < keyParas.Count; i++)
            {
                for (int j = 0; j < keyParas[i].Count; j++)
                {
                    int cur = LibWGM.Rk3229GetKey(keyParas[i][j].keyState.no, keyParas[i][j].keyState.type);
                   
                    if (a < 200 && i == 0 && j == 0)
                    {
                        cur = 1;
                    }
           
                    if (keyParas[i][j].keyState.state != cur)
                    {
                        keyParas[i][j].curTime = 0;
                    }

                    keyParas[i][j].keyState.state = cur;
                    if (keyParas[i][j].keyState.state != baseVR )
                    {
                   
                        keyParas[i][j].curTime += milliseconds;
                        if (keyParas[i][j].IsDown()&& keyParas[i][j].keyState.keyCodeState == 0)
                        {
                            keyParas[i][j].keyState.keyCodeState = 1;
                            
                            keyStates.Enqueue(keyParas[i][j].keyState);
                            keyParas[i][j].longTimer = 0;
                            
                            if (i == 1 && j == 4 || i == 1 && j == 4)
                                prizeCount--;
                            if (prizeCount<=0)
                            {
                                LibWGM.Rk3229SetGpio(0, 0, 0);//???
                                LibWGM.Rk3229SetGpio(1, 0, 0);
                            }
                        }
                        if (keyParas[i][j].IsLong(milliseconds) && keyParas[i][j].keyState.keyCodeState != 0 )
                        {

                            keyParas[i][j].keyState.keyCodeState = 2;
                            keyStates.Enqueue(keyParas[i][j].keyState);
                            
                            
                        }
                    }
                    else {
                        
                         keyParas[i][j].curTime += milliseconds;
                        if (keyParas[i][j].IsUp()&& keyParas[i][j].keyState.keyCodeState != 0)
                        {
                            keyParas[i][j].keyState.keyCodeState = 3;
                            keyStates.Enqueue(keyParas[i][j].keyState);
                            keyParas[i][j].keyState.keyCodeState = 0;
                            
                        }
                       

                    }
               
                }

            }
            
           
        }
    }
 

   

}

