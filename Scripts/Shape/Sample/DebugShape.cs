using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class DebugShape : MonoBehaviour
    {
        // テンプレート
        public UnitShape UnitShape;
        public ShapeWordData ShapeWordDataStart;
        public ShapeWordData ShapeWordDataZ;
        public ShapeWordData ShapeWordDataX;
        public ShapeWordData ShapeWordDataC;

        private void Start()
        {
            if (ShapeWordDataStart != null)
                UnitShape.SetShapeWard(ShapeWordDataStart);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                UnitShape.SetShapeWard(ShapeWordDataZ);
            if (Input.GetKeyDown(KeyCode.X))
                UnitShape.SetShapeWard(ShapeWordDataX);
            if (Input.GetKeyDown(KeyCode.C))
                UnitShape.SetShapeWard(ShapeWordDataC);
        }
    }
}
