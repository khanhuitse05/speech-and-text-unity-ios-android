using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class SpeechButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SampleSpeechToText sample;
    public GameObject effect;
    public float speedEffect = 1;
    public float scaleEffect = 1.2f;
    float speed;
    float scale = 1;

    void Start()
    {
        effect.SetActive(false);
        speed = speedEffect;
    }
    void Update()
    {
        if (effect.activeSelf)
        {
            scale += Time.deltaTime * speed;
            if (scale > scaleEffect)
            {
                speed = -speedEffect;
            }
            if (scale < scaleEffect - 0.1f)
            {
                speed = speedEffect;
            }
            effect.transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        bool needEffect = true;
#if UNITY_ANDROID
        if (sample.isShowPopupAndroid)
        {
            needEffect = false;
        }
#endif
        if (needEffect)
        {
            effect.SetActive(true);
            scale = 1;
        }
        sample.StartRecording();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        effect.SetActive(false);
        sample.StopRecording();
    }
}
