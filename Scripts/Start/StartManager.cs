using Cinemachine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    private CinemachineBrain _brain;
    public List<StartCamera> StartCameras = new List<StartCamera>();
    async void Start()
    {
        var player = GameObject.Find("Player");
        if(player != null)
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
        }

        _brain = Camera.main.GetComponent<CinemachineBrain>();  

        foreach (var st in StartCameras)
        {
            await UniTask.Delay((int)(1000 * st.SetTime));
            _brain.m_DefaultBlend.m_Time = st.SetTime;
            develop_easymovie.CameraManager.Instance.ChangeActiveCamera(st.VCam);
        }
        await UniTask.Delay(10);
        _brain.m_DefaultBlend.m_Time = 0f;
        develop_easymovie.CameraManager.Instance.SetDefaultCamera(false);
    }
}

[Serializable]
public class StartCamera
{
    public float SetTime;
    public float BlendTime;
    public CinemachineVirtualCamera VCam;
}
