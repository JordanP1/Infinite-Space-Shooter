using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EnemyExplosion : MonoBehaviour
{
    private void Awake()
    {
        //Setup particle system to use the OnParticleSystemStopped() callback.
        ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    private void OnParticleSystemStopped()
    {
        //Destroy particle system when done playing.
        Destroy(gameObject);
    }
}
