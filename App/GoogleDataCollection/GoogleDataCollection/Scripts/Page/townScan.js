var townToScan;                     // holds geocode information of the town to scan
var townMarker;                     // holds the map marker to visually show the town selected
var queueRunning = false;           // flag to show if request queue is being operated on
var requestQueue = [];              // stores the queued requests
var requestPending = 0;             // Stores how many requests have been sent but not recieved a response
var searchMarkers = [];             // stores all the markers used to depict search areas
var searchResults = [];             // stores all ID's and marker locations for the collected services
var searchResultsRejected = [];     // stores the ID's of services collected but rejected (preliminary tests find they are not in the requested town)
var serviceDetails = [];            // stores the details of each service (after detail request to google)
var requestCount = 0;               // temporary
var serviceTypes = [];
var cancelProcessing = false;       // Flag to cause the processing to cancel



// in ran when user clicks "locate town", takes input from the seach text box, sends a request to google.
// asks users to cycle throught the responses and choose the one they want
function locateTown() {
    $('#locateTown').prop('disabled', true);    
    var address = $('#towntoSearch').val();     

    // make geocode request
    geocoder.geocode({ 'address': address }, function (results, status) {
                               
        if (status == google.maps.GeocoderStatus.OK) {

            // sometimes multiple options are returned
            townToScan = results;

            // populate dropdown with town options
            $('#selectedTown').html("");
            for (var i = 0; i < townToScan.length; i++) 
                $('#selectedTown').append('<option value="' + i + '">' + (i + 1) + "/" + townToScan.length + ", " + townToScan[i].formatted_address + '</option>');

            changeView("scan");
            $('#towntoSearch').val("");
            $('#locateTown').prop('disabled', false);
            
            $('#selectedTown').change(function () {
                var val = $('#selectedTown').val();
                if (townMarker)
                    townMarker.setMap(null);                                        // remove current marker

                townMarker = centerAndMarker(townToScan[val].geometry.location);    // add new marker and move map
            });

            $('#selectedTown').trigger('change'); // trigger the event for the first option

        } else {
            log('Geocode was not successful for the following reason: ' + status);
            $('#locateTown').prop('disabled', false);
        }

    });
}


// centres map on passed location and creates and returns a marker
function centerAndMarker(loc) {
    map.setCenter(loc);
    return new google.maps.Marker({
        map: map,
        position: loc
    });
}

// fired when "scan town" is clicked. Starts off the scanning process
function scanTownStart() {
    cancelProcessing = false;
    queueRunning = false;
    requestQueue = [];
    searchResults = [];
    requestCount = 0;
    requestPending = 0;

    changeView("processing");

    $('#infoDisplay').val("Scanning...");


    townMarker.setMap(null);                                                // hide the confirmation marker
    townToScan = townToScan[$('#selectedTown').val()];
    getnearby(townToScan.geometry.location,                                // grab town centre
        findAddressComponent(townToScan.address_components, "locality"),   // locality is the value we check against
        "all");   // send the inital request
}

// operates on the request queues, needed to slow down request rate, (max 5/second) to be safe we send one every half second. 
function runNearbyQueue() {
    if (!cancelProcessing) {
        queueRunning = true;
        var tempResult = [requestQueue[0].l, requestQueue[0].t, requestQueue[0].d];
        requestQueue.splice(0, 1);                                          // remove sent request from the array

        //if (requestCount < 6) 
        if (requestQueue.length > 0)                                        // either self recurse or end
            setTimeout(runNearbyQueue, 500);
        else queueRunning = false;

        getnearby(tempResult[0], tempResult[1], tempResult[2]);             // so the queue running check and flag is set before this is fired off.


        $('#infoDisplay').val("count: " + requestQueue.length + " q: " + queueRunning + " P: " + requestPending);
    }

}

// sends the search request to google handles the result.
// location is lat lng of search area
// townName is string to compare address of returned services with
// direction is related to finding which locations to search next
function getnearby(location, townName, direction) {

    var request = {
        location: location,
        radius: 1000 // not accurate, to get more results, need a smaller search area (250m gave 1.8k buisnesses), but also can go over query limit?, maybe nearing daily limit.  
    };

    var info = "";
    requestPending++;                                                   // there is a request pending
    requestCount++;

    placesService.nearbySearch(request, function (results, status) {
        requestPending--;                                               // one request has returned

        if (status == google.maps.places.PlacesServiceStatus.OK) {
            var inTown = false;

            $.each(results, function (index, value) {
                var duplicate = false;
                for (var i = 0; i < searchResults.length; i++)
                    if (searchResults[i].id == value.place_id) { duplicate = true; break; }

                if (!duplicate) {
                    if (containsString(value.vicinity, townName)) {               // check each service in the reply, see if it has the townName in its short address
                        inTown = true;

                        var marker = new google.maps.Marker({
                            position: value.geometry.location,
                            map: map,
                            icon: {
                                // Star
                                path: 'M 0,-24 6,-7 24,-7 10,4 15,21 0,11 -15,21 -10,4 -24,-7 -6,-7 z',
                                fillColor: '#ffff00',
                                fillOpacity: 1,
                                scale: 1 / 4,
                                strokeColor: '#bd8d2c',
                                strokeWeight: 1
                            },
                            title: value.name
                        });

                        searchResults.push({ id: value.place_id, marker: marker });         // create a marker and push it and the service ID into the results array

                    } else searchResultsRejected.push(value.place_id);                     // store the services rejected in this first round of evaluation
                }
            });

            if (inTown) {                                                               // at least one service was in the town, so continue search further out
                var marker = new google.maps.Marker({                                   
                    position: location,
                    map: map
                });
                searchMarkers.push(marker);                                             // visually show that this search was a sucess, put a marker down

                var newLocs = findNewLocations(location, direction, 1000);              // find the next set of locations to search
                $.each(newLocs, function (index, value) {
                    requestQueue.push({ l: value.l, t: townName, d: value.d });         // add each one to the queue
                });

                if (!queueRunning) runNearbyQueue();                                    // run the queue if needed

            }
        } else log("Bad nearby Request: " + status);

        //if (!queueRunning) {
        if (requestPending <= 0 && !queueRunning || cancelProcessing) {                 // work out if the search is exhausted, no pending requests and the queue not running
            if (!cancelProcessing) {
                //all done
                requestCount = 0;
                $('#TownData').val(JSON.stringify(townToScan));                            // to indentify this town

                for (var i = 0; i < searchMarkers.length; i++) {                            // remove all the search area markers
                    searchMarkers[i].setMap(null);
                }

                $('#infoDisplay').val("Getting Service Details....");
                runDetailQueue();                                                           // run the next section (service details requests)
            }
            else uploadReturn(true);
        }

    });
}

// Finds new lat lng a set distance away from the orignal location
// oriLoc: orignal location
// direction: related to the search pattern, the direction dictates which and how many new locations are returned
// distance: distance in meters from oriLoc
function findNewLocations(oriLoc, direction, distance) {

    // slightly simplified algorithm/calculation to find new lat lng coords
    // javascript maths functions work in radians( deg to rad: * Math.PI / 180)

    var latRad = oriLoc.lat() * Math.PI / 180;

    // variables to model the shape of the earth (simply)
    var earthR = 6378137;
    var eSquared = 0.00669438000426;

    // f = latRad
    var sinF = Math.sin(latRad);

    // sqrt(1 - e^2 sin(f)^2)
    var sqrtF = Math.sqrt(1 - eSquared * sinF ^ 2);

    // for calc lat
    // r = earthR * cos(f) / sqrtF 
    var r = earthR * Math.cos(latRad) / sqrtF;

    // for calc lng
    // s = R * (1- e^2) / sqrtF^3
    var s = earthR * (1 - eSquared) / sqrtF ^ 3;

    // direction lat distance (500m) / s. ( to degrees --> * 180 / Math.PI)
    var dLat = (distance / s) * 180 / Math.PI;

    //direction lng distance / r.
    var dLng = (distance / r) * 180 / Math.PI;

    // North: oriLat + dLat
    // South: oriLat - dLat
    // East:  oriLng + dLng
    // West:  oriLng - dlng
    var newLocN = new google.maps.LatLng((oriLoc.lat() + dLat).toPrecision(9), oriLoc.lng().toPrecision(9), false);
    var newLocS = new google.maps.LatLng((oriLoc.lat() - dLat).toPrecision(9), oriLoc.lng().toPrecision(9), false);
    var newLocE = new google.maps.LatLng(oriLoc.lat().toPrecision(9), (oriLoc.lng() + dLng).toPrecision(9), false);
    var newLocW = new google.maps.LatLng(oriLoc.lat().toPrecision(9), (oriLoc.lng() - dLng).toPrecision(9), false);

    // based on the direction, returns the correct data
    if (direction == "n")
        return [{ l: newLocN, d: "n" }];

    if (direction == "s")
        return [{ l: newLocS, d: "s" }];

    if (direction == "e")
        return [{ l: newLocE, d: "e" }];

    if (direction == "w")
        return [{ l: newLocW, d: "w" }];

    if (direction == "ens")
        return [{ l: newLocE, d: "ens" }, { l: newLocN, d: "n" }, { l: newLocS, d: "s" }];

    if (direction == "wns")
        return [{ l: newLocW, d: "wns" }, { l: newLocN, d: "n" }, { l: newLocS, d: "s" }];

    if (direction == "all")
        return [{ l: newLocN, d: "n" }, { l: newLocS, d: "s" }, { l: newLocE, d: "ens" }, { l: newLocW, d: "wns" }];
}

var waitTime = 700;
var successCount = 0;
// same style queue operation as above, but with adaptive wait time
// for every overlimit error 100 ms is added to the pause
// for every 3 requests that don't incur a overlimit error, remove 100ms.
function runDetailQueue() {
    if (!cancelProcessing) {

        log("\nInital Length " + searchResults.length);

        queueRunning = true;
        var tempResult = searchResults[0];
        searchResults.splice(0, 1);

        log("requestData: " + tempResult);
        log(waitTime + " " + searchResults.length + " remaining");

        //if (requestCount < 10)
        if (searchResults.length > 0)
            setTimeout(runDetailQueue, waitTime);
        else queueRunning = false;

        if (tempResult)
            detailsRequest(tempResult); // so the queue running check and flag is set before this is fired off.

        log("queue: " + queueRunning);
        log("Processing: " + requestPending);


        $('#infoDisplay').val("count: " + searchResults.length + " q: " + queueRunning + " P: " + requestPending);
    }
}

// simlar to above, run the request for service details
function detailsRequest(service) {
    var request = { placeId: service.id };
    log("id: " + service.id);
    requestPending++;                                                                               // request is pending
    requestCount++;
    placesService.getDetails(request, function (place, status) {
        log("async reply");
        requestPending--;                                                                           // request returned
        if (status == google.maps.places.PlacesServiceStatus.OK) {
            successCount++
            if (waitTime > 100) {
                successCount = 0;
               // waitTime -= 50;
            }

            

            var locality = findAddressComponent(place.address_components, "locality");              // simplify these lines
            var postalTown = findAddressComponent(place.address_components, "postal_town");
            var testLocality = findAddressComponent(townToScan.address_components, "locality");
            if (containsString(locality, testLocality) || containsString(postalTown, testLocality)) {           // make sure this service actually is in the town/city specified
                var marker = new google.maps.Marker({
                    position: place.geometry.location,
                    map: map,
                    title: place.name
                });
                searchMarkers.push(marker);                                                         // put a marker to show that is has been processed

                delete place.photos;                                                                // prune out unneeded info from the object
                delete place.price_level;
                delete place.url;
                delete place.adr_address;

                for (var i = 0; i < place.types.length; i++) {
                    if ($.inArray(place.types[i], serviceTypes) == -1)
                        serviceTypes.push(place.types[i]);
                }

                JSONstring.compactOutput = true;
                serviceDetails.push(JSON.stringify(place));
                //serviceDetails.push(JSONstring.make(place));                                        // stringify (serialize) the object into a JSON string
            }

            service.marker.setMap(null);                                                            // remove the marker placed in the initial search
        } else {
            log("Bad details Request: " + status);
            if (status == google.maps.places.PlacesServiceStatus.OVER_QUERY_LIMIT || status == google.maps.places.PlacesServiceStatus.UNKNOWN_ERROR) {
                successCount = 0;
                //if (waitTime < 1000)
                    //waitTime += 300;

                searchResults.push(service); // adding the service back into the pending list
                log("Re-added");
                log("queue running?: " + queueRunning);
                if (!queueRunning) runDetailQueue();
            }  
        }

        //if (!queueRunning) {   
        log("Queue Async: " + queueRunning);
        log("processing Async: " + requestPending);
        if (requestPending <= 0 && !queueRunning || cancelProcessing) {                                                 // once again, wait till the end
            //all done
            if (!cancelProcessing) updateServiceData();
            else uploadReturn(true);
        }
    });
    log("end");
}

// puts the data into a hidden input on the aspx page and causes a post back via button click
function updateServiceData() {
    $('#ServiceData').val(serviceDetails[0]);           // add top service to the field for upload
    serviceDetails.splice(0, 1);                        // remove that service from the array
    document.getElementById('uploadResults').click();   // cause postback
}

function uploadReturn(cancel) {
    for (var i = 0; i < searchMarkers.length; i++) {                            // remove all the Markers
        searchMarkers[i].setMap(null);
    }

    for (var i = 0; i < searchResults.length; i++) {
        searchResults[i].marker.setMap(null);
    }

    if (!cancel)
        document.getElementById('UpdateCountAndNormal').click();   // cause postback
    else
       alert("Upload Cancelled");

}

function uploadFinalReturn()
{
    alert("uploaded");
    uploadCompleted = false;
    changeView("search");
}

// a caseless comparison of two string. return if A (string) contains B (contains).
function containsString(string, contains) {
    if (typeof string == 'undefined' || typeof contains == 'undefined') return false;
    return (string.toUpperCase().indexOf(contains.toUpperCase()) > -1);
}

// loops through the google geocode address component array, returns the name of the componet of type (comp).
function findAddressComponent(address, comp) {
    for (var i = 0; i < address.length; i++) {
        for (var ti = 0; ti < address[i].types.length; ti++) {
            if (containsString(address[i].types[ti], comp)) return address[i].long_name;
        }
    }
    return "";
}

function log(s) {
    var v = $('#errorText').val();
    $('#errorText').val(v + "\n" + s);
}

