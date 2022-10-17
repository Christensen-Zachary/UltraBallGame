using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCollision : MonoBehaviour
{
    public PolygonCollider2D Collider2D { get; set; } // 0 = tl, 1 = tr, 2 = br, 3 = bl also index of binary IsProblemCorner index
    public int IsProblemCorner = 0; // index binary digits by index of PlygonCollider2D point index for t/f value
    public int Col { get; set; }
    public int Row { get; set; }
    public bool HasBeenUsed = false;
    public Dictionary<int, BrickCollision> Neighbors { get; private set; } = new Dictionary<int, BrickCollision>();

    private void Awake()
    {
        Collider2D = GetComponent<PolygonCollider2D>();

        Neighbors.Add(0, null);
        Neighbors.Add(1, null);
        Neighbors.Add(2, null);
        Neighbors.Add(3, null);
        //Neighbors.Add(4, null);
        //Neighbors.Add(5, null);
        //Neighbors.Add(6, null);
        //Neighbors.Add(7, null);
    }

}
