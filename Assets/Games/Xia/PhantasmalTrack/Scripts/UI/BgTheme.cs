using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhantasmalTrack
{
    public class BgTheme : MonoBehaviour
    {
        private SpriteRenderer m_SpriteRenderer;
        private ManagerVars vars;

        private void Awake()
        {
            vars = FindObjectOfType<ManagerVars>();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            int ranValue = Random.Range(0, vars.bgThemeSpriteList.Count);
            m_SpriteRenderer.sprite = vars.bgThemeSpriteList[ranValue];
        }
    }
}
