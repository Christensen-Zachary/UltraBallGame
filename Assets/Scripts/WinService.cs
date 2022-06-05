using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinService : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    public int NumberOfBricksToWin { get; set; }
    public int NumberOfBricksDestroyed { get; set; } = 0;


    private void Awake()
    {
        ResourceLocator.AddResource("WinService", this);
    }


    public bool HasWon()
    {
        return NumberOfBricksDestroyed == NumberOfBricksToWin;
    }
}
