using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class BlinkGlow : MonoBehaviour
{
    [field: SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; } // reference set in prefab
    private float MaxGlow { get; set; } = 4f;
    private bool _reactRunning = false;
    private bool _isShrinking = false;
    private float _duration = 0.1f;
    private float _timer = 0;
    private Color _originalColor = new Color(0.5f, 0.5f, 0.5f, 1);


    private void Awake() 
    {
        SpriteRenderer.material.SetTexture("_GlowTex", SpriteRenderer.sprite.texture);    
        
        _originalColor = ThemeData.NormalDmgBlink;
        SetColor(_originalColor, ThemeData.NormalBlinkStrength);
        SetFadeOutColor(_originalColor);
    }

    private void Update()
    {

        if (_reactRunning)
        {
            _timer += Time.deltaTime;
            
            if (_isShrinking)
            {
                if (_timer > _duration)
                {
                    _isShrinking = false;
                    _reactRunning = false;
                    _timer = 0;
                    SpriteRenderer.material.SetFloat("_Glow", 0);
                    SetColor(_originalColor, ThemeData.NormalBlinkStrength);
                }
                else SpriteRenderer.material.SetFloat("_Glow", Mathf.Lerp(MaxGlow, 0, _timer / _duration));
            }
            else
            {
                if (_timer > _duration)
                {
                    _isShrinking = true;
                    _timer = 0;
                    SpriteRenderer.material.SetFloat("_Glow", MaxGlow);
                }
                else SpriteRenderer.material.SetFloat("_Glow", Mathf.Lerp(0, MaxGlow, _timer / _duration));
            }
        }
    }

    public void React()
    {
        if (_reactRunning)
        {
            if (_isShrinking)
            {
                _timer = 0;
                SpriteRenderer.material.SetFloat("_Glow", MaxGlow);
            }
            else 
            {
                _timer = _duration * 0.35f;
                _isShrinking = false;
                SpriteRenderer.material.SetFloat("_Glow", MaxGlow * 0.35f);
            }
        }

        _reactRunning = true;
    }

    public void SetColor(Color color, float maxGlow)
    {
        SpriteRenderer.material.SetColor("_GlowColor", color);
        MaxGlow = maxGlow;
    }

    public void SetFadeOutColor(Color color)
    {
        SpriteRenderer.material.SetColor("_FadeBurnColor", color);
    }

}
