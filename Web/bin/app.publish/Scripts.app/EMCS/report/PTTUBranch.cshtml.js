//$('#div-report-pptu').hide();
//$('#btn-show-report').on('click', function () {
//    $('#div-report-pptu').show(1000);
//});

var startDate = '';
var endDate = '';

$(function () {
    $('.cal-start-date').click(function () {
        $('#inp-start-date').focus();
    });
    $('#inp-start-date').focus().datepicker({
        format: "mm/yyyy",
        viewMode: "months",
        minViewMode: "months",
        autoclose: true
    //}).on('changeMonth', function (e) {
    //    $('#inp-end-date').datepicker('setDate', '');
    //    $('#inp-end-date').datepicker('setStartDate', moment(e.date).format('MM/YYYY'));
    //    $('#inp-end-date').datepicker('setEndDate', '12/' + moment(e.date).format('YYYY'));
    });
    $('#inp-start-date').datepicker('setDate', '01/2019');

    $('.cal-end-date').click(function () {
        $('#inp-end-date').focus();
    });
    $('#inp-end-date').focus().datepicker({
        format: "mm/yyyy",
        viewMode: "months",
        minViewMode: "months",
        autoclose: true
    //}).on('changeMonth', function (e) {
    //    $('#inp-start-date').datepicker('setEndDate', moment(e.date).format('MM/YYYY'));
    });
    $('#inp-end-date').datepicker('setDate', '12/2019');

    var columns = [
        [{
            field: "RowNumber",
            title: "No",
            rowspan: 2,
            align: 'right',
            valign: "middle"
        }, {
            field: "Name",
            title: 'PPTU BRANCH',
            rowspan: 2,
            sortable: true,
            align: "left",
            valign: "middle",
            width: "100"
        }, {
            field: "",
            title: "TOTAL PEB",
            colspan: 12,
            sortable: true,
            align: "center",
        }, {
            field: "TotalPEB",
            title: "TOTAL",
            rowspan: 2,
            sortable: true,
            align: "right",
            valign: "middle",
            //formatter: function (data, row, index) {
            //    return row['januari'] + row['februari'] + row['maret'] + row['april'] + row['mei'] + row['juni'] + row['juli'] + row['agustus'] + row['september'] + row['oktober'] + row['november'] + row['desember'];
            //}

        }],
        [{
            field: "TotalPebJan",
            title: "JAN",
            sortable: true,
            filterControl: true,
            align: "right"
        }, {
            field: "TotalPebFeb",
            title: "FEB",
            sortable: true,
            filterControl: true,
            align: "right"
        }, {
            field: "TotalPebMar",
            title: "MAR",
            sortable: true,
            filterControl: true,
            align: "right"
        }, {
            field: "TotalPebApr",
            title: "APR",
            sortable: true,
            filterControl: true,
            align: "right"
        }, {
            field: "TotalPebMay",
            title: "MAY",
            sortable: true,
            filterControl: true,
            align: "right"
        }, {
            field: "TotalPebJun",
            title: "JUN",
            sortable: true,
            filterControl: true,
            align: "right"
        }, {
            field: "TotalPebJul",
            title: "JUL",
            sortable: true,
            filterControl: true,
            align: "right"
        }, {
            field: "TotalPebAug",
            title: "AUG",
            sortable: true,
            filterControl: true,
            align: "right"
        }, {
            field: "TotalPebSep",
            title: "SEP",
            sortable: true,
            filterControl: true,
            align: "right"
        }, {
            field: "TotalPebOct",
            title: "OCT",
            sortable: true,
            filterControl: true,
            align: "right"
        }, {
            field: "TotalPebNov",
            title: "NOV",
            sortable: true,
            filterControl: true,
            align: "right"
        }, {
            field: "TotalPebDec",
            title: "DEC",
            sortable: true,
            filterControl: true,
            align: "right"
        }
        ]
    ]

    $("#tbl-pttu-branch").bootstrapTable({
        //url: "/Json/Transaction/PPTU.json",
        columns: columns,
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
    });

    //searchDataReport();
    initAverageTable();
});

function initAverageTable() {
    var columns = [
        [{
            title: 'DESCRIPTION',
            rowspan: 2,
            width: "200",
            align: "center"
        }, {
            field: "",
            title: "TOTAL PEB",
            colspan: 12,
            align: "center"
        }, {
            field: "",
            title: "GRAND TOTAL",
            rowspan: 2,
            align: "center"
        }],
        [{
            field: "",
            title: "JAN",
            align: "center"
        }, {
            field: "",
            title: "FEB",
            align: "center"
        }, {
            field: "",
            title: "MAR",
            align: "center"
        }, {
            field: "",
            title: "APR",
            align: "center"
        }, {
            field: "",
            title: "MAY",
            align: "center"
        }, {
            field: "",
            title: "JUN",
            align: "center"
        }, {
            field: "",
            title: "JUL",
            align: "center"
        }, {
            field: "",
            title: "AUG",
            align: "center"
        }, {
            field: "",
            title: "SEP",
            align: "center"
        }, {
            field: "",
            title: "OCT",
            align: "center"
        }, {
            field: "",
            title: "NOV",
            align: "center"
        }, {
            field: "",
            title: "DEC",
            align: "center"
        }]
    ]

    $("#tbl-pttu-average").bootstrapTable({
        columns: columns,
        cache: false,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
    });
}

function searchDataReport() {
    //if ($('#inp-start-date').val() !== undefined && $('#inp-start-date').val() !== null && $('#inp-start-date').val() !== '') {
        var date = $('#inp-start-date').val().split('/');
        startDate = date[1] + '-' + date[0] + '-' + '01';
    //}
    //if ($('#inp-end-date').val() !== undefined && $('#inp-end-date').val() !== null && $('#inp-end-date').val() !== '') {
        var date = $('#inp-end-date').val().split('/');
        endDate = date[1] + '-' + date[0] + '-' + '01';
    //}

    var jan = 0, feb = 0, mar = 0, apr = 0, may = 0, jun = 0, jul = 0, aug = 0, sep = 0, oct = 0, nov = 0, dec = 0, total = 0;

    /* Category Table */
    $.ajax({
        url: '/EMCS/GetPTTUBranchList?StartDate=' + startDate + '&EndDate=' + endDate + '&Type=' + $('#jenisBarangCipl').val(),
        autoload: true,
        success: function (data) {
            var tableColumns = $("#tbl-pttu-branch").find('thead > tr > th > div.th-inner.sortable').get();
            tableColumns[0].innerHTML = $('#jenisBarangCipl').val() == 'Branch' ? 'PPTU BRANCH' : 'LOADING PORT';
            
            $.each(data.rows, function (i, data) {
                jan += data.TotalPebJan;
                feb += data.TotalPebFeb;
                mar += data.TotalPebMar;
                apr += data.TotalPebApr;
                may += data.TotalPebMay;
                jun += data.TotalPebJun;
                jul += data.TotalPebJul;
                aug += data.TotalPebAug;
                sep += data.TotalPebSep;
                oct += data.TotalPebOct;
                nov += data.TotalPebNov;
                dec += data.TotalPebDec;
                total += data.TotalPeb;
            });

            $("#tbl-pttu-branch").bootstrapTable('load', data);

            var totalRow =
                '<tr>' +
                '   <td align="center" colspan= "2" >Total</td>' +
                '    <td align="right">' + jan + '</td>' +
                '    <td align="right">' + feb + '</td>' +
                '    <td align="right">' + mar + '</td>' +
                '    <td align="right">' + apr + '</td>' +
                '    <td align="right">' + may + '</td>' +
                '    <td align="right">' + jun + '</td>' +
                '    <td align="right">' + jul + '</td>' +
                '    <td align="right">' + aug + '</td>' +
                '    <td align="right">' + sep + '</td>' +
                '    <td align="right">' + oct + '</td>' +
                '    <td align="right">' + nov + '</td>' +
                '    <td align="right">' + dec + '</td>' +
                '    <td align="right">' + total + '</td>' +
                '</tr >';

            $("#tbl-pttu-branch tbody").append(totalRow);
        }
    });

    /* AVG Table */
    $.ajax({
        url: '/EMCS/GetPTTUBranchAverage?StartDate=' + startDate + '&EndDate=' + endDate,
        autoload: true,
        success: function (data) {
            $("#tbl-pttu-average > tbody").html("");
            
            var q1 = jan + feb + mar;
            var q2 = apr + may + jun;
            var q3 = jul + aug + sep;
            var q4 = oct + nov + dec;
            var averageRows =
                '<tr>' +
                '   <td align="left">Total Per Month</td>' +
                '    <td align="right">' + jan + '</td>' +
                '    <td align="right">' + feb + '</td>' +
                '    <td align="right">' + mar + '</td>' +
                '    <td align="right">' + apr + '</td>' +
                '    <td align="right">' + may + '</td>' +
                '    <td align="right">' + jun + '</td>' +
                '    <td align="right">' + jul + '</td>' +
                '    <td align="right">' + aug + '</td>' +
                '    <td align="right">' + sep + '</td>' +
                '    <td align="right">' + oct + '</td>' +
                '    <td align="right">' + nov + '</td>' +
                '    <td align="right">' + dec + '</td>' +
                '    <td align="center" rowspan= "4" >' + total + '</td>' +
                '</tr >' +
                '<tr>' +
                '   <td align="left">Total In Quarter</td>' +
                '    <td align="right" colspan= "3" >' + q1 + '</td>' +
                '    <td align="right" colspan= "3" >' + q2 + '</td>' +
                '    <td align="right" colspan= "3" >' + q3 + '</td>' +
                '    <td align="right" colspan= "3" >' + q4 + '</td>' +
                '</tr >';
            
            $.each(data.avg, function (i, data) {
                var row =
                    '<tr>' +
                    '   <td align="left">' + data.Description + '</td>' +
                    '    <td align="right">' + data.Jan + '</td>' +
                    '    <td align="right">' + data.Feb + '</td>' +
                    '    <td align="right">' + data.Mar + '</td>' +
                    '    <td align="right">' + data.Apr + '</td>' +
                    '    <td align="right">' + data.May + '</td>' +
                    '    <td align="right">' + data.Jun + '</td>' +
                    '    <td align="right">' + data.Jul + '</td>' +
                    '    <td align="right">' + data.Aug + '</td>' +
                    '    <td align="right">' + data.Sep + '</td>' +
                    '    <td align="right">' + data.Oct + '</td>' +
                    '    <td align="right">' + data.Nov + '</td>' +
                    '    <td align="right">' + data.Dec + '</td>' +
                    '</tr >';
                averageRows += row;
            });

            averageRows += 
                '<tr>' +
                '   <td align="left">Monthly Average (YTD)</td>' +
                '    <td align="center" colspan= "13" >' + data.ytd_avg.YtdMonthlyAvg + '</td>' +
                '</tr >'+
                '<tr>' +
                '   <td align="left">Weekly Average (YTD)</td>' +
                '    <td align="center" colspan= "13" >' + data.ytd_avg.YtdWeeklyAvg + '</td>' +
                '</tr >'+
                '<tr>' +
                '   <td align="left">Daily Average (YTD)</td>' +
                '    <td align="center" colspan= "13" >' + data.ytd_avg.YtdDailyAvg + '</td>' +
                '</tr >';

            $("#tbl-pttu-average tbody").append(averageRows);
        }
    });
}

function exportDataReport() {
    if ($('#inp-start-date').val() !== undefined && $('#inp-start-date').val() !== null && $('#inp-start-date').val() !== '') {
        var date = $('#inp-start-date').val().split('/');
        startDate = date[1] + '-' + date[0] + '-' + '01';
    }
    if ($('#inp-end-date').val() !== undefined && $('#inp-end-date').val() !== null && $('#inp-end-date').val() !== '') {
        var date = $('#inp-end-date').val().split('/');
        endDate = date[1] + '-' + date[0] + '-' + '01';
    }
    window.open('/EMCS/DownloadPTTUBranch?StartDate=' + startDate + '&EndDate=' + endDate + '&Type=' + $('#jenisBarangCipl').val(), '_blank');
}
