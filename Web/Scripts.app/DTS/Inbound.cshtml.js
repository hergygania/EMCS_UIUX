$table = $('#tableInbound');
$tableHistory = $("#tableHistoryList");
$dataDetail = [];
var detailInbound = [];
var columnListHistory = [
    [
        {
            field: 'AjuNo',
            title: 'RTS',
            colspan: 2,
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        }, {
            field: 'AjuNo',
            title: 'On Board Vessel',
            halign: 'center',
            colspan: 2,
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        }, {
            field: 'AjuNo',
            title: 'Port In',
            colspan: 2,
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        }, {
            field: 'AjuNo',
            title: 'Port Out',
            colspan: 2,
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        }, {
            field: 'AjuNo',
            title: 'PLB In',
            colspan: 2,
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        }, {
            field: 'AjuNo',
            title: 'PLB Out',
            halign: 'center',
            colspan: 2,
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        }, {
            field: 'AjuNo',
            title: 'Yard In',
            halign: 'center',
            colspan: 2,
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        }, {
            field: 'AjuNo',
            title: 'Yard Out',
            colspan: 2,
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        }
    ],
    [
        {
            field: 'AjuNo',
            title: 'Plan',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Actual',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Plan',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Actual',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Plan',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Actual',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Plan',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Plan',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Plan',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Actual',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Plan',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Actual',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Plan',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Actual',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Plan',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'AjuNo',
            title: 'Actual',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
    ]
];
var columnList = [
    {
        field: 'AjuNo',
        title: 'AJU NO',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'PONo',
        title: 'PO NO',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'MSONo',
        title: 'MSO NO',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'LoadingPort',
        title: 'LOADING PORT',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'DischargePort',
        title: 'DISCHARGE PORT',
        halign: 'center',
        class: 'text-nowrap',
        align: 'left',
        sortable: true
    },
    {
        field: 'Model',
        title: 'MODEL',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'ModelDescription',
        title: 'MODEL DESCRIPTION',
        halign: 'center',
        class: 'text-nowrap',
        align: 'left',
        formatter: function (data, index, row) {
            if (data === 'null' || data === '') {
                return '-';
            }
            return data;
        },
        sortable: true
    },
    {
        field: 'Status',
        title: 'STATUS',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'SerialNumber',
        title: 'SERIAL NUMBER',
        halign: 'center',
        class: 'text-nowrap',
        align: 'left',
        sortable: true
    },
    {
        field: 'BatchNumber',
        title: 'BATCH NUMBER',
        halign: 'center',
        class: 'text-nowrap',
        align: 'left',
        sortable: true
    },
    //{
    //    field: 'ETAPort',
    //    title: 'ETA PORT',
    //    halign: 'center',
    //    align: 'left',
    //    class: 'text-nowrap',
    //    sortable: true,
    //    formatter: dateFormatterCAT
    //},
    //{
    //    field: 'ETACakung',
    //    title: 'ETA CAKUNG',
    //    halign: 'center',
    //    align: 'center',
    //    class: 'text-nowrap',
    //    sortable: true,
    //    formatter: dateFormatterCAT
    //},
    //{
    //    field: 'ATAPort',
    //    title: 'ATA PORT',
    //    halign: 'center',
    //    class: 'text-nowrap',
    //    align: 'center',
    //    sortable: true,
    //    formatter: dateFormatterCAT
    //},
    //{
    //    field: 'ATACakung',
    //    title: 'ATA CAKUNG',
    //    halign: 'center',
    //    align: 'center',
    //    class: 'text-nowrap',
    //    sortable: true,
    //    formatter: dateFormatterCAT
    //},
    //{
    //    field: 'Position',
    //    title: 'POSITION',
    //    halign: 'center',
    //    align: 'left',
    //    class: 'text-nowrap',
    //    sortable: true
    //},
    {
        field: 'Notes',
        title: 'NOTES 1',
        class: 'nowrap text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'Remark',
        title: 'NOTES 2',
        class: 'nowrap text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'Plant',
        title: 'PLANT',
        halign: 'center',
        class: 'text-nowrap',
        align: 'left',
        sortable: true
    }];
function operateEvents() {

}
function operateFormatter(options) {
    var btn = [];
    btn.push('<button onclick = "showHistoryTable()" class="btn btn-xs btn-primary" alt="Edit remarks">');
    btn.push('<i class="fa fa-edit"></i>');
    btn.push('</button>');
    return btn.join('');
}
function showHistoryTable() {
    $("#myModalPlace").modal({
        keyboard: true
    }, "show");
}
function searchData() {
    var key = $("#searchKey").val();
    if (key === "") {
        sAlert('Warning', 'Silahkan masukkan kriteria pencarian', 'warning');
    } else {
        $("#loading").show();
        $.ajax({
            url: "/DTS/getDetailDataInbound/",
            method: 'POST',
            //async: false,
            beforeSend: function () {
                $("#loading").show();
                $("#inboundDetailOnce").hide();
            },
            data: { Key: key },
            success: function (resultData) {
                detailInbound = [];
                var data = resultData.data;
                if (data !== null) {
                    detailInbound = data;
                    data = resultData;
                    $(".detailView").html("-");

                    $("#MSONo").html(data.MSONo);
                    $("#PoNumber").html(data.PONo);
                    $("#PODate").html(dateFormatterCAT(data.PODate));
                    $("#LoadingPort").html(data.LoadingPort);
                    $("#DischargePort").html(data.DischargePort);
                    $("#Status").html(data.Status);
                    $("#SerialNumber").html(data.SerialNumber);
                    $("#BatchNumber").html(data.BatchNumber);
                    $("#model").html(data.Model);
                    $("#modelDescription").html(data.ModelDescription);
                    $("#Position").html(data.Position);
                    $("#EtaPort").html(data.ETAPort);
                    $("#AtaPort").html(data.ATAPort);
                    $("#EtaCakung").html(data.ETACakung);
                    $("#AtaCakung").html(data.ATACakung);

                    $("#RtsPlan").html(dateFormatterCAT(data.RTSPlan));
                    $("#RtsActual").html(dateFormatterCAT(data.RTSActual));

                    $("#loadingPort").html(dateFormatterCAT(data.LoadingPort));
                    $("#VesselActual").html(dateFormatterCAT(data.VesselActual));
                    $("#VesselPlan").html(dateFormatterCAT(data.VesselPlan));

                    $("#PortInPlan").html(dateFormatterCAT(data.PortInPlan));

                    $("#PortInActual").html(dateFormatterCAT(data.PortInActual));

                    $("#PortOutPlan").html(dateFormatterCAT(data.PortOutPlan));
                    $("#PortOutActual").html(dateFormatterCAT(data.PortOutActual));
                    $("#PlbInPlan").html(dateFormatterCAT(data.PLBInPlan));
                    $("#PlbInActual").html(dateFormatterCAT(data.PLBInActual));
                    $("#PlbOutPlan").html(dateFormatterCAT(data.PLBOutPlan));
                    $("#PlbOutActual").html(dateFormatterCAT(data.PLBOutActual));
                    $("#YardInPlan").html(dateFormatterCAT(data.YardInPlan));
                    $("#YardInActual").html(dateFormatterCAT(data.YardInActual));
                    $("#YardOutPlan").html(dateFormatterCAT(data.YardOutPlan));
                    $("#YardOutActual").html(dateFormatterCAT(data.YardOutActual));
                    $("#Notes").html(data.Notes ? data.Notes : "-");
                    $("#Remark").html(data.Remark ? data.Remark : "-");
                    $("#ResultPlant").html(data.Plant ? data.Plant : "-");
                    $dataDetail = data.DetailList;
                    $("#inboundDetailNone").hide();
                    $("#inboundDetailOnce").show(700);
                    $("#loading").hide();
                } else {
                    $("#inboundDetailOnce").hide(700);
                    $("#inboundDetailNone").show();
                    $("#loading").hide();
                }
            },
            errof: function (err) {
                console.log(err);
                $("#loading").hide();
            }
        });
    }
}
function saveRemark() {
    var remark = $("#Remark").val();
    var PONo = $("#PoNumber").html().trim();

    $.ajax({
        url: "/DTS/submitRemarkInbound/",
        method: 'POST',
        data: { Remark: remark, PONo: PONo },
        success: function (d) {
            console.log(d);
            if (d.Msg !== undefined) {
                sAlert('Success', d.Msg, 'success');
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}
function showHistory(keyHistory) {
    console.log(keyHistory);
    $title = "";
    $dataList = [];
    console.log('list data detail : ', $dataDetail);
    jQuery.each($dataDetail, function (index, detail) {
        $dataListHtml = "<tr>";
        $dataListHtml += "<td class='text-center'>" + detail.RTSPlan + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.RTSActual + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.VesselPlan + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.VesselActual + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.PortInPlan + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.PortInActual + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.PortOutPlan + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.PortOutActual + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.PLBInPlan + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.PLBInActual + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.PLBOutPlan + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.PLBOutActual + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.YardInPlan + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.YardInActual + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.YardOutPlan + "</td>";
        $dataListHtml += "<td class='text-center'>" + detail.YardOutActual + "</td>";
        $dataListHtml += "<tr>";
    });
    $("#HistoryTitle").html('<strong>Detail Inbound</strong>');
    $tableHistory = $("#tableHistoryList tbody").html($dataListHtml);
    $('#myModalPlace').modal({
        keyboard: true
    }, 'show');
}
function searchDataList() {
    var RTS = $("#dtRTS").val();
    if (RTS !== "") {
        var RTSData = RTS.split("-");
        if (RTSData !== "") {
            var RTSFrom = moment(RTSData[0].trim()).format('YYYY-MM-DD');
            var RTSEnd = moment(RTSData[1].trim()).format('YYYY-MM-DD');
        }
    }

    var Vessel = $("#dtOnboard").val();
    if (Vessel !== "") {
        var VesselData = Vessel.split("-");
        if (VesselData !== "") {
            var VesselFrom = moment(VesselData[0].trim()).format('YYYY-MM-DD');
            var VesselEnd = moment(VesselData[1].trim()).format('YYYY-MM-DD');
        }
    }

    var Portin = $("#dtPortin").val();
    if (Portin !== "") {
        var PortinData = Portin.split("-");
        if (PortinData !== "") {
            var PortinFrom = moment(PortinData[0].trim()).format('YYYY-MM-DD');
            var PortinEnd = moment(PortinData[1].trim()).format('YYYY-MM-DD');
        }
    }

    //var Portout = $("#dtPortout").val();
    //if (Portout !== "") {
    //    var PortoutData = Portout.split("-");
    //    if (PortoutData !== "") {
    //        var PortoutFrom = moment(PortoutData[0].trim()).format('YYYY-MM-DD');
    //        var PortoutEnd = moment(PortoutData[1].trim()).format('YYYY-MM-DD');
    //    }
    //}
    var Status = $("#dtStatus").val();
    var Position = $("#dtPosition").val();

    window.pis.table({
        objTable: $table,
        urlSearch: '/DTS/InboundManagementPage',
        urlPaging: '/DTS/InboundManagementPageXt',
        searchParams: {
            PoNumber: $('#txtlistidinbound').val().trim(),
            AjuNumber: $("#txtajunumber").val().trim(),
            SerialNumber: $("#txtserialnumber").val().trim(),
            BatchNumber: $("#txtbatchnumber").val().trim(),
            Model: $("#dtModel").val().trim(),
            Status: Status.trim(),
            Position: Position,
            RTSFrom: RTSFrom,
            RTSTo: RTSEnd,
            OnBoardVesselFrom: VesselFrom,
            OnBoardVesselTo: VesselEnd,
            PortInFrom: PortinFrom,
            PortInTo: PortinEnd,
            //PortOutFrom: PortoutFrom,
            //PortOutTo: PortoutEnd
        },
        autoLoad: true
    });

}
function savePosition() {
    var position = $("#Position").val();
    var PONo = $("#PoNumber").val();
    console.log(PONo);
    $.ajax({
        url: "/DTS/submitPositionInbound/",
        method: 'POST',
        data: { Position: position, PONo: PONo },
        success: function (d) {
            console.log(d);
            if (d.Msg !== undefined) {
                sAlert('Success', d.Msg, 'success');
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}
$(function () {
    $(".datepickr").daterangepicker();
    $("#moreFilter").click(function () {
        $("#formAdvanceSearch").toggle(700);
    });
    $("#mySearch").insertBefore($("[name=refresh]"));
    $(".downloadExcelInbound").click(function () {

        var RTS = $("#dtRTS").val();
        if (RTS !== "") {
            var RTSData = RTS.split("-");
            if (RTSData !== "") {
                var RTSFrom = moment(RTSData[0].trim()).format('YYYY-MM-DD');
                var RTSEnd = moment(RTSData[1].trim()).format('YYYY-MM-DD');
            }
        }


        var Vessel = $("#dtOnboard").val();
        if (Vessel !== "") {
            var VesselData = Vessel.split("-");
            if (VesselData !== "") {
                var VesselFrom = moment(VesselData[0].trim()).format('YYYY-MM-DD');
                var VesselEnd = moment(VesselData[1].trim()).format('YYYY-MM-DD');
            }
        }

        var Portin = $("#dtPortin").val();
        if (Portin !== "") {
            var PortinData = Portin.split("-");
            if (PortinData !== "") {
                var PortinFrom = moment(PortinData[0].trim()).format('YYYY-MM-DD');
                var PortinEnd = moment(PortinData[1].trim()).format('YYYY-MM-DD');
            }
        }

        var Status = $("#dtStatus").val();
        var Position = $("#dtPosition").val();

        //var Portout = $("#dtPortout").val();
        //if (Portout !== "") {
        //    var PortoutData = Portout.split("-");
        //    if (PortoutData !== "") {
        //        var PortoutFrom = moment(PortoutData[0].trim()).format('YYYY-MM-DD');
        //        var PortoutEnd = moment(PortoutData[1].trim()).format('YYYY-MM-DD');
        //    }
        //}


        enableLink(false);
        $.ajax({
            url: "/DTS/DownloadInbound",
            type: 'GET',
            data: {
                IdString: $('#txtlistidinbound').val(),
                PoNumber: $('#txtlistidinbound').val().trim(),
                AjuNumber: $("#txtajunumber").val().trim(),
                SerialNumber: $("#txtserialnumber").val().trim(),
                BatchNumber: $("#txtbatchnumber").val().trim(),
                Model: $("#dtModel").val().trim(),
                Status: Status.trim(),
                Position: Position,
                RTSFrom: RTSFrom,
                RTSTo: RTSEnd,
                OnBoardVesselFrom: VesselFrom,
                OnBoardVesselTo: VesselEnd,
                PortInFrom: PortinFrom,
                PortInTo: PortinEnd,

                //RTSFrom: RTSFrom,
                //RTSTo: RTSEnd,
                //OnBoardVesselFrom: VesselFrom,
                //OnBoardVesselTo: VesselEnd,
                //PortInFrom: PortinFrom,
                //PortInTo: PortinEnd,
                //Model: $("#dtModel").val(),
                //PortOutFrom: PortoutFrom,
                //PortOutTo: PortoutEnd,
                //Status: $('#Status').val(),
                //Position: $('#Position').val()
            },
            success: function (guid) {
                enableLink(true);
                window.open('/DTS/DownloadToExcelInbound?guid=' + guid, '_blank');
            },
            cache: false,
        });
    });
    $tableHistory.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        sidePagination: 'server',
        showExport: false,
        exportDataType: 'basic',
        exportTypes: ['excel'],
        exportOptions: {
            ignoreColumn: [0],
            fileName: 'file.xls'
        },
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        columns: columnListHistory
    });
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        sidePagination: 'server',
        detailView: true,
        showExport: false,
        exportDataType: 'basic',
        exportTypes: ['excel'],
        exportOptions: {
            ignoreColumn: [0],
            fileName: 'file.xls'
        },
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        onExpandRow: function (index, row, $detail) {
            $detail.html('<span class="text-center" style="font-size:16px;"><i class="fa fa-spinner fa-pulse fa-fw"></i></span> Loading, please wait...');
            $.ajax({
                url: "/DTS/PartialListDetail",
                dataType: "html",
                method: 'GET',
                data: { InboundID: row.PONo, InboundSN: row.SerialNumber },
                success: function (resultHtml) {
                    $detail.html(resultHtml);
                },
                error: function (e) {
                    $detail.html("Data not found");
                }
            });
        },
        columns: columnList
    });
});