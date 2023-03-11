using Unity.VisualScripting;
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
        if (firePowerup.Damage > 10) // is strong fire
            themeItem = ThemeItem.FirePowerup2;
        
        firePowerup.PSGameObject = ThemeData.ThemeParticleSystems[themeItem];
        firePowerup.PSGameObject = Instantiate(firePowerup.PSGameObject);
        firePowerup.PSGameObject.transform.SetParent(firePowerup.transform);
        firePowerup.PSGameObject.transform.localPosition = Vector3.zero;
        firePowerup.PSGameObject.transform.localScale = Vector3.one;

        firePowerup.PSGameObjectToClone = ThemeData.ThemeParticleSystems[themeItem];

        firePowerup.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[themeItem];      
}

    public static void Visit(ThemeButtonImage themeButtonImage, ThemeButtonSize themeButtonSize)
    {
        Image image = themeButtonImage.GetComponent<UnityEngine.UI.Image>();
        SpriteRenderer sr = themeButtonImage.GetComponent<SpriteRenderer>();
        if (themeButtonSize == ThemeButtonSize.Wide) // blur menu button
        {
            if (image != null) image.color = new Color(1, 1, 1, 0.37f);
            if (sr != null) sr.color = new Color(1, 1, 1, 0.37f);
        }
        else if (themeButtonSize == ThemeButtonSize.Slider || themeButtonSize == ThemeButtonSize.SliderKnob)
        {
            if (image != null) image.color = Color.white;
            if (sr != null) sr.color = Color.white;
        }
        else if (ThemeData.IsLight(ThemeData.ThemeColors[ThemeItem.SuperBackground]))
        {
            float average = ThemeData.GetColorAverage(ThemeData.ThemeColors[ThemeItem.SuperBackground]) / 2f;
            if (image != null) image.color = new Color(1, 1, 1, average);//ThemeData.ThemeButtonImageColor;
            if (sr != null) sr.color = new Color(1, 1, 1, average);
        }
        else
        {
            float average = ThemeData.GetColorAverage(ThemeData.ThemeColors[ThemeItem.SuperBackground]);
            if (image != null) image.color = new Color(1, 1, 1, average);//ThemeData.ThemeButtonImageColor;
            if (sr != null) sr.color = new Color(1, 1, 1, average);
        }


        if (image != null) image.sprite = ThemeData.GetThemeButton(themeButtonSize);
        if (sr != null) sr.sprite = ThemeData.GetThemeButton(themeButtonSize);

        //themeButtonImage.GetComponent<UnityEngine.UI.Image>().material = Resources.Load<Material>("Materials/Themes/1/PowerButton");
        //themeButtonImage.GetComponent<UnityEngine.UI.Image>().material.SetColor("_GlowColor", ThemeData.ThemeColors[ThemeItem.GameboardBorder]);
    }

    public static void Visit(ThemeFontColor themeFontColor)
    {
        if (themeFontColor.TextMesh != null) themeFontColor.TextMesh.color = ThemeData.ThemeFontColor;
        if (themeFontColor.TextMeshOnGameObjects != null) themeFontColor.TextMeshOnGameObjects.color = ThemeData.ThemeFontColor;
    }

    public static void Visit(InvincibleBrick invincibleBrick)
    {
        invincibleBrick.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.InvincibleBrick];
    }
    public static void Visit(BrickNumber brickNumber)
    {
        brickNumber.TextMesh.color = ThemeData.BrickTextColor;
        // below is theme color but transparent
        // brickNumber.TextMesh.color = new Color32(ThemeData.BrickTextColor.r, ThemeData.BrickTextColor.g, ThemeData.BrickTextColor.b, 0);
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
                player.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", ThemeData.ThemeColors[ThemeItem.Player]);
                player.GetComponent<SpriteRenderer>().material.SetFloat("_Glow", ThemeData.PlayerBrightness);
                break;
        }
    }

    public static void Visit(Aim aim)
    {
        aim.EndPredictionSprite.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.EndPrediction];
        aim.EndPredictionSprite.GetComponent<SpriteRenderer>().sharedMaterial.SetColor("_GlowColor", ThemeData.ThemeColors[ThemeItem.EndPrediction]);
        aim.EndPredictionSprite.GetComponent<SpriteRenderer>().sharedMaterial.SetFloat("_Glow", ThemeData.PlayerBrightness);

        aim.MidPredictionSprite.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.MidPrediction];
        aim.MidPredictionSprite.GetComponent<SpriteRenderer>().sharedMaterial.SetColor("_GlowColor", ThemeData.ThemeColors[ThemeItem.MidPrediction]);
        aim.MidPredictionSprite.GetComponent<SpriteRenderer>().sharedMaterial.SetFloat("_Glow", ThemeData.PlayerBrightness);
    }


    public static void Visit(Shootable shootable)
    {
        shootable.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.BasicBall];
        shootable.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", ThemeData.ThemeColors[ThemeItem.BasicBall]);
        shootable.GetComponent<SpriteRenderer>().material.SetFloat("_Glow", ThemeData.PlayerBrightness);

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

    public static void Visit(ThemeGameboardBackground themeGameboardBackground)
    {
        themeGameboardBackground.GetComponent<SpriteRenderer>().color = ThemeData.ThemeColors[ThemeItem.GameboardBackground];
        themeGameboardBackground.GetComponent<SpriteRenderer>().sortingOrder = -101;
        themeGameboardBackground.GetComponent<SpriteRenderer>().material.SetVector("_OuterOutlineColor", ThemeData.ThemeBorderBrightness * ThemeData.ThemeColors[ThemeItem.GameboardBorder]);
    }

    

}
