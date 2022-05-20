var $table = $('#table-out-oldcore-sum');
setTimeout(function () { $(".model-parent").find(".select2").addClass("width30"); }, 300);

$(function () {
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
        showRefresh: true,
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
        onAll: function (name, args) {
            $('[data-field="OutOldCore0day"]').css("background-color", "green");
            $('[data-field="OutOldCore11day"]').css("background-color", "rgba(255, 255, 0, 0.82)");
            $('[data-field="OutOldCore21day"]').css("background-color", "red");
        },
        columns: [
            { field: 'Store', title: 'Store', halign: 'center', width: '180px', align: 'left', sortable: true, switchable: false },
            //{ field: 'Location', title: 'Location', halign: 'center', width: '120px', align: 'left', sortable: true, switchable: false },
            { field: 'OutOldCore0day', title: 'Outstanding Old Core Elapsed 0 - 10 DAYS', width: '250px', halign: 'center', align: 'center', sortable: true, switchable: false },
            { field: 'OutOldCore11day', title: 'Outstanding Old Core Elapsed 11 - 20 DAYS', halign: 'center', width: '250px', align: 'center', sortable: true },
            { field: 'OutOldCore21day', title: 'Outstanding Old Core Elapsed >= 21 DAYS', width: '250px', halign: 'center', align: 'center', sortable: true }
        ]
    });
    $("[name=refresh]").css('display', 'none');

    $('#btn-filter').click(function () {
        window.pis.table({
            objTable: $table,
            urlSearch: '/cat/OutstandingOldCoreSummaryPage',
            urlPaging: '/cat/OutstandingOldCoreSummaryPageXt',
            searchParams: {             
                storeid: $('#storeid').val(),
                DateFilter: $("#DateFilter").val()
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
            url: "DownloadOutstandingOldCoreSummary",
            type: 'GET',
            data: {
                storeid: $('#storeid').val(),
                DateFilter: $("#DateFilter").val()
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
    $('#storeid').val('', 'ALL').change();
    $('#locationid').val('', 'ALL').change();
}