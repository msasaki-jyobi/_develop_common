using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class InstanceManager : MonoBehaviour
    {
        public string CostumeName = "：デフォ";
        public List<InstanceInfo> InstanceInfos = new List<InstanceInfo>();
        public void ChangeKeyNameActive(string keyName)
        {
            foreach (var info in InstanceInfos)
            {
                string targetInfoKeyName = info.KeyName;
                string targetkeyName = keyName;
                if (info.IsCostumeName) // KayNameの検知にTargetNameの付与が必要な場合
                    targetkeyName += $"{CostumeName}"; // KeyName + TargetNameにする

                Debug.Log($"targetInfoKeyName:{targetInfoKeyName}, targetkeyName:{targetkeyName}");

                if (info.KeyName == targetkeyName)
                {
                   foreach(var info2 in info.ActiveObjects)
                        info2.SetActive(true);
                    foreach (var info3 in info.NotActiveObjects)
                        info3.SetActive(false);
                }
            }
        }
    }

    [System.Serializable]
    public class InstanceInfo
    {
        public string KeyName;
        [Header("：xxxが一致する場合のみにする")]
        public bool IsCostumeName; // KeyNameにTargetNameの指定が必要
        public List<GameObject> ActiveObjects;
        public List<GameObject> NotActiveObjects;
    }
}
