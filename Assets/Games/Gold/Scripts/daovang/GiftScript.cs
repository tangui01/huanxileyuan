using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftScript : MonoBehaviour {

    int randomGift;

    public bool isMoveFollow = false;
    public float maxY;
    public float speed;
    // Use this for initialization
    void Start () {
        isMoveFollow = false;
    }
    private void FixedUpdate()
    {
        moveFllowTarget(LuoiCauScript.instance.transform);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "luoiCau"
            && DayCauScript.instance.typeAction != TypeAction.KeoCau)
        {
            LuoiCauScript.instance.cameraOut = false;
            isMoveFollow = true;
            DayCauScript.instance.typeAction = TypeAction.KeoCau;
            LuoiCauScript.instance.velocity = -LuoiCauScript.instance.velocity;
            LuoiCauScript.instance.SetSpeed(4);
        }
    }

    void moveFllowTarget(Transform target)
    {
        if (isMoveFollow)
        {
            Quaternion tg = Quaternion.Euler(target.parent.transform.rotation.x,
                                             target.parent.transform.rotation.y,
                                             90 + target.parent.transform.rotation.z);
            transform.position = new Vector3(target.position.x,
                                             target.position.y - gameObject.GetComponent<Collider2D>().bounds.size.y / 2,
                                             transform.position.z);
            if (DayCauScript.instance.typeAction == TypeAction.Nghi)
            {
                if (GoldMinerGameManager.instance.clover)
                {
                    randomGift = Random.RandomRange(1, 5);
                    switch (randomGift)
                    {
                        case 1:
                            GamePlayScript.instance.Power();
                            break;
                        case 2:
                            int score = Random.RandomRange(300, 400);
                            GoldMinerGameManager.instance.AddScore(score);
                            GamePlayScript.instance.CreateScoreFly(score);
                            break;
                        case 3:
                            OngGiaScript.instance.Angry();
                            break;
                    }
                }
                else
                {
                    randomGift = Random.RandomRange(1, 8);
                    //randomGift = 2;
                    switch (randomGift)
                    {
                        case 1:
                            int score = Random.RandomRange(50, 150);
                            GamePlayScript.instance.CreateScoreFly(score);
                            break;
                        case 2:
                           // GamePlayScript.instance.CreateBoomFly();
                           OngGiaScript.instance.Angry();
                            break;
                        case 3:
                            GamePlayScript.instance.Power();
                            break;
                        case 4:
                            GamePlayScript.instance.Power();
                            break;
                        case 5:
                            //GamePlayScript.instance.CreateBoomFly();
                            OngGiaScript.instance.Angry();
                            break;
                        case 6:
                            int score6 = Random.RandomRange(50, 150);
                            GamePlayScript.instance.CreateScoreFly(score6);
                            break;
                        case 7:
                            int score7 = Random.RandomRange(50, 150);
                            GamePlayScript.instance.CreateScoreFly(score7);
                            break;

                    }
                }
                Destroy(gameObject);
            }
        }
    }


}
