using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactionName;
    public State current_state;
    public AudioClip use;
    public bool DirectlyInteractable = true;
    protected AudioSource sound;
    public enum State {
        Interacted,
        Uninteracted
    }
    Being being;
    bool blocked;

    void Start() {
        TryGetComponent<AudioSource>(out sound);
    }

    // Input
    void OnTriggerEnter(Collider col) {
        col.TryGetComponent<Being>(out being);
        //Clear previous nearest interaction
        OnLeaveTrigger();

        if (being && DirectlyInteractable && !blocked) {
            OnEnterTrigger();
        }
    }

    void OnTriggerStay(Collider col) {
        bool IsNotAlreadyInteractedWith = (current_state != State.Interacted);
        col.TryGetComponent<Being>(out being);

        if (being && !blocked && being.GetNearestInteraction() == this) {
            if (being._input && being._input.use && IsNotAlreadyInteractedWith && DirectlyInteractable) {
                if (sound) {
                    sound.PlayOneShot(use);
                }
                being.SetNearestInteraction(this);
                current_state = State.Interacted;
            }
        }
    }

    void OnTriggerExit(Collider col) {
        col.TryGetComponent<Being>(out being);

        if (being) {
            OnLeaveTrigger();
        }
    }
    // Update
    protected virtual void Update()
    {
        if (current_state == State.Interacted) {
            bool done = false;
            if (!done) {
                done = true;
                OnActivate();
            }
            Activate();
        }

        if (current_state == State.Uninteracted) {
            Deactivate();
        }

        Check();
    }

    protected virtual void OnActivate() {

    }


    // Custom Interactable Methods
    protected virtual void Activate() {
        // Activate
    }

    protected virtual void Deactivate() {
        // Deactivate
    }

    protected virtual void OnEnterTrigger() {
        if (current_state != State.Interacted) {
            being.SetNearestInteraction(this);
        }
    }

    protected virtual void OnLeaveTrigger() {
        being.SetNearestInteraction(null);
    }

    protected virtual void Check() {
        // Update
        if (being) {
            if (!Physics.Linecast(transform.position, being.transform.position)) {
                blocked = false;
                
            }
            else {
                blocked = true;
            }
        }
    }

    protected virtual void GivePlayer(Item item) {
        if (being) {
            being.AddItemToInventory(item);
        }
    }

    protected Being GetBeing() {
        return being;
    }
}
