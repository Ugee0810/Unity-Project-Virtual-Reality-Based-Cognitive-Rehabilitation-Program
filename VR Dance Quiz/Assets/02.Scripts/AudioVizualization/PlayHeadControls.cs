using UnityEngine;

public class PlayHeadControls : MonoBehaviour
{
    private void Start()
    {
        Singleton<GameManager>.Instance.playedMusicSlide.minValue = 0;
        Singleton<GameManager>.Instance.playedMusicSlide.value = 0;
    }

    void FixedUpdate()
    {
        if (Singleton<GameManager>.Instance.isStart)
        {
            if (!Singleton<GameManager>.Instance.btnModes[3].interactable)
            {
                Singleton<GameManager>.Instance.playedMusicSlide.maxValue = (Singleton<GameManager>.Instance.music[2].clip.length * Singleton<GameManager>.Instance.music[2].clip.frequency * Singleton<GameManager>.Instance.music[2].clip.channels / 2) - 1;
                Event();
            }
            else if (!Singleton<GameManager>.Instance.btnModes[4].interactable)
            {
                Singleton<GameManager>.Instance.playedMusicSlide.maxValue = (Singleton<GameManager>.Instance.music[2].clip.length * Singleton<GameManager>.Instance.music[2].clip.frequency * Singleton<GameManager>.Instance.music[2].clip.channels) - 1;
                Event();
            }
        }
    }

    void Event()
    {
        Singleton<GameManager>.Instance.playedMusicSlide.value = Singleton<GameManager>.Instance.music[2].time * Singleton<GameManager>.Instance.music[2].clip.frequency * Singleton<GameManager>.Instance.music[2].clip.channels;
    }

    public void Scrub()
    {
        Singleton<GameManager>.Instance.music[2].time = Singleton<GameManager>.Instance.playedMusicSlide.value / (Singleton<GameManager>.Instance.music[2].clip.frequency * Singleton<GameManager>.Instance.music[2].clip.channels);
    }
}