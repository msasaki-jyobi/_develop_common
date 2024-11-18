using Cinemachine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : SingletonMonoBehaviour<StartManager>
{
    private CinemachineBrain _brain;
    public List<StartCamera> StartCameras = new List<StartCamera>();

    private FadeController _fadeController;

    public event Action StartFinishEvent;
    async void Start()
    {
        _fadeController = FadeController.Instance;
        _brain = Camera.main.GetComponent<CinemachineBrain>();

        if (_fadeController)
        {
            _fadeController.FadeIn();
            await UniTask.Delay((int)(_fadeController.FadeInTime * 1000));
        }
        // Player‚Ì‰ŠúˆÊ’u‚ğİ’è
        var player = GameObject.Find("Player");
        if (player != null)
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
        }

        await UniTask.Delay(10);
        foreach (var st in StartCameras)
        {
            _brain.m_DefaultBlend.m_Time = st.DelayTime;
            develop_easymovie.CameraManager.Instance.ChangeActiveCamera(st.VCam);
            await UniTask.Delay((int)(1000 * st.DelayTime));
        }
        await UniTask.Delay(10);
        if (_fadeController)
        {
            _fadeController.FadeIn();
            await UniTask.Delay((int)(_fadeController.FadeInTime * 1000));
        }
        _brain.m_DefaultBlend.m_Time = 0f;
        develop_easymovie.CameraManager.Instance.SetDefaultCamera(false);

        StartFinishEvent?.Invoke(); // StartController:AIATpsController‚Ì‘€ì‰ğœ‚È‚Ç
    }
}

[Serializable]
public class StartCamera
{
    public float BlendTime;
    public CinemachineVirtualCamera VCam;
    [Space(8)]
    public float DelayTime;
}
