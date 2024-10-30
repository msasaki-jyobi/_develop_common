using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "ShapeData", menuName = "develop_common / ShapeData")]
    public class BlendShapeData : ScriptableObject
    {
        public List<BlandValue> BlendShapeList = new List<BlandValue>();
    }

    [Serializable]
    public class BlandValue
    {
        public string ShapeName;
        [Range(0, 100)]
        public int ShapeValue;
    }

}
