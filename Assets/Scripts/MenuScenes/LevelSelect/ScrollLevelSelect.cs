using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollLevelSelect : MonoBehaviour
{
    [field: SerializeField]
    public GameObject LevelButtonPrefab { get; set; } // reference set in editor 
    [field: SerializeField]
    public GameObject BackgroundPrefab { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image HeaderFade { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image Header { get; set; } // reference set in editor 
    [field: SerializeField]
    public Image HeaderEdge { get; set; } // reference set in editor 
    [field: SerializeField]
    public GameObject ButtonParent { get; set; } // reference set in editor 

    [field: SerializeField]
    public float HeightPadding { get; set; } = 0.1f;
    [field: SerializeField]
    public float HeightWidthRatio { get; set; } = 0.2f;
    [field: SerializeField]
    public float ButtonPadding { get; set; } = 0.8f;
    
    private GameObject background;
    float height = 0f;
    float width = 0f;
    float unitScale = 0f;
    Vector2 origin = Vector2.zero;
    int columnCount = 5;
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
        height = height * (1 - HeightPadding);
        width = width * (1 - HeightPadding);//height * HeightWidthRatio;
        background.transform.localScale = new Vector3(width, height, 0);

        unitScale = width /columnCount;
        origin = new Vector2(-background.GetComponent<SpriteRenderer>().bounds.extents.x, background.GetComponent<SpriteRenderer>().bounds.extents.y) + new Vector2(0.5f, -0.5f) * unitScale;
        // five buttons per column
        // create 10 rows
        for (int row = 1; row < 10; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                GameObject button = Instantiate(LevelButtonPrefab);
                button.name = $"LevelButton {System.Guid.NewGuid()}";
                button.transform.SetParent(ButtonParent.transform);
                button.transform.localScale = Vector3.one * unitScale * (1 - ButtonPadding);
                button.transform.position = GetPosition(col, row);
            }
        }

        HeaderFade.color = DivideColor(ThemeData.ThemeColors[ThemeItem.SuperBackground], 2);//ThemeData.ThemeColors[ThemeItem.SuperBackground];
        HeaderEdge.color = ThemeData.ThemeColors[ThemeItem.GameboardBorder];
        
        if (PlayerPrefs.GetInt(ToggleHDR.HDR_ENABLED_KEY, 1) == 1) HeaderEdge.material.SetFloat("_Brightness", ThemeData.ThemeBorderBrightness / 12f);
        else HeaderEdge.material.SetFloat("_Brightness", 3);

        Header.color = ThemeData.ThemeColors[ThemeItem.SuperBackground];
    }


    private Vector2 GetPosition(float col, float row)
    {
        return origin + new Vector2(col, -row) * new Vector2(unitScale, unitScale * (1 + ButtonPadding / 3f));
    }

    private Color DivideColor(Color color, float divisor)
    {
        return new Color(color.r / divisor, color.g / divisor, color.b / divisor, color.a);
    }

}
