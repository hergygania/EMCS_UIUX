var $table = $("#tableDailyReport");
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
        title: 'DISCHARD PORT',
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
    {
        field: 'ETAPort',
        title: 'ETA PORT',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: dateFormatterCAT
    },
    {
        field: 'ETACakung',
        title: 'ETA CAKUNG',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: dateFormatterCAT
    },
    {
        field: 'ATAPort',
        title: 'ATA PORT',
        halign: 'center',
        class: 'text-nowrap',
        align: 'center',
        sortable: true,
        formatter: dateFormatterCAT
    },
    {
        field: 'ATACakung',
        title: 'ATA CAKUNG',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: dateFormatterCAT
    },
    {
        field: 'Position',
        title: 'POSITION',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Notes',
        title: 'NOTES',
        class: 'nowrap text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    }];
function showListReport() {
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

    var Portout = $("#dtPortout").val();
    if (Portout !== "") {
        var PortoutData = Portout.split("-");
        if (PortoutData !== "") {
            var PortoutFrom = moment(PortoutData[0].trim()).format('YYYY-MM-DD');
            var PortoutEnd = moment(PortoutData[1].trim()).format('YYYY-MM-DD');
        }
    }
    var Status = $("#dtStatus").val();
    var Position = $("#dtPosition").val();

    window.pis.table({
        objTable: $table,
        urlSearch: '/DTS/InboundManagementPage',
        urlPaging: '/DTS/InboundManagementPageXt',
        searchParams: {
            PoNumber: $('#txtlistidinbound').val(),
            AjuNumber: $("#txtajunumber").val(),
            SerialNumber: $("#txtserialnumber").val(),
            Status: Status,
            Position: Position,
            RTSFrom: RTSFrom,
            RTSTo: RTSEnd,
            OnBoardVesselFrom: VesselFrom,
            OnBoardVesselTo: VesselEnd,
            PortInFrom: PortinFrom,
            PortInTo: PortinEnd,
            PortOutFrom: PortoutFrom,
            PortOutTo: PortoutEnd
        },
        autoLoad: true
    });
}

$(function () {
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        sidePagination: 'server',
        showRefresh: true,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: columnList
    });
    $(".downloadExcelInbound").click(function () {
        var RTS = $("#dtRTS").val();
        var RTSData = RTS.split("-");
        if (RTSData !== "") {
            var RTSFrom = RTSData[0] ? moment(RTSData[0].trim()).format('YYYY-MM-DD') : "";
            var RTSEnd = RTSData[1] ? moment(RTSData[1].trim()).format('YYYY-MM-DD') : "";
        }

        var Vessel = $("#dtOnboard").val();
        var VesselData = Vessel.split("-");
        if (VesselData !== "") {
            var VesselFrom = VesselData[0] ? moment(VesselData[0].trim()).format('YYYY-MM-DD') : "";
            var VesselEnd = VesselData[1] ? moment(VesselData[1].trim()).format('YYYY-MM-DD') : "";
        }

        var Portin = $("#dtPortin").val();
        var PortinData = Portin.split("-");
        if (PortinData !== "") {
            var PortinFrom = PortinData[0] ? moment(PortinData[0].trim()).format('YYYY-MM-DD') : "";
            var PortinEnd = PortinData[1] ? moment(PortinData[1].trim()).format('YYYY-MM-DD') : "";
        }


        var Portout = $("#dtPortout").val();
        var PortoutData = Portout.split("-");
        if (PortoutData > 0) {
            if (PortoutData !== "") {
                var PortoutFrom = PortoutData[0].trim() ? moment(PortoutData[0].trim()).format('YYYY-MM-DD') : "";
                var PortoutEnd = PortoutData[0].trim() ? moment(PortoutData[1].trim()).format('YYYY-MM-DD') : "";
            }
        }
        var Status = $("#dtStatus").val();
        var Position = $("#dtPosition").val();

        enableLink(false);
        $.ajax({
            url: "/DTS/DownloadInbound",
            type: 'GET',
            data: {
                PoNumber: $('#txtlistidinbound').val(),
                AjuNumber: $("#txtajunumber").val(),
                SerialNumber: $("#txtserialnumber").val(),
                Status: Status,
                Position: Position,
                RTSFrom: RTSFrom,
                RTSTo: RTSEnd,
                OnBoardVesselFrom: VesselFrom,
                OnBoardVesselTo: VesselEnd,
                PortInFrom: PortinFrom,
                PortInTo: PortinEnd,
                PortOutFrom: PortoutFrom,
                PortOutTo: PortoutEnd
            },
            success: function (guid) {
                enableLink(true);
                window.open('/DTS/DownloadToExcelInbound?guid=' + guid, '_blank');
            },
            cache: false,
        });
    });
});