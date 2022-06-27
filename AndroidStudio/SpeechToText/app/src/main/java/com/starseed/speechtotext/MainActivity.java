package com.starseed.speechtotext;

import android.content.Intent;
import android.os.Bundle;
import android.speech.RecognitionListener;
import android.speech.RecognizerIntent;
import android.speech.SpeechRecognizer;
import android.speech.tts.TextToSpeech;
import android.speech.tts.UtteranceProgressListener;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import java.util.ArrayList;
import java.util.Locale;


public class MainActivity extends UnityPlayerActivity
{
    private TextToSpeech tts;
    private SpeechRecognizer speech;
    private Intent intent;
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        tts = new TextToSpeech(this, initListener);

        speech = SpeechRecognizer.createSpeechRecognizer(this);
        speech.setRecognitionListener(recognitionListener);
    }
    @Override
    public void onDestroy() {
        // Don't forget to shutdown tts!
        if (tts != null) {
            tts.stop();
            tts.shutdown();
        }
        if (speech != null) {
            speech.destroy();
        }
        super.onDestroy();
    }
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode == RESULT_OK && null != data) {
            ArrayList<String> text = data.getStringArrayListExtra(RecognizerIntent.EXTRA_RESULTS);
            UnityPlayer.UnitySendMessage("SpeechToText", "onResults", text.get(0));
        }
    }

    // speech to text
    public void OnStartRecording() {
        intent = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_PREFERENCE, Bridge.languageSpeech);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL, Bridge.languageSpeech);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE, Bridge.languageSpeech);
        //intent.putExtra(RecognizerIntent.EXTRA_SPEECH_INPUT_MINIMUM_LENGTH_MILLIS, 2000);
        intent.putExtra(RecognizerIntent.EXTRA_SPEECH_INPUT_COMPLETE_SILENCE_LENGTH_MILLIS, 2000);
        //intent.putExtra(RecognizerIntent.EXTRA_SPEECH_INPUT_POSSIBLY_COMPLETE_SILENCE_LENGTH_MILLIS, 2000);
        intent.putExtra(RecognizerIntent.EXTRA_CALLING_PACKAGE, this.getPackageName());
        intent.putExtra(RecognizerIntent.EXTRA_MAX_RESULTS, 3);

        this.runOnUiThread(new Runnable() {

            @Override
            public void run() {
                speech.startListening(intent);
            }
        });
        UnityPlayer.UnitySendMessage("SpeechToText", "onMessage", "CallStart, Language:" + Bridge.languageSpeech);
    }
    public void OnStopRecording() {
        this.runOnUiThread(new Runnable() {

            @Override
            public void run() {
                speech.stopListening();
            }
        });
        UnityPlayer.UnitySendMessage("SpeechToText", "onMessage", "CallStop");
    }

    RecognitionListener recognitionListener = new RecognitionListener() {
        @Override
        public void onReadyForSpeech(Bundle params) {
            UnityPlayer.UnitySendMessage("SpeechToText", "onReadyForSpeech", params.toString());
        }
        @Override
        public void onBeginningOfSpeech() {
            UnityPlayer.UnitySendMessage("SpeechToText", "onBeginningOfSpeech", "");
        }
        @Override
        public void onRmsChanged(float rmsdB) {
            UnityPlayer.UnitySendMessage("SpeechToText", "onRmsChanged", "" + rmsdB);
        }
        @Override
        public void onBufferReceived(byte[] buffer) {
            UnityPlayer.UnitySendMessage("SpeechToText", "onMessage", "onBufferReceived: " + buffer.length);
        }
        @Override
        public void onEndOfSpeech() {
            UnityPlayer.UnitySendMessage("SpeechToText", "onEndOfSpeech", "");
        }
        @Override
        public void onError(int error) {
            UnityPlayer.UnitySendMessage("SpeechToText", "onError", "" + error);
        }
        @Override
        public void onResults(Bundle results) {
            ArrayList<String> text = results.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION);
            UnityPlayer.UnitySendMessage("SpeechToText", "onResults", text.get(0));
        }
        @Override
        public void onPartialResults(Bundle partialResults) {
            ArrayList<String> text = partialResults.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION);
            UnityPlayer.UnitySendMessage("SpeechToText", "onPartialResults", text.get(0));
        }
        @Override
        public void onEvent(int eventType, Bundle params) {

            UnityPlayer.UnitySendMessage("SpeechToText", "onMessage", "onEvent");
        }
    };


    ////
    public  void OnStartSpeak(String valueText)
    {
        tts.speak(valueText, TextToSpeech.QUEUE_FLUSH, null, valueText);
    }
    public void OnSettingSpeak(String language, float pitch, float rate) {
        tts.setPitch(pitch);
        tts.setSpeechRate(rate);
        int result = tts.setLanguage(getLocaleFromString(language));
        UnityPlayer.UnitySendMessage("TextToSpeech", "onSettingResult", "" + result);
    }
    public void OnStopSpeak()
    {
        tts.stop();
    }

    TextToSpeech.OnInitListener initListener = new TextToSpeech.OnInitListener()
    {
        @Override
        public void onInit(int status) {
            if (status == TextToSpeech.SUCCESS)
            {
                OnSettingSpeak(Locale.US.toString(), 1.0f, 1.0f);
                tts.setOnUtteranceProgressListener(utteranceProgressListener);
            }
        }
    };

    UtteranceProgressListener utteranceProgressListener=new UtteranceProgressListener() {
        @Override
        public void onStart(String utteranceId) {
            UnityPlayer.UnitySendMessage("TextToSpeech", "onStart", utteranceId);
        }
        @Override
        public void onError(String utteranceId) {
            UnityPlayer.UnitySendMessage("TextToSpeech", "onError", utteranceId);
        }
        @Override
        public void onDone(String utteranceId) {
            UnityPlayer.UnitySendMessage("TextToSpeech", "onDone", utteranceId);
        }
    };

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
        if (localeString.equalsIgnoreCase("default"))
        {
            return Locale.getDefault();
        }

        // Extract language
        int languageIndex = localeString.indexOf('_');
        String language;
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
        String country;
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
