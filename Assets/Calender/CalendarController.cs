using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalendarController : MonoBehaviour
{
    [Header("Day Settings")]
    [SerializeField]
    int numMinutesInHour;
    [SerializeField]
    int numHoursInDay;
    [SerializeField]
    [Tooltip("How many seconds before calling IncrementTime")]
    float maxTimeTick; /* How many seconds before TimeIncrements */
    float timeTick;
    [SerializeField]
    [Range(1, 30)]
    int minuteIncrement;
    int currentMin = 0;
    int currentHour = 0;

    [Header("Calendar Settings")]
    [SerializeField]
    [Range(1, 35)]
    int numDaysInMonth;
    [SerializeField]
    int numMonthsInYear;
    int startYear;

    [Header("References")]
    [SerializeField]
    GameObject dayPrefab;
    [SerializeField]
    TMP_Text calendarDate;
    CalendarDay[] dayList;
    [SerializeField]
    EventTable masterEventTable;
    [SerializeField]
    TMP_Text timeText;

    [Header("Gameplay Settings")]
    public int currentDayInMonth;
    public int currentMonth;
    public int currentYear;
    [SerializeField]
    List<CalendarEvent> activeEvents;

    // Start is called before the first frame update
    void Start()
    {
        dayList = new CalendarDay[numDaysInMonth];
        activeEvents = new List<CalendarEvent>();
        for(int i = 0; i < numDaysInMonth; i++)
        {
            GameObject temp = Instantiate(dayPrefab);
            temp.transform.parent = gameObject.transform;
            temp.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            dayList[i] = temp.GetComponent<CalendarDay>();
            dayList[i].dayNum = i + 1;
        }

        if (currentDayInMonth == 0)
            currentDayInMonth = 1;
        if (currentMonth == 0)
            currentMonth = 1;

        currentHour = 2;
        UpdateCalendarText();
        dayList[currentDayInMonth - 1].isCurrentDay = true;
        UpdateDays();
        StartDay();
        RunTimeSensitiveEvents();
        UpdateTimerText();
    }

    // Update is called once per frame
    void Update()
    {
        timeTick += Time.deltaTime;
        if (timeTick >= maxTimeTick)
        {
            timeTick = 0;
            IncrementTime();
        }

        if (Input.GetKeyDown("."))
        {
            IncrementTime();
        }

        if (Input.GetKeyDown("]"))
        {
            AdvanceDay();
        }
    }

    public void AdvanceDay()
    {
        dayList[currentDayInMonth - 1].isCurrentDay = false;
        currentDayInMonth++;
        if(currentDayInMonth > numDaysInMonth)
        {
            currentDayInMonth = 1;
            currentMonth++;
            if(currentMonth > numMonthsInYear)
            {
                currentMonth = 1;
                currentYear++;
            }
        }
        dayList[currentDayInMonth - 1].isCurrentDay = true;
        ResetTimer();
        UpdateCalendarText();
        UpdateDays();
        StartDay();
    }

    public void UpdateDays()
    {
        foreach(CalendarDay day in dayList)
        {
            day.UpdateDay();
        }
    }

    public void UpdateCalendarText()
    {
        string temp = "Day: " + currentDayInMonth + "    Month: " + currentMonth + "    Year: " + currentYear;

        calendarDate.text = temp;
    }

    private void StartDay()
    {
        foreach (CalendarEvent cEvent in activeEvents)
        {
            if (cEvent.eventEndMonth == currentMonth && cEvent.eventEndDay == currentDayInMonth)
            {
                if (!cEvent.timeSensitive && cEvent.dateSensitive)
                {
                    WorldController.Instance.EndEvent(cEvent);
                    activeEvents.Remove(cEvent);
                    break;
                }
            }
        }

        foreach (CalendarEvent cEvent in masterEventTable.GetTable())
        {
            if(cEvent.eventStartMonth == currentMonth && cEvent.eventStartDay == currentDayInMonth)
            {
                if (cEvent.dateSensitive)
                {
                    activeEvents.Add(cEvent);
                }
            }
        }

        foreach(CalendarEvent cEvent in activeEvents)
        {
            if(!cEvent.timeSensitive)
                cEvent.StartEvent();
        }

        if(activeEvents.Count == 0)
        {
            WorldController.Instance.RegularDay();
        }
    }

    private void IncrementTime()
    {
        currentMin += minuteIncrement;
        if (currentMin >= numMinutesInHour)
        {
            currentHour++;
            RunTimeSensitiveEvents();
            currentMin = 0;
            if (currentHour >= numHoursInDay)
            {
                currentHour = 0;
                AdvanceDay();
            }
        }
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        string temp;
        if (currentHour < 10 && currentMin < 10)
            temp = "0" + currentHour + " : " + "0" + currentMin;
        else if (currentHour < 10 && currentMin >= 10)
            temp = "0" + currentHour + " : " + currentMin;
        else if (currentHour >= 10 && currentMin < 10)
            temp = currentHour + " : " + "0" + currentMin;
        else
            temp = currentHour + " : " + currentMin;

        timeText.text = temp;
    }

    public void ResetTimer()
    {
        currentMin = 0;
        currentHour = 0;
        timeTick = 0;
        UpdateTimerText();
    }

    public void RunTimeSensitiveEvents()
    {
        foreach (CalendarEvent cEvent in activeEvents)
        {
            if (cEvent.timeSensitive)
            {
                if (cEvent.dateSensitive)
                {
                    if (currentHour == cEvent.endHour && cEvent.eventEndMonth == currentMonth && cEvent.eventEndDay == currentDayInMonth)
                    {
                        WorldController.Instance.EndEvent(cEvent);
                        activeEvents.Remove(cEvent);
                        break;
                    }
                }
                else
                {
                    if(currentHour == cEvent.endHour)
                    {
                        WorldController.Instance.EndEvent(cEvent);
                        activeEvents.Remove(cEvent);
                        break;
                    }
                }
            }
        }

        foreach (CalendarEvent cEvent in activeEvents)
        {
            if(cEvent.timeSensitive)
            {
                if(currentHour == cEvent.startHour)
                {
                    cEvent.StartEvent();
                }
            }
        }

        foreach (CalendarEvent cEvent in masterEventTable.GetTable())
        {
            if(cEvent.timeSensitive && !cEvent.dateSensitive)
            {
                if (cEvent.startHour == currentHour)
                    cEvent.StartEvent();
            }
        }
    }
}
