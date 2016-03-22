pageBackgroundColour = {
    main: "",
    profile: "#1ba1e2",
    stats: "#662E93",
    maps: "#60a917"
}
pageBackgroundImage = {
    main: "url(/Resources/Backgrounds/Background.png)",
    profile: "",
    stats: "",
    maps: ""
}


currentPage = "main"


$(document).ready(function () {

    resizeHeaderIcon();

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
        resizeHeaderIcon();

    }

    // Called when async postback ends
    function prm_EndRequest(sender, args) {
        $('#loadingModal').modal('hide')
        resizeHeaderIcon();
    }

    function centerModal() {
        $(this).css('display', 'block');
        var $dialog = $(this).find(".modal-dialog");
        var offset = ($(window).height() - $dialog.height()) / 2;
        // Center modal vertically in window
        $dialog.css("margin-top", offset);
    }

    $('#loadingModal').on('show.bs.modal', centerModal);
    $(window).on("resize", function () {
        $('#loadingModal:visible').each(centerModal);
    });

    setView();
});

function changeCurrentPage(page)
{
    currentPage = page;
    setView();
}

function setView()
{
    $(document.body).css("background-image", pageBackgroundImage[currentPage]);
    $(document.body).css("background-color", pageBackgroundColour[currentPage]);

    if (currentPage == "main") $('#homeBtn').hide();
    else $('#homeBtn').show();
}

function hideSignoutBtn(hide)
{
    if (hide) $('#signOutBtn').hide();
    else $('#signOutBtn').show();
}

function wireUp_LoginEvents() {
    //input Validation
    //login
    $("#userIn, #passwordIn").on("blur keyup change", function () {
        textRequired($(this), 1);
    });

    // validate everything before allowing or supressing postback for logging in
    $("#loginBtn").click(function () {
        var valid = true;
        $(this).siblings(".form-group").each(function () {
            if (!$(this).hasClass("has-success")) {
                valid = false;
                addError($(this));
            }
        });

        if (valid) return true;
        else return false;
    });

    $('#toggleCreate, #loginBtn').click(function () {
        $('#loginModal').modal('hide')          // hide modal before postback when loading the create account view
        return true;
    });

}


function showLogin() {
    $('#loginModal').modal();
}

function resizeHeaderIcon()
{
   // $("#headerProfileBtn").height($(window).height() * 0.1);  
  //  $('#signoutBtnContainer').width($("#headerProfileBtn").width());
}

$(window).resize(function () {
    resizeHeaderIcon();
})


