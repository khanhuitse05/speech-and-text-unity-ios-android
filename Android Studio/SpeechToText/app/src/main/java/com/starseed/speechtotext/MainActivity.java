package com.starseed.speechtotext;

import android.content.Intent;
import android.os.Bundle;
import android.speech.RecognizerIntent;
import android.speech.tts.TextToSpeech;
import android.speech.tts.UtteranceProgressListener;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Locale;


public class MainActivity extends UnityPlayerActivity implements TextToSpeech.OnInitListener
{
    private TextToSpeech tts;
    String valueText = "";
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (tts == null) {
            tts = new TextToSpeech(this, this);
        }
    }
    @Override
    public void onDestroy() {
        // Don't forget to shutdown tts!
        if (tts != null) {
            tts.stop();
            tts.shutdown();
        }
        super.onDestroy();
    }
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode == RESULT_OK && null != data) {
            ArrayList<String> text = data.getStringArrayListExtra(RecognizerIntent.EXTRA_RESULTS);
            UnityPlayer.UnitySendMessage("SpeechToText", "CallbackSpeechToText", text.get(0));
        }
    }

    @Override
    public void onInit(int status) {
        if (status == TextToSpeech.SUCCESS)
        {
            OnSettingSpeak(Locale.KOREA.toString(), 1.0f, 1.0f);
            tts.setOnUtteranceProgressListener(utteranceProgressListener);
        }
    }

    UtteranceProgressListener utteranceProgressListener=new UtteranceProgressListener() {

        @Override
        public void onStart(String utteranceId) {
            UnityPlayer.UnitySendMessage("TextToSpeech", "CallbackTextToSpeech", "onStart");
        }

        @Override
        public void onError(String utteranceId) {
            UnityPlayer.UnitySendMessage("TextToSpeech", "CallbackTextToSpeech", "onError");
        }

        @Override
        public void onDone(String utteranceId) {
            UnityPlayer.UnitySendMessage("TextToSpeech", "CallbackTextToSpeech", "onDone");
        }
    };
    public void OnInitSpeak()
    {
        if (tts == null) {
            tts = new TextToSpeech(this, this);
        }
    }
    public  void OnStartSpeak(String text)
    {
        valueText = text;
        speakOut();
    }
    private void speakOut()
    {
        HashMap<String, String> map = new HashMap<String, String>();
        map.put(TextToSpeech.Engine.KEY_PARAM_UTTERANCE_ID,"id01");
        tts.speak(valueText, TextToSpeech.QUEUE_FLUSH, map);
    }
    public void OnSettingSpeak(String language, float pitch, float rate)
    {
        tts.setPitch(pitch);
        tts.setSpeechRate(rate);
        int result = tts.setLanguage(getLocaleFromString(language));
        if (result == TextToSpeech.LANG_MISSING_DATA || result == TextToSpeech.LANG_NOT_SUPPORTED) {
            UnityPlayer.UnitySendMessage("TextToSpeech", "CallbackTextToSpeech", "This Language is not supported");
        } else {
            UnityPlayer.UnitySendMessage("TextToSpeech", "CallbackTextToSpeech", "This Language valid");
        }
    }
    public void OnStopSpeak()
    {
        tts.stop();
    }
    /**
     * Convert a string based locale into a Locale Object.
     * Assumes the string has form "{language}_{country}_{variant}".
     * Examples: "en", "de_DE", "_GB", "en_US_WIN", "de__POSIX", "fr_MAC"
     *
     * @param localeString The String
     * @return the Locale
     */
    public static Locale getLocaleFromString(String localeString)
    {
        if (localeString == null)
        {
            return null;
        }
        localeString = localeString.trim();
        if (localeString.toLowerCase().equals("default"))
        {
            return Locale.getDefault();
        }

        // Extract language
        int languageIndex = localeString.indexOf('_');
        String language = null;
        if (languageIndex == -1)
        {
            // No further "_" so is "{language}" only
            return new Locale(localeString, "");
        }
        else
        {
            language = localeString.substring(0, languageIndex);
        }

        // Extract country
        int countryIndex = localeString.indexOf('_', languageIndex + 1);
        String country = null;
        if (countryIndex == -1)
        {
            // No further "_" so is "{language}_{country}"
            country = localeString.substring(languageIndex+1);
            return new Locale(language, country);
        }
        else
        {
            // Assume all remaining is the variant so is "{language}_{country}_{variant}"
            country = localeString.substring(languageIndex+1, countryIndex);
            String variant = localeString.substring(countryIndex+1);
            return new Locale(language, country, variant);
        }
    }
}
