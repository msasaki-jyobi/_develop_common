using _develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class DamageManager : SingletonMonoBehaviour<DamageManager>
    {
        [Header("DamageCatalog（全体管理）")]
        public List<GameObject> DamageActions;
        // Action：ほにゃらら
        // 打ち上げ1：
        // 打ち上げ2：
        // 地面埋まり：
        // ノーマルダメージ：
        // 部位別ダメージ（小）：
        // DDT_vic(イメージとしては "再生するだけ" ：
        // くっつき：仰向けパタパタ
        // くっつき：うつ伏せパタパタ


        public GameObject GetDamageActionOfName(string actionName)
        {
            foreach(var action in DamageActions)
            {
                if (action.name == $"DamageAction：{actionName}")
                    return action;
            }
            return null;
        }

        public GameObject GetDamageAction(GameObject damageAction)
        {
            foreach (var action in DamageActions)
            {
                if (action == damageAction)
                    return action;
            }
            return null;
        }


    }
}