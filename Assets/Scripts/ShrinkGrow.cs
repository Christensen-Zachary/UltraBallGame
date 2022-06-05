using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkGrow : MonoBehaviour
{
    [field: SerializeField]
    private Transform SpriteTransform { get; set; }
    [field: SerializeField]
    public float ShrinkByPercent { get; set; } = 15;
    private bool _reactRunning = false;

    public void ShowSprite()
    {
        if (SpriteTransform.TryGetComponent(out SpriteRenderer sr))
        {
            sr.enabled = true;
        }
        else
        {
            string errMsg = $"No sprite found on {gameObject.name} using ShrinkGrow";
            Debug.LogError(errMsg);
            throw new System.Exception(errMsg);
        }
    }

    public void HideSprite()
    {
        if (SpriteTransform.TryGetComponent(out SpriteRenderer sr))
        {
            sr.enabled = false;
        }
        else
        {
            string errMsg = $"No sprite found on {gameObject.name} using ShrinkGrow";
            Debug.LogError(errMsg);
            throw new System.Exception(errMsg);
        }
    }

    public void React()
    {
        if (!_reactRunning)
        {
            StopAllCoroutines();
            StartCoroutine(ShrinkGrowRoutine());
        }
    }
    private IEnumerator ShrinkGrowRoutine()
    {
        _reactRunning = true;

        float shrinkByPercent = ShrinkByPercent;
        float speed = 2.5f;

        Vector2 scale = SpriteTransform.localScale;
        Vector2 shrunkScale = SpriteTransform.localScale * (1f - shrinkByPercent / 100f);
        while (SpriteTransform.localScale.x > shrunkScale.x)
        {
            SpriteTransform.localScale -= Vector3.one * Time.deltaTime * speed;
            yield return null;
        }
        while (SpriteTransform.localScale.x < scale.x)
        {
            SpriteTransform.localScale += Vector3.one * Time.deltaTime * speed;
            yield return null;
        }
        SpriteTransform.localScale = scale;

        _reactRunning = false;
    }
}

