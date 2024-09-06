using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSet.Common
{
    // EnemyHealth クラス
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private AnimatorStateController _animatorStateController;

        [SerializeField]
        private EUnitType unitType = EUnitType.Enemy;
        public EUnitType UnitType => unitType;
        [field: SerializeField] public float CurrentHealth { get; private set; } = 50f;

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
                // 敵が死亡する処理
            }
        }

        public void Heal(float amount)
        {
            CurrentHealth += amount;
        }
    }
}

