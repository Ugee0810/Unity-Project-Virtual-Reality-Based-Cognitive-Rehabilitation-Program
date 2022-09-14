using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    // Song beats per minute
    // This is determined by the song you're trying to sync up to
    // 분당 Beat, 동기화하려는 노래에 따라 결정
    public float songBpm;

    // The number of seconds for each song beat
    // 각 노래 박자에 대한 시간(초)
    public float secPerBeat;

    // Current song position, in seconds
    // 현재 노래 위치(초)
    public float songPosition;

    // Current song position, in beats
    // 현재 곡 위치(박자 단위)
    public float songPositionInBeats;

    // How many seconds have passed since the song started
    // 노래 경과 시간
    public float dspSongTime;

    // an AudioSource attached to this GameObject that will play the music.
    // 음악을 재생할 이 게임 개체에 연결된 오디오 소스
    public AudioSource musicSource;

    // 장면이 시작되면 몇 가지 계산을 수행하여 일부 변수를 결정하고 참조용으로 오디오가 시작된 시간도 기록해야 합니다.
    void Start()
    {
        // Load the AudioSource attached to the Conductor GameObject
        // 지휘자 게임 개체에 연결된 오디오 소스 로드
        musicSource = GetComponent<AudioSource>();

        // Calculate the number of seconds in each beat
        // 각 박자의 초 수를 계산
        secPerBeat = 60f / songBpm;

        // Record the time when the music starts
        // 음악이 시작되는 시간 기록
        dspSongTime = (float)AudioSettings.dspTime;

        // Start the music
        // 음악 시작
        musicSource.Play();
    }


    // 오디오 시스템에 따른 현재 시간과 노래가 재생된 총 시간(초)에 해당하는 노래가 시작된 시간 사이의 차이를 제공하며 이를 변수 songPosition에 저장합니다.
    void Update()
    {
        // determine how many seconds since the song started
        // 노래 시작 후 몇 초 후인지 확인
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);

        // determine how many beats since the song started
        // 노래가 시작된 이후 몇 박자인지 확인
        songPositionInBeats = songPosition / secPerBeat;
    }
}
