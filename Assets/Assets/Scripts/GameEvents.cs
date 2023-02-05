using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ResourceType
{
    Energy,
    Nutrients,
    Potato,
    Fertilizer
}

public class VFXEvent : UnityEvent<Vector3, string> { }

public class ResourcePoolEvent : UnityEvent<float> { }

public class ResourceCollectedEvent : UnityEvent<ResourceType, float> { }

public static class GameEvents
{
    public static VFXEvent VFXEventInvoker = new VFXEvent();
    
    public static ResourcePoolEvent EnergyPoolChangedValue = new ResourcePoolEvent();
    public static ResourcePoolEvent EnergyPoolChangedPerc = new ResourcePoolEvent();

    public static ResourceCollectedEvent ResourceCollected = new ResourceCollectedEvent();
}
