using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // �v���C���[��֘A�I�u�W�F�N�g��j�����Ȃ�
    }

}