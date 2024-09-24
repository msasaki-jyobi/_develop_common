using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class HitActionCollider : MonoBehaviour
    {
        public GameObject DamageAction;

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
            if (hit.TryGetComponent<UnitActionLoader>(out var hitActionLoader))
            {
                if (DamageAction.TryGetComponent<ActionBase>(out var actionBase))
                {
                    // �_���[�W���o�����s����
                    if(actionBase.ActionDamageValue != null)
                        hitActionLoader.LoadAction(DamageAction);

                    var actionDamageValue = actionBase.ActionDamageValue;
                    if(actionDamageValue != null)
                    {
                        var damageValue = actionDamageValue.DamageValue;
                        // �_���[�W��
                        int damage = damageValue.Amount * damageValue.WeightDiff;
                    }
                    
                }
            }
        }
    }

}
