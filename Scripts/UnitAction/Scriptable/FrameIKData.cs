using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "FrameIK", menuName = "develop_common / FrameInfo / FrameIK")]
    public class FrameIKData : ScriptableObject
    {
        public string IKKeyName;
        public string IKTargetKeyName;
        public float IKLifeTime;
    }
}
