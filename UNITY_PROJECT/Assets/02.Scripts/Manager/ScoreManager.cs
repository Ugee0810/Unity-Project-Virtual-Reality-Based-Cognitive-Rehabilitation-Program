using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text textScore;
    public Text textKcal;

    public UnityEvent HoverEvent;

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


    public void IncreaseScore()
    {
        GameManager.instance.score += 1000;
        SetScore();
    }

    public void IncreaseKcal()
    {
        GameManager.instance.kcal += Random.Range(0.1f, 0.2f);
        SetKcal();
    }


    public void SetScore()
    {
        textScore.text = GameManager.instance.score.ToString();
    }

    public void SetKcal()
    {
        textKcal.text  = GameManager.instance.kcal.ToString("F2");
    }
}