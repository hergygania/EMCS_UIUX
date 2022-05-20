$table = $('#tableOutbound');
function showDetail() {
    $("#OutboundDetailTrue").show(500);
    $("#OutboundDetailNone").hide();
}
function hideDetail() {
    $("#OutboundDetailTrue").hide(500);
    $("#OutboundDetailNone").show();
}
function formatDateLocal(string) {
    if (string !== null) {
        var newFormat = moment(string).format("DD MMM YYYY");
        return newFormat;
    } else {
        return "-";
    }
}
function saveRemark() {
    var Position = $("#Position").val();
    var Status = $("#Status").val();
    var remark = $("#Remarks").val();
    var DANo = $("#DANo").val();
    var DI = $("#DINo").html().trim();
    var SerialNumber = $("#SerialNumber").html().trim();
    var IDoutbound = $("#IDOutbound").val();
    var NoPol = $("#NoPol").val();
    var DriverName = $("#DriverName").val();
    var HPInlandFreight = $("#HPInlandFreight").val();
    var VesselName = $("#VesselName").val();
    var PIC = $("#PIC").val();
    var HPSealandFreight = $("#HPSealandFreight").val();

    console.log(remark + '-' + DANo);
    $.ajax({
        url: "/DTS/submitRemarkOutbound/",
        method: 'POST',
        data: {
            remarks: remark, SerialNumber: SerialNumber, DI: DI, Position: Position, Status: Status, ID: IDoutbound, NoPol: NoPol, DriverName: DriverName, HPInlandFreight: HPInlandFreight,
            VesselName: VesselName, PIC: PIC, HPSealandFreight: HPSealandFreight
        },
        success: function (d) {
            console.log(d);
            if (d.Msg != undefined) {
                sAlert('Success', d.Msg, 'success');
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}
function searchData() {
    var key = $("#searchKey").val();
    $("#Status").val('');
    $("#Position").val('');
    $("#Remarks").html('');
    if (key === "") {
        sAlert('Warning', 'Silahkan masukkan kriteria pencarian', 'warning');
        hideDetail();
    } else {
        $.ajax({
            url: "/DTS/getDetailDataOutbound/",
            method: 'POST',
            beforeSend: function () {
                // setting a timeout
                $("#loading").show();
                $("#inboundDetailOnce").hide();
            },
            data: { Key: key },
            success: function (resultData) {
                console.log(resultData);
                var data = resultData;
                console.log(data);
                $(".DetailView").html("-");
                $("#IDOutbound").val(data.ID);
                $("#DANo").html(data.DA);
                $("#DINo").html(data.DI);
                $("#DIDate").html('' + formatDateLocal(data.DIDate) + '');
                $("#Origin").html(data.Origin);
                $("#Destination").html(data.Destination);
                $("#DeliveryName").html(data.DeliveryContact);

                $("#NoPol").html(data.NoPol);
                $("#DriverName").html(data.DriverName);
                $("#HPInlandFreight").html(data.HPInlandFreight);
                $("#NoPol").val(data.NoPol);
                $("#DriverName").val(data.DriverName);
                $("#HPInlandFreight").val(data.HPInlandFreight);

                $("#VesselName").html(data.VesselName);
                $("#PIC").html(data.PIC);
                $("#HPSealandFreight").html(data.HPSealandFreight);
                $("#VesselName").val(data.VesselName);
                $("#PIC").val(data.PIC);
                $("#HPSealandFreight").val(data.HPSealandFreight);

                $("#Moda").html(data.Moda);
                $("#UnitModa").html(data.ShipmentModa);
                $("#UnitType").html(data.UnitType ? data.UnitType : "-");

                $("#Model").html(data.Model ? data.Model : "-");
                $("#SerialNumber").html(data.SerialNumber ? data.SerialNumber : "-");
               
                $("#ETD").html(formatDateLocal(data.ETD));
                $("#ATD").html(formatDateLocal(data.ATD));
                $("#ETA").html(formatDateLocal(data.ETA));
                $("#ATA").html(formatDateLocal(data.ATA));
                $("#Status").html(data.Status);
                $("#Position").html(data.Position);
                $("#Status").val(data.Status);
                $("#Position").val(data.Position);
                $("#Remarks").html(data.Remarks);

                $("#loading").hide();
                showDetail();
            },
            error: function (err) {
                console.log(err);
                hideDetail();
                $("#loading").hide();
            }
        });
    }
}
$(function () {
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: false,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        //onClickRow: selectRow,
        sidePagination: 'server',
        detailView: false,
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        //fixedColumns: true,
        //data: dataList,
        //fixedNumber: '5',
        columns: [
            { field: 'DI', title: 'DI', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
            { field: 'DIDate', title: 'DI Date', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap',formatter: dateFormatterCAT },
            { field: 'DA', title: 'DA', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
            { field: 'Moda', title: 'SHIPMENT MODA', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
            { field: 'UnitModa', title: 'MODA TRANSPORT', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
            { field: 'Origin', title: 'ORIGIN', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
            { field: 'Destination', title: 'DESTINATION', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
            { field: 'UnitType', title: 'UNIT TYPE', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
            { field: 'Model', title: 'MODEL', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
            { field: 'SerialNumber', title: 'SERIAL NUMBER', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
            { field: 'ETD', title: 'ETD', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap', formatter: dateFormatterCAT },
            { field: 'ATD', title: 'ATD', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap', formatter: dateFormatterCAT },
            { field: 'ETA', title: 'ETA', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap', formatter: dateFormatterCAT },
            { field: 'ATA', title: 'ATA', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap', formatter: dateFormatterCAT },
            { field: 'Status', title: 'STATUS', halign: 'center', align: 'center', sortable: true, class: 'text-nowrap' },
            { field: 'Position', title: 'POSITION', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap' },
            { field: 'Remarks', title: 'NOTES', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true }
            //{ field: 'CRC_PCD', title: 'CRC Promised<br>Completion Date', halign: 'center', align: 'center', formatter: dateFormatterCAT, sortable: true }
        ]
    });
    $("#moreFilter").click(function () {
        $("#formAdvanceSearch").toggle(700);
    });
    $("#BtnFilter").click(function () {
        var PrimaryParam = $("#DANumber").val();
        var DINumber = $("#DINumber").val();
        var Position = $("#FilterPosition").val();
        var Status = $("#FilterStatus").val();
        var UnitType = $("#FilterUnitType").val();
        var Moda = $("#FilterModa").val();
        var Model = $("#FilterModel").val();
        var Origin = $("#filterOrigin").val();
        var Destination = $("#filterDestination").val();
        var SN = $("#FilterSerialNumber").val();

        window.pis.table({
            objTable: $table,
            urlSearch: '/DTS/OutboundPage',
            urlPaging: '/DTS/OutboundPageXt',
            searchParams: {
                PrimaryParam: PrimaryParam,
                DANumber: PrimaryParam,
                DINumber: DINumber,
                Position: Position,
                Origin: Origin,
                Destination: Destination,
                Status: Status,
                UnitType: UnitType,
                Moda: Moda,
                Model: Model,
                SerialNumber: SN
            },
            //dataHeight: 412,
            autoLoad: true
        });
    });
    $("#btnExportOutbound").click(function () {
        var PrimaryParam = $("#FilterSearchKeyParam").val();
        var DINumber = $("#DINumber").val();
        var Position = $("#FilterPosition").val();
        var Status = $("#FilterStatus").val();
        var UnitType = $("#FilterUnitType").val();
        var Moda = $("#FilterModa").val();
        var Model = $("#FilterModel").val();
        var Origin = $("#filterOrigin").val();
        var Destination = $("#filterDestination").val();
        var SN = $("#FilterSerialNumber").val();

        enableLink(false);
        $.ajax({
            url: "/DTS/DownloadOutbound",
            type: 'GET',
            data: {
                PrimaryParam: PrimaryParam,
                DANumber: PrimaryParam,
                DINumber: DINumber,
                Position: Position,
                Origin: Origin,
                Destination: Destination,
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



