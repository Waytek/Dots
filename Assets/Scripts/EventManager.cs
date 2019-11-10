using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager
{
  public enum Events
  {
    DotsDestroyed
  }


  private static Dictionary<Events, UnityEvent> eventDictionary = new Dictionary<Events, UnityEvent>();

  private static EventManager eventManager;

  public static void StartListening(Events eventName, UnityAction listener)
  {
    UnityEvent thisEvent = null;
    if (eventDictionary.TryGetValue(eventName, out thisEvent))
    {
      thisEvent.AddListener(listener);
    }
    else
    {
      thisEvent = new UnityEvent();
      thisEvent.AddListener(listener);
      eventDictionary.Add(eventName, thisEvent);
    }
  }

  public static void StopListening(Events eventName, UnityAction listener)
  {
    if (eventManager == null) return;
    UnityEvent thisEvent = null;
    if (eventDictionary.TryGetValue(eventName, out thisEvent))
    {
      thisEvent.RemoveListener(listener);
    }
  }

  public static void TriggerEvent(Events eventName)
  {
    UnityEvent thisEvent = null;
    if (eventDictionary.TryGetValue(eventName, out thisEvent))
    {
      thisEvent.Invoke();
    }
  }
}
