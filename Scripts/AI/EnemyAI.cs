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
        Staggered, // ���ݏ��
        Dead       // ���S���
    }

    // �ǉ�����
    public AnimatorStateController AnimatorStateController;
    public UnitActionLoader UnitActionLoader;
    public List<EnemySkillInfo> SkillActions = new List<EnemySkillInfo>();
    public string IdleMotion = "idleCombat";
    public string WalkMotion = "walkForwardCombat";
    public string RunMotion = "runNormal";
    public string StaggerMotion = "getHitFront";
    public string DieMotion = "death";

    [Header("AI�ݒ�")]
    public EnemyState currentState = EnemyState.Patrolling;
    public Transform[] patrolPoints;
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 4.0f;
    public float sightRange = 10.0f;
    public float attackRange = 2.0f;
    public float fleeHealthThreshold = 0.2f;
    public float attackCooldown = 2.0f;
    public float maxHealth = 100f; // �ő�̗�
    public float staggerThreshold = 20f; // �Ђ�ݔ���臒l

    private float currentHealth;
    private float damageTaken = 0f; // �~�σ_���[�W
    private NavMeshAgent agent;
    private Transform player;
    private int currentPatrolIndex;
    private bool isAttacking = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform; // �C���P

        currentHealth = maxHealth;
        currentPatrolIndex = 0;
        PatrolNextPoint();

        AnimatorStateController.FinishMotionEvent += OnFinishMotionHandle;
    }

    private void Update()
    {
        // ���S��ԂȂ牽�����Ȃ�
        if (currentState == EnemyState.Dead) return;

        // �U������Ђ�ݒ��Ȃ瓮����~�߂�
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
                // �U���X�e�[�g�͕ʓr���������
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

    // �v���C���[�����E�ɂ��邩�`�F�b�N
    private bool IsPlayerInSight()
    {
        return Vector3.Distance(transform.position, player.position) <= sightRange;
    }

    // ���̃p�g���[���|�C���g�Ɉړ�
    private void PatrolNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        agent.speed = patrolSpeed;
        AnimatorStateController.StatePlay(WalkMotion, EStatePlayType.SinglePlay, false); // Walk�A�j���[�V����

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

    // �v���C���[��ǐ�
    private void ChasePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            // �v���C���[���U���͈͊O�Ȃ�ǐՂ𑱂���
            //agent.isStopped = false; // NavMeshAgent����~���Ă���ꍇ�͍ĊJ
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            AnimatorStateController.StatePlay(RunMotion, EStatePlayType.SinglePlay, false); // Run�A�j���[�V����
        }
        else
        {
            // �v���C���[���U���͈͓��ɂ���ꍇ�͍U��
            currentState = EnemyState.Attacking;
            //StartAttack();
        }
    }

    // �U������
    private async void StartAttack()
    {
        if (isAttacking) return;



        //AnimatorStateController.StatePlay("Attack", EStatePlayType.SinglePlay, true); // �U���A�j���[�V����
        var skill = SearchSkill(Vector3.Distance(transform.position, player.transform.position));
        if(skill != null)
        {
            agent.enabled = false;
            UnitActionLoader.LoadAction(skill.SkillAction);

            isAttacking = true;

            //agent.isStopped = true; // �U�����͈ړ����~
        }
        else
        {
            agent.enabled = true;
            // �v���C���[���܂��͈͓����ǂ����m�F
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                StartAttack();
            }
            else
            {
                currentState = EnemyState.Chasing;
            }
        }

        // �U���A�j���[�V�������I������܂őҋ@
        //await UniTask.Delay(1000);


    }

    // �_���[�W���󂯂��ۂ̏���
    public async void TakeDamage(float damage)
    {
        if (currentState == EnemyState.Dead) return; // ���łɎ��S���Ă���ꍇ�͉������Ȃ�

        currentHealth -= damage;
        damageTaken += damage;

        if (currentHealth <= 0f)
        {
            Die();
            return;
        }

        // �Ђ�ݏ�Ԃ̃`�F�b�N
        if (damageTaken >= staggerThreshold)
        {
            // ���ޏ���
            Stagger();
            //await UniTask.Delay(1000); // ���݃��[�V�����̍Đ�����


        }
    }

    // ���ݏ���
    private void Stagger()
    {
        currentState = EnemyState.Staggered;
        //.isStopped = true; // ���ݒ��͈ړ���~
        AnimatorStateController.StatePlay(StaggerMotion, EStatePlayType.SinglePlay, true); // ���݃��[�V����

        // �����U�����ł���΁A���̓�����L�����Z��
        isAttacking = false;
    }

    // ���S����
    private async void Die()
    {
        currentState = EnemyState.Dead; // �X�e�[�g��Dead�ɕύX
        //agent.isStopped = true; // �ړ����~
        AnimatorStateController.StatePlay(DieMotion, EStatePlayType.SinglePlay, true); // ���S���[�V�������Đ�

        // ���S���[�V��������������܂őҋ@
        await UniTask.Delay(2000); // ���S���[�V�����̍Đ�����
        // �K�v�ɉ�����AI�I�u�W�F�N�g��j������Ȃǂ̏������s��
    }

    // ��������
    private void Flee()
    {
        if (currentHealth <= fleeHealthThreshold)
        {
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            Vector3 fleeDestination = transform.position + fleeDirection * 10.0f;
            agent.SetDestination(fleeDestination);
            agent.speed = patrolSpeed;
            AnimatorStateController.StatePlay(WalkMotion, EStatePlayType.SinglePlay, false); // Walk�A�j���[�V����

            if (Vector3.Distance(transform.position, player.position) > sightRange)
            {
                currentState = EnemyState.Patrolling;
            }
        }
    }

    // �v���C���[�Ƃ̋�������鏈��
    private void Retreat()
    {
        Vector3 retreatDirection = (transform.position - player.position).normalized;
        Vector3 retreatDestination = transform.position + retreatDirection * 5.0f;
        agent.SetDestination(retreatDestination);
        agent.speed = patrolSpeed;

        AnimatorStateController.StatePlay(WalkMotion, EStatePlayType.SinglePlay, false); // Walk�A�j���[�V����

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = EnemyState.Chasing;
        }
    }

    // �f�o�b�O�p�̃M�Y���\��
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    

    /// <summary>
    /// �����X�L������I�肷��
    /// </summary>
    /// <param name="targetDistance">�^�[�Q�b�g�Ƃ̋���</param>
    private EnemySkillInfo SearchSkill(float targetDistance)
    {
        int ran = Random.Range(0, SkillActions.Count);
        return SkillActions[ran];

        // LINQ���g���Ĕ����\�ȃX�L����I��
        var availableSkills = SkillActions
            .Where(skill => skill.Distance >= targetDistance) // �^�[�Q�b�g�����ȓ��̃X�L����I��
            .ToList(); // ���X�g�ɕϊ�

        // ���p�\�ȃX�L�������݂���ꍇ�A���̒����烉���_����1�I��
        if (availableSkills.Any())
        {
            var selectedSkill = availableSkills[UnityEngine.Random.Range(0, availableSkills.Count)];
            Debug.Log("�I�肳�ꂽ�X�L��: " + selectedSkill.SkillAction.name);

            // �X�L���𔭓����鏈���������ɋL�q
            //ActivateSkill(selectedSkill.SkillAction);
            return selectedSkill;
        }
        else
        {
            Debug.Log("�����\�ȃX�L��������܂���B");
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
    //        // �X���[�Y�ɉ�]���邽�߂�Slerp���g�p
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    //    }
    //}
    private async UniTask Look()
    {
        bool isLookingAtPlayer = false; // �v���C���[�̕��������Ă��邩�̃t���O

        while (!isLookingAtPlayer)
        {
            if (player == null) // player��null�łȂ����Ƃ��m�F
            {
                Debug.LogError("Player is not assigned!");
                break;
            }

            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0; // ���������̉�]�Ɍ���

            if (direction.sqrMagnitude > 0.001f) // �v���C���[���قړ����ʒu�ɂ��Ȃ����Ƃ��m�F
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

                // ���݂̉�]�ƖڕW�̉�]���قڈ�v���Ă��邩���`�F�b�N
                if (Quaternion.Angle(transform.rotation, targetRotation) < 1f) // �Ⴆ��1�x�ȉ��̌덷
                {
                    isLookingAtPlayer = true; // �v���C���[�̕����������Ɣ��f���ă��[�v�𔲂���
                }
            }

            // ���̃t���[���܂őҋ@
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    private async void OnFinishMotionHandle(string stateName, bool flg)
    {
        if (isAttacking)
        {
            agent.enabled = true;
            //agent.isStopped = false;
            AnimatorStateController.StatePlay(IdleMotion, EStatePlayType.SinglePlay, true); // Idle�A�j���[�V����

            // �U����̃N�[���_�E����ݒ�
            await UniTask.Delay(Mathf.RoundToInt(attackCooldown * 1000));
            await Look();


            //// �v���C���[���܂��͈͓����ǂ����m�F
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
            // ���݌�ɍēx������ĊJ
            damageTaken = 0f; // �_���[�W�̒~�ς����Z�b�g
            currentState = EnemyState.Chasing; // ���݌�͒ǐՂɖ߂�
        }
    }
}