/// <summary>
/// ResultElements.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (V-Light Studio)
/// This software is released under the GPL-2.0 license
/// 
/// 동적 프리팹 각 오브젝트의 TMP_Text 컴포넌트에 곡이 끝난 결과를 키 값으로 저장
/// </summary>

using TMPro;
using UnityEngine;

public class ResultElements : MonoBehaviour
{
    private void Start()
    {
        /*Title*/ gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = GameManager.instance.textKeys[0].text;
        /*Level*/ gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = GameManager.instance.textKeys[1].text;
        /*Score*/ gameObject.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = GameManager.instance.textKeys[2].text;
        /* Kcal*/ gameObject.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = GameManager.instance.textKeys[3].text;
    }
}