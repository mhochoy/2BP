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
    List<Transform> _Enemies;
    Being being; 
    

    void Start() {
        Player.TryGetComponent<Being>(out being);
    }

    void Update()
    {
        HandleGameState();
        HandleUI(Player);
        SyncInternalEnemiesList();
    }

    void HandleUI(Transform player) {
        if (being) {
            ui.SetHealth(being.health);
            ui.SetAmmoText(being.GetItemStats());
            ui.SetInteractableText(being.GetNearestInteraction());
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

    public List<Transform> GetEnemies() {
        return _Enemies;
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
