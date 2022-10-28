/// <summary>
/// PanelDestroyOnHover.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (V-Light Stutio)
/// This software is released under the GPL-2.0 license
/// 
/// - Motion Panel과 Answer Panel의 좌/우 콜라이더에 XRGrabInteractable.cs의 Hover Entered와 Hover Exited에 각 함수를 지정하여 플래그 변수를 조작합니다.
/// </summary>

using UnityEngine;

public class PanelDestroyOnHover : MonoBehaviour
{
    public void OnHoverEntered_LEFT()
    {
        //Debug.Log($"{gameObject.name} - OnHoverEntered LEFT");
        Singleton<GameManager>.Instance.isSensorLeft = true;
    }

    public void OnHoverExited_LEFT()
    {
        //Debug.Log($"{gameObject.name} - OnHoverExited LEFT");
        Singleton<GameManager>.Instance.isSensorLeft = false;
    }

    public void OnHoverEntered_RIGHT()
    {
        //Debug.Log($"{gameObject.name} - OnHoverEntered RIGHT");
        Singleton<GameManager>.Instance.isSensorRight = true;
    }

    public void OnHoverExited_RIGHT()
    {
        //Debug.Log($"{gameObject.name} - OnHoverExited RIGHT");
        Singleton<GameManager>.Instance.isSensorRight = false;
    }
}