$(function () {
    $(".yearpicker").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years",
        startDate: new Date('2009'),
        endDate: new Date(),
        autoClose: true
    });

    $('#i-total-export-monthly').on('click', function (e) {
        $("#open-total-export-monthly").toggle("show");
    });

    $('#i-total-export-port').on('click', function (e) {
        $("#open-total-export-port").toggle("show");
    });
});

$(document).ready(function () {
    getExportValueMonthly();
    getExportValuePort();
});

$("#form-total-export-monthly").submit(function (e) {
    e.preventDefault();
    getExportValueMonthly($('#date1-total-export-monthly').val());
});
$("#form-total-export-port").submit(function (e) {
    e.preventDefault();
    getExportValuePort($('#date1-total-export-port').val());
});
$('#btn-total-export-monthly-download').on('click', function () {
    var year = $('#date1-export-type').val();
    var exporttype = $('#slc-export-type').val();
    window.open('/EMCS/DownloadExportTransactionMonthly?searchId=' + year, '_blank');
})
$('#btn-total-export-port-download').on('click', function () {
    var year = $('#date1-export-type').val();
    var exporttype = $('#slc-export-type').val();
    window.open('/EMCS/DownloadExportTransactionPort?searchId=' + year, '_blank');
})


function getExportValueMonthly(date1 = '') {
    $.ajax({
        url: "/emcs/TotalExportMonthly",
        data: {
            searchCode: date1,
        },
        async: false,
        success: function (data) {
            var categories_monthly = [];
            var series_monthly = [];
            $.each(data, function (i, e, n) {
                n++;
                categories_monthly.push(i);
                series_monthly.push(e);
            })

            var monthly = Highcharts.chart('container', {
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Total Export Value (Monthly)'
                },
                subtitle: {
                    text: 'Periode ' + $('#date1-total-export-monthly').val()
                },
                xAxis: {
                    categories: categories_monthly,
                    title: {
                        text: null
                    }
                },
                tooltip: {
                    valueSuffix: ' case'
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    x: -40,
                    y: 80,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                    shadow: true
                },
                credits: {
                    enabled: false
                },
                series: [{
                    data: series_monthly
                }]
            });
            monthly.reflow();
        }
    });
   
}

function getExportValuePort(date1 = '') {
    $.ajax({
        url: "/emcs/TotalExportPort",
        data: {
            searchCode: date1,
        },
        async: false,
        success: function (data) {
            console.log(data);
            if (data.length === 0) {
                var title2 = "No Data Available";
            } else {
                var title2 = "TOTAL EXPORT VALUE (LOADING PORT)"
            }
            
            var categories_port = [];
            var series_port = [];

            $.each(data, function (i, e) {
                categories_port.push(e.PortOfLoading);
                series_port.push(e.Total);
            })

            var loadingPort = Highcharts.chart('container2', {
                chart: {
                    type: 'bar'
                },
                title: {
                    text: title2
                },
                subtitle: {
                    text: 'Periode ' + $('#date1-total-export-port').val()
                },
                xAxis: {
                    categories: categories_port,
                    title: {
                        text: null
                    }
                },
                tooltip: {
                    valueSuffix: ' case'
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    x: -40,
                    y: 80,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                    shadow: true
                },
                credits: {
                    enabled: false
                },
                series: [{
                    data: series_port
                }]
            });
            loadingPort.reflow();
        }
    });

}
