using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    [SerializeField] private ParticleFX _fxDirtExplosition;
    
    // Start is called before the first frame update
    private void Start()
    {
        GameEvents.VFXEventInvoker.AddListener(OnVFXEventInvoked);
    }


    private void OnVFXEventInvoked(Vector3 position, string id)
    {
        ParticleFX fx;
        switch (id)
        {
            default:
                fx = Instantiate(_fxDirtExplosition);
                break;
        }

        if (!fx)
        {
            Debug.LogError($"FX DOES NOT EXIST");
            return;
        }

        fx.transform.position = position;
    }
}
