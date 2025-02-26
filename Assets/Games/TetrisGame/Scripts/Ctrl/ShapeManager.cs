using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/****************************************************
    文件：ShapeManager.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：方块生成器
*****************************************************/
namespace TetrisGame
{
    public class ShapeManager : MonoBehaviour
    {
          public Shape[] shapes;
          public Color[] colors;
          private Controller ctrl;
          
          private Shape currentShape;
          private Shape nextShape;
          private Transform blockHolder;
          private Vector3 NextPoint=new Vector2(18f,5f);
          private Vector3 BornPoint=new Vector2(6,19);
          private void Awake()
          {
              ctrl = GetComponent<Controller>();
              blockHolder=transform.Find("BlockHolder");
          }

          private void Update()
          {
              if (currentShape==null&&ctrl.model.CanGenerateStar())
              {
                  SpawnShape();
              }
          }

          private void SpawnShape()
          {
              if (nextShape==null)
              {
                  //如果是开始时需要实例化一个开始的图形和下一个图形
                  currentShape = Instantiate(shapes[RandomShapeIndex()],BornPoint,Quaternion.identity,blockHolder);
                  currentShape.transform.SetParent(blockHolder) ;
                  currentShape.Init(colors[RandomShapeIndexColor()],ctrl,this,ShapeType.current);
                  currentShape.CreateDownShape(ctrl,this);
                  SpawnNextShape();
              }
              else
              {
                  currentShape = nextShape;
                  currentShape.transform.SetParent(blockHolder);
                  currentShape.transform.position = BornPoint;
                  currentShape.SetShapeState(ShapeType.current);
                  currentShape.CreateDownShape(ctrl,this);
                  SpawnNextShape();
              }
          }
          private void SpawnNextShape()
          {
              nextShape = Instantiate(shapes[RandomShapeIndex()],NextPoint,Quaternion.identity);
              nextShape.Init(colors[RandomShapeIndexColor()],ctrl,this,ShapeType.Next);
          }
          private int RandomShapeIndex()
          {
             return UnityEngine.Random.Range(0, shapes.Length);
          }
          private int RandomShapeIndexColor()
          {
              return UnityEngine.Random.Range(0, colors.Length);
          }
          public void FallDown()
          {
              currentShape = null;
              foreach(Transform t in blockHolder)
              {
                  if (t.childCount <= 1)
                  {
                      Destroy(t.gameObject);
                  }
              }
              if (ctrl.model.IsGameOver())
              {
                 ctrl.GameOver();
              }
          }
    }
}

