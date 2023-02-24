using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DamageCounter : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private int _damage = 0;
    public int DamageCount { get { return _damage; } set { _damage = value; }}
    private int _destroyedCount = 0;
    public int DestroyedCount { get { return _destroyedCount; } set { 
        _destroyedCount = value;
         if (_addBallProgress != null)
         {
            _addBallProgress.SetProgressValue(GetAddBallScoreRemainder() / (float)_gameSettings.destroyedBricksToAddBall); 
            _addBallProgress.SetAddBallValue(GetAddBallCount());
         }
    }}

    private List<(int damage, int destroyedCount, float time)> DamagePerTurn { get; set; } = new List<(int damage, int destroyedCount, float time)>();

    private bool _turnActive = false;
    private float _timer = 0;

    private int _leftOverAddBallDestroyCount = 0;

    private GameSettings _gameSettings;
    public AddBallProgress _addBallProgress; // reference set in editor


    private void Awake() 
    {
        ResourceLocator.AddResource("DamageCounter", this);

        _gameSettings = ResourceLocator.GetResource<GameSettings>("GameSettings");
    }

    private void Update() 
    {
        if (_turnActive)
        {
            _timer += Time.deltaTime;
        }    
    }

    public void StartTurn()
    {
        _turnActive = true;
        _timer = 0;
    }

    // returns number of balls to add to player
    public int EndTurn()
    {
        _turnActive = false;

        // calculate how many balls to give player
        _leftOverAddBallDestroyCount += DestroyedCount;
        int ballsToAdd = 0;
        if (_leftOverAddBallDestroyCount >= _gameSettings.destroyedBricksToAddBall)
        {
            while (_leftOverAddBallDestroyCount >= _gameSettings.destroyedBricksToAddBall)
            {
                ballsToAdd++;
                _leftOverAddBallDestroyCount -= _gameSettings.destroyedBricksToAddBall;
            }
        }

        // keep track of turn statistics
        DamagePerTurn.Add((DamageCount, DestroyedCount, _timer));
        DamageCount = 0;
        DestroyedCount = 0;
        return ballsToAdd;
    }

    public float GetAddBallScoreRemainder()
    {
        // current destroyed count plus leftover. Leftover is decremented at end of turn when bals are granted
        return (DestroyedCount + _leftOverAddBallDestroyCount) % _gameSettings.destroyedBricksToAddBall; 
    }

    public int GetAddBallCount()
    {
        return (DestroyedCount + _leftOverAddBallDestroyCount) / _gameSettings.destroyedBricksToAddBall;
    }

    public void ResetCounters()
    {
        DamagePerTurn = new();
        DamageCount = 0;
        DestroyedCount = 0;
        _turnActive = false;

    }

    public string TurnDamageString()
    {
        // totalDamage, destroyedCount, 
        return $"{DamagePerTurn.Last().damage},{DamagePerTurn.Last().destroyedCount}";
        //print($"{DamagePerTurn.Count}: {DamagePerTurn.Last().damage} - {DamagePerTurn.Last().destroyedCount} - {DamagePerTurn.Last().time} - {DamagePerTurn.Last().damage / DamagePerTurn.Last().time}");
    }

}
