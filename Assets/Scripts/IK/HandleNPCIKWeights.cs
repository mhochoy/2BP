using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandleNPCIKWeights : MonoBehaviour
{
    [SerializeField] private MultiAimConstraint HeadConstraint;
    float originalHeadConstraintWeight;
    public Transform target;
    bool PlayerSpottedInRaycast;

    void Start() {
        if (HeadConstraint) {
            originalHeadConstraintWeight = HeadConstraint.weight;
        }
    }

    public void EnableLook() {
        if (HeadConstraint) {
            HeadConstraint.weight = originalHeadConstraintWeight;
        }
        gameObject.transform.LookAt(Vector3.Slerp(transform.position, target.position, 2f), Vector3.up);
    }

    public void DisableLook() {
        if (HeadConstraint) {
            HeadConstraint.weight = 0;
        }
        gameObject.transform.LookAt(null, Vector3.up);
    }

    public void Deactivate() {
        target = null;
    }

    public void SetTarget(Transform _target) {
        target = _target;
    }
}   
