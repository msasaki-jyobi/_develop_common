using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "LineData", menuName = "GameSet / LineData", order = 0)]
    public class LineData : ScriptableObject
    {
        public List<Vector3> StartPosition;
        public List<Vector3> EndPosition;
        public LayerMask LineLayer;
        public Color LineColor = Color.red;
    }
}
