using System.Collections;
using UnityEngine;

namespace develop_common
{
    [AddComponentMenu("ActionStart：開始時")]
    public class ActionStart : MonoBehaviour
    {
        [Header("開始時：モーション関連")]
        // 再生モーション
        [Tooltip("再生するモーション")]
        public string PlayClip;
        // ApplyRootMotion
        [Tooltip("モーションに座標移動・回転があればキャラクターのTransformに反映させる")]
        public bool IsApplyRootMotion;
        // モーション繰り返しの有無
        [Tooltip("モーションの繰り返し情報（基本Singleで良い）")]
        public EStatePlayType StatePlayType;
        // 同じモーションでも繰り返し行うか
        [Tooltip("今再生中、前回再生中のモーションが同じでも、最初からモーションを再生するか？")]
        public bool IsStateReset;
        [Header("開始時：UnitStatus変更")]
        [Tooltip("開始時に実行者のUnitActionLoaderの状態が変更される")]
        // 開始時に切り替える状態
        public EUnitStatus SetStartStatus;
        [Header("開始時：Velocity Reset")]
        [Tooltip("開始時に移動や吹き飛び、落下などの速度をリセットする")]
        // Velocity Reset
        public bool IsResetVelocity;
        [Tooltip("実行時に親オブジェクトを初期状態に戻す（固定化を外したりなど）")]
        // Velocity Reset
        public bool IsParentEntity; // 親オブジェクトをデフォルトに戻す

        // レイヤー用
        [Header("レイヤー指定がある場合")]
        [Tooltip("AvatarMaskを使ったレイヤーを再生したい場合に使う Layer0以外を指定された時に機能がONになる")]
        public int AnimatorLayer = 0;
        public float WeightValue = 1;
        public float WeightTime = 0.5f;
    }
}