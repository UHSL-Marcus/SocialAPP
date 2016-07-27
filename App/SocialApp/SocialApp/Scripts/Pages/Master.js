
$(document).ready(function () {
    setMouseoverSizes();

    // Get the instance of PageRequestManager.
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    // Add initializeRequest and endRequest
    prm.add_initializeRequest(prm_InitializeRequest);
    prm.add_endRequest(prm_EndRequest);

    // Called when async postback begins
    function prm_InitializeRequest(sender, args) {
        $('#loadingModal').modal({            // show the modal with some options
            keyboard: false,
            backdrop: "static"
        })
    }

    // Called when async postback ends
    function prm_EndRequest(sender, args) {
        $('#loadingModal').modal('hide')
    }
    addModal("loadingModal");

});

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

function hideBackBtn() {
    $("#homeBtn").addClass("hidden");
}