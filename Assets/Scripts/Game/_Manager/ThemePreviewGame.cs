using System;
using UnityEngine;

public class ThemePreviewGame : MonoBehaviour, IWin
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }


    [field: SerializeField]
    public GameState GameState { get; set; } // reference set in editor

    private void Awake() 
    {
        ResourceLocator.AddResource("ThemePreviewGame", this);    
    }

    public void Win()
    {
        GameState.State = GState.SetupLevel;
    }

}
