/// <summary>
/// QuizPanelsQ.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// 'Canvas Quiz' GameObject에 적용되는 스크립트입니다.
/// Easy / Normal : 왼쪽-색 / 오른쪽-글귀
/// Hard : 각각 좌/우 랜덤 위치에서 색과 글귀가 출제
/// 
/// Canvas Quiz A.cs(임시)
/// 컬러는 둘 다 같아도 됨
/// </summary>

using UnityEngine;
using UnityEngine.UI;

public class QuizPanelQ : MonoBehaviour
{
    // Color Balls(size : 7)
    public GameObject[] leftcolorBallArray;
    public GameObject[] rightcolorBallArray;

    // Letters(size : 49)
    public string[] leftLetterArray = 
        { "집중", "평화", "용서", "감사", "침착", "정의", "조화", "자유", "정직", "지혜", 
        "친절", "이해", "활력", "영감", "공감", "겸손", "힘", "지성", "고요", "결단", 
        "사랑", "관용", "자비", "부드러움", "축복", "생명", "믿음", "젊음", "자신감", "덕성", 
        "행복", "영혼", "민첩성", "건강", "충만", "일관성", "끈기", "목적", "성취", "풍요", 
        "성공", "숙달", "능력", "에너치", "소명", "침묵", "직관", "재생", "소생" };
    public string[] rightLetterArray =
        { "집중", "평화", "용서", "감사", "침착", "정의", "조화", "자유", "정직", "지혜",
        "친절", "이해", "활력", "영감", "공감", "겸손", "힘", "지성", "고요", "결단",
        "사랑", "관용", "자비", "부드러움", "축복", "생명", "믿음", "젊음", "자신감", "덕성",
        "행복", "영혼", "민첩성", "건강", "충만", "일관성", "끈기", "목적", "성취", "풍요",
        "성공", "숙달", "능력", "에너치", "소명", "침묵", "직관", "재생", "소생" };

    public Text leftLetter;
    public Text rightLetter;

    private void OnEnable()
    {
        if (GameManager.instance.isStart && GameManager.instance.musicPlayed.isPlaying && (!GameManager.instance.btnEasy.interactable || !GameManager.instance.btnNormal.interactable))
        {
            leftcolorBallArray[Random.Range(0, 7)].SetActive(true);
            rightLetter.text = leftLetterArray[Random.Range(0, 49)];
            PanelManager.instance.isQuiz = true;
        }
        else if (GameManager.instance.isStart && GameManager.instance.musicPlayed.isPlaying && !GameManager.instance.btnHard.interactable)
        {
            // Hard 전용 랜덤 변수 (0 == Color is Left | 1 == Color is Right)
            int randomDir = Random.Range(0, 2);
            switch (randomDir)
            {
                case 0:
                    leftcolorBallArray[Random.Range(0, 7)].SetActive(true);
                    rightLetter.text = leftLetterArray[Random.Range(0, 49)];
                    PanelManager.instance.isQuiz = true;
                    break;
                case 1:
                    rightcolorBallArray[Random.Range(0, 7)].SetActive(true);
                    leftLetter.text = leftLetterArray[Random.Range(0, 49)];
                    PanelManager.instance.isQuiz = true;
                    break;
            }
        }
    }
}