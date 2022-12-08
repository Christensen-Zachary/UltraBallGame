

public interface IGetState
{
    GState GetState();
}

public interface IEmpty
{
    void Empty();
}
public interface ISetupLevel
{
    void SetupLevel();
}

public interface IWaitingForPlayerInput
{
    void WaitingForPlayerInput();
}

public interface IMovingPlayer
{
    void MovingPlayer();
}

public interface IAiming
{
    void Aiming();
}

public interface ISliderAiming
{
    void SliderAiming();
}

public interface IFiring
{
    void Firing();
}

public interface IEndTurn
{
    void EndTurn();
}

public interface IGameOver
{
    void GameOver();
}

public interface IWin
{
    void Win();
}

public interface IOptionsPanel
{
    void OptionsPanel();
}


