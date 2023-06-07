var $table = $('#ExportProblemList');

$(function () {
    $table.treegrid({
        url: '/EMCS/GetExportProblemList',
        method: 'GET',
        idField: 'Id',
        pageSize: 1,
        pageList: [2, 10, 20],
        nowrap: false,
        treeField: 'Name',
        animate: true,
        showFooter: false,
        lines: false,
        columns: [[
            { field: 'Name', title: 'Category / Case / Reason', width: 550, halign: 'center' },
            {
                field: 'Impact', title: 'Case Impact', width: 300, halign: 'center', align: 'left',
                formatter: function (data, row, index) {
                    if (data === "-") {
                        return "";
                    }
                    return data;
                }
            },
            {
                field: 'TotalReason', title: 'Total Reason', nowrap: false, width: 100, halign: 'center', align: 'center',
                formatter: function (data, row, index) {
                    if (data === 0) {
                        return "";
                    }
                    return data;
                }
            },
            {
                field: 'TotalCases', title: 'Total Cases', nowrap: false, width: 100, halign: 'center', align: 'center',
                formatter: function (data, row, index) {
                    if (data === 0) {
                        return "";
                    }
                    return data;
                }
            },
            {
                field: 'TotalCategory', title: 'Total Problem<br> Category', nowrap: false, width: 100, halign: 'center', align: 'center',
                formatter: function (data, row, index) {
                    if (data === 0) {
                        return "";
                    }
                    return data;
                }
            },
            {
                field: "TotalCategoryPercentage", title: "Percentage Problem<br> Category (%)", nowrap: false, width: 110, halign: 'center', align: 'center',
                formatter: function (data, row, index) {
                    var all = $table.treegrid("getData");
                    var total = 0;
                    var totalProblem = 0;
                    $.each(all, function (idx, val) {
                        total += parseFloat(val.TotalCategoryPercentage);
                        totalProblem += parseFloat(val.TotalCategory);
                    });

                    if (data !== 100) {
                        if (row.TotalCategory === 0 && row.Name !== "Result") {
                            return "0";
                        }
                        else {
                            if (row.Name !== "Result" && row.Name !== "Grand Total") {
                                return parseFloat(row.TotalCategoryPercentage);
                            } else if (row.Name === "Grand Total") {
                                return Math.round(total);
                            } else if (row.Name === "Result") {
                                if (totalProblem > 20) {
                                    return "GOOD";
                                } else if (totalProblem < 20 && totalProblem > 10) {
                                    return "POOR";
                                } else if (totalProblem <= 10 && totalProblem >= 1) {
                                    return "POOR";
                                } else {
                                    return "EXCELENT";
                                }
                            } else {
                                return row.TotalCategoryPercentage;
                            }
                        }
                    }
                    return data;
                },
                onLoadSuccess: function () {

                }
            }
        ]],
        onLoadSuccess: function (data1, data2) {
            //var totalProblem = data2.Footer[0].TotalCategory;
            $(this).treegrid("collapseAll");
        },
        onBeforeLoad: function (row, param) {
            var startDate = $('#inp-start-date').val() === null || $('#inp-start-date').val() === '' ? '' : moment($('#inp-start-date').val()).format('YYYY-MM-DD');
            var endDate = $('#inp-end-date').val() === null || $('#inp-end-date').val() === '' ? '' : moment($('#inp-end-date').val()).format('YYYY-MM-DD');

            param.startDate = startDate;
            param.endDate = endDate;
            return param;
        }
    });

    $(".datagrid-wrap").css("width", "100%");

    $('.cal-start-date').click(function () {
        $('#inp-start-date').focus().datepicker({
            format: "mm/DD/YYYY",
            autoclose: true
        });
    });
    $('.cal-end-date').click(function () {
        $('#inp-end-date').focus().datepicker({
            format: "mm/DD/YYYY",
            autoclose: true
        });
    });
    getProblemByCategory();
});

function searchDataReport() {
    $table.treegrid('load', {
        StartDate: $('#inp-start-date').val() === null || $('#inp-start-date').val() === '' ? '' : moment($('#inp-start-date').val()).format('YYYY-MM-DD'),
        EndDate: $('#inp-end-date').val() === null || $('#inp-end-date').val() === '' ? '' : moment($('#inp-end-date').val()).format('YYYY-MM-DD')
    });
}

function exportDataReport() {
    var startDate = $('#inp-start-date').val() === null || $('#inp-start-date').val() === '' ? '' : moment($('#inp-start-date').val()).format('YYYY-MM-DD');
    var endDate = $('#inp-end-date').val() === null || $('#inp-end-date').val() === '' ? '' : moment($('#inp-end-date').val()).format('YYYY-MM-DD');
    window.open('/EMCS/DownloadProblemAnalysis?StartDate=' + startDate + '&EndDate=' + endDate, '_blank');
}

function getProblemByCategory() {
    var startDate = $('#inp-start-date').val() === null || $('#inp-start-date').val() === '' ? '' : moment($('#inp-start-date').val()).format('YYYY-MM-DD');
    var endDate = $('#inp-end-date').val() === null || $('#inp-end-date').val() === '' ? '' : moment($('#inp-end-date').val()).format('YYYY-MM-DD');

    $.ajax({
        url: 'GetProblemByCategory?StartDate=' + startDate + '&EndDate=' + endDate,
        success: function (data) {
            var byCategory = [{
                name: 'Category',
                colorByPoint: true,
                data: []
            }];

            $.each(data, function (i, data) {
                byCategory[0].data.push({
                    name: data.Category,
                    y: data.TotalCategoryPercentage * 100,
                    sliced: true
                });
            });

            var ProblemByCategoryChart = Highcharts.chart('chart_container', {
                chart: {
                    type: 'pie',
                    options3d: {
                        enabled: true,
                        alpha: 45,
                        beta: 0
                    }
                },
                colors: ['#ffca22', '#9dd45d', '#05beff', '#ff696a', '#c63bff'],
                title: {
                    text: 'Problem by Category'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.2f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        depth: 35,
                        dataLabels: {
                            enabled: true,
                            format: '{point.name}'
                        }
                    }
                },
                credits: {
                    enabled: false
                },
                series: byCategory
            });
            ProblemByCategoryChart.reflow();
        }
    });
}