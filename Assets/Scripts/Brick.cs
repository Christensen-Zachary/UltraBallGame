using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick
{
    public int Row { get; set; } = 0;
    public int Col { get; set; } = 0;

    public int Health { get; set; } = 10;


    public Brick(int col, int row, int health = 10)
    {
        Col = col;
        Row = row;
        Health = health;
    }
}
