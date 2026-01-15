using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPS.SimpleUIFramework
{
    

[RequireComponent(typeof(Animator))]
public class MenuToggleAnimatorController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        if (animator == null)
        {
            this.animator = GetComponent<Animator>();
        }
    }

    private enum ToggleAnimations
    {
        Enabled,
        Disabled
    }

    public void SetToggleActive(bool active)
    {
        if (this.animator == null)
        {
            return;
        }

        this.animator.SetTrigger((active ? ToggleAnimations.Enabled : ToggleAnimations.Disabled) + "");
    }


}

}