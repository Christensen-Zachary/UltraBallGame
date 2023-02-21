using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;
using System;
using Unity.Collections;
using Unity.VisualScripting;

public class LevelService : MonoBehaviour
{

    public int _levelNumber = -1;
    public int LevelNumber { get { return _levelNumber; } set
        {
            if (_levelNumberText != null)
            {
                _levelNumberText.SetNumber(value);
            }
            _levelNumber = value;
        }
    }
    public LevelNumberText _levelNumberText;
    public int NumberOfDivisions { get; set; } = 12;
    private int _extraBallPowerUpCount = 0;
    public ExtraBallsText _extraBallsText;
    public int ExtraBallPowerUpCount { get { return _extraBallPowerUpCount; } set { if (_extraBallsText != null) { _extraBallsText.SetNumber(value); } _extraBallPowerUpCount = value; } }
    public FloorBricksText _floorBricksText;
    private int _floorBricksPowerUpCount = 0;
    public int FloorBricksPowerUpCount { get { return _floorBricksPowerUpCount; } set { if (_floorBricksText != null) { _floorBricksText.SetNumber(value); } _floorBricksPowerUpCount = value; } }
    public FireBallsText _fireBallsText;
    private int _fireBallsPowerUpCount = 0;
    public int FireBallsPowerUpCount { get { return _fireBallsPowerUpCount; } set { if (_fireBallsText != null) { _fireBallsText.SetNumber(value); } _fireBallsPowerUpCount = value; } }
    private int _ballCounter = 0;
    public int BallCounter { get { return _ballCounter; } set { if (_ballCount != null) { _ballCount.SetNumber(value); } _ballCounter = value; } }
    public BallCount _ballCount;


    public List<Brick> Bricks { get; set; }
    public List<Ball> Balls { get; set; }
    public List<Ball> CurrentBalls { get; set; }
    public int RowCounter { get; private set; } = 0;
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    
    public int Health => level.Health;
    private Level level;
    public bool LevelLoaded => level != null;
    private void Awake()
    {
        ResourceLocator.AddResource("Level", this);

        int levelNum = ES3.Load<int>(BGStrings.ES_LEVELNUM, 1);
        if (LevelNumber >= 0)
        {
            levelNum = LevelNumber;
            LevelNumber = LevelNumber;
        }
        else LevelNumber = levelNum;

        level = LoadLevel(levelNum);

        if (!LevelLoaded) 
        {
            level = LoadLevel(0);
        }

        NumberOfDivisions = level.NumberOfDivisions;
        Bricks = level.Bricks;
        Balls = level.Balls;
        ResetCurrentBalls();

        BallCounter = Balls.Count;
        ExtraBallPowerUpCount = level.ExtraBallPowerUpCount;
        FloorBricksPowerUpCount = level.FloorBrickCount;
        FireBallsPowerUpCount = level.FireBallsPowerUpCount;
    }

    public void ResetLevelService()
    {
        RowCounter = 0;
        BallCounter = Balls.Count;
        ResetCurrentBalls();
        ExtraBallPowerUpCount = level.ExtraBallPowerUpCount;
        FloorBricksPowerUpCount = level.FloorBrickCount;
        FireBallsPowerUpCount = level.FireBallsPowerUpCount;
    }

    private void ResetCurrentBalls()
    {
        CurrentBalls = new List<Ball>();
        level.Balls.ForEach(x => CurrentBalls.Add(x));
    }

    public List<Brick> GetNextRow()
    {
        RowCounter++;
        return Bricks.Where(x => x.Row == RowCounter - 1).ToList();
    }


    private static readonly string NUMBEROFDIVISIONS_DELIMITER = "'";
    private static readonly string BRICK_TYPES_DELIMITER = "~";
    private static readonly string BRICK_TYPE_DELIMITER = ":";
    private static readonly string BRICK_DELIMITER = ";";
    private static readonly string BRICK_PARAMS_DELIMITER = ",";

    public static Level LoadLevel(int levelNumber)
    {
        FieldInfo field = typeof(UnityTest).GetField($"LEVEL{levelNumber}");
        if (field == null) return null;

        string levelString = field.GetValue(null) as string;
        if (levelString == null)
        {
            print($"levelString was NULL");
            return null;
        }
        
        Level level = new Level() { LevelNum = levelNumber };

        try
        {
            level.NumberOfDivisions = Convert.ToInt32(levelString.Split(NUMBEROFDIVISIONS_DELIMITER)[0]);
        }
        catch (Exception ex)
        {
            print($"Could not get [0] from NUMBEROFDIVISIONS split {ex.Message}");
            return null;
        }

        try
        {
            string str = levelString.Split(NUMBEROFDIVISIONS_DELIMITER)[1];
        }
        catch (Exception ex)
        {
            print($"Could not get [1] from NUMBEROFDIVISIONS split {ex.Message}");
            return null;
        }

        try
        {
            string[] str = levelString.Split(NUMBEROFDIVISIONS_DELIMITER)[1].Split(BRICK_TYPES_DELIMITER);
        }
        catch (Exception ex)
        {
            print($"Could not get string[] from NUMBEROFDIVISIONS split BRICKTYPES split {ex.Message}");
            return null;
        }

        foreach (var item in levelString.Split(NUMBEROFDIVISIONS_DELIMITER)[1].Split(BRICK_TYPES_DELIMITER))
        {
            if (item != "")
            {
                BrickType type;
                try
                {
                    type = (BrickType)Convert.ToInt32(item.Split(BRICK_TYPE_DELIMITER)[0]);
                }
                catch (Exception ex)
                {
                    print($"Could not get [0] from BRICKTYPE split {ex.Message}");
                    return null;
                }

                try
                {
                    string[] str = item.Split(BRICK_TYPE_DELIMITER)[1].Split(BRICK_DELIMITER);
                }
                catch (Exception ex)
                {
                    print($"Could not get [1] from BRICKTYPE split BRICK_DELIMITER split {ex.Message}");
                    foreach (var item1 in item.Split(BRICK_TYPE_DELIMITER))
                    {
                        print($"{item1}");
                    }
                    return null;
                }

                foreach (var item1 in item.Split(BRICK_TYPE_DELIMITER)[1].Split(BRICK_DELIMITER))
                {
                    if (item1 != "")
                    {

                        level.Bricks.Add(new Brick(type,
                            Convert.ToInt32(item1.Split(BRICK_PARAMS_DELIMITER)[0]),
                            Convert.ToInt32(item1.Split(BRICK_PARAMS_DELIMITER)[1]),
                            Convert.ToInt32(item1.Split(BRICK_PARAMS_DELIMITER)[2])
                        ));
                    }
                }
            }
        }

        field = typeof(PlayerResources).GetField($"LEVEL{levelNumber}");
        if (field == null)
        {
            print($"No resource count found for level {levelNumber}");
            field = typeof(PlayerResources).GetField($"DEFAULT");
        }
        levelString = field.GetValue(null) as string;

        // balls added manually because they are not currently saved and loaded
        for (int i = 0; i < Convert.ToInt32(levelString.Split(",")[0]); i++)
        {
            level.Balls.Add(new Ball(1, 0.8f));
        }

        level.ExtraBallPowerUpCount = Convert.ToInt32(levelString.Split(",")[1]);
        level.FloorBrickCount = Convert.ToInt32(levelString.Split(",")[2]);
        level.FireBallsPowerUpCount = Convert.ToInt32(levelString.Split(",")[3]);
        level.Health = Convert.ToInt32(levelString.Split(",")[4]);

        return level;
    }

    public static void SaveLevel(Level level)
    {
        using (StreamWriter sw = new StreamWriter("levelOutput.txt", true))
        {
            sw.Write($"{level.NumberOfDivisions}{NUMBEROFDIVISIONS_DELIMITER}");

            foreach (var brickType in level.Bricks.Select(x => x.BrickType).Distinct().ToArray())
            {
                sw.Write($"{(int)brickType}{BRICK_TYPE_DELIMITER}");
                foreach (var brick in level.Bricks.Where(x => x.BrickType == brickType))
                {
                    sw.Write($"{brick.Col}{BRICK_PARAMS_DELIMITER}{brick.Row}{BRICK_PARAMS_DELIMITER}{brick.Health}{BRICK_DELIMITER}");
                }
                sw.Write(BRICK_TYPES_DELIMITER);
            }

            sw.Write("\n");

            sw.Write($"{level.Balls.Count}{BRICK_PARAMS_DELIMITER}{level.ExtraBallPowerUpCount}{BRICK_PARAMS_DELIMITER}{level.FloorBrickCount}{BRICK_PARAMS_DELIMITER}{level.FireBallsPowerUpCount}{BRICK_PARAMS_DELIMITER}{level.Health}");

            sw.Write("\n");
        }
    }
}
