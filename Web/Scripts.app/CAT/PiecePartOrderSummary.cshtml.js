var $table = $('#table-piece-partorder-sum');
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
        columns: [
            { field: 'RefPartNo', title: 'Ref. Part Number', halign: 'center', width: '150px', align: 'left', sortable: true, switchable: false },
            { field: 'Model', title: 'Model', width: '90px', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'Prefix', title: 'Prefix', halign: 'center', width: '120px', align: 'left', sortable: true },
            { field: 'SMCS', title: 'SMCS', halign: 'center', width: '130px', align: 'left', sortable: true },
            { field: 'Component', title: 'Component', width: '120px', halign: 'center', align: 'left', sortable: true },
            { field: 'MOD', title: 'MOD', width: '70px', halign: 'center', align: 'center', sortable: true },
            { field: 'Last12mTrans', title: 'Last 12m Transaction', halign: 'center', width: '150px', align: 'center', sortable: true, switchable: false },
            { field: 'NeedToRebuid', title: 'Need To Rebuid', width: '100px', halign: 'center', align: 'center', sortable: true }
        ]
    });
    $("[name=refresh]").css('display', 'none');

    $('#btn-filter').click(function () {
        window.pis.table({
            objTable: $table,
            urlSearch: '/cat/PiecePartOrderSummaryPage',
            urlPaging: '/cat/PiecePartOrderSummaryPageXt',
            searchParams: {             
                DateFilter: $('#DateFilter').val(),
                ref_part_no: $('#ref_part_no').val(),
                model: $('#model').val(),
                prefix: $('#prefix').val(),
                smcs: $('#smcs').val(),
                component: $('#component').val(),
                mod: $('#mod').val(),                       
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
            url: "DownloadPiecePartOrderSummary",
            type: 'GET',
            data: {
                DateFilter: $('#DateFilter').val(),
                ref_part_no: $('#ref_part_no').val(),
                model: $('#model').val(),
                prefix: $('#prefix').val(),
                smcs: $('#smcs').val(),
                component: $('#component').val(),
                mod: $('#mod').val(),
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
    $('#ref_part_no').val('');
    $('#model').val('');
    $('#prefix').val('');
    $('#smcs').val('');
    $('#component').val('');
    $('#mod').val('');    
}