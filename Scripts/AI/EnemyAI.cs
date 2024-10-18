using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using develop_common;
using develop_tps;
using static Thry.AnimationParser;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking,
        Retreating,
        Fleeing,
        Staggered, // 怯み状態
        Dead       // 死亡状態
    }

    // 追加処理
    public AnimatorStateController AnimatorStateController;
    public UnitActionLoader UnitActionLoader;
    public List<EnemySkillInfo> SkillActions = new List<EnemySkillInfo>();
    public string IdleMotion = "idleCombat";
    public string WalkMotion = "walkForwardCombat";
    public string RunMotion = "runNormal";
    public string StaggerMotion = "getHitFront";
    public string DieMotion = "death";

    [Header("AI設定")]
    public EnemyState currentState = EnemyState.Patrolling;
    public Transform[] patrolPoints;
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 4.0f;
    public float sightRange = 10.0f;
    public float attackRange = 2.0f;
    public float fleeHealthThreshold = 0.2f;
    public float attackCooldown = 2.0f;
    public float maxHealth = 100f; // 最大体力
    public float staggerThreshold = 20f; // ひるみ発生閾値

    private float currentHealth;
    private float damageTaken = 0f; // 蓄積ダメージ
    private NavMeshAgent agent;
    private Transform player;
    private int currentPatrolIndex;
    private bool isAttacking = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform; // 修正１

        currentHealth = maxHealth;
        currentPatrolIndex = 0;
        PatrolNextPoint();

        AnimatorStateController.FinishMotionEvent += OnFinishMotionHandle;
    }

    private void Update()
    {
        // 死亡状態なら何もしない
        if (currentState == EnemyState.Dead) return;

        // 攻撃中やひるみ中なら動作を止める
        if (isAttacking || currentState == EnemyState.Staggered) return;

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Attacking:
                // 攻撃ステートは別途処理される
                StartAttack();
                break;
            case EnemyState.Retreating:
                Retreat();
                break;
            case EnemyState.Fleeing:
                Flee();
                break;
        }
    }

    // プレイヤーが視界にいるかチェック
    private bool IsPlayerInSight()
    {
        return Vector3.Distance(transform.position, player.position) <= sightRange;
    }

    // 次のパトロールポイントに移動
    private void PatrolNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        agent.speed = patrolSpeed;
        AnimatorStateController.StatePlay(WalkMotion, EStatePlayType.SinglePlay, false); // Walkアニメーション

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void Patrol()
    {
        if (agent.remainingDistance < 0.5f)
        {
            PatrolNextPoint();
        }

        if (IsPlayerInSight())
        {
            currentState = EnemyState.Chasing;
        }
    }

    // プレイヤーを追跡
    private void ChasePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            // プレイヤーが攻撃範囲外なら追跡を続ける
            agent.isStopped = false; // NavMeshAgentが停止している場合は再開
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            AnimatorStateController.StatePlay(RunMotion, EStatePlayType.SinglePlay, false); // Runアニメーション
        }
        else
        {
            // プレイヤーが攻撃範囲内にいる場合は攻撃
            currentState = EnemyState.Attacking;
            StartAttack();
        }
    }

    // 攻撃処理
    private async void StartAttack()
    {
        if (isAttacking) return;



        //AnimatorStateController.StatePlay("Attack", EStatePlayType.SinglePlay, true); // 攻撃アニメーション
        var skill = SearchSkill(Vector3.Distance(transform.position, player.transform.position));
        if(skill != null)
        {
            agent.enabled = false;
            UnitActionLoader.LoadAction(skill.SkillAction);
            isAttacking = true;
            agent.isStopped = true; // 攻撃中は移動を停止
        }
        else
        {
            agent.enabled = true;
            // プレイヤーがまだ範囲内かどうか確認
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                StartAttack();
            }
            else
            {
                currentState = EnemyState.Chasing;
            }
        }

        // 攻撃アニメーションが終了するまで待機
        //await UniTask.Delay(1000);


    }

    // ダメージを受けた際の処理
    public async void TakeDamage(float damage)
    {
        if (currentState == EnemyState.Dead) return; // すでに死亡している場合は何もしない

        currentHealth -= damage;
        damageTaken += damage;

        if (currentHealth <= 0f)
        {
            Die();
            return;
        }

        // ひるみ状態のチェック
        if (damageTaken >= staggerThreshold)
        {
            // 怯む処理
            Stagger();
            //await UniTask.Delay(1000); // 怯みモーションの再生時間


        }
    }

    // 怯み処理
    private void Stagger()
    {
        currentState = EnemyState.Staggered;
        agent.isStopped = true; // 怯み中は移動停止
        AnimatorStateController.StatePlay(StaggerMotion, EStatePlayType.SinglePlay, true); // 怯みモーション

        // もし攻撃中であれば、その動作をキャンセル
        isAttacking = false;
    }

    // 死亡処理
    private async void Die()
    {
        currentState = EnemyState.Dead; // ステートをDeadに変更
        agent.isStopped = true; // 移動を停止
        AnimatorStateController.StatePlay(DieMotion, EStatePlayType.SinglePlay, true); // 死亡モーションを再生

        // 死亡モーションが完了するまで待機
        await UniTask.Delay(2000); // 死亡モーションの再生時間
        // 必要に応じてAIオブジェクトを破棄するなどの処理を行う
    }

    // 逃走処理
    private void Flee()
    {
        if (currentHealth <= fleeHealthThreshold)
        {
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            Vector3 fleeDestination = transform.position + fleeDirection * 10.0f;
            agent.SetDestination(fleeDestination);
            agent.speed = patrolSpeed;
            AnimatorStateController.StatePlay(WalkMotion, EStatePlayType.SinglePlay, false); // Walkアニメーション

            if (Vector3.Distance(transform.position, player.position) > sightRange)
            {
                currentState = EnemyState.Patrolling;
            }
        }
    }

    // プレイヤーとの距離を取る処理
    private void Retreat()
    {
        Vector3 retreatDirection = (transform.position - player.position).normalized;
        Vector3 retreatDestination = transform.position + retreatDirection * 5.0f;
        agent.SetDestination(retreatDestination);
        agent.speed = patrolSpeed;

        AnimatorStateController.StatePlay(WalkMotion, EStatePlayType.SinglePlay, false); // Walkアニメーション

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = EnemyState.Chasing;
        }
    }

    // デバッグ用のギズモ表示
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    

    /// <summary>
    /// 発動スキルを一つ選定する
    /// </summary>
    /// <param name="targetDistance">ターゲットとの距離</param>
    private EnemySkillInfo SearchSkill(float targetDistance)
    {
        // LINQを使って発動可能なスキルを選定
        var availableSkills = SkillActions
            .Where(skill => skill.Distance >= targetDistance) // ターゲット距離以内のスキルを選定
            .ToList(); // リストに変換

        // 利用可能なスキルが存在する場合、その中からランダムで1つ選定
        if (availableSkills.Any())
        {
            var selectedSkill = availableSkills[UnityEngine.Random.Range(0, availableSkills.Count)];
            Debug.Log("選定されたスキル: " + selectedSkill.SkillAction.name);

            // スキルを発動する処理をここに記述
            //ActivateSkill(selectedSkill.SkillAction);
            return selectedSkill;
        }
        else
        {
            Debug.Log("発動可能なスキルがありません。");
            return null;
        }
    }
    //private void Look()
    //{
    //    Vector3 direction = player.transform.position - transform.position;
    //    direction.y = 0;

    //    if (direction.sqrMagnitude > 0.001f)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(direction);
    //        // スムーズに回転するためにSlerpを使用
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    //    }
    //}
    private async UniTask Look()
    {
        bool isLookingAtPlayer = false; // プレイヤーの方向を見ているかのフラグ

        while (!isLookingAtPlayer)
        {
            if (player == null) // playerがnullでないことを確認
            {
                Debug.LogError("Player is not assigned!");
                break;
            }

            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0; // 水平方向の回転に限定

            if (direction.sqrMagnitude > 0.001f) // プレイヤーがほぼ同じ位置にいないことを確認
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

                // 現在の回転と目標の回転がほぼ一致しているかをチェック
                if (Quaternion.Angle(transform.rotation, targetRotation) < 1f) // 例えば1度以下の誤差
                {
                    isLookingAtPlayer = true; // プレイヤーの方向を見たと判断してループを抜ける
                }
            }

            // 次のフレームまで待機
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    private async void OnFinishMotionHandle(string stateName, bool flg)
    {
        if (isAttacking)
        {
            agent.enabled = true;
            agent.isStopped = false;
            AnimatorStateController.StatePlay(IdleMotion, EStatePlayType.SinglePlay, true); // Idleアニメーション

            // 攻撃後のクールダウンを設定
            await UniTask.Delay(Mathf.RoundToInt(attackCooldown * 1000));
            await Look();


            //// プレイヤーがまだ範囲内かどうか確認
            //if (Vector3.Distance(transform.position, player.position) <= attackRange)
            //{
            //    StartAttack();
            //}
            //else
            //{
            //    currentState = EnemyState.Chasing;
            //}

            isAttacking = false;
        }

        if (currentState == EnemyState.Staggered)
        {
            // 怯み後に再度動作を再開
            damageTaken = 0f; // ダメージの蓄積をリセット
            currentState = EnemyState.Chasing; // 怯み後は追跡に戻る
        }
    }
}
