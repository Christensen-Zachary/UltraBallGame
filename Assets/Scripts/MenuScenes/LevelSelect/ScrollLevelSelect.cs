using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ScrollLevelSelect : MonoBehaviour
{
    [field: SerializeField]
    public GameObject LevelButtonPrefab { get; set; } // reference set in editor 
    [field: SerializeField]
    public GameObject BackgroundPrefab { get; set; } // reference set in editor 

    [field: SerializeField]
    public float HeightPadding { get; set; } = 0.1f;
    [field: SerializeField]
    public float HeightWidthRatio { get; set; } = 0.2f;
    
    private GameObject background;
    float height = 0f;
    float width = 0f;
    private void Awake() 
    {
        int levelNum = ES3.Load<int>(BGStrings.ES_LEVELNUM, 1);

        (height, width) = BGUtils.GetScreenSize();

        // always make grid width a ratio to the screen height. Should fit on iphone and everything, then fit to height
        // put image over top of screen that is same color as background that fades out. Buttons will go underneath but appear to fade out
        background = Instantiate(BackgroundPrefab);
        background.name = "Background";
        background.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
        background.transform.position = Vector3.zero;
        background.transform.localScale = new Vector3(height * HeightWidthRatio, height * (1 - HeightPadding), 0);

    }

    
    private void Update() 
    {
        background.transform.localScale = new Vector3(height * HeightWidthRatio, height * (1 - HeightPadding), 0);
    }

}
