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

    private Material _material;

    private void Awake() 
    {
        _material = SpriteRenderer.material;
        _material.SetTexture("_GlowTex", SpriteRenderer.sprite.texture);    
        
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
                    _material.SetFloat("_Glow", 0);
                }
                else _material.SetFloat("_Glow", Mathf.Lerp(MaxGlow, 0, _timer / _duration));
            }
            else
            {
                if (_timer > _duration)
                {
                    _isShrinking = true;
                    _timer = 0;
                    _material.SetFloat("_Glow", MaxGlow);
                }
                else _material.SetFloat("_Glow", Mathf.Lerp(0, MaxGlow, _timer / _duration));
            }
        }
    }

    public void React()
    {
        if (_reactRunning)
        {
            // if already running, maintain current glow and set is shrinking to false. Will reverse shrinking
            _isShrinking = false;
        }

        _reactRunning = true;
    }

    public void SetColor(Color color, float maxGlow)
    {
        _material.SetColor("_GlowColor", color);
        MaxGlow = maxGlow;
    }

    public void SetFadeOutColor(Color color)
    {
        _material.SetColor("_FadeBurnColor", color);
    }

}
