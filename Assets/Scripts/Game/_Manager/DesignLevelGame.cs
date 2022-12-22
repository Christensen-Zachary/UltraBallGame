using System.Collections.Generic;
using UnityEngine;

public class DesignLevelGame : MonoBehaviour, IWaitingForPlayerInput, ISetupLevel, IMovingPlayer
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    private GameState GameState { get; set; } // reference set in editor

    
    private DesignBrickManager _designBrickManager;

    private void Awake() 
    {
        ResourceLocator.AddResource("DesignLevelGame", this);

        _designBrickManager = ResourceLocator.GetResource<DesignBrickManager>("DesignBrickManager");

    }


    // single brick editing
    public void WaitingForPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _designBrickManager.CreateBrick();

        _designBrickManager.TryMoveBricks();
        
        _designBrickManager.TryClickSelectSingle();

    }

    // multiple brick edit
    public void MovingPlayer()
    {

    }

    public void SetupLevel()
    {
        _designBrickManager.SetupLevel();

        GameState.State = GState.WaitingForPlayerInput;
    }
}
