using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class LoopRotate : MonoBehaviour
    {
        public Vector3 RotatePower;
        public bool IsRotate = true;

        private void Update()
        {
            if (IsRotate)
                transform.Rotate(RotatePower * Time.deltaTime);
        }

        public void OnChangeRotate(bool flg)
        {
            IsRotate = flg;
        }
    }
}
