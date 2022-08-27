/// <summary>
/// UIElements.cs
/// 
/// ＃역할
/// Original과 Custom의 버튼 이벤트에서 로드되는 Elements UI Prefab들의 스크립트입니다.
/// 프리팹 각각의 앨범 이미지, 곡명, 재생 시간 정보를 얻고 UIManager에게 전달합니다.
/// 
/// ＃스크립팅 기술
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElements : UIManager
{
    [SerializeField] GameManager _GameManager;

    // public Image musicAlbum;
    // public Text txtTitle;
    // public Text txtPlayTime;
}
