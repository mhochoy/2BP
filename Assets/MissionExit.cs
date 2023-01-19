using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionExit : MonoBehaviour
{
    public Game game;
    
    void OnTriggerEnter(Collider col) {
        if (col.transform.CompareTag("Player")) {
            game.SetPlayerFoundExit(true);
        }
    }
}
