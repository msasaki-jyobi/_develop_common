using _develop_common;
using Cysharp.Threading.Tasks;
using develop_body;
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
    // PlayerHealth �N���X
    public class UnitHealth : MonoBehaviour, IHealth
    {
        [Header("Player Only")]
        [SerializeField] private develop_tps.InputReader _inputReader;
        [Header("Player and Enemy")]
        [SerializeField] private UnitComponents _unitComponents;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private AnimatorStateController _animatorStateController;
        [Header("Player and Enemy")]
        public List<string> DeadMotions = new List<string>() { "Dead", };

        public GameObject GetUpAction;

        [SerializeField]
        private EUnitType _unitType = EUnitType.Player;
        public EUnitType UnitType => _unitType;

        public ReactiveProperty<EUnitStatus> UnitStatus => new ReactiveProperty<EUnitStatus>();

        [field: SerializeField] public int CurrentHealth { get; private set; } = 50;
        public int MaxHealth { get; private set; } = 50;

        [field: SerializeField] public bool IsInvisible { get; private set; }

        public bool IsDeadSlow;
        public float SlowTimer = 1;
        [SerializeField] private List<ShakeController> Shakes = new List<ShakeController>();

        public event Action DamageActionEvent;
        private bool _initDead;


        private void Start()
        {
            _unitComponents.UnitActionLoader.FrameFouceEvent += OnFrameFouceHandle;
            if (_inputReader != null)
                _inputReader.PrimaryActionCrossEvent += OnCrossHandle;
        }

        private void Update()
        {
            //if (SlowTimer > 0)
            //{
            //    Time.timeScale = 0.2f;
            //    SlowTimer -= Time.unscaledDeltaTime;
            //}
            //else if (Time.timeScale != 1)
            //{
            //    Time.timeScale = 1f;
            //}
        }

        private async void OnCrossHandle(bool arg1, EInputReader reader)
        {
            if (CurrentHealth <= 0) return;
            // �N���オ��@�����GetUp�̃A�N�V�����ɂ�����������΂����񂶂�ˁH�����`�F�b�N��IsDown��True��DownValue��0�Ȃ炱��݂�����
            if (_unitComponents.PartAttachment.IsPull || (_unitComponents.PartAttachment.IsDown && _unitComponents.UnitActionLoader.UnitStatus.Value == EUnitStatus.Down))
            {
                // �_�E�����Ԃ���0.5�����Ȃ�Return
                if (_unitComponents.UnitActionLoader.DownTimer <= _unitComponents.UnitActionLoader.DownNoneActionTime) return;

                _unitComponents.PartAttachment.SetEntityParent();
                await UniTask.Delay(1);
                _unitComponents.PartAttachment.SetEntityParent(); // �Ȃ��������ł��Ă΂Ȃ��Ɛe�I�u�W�F�N�g��������Ȃ�
                _rigidBody.isKinematic = false;
                // �p�x���C��
                Vector3 rot = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(0, rot.y, 0);
                // �N���オ�胂�[�V���������s
                _unitComponents.UnitActionLoader.LoadAction(GetUpAction);
                IsInvisible = true;
                await UniTask.Delay(3000);
                IsInvisible = false;
            }
        }

        public async void TakeDamage(GameObject damageAction, bool isPull, int totalDamage, bool InitRandomCamera = false, List<string> bodyNames = default)
        {
            //if (_unitComponents.UnitActionLoader.DownTimer >= 5f) // ���G���Ԓ��Ȃ�Return 
            //    return;

            DamageActionEvent?.Invoke();

            CurrentHealth -= totalDamage;

            foreach (var shake in Shakes)
                shake.ShakeActionMove();

            if (CurrentHealth < 0)
            {
                if (IsDeadSlow)
                    SlowTimer = 3f;

                // �����オ���Ă�����Dead���[�V����
                if (_unitComponents.AnimatorStateController.MainStateName.Value == "Locomotion")
                {
                    _rigidBody.velocity = Vector3.zero;
                    string deadMotion = DeadMotions[UnityEngine.Random.Range(0, DeadMotions.Count)];
                    _unitComponents.UnitActionLoader.UnitStatus.Value = EUnitStatus.Executing;
                    _unitComponents.AnimatorStateController.StatePlay(deadMotion, EStatePlayType.SinglePlay, true);

                    // Infinity�̃_���[�W ���X�g���烉���_�����ł���������
                }
                // �G���U����Locomotion����Ȃ��̂Ŏ��ȂȂ�����
                if (_unitComponents.UnitActionLoader.NotHumanoid)
                {
                    if (_unitComponents.AnimatorStateController.MainStateName.Value != "Dead") // Generic ���S���[�V�������s�[�g�h�~
                    {
                        _unitComponents.UnitActionLoader.UnitStatus.Value = EUnitStatus.Executing;
                        _unitComponents.AnimatorStateController.StatePlay("Dead", EStatePlayType.SinglePlay, true);
                    }
                }
            }
      



            // Task: �_���[�W���󂯎���ă��[�V�����Đ����s���E�V�F�C�v�Đ��Ȃ�
            if (damageAction.TryGetComponent<ActionBase>(out var actionBase))
            {
                //if (actionBase.ActionDamageData.DamageType == EDamageType.Additive)
                //    _animatorStateController.AnimatorLayerPlay(1, actionBase.ActionStart.PlayClip, 0f);


                var additiveMotion = "";
                int ran = 0;
                if (actionBase.ActionStart != null)
                    additiveMotion = actionBase.ActionStart.PlayClip; // ��������Start���[�V������ݒ�
                if (actionBase.ActionDamageData.AdditiveDamageData != null)
                    ran = UnityEngine.Random.Range(0, actionBase.ActionDamageData.AdditiveDamageData.AdditiveDatas.Count);
                // Additive�f�[�^������Ώ㏑��
                if (actionBase.ActionDamageData != null)
                    if (actionBase.ActionDamageData.AdditiveDamageData != null)
                        additiveMotion = actionBase.ActionDamageData.AdditiveDamageData.AdditiveDatas[ran];

                if (actionBase.ActionDamageData.IsAddAddtive)
                {
                    _animatorStateController.AnimatorLayerPlay(1, additiveMotion, 0f);
                }
                if (actionBase.ActionDamageData.DamageVoiceKey != "")
                {
                    if (_unitComponents.UnitVoice != null)
                        _unitComponents.UnitVoice.PlayVoice(actionBase.ActionDamageData.DamageVoiceKey);
                }

                if (!isPull) // �Œ艻���[�V�����ȊO���Đ��̏ꍇ
                {
                    // Additive���l������K�v�����邪�A�Ƃ肠�������������K�v�����鐁����΂�
                    _rigidBody.isKinematic = false;
                    _unitComponents.PartAttachment.SetEntityParent();
                    //Vector3 rot = transform.rotation.eulerAngles;
                    //transform.rotation = Quaternion.Euler(0, rot.y, 0);

                    // �O���b�v���[�V�����̏ꍇ�@���W�Ɖ�]�l���l������K�v������ or �O���b�v���[�V������Pull�Ƃ��Ĉ���
                    if (actionBase.ActionDamageData.IsAddAddtiveOnly) return;

                    if (CurrentHealth > 0)
                    {
                        // �m�[�}�����[�V�����E�O���b�v���[�V�������Đ�
                        _unitComponents.UnitActionLoader.LoadAction(damageAction);
                    }
                    else // ���S�����ꍇ
                    {
                        _rigidBody.velocity = Vector3.zero;

                        // �ʃ��[�V�����Đ����Ȃ�Additive�̂�
                        _animatorStateController.AnimatorLayerPlay(1, $"Additive Damage0{UnityEngine.Random.Range(0, 3)}", 0f);

                        // �����オ���Ă�����Dead���[�V����
                        if (_unitComponents.AnimatorStateController.MainStateName.Value == "Locomotion")
                        {
                            if (actionBase.ActionDamageData.DeadAction != null) // ���S�f�[�^������ꍇ
                                _unitComponents.UnitActionLoader.LoadAction(actionBase.ActionDamageData.DeadAction);
                            else // DeadMotion���Ȃ��ꍇ
                            {
                                string deadMotion = DeadMotions[UnityEngine.Random.Range(0, DeadMotions.Count)];
                                _unitComponents.UnitActionLoader.UnitStatus.Value = EUnitStatus.Executing;
                                _unitComponents.AnimatorStateController.StatePlay(deadMotion, EStatePlayType.SinglePlay, true);
                            }
                        }
                    }

                }
                else // �Œ艻���[�V�������Đ��̏ꍇ
                {
                    if (!_unitComponents.PartAttachment.IsPull) // �܂��Œ肳��Ă��Ȃ�
                    {
                        _rigidBody.isKinematic = true;
                        _unitComponents.UnitActionLoader.UnitStatus.Value = EUnitStatus.Executing;
                        _unitComponents.AnimatorStateController.StatePlay(actionBase.ActionStart.PlayClip, EStatePlayType.SinglePlay, true);
                        // HitCollider�F�����_���J�����Ȃ�
                        if (InitRandomCamera)
                        {
                            PairManager.Instance.PlayRandomCamera(_unitComponents);
                        }
                    }
                    else // ���łɌŒ艻�ς�
                    {
                        _animatorStateController.AnimatorLayerPlay(1, additiveMotion, 0);
                    }

                    // �Œ艻 ���S
                    if (CurrentHealth <= 0)
                    {
                        if (_unitComponents.UnitVoice != null)
                            if (!_initDead)
                            {
                                _initDead = true;
                                _unitComponents.UnitVoice.PlayVoice("Dead", true);
                            }
                    }
                }
            }

            var unitShape = _unitComponents.UnitShape;
            if (unitShape != null)
            {
                ShapeWordData word = new ShapeWordData();
                word.WordData = "Da";
                word.NotWardData = new List<string>() { "�ʏ�" };
                if (CurrentHealth <= 0)
                    word.NotWardData.Add("�J");
                unitShape.SetShapeWard(word);
            }

            if (CurrentHealth <= 0)
            {
                // �v���C���[�����S���鏈��
                LogManager.Instance.AddLog(gameObject, "PlayerDead", 1);
            }
        }

        public void Heal(float amount)
        {
            CurrentHealth += (int)amount;
            // �ő�l�𒴂��Ȃ��悤�ɂ���
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
            // �v���C���[�������Ă�������ɍ��킹�ė͂�������
            Vector3 localForce = transform.forward * direction.z + transform.right * direction.x + transform.up * direction.y;
            // Rigidbody �ɗ͂������� (Impulse ���[�h�ŏu�ԓI�ɗ͂�������)
            _rigidBody.AddForce(localForce, ForceMode.Impulse);
        }
    }
}