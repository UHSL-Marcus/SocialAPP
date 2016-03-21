var createCurrentTab = "";


// make sure all events get wired up after each postback. 
function Create() {

    // tab control
    // when any of the tabs are clicked, save the ID, so that on postback tab position is not lost 
    $('#createTabs').children().on('shown.bs.tab', function (e) {
        var s = String(e.target);
        createCurrentTab = s.substring(s.indexOf("/#") + 1, s.length);
    })

    // if a current tab position has been saved, move the active tab to that position, used after postback to retain tab position
    if (createCurrentTab.length > 0) {
        $('#createTabs a[href="' + createCurrentTab + '"]').tab('show');
    }

    // input Validation
    //personal
    $("#createPersonalFName, #createPersonalLName").on("blur keyup change", function () {
        textRequired($(this), 1);
        groupValidation($(this).closest(".tab-pane"));
    });

    $("#createSelGender").on("blur change", function () {
        selectionRequired($(this), "");
        groupValidation($(this).closest(".tab-pane"));
    });

    $("#createSelYear, #createSelMonth, #createSelDay").on("blur change", function () {
        fillDays($(this));
        selectionRequired($(this), ".create-date");
        groupValidation($(this).closest(".tab-pane"));
        $("#createSelHiddenDay").val($("#createSelDay").val());
    });

    // profile
    $("#createEmail").on("blur keyup change", function () {
        emailRequired($(this));
        groupValidation($(this).closest(".tab-pane"));
    });
    $("#createUsername, #createPassword").on("blur keyup change", function () {
        textRequired($(this), 1);
        groupValidation($(this).closest(".tab-pane"));
    });
    $("#createConfPassword").on("blur keyup change", function () {
        textEqual($(this), $("#createPassword"), true);
        groupValidation($(this).closest(".tab-pane"));
    });

    //address
    $("#createHouseNumber, #createStreet, #createTown").on("blur keyup change", function () {
        textRequired($(this), 1);
        groupValidation($(this).closest(".tab-pane"));
    });
    $("#createPostcode").on("blur keyup change", function () {
        postCodeRequired($(this));
        groupValidation($(this).closest(".tab-pane"));
    });
    $("#createTel").on("blur keyup change", function () {
        telRequired($(this));
        groupValidation($(this).closest(".tab-pane"));
    });

    //Check validation  of create account inputs before allowing or surpresing postback
    $("#submitCreate").click(function () {
        var valid = true;
        validateAll("CreatePage");          // make sure all inputs have been validated
        $("#CreatePage").find(".tab-pane").each(function () {
            if (groupValidation($(this)))
                valid = false;
        });

        if (valid) return true;
        else return false;
    });
 
}



