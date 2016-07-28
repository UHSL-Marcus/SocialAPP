
$(document).ready(function () {
    $(".section-heading").mouseover(function () {
        if (!$(this).hasClass("section-heading-active"))
            $(this).addClass("section-heading-mouseover");
    });

    $(".section-heading").mouseout(function () {
        $(this).removeClass("section-heading-mouseover");
    });

    $(".section-heading").click(function () {
        if (!$(this).hasClass("section-heading-active")) {
            $(".section-heading-active").removeClass("section-heading-active");
            $(this).addClass("section-heading-active");
            $(".section-content").addClass("hidden");
            var headingID = $(this).attr('id');
            var section = headingID.substring(0, headingID.search("_heading"));

            $('#' + section + '_content').removeClass("hidden").trigger("section:visible");

            $('#selected_heading').val(section);
        }
    });

    $('#' + $('#selected_heading').val() + '_heading').click();
});