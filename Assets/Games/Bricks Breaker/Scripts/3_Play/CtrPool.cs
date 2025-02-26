using UnityEngine;

public class CtrPool : MonoBehaviour
{
    static CtrPool _instance;

    public static CtrPool instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CtrPool>();
            }

            return _instance;
        }
    }

    public GameObject pBlockGroups;
    public GameObject pBlockDefault;
    public GameObject pBlockAddBall;
    public GameObject pFxBlockHit;
    public GameObject pFxBlockBoom;
    public GameObject pFxBallGet;
    public GameObject pComboEffect;
    public GameObject pRoket;



    public void Awake()
    {
        BricksBreakerPoolManager.CreatePool(pBlockGroups, 10, false, 0);
        BricksBreakerPoolManager.CreatePool(pComboEffect, 10, false, 0);
        BricksBreakerPoolManager.CreatePool(pBlockDefault, 20, false, 0);
        BricksBreakerPoolManager.CreatePool(pBlockAddBall, 8, false, 0);
        BricksBreakerPoolManager.CreatePool(pFxBlockHit, 10, false, 0);
        BricksBreakerPoolManager.CreatePool(pFxBlockBoom, 5, false, 0);
        BricksBreakerPoolManager.CreatePool(pFxBallGet, 3, false, 0);
        BricksBreakerPoolManager.CreatePool(pRoket, 1, true, 1);

    }
}
