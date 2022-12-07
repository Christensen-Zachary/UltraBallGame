﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThemeData : MonoBehaviour
{
    [field: SerializeField]
    public TMPro.TMP_FontAsset Font1 { get; set; } // set in prefab
    [field: SerializeField]
    public TMPro.TMP_FontAsset Font2 { get; set; } // set in prefab
    [field: SerializeField]
    public TMPro.TMP_FontAsset Font3 { get; set; } // set in prefab

    public static ThemeType themeType;

    private void Awake()
    {
        themeType = ES3.Load<ThemeType>(BGStrings.ES_THEMETYPE, ThemeType.Default);
        SetThemeType(themeType);

        ThemeFonts = new Dictionary<ThemeType, TMPro.TMP_FontAsset>()
        {
            { ThemeType.Default, Font1 },
            { ThemeType.JellyFish, Font2 },
            { ThemeType.Retro, Font3 }
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
            { ThemeItem.PlayerMaxHealth, GetColor(CustomColor.DarkGreen) },
            { ThemeItem.PlayerMinHealth, GetColor(CustomColor.LightGreen) },
            { ThemeItem.MidPrediction, GetColor(CustomColor.LightYellow) },
            { ThemeItem.EndPrediction, GetColor(CustomColor.LightYellow) },
            { ThemeItem.BasicBall, GetColor(CustomColor.Orange) },
            { ThemeItem.SuperBackground, ConvertToColor(25, 25, 25) },
            { ThemeItem.InvincibleBrick, Color.white },
            { ThemeItem.Button, GetColor(CustomColor.Brown) },
            { ThemeItem.GameBorder, ConvertToColor(45, 45, 45) }
        };

        switch (themeType)
        {
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
                SetThemeColor(ThemeItem.GameBorder, ConvertToColor(70, 90, 165));

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
                SetThemeColor(ThemeItem.SuperBackground, ConvertToColor(1, 200, 200));
                SetThemeColor(ThemeItem.GameBorder, ConvertToColor(75, 72, 130));
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
                SetThemeColor(ThemeItem.GameBorder, ConvertToColor(1, 205, 254));
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
                SetThemeColor(ThemeItem.GameBorder, ConvertToColor(75, 72, 130));
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

                SetThemeColor(ThemeItem.GameBorder, ConvertToColor(0x00, 0x9C, 0xDF));
                SetThemeColor(ThemeItem.SuperBackground, ConvertToColor(0x5E, 0xBD, 0x3E));
                SetThemeColor(ThemeItem.Background, ConvertToColor(0x00, 0x9C, 0xDF));

                ThemeFontColor = new Color32(0xFF, 0xFF, 0xFF, 0xff);
                ThemeButtonImageColor = new Color32(0xFF, 0xFF, 0xFF, 0xff);
                break;
        }
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


}
