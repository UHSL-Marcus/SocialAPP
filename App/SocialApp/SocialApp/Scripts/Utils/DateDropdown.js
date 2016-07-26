// fills the day dropdown in date of birth based on the month and year
function fillDays(eID) {

    var year;
    var mon;
    var day;


    eID.siblings(".dob-input").addBack().each(function () { // grabs all three dropdowns and separates them out
        if ($(this).attr('id').indexOf("SelYear") > -1)
            year = $(this);
        else if ($(this).attr('id').indexOf("SelMonth") > -1)
            mon = $(this);
        else if ($(this).attr('id').indexOf("SelDay") > -1)
            day = $(this);
    });

    if (mon.prop("selectedIndex") > 0 && year.val() != "") { // if both the year and month have something selected

        var curDay = 1; // current day being displayed, set it as default 1
        var nood = daysInMonth(mon.prop("selectedIndex"), parseInt(year.val())); // set number of days in month
        if (day.val() != null)
            curDay = day.val(); // if a day is currently selected, overwrite current day.

        if (curDay > nood) curDay = nood; // make sure it is not larger than the maximum for the selected month

        day.html("");
        for (var i = 0; i < nood; i++) { // loop through and add the HTML option tags
            var s = '<option value="' + (i + 1) + '"';
            if (i + 1 == curDay) // make sure the current day is selected
                s += 'selected="true"';
            s += '>' + (i + 1) + '</option>';

            day.append(s);
        }
    }
}

//Month is 1 based 
function daysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
}

// change the set day item based on the hidden paired input (hidden input is used in the code behind)
function changeDay(dropdown, hidden) {
    fillDays($(dropdown));
    if ($(hidden).val() < $(dropdown).length)
        $(dropdown).val($(hidden).val());
}