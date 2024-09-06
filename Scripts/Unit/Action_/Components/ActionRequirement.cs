using System.Collections;
using UnityEngine;

namespace GameSet.Common
{
    public enum EUnitStatus
    {
        None,       // 未定義の状態
        Ready,      // 操作可能状態
        Executing   // アクション実行中
    }

    public class ActionRequirement : MonoBehaviour
    {
        // 再生するのに必要なトリガーアクション
        public GameObject TriggerAction;
        // 再生に必要なステータス
        public EUnitStatus TriggerStatus;
        // 追加入力状態の有無
        public bool IsNextAction;

        public bool CanExecute(GameObject unit)
        {
            bool check = true;

            if (unit.TryGetComponent<UnitActionLoader>(out var actionLoader))
            {
                // トリガーアクションは実行されているか？
                if (TriggerAction != null)
                    check = check && actionLoader.ActiveAction == TriggerAction;

                // ステートは一致するか
                if (TriggerStatus != EUnitStatus.None)
                    check = check && actionLoader.UnitStatus == TriggerStatus;

                // 追加入力が可能な状態か
                if (IsNextAction)
                    check = check && actionLoader.IsNextAction;
            }
            // True:実行可能 False:実行不可
            return check;
        }



    }
}