/// <summary>
/// ScoreManager.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text textScore;
    public Text textKcal;

    public static ScoreManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (GameManager.instance.isStart)
        {
            SetScore();
            SetKcal();
        }
    }

    IEnumerator Increase()
    {
        GameManager.instance.score += 1000;
        SetScore();

        GameManager.instance.kcal += Random.Range(0.1f, 0.2f);
        SetKcal();

        yield return null;
    }

    //public void IncreaseScore()
    //{
    //    GameManager.instance.score += 1000;
    //    SetScore();
    //}

    //public void IncreaseKcal()
    //{
    //    GameManager.instance.kcal += Random.Range(0.1f, 0.2f);
    //    SetKcal();
    //}


    public void SetScore()
    {
        textScore.text = GameManager.instance.score.ToString();
    }

    public void SetKcal()
    {
        textKcal.text  = GameManager.instance.kcal.ToString("F2");
    }
}