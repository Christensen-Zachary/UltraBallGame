using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;
using System.Security.Cryptography;

public class ThemeData : MonoBehaviour
{
    [field: SerializeField]
    public TMPro.TMP_FontAsset Font1 { get; set; } // set in prefab
    [field: SerializeField]
    public TMPro.TMP_FontAsset Font2 { get; set; } // set in prefab
    [field: SerializeField]
    public TMPro.TMP_FontAsset Font3 { get; set; } // set in prefab
    

    public static ThemeType ThemeType;
    public static Dictionary<ThemeItem, Color> ThemeColors { get; private set; }
    public static Dictionary<ThemeItem, GameObject> ThemeParticleSystems { get; private set; }


    public static Dictionary<ThemeType, TMPro.TMP_FontAsset> ThemeFonts { get; private set; }
    public static Color32 BrickTextColor;
    public static Color32 ThemeFontColor;
    public static Color32 ThemeButtonImageColor;

    public static Color NormalDmgBlink = new Color(0.5f, 0.5f, 0.5f, 1);
    public static Color FireDmgBlink = new Color(0.8f, 0, 0.1f, 1);
    public static Color ExtraFireDmgBlink = new Color(0.8f, 0, 0.8f, 1);
    public static float NormalBlinkStrength = 4f;
    public static float FireBlinkStrength = 4f;
    public static float ExtraFireBlinkStrength = 4f;

    public static float ThemeBorderBrightness = 48f;
    public static float PlayerBrightness = 2f;

    private void Awake()
    {
        ThemeType = ES3.Load<ThemeType>(BGStrings.ES_THEMETYPE, ThemeType.Default);
        SetThemeType(ThemeType);

        ThemeFonts = new Dictionary<ThemeType, TMPro.TMP_FontAsset>()
        {
            { ThemeType.Default, Font1 },
            { ThemeType.Retro, Font1 },
            { ThemeType.JellyFish, Font3 }
        };

    }

    public static Sprite GetThemeButton(ThemeButtonSize themeButtonSize)
    {
        switch (themeButtonSize)
        {
            case ThemeButtonSize.Normal:
                return GetNormalButton();
            case ThemeButtonSize.Square:
                return GetSquareButton();
            case ThemeButtonSize.Wide:
                return GetWideButton();
            case ThemeButtonSize.Slider:
                return GetSliderImage();
            default:
                return GetNormalButton();
        }
    }

    // normal button. 1:1.6 ratio
    public static Sprite GetNormalButton()
    {
        return Resources.Load<Sprite>("Sprites/UI/buttonHardShadow");
    }

    // wide button for options menu. doesn't need light/dark version
    public static Sprite GetWideButton()
    {
        return Resources.Load<Sprite>("Sprites/UI/buttonHardShadowLong");
    }

    // image for slider
    public static Sprite GetSliderImage()
    {
        return Resources.Load<Sprite>("Sprites/UI/buttonHardShadowExtraLong");
    }

    public static Sprite GetSquareButton()
    {   
        return Resources.Load<Sprite>("Sprites/UI/buttonHardShadowSquare");
    }

    public static void SetThemeType(ThemeType themeType)
    {
        BrickTextColor = new Color32(255, 255, 255, 255);
        ThemeFontColor = new Color32(0xff, 0xff, 0xff, 0xff);
        ThemeButtonImageColor = new Color32(0xff, 0xff, 0xff, 0xff);

        NormalDmgBlink = GetColor(CustomColor.DarkGreen);
        FireDmgBlink = new Color(0.8f, 0.4f, 0, 1);
        ExtraFireDmgBlink = new Color(0.8f, 0, 0.8f, 1);
        NormalBlinkStrength = 6f;
        FireBlinkStrength = 5f;
        ExtraFireBlinkStrength = 4f;

        ThemeBorderBrightness = 64f;
        PlayerBrightness = 2f;

        ThemeColors = new Dictionary<ThemeItem, Color>() {
            // default colors, are overwritten in GetThemeColors
            { ThemeItem.Player, GetColor(CustomColor.Orange) },
            { ThemeItem.Background, ConvertToColor(60, 60, 60) },
            { ThemeItem.MaxDamage, GetColor(CustomColor.DarkGreen) },
            { ThemeItem.MinDamage, GetColor(CustomColor.LightGreen) },
            { ThemeItem.PlayerMaxHealth, GetColor(CustomColor.DarkGreen) },
            { ThemeItem.PlayerMinHealth, GetColor(CustomColor.LightGreen) },
            { ThemeItem.MidPrediction, GetColor(CustomColor.Orange) },
            { ThemeItem.EndPrediction, GetColor(CustomColor.Orange) },
            { ThemeItem.BasicBall, GetColor(CustomColor.Orange) },
            { ThemeItem.SuperBackground, ConvertToColor(25, 25, 25) },
            { ThemeItem.InvincibleBrick, Color.white },
            { ThemeItem.Button, GetColor(CustomColor.Brown) },
            { ThemeItem.GameboardBackground, ConvertToColor(45, 45, 45) },
            { ThemeItem.FirePowerup1, Color.black },
            { ThemeItem.FirePowerup2, Color.white },
            { ThemeItem.GameboardBorder, GetColor(CustomColor.DarkGreen) }
        };

        ThemeParticleSystems = new Dictionary<ThemeItem, GameObject>() {
            { ThemeItem.FirePowerup1, Resources.Load<GameObject>($"ParticleSystems/Fire/BrickFire1/Fire{(int)themeType}") },
            { ThemeItem.FirePowerup2, Resources.Load<GameObject>($"ParticleSystems/Fire/BrickFire2/Fire{(int)themeType}") }
        };

        switch (themeType)
        {
            case ThemeType.BlackAndWhite:
                SetThemeColor(ThemeItem.Player, Color.black);
                SetThemeColor(ThemeItem.BasicBall, Color.black);
                SetThemeColor(ThemeItem.MidPrediction, Color.black);
                SetThemeColor(ThemeItem.EndPrediction, Color.black);

                BrickTextColor = new Color32(255, 255, 255, 255);
                SetThemeColor(ThemeItem.MaxDamage, Color.black);
                SetThemeColor(ThemeItem.MinDamage, ConvertToColor(55, 55, 55));
                SetThemeColor(ThemeItem.PlayerMaxHealth, Color.black);
                SetThemeColor(ThemeItem.PlayerMinHealth, Color.black);

                SetThemeColor(ThemeItem.SuperBackground, ConvertToColor(0x2c, 0x2c, 0x2c));
                SetThemeColor(ThemeItem.Background, ConvertToColor(0x5a, 0x5a, 0x5a));
                SetThemeColor(ThemeItem.GameboardBackground, ConvertToColor(0x5a, 0x5a, 0x5a));
                SetThemeColor(ThemeItem.GameboardBorder, Color.white);

                SetThemeColor(ThemeItem.Button, GetColor(CustomColor.Brown));

                NormalDmgBlink = new Color(1, 1, 1, 1);
                FireDmgBlink = new Color(0.35f, 0, 1f, 1);
                ExtraFireDmgBlink = new Color(1, 0, 0, 1);
                NormalBlinkStrength = 3f;
                FireBlinkStrength = 12f;
                ExtraFireBlinkStrength = 4f;
                ThemeBorderBrightness = 36f;
                break;

            case ThemeType.JellyFish:
                SetThemeColor(ThemeItem.Player, ConvertToColor(0x3e, 0xa1, 0xb6)); // moonstone
                SetThemeColor(ThemeItem.BasicBall, ConvertToColor(0x3e, 0xa1, 0xb6));
                SetThemeColor(ThemeItem.MidPrediction, ConvertToColor(0x3e, 0xa1, 0xb6)); // Dark blue grey
                SetThemeColor(ThemeItem.EndPrediction, ConvertToColor(0x3e, 0xa1, 0xb6));

                SetThemeColor(ThemeItem.MaxDamage, ConvertToColor(187, 144, 200)); // lenurple
                SetThemeColor(ThemeItem.MinDamage, ConvertToColor(239, 216, 236)); // piggy pink
                SetThemeColor(ThemeItem.PlayerMaxHealth, ConvertToColor(187, 144, 200));
                SetThemeColor(ThemeItem.PlayerMinHealth, ConvertToColor(239, 216, 236));

                SetThemeColor(ThemeItem.SuperBackground, ConvertToColor(9, 28, 42));
                SetThemeColor(ThemeItem.Background, ConvertToColor(56, 67, 123));
                SetThemeColor(ThemeItem.GameboardBackground, ConvertToColor(70, 90, 165));
                SetThemeColor(ThemeItem.GameboardBorder, ConvertToColor(0x3e, 0xa1, 0xb6));

                SetThemeColor(ThemeItem.Button, GetColor(CustomColor.Brown));

                PlayerBrightness = 3.5f;
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
                SetThemeColor(ThemeItem.SuperBackground, ConvertToColor(1, 200, 200));
                SetThemeColor(ThemeItem.GameboardBackground, ConvertToColor(75, 72, 130));
                SetThemeColor(ThemeItem.GameboardBorder, ConvertToColor(75, 72, 130));
                break;
            case ThemeType.VaporWave:
                SetThemeColor(ThemeItem.Player, ConvertToColor(255, 113, 206));
                SetThemeColor(ThemeItem.MidPrediction, ConvertToColor(255, 113, 206));
                SetThemeColor(ThemeItem.EndPrediction, ConvertToColor(255, 113, 206));
                SetThemeColor(ThemeItem.BasicBall, ConvertToColor(255, 113, 206));

                BrickTextColor = new Color32(255, 113, 206, 255);
                SetThemeColor(ThemeItem.MaxDamage, ConvertToColor(5, 255, 161));
                SetThemeColor(ThemeItem.MinDamage, ConvertToColor(255, 251, 150));
                SetThemeColor(ThemeItem.PlayerMaxHealth, ConvertToColor(5, 255, 161));
                SetThemeColor(ThemeItem.PlayerMinHealth, ConvertToColor(255, 251, 150));

                SetThemeColor(ThemeItem.Background, ConvertToColor(1, 205, 254));
                SetThemeColor(ThemeItem.SuperBackground, ConvertToColor(1, 144, 178));
                // SetThemeColor(ThemeItem.GameBorder, ConvertToColor(255, 113, 206));
                SetThemeColor(ThemeItem.GameboardBackground, ConvertToColor(1, 205, 254));
                SetThemeColor(ThemeItem.GameboardBorder, ConvertToColor(255, 113, 206));
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
                SetThemeColor(ThemeItem.GameboardBackground, ConvertToColor(75, 72, 130));
                SetThemeColor(ThemeItem.GameboardBorder, ConvertToColor(5, 255, 161));

                ThemeBorderBrightness = 48f;
                break;
            case ThemeType.Retro:
                BrickTextColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
                SetThemeColor(ThemeItem.MaxDamage, ConvertToColor(0x5E, 0xBD, 0x3E));
                SetThemeColor(ThemeItem.MinDamage, ConvertToColor(0xAD, 0xDF, 0x9C));
                SetThemeColor(ThemeItem.PlayerMaxHealth, ConvertToColor(0x00, 0x9C, 0xDF));
                SetThemeColor(ThemeItem.PlayerMinHealth, ConvertToColor(0x00, 0x4E, 0x6F));
                SetThemeColor(ThemeItem.InvincibleBrick, new Color(1, 1, 1));

                SetThemeColor(ThemeItem.Player, ConvertToColor(0xF7, 0x82, 0x00));
                SetThemeColor(ThemeItem.BasicBall, ConvertToColor(0xF7, 0x82, 0x00));
                SetThemeColor(ThemeItem.MidPrediction, ConvertToColor(0xF7, 0x82, 0x00));
                SetThemeColor(ThemeItem.EndPrediction, ConvertToColor(0xF7, 0x82, 0x00));

                SetThemeColor(ThemeItem.GameboardBackground, ConvertToColor(0x00, 0x9C, 0xDF));
                SetThemeColor(ThemeItem.SuperBackground, ConvertToColor(0x5E, 0xBD, 0x3E));
                SetThemeColor(ThemeItem.Background, ConvertToColor(0x00, 0x9C, 0xDF));
                SetThemeColor(ThemeItem.GameboardBorder, ConvertToColor(0xF7, 0x82, 0x00));

                ThemeFontColor = new Color32(0xFF, 0xFF, 0xFF, 0xff);
                ThemeButtonImageColor = new Color32(0xFF, 0xFF, 0xFF, 0xff);
                break;
        }

        // override settings from options
        if (PlayerPrefs.GetInt(ToggleHDR.HDR_ENABLED_KEY, 1) == 0) // if disabled
        {
            PlayerBrightness = 0;
        }
    }

    public static void RefreshTheme()
    {
        SetThemeType(ThemeType);
    }

    private static void SetThemeColor(ThemeItem themeItem, Color color)
    {
        ThemeColors[themeItem] = color;
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

    public static float GetColorAverage(Color color)
    {
        return (color.r + color.g + color.b) / 3f;;
    }

    public static bool IsLight(Color color)
    {
        return GetColorAverage(color) > 0.35f;
    }


}

