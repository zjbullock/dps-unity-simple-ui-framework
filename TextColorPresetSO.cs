using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DPS
{
    namespace SimpleUIFramework
    {
            [CreateAssetMenu(fileName = "UI_Text_Color", menuName = "ScriptableObjects/UI/Text Color Preset")]
        public class TextColorPresetSO : ScriptableObject
        {
            [SerializeField] private Color baseTextColor;

            public Color BaseTextColor { get => this.baseTextColor; }

            [SerializeField] private Color highlightTextColor;

            public Color HighlightTextColor { get => this.highlightTextColor; }

            [SerializeField] private Color warningTextColor;

            public Color WarningTextColor { get => this.warningTextColor; }

        }
    }

 
}
