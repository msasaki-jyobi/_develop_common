using _develop_common;
using Cysharp.Threading.Tasks;
using develop_tps;
using System;
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
        [SerializeField] private develop_tps.InputReader _inputReader;
        [SerializeField] private UnitComponents _unitComponents;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private AnimatorStateController _animatorStateController;
        public GameObject GetUpAction;

        [SerializeField]
        private EUnitType _unitType = EUnitType.Player;
        public EUnitType UnitType => _unitType;

        public ReactiveProperty<EUnitStatus> UnitStatus => new ReactiveProperty<EUnitStatus>();

        [field: SerializeField] public int CurrentHealth { get; private set; } = 50;
        public int MaxHealth { get; private set; } = 50;



        private void Start()
        {
            _unitComponents.UnitActionLoader.FrameFouceEvent += OnFrameFouceHandle;
            _inputReader.PrimaryActionCrossEvent += OnCrossHandle;
        }

        private async void OnCrossHandle(bool arg1, EInputReader reader)
        {
            if (_unitComponents.PartAttachment.IsPull || _unitComponents.PartAttachment.IsDown)
            {
                _unitComponents.PartAttachment.SetEntityParent();
                await UniTask.Delay(1);
                _unitComponents.PartAttachment.SetEntityParent(); // なぜかここでも呼ばないと親オブジェクト解除されない
                _rigidBody.isKinematic = false;
                // 角度を修正
                Vector3 rot = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(0, rot.y, 0);
                // 起き上がりモーションを実行
                _unitComponents.UnitActionLoader.LoadAction(GetUpAction);
            }
        }

        public async void TakeDamage(GameObject damageAction, bool isPull, int totalDamage)
        {
            CurrentHealth -= totalDamage;

            // Task: ダメージを受け取ってモーション再生を行う・シェイプ再生など

            if (!isPull) // 固定化モーション以外を再生の場合
            {
                // Additiveを考慮する必要があるが、とりあえず引き離す必要がある吹き飛ばす
                _rigidBody.isKinematic = false;
                _unitComponents.PartAttachment.SetEntityParent();
                //Vector3 rot = transform.rotation.eulerAngles;
                //transform.rotation = Quaternion.Euler(0, rot.y, 0);

                // グラップモーションの場合　座標と回転値を考慮する必要がある　or グラップモーションをPullとして扱う

                // ノーマルモーション・グラップモーションを再生
                _unitComponents.UnitActionLoader.LoadAction(damageAction);
                // Additiveモーションを再生パターン

            }
            else // 固定化モーションを再生の場合
            {
                if(!_unitComponents.PartAttachment.IsPull)
                {
                    _rigidBody.isKinematic = true;
                    _unitComponents.UnitActionLoader.UnitStatus = EUnitStatus.Executing;
                    //_unitComponents.AnimatorStateController.StatePlay(hitCollider.PullData.MotionName, EStatePlayType.SinglePlay, true);
                }
                else // すでに固定化済み
                {
                    _animatorStateController.AnimatorLayerPlay(1, "Additive1", 0);
                }

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
