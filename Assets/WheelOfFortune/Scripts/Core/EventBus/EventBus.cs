using System;
using System.Collections.Generic;
using UnityEngine;
using WheelOfFortune.Core.EventBus;

public sealed class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    public void Publish<T>(T eventData) where T : struct
    {
        var eventType = typeof(T);

        if (_subscribers.ContainsKey(eventType))
        {
            foreach (var subscriber in _subscribers[eventType])
            {
                ((Action<T>)subscriber)(eventData);
            }
        }
    }

    public void Subscribe<T>(Action<T> callback) where T : struct
    {
        if (callback == null)
        {
            Debug.LogError("Callback cannot be null");
            return;  
        } 
        
        var eventType = typeof(T);

        if (!_subscribers.ContainsKey(eventType))
        {
            _subscribers[eventType] = new List<Delegate>();
        }

        _subscribers[eventType].Add(callback);
    }

    public void Unsubscribe<T>(Action<T> callback) where T : struct
    {
        if (callback == null)
        {
            Debug.LogError("Callback cannot be null");
            return;
        }

        var eventType = typeof(T);

        if (_subscribers.ContainsKey(eventType))
        {
            _subscribers[eventType].Remove(callback);
        }
    }
}