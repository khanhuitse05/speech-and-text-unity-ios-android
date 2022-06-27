using UnityEngine;
using UnityEngine.UI;
using TextSpeech;

public class AndroidDebug : MonoBehaviour
{
    public Text txtLog;
    public Text txtNewLog;
    public RectTransform RmsBar;
    void Start()
    {
        SpeechToText.instance.onResultCallback = onResultCallback;
#if UNITY_ANDROID
        SpeechToText.instance.onReadyForSpeechCallback = onReadyForSpeechCallback;
        SpeechToText.instance.onEndOfSpeechCallback = onEndOfSpeechCallback;
        SpeechToText.instance.onRmsChangedCallback = onRmsChangedCallback;
        SpeechToText.instance.onBeginningOfSpeechCallback = onBeginningOfSpeechCallback;
        SpeechToText.instance.onErrorCallback = onErrorCallback;
        SpeechToText.instance.onPartialResultsCallback = onPartialResultsCallback;
#else
        gameObject.SetActive(false);
#endif
    }

    void AddLog(string log)
    {
        txtLog.text += "\n" + log;
        txtNewLog.text = log;
        Debug.Log(log);
    }
    void onResultCallback(string _data)
    {
        AddLog("Result: " + _data);
    }

    void onReadyForSpeechCallback(string _params)
    {
        AddLog("Ready for the user to start speaking");
    }
    void onEndOfSpeechCallback()
    {
        AddLog("User stops speaking");
    }
    void onRmsChangedCallback(float _value)
    {
        float _size = _value * 10;
        RmsBar.sizeDelta = new Vector2(_size, 5);
    }
    void onBeginningOfSpeechCallback()
    {
        AddLog("User has started to speak");
    }
    void onErrorCallback(string _params)
    {
        AddLog("Error: " + _params);
    }
    void onPartialResultsCallback(string _params)
    {
        AddLog("Partial recognition results are available " + _params);
    }
}
