using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class UIStateSample : MonoBehaviour
    {
        [SerializeField] private UIStateManager _uiStateManager;

        [Header("8,9で切り替え")]
        [SerializeField] private string UIStateName8;
        [SerializeField] private string UIStateName9;

        private void Start()
        {
            _uiStateManager.ChangeStateEvent += OnChangeStateHandle;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha8))
            {
                _uiStateManager.OnChangeStateAndButtons(UIStateName8);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                _uiStateManager.OnChangeStateAndButtons(UIStateName9);
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                switch(_uiStateManager.GetCurrentStateName())
                {
                    case "Shop_ItemSelect":
                        _uiStateManager.OnChangeStateAndButtons("Close");
                        break;                   
                    case "Shop_SelectOption":
                        _uiStateManager.OnChangeStateAndButtons("Shop_ItemSelect");
                        break;                    
                    case "Shop_SellSelect":
                        _uiStateManager.OnChangeStateAndButtons("Shop_SelectOption");
                        break;

                }
            }
        }

        private void OnChangeStateHandle(string stateName)
        {
            switch(stateName) 
            {
                case "Close":
                    Time.timeScale = 1;
                    break;
                case "Select":
                    Time.timeScale = 0;
                    break;
                //default:
                //    Time.timeScale = 0;
                //    break;
            }
        }
    }
}