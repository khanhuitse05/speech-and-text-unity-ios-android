using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TextSpeech;

public class SampleSpeechToText : MonoBehaviour
{
    public GameObject loading;
    public Text txtLog;
    public InputField inputLocale;
    public InputField inputText;
    public float pitch;
    public float rate;

    public Text txtLocale;
    public Text txtPitch;
    public Text txtRate;
    void Start()
    {
        Setting("en-US");
        loading.SetActive(false);
        SpeechToText.instance.onResultCallback = OnResultSpeech;
        AddLog("Unity Init Finish");
    }
    void AddLog(string log)
    {
        txtLog.text += "\n" + log;
        Debug.Log(log);
    }
    public void StartRecording()
    {
#if UNITY_EDITOR
#else
        speech.StartRecording("Speak any");
#endif
    }

    public void StopRecording()
    {
#if UNITY_EDITOR
        OnResultSpeech("Not support in editor.");
#else
        speech.StopRecording();
        loading.SetActive(true);
#endif
    }
    void OnResultSpeech(string _data)
    {
        loading.SetActive(false);
        inputText.text = _data;
    }
    public void OnClickSpeak()
    {
        TextToSpeech.instance.StartSpeak(inputText.text);
    }
    public void  OnClickStopSpeak()
    {
        TextToSpeech.instance.StopSpeak();
    }
    public void Setting(string code)
    {
        TextToSpeech.instance.Setting(code, pitch, rate);
        SpeechToText.instance.Setting(code);
        txtLocale.text = "Locale: " + code;
        txtPitch.text = "Pitch: " + pitch;
        txtRate.text = "Rate: " + rate;
    }
    public void OnClickApply()
    {
        Setting(inputLocale.text);
    }
}
