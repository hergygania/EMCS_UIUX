var $table = $('#tableDeliveryTracking');

$(function () {
    $table.bootstrapTable({
        cache: false,
        search: false,
        pagination: true,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        pageSize: '5',
        showExport: true,
        exportTypes: ['excel'],
        exportOptions: {
            ignoreColumn: [0],
            fileName: 'file.xls'
        },
        afterComplete: function () {
            $('html, body').animate({
                scrollTop: $table.offset().top
            }, 2000);
        },
        formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
        columns: [
            { field: 'Action', title: 'Action', width: '10%', halign: 'center', align: 'center', events: operateEvents, formatter: operateFormatter({ info: true }) },
            { field: 'NODA', title: 'No Delivery Advice', halign: 'center', align: 'left', sortable: true, visible: true, },
            { field: 'NODI', title: 'Out Bound Delivery', halign: 'center', align: 'left', sortable: true },
            { field: 'Origin', title: 'Origin', halign: 'center', align: 'left', sortable: true, visible: true, },
            { field: 'Destination', title: 'Destination', halign: 'center', align: 'left', sortable: true, visible: true, },
            { field: 'Status', title: 'Status', halign: 'center', align: 'center', sortable: true, visible: true, }
        ]
    });

    //window.pis.table({
    //    objTable: $table,
    //    urlSearch: '/DeliveryTrackingStatus/IndexPageDts',
    //    urlPaging: '/DeliveryTrackingStatus/GetDts',
    //    autoLoad: true
    //});
    $("#mySearch").insertBefore($("[name=refresh]"))
});

function operateFormatter(options) {
    var btn = [];

    btn.push('<div class="btn-group">');
    btn.push('<button type="button" class="btn btn-info info" title="detail"><i class="fa fa-search-plus"> Detail</i></button>');

    btn.push('</div>');

    return btn.join('');
}

operateFormatter.DEFAULTS = {
    Add: false,
    Edit: false,
    Delete: false,
    Info: false,
    View: false,
    History: false
}

window.operateEvents = {
    'click .info': function (e, value, row, index) {
        $("#panelHeader").fadeToggle("fast");
        showDetail('/DeliveryTrackingStatus/infoDTS', row);
    }
};

$(function () {
    $("#SimplePage").show();
    $("#btnFilter").show();
    $("#AdvancePage").hide();
    $("#btnShowAdvance").click(function () {
        if ($("#SN").attr("disabled") != undefined) {
            $("#SN").prop("disabled", false);
            $("#SN").addClass("Active");
            clearFilter();
        }
        else {
            $("#SN").val("");
            $("#SN").prop("disabled", true);
            $("#SN").removeClass("Active");
        }

        $(".inputSN.Active").val("");
        $("#btnFilter").fadeToggle('fast');
        $("#AdvancePage").fadeToggle('fast');
    });

    $(".btnFilter").click(function () {
        initFilterTable();
    });
});

function clearFilter() {
    $("#OriginID").val("").trigger("change");
    $("#DestinationID").val("").trigger("change");
    $("#model").val("");
    $("#OutBoundDelivery").val("");
    $("#SalesOrderNumber").val("");
    $("#Est_Departure_Date").val("");
    $("#Est_Arrival_Date").val("");
    $("#Actual_Departure_Date").val("");
    $("#Actual_Arrival_Date").val("");
}

function initFilterTable() {
    window.pis.table({
        objTable: $table,
        urlSearch: '/DeliveryTrackingStatus/IndexPageDts',
        urlPaging: '/DeliveryTrackingStatus/GetDts',
        autoLoad: true,
        searchParams: {
            SN: $(".inputSN.Active").val(),
            From: $("#OriginID").val(),
            To: $("#DestinationID").val(),
            Model: $("#model").val(),
            OutBoundDelivery: $("#OutBoundDelivery").val(),
            SalesOrderNumber: $("#SalesOrderNumber").val(),
            ETD: $("#Est_Departure_Date").val(),
            ATD: $("#Est_Arrival_Date").val(),
            ETA: $("#Actual_Departure_Date").val(),
            ATA: $("#Actual_Arrival_Date").val()
        },
        afterComplete: function () {
            $('html, body').animate({
                scrollTop: $table.offset().top
            }, 2000);
        }
    });
    $('.fixed-table-body-columns').css('display', 'block');
}

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $('#progress').show();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {

                if (result.Status == 0) {
                    $("#panelDetail").fadeToggle("fast");
                    V_DTS.OperatingPlan = $(".OperatingPlan").val();
                    V_DTS.Cost = $(".Cost").val();
                    V_DTS.Remarks = $(".Remarks")[0].value;
                    V_DTS.PICDriver = $(".PICDriver").val();
                    V_DTS.VendorName = $(".VendorName").val();

                    $("#panelDetail").hide('slow');
                    showDetail('/DeliveryTrackingStatus/infoDTS', V_DTS);
                    $("#panelDetail").show('slow');

                    if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
                    $('#myModalPlace').modal('hide');
                    $('#progress').hide();
                }
                else {
                    if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
                    $('#progress').hide();
                }
            }
        });
        return false;
    });
    //$("form").prop("action", dialog.childNodes[2].childNodes[1].action);
    $(".modal-dialog").css("width", "70%");
};

function showDetail(url, params) {
    $.ajax({
        type: "GET",
        url: url,
        dataType: "html",
        data: params,
        async: false,
        cache: false,
        beforeSend: function () {
            enableLink(false);
            $("#panelDetail").html("<div style=\"text-align:center;color:red\"><img src='/Content/images/ajax-loading.gif' style=\"padding-right:3px\">...Loading page...</div>");
        },
        success: function (data) {
            var _br = $('.box-header').length ? "" : "<br/>";
            $("#panelDetail").empty();
            $("#panelDetail").html(_br + data);
        },
        complete: function () {
            $("#panelDetail").fadeToggle("fast");
            initTableDetailDTS(params.NODI, params.NODA);
        }
    });
}

function showHeader() {
    $("#panelHeader").fadeToggle("fast");
    $("#panelDetail").fadeToggle("fast");
    //initFilterTable();
}

function showMilestoneDTS(url, param) {
    enableLink(false);
    $.ajax({
        type: "GET",
        url: url,
        dataType: "html",
        data: param,
        async: false,
        cache: false,
        success: function (resultHtml) {
            $('#myModalMilestoneContent').html(resultHtml);
            $('#myModalMilestone').modal({ keyboard: true }, 'show');
            enableLink(true);
        },
        complete: function () {

        }
    });

    $('#myModalMilestone').on('shown.bs.modal', function (e) {
        showMainScrollbar(false);
        $.fn.modal.Constructor.prototype.enforceFocus = function () { };
    });
    $('#myModalMilestone').on('hidden.bs.modal', function (e) {
        showMainScrollbar(true);
    });

    $(".modal-dialog").css("width", "70%");
}

function updateDTS(ID) {
    loadModal('/DeliveryTrackingStatus/UpdateDTS?id=' + ID)
}

function initTableDetailDTS(OutBoundDelivery, NoDeliveryAdvice) {
    var $tblDetailDTS = $('#tableDetailDTS');
    $tblDetailDTS.bootstrapTable({
        cache: false,
        search: false,
        pagination: true,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        pageSize: '5',
        showExport: true,
        exportTypes: ['excel'],
        exportOptions: {
            ignoreColumn: [0],
            fileName: 'file.xls'
        },
        formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
        columns: [
             //{ field: 'Action', title: 'Action', halign: 'center', align: 'center', events: operateEventsDetail, formatter: operateFormatterDetail({ info: true }), visible: false },
            { field: 'SN', title: 'Serial Number', halign: 'center', align: 'left', sortable: true, visible: true, },
            { field: 'Model', title: 'Model', halign: 'center', align: 'left', sortable: true },
            { field: 'ETD', title: 'Est. Departure', halign: 'center', align: 'left', sortable: true, visible: true, formatter: dateFormatter },
            { field: 'ATD', title: 'Act. Departure', halign: 'center', align: 'left', sortable: true, visible: true, formatter: dateFormatter },
            { field: 'ETA', title: 'Est. Arrival', halign: 'center', align: 'center', sortable: true, visible: true, formatter: dateFormatter },
            { field: 'ATA', title: 'Act. Arrival', halign: 'center', align: 'center', sortable: true, visible: true, formatter: dateFormatter },
            { field: 'Cost', title: 'Cost', halign: 'center', align: 'center', sortable: true, visible: true, },
            { field: 'Status', title: 'Status', halign: 'center', align: 'center', sortable: true, visible: true, }
        ]
    });

    window.pis.table({
        objTable: $tblDetailDTS,
        urlSearch: '/DeliveryTrackingStatus/IndexPageDetailDts',
        urlPaging: '/DeliveryTrackingStatus/GetDetailDts',
        searchParams: {
            OutBoundDelivery: OutBoundDelivery,
            NoDeliveryAdvice: NoDeliveryAdvice
        },
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function operateFormatterDetail(options) {
    var btn = [];

    btn.push('<div class="btn-group">');
    //if (options.info == true)
    btn.push('<button type="button" class="btn btn-info milestone" title="milestone"><i class="fa fa-search-plus"> Milestone</i></button>');

    btn.push('</div>');

    return btn.join('');
}

window.operateEventsDetail = {
    'click .milestone': function (e, value, row, index) {
        showMilestoneDTS('/DeliveryTrackingStatus/MilestoneDTS', row);
    }
};