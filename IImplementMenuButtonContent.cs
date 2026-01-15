using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPS.SimpleUIFramework
{
    
public interface IImplementMenuButtonContent
{
    void SetButtonContent(MenuButtonController menuButton, IMenuButtonContent content);
}
}