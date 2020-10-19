using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Callback<T>
{
    public delegate void OnCallback(T result);
    private event OnCallback OnCallbackEvent;

    public void SetCallbackFunction(OnCallback function)
    {
        OnCallbackEvent = function;
    }

    public void Call(T result)
    {
        OnCallbackEvent?.Invoke(result);
    }
}
