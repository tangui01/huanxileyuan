using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using WGM;

/****************************************************
    文件：Shape.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace TetrisGame
{
    public enum ShapeType
    {
        current,
        Down,
        Next,
    }
    public class Shape : MonoBehaviour
    {
        
        [SerializeField]private List<SpriteRenderer> srs;
        [SerializeField] private Transform RotateCenter;
        [SerializeField]private GameObject DownMain;
        public Action OnDownAction; //每一步下落要执行的事件

         private Controller ctrl;
         private ShapeManager shapeManager;
         private float timer;
         private bool isDown;
        
         private ShapeType shapeType;
        
         private float UpdaownTimer;
         private float MoveLeftTimer; 
         private float MoveRightTimer;
         

         private Color _color;
        
         [SerializeField]private List<Star> stars;
         [SerializeField] private Star star;
         
         [SerializeField]private ShapeStarPos shapeStarPos;

         private Shape DownShapeObj;
         private float DownDelay=0.2f;
         
        public void Init(Color color, Controller controller, ShapeManager _manager,ShapeType _shapetype)
        {
            GenerateStar();
            for (int i = 0; i < srs.Count; i++)
            {
                srs[i].color = color;
            }
            ctrl = controller;
            isDown = false;
            _color=color;
            shapeManager = _manager;
            UpdaownTimer = 1;
            shapeType=_shapetype;
        }

        public void CreateDownShape(Controller controller, ShapeManager _manager)
        {
            //生成下落位置
            DownShapeObj=Instantiate(gameObject,GetDownPosition(),transform.localRotation).GetComponent<Shape>();
            DownShapeObj.Init(new Color(_color.r,_color.g,_color.b,0.3f), controller, _manager,ShapeType.Down);
        }

        public void SetShapeState(ShapeType _shapetype)
        {
            shapeType=_shapetype;
        }

        private void SetDownShape()
        {
            if (DownShapeObj==null)
            {
                return;
            }
            DownShapeObj.transform.position = GetDownPosition();
            DownShapeObj.transform.rotation = transform.localRotation;
        }

        private void Update()
        {
            if (stars.Count<=0||Time.timeScale==0||shapeType==ShapeType.Next||isDown||shapeType==ShapeType.Down)
            {
                return;
            }
            timer += Time.deltaTime;
            Down();
            GetSpeedUpKeyInput();
            GetRotateKeyInput();
            GetMaxUpdwonInput();
            GetMoveKeyInput();
        }
        /// <summary>
        /// 获取加速键的输入
        /// </summary>
        private void GetSpeedUpKeyInput()
        {
            if (DealCommand.GetKey(1, AppKeyCode.ExtCh0))
            {
                UpdaownTimer =0.1f;
            }
            else if (DealCommand.GetKeyUp(1, AppKeyCode.ExtCh0))
            {
                UpdaownTimer =1f;
            }
        }

        public void DownShape(int dowValue)
        {
            transform.position += Vector3.down*(dowValue);
        }

        private void SpaceShape()
        {
                ctrl.model.PlaceShape(stars);
                ctrl.audioManager.PlayerPlaceShapeSound();
                shapeManager.FallDown();
                PoolManager.Instance.GetObj("DownMain",DownMain,RotateCenter.position , Quaternion.Euler(-90,0,0));
        }

        private void GetMaxUpdwonInput()
        {
            if (DealCommand.GetKeyDown(1,AppKeyCode.UpScore))
            {
                //直接下落
                transform.position=GetDownPosition();
                if (DownShapeObj!=null)
                {
                    DownShapeObj.DestroyMy();
                }
                isDown = true;
                Invoke("SpaceShape",DownDelay);
            }
        }
        public Vector3 GetDownPosition()
        {
            Vector3 Downpos =transform.position;
            List<Vector3> points = new List<Vector3>();
            int Downcount=0;
            for (int i = 0; i < stars.Count; i++)
            {
                points.Add(stars[i].transform.position);
            }
            while (ctrl.model.IsValidMapPosition(points))
            {
                for (int i = 0; i < points.Count; i++)
                {
                    points[i] += Vector3.down;
                }
                Downcount++;
            }
            Downpos.y-=Downcount-1;
            return Downpos;
        }

        /// <summary>
        /// 获取旋转键的输入
        /// </summary>
        private void GetRotateKeyInput()
        {
            if (DealCommand.GetKeyDown(1,AppKeyCode.Bet))
            {
                transform.RotateAround(RotateCenter.position, Vector3.forward, -90);
                if (ctrl.model.IsValidMapPosition(stars) == false)
                {
                    while (ctrl.model.IsValidMapPosition(stars) == false&&ctrl.model.IScrosstheborder(stars))
                    {
                        transform.position+=new Vector3(ctrl.model.GetMap(stars),0,0);
                    }
                }
                else
                {
                    ctrl.audioManager.PlayControl();
                }

                SetDownShape();
            }
        }
        /// <summary>
        /// 获取移动键的输入
        /// </summary>
        private void GetMoveKeyInput()
        {
            float h = 0;
            if (DealCommand.GetKey(1,AppKeyCode.TicketOut))
            {
                MoveLeftTimer+=Time.deltaTime;
                if (MoveLeftTimer>0.1f)
                {
                    h = -1;
                    MoveLeftTimer = 0;
                }
            }
            else if (DealCommand.GetKey(1,AppKeyCode.Flight))
            {
                MoveRightTimer += Time.deltaTime;
                if (MoveRightTimer>0.1f)
                {
                    MoveRightTimer = 0;
                    h = 1;
                }
              
            }
            if (h != 0)
            {
                Vector3 pos = transform.position;
                pos.x += h;
                transform.position = pos;
                if (ctrl.model.IsValidMapPosition(stars) == false)
                {
                    pos.x -= h;
                    transform.position = pos;
                }
                else
                {
                    ctrl.audioManager.PlayControl();
                    SetDownShape();
                }
            }
        }
        private void Down()
        {
            if (timer >= ctrl.model.ShapeDownSpeed*UpdaownTimer)
            {
                timer = 0;
                DownShape(1);
                if (DownShapeObj!=null)
                {
                    if (Vector3.Distance(transform.position,DownShapeObj.transform.position)<6)
                    {
                        DownShapeObj.DestroyMy();
                    } 
                }
                if (ctrl.model.IsValidMapPosition(stars)==false)
                {
                    DownShape(-1);
                    isDown = true;
                    SpaceShape();
                }
                ctrl.audioManager.PlayDrop();
            }
        }
        public void GetISStarClear(Star star)
        {
            int count = 0;
            for (int i = 0; i < stars.Count; i++)
            {
                if (stars[i] == star)
                {
                    stars.RemoveAt(i);
                }
            }
            if (stars.Count== 0)
            {
                Destroy(gameObject);
            }
        }
        //根据种类生成不同的形状
        private void GenerateStar()
        {
            for (int i = 0; i < shapeStarPos.Count; i++)
            {
                Star _star =PoolManager.Instance.GetObj("Star",star.gameObject,Vector3.zero,Quaternion.identity,transform).GetComponent<Star>();
                _star.transform.localPosition = shapeStarPos.pos[i];
                _star.Init(this);
                stars.Add(_star);
                srs.Add(_star.sprite) ;
            }
        }

        public void DestroyMy()
        {
            for (int i = 0; i < stars.Count; i++)
            {
                PoolManager.Instance.PushObj("Star",stars[i].gameObject);
            }
            Destroy(gameObject);
        }
    }
[Serializable]
    public class ShapeStarPos
    {
        public int Count;
        public List<Vector3> pos;
    }
}