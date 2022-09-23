using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactionName;
    public State current_state;
    public AudioClip use;
    protected AudioSource sound;
    public enum State {
        Interacted,
        Uninteracted
    }
    Being being;

    void Start() {
        TryGetComponent<AudioSource>(out sound);
    }

    // Input
    void OnTriggerEnter(Collider col) {
        col.TryGetComponent<Being>(out being);

        if (being) {
            OnEnterTrigger();
        }
    }

    void OnTriggerStay(Collider col) {
        bool IsNotAlreadyInteractedWith = (current_state != State.Interacted);
        col.TryGetComponent<Being>(out being);

        if (being) {
            bool BeingTryingToInteractAndImUninteractedWith = being._input && being._input.use && IsNotAlreadyInteractedWith;
            if (BeingTryingToInteractAndImUninteractedWith) {
                Interact();
            }
        }
    }

    void Interact() {
        if (sound) {
            sound.PlayOneShot(use);
        }
        being.SetNearestInteraction(this);
        current_state = State.Interacted;
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
            Activate();
        }

        if (current_state == State.Uninteracted) {
            Deactivate();
        }

        Check();
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
