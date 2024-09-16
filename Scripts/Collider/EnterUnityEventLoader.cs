using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace develop_common
{
    public class EnterUnityEventLoader : MonoBehaviour
    {
        // タグのリストを指定する
        public List<string> targetTags = new List<string>() { "Player", "Unit", "Character"};
        public bool IsFinishDestroy;
        public float DestroyTime = 0.5f;
        public UnityEvent EnterEvent;


        // 何かに衝突したときの処理
        private void OnCollisionEnter(Collision collision)
        {
            OnHit(collision.gameObject);
        }

        // トリガーに入ったときの処理
        private void OnTriggerEnter(Collider other)
        {
            OnHit(other.gameObject);
        }

        // タグをチェックして削除
        private void OnHit(GameObject hitObject)
        {
            // タグのリストに衝突したオブジェクトのタグが含まれているか確認
            if (targetTags.Contains(hitObject.tag))
            {
                EnterEvent?.Invoke();
                if (IsFinishDestroy)
                    Destroy(gameObject, DestroyTime);
            }
        }
    }
}