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
        if (c.gameObject.tag == "QUIZ LEFT")
        {

        }
        else if (c.gameObject.tag == "QUIZ RIGHT")
        {

        }
    }

    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.tag == "BLOCK")
        {
            if (GameManager.instance.score > 0)
            {
                GameManager.instance.score -= 10;
                ScoreManager.instance.SetScore();
            }
        }
    }
}