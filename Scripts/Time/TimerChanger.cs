using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerChanger : MonoBehaviour
{
    public float XChangeSpeed = 0.5f;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Time.timeScale == XChangeSpeed)
                Time.timeScale = 1f;
            else
                Time.timeScale = XChangeSpeed;
        }
    }
}
