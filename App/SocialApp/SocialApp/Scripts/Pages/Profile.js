﻿
function wireupHeadings() {
    $(".section-heading").mouseover(function () {
        if (!$(this).hasClass("section-heading-active"))
            $(this).addClass("section-heading-mouseover");
    });

    $(".section-heading").mouseout(function () {
            $(this).removeClass("section-heading-mouseover");
    });
    $(".section-heading").click(function () {
        $(".section-heading-active").removeClass("section-heading-active");
        $(this).addClass("section-heading-active");
        $(".secton-content").addClass("hidden");
        var headingID = $(this).attr('id');
        var section = headingID.substring(0, headingID.search("-heading"));

        $('#' + section + '-content').removeClass("hidden");

        $('#selected_heading').val(section);
    });

    $('#' + $('#selected_heading').val() + '-heading').click();

}

function wireupProfileValidation() {
    // Input Validation
    // personal
    $('#profilePersonalFName, #profilePersonalLName').on("blur keyup change", function () {
        textRequired($(this), 1);
        groupValidation($(this).closest(".tab-pane"));
    });

    $("#profileSelGender").on("blur change", function () {
        selectionRequired($(this), "");
        groupValidation($(this).closest(".tab-pane"));
    });

    $("#profileSelYear, #profileSelMonth").on("blur change", function () {
        changeDay('#profileSelDay', '#profileSelHiddenDay');
    });

    $("#profileSelYear, #profileSelMonth, #profileSelDay").on("blur change", function () {
        selectionRequired($(this), '.profile-date');
        groupValidation($(this).closest(".tab-pane"));
        $("#profileSelHiddenDay").val($("#profileSelDay").val());
    });

    // contact
    $("#profileHouseNumber, #profileStreet, #profileTown").on("blur keyup change", function () {
        textRequired($(this), 1);
        groupValidation($(this).closest(".tab-pane"));
    });
    $("#profilePostcode").on("blur keyup change", function () {
        postCodeRequired($(this));
        groupValidation($(this).closest(".tab-pane"));
    });
    $("#profileTel").on("blur keyup change", function () {
        //telRequired($(this));
        //groupValidation($(this).closest(".tab-pane"));
    });

    // login
    $("#profileEmail").on("blur keyup change", function () {
        emailRequired($(this));
        groupValidation($(this).closest(".tab-pane"));
    });
    $("#profileUsername").on("blur keyup change", function () {
        textRequired($(this), 1);
        groupValidation($(this).closest(".tab-pane"));
    });

    $("#profileConfPassword").on("blur keyup change", function () {
        textEqual($(this), $("#profileChangePassword"), false);
        groupValidation($(this).closest(".tab-pane"));
    });

    $("#profileChangePassword").on("blur keyup change", function () {
        textEqual($("#profileConfPassword"), $(this), false);
        groupValidation($(this).closest(".tab-pane"));
    });


    // submit
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
}


function setProfileDay() {
    changeDay('#profileSelDay', '#profileSelHiddenDay');
}

