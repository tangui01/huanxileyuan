using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****************************************************
    文件：CuttingCounter.cs
    作者：tg
    邮箱: 18178367954@139.com
    日期：#CreateTime#
    功能：切割计数器
*****************************************************/
namespace Crzaykitchen
{
    public class CuttingCounter : BaseCounter, IHasPrecess
    {
        [SerializeField] private KitchenObjectSO cuttingObject;
        [SerializeField] private CuttingRecipeSo[] cuttingRecipes;

        public event EventHandler<IHasPrecess.OnPrecessChangedEventArgs> OnPrecessChanged;
        public Action cuttingProcessAni;
        public static Action OnCut;
        
        private int CuttingProcess = 0;
        private int CuttingMax = 0;
        CuttingRecipeSo cuttingRecipeSo = null;
        KitchenObjectSO outPutKitchenObjectSo = null;

        public override void Interact(CrazyKitchenPlayer player)
        {
            if (!HasKitchenObject())
            {
                if (player.HasKitchenObject())
                {
                    if (HasOutKitchenObjectSo(player))
                    {
                        player.GetKitchenObject().SetKitChenObjectParent(this);
                        cuttingRecipeSo = GetCuttingRecipSoByInputKitchenObjectSo(GetKitchenObject().GetKitchenObjectSO());
                        CuttingMax = GetCuttingRecipSoByInputKitchenObjectSo(GetKitchenObject().GetKitchenObjectSO())
                            .CuttingProcessMax;
                        CuttingProcess = 0;
                    }
                }
                else
                {
                    
                }
            }
            else if (HasKitchenObject())
            {
                if (!player.HasKitchenObject())
                {
                    PlayerCutting(player);
                }
                else
                {
                    if (player.GetKitchenObject().TryPlate(out PlateKitchenObject plateObject))
                    {
                        if (plateObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                        {
                            GetKitchenObject().DestroySelf();
                        }
                    }
                }
               
            }
        }

        private void PlayerCutting(CrazyKitchenPlayer player)
        {
            CuttingProcess++;
            OnPrecessChanged?.Invoke(this, new IHasPrecess.OnPrecessChangedEventArgs
                {
                    Precess = CuttingProcess/(float)CuttingMax,
                }
            );
            OnCut?.Invoke();
            if (CuttingProcess == CuttingMax)
            {
                outPutKitchenObjectSo = cuttingRecipeSo.OutputKitchenObject;
                GetKitchenObject().DestroySelf();
                cuttingProcessAni?.Invoke();
                KitchenObject kitchenObject = KitchenObject.SpawnKitchenObject(outPutKitchenObjectSo, this);
            }
            else if (CuttingProcess == CuttingMax + 1)
            {
                GetKitchenObject().SetKitChenObjectParent(player);
            }
            else
            {
                cuttingProcessAni?.Invoke();
            }
        }

        private bool HasOutKitchenObjectSo(CrazyKitchenPlayer player)
        {
            if (GetCuttingRecipSoByInputKitchenObjectSo(player.GetKitchenObject().GetKitchenObjectSO()) != null)
            {
                return true;
            }

            return false;
        }

        public CuttingRecipeSo GetCuttingRecipSoByInputKitchenObjectSo(KitchenObjectSO InputKitchenObjectSo)
        {
            CuttingRecipeSo outKitchenObjectSo = null;
            for (int i = 0; i < cuttingRecipes.Length; i++)
            {
                if (cuttingRecipes[i].InputKitchenObject == InputKitchenObjectSo)
                {
                    outKitchenObjectSo = cuttingRecipes[i];
                }
            }

            return outKitchenObjectSo;
        }
    }
}