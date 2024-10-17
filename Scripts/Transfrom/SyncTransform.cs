using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTransform : MonoBehaviour
{
    public GameObject SyncTarget;
    public bool IsPosX;
    public bool IsPosY;
    public bool IsPosZ;
    public bool IsRotX;
    public bool IsRotY;
    public bool IsRotZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if(IsPosX) pos.x = SyncTarget.transform.position.x;
        if(IsPosY) pos.y = SyncTarget.transform.position.y;
        if(IsPosZ) pos.z = SyncTarget.transform.position.z;

        Vector3 rot = transform.rotation.eulerAngles;
        if(IsRotX) rot.x = SyncTarget.transform.rotation.x;
        if(IsRotY) rot.y = SyncTarget.transform.rotation.y;
        if(IsRotZ) rot.z = SyncTarget.transform.rotation.z;

        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);
    }
}
