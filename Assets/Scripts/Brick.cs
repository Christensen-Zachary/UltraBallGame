using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BrickType
{
    Square = 0
}

public class Brick
{
    public BrickType BrickType { get; set; } = BrickType.Square;
    public int Row { get; set; } = 0;
    public int Col { get; set; } = 0;

    public int Health { get; set; } = 10;

    public Brick() { }

    public Brick(BrickType brickType, int col, int row, int health = 10)
    {
        BrickType = brickType;
        Col = col;
        Row = row;
        Health = health;
    }
    
}
