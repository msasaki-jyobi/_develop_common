using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
namespace develop_common
{
    [Serializable]
    public class ThrowInfo
    {
        public string AtkStateName;
        public string DmgStateName;
        public List<int> DefaultDamageFrames;

        public Vector3 LocalPos;
        public Vector3 LocalRot;
        
        public bool AtkApplyRootMotion;
        public bool DmgApplyRootMotion;
        public int AtkLate = 30;
        public int DmgLate = 30;
    }
}
