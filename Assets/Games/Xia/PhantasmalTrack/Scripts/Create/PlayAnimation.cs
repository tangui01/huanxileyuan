using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhantasmalTrack
{
public class PlayAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ImageCFXTriAddGameScore()
    {
        GameManager.Instance.CFXTriAddGameScore();
    }
}
}