using System.Collections;
using UnityEngine;

namespace develop_common
{
    public enum EUnitStatus
    {
        None,       // 未定義の状態
        Ready,      // 操作可能状態
        Executing   // アクション実行中
    }

    public class ActionRequirement : MonoBehaviour
    {
        [Header("条件：トリガーアクション")]
        // 再生するのに必要なトリガーアクション
        public GameObject TriggerAction;
        [Header("条件：トリガーUnitStatus")]
        // 再生に必要なステータス
        public EUnitStatus TriggerStatus;
        [Header("条件：キー入力受付中のみ")]
        // 追加入力状態の有無
        public bool IsWaitingForKey;

        public bool CheckExecute(UnitActionLoader actionLoader)
        {
            bool check = true;
            string errorMessage = "";

            // トリガーアクションは実行されているか？
            if (TriggerAction != null)
            {
                check = check && actionLoader.ActiveActionObject == TriggerAction;
                errorMessage += ", トリガーアクションは実行されていません. ";
            }
            // ステートは一致するか
            if (TriggerStatus != EUnitStatus.None)
            {
                check = check && actionLoader.UnitStatus == TriggerStatus;
                errorMessage += $", UnitStatusは一致しません. Trigger:{TriggerStatus}|Unit:{actionLoader.UnitStatus}";
            }
            // 追加入力が可能な状態か
            if (IsWaitingForKey)
            {
                check = check && actionLoader.IsNextAction;
                errorMessage += $", IsWaitingForKeyはFalseです.";
            }

            if(!check)
                Debug.Log($"実行できません. {gameObject.name} {actionLoader.name}, {errorMessage}");

            // True:実行可能 False:実行不可
            return check;
        }



    }
}