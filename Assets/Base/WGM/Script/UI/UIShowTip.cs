//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;

//public class UIShowTip : MonoBehaviour {

//    public int playerID;
//	public class ShowTip
//	{
//		public Transform body;
//		public Func<bool> funcTrue;
//		public bool chain;
//		public Action action;
//		public ShowTip(Transform body, Func<bool> funcTrue, bool chain = true, Action ac = null)
//		{
//			this.body = body;
//			this.funcTrue = funcTrue;
//			this.chain = chain;
//			this.action = ac;
//		}
//	}
//	protected List<ShowTip> mListShowTip = new List<ShowTip>();

//	// Use this for initialization
//	void Start () {
//        mListShowTip.Add(new ShowTip(transform.Find("WaitCoinOut"), () => { return DealCommand.instance.GetStatus(playerID, PrizeStatus.WaitCoinOut); }));
//        mListShowTip.Add(new ShowTip(transform.Find("ReadyCoinOut"), () => { return DealCommand.instance.GetStatus(playerID, PrizeStatus.ReadyCoinOut); }));
//        mListShowTip.Add(new ShowTip(transform.Find("CoinOuting"), () => { return DealCommand.instance.GetStatus(playerID, PrizeStatus.CoinOuting); }, true, CoinOuting));
//        mListShowTip.Add(new ShowTip(transform.Find("CoinOutError"), () => { return DealCommand.instance.GetStatus(playerID, PrizeStatus.CoinOutError); }, true, CoinOutError));
//        mListShowTip.Add(new ShowTip(transform.Find("CoinOutErrorMax"), () => { return DealCommand.instance.GetStatus(playerID, PrizeStatus.CoinOutErrMax); }, true, CoinOutErrorMax));
//		if(transform.Find("BackstageError") != null) mListShowTip.Add(new ShowTip(transform.Find("BackstageError"), () => { return DealCommand.g_backstage_ret <= 0; }, false));
//	}

//	List<Transform> show = new List<Transform>();
//	void Update()
//	{
//		int i;

//		show.Clear();
//		for(i = 0; i < mListShowTip.Count; i++) {
//			if(mListShowTip[i].funcTrue()) {
//				mListShowTip[i].body.gameObject.SetActive(true);
//				if(mListShowTip[i].chain) show.Add(mListShowTip[i].body);
//				if(mListShowTip[i].action != null) mListShowTip[i].action();
//			} else {
//				mListShowTip[i].body.gameObject.SetActive(false);
//			}
//		}

//		for(i = 0; i < show.Count; i++) {
//			show[i].localPosition = Vector3.up * (70 + 25 * i);
//		}
//	}

//	void CoinOuting()
//	{
//        int count = LibWGM.GetPlayer(playerID, GetPly.score) / LibWGM.GetConfig(GetConf.gift_score);
//        UILabel mUILabel = transform.Find("CoinOuting").GetComponent<UILabel>();

//        if (count == 0)
//        {
//            mUILabel.gameObject.SetActive(false);
//            return;
//        }
//        else
//        {
//            mUILabel.gameObject.SetActive(true);
//        }

//		if(LibWGM.data.prize_type == (int)te_PrizeType.e_Prize) {
//			transform.Find("CoinOuting").GetComponent<UILabel>().text = Localization.Get("正在退礼品") + count;
//		} else {
            
//			transform.Find("CoinOuting").GetComponent<UILabel>().text = Localization.Get("正在退票") + count;
//		}
//	}

//	void CoinOutError()
//	{
//		if(LibWGM.data.prize_type == (int)te_PrizeType.e_Prize) {
//			transform.Find("CoinOutError").GetComponent<UILabel>().text = Localization.Get("退礼品错误");
//		} else {
//			transform.Find("CoinOutError").GetComponent<UILabel>().text = Localization.Get("退票错误");
//		}
//	}

//	void CoinOutErrorMax()
//	{
//		if(LibWGM.data.prize_type == (int)te_PrizeType.e_Prize) {
//            transform.Find("CoinOutErrorMax").GetComponent<UILabel>().text = Localization.Get("退礼品错误重启") + (LibWGM.GetPlayer(playerID, GetPly.score) / LibWGM.GetConfig(GetConf.gift_score));
//		} else {
//            transform.Find("CoinOutErrorMax").GetComponent<UILabel>().text = Localization.Get("退票错误重启") + (LibWGM.GetPlayer(playerID, GetPly.score) / LibWGM.GetConfig(GetConf.gift_score));
//		}
//	}
//}
