using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "FrameNormalData", menuName = "develop_common / FrameInfo / NormalData")]
    public class FrameNormalData : ScriptableObject
    {
        public bool IsResetVelocity;
        public bool IsForce;
        public Vector3 ForcePower;
        public bool IsKey;
        public bool IsSetEntityParent;
    }
}
