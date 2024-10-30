using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace develop_common
{
    // EnemyHealth クラス
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private UnitActionLoader _unitActionLoader;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private AnimatorStateController _animatorStateController;

        [SerializeField]
        private EUnitType _unitType = EUnitType.Enemy;
        public EUnitType UnitType => _unitType;

        public ReactiveProperty<EUnitStatus> UnitStatus => new ReactiveProperty<EUnitStatus>();
        [field: SerializeField] public int CurrentHealth { get; private set; } = 10;
        public int MaxHealth { get; private set; } = 10;

        private void Start()
        {
            _unitActionLoader.FrameFouceEvent += OnFrameFouceHandle;
        }
        public void TakeDamage(DamageValue damageValue)
        {

            CurrentHealth -= damageValue.Amount * damageValue.WeightDiff;

            if (damageValue.DamageAction.TryGetComponent<ActionBase>(out var actionBase))
                if (actionBase.ActionStart != null)
                    if (_unitActionLoader != null)
                        _unitActionLoader.LoadAction(damageValue.DamageAction);

            if (CurrentHealth <= 0)
            {
                LogManager.Instance.AddLog(gameObject, "EnemyDead");
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
        private void OnFrameFouceHandle(Vector3 power)
        {
            if (_rigidBody != null)
                Knockback(power);
        }
        /// <summary>
        /// Velocity knockback
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="force"></param>
        public void Knockback(Vector3 direction)
        {
            // プレイヤーが向いている方向に合わせて力を加える
            Vector3 localForce = transform.forward * direction.z + transform.right * direction.x + transform.up * direction.y;
            // Rigidbody に力を加える (Impulse モードで瞬間的に力を加える)
            _rigidBody.AddForce(localForce, ForceMode.Impulse);
        }
    }
}

