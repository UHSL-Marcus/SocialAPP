
var profile_mode;
var ModeEnum = {
    CREATE: "create",
    UPDATE: "update"
}

function wireupFormControls(mode) {

    profile_mode = mode;
    $("#section_next_btn").click(function () {
        $(".section-heading-active").next(".section-heading").click();
    });

    $(".input_range").on("input change", function () {
        var num = $(this).val();
        var num = num < 10 ? "0" + num : num;
        $(this).siblings(".range_number").html(num);

        var info = $('#lifestyle_info').val();
        var id = $(this).data("id");
        var id = id < 10 ? "0" + id : id;
        var loc = info.search(id + ":");
        var value = id + ":" + num;
        var new_value;

        if (loc > -1) {
            var old_value = info.substring(loc, loc + 5);
            new_value = info.replace(old_value, value);
            
        } else {
            new_value = info + "," + value;
        }

        $('#lifestyle_info').val(new_value);

    });

    $(".input_range").trigger("input").trigger("change");

    $(".section-content").on("section:visible", function () {
        if ($(this).hasClass("section-hide-next"))
            $("#section_next_btn").addClass("hidden");
        else
            $("#section_next_btn").removeClass("hidden");
    });

}

function wireupProfileValidation() {
    // Input Validation
    // personal
    $('#profilePersonalFName, #profilePersonalLName').on("blur keyup change", function () {
        textRequired($(this), 1);
        closestSectionValidation($(this));
    });

    $("#profileSelYear, #profileSelMonth").on("blur change", function () {
        changeDay('#profileSelDay', '#profileSelHiddenDay');
    });

    $("#profileSelYear, #profileSelMonth, #profileSelDay").on("blur change", function () {
        selectionRequired($(this), '.dob-input');
        closestSectionValidation($(this));
        $("#profileSelHiddenDay").val($("#profileSelDay").val());
    });

    // contact
    $("#profileHouseNumber, #profileStreet, #profileTown").on("blur keyup change", function () {
        textRequired($(this), 1);
        closestSectionValidation($(this));
    });
    $("#profilePostcode").on("blur keyup change", function () {
        postCodeRequired($(this));
        closestSectionValidation($(this));
    });
    $("#profileTel").on("blur keyup change", function () {
        //telRequired($(this));
        //closestSectionValidation($(this));
    });

    // login
    $("#profileEmail").on("blur keyup change", function () {
        emailRequired($(this));
        closestSectionValidation($(this));
    });
    $("#profileUsername").on("blur keyup change", function () {
        textRequired($(this), 1);
        closestSectionValidation($(this));
    });

    $("#profileConfPassword").on("blur keyup change", function () {
        
        if (profile_mode === ModeEnum.UPDATE) {
            clearFeedback($(this));
            clearFeedback($("#profileChangePassword"));
            textEqual($(this), $("#profileChangePassword"), true);
        }
        else if (profile_mode === ModeEnum.CREATE)
            textEqual($(this), $("#profileChangePassword"), true, true);
            

        closestSectionValidation($(this));
    });

    $("#profileChangePassword").on("blur keyup change", function () {
        
        if (profile_mode === ModeEnum.UPDATE) {
            clearFeedback($(this));
            clearFeedback($("#profileConfPassword"));
            textEqual($(this), $("#profileConfPassword"), true);
        }
        else if (profile_mode === ModeEnum.CREATE)
            textEqual($(this), $("#profileConfPassword"), true, true);
            

        closestSectionValidation($(this));
    });


    // submit
    $("#profileUpdateBtn, #profileCreateBtn").click(function () {
        var valid = true;
        validateAll(".secton-content");
        
        $(".section-heading").each(function () {
            if ($(this).hasClass("has-failure"))
                valid = false;
            return false;
        });

        if (valid) return true;
        else return false;
    });
}


function setProfileDay() {
    changeDay('#profileSelDay', '#profileSelHiddenDay');
}

function closestSectionValidation(eID) {
    var content_section = eID.closest(".secton-content");
    
    var contentID = content_section.attr('id');
    var heading = contentID.substring(0, contentID.search("_content")) + "_heading";

    groupValidation(content_section, $("#" + heading));
}

