using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;

public class TextToSpeech : MonoBehaviour
{

    public Action<string> onMessage;

    [Range(0.5f, 2)]
    public float pitch = 1f; //[0.5 - 2] Default 1
    [Range(0, 1)]
    public float rate = 1f; //[min - max] android:[0.5 - 2] iOS:[0 - 1]
    public void init()
    {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_InitSpeak();
#elif UNITY_ANDROID
        AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
        javaUnityClass.CallStatic("InitTextToSpeed");
#endif
    }
    public void SettingSpeak(string language, float _pitch, float _rate)
    {
        pitch = _pitch;
        rate = _rate;
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_SettingSpeak(language, pitch, rate);
#elif UNITY_ANDROID
        AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
        javaUnityClass.CallStatic("SettingTextToSpeed", language, pitch, rate + 0.5f);
#endif
    }
    public void StartSpeak(string _message)
    {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_StartSpeak(_message);
#elif UNITY_ANDROID
        AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
        javaUnityClass.CallStatic("OpenTextToSpeed", _message);
#endif
    }
    public void StopSpeak()
    {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_StopSpeak();
#elif UNITY_ANDROID
        AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
        javaUnityClass.CallStatic("StopTextToSpeed");
#endif
    }
    public void CallbackTextToSpeech(string _message)
    {
        if (_message != null)
            onMessage(_message);
        else
            onMessage("");
    }

#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern void _TAG_InitSpeak();

    [DllImport("__Internal")]
    private static extern void _TAG_StartSpeak(string _message);

    [DllImport("__Internal")]
    private static extern void _TAG_SettingSpeak(string _language, float _pitch, float _rate);
    
    [DllImport("__Internal")]
    private static extern void _TAG_StopSpeak();
#endif

}
