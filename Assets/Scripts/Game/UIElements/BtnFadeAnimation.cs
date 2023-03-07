using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BtnFadeAnimation : MonoBehaviour
{
    private Image image;
    public TextMeshProUGUI _text; // reference set in editor
    private bool _isTextPresent = false;

    public static readonly float FADE_DURATION = 0.4f;

    private void Awake() 
    {
        image = GetComponent<Image>();

        if (_text != null) _isTextPresent = true;
    }

    public void Show()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        if (_isTextPresent) _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);
    }

    public void Hide()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        if (_isTextPresent) _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0);
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        StartCoroutine(DoFadeIn());
    }

    public void FadeOut()
    {
        StartCoroutine(DoFadeOut());
        gameObject.SetActive(false);
    }

    private IEnumerator DoFadeOut()
    {
        float timer = 0;

        while (timer < FADE_DURATION)
        {
            timer += Time.deltaTime;

            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(1, 0, timer / FADE_DURATION));
            if (_isTextPresent) _text.color = new Color(_text.color.r, _text.color.g, _text.color.b,  Mathf.Lerp(1, 0, timer / FADE_DURATION));

            yield return null;            
        }
    }

    private IEnumerator DoFadeIn()
    {
        float timer = 0;

        while (timer < FADE_DURATION)
        {
            timer += Time.deltaTime;

            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(0, 1, timer / FADE_DURATION));
            if (_isTextPresent) _text.color = new Color(_text.color.r, _text.color.g, _text.color.b,  Mathf.Lerp(0, 1, timer / FADE_DURATION));

            yield return null;            
        }
    }
}
