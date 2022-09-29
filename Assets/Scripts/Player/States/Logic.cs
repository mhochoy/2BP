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
    [Range(0,25)]
    public int HitProbability;
    Transform target;
    Being target_being;

    void Start() {
        current_state = State.Idle;
    }

    // Animation
    void FixedUpdate() {
        if (!being.enabled || !being.isActive) {
            return;
        }

        if (being && being.IsLockedOn()) {
            target = being.GetTarget();
            target.TryGetComponent<Being>(out target_being);
            if (target_being && !target_being.isActive) {
                // Target is dead
                current_state = State.Idle;
            }
        }

        switch (current_state) {
            case State.Idle:
                being.Move(Vector3.zero);
                break;
            case State.Moving:
                being.Move(Vector3.forward);
                break;
            case State.Chasing:
                being.Move(target.position);
                being.Swap(1);
                if (being.HasAnItem()) {
                    int decision = UnityEngine.Random.Range(0, 25);
                    if (decision <= HitProbability) {
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
        bool CanSeePlayer = being.IsLockedOn("Player");
        bool CantSeePlayerButCanSeeTarget = !being.IsLockedOn("Player") && being.IsLockedOn();
        bool CantSeePlayerOrTarget = !being.IsLockedOn("Player") && !being.IsLockedOn();

        if (CanSeePlayer) {
            current_state = State.Chasing;
        }
        else if (CantSeePlayerButCanSeeTarget) {
            current_state = State.Moving;
        }
        else if (CantSeePlayerOrTarget) {
            current_state = State.Idle;   
        }
        else {
            current_state = State.Idle;
        }
    }
}
