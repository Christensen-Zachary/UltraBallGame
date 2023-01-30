using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyGame : MonoBehaviour, IGetState, IEmpty, ISetupLevel, IWaitingForPlayerInput, IMovingPlayer, IAiming, ISliderAiming, IFiring, IEndTurn, ICheckWinLose, IGameOver, IWin, IOptionsPanel
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; } 


    private void Awake() 
    {
        ResourceLocator.AddResource("EmptyGame", this);    
    }

    public void Aiming()
    {
        
    }

    public void CheckWinLose()
    {
        
    }

    public void Empty()
    {
        
    }

    public void EndTurn()
    {
        
    }

    public void Firing()
    {
        
    }

    public void GameOver()
    {
        
    }

    public GState GetState()
    {
        return GState.EmptyState;
    }

    public void MovingPlayer()
    {
        
    }

    public void OptionsPanel()
    {
        
    }

    public void SetupLevel()
    {
        
    }

    public void SliderAiming()
    {
        
    }

    public void WaitingForPlayerInput()
    {
        
    }

    public void Win()
    {
        
    }
}
