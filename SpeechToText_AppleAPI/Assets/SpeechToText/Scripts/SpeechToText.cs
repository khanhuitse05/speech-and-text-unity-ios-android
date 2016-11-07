using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;

public class SpeechToText : MonoBehaviour {

#if UNITY_IPHONE
    static public SpeechToText instance;
    public Action onStart;
    public Action onStop;
    public Action<string> onResult;

    [DllImport("__Internal")]
    private static extern void _TAG_startRecording();

    [DllImport("__Internal")]
    private static extern void _TAG_stopRecording();

    [DllImport("__Internal")]
    private static extern void _TAG_InitSpeed();

    public void InitSpeed()
    {
        _TAG_InitSpeed();
    }
    public void startRecording()
    {
        _TAG_startRecording();
    }
    public void stopRecording()
    {
        _TAG_stopRecording();
    }

    void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// Init one time at start
    /// </summary>
    void Start()
    {
        InitSpeed();
    }
    /// <summary>
    /// Finish stop recording and show result
    /// </summary>
    /// <param name="_message"></param>
    void CallbackSpeechToText(string _message)
    {
        if (onResult != null)
        {
            onResult(_message);
        }
        else
        {
            onResult("Null");
        }
    }
    /// <summary>
    /// Call back status, error
    /// </summary>
    /// <param name="_message"></param>
    void CallbackStatus(string _message)
    {
        Debug.Log(_message);
    }
    /// <summary>
    /// Start recording
    /// </summary>
    /// <param name="_message"></param>
    void CallStart(string _message)
    {
        onStart();
    }
    /// <summary>
    /// when call stop recording funtion
    /// </summary>
    /// <param name="_message"></param>
    void CallStop(string _message)
    {
        onStop();
    }
#endif
}
