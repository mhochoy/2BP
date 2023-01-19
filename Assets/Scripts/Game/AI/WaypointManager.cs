using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointManager : MonoBehaviour
{
    Vector3 CurrentPoint;
    public GameObject[] GeneralWaypoints;
    [SerializeField] List<Transform> PersonalWaypoints;
    NavMeshAgent _Agent;
    int destPoint = 0;
    float originalStoppingDistance;

    public void GoToNextPoint()
    {
        if (PersonalWaypoints.Count > 0) {
            destPoint = (destPoint + 1) % PersonalWaypoints.Count;
        }
    }

    public Vector3 GetCurrentPoint() {
        return CurrentPoint;
    }

    void Start()
    {
        _Agent = GetComponent<NavMeshAgent>();
        if (_Agent) {
            originalStoppingDistance = _Agent.stoppingDistance;
        }
        GeneralWaypoints = GameObject.FindGameObjectsWithTag("waypoint");
    }

    void Update() {
        if (!_Agent.enabled) {
            return;
        }
        bool HasPoint = (PersonalWaypoints.Count != 0);
        bool isMoving = (_Agent.velocity.magnitude != 0f);
        bool NoPointIsBeingLookedFor = ( !_Agent.pathPending && _Agent.remainingDistance < .1f);

        if (HasPoint) {
            if (NoPointIsBeingLookedFor && !isMoving)
            {
                GoToNextPoint();
            }

            CurrentPoint = PersonalWaypoints[destPoint].position;
        }
    }

    public int PersonalCount() {
        return PersonalWaypoints.Count;
    }

    public void ChasingModeStoppingDistance() {
        _Agent.stoppingDistance = originalStoppingDistance;
    }

    public void WaypointModeStoppingDistance() {
        _Agent.stoppingDistance = .1f;
    }
}
