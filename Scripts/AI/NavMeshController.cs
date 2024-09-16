using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace develop_common
{
    public class NavMeshController : MonoBehaviour
    {
        public GameObject Target;
        public NavMeshAgent Agent;
        public GameObject Eye;

        public float NormalSpeed = 3.5f;
        public float HuntSpeed = 3.5f;

        public bool IsDistance; // 指定距離以内が必要
        public float distance = 5f;
        public bool IsLook; // 視認が必要

        private void Start()
        {
            if(Target == null)
            Target = GameObject.Find("Player");

            // 半径10m以内の徘徊場所を自動取得
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (Target == null || Agent == null) return;  // Target または Agent がない場合は何もしない

            bool check = true;
            if (IsDistance) // 距離をチェック
                check = check && AIFunction.TargetDistance(Target, gameObject) <= distance;
            if (IsLook) // 視認をチェック
                check = check && AIFunction.GetRayTarget(Eye, Target, rayHeight: 0.5f);

            if (check)
            {
                // ターゲットを追跡するための移動を開始
                Agent.speed = HuntSpeed;  // 追跡用のスピードに設定
                Agent.SetDestination(Target.transform.position);  // ターゲットに向かって移動
            }
            else
            {
                // ターゲットを追わない場合、通常の動作に戻る（必要であれば実装）
                Agent.speed = NormalSpeed;  // 通常の速度に戻す

                // 半径10m以内の探索場所に向かう



            }
        }

    }
}
