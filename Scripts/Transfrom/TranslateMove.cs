using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateMove : MonoBehaviour
{
    public Vector3 Power = new Vector3(0, 0, 15f);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Power * Time.deltaTime);
    }
}
