using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class UnitInstance : MonoBehaviour
    {
        [Header("Timeline—p")]
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
                    var ef = Instantiate(prefab, body.Value.transform.position, Quaternion.identity);
                    Destroy(ef, destroyTime);

                }
            }
        }
    }
}