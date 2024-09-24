using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class LoopRotate : MonoBehaviour
    {
        public Vector3 RotatePower;

        private void Update()
        {
            transform.Rotate(RotatePower * Time.deltaTime);
        }
    }
}
