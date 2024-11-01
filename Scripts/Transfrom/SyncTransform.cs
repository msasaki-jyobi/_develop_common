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

    public bool IsTargetLook;
    public Transform Origin;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SyncTarget == null) return;

        Vector3 pos = transform.position;
        if (IsPosX) pos.x = SyncTarget.transform.position.x;
        if (IsPosY) pos.y = SyncTarget.transform.position.y;
        if (IsPosZ) pos.z = SyncTarget.transform.position.z;

        Vector3 rot = transform.rotation.eulerAngles;
        if (IsRotX) rot.x = SyncTarget.transform.rotation.x;
        if (IsRotY) rot.y = SyncTarget.transform.rotation.y;
        if (IsRotZ) rot.z = SyncTarget.transform.rotation.z;

        transform.position = pos;

        if (!IsTargetLook)
            transform.rotation = Quaternion.Euler(rot);
        else
        {
            if (Origin != null)
            {
                // AからBへの方向ベクトルを取得
                Vector3 direction = (SyncTarget.transform.position - Origin.transform.position).normalized;
                // Bのforwardに方向ベクトルをそのまま反映
                transform.transform.forward = direction;
            }
        }



    }
}
