var markers = {};
var routeMarkers = {};
var markerInfo = {};
var map;
var servInfoWindow;
var directionsDisplay;
var directionsService;
var placesService;
var homeMkr;
var currentPlacesRequest;
var allServices;
var allCategories;
var allSubCategories;
var radius;


function loadMap(lat, lng, home) {
    alert("map");
    $("#map_canvas").ready(function () {

        alert("ready");

        
        directionsService = new google.maps.DirectionsService();

        var mapOptions = {
            center: { lat: lat, lng: lng },
            zoom: 11,
            mapTypeControlOptions: {
                position:google.maps.ControlPosition.LEFT
            },
        };

        map = new google.maps.Map(document.getElementById('map_canvas'),
            mapOptions);

        setDirectionRenderer();

        servInfoWindow = new google.maps.InfoWindow();
        placesService = new google.maps.places.PlacesService(map);

        if (home) {
            homeMkr = new google.maps.Marker({
                position: { lat: lat, lng: lng },
                map: map,
                title: "Home"
            });
            $('#mapRadiusInput').val(lat + "," + lng);  
        }

        loadOverlay();
    });
}

function setDirectionRenderer()
{
    if (directionsDisplay) 
        directionsDisplay.setMap(null)

    directionsDisplay = new google.maps.DirectionsRenderer(
    {
        preserveViewport: true,
        suppressMarkers: true,
        map: map,

    });
    
}

/*function setMapView(lat, lng, home) {
    alert("set View");
    if (!map)
        loadMap(lat, lng);

    map.panTo({ lat: lat, lng: lng });

    if (home) {
        homeMkr = new google.maps.Marker({
            position: { lat: lat, lng: lng },
            map: map,
            title: "Home"
        });
    }
}*/

function setOverlayInfo(services, categories, subcategories) {
    alert("settings");
    if (services) allServices = services;
    if (categories) allCategories = categories;
    if (subcategories) allSubCategories = subcategories;

    $.each(allServices, function (index, value) {
        allServices[index] = JSONstring.toObject(value);
    });

    if (map) loadOverlay();
}

function loadOverlay() {
    alert("overlay");
    
    map.controls[google.maps.ControlPosition.TOP_LEFT].clear();
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(document.getElementById("control"));

    $('#mapsTownList').change(function () {
        $('#mapsTownSelectionHidden').val($(this).val());
        return true;
    })

    $('#mapCatList').append('<option>Select Category</option><option>All</option>');
    $.each(allCategories, function (cIndex, cValue) {
        $('#mapCatList').append('<option value="' + cIndex + '">' + cIndex + '</option>');
    })

    popSubCats();

    $('#mapCatList').change(function () {
        popSubCats();
        $('#mapSubCatList').trigger('change');
    });

    $('#mapSubCatList').change(function () {
        var subcat = $('#mapSubCatList').val();
        if (subcat == "All") {
            var all = document.getElementById('mapSubCatList').options;
            subCatMarkers(all[1].value, true); // first option clears previous markers (0 option is all). 

            for (i = 2; i < all.length; i++)
                subCatMarkers(all[i].value, false);

        } else subCatMarkers(subcat, true);

    });

    $('#mapsFindRoute').click(function () {
        var start = $('#mapFromInput').val();
        var end = $('#mapToInput').val();
        var request = {
            origin: start,
            destination: end,
            travelMode: google.maps.TravelMode.DRIVING
        };

        directionsService.route(request, function (response, status) {
            if (status == google.maps.DirectionsStatus.OK) {

                clearRoute();

                directionsDisplay.setDirections(response);                      // show the route on the map

                

                $.each(allServices, function (index, value) {                   // for each service in this town

                    var found = false;
                    var servLoc = new google.maps.LatLng(+value.Latitude, +value.Longitude);

                    for (var routeItr = 0; routeItr < response.routes.length; routeItr++) {    // each route in the response
                        var route = response.routes[routeItr];

                        if (!route.largerBounds) // expand the bounds by 200 meters, to make sure all services needed are captured
                            route.largerBounds = new google.maps.LatLngBounds(findNewLatLng(route.bounds.getSouthWest(), 200, 200, false, false), findNewLatLng(route.bounds.getNorthEast(), 200, 200, true, true));
                    
                        if (route.largerBounds.contains(servLoc)) {                                 // only carry on if this service is at least inside this bounds
                            for (var legItr = 0; legItr < route.legs.length; legItr++) {            // each leg in the route
                                var leg = route.legs[legItr];
                                for (var stepItr = 0; stepItr < leg.steps.length; stepItr++) {      // each step in the leg
                                    var step = leg.steps[stepItr];
                                    var lastPathPoint;
                                    for (var pathItr = 0; pathItr < step.path.length; pathItr++) {  // each point on the path for this step
                                        var pathPoint = step.path[pathItr];
                                        if (lastPathPoint == null || distanceBetweenPoints(pathPoint, lastPathPoint) >= 200) {

                                            if (distanceBetweenPoints(pathPoint, servLoc) <= 200) {     // if service is within 200 meters

                                                addServiceMarker(value, routeMarkers);                  // show a marker
                                                addServiceToReport(value);                              // Add service to the route report
                                               
                                                found = true;                                           // service is along the route, break
                                                break;
                                            }

                                            lastPathPoint = pathPoint;                                  // this was the last used point
                                        }
                                    }
                                    if (found) break;
                                }
                                if (found) break;
                            }
                        }
                        if (found) break;
                    }
                })

                // show the report modal
                $('#routeReportModal').modal();
                $("#mapsRouteReport").removeAttr("disabled");
            }
            else alert("Directions failed: " + status);
        });
    });

    $("#mapsRouteReport").click(function () {
        $('#routeReportModal').modal();
    });

    $('#mapsClearRoute').click(function () {
        clearRoute();
    });

    $('.mapsAddFromBtn').click(function (e) {
        alert("stuff");
        alert(e.target);
    });

    $('#mapsRadiusSearch').click(function () {

        if (radius)
        {
            radius.setMap(null);
            radius = null;
        }
        else
        {
            var latLng = $('#mapRadiusInput').val().split(",");
            radius = new google.maps.Circle({
                strokeColor: '#FF0000',
                strokeOpacity: 0.6,
                strokeWeight: 1,
                fillColor: '#FF0000',
                fillOpacity: 0,
                map: map,
                center: new google.maps.LatLng(latLng[0], latLng[1]),
                radius: 2000
            });
        }

        $('#mapSubCatList').trigger('change');

    })
}

function clearRoute()
{
    routeMarkers = clearMarkerCollection(routeMarkers);
    setDirectionRenderer();                                         // reset the direction renderer
    $('#routeReportModalBody').html("");
    $('#mapsRouteReport').attr("disabled", "disabled");
}

function clearMarkerCollection(collection)
{
    $.each(collection, function (index, value) {
        value.setMap(null);
        google.maps.event.clearInstanceListeners(value);
    })
    return {};
}

function popSubCats() {
    $('#mapSubCatList').html('');
    var cat = $('#mapCatList').val();
    if (cat != "Select Category") {
        $('#mapSubCatList').html('<option>All</option>');
        if (cat == "All") {
            $.each(allCategories, function (index, value) {
                $.each(value, function (idx, val) {
                    $('#mapSubCatList').append('<option value="' + val + '">' + val + '</option>');
                })
            })
        }
        else {
            $.each(allCategories[cat], function (idx, val) {
                $('#mapSubCatList').append('<option value="' + val + '">' + val + '</option>');
            })
        }
    }
}

function subCatMarkers(subcat, clear) {

    if (clear) {
        markers = clearMarkerCollection(markers);

        markerInfo = {};

    }

    var names = "";
    $.each(allSubCategories[subcat], function (index, value) {
        var service = allServices["" + value];
        if (radius) {   // check service distance from search centre if requried
            var centrelatLng = $('#mapRadiusInput').val().split(",");

            if (distanceBetweenPoints(new google.maps.LatLng(centrelatLng[0], centrelatLng[1]),
                new google.maps.LatLng(service.Latitude, service.Longitude)) <= 2000)
                addServiceMarker(service, markers);
        }
        else addServiceMarker(service, markers);
    });
}

function addServiceMarker(service, array)
{
    if (array["" + service.ServiceID] == null) {

        var servMarker = new google.maps.Marker({
            position: { lat: +service.Latitude, lng: +service.Longitude },
            map: map,
            title: service.Name
        });


        servMarker.setMap(map);


        array["" + service.ServiceID] = servMarker;



        //array.push({ id: service.ServiceID, marker: servMarker });


        var content = "<div class='flex-container'>" +
           "<div class='flex-1'>" +
           "<h3>" + service.Name + "</h3>" +
           "<h4>Rating</h4>" + service.Rating + "/10" +
           "<h4>Categories</h4>";

        //$.each(service.CategoryIDs, function (i, v) { alert(v); })

        $.each(service.CategoryNames, function (index, value) {
            content += value + " ";
        })
        content += "<h4>SubCategories</h4>";


        $.each(service.SubCategoryNames, function (index, value) {
            content += value + " ";
        })

        var virt = service.VirtualServices;
        if (service.HasVirtualServices.toLowerCase() == "true") {
            content += "<h4>Website</h4>";
            if (typeof (virt.string) != "string") {

                for (i = 0; i < virt.string.length; i++)
                    content += virt.string[i] + "</br>";
            } else
                content += virt.string + "</br>";
        }

        content += "</div>" +
            "<div style='margin-top:15%;'>" +
            "<input type='button' onclick='addFrom(\"" + service.Latitude + "," + service.Longitude + "\")' value='From' /></br>" +
            "<input type='button' onclick='addTo(\"" + service.Latitude + "," + service.Longitude + "\")' value='To' />" +
            "</div>" +
            "</div>";


        var regName = service.Name.replace(/\W/, "");
        markerInfo[regName] = content;


        google.maps.event.addListener(servMarker, 'click', function () {
            servInfoWindow.setContent(markerInfo[regName]);
            servInfoWindow.open(map, servMarker);
        });

    }
}

function markerAllreadyPresent(collection, id)
{
    for (var i = 0; i < collection.length; i++)
        if (collection[i].id == id) return true;

    return false;
}

function addFrom(coord) {
    $('#mapFromInput').val(coord);
}
function addTo(coord) {
    $('#mapToInput').val(coord);
}




Number.prototype.toRadians = function () {
    return this * Math.PI / 180;
}
Number.prototype.toDegrees = function () {
    return this * 180 / Math.PI;
}
// Calculates the distance between two google.maps.latlng points
// pointA: first point
// pointB: second point
// φ = Lat in radians
// λ = lng in radians
function distanceBetweenPoints(pointA, pointB) {

    var R = 6371000; // metres

    var φ1 = pointA.lat().toRadians();
    var φ2 = pointB.lat().toRadians();
    var Δφ = (pointB.lat() - pointA.lat()).toRadians();
    var Δλ = (pointB.lng() - pointA.lng()).toRadians();

    var a = Math.sin(Δφ / 2) * Math.sin(Δφ / 2) +
            Math.cos(φ1) * Math.cos(φ2) *
            Math.sin(Δλ / 2) * Math.sin(Δλ / 2);
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));

    return R * c;
}

// returns the lat and lng the passed distance north and east from the orignal location (google.maps.LatLng)
// oriLoc: orignal location (google.maps.LatLng)
// latDist: distance to move on latitude (meters)   default 0
// lngDist: distance to move on longtitude (meters) default 0
// north: true = north, false = south
// east:  true = east, false = west
function findNewLatLng(oriLoc, latDist, lngDist, north, east) {

    latDist = typeof latDist !== 'undefined' ? latDist : 0;
    lngDist = typeof lngDist !== 'undefined' ? lngDist : 0;

    // slightly simplified algorithm/calculation to find new lat lng coords
    // javascript maths functions work in radians( deg to rad: * Math.PI / 180)

    var latRad = oriLoc.lat().toRadians();

    // variables to model the shape of the earth (simply)
    var earthR = 6378137;
    var eSquared = 0.00669438000426;

    // f = latRad
    var sinF = Math.sin(latRad);

    // sqrt(1 - e^2 sin(f)^2)
    var sqrtF = Math.sqrt(1 - eSquared * sinF ^ 2);

    // for calc lng
    // r = earthR * cos(f) / sqrtF 
    var r = earthR * Math.cos(latRad) / sqrtF;

    // for calc lat
    // s = R * (1- e^2) / sqrtF^3
    var s = earthR * (1 - eSquared) / sqrtF ^ 3;

    var newLat = oriLoc.lat();
    if (latDist) {
        // direction lat distance / s. ( to degrees --> * 180 / Math.PI)
        var dLat = (latDist / s).toDegrees();
        if (north)
            newLat = (oriLoc.lat() + dLat).toPrecision(9);
        else
            newLat = (oriLoc.lat() - dLat).toPrecision(9);
    }

    var newLng = oriLoc.lng();
    if (lngDist) {
        //direction lng distance / r.
        var dLng = (lngDist / r).toDegrees();
        if (east)
            newLng = (oriLoc.lng() + dLng).toPrecision(9);
        else
            newLng = (oriLoc.lng() - dLng).toPrecision(9);
    }
    return new google.maps.LatLng(newLat, newLng);
}


function addServiceToReport(service) {

    var content = "<div>" +
                        "<button class=\"btn btn-default\" type=\"button\" data-toggle=\"collapse\" data-target=\"#" + service.Name.replace(/\W/g, '') + "CollapseInfo\">" + service.Name + "</button>" +
                        "<div class=\"collapse\" id=\"" + service.Name.replace(/\W/g, '') + "CollapseInfo\">" +
                            "<h3>" + service.Name + "</h3>" +
                            "<h4>Rating</h4>" + service.Rating + "/10";

                                var virt = service.VirtualServices;
                                if (service.HasVirtualServices.toLowerCase() == "true") {
                                    content += "<h4>Website</h4>";
                                    if (typeof (virt.string) != "string") {

                                        for (i = 0; i < virt.string.length; i++)
                                            content += virt.string[i] + "</br>";
                                    } else
                                        content += virt.string + "</br>";
                                }
        content +=      "</div>" +
                    "</div>";

        $("#routeReportModalBody").append(content);

}
