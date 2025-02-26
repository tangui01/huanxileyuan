using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WGM;

public class Veisions : MonoBehaviour
{
    Text versionText;
    // Start is called before the first frame update
    void Start()
    {
        versionText = transform.GetComponent<Text>();
      versionText.text = (string.IsNullOrEmpty (LibWGM.machine.MachineId) ?"":"M" + LibWGM.machine.MachineId) + " V" + Application.version;
      
    }

    private void Update()
    {
        versionText.text = (string.IsNullOrEmpty(LibWGM.machine.MachineId) ? "" : "M" + LibWGM.machine.MachineId) + " V" + Application.version;
    }
}
