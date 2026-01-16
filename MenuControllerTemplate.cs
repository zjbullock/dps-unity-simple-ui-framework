using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DPS.Common;

namespace DPS
{
    namespace SimpleUIFramework
    {
        public abstract class MenuControllerTemplate : MonoBehaviour
{
    [Header("Component Refs")]
    [SerializeField]
    protected EventSystem eventSystem;

    [SerializeField]
    private GameObject firstGameObject;

    [SerializeField]
    private GameObject canvasBlocker;
    
    [SerializeField]
    protected CanvasGroup canvasGroup;

    [SerializeField]
    protected PlayerUIInputBaseController mainMenuController;

    [SerializeField]
    protected TextColorPresetSO textColor;


    [Header("Configurations")]
    [SerializeField]
    private string menuName;
    public string MenuName { get => this.menuName; }
    [SerializeField]
    protected bool shouldDisableOnStart;

    [SerializeField]
    protected bool shouldDisablePreviousMenu = false;

    public bool ShouldDisablePreviousMenu { get => this.shouldDisablePreviousMenu; }

    [Serializable]
    public struct MenuControllerTemplateSetup
    {
        [SerializeField]
        private bool _shouldDisablePreviousMenu;

        public bool ShouldDisablePreviousMenu { get => this._shouldDisablePreviousMenu; }

        [SerializeField]
        private bool _shouldDisablePreviousCanvas;

        public bool ShouldDisablePreviousCanvas { get => this._shouldDisablePreviousCanvas; }
    }

    [SerializeField]
    private GenericDictionary<MenuControllerTemplate, MenuControllerTemplateSetup> previousMenuSpecialBehavior;

    public GenericDictionary<MenuControllerTemplate, MenuControllerTemplateSetup> PreviousMenuSpecialBehavior { get => this.previousMenuSpecialBehavior; }

    [SerializeField]
    protected bool shouldDisablePreviousCanvas = false;

    public bool ShouldDisablePreviousCanvas{ get => this.shouldDisablePreviousCanvas; }


    protected virtual void Awake()
    {
        this.eventSystem = EventSystem.current;
        // this.SetCurrentMenu();
    }

    protected virtual void Start() {
        if (this.shouldDisableOnStart) {
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        this.ResetMenu();
    }

    protected virtual void ResetMenu() {
        return;
    }

    [Header("Unity Events")]
    [Header("Directional Inputs")]
    [SerializeField]
    protected UnityEvent OnUpInputEvent;

    [SerializeField]
    protected UnityEvent OnDownInputEvent;

    [SerializeField]
    protected UnityEvent OnLeftInputEvent;

    [SerializeField]
    protected UnityEvent OnRightInputEvent;

    [Space]
    [Header("Main Controller Buttons")]

    [SerializeField]
    protected UnityEvent onCancelInputEvent;

    [SerializeField]
    protected UnityEvent onSubmitInputEvent;

    [SerializeField]
    protected UnityEvent onOptionOneEvent;

    [SerializeField]
    protected UnityEvent onOptionTwoEvent;

    [Space]
    [Header("Bumpers and Triggers")]
    
    [SerializeField]
    protected UnityEvent onRightBumperInputEvent;

    [SerializeField]
    protected UnityEvent onLeftBumperInputEvent;

    [SerializeField]
    protected UnityEvent onRightTriggerInputEvent;

    [SerializeField]
    protected UnityEvent onLeftTriggerInputEvent;

    public virtual void OnUpInput() {

        this.OnUpInputEvent?.Invoke();
    }

    public virtual void OnDownInput() {
        this.OnDownInputEvent?.Invoke();
    }

    public virtual void OnLeftInput() {
        this.OnLeftInputEvent?.Invoke();
    }

    public virtual void OnRightInput() {
        this.OnRightInputEvent?.Invoke();
    }

    public virtual void OnMenuButtonCancel() {
        this.onCancelInputEvent?.Invoke();
    }

    public virtual void OnLeftBumperInput() {
        this.onLeftBumperInputEvent?.Invoke();
    }
    
    public virtual void OnRightBumperInput() {
        this.onRightBumperInputEvent?.Invoke();
    }

    public virtual void OnLeftTriggerInput() {
        this.onLeftTriggerInputEvent?.Invoke();
    }
    public virtual void OnRightTriggerInput() {
        this.onRightTriggerInputEvent?.Invoke();
    }

    public virtual void OnMenuButtonSubmit() {
        this.onSubmitInputEvent?.Invoke();
    }

    public virtual void OnMenuButtonOptionOne() {
        this.onOptionOneEvent?.Invoke();
    }
    
    public virtual void OnMenuButtonOptionTwo() {
        this.onOptionTwoEvent?.Invoke();
    }



    public void SetActive(bool active) {
        this.gameObject.SetActive(active);

    }

    public void SetInteractable(bool active) {
        this.SetCanvasInteractable(active);
        this.SetCanvasBlockerActive(!active);
    }

    private void SetCanvasInteractable(bool active) {
        if (this.canvasGroup == null) {
            return;
        }

        this.canvasGroup.interactable = active;

    }

    private void SetCanvasBlockerActive(bool active) {
        if (this.canvasBlocker == null) {
            return;
        }

        this.canvasBlocker.gameObject.SetActive(active);
    }
    protected void UpdateUISections()
    {
        this.ResetMenu();
        this.SetCurrentMenu();
    }

    protected abstract void SetCurrentMenu();

    public virtual void Focus()
    {
        this.UpdateUISections();
        StartCoroutine(DoFocus());
    }

    #nullable enable
    protected IEnumerator DoUnFocus(System.Action? afterUnfocus = null) {
        yield return new WaitForEndOfFrame();
        this.eventSystem.SetSelectedGameObject(null);
        afterUnfocus?.Invoke();
    }

    protected IEnumerator DoFocus(System.Action? afterFocus = null) {
        yield return null;
        this.eventSystem.SetSelectedGameObject(null);
        if (this.firstGameObject != null)
        {
            this.eventSystem.SetSelectedGameObject(this.firstGameObject);
        }
        afterFocus?.Invoke();
    }
}
    }
    


}



