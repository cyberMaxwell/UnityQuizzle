using UnityEngine;
using System;

public class TimerMaster : MonoBehaviour
{
    DateTime currentDate;
    DateTime oldDate;

    public string saveLocation;
    public static TimerMaster instance;

    void Awake()
    {
        instance = this;

        saveLocation = "lastSavedDate1";

    }

    public float CheckDate()
    {
        currentDate = DateTime.Now;

        string tempString = PlayerPrefs.GetString(saveLocation, "1");

        long tempLong = Convert.ToInt64(tempString);

        oldDate = DateTime.FromBinary(tempLong);

        TimeSpan difference = currentDate.Subtract(oldDate);

        return (float)difference.TotalSeconds;
    }

    public void SaveDate()
    {
        PlayerPrefs.SetString(saveLocation, DateTime.Now.ToBinary().ToString());
    }

}
