// triggers the change event in every input to cause validation
function validateAll(identifier) {
    $(identifier).find("*").blur();
}

// checks each input and tallys up the errors for display
// Adds has-failure to infoElement and returns false if there is atleast 1 element
function groupValidation(eID, infoElement) {

    clearFeedback(infoElement);

    var errors = 0;
    eID.find("*").each(function () {
        if ($(this).hasClass("has-failure"))
            errors++;
    });

    if (errors > 0) {
        infoElement.addClass("has-failure") // add the error class
        return false;
    }

    return true;
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

    // add the bootstrap error or success classes
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
// takes the object being validated (eID), the object to compare to (eID2), skip if both are blank (ignoreBlank)
function textEqual(eID, eID2, ignoreBlank, blankFails) {

    if (ignoreBlank) {
        if (eID.val() == "" && eID2.val() == "") {
            if (blankFails) {
                addError(eID);
                addError(eID2);
            }
            return;
        }
    }

    if (eID.val() == eID2.val()) {
        addSuccess(eID);
        addSuccess(eID2);
    }
    else {
        addError(eID);
        addError(eID2);
    }
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
    eID.siblings('.glyphicon-remove').addClass("hidden");
    eID.siblings('.glyphicon-ok').removeClass("hidden");
    eID.addClass("has-success").removeClass("has-failure");
}

// Shows and hides the required boostrap elements and adds the required bootstrap classes for failure/error
// takes the object being validated (eID)
function addError(eID) {
    eID.siblings('.glyphicon-ok').addClass("hidden");
    eID.siblings('.glyphicon-remove').removeClass("hidden");
    eID.addClass("has-failure").removeClass("has-success");
}

function clearFeedback(eID) {
    eID.removeClass("has-failure").removeClass("has-success");
    eID.siblings('.glyphicon-ok').addClass("hidden");
    eID.siblings('.glyphicon-remove').addClass("hidden");
}