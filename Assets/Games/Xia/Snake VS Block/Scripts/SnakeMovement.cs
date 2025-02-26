using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGM;

namespace SnakeVSBlock
{
    public class SnakeMovement : MonoBehaviour
    {
        [Header("Managers")] public SnakeVSBlockGameController GC;

        [Header("Some Snake Variables & Storing")]
        public List<Transform> BodyParts = new List<Transform>();

        public float minDistance = 0.25f;
        public int initialAmount;
        public float speed = 1;
        public float rotationSpeed = 50;
        public float LerpTimeX;
        public float LerpTimeY;

        [Header("Snake Head Prefab")] public GameObject BodyPrefab;

        [Header("Parts Text Amount Management")]
        public TextMesh PartsAmountTextMesh;

        [Header("Private Fields")] private float distance;
        private Vector3 refVelocity;

        private Transform curBodyPart;
        private Transform prevBodyPart;

        private bool firstPart;

        [Header("Particle System Management")] public ParticleSystem SnakeParticle;

        private bool isAddSpeed = false;
        private Transform addSpeedVFX;
        void Start()
        {
            addSpeedVFX = GameObject.Find("VFXPool").transform.GetChild(3);
            firstPart = true;

            //Add the initial BodyParts
            for (int i = 0; i < initialAmount; i++)
            {
                //Use invoke to avoid a weird bug where the snake goes down at the beginning.
                Invoke("AddBodyPart", 0.1f);
            }
        }

        public void SpawnBodyParts()
        {
            firstPart = true;

            //Add the initial BodyParts
            for (int i = 0; i < initialAmount; i++)
            {
                //Use invoke to avoid a weird bug where the snake goes down at the beginning.
                Invoke("AddBodyPart", 0.1f);
            }
        }

        // Update is called once per frame
        void Update()
        {
            //We constantly move if the game is on
            if (SnakeVSBlockGameController.gameState == SnakeVSBlockGameController.GameState.GAME)
            {
                Move();

                //Check if no more snake parts
                if (BodyParts.Count == 0)
                    GC.SetGameover();
            }

            //Update the Parts Amount text
            if (PartsAmountTextMesh != null)
                PartsAmountTextMesh.text = transform.childCount + "";
            
#if UNITY_EDITOR
            if (!isAddSpeed && Input.GetKeyDown(KeyCode.J) && BodyParts.Count >= 6)
#else
            if (!isAddSpeed && DealCommand.GetKeyDown(1,(AppKeyCode)3) && BodyParts.Count >= 6)
#endif            
            {
                isAddSpeed = true;
                StartCoroutine(AddBodyPartSpeed());
            }
            else
            {
                //提示身体长度大于10才能使用加速
            }
                
        }

        IEnumerator AddBodyPartSpeed()
        {
            //消耗身体长度
            for (int i = 0; i < 2; i++)
            {
                var Part = BodyParts[BodyParts.Count-1];
                BodyParts.Remove(BodyParts[BodyParts.Count-1]);
                Destroy(Part.gameObject);
            }
            addSpeedVFX.position = BodyParts[0].position;
            addSpeedVFX.GetComponent<ParticleSystem>().Play();
            yield return new WaitForSeconds(4f);
            isAddSpeed = false;
        }
        public void Move()
        {
            if(Time.timeScale == 0)
                return;
            float curSpeed = speed;
            if (isAddSpeed)
                curSpeed += 1;
            curSpeed = Mathf.Min(7f, curSpeed);
            if (isAddSpeed)
            {
                addSpeedVFX.position = BodyParts[0].position;
                addSpeedVFX.localScale = BodyParts[0].transform.localScale;
            }
            else
                addSpeedVFX.GetComponent<ParticleSystem>().Stop();

            LerpTimeY = 0.225f + (curSpeed-3) * 0.04f - BodyParts.Count * 0.002f;
            //Always move the body Up
            if (BodyParts.Count > 0)
                BodyParts[0].Translate(Vector2.up * curSpeed * Time.smoothDeltaTime);

            //check if we are still on screen
            float maxX = Camera.main.orthographicSize * Screen.width / Screen.height;

            if (BodyParts.Count > 0)
            {
                if (BodyParts[0].position.x > maxX) //Right pos
                {
                    BodyParts[0].position = new Vector3(maxX - 0.01f, BodyParts[0].position.y, BodyParts[0].position.z);
                }
                else if (BodyParts[0].position.x < -maxX) //Left pos
                {
                    BodyParts[0].position =
                        new Vector3(-maxX + 0.01f, BodyParts[0].position.y, BodyParts[0].position.z);
                }
            }

            float horizontalInput = 0f;
#if UNITY_EDITOR
            // 处理键盘输入来控制蛇的水平移动
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    if (isAddSpeed)
                        horizontalInput = -0.3f;
                    else
                        horizontalInput = -0.25f;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    if (isAddSpeed)
                        horizontalInput = 0.3f;
                    else
                        horizontalInput = 0.25f;
                }
            }
#else
            // 处理键盘输入来控制蛇的水平移动
            if (DealCommand.GetKey(1, (AppKeyCode)6) || DealCommand.GetKey(1, (AppKeyCode)1))
            {
                if (DealCommand.GetKey(1, (AppKeyCode)6)){ 
                    if (isAddSpeed)
                        horizontalInput = -0.2f;
                    else
                        horizontalInput = -0.15f;
                }

                if (DealCommand.GetKey(1, (AppKeyCode)1))
                {
                    if (isAddSpeed)
                        horizontalInput = 0.2f;
                    else
                        horizontalInput = 0.15f;
                }
                    
            }
#endif            
            else
            {
                horizontalInput = 0f;
            } 
            if (horizontalInput != 0 && BodyParts.Count > 0 && Mathf.Abs(BodyParts[0].position.x) < maxX)
            {
                BodyParts[0].GetComponent<Rigidbody2D>().AddForce(Vector2.right * rotationSpeed * horizontalInput);
            }

            //Move the other body parts depending on the Head, that's why we start the loop at 1
            for (int i = 1; i < BodyParts.Count; i++)
            {
                curBodyPart = BodyParts[i];
                prevBodyPart = BodyParts[i - 1];

                distance = Vector3.Distance(prevBodyPart.position, curBodyPart.position);

                Vector3 newPos = prevBodyPart.position;

                newPos.z = BodyParts[0].position.z;

                //Try 2 Lerps, one on the x pos and one on the Y
                Vector3 pos = curBodyPart.position;

                pos.x = Mathf.Lerp(pos.x, newPos.x, LerpTimeX);
                pos.y = Mathf.Lerp(pos.y, newPos.y, LerpTimeY);

                curBodyPart.position = pos;
            }
        }

        public void AddBodyPart()
        {
            Transform newPart;

            if (firstPart)
            {
                newPart = (Instantiate(BodyPrefab, new Vector3(0, 0, 0),
                    Quaternion.identity) as GameObject).transform;

                //Disable the collision with snake

                // Set this part as the parent of the Text Mesh
                PartsAmountTextMesh.transform.parent = newPart;

                //Place it correctly
                PartsAmountTextMesh.transform.position = newPart.position +
                                                         new Vector3(0, 0.5f, 0);

                firstPart = false;
            }
            else
                newPart = (Instantiate(BodyPrefab, BodyParts[BodyParts.Count - 1].position,
                    BodyParts[BodyParts.Count - 1].rotation) as GameObject).transform;


            newPart.SetParent(transform);

            BodyParts.Add(newPart);
        }
    }
}