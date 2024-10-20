using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRandomScaler : MonoBehaviour
{
    public float min = 0.2f; 
    public float max = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        float ran = Random.Range(min, max);
        transform.localScale = Vector3.one * ran;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
