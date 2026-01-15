using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DPS.SimpleUIFramework
{
    


    [RequireComponent(typeof(Animator))]
    public class UIContentRowController : MonoBehaviour
    {
        [Header("Component Refs")]
        [SerializeField]
        private TextMeshProUGUI textContent;

        [SerializeField]
        private Animator animator;


        public void SetTextContent(string text) {
            if (this.textContent == null) {
                return;
            }

            this.textContent.text = text;
        }

        public void ClearContent() {
            if (this.textContent == null) {
                return;
            }

            this.textContent.text = "";
        }

        public void SetState() {
            
        }
    }
}