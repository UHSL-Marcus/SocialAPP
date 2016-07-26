
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
        var section = headingID.substring(0, headingID.search("_heading"));

        $('#' + section + '_content').removeClass("hidden");

        $('#selected_heading').val(section);

        if ($(this).hasClass("section-hide-next")) 
            $("#section_next_btn").addClass("hidden");
        else 
            $("#section_next_btn").removeClass("hidden");

    });

    $('#' + $('#selected_heading').val() + '_heading').click();
}

function wireupFormControls() {
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
        clearFeedback($(this));
        clearFeedback($("#profileChangePassword"));
        textEqual($(this), $("#profileChangePassword"), true);
        closestSectionValidation($(this));
    });

    $("#profileChangePassword").on("blur keyup change", function () {
        clearFeedback($(this));
        clearFeedback($("#profileConfPassword"));
        textEqual( $(this), $("#profileConfPassword"), true);
        closestSectionValidation($(this));
    });


    // submit
    $("#profileUpdateBtn").click(function () {
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

