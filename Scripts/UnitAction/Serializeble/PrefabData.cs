using System.Collections;
using UnityEngine;

namespace develop_common
{
    public enum ELookType
    {
        Origin,
        Camera
    }
    public enum EParentType // 親オブジェクトを設定するかどうか？
    {
        SetParent,
        NotParent
    }
    /// <summary>
    /// 生成するプレハブに情報を付加できるクラス
    /// </summary>
    [System.Serializable]
    public class PrefabData
    {
        [Tooltip("名前")] public string Title;
        [Space(5)][Tooltip("生成するプレハブ")] public GameObject Prefab;
        [Space(5)][Tooltip("生成音")] public AudioClip CreateSe;
        public float DestroyTime = 1f;
        //[Tooltip("キャラ毎に設定された生成場所")] public EBodyPoint BodyPoint;
        [Tooltip("生成ローカル座標")] public Vector3 LocalPosition;
        [Tooltip("生成ローカル回転値")] public Vector3 LocalEulerAngle;
        [Tooltip("生成ローカルスケール")] public Vector3 SetScale = Vector3.one;
        [Tooltip("オブジェクトの向く方向")] public ELookType LookType;
        [Tooltip("親オブジェクトを設定するか？")] public EParentType ParentType;
        [Space(5)][Tooltip("InstanceManager 親オブジェクトのKey")] public string ParentKeyName; // InstanceManager

      
    }
}