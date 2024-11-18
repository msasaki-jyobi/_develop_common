using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeButton : MonoBehaviour
{
    public void OnLoadSceneFadeOut(string sceneName)
    {
        FadeController.Instance.LoadSceneFadeIn(sceneName);
    }
    public void OnReLoadSceneFadeOut()
    {
        FadeController.Instance.ReloadSceneFadeIn();
    }
}
