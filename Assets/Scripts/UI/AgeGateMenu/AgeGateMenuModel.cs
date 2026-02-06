using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgeGateMenuModel : Model
{
    [SerializeField] private float waitTime = 1;

    private Dictionary<int, int> daysInMonths = new Dictionary<int, int>()
    {
        {0, 31},
        {1, 31},
        {2, 28},
        {3, 31},
        {4, 30},
        {5, 31},
        {6, 30},
        {7, 31},
        {8, 31},
        {9, 30},
        {10, 31},
        {11, 30},
        {12, 31},
    };

    private UserInfo userInfo => SaveManager.Instance.userInfo;
    private SceneLoadData sceneLoadData => SaveManager.Instance.sceneLoadData;

    private bool _readyToGo;
    public bool ReadyToGo
    {
        get
        {
            return _readyToGo;
        }
        set
        {
            _readyToGo = value;
            Refresh();
        }
    }

    private int _month;
    public int Month
    {
        get
        {
            return _month;
        }
        set
        {
            _month = value;
            Refresh();
        }
    }

    private int _day;
    public int Day
    {
        get
        {
            return _day;
        }
        set
        {
            _day = value;
            Refresh();
        }
    }

    private int _year;
    public int Year
    {
        get
        {
            return _year;
        }
        set
        {
            _year = value;
            Refresh();
        }
    }

    public int GetDaysThisMonth()
    {
        var daysThisMonth = daysInMonths[Month];

        if (Month == 2 && Year > 0 && DateTime.IsLeapYear(Year))
        {
            daysThisMonth += 1; // add an extra day to february on leap years
        }

        return daysThisMonth;
    }

    public void Submit()
    {
        var age = new DateTime(Year, Month, Day).GetAge();
        userInfo.isOldEnoughForAds = age > 13;
        userInfo.AgeGateQuestionAnswered = true;
        userInfo.yearBorn = Year;
        userInfo.monthBorn = Month;
        userInfo.dayBorn = Day;

        SaveManager.Instance.Save();

        ReadyToGo = true;
        StartCoroutine(WaitAndGoToTitleScreen(waitTime));
    }

    private void Start()
    {
        if (userInfo.AgeGateQuestionAnswered)
        {
            ReadyToGo = true;
            StartCoroutine(WaitAndGoToTitleScreen(waitTime));
        }
    }

    private IEnumerator WaitAndGoToTitleScreen(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Levels.Load(Levels.TitleScreen);
    }
}

public static class DateTimeExtensions
{
    public static int GetAge(this DateTime dateOfBirth)
    {
        var today = DateTime.Today;

        var todayAsInt = (today.Year * 100 + today.Month) * 100 + today.Day;
        var birthdayAsInt = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

        // the two dates are ints in the form yyyyMMdd (ie: 3/14/1995 would be 19950314, 2/6/2026 would be 20260206)
        // subtracting these ints gets us their age * 100,000 (20260206 - 19950314 = 309892)
        // dividing an int by 100,000 does the floor value and gets us 30
        return (todayAsInt - birthdayAsInt) / 10000;
    }
}