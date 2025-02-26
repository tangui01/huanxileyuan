using UnityEngine;
using UnityEngine.UI;
using WGM;

public class SubmarineParkourManage : MonoBehaviour
{
    public Image skill;
    public float cooldown = 20f;
    float timer;
    
    void Start()
    {
        timer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        skill.fillAmount = timer / cooldown;
        if (timer <= 0 && DealCommand.GetKey(1,(AppKeyCode)3))
        {
            SubmarineParkourGUIManager.Instance.ActivateSprint();
        }
    }

    public void UpdateCooldown()
    {
        timer = cooldown;
    }
    private void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
        }
        
    }
}
