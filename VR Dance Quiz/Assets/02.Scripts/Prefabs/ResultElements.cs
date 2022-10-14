/// <summary>
/// ResultElements.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// </summary>

using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultElements : MonoBehaviour
{
    private void Start()
    {
        /*타이틀*/
        gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = GameManager.instance.textTitle.text;
        /*난이도*/
        gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = GameManager.instance.textLevel.text;
        /*스코어*/
        gameObject.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = GameManager.instance.textScore.text;
        /*칼로리*/
        gameObject.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = GameManager.instance.textKcal.text;
    }
}