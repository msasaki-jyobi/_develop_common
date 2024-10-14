using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace develop_common
{
    public class NavMeshController : MonoBehaviour
    {
        public GameObject Target;

        public NavMeshAgent Agent;
        public GameObject Eye;
        public AnimatorStateController AnimatorStateController;
        public Animator Animator;

        [Header("Patrol Positions")]
        public List<GameObject> PatrolPositions = new List<GameObject>();

        [Header("AI Speed")]
        public float NormalSpeed = 3.5f;
        public float HuntSpeed = 3.5f;

        [Header("AI Parameter")]
        public bool IsDistance; // 指定距離以内が必要
        public float Distance = 5f;
        public bool IsLook; // 視認が必要
        public float RayHeight = 1f; // 視認が必要
        public bool IsAngle; // 角度が必要
        public float Angle = 45f; // 角度が必要
        public float LostHuntTime = 1f; // 見失うまでの時間

        [Header("AI Debug")]
        public bool IsDebug;

        private float _huntTimer;
        private ReactiveProperty<bool> _isHunt = new ReactiveProperty<bool>();
        private bool _isStop;
        private GameObject _patrolTarget;
        private bool _isPatrolCheck;

        private float distance;
        private void Start()
        {
            if (Target == null)
                Target = GameObject.Find("Player");

            // 半径10m以内の徘徊場所を自動取得

            _isHunt
                .Subscribe((x) =>
                {
                    if (_isStop) return;

                    if (AnimatorStateController != null)
                        if (AnimatorStateController.Animator != null)
                        {
                            var motionName = x ? "Run" : "Idle";
                            AnimatorStateController.StatePlay(motionName, EStatePlayType.Loop, false);
                        }

                });
        }

        private void Update()
        {
            if (_huntTimer > 0) _huntTimer -= Time.deltaTime;

            if (_isStop) return;
            Move();

        }

        private async UniTask Move()
        {
            if (Target == null || Agent == null) return;  // Target または Agent がない場合は何もしない

            bool check = true;
            if (IsDistance) // 距離をチェック
            {
                distance = AIFunction.TargetDistance(Target, gameObject);
                check = check && distance <= Distance;
            }
            if (IsLook) // 視認をチェック
                check = check &&
                    (AIFunction.GetRayTarget(Eye, Target, distance:Distance, rayHeight: RayHeight) == Target ||
                     AIFunction.GetRayTarget(Eye, Target, distance: Distance, rayHeight: RayHeight / 2) == Target ||
                     AIFunction.GetRayTarget(Eye, Target, distance: Distance, rayHeight: RayHeight / 4) == Target);
            if (IsAngle) // 角度をチェック
                check = check && Mathf.Abs(AIFunction.TargetAngle(Eye, Target)) <= Angle;

            if (IsDebug)
                Debug.Log($"Angle:{AIFunction.TargetAngle(Eye, Target)}");

            // ハント状態切替
            _isHunt.Value = check;
            if (check || _huntTimer > 0)
            {
                // ターゲットを追跡するための移動を開始
                Agent.speed = HuntSpeed;  // 追跡用のスピードに設定
                Agent.SetDestination(Target.transform.position);  // ターゲットに向かって移動
                if(check)
                    _huntTimer = LostHuntTime;
            }
            else
            {
                // ターゲットを追わない場合、通常の動作に戻る（必要であれば実装）
                Agent.speed = NormalSpeed;  // 通常の速度に戻す

                // 半径10m以内の探索場所に向かう
                if (_patrolTarget == null)
                {
                    if (PatrolPositions.Count > 0)
                    {
                        if (!_isPatrolCheck)
                        {
                            _isPatrolCheck = true;
                            await UniTask.Delay(1000 * Random.Range(0, 5));
                            _patrolTarget = PatrolPositions[Random.Range(0, PatrolPositions.Count)];
                            Agent.speed = NormalSpeed;
                            Agent.SetDestination(_patrolTarget.transform.position);  // ターゲットに向かって移動
                            _isPatrolCheck = false;
                        }
                    }
                }
                else
                {
                    var distance = Vector3.Distance(transform.position, _patrolTarget.transform.position);
                    if (distance <= 0.2f)
                        _patrolTarget = null;
                }
            }

            if (Animator != null)
                // 速度をAnimatorに反映する
                Animator?.SetFloat("Speed", GetAgentSpeed(), 0.1f, Time.deltaTime);
        }

        public void OnStopAgent()
        {
            _isStop = true;
            Agent.enabled = false;
        }

        public float GetAgentSpeed()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            Vector3 velocity = agent.velocity; // 現在の移動速度のベクトル
            return velocity.magnitude;  // 移動速度の大きさを取得
        }
    }
}
