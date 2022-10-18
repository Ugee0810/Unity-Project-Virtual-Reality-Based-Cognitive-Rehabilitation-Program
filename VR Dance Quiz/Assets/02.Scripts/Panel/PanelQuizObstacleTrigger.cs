/// <summary>
/// PanelQuizObstacleTrigger.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// - 핸드 컨트롤러 오브젝트에 적용된 스크립트
/// - 퀴즈의 정답 방향에 따라 정답/오답 처리(SFX, +-Score, Combo)
/// - 장애물 블럭과 트리거 유지될 동안 점수가 하락 / 콤보 제거
/// </summary>

using UnityEngine;

public class PanelQuizObstacleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider c)
    {
        if (GameManager.instance.isStart)
        {
            if (PanelManager.instance.isCurLeft)
            {
                if (c.gameObject.tag == "QUIZ LEFT")
                {
                    // SFX(Currect)
                    GameManager.instance.sFX[1]?.Invoke();
                    ScoreManaged.SetScore(GameManager.instance.score += 10000);
                    ComboManager.instance.IncreaseCombo();
                    PanelManager.instance.isCurLeft = false;
                    Destroy(c.gameObject.transform.parent.gameObject);
                    if (TutorialManager.instance.isTutorial)
                        TutorialManager.instance.tutoPanelDestroyCount++;
                }
                else if (c.gameObject.tag == "QUIZ RIGHT")
                {
                    // SFX(Fail)
                    GameManager.instance.sFX[2]?.Invoke();
                    if (GameManager.instance.score > 0)
                        ScoreManaged.SetScore(GameManager.instance.score -= 10000);
                    PanelManager.instance.isCurLeft = false;
                    ComboManager.instance.Clear();
                }
            }
            if (PanelManager.instance.isCurRight)
            {
                if (c.gameObject.tag == "QUIZ LEFT")
                {
                    // SFX(Fail)
                    GameManager.instance.sFX[2]?.Invoke();
                    if (GameManager.instance.score > 0)
                        ScoreManaged.SetScore(GameManager.instance.score -= 10000);
                    PanelManager.instance.isCurRight = false;
                    ComboManager.instance.Clear();
                }
                else if (c.gameObject.tag == "QUIZ RIGHT")
                {
                    // SFX(Currect)
                    GameManager.instance.sFX[1]?.Invoke();
                    ScoreManaged.SetScore(GameManager.instance.score += 10000);
                    ComboManager.instance.IncreaseCombo();
                    PanelManager.instance.isCurRight = false;
                    Destroy(c.gameObject.transform.parent.gameObject);
                    if (TutorialManager.instance.isTutorial)
                        TutorialManager.instance.tutoPanelDestroyCount++;
                }
            }
        }
    }

    private void OnTriggerStay(Collider c)
    {
        if (GameManager.instance.isStart)
        {
            if (c.gameObject.tag == "BLOCK")
            {
                if (GameManager.instance.score > 0)
                    ScoreManaged.SetScore(GameManager.instance.score -= 100);
                ComboManager.instance.Clear();
            }
        }
    }
}