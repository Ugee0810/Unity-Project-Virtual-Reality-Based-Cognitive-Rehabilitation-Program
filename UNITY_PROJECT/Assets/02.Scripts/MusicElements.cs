/// <summary>
/// MusicElements.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicElements : MonoBehaviour
{
    public void Select()
    {
        GameManager.instance.btnEasy.interactable   = true;
        GameManager.instance.btnNormal.interactable = true;
        GameManager.instance.btnHard.interactable   = true;

        // BGM Pause
        GameManager.instance.musicBackGround.Pause();

        // 방금 클릭한 게임 오브젝트를 가져와서 저장
        GameObject selectedElement = EventSystem.current.currentSelectedGameObject;

        GameManager.instance.playTime = selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.length;
        GameManager.instance.bpm = UniBpmAnalyzer.AnalyzeBpm(selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip);
        GameManager.instance.playTimeOffset = GameManager.instance.playTime - 10f;

        // textTitle.text ← customMusicElements.AudioSource.text
        GameManager.instance.infoTitle.GetComponent<Text>().text =
            $"- {selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name}";

        // 프리팹 오디오 소스 클립 -> musicSelected의 클립
        GameManager.instance.musicSelected.GetComponent<AudioSource>().clip = 
            selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip;

        // 플레이에 사용 될 오디오 소스 대칭화
        GameManager.instance.musicPlayed.GetComponent<AudioSource>().clip = 
            GameManager.instance.musicSelected.GetComponent<AudioSource>().clip;

        GameManager.instance.musicSelected.Play();
    }
}