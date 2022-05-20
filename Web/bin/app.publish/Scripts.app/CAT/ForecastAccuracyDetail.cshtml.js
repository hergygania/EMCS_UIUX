var $table = $('#table-fore-acc-detail');
setTimeout(function () { $(".model-parent").find(".select2").addClass("width30"); }, 300);

$(function () {
    $('.cal').click(function () {
        $('#year').focus();
    });
    $("#year").datepicker({
        format: " yyyy",
        viewMode: "years",
        minViewMode: "years"
    });

    clearScreen();
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');

    $table.bootstrapTable({
        cache: false,
        search: false,
        toolbar: '.toolbar',
        pagination: true,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: false,
        showRefresh: false,
        smartDisplay: false,
        pageSize: '5',
        showExport: false,
        exportTypes: ['excel'],
        exportOptions: {
            ignoreColumn: [0],
            fileName: 'file.xls'
        },
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        //fixedColumns:true,
        //fixedNumber:4,
        columns: [
            { field: 'Store', title: 'Store', halign: 'center',  align: 'left', sortable: true, switchable: false },
            { field: 'RefPartNumber', title: 'Part No.', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'Customer', title: 'Customer', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'Component', title: 'Component', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'Model', title: 'Model', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'Prefix', title: 'Prefix', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'Forecast', title: 'Forecast', halign: 'center', align: 'center', sortable: true, switchable: false },
            { field: 'Sales', title: 'Sales', halign: 'center', align: 'center', sortable: true },            
            { field: 'ForecastedSales', title: 'Forecasted Sales', halign: 'center', align: 'center', sortable: true },            
            { field: 'UnForecastedSales', title: 'Unforecasted Sales', halign: 'center', align: 'center', sortable: true }
        ]
    });
    $("[name=refresh]").css('display', 'none');

    $('#btn-filter').click(function () {
        window.pis.table({
            objTable: $table,
            urlSearch: '/cat/ForecastAccuracyDetailPage',
            urlPaging: '/cat/ForecastAccuracyDetailPageXt',
            searchParams: {             
                customer_id: $('#customer_id').val(),
                store_id: $('#store_id').val(),
                month_id: $('#month_id').val(),
                year: $('#year').val()
            },
            dataHeight: 412,
            autoLoad: true
        });
        $('.fixed-table-body-columns').css('display', 'block');
    });

    $('#btn-clear').click(function () {
        clearScreen();
    });    

    $(".downloadExcel").click(function () {
        enableLink(false);
        $.ajax({
            url: "DownloadForecastAccuracyDetail",
            type: 'GET',
            data: {
                customer_id: $('#customer_id').val(),
                store_id: $('#store_id').val(),
                month_id: $('#month_id').val(),
                year: $('#year').val()
            },
            success: function (guid) {
                enableLink(true);
                window.open('DownloadToExcel?guid=' + guid, '_blank');
            },
            cache: false,
        });
    });
});

function clearScreen() {
    $('#customer_id').val('', 'ALL').change();
    $('#store_id').val('', 'ALL').change();
    $('#month_id').val('', 'ALL').change();
    $('#year').val('');
}