using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using WGM;
using Random = UnityEngine.Random;

public class TheNameOfAGameController : MonoBehaviour
{
    private TheNameOfAGameCore core;
    private Text txtRecord;
    private TextMeshProUGUI txtScore;
    private Transform bgTf;//背景变换组件 
    private RectTransform[,] bgTfArray;//背景图片的变换组件数组
    private AudioSource audioSource;
    private TheNameOfAMoveDirection dir;
    private float fixtime = 0.1f;
    public bool isContinueGame = true;
    // private TheNameOfAResourceManager _aResourceManager;
    private void Start()
    {
        // _aResourceManager = GetComponent<TheNameOfAResourceManager>();
        core = new TheNameOfAGameCore();
        Screen.SetResolution(1024, 768, false);

        screenFaderImage = FindObjectOfType<TheNameOfAUIManager>().Instance.GetUIByName(TheNameOfANameManager.CANVAS, TheNameOfANameManager.SCREEN_FADER).GetComponent<Image>();
        bgTf = this.transform.Find(TheNameOfANameManager.BACKGROUND);
        txtScore = GameObject.Find("Score")?.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        bgTfArray = new RectTransform[4, 4];
        spriteArray = new TheNameOfANumberSprite[4, 4];

        audioSource = this.GetComponent<AudioSource>();

        SetGridLayoutGroup();

        InitBackground();
        

        Invoke("InitNewNumber", 0.4f);
    }

    /// <summary>
    /// 方格间隔
    /// </summary>
    public int interval = 15;
    //设置布局组
    private void SetGridLayoutGroup()
    {
        GridLayoutGroup grid = bgTf.GetComponent<GridLayoutGroup>();

        grid.padding = new RectOffset(interval, interval, interval, interval);
        float size = (this.GetComponent<RectTransform>().sizeDelta.x - 5 * interval) / 4;
        grid.cellSize = new Vector2(size, size);
        grid.spacing = new Vector2(interval, interval);
    }

    //初始化新数字
    private void InitNewNumber()
    {
        core.CalculateEmpty();//统计空位
        GenerateNumberSprite();
        GenerateNumberSprite();
    }

    //创建背景精灵
    private RectTransform CreateBackgroundSprite(TheNameOfALocation loc)
    {
        GameObject spriteGo = new GameObject(loc.RIndex.ToString() + loc.CIndex);
        spriteGo.AddComponent<Image>().sprite = TheNameOfAResourceManager.GetImage(0);
        spriteGo.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        spriteGo.transform.SetParent(bgTf.transform, false);
        return spriteGo.GetComponent<RectTransform>();
    }

    //初始化  创建背景图片
    private void InitBackground()
    {
        for (int r = 0; r < 4; r++)
            for (int c = 0; c < 4; c++)
                bgTfArray[r, c] = CreateBackgroundSprite(new TheNameOfALocation(r, c));
    }

    //创建数字精灵
    private TheNameOfANumberSprite CreateNumberSprite(TheNameOfALocation loc, int number)
    {
        GameObject spriteObject = new GameObject("NumSprite");
        spriteObject.AddComponent<Image>();
        spriteObject.transform.SetParent(this.transform,false);
        TheNameOfANumberSprite script = spriteObject.AddComponent<TheNameOfANumberSprite>(); 
        script.Init(number, bgTfArray[loc.RIndex, loc.CIndex].position, bgTfArray[loc.RIndex, loc.CIndex].sizeDelta.x);
        return script;
    }

    //精灵数组
    private TheNameOfANumberSprite[,] spriteArray;
    //创建新数字精灵
    private void GenerateNumberSprite()
    {
        TheNameOfALocation? loc;
        int? num;
        //生成新数
        core.GenerateNumber(out num, out loc);
        TheNameOfANumberSprite script = CreateNumberSprite(loc.Value, num.Value);
        //添加到精灵数组中
        spriteArray[loc.Value.RIndex, loc.Value.CIndex] = script;
        script.CreateEffect();
    }

    public void SuspendGame()
    {
        isContinueGame = false;
    }

    public void ContinueGame()
    {
        isContinueGame = true;
    }
    
    public TheNameOfAMoveDirection InputClick()
    {
        if (!isContinueGame)
        {
            return TheNameOfAMoveDirection.None;
        }
        if (DealCommand.GetKeyDown(1,(AppKeyCode)0))
        {
            return TheNameOfAMoveDirection.Up;
        }
        else if (DealCommand.GetKeyDown(1,(AppKeyCode)2))
        {
            return TheNameOfAMoveDirection.Down;
        }
        else if (DealCommand.GetKeyDown(1,(AppKeyCode)6))
        {
            return TheNameOfAMoveDirection.Left;
        }
#if UNITY_EDITOR
        else if (Input.GetKeyDown(KeyCode.D))
        {
            return TheNameOfAMoveDirection.Right;
        }
#else 
        else if (DealCommand.GetKeyDown(1,(AppKeyCode)1))
        {
            return TheNameOfAMoveDirection.Right;
        }
#endif        
        return TheNameOfAMoveDirection.None;
    }
    

    private void Update()
    {
        if (sceneStarting)
            StartScene(); 
        dir = InputClick();
        fixtime -= Time.deltaTime;
        if (dir != TheNameOfAMoveDirection.None && fixtime <= 0)
        {
            fixtime = 0.1f;
            core.Move(dir); 
        }
        


        //如果地图有改变
        if (core.IsChange)
        {
            SoundEffect();

            MoveEffects();

            ChangeMap();//修改地图

            DisplayScore();

            MergeEffects();//合并

            GenerateNumberSprite();

            if (core.IsOver())
                GameObject.FindObjectOfType<TheNameOfAGameOverController>().Display();

            core.IsChange = false;
        }
    }
    public List<AudioClip> audios = new List<AudioClip>();
    public GameObject VFXPool;
    private int VFXIndex = 0;
    private void SoundEffect()
    {
        AudioManager.Instance.SetEff(audios[0],1,0);
        if (core.MergeLocationList.Count > 0)
            AudioManager.Instance.SetEff(audios[1],1,1);
    }

    //数字精灵移动效果
    private void MoveEffects()
    {
        for (int i = 0; i < core.MoveDataList.Count; i++)
        {
            //从核心类中获取需要移动的精灵位置
            TheNameOfAMoveData data = core.MoveDataList[i];//包含原位置  目标位置
   
            //原位置精灵移动到目标位置的效果
            spriteArray[data.originalLoc.RIndex, data.originalLoc.CIndex].MoveEffect(bgTfArray[data.targetLoc.RIndex, data.targetLoc.CIndex].position);
            //将原位置引用复制到新位置中
            spriteArray[data.targetLoc.RIndex, data.targetLoc.CIndex] = spriteArray[data.originalLoc.RIndex, data.originalLoc.CIndex];//将当前位置图片精灵引用移动到上面0的位置
            //原位置引用清空
            spriteArray[data.originalLoc.RIndex, data.originalLoc.CIndex] = null;
        }
    }
    
    //合并效果
    private void MergeEffects()
    {
        for (int i = 0; i < core.MergeLocationList.Count; i++)
        {
            var vfx =  VFXPool.transform.GetChild(VFXIndex % VFXPool.transform.childCount).gameObject;
            VFXIndex++;
            spriteArray[core.MergeLocationList[i].RIndex, core.MergeLocationList[i].CIndex].MergeEffect(vfx);
        }
    }

    public GameObject CFXRprefabs;
    private GameObject CFXReff;
    public Transform CFXRend;
    private int VFXRarray = 8;
    public void CFXRplay(Transform cfxr)
    {
        Vector3 Startpos = cfxr.position;
        Startpos.z = 80;
        if (CFXReff == null)
        {
            CFXReff = Instantiate(CFXRprefabs,Startpos, Quaternion.identity);
        }
        else
        {
            CFXReff.transform.position = Startpos;
            CFXReff.SetActive(true);
        }

        GetComponent<AudioSource>().volume = LibWGM.machine.SeVolume / 10;
        
        CFXReff.GetComponent<ParticleSystem>().Play();
        CFXReff.transform.DOMove(CFXRend.position,0.95f).onComplete = () =>
        {
            StartCoroutine(CFXRMove());
        };
    }
    IEnumerator CFXRMove()
    {
        GetComponent<AudioSource>().Play();
        core.Score += 100; 
        DisplayScore();
        yield return new WaitForSeconds(1f);
        CFXReff.SetActive(false);
    }
    //修改地图
    public GameObject winCFXR;
    private int winCFXRarray = 128;
    private void ChangeMap()
    {
        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                if (core.Map[r, c] == 0)
                {
                    if (spriteArray[r, c] != null)//如果地图为0  界面上该位置存在图片 则销毁
                        Destroy(spriteArray[r, c].gameObject);
                }
                else
                {
                    spriteArray[r, c].SetImage(core.Map[r, c]);
                    if (VFXRarray == core.Map[r, c])
                    {
                        VFXRarray *=2 ;
                        CFXRplay(spriteArray[r, c].gameObject.transform);
                        if (core.Map[r, c] >= winCFXRarray)
                        {
                            winCFXRarray *= 2;
                            StartCoroutine(winCFXRplay());  
                        }

                    }
                }
            }
        }
    }

    IEnumerator winCFXRplay()
    {
        winCFXR.SetActive(true);
        winCFXR.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(2.5f);
        winCFXR.SetActive(false);
    }
    
    private Vector2 beginPointer;
    private bool isDown = false;

    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 重新开始
    /// </summary>
    public void Restart()
    {
        ClearSprite();
        txtScore.text = "0";
        core.Score = 0;
        core.ClearMap(); 
        Invoke("InitNewNumber", 0.4f);
    }

    //清空所有UI数字精灵
    private void ClearSprite()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).name != "Background" || this.transform.GetChild(i).name != "Hint")
                Destroy(this.transform.GetChild(i).gameObject);
        }
    }
    

    //显示成绩
    private void DisplayScore()
    {
        txtScore.text = core.Score.ToString();
        // if (int.Parse(txtRecord.text) < core.Score)
        //     txtRecord.text = txtScore.text;
    }

    private Image screenFaderImage;
    private bool sceneStarting = true;
    private void StartScene()
    {
        screenFaderImage.color = Color.Lerp(screenFaderImage.color, Color.clear, Time.deltaTime);
        if (screenFaderImage.color.a <= 0.01f)
        {
            screenFaderImage.color = Color.clear;
            sceneStarting = false;
        }
    }


}
