/// <summary>
/// PanelEndLineDestroy.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// - 플레이어 뒤로 지나간 노트들을 삭제해줍니다.
/// - GameManager.cs의 SFX[] 유니티 이벤트로 트리거 됐을 때 실패음이 출력됩니다.
/// - ComboManager의 Clear()를 호출하여 콤보를 제거합니다.
/// </summary>

using UnityEngine;

public class PanelEndLineDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "QUIZ")
        {
            // Combo Reset
            ComboManager.instance.Clear();
            // Delete Collision Obj
            Destroy(c.gameObject);
            // Tuto Destory Count++
            if (TutorialManager.instance.isTutorial) TutorialManager.instance.tutoPanelDestroyCount++;
            // SFX(Fail)
            GameManager.instance.sFX[2]?.Invoke();
        }

        if (c.gameObject.tag == "BLOCK")
        {
            // Delete Collision Obj
            Destroy(c.gameObject);
            // Tuto Destory Count++
            if (TutorialManager.instance.isTutorial) TutorialManager.instance.tutoPanelDestroyCount++;
        }

        if (c.gameObject.tag == "MOTION")
        {
            // Combo Reset
            ComboManager.instance.Clear();
            // Delete Collision Obj
            Destroy(c.gameObject);
            // Tuto Destory Count++
            if (TutorialManager.instance.isTutorial) TutorialManager.instance.tutoPanelDestroyCount++;
            // SFX(Fail)
            GameManager.instance.sFX[2]?.Invoke();
        }
    }
}