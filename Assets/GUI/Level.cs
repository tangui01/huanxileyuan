using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level : MonoBehaviour {
    public int number;
    public Text label;
    public GameObject lockimage;

	// Use this for initialization
	void Start () {
        lockimage.gameObject.SetActive( false );
        label.text = "" + number;

        int stars = 3;
        if( stars > 0 )
        {
            for( int i = 1; i <= stars; i++ )
            {
                transform.Find( "Star" + i ).gameObject.SetActive( true );
            }

        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartLevel()
    {
        InitScriptName.InitScript.Instance.OnLevelClicked( number );
    }
}
