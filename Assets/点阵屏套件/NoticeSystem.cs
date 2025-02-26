using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using WGM;

public class NoticeSystem : MonoBehaviour
{
    UILabel[] uILabels;
    public Queue<string> notices=new Queue<string>();
    public Queue<string> notified=new Queue<string>();
    public float between=50;

    UILabel lastUIlabel;
    [Header("滚完一次的时间")]
    public float time;
    float startX,endX;
    public static List<MqttRankingListItem> mqttRankingListItems = new List<MqttRankingListItem>();
    // Start is called before the first frame update
    void Awake()
    {
        if (LibWGM.machine.Language == 2)
        {
            gameObject.SetActive(false);
            return;
        }
        uILabels = GetComponentsInChildren<UILabel>();
        for (int i = 0; i < uILabels.Length; i++)
        {
            uILabels[i].enabled = false;
        }
        startX = GetComponent<UIPanel>().width / 2;
        endX = -startX;
        
    }
    private void OnEnable()
    {
        LedScreenSerialPort.onRankingUpdate += UpdateRanking;
        StartCoroutine("RequstRanking");
        
    }
    
    private void OnDisable()
    {
        LedScreenSerialPort.onRankingUpdate -= UpdateRanking;
    }

    void SendScore()
    {
        int score = 0;
        ReqMqttRankingList reqMqttRankingList = new ReqMqttRankingList();
        reqMqttRankingList.IsUpdate = false;
        reqMqttRankingList.Score = 100;
        reqMqttRankingList.RankTopMax = 3;
        string json = JsonConvert.SerializeObject(reqMqttRankingList);
        byte[] byteArray = Encoding.UTF8.GetBytes(json);

        byte[] data = new byte[1024];
        data[0] = 0x01;
        byteArray.CopyTo(data, 1);
        int cnt = 0;
        cnt = byteArray.Length + 1;
        if(LibWGM.machine.DeveloperMode)
        print("向盒子传送分数,反回一个排行榜");
        DealCommand.Instance.SerialPortManager.SendCommand(LibWGM.CmdType.UploadScore, data, cnt);
    }

    void UpdateRanking(bool self, MqttRankingListItem json)
    {
        if (json == null)
        {
            StopCoroutine("RequstRanking");
            Debug.LogError ("排行榜没有数据");
            return;
        }
        //RespMqttRankingList list=JsonConvert.DeserializeObject<RespMqttRankingList>(json);
        if (!self)
            mqttRankingListItems.Add(json);
    
    }
    void OnRankingList(List<MqttRankingListItem> respRankingList)
    {
        if (mqttRankingListItems .Count >0)
        {
            List<string> strList = new List<string>();
            if (mqttRankingListItems.Count > 0 && mqttRankingListItems[0].Score > 0)
                strList.Add(Localization.Get("第一名：") + $"{mqttRankingListItems[0].City} {mqttRankingListItems[0].Score} 分");
            if (mqttRankingListItems.Count>1&&mqttRankingListItems[1].Score > 0)
                strList.Add(Localization.Get("第二名：") + $"{mqttRankingListItems[1].City} {mqttRankingListItems[1].Score} 分");
            if (mqttRankingListItems.Count > 2 && mqttRankingListItems[2].Score > 0)
                strList.Add(Localization.Get("第三名：") + $"{mqttRankingListItems[2].City} {mqttRankingListItems[2].Score} 分");
            Play(strList.ToArray());
        }

    }
    public void Play(string[] strs)
    {
        notices.Clear();
        notified.Clear();
        for (int i = 0; i < strs.Length; i++)
        {
            notices.Enqueue(strs[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (notices.Count > 0&&HasFreeLabel()&&CanPlay())
        {
            PlayNotice(notices.Dequeue());
        }
        if (notices.Count == 0 )
        {
            notices = notified;
        }
    }

    void PlayNotice(string n)
    {
        notified.Enqueue(n);
        lastUIlabel = GetFreeLabel();
        lastUIlabel.text = n;
        lastUIlabel.enabled = true;
        UILabel uILabel = lastUIlabel;
        float labelStartX = startX + lastUIlabel.width / 2;
        uILabel.transform.localPosition = Vector3.right * labelStartX;
        float end = endX - lastUIlabel.width / 2;
        uILabel.transform.DOLocalMove(Vector3.right* end,time).SetEase (Ease.Linear).OnComplete(()=> { uILabel.enabled = false;  });
    }

    bool HasFreeLabel()
    {
        for (int i = 0; i < uILabels.Length; i++)
        {
            if (!uILabels[i].enabled)
                return true;
        }
        return false;
    }

    bool CanPlay()
    {
        if (lastUIlabel != null)
        {
            if (lastUIlabel.enabled == false)
                return true;

            float labelEndX = lastUIlabel.transform.localPosition.x + lastUIlabel.width / 2;
            if (lastUIlabel.transform.localPosition.x + labelEndX + between < startX)
                return true;
            else
                return false;
        }
        return true;
    }

    UILabel GetFreeLabel()
    {
        for (int i = 0; i < uILabels.Length; i++)
        {
            if (!uILabels[i].enabled)
                return uILabels[i];
        }
        return null;
    }

    IEnumerator RequstRanking()
    {
        while (mqttRankingListItems.Count == 0)
        {
            SendScore();
            yield return new WaitForSeconds(35);
        }
        StartCoroutine(Show(1));
     
    }

    IEnumerator Show(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnRankingList(mqttRankingListItems);
    }
}
