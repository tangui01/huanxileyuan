

using UnityEngine;

namespace Crzaykitchen
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        private const string SOUNDMANAGER_VOLUME = "SoundManagerVolume";

        [SerializeField] private AudioClipRefsSO audioClipRefsSO;
        public AudioClip bgm;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            CrzayKitchenGameManager.instance.OnRecipeSuccessed += OrderManager_OnRecipeSuccessed;
            CrzayKitchenGameManager.instance.OnRecipeFailed += OrderManager_OnRecipeFailed;
            CuttingCounter.OnCut = CuttingCounter_OnCut; 
            KitchenObject.OnDrop += KitchenObjectHolder_OnDrop;
            KitchenObject.OnPickup += KitchenObjectHolder_OnPickup;
            TrashCounter.OnObjectTrashed = TrashCounter_OnObjectTrashed;
            AudioManager.Instance.playerBGm(bgm);
        }

        private void TrashCounter_OnObjectTrashed()
        {
           AudioManager.Instance.playerEffect3(RandomSound(audioClipRefsSO.trash));
        }

        private void KitchenObjectHolder_OnPickup()
        {
           AudioManager.Instance.playerEffect3(RandomSound(audioClipRefsSO.objectPickup));
        }

        private void KitchenObjectHolder_OnDrop()
        { 
            AudioManager.Instance.playerEffect3(RandomSound(audioClipRefsSO.objectDrop));
        }

        private void CuttingCounter_OnCut()
        {
           AudioManager.Instance.playerEffect3(RandomSound(audioClipRefsSO.chop));
        }

        private void OrderManager_OnRecipeFailed()
        {
           AudioManager.Instance.playerEffect3(RandomSound(audioClipRefsSO.deliveryFail));
        }

        private void OrderManager_OnRecipeSuccessed()
        {
           AudioManager.Instance.playerEffect3(RandomSound(audioClipRefsSO.deliverySuccess));
        }

        public void PlayWarningSound()
        {
            AudioManager.Instance.playerEffect2(RandomSound(audioClipRefsSO.warning));
        }

        public void PlayCountDownSound()
        {
            AudioManager.Instance.playerEffect2(RandomSound(audioClipRefsSO.warning));
        }

        public void PlayerGenerateSound()
        {
            AudioManager.Instance.playerEffect2(RandomSound(audioClipRefsSO.generateSound));
        }

        public void PlayStepSound()
        {
             AudioManager.Instance.playerEffect1(RandomSound(audioClipRefsSO.footstep));
        }

        public void PlayerStoveSizzleSound()
        {
            AudioManager.Instance.playerEffect4(audioClipRefsSO.stoveSizzle[0]);
        }

        private AudioClip RandomSound(AudioClip[] clips)
        {
           return clips[Random.Range(0, clips.Length)];
        }
    }
}