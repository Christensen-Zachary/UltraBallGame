using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemePreviewUI : MonoBehaviour
{
    private ThemeType _b4Theme = ThemeType.Default;

    public Animator animator;

    private void Awake()
    {
        _b4Theme = ES3.Load<ThemeType>(BGStrings.ES_THEMETYPEB4PREVIEW);
    }



    public void OpenThemeSelect()
    {
        StartCoroutine(OpenThemeSelectRoutine());
    }

    public IEnumerator OpenThemeSelectRoutine()
    {
        animator.SetTrigger("Close");

        yield return new WaitForSeconds(MainMenuUI.SCENE_TRANSITION_WAIT_TIME);

        ES3.Save(BGStrings.ES_THEMETYPE, _b4Theme);

        SceneManager.LoadScene("ThemeSelect");
    }
}
