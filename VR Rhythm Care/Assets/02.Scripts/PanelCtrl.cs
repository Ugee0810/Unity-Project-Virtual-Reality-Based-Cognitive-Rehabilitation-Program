/// <summary>
/// PanelCtrl.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (V-Light Stutio)
/// This software is released under the GPL-2.0 license
/// 
/// </summary>

using UnityEngine;

public class PanelCtrl : MonoBehaviour
{
    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (!Singleton<TutorialManager>.Instance.isTutorial)
            transform.position += transform.forward
                                * (-Singleton<GameManager>.Instance.moveSpeed * Singleton<GameManager>.Instance.modePanelSpeed)
                                * Time.deltaTime;
        else
            transform.position += transform.forward
                                * -Singleton<TutorialManager>.Instance.tutoMoveSpeed
                                * Time.deltaTime;
    }
}