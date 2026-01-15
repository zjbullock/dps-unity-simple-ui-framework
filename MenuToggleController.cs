using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DPS.SimpleUIFramework
{
    


    public class MenuToggleController : Toggle
    {
        [SerializeField]
        private MenuToggleAnimatorController menuToggleAnimatorController;

        protected override void Awake()
        {
            base.Awake();
            if (menuToggleAnimatorController == null)
            {
                this.menuToggleAnimatorController = GetComponent<MenuToggleAnimatorController>();
            }
        }

        protected override void Start()
        {
            base.Start();
            base.onValueChanged.AddListener((val) =>
            {
                if (this.menuToggleAnimatorController == null)
                {
                    return;
                }
                this.menuToggleAnimatorController.SetToggleActive(val);
            });
        }
    }
}