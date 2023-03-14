using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAggro : MonoBehaviour
{
    SphereCollider col;
    bool Blocked;

    void OnTriggerEnter(Collider _col) {
        AggroNPC(collider: _col);
    }

    void OnTriggerStay(Collider _col) {
        AggroNPC(collider: _col);
    }

    void AggroNPC(Collider collider) {
        Being being;
        collider.TryGetComponent<Being>(out being);
        if (being) {
        
            if (Physics.Linecast(transform.position, collider.transform.position)) {
                Blocked = true;
            }
            else {
                Blocked = false;
            }

            if (being.isAI && !Blocked) {
                being.SetAggro(true);
            }
        } 
        Invoke("DisableTrigger", .5f);
    }

    // Update is called once per frame
    void Update()
    {
        col = GetComponent<SphereCollider>();
    }

    void DisableTrigger() {
        col.enabled = false;
    }
}
