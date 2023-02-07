using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [field: SerializeField]
    private ResourceLocator ResourceLocator { get; set; }

    private Player _player;
    private DamageCounter _damageCounter;
    private LevelService _levelService;
    private WinService _winService;
    private FacBrick _facBrick;

    private string _gameID = "";
    private int _turnCount = 0;

    public float ShotAngle { get; set; }
    public float ShotPosition { get; set; }
    public List<Brick> BeforeGameboard { get; set; }

    private void Awake() 
    {
        ResourceLocator.AddResource("GameData", this);

        _player = ResourceLocator.GetResource<Player>("Player");
        _damageCounter = ResourceLocator.GetResource<DamageCounter>("DamageCounter");
        _levelService = ResourceLocator.GetResource<LevelService>("Level");
        _winService = ResourceLocator.GetResource<WinService>("WinService");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
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
        List<Brick> bricks = _facBrick.GetBricks();
        ConvertBricksToString(bricks);
        // print($"Returned bricks length {bricks.Count}");
        bricks.Select(x => x.Row).Distinct().ToList().ForEach(x => print($"Row {x} found"));
        // for (int i = 1; i < 19; i++)
        // {
        //     print($"Number of bricks in row {i} {bricks.Where(x => x.Row == i).Count()}");
        // }
        int damageTaken = bricks.Where(x => x.Row == 1).Count(); // is snapshot of bricks before advance, so first row bricks will cause damage
        // turn number, ball count, damage dealt, bricks destroyed, damage taken, shot angle, shot position, before gameboard, resulting gameboard, _gameID
        print("turn number, ball count, damage dealt, bricks destroyed, damage taken, shot angle, shot position, before gameboard, resulting gameboard, _gameID");
        string dataString = $"{_turnCount},{_player.Shootables.Count},{_damageCounter.TurnDamageString()},{damageTaken},{ShotAngle},{ShotPosition},{_gameID}";
        print(dataString);
    }

    public void SaveGameToFile()
    {
        // Won, number of turns, health lost, number of balls, starting gameboard, _gameID
        List<Brick> bricks = _levelService.Bricks;

        string dataString = $"{(_winService.HasWon() ? 1 : 0)},{_turnCount},{_levelService.Health - _player.Health},{_player.Shootables.Count},{_gameID}";
        print(dataString);
    }

    public string ConvertBricksToString(List<Brick> bricks)
    {
        string brickData = "";

        // track data row by row

        // 0x0 0x0 0x0 0x0 0x0 0x0 0x0 0x0 0x0 0x0 0x0 0x0 0x0 0x0 0x0 0x0
        // 64 / 12 = 5
        // brick type, brick value
        // two numbers can not share 5 bits
        // two columns for each row
        // one column for brick type
        // one column for brick value
        for (int i = 1; i < 19; i++)
        {
            List<Brick> row = bricks.Where(x => x.Row == i).ToList();
            BigInteger brickType = 0;
            BigInteger brickValue = 0;
            for (int j = 0; j < 12; j++)
            {
                Brick brick = row.Find(x => x.Col == j);
                if (brick != null)
                {
                    brickType |= ((int)brick.BrickType + 1) * (BigInteger)Mathf.Pow(2, j * BITS_PER_BRICK); // must add 1 to brick type because starts at base 0 index with square
                    if (Brick.IsDamageable(brick.BrickType) && brick.Health > 0)
                    {
                        brickValue |= (BigInteger)brick.Health * (BigInteger)Mathf.Pow(2, j * BITS_PER_BRICK);
                    }
                }
            }
            print($"Row {i} brickType value {brickType.ToSafeString()} brickValue value {brickValue.ToSafeString()}");
        }

        return brickData;
    }

    public static readonly int BITS_PER_BRICK = 8;
}
