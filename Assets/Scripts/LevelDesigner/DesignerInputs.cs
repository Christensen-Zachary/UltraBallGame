using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesignerInputs
{
    public bool InputEvilBrick()
    {
        return Input.GetKeyDown(KeyCode.N);
    }

    public bool InputSetDirectional0()
    {
        return Input.GetKeyDown(KeyCode.Comma);
    }

    public bool InputSetFirePowerup()
    {
        return Input.GetKeyDown(KeyCode.M);
    }

    public bool InputLoadLevel()
    {
        return Input.GetKeyDown(KeyCode.L);
    }

    public bool InputDeletedSelectedBrick()
    {
        return Input.GetKeyDown(KeyCode.Backspace);
    }

    public bool InputSaveLevel()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }

    public bool InputSetSelectedDown()
    {
        return Input.GetKeyDown(KeyCode.Keypad2);
    }

    public bool InputSetSelectedUp()
    {
        return Input.GetKeyDown(KeyCode.Keypad5);
    }

    public bool InputSetSelectedLeft()
    {
        return Input.GetKeyDown(KeyCode.Keypad1);
    }

    public bool InputSetSelectedRight()
    {
        return Input.GetKeyDown(KeyCode.Keypad3);
    }

    public bool InputSetTriangle270()
    {
        return Input.GetKeyDown(KeyCode.P);
    }

    public bool InputSetTriangle180()
    {
        return Input.GetKeyDown(KeyCode.O);
    }

    public bool InputSetTriangle90()
    {
        return Input.GetKeyDown(KeyCode.I);
    }

    public bool InputSetTriangle0()
    {
        return Input.GetKeyDown(KeyCode.U);
    }

    public bool InputSetSquare()
    {
        return Input.GetKeyDown(KeyCode.Y);
    }

    public bool InputSetInvincibleTriangle270()
    {
        return Input.GetKeyDown(KeyCode.Semicolon);
    }

    public bool InputSetInvincibleTriangle180()
    {
        return Input.GetKeyDown(KeyCode.L);
    }

    public bool InputSetInvincibleTriangle90()
    {
        return Input.GetKeyDown(KeyCode.K);
    }

    public bool InputSetInvincibleTriangle0()
    {
        return Input.GetKeyDown(KeyCode.J);
    }

    public bool InputSetInvincibleSquare()
    {
        return Input.GetKeyDown(KeyCode.H);
    }

    public int InputGetBrickType()
    {
        if (InputSetSquare())
        {
            return (int)BrickType.Square;
        }
        else if (InputSetTriangle0())
        {
            return (int)BrickType.Triangle0;
        }
        else if (InputSetTriangle90())
        {
            return (int)BrickType.Triangle90;
        }
        else if (InputSetTriangle180())
        {
            return (int)BrickType.Triangle180;
        }
        else if (InputSetTriangle270())
        {
            return (int)BrickType.Triangle270;
        }
        else if (InputEvilBrick())
        {
            return (int)BrickType.EvilBrick;
        }
        else if (InputSetFirePowerup())
        {
            return (int)BrickType.FirePowerup;
        }
        else if (InputSetInvincibleSquare())
        {
            return (int)BrickType.InvincibleSquare;
        }
        else if (InputSetInvincibleTriangle0())
        {
            return (int)BrickType.InvincibleTriangle0;
        }
        else if (InputSetInvincibleTriangle90())
        {
            return (int)BrickType.InvincibleTriangle90;
        }
        else if (InputSetInvincibleTriangle180())
        {
            return (int)BrickType.InvincibleTriangle180;
        }
        else if (InputSetInvincibleTriangle270())
        {
            return (int)BrickType.InvincibleTriangle270;
        }
        else if (InputSetDirectional0())
        {
            return (int)BrickType.DirectionalBrick0;
        }

        return -1;
    }

    public bool InputInvertBrickSelect()
    {
        return Input.GetKeyDown(KeyCode.Minus);
    }

    public bool InputSelectAllBricks()
    {
        return Input.GetKeyDown(KeyCode.Equals);
    }

    public bool InputSwitchSelectMode()
    {
        return Input.GetKeyDown(KeyCode.LeftControl);
    }

    public bool InputCloneBrick()
    {
        return Input.GetKeyDown(KeyCode.C);
    }

    public bool InputBeginCreate()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    

    public bool InputEndSetHealth()
    {
        return Input.GetKeyDown(KeyCode.F);
    }

    public bool InputStartSetHealth()
    {
        return Input.GetKeyDown(KeyCode.G);
    }

    public bool InputMoveRun()
    {
        return Input.GetKey(KeyCode.RightAlt);
    }

    public bool InputMoveUp()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }

    public bool InputMoveDown()
    {
        return Input.GetKeyDown(KeyCode.DownArrow);
    }

    public bool InputMoveRight()
    {
        return Input.GetKeyDown(KeyCode.RightArrow);
    }

    public bool InputMoveLeft()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow);
    }

    public bool Input9()
    {
        return Input.GetKeyDown(KeyCode.Alpha9);
    }

    public bool Input8()
    {
        return Input.GetKeyDown(KeyCode.Alpha8);
    }

    public bool Input7()
    {
        return Input.GetKeyDown(KeyCode.Alpha7);
    }

    public bool Input6()
    {
        return Input.GetKeyDown(KeyCode.Alpha6);
    }

    public bool Input5()
    {
        return Input.GetKeyDown(KeyCode.Alpha5);
    }

    public bool Input4()
    {
        return Input.GetKeyDown(KeyCode.Alpha4);
    }

    public bool Input3()
    {
        return Input.GetKeyDown(KeyCode.Alpha3);
    }

    public bool Input2()
    {
        return Input.GetKeyDown(KeyCode.Alpha2);
    }

    public bool Input1()
    {
        return Input.GetKeyDown(KeyCode.Alpha1);
    }

    public bool Input0()
    {
        return Input.GetKeyDown(KeyCode.Alpha0);
    }

}
