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
    getExportValueMonthly($('#date1-total-export-monthly').val(), $('#TrendExportFilterExportType1').val());
});
$("#form-total-export-port").submit(function (e) {
    e.preventDefault();
    getExportValuePort($('#date1-total-export-port').val(), $('#TrendExportFilterExportType2').val());
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


function getExportValueMonthly(date1 = '', filter = '') {
    $.ajax({
        url: "/emcs/TotalExportMonthly",
        data: {
            searchCode: date1,
            searchName: filter
        },
        async: false,
        success: function (data) {
            var categories_monthly = [];
            var series_monthly_sales = [];
            var series_monthly_non_sales = [];

            //filter month
            $.each(data, function (i, e) {
                if (i == "January" || i == "February" || i == "March" || i == "April" || i == "May" || i == "June" || i == "July" ||
                    i == "August" || i == "September" || i == "October" || i == "November" || i == "December" || i == "Total")
                    categories_monthly.push(i);
            });

            //filter data sales
            $.each(data, function (i, e) {
                if (i == "JanuarySales" || i == "FebruarySales" || i == "MarchSales" || i == "AprilSales" || i == "MaySales" || i == "JuneSales" || i == "JulySales" ||
                    i == "AugustSales" || i == "SeptemberSales" || i == "OctoberSales" || i == "NovemberSales" || i == "DecemberSales" || i == "TotalSales")
                    series_monthly_sales.push(e);
            });

            //filter data non sales
            $.each(data, function (i, e) {
                if (i == "JanuaryNonSales" || i == "FebruaryNonSales" || i == "MarchNonSales" || i == "AprilNonSales" || i == "MayNonSales" || i == "JuneNonSales" || i == "JulyNonSales" ||
                    i == "AugustNonSales" || i == "SeptemberNonSales" || i == "OctoberNonSales" || i == "NovemberNonSales" || i == "DecemberNonSales" || i == "TotalNonSales")
                    series_monthly_non_sales.push(e);
            });

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
                series: [
                    {
                        name: "Sales",
                        data: series_monthly_sales
                    },
                    {
                        name: "Non Sales",
                        data: series_monthly_non_sales
                    }
                ]
            });
            monthly.reflow();
        }
    });
   
}

function getExportValuePort(date1 = '', filter = '') {
    $.ajax({
        url: "/emcs/TotalExportPort",
        data: {
            searchCode: date1,
            searchName: filter
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
            var series_port_sales = [];
            var series_port_non_sales = [];

            $.each(data, function (i, e) {
                categories_port.push(e.PortOfLoading);
                series_port_sales.push(e.TotalSales);
                series_port_non_sales.push(e.TotalNonSales);
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
                series: [
                    {
                        name: "Sales",
                        data: series_port_sales
                    },
                    {
                        name: "Non Sales",
                        data: series_port_non_sales
                    }
                ]
            });
            loadingPort.reflow();
        }
    });

}
