using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameUI ui;
    public void Resume() {
        ui.DisablePauseMenu();
    }

    public void Settings() {

    }

    public void Quit() {
        
    }
}
