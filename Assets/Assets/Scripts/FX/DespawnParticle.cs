using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class DespawnParticle : MonoBehaviour
{
    private CancellationTokenSource _cancellation;
    
    [SerializeField] private ParticleSystem _particleSystem;

    public void Start()
    {
        Task.Run(DelayDespawn);
    }

    private async void DelayDespawn()
    {
        try
        {
            await Task.Delay((int) (_particleSystem.main.startLifetime.constant * 1000));
            _cancellation.Token.ThrowIfCancellationRequested();
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
        
        Destroy(gameObject);
    }
}
