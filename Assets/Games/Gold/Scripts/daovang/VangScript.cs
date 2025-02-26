using UnityEngine;
using System.Collections;

public class VangScript : MonoBehaviour {
    
	public bool isMoveFollow = false;
	public float maxY;
	public int score;
	public int speed;
    Vector3 position;
    public Animator anim;
    
	// Use this for initialization
	void Start () {
		isMoveFollow = false;
        //Debug.Log("tao vang");
        
        StartCoroutine(RunEffect());

    }
    IEnumerator RunEffect()
    {
        int ran = Random.RandomRange(2, 5);
        yield return new WaitForSeconds(ran);
        anim.SetBool("Run", true);
        StartCoroutine(StopEffect());
    }
    IEnumerator StopEffect()
    {
        int ran = Random.RandomRange(1, 3);
        yield return new WaitForSeconds(ran);
        anim.SetBool("Run", false);
        StartCoroutine(RunEffect());
    }

	void FixedUpdate() 
    {
		moveFllowTarget(LuoiCauScript.instance.transform);
    }

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.name == "luoiCau"
            && GameObject.Find("dayCau").GetComponent<DayCauScript>().typeAction != TypeAction.KeoCau)
        {
			isMoveFollow = true;
            LuoiCauScript.instance.cameraOut = false;
			DayCauScript.instance.typeAction = TypeAction.KeoCau;
			LuoiCauScript.instance.velocity = -LuoiCauScript.instance.velocity;
            LuoiCauScript.instance.SetSpeed(speed);
            GamePlayScript.instance.itemSeclected = gameObject.name;
        }
        else if (other.tag == "fire")
        {
            position = gameObject.transform.position;
            Instantiate(Resources.Load("GoldFire"), position, Quaternion.identity);
            Destroy(gameObject);
        }
	}

	void moveFllowTarget(Transform target) {
		if(isMoveFollow) 
		{
            Quaternion tg = Quaternion.Euler(target.parent.transform.rotation.x,
                                             target.parent.transform.rotation.y,
                                              target.parent.transform.rotation.z * 40);
            transform.rotation = Quaternion.Slerp(transform.rotation, tg, 0.5f);
            transform.position = new Vector3(target.position.x,
                                                target.position.y - gameObject.GetComponent<Collider2D>().bounds.size.y / 3,
                                                transform.position.z);
            if (DayCauScript.instance.typeAction == TypeAction.Nghi) {
                //play sound
                if(score > 0 && score <= 150)
                {
                    GamePlayScript.instance.PlaySound(1);
                }
                else if( score >150 && score <= 400)
                {
                    GamePlayScript.instance.PlaySound(2);
                }
                else
                {
                    OngGiaScript.instance.Happy();
                    GamePlayScript.instance.PlaySound(3);
                }
                //increase score
                if (gameObject.tag == "Stone")
                {
                    if (GoldMinerGameManager.instance.bookStone)
                    {
                        //GamePlayScript.instance.score += this.score * 3;
                        GamePlayScript.instance.CreateScoreFly(this.score * 3);
                    }
                    else
                    {
                       // GamePlayScript.instance.score += this.score;
                        GamePlayScript.instance.CreateScoreFly(this.score);
                        OngGiaScript.instance.Angry();
                    }
                       
                }
                else if (gameObject.tag == "Diamond" && GoldMinerGameManager.instance.diamond)
                {
                    //GamePlayScript.instance.score += this.score  +100;
                    GamePlayScript.instance.CreateScoreFly(this.score + 100);
                }
                else
                {
                    GamePlayScript.instance.CreateScoreFly(this.score);
                }
				
				Destroy(gameObject);
               
			}
		}
	}
}
