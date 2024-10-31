//using develop_common;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DamageInfos : MonoBehaviour
//{
//    [Header("お試しクラスたち")]

//    [Header("DamageCatalog（全体管理）")]
//    public List<stringDamageValue> Catalog;
//    // 打ち上げ1：
//    // 打ち上げ2：
//    // 地面埋まり：
//    // ノーマルダメージ：
//    // 部位別ダメージ（小）：
//    // DDT_vic(イメージとしては "再生するだけ" ：
//    // くっつき：仰向けパタパタ
//    // くっつき：うつ伏せパタパタ


//    [Header("Unit")]
//    public List<stringDamageDealer> Wepons;

//    [Header("DamageDealer Collider L-Hand")]
//    public TestDamageDealer TestDamageDealer;



//    // DamageCatalog (全体で共有） EDamageMode
//    // ModeA: Additive 他のユニットの影響　[]Normal []Catch []Down
//    // ModeB: DamageA
//    // ModeC: 打ち上げA
//    // ModeD: 打ち上げB
//    // ModeE: 打ち上げC

//    // DamageModeInfo
//    // EDamageMode
//    // DamagValue


//    // 変数
//    // ダメージが発生するHitColliderリスト string:EDamageMode
//    // 


//}
//public enum EDamageMode
//{
//    Additive,
//    Normal,
//    Catch,
//}
//public class MO
//{
//    public EDamageMode DamageMode;
//}
//[Serializable]
//public class stringDamageValue
//{
//    public string KeyName;
//    public DamageValueScriptable DamageValueScriptable;
//}
//[Serializable]
//public class stringDamageDealer
//{
//    public string KeyName;
//    public DamageDealer DamageDealer;

//}
//[Serializable]
//public class TestDamageDealer
//{
//    public DamageValueScriptable DamageValueScriptable;
//    public int KisoDamageWeight;
//}

