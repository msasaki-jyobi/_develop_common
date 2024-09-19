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

        // �X�e�[�g�J�������ɌĂяo���C�x���g
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
        /// �w�肳�ꂽUI�X�e�[�g�̃p�[�c�̂݉�ʕ\�� 
        /// </summary>
        /// <param name="stateName"></param>
        public void ChangeUIState(string stateName)
        {
            UIStateInfo uiStateInfo = null;
            // �������񂷂ׂ�OFF
            for (int i = 0; i < States.Count; i++)
            {
                foreach (var content in States[i].UIContents)
                    content.gameObject.SetActive(false);
                if (States[i].StateName == stateName)
                    uiStateInfo = States[i];
            }
            // �Y����UI�����\��
            foreach (var content in uiStateInfo.UIContents)
                content.gameObject.SetActive(true);
            // State Save
            _currentStateName = uiStateInfo.StateName;
            // �C�x���g�Ăяo��
            ChangeStateEvent?.Invoke(stateName);

        }
        /// <summary>
        /// �w�肳�ꂽ�{�^���̉����ł����Ԃ�؂�ւ�
        /// </summary>
        public void ChangeButtonInteractable(string interactableStateName)
        {
            var buttons = new List<Button>();

            // �������񂷂ׂ�OFF
            for (int i = 0; i < ButtonInfos.Count; i++)
            {
                foreach (var button in ButtonInfos[i].EnableButtons)
                    button.interactable = false;
                if (ButtonInfos[i].InteractableStateName == interactableStateName)
                    buttons = ButtonInfos[i].EnableButtons;
            }
            // �Y���̃{�^����ON
            foreach (var enablebutton in buttons)
                enablebutton.interactable = true;
            // �t�H�[�J�X���ݒ�
            if (buttons != null && buttons.Count != 0)
            {
                _firstFocusButton = buttons[0];
                EventSystem.current.SetSelectedGameObject(_firstFocusButton.gameObject);
            }

        }
        /// <summary>
        /// ���݊J���Ă���UI�X�e�[�g�����擾
        /// </summary>
        /// <returns></returns>
        public string GetCurrentStateName()
        {
            return _currentStateName;
        }

        /// <summary>
        /// �{�^����ButtonInfos�ɒǉ�����iContent�ւ̒ǉ��Ȃǂ͕ʃ��W�b�N�j
        /// </summary>
        /// <param name="interactableStateName">�{�^��State��</param>
        /// <param name="targetButton">�ǉ��ΏۂƂȂ�{�^��</param>
        /// <param name="checkForDuplicates">�d���`�F�b�N�i�d�����͉��������I���j</param>
        public void AddInteractableButton(string interactableStateName, Button targetButton, bool checkForDuplicates = false)
        {
            List<Button> buttons = new List<Button>();

            for (int i = 0; i < ButtonInfos.Count; i++)
            {
                if (ButtonInfos[i].InteractableStateName == interactableStateName) // ��v�F�X�e�[�g��
                {
                    buttons = ButtonInfos[i].EnableButtons; // �ێ��F�{�^�����X�g
                    // �d���`�F�b�N
                    if (checkForDuplicates)
                        foreach (var button in ButtonInfos[i].EnableButtons)
                            if (button.name == targetButton.name) // ��v�F�{�^����
                                return; // �I��

                }
            }
            // �{�^���ǉ�
            buttons.Add(targetButton);
        }

        public void RemoveInteractableButton(string interactableStateName, string buttonName, bool isSingleRemove = true)
        {
            for (int i = 0; i < ButtonInfos.Count; i++)
            {
                if (ButtonInfos[i].InteractableStateName == interactableStateName) // ��v�F�X�e�[�g��
                {
                    for(int j = 0; j < ButtonInfos[i].EnableButtons.Count;j++) // �����F�{�^�����X�g
                    {
                        if (ButtonInfos[i].EnableButtons[j].name == buttonName) // ��v�F�{�^����
                        {
                            var removeButton = ButtonInfos[i].EnableButtons[j];
                            ButtonInfos[i].EnableButtons.Remove(removeButton); // �폜�F���X�g����
                            Destroy(removeButton); // �폜�F�I�u�W�F�N�g

                            if (isSingleRemove) // ����F1��̂�
                                return; // �I��
                        }
                    }
                }
            }
        }

        /// <summary>
        /// UnityEvent�p�F�X�e�[�g�E�{�^�����ɐ؂�ւ�
        /// </summary>
        /// <param name="interactableStateName"></param>
        public void OnChangeStateAndButtons(string stateName)
        {
            ChangeUIState(stateName);
            ChangeButtonInteractable(stateName);
        }
        /// <summary>
        /// UnityEvent�p�F�{�^����Interactable�̂ݐ؂�ւ�
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
                                Debug.LogWarning($"Log {_firstFocusButton.gameObject.name} �� Interactable �� Disabled �ł�.");
                        else
                            Debug.LogWarning($"Log {_firstFocusButton.gameObject.name} �� ��\���ł�.");
                    else
                        Debug.LogWarning($"Log _firstFocusButton �� Null�ł�.");
        }
    }



}