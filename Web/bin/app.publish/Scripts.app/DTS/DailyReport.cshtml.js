$table = $("#tableDailyReport");
var columnList = [
    { field: 'DA', title: 'DA', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
    { field: 'DI', title: 'DI', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
    { field: 'Origin', title: 'ORIGIN', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
    { field: 'Destination', title: 'DESTINATION', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
    { field: 'Moda', title: 'MODA', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap' },
    { field: 'UnitModa', title: 'UNIT MODA', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap' },
    { field: 'UnitType', title: 'UNIT TYPE', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap' },
    { field: 'Model', title: 'MODEL', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap' },
    { field: 'ETD', title: 'ETD', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap', formatter: dateFormatterCAT },
    { field: 'ATD', title: 'ATD', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap', formatter: dateFormatterCAT },
    { field: 'ETA', title: 'ETA', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap', formatter: dateFormatterCAT },
    { field: 'ATA', title: 'ATA', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap', formatter: dateFormatterCAT },
    { field: 'Position', title: 'POSITION', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap' },
    { field: 'Status', title: 'STATUS', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap' },
    { field: 'SerialNumber', title: 'SN', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap' },
    { field: "Remarks", title: "NOTES", class: "text-nowrap" },
    { field: "Cost", title: "COST", class: "text-nowrap" },
    { field: "EntrySheetDate", title: "ENTRYSHEET DATE", class: "text-nowrap", formatter: dateFormatterCAT },
    { field: "ReleaseDate", title: "RELEASE DATE BY COMM", class: "text-nowrap", formatter: dateFormatterCAT },
    { field: "ShipmentDoc", title: "SHIPMENT DOC", class: "text-nowrap" },
    { field: "ShipmentCost", title: "SHIPMENT COST", class: "text-nowrap" },
    { field: "EntrySheetDate", title: "ENTRY SHEET", class: "text-nowrap", formatter: dateFormatterCAT },
    { field: "RejectReason", title: "REJECT REASON", class: "text-nowrap" }
];
function showList() {
    var PrimaryParam = $("#DANumber").val();
    var DINumber = $("#DINumber").val();
    var Position = $("#FilterFilterPosition").val();
    var Status = $("#FilterStatus").val();
    var UnitType = $("#FilterUnitType").val();
    var Moda = $("#FilterModa").val();
    var SerialNumber = $("#SerialNumber").val();
    var Model = $("#FilterModel").val();

    window.pis.table({
        objTable: $table,
        urlSearch: '/DTS/OutboundReportPage',
        urlPaging: '/DTS/OutboundReportPageXt',
        searchParams: {
            PrimaryParam: PrimaryParam,
            DANumber: PrimaryParam,
            SerialNumber: SerialNumber,
            DINumber: DINumber,
            Position: Position,
            Status: Status,
            UnitType: UnitType,
            Moda: Moda,
            Model: Model
        },
        //dataHeight: 412,
        autoLoad: true
    });
}

$(function () {
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        sidePagination: 'server',
        detailView: false,
        showExport: false,
        pageSize: '10',
        showRefresh: true,
        columns: columnList
    });
    $("#btnExportOutbound").click(function () {
        var PrimaryParam = $("#DANumber").val();
        var DINumber = $("#DINumber").val();
        var Position = $("#FilterFilterPosition").val();
        var Status = $("#FilterStatus").val();
        var UnitType = $("#FilterUnitType").val();
        var Moda = $("#FilterModa").val();
        var SerialNumber = $("#SerialNumber").val();
        var Model = $("#FilterModel").val();
        var SN = $("#FilterSerialNumber").val();
        enableLink(false);
        $.ajax({
            url: "/DTS/DownloadOutboundReport",
            type: 'GET',
            data: {
                PrimaryParam: PrimaryParam,
                DANumber: PrimaryParam,
                SerialNumber: SerialNumber,
                DINumber: DINumber,
                Position: Position,
                Status: Status,
                UnitType: UnitType,
                Moda: Moda,
                Model: Model,
                SerialNumber: SN
            },
            success: function (guid) {
                enableLink(true);
                window.open('/DTS/DownloadToExcelOutbound?guid=' + guid, '_blank');
            },
            cache: false
        });
    });
});