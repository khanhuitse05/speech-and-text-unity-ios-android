using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SampleSpeechToText : MonoBehaviour {

    public SpeechToText speech;
    public GameObject loading;
    public Button button;
    public Text txtStatus;
    public Text txtResult;
    public Text txtQuestion;
    public Text txtPercent;
    public TextAsset dictionaryTextFile;
    public float timeOut = 20;
    bool isFirst;
    float timeCount;
    double value;
    string results;
    string[] listData;

    void Start () {
        txtResult.text = "";
        txtStatus.text = "Tap to recording ...";
        txtQuestion.text = "";
        txtPercent.text = "";
        isFirst = false;
        loading.SetActive(false);
        button.interactable = true;
#if UNITY_IPHONE
        speech.onStart = OnStartSpeech;
        speech.onStop = OnStopSpeech;
        speech.onResult = OnResultSpeech;
#endif
        LoadData();
    }

    bool isRecording = false;
    public void btnClick()
    {
#if UNITY_IPHONE
        if (isRecording == false)
        {
            speech.startRecording();
        }
        else
        {
            speech.stopRecording();
        }
#endif
    }
    void Update()
    {
        if (isRecording)
        {
            timeCount -= Time.deltaTime;
            txtStatus.text = "" + (int)timeCount;
            if (timeCount <= 0)
            {
                speech.stopRecording();
            }
        }

    }
    void OnStartSpeech()
    {
        isRecording = true;
        timeCount = timeOut;
        int _index = UnityEngine.Random.Range(0, listData.Length);
        txtQuestion.text = listData[_index];
        txtResult.text = "";
        txtPercent.text = "";
        txtStatus.text = "" + timeCount;
    }

    void OnStopSpeech()
    {
        isRecording = false;
        button.interactable = false;
        loading.SetActive(true);
        txtStatus.text = "Stop";
    }
    void OnResultSpeech(string _data)
    {
        button.interactable = true;
        loading.SetActive(false);
        if (results != "")
        {
            results = _data;
            txtResult.text = _data;
            value = CalculateSimilarity(txtResult.text.ToLower(), txtQuestion.text.ToLower());
            txtPercent.text = "" + ((int)(value * 100)).ToString() + "%";
        }
        else
        {
            txtResult.text = "null";
            value = 0;
            txtPercent.text = "00%";
        }
    }



    void LoadData()
    {
        string theWholeFileAsOneLongString = dictionaryTextFile.text;
        char[] delimiterChars = {'\n', '\r'};
        listData = theWholeFileAsOneLongString.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Returns the number of steps required to transform the source string
    /// into the target string.
    /// </summary>
    int ComputeLevenshteinDistance(string source, string target)
    {
        if ((source == null) || (target == null)) return 0;
        if ((source.Length == 0) || (target.Length == 0)) return 0;
        if (source == target) return source.Length;

        int sourceWordCount = source.Length;
        int targetWordCount = target.Length;

        // Step 1
        if (sourceWordCount == 0)
            return targetWordCount;

        if (targetWordCount == 0)
            return sourceWordCount;

        int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

        // Step 2
        for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
        for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

        for (int i = 1; i <= sourceWordCount; i++)
        {
            for (int j = 1; j <= targetWordCount; j++)
            {
                // Step 3
                int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                // Step 4
                distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
            }
        }
        return distance[sourceWordCount, targetWordCount];
    }
    /// <summary>
    /// Calculate percentage similarity of two strings
    /// <param name="source">Source String to Compare with</param>
    /// <param name="target">Targeted String to Compare</param>
    /// <returns>Return Similarity between two strings from 0 to 1.0</returns>
    /// </summary>
    double CalculateSimilarity(string source, string target)
    {
        if ((source == null) || (target == null)) return 0.0;
        if ((source.Length == 0) || (target.Length == 0)) return 0.0;
        if (source == target) return 1.0;

        int stepsToSame = ComputeLevenshteinDistance(source, target);
        return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
    }
}
