using develop_common;
using develop_tps;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InputEnemyAI : MonoBehaviour
{
    [Header("����")]
    public TPSUnitController TPSUnitController;
    public UnitActionLoader UnitActionLoader;
    public float StoppingDistance = 0.5f; // �ړI�n�ɓ��B�ƌ��Ȃ�����
    [Header("�p�g���[�� �֘A")]
    public List<GameObject> PatrolPoints = new List<GameObject>(); // �T���ꏊ
    [Header("�U�� �֘A")]
    public float AttackTime = 5;
    public List<GameObject> AttackActions = new List<GameObject>();
    public List<GameObject> StepActions = new List<GameObject>();

    [Header("���� �擾")]
    public Transform AttackTarget;  // �U���ڕW
    public Transform PatrolTarget;  // �T���ڕW
    public Transform Target;  // �ڕW�n�_

    private NavMeshPath _path;  // NavMesh�̌o�H���
    private int _currentPathIndex = 0;
    private float _attackTimer = 0;

    void Start()
    {
        _path = new NavMeshPath();
        PatrolTarget = GetPatrolPoint();
    }

    void Update()
    {
        // �ڕW�̌��ߕ��@�v���C���[�Ƃ̋���
        // �ΏےǐՂɓ���ɂ� State��Ready�̎�
        // �U���ɓ���ɂ́A�Ώۂ��v���C���[�A�U���\���ԂɂȂ�����
        if (AttackTarget == null)
            AttackTarget = GameObject.Find("Player").transform;
        if (AttackTarget == null) return;

        // �U���ΏۂƂ̋���
        float atkTargetDistance = Vector3.Distance(transform.position, AttackTarget.position);
        if (PatrolPoints.Count > 0) // �p�g���[���|�C���g������Ȃ�
            Target = atkTargetDistance > 5f ? PatrolTarget : AttackTarget.transform;
        else // �p�g���[���|�C���g���Ȃ��Ȃ�U���Ώۂ�Target
            Target = AttackTarget.transform;


        if (UnitActionLoader.UnitStatus.Value == EUnitStatus.Ready)
            if (Target != null)
            {
                // ���ݒn�ƖڕW�n�_�̋������v�Z
                float distanceToTarget = atkTargetDistance;
                if (Target != AttackTarget)
                    distanceToTarget = Vector3.Distance(transform.position, Target.position);

                // �ړI�n�ɋ߂Â��������ꍇ
                if (distanceToTarget <= StoppingDistance)
                {
                    // ��~�i�������͎��̃��W�b�N��҂j
                    MoveCharacter(0, 0);

                    // �p�g���[�����Ȃ�
                    if (Target != AttackTarget)
                    {
                        if (PatrolPoints.Count > 0)
                            PatrolTarget = GetPatrolPoint();
                    }
                    else // �U���ΏۂȂ�U�����s
                    {
                        UnitActionLoader.LoadAction(AttackActions[0], ignoreRequirement: true);
                    }
                    return;
                }

                // �ڕW�n�_�ւ̃p�X���v�Z
                NavMesh.CalculatePath(transform.position, Target.position, NavMesh.AllAreas, _path);

                // �p�X��i�ރ��W�b�N
                if (_path.corners.Length > 1 && _currentPathIndex < _path.corners.Length - 1)
                {
                    Vector3 direction = (_path.corners[_currentPathIndex + 1] - transform.position).normalized;

                    float inputX = direction.x;
                    float inputY = direction.z; // Z��O�i���Ƃ���

                    MoveCharacter(inputX, inputY);

                    if (Vector3.Distance(transform.position, _path.corners[_currentPathIndex + 1]) < 0.5f)
                    {
                        _currentPathIndex++;
                    }
                }
                else
                {
                    // �p�X���Čv�Z
                    _currentPathIndex = 0;
                }
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