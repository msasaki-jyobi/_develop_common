using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class DelayedDropPlatform : MonoBehaviour
    {
        public string TargetTagName = "Player";
        public float DelayTime = 1f;
        public GameObject DropObject;
        
        public bool IsDrop;

        public bool _isComplete;
        private float _timer;

        private void Start()
        {
            if (DropObject == null)
                DropObject = gameObject;
        }

        private void Update()
        {
            if(IsDrop)
                if(!_isComplete)
                {
                    _timer += Time.deltaTime;
                    if(_timer >= DelayTime)
                    {
                        _isComplete = true;
                        if(DropObject.TryGetComponent<Rigidbody>(out var rigidbody))
                        {
                            rigidbody.useGravity = true;
                            rigidbody.isKinematic = false;
                        }
                        else
                        {
                            Debug.LogError($"DropObject： {DropObject.name}にはRigidBodyが存在しません");
                        }
                    }
                }
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnHit(collision.gameObject);
        }
        private void OnTriggerEnter(Collider other)
        {
            OnHit(other.gameObject);
        }
        private void OnHit(GameObject hit)
        {
            if(hit.CompareTag(TargetTagName))
            {
                IsDrop = true;
            }
        }

        public void OnChangeIsDrop(bool flg)
        {
            IsDrop = flg;
        }

    }
}