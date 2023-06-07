var $tableMileStone = $('#tableMilestoneDTS');

function iniTableMilestineDTS(OBD, SO) {
    $tableMileStone.bootstrapTable({
        cache: false,
        search: false,
        pagination: false,
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
        formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
        columns: [
            { field: 'Milestone', title: 'Milestone', width: '10%', halign: 'center', align: 'left' },
            { field: 'SalesOrder', title: 'Sales Order', halign: 'center', align: 'left' },
            //{ field: 'Workable', title: 'Workable', halign: 'center', align: 'left' },
            { field: 'Allocation', title: 'Allocation', halign: 'center', align: 'left' },
            //{ field: 'RA', title: 'Release', halign: 'center', align: 'left'},
            { field: 'OD', title: 'Out Bound Delivery', halign: 'center', align: 'left' },
            { field: 'Departure', title: 'Departure', halign: 'center', align: 'left' },
            { field: 'GI', title: 'Good Issue', halign: 'center', align: 'left' },
            { field: 'Arrival', title: 'Arrival', halign: 'center', align: 'left' }
        ]
    });

    window.pis.table({
        objTable: $tableMileStone,
        searchParams: {
            OutBoundDelivery: OBD,
            SalesOrderNumber: SO
        },
        urlSearch: '/DeliveryTrackingStatus/IndexPageMilestoneDTS',
        urlPaging: '/DeliveryTrackingStatus/GetMilstoneDTS',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"));
}