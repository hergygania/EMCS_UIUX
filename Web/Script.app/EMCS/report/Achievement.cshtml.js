var $table = $('#tbl-achievement');
var chartSpeed;
var Cycle = '';
var _getChartAchievement;

var columns = [
    {
        field: "Cycle",
        title: "Cycle",
        sortable: true,
        rownspan: 2,
        align: "left",
        class: "text-nowrap"
    },{
        field: "Target",
        title: "Target",
        sortable: true,
        rownspan: 2,
        align: "left",
        class: "text-nowrap"
    }, {
        field: "Actual",
        title: "Actual (avg)",
        sortable: true,
        rownspan: 2,
        align: "left",
        class: "text-nowrap"
    },{
        field: "Achieved",
        title: "Achieved",
        sortable: true,
        rownspan: 2,
        align: "left",
        class: "text-nowrap"
    }, {
        field: "TotalData",
        title: "Total Data",
        sortable: true,
        rownspan: 2,
        align: "left",
        class: "text-nowrap"
    },{
        field: "Achievement",
        title: "Achievement (%)",
        sortable: true,
        rownspan: 2,
        align: "left",
        class: "text-nowrap"
    }
]


$(function () {
    AchievementSearch();
});

function ChartAchievement(Cycle) {
    _getChartAchievement = $.ajax({
        url: "/emcs/RAchievementListPage",
        data: "StartDate=" + moment($("#inp-start-date").val()).format('YYYY-MM-DD') + "&EndDate=" + moment($("#inp-end-date").val()).format('YYYY-MM-DD'),
        async: true,
        success: function (data) {
            
            var totalAchievement = new Array();
            var dataChart = data.Data.result.filter((c) => {
                return c.Cycle == Cycle;
            }) || [];

            if (dataChart.length == 0) {
                Cycle = "Summary";
                dataChart[0] = { Achieved: 0, TotalData: 0 }
                data.Data.result.forEach((c) => {
                    dataChart[0].Achieved += c.Achieved;
                    dataChart[0].TotalData += c.TotalData;
                });
            }

            totalAchievement.push(
                (dataChart[0].Achieved / dataChart[0].TotalData * 100)
            );
            
            var gaugeOptions = {
                chart: {
                    type: 'solidgauge',
                    backgroundColor: "#ffffff",
                    
                },
                title: null,
                //height: "20px",
                pane: {
                    center: ['50%', '50%'],
                    size: '100%',
                    startAngle: -90,
                    endAngle: 90,
                    background: {
                        backgroundColor:
                            //Highcharts.defaultOptions.legend.backgroundColor || '#EEE',
                          Highcharts.theme && Highcharts.theme.background2 || '#EEE',
                        
                        outerRadius: '102%',
                        shape: 'arc'
                    }
                },
                tooltip: {
                    enabled: true
                },
                // the value axis
                yAxis: {
                    stops: [
                        [0.1, '#DF5353'], // green
                        [0.49, '#DF5353'], // green
                        [0.5, '#DDDF0D'], // yellow
                        [0.79, '#DDDF0D'], // yellow
                        [0.8, '#55BF3B'] // red
                    ],
                    lineWidth: 1,
                    minorTickInterval: null,
                    tickAmount: 2
                },

                plotOptions: {
                    solidgauge: {
                        dataLabels: {
                            y: 5,
                            borderWidth: 0,
                            useHTML: true
                        }
                    }
                }
            };

            // The speed gauge
            chartSpeed = Highcharts.chart('container-speed', Highcharts.merge(gaugeOptions, {
                yAxis: {
                    min: 0,
                    max: 100,

                    /* title: {
                        text: 'Speed'
                    } */
                },

                credits: {
                    enabled: false
                },

                series: [{
                    name: 'Achievement',
                    data: totalAchievement,
                    dataLabels: {
                        format:
                            '<div style="text-align:center">' +
                            '<span style="font-size:25px">'+ Cycle +'</span><br/>' +
                            '</div>'
                    },
                    tooltip: {
                        valueSuffix: '%'
                    }
                }]

            }));
        }
    });
}

function AchievementSearch() {

    ChartAchievement(Cycle);

    $table.bootstrapTable({
        columns: columns,
        cache: false,
        pagination: false,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.hasniToolbar',
        toolbarAlign: 'right',
        onClickRow: clickRowEvent,
        sidePagination: 'server',
        showColumns: false,
        showRefresh: false,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/EMCS/RAchievementListPage?StartDate=' + moment($("#inp-start-date").val()).format('YYYY-MM-DD') + '&EndDate=' + moment($("#inp-end-date").val()).format('YYYY-MM-DD'),
        urlPaging: '/EMCS/RAchievementPageXt?StartDate=' + moment($("#inp-start-date").val()).format('YYYY-MM-DD') + '&EndDate=' + moment($("#inp-end-date").val()).format('YYYY-MM-DD'),
        autoLoad: true
    });

}

function exportDataReport() {
    var startDate = $('#inp-start-date').val() === null || $('#inp-start-date').val() === '' ? '' : moment($('#inp-start-date').val()).format('YYYY-MM-DD');
    var endDate = $('#inp-end-date').val() === null || $('#inp-end-date').val() === '' ? '' : moment($('#inp-end-date').val()).format('YYYY-MM-DD');
    window.open('/EMCS/DownloadExportAchievement?StartDate=' + startDate + '&EndDate=' + endDate, '_blank');
}

function clickRowEvent(row, $element) {
    ChartAchievement(row.Cycle);
    return selectRow(row, $element);
}