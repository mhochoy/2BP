using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRagdoll : MonoBehaviour
{
    CharacterController col;
    Rigidbody[] bodies;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<CharacterController>(out col);
        bodies = GetComponentsInChildren<Rigidbody>();
    }

    public bool ActivateRagdoll()
    {
        if (bodies.Length > 1) 
        {
            foreach (Rigidbody body in bodies)
            {
                Collider _col;
                body.TryGetComponent<Collider>(out _col);
                if (_col && !_col.enabled) {
                    _col.enabled = true;
                }
                body.isKinematic = false;
                body.useGravity = true;
            }
            col.enabled = false;
            return true;
        }
        if (bodies.Length <= 1) {
            return false;
        }
        else {
            return false;
        }
    }

    public bool DeactivateRagdoll()
    {
        if (bodies != null) {
            if (bodies.Length > 1) 
            {
                foreach (Rigidbody body in bodies)
                {
                    Collider _col;
                    CharacterJoint _joint;
                    body.TryGetComponent<Collider>(out _col);
                    body.gameObject.TryGetComponent<CharacterJoint>(out _joint);
                    if (_col && !_col.enabled) 
                    {
                        _col.enabled = false;
                    }
                    body.isKinematic = true;
                    body.useGravity = false;
                }
                return true;
            }
            if (bodies.Length <= 1) {
                return false;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }

    void DeactivateSound() {
        AudioSource source;
        TryGetComponent<AudioSource>(out source);

        if (source.enabled) {
            source.enabled = false;
        }
    }
}
