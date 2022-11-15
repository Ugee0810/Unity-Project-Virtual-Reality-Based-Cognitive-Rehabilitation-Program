/// <summary>
/// PanelQuizObstacleTrigger.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (V-Light Stutio)
/// This software is released under the GPL-2.0 license
/// 
/// - 핸드 컨트롤러 오브젝트에 적용된 스크립트
/// - 퀴즈의 정답 방향에 따라 정답/오답 처리(SFX, +-Score, Combo)
/// - 장애물 블럭과 트리거 유지될 동안 점수가 하락 / 콤보 제거
/// </summary>

using UnityEngine;

public class PanelQuizObstacleTrigger : MonoBehaviour
{
    void Currect(Collider c)
    {
        // SFX(Currect)
        Singleton<GameManager>.Instance.sFX[1].Play();
        ScoreManaged.SetScore(Singleton<GameManager>.Instance.score += 10000);
        Singleton<ComboManager>.Instance.IncreaseCombo();
        Singleton<PanelManager>.Instance.isCurLeft = false;
        Destroy(c.gameObject.transform.parent.gameObject);
        if (Singleton<TutorialManager>.Instance.isTutorial)
        {
            Singleton<TutorialManager>.Instance.tutoPanelDestroyCount++;
        }
    }

    void Fail()
    {
        // SFX(Fail)
        Singleton<GameManager>.Instance.sFX[2].Play();
        if (Singleton<GameManager>.Instance.score > 0)
        {
            ScoreManaged.SetScore(Singleton<GameManager>.Instance.score -= 10000);
            if (Singleton<GameManager>.Instance.score < 0)
            {
                ScoreManaged.SetScore(Singleton<GameManager>.Instance.score = 0);
            }
        }
        Singleton<PanelManager>.Instance.isCurLeft = false;
        Singleton<ComboManager>.Instance.Clear();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (Singleton<GameManager>.Instance.isStart)
        {
            if (Singleton<PanelManager>.Instance.isCurLeft)
            {
                if (c.gameObject.tag == "QUIZ LEFT")  Currect(c);
                if (c.gameObject.tag == "QUIZ RIGHT") Fail();
            }
            if (Singleton<PanelManager>.Instance.isCurRight)
            {
                if (c.gameObject.tag == "QUIZ LEFT")  Fail();
                if (c.gameObject.tag == "QUIZ RIGHT") Currect(c);
            }
        }
    }

    private void OnTriggerStay(Collider c)
    {
        if (Singleton<GameManager>.Instance.isStart)
        {
            if (c.gameObject.tag == "BLOCK")
            {
                if (Singleton<GameManager>.Instance.score > 0)
                {
                    ScoreManaged.SetScore(Singleton<GameManager>.Instance.score -= 100);
                }
                Singleton<ComboManager>.Instance.Clear();
            }
        }
    }
}