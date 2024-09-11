using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    // EnemyHealth ƒNƒ‰ƒX
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private AnimatorStateController _animatorStateController;

        [SerializeField]
        private EUnitType unitType = EUnitType.Enemy;
        public EUnitType UnitType => unitType;
        [field: SerializeField] public int CurrentHealth { get; private set; } = 50;
        public int MaxHealth { get; private set; } = 50;

        public void TakeDamage(DamageValue damageValue)
        {

            CurrentHealth -= damageValue.Amount * damageValue.WeightDiff;

            if (damageValue.DamageAction.TryGetComponent<ActionBase>(out var actionBase))
            {
                _animatorStateController.ChangeMotion(actionBase.MotionName, actionBase.MotionLate, actionBase.StatePlayType, actionBase.StateReset, actionBase.ApplyRootMotion);
            }

            if (CurrentHealth <= 0)
            {
                Debug.Log("Enemy is dead");
                // “G‚ªŽ€–S‚·‚éˆ—
            }
        }

        public void Heal(float amount)
        {
            CurrentHealth += (int)amount;
        }
    }
}

