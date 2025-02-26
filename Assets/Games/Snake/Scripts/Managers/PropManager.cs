using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/****************************************************
    文件：PropManager.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：道具管理器(道具的产生和效果)
*****************************************************/
namespace SnakeGame
{
    public class PropManager : MonoBehaviour
    {
         public static PropManager Instance;
        [SerializeField] private PropPanel propPanel;
        [SerializeField] private Prop[] props;


        [SerializeField] private SnakePlayerController player;
        
        private List<Prop> propsList=new List<Prop>();
        private void Awake()
        {
            if (Instance==null)
            {
                Instance=this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Init()
        {
            CreatProp();
            InvokeRepeating("RandomCreateProp",10,10);
        }

        private void CreatProp()
        {
            for (int i = 0; i < props.Length; i++)
            {
                Prop  prop=PoolManager.Instance.GetObj("Prop_" + props[i].propType,props[i].gameObject,SnakeGameConstant.GetRandomPositionInMap(),Quaternion.identity).GetComponent<Prop>();
                propsList.Add(prop);
            }
        }

        private void RandomCreateProp()
        {
            if (propsList.Count>10)
            {
                return;
            }
            int propIndex = Random.Range(0, props.Length);
            Prop  prop= PoolManager.Instance.GetObj("Prop_" + props[propIndex].propType,props[propIndex].gameObject,SnakeGameConstant.GetRandomPositionInMap(),Quaternion.identity).GetComponent<Prop>();
            propsList.Add(prop);
        }

        public void AddCount(PropType propType)
        {
            //ui更新
            propPanel.AddProp(propType);
            //效果显示
            PropEffect(propType);
        }
        public void PropEffect(PropType propType)
        {
            switch (propType)
            {
                case PropType.UnZoom: 
                    CameraController.instance.UnZoom();
                    FBParticleManager.instance.CreateParticle(8,player.transform.position);
                    break;
                case PropType.Magnet: 
                    player.EnterMagnet();
                    FBParticleManager.instance.CreateParticle(9,player.transform.position);
                    break;
                case PropType.ScoreMultiplier:
                    player.EnterDobleSCore();
                    FBParticleManager.instance.CreateParticle(10,player.transform.position);
                    break;
                case PropType.ExtraSpeed: 
                    player.EnterExtraSpeed(true);
                    FBParticleManager.instance.CreateParticle(11,player.transform.position);
                    break;
            }
        }
        public void ExitPropEffect(PropType propType)
        {
            switch (propType)
            {
                case PropType.UnZoom: 
                    CameraController.instance.ExitUnZoom();
                    break;
                case PropType.Magnet: 
                    player.ExitMagnet();
                    break;
                case PropType.ScoreMultiplier: 
                    player.ExitDobleSCore();
                    break;
                case PropType.ExtraSpeed: 
                    player.ExitExtraSpeed();
                    break;
            }
            SnakeAudioManger.instance.PlayerPropTimerEnd();
        }
    }
}

