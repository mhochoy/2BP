using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject PanelContextMenu;
    public GameObject StoryContextMenu;
    public GameObject SettingsContextMenu;
    public Scene PrologueScene;

    public void Story() {
        DisablePanelContextMenu();
        StoryContextMenu.SetActive(true);
    }

    public void Settings() {
        DisablePanelContextMenu();
        SettingsContextMenu.SetActive(true);
    }

    public void Back() {
        PanelContextMenu.SetActive(true);
        CheckAndCloseMenus();
    }

    public void NewGame() {
        SceneManager.LoadScene("LoadingScreenScene");
    }

    public void Quit() {
        Application.Quit();
    }

    public void CheckAndCloseMenus() {
        if (StoryContextMenu.activeSelf) {
            StoryContextMenu.SetActive(false);
        }
        if (SettingsContextMenu.activeSelf) {
            SettingsContextMenu.SetActive(false);
        }
    }

    void DisablePanelContextMenu() {
        if (PanelContextMenu.activeSelf) {
            PanelContextMenu.SetActive(false);
        }
    }
}
