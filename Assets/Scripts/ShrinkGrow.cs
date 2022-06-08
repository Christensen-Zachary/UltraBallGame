using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkGrow : MonoBehaviour
{
    [field: SerializeField]
    public Transform SpriteTransform { get; private set; }
    [field: SerializeField]
    public float ShrinkByPercent { get; set; } = 15;
    private bool _reactRunning = false;
    private Vector2 _scale;
    private Vector2 _shrunkScale;
    private float _speed = 2.5f;
    private bool _isShrinking = true;

    private void Update()
    {
        if (_reactRunning)
        {
            if (_isShrinking)
            {
                SpriteTransform.localScale -= Vector3.one * Time.deltaTime * _speed;
                if (SpriteTransform.localScale.x < _shrunkScale.x)
                {
                    _isShrinking = false;
                }
            }
            else
            {
                SpriteTransform.localScale += Vector3.one * Time.deltaTime * _speed;
                if (SpriteTransform.localScale.x > _scale.x)
                {
                    SpriteTransform.localScale = _scale;
                    _isShrinking = true;
                    _reactRunning = false;
                }
            }
        }
    }

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
        _reactRunning = true;
    }

    // must be called before game runs
    public void SetScales()
    {
        _scale = SpriteTransform.localScale;
        _shrunkScale = SpriteTransform.localScale * (1f - ShrinkByPercent / 100f);
    }

    
}

