using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETriggers
{
    ONGRIDLOADED
}


public class TriggerWatcher
{
    public static TriggerWatcher instance;
    private Dictionary<ETriggers, Action<Dictionary<string, object>>> m_Triggers;


    public static TriggerWatcher Instance()
    {
        if (instance == null)
        {
            instance = new TriggerWatcher();

        }
        return instance;
    }


    public void StartListening(ETriggers TriggerringEvent, Action<Dictionary<string, object>> eventToTrigger)
    {
        if (m_Triggers == null)
        {
            m_Triggers = new Dictionary<ETriggers, Action<Dictionary<string, object>>>();
        }

        if (m_Triggers.ContainsKey(TriggerringEvent))
        {
            m_Triggers[TriggerringEvent] += eventToTrigger;
        }
        else
        {
            m_Triggers.Add(TriggerringEvent, eventToTrigger);
        }
    }
    public void StopListening(ETriggers TrigerringEvent, Action<Dictionary<string, object>> eventToTrigger)
    {
        if (m_Triggers.ContainsKey(TrigerringEvent))
        {
            m_Triggers[TrigerringEvent] -= eventToTrigger;
            if (m_Triggers[TrigerringEvent] == null)
            {
                m_Triggers.Remove(TrigerringEvent);
            }
        }

    }
    public void TriggerEvent(ETriggers TriggerringEvent, Dictionary<string, object> eventParams)
    {
        if (m_Triggers.ContainsKey(TriggerringEvent))
        {
            m_Triggers[TriggerringEvent]?.Invoke(eventParams);
        }
    }

}