using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PrefsSave : MonoBehaviour
{
    [Header("�Z�[�uNo�i���Əd�����Ȃ��ԍ���U�낤")]
    public int SavePointNum = 0;
    [Header("�����蔻������m����I�u�W�F�N�g��")]
    public string TargetName = "cat";
    [Header("�f�o�b�O�p�F�Z�[�u�f�[�^���Z�b�g")]
    public bool IsSaveReset;

    private GameObject player;
    private string _saveKey = "SavePoint";

    // Start����������x�����Ă΂��
    private void Awake() 
    {
        // player�I�u�W�F�N�g���擾
        player = GameObject.Find(TargetName);

        // �R���|�[�l���g�̎����ݒ�
        if (TryGetComponent<BoxCollider2D>(out var collider))
            collider.isTrigger = true;
        if (TryGetComponent<Rigidbody2D>(out var rigid))
            rigid.isKinematic = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsSaveReset) // �Z�[�u���Z�b�g
            PlayerPrefs.SetInt(_saveKey, 0); // �Z�[�u�f�[�^���㏑��

        // �ۑ�����Ă���l���擾
        var save = PlayerPrefs.GetInt(_saveKey, 0); // �Z�[�u�f�[�^�����݂��Ȃ��ꍇ 0 

        // �Z�[�u�f�[�^����v����ꍇ�APlayer�������̏ꏊ�Ɉړ�
        if (save == SavePointNum)
            player.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnHit(other.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnHit(collision.gameObject);
    }

    private void OnHit(GameObject hit)
    {
        if (hit.gameObject == player) // �G�ꂽ�I�u�W�F�N�g��Player�Ȃ�
        {
            PlayerPrefs.SetInt(_saveKey, SavePointNum); // �Z�[�u���s
            Debug.Log($"Save���������܂���. {SavePointNum}");
        }
    }
}