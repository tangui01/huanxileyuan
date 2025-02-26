using UnityEngine;
using System.Collections;

public class GameFinishFlag : MonoBehaviour
{
    public AudioClip sound;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<SuperCommandoPlayer>() == null)
            return;

        if (SuperCommandoGameManager.Instance.State == SuperCommandoGameManager.GameState.Finish)
            return;

        SuperCommandoGameManager.Instance.GameFinish();
        if (GetComponent<Animator>() != null)
            GetComponent<Animator>().SetBool("finish", true);
        SuperCommandoSoundManager.Instance.PlaySfx(sound, 0.5f);
        Destroy(this);
    }
}
