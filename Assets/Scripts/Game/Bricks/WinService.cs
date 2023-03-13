using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinService : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    private int _numberOfBricksToWin = 0;
    public int NumberOfBricksToWin { get { return _numberOfBricksToWin; } set { _numberOfBricksToWin = value; if (_brickCountText != null) { _brickCountText.MaxBricks = value; } } }
    private int _numberOfBricksDestroyed = 0;
    public int NumberOfBricksDestroyed { get { return _numberOfBricksDestroyed;  } set { _numberOfBricksDestroyed = value; if (_brickCountText != null) { _brickCountText.CurrentBricks = value; } } }

    private BrickCountText _brickCountText;

    private void Awake()
    {
        ResourceLocator.AddResource("WinService", this);

        _brickCountText = ResourceLocator.GetResource<BrickCountText>("BrickCountText");
    }


    public bool HasWon()
    {
        return NumberOfBricksDestroyed == NumberOfBricksToWin;
    }


}
