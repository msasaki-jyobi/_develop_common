using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class AnimatorIKController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _lookPoint;
        [SerializeField, Range(0, 1)] private float _weight = 1; // ウェイト 0 ～ 1
        [SerializeField, Range(0, 1)] private float _weightBody = 1; // ボディウェイト 0 ～ 1
        [SerializeField, Range(0, 1)] private float _weightHeight; // 頭ウェイト 0 ～ 1
        [SerializeField, Range(0, 1)] private float _weightEyes; // 目ウェイト 0 ～ 1
        [SerializeField, Range(0, 1)] private float _weightClamp; // 制限 0 ～ 1 1:完全制限 0.5: 半分 0: 自由利用可能
        [SerializeField, Range(0, 1)] private float _ikCameraDistance = 4.8f; // 制限 0 ～ 1 1:完全制限 0.5: 半分 0: 自由利用可能

        private void OnAnimatorIK(int layerIndex)
        {
            _animator.SetLookAtWeight(_weight, _weightBody, _weightHeight, _weightEyes, _weightClamp);
            _animator.SetLookAtPosition(_lookPoint.transform.position);
        }
    }
}