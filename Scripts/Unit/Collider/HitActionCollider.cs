using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class HitActionCollider : MonoBehaviour
    {
        public DamageValue DamageValue;

        private void OnCollisionEnter(Collision collision)
        {
            OnHit(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnHit(other.gameObject);
        }

        public void OnHit(GameObject hit)
        {
            if(hit.TryGetComponent<UnitActionLoader>(out var hitActionLoader))
            {
                hitActionLoader.LoadAction(DamageValue.DamageAction);
            }
        }
    }

}

