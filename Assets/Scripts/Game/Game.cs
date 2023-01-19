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
    int Bodies;
    int TotalMissions;
    bool PlayerHasFoundExit;
    List<Mission> completed_missions = new List<Mission>();
    List<Transform> _Beings = new List<Transform>();
    List<Being> _DEADLIST = new List<Being>();
    Being _being;
    

    void Start() {
        TotalMissions = missions.Count;
        Player.TryGetComponent<Being>(out _being);
        ui.DisableMissionCompleteMenu();
    }

    void Update()
    {
        foreach (Mission mission in missions) {
            if (mission.current_state == Mission.State.Complete) {
                completed_missions.Add(mission);
                missions.Remove(mission);
            }
        }
        foreach (Transform being in Beings) {
            Being __being;
            being.TryGetComponent<Being>(out __being);
            if (__being && !__being.isActive && !_DEADLIST.Contains(__being)) {
                _DEADLIST.Add(__being);
                Bodies++;
            }
        }
        if (current_state == State.Complete) {
            if (PlayerHasFoundExit) {
                if (!_being._input.AreKeysLocked()) {
                    _being._input.LockKeys();
                }
                ui.SetMissionMoneyText($"{_being.points}");
                ui.SetMissionBodiesText($"{Bodies}");
                ui.SetMissionsDoneText($"{completed_missions.Count}/{TotalMissions}");
                ui.HideHUD();
                ui.EnableMissionCompleteMenu();
                return;
            }
            else {
                if (_being._input.AreKeysLocked()) {
                    _being._input.LockKeys();
                }
            }
        }
        else {
        }
        if (ui.IsGameOverMenuActive()) {
            ui.HideHUD();
            ui.DisablePauseMenu();
            return;
        }
        HandleUI();
        HandleGameOverState();
        HandlePauseMenu(_being._input.paused);
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
        if (_being) {
            Interactable interactable = _being.GetNearestInteraction();

            if (missions.Count > 0) {
                ui.InProgressObjectiveText();
                ui.SetObjectiveText(missions[0].description);
            }
            else {
                ui.CompletedObjectiveText();
            }
            ui.SetHealth(_being.health);
            ui.SetAmmoText(_being.GetItemStats());
            ui.SetMoneyText($"${_being.points}");
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
        if (!_being.isActive && !ui.IsGameOverMenuActive()) {
            ui.EnableGameOverMenu();
        }
    }

    public void SetPlayerFoundExit(bool value) {
        PlayerHasFoundExit = value;
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
            foreach (Transform being in Beings) {
                if (!_Beings.Contains(being)) {
                    Logic being_logic;
                    being.TryGetComponent<Logic>(out being_logic);
                    
                    if (being_logic) {
                        being_logic.SetPlayerBeing(_being);
                    }
                    _Beings.Add(being);
                }
            }
        }
    }
}
