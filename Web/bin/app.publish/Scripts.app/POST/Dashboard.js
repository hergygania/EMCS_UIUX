$(document).ready(function () {


    //FilterTop5VendorDelay();
    //FilterTop5PODelay();
    FilterVendorActice();
    //InitChartTopVendorDelay();
    //InitChartTopPODelay();

    //InitChartCount(); //(query lambat)

    PageLoadActiveVendor();
    InitChartActiveVendorNewVerse();
    PageLoadActiveVendor();
    InitChartActiveVendorNewVerse();

    FilterEmployeeInternal();
    PageLoadActiveEmp();
    InitChartActiveEmployee();
    //InitChartTopVendorHitrate();
    //InitChartTopVendorHitrateTbl();

    //InitChartTopEmployeeHitrate();
    //InitChartTopEmployeeHitrateTbl();

    //InitChartSLARole();
    //InitChartTopBranch();
    //var GroupID = $('#GroupID').val();
    //if (GroupID == 19) {
    //    document.getElementById("chart-dashboard").style.display = "none";
    //}
});

var selectyear5VendorDelay = $("#year5VendorDelay");
var selectmonth5VendorDelay = $("#month5VendorDelay");
var selectdayTop5VendorDelay = $("#dayTop5VendorDelay");
var selectyear5PODelay = $("#year5PODelay");
var selectmonth5PODelay = $("#month5PODelay");
var selectdayTop5PODelay = $("#dayTop5PODelay");
var selectyearVendorActice = $("#yearVendorActice");
var selectmonthVendorActice = $("#monthVendorActice");
var selectdayVendorActice = $("#dayVendorActice");

var selectyearVendorActiceFrom = $("#yearVendorActiceFrom");
var selectyearVendorActiceTo = $("#yearVendorActiceTo");
var selectmonthVendorActiceFrom = $("#monthVendorActiceFrom");
var selectmonthVendorActiceTo = $("#monthVendorActiceTo");
var selectdayVendorActiceFrom = $("#dayVendorActiceFrom");
var selectdayVendorActiceTo = $("#dayVendorActiceTo");

var selectddlFilterType = $("#ddlFilterType");

var selectyearEmpActiveFromIntrnl = $("#yearEmpActiveFromIntrnl");
var selectmonthEmpActiveFromIntrnl = $("#monthEmpActiveFromIntrnl");
var selectdayVendorActiceFromIntrnl = $("#dayVendorActiceFromIntrnl");
var selectyearEmpActiveToIntrnl = $("#yearEmpActiveToIntrnl");
var selectmonthEmpActiveToIntrnl = $("#monthEmpActiveToIntrnl");
var selectdayVendorActiceToIntrnl = $("#dayVendorActiceToIntrnl");
var selectyearEmpActiveIntrnl = $("#yearEmpActiveIntrnl");
var selectmonthEmpActiveIntrnl = $("#monthEmpActiveIntrnl");
var selectdayVendorActiceIntrnl = $("#dayVendorActiceIntrnl");

var selectddlFilterTypeIntrnl = $("#ddlFilterTypeIntrnl");



function FilterTop5VendorDelay() {
    const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    var qntYears = 4;
    var selectYear = $("#year5VendorDelay");
    var selectMonth = $("#month5VendorDelay");
    var selectDay = $("#dayTop5VendorDelay");
    var currentYear = new Date().getFullYear();

    for (var y = 0; y < qntYears; y++) {
        var yearElem = document.createElement("option");
        yearElem.value = currentYear
        yearElem.textContent = currentYear;
        selectYear.append(yearElem);
        currentYear--;
    }

    for (var m = 0; m < 12; m++) {
        let monthNum = new Date(2018, m).getMonth()
        let months = monthNames[monthNum];
        var monthElem = document.createElement("option");
        monthElem.value = monthNum;
        monthElem.textContent = months;
        selectMonth.append(monthElem);
    }

    var d = new Date();
    var month = d.getMonth();
    var year = d.getFullYear();
    var day = d.getDate();

    selectYear.val(year);
    selectYear.on("change", AdjustDays);
    selectMonth.val(month);
    selectMonth.on("change", AdjustDays);

    AdjustDays();
    selectDay.val(day)

    function AdjustDays() {
        var years = selectYear.val();
        var month = parseInt(selectMonth.val()) + 1;
        selectDay.empty();

        //get the last day, so the number of days in that month
        var days = new Date(years, month, 0).getDate();

        //lets create the days of that month
        for (var ddd = 1; ddd <= days; ddd++) {
            var dayElem = document.createElement("option");
            dayElem.value = ddd;
            dayElem.textContent = ddd;
            selectDay.append(dayElem);
        }
    }
}


function FilterTop5PODelay() {
    const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    var qntYears = 4;
    var selectYear = $("#year5PODelay");
    var selectMonth = $("#month5PODelay");
    var selectDay = $("#dayTop5PODelay");
    var currentYear = new Date().getFullYear();

    for (var y = 0; y < qntYears; y++) {
        var yearElem = document.createElement("option");
        yearElem.value = currentYear
        yearElem.textContent = currentYear;
        selectYear.append(yearElem);
        currentYear--;
    }

    for (var m = 0; m < 12; m++) {
        let monthNum = new Date(2018, m).getMonth()
        let months = monthNames[monthNum];
        var monthElem = document.createElement("option");
        monthElem.value = monthNum;
        monthElem.textContent = months;
        selectMonth.append(monthElem);
    }

    var d = new Date();
    var month = d.getMonth();
    var year = d.getFullYear();
    var day = d.getDate();

    selectYear.val(year);
    selectYear.on("change", AdjustDays);
    selectMonth.val(month);
    selectMonth.on("change", AdjustDays);

    AdjustDays();
    selectDay.val(day)

    function AdjustDays() {
        var years = selectYear.val();
        var month = parseInt(selectMonth.val()) + 1;
        selectDay.empty();

        //get the last day, so the number of days in that month
        var days = new Date(years, month, 0).getDate();

        //lets create the days of that month
        for (var ddd = 1; ddd <= days; ddd++) {
            var dayElem = document.createElement("option");
            dayElem.value = ddd;
            dayElem.textContent = ddd;
            selectDay.append(dayElem);
        }
    }
}
function FilterVendorActice() {
    const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    var qntYears = 4;
    var selectYear = $("#yearVendorActice");
    var selectMonth = $("#monthVendorActice");
    var selectDay = $("#dayVendorActice");

    var selectyearVendorActiceFrom = $("#yearVendorActiceFrom");
    var selectyearVendorActiceTo = $("#yearVendorActiceTo");
    var selectmonthVendorActiceFrom = $("#monthVendorActiceFrom");
    var selectmonthVendorActiceTo = $("#monthVendorActiceTo");
    var selectdayVendorActiceFrom = $("#dayVendorActiceFrom");
    var selectdayVendorActiceTo = $("#dayVendorActiceTo");

    var currentYear = new Date().getFullYear();

    for (var y = 0; y < qntYears; y++) {
        var yearElem = document.createElement("option");
        var yearElemFrom = document.createElement("option");
        var yearElemTo = document.createElement("option");

        yearElem.value = currentYear
        yearElem.textContent = currentYear;
        yearElemFrom.value = currentYear
        yearElemFrom.textContent = currentYear;
        yearElemTo.value = currentYear
        yearElemTo.textContent = currentYear;

        selectYear.append(yearElem);
        selectyearVendorActiceTo.append(yearElemFrom);
        selectyearVendorActiceFrom.append(yearElemTo);

        currentYear--;
    }

    for (var m = 0; m < 12; m++) {
        let monthNum = new Date(2018, m).getMonth()
        let months = monthNames[monthNum];
        var monthElem = document.createElement("option");
        var monthElemFrom = document.createElement("option");
        var monthElemTo = document.createElement("option");

        monthElem.value = monthNum;
        monthElem.textContent = months;
        monthElemFrom.value = monthNum;
        monthElemFrom.textContent = months;
        monthElemTo.value = monthNum;
        monthElemTo.textContent = months;

        selectMonth.append(monthElem);
        selectmonthVendorActiceFrom.append(monthElemFrom);
        selectmonthVendorActiceTo.append(monthElemTo);
    }

    var d = new Date();
    var month = d.getMonth();
    var year = d.getFullYear();
    var day = d.getDate();

    selectYear.val(year);
    selectYear.on("change", AdjustDays);
    selectyearVendorActiceFrom.val(year);
    //selectyearVendorActiceFrom.on("change", AdjustDays);
    selectyearVendorActiceTo.val(year);
    //selectyearVendorActiceTo.on("change", AdjustDays);

    selectMonth.val(month);
    selectMonth.on("change", AdjustDays);
    selectmonthVendorActiceFrom.val(month);
    //selectmonthVendorActiceFrom.on("change", AdjustDays);
    selectmonthVendorActiceTo.val(month);
    //selectmonthVendorActiceTo.on("change", AdjustDays);

    AdjustDays();
    selectDay.val(day)
    //selectdayVendorActiceFrom.val(day)
    //selectdayVendorActiceTo.val(day)

    function AdjustDays() {
        var years = selectYear.val();
        var month = parseInt(selectMonth.val()) + 1;
        selectDay.empty();

        //get the last day, so the number of days in that month
        var days = new Date(years, month, 0).getDate();

        //lets create the days of that month
        for (var ddd = 1; ddd <= days; ddd++) {
            var dayElem = document.createElement("option");
            dayElem.value = ddd;
            dayElem.textContent = ddd;
            selectDay.append(dayElem);
        }
    }
}

function InitChartCount() {
    var year = selectyear5VendorDelay.val() ?? 0;
    var month = selectmonth5VendorDelay.val() ?? 0;
    $.getJSON("/POST/GetDataCount?Year=" + year + "&Month=" + month + "").done(function (data) {
        $('#CountPODNotBAST').text(data.result.CountPODNotBAST);
        $('#CountBASTNotGR').text(data.result.CountBASTNotGR);
        $('#CountGRNotInvoice').text(data.result.CountGRNotInvoice);
        $('#CountInvoiceNotPayment').text(data.result.CountInvoiceNotPayment);
    });
}

function InitChartTopVendorDelay() {
    var year = selectyear5VendorDelay.val() ?? 0;
    var month = selectmonth5VendorDelay.val() ?? 0;
    $.getJSON("/POST/GetDashboardTopDelayVendor?Year=" + year + "&Month=" + month + "").done(function (data) {

        var label = data.result.map(function (e) {
            return e.VendorName;
        });

        var Total = data.result.map(function (e) {
            return e.CountDelay;
        });

        var Value = data.result.map(function (e) {
            return 0;
        });


        Highcharts.chart('container1', {
            chart: {
                zoomType: 'xy'
            },
            title: {
                text: 'Top 5 Vendor Delay'
            },
            xAxis: {
                categories: label
            },
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: 'Qty',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                }
            }, { // Secondary yAxis
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: 'Qty',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                opposite: true
            }],
            plotOptions: {
                column: {
                    cursor: 'pointer',
                    dataLabels: {
                        inside: true,
                        enabled: true,
                        formatter: function () {
                            var qty = this.y;
                            var res = qty;
                            return res;
                        }
                    }
                },

            },
            tooltip: {
                shared: true
            },
            labels: {
                items: [{
                    style: {
                        left: '50px',
                        top: '18px',
                        color: ( // theme
                            Highcharts.defaultOptions.title.style &&
                            Highcharts.defaultOptions.title.style.color
                        ) || 'black'
                    }
                }]
            },
            series: [{
                type: 'column',
                yAxis: 1,
                name: 'Area',
                data: Total
            }, {
                type: 'spline',
                name: 'Total',
                data: Value,
                marker: {
                    lineWidth: 2,
                    lineColor: Highcharts.getOptions().colors[3],
                    fillColor: 'white'
                }
            }]
        });
    });
}

function InitChartTopPODelay() {
    var year = selectyear5PODelay.val() ?? 0;
    var month = selectmonth5PODelay.val() ?? 0;

    var title = "PO Delay";

    $.getJSON("/POST/GetDashboardTopDelayPO?Year=" + year + "&Month=" + month + "").done(function (data) {
        Highcharts.chart('container3',
            {
                credits: {
                    enabled: false
                },
                title: {
                    text: title,
                    style: {
                        fontFamily: "serif",
                        color: "#333333",
                        fontSize: "18px",
                        fontWeight: "normal",
                        fontStyle: "normal"
                    }
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    "series": {
                        "allowPointSelect": true,
                        "states": {
                            "select": {
                                "color": "#EFFFEF",
                                "borderColor": "black",
                                "dashStyle": "dot"
                            }
                        },
                        animation: {
                            duration: 2000
                        }
                    },
                    pie: {
                        credits: {
                            enabled: false
                        },
                        allowPointSelect: true,
                        cursor: 'pointer',
                        //"showInLegend": true,
                        //    colors: pieColors,
                        dataLabels: {
                            //enabled: true,
                            format: '<b>{point.name} : {point.y} ',
                        },
                        distance: -50,
                        style: {
                            fontfamily: "muli"
                        },
                        filter: {
                            property: 'percentage',
                            operator: '>',
                            value: 4
                        }
                    },
                    "line": {
                        //"dataLabels": {
                        //    "enabled": true
                        //},
                        "enableMouseTracking": false
                    }
                },
                "exporting": {},
                "credits": {
                    "text": "cloud.highcharts.com",
                    "href": "https://cloud.highcharts.com"
                },
                "chart": {
                    style: {
                        fontFamily: "Muli"
                    },
                    "options3d": {
                        "enabled": true,
                        "alpha": 45,
                        "beta": 10
                    },
                    "polar": true,
                    "type": "line",
                    "borderColor": "#ec407a",
                    "borderRadius": 1
                },
                "yAxis": {
                    "title": {
                        "text": "Qty"
                    }
                },
                series: [
                    {
                        "turboThreshold": 0,
                        "_colorIndex": 0,
                        "_symbolIndex": 0,
                        "type": "pie",
                        "marker": {
                            "symbol": "circle",
                            "enabled": true
                        },
                        "colorByPoint": true,
                        name: 'Share',
                        data: data.result
                    }
                ],
                "colors": [
                    "#7cb5ec",
                    "#673ab7",
                    "#8bc34a",
                    "#ff80ab",
                    "#ffea00"
                ],
                "legend": {
                    "enabled": false,
                    //align: 'left',
                    //"layout": "horizontal",
                    "itemStyle": {

                        //    layout: 'vertical',
                        //    verticalAlign: 'top',
                        //    x: 40,
                        //    y: 0
                        //}
                        fontFamily: "Muli",
                        "color": "#333333",
                        "fontSize": "10px",
                        "fontWeight": "bold",
                        "fontStyle": "normal",
                        "cursor": "pointer"
                    }
                },
                "stockTools": {
                    "gui": {
                        "buttons": [
                            "simpleShapes",
                            "lines",
                            "crookedLines"
                        ],
                        "enabled": false
                    }
                },
                "navigation": {
                    "events": {
                        "showPopup":
                            "function(e){this.chart.indicatorsPopupContainer||(this.chart.indicatorsPopupContainer=document.getElementsByClassName(\"highcharts-popup-indicators\")[0]),this.chart.annotationsPopupContainer||(this.chart.annotationsPopupContainer=document.getElementsByClassName(\"highcharts-popup-annotations\")[0]),\"indicators\"===e.formType?this.chart.indicatorsPopupContainer.style.display=\"block\":\"annotation-toolbar\"===e.formType&&(this.chart.activeButton||(this.chart.currentAnnotation=e.annotation,this.chart.annotationsPopupContainer.style.display=\"block\")),this.popup&&(c=this.popup)}",
                        "closePopup":
                            "function(){this.chart.annotationsPopupContainer.style.display=\"none\",this.chart.currentAnnotation=null}",
                        "selectButton":
                            "function(e){var t=e.button.className+\" highcharts-active\";e.button.classList.contains(\"highcharts-active\")||(e.button.className=t,this.chart.activeButton=e.button)}",
                        "deselectButton":
                            "function(e){e.button.classList.remove(\"highcharts-active\"),this.chart.activeButton=null}"
                    },
                    "bindingsClassName": "tools-container"
                },
                "pane": {
                    "background": []
                },
                "responsive": {
                    "rules": []
                },
                "lang": {
                    "decimalPoint": "2",
                    "thousandsSep": ",",
                    "downloadJPEG": ""
                },
                "caption": {
                    "text": ""
                },
                "annotations": []
            });
    });
}

function InitChartActiveVendor() {
    var year = selectyearVendorActice.val() ?? 0;
    var month = parseInt(selectmonthVendorActice.val()) + 1 ?? 0;

    var title = "Vendor";

    $.getJSON("/POST/GetDashboardActiveVendor?Year=" + year + "&Month=" + month + "").done(function (data) {
        Highcharts.chart('container2',
            {
                credits: {
                    enabled: false
                },
                title: {
                    text: title,
                    style: {
                        fontFamily: "Muli",
                        color: "#333333",
                        fontSize: "18px",
                        fontWeight: "normal",
                        fontStyle: "normal"
                    }
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    "series": {
                        "allowPointSelect": true,
                        "states": {
                            "select": {
                                "color": "#EFFFEF",
                                "borderColor": "black",
                                "dashStyle": "dot"
                            }
                        },
                        animation: {
                            duration: 2000
                        }
                    },
                    pie: {
                        credits: {
                            enabled: false
                        },
                        allowPointSelect: true,
                        cursor: 'pointer',
                        //"showInLegend": true,
                        //    colors: pieColors,
                        dataLabels: {
                            //enabled: true,
                            format: '<b>{point.name} : {point.percentage:.1f} %'
                            //distance: -50,
                            //style: {
                            //    fontfamily: "muli"
                            //},
                            //formatter: function () {
                            //    console.log(this);
                            //    //            var qty = this.y;
                            //    //            var res = qty + " %";
                            //    //            return res;
                            //},
                            ////    }
                            //filter: {
                            //    property: 'percentage',
                            //    operator: '>',
                            //    value: 4
                            //}
                        }
                    },
                    "line": {
                        //"dataLabels": {
                        //    "enabled": true
                        //},
                        "enableMouseTracking": false
                    }
                },
                "exporting": {},
                "credits": {
                    "text": "cloud.highcharts.com",
                    "href": "https://cloud.highcharts.com"
                },
                "chart": {
                    style: {
                        fontFamily: "Muli"
                    },
                    "options3d": {
                        "enabled": true,
                        "alpha": 45,
                        "beta": 10
                    },
                    "polar": true,
                    "type": "line",
                    "borderColor": "#ec407a",
                    "borderRadius": 1
                },
                "yAxis": {
                    "title": {
                        "text": "Qty"
                    }
                },
                series: [
                    {
                        "turboThreshold": 0,
                        "_colorIndex": 0,
                        "_symbolIndex": 0,
                        "type": "pie",
                        "marker": {
                            "symbol": "circle",
                            "enabled": true
                        },
                        "colorByPoint": true,
                        name: 'Share',
                        data: data.result
                    }
                ],
                "colors": [
                    "#7cb5ec",
                    "#673ab7",
                    "#8bc34a",
                    "#ff80ab",
                    "#ffea00"
                ],
                "legend": {
                    "enabled": false,
                    //align: 'left',
                    //"layout": "horizontal",
                    "itemStyle": {

                        //    layout: 'vertical',
                        //    verticalAlign: 'top',
                        //    x: 40,
                        //    y: 0
                        //}
                        fontFamily: "Muli",
                        "color": "#333333",
                        "fontSize": "10px",
                        "fontWeight": "bold",
                        "fontStyle": "normal",
                        "cursor": "pointer"
                    }
                },
                "stockTools": {
                    "gui": {
                        "buttons": [
                            "simpleShapes",
                            "lines",
                            "crookedLines"
                        ],
                        "enabled": false
                    }
                },
                "navigation": {
                    "events": {
                        "showPopup":
                            "function(e){this.chart.indicatorsPopupContainer||(this.chart.indicatorsPopupContainer=document.getElementsByClassName(\"highcharts-popup-indicators\")[0]),this.chart.annotationsPopupContainer||(this.chart.annotationsPopupContainer=document.getElementsByClassName(\"highcharts-popup-annotations\")[0]),\"indicators\"===e.formType?this.chart.indicatorsPopupContainer.style.display=\"block\":\"annotation-toolbar\"===e.formType&&(this.chart.activeButton||(this.chart.currentAnnotation=e.annotation,this.chart.annotationsPopupContainer.style.display=\"block\")),this.popup&&(c=this.popup)}",
                        "closePopup":
                            "function(){this.chart.annotationsPopupContainer.style.display=\"none\",this.chart.currentAnnotation=null}",
                        "selectButton":
                            "function(e){var t=e.button.className+\" highcharts-active\";e.button.classList.contains(\"highcharts-active\")||(e.button.className=t,this.chart.activeButton=e.button)}",
                        "deselectButton":
                            "function(e){e.button.classList.remove(\"highcharts-active\"),this.chart.activeButton=null}"
                    },
                    "bindingsClassName": "tools-container"
                },
                "pane": {
                    "background": []
                },
                "responsive": {
                    "rules": []
                },
                "lang": {
                    "decimalPoint": "2",
                    "thousandsSep": ",",
                    "downloadJPEG": ""
                },
                "caption": {
                    "text": ""
                },
                "annotations": []
            });
    });
}

function InitChartTopBranch(area) {
    var areaname = (area == undefined) ? "" : area;
    var year = $("#Year").dxSelectBox('instance').option('value');
    var month = $("#Month").dxSelectBox('instance').option('value');
    var currency = $("#Currency").dxSelectBox('instance').option('value');

    var title = 'Top 10 Branch Refund';
    if (areaname != '') {
        title = 'Summary Refund per Branch on ' + areaname;
    }
    $.getJSON(window.baseUrl + "/Home/ChartTopBranch?year=" + year + "&Currency=" + currency + "&Area=" + areaname + "&Month=" + month + "").done(function (data) {

        Highcharts.chart('container2', {
            credits: {
                enabled: false
            },
            title: {
                fontFamily: 'Muli',
                text: 'Summary Refund'
            },
            chart: {
                type: 'column',
                style: {
                    fontFamily: "Muli"
                },
                events: {
                    drilldown: function (e) {
                        if (!e.seriesOptions) {
                            this.setTitle(null, { text: 'Refund Detail ' + e.point.name });
                            var chart = this,
                                drilldowns = {
                                    Bandung: {
                                        name: 'Bandung',
                                        data: [
                                            { amount: 2140, name: 'Jan', y: 4 },
                                            { amount: 2310, name: 'Feb', y: 5 },
                                            { amount: 1000, name: 'Mar', y: 6 },
                                            { amount: 1000, name: 'Apr', y: 8 },
                                            { amount: 2500, name: 'May', y: 9 },
                                            { amount: 1000, name: 'Jun', y: 10 },
                                            { amount: 4300, name: 'Jul', y: 4 },
                                            { amount: 1000, name: 'Aug', y: 5 },
                                            { amount: 2300, name: 'Sep', y: 6 },
                                            { amount: 2100, name: 'Oct', y: 7 },
                                            { amount: 1500, name: 'Nov', y: 4 },
                                            { amount: 1000, name: 'Dec', y: 2 }
                                        ]
                                    },
                                    2017: {
                                        name: '2017',
                                        data: [
                                            { amount: 1000, name: 'Jan', y: 4 },
                                            { amount: 1000, name: 'Feb', y: 5 },
                                            { amount: 1000, name: 'Mar', y: 6 },
                                            { amount: 1000, name: 'Apr', y: 8 },
                                            { amount: 1000, name: 'May', y: 9 },
                                            { amount: 1000, name: 'Jun', y: 10 },
                                            { amount: 1000, name: 'Jul', y: 4 },
                                            { amount: 1000, name: 'Aug', y: 5 },
                                            { amount: 1000, name: 'Sep', y: 6 },
                                            { amount: 1000, name: 'Oct', y: 7 },
                                            { amount: 1000, name: 'Nov', y: 4 },
                                            { amount: 1000, name: 'Dec', y: 2 }
                                        ]
                                    },
                                    2019: {
                                        name: '2018',
                                        data: [
                                            { amount: 1000, name: 'Jan', y: 4 },
                                            { amount: 1000, name: 'Feb', y: 5 },
                                            { amount: 1000, name: 'Mar', y: 6 },
                                            { amount: 1000, name: 'Apr', y: 8 },
                                            { amount: 1000, name: 'May', y: 9 },
                                            { amount: 1000, name: 'Jun', y: 10 },
                                            { amount: 1000, name: 'Jul', y: 4 },
                                            { amount: 1000, name: 'Aug', y: 5 },
                                            { amount: 1000, name: 'Sep', y: 6 },
                                            { amount: 1000, name: 'Oct', y: 7 },
                                            { amount: 1000, name: 'Nov', y: 4 },
                                            { amount: 1000, name: 'Dec', y: 2 }
                                        ]
                                    },
                                    2020: {
                                        name: '2019',
                                        data: [
                                            { amount: 1000, name: 'Jan', y: 4 },
                                            { amount: 1000, name: 'Feb', y: 5 },
                                            { amount: 1000, name: 'Mar', y: 6 },
                                            { amount: 1000, name: 'Apr', y: 8 },
                                            { amount: 1000, name: 'May', y: 9 },
                                            { amount: 1000, name: 'Jun', y: 10 },
                                            { amount: 1000, name: 'Jul', y: 4 },
                                            { amount: 1000, name: 'Aug', y: 5 },
                                            { amount: 1000, name: 'Sep', y: 6 },
                                            { amount: 1000, name: 'Oct', y: 7 },
                                            { amount: 1000, name: 'Nov', y: 4 },
                                            { amount: 1000, name: 'Dec', y: 2 }
                                        ]
                                    }
                                },
                                series = drilldowns[e.point.name];

                            // Show the loading label
                            chart.showLoading('Reloading Detail Data ...');

                            setTimeout(function () {
                                chart.hideLoading();
                                chart.addSeriesAsDrilldown(e.point, series);
                            }, 1000);
                        }

                    }
                }
            },
            title: {
                text: title,
                fontFamily: "Muli"
            },
            yAxis: {
                title: {
                    text: 'Qty'
                }
            },
            xAxis: {
                type: 'category'
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                column: {
                    cursor: 'pointer',
                    events: {
                        click: function (event) {
                            InitChartSLARole(event.point.name);
                            InitChartTopRefundType(event.point.name, '');
                        }
                    },
                    dataLabels: {
                        enabled: true,
                        formatter: function () {
                            var amount = this.point.amount;
                            var qty = this.y;
                            var stringAmount = numeral(parseInt(amount)).format('0,0');
                            var res = qty + '<br> (' + currency + ' ' + stringAmount + ' k)';
                            return res;
                        }
                    }
                }
            },
            series: [{
                name: 'Refund',
                data: data
            }],
            responsive: {
                rules: [{
                    condition: {
                        maxWidth: 500
                    },
                    chartOptions: {
                        legend: true
                    }
                }]
            }
        })
    })
};

function InitChartSLARole(branch) {
    var branchname = (branch == undefined) ? "" : branch;

    var title = 'Service Level Agremeent';
    if (branchname != '') {
        title = 'Service Level Agremeent on Branch ' + branchname;
    }
    $.getJSON(window.baseUrl + "/Home/ChartSLARole?Branch=" + branchname).done(function (data) {
        Highcharts.chart('container4', {
            credits: {
                enabled: false
            },
            chart: {
                type: 'area',
                style: {
                    fontFamily: "Muli"
                },
                options3d: {
                    enabled: true,
                    alpha: 45,
                    beta: 0
                }
            },
            title: {
                text: title
            },
            xAxis: {
                type: 'category'
            },
            legend: false,
            yAxis: {
                title: {
                    text: 'Days'
                },
                labels: {
                    formatter: function () {
                        return this.value;
                    }
                }
            },
            tooltip: {
                fontFamily: "Muli",
                pointFormat: '{series.name} had stockpiled <b>{point.y:,.0f}</b><br/>warheads in {point.x}'
            },
            plotOptions: {
                area: {
                    dataLabels: {
                        fontFamily: "Muli",
                        enabled: true,
                        formatter: function () {
                            var qty = this.y;
                            var res = qty + ' days';
                            return res;
                        }
                    },
                    marker: {
                        enabled: true,
                        symbol: 'circle',
                        radius: 2,
                        states: {
                            hover: {
                                enabled: true
                            }
                        }
                    }
                }
            },
            series: [{
                name: 'SLA',
                data: data
            }]
        });

    });
}

function InitDouchnutChartRefundArea() {
    $.getJSON(baseUrl + "/Home/ChartRequestRefundOneYearPerArea").done(function (data) {
        var Filter = new Array();
        var Total = 0;
        $.each(data, function (key, value) {
            if (value.Total > 0) {
                Filter.push(value);
                Total = Total + value.Total;
            }
        });

        var labels_doughnut = Filter.map(function (e) {
            return e.Area;
        });
        var data_doughnut = Filter.map(function (e) {
            return e.Total;
        });

        var doughnutSalesChartCTX = $("#doughnutchartAreaYear");
        var browserStatsChartOptions = {
            cutoutPercentage: 70,
            responsive: true,
            legend: {
                display: true,
                position: 'bottom',
                fullWidth: true,
                labels: {
                    boxWidth: 10,
                }
            }, plugins: {
                datalabels: {
                    display: false,
                    anchor: 'end',
                    align: 'end',
                    formatter: Math.round,
                    color: '#fff',
                    labels: {
                        title: {
                            font: {
                                weight: 'bold'
                            }
                        },
                        value: {
                            color: '#fff',
                        }
                    },
                }
            }
        };

        var doughnutSalesChartData = {
            labels: labels_doughnut,//["PP Down Payment", "PP wrong Trasnfer", "PP Over Payment"],
            datasets: [
                {
                    label: "Count",
                    data: data_doughnut,
                    backgroundColor: ["#F7464A", "#0077FF", "#FEA735", "#D0E6A5", "#7E9680", "#79616F", "#EAD7BA", "#83a73d", "#3da5a7", "#473da7", "#a53da7", "#a73d69", "#a78c3d", "#a74c3d"]
                }
            ]
        };

        var doughnutSalesChartConfig = {
            type: "doughnut",
            options: browserStatsChartOptions,
            data: doughnutSalesChartData
            //options: {
            //    elements: {
            //        center: {
            //            text: Total + ' Request',
            //            color: '#000', //Default black
            //            sidePadding: 12 //Default 20 (as a percentage)
            //        }
            //    }
            //}
        };

        new Chart(doughnutSalesChartCTX, doughnutSalesChartConfig);
        $('.doughnutcharttotalAreaYear').text(Total);

    });
}
//Chart.pluginService.register({
//    beforeDraw: function (chart) {
//        if (chart.config.options.elements.center) {
//            //Get ctx from string
//            var ctx = chart.chart.ctx;

//            //Get options from the center object in options
//            var centerConfig = chart.config.options.elements.center;
//            var fontStyle = centerConfig.fontStyle || 'Arial';
//            var txt = centerConfig.text;
//            var color = centerConfig.color || '#000';
//            var sidePadding = centerConfig.sidePadding || 20;
//            var sidePaddingCalculated = (sidePadding / 100) * (chart.innerRadius * 2)
//            //Start with a base font of 30px
//            ctx.font = "30px " + fontStyle;

//            //Get the width of the string and also the width of the element minus 10 to give it 5px side padding
//            var stringWidth = ctx.measureText(txt).width;
//            var elementWidth = (chart.innerRadius * 2) - sidePaddingCalculated;

//            // Find out how much the font can grow in width.
//            var widthRatio = elementWidth / stringWidth;
//            var newFontSize = Math.floor(30 * widthRatio);
//            var elementHeight = (chart.innerRadius * 2);

//            // Pick a new font size so it will not be larger than the height of label.
//            var fontSizeToUse = Math.min(newFontSize, elementHeight);

//            //Set font settings to draw it correctly.
//            ctx.textAlign = 'center';
//            ctx.textBaseline = 'middle';
//            var centerX = ((chart.chartArea.left + chart.chartArea.right) / 2);
//            var centerY = ((chart.chartArea.top + chart.chartArea.bottom) / 2);
//            ctx.font = fontSizeToUse + "px " + fontStyle;
//            ctx.fillStyle = color;

//            //Draw text in center
//            ctx.fillText(txt, centerX, centerY);
//        }
//    }
//});
function InitRadarChartRequestType() {
    $.getJSON(baseUrl + "/Home/ChartVolumeTotalRequestTypePerYear").done(function (data) {

        var labels_trending = data.map(function (e) {
            return e.Name;
        });
        var data_trending = data.map(function (e) {
            return e.Total;
        });

        var browserStatsChartCTX = $("#RadarChartRequestType");

        var browserStatsChartRequestTypeOptions = {
            responsive: true,
            legend: {
                display: false
            },
            hover: {
                mode: "label"
            },
            scale: {
                angleLines: { color: "rgba(255,255,255,0.4)" },
                gridLines: { color: "rgba(255,255,255,0.2)" },
                ticks: {
                    display: false
                },
                pointLabels: {
                    fontColor: "#fff"
                }
            }, plugins: {
                datalabels: {
                    display: false,
                    anchor: 'end',
                    align: 'end',
                    formatter: Math.round,
                    color: '#fff',
                    labels: {
                        title: {
                            font: {
                                weight: 'bold'
                            }
                        },
                        value: {
                            color: '#fff',
                        }
                    },
                }
            }
        };

        var browserStatsChartData = {
            labels: labels_trending,//["PP Down payment", "PP Wrong Transfer", "PP Over Payment"],
            datasets: [
                {
                    label: "Count",
                    data: data_trending,//[5, 6, 7],
                    fillColor: "rgba(255,255,255,0.2)",
                    borderColor: "#fff",
                    pointBorderColor: "#fff",
                    pointBackgroundColor: "#00bfa5",
                    pointHighlightFill: "#fff",
                    pointHoverBackgroundColor: "#fff",
                    borderWidth: 2,
                    pointBorderWidth: 2,
                    pointHoverBorderWidth: 4
                }
            ]
        };

        var browserStatsChartConfig = {
            type: "radar",
            options: browserStatsChartRequestTypeOptions,
            data: browserStatsChartData
        };

        new Chart(browserStatsChartCTX, browserStatsChartConfig);
    });
}

function InitHeaderInfo() {
    $.getJSON(baseUrl + "/Home/TotalRequestPerYear").done(function (data) {
        $('#TotalRequest').text(data);
    });
    $.getJSON(baseUrl + "/Home/TotalUrgentRequestPerYear").done(function (data) {
        $('#TotalUrgentRequest').text(data);
    });
    $.getJSON(baseUrl + "/Home/TotalUnFinishedRequest").done(function (data) {
        $('#TotalUnFinishedRequest').text(data);
    });
    $.getJSON(baseUrl + "/Home/CurrencyUSDPerNow").done(function (data) {
        $('#CurrencUSDyPerNow').text(Globalize("en").currencyFormatter("USD")(data));
    });
}

function InitBarChartRefundArea() {
    $.getJSON(baseUrl + "/Home/BarChartValueByAreaMonthlyPerYear").done(function (data) {
        var labels_period = data.map(function (e) {
            return e.Month;
        });
        var data_period = data.map(function (e) {
            return e.Total;
        });


        var monthlyRevenueChartCTX = $("#BarChartAreaYear");
        var monthlyRevenueChartOptions = {
            responsive: true,
            legend: {
                display: false
            },
            hover: {
                mode: "label"
            },
            scales: {
                xAxes: [
                    {
                        display: true,
                        gridLines: {
                            display: false
                        }
                    }
                ],
                yAxes: [
                    {
                        display: true,
                        fontColor: "#fff",
                        gridLines: {
                            display: false
                        },
                        ticks: {
                            beginAtZero: true,
                            suggestedMax: 50,
                        }
                    }
                ]
            },
            plugins: {
                datalabels: {
                    display: false,
                    anchor: 'end',
                    align: 'top',
                    formatter: Math.round,
                    color: '#46BFBD',
                    labels: {
                        title: {
                            font: {
                                weight: 'bold'
                            }
                        },
                        value: {
                            color: '#fff',
                        }
                    },
                }
            }
        };
        var monthlyRevenueChartData = {
            labels: labels_period,
            datasets: [
                {
                    label: "Sales",
                    data: data_period,
                    backgroundColor: "#46BFBD",
                    hoverBackgroundColor: "#009688"
                }
            ]
        };
        var monthlyRevenueChartConfig = {
            type: "bar",
            options: monthlyRevenueChartOptions,
            data: monthlyRevenueChartData
        };

        new Chart(monthlyRevenueChartCTX, monthlyRevenueChartConfig);
    });
}

function InitLineChartRefundArea() {
    $.getJSON(baseUrl + "/Home/ChartValueByAreaYear").done(function (data) {
        var labelYear = data.map(function (e) {
            return e.Area;
        });
        var data_year = data.map(function (e) {
            return e.Total;
        });
        var data_amount = data.map(function (e) {
            return e.Sum;
        })

        var revenueLineChartYearOptions = {
            responsive: true,
            legend: {
                display: false
            },
            hover: {
                mode: "label"
            },
            scales: {
                xAxes: [
                    {
                        stacked: true,
                        display: true,
                        gridLines: {
                            display: false
                        },
                        ticks: {
                            beginAtZero: true,
                            fontColor: "#fff",
                            suggestedMax: 50,
                        }
                    }
                ],
                yAxes: [
                    {
                        stacked: true,
                        display: true,
                        fontColor: "#fff",
                        gridLines: {
                            display: true,
                            color: "rgba(255,255,255,0.3)"
                        },
                        ticks: {
                            beginAtZero: true,
                            fontColor: "#fff"
                        }
                    }
                ]
            },
            plugins: {
                datalabels: {
                    display: true,
                    anchor: 'end',
                    align: 'end',
                    formatter: function (value, obj) {
                        return convertToRupiah(data_amount[obj.dataIndex]);
                    },
                    color: '#fff',
                    labels: {
                        title: {
                            font: {
                                weight: 'bold'
                            }
                        },
                        value: {
                            color: '#fff',
                        }
                    },
                }
            }
        };


        var revenueLineChartYearCTX = $("#LineChartAreaYear");

        var revenueLineChartDataYear = {
            labels: labelYear,
            datasets: [
                {
                    label: "Year",
                    data: data_year,
                    backgroundColor: "rgba(128, 222, 234, 0.5)",
                    borderColor: "#d1faff",
                    pointBorderColor: "#d1faff",
                    pointBackgroundColor: "#00bcd4",
                    pointHighlightFill: "#d1faff",
                    pointHoverBackgroundColor: "#d1faff",
                    borderWidth: 2,
                    pointBorderWidth: 2,
                    pointHoverBorderWidth: 4,
                    pointRadius: 4
                }
            ]
        };

        var revenueLineChartYearConfig = {
            type: "horizontalBar",
            options: revenueLineChartYearOptions,
            data: revenueLineChartDataYear
        };
        var revenueLineChartYear = new Chart(revenueLineChartYearCTX, revenueLineChartYearConfig);

    });
    $.getJSON(baseUrl + "/Home/ChartValueByAreaMonth").done(function (data) {
        var labelMonth = data.map(function (e) {
            return e.Area;
        });
        var data_month = data.map(function (e) {
            return e.Total;
        });
        data_amount = data.map(function (e) {
            return e.Sum;
        })

        var revenueLineChartMonthOptions = {
            responsive: true,
            legend: {
                display: false
            },
            hover: {
                mode: "label"
            },
            scales: {
                xAxes: [
                    {
                        stacked: true,
                        display: true,
                        gridLines: {
                            display: false
                        },
                        ticks: {
                            beginAtZero: true,
                            fontColor: "#fff",
                            suggestedMax: 50,
                        }
                    }
                ],
                yAxes: [
                    {
                        stacked: true,
                        display: true,
                        fontColor: "#fff",
                        gridLines: {
                            display: true,
                            color: "rgba(255,255,255,0.3)"
                        },
                        ticks: {
                            beginAtZero: true,
                            fontColor: "#fff"
                        }
                    }
                ]
            },
            plugins: {
                datalabels: {
                    display: true,
                    anchor: 'end',
                    align: 'end',
                    color: '#fff',
                    labels: {
                        title: {
                            font: {
                                weight: 'bold'
                            }
                        },
                        value: {
                            color: '#fff',
                        }
                    },
                    formatter: function (value, obj) {
                        return convertToRupiah(data_amount[obj.dataIndex]);
                    },
                }
            }
        };


        var revenueLineChartMonthCTX = $("#LineChartAreaMonth");

        var revenueLineChartMonthData = {
            labels: labelMonth,
            datasets: [
                {
                    label: "Month",
                    data: data_month,
                    backgroundColor: "rgba(128, 222, 234, 0.5)",
                    borderColor: "#d1faff",
                    pointBorderColor: "#d1faff",
                    pointBackgroundColor: "#00bcd4",
                    pointHighlightFill: "#d1faff",
                    pointHoverBackgroundColor: "#d1faff",
                    borderWidth: 2,
                    pointBorderWidth: 2,
                    pointHoverBorderWidth: 4,
                    pointRadius: 4
                }
            ]
        };

        var revenueLineChartMonthConfig = {
            type: "horizontalBar",
            options: revenueLineChartMonthOptions,
            data: revenueLineChartMonthData
        };
        var revenueLineChartMonth = new Chart(revenueLineChartMonthCTX, revenueLineChartMonthConfig);
    });
}
function a() {


    //periode

    //TOP 5 BRANCH
    var jsonfile_branch = {
        "jsonarray_branch": [{
            "branch": "Kalimantan",
            "nilai_branch": 50
        }, {
            "branch": "Sumatra",
            "nilai_branch": 70
        }, {
            "branch": "Jakarta",
            "nilai_branch": 60
        }, {
            "branch": "Jawa Barat",
            "nilai_branch": 80
        }, {
            "branch": "Jawa Timur",
            "nilai_branch": 90
        }]
    };
    var labels_branch = jsonfile_branch.jsonarray_branch.map(function (e) {
        return e.branch;
    });
    var data_branch = jsonfile_branch.jsonarray_branch.map(function (e) {
        return e.nilai_branch;
    });

    /*Line Chart */

    //Doughnut chart


    /*per tahun */
    var yearlyrevenueLineChartCTX = $("#revenue-line-chart");

    var yearlyrevenueLineChartOptions = {
        responsive: true,
        legend: {
            display: false
        },
        hover: {
            mode: "label"
        },
        scales: {
            xAxes: [
                {
                    display: true,
                    gridLines: {
                        display: false
                    },
                    ticks: {
                        fontColor: "#fff"
                    }
                }
            ],
            yAxes: [
                {
                    display: true,
                    fontColor: "#fff",
                    gridLines: {
                        display: true,
                        color: "rgba(255,255,255,0.3)"
                    },
                    ticks: {
                        beginAtZero: false,
                        fontColor: "#fff"
                    }
                }
            ]
        }
    };

    var yearlyrevenueLineChartData = {

        labels: labels_branch,
        datasets: [
            {
                label: "Today",
                data: data_branch,
                backgroundColor: "rgba(128, 222, 234, 0.5)",
                borderColor: "#D0E6A5",
                pointBorderColor: "#d1faff",
                pointBackgroundColor: "#00bcd4",
                pointHighlightFill: "#d1faff",
                pointHoverBackgroundColor: "#d1faff",
                borderWidth: 2,
                pointBorderWidth: 2,
                pointHoverBorderWidth: 4,
                pointRadius: 4

            }
        ]
    };

    var yearlyrevenueLineChartConfig = {
        type: "line",
        options: yearlyrevenueLineChartOptions,
        data: yearlyrevenueLineChartData
    };

    /* PER TOP 5 BRANCH*/
    /*per tahun */
    $("#revenue-line-chart-top-five");

    var topFiveRevenueLineChartOptions = {
        responsive: true,
        legend: {
            display: false
        },
        hover: {
            mode: "label"
        },
        scales: {
            xAxes: [
                {
                    display: true,
                    gridLines: {
                        display: false
                    },
                    ticks: {
                        fontColor: "#fff"
                    }
                }
            ],
            yAxes: [
                {
                    display: true,
                    fontColor: "#fff",
                    gridLines: {
                        display: true,
                        color: "rgba(255,255,255,0.3)"
                    },
                    ticks: {
                        beginAtZero: false,
                        fontColor: "#fff"
                    }
                }
            ]
        }
    };

    var topFiveRevenueLineChartData = {

        labels: labels_branch,
        datasets: [
            {
                label: "Today",
                data: data_branch,
                backgroundColor: "rgba(128, 222, 234, 0.5)",
                //"#DDE6A5"
                borderColor: "#D0E6A5",//"#d1faff",
                pointBorderColor: "#d1faff",
                pointBackgroundColor: "#00bcd4",
                pointHighlightFill: "#d1faff",
                pointHoverBackgroundColor: "#d1faff",
                borderWidth: 2,
                pointBorderWidth: 2,
                pointHoverBorderWidth: 4,
                pointRadius: 4

            }
        ]
    };

    var topFiveRevenueLineChartConfig = {
        type: "line",
        options: topFiveRevenueLineChartOptions,
        data: topFiveRevenueLineChartData
    };

}

function PageLoadActiveVendor() {
    var filter_type = selectddlFilterType.val();

    if (filter_type === 'period') {
        document.getElementById('yearact').style.display = 'none';
        document.getElementById('monthact').style.display = 'none';

        document.getElementById('yearseparate').style.display = 'block';
        document.getElementById('yearfrom').style.display = 'block';
        document.getElementById('monthfrom').style.display = 'block';
        document.getElementById('yearto').style.display = 'block';
        document.getElementById('monthto').style.display = 'block';
    } else {
        document.getElementById('yearact').style.display = 'block';
        document.getElementById('monthact').style.display = 'block';

        document.getElementById('yearseparate').style.display = 'none';
        document.getElementById('yearfrom').style.display = 'none';
        document.getElementById('monthfrom').style.display = 'none';
        document.getElementById('yearto').style.display = 'none';
        document.getElementById('monthto').style.display = 'none';
    }
}

function InitChartActiveVendorNewVerse() {
    var year = selectyearVendorActice.val() ?? 0;
    var month = parseInt(selectmonthVendorActice.val()) + 1 ?? 0;
    var yearFrom = selectyearVendorActiceFrom.val() ?? 0;
    var monthFrom = parseInt(selectmonthVendorActiceFrom.val()) + 1 ?? 0;
    var yearTo = selectyearVendorActiceTo.val() ?? 0;
    var monthTo = parseInt(selectmonthVendorActiceTo.val()) + 1 ?? 0;

    var FilterType = selectddlFilterType.val();


    var title = "Hit Rate based on PO Population is on Current Month and Year Selection";

    //$.getJSON("/POST/GetDashboardActiveVendor?Year=" + year + "&Month=" + month + "").done(function (data) {
    $.getJSON("/POST/GetDashboardActiveVendorNewVerse?Year=" + year + "&Month=" + month + "&YearFrom=" + yearFrom + "&MonthFrom=" + monthFrom + "&YearTo=" + yearTo + "&MonthTo=" + monthTo + "&Type=" + FilterType + "").done(function (data) {
        const chartActiveVendor = Highcharts.chart('container2',
            {
                credits: {
                    enabled: false
                },
                title: {
                    text: title,
                    //text: '{point.y}',
                    style: {
                        fontFamily: "Muli",
                        color: "#333333",
                        fontSize: "18px",
                        fontWeight: "normal",
                        fontStyle: "normal"
                    }
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    "series": {
                        "allowPointSelect": true,
                        "states": {
                            "select": {
                                "color": "#EFFFEF",
                                "borderColor": "black",
                                "dashStyle": "dot"
                            }
                        },
                        animation: {
                            duration: 2000
                        }
                    },
                    pie: {

                        credits: {
                            enabled: false
                        },
                        allowPointSelect: true,
                        cursor: 'pointer',
                        //"showInLegend": true,
                        //    colors: pieColors,
                        dataLabels: {
                            //enabled: true,
                            format: '<b>{point.name} : {point.y}'
                            //distance: -50,
                            //style: {
                            //    fontfamily: "muli"
                            //},
                            //formatter: function () {
                            //    console.log(this);
                            //    //            var qty = this.y;
                            //    //            var res = qty + " %";
                            //    //            return res;
                            //},
                            ////    }
                            //filter: {
                            //    property: 'percentage',
                            //    operator: '>',
                            //    value: 4
                            //}
                        }
                    },
                    "line": {
                        //"dataLabels": {
                        //    "enabled": true
                        //},
                        "enableMouseTracking": false
                    }
                },
                "exporting": {},
                "credits": {
                    "text": "cloud.highcharts.com",
                    "href": "https://cloud.highcharts.com"
                },
                "chart": {
                    style: {
                        fontFamily: "Muli"
                    },
                    "options3d": {
                        "enabled": true,
                        "alpha": 45,
                        "beta": 10
                    },
                    "polar": true,
                    "type": "line",
                    "borderColor": "#ec407a",
                    "borderRadius": 1
                },
                "yAxis": {
                    "title": {
                        "text": "Qty"
                    }
                },
                series: [
                    {
                        "turboThreshold": 0,
                        "_colorIndex": 0,
                        "_symbolIndex": 0,
                        "type": "pie",
                        "marker": {
                            "symbol": "circle",
                            "enabled": true
                        },
                        "colorByPoint": true,
                        name: 'Share',
                        data: data.result
                    }
                ],
                "colors": [
                    "#7cb5ec",
                    "#673ab7",
                    "#8bc34a",
                    "#ff80ab",
                    "#ffea00"
                ],
                "legend": {
                    "enabled": false,
                    //align: 'left',
                    //"layout": "horizontal",
                    "itemStyle": {

                        //    layout: 'vertical',
                        //    verticalAlign: 'top',
                        //    x: 40,
                        //    y: 0
                        //}
                        fontFamily: "Muli",
                        "color": "#333333",
                        "fontSize": "10px",
                        "fontWeight": "bold",
                        "fontStyle": "normal",
                        "cursor": "pointer"
                    }
                },
                "stockTools": {
                    "gui": {
                        "buttons": [
                            "simpleShapes",
                            "lines",
                            "crookedLines"
                        ],
                        "enabled": false
                    }
                },
                "navigation": {
                    "events": {
                        "showPopup":
                            "function(e){this.chart.indicatorsPopupContainer||(this.chart.indicatorsPopupContainer=document.getElementsByClassName(\"highcharts-popup-indicators\")[0]),this.chart.annotationsPopupContainer||(this.chart.annotationsPopupContainer=document.getElementsByClassName(\"highcharts-popup-annotations\")[0]),\"indicators\"===e.formType?this.chart.indicatorsPopupContainer.style.display=\"block\":\"annotation-toolbar\"===e.formType&&(this.chart.activeButton||(this.chart.currentAnnotation=e.annotation,this.chart.annotationsPopupContainer.style.display=\"block\")),this.popup&&(c=this.popup)}",
                        "closePopup":
                            "function(){this.chart.annotationsPopupContainer.style.display=\"none\",this.chart.currentAnnotation=null}",
                        "selectButton":
                            "function(e){var t=e.button.className+\" highcharts-active\";e.button.classList.contains(\"highcharts-active\")||(e.button.className=t,this.chart.activeButton=e.button)}",
                        "deselectButton":
                            "function(e){e.button.classList.remove(\"highcharts-active\"),this.chart.activeButton=null}"
                    },
                    "bindingsClassName": "tools-container"
                },
                "pane": {
                    "background": []
                },
                "responsive": {
                    "rules": []
                },
                "lang": {
                    "decimalPoint": "2",
                    "thousandsSep": ",",
                    "downloadJPEG": ""
                },
                "caption": {
                    "text": ""
                },
                "annotations": []
            });
    });



    InitChartTopVendorHitrate();
    InitChartTopVendorHitrateTbl();
}

function FilterEmployeeInternal() {
    const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    var qntYears = 4;

    var selectyearEmpActiveIntrnl = $("#yearEmpActiveIntrnl");
    var selectmonthEmpActiveIntrnl = $("#monthEmpActiveIntrnl");
    var selectdayVendorActiceIntrnl = $("#dayVendorActiceIntrnl");

    var selectyearEmpActiveFromIntrnl = $("#yearEmpActiveFromIntrnl");
    var selectmonthEmpActiveFromIntrnl = $("#monthEmpActiveFromIntrnl");
    var selectyearEmpActiveToIntrnl = $("#yearEmpActiveToIntrnl");
    var selectmonthEmpActiveToIntrnl = $("#monthEmpActiveToIntrnl");
    var selectdayVendorActiceFrom = $("#dayVendorActiceFrom");
    var selectdayVendorActiceTo = $("#dayVendorActiceTo");

    var currentYear = new Date().getFullYear();

    for (var y = 0; y < qntYears; y++) {
        var yearElem = document.createElement("option");
        var yearElemFrom = document.createElement("option");
        var yearElemTo = document.createElement("option");

        yearElem.value = currentYear
        yearElem.textContent = currentYear;
        yearElemFrom.value = currentYear
        yearElemFrom.textContent = currentYear;
        yearElemTo.value = currentYear
        yearElemTo.textContent = currentYear;

        selectyearEmpActiveIntrnl.append(yearElem);
        selectyearEmpActiveFromIntrnl.append(yearElemFrom);
        selectyearEmpActiveToIntrnl.append(yearElemTo);

        currentYear--;
    }

    for (var m = 0; m < 12; m++) {
        let monthNum = new Date(2018, m).getMonth()
        let months = monthNames[monthNum];
        var monthElem = document.createElement("option");
        var monthElemFrom = document.createElement("option");
        var monthElemTo = document.createElement("option");

        monthElem.value = monthNum;
        monthElem.textContent = months;
        monthElemFrom.value = monthNum;
        monthElemFrom.textContent = months;
        monthElemTo.value = monthNum;
        monthElemTo.textContent = months;

        selectmonthEmpActiveIntrnl.append(monthElem);
        selectmonthEmpActiveFromIntrnl.append(monthElemFrom);
        selectmonthEmpActiveToIntrnl.append(monthElemTo);
    }

    var d = new Date();
    var month = d.getMonth();
    var year = d.getFullYear();
    var day = d.getDate();

    selectyearEmpActiveIntrnl.val(year);
    //selectyearEmpActiveIntrnl.on("change", AdjustDays);
    selectyearEmpActiveFromIntrnl.val(year);
    selectyearEmpActiveToIntrnl.val(year);

    selectmonthEmpActiveIntrnl.val(month);
    selectmonthEmpActiveFromIntrnl.val(month);
    selectmonthEmpActiveToIntrnl.val(month);

    //AdjustDays();
    //selectDay.val(day)

    //function AdjustDays() {
    //    var years = selectYear.val();
    //    var month = parseInt(selectMonth.val()) + 1;
    //    selectDay.empty();

    //    //get the last day, so the number of days in that month
    //    var days = new Date(years, month, 0).getDate();

    //    //lets create the days of that month
    //    for (var ddd = 1; ddd <= days; ddd++) {
    //        var dayElem = document.createElement("option");
    //        dayElem.value = ddd;
    //        dayElem.textContent = ddd;
    //        selectDay.append(dayElem);
    //    }
    //}
}

function PageLoadActiveEmp() {
    var filter_type_emp = selectddlFilterTypeIntrnl.val();

    if (filter_type_emp === 'period') {
        document.getElementById('yearemp').style.display = 'none';
        document.getElementById('monthemp').style.display = 'none';

        document.getElementById('yearseparateEmpPeriod').style.display = 'block';
        document.getElementById('yearfromEmp').style.display = 'block';
        document.getElementById('monthfromEmp').style.display = 'block';
        document.getElementById('yeartoEmp').style.display = 'block';
        document.getElementById('monthtoEmp').style.display = 'block';
    } else {
        document.getElementById('yearemp').style.display = 'block';
        document.getElementById('monthemp').style.display = 'block';

        document.getElementById('yearseparateEmpPeriod').style.display = 'none';
        document.getElementById('yearfromEmp').style.display = 'none';
        document.getElementById('monthfromEmp').style.display = 'none';
        document.getElementById('yeartoEmp').style.display = 'none';
        document.getElementById('monthtoEmp').style.display = 'none';
    }
}

function InitChartActiveEmployee() {
    var year = selectyearEmpActiveIntrnl.val() ?? 0;
    var month = parseInt(selectmonthEmpActiveIntrnl.val()) + 1 ?? 0;
    var yearFrom = selectyearEmpActiveFromIntrnl.val() ?? 0;
    var monthFrom = parseInt(selectmonthEmpActiveFromIntrnl.val()) + 1 ?? 0;
    var yearTo = selectyearEmpActiveToIntrnl.val() ?? 0;
    var monthTo = parseInt(selectmonthEmpActiveToIntrnl.val()) + 1 ?? 0;

    var FilterType = selectddlFilterTypeIntrnl.val();


    var title = "Employee";

    $.getJSON("/POST/GetDashboardActiveEmployee?Year=" + year + "&Month=" + month + "&YearFrom=" + yearFrom + "&MonthFrom=" + monthFrom + "&YearTo=" + yearTo + "&MonthTo=" + monthTo + "&Type=" + FilterType + "").done(function (data) {
        Highcharts.chart('container5',
            {
                credits: {
                    enabled: false
                },
                title: {
                    text: title,
                    style: {
                        fontFamily: "Muli",
                        color: "#333333",
                        fontSize: "18px",
                        fontWeight: "normal",
                        fontStyle: "normal"
                    }
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    "series": {
                        "allowPointSelect": true,
                        "states": {
                            "select": {
                                "color": "#EFFFEF",
                                "borderColor": "black",
                                "dashStyle": "dot"
                            }
                        },
                        animation: {
                            duration: 2000
                        }
                    },
                    pie: {
                        credits: {
                            enabled: false
                        },
                        allowPointSelect: true,
                        cursor: 'pointer',
                        //"showInLegend": true,
                        //    colors: pieColors,
                        dataLabels: {
                            //enabled: true,
                            format: '<b>{point.name} : {point.y}'
                        }
                    },
                    "line": {
                        //"dataLabels": {
                        //    "enabled": true
                        //},
                        "enableMouseTracking": false
                    }
                },
                "exporting": {},
                "credits": {
                    "text": "cloud.highcharts.com",
                    "href": "https://cloud.highcharts.com"
                },
                "chart": {
                    style: {
                        fontFamily: "Muli"
                    },
                    "options3d": {
                        "enabled": true,
                        "alpha": 45,
                        "beta": 10
                    },
                    "polar": true,
                    "type": "line",
                    "borderColor": "#ec407a",
                    "borderRadius": 1
                },
                "yAxis": {
                    "title": {
                        "text": "Qty"
                    }
                },
                series: [
                    {
                        "turboThreshold": 0,
                        "_colorIndex": 0,
                        "_symbolIndex": 0,
                        "type": "pie",
                        "marker": {
                            "symbol": "circle",
                            "enabled": true
                        },
                        "colorByPoint": true,
                        name: 'Share',
                        data: data.result
                    }
                ],
                "colors": [
                    "#7cb5ec",
                    "#673ab7",
                    "#8bc34a",
                    "#ff80ab",
                    "#ffea00"
                ],
                "legend": {
                    "enabled": false,
                    //align: 'left',
                    //"layout": "horizontal",
                    "itemStyle": {

                        //    layout: 'vertical',
                        //    verticalAlign: 'top',
                        //    x: 40,
                        //    y: 0
                        //}
                        fontFamily: "Muli",
                        "color": "#333333",
                        "fontSize": "10px",
                        "fontWeight": "bold",
                        "fontStyle": "normal",
                        "cursor": "pointer"
                    }
                },
                "stockTools": {
                    "gui": {
                        "buttons": [
                            "simpleShapes",
                            "lines",
                            "crookedLines"
                        ],
                        "enabled": false
                    }
                },
                "navigation": {
                    "events": {
                        "showPopup":
                            "function(e){this.chart.indicatorsPopupContainer||(this.chart.indicatorsPopupContainer=document.getElementsByClassName(\"highcharts-popup-indicators\")[0]),this.chart.annotationsPopupContainer||(this.chart.annotationsPopupContainer=document.getElementsByClassName(\"highcharts-popup-annotations\")[0]),\"indicators\"===e.formType?this.chart.indicatorsPopupContainer.style.display=\"block\":\"annotation-toolbar\"===e.formType&&(this.chart.activeButton||(this.chart.currentAnnotation=e.annotation,this.chart.annotationsPopupContainer.style.display=\"block\")),this.popup&&(c=this.popup)}",
                        "closePopup":
                            "function(){this.chart.annotationsPopupContainer.style.display=\"none\",this.chart.currentAnnotation=null}",
                        "selectButton":
                            "function(e){var t=e.button.className+\" highcharts-active\";e.button.classList.contains(\"highcharts-active\")||(e.button.className=t,this.chart.activeButton=e.button)}",
                        "deselectButton":
                            "function(e){e.button.classList.remove(\"highcharts-active\"),this.chart.activeButton=null}"
                    },
                    "bindingsClassName": "tools-container"
                },
                "pane": {
                    "background": []
                },
                "responsive": {
                    "rules": []
                },
                "lang": {
                    "decimalPoint": "2",
                    "thousandsSep": ",",
                    "downloadJPEG": ""
                },
                "caption": {
                    "text": ""
                },
                "annotations": []
            });
    });

    InitChartTopEmployeeHitrate();
    InitChartTopEmployeeHitrateTbl();
}


function InitChartTopVendorHitrate() {
    var year = selectyearVendorActice.val() ?? 0;
    var month = parseInt(selectmonthVendorActice.val()) + 1 ?? 0;
    var yearFrom = selectyearVendorActiceFrom.val() ?? 0;
    var monthFrom = parseInt(selectmonthVendorActiceFrom.val()) + 1 ?? 0;
    var yearTo = selectyearVendorActiceTo.val() ?? 0;
    var monthTo = parseInt(selectmonthVendorActiceTo.val()) + 1 ?? 0;

    var FilterType = selectddlFilterType.val();
    var strMonth = selectmonthVendorActice.children("option").filter(":selected").text();
    var strMonthFrom = selectmonthVendorActiceFrom.children("option").filter(":selected").text();
    var strMonthTo = selectmonthVendorActiceTo.children("option").filter(":selected").text();

    var period = strMonth + ' ' + year;

    if (FilterType === 'period') {
        period = strMonthFrom + ' ' + yearFrom + ' - ' + strMonthTo + ' ' + yearTo;
    }

    $.getJSON("/POST/GetDashboardVendorHitrate?Year=" + year + "&Month=" + month + "&YearFrom=" + yearFrom + "&MonthFrom=" + monthFrom + "&YearTo=" + yearTo + "&MonthTo=" + monthTo + "&Type=" + FilterType + "").done(function (data) {

        var label = data.result.map(function (e) {
            return e.name;
        });

        var Total = data.result.map(function (e) {
            return e.total;
        });

        var Value = data.result.map(function (e) {
            return 0;
        });


        Highcharts.chart('containerMdlHitVendor', {
            chart: {
                zoomType: 'xy'
            },
            title: {
                text: 'Top 5 Vendor Hit Rate  (' + period + ')'
            },
            xAxis: {
                categories: label
            },
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: 'Total Hit',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                }
            }, { // Secondary yAxis
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: 'Total Hit',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                opposite: true
            }],
            plotOptions: {
                column: {
                    cursor: 'pointer',
                    dataLabels: {
                        inside: true,
                        enabled: true,
                        formatter: function () {
                            var qty = this.y;
                            var res = qty;
                            return res;
                        }
                    }
                },

            },
            tooltip: {
                shared: true
            },
            labels: {
                items: [{
                    style: {
                        left: '50px',
                        top: '18px',
                        color: ( // theme
                            Highcharts.defaultOptions.title.style &&
                            Highcharts.defaultOptions.title.style.color
                        ) || 'black'
                    }
                }]
            },
            series: [{
                type: 'column',
                yAxis: 1,
                name: 'Vendor',
                data: Total
            }, {
                type: 'spline',
                name: 'Total',
                data: Value,
                marker: {
                    lineWidth: 2,
                    lineColor: Highcharts.getOptions().colors[3],
                    fillColor: 'white'
                }
            }]
        });
    });
}

function InitChartTopVendorHitrateTbl() {
    var uyear = selectyearVendorActice.val() ?? 0;
    var umonth = parseInt(selectmonthVendorActice.val()) + 1 ?? 0;
    var uyearFrom = selectyearVendorActiceFrom.val() ?? 0;
    var umonthFrom = parseInt(selectmonthVendorActiceFrom.val()) + 1 ?? 0;
    var uyearTo = selectyearVendorActiceTo.val() ?? 0;
    var umonthTo = parseInt(selectmonthVendorActiceTo.val()) + 1 ?? 0;
    var FilterType = selectddlFilterType.val();

    //var uyearFrom = "2021"; //$('#kpiname').val(),
    //var umonthFrom = "5"; //$('#kpitype').val(),
    //var uyearTo = "2021"; //$('#kpitype').val(),
    //var umonthTo = "5"; //$('#kpitype').val(),

    $.ajax({
        url: "/POST/GetDashboardVendorHitrateTbl",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ year: uyear, month: umonth, yearFrom: uyearFrom, monthFrom: umonthFrom, yearTo: uyearTo, monthTo: umonthTo, type: FilterType }),
        dataType: "json",
        success: function (data) {
            var row = "";
            $.each(data, function (index, item) {
                row += "<tr><td>" + item.no + "</td><td>" + item.name + "</td><td class='center'>" + item.total + "</td></tr>";
            });
            $("#vendorhitrate").html(row);
        },
        error: function (result) {
            alert(result);
        }
    });

    //$.getJSON("/POST/GetListTblVendorHitrate?Year=2021&Month=5&YearFrom=2021&MonthFrom=5&YearTo=2021&MonthTo=5&Type=").done(function (data) {
    //});

    //$.getJSON("/POST/GetDashboardVendorHitrateTbl?Year=2021&Month=5&YearFrom=2021&MonthFrom=5&YearTo=2021&MonthTo=5&Type=" + FilterType + "").done(function (data) {
    //    //code to bind table
    //    $.each(data, function (i, item) {
    //        var html = "<tr><td>" + item.Name + "</td>";
    //        html += "<td>" + item.total + "</td>";
    //        // and html += other fields...
    //        $("#grd1 tr:last").after(html);
    //        // the above line is like that because you use <tbody> 
    //        // in table definition.
    //    });
    //});

    //$("#tablevendorhitrate").bootstrapTable({
    //    url: "/POST/GetDashboardVendorHitrateTbl",
    //    pagination: true,
    //    sidePagination: 'server',
    //    pageSize: 10,
    //    queryParams: function () {
    //        return {
    //            yearFrom: 2021,
    //            monthFrom: 5,
    //            yearTo: 2021,
    //            monthTo: 5,
    //            type: 'monthly'
    //        };
    //    },
    //    pageList: [10, 25, 50, 100],
    //    responseHandler: function (res) {
    //        var offset = res.offset;
    //        $.each(res.rows, function (i, row) {
    //            row.index = offset + (i + 1);
    //        });
    //        return res;
    //    },
    //    cache: false,
    //    smartDisplay: false,
    //    trimOnSearch: false,
    //    columns: [{
    //        field: 'Name',
    //        title: 'Name'
    //    }, {
    //        field: 'total',
    //        title: 'total'
    //    }],
    // });
}

function InitChartTopEmployeeHitrate() {
    var year = selectyearEmpActiveIntrnl.val() ?? 0;
    var month = parseInt(selectmonthEmpActiveIntrnl.val()) + 1 ?? 0;
    var yearFrom = selectyearEmpActiveFromIntrnl.val() ?? 0;
    var monthFrom = parseInt(selectmonthEmpActiveFromIntrnl.val()) + 1 ?? 0;
    var yearTo = selectyearEmpActiveToIntrnl.val() ?? 0;
    var monthTo = parseInt(selectmonthEmpActiveToIntrnl.val()) + 1 ?? 0;

    var FilterType = selectddlFilterTypeIntrnl.val();
    var strMonth = selectmonthEmpActiveIntrnl.children("option").filter(":selected").text();
    var strMonthFrom = selectmonthEmpActiveFromIntrnl.children("option").filter(":selected").text();
    var strMonthTo = selectmonthEmpActiveToIntrnl.children("option").filter(":selected").text();

    var period = strMonth + ' ' + year;

    if (FilterType === 'period') {
        period = strMonthFrom + ' ' + yearFrom + ' - ' + strMonthTo + ' ' + yearTo;
    }

    $.getJSON("/POST/GetDashboardEmployeeHitrate?Year=" + year + "&Month=" + month + "&YearFrom=" + yearFrom + "&MonthFrom=" + monthFrom + "&YearTo=" + yearTo + "&MonthTo=" + monthTo + "&Type=" + FilterType + "").done(function (data) {

        var label = data.result.map(function (e) {
            return e.name;
        });

        var Total = data.result.map(function (e) {
            return e.total;
        });

        var Value = data.result.map(function (e) {
            return 0;
        });


        Highcharts.chart('containerMdlHitEmployee', {
            chart: {
                zoomType: 'xy'
            },
            title: {
                text: 'Top 10 Employee Hit Rate  (' + period + ')'
            },
            xAxis: {
                categories: label
            },
            yAxis: [{ // Primary yAxis
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: 'Total Hit',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                }
            }, { // Secondary yAxis
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: 'Total Hit',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                opposite: true
            }],
            plotOptions: {
                column: {
                    cursor: 'pointer',
                    dataLabels: {
                        inside: true,
                        enabled: true,
                        formatter: function () {
                            var qty = this.y;
                            var res = qty;
                            return res;
                        }
                    }
                },

            },
            tooltip: {
                shared: true
            },
            labels: {
                items: [{
                    style: {
                        left: '50px',
                        top: '18px',
                        color: ( // theme
                            Highcharts.defaultOptions.title.style &&
                            Highcharts.defaultOptions.title.style.color
                        ) || 'black'
                    }
                }]
            },
            series: [{
                type: 'column',
                yAxis: 1,
                name: 'Name',
                data: Total
            }, {
                type: 'spline',
                name: 'Total',
                data: Value,
                marker: {
                    lineWidth: 2,
                    lineColor: Highcharts.getOptions().colors[3],
                    fillColor: 'white'
                }
            }]
        });
    });
}

function InitChartTopEmployeeHitrateTbl() {
    var uyear = selectyearEmpActiveIntrnl.val() ?? 0;
    var umonth = parseInt(selectmonthEmpActiveIntrnl.val()) + 1 ?? 0;
    var uyearFrom = selectyearEmpActiveFromIntrnl.val() ?? 0;
    var umonthFrom = parseInt(selectmonthEmpActiveFromIntrnl.val()) + 1 ?? 0;
    var uyearTo = selectyearEmpActiveToIntrnl.val() ?? 0;
    var umonthTo = parseInt(selectmonthEmpActiveToIntrnl.val()) + 1 ?? 0;
    var FilterType = selectddlFilterTypeIntrnl.val();

    $.ajax({
        url: "/POST/GetDashboardEmployeeHitrateTbl",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ year: uyear, month: umonth, yearFrom: uyearFrom, monthFrom: umonthFrom, yearTo: uyearTo, monthTo: umonthTo, type: FilterType }),
        dataType: "json",
        success: function (data) {
            var row = "";
            $.each(data, function (index, item) {
                row += "<tr><td>" + item.no + "</td><td>" + item.name + "</td><td class='center'>" + item.total + "</td></tr>";
            });
            $("#employeehitrate").html(row);
        },
        error: function (result) {
            alert(result);
        }
    });
}