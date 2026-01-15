using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DPS.SimpleUIFramework
    {

    public class MenuButtonController : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
    {
        [Header("References")]
        [SerializeField]
        protected TextMeshProUGUI buttonText;


        [Serializable]
        public class ButtonClickedEvent : UnityEvent
        {
        }

        [Serializable]
        public class ButtonSelectEvent: UnityEvent {}

        [Header("Menu Button Events")]

        [SerializeField]
        private ButtonClickedEvent onClick = new ButtonClickedEvent();

        public ButtonClickedEvent OnClick
        {
            get
            {
                return onClick;
            }
            set
            {
                onClick = value;
            }
        }

        [SerializeField]
        private ButtonSelectEvent onSelectEvent = new ButtonSelectEvent();

        
        public ButtonSelectEvent OnSelectEvent
        {
            get
            {
                return onSelectEvent;
            }
            set
            {
                onSelectEvent = value;
            }
        }

        public void SetActive(bool active) {
            this.gameObject.SetActive(active);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (IsActive() && IsInteractable()) {
                StartCoroutine(FinishSelect());
            }
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            if (IsActive() && IsInteractable()) {
                StartCoroutine(FinishSelect());
            }
        }

        private void Press()
        {
            if (IsActive() && IsInteractable())
            {
                UISystemProfilerApi.AddMarker("Button.onClick", this);
                onClick.Invoke();
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Press();
            }
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();
            if (IsActive() && IsInteractable())
            {
                DoStateTransition(SelectionState.Pressed, instant: false);
                StartCoroutine(OnFinishSubmit());
            }
        }

        private IEnumerator OnFinishSubmit()
        {
            float fadeTime = base.colors.fadeDuration;
            float elapsedTime = 0f;
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(base.currentSelectionState, instant: false);
        }

        private IEnumerator FinishSelect() {
            yield return new WaitForEndOfFrame();
            if (EventSystem.current != null) {
                EventSystem.current.SetSelectedGameObject(this.gameObject);
            }
            this.onSelectEvent?.Invoke();
        }




        #nullable enable
        private object? objectRef;

        public object? ObjectRef { get => this.objectRef; }
        #nullable disable


        #nullable enable
        public virtual void SetButtonContent(
            object? objectRef,
            string buttonText
        ) {
            this.gameObject.SetActive(true);
            this.objectRef = objectRef;
            this.SetButtonText(buttonText);
        }
        #nullable disable


        private void SetButtonText(string buttonText) {
            if (this.buttonText == null) {
                return;
            }

            this.buttonText.text = buttonText;
        }

        public virtual void ClearContent()
        {
            this.objectRef = null;
            this.SetButtonText("");
        }
    }
}