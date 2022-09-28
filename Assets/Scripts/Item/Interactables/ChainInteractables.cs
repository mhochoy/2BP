using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainInteractables : MonoBehaviour
{
    public List<Interactable> mainInteractables;
    public List<Being> enemyInteractables = new List<Being>();
    public List<Interactable> otherInteractables;
    bool done;

    void Update() {
        bool MainInteractionsAreCompleted = mainInteractables.TrueForAll( (Interactable interactable) => {return interactable.current_state == Interactable.State.Interacted;} );
        bool EnemiesAreDefeated = true;

        if (enemyInteractables.Count > 0) {
            EnemiesAreDefeated = enemyInteractables.TrueForAll( (Being enemy) => {return !enemy.isActive;});
        }

        if (MainInteractionsAreCompleted && EnemiesAreDefeated && !done) {
            foreach (Interactable interactable in otherInteractables) {
                interactable.current_state = Interactable.State.Interacted;
            }
            done = true;
        }
    }
}
