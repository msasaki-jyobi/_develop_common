using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : SingletonMonoBehaviour<SceneReloader>
{
    public bool IsReloadAlpha0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            if (IsReloadAlpha0)
                OnReloadScene();
    }

    public void OnReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnLoadNameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
