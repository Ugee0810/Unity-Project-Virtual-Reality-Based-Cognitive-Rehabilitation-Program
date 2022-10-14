using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayHeadControls : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.playedMusicSlide.minValue = 0;
        GameManager.instance.playedMusicSlide.value = 0;
    }

    void FixedUpdate()
    {
        if (GameManager.instance.isStart)
        {
            if (!GameManager.instance.btnHalf.interactable)
            {
                GameManager.instance.playedMusicSlide.maxValue = (GameManager.instance.musicPlayed.clip.length * GameManager.instance.musicPlayed.clip.frequency * GameManager.instance.musicPlayed.clip.channels / 2) - 1;
                Event();
            }
            else if (!GameManager.instance.btnAll.interactable)
            {
                GameManager.instance.playedMusicSlide.maxValue = (GameManager.instance.musicPlayed.clip.length * GameManager.instance.musicPlayed.clip.frequency * GameManager.instance.musicPlayed.clip.channels) - 1;
                Event();
            }
        }
    }

    void Event()
    {
        GameManager.instance.playedMusicSlide.value = GameManager.instance.musicPlayed.time * GameManager.instance.musicPlayed.clip.frequency * GameManager.instance.musicPlayed.clip.channels;
    }

    public void Scrub()
    {
        GameManager.instance.musicPlayed.time = GameManager.instance.playedMusicSlide.value / (GameManager.instance.musicPlayed.clip.frequency * GameManager.instance.musicPlayed.clip.channels);
    }
}