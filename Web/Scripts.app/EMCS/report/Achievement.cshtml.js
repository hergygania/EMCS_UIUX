var $table = $('#tbl-achievement');
var chartSpeed;
var Cycle = '';

window.operateEvents = {
    'click .link': function (e, value, row, index) {
        //console.log(value);
        ChartAchievement(value);
        //alert(value);
        //window.location.href = "/emcs/UpdateCargo?CargoID=" + row.Id;
    }
};

$(function () {

    var columns = [
        {
            field: "Cycle",
            title: "Cycle",
            sortable: true,
            rownspan: 2,
            align: "left",
            class: "text-nowrap",
            events: operateEvents,
            formatter: function (data, row, index) {
                var btn = [];
                btn.push('<a href="#" class="link" title="Add">'+data+'</a>');
                return btn.join(' ');
            }
        },{
            field: "Target",
            title: "Target",
            sortable: true,
            rownspan: 2,
            align: "left",
            class: "text-nowrap"
        },{
            field: "Actual",
            title: "Actual (avg)",
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
        onClickRow: selectRow,
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
    
    ChartAchievement(Cycle);
    
});

function ChartAchievement(Cycle) {
    $.ajax({
        url: "/emcs/getPercentageAchievement",
        data: "StartDate=" + moment($("#inp-start-date").val()).format('YYYY-MM-DD') + "&EndDate=" + moment($("#inp-end-date").val()).format('YYYY-MM-DD') + "&Cycle=" + Cycle,
        async: false,
        success: function (data) {
            
            var totalAchievement = new Array();

            totalAchievement.push(data[0].TotAchievement);
            
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
                        [0.1, '#DDDF0D'], // green
                        [0.5, '#DDDF0D'], // yellow
                        [0.9, '#55BF3B'] // red
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
                            '<span style="font-size:25px">{y} %</span><br/>' +
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
    var startdate = $('#inp-start-date').val();
    var enddate = $('#inp-end_date').val();
    if (startdate !== null && startdate !== '' && enddate !== null && enddate !== '') {
        ChartAchievement();
    }
}

function exportDataReport() {
    var startDate = $('#inp-start-date').val() === null || $('#inp-start-date').val() === '' ? '' : moment($('#inp-start-date').val()).format('YYYY-MM-DD');
    var endDate = $('#inp-end-date').val() === null || $('#inp-end-date').val() === '' ? '' : moment($('#inp-end-date').val()).format('YYYY-MM-DD');
    window.open('/EMCS/DownloadExportAchievement?StartDate=' + startDate + '&EndDate=' + endDate, '_blank');
}