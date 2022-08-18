using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public string description;
    public enum State {
        InProgress,
        Complete
    }
    public State current_state;
    public List<Interactable> requiredInteractions;

    void Update()
    {
        bool CompletedAllInteractions = requiredInteractions.TrueForAll( (Interactable interactable)=>{return interactable.current_state == Interactable.State.Interacted;} );

        if (CompletedAllInteractions) {
            current_state = State.Complete;
        }
        else {
            current_state = State.InProgress;
        }
    }
}
