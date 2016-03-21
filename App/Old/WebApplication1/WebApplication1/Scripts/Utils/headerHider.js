

hideMode = false;

function setHeaderHidden(hide)
{
    if (hide) modeSwitch(false);
    else modeSwitch(true);
    hideHeader(hide);
    hideMode = hide;
}

function modeSwitch(overlay)
{
    if (overlay) $('.headerFixed').removeClass("headerFixed").addClass("headerOverlay"); 
    else $('.headerOverlay').removeClass("headerOverlay").addClass("headerFixed");
}

function hideHeader(hide)
{
    if (hide) {
        $(".headerFixed").hide();
        $('#mouseoverHeaderDiv').show();
        $('#mouseoverHeaderHideDiv').hide();
        
    }
    else {
        $(".headerFixed").show();
        $('#mouseoverHeaderDiv').hide();
        $('#mouseoverHeaderHideDiv').show();
        $('#mouseoverHeaderHideDiv').css("top", Math.ceil($('.headerFixed').height())+"px");
    }
}

function wireup_mouseoverEvent()
{
    //$('#mouseoverHeaderHideDiv').off();
    $('#mouseoverHeaderDiv').mouseover(function () {
        if (hideMode) hideHeader(false);
    })

    $('#mouseoverHeaderHideDiv').mouseover(function () {
        if (hideMode) hideHeader(true);
    })
}

function wireup_mouseoutEvent()
{
    $('#mouseoverHeaderDiv').off();

    
}