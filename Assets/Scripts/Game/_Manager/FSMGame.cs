using System.Net;
using UnityEngine;

public enum GState
{
    EmptyState,
    SetupLevel,
    WaitingForPlayerInput,
    MovingPlayer,
    Aiming,
    SliderAiming,
    Firing,
    EndTurn,
    CheckWinLose,
    GameOver,
    Win,
    OptionsPanel
}


public class FSMGame : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    [field: SerializeField]
    private FSMGameComposition FSMGameComposition; // reference set in editor

    void Start()
    {
        BGUtils.SetFrameRate();
        //Time.timeScale = 2f;
    }


    void Update()
    {
        //print($"state: {FSMGameComposition.GetState()}");

        switch (FSMGameComposition.GetState())
        {
            case GState.EmptyState:
                break;
            case GState.SetupLevel:
                FSMGameComposition.SetupLevel();
                break;
            case GState.WaitingForPlayerInput:
                FSMGameComposition.WaitingForPlayerInput();
                break;
            case GState.MovingPlayer:
                FSMGameComposition.MovingPlayer();
                break;
            case GState.Aiming:
                FSMGameComposition.Aiming();
                break;
            case GState.SliderAiming:
                FSMGameComposition.SliderAiming();
                break;
            case GState.Firing:
                FSMGameComposition.Firing();
                break;
            case GState.EndTurn:
                FSMGameComposition.EndTurn();
                break;
            case GState.CheckWinLose:
                FSMGameComposition.CheckWinLose();
                break;
            case GState.GameOver:
                FSMGameComposition.GameOver();
                break;
            case GState.Win:
                FSMGameComposition.Win();
                break;
            case GState.OptionsPanel:
                FSMGameComposition.OptionsPanel();
                break;
        }

    }

}
