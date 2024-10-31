using _develop_common;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace develop_common
{
    public enum EUnitType
    {
        Enemy,
        Player,
        Other
    }
    // PlayerHealth クラス
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private UnitComponents _unitComponents;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private AnimatorStateController _animatorStateController;

        [SerializeField]
        private EUnitType _unitType = EUnitType.Player;
        public EUnitType UnitType => _unitType;

        public ReactiveProperty<EUnitStatus> UnitStatus => new ReactiveProperty<EUnitStatus>();

        [field: SerializeField] public int CurrentHealth { get; private set; } = 50;
        public int MaxHealth { get; private set; } = 50;

        private void Start()
        {
            _unitComponents.UnitActionLoader.FrameFouceEvent += OnFrameFouceHandle;
        }

        public async void TakeDamage(HitCollider hitCollider, int totalDamage)
        {
            CurrentHealth -= totalDamage;

            // ダメージモーション関連
            if(!hitCollider.IsPull)
            {
                // Additiveを考慮する必要があるが、とりあえず引き離す必要がある吹き飛ばす
                _rigidBody.isKinematic = false;
                transform.parent = null;
                Vector3 rot = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(0, rot.y, 0);

                // モーション再生を確定

                // ノーマルモーション・グラップモーションを再生
                _unitComponents.UnitActionLoader.LoadAction(hitCollider.DamageAction);
                // Additiveモーションを再生

            }
            else // 固定化モーションを再生
            {
                _rigidBody.isKinematic = true;
                _unitComponents.UnitActionLoader.UnitStatus = EUnitStatus.Executing;
                _unitComponents.AnimatorStateController.StatePlay(hitCollider.PullData.MotionName, EStatePlayType.Loop, true);

                // 仮実装：モーション終了時にすること Ready, Kinematic:False
                //await UniTask.Delay(2000);
                //_unitComponents.UnitActionLoader.UnitStatus = EUnitStatus.Ready;

            }

            var unitShape = _unitComponents.UnitShape;
            if (unitShape != null)
            {
                ShapeWordData word = new ShapeWordData();
                word.WordData = "Da";
                word.NotWardData = new List<string>() { "通常" };
                unitShape.SetShapeWard(word);
            }

            if (CurrentHealth <= 0)
            {
                // プレイヤーが死亡する処理
                LogManager.Instance.AddLog(gameObject, "PlayerDead", 1);
            }
        }

        public void Heal(float amount)
        {
            CurrentHealth += (int)amount;
            // 最大値を超えないようにする
        }

        public void ChangeStatus(EUnitStatus status)
        {

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
