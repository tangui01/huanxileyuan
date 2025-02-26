using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    GameObject thisEnemy;
    int healthIndex;
    public int damage = 0;
    public int health;
    public int minStage;
    [SerializeField] public bool HasGun;
    int currentMaxStage;
    bool canShoot = false;
    Transform enemyBulletPool;
    Transform mainCameraTransform;

    static int bulletIndex = 0;
    [SerializeField] [HideInInspector] public float fireRate = 0.5f;
    float fireRateCounter = 0;
    int[] healthValuesMainGunForAirBasic = new int[] { 50, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
    int[] healthValuesWingGunLvlForAirBasic = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110 };
    int[] healthValuesSideGunLvlForAirBasic = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110 };
    int[] healthValuesBombLvlForAirBasic = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110 };
    int[] healthValuesTeslaLvlForAirBasic = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110 };
    int[] healthValuesLaserLvlForAirBasic = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110 };
    int[] healthValuesBladesLvlForAirBasic = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110 };

    int[] healthValuesMainGunForAirIntermediate =
        new int[] { 100, 150, 300, 450, 600, 750, 900, 1050, 1200, 1350, 1500 };

    int[] healthValuesWingGunLvlForAirIntermediate = new int[] { 10, 25, 40, 55, 70, 85, 100, 115, 130, 145, 160 };
    int[] healthValuesSideGunLvlForAirIntermediate = new int[] { 10, 25, 40, 55, 70, 85, 100, 115, 130, 145, 160 };
    int[] healthValuesBombLvlForAirIntermediate = new int[] { 10, 25, 40, 55, 70, 85, 100, 115, 130, 145, 160 };
    int[] healthValuesTeslaLvlForAirIntermediate = new int[] { 10, 25, 40, 55, 70, 85, 100, 115, 130, 145, 160 };
    int[] healthValuesLaserLvlForAirIntermediate = new int[] { 10, 25, 40, 55, 70, 85, 100, 115, 130, 145, 160 };
    int[] healthValuesBladesLvlForAirIntermediate = new int[] { 10, 25, 40, 55, 70, 85, 100, 115, 130, 145, 160 };

    int[] healthValuesMainGunForAirAdvanced = new int[] { 150, 200, 400, 600, 800, 1000, 1200, 1400, 1600, 1800, 2000 };
    int[] healthValuesWingGunLvlForAirAdvanced = new int[] { 20, 30, 60, 90, 120, 150, 170, 190, 210, 230, 250 };
    int[] healthValuesSideGunLvlForAirAdvanced = new int[] { 20, 30, 60, 90, 120, 150, 170, 190, 210, 230, 250 };
    int[] healthValuesBombLvlForAirAdvanced = new int[] { 20, 30, 60, 90, 120, 150, 170, 190, 210, 230, 250 };
    int[] healthValuesTeslaLvlForAirAdvanced = new int[] { 20, 30, 60, 90, 120, 150, 170, 190, 210, 230, 250 };
    int[] healthValuesLaserLvlForAirAdvanced = new int[] { 20, 30, 60, 90, 120, 150, 170, 190, 210, 230, 250 };
    int[] healthValuesBladesLvlForAirAdvanced = new int[] { 20, 30, 60, 90, 120, 150, 170, 190, 210, 230, 250 };

    int[] healthValuesMainGunForGroundAndWaterStatic =
        new int[] { 70, 130, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesWingGunLvlForGroundAndWaterStatic =
        new int[] { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesSideGunLvlForGroundAndWaterStatic =
        new int[] { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesBombLvlForGroundAndWaterStatic =
        new int[] { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesTeslaLvlForGroundAndWaterStatic =
        new int[] { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesLaserLvlForGroundAndWaterStatic =
        new int[] { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesBladesLvlForGroundAndWaterStatic =
        new int[] { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesMainGunForGroundAndWaterMobile =
        new int[] { 90, 140, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesWingGunLvlForGroundAndWaterMobile =
        new int[] { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesSideGunLvlForGroundAndWaterMobile =
        new int[] { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesBombLvlForGroundAndWaterMobile =
        new int[] { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesTeslaLvlForGroundAndWaterMobile =
        new int[] { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesLaserLvlForGroundAndWaterMobile =
        new int[] { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    int[] healthValuesBladesLvlForGroundAndWaterMobile =
        new int[] { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550 };

    public enum EnemyType
    {
        AirBasic, // leteci neprijatelj bez pucaljki
        AirIntermediate, // leteci neprijatelj sa pucaljkama, kao i razne prepreke
        AirAdvanced, //napredne letelice
        GroundAndWaterStatic, //zemljani mobilni(krecu se) neprijatelj(tenk, razna vozila...) svi pucaju
        GroundAndWaterMobile //vodeni staticni neprijatelj(brod, podmornica...) svi pucaju
    };

    [SerializeField]
    public enum AirBasic
    {
        AirBasic1StraighForward,
        AirBasic2UpperLeftLowerRight,
        AirBasic3UpperRightLowerLeft,
        AirBasic4Upper180RightToLeft,
        AirBasic5LeftMiddleRightMiddlePlusOffset,
        AirBasic6RightMiddleLeftMiddlePlusOffset,
        AirBasic7MiddleLeftToRightWithJump,
        AirBasic8LeftToRightMiddleDown,
        AirBasic9TwoEnemiesMiddleThenLeftRight,
        AirBasic10FromRightAndLeftToMiddle,
        AirBasic11UpperLeftMiddleRightTwoWaves,
        AirBasic12MiddleToRightTurnAndBackStraightLine,
        AirBasic13CikCak,
        AirBasic14TwoWavesStraightForwardAndLeftToRight,
        AirBasic15TwoWavesStraightForwardLeftAndRight,
        AirBasic16Loop,
        AirBasic17Fireballs,
        AirBasic18AirMines,
        AirBasic19Rockets,
        AirBasic20JetPackOne,
        AirBasic21JetPackTwo,
        AirBasic22Tesla,
        AirBasic23JetPackRockets,
    };

    [SerializeField]
    public enum AirIntermediate
    {
        AirIntermediate1Fireballs,
        AirIntermediate2AirMines,
        AirIntermediate3AirLavirint,
        AirIntermediate4GroundThunder,
        AirIntermediate5DumbSentinels,
        AirIntermediate6OneAndFire,
        AirIntermediate7TwoAndFire,
        AirIntermediate8BottomLeftThenBottomRight,
        AirIntermediate9AirplaneBombKamikaza,
        AirIntermediate10CikCakWithPauseAndFire,
        AirIntermediate11Rockets,
        AirIntermediate12UpperRightToMiddleWithFire,
        AirIntermediate13Helicopter,
        AirIntermediate14RightUpToCenterPauseFireToLeftDown,
        AirIntermediate15JetPackOne,
        AirIntermediate16JetPackTwo,
        AirIntermediate17Kamikaza,
        AirIntermediate18CountdownMine,
        AirIntermediate19Tesla,
        AirIntermediate20MaceGoblin,
        AirIntermediate21JetPackRockets,
    };

    [SerializeField]
    public enum AirAdvanced
    {
        AirAdvanced1Tesla,
        AirAdvanced2OneToMany,
        AirAdvanced3WithDestructableObjectShield,
        AirAdvanced4WithBlades,
        AirAdvanced5FireWithRotation,
        AirAdvanced6FireForwardAllDirections,
        AirAdvanced7AirplaneWithLaser,
        AirAdvanced8MaceGoblin,
        AirAdvanced9AirMines,
        AirAdvanced10Kamikaza,
        AirAdvanced11CountdownMine,
    };

    [SerializeField]
    public enum GroundAndWaterStatic
    {
        TankStatic,
        Turret1,
        Turret2,
        Catapult,
    };

    [SerializeField]
    public enum GroundAndWaterMobile
    {
        TankMobile,
        ShipMobile,
    };

    public EnemyType TypeOfEnemy; // has a drop down list in inspector.
    [HideInInspector] public AirBasic AirBasicReference;
    [HideInInspector] public AirIntermediate AirIntermediateReference;
    [HideInInspector] public AirAdvanced AirAdvancedReference;
    [HideInInspector] public GroundAndWaterStatic GroundAndWaterStaticReference;
    [HideInInspector] public GroundAndWaterMobile GroundAndWaterMobileReference;

    bool continuousDamage = false;

    // Use this for initialization
    void Start()
    {
        thisEnemy = this.gameObject;
        if (PlayerPrefs.HasKey("MaxStage"))
        {
            int cms = PlayerPrefs.GetInt("MaxStage");
            int ghe = PlayerPrefs.GetInt("ghE67+=as23") - 5;
            int fsd = PlayerPrefs.GetInt("Fsdfs+=as23") - 11;

            if (cms == ghe)
            {
                if (cms == fsd)
                {
                    currentMaxStage = fsd;
                }
                else
                {
                    currentMaxStage = 1;
                }
            }
            else
            {
                currentMaxStage = 1;
            }
        }
        else
        {
            currentMaxStage = 1;
        }

        enemyBulletPool = GameObject.Find("EnemyBulletPool").transform;

        if (!HasGun)
        {
            Object.Destroy(thisEnemy.transform.Find("BulletPool").transform.gameObject);
        }

        InitializeEnemyHealth();
        fireRate = 1.75f; //0.95f;
        mainCameraTransform = Camera.main.transform;
        damage = (int)((LevelGenerator.currentStage - 1) * 20 + (float)PandaPlane.Instance.healthValues[1] * 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
//		TargetAndFire();
        if (HasGun && canShoot)
        {
            if (fireRateCounter >= fireRate)
            {
                FireBullet();
                fireRateCounter = 0;
            }
            else
            {
                fireRateCounter += Time.deltaTime;
            }
        }
    }

    public void InitializeEnemyHealth()
    {
        health = 0;
        if (TypeOfEnemy == EnemyType.AirBasic)
        {
            health = healthValuesMainGunForAirBasic[PandaPlane.Instance.mainGunLvl];
            health += healthValuesWingGunLvlForAirBasic[PandaPlane.Instance.wingGunLvl];
            health += healthValuesSideGunLvlForAirBasic[PandaPlane.Instance.sideGunLvl];
            health += healthValuesBombLvlForAirBasic[PandaPlane.Instance.bombLvl];
            health += healthValuesTeslaLvlForAirBasic[PandaPlane.Instance.teslaLvl];
            health += healthValuesLaserLvlForAirBasic[PandaPlane.Instance.laserLvl];
            health += healthValuesBladesLvlForAirBasic[PandaPlane.Instance.bladesLvl];

            if (currentMaxStage > 1)
            {
                if (AirBasicReference == AirBasic.AirBasic1StraighForward)
                {
                    health += 50;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic2UpperLeftLowerRight)
                {
                    health += 50;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic3UpperRightLowerLeft)
                {
                    health += 50;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic4Upper180RightToLeft)
                {
                    health += 50;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic5LeftMiddleRightMiddlePlusOffset)
                {
                    health += 50;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic6RightMiddleLeftMiddlePlusOffset)
                {
                    health += 40;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic7MiddleLeftToRightWithJump)
                {
                    health += 40;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic8LeftToRightMiddleDown)
                {
                    health += 40;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic9TwoEnemiesMiddleThenLeftRight)
                {
                    health += 50;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic10FromRightAndLeftToMiddle)
                {
                    health += 50;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic11UpperLeftMiddleRightTwoWaves)
                {
                    health += 50;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic12MiddleToRightTurnAndBackStraightLine)
                {
                    health += 40;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic13CikCak)
                {
                    health += 60;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic14TwoWavesStraightForwardAndLeftToRight)
                {
                    health += 40;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic15TwoWavesStraightForwardLeftAndRight)
                {
                    health += 45;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic16Loop)
                {
                    health += 45;
                    damage += 10;
                }
                else if (AirBasicReference == AirBasic.AirBasic17Fireballs)
                {
                    health += 45;
                    damage += 50;
                }
                else if (AirBasicReference == AirBasic.AirBasic18AirMines)
                {
                    health += 150;
                    damage += 100;
                }
                else if (AirBasicReference == AirBasic.AirBasic19Rockets)
                {
                    health += 150;
                    damage += 100;
                }
                else if (AirBasicReference == AirBasic.AirBasic20JetPackOne)
                {
                    health += 600;
                    damage += 60;
                }
                else if (AirBasicReference == AirBasic.AirBasic21JetPackTwo)
                {
                    health += 300;
                    damage += 30;
                }
                else if (AirBasicReference == AirBasic.AirBasic22Tesla)
                {
                    health += 400;
                    damage += 100;
                }
                else if (AirBasicReference == AirBasic.AirBasic23JetPackRockets)
                {
                    health += 400;
                    damage += 100;
                }
            }
        }
        else if (TypeOfEnemy == EnemyType.AirIntermediate)
        {
            health = healthValuesMainGunForAirIntermediate[PandaPlane.Instance.mainGunLvl];
            health += healthValuesWingGunLvlForAirIntermediate[PandaPlane.Instance.wingGunLvl];
            health += healthValuesSideGunLvlForAirIntermediate[PandaPlane.Instance.sideGunLvl];
            health += healthValuesBombLvlForAirIntermediate[PandaPlane.Instance.bombLvl];
            health += healthValuesTeslaLvlForAirIntermediate[PandaPlane.Instance.teslaLvl];
            health += healthValuesLaserLvlForAirIntermediate[PandaPlane.Instance.laserLvl];
            health += healthValuesBladesLvlForAirIntermediate[PandaPlane.Instance.bladesLvl];

            if (currentMaxStage > 1)
            {
                if (AirIntermediateReference == AirIntermediate.AirIntermediate1Fireballs)
                {
                    health += 15;
                    damage += 20;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate2AirMines)
                {
                    health += 250;
                    damage += 60;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate3AirLavirint)
                {
                    health += 20;
                    damage += 10;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate4GroundThunder)
                {
                    health += 20;
                    damage += 10;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate5DumbSentinels)
                {
                    health += 25;
                    damage += 10;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate6OneAndFire)
                {
                    health += 800;
                    damage += 80;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate7TwoAndFire)
                {
                    health += 400;
                    damage += 40;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate8BottomLeftThenBottomRight)
                {
                    health += 200;
                    damage += 30;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate9AirplaneBombKamikaza)
                {
                    health += 200;
                    damage += 40;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate10CikCakWithPauseAndFire)
                {
                    health += 400;
                    damage += 50;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate11Rockets)
                {
                    health += 200;
                    damage += 10;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate12UpperRightToMiddleWithFire)
                {
                    health += 150;
                    damage += 30;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate13Helicopter)
                {
                    health += 60;
                    damage += 50;
                }
                else if (AirIntermediateReference ==
                         AirIntermediate.AirIntermediate14RightUpToCenterPauseFireToLeftDown)
                {
                    health += 60;
                    damage += 60;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate15JetPackOne)
                {
                    health += 800;
                    damage += 80;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate16JetPackTwo)
                {
                    health += 400;
                    damage += 40;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate17Kamikaza)
                {
                    health += 350;
                    damage += 40;
                    Invoke("GoblinKamikazaLaugh", 0.5f);
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate18CountdownMine)
                {
                    health += 400;
                    damage += 50;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate19Tesla)
                {
                    health += 800;
                    damage += 120;
                }
                else if (AirIntermediateReference == AirIntermediate.AirIntermediate20MaceGoblin)
                {
                    health += 800;
                    damage += 100;
                }
            }
        }
        else if (TypeOfEnemy == EnemyType.AirAdvanced)
        {
            health = healthValuesMainGunForAirAdvanced[PandaPlane.Instance.mainGunLvl];
            health += healthValuesWingGunLvlForAirAdvanced[PandaPlane.Instance.wingGunLvl];
            health += healthValuesSideGunLvlForAirAdvanced[PandaPlane.Instance.sideGunLvl];
            health += healthValuesBombLvlForAirAdvanced[PandaPlane.Instance.bombLvl];
            health += healthValuesTeslaLvlForAirAdvanced[PandaPlane.Instance.teslaLvl];
            health += healthValuesLaserLvlForAirAdvanced[PandaPlane.Instance.laserLvl];
            health += healthValuesBladesLvlForAirAdvanced[PandaPlane.Instance.bladesLvl];

            if (AirAdvancedReference == AirAdvanced.AirAdvanced1Tesla)
            {
                health += 1000;
                damage += 130;
            }
            else if (AirAdvancedReference == AirAdvanced.AirAdvanced2OneToMany)
            {
                health += 600;
                damage += 120;
            }
            else if (AirAdvancedReference == AirAdvanced.AirAdvanced3WithDestructableObjectShield)
            {
                health += 100;
                damage += 10;
            }
            else if (AirAdvancedReference == AirAdvanced.AirAdvanced4WithBlades)
            {
                health += 100;
                damage += 10;
            }
            else if (AirAdvancedReference == AirAdvanced.AirAdvanced5FireWithRotation)
            {
                health += 100;
                damage += 10;
            }
            else if (AirAdvancedReference == AirAdvanced.AirAdvanced6FireForwardAllDirections)
            {
                health += 100;
                damage += 10;
            }
            else if (AirAdvancedReference == AirAdvanced.AirAdvanced7AirplaneWithLaser)
            {
                health += 100;
                damage += 10;
            }
            else if (AirAdvancedReference == AirAdvanced.AirAdvanced8MaceGoblin)
            {
                health += 850;
                damage += 130;
            }
            else if (AirAdvancedReference == AirAdvanced.AirAdvanced9AirMines)
            {
                health += 300;
                damage += 80;
            }
            else if (AirAdvancedReference == AirAdvanced.AirAdvanced10Kamikaza)
            {
                health += 600;
                damage += 90;
                Invoke("GoblinKamikazaLaugh", 0.5f);
            }
            else if (AirAdvancedReference == AirAdvanced.AirAdvanced11CountdownMine)
            {
                health += 600;
                damage += 75;
            }
        }
        else if (TypeOfEnemy == EnemyType.GroundAndWaterStatic)
        {
            health = healthValuesMainGunForGroundAndWaterStatic[PandaPlane.Instance.mainGunLvl];
            health += healthValuesWingGunLvlForGroundAndWaterStatic[PandaPlane.Instance.wingGunLvl];
            health += healthValuesSideGunLvlForGroundAndWaterStatic[PandaPlane.Instance.sideGunLvl];
            health += healthValuesBombLvlForGroundAndWaterStatic[PandaPlane.Instance.bombLvl];
            health += healthValuesTeslaLvlForGroundAndWaterStatic[PandaPlane.Instance.teslaLvl];
            health += healthValuesLaserLvlForGroundAndWaterStatic[PandaPlane.Instance.laserLvl];
            health += healthValuesBladesLvlForGroundAndWaterStatic[PandaPlane.Instance.bladesLvl];

            if (currentMaxStage > 1)
            {
                if (GroundAndWaterStaticReference == GroundAndWaterStatic.TankStatic)
                {
                    health += 40;
                    damage += 40;
                }
                else if (GroundAndWaterStaticReference == GroundAndWaterStatic.Turret1)
                {
                    health += 60;
                    damage += 20;
                }
                else if (GroundAndWaterStaticReference == GroundAndWaterStatic.Turret2)
                {
                    health += 50;
                    damage += 30;
                }
                else if (GroundAndWaterStaticReference == GroundAndWaterStatic.Catapult)
                {
                    health += 140;
                    damage += 75;
                }
            }
            else
            {
                health -= 150;
            }
        }
        else if (TypeOfEnemy == EnemyType.GroundAndWaterMobile)
        {
            health = healthValuesMainGunForGroundAndWaterMobile[PandaPlane.Instance.mainGunLvl];
            health += healthValuesSideGunLvlForGroundAndWaterMobile[PandaPlane.Instance.wingGunLvl];
            health += healthValuesSideGunLvlForGroundAndWaterMobile[PandaPlane.Instance.sideGunLvl];
            health += healthValuesBombLvlForGroundAndWaterMobile[PandaPlane.Instance.bombLvl];
            health += healthValuesTeslaLvlForGroundAndWaterMobile[PandaPlane.Instance.teslaLvl];
            health += healthValuesLaserLvlForGroundAndWaterMobile[PandaPlane.Instance.laserLvl];
            health += healthValuesBladesLvlForGroundAndWaterMobile[PandaPlane.Instance.bladesLvl];
            if (currentMaxStage > 1)
            {
                if (GroundAndWaterMobileReference == GroundAndWaterMobile.TankMobile)
                {
                    health += 50;
                    damage += 40;
                }
                else if (GroundAndWaterMobileReference == GroundAndWaterMobile.ShipMobile)
                {
                    health += 40;
                    damage += 30;
                }
            }
            else
            {
                health -= 150;
            }
        }

        health += (LevelGenerator.currentStage - 1) * 20;
        //		Debug.Log("Ukupan health neprijatelja je: "+health);
        //			mainGunLvl, wingGunLvl, sideGunLvl;
    }

//	public void TakeDamage(int damage)
//	{
//		if(health-damage<0)
//		{
//			if(continuousDamage)
//				continuousDamage = false;
////			Object.Destroy(this.gameObject);
////			this.gameObject.SetActive(false);
////			transform.parent.GetComponent<Animation>().Play("DeathPlane2"); // za avione
////			transform.parent.GetComponent<Animation>().Play("DeathPlane1");
//			health=0;
//			canShoot = false;
////			gameObject.transform.parent.parent.gameObject.SetActive(false);
//			transform.parent.GetComponent<Animation>().Play("Death");
//			CoinsOnDeath();
//
//		}
//
//		else
//		{
//			StartCoroutine("Flash");
//			health-=damage;
//		}
//	}
    void GoblinKamikazaLaugh()
    {
        if (gameObject.activeSelf)
            SoundManager.Instance.Play_GoblinLaugh();
    }

    public void TakeDamage(int damage)
    {
        if (health - damage < 0)
        {
            if (name.Contains("Helicopter") && GetComponent<AudioSource>().isPlaying && SoundManager.soundOn == 1)
                GetComponent<AudioSource>().Stop();
            //if(name.Contains("Helicopter"))
            //	SoundManager.Instance.Stop_HelicopterMoving();

            if (TypeOfEnemy == EnemyType.GroundAndWaterMobile)
            {
                if (GroundAndWaterMobileReference == GroundAndWaterMobile.ShipMobile)
                {
                    SoundManager.Instance.Play_ShipExplode();
                }
                else if (GroundAndWaterMobileReference == GroundAndWaterMobile.TankMobile)
                {
                    SoundManager.Instance.Play_TankExplode();
                }
            }
            else if (TypeOfEnemy == EnemyType.GroundAndWaterStatic)
            {
                if (GroundAndWaterStaticReference == GroundAndWaterStatic.TankStatic)
                {
                    SoundManager.Instance.Play_TankExplode();
                }
                else if (GroundAndWaterStaticReference == GroundAndWaterStatic.Turret1 ||
                         GroundAndWaterStaticReference == GroundAndWaterStatic.Turret2)
                {
                    SoundManager.Instance.Play_TurretExplode();
                }
            }
            else // if(TypeOfEnemy == EnemyType.AirBasic)
            {
                SoundManager.Instance.Play_EnemyPlaneExplode();
            }


            if (continuousDamage)
                continuousDamage = false;
            //   Object.Destroy(this.gameObject);
            //   this.gameObject.SetActive(false);
            //   transform.parent.GetComponent<Animation>().Play("DeathPlane2"); // za avione
            //   transform.parent.GetComponent<Animation>().Play("DeathPlane1");
            health = 0;
            canShoot = false;
            //   gameObject.transform.parent.parent.gameObject.SetActive(false);
            transform.parent.GetComponent<Animation>().Play("Death");
            PandaPlane.Instance.AddScore(25 * LevelGenerator.currentStage);
            CoinsOnDeath();
            PandaPlane.Instance.enemiesKilledFromLastCollectable++;
            if (PandaPlane.Instance.collectableNumber <= PandaPlane.Instance.enemiesKilledFromLastCollectable)
            {
                PandaPlane.Instance.enemiesKilledFromLastCollectable = 0;
                PandaPlane.Instance.collectableNumber = Random.Range(15, 21);
                CollectableOnDeath();
            }
        }

        else
        {
            SoundManager.Instance.Play_EnemyHit();
            StartCoroutine("Flash");
            health -= damage;
        }
    }


    void FireBullet()
    {
        if (transform.position.y > mainCameraTransform.position.y - 22f)
        {
            if (bulletIndex == enemyBulletPool.childCount)
                bulletIndex = 0;

            EnemyBullet tempScript = enemyBulletPool.GetChild(bulletIndex).GetComponent<EnemyBullet>();
            tempScript.GetComponent<EnemyDamage>().damage = damage;
            if (tempScript.available)
            {
                tempScript.enemyPosition = transform.position;
                tempScript.initialized = true;
                SoundManager.Instance.Play_EnemyFire();
                //break;
            }

            bulletIndex++;
        }
    }

    void OnBecameVisible()
    {
        if (health > 0)
        {
            canShoot = true;
            if (name.Contains("Helicopter") && !GetComponent<AudioSource>().isPlaying && SoundManager.soundOn == 1)
                GetComponent<AudioSource>().Play();
            //if(name.Contains("Helicopter"))
            //	SoundManager.Instance.Play_HelicopterMoving();
        }
    }

    void OnBecameInvisible()
    {
        if (gameObject != null && mainCameraTransform != null)
            if (transform.position.y < mainCameraTransform.position.y - 22)
                canShoot = false;

        if (name.Contains("Helicopter") && GetComponent<AudioSource>().isPlaying && SoundManager.soundOn == 1)
            GetComponent<AudioSource>().Stop();
        //if(name.Contains("Helicopter"))
        //	SoundManager.Instance.Stop_HelicopterMoving();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (transform.position.y < mainCameraTransform.position.y + 22)
        {
            if (col.tag.Equals("PlayerBullet"))
            {
                //			gameObject.SetActive(false);
                col.gameObject.SetActive(false);
                TakeDamage(PandaPlane.Instance.mainGunDamage); //25
                //			TakeDamage(30);
                //			TakeDamage(5);
            }
            else if (col.gameObject.tag.Equals("WingBullet"))
            {
                col.gameObject.SetActive(false);
                TakeDamage(PandaPlane.Instance.wingGunDamage);
            }
            else if (col.gameObject.tag.Equals("SideBullet"))
            {
                col.gameObject.SetActive(false);
                TakeDamage(PandaPlane.Instance.sideGunDamage);
            }
            else if (col.tag.Equals("Shield"))
            {
                StartCoroutine(col.GetComponent<Bomb>().ShieldHit());
                TakeDamage(5000);
            }
            else if (col.tag.Equals("Bomb"))
            {
                TakeDamage(PandaPlane.Instance.bombDamage);
            }
            else if (col.tag.Equals("Laser"))
            {
                continuousDamage = true;
                StartCoroutine(DoContinuousDamage(PandaPlane.Instance.laserDamage));
            }
            else if (col.tag.Equals("Tesla"))
            {
                continuousDamage = true;
                StartCoroutine(DoContinuousDamage(PandaPlane.Instance.teslaDamage));
            }
            else if (col.tag.Equals("Blades"))
            {
                TakeDamage(PandaPlane.Instance.bladesDamage);
            }
        }
//		else if(col.tag.Equals("PlayerRocket"))
//		{
//			Debug.Log("###Rockets### "+"BOOOOOOOM raketa pogodila cilj");
////						gameObject.transform.parent.parent.gameObject.SetActive(false);
////			col.gameObject.SetActive(false);
//			RocketsTest.targetAquired=false;
//			RocketsTest.target=null;
//			gameObject.tag = "Untagged";
//			col.gameObject.transform.parent.GetComponent<RocketsTest>().TargetDestroyed();
//			col.gameObject.transform.parent.GetComponent<Animation>().Play("RocketExplosion");
//			TakeDamage(300);
//			//			TakeDamage(30);
//			//			TakeDamage(5);
//		}
    }

    IEnumerator DoContinuousDamage(int damage)
    {
        while (continuousDamage)
        {
            TakeDamage(damage);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (continuousDamage)
            continuousDamage = false;
    }

    IEnumerator Flash()
    {
        transform.Find("EnemyHIT").GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        transform.Find("EnemyHIT").GetComponent<SpriteRenderer>().enabled = false;
    }

    void CoinsOnDeath()
    {
//		int number = Random.Range(1,20);
//		if((number%2)==1)
//		{
//			Debug.Log("Neparan broj");
//		}
//		else
//		{
//			Debug.Log("paran broj daj mu coine");
//		}

        int number = Random.Range(0, 5);

        for (int i = 0; i < number; i++)
        {
            float rotate = Random.Range(-179, 179);
            GameObject Star = (GameObject)Instantiate(Resources.Load("StarHolder"));
            Star.transform.rotation = Quaternion.Euler(0, 0, rotate);
            Star.transform.position = new Vector3(transform.position.x + Random.Range(-2f, 2f),
                transform.position.y + Random.Range(-2f, 2f), -45);
        }
    }

    void CollectableOnDeath()
    {
        
        do
        {
            PandaPlane.Instance.generatedCollectable = Random.Range(0, PandaPlane.Instance.collectables.Count);
            
        } while (PandaPlane.Instance.generatedCollectable == PandaPlane.Instance.lastGeneratedCollectable);


        while (PandaPlane.Instance.lastGeneratedCollectable == 5 || PandaPlane.Instance.lastGeneratedCollectable == 6 ||PandaPlane.Instance.lastGeneratedCollectable == 8)
        {
            PandaPlane.Instance.generatedCollectable = Random.Range(0, PandaPlane.Instance.collectables.Count);
            if (PandaPlane.Instance.generatedCollectable != 5 && PandaPlane.Instance.generatedCollectable != 6 && PandaPlane.Instance.generatedCollectable != 8)
            {
                break;
            }
        }
        PandaPlane.Instance.lastGeneratedCollectable = PandaPlane.Instance.generatedCollectable;
        GameObject Collectable = new GameObject();

        if (!AircraftBattleGameManager.Instance.bossTime)
        {
            switch (PandaPlane.Instance.collectables[PandaPlane.Instance.generatedCollectable])
            {
                //LEGEND: 1-health; 2-doubleStars; 3-magnet; 4-shield; 5-laser; 9-armor
                case 1: //generisi health
                    Collectable = (GameObject)Instantiate(Resources.Load("Collectables/PowerUp_Health"));
                    break;
                case 2: //generisi laser
                    Collectable = (GameObject)Instantiate(Resources.Load("Collectables/PowerUp_Tesla"));
                    break;
                case 3: //generisi magnet
                    Collectable = (GameObject)Instantiate(Resources.Load("Collectables/PowerUp_Magnet"));
                    break;
                case 4: //generisi shieldT
                    Collectable = (GameObject)Instantiate(Resources.Load("Collectables/PowerUp_Shield"));
                    break;
                case 5: //generisi laser
                    Collectable = (GameObject)Instantiate(Resources.Load("Collectables/PowerUp_Laser"));
                    break;
                case 6: //generisi laser
                    Collectable = (GameObject)Instantiate(Resources.Load("Collectables/PowerUp_Bomb"));
                    break;

            }

            Collectable.transform.position = new Vector3(transform.position.x, transform.position.y, -46);
            Collectable.transform.GetChild(0).GetComponent<Animation>().Play();
            Collectable.transform.GetChild(0).GetComponent<Animation>()
                .PlayQueued("PowerUpIdle", QueueMode.CompleteOthers);
        }
    }
}