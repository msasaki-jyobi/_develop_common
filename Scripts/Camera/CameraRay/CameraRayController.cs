using System.Collections;
using UnityEngine;

namespace GameSet.Common
{
    public class CameraRayController : MonoBehaviour
    {
        [SerializeField] private bool _checkUp;
        [SerializeField] private float _rayDistance = 10.0f;
        [SerializeField] private string _checkTagName = "CameraCollider";

        private void Update()
        {
            GetRay();
        }

        private void GetRay()
        {
            RaycastHit hit;
            Vector3 rayDirection = _checkUp ? Vector3.up : Vector3.down;

            // Rayを飛ばし、何かに衝突したかチェック
            if (Physics.Raycast(transform.position, rayDirection, out hit, _rayDistance))
            {
                if (hit.collider.gameObject.CompareTag(_checkTagName))
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    //if (hit.collider.TryGetComponent<VirtualCameraController>(out var controller))
                    //{
                    //    controller.OnChangeCamera();
                    //}
                }
            }
        }
    }
}