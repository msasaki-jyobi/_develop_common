using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShot : MonoBehaviour
{
    public GameObject ShotPrefab;
    public float LifeTime = 100f;
    public float ShotSpan = 1f;

    private Camera _camera;
    private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (Input.GetMouseButton(1))
        {
            if (_timer > ShotSpan)
            {
                _timer = 0;
                var shot = Instantiate(ShotPrefab, transform.position, _camera.transform.rotation);
                Destroy(shot, LifeTime);
            }
        }
    }
}
