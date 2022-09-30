using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Logic : MonoBehaviour
{
    public Being being;
    public State current_state;
    public enum State {
        Idle,
        Moving,
        Chasing,
    }
    public bool Bloodthirsty; // Will keep looking for Player, even when Player is out of range
    [Range(0,50)]
    public int HearingRange;
    [Range(0,25)]
    public int ShootProbability;
    WaypointManager waypoints;
    Transform target;
    Being player_being;
    bool PlayerSpottedInRaycast = false;
    int encounters = 0;

    void Start() {
        TryGetComponent<WaypointManager>(out waypoints);
        current_state = State.Idle;
    }

    // Animation
    void FixedUpdate() {
        if (!being.enabled || !being.isActive) {
            return;
        }

        if (being) {
            //target = being.GetTarget();
            //target.TryGetComponent<Being>(out target_being);
            if (player_being && !player_being.isActive) {
                // Target is dead
                //current_state = State.Idle;
            }
        }

        switch (current_state) {
            case State.Idle:
                // Do nothing
                being.SetTarget(null);
                break;
            case State.Moving:
                being.Move(waypoints.CurrentWaypoint.position);
                being.DisableLockOn();
                break;
            case State.Chasing:
                being.Move(player_being.transform.position);
                
                being.Swap(1);
                if (being.HasAnItem()) {
                    int decision = UnityEngine.Random.Range(0, 25);
                    if (decision <= ShootProbability) {
                        being.Use(true);
                    }
                }
                break;
            default:
                current_state = State.Idle;
                break;
        }
    }

    // Input
    void Update() {
        if (!being.enabled || !being.isActive) {
            being.SetTarget(null);
            being.DisableLockOn();
            current_state = State.Idle;
            return;
        }

        bool NoticesPlayer = Vector3.Distance(player_being.transform.position, transform.position) < HearingRange;
        bool CanSeePlayer = ((PlayerSpottedInRaycast && NoticesPlayer) || (encounters > 0 && Bloodthirsty) || being.IsAggro() && being.enemy) && player_being.isActive;
        bool HasWaypoint = waypoints.CurrentWaypoint;
        bool HasNoTargetOrWaypoint = !waypoints.CurrentWaypoint && !PlayerSpottedInRaycast || !player_being.isActive;

        if (NoticesPlayer) {
            being.SetTarget(player_being.transform);
            being.EnableLockOn();
            current_state = State.Idle;
        }
        else {
            being.SetTarget(null);
            being.DisableLockOn();
            current_state = State.Moving;
        }

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position+new Vector3(0,1f,0), transform.forward), out hit)) {
            if (hit.transform.gameObject.layer == 7) {
                encounters++;
                PlayerSpottedInRaycast = true;
            }
            else {
                PlayerSpottedInRaycast = false;
            }
        }
        
        if (CanSeePlayer) {
            current_state = State.Chasing;
        }
        else if (HasWaypoint) {
            current_state = State.Moving;
        }
        else if (HasNoTargetOrWaypoint) {
            current_state = State.Idle;   
        }
        else {
            current_state = State.Idle;
        }
    }

    public void SetPlayerBeing(Being _being) {
        player_being = _being;
    }
}
