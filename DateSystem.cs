using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DateSystem
{
    public enum DateWeekDayName
    {
        Friday,
        Saturady,
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday
    }
    public enum DateMonthName
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public int dateHour;
    public int dateDay;
    public DateWeekDayName dateWeekDay;
    public DateMonthName dateMonth;
    public int dateYear;

    public DateSystem(int hour, int day, DateMonthName month, int year)
    {
        dateHour = hour;
        dateDay = day;
        dateMonth = month;
        dateYear = year;
        dateWeekDay = GetWeakDay(day, month, year);
    }

    public DateWeekDayName GetWeakDay(int day, DateMonthName month, int year)
    {
        //get previous month days and add it
        int previousMonthDays = 0;
        switch (month)
        {
            case DateMonthName.February:
                previousMonthDays = 31;
                break;
            case DateMonthName.March:
                previousMonthDays = 59;
                break;
            case DateMonthName.April:
                previousMonthDays = 90;
                break;
            case DateMonthName.May:
                previousMonthDays = 120;
                break;
            case DateMonthName.June:
                previousMonthDays = 151;
                break;
            case DateMonthName.July:
                previousMonthDays = 181;
                break;
            case DateMonthName.August:
                previousMonthDays = 212;
                break;
            case DateMonthName.September:
                previousMonthDays = 243;
                break;
            case DateMonthName.October:
                previousMonthDays = 273;
                break;
            case DateMonthName.November:
                previousMonthDays = 304;
                break;
            case DateMonthName.December:
                previousMonthDays = 334;
                break;
        }
        //get get if current year is a leap year
        int leapYearOffset = year % 4;
        if (leapYearOffset == 0 && (int)month > 1)
        {
            previousMonthDays += 1;
        }
        //we dont count the current year for our days count, since it did not end yet and it is calculated above
        year -= 1;
        leapYearOffset = year % 4;
        //get leap years days offset and add it
        int leapYearDays = (year - leapYearOffset) / 4;
        int totalDays = day + ((year) * 365) + leapYearDays + previousMonthDays;
        totalDays %= 7;
        DateWeekDayName weekDay = (DateWeekDayName)totalDays;
        return weekDay;
    }

    public void ChangeHour(int hours)
    {
        dateHour += hours;
        if(dateHour >= 24)
        {
            int restingHours = dateHour % 24;
            int hoursToDays = (dateHour - restingHours)/24;
            dateHour = restingHours;
            ChangeDay(hoursToDays);
        }
    }
    public void ChangeDay(int days)
    {
        dateDay += days;

        //changeDayOfWeek
        int currentDayOfWeek = (int)dateWeekDay;
        int dayWeek = (days % 7) + currentDayOfWeek;
        if(dayWeek > 6)
        {
            dayWeek -= 7;
        }
        dateWeekDay = (DateWeekDayName)dayWeek;

        // if days more than 365 and year is not leap jump a whole year else if leap and more than 366 days jump a whole year And substract year amount to days saves a lot of iterations
        // if more than 365, years = days - (dyas %365) / 365, amountofleapyears = ((year%4) + years) - ((year%4) + years)%4)) / 4, dateday = dateday - ((365 * years) + amountofleapyears)
        if(dateDay > 365)
        {
            //first get 4 year loop
            int fourYearDays = (365 * 4) + 1;
            int fourYearsRest = dateDay % fourYearDays;
            int fourYearsAmount = (dateDay - (fourYearsRest)) / fourYearDays;
            Debug.Log(fourYearDays + " / " + fourYearsRest + " / " + fourYearsAmount + " / ");
            //substract the 4 year loop
            dateDay -= fourYearsAmount * fourYearDays;
            ChangeYear(fourYearsAmount * 4);

            //and now for the rest
            //actual leap state
            int actualLeapState = dateYear % 4;
            //Compare to get the leap state for the rest and loop trough the rest
            int yearsLeft = 0;
            for (int i = actualLeapState; i < actualLeapState + 4; i++)
            {
                if (dateDay < 365 || (dateDay == 365 && i == 4))
                {
                    break;
                }
                yearsLeft++;
                dateDay -= 365;
                if(i == 4)
                {
                    dateDay -= 1;
                }
            }
            ChangeYear(yearsLeft);
        }
        while (dateDay > 28)
        {
            if (dateMonth == DateMonthName.April || dateMonth == DateMonthName.June || dateMonth == DateMonthName.September || dateMonth == DateMonthName.November)
            {
                if(dateDay > 30)
                {
                    dateDay -= 30;
                }
                else
                {
                    break;
                }
            }
            else if(dateMonth == DateMonthName.February)
            {
                if (dateYear % 4 == 0)
                {
                    if(dateDay > 29)
                    {
                        dateDay -= 29;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    dateDay -= 28;
                }
            }
            else
            {
                if(dateDay > 31)
                {
                    dateDay -= 31;
                }
                else
                {
                    break;
                }
            }
            ChangeMonth(1);
        }
    }
    public void ChangeMonth(int months)
    {
        int targetMonth = (int)dateMonth + months;
        if(targetMonth > 11)
        {
            int restingMonths = targetMonth % 12;
            int monthsToYears = (targetMonth - restingMonths) / 12;
            targetMonth = restingMonths;
            ChangeYear(monthsToYears);
        }
        dateMonth = (DateMonthName)targetMonth;
    }
    public void ChangeYear(int years)
    {
        dateYear += years;
    }

}
