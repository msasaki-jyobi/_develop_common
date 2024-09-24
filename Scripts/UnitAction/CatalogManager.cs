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

        // ダメージカタログリスト{ 通常ダメージ, 痛がりダメージ, 嗚咽ダメージ}
        // ダウンカタログリスト{ 通常ダウン, 混乱ダウン, レスラー系ダウン、特殊ダウン}
        // 投げカタログリスト{ レスラー系, ソード系, アイテム召喚系}
        // リカバリーカタログリスト{ 通常起き上がり, 投げられ起き上がり, 特殊起き上がり}


        public DamageInfo GetDamageInfo(int id)
        {
            foreach (var Catalog in DamageCatalogs)
                foreach (var info in Catalog.Damages)
                    if (info.DamageID == id)
                        return info;

            Debug.LogError($"Damage:{id} は存在しません. ");
            return null;
        }

        public DownInfo GetDownInfo(int id)
        {
            foreach (var Catalog in DownCatalogs)
                foreach (var info in Catalog.Downs)
                if (info.DownID == id)
                    return info;

            Debug.LogError($"Down:{id} は存在しません. ");
            return null;
        }

        public ThrowInfo GetThrowInfo(int id)
        {
            foreach (var Catalog in ThrowCatalogs)
                foreach (var info in Catalog.Throws)
                if (info.ThrowID == id)
                    return info;

            Debug.LogError($"Throw:{id} は存在しません. ");
            return null;
        }

        public RecoveryInfo GetRecoveryInfo(int id)
        {
            foreach (var Catalog in RecoveryCatalogs)
                foreach (var info in Catalog.Recoverys)
                if (info.RecoveryID == id)
                    return info;

            Debug.LogError($"Recovery:{id} は存在しません. ");
            return null;
        }
    }
}
