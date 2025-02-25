﻿using UnityEngine;
using System.Collections;

public class FireEvil : MonoBehaviour, ICanTakeDamage {
	public float speed = 5;
	Vector3 old_position;
    public AudioClip showUpSound, deadSound;

    public float timeLive = 5;

	// Use this for initialization
	void Start () {
		old_position = transform.position;

        Destroy(gameObject, timeLive);
	}

    private void OnEnable()
    {
        if (SuperCommandoGameManager.Instance && SuperCommandoGameManager.Instance.State == SuperCommandoGameManager.GameState.Playing)
        {
            if (SuperCommandoSoundManager.Instance)
                SuperCommandoSoundManager.Instance.PlaySfx(showUpSound);
        }
    }

    void Update () {
		transform.Translate (speed * Time.deltaTime * transform.right.x, 0, 0, Space.World);
	}

    public void TakeDamage(int damage, Vector2 force, GameObject instigator, Vector3 hitPoint)
    {
        //Debug.LogError("TakeDamage");
        transform.position = old_position;
        SuperCommandoSoundManager.Instance.PlaySfx(deadSound);

        //try spawn random item
        var spawnItem = GetComponent<EnemySpawnItem>();
        if (spawnItem != null)
        {
            spawnItem.SpawnItem();
        }

        gameObject.SetActive(false);
    }
}
