using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BrickType
{
    Square = 0,
    Triangle0 = 1,
    Triangle90 = 2,
    Triangle180 = 3,
    Triangle270 = 4,
    InvincibleSquare = 5,
    FirePowerup = 6
}

public class Brick
{
    public static System.Func<BrickType, bool> IsDamageable => (BrickType x) => { return x != BrickType.InvincibleSquare && x != BrickType.FirePowerup; };

    public string ID { get; set; }
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
