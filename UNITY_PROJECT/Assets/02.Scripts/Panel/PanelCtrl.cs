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
    float moveSpeed = 2.0f;

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        transform.position += transform.forward * -moveSpeed * Time.deltaTime;
    }
}