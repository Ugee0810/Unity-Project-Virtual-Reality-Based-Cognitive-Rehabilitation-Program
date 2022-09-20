/// <summary>
/// PanelManager.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// </summary>

using UnityEngine;
using Random = UnityEngine.Random;

public class PanelManager : MonoBehaviour
{
    [Header("[패널 상호작용 요소]")]
    public Transform  panelSpawnPoint; // 패널 생성 좌표
    public GameObject panelCheck;

    [Header("[패널 프리팹]")]
    public GameObject[] quiz;         // 패널 프리팹 배열
    public GameObject[] block;        // 패널 프리팹 배열
    public GameObject[] motion;       // 패널 프리팹 배열

    public static PanelManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        panelSpawnCount = -1;
        panelLastIndex  = -1;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isStart && GameManager.instance.musicPlayed.isPlaying)
        {
            PanelCheck();

            GameManager.instance.offsetTimer += Time.deltaTime;
            if (GameManager.instance.playTimeOffset >= GameManager.instance.offsetTimer)
            {
                PanelInstance();
            }
        }
    }

    public int panelSpawnCount;
    public int panelLastIndex;
    public bool isQuiz;
    public void PanelInstance()
    {
        GameManager.instance.timer += Time.deltaTime;
        if (GameManager.instance.timer > GameManager.instance.secPerBeat)
        {
            GameManager.instance.timer -= GameManager.instance.secPerBeat;

            int panelIndex = Random.Range(0, 10); // <--- 전체 패널 확률
            int quizCool = Random.Range(5, 20); // <--- 퀴즈 쿨타임

            /* QUIZ 10% */
            if (panelIndex == 0)
            {
                if (!isQuiz)
                {
                    Debug.Log("퀴즈 패널 패턴이 아니므로 모션 패널 생성");
                    GameObject _motion = Instantiate(motion[Random.Range(0, 1)], panelSpawnPoint);
                    _motion.name = "MOTION";

                    panelSpawnCount++;
                    panelLastIndex++;

                    if (panelSpawnCount == quizCool)
                    {
                        _motion.transform.GetChild(4).gameObject.SetActive(true);
                        panelSpawnCount -= quizCool;
                    }
                }
                else if (isQuiz)
                {
                    Debug.Log("퀴즈 패널 생성");
                    GameObject _quiz = Instantiate(quiz[0], panelSpawnPoint);
                    _quiz.name = "QUIZ";

                    panelSpawnCount++;
                    panelLastIndex++;

                    isQuiz = false;
                }
            }

            /* BLOCK 10% */
            else if (panelIndex == 1)
            {
                GameObject _block = Instantiate(block[Random.Range(0, 3)], panelSpawnPoint);
                _block.name = "BLOCK";

                panelSpawnCount++;
                panelLastIndex++;
            }

            /* MOTION 80% */
            else if (panelIndex > 1)
            {
                GameObject _motion = Instantiate(motion[Random.Range(0, 1)], panelSpawnPoint);
                _motion.name = "MOTION";

                panelSpawnCount++;
                panelLastIndex++;

                if (panelSpawnCount == quizCool) // 2회차 퀴즈 나오는지 내일(21일) 체크하기
                {
                    _motion.transform.GetChild(4).gameObject.SetActive(true);
                    panelSpawnCount -= quizCool;
                }
            }
        }
    }

    /*
    2022-09-20 21:35 'Canvas Quiz' 게임 오브젝트를 Enable 하는 타이밍
    모션 패널 다음에 나올 패널이 퀴즈 패널임을 예측하는 법

    퀴즈 UI 활성화 코드
    _motion.transform.GetChild(4).gameObject.SetActive(true);
    */

    //public void PanelInstance()
    //{
    //    GameManager.instance.timer += Time.deltaTime;
    //    int _PanelIndex = Random.Range(0, 10);
    //    if (GameManager.instance.timer > GameManager.instance.secPerBeat)
    //    {
    //        /* QUIZ 10% */ if (_PanelIndex == 0)
    //        {
    //            if (!GameManager.instance.isSafeQuiz)
    //            {
    //                Debug.Log("최초에 퀴즈 패널이 안 나오도록 구현");
    //                GameObject _motion = Instantiate(motion[Random.Range(0, 1)], panelSpawnPoint);
    //                _motion.name = "MOTION";
    //                GameManager.instance.panelLastIndex++;
    //                return;
    //            }
    //            if (GameManager.instance.isSafeQuiz)
    //            {
    //                if (panelSpawnPoint.transform.GetChild(GameManager.instance.panelLastIndex).transform.name == "MOTION")
    //                {
    //                    Debug.Log("퀴즈 패널 생성");
    //                    GameObject _quiz = Instantiate(quiz[0], panelSpawnPoint);
    //                    _quiz.name = "QUIZ";
    //                    GameManager.instance.panelLastIndex++;
    //                }
    //                else if (panelSpawnPoint.transform.GetChild(GameManager.instance.panelLastIndex).transform.name == "BLOCK")
    //                {
    //                    Debug.Log("블럭 패널이 나와서 퀴즈 패널 대신 모션 패널 생성");
    //                    GameObject _motion = Instantiate(motion[Random.Range(0, 1)], panelSpawnPoint);
    //                    _motion.name = "MOTION";
    //                    GameManager.instance.panelLastIndex++;
    //                }
    //                else if (panelSpawnPoint.transform.GetChild(GameManager.instance.panelLastIndex).transform.name == "QUIZ")
    //                {
    //                    Debug.Log("퀴즈 패널이 나와서 퀴즈 패널 대신 모션 패널 생성");
    //                    GameObject _motion = Instantiate(motion[Random.Range(0, 1)], panelSpawnPoint);
    //                    _motion.name = "MOTION";
    //                    GameManager.instance.panelLastIndex++;
    //                }
    //            }
    //        }
    //        /* BLOCK 10% */ else if (_PanelIndex == 1)
    //        {
    //            GameObject _block = Instantiate(block[Random.Range(0, 3)], panelSpawnPoint);
    //            _block.name = "BLOCK";
    //            GameManager.instance.panelLastIndex++;
    //            GameManager.instance.isSafeQuiz = true;
    //        }
    //        /* MOTION 80% */ else if (_PanelIndex > 1)
    //        {
    //            GameObject _motion = Instantiate(motion[Random.Range(0, 1)], panelSpawnPoint);
    //            _motion.name = "MOTION";
    //            GameManager.instance.panelLastIndex++;
    //            GameManager.instance.isSafeQuiz = true;
    //        }
    //        GameManager.instance.timer -= GameManager.instance.secPerBeat;
    //    }
    //}

    void PanelCheck()
    {
        if (GameManager.instance.isSensorLeft && GameManager.instance.isSensorRight)
        {
            panelCheck.SetActive(true);
            if (panelCheck.activeSelf) StartCoroutine(ScoreManager.instance.Increase());
        }
        else if (!GameManager.instance.isSensorLeft || !GameManager.instance.isSensorRight)
        {
            panelCheck.SetActive(false);
        }
    }
}