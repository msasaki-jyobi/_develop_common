using Cysharp.Threading.Tasks;
using develop_easymovie;
using develop_timeline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class FlgManager : SingletonMonoBehaviour<FlgManager>
    {

        // ゲーム全体のトリガー情報。　トリガーに応じてオブジェクトの表示・非表示や差し替えを行う　アイテム持っていたら？　持っていなかったら？のやつどうすっぺ。 GetItem とか LostItemとか？
        public List<FlgTrigger> AllTriggers = new List<FlgTrigger>();
        [Space(10)]
        public List<string> Flgs = new List<string>();
        public List<string> Triggers = new List<string>();

        private void Start()
        {
            LoadAllTriggers();
        }

        /// <summary>
        /// トリガーを実行する
        /// </summary>
        /// <param name="triggerName">トリガー名</param>
        /// <param name="hasRunOnce">一度でも再生したトリガー（リストに追加済み）ならRetrunする</param>
        public async void LoadTrigger(string triggerName, bool hasRunOnce = false)
        {
            bool check = AddTrigger(triggerName);
            if (hasRunOnce && !check)
            {
                // 実行しない
                return;
            }


            foreach (var triggers in AllTriggers)
            {
                if (triggers.TriggerName == triggerName) // それぞれ実行を行う
                {
                    foreach (var visible in triggers.VisibleEMObjects) // 表示
                        visible.gameObject.SetActive(true);

                    foreach (var hidden in triggers.HiddenEMObjects) // 非表示
                        hidden.gameObject.SetActive(false);

                    foreach (var enable in triggers.EnabledEMObjects) // 実行可能
                        enable.IsHidden = false;

                    foreach (var disabled in triggers.DisabledEMObjects) // 実行不可
                        disabled.IsHidden = true;

                    //if (triggers.AutoPlayEMObject != null) // EM自動再生
                    //    DirectorManager.Instance.PlayEasyMovie(triggers.AutoPlayEMObject);

                    foreach(var replacement in triggers.ReplacementInfos)
                    {
                        if (replacement.ReplacementEMObject != null) // EM差し替え
                        {
                            if (replacement.ReplacementEMObject.EasyMoviePlayer != null) // Movieがあれば差し替える
                            {
                                replacement.ReplacementEMObject.EasyMoviePlayer = replacement.ReplacementEMData;
                            }
                            else // ムービーがなければメッセージとTriggerを差し替える
                            {
                                if (replacement.ReplacementEMMessage != null)
                                    replacement.ReplacementEMObject.EnterMessage = replacement.ReplacementEMMessage;
                                if (replacement.ReplacementEMTrigger != null)
                                    replacement.ReplacementEMObject.EnterTrigger = replacement.ReplacementEMTrigger;
                            }
                        }
                    }

                    int delayTime = 0;
                    if (triggers.TriggerName != "") // 目標の更新
                    {
                        delayTime += 3;
                        TextFadeController.Instance.UpdateMileStone(triggers.TriggerName);
                    }

                    if (triggers.Message != "") // Message
                    {
                        await UniTask.Delay(delayTime * 1000);
                        TextFadeController.Instance.UpdateMessageText(triggers.Message);
                    }

                    if (triggers.AddFlg != "")
                        FlgManager.Instance.AddFlg(triggers.AddFlg);
                }
            }
        }

        /// <summary>
        /// ゲームロード時など、過去のトリガーをすべて発動させる
        /// </summary>
        public async void LoadAllTriggers()
        {
            foreach (var trigger in Triggers)
            {
                LoadTrigger(trigger);
                await UniTask.Delay(10);
            }
        }

        public bool CheckFlg(string flgName)
        {
            bool check = false;

            foreach (var flg in Flgs)
                if (flg == flgName)
                    check = true;

            return check;
        }

        public void AddFlg(string flgName)
        {
            for (int i = 0; i < Flgs.Count; i++)
            {
                if (Flgs[i] == flgName)
                {
                    Debug.Log($"すでに存在するフラグ:{flgName}");
                    return;
                }
            }

            Debug.Log($"新規追加フラグ:{flgName}");
            Flgs.Add(flgName);
        }

        public bool AddTrigger(string triggerName)
        {
            for (int i = 0; i < Triggers.Count; i++)
            {
                if (Triggers[i] == triggerName)
                {
                    Debug.Log($"すでに存在するトリガー:{triggerName}");
                    return false; // 新規追加は中止された
                }
            }

            Debug.Log($"新規追加トリガー:{triggerName}");
            Triggers.Add(triggerName);
            return true; // 新規追加できた
        }
    }
}