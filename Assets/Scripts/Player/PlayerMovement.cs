using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    Animator _animator;
    PlayerInput _input;
    CharacterController _controller;
    Vector3 direction;
    [Range(0, 20)]
    public float speed;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<PlayerInput>();
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(_input.x, _input.z);
        HandleWeaponSelect(_input.switch_axis);
    }

    void Move(float x, float y)
    {
        if (x != 0) {
            _animator.SetFloat("x", x);
        }
        if (y != 0) {
            _animator.SetFloat("y", y);
        }
        direction = transform.forward * y + transform.right * x;
        _controller.Move(direction * speed * Time.deltaTime);
    }

    void HandleWeaponSelect(float selected) {
        float currentWeapon = _animator.GetFloat("selected_weapon");
        if (currentWeapon < 0) {
            currentWeapon = 0;
            _animator.SetFloat("selected_weapon", 0);
        }
        if (selected > 0f) {
            _animator.SetFloat("selected_weapon", currentWeapon + 1);
        }
        if (selected < 0f) {
            _animator.SetFloat("selected_weapon", currentWeapon - 1);
        }
    }
}
