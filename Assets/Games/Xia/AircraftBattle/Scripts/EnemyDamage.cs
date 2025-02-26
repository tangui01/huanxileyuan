using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour {

//	public enum type { bossTurretLvl1, bossTurretLvl2, bossTurretLvl3, bossLaserLvl1, bossLaserLvl2 };
//	public type damageType;
	public int damage;

	void Start () 
	{
		if(name.Contains("Rocket"))
		{
			damage += 80 + LevelGenerator.currentStage*20;
		}
		else if(name.Contains("LongBulletJepPack"))
		{
			damage += LevelGenerator.currentStage*12;
		}
		else if(name.Contains("LongBullet"))
		{
			damage += LevelGenerator.currentStage*15;
		}
	}
	

}
