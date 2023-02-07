using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }

    private Player _player;
    private DamageCounter _damageCounter;

    private string _gameID = "";
    private int _turnCount = 0;

    private void Awake() 
    {
        ResourceLocator.AddResource("GameData", this);

        _player = ResourceLocator.GetResource<Player>("Player");
        _damageCounter = ResourceLocator.GetResource<DamageCounter>("DamageCounter");
    }

    public void ResetGameData()
    {
        _gameID = System.Guid.NewGuid().ToString();
        _turnCount = 0;
    }

    public void AdvanceTurn()
    {
        _turnCount++;
    }

    public void SaveTurnToFile()
    {
        string dataString = $"{_turnCount},{_player.Shootables.Count},{_damageCounter.TurnDamageString()},{_gameID}";
        print(dataString);
    }
}
