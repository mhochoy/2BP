using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandleNPCIKWeights : MonoBehaviour
{
    public bool Activate = true;
    bool deactivated;
    [SerializeField] private Rig _BodyRig;
    public Transform target;
    public bool LockOnActive;
    [SerializeField] private float _lookDistance;

    void Update()
    {
        if (Activate) {
            bool TargetIsWithinRange = Vector3.Distance(transform.position, target.position) < _lookDistance;
            bool TargetIsNotWithinRange = Vector3.Distance(transform.position, target.position) > _lookDistance;
            deactivated = false;
            if (TargetIsWithinRange) {
                if (_BodyRig) {
                    _BodyRig.weight = .75f;
                }
                gameObject.transform.LookAt(Vector3.Slerp(transform.position, target.position, 2f), Vector3.up);
                LockOnActive = true;
            }
            else if (TargetIsNotWithinRange) {
                if (_BodyRig) {
                    _BodyRig.weight = 0;
                }
                gameObject.transform.LookAt(null, Vector3.up);
                LockOnActive = false;
            }
        }
        else {
            if (deactivated == false) {
                deactivated = true;
                gameObject.transform.LookAt(null);
            }
        }
    }

    public void Deactivate() {
        Activate = false;
    }
}   
