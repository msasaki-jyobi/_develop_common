using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{

    public class StageGenerateManager : MonoBehaviour
    {
        // �I�u�W�F�N�g���ɃT�C�Y�ݒ�ł���Ɨǂ������B�@���ׂă����_���z�u�ݒ�B
        public bool IsIgnoreSubmit;
        public List<GameObject> GenerateObjects = new List<GameObject>();
        public Vector3 SetSize = Vector3.one;
        
        
        
        private GameObject _submitPrefab;



        private void Awake()
        {
            int ran = Random.Range(0, GenerateObjects.Count);
            _submitPrefab = GenerateObjects[ran];

            foreach(var ch in transform.GetComponentsInChildren<Transform>())
            {
                if (transform == ch) continue;

                var submitObject = !IsIgnoreSubmit ? _submitPrefab : GenerateObjects[Random.Range(0, GenerateObjects.Count)];
                var parent = ch;
                var prefab = Instantiate(submitObject, ch.transform.position, ch.transform.rotation);
                prefab.transform.parent = parent;
                prefab.transform.localScale = SetSize;
            }
        }
    }
}