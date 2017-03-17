using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Language
{
    public string name;
    public string code;
    public string example;
}
public class SampleSpeechToText : MonoBehaviour
{
    public SpeechToText speech;
    public TextToSpeech speak;
    public List<Language> listLanguage;
    public GameObject loading;
    public GameObject setting;
    public Text txtResult;
    public Text txtQuestion;
    public Text txtStatus;
    // setting
    public Dropdown dropSpeech;
    public Dropdown dropSpeak;
    public Slider sliderPitch;
    public Slider sliderRate;

    void Start()
    {
        speak.init();
        speech.init();
        List<string> _listLanguage = new List<string>();
        for (int i = 0; i < listLanguage.Count; i++)
        {
            _listLanguage.Add(listLanguage[i].name);
        }
        dropSpeech.AddOptions(_listLanguage);
        dropSpeech.value = 0;
        dropSpeak.AddOptions(_listLanguage);
        dropSpeak.value = 0;
        sliderPitch.value = 1;
        sliderRate.value = 1;
        SaveSetting();

        txtResult.text = "Tap to Speech";
#if UNITY_IPHONE
        txtResult.text = "Tap and Hold to Speech";
#endif
        loading.SetActive(false);
        speech.onResult = OnResultSpeech;
        speak.onMessage = OnMessageTextToSpeak;
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
#endif
#if UNITY_IPHONE
        loading.SetActive(true);
#endif
    }

    void OnResultSpeech(string _data)
    {
#if UNITY_IPHONE
        loading.SetActive(false);
#endif
        txtResult.text = _data;
    }
    public void OnClickSpeak()
    {
        speak.StartSpeak(txtQuestion.text);
    }
    public void  OnClickStopSpeak()
    {
        speak.StopSpeak();
    }
    public void OnMessageTextToSpeak(string _message)
    {
        txtStatus.text = _message;
    }
    public void OnClickSetting()
    {
        setting.SetActive(true);
    }
    public void SaveSetting()
    {
        setting.SetActive(false);
        speak.SettingSpeak(listLanguage[dropSpeak.value].code, sliderPitch.value, sliderRate.value);
        speech.SettingRecording(listLanguage[dropSpeech.value].code);
        txtQuestion.text = listLanguage[dropSpeak.value].example;
    }
}
