using UnityEngine;
using System.Collections;

public class UICountdown : MonoBehaviour {
    public float time=10;
   public static float timer;
   float realTimer;
    protected UILabel label;
    UITweener[] tweeners;
    public EventDelegate onSecond, onOver;
    public AudioClip audioSecond;
    AudioSource audio;
	// Use this for initialization
	void OnEnable () {
        timer = time;
    
        if (label == null)
        {
            audio=GetComponent <AudioSource >();
            audio.clip = audioSecond;
            label = transform.Find("Second").GetComponent<UILabel>();
            tweeners = GetComponentsInChildren<UITweener>();
        }
        label.text = timer.ToString();
        if (tweeners!=null)
            for (int i = 0; i < tweeners.Length; i++)
            {
                tweeners[i].ResetToBeginning();
                tweeners[i].PlayForward();
            }
            realTimer = 0;
        StartCoroutine(Countdown());
	}
    public void Stop()
    {
        StopAllCoroutines();

    }
    WaitForSeconds waitTime = new WaitForSeconds(1);
	// Update is called once per frame
    IEnumerator Countdown()
    {
        while (timer > -1)
        {
            yield return null;
            realTimer += Time.unscaledDeltaTime ;
            if (realTimer > 1)
            {
                audio.Play();
                label.gameObject.SendMessage("ResetToBeginning");
                label.gameObject.SendMessage("PlayForward");
                realTimer -= 1;
                timer -= 1;
                label.text = timer.ToString();
            }
            if (timer < 0)
            {
                if (onOver != null)
                    onOver.Execute();
                gameObject.SetActive(false);
            }
            if (onSecond != null)
                onSecond.Execute();
          
            
           
        }
       
	}
}
