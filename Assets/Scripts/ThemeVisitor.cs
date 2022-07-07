using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ThemeItem
{
    Player,
    Background,
    MaxDamage,
    MinDamage,
    PlayerMaxHealth,
    PlayerMinHealth,
    MidPrediction,
    EndPrediction,
    BasicBall,
    SuperBackground,
    GameBorder, // for colorless borders
    InvincibleBrick,
    Button
}

public enum ThemeType
{
    Default,
    JellyFish,
    Theme2,
    VaporWave,
    Theme3,
    Exterminate
}

public enum CustomColor
{
    Black,
    Grey,
    DarkGreen,
    LightGreen,
    LightRed,
    Brown,
    LightYellow,
    Orange,
    Green,
    Red
}

public class ThemeVisitor : MonoBehaviour
{
    [field: SerializeField]
    public TMPro.TMP_FontAsset Font1 { get; set; } // set in prefab
    [field: SerializeField]
    public TMPro.TMP_FontAsset Font2 { get; set; } // set in prefab
    [field: SerializeField]
    public TMPro.TMP_FontAsset Font3 { get; set; } // set in prefab

    private static ThemeType themeType;
    private void Awake()
    {
        
        themeType = ES3.Load<ThemeType>(BGStrings.ES_THEMETYPE, ThemeType.Default);
        SetThemeType(themeType);
        
        ThemeFonts = new Dictionary<ThemeType, TMPro.TMP_FontAsset>()
        {
            { ThemeType.Default, Font1 },
            { ThemeType.JellyFish, Font2 },
            { ThemeType.Exterminate, Font3 }
        };

    }

    public static Dictionary<ThemeItem, Color> ThemeColors { get; private set; }

    public static Dictionary<ThemeType, TMPro.TMP_FontAsset> ThemeFonts { get; private set; }
    public static Color32 BrickTextColor;
    public static Color32 ThemeFontColor;
    public static Color32 ThemeButtonImageColor;
    public static void SetThemeType(ThemeType themeType)
    {
        BrickTextColor = new Color32(255, 255, 255, 255);
        ThemeFontColor = new Color32(0xff, 0xff, 0xff, 0xff);
        ThemeButtonImageColor = new Color32(0xff, 0xff, 0xff, 0xff);

        ThemeColors = new Dictionary<ThemeItem, Color>() {
            // default colors, are overwritten in GetThemeColors
            { ThemeItem.Player, GetColor(CustomColor.Orange) },
            { ThemeItem.Background, ConvertToColor(60, 60, 60) },
            { ThemeItem.MaxDamage, GetColor(CustomColor.DarkGreen) },
            { ThemeItem.MinDamage, GetColor(CustomColor.LightGreen) },
            { ThemeItem.PlayerMaxHealth, GetColor(CustomColor.Green) },
            { ThemeItem.PlayerMinHealth, GetColor(CustomColor.Red) },
            { ThemeItem.MidPrediction, GetColor(CustomColor.LightYellow) },
            { ThemeItem.EndPrediction, GetColor(CustomColor.LightYellow) },
            { ThemeItem.BasicBall, GetColor(CustomColor.Orange) },
            { ThemeItem.SuperBackground, ConvertToColor(35, 35, 35) },
            { ThemeItem.InvincibleBrick, Color.white },
            { ThemeItem.Button, GetColor(CustomColor.Brown) }
        };

        switch (themeType)
        {
            case ThemeType.JellyFish:
                SetThemeColor(ThemeItem.Player, ConvertToColor(0x3e, 0xa1, 0xb6)); // moonstone
                SetThemeColor(ThemeItem.BasicBall, ConvertToColor(0x3e, 0xa1, 0xb6));
                SetThemeColor(ThemeItem.MidPrediction, ConvertToColor(107, 102, 158)); // Dark blue grey
                SetThemeColor(ThemeItem.EndPrediction, ConvertToColor(107, 102, 158));

                SetThemeColor(ThemeItem.MaxDamage, ConvertToColor(187, 144, 200)); // lenurple
                SetThemeColor(ThemeItem.MinDamage, ConvertToColor(239, 216, 236)); // piggy pink
                SetThemeColor(ThemeItem.PlayerMaxHealth, ConvertToColor(187, 144, 200));
                SetThemeColor(ThemeItem.PlayerMinHealth, ConvertToColor(239, 216, 236)); 
                
                SetThemeColor(ThemeItem.SuperBackground, ConvertToColor(9, 28, 42));
                SetThemeColor(ThemeItem.Background, Color.black);

                SetThemeColor(ThemeItem.Button, GetColor(CustomColor.Brown));
                break;
            case ThemeType.Theme2: // https://www.schemecolor.com/working-back.php
                SetThemeColor(ThemeItem.Player, ConvertToColor(151, 204, 184));
                SetThemeColor(ThemeItem.BasicBall, ConvertToColor(151, 204, 184));
                SetThemeColor(ThemeItem.EndPrediction, ConvertToColor(188, 230, 194));
                SetThemeColor(ThemeItem.MidPrediction, ConvertToColor(188, 230, 194));

                SetThemeColor(ThemeItem.MaxDamage, ConvertToColor(221, 189, 152));
                SetThemeColor(ThemeItem.MinDamage, ConvertToColor(240, 224, 201));
                SetThemeColor(ThemeItem.PlayerMaxHealth, ConvertToColor(221, 189, 152));
                SetThemeColor(ThemeItem.PlayerMinHealth, ConvertToColor(240, 224, 201));

                SetThemeColor(ThemeItem.Background, ConvertToColor(75, 72, 130));
                SetThemeColor(ThemeItem.SuperBackground, ConvertToColor(8, 34, 19));
                break;
            case ThemeType.VaporWave:
                SetThemeColor(ThemeItem.Player, ConvertToColor(255, 113, 206));
                SetThemeColor(ThemeItem.MidPrediction, ConvertToColor(255, 113, 206));
                SetThemeColor(ThemeItem.EndPrediction, ConvertToColor(255, 113, 206));
                SetThemeColor(ThemeItem.BasicBall, ConvertToColor(255, 113, 206));
                
                SetThemeColor(ThemeItem.MaxDamage, ConvertToColor(5, 255, 161));
                SetThemeColor(ThemeItem.MinDamage, ConvertToColor(255, 251, 150));
                SetThemeColor(ThemeItem.PlayerMaxHealth, ConvertToColor(5, 255, 161));
                SetThemeColor(ThemeItem.PlayerMinHealth, ConvertToColor(255, 251, 150));

                SetThemeColor(ThemeItem.Background, ConvertToColor(1, 205, 254));
                SetThemeColor(ThemeItem.SuperBackground, ConvertToColor(1, 144, 178));
                break;
            case ThemeType.Theme3:
                SetThemeColor(ThemeItem.Player, ConvertToColor(0xfe, 0x88, 0x04)); // orange
                SetThemeColor(ThemeItem.BasicBall, ConvertToColor(0xfe, 0x88, 0x04));
                SetThemeColor(ThemeItem.MidPrediction, ConvertToColor(0xfe, 0x88, 0x04));
                SetThemeColor(ThemeItem.EndPrediction, ConvertToColor(0xfe, 0x88, 0x04));

                BrickTextColor = new Color32(75, 72, 130, 255);
                SetThemeColor(ThemeItem.MaxDamage, ConvertToColor(5, 255, 161));
                SetThemeColor(ThemeItem.MinDamage, ConvertToColor(255, 251, 150));
                SetThemeColor(ThemeItem.PlayerMaxHealth, ConvertToColor(5, 255, 161));
                SetThemeColor(ThemeItem.PlayerMinHealth, ConvertToColor(255, 251, 150));

                SetThemeColor(ThemeItem.Background, ConvertToColor(75, 72, 130)); 
                SetThemeColor(ThemeItem.SuperBackground, ConvertToColor(0xfe, 0x88, 0x04));
                break;
            case ThemeType.Exterminate:
                BrickTextColor = new Color32(0xBB, 0xBB, 0x00, 0xff);
                SetThemeColor(ThemeItem.MaxDamage, ConvertToColor(0xa1, 0x33, 0x33));
                SetThemeColor(ThemeItem.MinDamage, ConvertToColor(0xb3, 0x54, 0x1e));
                SetThemeColor(ThemeItem.PlayerMaxHealth, ConvertToColor(0xa1, 0x33, 0x33));
                SetThemeColor(ThemeItem.PlayerMinHealth, ConvertToColor(0xb3, 0x54, 0x1e));
                SetThemeColor(ThemeItem.InvincibleBrick, new Color(0.65f, 0.65f, 0.65f));

                SetThemeColor(ThemeItem.MidPrediction, ConvertToColor(0x70, 0x70, 0x70));
                SetThemeColor(ThemeItem.EndPrediction, ConvertToColor(0x70, 0x70, 0x70));

                SetThemeColor(ThemeItem.GameBorder, ConvertToColor(0x46, 0x11, 0x11));
                SetThemeColor(ThemeItem.SuperBackground, ConvertToColor(4, 3, 3));
                SetThemeColor(ThemeItem.Background, ConvertToColor(4, 3, 3));

                ThemeFontColor = new Color32(0xBB, 0xBB, 0x00, 0xff);
                ThemeButtonImageColor = new Color32(0xBB, 0xBB, 0x00, 0xff);
                break;
        }
    }

    private static void SetThemeColor(ThemeItem themeItem, Color color)
    {
        ThemeColors[themeItem] = color;
    }

    public static void Visit(ThemeButtonImage themeButtonImage)
    {
        themeButtonImage.GetComponent<UnityEngine.UI.Image>().color = ThemeButtonImageColor;
    }

    public static void Visit(ThemeFontColor themeFontColor)
    {
        themeFontColor.TextMesh.color = ThemeFontColor;
    }

    public static void Visit(InvincibleBrick invincibleBrick)
    {
        invincibleBrick.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.InvincibleBrick];
    }
    public static void Visit(BrickNumber brickNumber)
    {
        brickNumber.TextMesh.color = BrickTextColor;
    }

    public static void Visit(Damageable damageable)
    {
        damageable.MaxColor = ThemeColors[ThemeItem.MaxDamage];
        damageable.MinColor = ThemeColors[ThemeItem.MinDamage];
    }

    public static void Visit(Player player)
    {

        switch (themeType)
        {
            case ThemeType.Theme3:
                player.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PNG/JackOLantern");
                break;
            default:
                player.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.Player];
                break;
        }
    }

    public static void Visit(Aim aim)
    {
        aim.EndPredictionSprite.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.EndPrediction];
        aim.MidPredictionSprite.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.MidPrediction];
    }


    public static void Visit(Shootable shootable)
    {
        shootable.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.BasicBall];
    }

    public static void Visit(PlayerHealth playerHealth)
    {
        playerHealth.MaxHealthColor = ThemeColors[ThemeItem.MaxDamage];
        playerHealth.MinHealthColor = ThemeColors[ThemeItem.MinDamage];
    }

    public static void Visit(Background background)
    {
        background.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.Background];
    }
    public static void Visit(SuperBackground superBackground)
    {
        switch (themeType)
        {
            case ThemeType.JellyFish:
                SetSpriteSuperBackground("Sprites/PNG/bg4", superBackground.gameObject);
                break;
            case ThemeType.VaporWave:
                SetSpriteSuperBackground("Sprites/PNG/bg1", superBackground.gameObject);
                break;
            default:
                superBackground.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.SuperBackground];
                break;
        }

    }

    private static void SetSpriteSuperBackground(string spritePath, GameObject gameObject)
    {
        Sprite sprite = SetSprite(spritePath, gameObject);

        (float height, float width) = BGUtils.GetScreenSize();
        float spriteRatio = sprite.rect.height / sprite.rect.width;
        float screenRatio = height / width;

        if (spriteRatio > screenRatio) // sprite tall and narrow compared to screen. Fit to width
        {
            print("Fitting sprite to width");
            gameObject.transform.localScale = spriteRatio * 0.95f * width * Vector2.one;
        }
        else // sprite short and wide compared to screen. Fit to height
        {
            print("Fitting sprite to height");
            gameObject.transform.localScale = height * 0.95f * Vector2.one;
        }
    }

    private static Sprite SetSprite(string spritePath, GameObject gameObject)
    {
        Sprite sprite = Resources.Load<Sprite>(spritePath);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        return sprite;
    }

    public static void Visit(ThemeText themeText)
    {
        if (ThemeFonts.ContainsKey(themeType)) themeText.SetFont(ThemeFonts[themeType]);
        else themeText.SetFont(ThemeFonts[ThemeType.Default]);
    }

    public static void Visit(ThemeGameBorder themeGameBorder)
    {
        switch (themeType)
        {
            case ThemeType.Default:
                SetSprite("Sprites/PNG/bg5", themeGameBorder.gameObject);
                themeGameBorder.GetComponent<SpriteRenderer>().sortingOrder = 0;
                break;
            case ThemeType.JellyFish:
                SetSprite("Sprites/PNG/bg6", themeGameBorder.gameObject);
                themeGameBorder.GetComponent<SpriteRenderer>().sortingOrder = 0;
                break;
            case ThemeType.VaporWave:
                SetSprite("Sprites/PNG/bg2", themeGameBorder.gameObject);
                themeGameBorder.GetComponent<SpriteRenderer>().sortingOrder = 0;
                break;
            case ThemeType.Theme3:
                SetSprite("Sprites/PNG/bg5", themeGameBorder.gameObject);
                themeGameBorder.GetComponent<SpriteRenderer>().sortingOrder = 0;
                break;
            case ThemeType.Exterminate:
                SetSprite("Sprites/PNG/bg0", themeGameBorder.gameObject);
                themeGameBorder.GetComponent<SpriteRenderer>().color = ThemeColors[ThemeItem.GameBorder];
                themeGameBorder.GetComponent<SpriteRenderer>().sortingOrder = -101;
                break;
            default:
                break;
        }
    }

    public static Color GetColor(CustomColor customColor)
    {
        switch (customColor)
        {
            case CustomColor.Green:
                return Color.green;
            case CustomColor.Red:
                return Color.red;
            case CustomColor.Black:
                return Color.black;
            case CustomColor.Grey:
                return new Color(0.1f, 0.1f, 0.2f);
            case CustomColor.DarkGreen:
                return ConvertToColor(0x0B, 0x72, 0x6D);
            case CustomColor.LightGreen:
                return ConvertToColor(0xD5, 0xFC, 0xD4);
            case CustomColor.LightRed:
                return ConvertToColor(0xFF, 0x91, 0xA8);
            case CustomColor.Brown:
                return ConvertToColor(0x4B, 0x4B, 0x4B);
            case CustomColor.LightYellow:
                return ConvertToColor(0xF1, 0xDD, 0x89);
            case CustomColor.Orange:
                return ConvertToColor(0xFF, 0xA5, 0x00);
            default:
                return Color.white;
        }

    }

    private static Color ConvertToColor(int r, int g, int b)
    {
        return new Color(r / 256f, g / 256f, b / 256f);
    }


}
