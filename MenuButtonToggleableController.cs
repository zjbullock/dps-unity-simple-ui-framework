using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DPS.SimpleUIFramework
{
    


public class MenuButtonToggleableController : MenuButtonController
{
    [SerializeField] private Animator checkBoxAnimator;

    public enum Toggle
    {
        Enabled,
        Disabled
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetCheckBox(Toggle toggle)
    {
        if (this.checkBoxAnimator == null)
        {
            return;
        }

        string toggleEnum = EnumStringCleaner(toggle + "");
        this.checkBoxAnimator.SetTrigger(toggleEnum);
    }

    private string EnumStringCleaner(string enumString)
    {
        if (enumString == null)
        {
            return "";
        }

        return enumString.Replace("_", " ");
    }

    #nullable enable
    public void SetButtonContent(
        object? objectRef,
        string buttonText,
        Toggle toggle
    )
    {
        base.SetButtonContent(objectRef, buttonText);
        SetCheckBox(toggle);
    }
    #nullable disable
}
}