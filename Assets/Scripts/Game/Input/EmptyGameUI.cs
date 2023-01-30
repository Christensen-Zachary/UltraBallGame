using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyGameUI : MonoBehaviour, IResetGame, INextLevel, IOpenMainMenu, ICloseMainMenuPanel, IOpenMainMenuPanel, IOpenOptions, ICloseOptionsPanel, IStartSliderAim, IEndSliderAim, IStartFireUI, IGiveExtraBalls, IGiveFloorBricks, IStartMove, IEndMove, IReturnFire, ISetBallsOnFire
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private void Awake()
    {
        ResourceLocator.AddResource("EmptyGameUI", this);
        
    }

    public bool CloseMainMenuPanel()
    {
        return true;
    }

    public bool CloseOptionsPanel()
    {
        return true;
    }

    public bool EndMove()
    {
        return false;
    }

    public bool EndSliderAim()
    {
        return true;
    }

    public bool GiveExtraBalls()
    {
        return false;
    }

    public bool GiveFloorBricks()
    {
        return false;
    }

    public bool NextLevel()
    {
        return false;
    }

    public bool OpenMainMenu()
    {
        return false;
    }

    public bool OpenMainMenuPanel()
    {
        return false;
    }

    public bool OpenOptions()
    {
        return false;
    }

    public bool ResetGame()
    {
        return false;
    }

    public bool ReturnFire()
    {
        return false;
    }

    public bool StartFire()
    {
        return false;
    }

    public bool StartMove()
    {
        return false;
    }

    public bool StartSliderAim()
    {
        return false;
    }

    public bool SetBallsOnFire()
    {
        return false;
    }
}
