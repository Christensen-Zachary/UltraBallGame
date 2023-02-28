using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BtnOptionsAnimation : MonoBehaviour
{
    private Image image;

    private readonly float _fadeDuration = 0.4f;

    private void Awake() 
    {
        image = GetComponent<Image>();    
    }

    public void Show()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }

    public void Hide()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    public IEnumerator FadeOut()
    {
        float timer = 0;

        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;

            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(1, 0, timer / _fadeDuration));

            yield return null;            
        }
    }

    public IEnumerator FadeIn()
    {
        float timer = 0;

        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;

            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(0, 1, timer / _fadeDuration));

            yield return null;            
        }
    }
}
