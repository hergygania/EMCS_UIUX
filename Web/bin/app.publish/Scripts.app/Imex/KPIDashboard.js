
$(document).ready(function () {
    getDataChangeLogChart();
    getDataChangeLogChartNewMapping();
    getDataSCISvsCCOS();
});

function initSCISvsCCOS(dataTable) {
    Highcharts.chart('container', {
        credits: false,
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'CCOS vs SCIS'
        },
        tooltip: {
            pointFormat: 'Percentage : <b>{point.percentage:.1f}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.3f} %'
                }
            }
        },
        series: [{
            name: 'SCIS vs CCOS',
            colorByPoint: true,
            data: JSON.parse(dataTable)
        }]
    });
}

function getDataSCISvsCCOS() {
    $.ajax({
        url: baseUrl + '/KPIDashboard/GetSCISvsCCOS',
        type: 'GET',
        async: true,
        dataType: "json",
        success: function (data) {
            initSCISvsCCOS(data);
        }
    });
}

function initChangeLogChart(dataTable) {
    var a = JSON.parse(dataTable)
    var custom_arr1 = [];
    $.each(a, function (index) {
        custom_arr1.push(a[index].name);
    });
    Highcharts.chart('changeLogChart', {
        menuItems: false,
        credits: false,
        chart: {
            type: 'column'
        },
        title: {
            text: 'Monthly HS Code Replacement'
        },
        //subtitle: {
        //    text: 'Subtitle'
        //},
        xAxis: {
            categories: custom_arr1,
            crosshair: true
        },
        yAxis: {
            title: {
                text: 'TOTAL'
            }
        },
        //yAxis: {
        //    min: 0,
        //    title: {
        //        text: 'Rainfall (mm)'
        //    }
        //},
        //tooltip: {
        //    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
        //    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
        //        '<td style="padding:0"><b>{point.y}</b></td></tr>',
        //    footerFormat: '</table>',
        //    shared: true,
        //    useHTML: true
        //},
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [{
            showInLegend: false,
            name: 'Total',
            colorByPoint: true,
            data: JSON.parse(dataTable)
        },
        {
            showInLegend: false,
            name: 'Total',
            colorByPoint: true,
            type: 'spline',
            lineWidth: 2,
            lineColor: Highcharts.getOptions().colors[3],
            data: JSON.parse(dataTable)
        }]
    });
}

function getDataChangeLogChart() {
    $.ajax({
        url: baseUrl + '/KPIDashboard/GetChangeLogChart',
        type: 'GET',
        async: true,
        dataType: "json",
        success: function (data) {
            initChangeLogChart(data);
        }
    });
}

function initChangeLogChartNewMapping(dataTable) {
    var a = JSON.parse(dataTable)
    var custom_arr1 = [];
    $.each(a, function (index) {
        custom_arr1.push(a[index].name);
    });
    Highcharts.chart('changeLogChartNewMapping', {
        menuItems: false,
        credits: false,
        //chart: {
        //    type: 'column'
        //},
        title: {
            text: 'Monthly New Part Number Created'
        },
        //subtitle: {
        //    text: 'Subtitle'
        //},
        xAxis: {
            categories: custom_arr1,
            crosshair: true
        },
        yAxis: {
            title: {
                text: 'TOTAL'
            }
        },
        //yAxis: {
        //    min: 0,
        //    title: {
        //        text: 'Rainfall (mm)'
        //    }
        //},
        //tooltip: {
        //    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
        //    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
        //        '<td style="padding:0"><b>{point.y}</b></td></tr>',
        //    footerFormat: '</table>',
        //    shared: true,
        //    useHTML: true
        //},
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [{
            showInLegend: false,
            name: 'Total',
            colorByPoint: true,
            type: 'column',
            data: JSON.parse(dataTable)
        },
        {
            showInLegend: false,
            name: 'Total',
            colorByPoint: true,
            type: 'spline',
            lineWidth: 2,
            lineColor: Highcharts.getOptions().colors[3],
            data: JSON.parse(dataTable)
        }]
    });
}

function getDataChangeLogChartNewMapping() {
    $.ajax({
        url: baseUrl + '/KPIDashboard/GetChangeLogChartNewMapping',
        type: 'GET',
        async: true,
        dataType: "json",
        success: function (data) {
            initChangeLogChartNewMapping(data);
        }
    });
}
