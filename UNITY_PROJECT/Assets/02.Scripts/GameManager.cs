using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// GameManager.cs
public class GameManager : MonoBehaviour
{
    [Header("[Scripts]")]
    public MusicElements _MusicElements;

    [Header("[UI]")]
    public GameObject lobby;
    public GameObject inGame;

    public GameObject scrollOriginal;
    public GameObject originalMusicElement; // Original Btn Prefab
    public GameObject scrollOriginalContent;

    public GameObject scrollCustom;
    public GameObject customMusicElement;   // Custom Btn Prefab
    public GameObject scrollCustomContent;

    //[SerializeField] Image musicAlbum;
    //[SerializeField] Text txtTitle;
    //[SerializeField] Text txtPlayTime;
    //[SerializeField] AudioSource selectMusic;

    [Header("[SFX]")]
    public GameObject vizualizationObjects;

    [Header("[Environment]")]
    public GameObject originHome;
    public GameObject panelLane;
    public GameObject panelStartLight;
    public GameObject panelDestroyColl;


    [Header("[Music Start]")]
    public Transform panelSpawnPoint; // 패널 생성 위치
    public float beat;                // BPM
    [SerializeField] float timer;     // BPM Timer

    [Header("[플래그 변수]")]
    [SerializeField] bool isStart = false;       // 인게임 진행 상태
    [SerializeField] bool isStop = false;        // 인게임 도중 일시 정지 상태

    [SerializeField] bool isOriginal;    // Original Btn
    [SerializeField] bool isCustom = false;      // Custom Btn

    [SerializeField] bool isLevelEasy = false;   // Level Easy
    [SerializeField] bool isLevelNormal = false; // Level Normal
    [SerializeField] bool isLevelHard = false;   // Level Hard


    [Header("[패널 프리팹]")]
    public EnumPanel[] Panels;

    private void Awake()
    {
        isOriginal = true;
    }

    private void Update()
    {
        if (isStart)
        {
            if (timer > beat)
            {
                Instantiate(Panels[UnityEngine.Random.Range(0, 16)], panelSpawnPoint);
                timer -= beat;
            }

            timer += Time.deltaTime;
        }
        else if (isStart && isStop)
        {
            // 일시정지 UI 출력
            
            // 패널/시간 정지
        }
        else
        {

            // 타이머, 패널 초기화
        }
        // 경과 시간이 종료되면 결과 출력
    }

    private void LateUpdate()
    {
        // UI : 노래 경과 시간
    }

    public void InGameStart() // Btn Play
    {
        isStart = true;
        lobby.SetActive(false);
        inGame.SetActive(true);
        vizualizationObjects.SetActive(true);
        originHome.SetActive(true);
        panelLane.SetActive(true);
        panelStartLight.SetActive(true);
        panelDestroyColl.SetActive(true);

        // + 콘트롤러 변경
    }

    public void InGameStop() // 일시 정지
    {
        isStop = true;
        // 일시 정지 UI 출력
    }

    object[] originalMusics;
    object[] customMusics;

    public void Original() // Btn Original
    {
        if (!isOriginal && isCustom)
        {
            scrollOriginal.SetActive(true);
            scrollCustom.SetActive(false);
            StartCoroutine(OriginalSearch());
        }
    }

    public void Custom() // Btn Custom
    {
        if (!isCustom && isOriginal)
        {
            scrollOriginal.SetActive(false);
            scrollCustom.SetActive(true);
            StartCoroutine(CustomSearch());
        }
    }

    IEnumerator OriginalSearch()
    {
        if (true)
        {
            Destroy(_MusicElements.gameObject);
        }
        isCustom = false;
        isOriginal = true;

        originalMusics = Resources.LoadAll<AudioClip>("Original Music"); // Custom Music 폴더의 AudioClip 속성 파일 조회

        for (int i = 0; i < originalMusics.Length; i++)
        {
            GameObject originalMusicElements = originalMusics[i] as GameObject; // AudioClip to GameObject

            originalMusicElements = Instantiate(originalMusicElement);
            originalMusicElements.name = "Original Music Element_" + i;
            originalMusicElements.transform.parent     = scrollOriginalContent.transform;
            originalMusicElements.transform.localScale = Vector3.one;
            originalMusicElements.transform.position   = scrollOriginalContent.transform.position;

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
        StopCoroutine(OriginalSearch());
    }

    IEnumerator CustomSearch()
    {
        Destroy(_MusicElements.gameObject);
        isCustom = true;
        isOriginal = false;

        customMusics = Resources.LoadAll<AudioClip>("Custom Music"); // Custom Music 폴더의 AudioClip 속성 파일 조회

        for (int i = 0; i < customMusics.Length; i++)
        {
            GameObject customMusicElements = customMusics[i] as GameObject; // AudioClip to GameObject

            customMusicElements = Instantiate(customMusicElement);
            customMusicElements.name = "Custom Music Element_" + i;
            customMusicElements.transform.parent     = scrollCustomContent.transform;
            customMusicElements.transform.localScale = Vector3.one;
            customMusicElements.transform.position   = scrollCustomContent.transform.position;

            // 프로젝트 폴더의 mp3 -> 오디오 소스의 클립에 저장
            customMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip = (AudioClip)customMusics[i];
            // 오디오 소스의 텍스트를 타이틀 텍스트에 저장
            customMusicElements.transform.GetChild(1).gameObject.GetComponent<Text>().text = customMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name;

            // 프리팹 하위 인스턴스 조회
            //customMusicElements.transform.GetChild(0).gameObject.GetComponent<Image>();
            //customMusicElements.transform.GetChild(1).gameObject.GetComponent<Text>();
            //customMusicElements.transform.GetChild(2).gameObject.GetComponent<Text>();
            //customMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>();
        }

        foreach (var t in customMusics)
        {
            Debug.Log(t);
        }
        yield return new WaitForSeconds(1);
        StopCoroutine(CustomSearch());
    }
}