using UnityEngine;
using System.Collections;
using WGM;

namespace ninjaGame
{
	public class InputController : MonoBehaviour
	{



		public static InputController Static;
		PlayerController playerScript;
		public bool isJump = false, isdoubleJump = false, isAttack = false;

		void Start()
		{

			Static = this;
			playerScript =
				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); //for getting player scipt

		}

		float lastJumpTime;
		float lastAttackTime;

		void Update()
		{
			if (DealCommand.GetKeyDown(1,AppKeyCode.UpScore))
			{
				if (Time.timeSinceLevelLoad - lastJumpTime > 0.2f)
				{
					 if (Time.timeSinceLevelLoad - lastAttackTime > 0.3f)
					{
						isAttack = true;
						lastAttackTime = Time.timeSinceLevelLoad;
					}
				}
			}
			else if (DealCommand.GetKeyDown(1,AppKeyCode.Bet))
			{
				//to avoid repeated tapping
				isJump = true;
				lastJumpTime = Time.timeSinceLevelLoad;
			}
		}
	}
}
