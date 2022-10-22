using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIMKB : MonoBehaviour, IResetGame, INextLevel, IOpenMainMenu, ICloseMainMenuPanel, IOpenMainMenuPanel, IOpenOptions, ICloseOptionsPanel, IStartSliderAim, IEndSliderAim, IStartFireUI, IGiveExtraBalls, IGiveFloorBricks, IStartMove, IEndMove, IReturnFire
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    private void Awake()
    {
        ResourceLocator.AddResource("GameUIMKB", this);
    }

    public bool CloseMainMenuPanel()
    {
        return false;
    }

    public bool CloseOptionsPanel()
    {
        return false;
    }

    public bool EndMove()
    {
        return false;
    }

    public bool EndSliderAim()
    {
        return false;
    }

    public bool GiveExtraBalls()
    {
        return Input.GetKeyDown(KeyCode.B);
    }

    public bool GiveFloorBricks()
    {
        return Input.GetKeyDown(KeyCode.N);
    }

    public bool NextLevel()
    {
        return Input.GetKeyDown(KeyCode.N);
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
        return Input.GetKeyDown(KeyCode.O);
    }

    public bool ResetGame()
    {
        return Input.GetKeyDown(KeyCode.R);
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
}
