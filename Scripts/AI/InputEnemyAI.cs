using develop_tps;
using UnityEngine;
using UnityEngine.AI;

public class InputEnemyAI : MonoBehaviour
{
    public TPSUnitController TPSUnitController;
    public Transform target;  // 目標地点
    private NavMeshPath path;  // NavMeshの経路情報
    private int currentPathIndex = 0;
    public float stoppingDistance = 0.5f; // 目的地に到達と見なす距離

    void Start()
    {
        path = new NavMeshPath();
    }

    void Update()
    {
        if (target != null)
        {
            // 現在地と目標地点の距離を計算
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // 目的地に近づきすぎた場合
            if (distanceToTarget <= stoppingDistance)
            {
                // 停止（もしくは次のロジックを待つ）
                MoveCharacter(0, 0);
                return;
            }

            // 目標地点へのパスを計算
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);

            // パスを進むロジック
            if (path.corners.Length > 1 && currentPathIndex < path.corners.Length - 1)
            {
                Vector3 direction = (path.corners[currentPathIndex + 1] - transform.position).normalized;

                float inputX = direction.x;
                float inputY = direction.z; // Zを前進軸とする

                MoveCharacter(inputX, inputY);

                if (Vector3.Distance(transform.position, path.corners[currentPathIndex + 1]) < 0.5f)
                {
                    currentPathIndex++;
                }
            }
            else
            {
                // パスを再計算
                currentPathIndex = 0;
            }
        }
    }

    void MoveCharacter(float inputX, float inputY)
    {
        TPSUnitController.InputX = inputX;
        TPSUnitController.InputY = inputY;
    }
}
