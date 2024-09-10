using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace develop_common
{
    public class Jump : MonoBehaviour
    {
        // 空気抵抗
        [SerializeField] private float _drag = 0;
        [SerializeField] private float _angularDrag = 0.05f;
        [SerializeField] private float _jumpPower = 5f;
        [SerializeField] private LineData _groundLineData;

        // Components
        private Rigidbody _rigidbody;
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.drag = _drag;
            _rigidbody.angularDrag = _angularDrag;
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(CheckLineData(_groundLineData))
                    _rigidbody.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
            }
        }

        /// <summary>
        /// LineCastを検知するメソッド
        /// </summary>
        private bool CheckLineData(LineData lineData)
        {
            if (lineData == null) return false;

            for (int i = 0; i < lineData.StartPosition.Count; i++)
            {
                // 向いている方向で座標を計算
                Vector3 lookStartPos =
                        transform.right * lineData.StartPosition[i].x +
                        transform.up * lineData.StartPosition[i].y +
                        transform.forward * lineData.StartPosition[i].z;
                Vector3 lookEndPos =
                        transform.right * lineData.EndPosition[i].x +
                        transform.up * lineData.EndPosition[i].y +
                        transform.forward * lineData.EndPosition[i].z;
                // 開始位置の座標
                Vector3 startPos = transform.position + lookStartPos;
                // 終了位置の座標
                Vector3 endPos = transform.position + lookEndPos;
                // 描画
                Debug.DrawLine(startPos, endPos, lineData.LineColor);
                // 判定チェック
                if (Physics.Linecast(startPos, endPos, lineData.LineLayer))
                    return true;
            }
            return false;
        }
    }

}
