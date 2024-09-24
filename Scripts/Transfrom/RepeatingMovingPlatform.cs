using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class RepeatingMovingPlatform : MonoBehaviour
    {
        public Vector3 offsetA = new Vector3(-5.0f, 0, 0);  // 初期位置から左に移動するオフセット
        public Vector3 offsetB = new Vector3(5.0f, 0, 0);   // 初期位置から右に移動するオフセット
        public float speed = 2.0f;  // 移動速度
        public float pauseDuration = 1.0f;  // 移動停止時間

        private Vector3 pointA;  // 実際の移動開始地点
        private Vector3 pointB;  // 実際の移動終了地点
        private Vector3 targetPosition;  // 次の目標地点
        private bool movingToB = true;  // AからBに向かっているかどうか
        private float pauseTimer = 0.0f;  // 停止タイマー

        void Start()
        {
            // 現在の座標を基準にオフセットを加えて移動地点を設定
            pointA = transform.position + offsetA;
            pointB = transform.position + offsetB;

            // 初期目標をB地点に設定
            targetPosition = pointB;
        }

        void Update()
        {
            // 停止時間がある場合、タイマーを進める
            if (pauseTimer > 0)
            {
                pauseTimer -= Time.deltaTime;
                return;
            }

            // 目標地点に向かって移動
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // 目標地点に到達したら停止し、逆方向に切り替える
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                movingToB = !movingToB;  // 移動方向を反転
                targetPosition = movingToB ? pointB : pointA;  // 次の目標地点を設定
                pauseTimer = pauseDuration;  // 停止タイマーをリセット
            }
        }

        // Gizmosを使ってシーンビューで移動範囲を視覚化
        void OnDrawGizmos()
        {
            // 座標がまだ計算されていない場合は、仮の位置で表示
            Vector3 a = transform.position + offsetA;
            Vector3 b = transform.position + offsetB;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(a, b);
        }


    }
}