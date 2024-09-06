using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameSet.Common
{
    [System.Serializable]
    public class FrameInfo
    {

        //[Header("タイトル")]
        //public string Title;
        [Header("発動フレーム")]
        public float PlayFrame;
        [Header("力を加える")]
        public bool IsForce;
        public Vector3 FoucePower;
        [Header("プレハブを生成する")]
        public bool IsPrefab;
        public int PrefabNum;
        [Header("追加入力が可能になる")]
        public bool IsNextAction;

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
            this.IsForce = other.IsForce;
            this.FoucePower = other.FoucePower;
            this.IsPrefab = other.IsPrefab;
            this.PrefabNum = other.PrefabNum;
            this.IsNextAction = other.IsNextAction;



            this.IsComplete = other.IsComplete;
        }
    }
}