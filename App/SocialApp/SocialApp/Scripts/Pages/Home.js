

$(document).ready(function () {

});

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