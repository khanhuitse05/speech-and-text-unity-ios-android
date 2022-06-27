package com.starseed.speechtotext;

import android.content.Intent;
import android.speech.RecognizerIntent;
import com.unity3d.player.UnityPlayer;

/**
 * Created by J1mmyTo9
 */
public class Bridge {

    // Speak To Text
    protected static int RESULT_SPEECH = 1;
    protected static String languageSpeech = "en-US";
    public static void OpenSpeechToText(String prompt){
        Intent intent = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE, languageSpeech);
        intent.putExtra(RecognizerIntent.EXTRA_PARTIAL_RESULTS, true);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_PREFERENCE, languageSpeech);
        intent.putExtra(RecognizerIntent.EXTRA_ONLY_RETURN_LANGUAGE_PREFERENCE, languageSpeech);
        intent.putExtra(RecognizerIntent.EXTRA_SPEECH_INPUT_MINIMUM_LENGTH_MILLIS, Long.valueOf(5000));
        intent.putExtra(RecognizerIntent.EXTRA_SPEECH_INPUT_COMPLETE_SILENCE_LENGTH_MILLIS, Long.valueOf(3000));
        intent.putExtra(RecognizerIntent.EXTRA_SPEECH_INPUT_POSSIBLY_COMPLETE_SILENCE_LENGTH_MILLIS, Long.valueOf(3000));
        if (!prompt.equals("")) intent.putExtra(RecognizerIntent.EXTRA_PROMPT, prompt);
        UnityPlayer.currentActivity.startActivityForResult(intent, RESULT_SPEECH);
    }
    public static void SettingSpeechToText(String language){
        languageSpeech = language;
    }
    public static void StartRecording() {
        MainActivity activity = (MainActivity)UnityPlayer.currentActivity;
        activity.OnStartRecording();
    }
    public static void StopRecording() {
        MainActivity activity = (MainActivity)UnityPlayer.currentActivity;
        activity.OnStopRecording();
    }

    // Text To Speech
    public static void OpenTextToSpeed(String text) {
        MainActivity activity = (MainActivity)UnityPlayer.currentActivity;
        activity.OnStartSpeak(text);
    }
    public static void SettingTextToSpeed(String language, float pitch, float rate) {
        MainActivity activity = (MainActivity)UnityPlayer.currentActivity;
        activity.OnSettingSpeak(language, pitch, rate);
    }
    public static void StopTextToSpeed(){
        MainActivity activity = (MainActivity)UnityPlayer.currentActivity;
        activity.OnStopSpeak();
    }
}
