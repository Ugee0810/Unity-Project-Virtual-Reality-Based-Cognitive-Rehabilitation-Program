/// <summary>
/// QuizPanelA.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (V-Light Stutio)
/// This software is released under the GPL-2.0 license
/// 
/// 컬러는 둘 다 같아도 됨
/// </summary>

using TMPro;
using UnityEngine;

public class QuizPanelA : MonoBehaviour
{
    public TMP_Text leftLetter;
    public TMP_Text rightLetter;

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
                Singleton<PanelManager>.Instance.isCurLeft = true;
                //Debug.Log("isCurLeft : " + Singleton<PanelManager>.Instance.isCurLeft);

                leftLetter.text = Singleton<PanelManager>.Instance.curLetter;
                switch (Singleton<PanelManager>.Instance.curColor)
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

                rightLetter.text = Singleton<PanelManager>.Instance._LetterList[Random.Range(0, 49)];
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
                if (leftLetter.text == rightLetter.text)
                    rightLetter.text = Singleton<PanelManager>.Instance._LetterList[Random.Range(0, 49)];
                break;


            case 1: // Currect Letter is Right
                Singleton<PanelManager>.Instance.isCurRight = true;
                //Debug.Log("isCurRight : " + Singleton<PanelManager>.Instance.isCurRight);

                leftLetter.text = Singleton<PanelManager>.Instance._LetterList[Random.Range(0, 49)];
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

                rightLetter.text = Singleton<PanelManager>.Instance.curLetter;
                switch (Singleton<PanelManager>.Instance.curColor)
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
                if (leftLetter.text == rightLetter.text)
                    leftLetter.text = Singleton<PanelManager>.Instance._LetterList[Random.Range(0, 49)];
                break;
        }
    }
}