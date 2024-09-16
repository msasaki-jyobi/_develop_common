using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace develop_common
{
    public class AIFunction : MonoBehaviour
    {
        //    /// <summary>
        //    /// プレイヤーへの追跡が可能かどうか？
        //    /// </summary>
        //    public bool LookPlayer()
        //    {
        //        float targetDistance =
        //               Vector3.Distance(transform.position, player.transform.position); // 追跡対象との距離取得
        //        float angle = TargetAngle(transform, player.transform); // 追跡対象がいる角度を取得
        //        GameObject getTarget = GetRayTarget(eye, player); // 追跡対象にRayを飛ばす

        //        bool check = true;
        //        if (!ignoreDistance) check = check && targetDistance <= chaseDistance; // 追跡対象との距離が視認距離以内か？
        //        if (!ignoreAngle) check = check && Mathf.Abs(angle) <= recognitionAngle; // 追跡対象が視認角度内にいるか？
        //        if (!ignoreRay) check = check && getTarget == player; // Rayを飛ばした結果、返されたオブジェクトが追跡対象か？

        //        return check;
        //    }

        /// <summary>
        /// 自分とターゲットの距離を取得
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        static public float TargetDistance(GameObject self, GameObject target)
        {
            return Vector3.Distance(self.transform.position, target.transform.position);
        }

        /// <summary>
        /// 追跡対象との角度を取得
        /// </summary>
        /// <param name="self">自身</param>
        /// <param name="target">追跡対象</param>
        /// <returns></returns>
        static public float TargetAngle(GameObject self, GameObject target)
        {
            var diff = target.transform.position - self.transform.position;
            var axis = Vector3.Cross(self.transform.forward, diff);
            var angle = Vector3.Angle(self.transform.forward, diff)
                * (axis.y < 0 ? -1 : 1);
            return angle;
        }

        /// <summary>
        /// 追跡対象へRayを飛ばして、オブジェクトを検知
        /// </summary>
        /// <param name="self">自身</param>
        /// <param name="target">追跡対象</param>
        /// <param name="distance">rayを飛ばす距離</param>
        /// <param name="rayHeight">Rayを飛ばす先の高さ補正(足元に飛ばしてしまう為）</param>
        /// <returns></returns>
        static public GameObject GetRayTarget(GameObject self, GameObject target, float distance = 10f, float rayHeight = 1f)
        {
            // 自身から追跡対象の方向へＲａｙを飛ばす
            Ray ray = new Ray(self.transform.position, (target.transform.position + target.transform.up * rayHeight) - self.transform.position);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);


            // 14番目を1：シフト演算子 << を使用して、整数値 1 を14ビット左にシフト 0x4000:バイナリ形式(0100 0000 0000 0000)
            int layerMask = 1 << 14;
            // ~ 演算子はビットごとのNOT（反転）演算 結果、14番目のビットだけが0で、他のすべてのビットが1
            // 14番目のレイヤーだけRayの対象外になる
            layerMask = ~layerMask;

            // 何かしらオブジェクトを検知した場合、オブジェクトを返す
            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                return hit.collider.gameObject;
            }
            return null;
        }
        /// <summary>
        /// ターゲットが前方にいるか検知
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        static public bool TargetForward(GameObject self, GameObject target)
        {
            var check = self.transform.InverseTransformPoint(target.transform.position).z;
            if (check <= 0)
            {
                //Debug.Log("後方にいる");
                return false;
            }
            else
            {
                //Debug.Log("前方にいる");
                return true;
            }
        }


        
        /// <summary>
        /// 自分の視認範囲内にいる敵をすべて取得
        /// </summary>
        /// <param name="self">自身</param>
        /// <param name="angle">視認角度</param>
        /// <param name="num">取得する敵の数</param>
        /// <param name="targetTag">対象タグ</param>
        /// <returns></returns>
        public static List<GameObject> GetEnemyesWithinAngle(GameObject self, float angle, int num, string targetTag = "Enemy")
        {
            // 敵を保持するリスト
            List<GameObject> enemiesWithinAngle = new List<GameObject>();
            // 全てのEnemyタグのオブジェクトを取得
            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag(targetTag);

            foreach (GameObject enemy in allEnemies)
            {
                // 自身と敵との間の角度を計算
                float angleToEnemy = TargetAngle(self, enemy);

                // 角度が指定した角度A以内かどうかをチェック
                if (Mathf.Abs(angleToEnemy) <= angle)
                {
                    enemiesWithinAngle.Add(enemy);
                    //if (enemy.TryGetComponent<UnitStatus>(out var status))
                    //{
                    //    // 生きてるならリストに追加
                    //    if (!status.IsDead)
                    //        enemiesWithinAngle.Add(enemy);
                    //}
                }
            }

            // 敵を自身に対する距離でソート
            List<GameObject> sortedEnemies = enemiesWithinAngle.OrderBy(enemy => (enemy.transform.position - self.transform.position).sqrMagnitude).ToList();

            // 指定された数Bだけ取得
            return sortedEnemies.Take(num).ToList();
        }

        /// <summary>
        /// 自分から見てターゲットの上下の角度を取得
        /// </summary>
        /// <param name="self">自身のGameObject</param>
        /// <param name="target">ターゲットのGameObject</param>
        /// <returns>上下の角度</returns>
        public static float TargetVerticalAngle(GameObject self, GameObject target)
        {
            // ターゲットへのベクトルを計算
            Vector3 toTarget = target.transform.position - self.transform.position;

            // 自分の前方向ベクトルからY成分を除いて水平面上のベクトルを作成
            Vector3 forwardHorizontal = self.transform.forward;
            forwardHorizontal.y = 0;

            // 水平面のベクトルを正規化
            forwardHorizontal.Normalize();

            // ターゲットへのベクトルの水平成分を除去して、上下方向のみのベクトルを取得
            Vector3 toTargetVertical = toTarget - Vector3.Project(toTarget, forwardHorizontal);

            // 自分の上方向とターゲットの上下方向ベクトルとの角度を計算
            float angle = Vector3.Angle(self.transform.up, toTargetVertical) * (toTargetVertical.y < 0 ? -1 : 1);

            return angle;
        }
    }
}