using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int LevelNum { get; set; }
    public int NumberOfDivisions { get; set; } = 12;

    public List<Brick> Bricks { get; set; }
    public List<Ball> Balls { get; set; }


    public static Level GetDefault()
    {
        Level level = new Level()
        {
            LevelNum = 1,
            NumberOfDivisions = 12,
            Bricks = new List<Brick>(),
            Balls = new List<Ball>()
        };

        for (int i = 0; i < level.NumberOfDivisions - 3; i++)
        {
            for (int j = 0; j < level.NumberOfDivisions; j++)
            {
                if (j % 2 == 0 && i % 2 != 0 || j % 2 != 0 && i % 2 == 0) level.Bricks.Add(new Brick(i, j, 10));   
            }
        }

        for (int i = 0; i < 10; i++)
        {
            level.Balls.Add(new Ball(1, 0.75f));
        }

        return level;
    }
}
