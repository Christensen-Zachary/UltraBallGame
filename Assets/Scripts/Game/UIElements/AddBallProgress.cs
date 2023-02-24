using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddBallProgress : MonoBehaviour
{
    private Image _image;
    private float _animationDuration = 0.3f; // seconds

    public TextMeshProUGUI _addBallCount; // reference set in editor

    private void Awake() 
    {
        _image = GetComponent<Image>();    
    }

    public void SetProgressValue(float value) // between 0 and 1, i.e. already lerp'd
    {
        StopAllCoroutines();
        StartCoroutine(Animate(_image.fillAmount, value));
    }

    public void SetAddBallValue(int ballCount)
    {
        if (_addBallCount == null) return;

        _addBallCount.text = "+" + ballCount.ToString();
    }

    public IEnumerator Animate(float from, float to)
    {
        float timer = 0;
        if (to < from) // then animate upwards the rest of the way and reset to 0
        {
            while (timer < _animationDuration)
            {
                timer += Time.deltaTime;

                _image.fillAmount = Mathf.Lerp(from, 1, timer / _animationDuration);
                yield return null;
            }
            _image.fillAmount = 0;
            from = 0;
        }
        
        timer = 0;
        while (timer < _animationDuration)
        {
            timer += Time.deltaTime;

            _image.fillAmount = Mathf.Lerp(from, to, timer / _animationDuration);
            yield return null;
        }
        _image.fillAmount = to;
    }
}
