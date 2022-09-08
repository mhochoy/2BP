using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject Menu;
    public TMPro.TMP_Text InteractableText;
    public TMPro.TMP_Text AmmoText;
    public TMPro.TMP_Text MoneyText;
    public Slider Health;

    public void SetHealth(int value) {
        Health.value = value;
    }

    public void SetInteractableText(string interactableObjectName) {
        InteractableText.text = interactableObjectName;
    }

    public void SetAmmoText(string text) {
        AmmoText.text = text;
    }

    public void SetMoneyText(string text) {
        MoneyText.text = text;
    }

    public void EnablePauseMenu() {
        Time.timeScale = 0f;
        Menu.SetActive(true);
    }

    public void DisablePauseMenu() {
        Time.timeScale = 1;
        Menu.SetActive(false);
    }

    public bool IsPauseMenuActive() {
        return Menu.activeInHierarchy;
    }
}
