using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Double_Duck_Booster : MonoBehaviour {

    bool alreadyEntered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!alreadyEntered){

            if(collision.transform.tag == "Ball"){
				alreadyEntered = true;

                //Instantiate another duck
                Instantiate(collision.transform.gameObject, collision.transform.position, Quaternion.identity, collision.transform.parent);

                //Enable the double again after some time
                Invoke("ReEnableDouble", 0.5f);
			}

        }
    }

    public void ReEnableDouble(){
        alreadyEntered = false;
    }
}
