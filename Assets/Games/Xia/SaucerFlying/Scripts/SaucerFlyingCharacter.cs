using UnityEngine;

namespace SaucerFlying
{
    public class SaucerFlyingCharacter : MonoBehaviour
    {
        public int characterSequenceNumber;
        public string characterName;
        public int price;
        public bool isFree = false;
        public Color charColor;

        public bool IsUnlocked
        {
            get
            {
                return (isFree);
            }
        }

        void Awake()
        {
            characterName = characterName.ToUpper();
        }

        public bool Unlock()
        {
            if (IsUnlocked)
                return true;

            if (SaucerFlyingCoinManager.Instance.Coins >= price)
            {
                SaucerFlyingCoinManager.Instance.RemoveCoins(price);
                return true;
            }

            return false;
        }
    }
}