using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance;

    public GameObject panelCheck;

    [Header("[패널 프리팹]")]
    public Transform panelSpawnPoint; // 패널 생성 좌표
    public GameObject[] quiz;         // 패널 프리팹 배열
    public GameObject[] block;        // 패널 프리팹 배열
    public GameObject[] motion;       // 패널 프리팹 배열

    [Header("[UI Canvas 프리팹]")]
    public GameObject quizImageAnswer;
    public GameObject quizImageQuestion;
    public GameObject quizTextAnswer;
    public GameObject quizTextQuestion;

    [Header("[Music BPM]")]
    public float timer;                 // BPM 계산 타이머
    public float beat;                  // BPM

    private void Awake()
    {
        if (instance == null) instance = this;
        lastIndex = -1;
    }

    private void Update()
    {
        if (GameManager.instance.isStart)
        {
            PanelInstance();
            PanelCheck();
        }
    }

    public int lastIndex;
    public bool safeQuiz = false;
    public void PanelInstance()
    {
        timer += Time.deltaTime;
        
        int _PanelIndex = Random.Range(0, 10);
        if (timer > beat)
        {
            /* QUIZ 10% */ if (_PanelIndex == 0)
            {
                if (!safeQuiz)
                {
                    Debug.Log("최초에 퀴즈 패널이 안 나오도록 구현");
                    GameObject _motion = Instantiate(motion[Random.Range(0, 12)], panelSpawnPoint);
                    _motion.name = "MOTION";
                    lastIndex++;
                    return;
                }

                if (safeQuiz)
                {
                    if (panelSpawnPoint.transform.GetChild(lastIndex).transform.name == "MOTION")
                    {
                        Debug.Log("퀴즈 패널 생성");
                        GameObject _quiz = Instantiate(quiz[0], panelSpawnPoint);
                        _quiz.name = "QUIZ";
                        lastIndex++;
                    }
                    else if (panelSpawnPoint.transform.GetChild(lastIndex).transform.name == "BLOCK")
                    {
                        Debug.Log("블럭 패널이 나와서 퀴즈 패널 대신 모션 패널 생성");
                        GameObject _motion = Instantiate(motion[Random.Range(0, 12)], panelSpawnPoint);
                        _motion.name = "MOTION";
                        lastIndex++;
                    }
                    else if (panelSpawnPoint.transform.GetChild(lastIndex).transform.name == "QUIZ")
                    {
                        Debug.Log("퀴즈 패널이 나와서 퀴즈 패널 대신 모션 패널 생성");
                        GameObject _motion = Instantiate(motion[Random.Range(0, 12)], panelSpawnPoint);
                        _motion.name = "MOTION";
                        lastIndex++;
                    }
                }
            }
            /* BLOCK 10% */ else if (_PanelIndex == 1)
            {
                GameObject _block = Instantiate(block[Random.Range(0, 3)], panelSpawnPoint);
                _block.name = "BLOCK";
                lastIndex++;
                safeQuiz = true;
            }
            /* MOTION 80% */ else if (_PanelIndex > 1)
            {
                GameObject _motion = Instantiate(motion[Random.Range(0, 12)], panelSpawnPoint);
                _motion.name = "MOTION";
                lastIndex++;
                safeQuiz = true;
            }
            timer -= beat;
        }
    }

    void PanelCheck()
    {
        if (GameManager.instance.isSensorLeft && GameManager.instance.isSensorRight) panelCheck.SetActive(true);
        else if (GameManager.instance.isSensorLeft == false || GameManager.instance.isSensorRight == false) panelCheck.SetActive(false);
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