

$(document).ready(function () {
    adjustForHeaderAndFooter();
});

$(window).resize(function () {
    adjustForHeaderAndFooter();
})

function adjustForHeaderAndFooter() {
    var navHeight = $('#master_header').css('height');
    var footerHeight = $('#master_footer').css('height');
    $('#MainContentHolder').css('padding-top', navHeight);
    $('#MainContentHolder').css('padding-bottom', footerHeight);

}