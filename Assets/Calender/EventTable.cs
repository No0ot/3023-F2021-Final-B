using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEventTable", menuName = "EventSystem/EventTable")]
public class EventTable : ScriptableObject
{
    [SerializeField]
    private List<CalendarEvent> table;  //The index of each item in the table is its ID
    public CalendarEvent GetItem(int id)
    {
        return table[id];
    }
    public void AssignItemIDs() // Give each item an ID based on its location in the list
    {
        for (int i = 0; i < table.Count; i++)
        {
            try
            {
                table[i].Id = i;
            }
            catch (CalendarEventModifiedException)
            {
                //it's ok
            }
        }
    }

    public List<CalendarEvent> GetTable()
    {
        return table;
    }
}
