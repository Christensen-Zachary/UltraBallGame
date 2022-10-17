using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignerInputs
{
    public bool InputEvilBrick()
    {
        return Input.GetKeyDown(KeyCode.O);
    }

    public bool InputSetDirectional0()
    {
        return Input.GetKeyDown(KeyCode.I);
    }

    public bool InputSetFirePowerup()
    {
        return Input.GetKeyDown(KeyCode.U);
    }

    public bool InputSetInvincibleSquare()
    {
        return Input.GetKeyDown(KeyCode.Y);
    }

    public bool InputLoadLevel()
    {
        return Input.GetKeyDown(KeyCode.L);
    }

    public bool InputDeletedSelectedBrick()
    {
        return Input.GetKeyDown(KeyCode.D);
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
        return Input.GetKeyDown(KeyCode.T);
    }

    public bool InputSetTriangle180()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    public bool InputSetTriangle90()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public bool InputSetTriangle0()
    {
        return Input.GetKeyDown(KeyCode.W);
    }

    public bool InputSetSquare()
    {
        return Input.GetKeyDown(KeyCode.Q);
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
        return Input.GetKeyDown(KeyCode.J);
    }

    public bool InputStartSetHealth()
    {
        return Input.GetKeyDown(KeyCode.H);
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
