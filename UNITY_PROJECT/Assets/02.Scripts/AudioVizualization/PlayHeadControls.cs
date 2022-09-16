using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayHeadControls : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider slider;

    private void Awake()
    {
        slider.minValue = 0;
        slider.value = 0;
    }

    void Update()
    {
        if (GameManager.instance.isStart)
        {
            slider.maxValue = (audioSource.clip.length * audioSource.clip.frequency * audioSource.clip.channels) - 1;
            Event();
        }
        else
        {
            slider.minValue = 0;
            slider.value = 0;
        }
    }

    void Event()
    {
        slider.value = audioSource.time * audioSource.clip.frequency * audioSource.clip.channels;
    }

    public void Scrub()
    {
        audioSource.time = slider.value / (audioSource.clip.frequency * audioSource.clip.channels);
    }
}