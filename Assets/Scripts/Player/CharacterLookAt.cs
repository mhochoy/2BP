using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterLookAt : MonoBehaviour
{
    Camera _mainCamera;

    Vector3 LookAtPosition;

    public Transform Target;
    
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        _mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector3 worldAimTarget = LookAtPosition;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
    }

    private void OnAnimatorIK(int layerIndex) 
    {
        if (layerIndex == 0) {
            Ray lookAtRay = new Ray(transform.position, _mainCamera.transform.forward);
            LookAtPosition = lookAtRay.GetPoint(20);
            if (Physics.Raycast(lookAtRay, out RaycastHit raycastHit, 999f, ~groundLayer)) {
                LookAtPosition = raycastHit.point;
            }
            Target.position = LookAtPosition;
        }
    }
}
