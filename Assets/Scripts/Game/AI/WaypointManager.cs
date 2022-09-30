using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public Transform CurrentWaypoint;
    public GameObject[] GeneralWaypoints;
    [SerializeField] List<GameObject> PersonalWaypoints;
    void Start()
    {
        GeneralWaypoints = GameObject.FindGameObjectsWithTag("waypoint");
    }

    public void RemoveWaypoint(GameObject waypoint) {
        PersonalWaypoints.Remove(waypoint);
    }

    public void SetWaypoint(GameObject waypoint) {
        CurrentWaypoint = waypoint.transform;
    }
}
