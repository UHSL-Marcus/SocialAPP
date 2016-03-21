var profileCurrentTab = "";

$(document).ready(function () {

    // tab control
    // when any of the tabs are clicked, save the ID, so that on postback tab position is not lost 
    $('#profileTabs').children().on('shown.bs.tab', function (e) {
        var s = String(e.target);
        profileCurrentTab = s.substring(s.indexOf("/#") + 1, s.length);
    })

    // if a current tab position has been saved, move the active tab to that position, used after postback to retain tab position
    if (profileCurrentTab.length > 0) {
        $('#profileTabs a[href="' + profileCurrentTab + '"]').tab('show');
    }

    // input Validation
    //personal
    $('#profilePersonalFName, #profilePersonalLName').on("keyup change", function () {
        textRequired($(this), 1);
        groupValidation($(this).closest(".tab-pane"));
    });

    $("#profileSelGender").on("change", function () {
        selectionRequired($(this), "");
        groupValidation($(this).closest(".tab-pane"));
    });

    $("#profileSelYear, #profileSelMonth, #profileSelDay").on("change", function () {
        fillDays($(this));
        selectionRequired($(this), '.profile-date');
        groupValidation($(this).closest(".tab-pane"));
        $("#profileSelHiddenDay").val($("#profileSelDay").val());
    });

    //contact
    $("#profileHouseNumber, #profileStreet, #profileTown").on("keyup change", function () {
        textRequired($(this), 1);
        groupValidation($(this).closest(".tab-pane"));
    });
    $("#profilePostcode").on("keyup change", function () {
        postCodeRequired($(this));
        groupValidation($(this).closest(".tab-pane"));
    });
    $("#profileTel").on("keyup change", function () {
        //telRequired($(this));
        //groupValidation($(this).closest(".tab-pane"));
    });

    //login
    $("#profileEmail").on("keyup change", function () {
        emailRequired($(this));
        groupValidation($(this).closest(".tab-pane"));
    });
    $("#profileUsername").on("keyup change", function () {
        textRequired($(this), 1);
        groupValidation($(this).closest(".tab-pane"));
    });

    $("#profileConfPassword").on("keyup change", function () {
        textEqual($(this), $("#profileChangePassword"), false);
        groupValidation($(this).closest(".tab-pane"));
    });

    $("#profileChangePassword").on("keyup change", function () {
        textEqual($("#profileConfPassword"), $(this), false);
        groupValidation($(this).closest(".tab-pane"));
    });


    //submit
    $("#updateProfile").click(function () {
        var valid = true;
        validateAll("ProfilePage");
        $('#ProfilePage').find(".tab-pane").siblings().each(function () {
            if (groupValidation($(this)))
                valid = false;
        });

        if (valid) return true;
        else return false;
    });
});

function setProfileDay() {
    fillDays($('#profileSelDay'));
    $('#profileSelDay').val($('#profileSelHiddenDay').val());
}