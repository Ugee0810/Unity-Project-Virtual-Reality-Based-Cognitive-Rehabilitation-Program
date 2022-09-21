/// <summary>
/// QuizPanelA.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// 컬러는 둘 다 같아도 됨
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizPanelA : MonoBehaviour
{
    public Text leftLetter;
    public Text rightLetter;

    public GameObject btnLeft;
    public GameObject btnRight;

    static Color color;

    static string r = "#FF0000";
    static string o = "#FF6400";
    static string y = "#FFFF00";
    static string g = "#00FF00";
    static string s = "#0096FF";
    static string b = "#0014FF";
    static string p = "#9600FF";

    private void OnEnable()
    {
        // (0 == Currect Letter is Left | 1 == Currect Letter is Right)
        int randomDir = Random.Range(0, 2);
        switch (randomDir)
        {
            case 0: // Currect Letter is Left
                PanelManager.instance.isCurLeft = true;
                Debug.Log("isCurLeft : " + PanelManager.instance.isCurLeft);

                leftLetter.text = PanelManager.instance.curLetter;
                switch (PanelManager.instance.curColor)
                {
                    case "Red(Clone)":
                        if (ColorUtility.TryParseHtmlString(r, out color))
                            leftLetter.color = color;
                        break;
                    case "Orange(Clone)":
                        if (ColorUtility.TryParseHtmlString(o, out color))
                            leftLetter.color = color;
                        break;
                    case "Yellow(Clone)":
                        if (ColorUtility.TryParseHtmlString(y, out color))
                            leftLetter.color = color;
                        break;
                    case "Green(Clone)":
                        if (ColorUtility.TryParseHtmlString(g, out color))
                            leftLetter.color = color;
                        break;
                    case "Sky Blue(Clone)":
                        if (ColorUtility.TryParseHtmlString(s, out color))
                            leftLetter.color = color;
                        break;
                    case "Blue(Clone)":
                        if (ColorUtility.TryParseHtmlString(b, out color))
                            leftLetter.color = color;
                        break;
                    case "Purple(Clone)":
                        if (ColorUtility.TryParseHtmlString(p, out color))
                            leftLetter.color = color;
                        break;
                }

                rightLetter.text = PanelManager.instance._LetterList[Random.Range(0, 49)];
                int rightColorindex = Random.Range(0, 7);
                switch (rightColorindex)
                {
                    case 0:
                        if (ColorUtility.TryParseHtmlString(r, out color))
                            rightLetter.color = color;
                        break;
                    case 1:
                        if (ColorUtility.TryParseHtmlString(o, out color))
                            rightLetter.color = color;
                        break;
                    case 2:
                        if (ColorUtility.TryParseHtmlString(y, out color))
                            rightLetter.color = color;
                        break;
                    case 3:
                        if (ColorUtility.TryParseHtmlString(g, out color))
                            rightLetter.color = color;
                        break;
                    case 4:
                        if (ColorUtility.TryParseHtmlString(s, out color))
                            rightLetter.color = color;
                        break;
                    case 5:
                        if (ColorUtility.TryParseHtmlString(b, out color))
                            rightLetter.color = color;
                        break;
                    case 6:
                        if (ColorUtility.TryParseHtmlString(p, out color))
                            rightLetter.color = color;
                        break;
                }
                if (leftLetter.text == rightLetter.text) rightLetter.text = PanelManager.instance._LetterList[Random.Range(0, 49)];
                break;


            case 1: // Currect Letter is Right
                PanelManager.instance.isCurRight = true;
                Debug.Log("isCurRight : " + PanelManager.instance.isCurRight);

                leftLetter.text = PanelManager.instance._LetterList[Random.Range(0, 49)];
                int leftColorindex = Random.Range(0, 7);
                switch (leftColorindex)
                {
                    case 0:
                        if (ColorUtility.TryParseHtmlString(r, out color))
                            leftLetter.color = color;
                        break;
                    case 1:
                        if (ColorUtility.TryParseHtmlString(o, out color))
                            leftLetter.color = color;
                        break;
                    case 2:
                        if (ColorUtility.TryParseHtmlString(y, out color))
                            leftLetter.color = color;
                        break;
                    case 3:
                        if (ColorUtility.TryParseHtmlString(g, out color))
                            leftLetter.color = color;
                        break;
                    case 4:
                        if (ColorUtility.TryParseHtmlString(s, out color))
                            leftLetter.color = color;
                        break;
                    case 5:
                        if (ColorUtility.TryParseHtmlString(b, out color))
                            leftLetter.color = color;
                        break;
                    case 6:
                        if (ColorUtility.TryParseHtmlString(p, out color))
                            leftLetter.color = color;
                        break;
                }

                rightLetter.text = PanelManager.instance.curLetter;
                switch (PanelManager.instance.curColor)
                {
                    case "Red(Clone)":
                        if (ColorUtility.TryParseHtmlString(r, out color))
                            rightLetter.color = color;
                        break;
                    case "Orange(Clone)":
                        if (ColorUtility.TryParseHtmlString(o, out color))
                            rightLetter.color = color;
                        break;
                    case "Yellow(Clone)":
                        if (ColorUtility.TryParseHtmlString(y, out color))
                            rightLetter.color = color;
                        break;
                    case "Green(Clone)":
                        if (ColorUtility.TryParseHtmlString(g, out color))
                            rightLetter.color = color;
                        break;
                    case "Sky Blue(Clone)":
                        if (ColorUtility.TryParseHtmlString(s, out color))
                            rightLetter.color = color;
                        break;
                    case "Blue(Clone)":
                        if (ColorUtility.TryParseHtmlString(b, out color))
                            rightLetter.color = color;
                        break;
                    case "Purple(Clone)":
                        if (ColorUtility.TryParseHtmlString(p, out color))
                            rightLetter.color = color;
                        break;
                }
                if (leftLetter.text == rightLetter.text) leftLetter.text = PanelManager.instance._LetterList[Random.Range(0, 49)];
                break;
        }
    }
}