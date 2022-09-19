/// <summary>
/// ResultElements.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultElements : MonoBehaviour
{
    private void Start()
    {
        /*타이틀*/
        gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = GameManager.instance._TextTitle.text;
        /*난이도*/
        gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = GameManager.instance._TextLevel.text;
        /*스코어*/
        gameObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = GameManager.instance._TextScore.text;
        /*칼로리*/
        gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = GameManager.instance._TextKcal.text;
    }
}