var $table = $('#table-out-oldcore-detail');
setTimeout(function () { $(".model-parent").find(".select2").addClass("width30"); }, 300);

$(function () {
    clearScreen();
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');

    $('.cal').click(function () {
        $('#EntryDate').focus();
    });

    var customer = new mySelect2({
        id: 'CustomerID',
        url: '/Picker/SelectToCustomer'
    }).load();

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
                { field: 'Store', title: 'Store', halign: 'center', align: 'left', rowspan: 2, sortable: true, },
                { field: 'SOS', title: 'SOS', halign: 'center', align: 'left', rowspan: 2, sortable: true, },
                { field: 'PartNo', title: 'Part Number', halign: 'center', align: 'left', rowspan: 2, sortable: true, },
                { field: 'KAL', title: 'KAL', halign: 'center', align: 'left', rowspan: 2, sortable: true, },
                { field: 'Model', title: 'Model', halign: 'center', align: 'left', rowspan: 2, sortable: true },
                { field: 'Prefix', title: 'Prefix', halign: 'center', align: 'left', rowspan: 2, sortable: true },
                { field: 'Component', title: 'Component', halign: 'center', align: 'left', rowspan: 2, sortable: true },
                { field: 'DetailSuply', title: 'DETAIL SUPPLY', halign: 'center', align: 'center', colspan: 8,  sortable: false },
                { field: 'NextAllocation', title: 'NEXT ALLOCATION', halign: 'center', align: 'center', colspan: 6, sortable: false },
                { field: 'Section', title: 'Section', halign: 'center', align: 'left', rowspan: 2, sortable: true },
                { field: 'RebuildOption', title: 'Rebuild Option', halign: 'center', align: 'left', rowspan: 2, sortable: true },
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
        var _starDate, _endDate;
        if ($('#EntryDate').val() != '') {
            _starDate = $('#EntryDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
            _endDate = $('#EntryDate').data('daterangepicker').endDate.format('MM/DD/YYYY');
        }
        window.pis.table({
            objTable: $table,
            urlSearch: '/cat/OutstandingOldCoreDetailPage',
            urlPaging: '/cat/OutstandingOldCoreDetailPageXt',
            searchParams: {             
                store_id: $('#StoreID').val(),
                sos_id: $('#SOSID').val(),
                part_no: $('#PartNo').val(), 
                kal: $('#KAL').val(), 
                model: $('#Model').val(), 
                component: $('#Component').val(), 
                sn_unit: $('#SNUnit').val(), 
                customer_id: $('#CustomerID').val(), 
                prefix: $('#Prefix').val(), 
                store_supplied_date_start: _starDate, 
                store_supplied_date_end: _endDate,
                reconditioned_wo: $('#ReconditionedWO').val(),
                doc_c: $('#DocC').val(),
                doc_r: $('#DocR').val(),
                wcsl: $('#WCSL').val(),
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
        var _starDate, _endDate;
        if ($('#EntryDate').val() != '') {
            _starDate = $('#EntryDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
            _endDate = $('#EntryDate').data('daterangepicker').endDate.format('MM/DD/YYYY');
        }
        $.ajax({
            url: "DownloadOutstandingOldCoreDetail",
            type: 'GET',
            data: {
                store_id: $('#StoreID').val(),
                sos_id: $('#SOSID').val(),
                part_no: $('#PartNo').val(),
                kal: $('#KAL').val(),
                model: $('#Model').val(),
                component: $('#Component').val(),
                sn_unit: $('#SNUnit').val(),
                customer_id: $('#CustomerID').val(),
                prefix: $('#Prefix').val(),
                store_supplied_date_start: _starDate,
                store_supplied_date_end: _endDate,
                reconditioned_wo: $('#ReconditionedWO').val(),
                doc_c: $('#DocC').val(),
                doc_r: $('#DocR').val(),
                wcsl: $('#WCSL').val(),
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
    $('#StoreID').val('', 'ALL').change();
    $('#SOSID').val('', 'ALL').change();
    $('#PartNo').val('');
    $('#KAL').val('');
    $('#Model').val('');
    $('#Component').val('');
    $('#SNUnit').val('');
    $('#CustomerID').val('', 'ALL').change();
    $('#PartNo').val('');
    $('#Prefix').val('');
    $('#EntryDate').val('');
    $('#ReconditionedWO').val('');
    $('#DocC').val('');
    $('#DocR').val('');
    $('#WCSL').val('');
}