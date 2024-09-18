using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace develop_common
{
    public class UIStateManager : MonoBehaviour
    {
        public List<UIStateInfo> States = new List<UIStateInfo>();
        public List<UIButtonInfo> ButtonInfos = new List<UIButtonInfo>();

        [SerializeField] private bool AutoFirstFocus;
        [SerializeField] private bool AutoStartUIState;
        [SerializeField] private string _startUIStateName;
        
        // ステート開いた時に呼び出すイベント
        public event Action<string> ChangeStateEvent;

        private Button _firstFocusButton;
        private string _currentStateName = "";


        private void Start()
        {
            if (AutoStartUIState)
                OnChangeStateAndButtons(_startUIStateName);
        }

        private void Update()
        {
            FocusCheck();
        }


        /// <summary>
        /// 指定されたUIステートのパーツのみ画面表示 
        /// </summary>
        /// <param name="stateName"></param>
        public void ChangeUIState(string stateName)
        {
            UIStateInfo uiStateInfo = null;
            // いったんすべてOFF
            for (int i = 0; i < States.Count; i++)
            {
                foreach (var content in States[i].UIContents)
                    content.gameObject.SetActive(false);
                if (States[i].StateName == stateName)
                    uiStateInfo = States[i];
            }
            // 該当のUIだけ表示
            foreach (var content in uiStateInfo.UIContents)
                content.gameObject.SetActive(true);
            // State Save
            _currentStateName = uiStateInfo.StateName;
            // イベント呼び出し
            ChangeStateEvent?.Invoke(stateName);

        }
        /// <summary>
        /// 指定されたボタンの押下できる状態を切り替え
        /// </summary>
        public void ChangeButtonInteractable(string interactableStateName)
        {
            var buttons = new List<Button>();

            // いったんすべてOFF
            for (int i = 0; i < ButtonInfos.Count; i++)
            {
                foreach (var button in ButtonInfos[i].EnableButtons)
                    button.interactable = false;
                if (ButtonInfos[i].InteractableStateName == interactableStateName)
                    buttons = ButtonInfos[i].EnableButtons;
            }
            // 該当のボタンはON
            foreach (var enablebutton in buttons)
                enablebutton.interactable = true;
            // フォーカスも設定
            if(buttons != null && buttons.Count != 0)
            {
                _firstFocusButton = buttons[0];
                EventSystem.current.SetSelectedGameObject(_firstFocusButton.gameObject);
            }

        }

        public string GetCurrentStateName()
        {
            return _currentStateName;
        }

        /// <summary>
        /// UnityEvent用：ステート・ボタン共に切り替え
        /// </summary>
        /// <param name="interactableStateName"></param>
        public void OnChangeStateAndButtons(string stateName)
        {
            ChangeUIState(stateName);
            ChangeButtonInteractable(stateName);
        }
        /// <summary>
        /// UnityEvent用：ボタンのInteractableのみ切り替え
        /// </summary>
        /// <param name="interactableStateName"></param>
        public void OnOnlyChangeButtonInteractable(string interactableStateName)
        {
            ChangeButtonInteractable(interactableStateName);
        }



        private void FocusCheck()
        {
            if (AutoFirstFocus)
                if (EventSystem.current.currentSelectedGameObject == null)
                    if (_firstFocusButton != null)
                        if (_firstFocusButton.gameObject.activeSelf)
                            if (_firstFocusButton.interactable)
                                EventSystem.current.SetSelectedGameObject(_firstFocusButton.gameObject);
                            else
                                Debug.LogWarning($"Log {_firstFocusButton.gameObject.name} の Interactable は Disabled です.");
                        else
                            Debug.LogWarning($"Log {_firstFocusButton.gameObject.name} は 非表示です.");
                    else
                        Debug.LogWarning($"Log _firstFocusButton は Nullです.");
        }
    }



}
