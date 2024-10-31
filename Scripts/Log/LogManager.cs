using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class LogManager : SingletonMonoBehaviour<LogManager>
    {
        [SerializeField] private bool _isPlayLog;
        [SerializeField] private bool _isCollapse;

        [Header("登録優先度")]
        [SerializeField] private int _priority = 0;
        [Header("表示したいオブジェクト（0件ですべて表示）")]
        [SerializeField] private List<GameObject> _enableGameObjects = new List<GameObject>();
        [Header("自動追加")]
        [SerializeField] private List<GameLog> _gameLogs = new List<GameLog>();

        public void AddLog(GameObject logObject, string log, int priority = 0)
        {
            // 機能OFFならReturn
            if (!_isPlayLog) return;

            // 表示優先度が低いならReturn
            if (priority < _priority) return;

            // 指定したオブジェクトだけログに追加する場合
            if (_enableGameObjects.Count > 0)
            {
                bool check = false;
                foreach (GameObject go in _enableGameObjects)
                    if (go == logObject) check = true; // 一致オブジェクトなら進行
                if (!check) return;
            }

            // 重複する場合カウントするだけなら
            if (_isCollapse)
                foreach (var glog in _gameLogs)
                {
                    if (glog.Target == logObject)
                        if (glog.LogText == log)
                        {
                            glog.Count++;
                            return;
                        }
                }

            // 新たに追加
            var gameLog = new GameLog();
            gameLog.Target = logObject;
            gameLog.LogText = log;
            _gameLogs.Add(gameLog);
        }

        public void ConsoleLog(GameObject logObject, string log, int priority = 0)
        {
            // 機能OFFならReturn
            if (!_isPlayLog) return;

            // 表示優先度が低いならReturn
            if (priority < _priority) return;

            // 指定したオブジェクトだけログに追加する場合
            if (_enableGameObjects.Count > 0)
            {
                bool check = false;
                foreach (GameObject go in _enableGameObjects)
                    if (go == logObject) check = true; // 一致オブジェクトなら進行
                if (!check) return;
            }

            // Debug
            Debug.Log($"Obj:{logObject} ::: ${log}");
        }

    }
    [Serializable]
    public class GameLog
    {
        public GameObject Target;
        public string LogText;
        public int Count;
    }
}