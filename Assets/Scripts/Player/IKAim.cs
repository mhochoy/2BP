using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKAim : MonoBehaviour
{
    Animator _animator;
    public Transform Target;
    public Transform IKAimShoulder;
    public Transform IKAimElbow;
    public Transform IKAimHand;

    public Vector3 RotationOffset;

    [Range(0, 1f)]
    public float IKWeight=1;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex) 
    {
        if (layerIndex != 0) {
            IKAimShoulder.LookAt(Target.position);
            IKAimHand.LookAt(Target.position);
            IKAimHand.Rotate(RotationOffset);
            // Set Weights
            _animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1);
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IKWeight);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, IKWeight);
            // Set Positions
            _animator.SetIKPosition(AvatarIKGoal.RightHand, IKAimHand.position);
            _animator.SetIKRotation(AvatarIKGoal.RightHand, IKAimHand.rotation);
            _animator.SetIKHintPosition(AvatarIKHint.RightElbow, IKAimElbow.position);
            Debug.DrawRay(IKAimShoulder.position, IKAimShoulder.forward * 20, Color.red);
        }
    }
}
