using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace develop_common
{
    // EnemyHealth ƒNƒ‰ƒX
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private AnimatorStateController _animatorStateController;

        [SerializeField]
        private EUnitType _unitType = EUnitType.Enemy;
        public EUnitType UnitType => _unitType;

        public ReactiveProperty<EUnitStatus> UnitStatus => new ReactiveProperty<EUnitStatus>();
        [field: SerializeField] public int CurrentHealth { get; private set; } = 10;
        public int MaxHealth { get; private set; } = 10;


        public void TakeDamage(DamageValue damageValue)
        {

            CurrentHealth -= damageValue.Amount * damageValue.WeightDiff;

            if (damageValue.DamageAction.TryGetComponent<ActionBase>(out var actionBase))
            {
                if(actionBase.ActionStart != null) 
                {
                    var motionName = actionBase.ActionStart.MotionName;
                    var statePlayType = actionBase.ActionStart.StatePlayType;
                    var isStateReset = actionBase.ActionStart.IsStateReset;
                    var isApplyRootMotion = actionBase.ActionStart.IsApplyRootMotion;

                    _animatorStateController.StatePlay(motionName, statePlayType, isStateReset, isApplyRootMotion);
                }

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

        public void ChangeStatus(EUnitStatus status)
        {
            throw new System.NotImplementedException();
        }
    }
}

