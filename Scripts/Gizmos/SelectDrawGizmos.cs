using System.Collections;
using UnityEngine;

namespace develop_common
{

    public class SelectDrawGizmos : MonoBehaviour
    {
        [SerializeField] private float _size = 0.1f;
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, _size);
        }
    }
}