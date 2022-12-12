using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public enum BrickType
{
    Square = 0,
    Triangle0 = 1,
    Triangle90 = 2,
    Triangle180 = 3,
    Triangle270 = 4,
    InvincibleSquare = 5,
    FirePowerup = 6,
    InvincibleTriangle0 = 7,
    InvincibleTriangle90 = 8,
    InvincibleTriangle180 = 9,
    InvincibleTriangle270 = 10,
    DirectionalBrick0 = 11,
    EvilBrick = 12
}

public class Brick
{
    // because damageable bricks that don't count towards winning are never in the bricks list, this function works to count those bricks to know how many bricks to destroy to win the level
    public static System.Func<BrickType, bool> IsDamageable => (BrickType x) => { return x == BrickType.Square || x == BrickType.Triangle0 || x == BrickType.Triangle90 || x == BrickType.Triangle180 || x == BrickType.Triangle270; };
    public static System.Func<BrickType, bool> IsInvincible => (BrickType x) => { return x == BrickType.InvincibleSquare || x == BrickType.InvincibleTriangle0 || x == BrickType.InvincibleTriangle90 || x == BrickType.InvincibleTriangle180 || x == BrickType.InvincibleTriangle270; };

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


    public void CopySelfInto(Brick brick)
    {
        brick.BrickType = BrickType;
        brick.Row = Row;
        brick.Col = Col;
        brick.Health = Health;
    }
    
}
