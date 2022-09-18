/// <summary>
/// PanelManager.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PanelManager : MonoBehaviour
{
    [Header("[패널 프리팹]")]
    public Transform panelSpawnPoint; // 패널 생성 좌표
    public GameObject panelCheck;
    public GameObject[] quiz;         // 패널 프리팹 배열
    public GameObject[] block;        // 패널 프리팹 배열
    public GameObject[] motion;       // 패널 프리팹 배열

    [Header("[UI Canvas 프리팹]")]
    public GameObject quizImageAnswer;
    public GameObject quizImageQuestion;
    public GameObject quizTextAnswer;
    public GameObject quizTextQuestion;

    public static PanelManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        GameManager.instance.panelLastIndex = -1;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isStart && GameManager.instance.musicPlayed.isPlaying)
        {
            GameManager.instance.offsetTimer += Time.deltaTime;

            if (GameManager.instance.playTimeOffset >= GameManager.instance.offsetTimer)
            {
                PanelInstance();
                PanelCheck();
            }
        }
    }

    public void PanelInstance()
    {
        GameManager.instance.timer += Time.deltaTime;
        
        int _PanelIndex = Random.Range(0, 10);
        if (GameManager.instance.timer > GameManager.instance.secPerBeat)
        {
            /* QUIZ 10% */ if (_PanelIndex == 0)
            {
                if (!GameManager.instance.isSafeQuiz)
                {
                    Debug.Log("최초에 퀴즈 패널이 안 나오도록 구현");
                    GameObject _motion = Instantiate(motion[Random.Range(0, 12)], panelSpawnPoint);
                    _motion.name = "MOTION";
                    GameManager.instance.panelLastIndex++;
                    return;
                }
                if (GameManager.instance.isSafeQuiz)
                {
                    if (panelSpawnPoint.transform.GetChild(GameManager.instance.panelLastIndex).transform.name == "MOTION")
                    {
                        Debug.Log("퀴즈 패널 생성");
                        GameObject _quiz = Instantiate(quiz[0], panelSpawnPoint);
                        _quiz.name = "QUIZ";
                        GameManager.instance.panelLastIndex++;
                    }
                    else if (panelSpawnPoint.transform.GetChild(GameManager.instance.panelLastIndex).transform.name == "BLOCK")
                    {
                        Debug.Log("블럭 패널이 나와서 퀴즈 패널 대신 모션 패널 생성");
                        GameObject _motion = Instantiate(motion[Random.Range(0, 12)], panelSpawnPoint);
                        _motion.name = "MOTION";
                        GameManager.instance.panelLastIndex++;
                    }
                    else if (panelSpawnPoint.transform.GetChild(GameManager.instance.panelLastIndex).transform.name == "QUIZ")
                    {
                        Debug.Log("퀴즈 패널이 나와서 퀴즈 패널 대신 모션 패널 생성");
                        GameObject _motion = Instantiate(motion[Random.Range(0, 12)], panelSpawnPoint);
                        _motion.name = "MOTION";
                        GameManager.instance.panelLastIndex++;
                    }
                }
            }
            /* BLOCK 10% */ else if (_PanelIndex == 1)
            {
                GameObject _block = Instantiate(block[Random.Range(0, 3)], panelSpawnPoint);
                _block.name = "BLOCK";
                GameManager.instance.panelLastIndex++;
                GameManager.instance.isSafeQuiz = true;
            }
            /* MOTION 80% */ else if (_PanelIndex > 1)
            {
                GameObject _motion = Instantiate(motion[Random.Range(0, 12)], panelSpawnPoint);
                _motion.name = "MOTION";
                GameManager.instance.panelLastIndex++;
                GameManager.instance.isSafeQuiz = true;
            }
            GameManager.instance.timer -= GameManager.instance.secPerBeat;
        }
    }

    void PanelCheck()
    {
        if (GameManager.instance.isSensorLeft && GameManager.instance.isSensorRight)
        {
            panelCheck.SetActive(true);
        }
        else if (!GameManager.instance.isSensorLeft || !GameManager.instance.isSensorRight)
        {
            panelCheck.SetActive(false);
        }
    }

    // 패널 프리팹의 Canvas를 바꿔준다. (텍스트, 이미지)
    void QuizThemeChange() 
    {

    }

    // 텍스트 테마
    void TxtTheme()
    {

    }   

    // 이미지 테마
    void ImageTheme()
    {

    }
}