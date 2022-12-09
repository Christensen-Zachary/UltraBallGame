using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignerBrick : MonoBehaviour
{
    
    public SpriteRenderer SpriteRenderer { get; set; }
    public Brick Brick { get; set; } = new Brick(BrickType.Square, 0, 1);

    private void Awake() 
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();    
    }
}
