using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace DPS.SimpleUIFramework
{
    


    public class MenuButtonListController : MonoBehaviour
    {


        [Header("Component References")]

        [SerializeField]

        protected List<MenuButtonController> menuButtonControllers;

        public List<MenuButtonController> MenuButtonControllers { get => this.menuButtonControllers; }

        void Awake()
        {
            this.GetTotalButtonControllers();
        }


        //Exposing this method to allow re-fetching of button controllers if needed.
        public void GetTotalButtonControllers()
        {
            if (this.menuButtonControllers.Count > 0)
            {
                Debug.LogWarning("Menu Controller - " + this.gameObject.name + " Already has menu button controllers assigned.");
                return;
            }
            this.menuButtonControllers = new(GetComponentsInChildren<MenuButtonController>());
        }

        public void ResetAllButtonContent()
        {
            if (menuButtonControllers.Count == 0)
            {
                return;
            }

            foreach (MenuButtonController menuButtonController in this.menuButtonControllers)
            {
                menuButtonController.ClearContent();
                menuButtonController.OnSelectEvent.RemoveAllListeners();
                menuButtonController.OnClick.RemoveAllListeners();
            }
        }

        #nullable enable
        public void ToggleButtonsBasedOnAction(System.Func<MenuButtonController, bool>? action, bool active)
        {
            foreach (MenuButtonController menuButton in this.menuButtonControllers)
            {
                //If action is null, this means that the active value will be set without a condition.
                if (action != null && !action.Invoke(menuButton))
                {
                    continue;
                }

                menuButton.gameObject.SetActive(active);
            }
        }


    }
}