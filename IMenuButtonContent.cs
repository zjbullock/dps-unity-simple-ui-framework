using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPS.SimpleUIFramework
{
    public interface IMenuButtonContent
    {
    #nullable enable
        public void OnClick();
        public void OnSelect();

        public object? GetContent();

        public string GetContentName();
    #nullable disable
    }

    public class UIMenuButtonListPage
    {
        public List<IMenuButtonContent> menuButtonList = new();

    } 
}

















