using System.Collections;
using UnityEngine;

namespace develop_common 
{
    [AddComponentMenu("ActionDamageValue：ダメージ情報")]
    public class ActionDamageValue : MonoBehaviour
    {
        // 情報引き継ぐ（Prefab設定用の） 
        public DamageValue DamageValue;
    }
}