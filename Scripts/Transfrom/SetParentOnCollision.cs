using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace develop_common
{
    public class SetParentOnCollision : MonoBehaviour
    {
        public string TargetTagName = "Player"; 
        public GameObject ParentTarget;

        private void Start()
        {
            if (ParentTarget == null)
                ParentTarget = gameObject;
        }
        private void OnCollisionEnter(Collision collision)
        {
            OnHit(collision.gameObject);
        }
        private void OnTriggerEnter(Collider other)
        {
            OnHit(other.gameObject);
        }
        private void OnCollisionExit(Collision collision)
        {
            OnExit(collision.gameObject);
        }
        private void OnTriggerExit(Collider other)
        {
            OnExit(other.gameObject);
        }
        private void OnHit(GameObject hit)
        {
            if (hit.CompareTag(TargetTagName))
            {
                hit.transform.parent = ParentTarget.transform;
            }
        }
        private void OnExit(GameObject hit)
        {
            if (hit.CompareTag(TargetTagName))
            {
                hit.transform.parent = null;
            }
        }

    }
}