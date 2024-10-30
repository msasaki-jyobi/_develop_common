using develop_common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfos : MonoBehaviour
{
    [Header("�������N���X����")]

    [Header("DamageCatalog�i�S�̊Ǘ��j")]
    public List<stringDamageValue> Catalog;
    // �ł��グ1�F
    // �ł��グ2�F
    // �n�ʖ��܂�F
    // �m�[�}���_���[�W�F
    // ���ʕʃ_���[�W�i���j�F
    // DDT_vic(�C���[�W�Ƃ��Ă� "�Đ����邾��" �F
    // �������F�����p�^�p�^
    // �������F�������p�^�p�^


    [Header("Unit")]
    public List<stringDamageDealer> Wepons;

    [Header("DamageDealer Collider L-Hand")]
    public TestDamageDealer TestDamageDealer;



    // DamageCatalog (�S�̂ŋ��L�j EDamageMode
    // ModeA: Additive ���̃��j�b�g�̉e���@[]Normal []Catch []Down
    // ModeB: DamageA
    // ModeC: �ł��グA
    // ModeD: �ł��グB
    // ModeE: �ł��グC

    // DamageModeInfo
    // EDamageMode
    // DamagValue


    // �ϐ�
    // �_���[�W����������HitCollider���X�g string:EDamageMode
    // 


}
public enum EDamageMode
{
    Additive,
    Normal,
    Catch,
}
public class MO
{
    public EDamageMode DamageMode;
}
[Serializable]
public class stringDamageValue
{
    public string KeyName;
    public DamageValueScriptable DamageValueScriptable;
}
[Serializable]
public class stringDamageDealer
{
    public string KeyName;
    public DamageDealer DamageDealer;
    // ���S���̃��A�N�V����
}
[Serializable]
public class TestDamageDealer
{
    public DamageValueScriptable DamageValueScriptable;
    public int KisoDamageWeight;
}
