using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WGM;

public class QrCode : MonoBehaviour
{
    GameObject content;
    bool createPicture;
    public GameObject noQrShowItem;
    // Start is called before the first frame update
    void Start()
    {
        content = transform.Find("Content").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (LibWGM.machine.showQrCode==0)
        {
            if (noQrShowItem)
                noQrShowItem.SetActive(true);
            content.SetActive(false);
            return;
        }

        if ((DealCommand.handles[0]<0&& DealCommand.handles[1]<0)  || LibWGM.machine.Language == 2|| string.IsNullOrEmpty(LibWGM.machine.QrCodeUrl) || UINetCheck.curNetState == NetState.none || UINetCheck. curNetState==NetState.noNet)
        { 
            content.SetActive(false);
            if(noQrShowItem)
            noQrShowItem.SetActive(true);
        }
        else {
            content.SetActive(true);
            if (noQrShowItem)
                noQrShowItem.SetActive(false);
            if (!createPicture)
            {
                createPicture = true;
                if(content.transform.Find("QrCode").GetComponent<UI2DSprite>()!=null)
                content.transform.Find("QrCode").GetComponent<UI2DSprite>().sprite2D = LibMisc.GetQrCode(LibWGM.machine.QrCodeUrl);
                else
                    if (content.transform.Find("QrCode").GetComponent<Image>() != null)
                    content.transform.Find("QrCode").GetComponent<Image>().sprite = LibMisc.GetQrCode(LibWGM.machine.QrCodeUrl);
            }
        }
    }
}
