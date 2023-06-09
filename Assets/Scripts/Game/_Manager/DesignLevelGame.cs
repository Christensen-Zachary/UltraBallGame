using System.Collections.Generic;
using UnityEngine;

public class DesignLevelGame : MonoBehaviour, IWaitingForPlayerInput, ISetupLevel, IMovingPlayer
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    [field: SerializeField]
    private GameState GameState { get; set; } // reference set in editor

    
    private DesignBrickManager _designBrickManager;

    private DesignerInputs _designerInputs = new DesignerInputs();

    private Grid _grid;
    private FacBrick _facBrick;

    private void Awake() 
    {
        _grid = ResourceLocator.GetResource<Grid>("Grid");
        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");

        ResourceLocator.AddResource("DesignLevelGame", this);

        _designBrickManager = ResourceLocator.GetResource<DesignBrickManager>("DesignBrickManager");

    }

    private void Update()
    {
        if (_designerInputs.InputSaveLevel())
            _designBrickManager.Save();
    }

    // single brick editing
    public void WaitingForPlayerInput()
    {
        if (_designerInputs.InputBeginCreate())
            _designBrickManager.CreateBrickAndSingleSelect();

        _designBrickManager.TryMoveBricks();
        
        _designBrickManager.TryClickSelectSingle();

        _designBrickManager.TryUpdateBrickOptions();

        _designBrickManager.TryDeleteBricks();

        _designBrickManager.TrySetHealth();

        if (_designerInputs.InputCloneBrick())
        {
            _designBrickManager.CloneSelected();
        }

        if (_designerInputs.InputSwitchSelectMode())
        {
            GameState.State = GState.MovingPlayer; // multiple brick editing state
        }

        if (_designerInputs.InputLoadLevel())
        {
            _designBrickManager.LoadLevel();
        }

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            _designBrickManager.LoadRandomLevel();
        }
    }

    // multiple brick edit
    public void MovingPlayer()
    {
        _designBrickManager.TryMoveBricks();

        _designBrickManager.TryClickAddBrick();

        _designBrickManager.TryUpdateBrickOptions();

        _designBrickManager.TryDeleteBricks();

        _designBrickManager.TrySetHealth();

        if (_designerInputs.InputSwitchSelectMode())
        {
            _designBrickManager.SetSelectedSingleBrick();
            GameState.State = GState.WaitingForPlayerInput; // multiple brick editing state
            return;
        }

        if (_designerInputs.InputSelectAllBricks())
        {
            _designBrickManager.SelectAllBricks();
        }
        else if (_designerInputs.InputInvertBrickSelect())
        {
            _designBrickManager.InvertBrickSelection();
        }
        else if (_designerInputs.InputCloneBrick())
        {
            _designBrickManager.CloneSelected();
        }

        if (_designerInputs.InputHoverSelect())
        {
            _designBrickManager.HoverSelect();
        }
        else if (_designerInputs.InputHoverDeselect())
        {
            _designBrickManager.HoverDeselect();
        }
    }

    public void SetupLevel()
    {
        

        GameState.State = GState.WaitingForPlayerInput;
    }
}
