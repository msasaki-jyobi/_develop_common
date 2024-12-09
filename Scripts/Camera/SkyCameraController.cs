using Cinemachine;
using develop_timeline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyCameraController : MonoBehaviour
{
    public float Speed = 1f;
    public float DashRange = 15f;
    public float MouseSpeed = 0.2f;
    public CinemachineVirtualCamera Vcam;
    [Header("Startéû é©ìÆê›íË")]
    public float DefaultSpeed;

    private CinemachinePOV _pov;
    private Camera _camera;

    private bool _isSky;

    void Start()
    {
        DefaultSpeed = Speed;
        _camera = Camera.main;
        _pov = Vcam.GetCinemachineComponent<CinemachinePOV>();
        _pov.m_HorizontalAxis.m_MaxSpeed = 0;
        _pov.m_VerticalAxis.m_MaxSpeed = 0;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            _isSky = !_isSky;
            CinemachineBrain.SoloCamera = _isSky ? Vcam : null; // É\ÉçÇON
        }

        if (Input.GetMouseButton(1))
        {
            _pov.m_HorizontalAxis.m_MaxSpeed = MouseSpeed;
            _pov.m_VerticalAxis.m_MaxSpeed = MouseSpeed;
        }
        else
        {
            _pov.m_HorizontalAxis.m_MaxSpeed = 0;
            _pov.m_VerticalAxis.m_MaxSpeed = 0;
        }

        if(_isSky)
        {
            Speed = Input.GetKey(KeyCode.LeftShift) ? DefaultSpeed * DashRange : DefaultSpeed;

            var right = Input.GetAxisRaw("Horizontal") * Speed;
            var up = 0f;
            var forward = Input.GetAxisRaw("Vertical") * Speed;

            //if (Input.GetKey(KeyCode.W))
            //    forward = Speed * Time.deltaTime;
            //if (Input.GetKey(KeyCode.S))
            //    forward = -Speed * Time.deltaTime;

            //if (Input.GetKey(KeyCode.D))
            //    right = Speed * Time.deltaTime;
            //if (Input.GetKey(KeyCode.A))
            //    right = -Speed * Time.deltaTime;

            if (Input.GetKey(KeyCode.E))
                up = Speed;
            if (Input.GetKey(KeyCode.Q))
                up = -Speed;

            if (Input.GetKey(KeyCode.Z))
                transform.position = DirectorManager.Instance.UnitAComponents.UnitInstance.SearchObject("Hips").transform.position;
                    // + DirectorManager.Instance.UnitAComponents.UnitInstance.SearchObject("Hips").transform.forward * -1f;

            transform.rotation = _camera.transform.rotation;
            transform.Translate(right * Time.unscaledDeltaTime, up * Time.unscaledDeltaTime, forward * Time.unscaledDeltaTime);
        }
    }
}
