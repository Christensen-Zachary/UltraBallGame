using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

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
        // bricks.Select(x => x.Row).Distinct().ToList().ForEach(x => print($"Row {x} found"));
        // for (int i = 1; i < 19; i++)
        // {
        //     print($"Number of bricks in row {i} {bricks.Where(x => x.Row == i).Count()}");
        // }
        int damageTaken = bricks.Where(x => x.Row == 1).Count(); // is snapshot of bricks before advance, so first row bricks will cause damage
        // turn number, ball count, damage dealt, bricks destroyed, damage taken, shot angle, shot position, before gameboard, resulting gameboard, _gameID
        print("turn number, ball count, damage dealt, bricks destroyed, damage taken, shot angle, shot position, before gameboard, resulting gameboard, _gameID");
        string dataString = $"{_turnCount},{_player.Shootables.Count},{_damageCounter.TurnDamageString()},{damageTaken},{ShotAngle},{ShotPosition},{ConvertBricksToString(BeforeGameboard)},{ConvertBricksToString(bricks)},{_gameID}";
        using (StreamWriter sw = new StreamWriter("./gameOutput.csv", true))
        {
            sw.WriteLine(dataString);
        }
    }

    public void SaveGameToFile()
    {
        // Won, number of turns, health lost, number of balls, starting gameboard, _gameID
        string dataString = $"{(_player.Health > 0 && _winService.HasWon() ? 1 : 0)},{_turnCount},{_levelService.Health - _player.Health},{_player.Shootables.Count},{ConvertBricksToString(_levelService.Bricks)},{_gameID}";
        using (StreamWriter sw = new StreamWriter("./fullGameOutput.csv", true))
        {
            sw.WriteLine(dataString);
        }

    }

    public string ConvertBricksToString(List<Brick> bricks)
    {
        string brickData = "";

        // track data row by row
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
            brickData += $"{brickType.ToSafeString()},{brickValue.ToSafeString()}";
            if (i != 18) brickData += ",";
        }

        return brickData;
    }

    public List<Brick> ConvertStringToBricks(string brickTypeString, string brickValueString, int row)
    {
        List<Brick> bricks = new List<Brick>();

        BigInteger brickTypes = BigInteger.Parse(brickTypeString);
        BigInteger brickValues = BigInteger.Parse(brickValueString);
        
        for (int i = 0; i < 12; i++)
        {
            int typeBits = 0;
            int valueBits = 0;
            for (int j = 0; j < BITS_PER_BRICK; j++)
            {
                if (brickTypes / (BigInteger)Mathf.Pow(2, i * BITS_PER_BRICK + j) % 2 == 1)
                {
                    typeBits |= 1 << j;
                }

                if (brickValues / (BigInteger)Mathf.Pow(2, i * BITS_PER_BRICK + j) % 2 == 1)
                {
                    valueBits |= 1 << j;
                }
            }

            if (typeBits > 0) 
            {
                typeBits--; // -- because brick types are offset by 1 since square is 0
                
                bricks.Add(new Brick()
                {
                    Row = row,
                    Col = i,
                    Health = valueBits,
                    BrickType = (BrickType)typeBits
                });
            }
        }
    

        return bricks;
    }

    public void TestBrickStringConversion()
    {
            List<Brick> bricks = _facBrick.GetBricks();
            string brickString = ConvertBricksToString(bricks);
            List<string> brickStrings = brickString.Split(',').ToList();
            List<Brick> bricksFromString = new List<Brick>();
            for (int i = 0; i < brickStrings.Count; i += 2)
            {
                // i + 1 th row because rows start at 1 and count up
                ConvertStringToBricks(brickStrings[i], brickStrings[i+1], (i / 2) + 1).ForEach(x => bricksFromString.Add(x));
            }

            // for (int i = 0; i < bricks.Count; i++)
            // {
            //     print($"{bricks[i].Row},{bricks[i].Col},{bricks[i].BrickType} - {bricksFromString[i].Row},{bricksFromString[i].Col},{bricksFromString[i].BrickType}");
            // }

            for (int i = 0; i < bricks.Count; i++)
            {
                Assert.IsTrue(bricksFromString.Any(y => 
                {
                    return 
                    bricks[i].BrickType == y.BrickType && 
                    bricks[i].Row == y.Row && 
                    bricks[i].Col == y.Col && 
                    (!Brick.IsDamageable(bricks[i].BrickType) || bricks[i].Health == y.Health);
                }), $"{bricks[i].BrickType},{bricks[i].Row},{bricks[i].Col},{bricks[i].Health}");
            }
    }

    public static readonly int BITS_PER_BRICK = 9; // largest brick value is 270 so 9 bits needed for max value of 511
}
