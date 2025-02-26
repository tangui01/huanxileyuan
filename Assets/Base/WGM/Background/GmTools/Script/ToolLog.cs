using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToolLog : MonoBehaviour {
    public static ToolLog Instance;
	public UILabel uil_title;
	public UILabel uil_log;
	public UILabel uil_fps;
	public GameObject m_go_window;

	public bool g_enable_log = false;
	private bool g_only_error = false;
	private bool g_pause_log = false;

	private const int MAX_DISP_LOG = 20;
	private string m_head_error   = "[ff0000]E[-]";
	private string m_head_warning = "[ffff00]W[-]";
	private string m_head_normal  = "[ffffff]@[-]";
	private string m_tooltip_only_error_en  = " [FF0000]F7[-]:[00FF00]only error[-]";
	private string m_tooltip_only_error_dis = " [FF0000]F7[-]:[FFFFFF]only error[-]";
	private string m_tootip_pause_en  = " [FF0000]F8[-]:[00FF00]pause[-]";
	private string m_tootip_pause_dis = " [FF0000]F8[-]:[FFFFFF]pause[-]";
	private string m_tooltip_other = "[FF0000]F5[-]:en/disable [FF0000]F6[-]:clear";
	private List<string> m_log_list = new List<string>(MAX_DISP_LOG);
	private int m_log_id = 0;

	private int m_frame_count = 0;
	private float m_passed_time = 0;

    public bool enable
    {
        get
        {
            return g_enable_log;
        }
        set
        {
            g_enable_log = value;
            m_go_window.SetActive(g_enable_log);
            transform.localPosition = Vector3.zero;
            DisplayLog();
        }
    }

	void Awake() {
        Instance = this;
		g_enable_log = false;
		g_only_error = false;
		g_pause_log = false;
        Application.RegisterLogCallback(HandleLog);
	}

	void Start() {
		uil_title.text = m_tooltip_other + m_tooltip_only_error_dis + m_tootip_pause_dis;
	}

	void Update() {
		m_frame_count++;
		m_passed_time += Time.unscaledDeltaTime;
		if(m_passed_time >= 0.5f) {
			uil_fps.text = "Fps:" + (m_frame_count / m_passed_time).ToString("f1");
			m_frame_count = 0;
			m_passed_time = 0;
		}

		if(Input.GetKeyDown(KeyCode.F5)) {
			g_enable_log = !g_enable_log;
			m_go_window.SetActive(g_enable_log);
			transform.localPosition = Vector3.zero;
			DisplayLog();
		}

		if(g_enable_log == false) {
			return;
		}else if(m_go_window.activeInHierarchy == false) {
			m_go_window.SetActive(g_enable_log);
		}

		if(Input.GetKeyDown(KeyCode.F6)) {
			m_log_list.Clear();
			DisplayLog();
		}

		if(Input.GetKeyDown(KeyCode.F7)) {
			g_only_error = !g_only_error;
			if(g_only_error == true) {
				uil_title.text = uil_title.text.Replace(m_tooltip_only_error_dis, m_tooltip_only_error_en);
			} else {
				uil_title.text = uil_title.text.Replace(m_tooltip_only_error_en, m_tooltip_only_error_dis);
			}
			DisplayLog();
		}

		if(Input.GetKeyDown(KeyCode.F8)) {
			g_pause_log = !g_pause_log;
			if(g_pause_log == true) {
				uil_title.text = uil_title.text.Replace(m_tootip_pause_dis, m_tootip_pause_en);
			} else {
				uil_title.text = uil_title.text.Replace(m_tootip_pause_en, m_tootip_pause_dis);
			}
		}
	}

	void HandleLog(string logString, string stackTrace, LogType type)
	{
		if(!Application.isPlaying) {
			return;
		}

		if(++m_log_id > 9999) {
			m_log_id = 0;
		}
		if(type == LogType.Error || type == LogType.Exception) {
			logString = logString.Insert(0, m_head_error+m_log_id.ToString("d4")+" ");
		} else if(type == LogType.Warning) {
			logString = logString.Insert(0, m_head_warning+m_log_id.ToString("d4")+" ");
		} else {
			logString = logString.Insert(0, m_head_normal+m_log_id.ToString("d4")+" ");
		}
		//Log(logString, stackTrace);
		Log(logString);
	}

	private bool m_log_output = false;
	void Log(params object[] objs)
	{
		string text = "";
		for(int i = 0; i < objs.Length; i++) {
			if(i == 0) {
				text += objs[i].ToString() + System.Environment.NewLine;
			} else {
				text += objs[i].ToString();
			}
		}

		if(m_log_list.Count >= MAX_DISP_LOG) {
			m_log_list.RemoveAt(0);
		}
		m_log_list.Add(text);
		
		if(m_log_output == false && g_enable_log == true && g_pause_log == false) {
			m_log_output = true;
			Invoke("DisplayLog", 0.1f);
		}
	}

	void DisplayLog()
	{
		string text = "";
		foreach(string s in m_log_list) {
			if(g_only_error && s[8] != 'E') {
				continue;
			}
			text += s;
		}
		uil_log.text = text;
		m_log_output = false;
	}
}