using develop_common;
using develop_tps;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InputEnemyAI : MonoBehaviour
{
    [Header("共通")]
    public TPSUnitController TPSUnitController;
    public UnitActionLoader UnitActionLoader;
    public UnitHealth UnitHealth;
    public float StoppingDistance = 0.5f; // 目的地に到達と見なす距離
    public float HuntDistance = 5f; // 目的地に到達と見なす距離
    [Header("パトロール 関連")]
    public List<GameObject> PatrolPoints = new List<GameObject>(); // 探索場所
    [Header("攻撃 関連")]
    public float AttackTime = 5;
    public List<GameObject> AttackActions = new List<GameObject>();
    public List<GameObject> StepActions = new List<GameObject>();

    [Header("自動 取得")]
    public Transform AttackTarget;  // 攻撃目標
    public Transform PatrolTarget;  // 探索目標
    public Transform Target;  // 目標地点

    private NavMeshPath _path;  // NavMeshの経路情報
    private int _currentPathIndex = 0;
    private float _attackTimer = 0;

    void Start()
    {
        _path = new NavMeshPath();
        if (PatrolPoints.Count > 0)
            PatrolTarget = GetPatrolPoint();
    }

    void Update()
    {
        // 目標の決め方　プレイヤーとの距離
        // 対象追跡に入るには StateがReadyの時
        // 攻撃に入るには、対象がプレイヤー、攻撃可能時間になったら
        if (AttackTarget == null)
            AttackTarget = GameObject.Find("Player").transform;
        if (AttackTarget == null) return;

        // 攻撃対象との距離
        float atkTargetDistance = Vector3.Distance(transform.position, AttackTarget.position);
        if (PatrolPoints.Count > 0) // パトロールポイントがあるなら
            Target = atkTargetDistance > HuntDistance ? PatrolTarget : AttackTarget.transform;
        else // パトロールポイントがないなら攻撃対象をTarget
            Target = AttackTarget.transform;


        if (UnitActionLoader.UnitStatus.Value == EUnitStatus.Ready)
        {
            if (Target != null)
            {
                // 現在地と目標地点の距離を計算
                float distanceToTarget = atkTargetDistance;
                if (Target != AttackTarget)
                    distanceToTarget = Vector3.Distance(transform.position, Target.position);

                // 目的地に近づきすぎた場合
                if (distanceToTarget <= StoppingDistance)
                {
                    // 停止（もしくは次のロジックを待つ）
                    MoveCharacter(0, 0);

                    // パトロール中なら
                    if (Target != AttackTarget)
                    {
                        if (PatrolPoints.Count > 0)
                            PatrolTarget = GetPatrolPoint();
                    }
                    else if (AttackActions.Count + StepActions.Count > 0)// 攻撃対象なら攻撃実行
                    {
                        int ran = Random.Range(0, 10);

                        transform.LookAt(Target.transform.position);
                        Vector3 rot = transform.rotation.eulerAngles;
                        transform.rotation = Quaternion.Euler(0, rot.y, 0);

                        if (ran <= 3)
                        {
                            if (StepActions.Count > 0)
                                UnitActionLoader.LoadAction(StepActions[Random.Range(0, StepActions.Count)], ignoreRequirement: true);
                            else
                                UnitActionLoader.LoadAction(AttackActions[Random.Range(0, AttackActions.Count)], ignoreRequirement: true);
                        }
                        else
                            UnitActionLoader.LoadAction(AttackActions[Random.Range(0, AttackActions.Count)], ignoreRequirement: true);

                    }
                    return;
                }

                // 目標地点へのパスを計算
                NavMesh.CalculatePath(transform.position, Target.position, NavMesh.AllAreas, _path);

                // パスを進むロジック
                if (_path.corners.Length > 1 && _currentPathIndex < _path.corners.Length - 1)
                {
                    Vector3 direction = (_path.corners[_currentPathIndex + 1] - transform.position).normalized;

                    float inputX = direction.x;
                    float inputY = direction.z; // Zを前進軸とする

                    MoveCharacter(inputX, inputY);

                    if (Vector3.Distance(transform.position, _path.corners[_currentPathIndex + 1]) < 0.5f)
                    {
                        _currentPathIndex++;
                    }
                }
                else
                {
                    // パスを再計算
                    _currentPathIndex = 0;
                }
            }
        }
        else if (UnitActionLoader.UnitStatus.Value == EUnitStatus.Down) // ダウン中なら
        {
            UnitActionLoader.LoadAction(UnitHealth.GetUpAction);
        }

    }


    private void MoveCharacter(float inputX, float inputY)
    {
        TPSUnitController.InputX = inputX;
        TPSUnitController.InputY = inputY;
    }

    private Transform GetPatrolPoint()
    {
        int ran = Random.Range(0, PatrolPoints.Count);
        return PatrolPoints[ran].transform;
    }
}
