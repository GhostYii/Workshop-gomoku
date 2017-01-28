///ORG: Ghostyii & MoonLight Game
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class LogoCross : MonoBehaviour
{
    public List<Image> images = new List<Image>();
    [Range(0.1f, 1f)]
    public float fadeSpeed = 0.5f;
    //public bool zoomOut = true;
    [Space(10)]
    public OnXXXEvent onLaunchEnd = new OnXXXEvent();

    private Image crentImage = null;
    private bool started = false;
    private bool launched = false;

    private void Awake()
    {
        Color c = new Color(1, 1, 1, 0);
        foreach (Image item in images)
            item.color = c;


    }

    private void Update()
    {
        //if (images.Count <= 0) return;
        if (!started) { StartCoroutine(CrossImages()); started = true; }

        if (launched) { onLaunchEnd.Invoke(); launched = false; }
    }

    IEnumerator CrossImages()
    {
        for (int i = 0; i < images.Count; i++)
        {
            crentImage = images[i];
            while (crentImage.color.a < 0.97f)
            {
                Color c = crentImage.color;
                c.a += Time.deltaTime * fadeSpeed;
                crentImage.color = c;
                yield return new WaitForEndOfFrame();
            }

            crentImage.color = Color.clear;
            yield return new WaitForEndOfFrame();
        }

        launched = true;
        yield return null;
    }

    [System.Serializable]
    public class OnXXXEvent : UnityEvent { }
}