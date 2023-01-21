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
    protected Being being;
    bool blocked;

    void Start() {
        TryGetComponent<AudioSource>(out sound);
    }

    // Input
    protected virtual void OnTriggerEnter(Collider col) {
        col.TryGetComponent<Being>(out being);

        if (being && DirectlyInteractable) {
            OnEnterTrigger();
        }
    }

    protected virtual void OnTriggerStay(Collider col) {
        col.TryGetComponent<Being>(out being);
        bool IsNotAlreadyInteractedWith = (current_state != State.Interacted);
        bool ThisIsTheNearestInteractable = (being && being.GetNearestInteraction() == this);
        bool BeingIsInteractingWithInteractable = (being && being._input && being._input.use);

        if (being && ThisIsTheNearestInteractable) {
            being.SetNearestInteraction(this);
            if (BeingIsInteractingWithInteractable && IsNotAlreadyInteractedWith && DirectlyInteractable) {
                if (sound) {
                    sound.PlayOneShot(use);
                }
                OnTrigger();
                current_state = State.Interacted;
            }
        }
    }

    protected virtual void OnTrigger() {
        
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
        if (being) {
            being.SetNearestInteraction(null);
        }
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
