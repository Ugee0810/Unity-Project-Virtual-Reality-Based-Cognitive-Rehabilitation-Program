/// <summary>
/// MusicElements.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// - 동적 프리팹 접근을 위해 자신(버튼)에게 Select()를 OnClick()으로 매핑합니다.
/// - EventSystem.current.currentSelectedGameObject을 이용하여 방금 클릭한 게임 오브젝트를 지정해줍니다.
/// - UniBpmAnalyzer.cs의 AnalyzeBpm() 함수로 선택한 오브젝트의 자식 오디오 소스의 BPM을 조사한 수치를 GameManager의 bpm에 선언합니다.
/// - 똑같은 방법으로 전체 길이, 절반 길이 등 필요한 값들을 받아옵니다.
/// - 소스 네임과 클립 등을 대칭화 해주고 선택된 곡의 샘플링 오디오를 재생합니다.
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MusicElements : MonoBehaviour
{
    public void Select()
    {
        GameManager.instance.btnEasy.interactable   = true;
        GameManager.instance.btnNormal.interactable = true;
        GameManager.instance.btnHard.interactable   = true;
        GameManager.instance.btnPlay.interactable   = false;

        // 방금 클릭한 게임 오브젝트를 가져 와 selectedElement에 저장
        GameObject selectedElement = EventSystem.current.currentSelectedGameObject;

        // UniBpmAnalyzer.cs의 AnalyzeBpm() 함수로 선택한 오브젝트의 자식 오디오 소스의 BPM을 조사한 수치를 GameManager의 bpm에 선언
        GameManager.instance.bpm = UniBpmAnalyzer.AnalyzeBpm(selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip);

        // 플레이 타임(100%)
        GameManager.instance.playTime = selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.length;
        GameManager.instance.playTimeOffset = GameManager.instance.playTime - 15f;

        // 플레이 타임(50%)
        GameManager.instance.halfPlayTime = GameManager.instance.playTime / 2;
        GameManager.instance.halfHalfPlayTimeOffset = GameManager.instance.playTime / 2 - 15f;

        // textTitle.text ← customMusicElements.AudioSource.text
        GameManager.instance.infoTitle.GetComponent<Text>().text =
            $"- {selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name}";

        // 프리팹 오디오 소스 클립 -> musicSelected의 클립
        GameManager.instance.musicSelected.GetComponent<AudioSource>().clip = 
            selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip;

        // 플레이에 사용 될 오디오 소스 대칭화
        GameManager.instance.musicPlayed.GetComponent<AudioSource>().clip = 
            GameManager.instance.musicSelected.GetComponent<AudioSource>().clip;

        // BGM Pause
        GameManager.instance.musicBackGround.Pause();

        // MusicSelected Play
        GameManager.instance.musicSelected.Play();
    }
}