using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class InstanceManager : SingletonMonoBehaviour<InstanceManager>
    {
        public UnitVoice UnitVoice;
        public UnitActionLoader UnitActionLoader;
        public UnitShape UnitShape;
        public List<StringKeyGameObjectValuePair> InstanceBodys = new List<StringKeyGameObjectValuePair>();

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
                    var ef = GameObject.Instantiate(prefab, body.Value.transform.position, Quaternion.identity);
                    Destroy(ef, destroyTime);

                }
            }
        }
    }
}