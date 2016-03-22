
function adjustForHeaderAndFooter() {
    var navHeight = $('#master_header').css('height');
    var footerHeight = $('#master_footer').css('height');
    $('#MainContentHolder').css('padding-top', navHeight);
    $('#MainContentHolder').css('padding-bottom', footerHeight);

}

function hideSignoutBtn(hide) {
    if (hide) $('#signOutBtn').hide();
    else $('#signOutBtn').show();
}