using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class SavePoint : MonoBehaviour
    {
        public string TargetTagName = "Player";
        public int PointID; // SaveID

        private void Awake()
        {
            SavePointManager.Instance.AddSavePoint(this);
        }

        private void Start()
        {
            
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
                SavePointManager.Instance.OnSave(PointID);
            }
        }

    }
}

