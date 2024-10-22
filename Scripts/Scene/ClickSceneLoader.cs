using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickSceneLoader : MonoBehaviour
{
    public Image FadeImage;
    public int Duration = 1;
    public float TargetAlpha = 1;

    public string LoadSceneName;

    private bool _isLoad;
    void Start()
    {
        //InputReader.StartedFireEvent += OnFireHandle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async void OnFireHandle()
    {
        if (_isLoad) return;
        _isLoad = true;
        FadeImage.DOFade(TargetAlpha, Duration);
        await UniTask.Delay(Duration * 1000);
        SceneManager.LoadScene(LoadSceneName);
    }
}
