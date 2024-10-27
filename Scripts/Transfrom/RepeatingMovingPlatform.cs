using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class RepeatingMovingPlatform : MonoBehaviour
    {
        public Vector3 OffsetB = new Vector3(0f, 1.0f, 0); // ローカル座標での目的地までのオフセット
        public float Speed = 2.0f;  // 移動速度
        public float PauseDuration = 1.0f;  // 移動停止時間

        private Vector3 _initialLocalPosition; // 初期位置（ローカル座標）
        private Vector3 _targetLocalPosition;  // 目的地（ローカル座標）
        private bool _movingToB = true;        // 初期位置から目的地に向かっているかどうか
        private float _pauseTimer = 0.0f;      // 停止タイマー

        public bool IsMove = true;

        void Start()
        {
            // 初期位置（ローカル座標）を保存し、OffsetBを加えて目的地を設定
            _initialLocalPosition = transform.localPosition;
            _targetLocalPosition = _initialLocalPosition + OffsetB;
        }

        void Update()
        {
            if (!IsMove) return;

            if (_pauseTimer > 0)
            {
                _pauseTimer -= Time.deltaTime;
                return;
            }

            // 初期位置と目的地間をローカル座標でリピート移動
            Vector3 destination = _movingToB ? _targetLocalPosition : _initialLocalPosition;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, Speed * Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, destination) < 0.01f)
            {
                _movingToB = !_movingToB;  // 移動方向を反転
                _pauseTimer = PauseDuration;  // 停止タイマーをリセット
            }
        }

        void OnDrawGizmos()
        {
            // 初期位置と目的地間に線を描画（ローカル座標で計算）
            Vector3 startPosition = transform.parent ? transform.parent.TransformPoint(_initialLocalPosition) : transform.position;
            Vector3 endPosition = transform.parent ? transform.parent.TransformPoint(_initialLocalPosition + OffsetB) : transform.position + OffsetB;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPosition, endPosition);
        }

        public void OnChangeMove(bool flg)
        {
            IsMove = flg;
        }
    }
}








//public class RepeatingMovingPlatform : MonoBehaviour
//{
//    public Vector3 OffsetA = new Vector3(0f, 0, 0);  // 初期位置から左に移動するオフセット
//    public Vector3 OffsetB = new Vector3(0f, 0, 0);   // 初期位置から右に移動するオフセット
//    public float Speed = 2.0f;  // 移動速度
//    public float PauseDuration = 1.0f;  // 移動停止時間

//    private Vector3 _pointA;  // 実際の移動開始地点
//    private Vector3 _pointB;  // 実際の移動終了地点
//    private Vector3 _targetPosition;  // 次の目標地点
//    private bool _movingToB = true;  // AからBに向かっているかどうか
//    private float _pauseTimer = 0.0f;  // 停止タイマー

//    public bool IsMove = true;

//    void Start()
//    {
//        // 現在の座標を基準にオフセットを加えて移動地点を設定
//        _pointA = transform.position + OffsetA;
//        _pointB = transform.position + OffsetB;

//        // 初期目標をB地点に設定
//        _targetPosition = _pointB;
//    }

//    void Update()
//    {
//        if (!IsMove) return;

//        // 停止時間がある場合、タイマーを進める
//        if (_pauseTimer > 0)
//        {
//            _pauseTimer -= Time.deltaTime;
//            return;
//        }

//        // 目標地点に向かって移動
//        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Speed * Time.deltaTime);

//        // 目標地点に到達したら停止し、逆方向に切り替える
//        if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
//        {
//            _movingToB = !_movingToB;  // 移動方向を反転
//            _targetPosition = _movingToB ? _pointB : _pointA;  // 次の目標地点を設定
//            _pauseTimer = PauseDuration;  // 停止タイマーをリセット
//        }
//    }

//    // Gizmosを使ってシーンビューで移動範囲を視覚化
//    void OnDrawGizmos()
//    {
//        // 座標がまだ計算されていない場合は、仮の位置で表示
//        Vector3 a = transform.position + OffsetA;
//        Vector3 b = transform.position + OffsetB;

//        Gizmos.color = Color.green;
//        Gizmos.DrawLine(a, b);
//    }

//    public void OnChangeMove(bool flg)
//    {
//        IsMove = flg;
//    }
//}
