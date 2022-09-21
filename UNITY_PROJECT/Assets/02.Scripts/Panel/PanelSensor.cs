/// <summary>
/// PanelSensor.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSensor : MonoBehaviour
{
    private void OnTriggerEnter(Collider c)
    {
        if (PanelManager.instance.isCurLeft)
        {
            if (c.gameObject.tag == "QUIZ LEFT")
            {
                PanelManager.instance.isCurLeft = false;
                ComboManager.instance.IncreaseCombo();
                PanelManager.instance.panelLastIndex--;
                Destroy(c.gameObject.transform.parent.gameObject);
            }
            else if (c.gameObject.tag == "QUIZ RIGHT")
            {
                if (GameManager.instance.score > 0)
                {
                    GameManager.instance.score -= 10000;
                    ScoreManager.instance.SetScore();
                }
                PanelManager.instance.isCurLeft = false;
                ComboManager.instance.Clear();
            }
        }
        if (PanelManager.instance.isCurRight)
        {
            if (c.gameObject.tag == "QUIZ LEFT")
            {
                if (GameManager.instance.score > 0)
                {
                    GameManager.instance.score -= 10000;
                    ScoreManager.instance.SetScore();
                }
                PanelManager.instance.isCurRight = false;
                ComboManager.instance.Clear();
            }
            else if (c.gameObject.tag == "QUIZ RIGHT")
            {
                PanelManager.instance.isCurRight = false;
                ComboManager.instance.IncreaseCombo();
                PanelManager.instance.panelLastIndex--;
                Destroy(c.gameObject.transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.tag == "BLOCK")
        {
            if (GameManager.instance.score > 0)
            {
                GameManager.instance.score -= 100;
                ScoreManager.instance.SetScore();
            }
            ComboManager.instance.Clear();
        }
    }
}