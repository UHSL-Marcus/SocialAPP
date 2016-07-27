
var statsCurrentTab = "";
var statsCurrentDataPoints = 0;
var userRatings;
var townRatings;
var townVirtRatings;



function LoadStats(userRatingsJson, townRatingsJson, townVirtRatingsJson) {

    if (userRatingsJson) userRatings = userRatingsJson;
    if (townRatingsJson) townRatings = townRatingsJson;
    if (townVirtRatingsJson) townVirtRatings = townVirtRatingsJson;

    // when the modal is loaded up, set the correct title and show the section required
    $('#statExpandModal').on('show.bs.modal', function () {
        var section = $('#statExpandModal').data("cat")
        var modal = $(this)
        modal.find('.modal-title').text(section)
        modal.find('#stat' + section.replace(/\W/g, '') + 'Modal').removeClass('hidden');
    })

    // when the modal is hidden, re-hide the previously shown section
    $('#statExpandModal').on('hide.bs.modal', function (event) {
        section = $(this).find(".modal-title").text();
        $(this).find('#stat' + section.replace(/\W/g, '') + 'Modal').addClass('hidden');
    });

    addModal("statExpandModal");
    function showStatModal(e) {
        $('#statExpandModal').data("cat", e.dataPoint.label);
        $('#statExpandModal').modal('show');
    }
    

    // build graphs for Profile Vs Town View
    if (typeof ($(".proVsTownChartContainer").val()) != "undefined") { // only execute if the stats page is currently loaded
        // take data from the passed JSON and fill two arrays based on the data
        var servRate = [];
        var servRateTtemp = [];
        var servRateT = [];


        $.each(userRatings, function (key, value) {
            servRate.push({
                y: +value,
                label: key,
                click: showStatModal
            });
        })

        var t2 = "t2";
        $.each(townRatings, function (key, value) {
            servRateTtemp.push({
                y: +value,
                label: key,
                click: showStatModal
            });
        })

        // these two arrays need to be the same order
        for (var i = 0; i < servRate.length; i++)
        {
            var tserv = findObject(servRateTtemp, "label", servRate[i].label);
            if (tserv != null) 
                servRateT.push(tserv);
            else servRateT.push({
                    y: 0,
                    label: servRate[i].label,
                    click: showStatModal
                });
            
        }
        
        // too many categorys for one graph, split each array it to two
        var servRate2 = servRate.slice(0, Math.floor(servRate.length / 2));
        servRate = servRate.slice(Math.floor(servRate.length / 2) + 1);

        var servRateT2 = servRateT.slice(0, Math.floor(servRateT.length / 2));
        servRateT = servRateT.slice(Math.floor(servRateT.length / 2) + 1);

        // set up both graphs
        $(".proVsTownChartContainer").CanvasJSChart({
            animationEnabled: true,
            toolTip: {
                shared: true,
                content: function (e) {
                    var data = e.entries;
                    var m = "";
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].dataSeries.visible) {
                            if (m == "") m = data[i].dataPoint.label;
                            m += "<br/>" + data[i].dataSeries.legendText + ": <strong>" + data[i].dataPoint.y + "</strong>";
                        }
                    }
                    return m;
                }
            },
            legend: {
                verticalAlign: "bottom",
                horizontalAlign: "center",
                fontSize: 12
            },
            axisX: {
                title: "Category",
                interval: 1,
                labelAutoFit: true,
                labelFontSize: 12
            },
            axisY: {
                title: "Rating",
                maximum: 10,
                interval: 1,
                labelFontSize: 12
            },
            data: [
                {
                    type: "column",
                    color: "darkred",
                    name: "Your Desired Rating",
                    legendText: "Your Desired Rating",
                    showInLegend: true,
                    visible: true,
                    dataPoints: servRate
                },
                {
                    type: "column",
                    color: "darkgreen",
                    name: "Town Average Rating",
                    legendText: "Town Average Rating",
                    showInLegend: true,
                    visible: true,
                    dataPoints: servRateT
                },
                {
                    type: "column",
                    color: "darkred",
                    name: "Your Desired Rating",
                    legendText: "Your Desired Rating",
                    showInLegend: false,
                    visible: false,
                    dataPoints: servRate2
                },
                {
                    type: "column",
                    color: "darkgreen",
                    name: "Town Average Rating",
                    legendText: "Town Average Rating",
                    showInLegend: false,
                    visible: false,
                    dataPoints: servRateT2
                }
            ]
        });
    }

    if (typeof ($(".physServeChartContainer").val()) != "undefined") { // only execute if the stats page is currently loaded
        // take data from the passed JSON and fill two arrays based on the data
        var servRate = [];

        $.each(townRatings, function (key, value) {
            servRate.push({
                y: +value,
                label: key,
                click: showStatModal
            });
        })

        // too many categorys for one graph, split each array it to two
        var servRate2 = servRate.slice(0, Math.floor(servRate.length / 2));
        servRate = servRate.slice(Math.floor(servRate.length / 2) + 1);

        // set up both graphs
        $(".physServeChartContainer").CanvasJSChart({
            animationEnabled: true,
            toolTip: {
                content: function (e) {
                    var data = e.entries[0];
                    return data.dataPoint.label + "<br/>" + data.dataPoint.y;
                }
            },
            axisX: {
                title: "Category",
                interval: 1,
                labelAutoFit: true,
                labelFontSize: 12
            },
            axisY: {
                title: "Rating",
                maximum: 10,
                interval: 1,
                labelFontSize: 12
            },
            data: [
                {
                    type: "column",
                    color: "darkred",
                    visible: true,
                    dataPoints: servRate
                },

               {
                   type: "column",
                   color: "darkred",
                   visible: false,
                   dataPoints: servRate2
               }

            ]
        });
    }

    if (typeof ($(".virtServeChartContainer").val()) != "undefined") { // only execute if the stats page is currently loaded
        // take data from the inputs on the page and fill two arrays based on the data
        var servRate = [];

        $.each(townVirtRatings, function (key, value) {
            servRate.push({
                y: +value,
                label: key,
                click: showStatModal
            });
        })

        // too many categorys for one graph, split each array it to two
        var servRate2 = servRate.slice(0, Math.floor(servRate.length / 2));
        servRate = servRate.slice(Math.floor(servRate.length / 2) + 1);

        // set up both graphs
        $(".virtServeChartContainer").CanvasJSChart({
            animationEnabled: true,
            toolTip: {
                content: function (e) {
                    var data = e.entries;
                    for (var i = 0; i < 2; i++)
                        if (data[i].dataSeries.visible)
                            return data[i].dataPoint.label + "<br/>" + data[i].dataPoint.y;
                }
            },
            axisX: {
                title: "Category",
                interval: 1,
                labelAutoFit: true,
                labelFontSize: 12
            },
            axisY: {
                title: "Rating",
                maximum: 10,
                interval: 1,
                labelFontSize: 12
            },
            data: [
                {
                    type: "column",
                    color: "darkred",
                    visible: true,
                    dataPoints: servRate
                },
                {
                    type: "column",
                    color: "darkred",
                    visible: false,
                    dataPoints: servRate2
                }
            ]
        });
    }

    // called when ever the tab is swiched
    $(".section-content").on("section:visible", function () {
        $(".statGraphContainer").each(function () {
            $(this).CanvasJSChart().render();
        });
    })

    if (+statsCurrentDataPoints == 1) {
        statsCurrentDataPoints = 0;
        $('.statGraphSwap').first().trigger('click');
    }

    // hide and show the data groups when the cycle buttons are clicked
    $('.statGraphSwap').click(function () {
        statsCurrentDataPoints = (statsCurrentDataPoints + 1) % 2;
        $('.statGraphContainer').each(function () {
            var data = $(this).CanvasJSChart().options.data;
            for (var i = 0; i < data.length; i++) {
                if (data[i].visible != true) {
                    data[i].visible = true;
                    if (typeof (data[i].showInLegend) != "undefined") data[i].showInLegend = true; // only change this if needed
                } else {
                    data[i].visible = false;
                    if (typeof (data[i].showInLegend) != "undefined") data[i].showInLegend = false;
                }

            }
            $(this).CanvasJSChart().render();

        });
    });
}

function findObject(array, property, prop_value)
{
    for (var i = 0; i < array.length; i++)
    {
        if (array[i][property] == prop_value)
            return array[i];
    }
    return null;
}