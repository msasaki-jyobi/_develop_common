using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

namespace develop_common
{
    // 実行しているアクションの種類
    public enum EActionKinds
    {
        Action,
        Damage,
    }

    public enum ERequirements
    {
        None,
        StatePlay,
        StateAction,
        StateDamage,
        StateWallCatch,
        StateWallUp,
        AddKey,
        //InputDown_X,
        //InputDown_Up,
        //InputDown_Down,
        //HitPoint1,
        //HitPoint2,
        //HitPoint3,
        //HitPoint4,
        //HitPoint5,
        Fire1,
        //Fire2,
        //Fire3,
        //Fire4,
        //InputUp_X,
        //InputDown_Forward,
        //InputDown_Back,
        //Mode_Tps,
        //Mode_Battle,
        //StateCatchDamage_ExitChange,
        //Stamina20,
        //Stamina40,
        //Stamina60,
        //Stamina80,
        //Stamina100,
        //CancelFire2,
        JumpFire,
        CanJumpTrue,
        CanJumpFalse,
        CanWallTrue,
        CanWallFalse,
        CanCliffTrue,
        CanCliffFalse,
    }
    public enum EOption
    {
        None,
        VelocityReset,
        ApplyRootMotion,
        LookCamera,
        ChangeStatePlay,
        ChangeStateAction,
        ChangeStateDamage,
        ChangeStateWallCatch,
        DefaultSetMotionStates,
        DefaultSetSpeed,
        GuardTrue,
        GuardFalse,
        DefaultSetGuardValue,
        Voice_Action_A,
        Voice_Action_B,
        Voice_Action_C,
        Voice_Damage_A,
        Voice_Damage_B,
        Voice_Damage_C,
        DamageReset,
        SetDown, // Down状態をセット
        ChangeStateCatchDamage_ExitChange,
        Voice_Action_D,
        Voice_Action_E,
        Voice_Action_F,
        Voice_Damage_D,
        Voice_Damage_E,
        Voice_Damage_F,
    }
    [CreateAssetMenu(fileName = "ActionData", menuName = "GameSet / Action / UnitActionData")]
    public class UnitActionData : ScriptableObject
    {
        // アクションの種類
        public EActionKinds ActionKinds;
        // 再生モーション
        public string MotionName;
        // 再生モーションのレート
        public int MotionLate = 30;
        // 同じUnitActionDataを繰り返し実行できるようにする
        public bool ResetPlay;
        public bool IsMotionBodyIDAdd;
        // 実行に必要なアクションデータ
        public UnitActionData RequirementsActionData;
        // 実行に必要なその他条件
        public List<ERequirements> Requirements = new List<ERequirements>();
        // 実行オプション
        public List<EOption> Options = new List<EOption>();
        // 次に実行するActionData
        public UnitActionData NextActionData;
        // 攻撃対象に再生させるアクション
        public UnitActionData AttakerUnitActionData;
        public Vector3 OffSetPos;
        public Vector3 OffSetRot;
        // 当たり判定の位置に同期する用
        public Vector3 SyncOffSetPos; // そこからずらす値
        public Vector3 SyncOffSetRot; // そこからずらす値
                                      // UnitActionDataの文字列オプション
        public List<ActionOption> ActionOptions = new List<ActionOption>();
        // FrameInfo
        public List<FrameInfo> FrameInfos = new List<FrameInfo>();
    }
}