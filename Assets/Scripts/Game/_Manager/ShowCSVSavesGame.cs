using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShowCSVSavesGame : MonoBehaviour, ISetupLevel, IWaitingForPlayerInput
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }

    [field: SerializeField]
    public GameState GameState { get; set; } // reference set in editor

    private FacBrick _facBrick;

    private int _counter = 0;
    private bool _beforeOrAfter = true;

    private void Awake() 
    {
        ResourceLocator.AddResource("ShowCSVSavesGame", this);    

        _facBrick = ResourceLocator.GetResource<FacBrick>("FacBrick");
    }


    public void SetupLevel()
    {
        LoadCSVSave();        

        GameState.State = GState.WaitingForPlayerInput;
    }

    public void WaitingForPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _counter++;
            LoadCSVSave();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _counter--;
            LoadCSVSave();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _beforeOrAfter = true;
            LoadCSVSave();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _beforeOrAfter = false;
            LoadCSVSave();
        }
    }

    private void LoadCSVSave()
    {
        _facBrick.DestroyBricks();
        using (StreamReader sr = new StreamReader("./gameOutput.csv"))
        {
            sr.ReadLine(); // read past headers
            for (int i = 0; i < _counter; i++) sr.ReadLine(); // read to line of counter
            string[] rowString = sr.ReadLine().Split(",");
            for (int i = 0; i < 18 * 2; i += 2)
            {
                int index = i + 8 + (_beforeOrAfter ? 0 : 18 * 2); // starts at 8, is 18 long
                // can not access gameData possibly, need method for conversion
                string rowTypes = rowString[index];
                string rowValues = rowString[index + 1];
                List<Brick> bricks = GameData.ConvertStringToBricks(rowTypes, rowValues, i / 2 + 1);
                bricks.ForEach(x => _facBrick.Create(x));
            }

        }
    }
}
