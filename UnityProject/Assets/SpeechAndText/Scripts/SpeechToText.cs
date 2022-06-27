using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;

namespace TextSpeech
{
    public class SpeechToText : MonoBehaviour
    {

        #region Init
        static SpeechToText _instance;
        public static SpeechToText instance
        {
            get
            {
                if (_instance == null)
                {
                    Init();
                }
                return _instance;
            }
        }
        public static void Init()
        {
            if (instance != null) return;
            GameObject obj = new GameObject();
            obj.name = "TextToSpeech";
            _instance = obj.AddComponent<SpeechToText>();
        }
        void Awake()
        {
            _instance = this;
        }
        #endregion

        public Action<string> onResultCallback;

        public void Setting(string _language)
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
        if (isShowPopupAndroid)
        {
            AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
            javaUnityClass.CallStatic("OpenSpeechToText", _message);
        }
        else
        {
            AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
            javaUnityClass.CallStatic("StartRecording");
        }
#endif
        }
        public void StopRecording()
        {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_stopRecording();
#elif UNITY_ANDROID
        if (isShowPopupAndroid == false)
        {
            AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
            javaUnityClass.CallStatic("StopRecording");
        }
#endif
        }

#if UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern void _TAG_startRecording();

        [DllImport("__Internal")]
        private static extern void _TAG_stopRecording();

        [DllImport("__Internal")]
        private static extern void _TAG_SettingSpeech(string _language);
#endif

        public void onMessage(string _message)
        {
        }
        public void onErrorMessage(string _message)
        {
            Debug.Log(_message);
        }
        /** Called when recognition results are ready. */
        public void onResults(string _results)
        {
            if (onResultCallback != null)
                onResultCallback(_results);
        }

        #region Android STT custom
#if UNITY_ANDROID
        #region Error Code
        /** Network operation timed out. */
        public const int ERROR_NETWORK_TIMEOUT = 1;
        /** Other network related errors. */
        public const int ERROR_NETWORK = 2;
        /** Audio recording error. */
        public const int ERROR_AUDIO = 3;
        /** Server sends error status. */
        public const int ERROR_SERVER = 4;
        /** Other client side errors. */
        public const int ERROR_CLIENT = 5;
        /** No speech input */
        public const int ERROR_SPEECH_TIMEOUT = 6;
        /** No recognition result matched. */
        public const int ERROR_NO_MATCH = 7;
        /** RecognitionService busy. */
        public const int ERROR_RECOGNIZER_BUSY = 8;
        /** Insufficient permissions */
        public const int ERROR_INSUFFICIENT_PERMISSIONS = 9;
        /////////////////////
        String getErrorText(int errorCode)
        {
            String message;
            switch (errorCode)
            {
                case ERROR_AUDIO:
                    message = "Audio recording error";
                    break;
                case ERROR_CLIENT:
                    message = "Client side error";
                    break;
                case ERROR_INSUFFICIENT_PERMISSIONS:
                    message = "Insufficient permissions";
                    break;
                case ERROR_NETWORK:
                    message = "Network error";
                    break;
                case ERROR_NETWORK_TIMEOUT:
                    message = "Network timeout";
                    break;
                case ERROR_NO_MATCH:
                    message = "No match";
                    break;
                case ERROR_RECOGNIZER_BUSY:
                    message = "RecognitionService busy";
                    break;
                case ERROR_SERVER:
                    message = "error from server";
                    break;
                case ERROR_SPEECH_TIMEOUT:
                    message = "No speech input";
                    break;
                default:
                    message = "Didn't understand, please try again.";
                    break;
            }
            return message;
        }
        #endregion
        public bool isShowPopupAndroid = true;
        public Action<string> onReadyForSpeechCallback;
        public Action onEndOfSpeechCallback;
        public Action<float> onRmsChangedCallback;
        public Action onBeginningOfSpeechCallback;
        public Action<string> onErrorCallback;
        public Action<string> onPartialResultsCallback;
        /** Called when the endpointer is ready for the user to start speaking. */
        public void onReadyForSpeech(string _params)
        {
            if (onReadyForSpeechCallback != null)
                onReadyForSpeechCallback(_params);
        }
        /** Called after the user stops speaking. */
        public void onEndOfSpeech(string _paramsNull)
        {
            if (onEndOfSpeechCallback != null)
                onEndOfSpeechCallback();
        }
        /** The sound level in the audio stream has changed. */
        public void onRmsChanged(string _value)
        {
            float _rms = float.Parse(_value);
            if (onRmsChangedCallback != null)
                onRmsChangedCallback(_rms);
        }

        /** The user has started to speak. */
        public void onBeginningOfSpeech(string _paramsNull)
        {
            if (onBeginningOfSpeechCallback != null)
                onBeginningOfSpeechCallback();
        }

        /** A network or recognition error occurred. */
        public void onError(string _value)
        {
            int _error = int.Parse(_value);
            string _message = getErrorText(_error);
            Debug.Log(_message);

            if (onErrorCallback != null)
                onErrorCallback(_message);
        }
        /** Called when partial recognition results are available. */
        public void onPartialResults(string _params)
        {
            if (onPartialResultsCallback != null)
                onPartialResultsCallback(_params);
        }

#endif
        #endregion
    }
}