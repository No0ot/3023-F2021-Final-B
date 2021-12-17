using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalendarDay : MonoBehaviour
{
    public bool isCurrentDay;

    public int dayNum;

    [Header("References")]
    [SerializeField]
    GameObject currentDayFilter;
    [SerializeField]
    TMP_Text dateNum;

    private void Start()
    {
        dateNum.text = dayNum.ToString();
    }
    public void UpdateDay()
    {
        if (isCurrentDay)
            currentDayFilter.SetActive(true);
        else
            currentDayFilter.SetActive(false);
    }
}
