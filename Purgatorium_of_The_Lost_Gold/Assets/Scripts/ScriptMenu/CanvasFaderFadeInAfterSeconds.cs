using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFaderFadeInAfterSeconds : MonoBehaviour
{
    [SerializeField] bool playOnAwake = true;
    [SerializeField] float fadeInDuration;
    [SerializeField] float fadeInDelay;
    [SerializeField] CanvasGroup canvasGroup;


    private void Awake()
    {
        if (playOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        StartCoroutine(_Play());
    }

    IEnumerator _Play()
    {
        float timer = fadeInDelay;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        timer = 0;

        while (timer < fadeInDuration)
        {
            canvasGroup.alpha = timer / fadeInDuration;
            timer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1.0f;


    }
}
