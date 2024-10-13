using develop_easymovie;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [Serializable]
    public class FlgTrigger
    {
        // トリガー名
        public string TriggerName;
        // オブジェクトごと表示されるEMオブジェクト
        public List<SwitchOutlinable> VisibleEMObjects = new List<SwitchOutlinable>();
        // オブジェクトごと非表示になるEMオブジェクト
        public List<SwitchOutlinable> HiddenEMObjects = new List<SwitchOutlinable>();

        // 実行可能になるEMオブジェクト
        public List<SwitchOutlinable> EnabledEMObjects = new List<SwitchOutlinable>();
        // 実行不可になるEMオブジェクト
        public List<SwitchOutlinable> DisabledEMObjects = new List<SwitchOutlinable>();
        // 自動実行されるEMオブジェクト たぶんこれが紛らわしい
        //public EasyMoviePlayer AutoPlayEMObject;

        // 差し替えるEMオブジェクト
        public SwitchOutlinable ReplacementEMObject;
        // 差し替えるEMデータ
        public EasyMoviePlayer ReplacementEMData;
        // 差し替えるEMデータ
        public string ReplacementEMMessage;
        // 差し替えるEMデータ
        public string ReplacementEMTrigger;

        [Space(8)]
        // 差し替える目標名
        //public string ChangeMileStone;
        // Message
        public string Message;
        // 追加フラグ
        public string AddFlg;

    }
}