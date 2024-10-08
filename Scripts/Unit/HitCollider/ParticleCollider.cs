﻿using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class ParticleCollider : MonoBehaviour
    {
        [SerializeField] DamageDealer _damageDealer;
        // Collisionにチェック, Worldにする、 SendCollisionMessagesにチェック入れる
        void OnParticleCollision(GameObject obj)
        {
            //Debug.Log($"{gameObject.name}, hit{obj.gameObject.name}");
            _damageDealer.HitCheck(obj);
        }
    }
}