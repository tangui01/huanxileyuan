using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

/****************************************************
    文件：Model.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace TetrisGame
{
    public class Model : SerializedMonoBehaviour
    {
        public int Score { get; private set; } //得分
        public float ShapeDownSpeed { get; private set; } = 0.8f; //方块下落速度
        public const int NORMAL_ROWS = 20;
        public const int MAX_ROWS = 23;
        public const int MAX_COLUMNS = 14;
        [SerializeField]private Star[,] map = new Star[MAX_COLUMNS, MAX_ROWS];

        public Action<int, int> OnScoreChanged;

        public Action OnClearLine;
        public Action<int> OnShapeDownSpeedChanged;
        public Action GameoverAction;
        private int Speed = 1;

        public Animator animator;

        private bool generateStar = true;

        private void Start()
        {
            generateStar = true;
        }

        public void AddScore(int value, int y)
        {
            Score += value;
            Level();
            OnScoreChanged?.Invoke(Score, y);
        }

        private void Level()
        {
            if (Score <=200)
            {
                ShapeDownSpeed = 0.8f;
                SetShapeDownSpeed(1);
            }
            else if (Score < 400 && Score >200)
            {
                ShapeDownSpeed = 0.6f;
                SetShapeDownSpeed(2);
            }
            else if (Score < 600 && Score >=400)
            {
                ShapeDownSpeed = 0.4f;
                SetShapeDownSpeed(3);
            }
            else if (Score < 800 && Score >=600)
            {
                ShapeDownSpeed = 0.2f;
                SetShapeDownSpeed(4);
            }
            else if (Score >=800&&Score<1000)
            {
                ShapeDownSpeed = 0.1f;
                SetShapeDownSpeed(5);
            }
            else
            {
                ShapeDownSpeed = 0.05f;
                SetShapeDownSpeed(6);
            }
        }

        public void SetShapeDownSpeed(int value)
        {
            Speed = value;
            OnShapeDownSpeedChanged?.Invoke(Speed);
        }

        public bool IsValidMapPosition(List<Star> t)
        {
            foreach (Star child in t)
            {
                if (child.tag != "Block") continue;
                Vector2 pos = new Vector2(Mathf.RoundToInt(child.transform.position.x),
                    Mathf.RoundToInt(child.transform.position.y));
                if (IsInsideMap(pos) == false) return false;
                if (map[(int)pos.x, (int)pos.y] != null) return false;
            }

            return true;
        }
/// <summary>
/// 是否左边或者右边越界
/// </summary>
/// <param name="t"></param>
/// <returns></returns>
        public bool IScrosstheborder(List<Star> t)
        {
            foreach (Star child in t)
            {
                Vector2 pos = new Vector2(Mathf.RoundToInt(child.transform.position.x),
                    Mathf.RoundToInt(child.transform.position.y));
                if (pos.x <0 || pos.x >MAX_COLUMNS)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsValidMapPosition(List<Vector3> t)
        {
            foreach (var child in t)
            {
                Vector2 pos = new Vector2(Mathf.RoundToInt(child.x),
                    Mathf.RoundToInt(child.y));
                if (IsInsideMap(pos) == false) return false;
                if (map[(int)pos.x, (int)pos.y] != null) return false;
            }
            return true;
        }

        public int GetMap(List<Star> t)
        {
            foreach (Star child in t)
            {
                if (child.transform.position.x<0)
                {
                    return 1;
                }
                else if (child.transform.position.x>MAX_COLUMNS)
                {
                    return -1;
                }
                
            }
            return 0;
        }

        public bool IsInsideMap(Vector2 pos)
        {
            return pos.x >= 0 && pos.x < MAX_COLUMNS && pos.y >= 0&&pos.y<=NORMAL_ROWS;
        }
        public void PlaceShape(List<Star> t)
        {
            foreach (Star child in t)
            {
                Vector2 pos = new Vector2(Mathf.RoundToInt(child.transform.position.x),Mathf.RoundToInt(child.transform.position.y) );
                ;
                map[(int)pos.x, (int)pos.y] = child;
            }
            animator.SetTrigger("Shake");
            CheckMap();
        }

        //检查地图是否不要消除行
      
        private void CheckMap()
        {
            bool isFull = false;
            bool Down = false;
            for (int i = 0; i < MAX_ROWS; i++)
            {
                isFull = CheckIsRowFull(i);
               
                if (isFull)
                {
                      DeleteRow(i);
                      generateStar=false;
                      MoveDownStar(i+1);
                      i--;
                      OnClearLine?.Invoke();
                }
            }
        }
/// <summary>
/// 将连续的上层方块看成一个集合，从StartRow -TargteRow  向下移动Count
/// </summary>
/// <param name="SatrtRow"></param>
/// <param name="Tagrerow"></param>
/// <param name="Count"></param>
        private void MoveDown(int SatrtRow, int Tagrerow,int Count)
        {
            for (int i =SatrtRow; i < Tagrerow; i++)
            {
                for (int j = 0; j < MAX_COLUMNS; j++)
                {
                    if (map[j, i] != null)
                    {
                        map[j, i-Count] = map[j, i];
                        map[j, i] = null;
                        map[j, i - Count].transform.DOMoveY( map[j, i - Count].transform.position.y -Count, 0.1f);
                    }
                }
            }
        }

        private void MoveDownStar(int row)
        {
            for (int i = row; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLUMNS; j++)
                {
                    if (map[j, i] != null)
                    {
                        map[j, i-1] = map[j, i];
                        map[j, i] = null;
                        map[j, i - 1].transform.position += Vector3.down;
                    }
                }
            }
        }
        private void DeleteRow(int row)
        {
            for (int i = 0; i < MAX_COLUMNS; i++)
            {
                map[i, row].AddScoreEf();
                map[i, row] = null;
            }
            AddScore(20, row);
            ClearFx();
            Invoke("SetGenerateStar",1f);
        }
        /// <summary>
        /// 消除方块时顿帧
        /// </summary>
        public void ClearFx()
        {
            StartCoroutine("onClearFx");
        }

        IEnumerator onClearFx()
        {
            Time.timeScale = 0.1f;
            yield return new WaitForSeconds(0.02f);
            Time.timeScale = 1f;
        }

        private bool CheckIsRowFull(int row)
        {
            for (int i = 0; i < MAX_COLUMNS; i++)
            {
                if (map[i, row] == null)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsGameOver()
        {
            for (int i = NORMAL_ROWS; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLUMNS; j++)
                {
                    if (map[j, i] != null)
                    {
                        GameoverAction?.Invoke();
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnDestroy()
        {
            OnScoreChanged = null;
            OnScoreChanged = null;
        }

        /// <summary>
        /// 是否能产生星星
        /// </summary>
        /// <returns></returns>
        public bool CanGenerateStar()
        {
            return generateStar;
        }

        public void SetGenerateStar()
        {
            generateStar = true;
        }
    }
}