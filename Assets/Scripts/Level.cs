using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    [field: SerializeField]
    public int LevelNum { get; set; }
    [field: SerializeField]
    public int NumberOfDivisions { get; set; } = 12;

    [field: SerializeField]
    public List<Brick> Bricks { get; set; } = new List<Brick>();
    [field: SerializeField]
    public List<Ball> Balls { get; set; } = new List<Ball>();


    public static Level GetDefault()
    {
        Level level = new Level()
        {
            LevelNum = 1,
            NumberOfDivisions = 12,
            Bricks = new List<Brick>(),
            Balls = new List<Ball>()
        };

        for (int row = 0; row < level.NumberOfDivisions - 2; row++)
        {
            for (int col = 0; col < level.NumberOfDivisions; col++)
            {
                if (col != level.NumberOfDivisions / 2 
                    && col != (level.NumberOfDivisions / 2) + 1 
                    && (row % 2 == 0 && col % 2 != 0 || row % 2 != 0 && col % 2 == 0)) 

                    level.Bricks.Add(new Brick(BrickType.Square, col, row, Random.Range(4, 25)));
            }
        }

        for (int i = 0; i < 5; i++)
        {
            level.Balls.Add(new Ball(1, 1.1f));
        }

        return level;
    }

    public static Level GetRandom()
    {
        Level level = new Level()
        {
            LevelNum = 1,
            NumberOfDivisions = Random.Range(25, 35),
            Bricks = new List<Brick>(),
            Balls = new List<Ball>()
        };

        PerlinNoise perlin = new PerlinNoise();
        float[,] perlinNoise = perlin.GeneratePerlinNoise(perlin.GenerateWhiteNoise(level.NumberOfDivisions, level.NumberOfDivisions), 2);
        float[,] perlinNoiseHealth = perlin.GeneratePerlinNoise(perlin.GenerateWhiteNoise(level.NumberOfDivisions, level.NumberOfDivisions), 2);

        for (int i = 0; i < perlinNoise.GetLength(0); i++)
        {
            for (int j = 0; j < perlinNoise.GetLength(1); j++)
            {
                if (perlinNoiseHealth[i, j] > 0.25f)
                //if (true)
                {
                    level.Bricks.Add(new Brick(BrickType.Square, i, j, (perlinNoiseHealth[i, j] > 0.5f) ? (int)(100 * perlinNoiseHealth[i, j]) : 1));
                }
            }
        }

        //for (int row = 0; row < level.NumberOfDivisions - 2; row++)
        //{
        //    for (int col = 0; col < level.NumberOfDivisions; col++)
        //    {
        //        if (Random.Range(1, 100) % 2 == 0)
        //            level.Bricks.Add(new Brick(BrickType.Square, col, row, Random.Range(10, 10)));
        //    }
        //}

        for (int i = 0; i < 5; i++)
        {
            level.Balls.Add(new Ball(1, 1.1f));
        }

        return level;
    }
}
