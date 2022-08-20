using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
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
            if (being._input.use && IsNotAlreadyInteractedWith) {
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
    void Update()
    {
        if (current_state == State.Interacted) {
            Activate();
        }

        if (current_state == State.Uninteracted) {
            Deactivate();
        }
    }


    // Custom Interactable Methods
    public virtual void Activate() {
        // Activate
        if (sound) {
            sound.PlayOneShot(use);
        }
    }

    public virtual void Deactivate() {
        // Deactivate
    }

    public virtual void OnEnterTrigger() {

    }

    public virtual void OnLeaveTrigger() {

    }

    public virtual void GivePlayer(Item item) {
        if (being) {
            being.AddItemToInventory(item);
        }
    }
}
