using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum State {
        InProgress,
        Complete
    }
    public State current_state;
    public List<Mission> missions;
    public GameUI ui;
    public Transform Player;
    public Transform Enemies;
    List<Transform> _Enemies = new List<Transform>();
    Being being; 
    

    void Start() {
        Player.TryGetComponent<Being>(out being);
    }

    void Update()
    {
        if (ui.IsGameOverMenuActive()) {
            ui.HideHUD();
            ui.DisablePauseMenu();
            return;
        }
        HandleUI();
        HandleGameOverState();
        HandlePauseMenu(being._input.paused);
        HandleGameState();
        
        SyncInternalEnemiesList();
    }

    void HandlePauseMenu(bool active) {
        if (active) {
            if (!ui.IsPauseMenuActive()) {
                ui.EnablePauseMenu();
            }
            
        }
        else {
            if (ui.IsPauseMenuActive()) {
                ui.DisablePauseMenu();
            }
        }
    }

    void HandleUI() {
        if (being) {
            Interactable interactable = being.GetNearestInteraction();

            ui.SetHealth(being.health);
            ui.SetAmmoText(being.GetItemStats());
            HandleInteractableUI(interactable);
        }
    }

    void HandleInteractableUI(Interactable interactable) {
        if (interactable) {
            if (interactable is ChatInteractable) {
                ChatInteractable chatInteraction = (ChatInteractable)interactable;
                if (chatInteraction.npc.health <= 0) {
                    ui.SetInteractableText("");
                    return;
                }
            }
            if (interactable.current_state == Interactable.State.Interacted) {
                ui.SetInteractableText("");
                return;
            }
            ui.SetInteractableText(interactable.interactionName);
        }
        else {
            ui.SetInteractableText("");
        }
    }

    void HandleGameOverState() {
        if (!being.isActive && !ui.IsGameOverMenuActive()) {
            ui.EnableGameOverMenu();
        }
    }

    public void HandleGameState() {
        bool AllMissionsComplete = missions.TrueForAll( (Mission mission)=>{return mission.current_state == Mission.State.Complete;} );

        if (AllMissionsComplete) {
            current_state = State.Complete;
        }
        else {
            current_state = State.InProgress;
        }
    }

    void SyncInternalEnemiesList() {
        if (Enemies.childCount != _Enemies.Count) {
            foreach (Transform enemy in Enemies) {
                if (!_Enemies.Contains(enemy)) {
                    _Enemies.Add(enemy);
                }
            }
        }
    }
}
