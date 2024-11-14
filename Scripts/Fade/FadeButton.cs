using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeButton : MonoBehaviour
{
    public void OnLoadSceneFadeOut(string sceneName)
    {
        FadeController.Instance.LoadSceneFadeout(sceneName);
    }
    public void OnReLoadSceneFadeOut()
    {
        FadeController.Instance.ReloadSceneFadeOut();
    }
}
