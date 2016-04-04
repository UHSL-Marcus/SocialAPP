

hideMode = false;

function setMouseoverSizes() {
    $(".overlay").each(function (index) {
        var classes = getElementClassArray($(this));
        var width = $(this).outerWidth(true);
        
        $.each(classes, function (index, item) {
            $(".mouseoverOverlayShowDiv." + item).width(width); 
        });
    });
}

function setHeaderHidden(hide)
{ 
    hideHeader(hide);
    hideMode = hide;
}


function hideHeader(hide, position)
{
    if (hide) {
        $(".overlay").hide();
        $('.mouseoverHeaderHideDiv').hide();
        $(".mouseoverOverlayShowDiv").height(5);
    }
    else {
        $(".overlay." + position).show();
        $('.mouseoverHeaderHideDiv').show();
        
        $(".mouseoverOverlayShowDiv." + position).height(
            $(".overlay." + position).outerHeight(true));

    }
}

function wireup_mouseoverEvent()
{
    alert("wireup");
    //$('#mouseoverHeaderHideDiv').off();
    $('.mouseoverOverlayShowDiv').mouseover(function () {
        var classes = getElementClassArray($(this));
        $.each(classes, function (index, item) {
            if (hideMode) hideHeader(false, item);
        });
        
    })

    $('.mouseoverHeaderHideDiv').mouseover(function () {
        if (hideMode) hideHeader(true, "");
    })
}

function wireup_mouseoutEvent()
{
    $('.mouseoverOverlayShowDiv').off();

    
}