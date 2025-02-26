using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

public class StopRotation : MonoBehaviour
{
    public GameObject Explosion;
    private float Volume;
    void Start()
    {
        GetComponent<AudioSource>().volume = LibWGM.machine.BgmVolume/100;
        Volume = LibWGM.machine.BgmVolume/100;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.transform.parent.rotation.z * -1.0f);
        if (Volume != LibWGM.machine.BgmVolume/100)
        {
            Volume  = LibWGM.machine.BgmVolume/100;
            GetComponent<AudioSource>().volume = Volume;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Blocker"))
        {
           GameObject a = Instantiate(Explosion, transform)as GameObject;
            a.transform.SetParent(null);
            a.transform.localScale = new Vector3(1, 1, 1);
          
            DualControlGameManager.instance.GameEnd();
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Finish")) {

            DualControlGameManager.instance.Cars +=1;
            DualControlGameManager.instance.AddScore(100);

        }
    }
    
    public void DestroyCar()
    {
        GameObject a = Instantiate(Explosion, transform)as GameObject;
        a.transform.SetParent(null);
        a.transform.localScale = new Vector3(1, 1, 1);
          
        DualControlGameManager.instance.GameEnd();
        Destroy(gameObject);
    }
    
}
