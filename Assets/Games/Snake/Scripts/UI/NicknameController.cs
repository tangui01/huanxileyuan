using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeGame
{
    public class NicknameController : MonoBehaviour
    {
        public GameObject targetToFollow;
        internal string nameToDisplay = "";
        public Text nameUI;

        public void UpdateNicknameData(GameObject _actor, string _name)
        {
            targetToFollow = _actor;
            nameToDisplay = _name;

            if (targetToFollow)
            {
                nameUI.text = "" + nameToDisplay;
            }
        }

        void LateUpdate()
        {
            if (targetToFollow)
            {
                transform.position = targetToFollow.transform.position + new Vector3(0, 2, 0);
            }
            else
            {
                DestoryNickname();
            }
        }

        public void DestoryNickname()
        {
            targetToFollow = null;
            nameUI.text = "";
            PoolManager.Instance.PushObj(SnakeGameManager.Instance.Conf.NicknamePre.name, gameObject);
        }
    }
}