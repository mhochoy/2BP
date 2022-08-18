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

    void Update()
    {
        bool AllMissionsComplete = missions.TrueForAll( (Mission mission)=>{return mission.current_state == Mission.State.Complete;} );

        if (AllMissionsComplete) {
            current_state = State.Complete;
        }
        else {
            current_state = State.InProgress;
        }
    }
}
