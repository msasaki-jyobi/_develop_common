using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncOffsetTransform : MonoBehaviour
{
    public Vector3 OffsetPos;
    public bool IsPosX;
    public bool IsPosY;
    public bool IsPosZ;

    public Vector3 OffsetRot;
    public bool IsRotX;
    public bool IsRotY;
    public bool IsRotZ;

    public bool IsTargetLook;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        if (IsPosX) pos.x = OffsetPos.x;
        if (IsPosY) pos.y = OffsetPos.y;
        if (IsPosZ) pos.z = OffsetPos.z;

        Vector3 rot = transform.localRotation.eulerAngles;
        if (IsRotX) rot.x = OffsetRot.x;
        if (IsRotY) rot.y = OffsetRot.y;
        if (IsRotZ) rot.z = OffsetRot.z;

        transform.localPosition = pos;
        transform.localRotation = Quaternion.Euler(rot);
    }
}
