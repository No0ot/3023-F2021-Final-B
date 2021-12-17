using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalendarUIController : MonoBehaviour
{
    [SerializeField]
    CalendarController calendar;
    [SerializeField]
    TMP_Text calendarDate;


    private void Start()
    {
       
    }

    public void UpdateCalendarText()
    {
        string temp = "Day: " + calendar.currentDayInMonth + "    Month: " + calendar.currentMonth + "    Year: " + calendar.currentYear;

        calendarDate.text = temp;
    }
}
