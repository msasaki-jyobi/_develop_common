using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "FramePrefabData", menuName = "develop_common / FrameInfo / FramePrefabData")]
    public class FramePrefabData : ScriptableObject
    {
        public PrefabData PrefabData;
    }
}
