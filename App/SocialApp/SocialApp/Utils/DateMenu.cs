using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocialApp.Utils
{
    public class DateMenu
    {
        private DropDownList selYear;
        private DropDownList selMonth;
        private DropDownList selDay;

        public DateMenu(DropDownList y, DropDownList m, DropDownList d)
        {
            selYear = y;
            selMonth = m;
            selDay = d;
        }
        public void setDateDropdown(int day = 0, int month = 0, int year = 0)
        {
            //Fill Years
            selYear.Items.Clear();
            if (year == 0) selYear.Items.Add("");
            for (int i = DateTime.Now.Year - 100; i <= DateTime.Now.Year; i++)
            {
                selYear.Items.Add(i.ToString()); //CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(8)
            }
            String y = "";
            if (year != 0) y = year.ToString();
            selYear.Items.FindByValue(y).Selected = true;


            //Fill Months
            selMonth.Items.Clear();
            if (month == 0) selMonth.Items.Add("");
            for (int i = 1; i <= 12; i++)
            {
                selMonth.Items.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i));
            }
            String m = "";
            if (month != 0) m = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);
            selMonth.Items.FindByValue(m).Selected = true;

            //Fill days
            fillDays(day);
        }

        public void fillDays(int day = 0)
        {
            if (!selYear.SelectedValue.Equals("") && !selMonth.SelectedValue.Equals(""))
            {
                // save currently selected day
                if (day < 0)
                    day = Convert.ToInt16(selDay.SelectedValue);

                
                selDay.Items.Clear();
                //getting numbner of days in selected month & year
                int noofdays = DateTime.DaysInMonth(Convert.ToInt32(selYear.SelectedValue), Convert.ToInt32(selMonth.SelectedIndex + 1));
                //Fill days
                for (int i = 1; i <= noofdays; i++)
                {
                    selDay.Items.Add(i.ToString());
                }

                // make sure selected day is not out of range
                if (day > noofdays)
                    day = noofdays;

                selDay.Items.FindByValue(day.ToString()).Selected = true;
                

            }
        }

        public static void setDateDropdown(DropDownList sMonth, DropDownList sYear, int month = 0, int year = 0)
        {
            //Fill Years
            sYear.Items.Clear();
            sYear.Items.Add("");
            for (int i = DateTime.Now.Year - 100; i <= DateTime.Now.Year; i++)
            {
                sYear.Items.Add(i.ToString()); //CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(8)
            }
            string y = "";
            if (year != 0) y = year.ToString();
            sYear.Items.FindByValue(y).Selected = true;


            //Fill Months
            sMonth.Items.Clear();
            sMonth.Items.Add("");
            for (int i = 1; i <= 12; i++)
            {
                sMonth.Items.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i));
            }
            string m = "";
            if (month != 0) m = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);
            sMonth.Items.FindByValue(m).Selected = true;
        }
    }
}