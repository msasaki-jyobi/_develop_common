using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class InstanceManager : SingletonMonoBehaviour<InstanceManager>
    {
        public string TargetPlayerName = "";
        public List<InstanceInfo> InstanceInfos = new List<InstanceInfo>();
        public void ChangeKeyNameActive(string keyName)
        {
            foreach (var info in InstanceInfos)
            {
                string targetInfoKeyName = info.KeyName;
                string targetkeyName = keyName;
                if (info.IsTargetName) // KayName�̌��m��TargetName�̕t�^���K�v�ȏꍇ
                    targetkeyName += TargetPlayerName; // KeyName + TargetName�ɂ���

                if (info.KeyName == targetkeyName)
                {
                    info.TargetObject.SetActive(info.IsSetActive);
                }
            }
        }
    }

    [System.Serializable]
    public class InstanceInfo
    {
        public string KeyName;
        public bool IsTargetName; // KeyName��TargetName�̎w�肪�K�v
        public bool IsSetActive;
        public GameObject TargetObject;
    }
}