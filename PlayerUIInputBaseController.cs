using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using DPS.SimplePlayerControlsFrameWork;
using System;
using DPS.Common;

namespace DPS
{
    namespace SimpleUIFramework
    {
        public abstract class PlayerUIInputBaseController : PlayerInputBaseController
{
    [Header("Sound Effects")]
    [SerializeField] private AudioClip confirmSFX;
    [SerializeField] private AudioClip cancelSFX;
    [SerializeField] private AudioClip navigate;

    [Header("UI Component Refs")]
    [SerializeField]
    private MenuControllerTemplate baseMenu;

    // [SerializeField] private AudioController sfxAudioController;
    [SerializeField] protected List<MenuControllerTemplate> menuControllerStack;
    
    [SerializeField]
    protected TextMeshProUGUI menuText;

    [Header("Runtime Values")]
    [SerializeField]
    private bool isActionInProgress = false;


    protected override void Start()
    {
        base.Start();
        this.CheckBaseState();
    }

    private void CheckBaseState()
    {
        this.menuControllerStack = new();
        this.PushMenu(this.baseMenu);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.CheckBaseState();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.PopAllMenus();
    }

    public override void EnablePlayerInputActions()
    {
        if (PlayerInputActions == null)
        {
            Debug.Log("PLayer input actions is NULL");
            return;
        }

        PlayerInputActions.UI.Navigate.performed += OnNavigateInput;
        // PlayerInputActions.UI.Navigate_Shoulder.performed += OnNavigateShoulderInput;
        // PlayerInputActions.UI.Navigate_Trigger.performed += OnNavigateTriggerInput;
        PlayerInputActions.UI.LeftShoulder.performed += OnLeftShoulderInput;
        PlayerInputActions.UI.LeftTrigger.performed += OnLeftTriggerInput;
        PlayerInputActions.UI.RightShoulder.performed += OnRightShoulderInput;
        PlayerInputActions.UI.RightTrigger.performed += OnRightTriggerInput;

        PlayerInputActions.UI.Submit.performed += OnSubmitInput;
        PlayerInputActions.UI.Cancel.performed += OnCancelInput;

        PlayerInputActions.UI.GamePadOptionOne.performed += OnOptionOneInput;
        PlayerInputActions.UI.GamePadOptionTwo.performed += OnOptionTwoInput;

        PlayerInputActions.UI.Navigate.Enable();
        // PlayerInputActions.UI.Navigate_Shoulder.Enable();
        // PlayerInputActions.UI.Navigate_Trigger.Enable();
        PlayerInputActions.UI.LeftShoulder.Enable();
        PlayerInputActions.UI.LeftTrigger.Enable();
        PlayerInputActions.UI.RightShoulder.Enable();
        PlayerInputActions.UI.RightTrigger.Enable();
        PlayerInputActions.UI.Submit.Enable();
        PlayerInputActions.UI.Cancel.Enable();
        PlayerInputActions.UI.GamePadOptionOne.Enable();
        PlayerInputActions.UI.GamePadOptionTwo.Enable();

        PlayerInputActions.UI.Pause.performed += OnPauseInput;

        PlayerInputActions.UI.Pause.Enable();
        PlayerInputActions.Player.Interact.Enable();

    }

    public override void DisablePlayerInputActions()
    {
        if (PlayerInputActions == null)
        {
            return;
        }

        PlayerInputActions.UI.Navigate.performed -= OnNavigateInput;
        // PlayerInputActions.UI.Navigate_Shoulder.performed -= OnNavigateShoulderInput;
        // PlayerInputActions.UI.Navigate_Trigger.performed -= OnNavigateTriggerInput;

        PlayerInputActions.UI.LeftShoulder.performed -= OnLeftShoulderInput;
        PlayerInputActions.UI.LeftTrigger.performed -= OnLeftTriggerInput;
        PlayerInputActions.UI.RightShoulder.performed -= OnRightShoulderInput;
        PlayerInputActions.UI.RightTrigger.performed -= OnRightTriggerInput;

        PlayerInputActions.UI.Submit.performed -= OnSubmitInput;
        PlayerInputActions.UI.Cancel.performed -= OnCancelInput;
        PlayerInputActions.UI.GamePadOptionOne.performed -= OnOptionOneInput;
        PlayerInputActions.UI.GamePadOptionTwo.performed -= OnOptionTwoInput;

        PlayerInputActions.UI.Navigate.Disable();
        // PlayerInputActions.UI.Navigate_Shoulder.Disable();
        // PlayerInputActions.UI.Navigate_Trigger.Disable();
        PlayerInputActions.UI.LeftShoulder.Disable();
        PlayerInputActions.UI.LeftTrigger.Disable();
        PlayerInputActions.UI.RightShoulder.Disable();
        PlayerInputActions.UI.RightTrigger.Disable();
        PlayerInputActions.UI.Submit.Disable();
        PlayerInputActions.UI.Cancel.Disable();
        PlayerInputActions.UI.GamePadOptionOne.Disable();
        PlayerInputActions.UI.GamePadOptionTwo.Disable();

        PlayerInputActions.UI.Pause.performed -= OnPauseInput;

        PlayerInputActions.UI.Pause.Disable();
        PlayerInputActions.Player.Interact.Disable();
    }

    protected virtual void SetMenuActive(MenuControllerTemplate menu)
    {
        menu.gameObject.SetActive(true);
        menu.SetInteractable(true);
        menu.Focus();
        if (this.menuText != null)
        {
            this.menuText.text = menu.MenuName;
        }
    }
    
    
    public void PopAllMenus()
    {
        for (int i = this.menuControllerStack.Count - 1; i > 0; i--)
        {
            this.RemoveMenuFromStack();
        }
    }

    public void PopMenu()
    {
        this.RemoveMenuFromStack();

        this.SetMenuActive(this.PeekMenuStack());
        return;
    }

    private void RemoveMenuFromStack()
    {
        if (menuControllerStack.Count == 0)
        {
            return;
        }

        MenuControllerTemplate previousMenu = this.menuControllerStack[this.menuControllerStack.Count - 1];
        previousMenu.gameObject.SetActive(false);

        this.menuControllerStack.RemoveAt(this.menuControllerStack.Count - 1);
    }

    #nullable enable
    public MenuControllerTemplate? PeekMenuStack()
    {
        if (this.menuControllerStack.Count == 0)
        {
            return null;
        }
        return this.menuControllerStack[menuControllerStack.Count - 1];
    }
    
    public override void OnPauseInput(InputAction.CallbackContext context)
    {
        Debug.Log("No Pause Functionality Implemented.  Is this intentional?");
        return;
    }

    #nullable disable
    public void PushMenu(MenuControllerTemplate uiView)
    {
        this.HandlePreviousMenu(uiView);
        this.menuControllerStack.Add(uiView);

        this.SetMenuActive(this.PeekMenuStack());
    }

    private void HandlePreviousMenu(MenuControllerTemplate uiView)
    {
        if (this.PeekMenuStack() == null)
        {
            return;
        }

        GenericDictionary<MenuControllerTemplate, MenuControllerTemplate.MenuControllerTemplateSetup> specialMenuBehaviors = uiView.PreviousMenuSpecialBehavior;

        if(specialMenuBehaviors.ContainsKey(this.PeekMenuStack()))
        {
            this.PeekMenuStack().SetActive(!specialMenuBehaviors[this.PeekMenuStack()].ShouldDisablePreviousMenu);
            this.PeekMenuStack().SetInteractable(!specialMenuBehaviors[this.PeekMenuStack()].ShouldDisablePreviousCanvas);   
            return;
        } 

        this.PeekMenuStack().SetActive(!uiView.ShouldDisablePreviousMenu);
        this.PeekMenuStack().SetInteractable(!uiView.ShouldDisablePreviousCanvas);   
    }


    public override void OnNavigateInput(InputAction.CallbackContext context)
    {
        // if (!VerifySceneTransitionService() || SceneTransitionService.instance.IsTransitioning() || !this.gameObject.activeSelf || this.CommitActionInProgress())
        // {
        //     return;
        // }
        if (!this.gameObject.activeSelf || this.CommitActionInProgress())
        {
            return;
        }

        Vector2 menuNavigation = context.ReadValue<Vector2>();

        if (!context.performed)
        {
            return;
        }
        // sfxAudioController.PlayAudio(SoundEffectEnums.Cursor);


        try
        {
            if (menuNavigation.y == 1)
            {
                this.PeekMenuStack().OnUpInput();
            }
            else if (menuNavigation.y == -1)
            {
                this.PeekMenuStack().OnDownInput();
            }
            else if (menuNavigation.x == 1)
            {
                this.PeekMenuStack().OnRightInput();
            }
            else if (menuNavigation.x == -1)
            {
                this.PeekMenuStack().OnLeftInput();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }


        this.ResetActionInProgress();
        // this.eventSystem.SetSelectedGameObject(this.currentlySelectedMenuButton.gameObject);
    }

    public override void OnOptionOneInput(InputAction.CallbackContext context)
    {
        // if (!VerifySceneTransitionService() || SceneTransitionService.instance.IsTransitioning() || !this.gameObject.activeSelf || this.CommitActionInProgress())
        // {
        //     return;
        // }
        if (!this.gameObject.activeSelf || this.CommitActionInProgress())
        {
            return;
        }

        try
        {
            this.PeekMenuStack().OnMenuButtonOptionOne();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        this.ResetActionInProgress();
    }

    public override void OnOptionTwoInput(InputAction.CallbackContext context)
    {
        // if (!VerifySceneTransitionService() || SceneTransitionService.instance.IsTransitioning() || !this.gameObject.activeSelf || this.CommitActionInProgress())
        // {
        //     return;
        // }
        if (!this.gameObject.activeSelf || this.CommitActionInProgress())
        {
            return;
        }

        try
        {
            this.PeekMenuStack().OnMenuButtonOptionTwo();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        this.ResetActionInProgress();
    }
    public override void OnSubmitInput(InputAction.CallbackContext context)
    {
        // if (!VerifySceneTransitionService() || SceneTransitionService.instance.IsTransitioning() || !this.gameObject.activeSelf || this.CommitActionInProgress())
        // {
        //     return;
        // }
        if (!this.gameObject.activeSelf || this.CommitActionInProgress())
        {
            return;
        }
        Debug.Log("Hittin gsubmit");

        try
        {
            this.PeekMenuStack().OnMenuButtonSubmit();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        this.ResetActionInProgress();
    }

    public override void OnLeftShoulderInput(InputAction.CallbackContext context)
    {
        // Intentionally left blank
        // if (!VerifySceneTransitionService() || SceneTransitionService.instance?.IsTransitioning() == true || !this.gameObject.activeSelf || this.CommitActionInProgress())
        // {
        //     return;
        // }
        if (!this.gameObject.activeSelf || this.CommitActionInProgress())
        {
            return;
        }

        if (!context.performed)
        {
            return;
        }

        // sfxAudioController.PlayAudio(SoundEffectEnums.Cursor);

        try
        {
            this.PeekMenuStack().OnLeftBumperInput();

        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }


        this.ResetActionInProgress();
    }

    public override void OnRightShoulderInput(InputAction.CallbackContext context)
    {
        // Intentionally left blank
        // if (!VerifySceneTransitionService() || SceneTransitionService.instance?.IsTransitioning() == true || !this.gameObject.activeSelf || this.CommitActionInProgress())
        // {
        //     return;
        // }
        
        if (!this.gameObject.activeSelf || this.CommitActionInProgress())
        {
            return;
        }


        if (!context.performed)
        {
            return;
        }

        // sfxAudioController.PlayAudio(SoundEffectEnums.Cursor);

        try
        {
            this.PeekMenuStack().OnRightBumperInput();

        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }


        this.ResetActionInProgress();
    }

    public override void OnLeftTriggerInput(InputAction.CallbackContext context)
    {
        // Intentionally left blank
        // if (!VerifySceneTransitionService() || SceneTransitionService.instance?.IsTransitioning() == true || !this.gameObject.activeSelf || this.CommitActionInProgress())
        // {
        //     return;
        // }

        if (!this.gameObject.activeSelf || this.CommitActionInProgress())
        {
            return;
        }

        if (!context.performed)
        {
            return;
        }

        // sfxAudioController.PlayAudio(SoundEffectEnums.Cursor);

        try
        {
            this.PeekMenuStack().OnLeftTriggerInput();

        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }


        this.ResetActionInProgress();
    }

    public override void OnRightTriggerInput(InputAction.CallbackContext context)
    {
        // Intentionally left blank
        // if (!VerifySceneTransitionService() || SceneTransitionService.instance?.IsTransitioning() == true || !this.gameObject.activeSelf || this.CommitActionInProgress())
        // {
        //     return;
        // }
        if (!this.gameObject.activeSelf || this.CommitActionInProgress())
        {
            return;
        }


        if (!context.performed)
        {
            return;
        }

        // sfxAudioController.PlayAudio(SoundEffectEnums.Cursor);

        try
        {
            this.PeekMenuStack().OnRightTriggerInput();

        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }


        this.ResetActionInProgress();
    }

    //public override void OnNavigateShoulderInput(InputAction.CallbackContext context)
    // {
    //     if (!VerifySceneTransitionService() || SceneTransitionService.instance?.IsTransitioning() == true || !this.gameObject.activeSelf || this.CommitActionInProgress())
    //     {
    //         return;
    //     }

    //     Vector2 menuNavigation = context.ReadValue<Vector2>();

    //     if (!context.performed)
    //     {
    //         return;
    //     }

    //     // sfxAudioController.PlayAudio(SoundEffectEnums.Cursor);

    //     try
    //     {
    //         Debug.Log("Shoulder Input: " + menuNavigation);
    //         // if (menuNavigation.y == 1) {
    //         //     this.PeekMenuStack().OnLeftBumperInput();
    //         // } else if (menuNavigation.y == -1) {
    //         //     this.PeekMenuStack().OnRightBumperInput();
    //         // } else 

    //         if (menuNavigation.x == 1)
    //         {
    //             this.PeekMenuStack().OnRightBumperInput();
    //         }
    //         else if (menuNavigation.x == -1)
    //         {
    //             this.PeekMenuStack().OnLeftBumperInput();
    //         }
    //     }
    //     catch (System.Exception e)
    //     {
    //         Debug.LogWarning(e.Message);
    //     }


    //     this.ResetActionInProgress();
    //     // this.eventSystem.SetSelectedGameObject(this.currentlySelectedMenuButton.gameObject);
    // }

    // public override void OnNavigateTriggerInput(InputAction.CallbackContext context)
    // {
    //     if (!VerifySceneTransitionService() || SceneTransitionService.instance?.IsTransitioning() == true || !this.gameObject.activeSelf || this.CommitActionInProgress())
    //     {
    //         return;
    //     }

    //     Vector2 menuNavigation = context.ReadValue<Vector2>();

    //     if (!context.performed)
    //     {
    //         return;
    //     }

    //     // sfxAudioController.PlayAudio(SoundEffectEnums.Cursor);

    //     try
    //     {
    //         // if (menuNavigation.y == 1) {
    //         //     this.PeekMenuStack().OnLeftTriggerInput();
    //         // } else if (menuNavigation.y == -1) {
    //         //     this.PeekMenuStack().OnRightTriggerInput();
    //         // } else 

    //         if (menuNavigation.x == 1)
    //         {
    //             this.PeekMenuStack().OnRightTriggerInput();
    //         }
    //         else if (menuNavigation.x == -1)
    //         {
    //             this.PeekMenuStack().OnLeftTriggerInput();
    //         }
    //     }
    //     catch (System.Exception e)
    //     {
    //         Debug.LogWarning(e.Message);
    //     }


    //     this.ResetActionInProgress();
    //     // this.eventSystem.SetSelectedGameObject(this.currentlySelectedMenuButton.gameObject);
    // }

    private bool CommitActionInProgress()
    {
        Debug.Log(Environment.StackTrace);
        Debug.Log("Committing action in progress");
        if (this.isActionInProgress)
        {
            return this.isActionInProgress;
        }

        this.isActionInProgress = true;
        return false;
    }


    private void ResetActionInProgress()
    {
        Debug.Log("Resetting Action In Progress");
        this.isActionInProgress = false;
    }

    public override void OnCancelInput(InputAction.CallbackContext context)
    {
        // if (!VerifySceneTransitionService() || SceneTransitionService.instance.IsTransitioning() || !this.gameObject.activeSelf || this.CommitActionInProgress())
        // {
        //     return;
        // }

        if ( !this.gameObject.activeSelf || this.CommitActionInProgress())
        {
            return;
        }
        // if (this.sfxAudioController != null)
        // {
        //     sfxAudioController.PlayAudio(this.cancelSFX);
        // }


        try
        {
            this.PeekMenuStack().OnMenuButtonCancel();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        this.ResetActionInProgress();

    }
}
    }

 
   
}
