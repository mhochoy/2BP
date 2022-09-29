using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StartImpulse : MonoBehaviour
{
    CinemachineImpulseSource source;

    void Awake()
    {
        TryGetComponent<CinemachineImpulseSource>(out source);

        if (source) {
            source.GenerateImpulse();
        }
    }
}
