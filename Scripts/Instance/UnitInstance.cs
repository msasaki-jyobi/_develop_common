using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class UnitInstance : MonoBehaviour
    {
        [Header("Body参照用(Tooltip詳細)")]
        [Tooltip("Timeline, PartAttachmentの固定先, IKの参照先")]
        public List<StringKeyGameObjectValuePair> InstanceBodys = new List<StringKeyGameObjectValuePair>();
       
        [Header("追加機能：Action時 カメラ切り替え")]
        public List<CinemachineVirtualCamera> RandomCameras = new List<CinemachineVirtualCamera>();


        public GameObject SearchObject(string keyName)
        {
            foreach(var body in InstanceBodys) 
            {
                if (body.Key == keyName)
                    return body.Value;
            }
            return null;
        }
        public void CreateObject(string keyName, GameObject prefab, float destroyTime)
        {
            foreach (var body in InstanceBodys)
            {
                if (body.Key == keyName)
                {
                    var ef = Instantiate(prefab, body.Value.transform.position, Quaternion.identity);
                    Destroy(ef, destroyTime);

                }
            }
        }
    }
}