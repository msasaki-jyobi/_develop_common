using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace develop_common
{
    [System.Serializable]
    public class FrameInfo
    {
        [Tooltip("発動フレーム")] // 追加したら下部のコピーメソッドに処理を追加すること！
        public float PlayFrame;
        public FrameNormalData NormalData;
        public FramePrefabData PrefabData;
        public GameObject Attack_DamageData;
        public FrameActiveAttackData ActiveAttackData;
        public FramePullData PullData;
        public FrameIKData IKData;

        //[Tooltip("Velocity Reset")]
        //public bool IsResetVelocity;
        //[Tooltip("力を加える")]
        //public bool IsForce;
        //public Vector3 FoucePower;
        //[Tooltip("プレハブを生成する")]
        //public bool IsPrefab;
        //public int PrefabNum;
        //[Tooltip("追加入力が可能になる")]
        //public bool IsNextAction;


        // ここから下は各クラス側でそのフレームなったらの処理にしたいな…あーでもエフェクトとか…うーん困った
        //[Tooltip("攻撃判定が発生する")]
        //public bool IsActiveAttack;
        //[Tooltip("固定が発生する")]
        //public bool IsPull;
        //public int PullNum;
        // 固定化して、しばらくしたら吹き飛ぶ判定になるはこれでいける。
        //[Tooltip("与えるダメージを変更する　投げ技用にもう一つほしいな、膝がアクティブになってAdditiveになるとか。")]
        //public bool IsChangeDamage;
        //public int ChangeDamageActionNum;

        // ヒットしたらこのモーションを互いに再生するがやりたいな。
        // あと該当フレームになったら、この部位のダメージモーションを差し替えるにしたいな…うーむ
        // 投げ技用のActionと、ランダムクラスを新たに用意して、ヒットしたらランダムのアクションを実行するとか？



        //[Header("オブジェクト生成")]
        //public List<PrefabData> Prefabs;
        //[Header("効果音")]
        //public AudioClip PlaySE;
        //[Header("力を加える")]
        //public Vector3 ForcePower;
        //[Header("追加入力可能状態にする")]
        //public bool AddKey;
        //[Header("SetKinematic")]
        //public bool IsKinematicChange;
        //public bool SetKinematic;
        //[Header("追加ダメージ")]
        //public int AddDamage;

        [Space(10)]
        public bool IsComplete;

        // コピーコンストラクタ
        public FrameInfo(FrameInfo other)
        {
            this.PlayFrame = other.PlayFrame;
            this.NormalData = other.NormalData;
            this.PrefabData = other.PrefabData;
            this.ActiveAttackData = other.ActiveAttackData;
            this.Attack_DamageData = other.Attack_DamageData;
            this.PullData = other.PullData;
            this.IKData = other.IKData;

            //this.IsResetVelocity = other.IsResetVelocity;

            //this.IsForce = other.IsForce;
            //this.FoucePower = other.FoucePower;

            //this.IsPrefab = other.IsPrefab;
            //this.PrefabNum = other.PrefabNum;

            //this.IsNextAction = other.IsNextAction;

            //this.IsActiveAttack = other.IsActiveAttack;

            //this.IsPull = other.IsPull;
            //this.PullNum = other.PullNum;

            //this.IsChangeDamage = other.IsChangeDamage;
            //this.ChangeDamageActionNum = other.ChangeDamageActionNum;





            this.IsComplete = other.IsComplete;
        }
    }
}