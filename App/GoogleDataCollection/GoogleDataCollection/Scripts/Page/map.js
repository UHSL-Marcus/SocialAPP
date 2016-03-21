var map;
var placesService;
var geocoder;
var curView = "search";
var views = {
    search: function () {
        $('.scanTown').hide();
        $('.info').hide();
        $('.locateTown').show();
    },
    scan: function () {
        $('.scanTown').show();
        $('.locateTown').hide();
    },
    processing: function () {
        $('.scanTown').hide();
        $('.locateTown').hide();
        $('.info').show();
    }
}


function loadMap() {

    $("#map-canvas").ready(function () {

        geocoder = new google.maps.Geocoder();

        map = new google.maps.Map(document.getElementById('map-canvas'),
            {
                center: new google.maps.LatLng(-34.397, 150.644),
                zoom: 12
            });

        placesService = new google.maps.places.PlacesService(map);

        loadOverlayInit(true);
    })
}

function loadOverlayInit(listen) {
    $('#control').hide();
    $('#errorLog').hide(); // the error log text box would cause the map to only display half the tiles, delaying the showing of these elements stopped that.

    if (listen) 
        google.maps.event.addListenerOnce(map, 'tilesloaded', function () { doShowOverlay(); });
    else doShowOverlay(); 

    $('#locateTown').click(function () { locateTown(); });
    $('#scanTown').click(function () { scanTownStart(); });
    $('#newSearch').click(function () { changeView("search"); });
    $('#cancelProcessing').click(function () {
        cancelProcessing = true;
        uploadReturn(true);

    });
    
}

function doShowOverlay() {
    map.controls[google.maps.ControlPosition.TOP_LEFT].clear();
    map.controls[google.maps.ControlPosition.RIGHT].clear();
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(document.getElementById("control"));
    map.controls[google.maps.ControlPosition.RIGHT].push(document.getElementById("errorLog"));
    $('#control').show();
    $('#errorLog').show();

    reloadView();
}

function reloadView() {
    views[curView]();
}
function changeView(v) {
    views[v]();
    curView = v;
}





