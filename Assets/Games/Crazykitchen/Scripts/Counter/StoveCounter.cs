using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****************************************************
    文件：StoveCounter.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
namespace Crzaykitchen
{
    public class StoveCounter : BaseCounter,IHasPrecess
    {
        public enum StoveCounterState
        {
            Idle,
            Frying,
            Fried,
            Burned,
        }
        [SerializeField] private FryingRecipSo[] fryingRecips;
        [SerializeField] private BurnedRecipSo[] burnedRecipSos;
        private StoveCounterState state = StoveCounterState.Idle;
        private float FryingTimer; 
        private float BurnedTimer;
        private float DestoryTimer;
        private FryingRecipSo currentFryingRecip;
        private BurnedRecipSo currentBurnedRecipSo;
        
        public Action<StoveCounterState> OnStateChanged;
        public event EventHandler<IHasPrecess.OnPrecessChangedEventArgs> OnPrecessChanged;
        public override void Interact(CrazyKitchenPlayer player)
        {
            if (!HasKitchenObject())
            {
                if (player.HasKitchenObject())
                {
                    if (HasOutKitchenObjectSo(player))
                    {
                        player.GetKitchenObject().SetKitChenObjectParent(this);
                        currentFryingRecip = GetFryingRecipSoByInputKitchenObjectSo(GetKitchenObject().GetKitchenObjectSO());
                        state = StoveCounterState.Frying;
                        FryingTimer = 0;
                        BurnedTimer = 0;
                        DestoryTimer = 0;
                    }  
                }
            }
            else if (HasKitchenObject())
            {
                if (!player.HasKitchenObject())
                {
                    GetKitchenObject().SetKitChenObjectParent(player);
                    state = StoveCounterState.Idle;
                    AudioManager.Instance.StopEffect4Player();
                    FryingTimer = 0;
                    BurnedTimer = 0;
                    DestoryTimer = 0;
                    OnStateChanged?.Invoke(state);
                    OnPrecessChanged?.Invoke(this, new IHasPrecess.OnPrecessChangedEventArgs
                        {
                            Precess = FryingTimer/currentFryingRecip.FryingTimeMax,
                        }
                    );
                }
                else
                {
                    if (player.GetKitchenObject().TryPlate(out PlateKitchenObject plateObject))
                    {
                        if (plateObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                        {
                            GetKitchenObject().DestroySelf();
                            state = StoveCounterState.Idle;
                            OnStateChanged?.Invoke(state);
                            OnPrecessChanged?.Invoke(this, new IHasPrecess.OnPrecessChangedEventArgs
                                {
                                    Precess = FryingTimer/currentFryingRecip.FryingTimeMax,
                                }
                            );
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if (CrzayKitchenGameManager.instance.GameOver)
            {
                return;
            }
            if (HasKitchenObject())
            {
                switch (state)
                {
                    case StoveCounterState.Idle:
                        AudioManager.Instance.StopEffect4Player();
                        break;
                    case StoveCounterState.Frying:
                        FryingTimer += Time.deltaTime;
                        OnStateChanged?.Invoke(state);
                        OnPrecessChanged?.Invoke(this, new IHasPrecess.OnPrecessChangedEventArgs
                            {
                                Precess = FryingTimer/currentFryingRecip.FryingTimeMax,
                            }
                        );
                        if (!AudioManager.Instance.Effect4ISPalyer())
                        {
                            SoundManager.Instance.PlayerStoveSizzleSound();
                        }
                        if (FryingTimer>currentFryingRecip.FryingTimeMax)
                        {
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpawnKitchenObject(currentFryingRecip.OutputKitchenObject,this);
                            currentBurnedRecipSo = GetBurnedRecipSoByInputKitchenObjectSo(GetKitchenObject().GetKitchenObjectSO());
                            state = StoveCounterState.Fried;
                        }
                        break;
                    case StoveCounterState.Fried:
                        BurnedTimer += Time.deltaTime;
                        if (BurnedTimer>currentBurnedRecipSo.BurnedRecipSoTimeMax)
                        {
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpawnKitchenObject(currentBurnedRecipSo.OutputKitchenObject,this);
                            state = StoveCounterState.Burned;
                        }
                        OnStateChanged?.Invoke(state);
                        OnPrecessChanged?.Invoke(this, new IHasPrecess.OnPrecessChangedEventArgs
                            {
                                Precess = BurnedTimer/(float)currentBurnedRecipSo.BurnedRecipSoTimeMax,
                            }
                        );
                        break;
                    case StoveCounterState.Burned:
                        BurnedTimer = 0;
                        DestoryTimer+=Time.deltaTime;
                        OnPrecessChanged?.Invoke(this, new IHasPrecess.OnPrecessChangedEventArgs
                            {
                                Precess = DestoryTimer/5,
                            }
                        );
                        if (DestoryTimer>5)
                        {
                            DestoryTimer = 0;
                            AudioManager.Instance.StopEffect4Player();
                            GetKitchenObject().DestroySelf(); 
                        }
                        break;
                }
            }
        }

        private bool HasOutKitchenObjectSo(CrazyKitchenPlayer player)
        {
            if (GetFryingRecipSoByInputKitchenObjectSo(player.GetKitchenObject().GetKitchenObjectSO()) != null)
            {
                return true;
            }

            return false;
        }

        public FryingRecipSo GetFryingRecipSoByInputKitchenObjectSo(KitchenObjectSO InputKitchenObjectSo)
        {
            FryingRecipSo outKitchenObjectSo = null;
            for (int i = 0; i < fryingRecips.Length; i++)
            {
                if (fryingRecips[i].InputKitchenObject == InputKitchenObjectSo)
                {
                    outKitchenObjectSo = fryingRecips[i];
                }
            }

            return outKitchenObjectSo;
        }
        public BurnedRecipSo GetBurnedRecipSoByInputKitchenObjectSo(KitchenObjectSO InputKitchenObjectSo)
        {
            BurnedRecipSo outKitchenObjectSo = null;
            for (int i = 0; i <burnedRecipSos.Length; i++)
            {
                if (burnedRecipSos[i].InputKitchenObject == InputKitchenObjectSo)
                {
                    outKitchenObjectSo = burnedRecipSos[i];
                }
            }
            return outKitchenObjectSo;
        }
        public bool IsFried()
        {
            return state == StoveCounterState.Fried;
        }
    }  
}