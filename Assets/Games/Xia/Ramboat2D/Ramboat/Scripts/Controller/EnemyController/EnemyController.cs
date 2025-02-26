using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	GameObject player;
	public Sprite[] sprites;
	SpriteRenderer localSprite;
	Vector3 endPoint;
	public float speed = 1.0F;
	private float healthEnemy;
	private bool addHealth = false;
	public float timeShoot=0;
	private float timeShootDelay;
	public bool moveSpecial;

	private Ramboat2DBezier myBezier;
	private float t = 0f;
	Vector3[] path;
	public float minDistance;
	private int lastPoint;
	// Use this for initialization
	void OnEnable() {
		Start ();
		if(!addHealth){
			healthEnemy = 30f;
			addHealth = true;
		}
		else
			healthEnemy = 30*((Ramboat2DLevelManager.THIS.level+1) / 3);

	}
	void Start () {
		localSprite = GetComponent<SpriteRenderer> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		if (!moveSpecial) {
			timeShootDelay = Ramboat2DLevelManager.THIS.timeEnemyToShoot+ UnityEngine.Random.Range(-1,4);
			Vector3 end = Ramboat2DLevelManager.THIS.pointEnemyDestroyed.transform.position;
			endPoint = new Vector3 (end.x, end.y + UnityEngine.Random.Range (-0.5f, 1f), 0);
		}
		else {
			timeShootDelay = Ramboat2DLevelManager.THIS.timeEnenmyFlyShoot +UnityEngine.Random.Range(-1,4);
			path = new Vector3[4];
			lastPoint = 0;
			t = 0;
			if(player!=null)
			path [0] = player.transform.position + new Vector3 (UnityEngine.Random.Range (-2f, 2f),UnityEngine.Random.Range (1f, 2.5f), 0);
			for (int i = 1; i < 4; i++) {
				Vector3 point = new Vector3 (UnityEngine.Random.Range (-4.5f, 4.5f), UnityEngine.Random.Range (1f,2.5f), 0);
				path [i] = point;

			}
			myBezier = new Ramboat2DBezier (transform.parent.position, UnityEngine.Random.insideUnitSphere * 1.5f, UnityEngine.Random.insideUnitSphere * 1.5f, path [0]);
		}
	}

	// Update is called once per frame
	void Update () {
		
			CheckAngleToFire ();
			if (!moveSpecial) {
				transform.parent.position = Vector3.MoveTowards (transform.parent.position, endPoint, speed * Time.deltaTime);
				if (transform.position.x <= -5.7f) {
					gameObject.transform.parent.gameObject.SetActive (false);

				}
			} else {
				MoveSpecial ();
			}
	
			timeShoot += Time.deltaTime;
			if (timeShoot > timeShootDelay &&!Ramboat2DPlayerController.Intance.playerDead) {
				timeShoot = 0;
				Shot ();
			}

		}

	void CheckAngleToFire ()
	{
		float angle;
		bool isRight = true;
		if (player != null) {
			angle = Mathf.Abs (Vector3.Angle (Vector3.down, player.transform.position - transform.position));
			isRight = (transform.position.x < player.transform.position.x) ? true : false;

			if (isRight ) {
				Vector3 eulerAngle = transform.localEulerAngles;
				eulerAngle.y = 180;
				transform.localEulerAngles = eulerAngle;
			} else {
				Vector3 eulerAngle = transform.localEulerAngles;
				eulerAngle.y = 0;
				transform.localEulerAngles = eulerAngle;
			}
			if (angle >= 0 && angle < 15)
				localSprite.sprite = sprites [0];
			if (angle >= 15 && angle < 45)
				localSprite.sprite = sprites [1];
			if (angle >= 45 && angle < 75)
				localSprite.sprite = sprites [2];
			if (angle >= 75 && angle <180)
				localSprite.sprite = sprites [3];

		}
	}

	// void OnParticleCollision(GameObject other) {          
	// 	TakeDame (PlayerController.Intance.gunPower);
	// }
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Bullet") {
			if (Ramboat2DPlayerController.Intance.gunType == GunType.NormalGun) {
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.pistolHitFlesh [Random.Range (0, 3)]);
			}else if(Ramboat2DPlayerController.Intance.gunType == GunType.SixBarreled){
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.shotHit);
			}else if(Ramboat2DPlayerController.Intance.gunType == GunType.Rocket){
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.rocketLauchHit [Random.Range (0, 3)]);
			}else if(Ramboat2DPlayerController.Intance.gunType == GunType.ThreeLineGun){
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.shotHit);
			}else if(Ramboat2DPlayerController.Intance.gunType == GunType.FireGun){
				Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.flameShotHit[Random.Range(0,3)]);
			}
			other.gameObject.SetActive (false);
			GameObject effectShootPlayer = Ramboat2DLevelManager.THIS.GetPooledObject (10);
			if (effectShootPlayer != null) {
				effectShootPlayer.transform.position = transform.position;
				effectShootPlayer.transform.eulerAngles = new Vector3 (0, 0, UnityEngine.Random.Range (0, 360));
				effectShootPlayer.SetActive (true);
			}
			TakeDame (Ramboat2DPlayerController.Intance.gunPower);
		}
	} 
	void TakeDame(float dame){
		if (healthEnemy > 0) {
			healthEnemy -= dame;
		} else
			Death ();
	}
	public void FireGunOnTriggerEnter2D()
	{
		Ramboat2DFXSound.THIS.fxSound.PlayOneShot (Ramboat2DFXSound.THIS.flameShotHit[Random.Range(0,3)]);
		GameObject effectShootPlayer = Ramboat2DLevelManager.THIS.GetPooledObject (10);
		if (effectShootPlayer != null) {
			effectShootPlayer.transform.position = transform.position;
			effectShootPlayer.transform.eulerAngles = new Vector3 (0, 0, UnityEngine.Random.Range (0, 360));
			effectShootPlayer.SetActive (true);
		}
		TakeDame (Ramboat2DPlayerController.Intance.gunPower);
	}
	public void Death(){
		ReadWriteTextMission.THIS.CheckMission (2);
		ReadWriteTextMission.THIS.CheckMission (34);
		GameObject blood = Ramboat2DLevelManager.THIS.GetPooledObject (16);
		if (blood != null) {
			blood.transform.position = transform.position;
			blood.SetActive (true);
		}
		int appearDollar = UnityEngine.Random.Range (0,5);
		if (appearDollar % 3 == 0) {
			GameObject dollar = Ramboat2DLevelManager.THIS.GetPooledObject (12);
			if (dollar != null) {
				dollar.transform.position = transform.position;
				dollar.SetActive (true);
			}
			int numberCoinAppear = UnityEngine.Random.Range (1, 4);
			for (int i = 0; i < numberCoinAppear; i++) {
				GameObject coin = Ramboat2DLevelManager.THIS.GetPooledObject (11);
				if (coin != null) {
					coin.transform.position = transform.position;
					coin.SetActive (true);
					coin.GetComponent<Rigidbody2D> ().velocity = new Vector3 (i * 5 - 5, Random.Range (6, 12), 0);
				}
			}
		} else {
			int numberCoinAppear = UnityEngine.Random.Range (1, 4);
			for (int i = 0; i < numberCoinAppear; i++) {
				GameObject coin = Ramboat2DLevelManager.THIS.GetPooledObject (11);
				if (coin != null) {
					coin.transform.position = transform.position;
					coin.SetActive (true);
					coin.GetComponent<Rigidbody2D> ().velocity = new Vector3 (i * 5 - 5, Random.Range (6, 12), 0);
				}
			}
		}


		transform.parent.gameObject.SetActive (false);

	}
	public void Shot(){
		if (transform.position.y >= 1) {
			GameObject bullet = Ramboat2DLevelManager.THIS.GetPooledObject (9);
			if (bullet != null) {
				bullet.transform.position = transform.position;
				bullet.gameObject.SetActive (true);
				Vector3 pointShot = (player.transform.position - transform.position) / (Vector3.Distance (player.transform.position, transform.position));
				bullet.GetComponent<Rigidbody2D> ().velocity = pointShot * 1.5f;
			}
		}
	}
	public void MoveSpecial(){
		if (Time.timeScale > 0.1f) {
			float distance = Vector3.Distance (path [lastPoint], transform.position);
			Vector3 vec = myBezier.GetPointAtTime (t);
			transform.parent.position = vec;
			t += Time.fixedDeltaTime / 10f;
			if (distance < minDistance) {
				lastPoint += 1;
				if (lastPoint == path.Length) {
					lastPoint = 1;
				}
				myBezier = new Ramboat2DBezier (transform.position, UnityEngine.Random.insideUnitSphere * 3f, UnityEngine.Random.insideUnitSphere * -2f, path [lastPoint]);
				t = 0;
			}

		}
	}
}
