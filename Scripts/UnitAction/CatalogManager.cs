using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class CatalogManager : SingletonMonoBehaviour<CatalogManager>
    {

        public List<DamageCatalog> DamageCatalogs;
        public List<DownCatalog> DownCatalogs;
        public List<RecoveryCatalog> RecoveryCatalogs;
        public List<ThrowCatalog> ThrowCatalogs;

        // �_���[�W�J�^���O���X�g{ �ʏ�_���[�W, �ɂ���_���[�W, �j��_���[�W}
        // �_�E���J�^���O���X�g{ �ʏ�_�E��, �����_�E��, ���X���[�n�_�E���A����_�E��}
        // �����J�^���O���X�g{ ���X���[�n, �\�[�h�n, �A�C�e�������n}
        // ���J�o���[�J�^���O���X�g{ �ʏ�N���オ��, �������N���オ��, ����N���オ��}


        public DamageInfo GetDamageInfo(int id)
        {
            foreach (var Catalog in DamageCatalogs)
                foreach (var info in Catalog.Damages)
                    if (info.DamageID == id)
                        return info;

            Debug.LogError($"Damage:{id} �͑��݂��܂���. ");
            return null;
        }

        public DownInfo GetDownInfo(int id)
        {
            foreach (var Catalog in DownCatalogs)
                foreach (var info in Catalog.Downs)
                if (info.DownID == id)
                    return info;

            Debug.LogError($"Down:{id} �͑��݂��܂���. ");
            return null;
        }

        public ThrowInfo GetThrowInfo(int id)
        {
            foreach (var Catalog in ThrowCatalogs)
                foreach (var info in Catalog.Throws)
                if (info.ThrowID == id)
                    return info;

            Debug.LogError($"Throw:{id} �͑��݂��܂���. ");
            return null;
        }

        public RecoveryInfo GetRecoveryInfo(int id)
        {
            foreach (var Catalog in RecoveryCatalogs)
                foreach (var info in Catalog.Recoverys)
                if (info.RecoveryID == id)
                    return info;

            Debug.LogError($"Recovery:{id} �͑��݂��܂���. ");
            return null;
        }
    }
}