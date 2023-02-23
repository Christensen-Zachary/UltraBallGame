using UnityEngine;
using UnityEngine.UI;

public class ThemeVisitor : MonoBehaviour
{
    [field: SerializeField]
    private ThemeData ThemeData { get; set; }

    public static void Visit(ThemeDimmer themeDimmer)
    {
        themeDimmer.GetComponent<Image>().color = ThemeData.ThemeColors[ThemeItem.SuperBackground];
    }

    public static void Visit(FirePowerup firePowerup)
    {
        ThemeItem themeItem = ThemeItem.FirePowerup1; // default to normal fire
        if (firePowerup.Damage > 2) // is strong fire
            themeItem = ThemeItem.FirePowerup2;
        
        firePowerup.PSGameObject = ThemeData.ThemeParticleSystems[themeItem];
        firePowerup.PSGameObject = Instantiate(firePowerup.PSGameObject);
        firePowerup.PSGameObject.transform.SetParent(firePowerup.transform);
        firePowerup.PSGameObject.transform.localPosition = Vector3.zero;
        firePowerup.PSGameObject.transform.localScale = Vector3.one;

        firePowerup.PSGameObjectToClone = ThemeData.ThemeParticleSystems[themeItem];

        firePowerup.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[themeItem];      
}

    public static void Visit(ThemeButtonImage themeButtonImage)
    {
        themeButtonImage.GetComponent<UnityEngine.UI.Image>().color = ThemeData.ThemeButtonImageColor;
        Color superBackgroundColor = ThemeData.ThemeColors[ThemeItem.SuperBackground];
        float average = (superBackgroundColor.r + superBackgroundColor.g + superBackgroundColor.b) / 3f;
        bool isLight =  average > 0.5f;
        // default to normal button
        Sprite sprite;
        if (isLight)
        {
            sprite = ThemeData.GetLightButton();
        }
        else
        {
            sprite = ThemeData.GetDarkButton();
        }

        // get ration of width to height
        float ratio = themeButtonImage.GetComponent<RectTransform>().sizeDelta.x / themeButtonImage.GetComponent<RectTransform>().sizeDelta.y;
        if (themeButtonImage.CompareTag("Slider"))
        {
            if (isLight)
            {
                sprite = ThemeData.GetExtraWideButtonLight();
            }
            else
            {
                sprite = ThemeData.GetExtraWideButtonDark();
            }
        }
        else if (themeButtonImage.CompareTag("OptionsButton"))
        {
            sprite = ThemeData.GetWideButton();
        }
        else if (Mathf.Approximately(ratio, 1))
        {
            if (isLight)
            {
                sprite = ThemeData.GetLightSquare();
            }
            else
            {
                sprite = ThemeData.GetDarkSquare();
            }
        }

        themeButtonImage.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
    }

    public static void Visit(ThemeFontColor themeFontColor)
    {
        themeFontColor.TextMesh.color = ThemeData.ThemeFontColor;
    }

    public static void Visit(InvincibleBrick invincibleBrick)
    {
        invincibleBrick.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.InvincibleBrick];
    }
    public static void Visit(BrickNumber brickNumber)
    {
        brickNumber.TextMesh.color = ThemeData.BrickTextColor;
    }

    public static void Visit(Damageable damageable)
    {
        damageable.MaxColor = ThemeData.ThemeColors[ThemeItem.MaxDamage];
        damageable.MinColor = ThemeData.ThemeColors[ThemeItem.MinDamage];
    }

    public static void Visit(Player player)
    {

        switch (ThemeData.ThemeType)
        {
            case ThemeType.Theme3:
                player.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Player/JackOLantern");
                break;
            default:
                player.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.Player];
                break;
        }
    }

    public static void Visit(Aim aim)
    {
        aim.EndPredictionSprite.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.EndPrediction];
        aim.MidPredictionSprite.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.MidPrediction];
    }


    public static void Visit(Shootable shootable)
    {
        shootable.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.BasicBall];

        GameObject psReturn = Resources.Load<GameObject>($"ParticleSystems/Balls/psReturn{(int)ThemeData.ThemeType}");
        if (psReturn == null)
        {
            psReturn = Resources.Load<GameObject>($"ParticleSystems/Balls/psReturn0");
        }
        psReturn = Instantiate(psReturn);
        psReturn.transform.SetParent(shootable.transform);
        psReturn.transform.localScale = Vector3.one;
        psReturn.transform.localPosition = new Vector3(0, 0, 1);
        shootable.PSReturn = psReturn.GetComponent<ParticleSystem>();
        psReturn.SetActive(true);
    }

    public static void Visit(PlayerHealth playerHealth)
    {
        playerHealth.MaxHealthColor = ThemeData.ThemeColors[ThemeItem.PlayerMaxHealth];
        playerHealth.MinHealthColor = ThemeData.ThemeColors[ThemeItem.PlayerMinHealth];
    }

    public static void Visit(Background background)
    {
        background.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.Background];
    }
    public static void Visit(SuperBackground superBackground)
    {
        if (superBackground.TryGetComponent(out SpriteRenderer sr)) // is in game
        {
            switch (ThemeData.ThemeType)
            {
                case ThemeType.JellyFish:
                    SetSpriteSuperBackground("Sprites/Background/bg4", superBackground.gameObject);
                    break;
                case ThemeType.VaporWave:
                    SetSpriteSuperBackground("Sprites/Background/bg1", superBackground.gameObject);
                    break;
                default:
                    sr.color = ThemeData.ThemeColors[ThemeItem.SuperBackground];
                    break;
            }
        }
        else if (superBackground.TryGetComponent(out Image image)) // is in UI
        {
            image.color = ThemeData.ThemeColors[ThemeItem.SuperBackground];
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
        if (ThemeData.ThemeFonts.ContainsKey(ThemeData.ThemeType)) 
        {
            themeText.SetFont(ThemeData.ThemeFonts[ThemeData.ThemeType]);
        }
        else themeText.SetFont(ThemeData.ThemeFonts[ThemeType.Default]);
    }

    public static void Visit(ThemeGameBorder themeGameBorder)
    {
        themeGameBorder.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.GameBorder];
        themeGameBorder.GetComponent<SpriteRenderer>().sortingOrder = -101;
    }

}
