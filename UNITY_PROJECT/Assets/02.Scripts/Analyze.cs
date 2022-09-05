using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analyze : MonoBehaviour
{
    public float RmsValue;
    public float DbValue;
    public float PitchValue;
    public float Rms;
    public float freqN;

    const int   QSamples  = 1024;
    const float RefValue  = 0.1f;
    const float Threshold = 0.02f;

    float[] _samples;
    float[] _spectrum;
    float   _fSample;

    void Start()
    {
        _samples = new float[QSamples];
        _spectrum = new float[QSamples];
        _fSample = AudioSettings.outputSampleRate;
    }

    void Update()
    {
        AnalyzeSound();
    }

    void AnalyzeSound()
    {
        GetComponent<AudioSource>().GetOutputData(_samples, 1); // 샘플로 배열 생성
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i]; // 샘플 제곱의 합
        }
        RmsValue = Mathf.Sqrt(sum / QSamples); // rms = 평균의 제곱근


        DbValue = 20 * Mathf.Log10(RmsValue / RefValue); //  dB 계산
        if (DbValue < -160) DbValue = -160; //  최소 -160dB로 고정
                                            // 사운드 스펙트럼 감지

        GetComponent<AudioSource>().GetSpectrumData(_spectrum, 1, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;
        for (i = 0; i < QSamples; i++)
        { // max값 탐색
            if (!(_spectrum[i] > maxV) || !(_spectrum[i] > Threshold))
                continue;

            maxV = _spectrum[i];
            maxN = i; // maxN는  최대값의 지수
        }
        float freqN = maxN; // 지수를 부동변수로
        if (maxN > 0 && maxN < QSamples - 1)
        { // 이웃샘플을 이용하여 지수를 보간
            var dL = _spectrum[maxN - 1] / _spectrum[maxN];
            var dR = _spectrum[maxN + 1] / _spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        PitchValue = freqN * (_fSample / 2) / QSamples / 3; // 지수를 주파수로 변환


        if (maxN > 0 && maxN < QSamples - 1)
        { // 이웃샘플을 이용하여 박자확인

            if (_spectrum[maxN] < _spectrum[maxN - 1] * 1.12)
            {
                Rms = 1;
                Debug.Log(Rms);
            }
            else
                Rms = 0;
            return;
        }
    }
}