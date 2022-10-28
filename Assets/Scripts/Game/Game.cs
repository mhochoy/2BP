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
    public Transform Beings;
    List<Transform> _Beings = new List<Transform>();
    Being being; 
    

    void Start() {
        Player.TryGetComponent<Being>(out being);
    }

    void Update()
    {
        foreach (Mission mission in missions) {
            if (mission.current_state == Mission.State.Complete) {
                missions.Remove(mission);
            }
        }
        if (ui.IsGameOverMenuActive()) {
            ui.HideHUD();
            ui.DisablePauseMenu();
            return;
        }
        if (current_state == State.Complete) {
            Debug.Log("Level Complete!");
            // Load in next level or cutscene
        }
        else {
        }
        HandleUI();
        HandleGameOverState();
        HandlePauseMenu(being._input.paused);
        HandleGameState();
        
        SyncInternalBeingsList();
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

            if (missions.Count > 0) {
                ui.InProgressObjectiveText();
                ui.SetObjectiveText(missions[0].description);
            }
            else {
                ui.CompletedObjectiveText();
            }
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

    void SyncInternalBeingsList() {
        if (Beings.childCount != _Beings.Count) {
            foreach (Transform _being in Beings) {
                if (!_Beings.Contains(_being)) {
                    Logic being_logic;
                    _being.TryGetComponent<Logic>(out being_logic);
                    
                    if (being_logic) {
                        being_logic.SetPlayerBeing(being);
                    }
                    _Beings.Add(_being);
                }
            }
        }
    }
}
