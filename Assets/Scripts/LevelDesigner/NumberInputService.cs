using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberInputService : MonoBehaviour
{
    
    public bool AcceptInput { get; set; } = false;

    private DesignerInputs _designerInputs = new DesignerInputs();
    private string _numberString = "";



    public int GetNumber()
    {
        if (int.TryParse(_numberString, out int result))
        {
            return result;
        }
        
        return -1;
    }

    public void ResetNumber()
    {
        AcceptInput = false;
        _numberString = "";
    }

    void Update()
    {
        if (AcceptInput)
        {
            if (_designerInputs.Input0())
            {
                _numberString += "0";
            }
            else if (_designerInputs.Input1())
            {
                _numberString += "1";
            }
            else if (_designerInputs.Input2())
            {
                _numberString += "2";
            }
            else if (_designerInputs.Input3())
            {
                _numberString += "3";
            }
            else if (_designerInputs.Input4())
            {
                _numberString += "4";
            }
            else if (_designerInputs.Input5())
            {
                _numberString += "5";
            }
            else if (_designerInputs.Input6())
            {
                _numberString += "6";
            }
            else if (_designerInputs.Input7())
            {
                _numberString += "7";
            }
            else if (_designerInputs.Input8())
            {
                _numberString += "8";
            }
            else if (_designerInputs.Input9())
            {
                _numberString += "9";
            }
        }
        else
        {
            _numberString = "";
        }
    }
}
