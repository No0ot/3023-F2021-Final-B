using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CalendarEventModifiedException : System.Exception
{
    public CalendarEventModifiedException(string message) : base(message)
    {
    }
}
[CreateAssetMenu(fileName = "NewCalendarEvent", menuName = "EventSystem/CalendarEvent")]
public class CalendarEvent : ScriptableObject
{
    private int id = -1;
    public int Id
    {
        get { return id; }
        set
        {
            id = value;
            throw new CalendarEventModifiedException("Oh no you dont!");
        }
    }

    public bool dateSensitive;
    public int eventStartMonth;
    public int eventStartDay;
    public int eventEndMonth;
    public int eventEndDay;

    public bool timeSensitive;
    public int startHour;
    public int endHour;

    public float eventProbability;
    public bool alterNaturalLight;
    public bool alterSun;
    public Color sunColor;
    public float sunIntensity;

    public bool hasWeather;
    public int weatherEvent;

    public AudioClip playerAudioEffect;
    public AudioClip worldAudioEffect;

    public void StartEvent()
    {
        int temp = Random.Range(1, 101);

        if (temp <= eventProbability)
        {
            WorldController.Instance.ActivateEvent(this);
        }
        else
        {
            WorldController.Instance.RegularDay();
            Debug.Log("Event Failed");
        }
    }
}


