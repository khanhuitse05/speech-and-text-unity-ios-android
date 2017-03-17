using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;

public class SpeechToText : MonoBehaviour
{
    public Action<string> onResult;
    
    public void init()
    {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_InitSpeech();
#elif UNITY_ANDROID
#endif
    }
    public void SettingRecording(string _language)
    {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_SettingSpeech(_language);
#elif UNITY_ANDROID
        AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
        javaUnityClass.CallStatic("SettingSpeechToText", _language);
#endif
    }
    public void StartRecording(string _message = "")
    {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_startRecording();
#elif UNITY_ANDROID
        AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
        javaUnityClass.CallStatic("OpenSpeechToText", _message);
#endif
    }
    public void StopRecording()
    {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_stopRecording();
#elif UNITY_ANDROID
#endif
    }

#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern void _TAG_startRecording();

    [DllImport("__Internal")]
    private static extern void _TAG_stopRecording();

    [DllImport("__Internal")]
    private static extern void _TAG_InitSpeech();

    [DllImport("__Internal")]
    private static extern void _TAG_SettingSpechd(string _language);    
#endif

    public void CallbackSpeechToText(string _message)
    {
        if (_message != null)
            onResult(_message);
        else
            onResult("");
    }
}
