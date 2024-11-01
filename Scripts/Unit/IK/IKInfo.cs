using DitzelGames.FastIK;
using System;
using System.Collections;
using UnityEngine;

namespace develop_common
{
    [Serializable]
    public class IKInfo
    {
        public string IKKeyName;
        public FastIKFabric IKFabric;
        public SyncTransform IKTarget;
    }
}