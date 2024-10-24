using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace develop_common
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PrefsSave : MonoBehaviour
    {
        [Header("セーブNo（他と重複しない番号を振ろう")]
        public int SavePointNum = 0;
        [Header("あたり判定を検知するオブジェクト名")]
        public string TargetName = "cat";
        [Header("デバッグ用：セーブデータリセット")]
        public bool IsSaveReset;

        private GameObject player;
        private string _saveKey = "SavePoint";

        // Startよりも早く一度だけ呼ばれる
        private void Awake()
        {
            // playerオブジェクトを取得
            player = GameObject.Find(TargetName);

            // コンポーネントの自動設定
            if (TryGetComponent<BoxCollider2D>(out var collider))
                collider.isTrigger = true;
            if (TryGetComponent<Rigidbody2D>(out var rigid))
                rigid.isKinematic = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (IsSaveReset) // セーブリセット
                PlayerPrefs.SetInt(_saveKey, 0); // セーブデータを上書き

            // 保存されている値を取得
            var save = PlayerPrefs.GetInt(_saveKey, 0); // セーブデータが存在しない場合 0 

            // セーブデータが一致する場合、Playerを自分の場所に移動
            if (save == SavePointNum)
                player.transform.position = transform.position;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnHit(other.gameObject);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnHit(collision.gameObject);
        }

        private void OnHit(GameObject hit)
        {
            if (hit.gameObject == player) // 触れたオブジェクトがPlayerなら
            {
                PlayerPrefs.SetInt(_saveKey, SavePointNum); // セーブ実行
                Debug.Log($"Saveが完了しました. {SavePointNum}");
            }
        }
    }

}
