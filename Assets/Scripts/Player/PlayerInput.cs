using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float x;
    public float z;
    public float switch_axis;
    public float switch_pressed;
    public bool shoot;
    public bool shoot_held;
    public bool reload;
    public bool use;
    bool switch_pressed_up;
    bool switch_pressed_down;
    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        shoot = Input.GetMouseButtonDown(0);
        shoot_held = Input.GetMouseButton(0);
        switch_axis = Input.GetAxis("Mouse ScrollWheel");    
        switch_pressed_down = Input.GetKeyDown(KeyCode.Q);
        switch_pressed_up = Input.GetKeyDown(KeyCode.E);
        reload = Input.GetKeyDown(KeyCode.R);
        use = Input.GetKey(KeyCode.F);
        if (switch_pressed_down) {
            switch_pressed = -1;
        }
        if (switch_pressed_up) {
            switch_pressed = 1;
        }
        else {
            switch_pressed = 0;
        }
    }
}
