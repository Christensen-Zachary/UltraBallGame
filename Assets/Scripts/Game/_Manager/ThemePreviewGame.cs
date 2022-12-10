using System;
using System.Collections;
using UnityEngine;

public class ThemePreviewGame : MonoBehaviour, ICheckWinLose
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }


    [field: SerializeField]
    public GameState GameState { get; set; } // reference set in editor

    private Player _player;
    private WinService _winService;
    private void Awake() 
    {
        ResourceLocator.AddResource("ThemePreviewGame", this);    

        _player = ResourceLocator.GetResource<Player>("Player");
        _winService = ResourceLocator.GetResource<WinService>("WinService");
    }


    public void CheckWinLose()
    {
        if (_player.Health <= 0)
        {
            GameState.State = GState.SetupLevel;
        }
        else if (_winService.HasWon())
        {
            GameState.State = GState.SetupLevel;
        }
        else
        {
            GameState.State = GState.WaitingForPlayerInput;
        }
    }
}
