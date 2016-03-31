// triggers the change event in every input to cause validation
function validateAll(div) {
    //$(".tab-pane").find(".form-group").children(".form-control").trigger("change");

    $('#' + div).find(".tab-pane").each(function () {
        $(this).find(".form-group").each(function () {
            $(this).children(".form-control").trigger("blur");
        });
    });
}

// checks each input in a form (eg .form-horizontal) tallys up the errors for display on the tabs
// returns true or false if there is at least one error or no errors repectivly. 
function groupValidation(eID) {
    var errors = 0;
    eID.find(".form-group").each(function () {
        if ($(this).hasClass("has-error"))
            errors++;
    });

    if (errors > 0) {
        $("#" + eID.attr("id") + "Error").text("" + errors + ""); // add the error count to the bootstrap graphic
        return true;
    }

    $("#" + eID.attr("id") + "Error").text(""); // set the error count to blank
    return false;
}

// for now this just sets all the inputs to 5
function categorySliders() {
    for (var i = 1; i < 18; i++) {
        $("#createCat" + i).val(5);
    }
}

// validation check for dropdown lists
// Takes the Element object being validated (eID) and the "class-in-common" of siblings,if any (sibs)
function selectionRequired(eID, sibs) {
    var e;
    var valid = true;
    if (sibs != "") e = eID.siblings(sibs).addBack(); // grab all siblings including self if needed
    else e = eID;


    // check each objects value
    e.each(function () {
        if ($(this).val() == "") {
            valid = false;
            //return false;
        }
    })

    // add the bootstrap error or sucess classes
    if (valid) e.each(function () { addSuccess($(this)) });
    else e.each(function () { addError($(this)) });
}

// UK Telephone validation, using an external script
// Takes the object being validated (eID)
function telRequired(eID) {
    var tel = checkUKTelephone(eID.val());
    if (!tel)
        addError(eID);
    else {
        addSuccess(eID);
        eID.val = tel;
    }
}

// UK postcode validation, using an external script
// Takes the object being validated (eID)
function postCodeRequired(eID) {
    var pc = checkPostCode(eID.val());
    if (!pc)
        addError(eID);
    else {
        addSuccess(eID);
        eID.val = pc;
    }
}

// pair validation, checks if two inputs have the same value
// takes the object being validated (eID), the object to compare to (eID2) and whether blank in an automatic fail or not (notBlank) 
function textEqual(eID, eID2, notBlank) {
    if (eID.val() == "") {
        if (notBlank)   // if blank is an automatic fail, add the fail class and exit the function
            addError(eID);
        return
    }

    if (eID.val() == eID2.val())
        addSuccess(eID);
    else addError(eID);
}

// validate Email using a fairly comprehensive regex
// takes the object being validated (eID)
function emailRequired(eID) {
    var re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (re.test(eID.val()))
        addSuccess(eID);
    else addError(eID);
}

// validates input to a required length.
// takes the object being validated (eID) and the minimum amount of charcters (count)
function textRequired(eID, count) {
    if (eID.val().length < count)
        addError(eID);
    else
        addSuccess(eID);
}

// Shows and hides the required boostrap elements and adds the required bootstrap classes for success
// takes the object being validated (eID)
function addSuccess(eID) {
    addClass(eID.siblings('.glyphicon-remove'), ["hidden"]);
    removeClass(eID.siblings('.glyphicon-ok'), ["hidden"]);
    addClass(eID.closest('.form-group'), ["has-success"]);
    removeClass(eID.closest('.form-group'), ["has-error"]);
}

// Shows and hides the required boostrap elements and adds the required bootstrap classes for failure/error
// takes the object being validated (eID)
function addError(eID) {
    addClass(eID.siblings('.glyphicon-ok'), ["hidden"]);
    removeClass(eID.siblings('.glyphicon-remove'), ["hidden"]);
    addClass(eID.closest('.form-group'), ["has-error"]);
    removeClass(eID.closest('.form-group'), ["has-success"]);
}

// defensive method to make sure a class is only added once (not sure if jquery has that built in or not), and allows more than one class to be added 
// takes the object reciving the class(es) (id) and an array of classes to add (classIDs)
function addClass(id, classIDs) {
    for (var i = 0; i < classIDs.length; i++) {
        if (!id.hasClass(classIDs[i]))
            id.addClass(classIDs[i]);
    }
}

// defensive method to makesure a class is only removed when that class is pressent (not sure if jquery has that built in or not) and allows more than one class to be removed
// takes the object losing the class(es) (id) and an array of classes to remove (classIDs)
function removeClass(id, classIDs) {
    for (var i = 0; i < classIDs.length; i++) {
        if (id.hasClass(classIDs[i]))
            id.removeClass(classIDs[i]);
    }
}