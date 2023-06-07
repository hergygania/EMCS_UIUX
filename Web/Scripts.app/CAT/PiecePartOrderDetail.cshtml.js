var $table = $('#table-piece-partorder-detail');
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
            [
                { field: 'RefPartNo', title: 'Ref. Part Number', halign: 'center', rowspan: 2, align: 'left', sortable: true},
                { field: 'PartNo', title: 'Part Number', halign: 'center', rowspan: 2, align: 'left', sortable: true},
                { field: 'Model', title: 'Model', rowspan: 2, halign: 'center', align: 'left', sortable: true},
                { field: 'Prefix', title: 'Prefix', halign: 'center', rowspan: 2, align: 'left', sortable: true },
                { field: 'SMCS', title: 'SMCS', halign: 'center', rowspan: 2, align: 'left', sortable: true },
                { field: 'Component', title: 'Component', rowspan: 2, halign: 'center', align: 'left', sortable: true },
                { field: 'MOD', title: 'MOD', rowspan: 2, halign: 'center', align: 'center', sortable: true },
                { field: 'Status', title: 'Status', halign: 'center', rowspan: 2, align: 'left', sortable: true },
                { field: 'Store', title: 'Store', rowspan: 2, halign: 'center', align: 'left', sortable: true },
                { field: 'SOS', title: 'SOS', halign: 'center', rowspan: 2, align: 'left', sortable: true},
                { field: 'KAL', title: 'KAL', rowspan: 2, halign: 'center', align: 'left', sortable: true},
                { field: 'DetailSuply', title: 'DETAIL SUPPLY', halign: 'center', align: 'center', colspan: 8, sortable: false},
                { field: 'NextAllocation', title: 'NEXT ALLOCATION', halign: 'center', align: 'center', colspan: 6, sortable: false },
            ],
            [
                { field: 'UsedSN', title: 'Used SN', halign: 'center', align: 'left', sortable: true },
                { field: 'EquipmentNo', title: 'Equipment No', halign: 'center', align: 'center', sortable: true },
                { field: 'Customer_Spuly', title: 'Customer', halign: 'center', align: 'left', sortable: true },
                { field: 'StoreSuppliedDate', title: 'Store Supplied Date', halign: 'center', align: 'center', sortable: true, formatter: dateFormatterCAT },
                { field: 'ReconditionedWO', title: 'Reconditioned WO', halign: 'center', align: 'left', sortable: true },
                { field: 'SaleDoc', title: 'Sales Doc', halign: 'center', align: 'left', sortable: true },
                { field: 'ReturnDoc', title: 'Return Doc', halign: 'center', align: 'left', sortable: true, switchable: false },
                { field: 'WCSL', title: 'WCSL', halign: 'center', align: 'left', sortable: true, switchable: false },

                { field: 'PONumber', title: 'PO Number', halign: 'center', align: 'left', sortable: true },
                { field: 'Schedule', title: 'Schedule', halign: 'center', align: 'left', sortable: true, formatter: dateFormatterCAT },
                { field: 'UnitNoSN', title: 'Unit No', halign: 'center', align: 'left', sortable: true },
                { field: 'SerialNo', title: 'Serial Number', halign: 'center', align: 'left', sortable: true },
                { field: 'Location', title: 'Location', halign: 'center', align: 'left', sortable: true },
                { field: 'Customer', title: 'Customer', halign: 'center', align: 'left', sortable: true },
            ]
        ]
    });
    $("[name=refresh]").css('display', 'none');

    $('#btn-filter').click(function () {
        window.pis.table({
            objTable: $table,
            urlSearch: '/cat/PiecePartOrderDetailPage',
            urlPaging: '/cat/PiecePartOrderDetailPageXt',
            searchParams: {             
                DateFilter: $('#DateFilter').val(),
                ref_part_no: $('#ref_part_no').val(),
                model: $('#model').val(),
                prefix: $('#prefix').val(),
                smcs: $('#smcs').val(),
                component: $('#component').val(),
                mod: $('#mod').val()            
            },
            dataHeight: 412,
            autoLoad: true
        });
        //$('.fixed-table-body-columns').css('display', 'block');
    });

    $('#btn-clear').click(function () {
        clearScreen();
    });   

    $(".downloadExcel").click(function () {
        enableLink(false);
        $.ajax({
            url: "DownloadPiecePartOrderDetail",
            type: 'GET',
            data: {
                DateFilter: $('#DateFilter').val(),
                ref_part_no: $('#ref_part_no').val(),
                model: $('#model').val(),
                prefix: $('#prefix').val(),
                smcs: $('#smcs').val(),
                component: $('#component').val(),
                mod: $('#mod').val()  
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