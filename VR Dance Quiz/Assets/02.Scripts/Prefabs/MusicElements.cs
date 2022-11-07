/// <summary>
/// MusicElements.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (V-Light Stutio)
/// This software is released under the GPL-2.0 license
/// 
/// - 동적 프리팹 접근을 위해 자신(버튼)에게 Select()를 OnClick()으로 매핑합니다.
/// - EventSystem.current.currentSelectedGameObject을 이용하여 방금 클릭한 게임 오브젝트를 지정해줍니다.
/// - UniBpmAnalyzer.cs의 AnalyzeBpm() 함수로 선택한 오브젝트의 자식 오디오 소스의 BPM을 조사한 수치를 GameManager의 bpm에 선언합니다.
/// - 똑같은 방법으로 전체 길이, 절반 길이 등 필요한 값들을 받아옵니다.
/// - 소스 네임과 클립 등을 대칭화 해주고 선택된 곡의 샘플링 오디오를 재생합니다.
/// </summary>

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MusicElements : MonoBehaviour
{
    public void Select()
    {
        if (Singleton<TutorialManager>.Instance.isTutorial)
            Singleton<TutorialManager>.Instance.TutorialStep();
        else
            for (int i = 0; i < Singleton<GameManager>.Instance.btnLevels.Length; i++)
                Singleton<GameManager>.Instance.btnLevels[i].interactable = true;
        Singleton<GameManager>.Instance.btnPlay.interactable = false;
        // 방금 클릭한 게임 오브젝트를 가져 와 selectedElement에 저장
        GameObject selectedElement = EventSystem.current.currentSelectedGameObject;
        // UniBpmAnalyzer.cs의 AnalyzeBpm() 함수로 선택한 오브젝트의 자식 오디오 소스의 BPM을 조사한 수치를 GameManager의 bpm에 선언
        Singleton<GameManager>.Instance.bpm = UniBpmAnalyzer.AnalyzeBpm(selectedElement.transform.GetChild(3).GetComponent<AudioSource>().clip);
        // 플레이 타임(100%)
        Singleton<GameManager>.Instance.playTime = selectedElement.transform.GetChild(3).GetComponent<AudioSource>().clip.length;
        Singleton<GameManager>.Instance.playTimeOffset = Singleton<GameManager>.Instance.playTime - 15f;
        // 플레이 타임(50%)
        Singleton<GameManager>.Instance.halfPlayTime = Singleton<GameManager>.Instance.playTime / 2;
        Singleton<GameManager>.Instance.halfHalfPlayTimeOffset = Singleton<GameManager>.Instance.playTime / 2 - 15f;
        // Info Title.text ← customMusicElements.AudioSource.text
        Singleton<GameManager>.Instance.infoTitle.GetComponent<TMP_Text>().text = $"※ {selectedElement.transform.GetChild(3).GetComponent<AudioSource>().clip.name}";
        // 프리팹 오디오 소스 클립 -> musicSelected의 클립
        Singleton<GameManager>.Instance.music[1].GetComponent<AudioSource>().clip = selectedElement.transform.GetChild(3).GetComponent<AudioSource>().clip;
        // 플레이에 사용 될 오디오 소스 대칭화
        Singleton<GameManager>.Instance.music[2].GetComponent<AudioSource>().clip = Singleton<GameManager>.Instance.music[1].GetComponent<AudioSource>().clip;
        // BGM Pause
        Singleton<GameManager>.Instance.music[0].Pause();
        // MusicSelected Play
        Singleton<GameManager>.Instance.music[1].Play();
    }
}