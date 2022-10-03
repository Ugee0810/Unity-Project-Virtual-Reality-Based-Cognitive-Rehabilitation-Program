/// <summary>
/// PanelCtrl.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// </summary>

using System;
using System.Collections;
using UnityEngine;

public class PanelCtrl : MonoBehaviour
{
    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (!TutorialManager.instance.isTutorial)
        transform.position += transform.forward
                            * (-GameManager.instance.moveSpeed * GameManager.instance.modePanelSpeed)
                            * Time.deltaTime;
        else if (TutorialManager.instance.isTutorial)
            transform.position += transform.forward
                                * -TutorialManager.instance.tutoMoveSpeed
                                * Time.deltaTime;
    }
}