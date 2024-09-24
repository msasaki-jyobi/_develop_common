using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
namespace develop_common
{
    [Serializable]
    public class ThrowInfo
    {
        public string ThrowActionData; // モーション名とFrame情報だけ参照する
        public int ThrowID; // 投げ技ID
        public int DownID; // ダウンID
        public string frames; // A1,A3,C11,C22 とかだと楽 ,区切りで取得とか
    }
}
