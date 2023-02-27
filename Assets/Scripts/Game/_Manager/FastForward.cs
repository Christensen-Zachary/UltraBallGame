using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FastForward : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private Player _player;
    private GameSettings _gameSettings;

    private float _firingTimer = 0;
    private readonly float _timeToFastForward = 18f; // seconds
    private readonly int _maxBallsActiveToTriggerFastForward = 6;
    private bool _fastForwardActive = false;
    public Animator _fastForwardAnimator; // set in editor
    private float _fastTime = 2f;

    private void Awake() 
    {
        ResourceLocator.AddResource("FastForward", this);    

        _player = ResourceLocator.GetResource<Player>("Player");
        _gameSettings = ResourceLocator.GetResource<GameSettings>("GameSettings");
    }

    public void AdvanceTimer()
    {
        _firingTimer += Time.deltaTime;
    }

    public void TryFastForward()
    {
        if (!_fastForwardActive && _firingTimer > _timeToFastForward)
        {
            if (_player.Shootables.Count(x => !x.IsReturned) <= _maxBallsActiveToTriggerFastForward || _firingTimer > _timeToFastForward * 2f)
            {
                if (_fastForwardAnimator != null) 
                {
                    _fastForwardAnimator.speed = 1f;
                    _fastForwardAnimator.SetTrigger("blink");
                }
                _fastForwardActive = true;
                if (_gameSettings.timeScale == 1) Time.timeScale = _fastTime;
            }
        }
    }

    public void Reset()
    {
        if (_fastForwardActive)
        {
            if (_fastForwardAnimator != null) _fastForwardAnimator.speed = _fastTime;
            if (_gameSettings.timeScale == 1) Time.timeScale = 1f;
            _fastForwardActive = false;
        }
        _firingTimer = 0;
    }
}
