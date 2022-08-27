/// <summary>
/// UIManager.cs
/// 
/// ＃역할
/// UIElements를 상속합니다.
/// Result, Score, Played Time 등을 계산하고 그 정보를 다른 매니저들에게 전달합니다.
/// 
/// ＃스크립팅 기술
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("[Manager Scripts]")]
    GameManager  _GameManager;
    UIManager    _UIManager;
    MusicManager _MusicManager;
    PanelManager _PanelManager;


    [Header("[Prefab Scripts]")]
    UIElements _UIElements;
    PanelCtrl  _PanelCtrl;


    [Header("[UI]")]
    public GameObject ui_Music_Select;
    public GameObject ui_Music_Start;
    public GameObject ui_Music_Paused;

    public GameObject originalScrollView;
    public GameObject customScrollView;

    public GameObject contentOriginal;
    public GameObject contentCustom;

    public GameObject originalMusicElement; // Original 버튼 프리팹
    public GameObject customMusicElement;   // Custom 버튼 프리팹


    private void LateUpdate()
    {
        // ＃UI : 노래 경과 시간
    }


    // 프리팹 하위 인스턴스 조회
    //customMusicElements.transform.GetChild(0).gameObject.GetComponent<Image>();
    //customMusicElements.transform.GetChild(1).gameObject.GetComponent<Text>();
    //customMusicElements.transform.GetChild(2).gameObject.GetComponent<Text>();
    //customMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>();


    object[] originalMusics;
    public IEnumerator Original_List_Renewal()
    {
        _GameManager.isCustom = false;
        _GameManager.isOriginal = true;

        originalMusics = Resources.LoadAll<AudioClip>("Original Music"); // Custom Music 폴더의 AudioClip 속성 파일 조회

        for (int i = 0; i < originalMusics.Length; i++)
        {
            GameObject originalMusicElements = originalMusics[i] as GameObject; // AudioClip to GameObject

            originalMusicElements = Instantiate(originalMusicElement);
            originalMusicElements.name = "Original Music Element_" + i;
            originalMusicElements.transform.parent = contentOriginal.transform;
            originalMusicElements.transform.localScale = Vector3.one;
            originalMusicElements.transform.position = contentOriginal.transform.position;

            // 프로젝트 폴더의 mp3 -> 오디오 소스의 클립에 저장
            originalMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip = (AudioClip)originalMusics[i];
            // 오디오 소스의 텍스트를 타이틀 텍스트에 저장
            originalMusicElements.transform.GetChild(1).gameObject.GetComponent<Text>().text = originalMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name;
        }

        foreach (var t in originalMusics)
        {
            Debug.Log(t);
        }
        yield return new WaitForSeconds(1);
        StopCoroutine(Original_List_Renewal());
    }


    object[] customMusics;
    public IEnumerator Custom_List_Renewal()
    {
        _GameManager.isCustom = true;
        _GameManager.isOriginal = false;

        customMusics = Resources.LoadAll<AudioClip>("Custom Music"); // Custom Music 폴더의 AudioClip 속성 파일 조회

        for (int i = 0; i < customMusics.Length; i++)
        {
            GameObject customMusicElements = customMusics[i] as GameObject; // AudioClip to GameObject

            customMusicElements = Instantiate(customMusicElement);
            customMusicElements.name = "Custom Music Element_" + i;
            customMusicElements.transform.parent = contentCustom.transform;
            customMusicElements.transform.localScale = Vector3.one;
            customMusicElements.transform.position = contentCustom.transform.position;

            // 프로젝트 폴더의 mp3 -> 오디오 소스의 클립에 저장
            customMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip = (AudioClip)customMusics[i];
            // 오디오 소스의 텍스트를 타이틀 텍스트에 저장
            customMusicElements.transform.GetChild(1).gameObject.GetComponent<Text>().text = customMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name;
        }

        foreach (var t in customMusics)
        {
            Debug.Log(t);
        }
        yield return new WaitForSeconds(1);
        StopCoroutine(Custom_List_Renewal());
    }
}
