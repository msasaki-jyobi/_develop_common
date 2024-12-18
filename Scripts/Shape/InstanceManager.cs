using develop_timeline;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace develop_common
{
    public class InstanceManager : MonoBehaviour
    {
        [Header("使わない")]
        public string CostumeName = "：デフォ";
        public List<InstanceInfo> InstanceInfos = new List<InstanceInfo>();

        [Header("YES")]
        public UnitEnableController UnitEnableController;
        public List<EasyInstanceInfo> CountInstanceInfo = new List<EasyInstanceInfo>();


        public event Func<string, bool> CheckReturnKeyEvent;


        private void Start()
        {
        }

        private void OnDestroy()
        {

        }

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
                    bool check = false;
                    if (info.IsReturnCheck)
                    {
                        check = CheckReturnKeyEvent?.Invoke(info.IsReturnKeyName) ?? false;
                    }


                    if (!check)
                    {
                        foreach (var info2 in info.ActiveObjects)
                            info2.SetActive(true);
                        foreach (var info3 in info.NotActiveObjects)
                            info3.SetActive(false);
                    }
                }
            }
        }

        public void OnChangeInstance(string keyName)
        {
            foreach (var info in CountInstanceInfo)
            {
                // 例：ダイヤマークキー削除
                var getKeyA = ""; // キーワード名 例：ダイヤマークキー
                var getKeyB = ""; // 削除 or 再生 例：削除
                ParseString(keyName, out getKeyA, out getKeyB);

                if (info.KeyName == getKeyA)
                {
                    if (getKeyB == "削除")
                    {
                        for (int i = 0; i < info.ActiveObjects.Count; i++) // 最短オブジェクトを表示
                        {
                            if (info.ActiveObjects[i] != null)
                                if (info.ActiveObjects[i].activeInHierarchy)
                                {
                                    UnitEnableController.OnChangeActiveObject(info.ActiveObjects[i], false); // 最短を削除
                                    if (info.NotActiveObjects[i] != null)
                                        if (!info.NotActiveObjects[i].activeInHierarchy)
                                            UnitEnableController.OnChangeActiveObject(info.NotActiveObjects[i], true); // 同じ要素のNotActiveを表示

                                    return;
                                }

                        }
                    }
                    else if (getKeyB == "再生")
                    {
                        for (int i = info.ActiveObjects.Count - 1; i >= 0; i--) // 最後のオブジェクトから見て非表示なら表示する
                        {
                            if (info.ActiveObjects[i] != null)
                                if (!info.ActiveObjects[i].activeInHierarchy)
                                {
                                    UnitEnableController.OnChangeActiveObject(info.ActiveObjects[i], true); // 最後尾から表示
                                    if (info.NotActiveObjects[i] != null)
                                        if (info.NotActiveObjects[i].activeInHierarchy)
                                            UnitEnableController.OnChangeActiveObject(info.NotActiveObjects[i], false); // 同じ要素のNotActiveを非表示
                                    return;
                                }
                        }
                    }
                }
            }

        }
        public void ParseString(string input, out string getA, out string getB)
        {
            // 判定するキーワードリスト
            string[] keywords = { "削除", "再生" };

            // 初期化
            getA = input;
            getB = string.Empty;

            foreach (string keyword in keywords)
            {
                if (input.EndsWith(keyword))
                {
                    getB = keyword;
                    getA = input.Substring(0, input.Length - keyword.Length);
                    break;
                }
            }
        }

    }

    [System.Serializable]
    public class InstanceInfo
    {
        public string KeyName;
        [Header("：<KeyName>が一致する場合のみにする")]
        public bool IsCostumeName; // KeyNameにTargetNameの指定が必要
        public bool IsReturnCheck; // 一致するキーがあったらActibve処理を実行せずReturn
        public string IsReturnKeyName; // Returnするキーネーム
        public List<GameObject> ActiveObjects;
        public List<GameObject> NotActiveObjects;
    }

    [System.Serializable]
    public class EasyInstanceInfo
    {
        public string KeyName;
        public List<GameObject> ActiveObjects;
        public List<GameObject> NotActiveObjects;
    }
}
