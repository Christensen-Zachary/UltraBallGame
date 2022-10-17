using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball
{
    public int Damage { get; set; } = 1;
    public float Size { get; set; } = 1;


    public Ball(int damage, float size = 1)
    {
        Damage = damage;
        Size = size;
    }
}
