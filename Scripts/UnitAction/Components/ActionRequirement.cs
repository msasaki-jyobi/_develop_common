using develop_tps;
using System.Collections;
using UnityEngine;

namespace develop_common
{
    public enum EUnitStatus
    {
        Ready,      // 操作可能状態
        Executing,   // アクション実行中
        None,       // 未定義の状態
    }
    [AddComponentMenu("ActionRequirement：条件")]
    public class ActionRequirement : MonoBehaviour 
    {
        [Header("条件：トリガーアクション")] // 再生するのに必要なトリガーアクション
        public GameObject TriggerAction;

        [Header("条件：トリガーUnitStatus")] // 再生に必要なステータス
        public EUnitStatus TriggerStatus;

        [Header("条件：キー入力受付中のみ")]
        public bool IsWaitingForKey;

        [Header("条件：地面にいる時のみ")]
        public bool IsGround;

        [Header("条件：空中にいる時のみ")]
        public bool IsAir;

        [Header("条件：指定キーが入力されているか？")]
        public EInputReader Key = EInputReader.None;

        public bool CheckExecute(UnitActionLoader actionLoader, EInputReader key)
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

            // 地面チェック
            if (IsGround)
                if (actionLoader.TryGetComponent<UnitGround>(out var ground))
                    check = check && ground.CanJump;

            // 空中チェック
            if (IsAir)
                if(actionLoader.TryGetComponent<UnitGround>(out var ground))
                        check = check && !ground.CanJump;

            // キーチェック
            if (Key != EInputReader.None)
                check = check && Key == key;

            // Log
            if(!check)
                Debug.Log($"実行できません. {gameObject.name} {actionLoader.name}, {errorMessage}");

            // True:実行可能 False:実行不可
            return check;
        }



    }
}