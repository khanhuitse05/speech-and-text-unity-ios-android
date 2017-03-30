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

        txtResult.text = "Tap and Hold to Speech";

        loading.SetActive(false);
        speech.onResultCallback = OnResultSpeech;
        txtStatus.text = txtStatus.text + " / Unity Init Finish";

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
        loading.SetActive(true);
    }
    void OnResultSpeech(string _data)
    {
        loading.SetActive(false);
        txtResult.text = _data;
    }
    void OnMessageSpeech(string _message)
    {
        txtStatus.text = txtStatus.text + " / " + _message;
    }
    public void OnClickSpeak()
    {
        speak.StartSpeak(txtQuestion.text);
    }
    public void  OnClickStopSpeak()
    {
        speak.StopSpeak();
    }
    public void OnClickSetting()
    {
        setting.SetActive(true);
    }
    public void SaveSetting()
    {
        setting.SetActive(false);
        speak.Setting(listLanguage[dropSpeak.value].code, sliderPitch.value, sliderRate.value);
        speech.Setting(listLanguage[dropSpeech.value].code);
        txtQuestion.text = listLanguage[dropSpeak.value].example;
    }
}
