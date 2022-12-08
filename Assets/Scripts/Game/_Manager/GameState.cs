using UnityEngine;

public class GameState : MonoBehaviour
{
    public GState State { get; set; } = GState.SetupLevel;
    public GState StateBeforeOptions { get; set;} = GState.EmptyState;
}