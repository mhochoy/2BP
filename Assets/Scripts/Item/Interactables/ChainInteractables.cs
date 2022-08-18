using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainInteractables : MonoBehaviour
{
    public List<Interactable> mainInteractables;
    public List<Interactable> otherInteractables;
    bool done;

    void Update() {
        bool MainInteractionsAreCompleted = mainInteractables.TrueForAll( (Interactable interactable) => {return interactable.current_state == Interactable.State.Interacted;} );

        if (MainInteractionsAreCompleted && !done) {
            foreach (Interactable interactable in otherInteractables) {
                interactable.current_state = Interactable.State.Interacted;
            }
            done = true;
        }
    }
}
