var formatDateStr = "DD.MM.YYYY"; 
var $DRData = null;
$table = $('#tableOutbound');
$formLogStatus = $('#formLogStatus');
function statusFormatter(str, index, row) {
    color = '';
    icon = '';
    text = formatUpperCase(str);
    switch (str) {
        case 'Delayed':
            color = 'danger';
            //icon = 'fa fa-times';
            break;
        case 'Risk Of Delayed':
            color = 'warning';
            //icon = 'fa fa-edit';
            break;
        case 'On Schedule':
            color = 'primary';
            //icon = 'fa fa-paper-plane';
            break;
        default:
            color = 'default';
            //icon = 'fa fa-file';
            break;
    }
    return "<div class='label label-" + color + "' style='width:100px;'>" + "<i class='" + icon + "'></i> " + text + "</div>";
}
function activityFormatter(str, index, row) {
    color = '';
    text = formatUpperCase(str);
    switch (str) {
        case 'DR Creation':
            color = 'default';
            icon = 'fa fa-file-medical';
            break;
        case 'Preparation':
            color = 'default';
            icon = 'fa fa-hourglass-half';
            break;
        case 'Pick Up':
            color = 'default';
            icon = 'fa fa-truck-pickup';
            break;
        case 'Instransit':
            color = 'default';
            icon = 'fa fafa-shipping-fast';
            break;
        case 'POD':
            color = 'default';
            icon = 'fa fa-handshake';
            break;
        case 'Rejected':
            color = 'danger';
            icon = 'fa fa-ban';
            break;
        default:
            color = 'default';
            icon = 'fa fa-file';
            break;
    }
    return "<div class='label label-" + color + "' style='width:100px;'>" + "<i class='" + icon + "'></i> " + text + "</div>";
}
var formLogValidator = $formLogStatus.validate({
    highlight: function (element, errorClass, validClass) {
        $formLogStatus.find("div[for=" + element.name + "]").show();
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass(errorClass).addClass(validClass);
        $formLogStatus.find("div[for=" + element.name + "]").hide();
    }
});

function tooltip() {
    $('[data-toggle="tooltip"]').tooltip()
}

function showModal(id = 'myModalRequest') {
    $('#' + id).modal("show");
}
function hideModal(id = 'myModalRequest') {
    $('#' + id).modal("hide");
}

function showModalLog() {
    $('#ApplyToAll').prop('checked', false);
    if ($DRData.header.ModaTransport == "SEA") {
        $('#sea-freight .driver-info').removeClass('hidden');
    } else if ($DRData.header.ModaTransport == "AIR") {
        $('#air-freight .driver-info').removeClass('hidden');
    }
    $('#modalLogStatus').modal("show");
}
function hideModalLog() {
    $('#modalLogStatus').modal("hide");
}

function showDetail() {
    $("#OutboundDetailTrue").show(500);
    $("#OutboundDetailNone").hide();
}
function hideDetail() {
    $("#OutboundDetailTrue").hide(500);
    $("#OutboundDetailNone").show();
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

    $.ajax({
        url: "/DTS/submitRemarkOutbound/",
        method: 'POST',
        data: {
            remarks: remark, SerialNumber: SerialNumber, DI: DI, Position: Position, Status: Status, ID: IDoutbound, NoPol: NoPol, DriverName: DriverName, HPInlandFreight: HPInlandFreight,
            VesselName: VesselName, PIC: PIC, HPSealandFreight: HPSealandFreight
        },
        success: function (d) {
            //console.log(d);
            if (d.Msg !== undefined) {
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
        $("#SupportingDocument").attr("download-url", "");
        $("#SupportingDocument").css("cursor", "auto");
        $("#SupportingDocument").removeClass("text-blue");

        $.ajax({
            url: myApp.fullPath + "/DTS/v2/getDetailDataOutbound",
            //method: 'POST',
            beforeSend: function () {
                ShowLoading();
                $("#inboundDetailOnce").hide();
                $("#OutboundDetailNone").hide();
            },
            data: { Key: key },
            success: function (json) {
                HideLoading();
                $(".DetailView").html("-");

                if (json.result && json.data && json.data.header) {
                    $DRData = json.data;
                    var data = json.data.header;
                    initTrackingStatus(data, json.data.details);
                    outboundTracking.HeaderID = data.ID;
                    outboundTracking.logStatus = json.data.log;
                    outboundTracking.refreshTableUnit(json.data.details);
                    $("#tableLog > tbody").empty();
                    showDetail();
                } else {
                    hideDetail();
                }
            },
            error: function (err) {
                hideDetail();
                HideLoading();
            }
        });
    }
}
function filterData() {
    showModal('myModalFilterDR');
}
function initTrackingStatus(header, units) {
    var statusUnitDesc = '';
    var statusUnit = '';
    var latestUpdateDate = null;
    var actionUnit = '';
    var x = 0;

    status = (header.Status != null) ? header.Status.toLocaleLowerCase() : '';

    for (x in units) {
        if (units[x].Status == "DLYD") {
            statusUnit = units[x].Status;
            statusUnitDesc = units[x].StatusDescription;
            break;
        }
        if (units[x].Status == "RDLY") {
            statusUnit = units[x].Status;
            statusUnitDesc = units[x].StatusDescription;
            continue;
        }
        if (statusUnit != "RDLY") {
            statusUnit = units[x].Status;
            statusUnitDesc = units[x].StatusDescription;
        }
    }

    x = 0;
    for (x in units) {
        if (units[x].Action == "INTR") {
            actionUnit = units[x].ActionDescription;
            break;
        }
        actionUnit = units[x].ActionDescription;
    }
    actionUnit = (actionUnit != null) ? actionUnit.toLocaleLowerCase() : '';

    $("#originTracking").html(header.Origin);
    $("#destinationTracking").html(header.CustName);
    $("#destTrackingAddress").html(header.CustAddress.replace(/[\n]/g, '<br/>'));
    if (header.ReRouted == true) {
        $('#flagReRaoute').removeClass('hidden');
    } else {
        $('#flagReRaoute').addClass('hidden');
    }
    
    var ExpTDTracking = 'ETD : ' + formatDateLocal(header.ExpectedTimeLoading, formatDateStr); // "ddd MM/DD/YYYY"
    var ExpTATracking = 'ETA : ' + formatDateLocal(header.ExpectedTimeArrival, formatDateStr);

    x = 0;
    var etd = eta = atd = ata = null;
    for (x in units){
        if (units[x].ActTimeDeparture != null && units[x].ActTimeDeparture != '') {
            if (atd == null || atd > moment(units[x].ActTimeDeparture)) {
                atd = moment(units[x].ActTimeDeparture);
            }
        }
        if (units[x].ActTimeArrival != null && units[x].ActTimeArrival != '') {
            if (ata == null || ata < moment(units[x].ActTimeArrival)) {
                ata = moment(units[x].ActTimeArrival);
            }
        }

        if (atd == null && units[x].EstTimeDeparture != null && units[x].EstTimeDeparture != '') {
            if (etd == null || etd > moment(units[x].EstTimeDeparture)) {
                etd = moment(units[x].EstTimeDeparture);
            }
        }
        if (ata == null && units[x].EstTimeArrival != null && units[x].EstTimeArrival != '') {
            if (eta == null || eta < moment(units[x].EstTimeArrival)) {
                eta = moment(units[x].EstTimeArrival);
            }
        }

        if (latestUpdateDate == null || latestUpdateDate < moment(units[x].UpdateDate)) {
            latestUpdateDate = moment(units[x].UpdateDate);
        }
    }

    if (atd != null) {
        ExpTDTracking = 'ATD : ' + atd.format(formatDateStr);
    } else if (etd != null) {
        ExpTDTracking = 'PICKUP PLAN : ' + etd.format(formatDateStr);
    }

    if (ata != null) {
        ExpTATracking = 'ATA : ' + ata.format(formatDateStr);
    } else if (eta != null) {
        ExpTATracking = 'ETA : ' + eta.format(formatDateStr);
    }

    $("#ExpTDTracking").html(ExpTDTracking);
    $("#ExpTATracking").html(ExpTATracking);

    $("#flowCreated").removeClass("activedr");
    $("#flowProgress").removeClass("activepr");
    $("#flowPickup").removeClass("activepck");
    $("#flowIntransit").removeClass("activeint");
    $("#flowPOD").removeClass("activepod");


    $("#flowPickup").removeClass("pck-risk-of-delay");
    $("#flowPickup").removeClass("pck-delay");
    $("#flowIntransit").removeClass("int-risk-of-delay");
    $("#flowIntransit").removeClass("int-delay");
    
    $("#flowCreated").addClass("dr");
    $("#flowProgress").addClass("pr");
    $("#flowPickup").addClass("pck");
    $("#flowIntransit").addClass("int");
    $("#flowPOD").addClass("pod");

    if (status != "reject") {
        $("#flowCreated").removeClass("dr");
        $("#flowCreated").addClass("activedr");
    }
    if (['approve', 'complete', 'rerouted', 'podc','podb'].indexOf(status) > -1) {
        $("#flowProgress").removeClass("pr");
        $("#flowProgress").addClass("activepr");
    }
    
    if (['complete', 'rerouted', 'podc','podb'].indexOf(status) > -1) {
        $("#flowPickup").removeClass("pck");
        $("#flowPickup").addClass("activepck");
        if ((statusUnitDesc == 'RISK OF DELAYED')) {
            $("#flowPickup").addClass("pck-risk-of-delay");
        }
        if ((statusUnitDesc == 'DELAYED')) {
            $("#flowPickup").addClass("pck-delay");
        }
    }
    if ((status == 'complete' || status == 'rerouted') && ['intransit', 'pod customer', 'pod branch'].indexOf(actionUnit) > -1) {
        $("#flowIntransit").removeClass("int");
        $("#flowIntransit").addClass("activeint");

        if ((statusUnitDesc == 'RISK OF DELAYED')) {
            $("#flowIntransit").addClass("int-risk-of-delay");
        }
        if ((statusUnitDesc == 'DELAYED')) {
            $("#flowIntransit").addClass("int-delay");
        }
    }
    if (['complete', 'rerouted'].indexOf(status) > -1 && (actionUnit === "pod customer" || actionUnit === "pod branch")) {
        $("#flowPOD").removeClass("pod");
        $("#flowPOD").addClass("activepod");
    }

    latestUpdateDate = (latestUpdateDate != null) ? latestUpdateDate.format(formatDateStr) : formatDateLocal(header.UpdateDate);
    $("#lblTrackingStatusDate").html("LATEST STATUS (" + latestUpdateDate + "): ");
    $("#lblTrackingStatus").html((statusUnitDesc != null) ? statusUnitDesc.toUpperCase() : '');
    $("#lblTrackingStatus").removeClass('label-dlyd label-rdly label-schd label-bast');
    $("#lblTrackingStatus").addClass('label-' + ((statusUnit != null) ? statusUnit.toLowerCase() : ''));

}

function resetFormLog() {
    $formLogStatus[0].reset();
    $("#Action").val(null).trigger("change");
    $("#Status").val(null).trigger("change");
    $("button[name=Save]").removeAttr("disabled");
}
function setDisabledFormLog(isdisabled) {
    if (isdisabled == false) {
        $("#Save").removeClass('hidden');
        $('div.ct-apply-to-all').removeClass('hidden');
        $('#formLogStatus ul.nav > li#tab1').removeClass('hidden');
        $('#formLogStatus ul.nav > li#tab1').addClass('active');
        $('#formLogStatus ul.nav > li#tab2').removeClass('active');
        $('#formLogStatus div.tab-content > #UnitInfo').addClass('in active');
        $('#formLogStatus div.tab-content > #TransportationInfo').removeClass('in active');
    } else {
        $("#Save").addClass('hidden');
        $('div.ct-apply-to-all').addClass('hidden');
        $('#formLogStatus ul.nav > li#tab1').addClass('hidden');
        $('#formLogStatus ul.nav > li#tab1').removeClass('active');
        $('#formLogStatus ul.nav > li#tab2').addClass('active');
        $('#formLogStatus div.tab-content > #UnitInfo').removeClass('in active');
        $('#formLogStatus div.tab-content > #TransportationInfo').addClass('in active');
    }
    $("form#formLogStatus :input").each(function () {
        $(this).attr("disabled", isdisabled);
    });
}
var formTransportation = {
    $formEl: null,
    data: null,
    initShow: function (header, units) {
        $('.freight').addClass('hidden');
        var SELF = formTransportation;
        if (header.ModaTransport == "SEA") {
            SELF.$formEl = $('#form-sea-freight');
            $('#sea-freight').removeClass('hidden');
            $('#VeselName').val(units.VeselName);
            $('#PICName').val(units.PICName);
            $('#PICHp').val(units.PICHp);
            $('#sea-freight').find('input[name="VeselNoPolice"]').val(units.VeselNoPolice);
            $('#sea-freight').find('input[name="DriverName"]').val(units.DriverName);
            $('#sea-freight').find('input[name="DriverHp"]').val(units.DriverHp);
        } else if (header.ModaTransport == "LAND") {
            SELF.$formEl = $('#form-land-freight');
            $('#land-freight').removeClass('hidden');
            $('#VeselNoPolice').val(units.VeselNoPolice);
            $('#DriverName').val(units.DriverName);
            $('#DriverHp').val(units.DriverHp);
        } else if (header.ModaTransport == "AIR") {
            SELF.$formEl = $('#form-air-freight');
            $('#air-freight').removeClass('hidden');
            $('#DANo').val(units.DANo);
            $('#air-freight').find('input[name="VeselNoPolice"]').val(units.VeselNoPolice);
            $('#air-freight').find('input[name="DriverName"]').val(units.DriverName);
            $('#air-freight').find('input[name="DriverHp"]').val(units.DriverHp);
        }

    }
}

var outboundTracking = {
    HeaderID: 0,
    selectedRow: null,
    logStatus: [],
    initTableUnit: function (units) {
        $("#tableOuboundUnit").bootstrapTable({
            cache: false,
            data: units,
            pagination: true,
            search: false,
            striped: true,
            clickToSelect: true,
            showColumns: false,
            showRefresh: false,
            smartDisplay: false,
            pageSize: '5',
            formatNoMatches: function () {
                return '<span class="noMatches">-</span>';
            },
            onClickRow: function (row, $element, field) {
                var SELF = outboundTracking;
                $("#tableLog > tbody").empty();
                // console.log(row.CustAddress);
                $('#destTrackingAddress').html(row.CustAddress);
                if (SELF.logStatus.length > 0) {
                    $.each(SELF.logStatus, function (index, value) {
                        if (value.RefItemId == row.RefItemId) {
                            var logDesc = (!value.LogDescription) ? "-" : value.LogDescription;
                            var TR = '<tr>' +
                                '<td width="60px">' + formatDateLocal(value.UpdateDate, formatDateStr) + '</td>' +
                                '<td>:</td>' +
                                '<td>' + logDesc + '</td>' +
                                '</tr>';
                            //$('#tableLog').append(TR);
                            $('#tableLog > tbody').append(TR);
                        }
                    });
                }
            },
            columns: [
                {
                    field: 'ID',
                    title: '#',
                    halign: 'center',
                    align: 'center',
                    width: '75px',
                    clickToSelect: false,
                    formatter: function (value, row, index) {
                        var htm = [];
                        if ((allowCreate == "True" || allowUpdate == "True") && $DRData.header.Status != 'reject') {
                            htm.push('<button class="editUnit btn btn-primary btn-xs" data-toggle="tooltip" data-placement="bottom" title="Edit"><i class="fa fa-edit"></i></button> ');
                            if ($DRData.header.ReRouted) {
                                htm.push('<button class="destinationUnit btn btn-warning btn-xs" data-toggle="tooltip" data-placement="bottom" title="Change Destination"><i class="fa fa-route"></i></button> ');
                            }
                        }
                        else {
                            if ($DRData.header.ModaTransport == 'SEA') {
                                htm.push('<button class="viewUnit btn btn-primary btn-xs" data-toggle="tooltip" data-placement="bottom" title="View"><i class="fa fa-ship"></i></button> ');

                            } else if ($DRData.header.ModaTransport == 'LAND') {
                                htm.push('<button class="viewUnit btn btn-primary btn-xs" data-toggle="tooltip" data-placement="bottom" title="View"><i class="fa fa-truck"></i></button> ');

                            } else if ($DRData.header.ModaTransport == 'AIR') {
                                htm.push('<button class="viewUnit btn btn-primary btn-xs" data-toggle="tooltip" data-placement="bottom" title="View"><i class="fa fa-plane"></i></button> ');

                            } else {
                                htm.push('<button class="viewUnit btn btn-primary btn-xs" data-toggle="tooltip" data-placement="bottom" title="View"><i class="fa fa-eye"></i></button> ');
                            }
                        }
                        return (['complete', 'rerouted', 'reject'].indexOf($DRData.header.Status) > -1) ? htm.join('') : '';
                    },
                    events: {
                        'click .editUnit': function (el, value, row) {
                            $('#modalLogStatus .modal-title').html('UPDATE TRACKING STATUS');
                            resetFormLog();
                            outboundTracking.selectedRow = row;
                            showModalLog();
                            if (row.EstTimeDeparture != null) $('#EstTimeDeparture').val(formatDateLocal(row.EstTimeDeparture));
                            if (row.ActTimeDeparture != null) $('#ActTimeDeparture').val(formatDateLocal(row.ActTimeDeparture));
                            if (row.EstTimeArrival != null) $('#EstTimeArrival').val(formatDateLocal(row.EstTimeArrival));
                            if (row.ActTimeArrival != null) $('#ActTimeArrival').val(formatDateLocal(row.ActTimeArrival));
                            formTransportation.initShow($DRData.header, row);
                            setDisabledFormLog(false);
                            if (row.Action == 'PODC') {                              
                                $('#VeselNoPolice').attr("disabled", "disabled");
                                $('#DriverName').attr("disabled", "disabled");
                                $('#DriverHp').attr("disabled", "disabled");                                
                            }
                        },
                        'click .viewUnit': function (el, value, row) {
                            $('#modalLogStatus .modal-title').html('TRACKING STATUS INFO');
                            resetFormLog();
                            outboundTracking.selectedRow = row;
                            showModalLog();
                            if (row.EstTimeDeparture != null) $('#EstTimeDeparture').val(formatDateLocal(row.EstTimeDeparture));
                            if (row.ActTimeDeparture != null) $('#ActTimeDeparture').val(formatDateLocal(row.ActTimeDeparture));
                            if (row.EstTimeArrival != null) $('#EstTimeArrival').val(formatDateLocal(row.EstTimeArrival));
                            if (row.ActTimeArrival != null) $('#ActTimeArrival').val(formatDateLocal(row.ActTimeArrival));

                            formTransportation.initShow($DRData.header, row);
                            //$formLogStatus.find('input').addAttr('disabled', 'disabled');
                            setDisabledFormLog(true);
                        },
                        'click .destinationUnit': function (el, value, row) {
                            //console.log(row);
                            outboundTrackingDestination.selectedRow = row;
                            var newOption2 = new Option(row.CustName || '', row.CustID, false, false);
                            $('#DCustID').append(newOption2).trigger('change');
                            $("#DCustID").val(row.CustID).trigger("change");
                            $("#DCustName").val(row.CustName || '');
                            $("#DCustAddress").val(row.CustAddress);
                            $("#DPicName").val(row.PICName);
                            $("#DPicHP").val(row.PICHp);
                            $("#DKecamatan").val(row.Kecamatan);
                            $("#DKabupaten").val(row.Kabupaten);
                            $("#DProvince").val(row.Province);
                            showModal('myModalDestination');
                        }
                    },
                    class: 'text-nowrap',
                    sortable: false,
                },
                {
                    field: 'HeaderID',
                    title: 'HeaderID',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    sortable: true,
                    visible: false,
                },
                {
                    field: 'RefItemId',
                    title: 'RefItemId',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    sortable: true,
                    visible: false,
                },
                {
                    field: 'UnitType',
                    title: 'TYPE',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    sortable: true
                },
                {
                    field: 'Model',
                    title: 'MODEL',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    formatter: formatUpperCase,
                    sortable: true
                },
                {
                    field: 'SerialNumber',
                    title: 'SN',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',                   
                    formatter: ActionFormatterSN,
                    events: EventsFormatter,
                    sortable: true
                },
                {
                    field: 'Batch',
                    title: 'BATCH',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    formatter: formatUpperCase,
                    sortable: true
                },
                {
                    field: 'EstTimeDeparture',
                    title: 'PICKUP PLAN',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    formatter: formatDateBT,
                    sortable: true
                },
                {
                    field: 'ActTimeDeparture',
                    title: 'ATD',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    formatter: formatDateBT,
                    sortable: true
                },
                {
                    field: 'EstTimeArrival',
                    title: 'ETA',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    formatter: formatDateBT,
                    sortable: true
                },
                {
                    field: 'ActTimeArrival',
                    title: 'ATA',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    formatter: formatDateBT,
                    sortable: true
                },
                {
                    field: 'Status',
                    title: 'STATUS',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    sortable: true,
                    visible: false,
                },
                {
                    field: 'StatusDescription',
                    title: 'STATUS',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    formatter: function (value, row, index, field) {
                        var labelStatus = (row.Status != null) ? row.Status.toLowerCase() : '';
                        return '<div class="label label-' + labelStatus + '" >' + formatUpperCase(value, row, index, field) + '</div>';
                    },
                    sortable: true
                },
                {
                    field: 'Action',
                    title: 'Action',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    sortable: true,
                    visible: false,
                },
                {
                    field: 'ActionDescription',
                    title: 'ACTIVITY',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    formatter: formatUpperCase,
                    sortable: true
                },
                {
                    field: 'Position',
                    title: 'POSITION',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    formatter: formatUpperCase,
                    sortable: true
                },
                {
                    field: 'Attachment1',
                    title: 'MAPS',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    formatter: function (value, row, index, field) {
                        var html = '';
                        if (row.Attachment1 != null && row.Attachment1 != '') {
                            var url = myApp.fullPath + row.Attachment1;
                            var fileName = row.Attachment1.split(/.*[\/|\\]/)[1];
                            html += '<a href="' + url + '" download><i class="fa fa-paperclip"></i></a>';
                        }
                        //if (row.Attachment2 != null && row.Attachment2 != '') {
                        //    var url = myApp.fullPath + row.Attachment2;
                        //    var fileName = row.Attachment2.split(/.*[\/|\\]/)[1];
                        //    html += '<a href="' + url + '" download><i class="fa fa-paperclip"></i></a>';
                        //}
                        return html;
                    },
                    sortable: false
                },
                {
                    field: 'Notes',
                    title: 'NOTES',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    visible: false,
                    sortable: true
                }

            ]
        });
        $("#tableOuboundUnitForPickup").bootstrapTable({
            cache: false,
            data: units,
            pagination: true,
            search: false,
            striped: true,
            clickToSelect: true,
            showColumns: false,
            showRefresh: false,
            smartDisplay: false,
            pageSize: '10',
            formatNoMatches: function () {
                return '<span class="noMatches">-</span>';
            },
            columns: [
                {
                    field: 'HeaderID',
                    title: 'HeaderID',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    sortable: true,
                    visible: false,
                },
                {
                    field: 'RefItemId',
                    title: 'RefItemId',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    sortable: true,
                    visible: false,
                },
                {
                    field: 'UnitType',
                    title: 'TYPE',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    sortable: true
                },
                {
                    field: 'Model',
                    title: 'MODEL',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    formatter: formatUpperCase,
                    sortable: true
                },
                {
                    field: 'SerialNumber',
                    title: 'SN',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    formatter: formatUpperCase,
                 
                    //events: EventsFormatter,
                    sortable: true
                },
                {
                    field: 'Batch',
                    title: 'BATCH',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    formatter: formatUpperCase,
                    sortable: true
                },
                {
                    field: 'EstTimeDeparture',
                    title: 'PICKUP PLAN',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    formatter: formatDateBT,
                    sortable: true
                },
                {
                    field: 'ActTimeDeparture',
                    title: 'ACTUAL TIME DEPARTURE',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    formatter: formatDateBT,
                    sortable: true
                },
                {
                    field: 'EstTimeArrival',
                    title: 'ESTIMATE TIME ARRIVAl',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    formatter: formatDateBT,
                    sortable: true
                },
                {
                    field: 'ActTimeArrival',
                    title: 'ACTUAL TIME ARRIVAL',
                    halign: 'center',
                    align: 'center',
                    class: 'text-nowrap',
                    formatter: formatDateBT,
                    sortable: true
                }
            ]
        });
    },
    refreshTableUnit: function (units) {
        $("#tableOuboundUnit").bootstrapTable('destroy');
        $("#tableOuboundUnitForPickup").bootstrapTable('destroy');
        //$("#tableOuboundUnit").bootstrapTable('refresh');
        outboundTracking.initTableUnit(units);
    }
}
function ActionFormatterSN(value, row, index) {
    var htm = [];
    htm.push('<a class="show-SN">'+ value +'</a>');
    return htm.join('');
}
function handleFileToTemp(e) {
    var ArrJsonData = [];
    var ep = new ExcelPlus({ showErrors: false, flashUsed: false });
    ep.openLocal(
        {
            "flashPath": myApp.fullPath + "/Scripts/ExcelPlus-master-2.4.1/swfobject/",
            "labelButton": "Open an Excel file",
            "idButton": "file-object",
        },
        function (status, message) { // On Ready
            if (!status) {
                sAlert('Warning', message, 'warning');
                return;
            }
            try {
                var SheetToUpload = "DTS - Outbound Tracking";
                var SheetNames = ep.getSheetNames();
                if (SheetNames.indexOf(SheetToUpload)) {
                    sAlert('Warning', 'The format of the template file is not appropriate. Sheet ' + SheetToUpload + ' not found', 'warning');
                    return false;
                }
                var XLSdata = ep.selectSheet(SheetToUpload).readAll();
                ArrJsonData = XlsToObjectNew(XLSdata);
            }
            catch (err) {
                console.log(err.message);
            }
        },
        function () { // OnUpload
            $.ajax({
                type: "POST",
                url: myApp.fullPath + "DTS/UploadTrackingHistory",
                data: JSON.stringify(ArrJsonData),
                dataType: 'json',
                traditional: true,
                contentType: 'application/json',
                beforeSend: function () {
                    ShowLoading();
                },
                success: function (d, status, xhr) {
                    HideLoading();
                    if (d.Status == 0) {
                        sAlert('Success', d.Msg, 'success');
                    } else {
                        sAlert('Error', d.Msg, 'error');
                    }
                },
                error: function (xhr, status, error) {
                    HideLoading();
                    console.error(error);
                }
            });
        }
    )
}
function XlsToObjectNew(XLSdata) {
    var ArrJsonData = [];
    for (var x in XLSdata) {
        if (x == 0) { // header file
            continue;
        }
        ArrJsonData.push({
            KeyCustom: XLSdata[x][0],
            Model: XLSdata[x][1],
            SerilaNumber: XLSdata[x][2],
            Batch: XLSdata[x][3],
            RefItemId: XLSdata[x][4],
            Action: XLSdata[x][5],
            Status: XLSdata[x][6],
            Position: XLSdata[x][7],
            ETD: XLSdata[x][8],
            ATD: XLSdata[x][9],
            ETA: XLSdata[x][10],
            ATA: XLSdata[x][11],
            Notes: XLSdata[x][12],
        });
    }

    return ArrJsonData;
}
function XlsToObject(XLSdata) {
    var ArrJsonData = [];
    for (var x in XLSdata) {
        if (x == 0) { // header file
            continue;
        }
        ArrJsonData.push({
            KeyCustom: XLSdata[x][0],
            SerilaNumber: XLSdata[x][1],
            Action: XLSdata[x][2], // (actionArr.length > 0) ? actionArr[0].trim() : '',
            Status: XLSdata[x][3], // (actionArr.length > 0) ? actionArr[0].trim() : '',
            Position: XLSdata[x][4],
            ETD: XLSdata[x][5],
            ATD: XLSdata[x][6],
            ETA: XLSdata[x][7],
            ATA: XLSdata[x][8],
            Notes: XLSdata[x][9],
        });
    }

    return ArrJsonData;
}
window.EventsFormatter = {
    'click .show-SN': function (e, value, row, index) {
        //$(".modal-title").html('Show Unit - #' + row.KeyCustom);
        $.ajax({
            type: "GET",
            url: myApp.root + 'DTS/GetSNVisionLink?SerialNumber=' + row.SerialNumber,
            beforeSend: function () { ShowLoading() },
            complete: function () { HideLoading() },
            dataType: "json",
            success: function (d) {
                if (d.length > 0) {
                    requestingFormSNVL.initTableSNVLInfo(d);
                    $("#myModalSNVisionLink").modal("show");
                } else {
                    invalidRef();
                    sAlert('Warning', "Data Vision Link Not Available", "warning");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                invalidRef();
                sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
            }
        });
    },
};
var outboundTrackingFilter = {
    table: $('#tableOutboundFilter'),
    columnList: [
        [
            {
                field: 'KeyCustom',
                title: 'DR NO.',
                halign: 'center',
                align: 'center',
                rowspan: 2,
                class: 'text-nowrap',
                filterControl: "input",
                sortable: true
            },
            {
                field: 'StatusTracking',
                title: 'ACTIVITY',
                //formatter: activityFormatter,
                halign: 'center',
                align: 'center',
                rowspan: 2,
                class: 'text-nowrap',
                filterControl: "input",
                sortable: true
            },
            {
                field: 'Status',
                title: 'STATUS',
                //formatter: statusFormatter,
                halign: 'center',
                align: 'center',
                rowspan: 2,
                class: 'text-nowrap',
                filterControl: "input",
                sortable: true
            },
            {
                field: 'ReqName',
                title: 'REQUESTOR',
                halign: 'center',
                align: 'center',
                rowspan: 2,
                class: 'text-nowrap',
                filterControl: "input",
                sortable: true
            },
            {
                field: 'Sales1Name',
                title: 'SALES NAME 1',
                halign: 'center',
                align: 'center',
                rowspan: 2,
                class: 'text-nowrap',
                filterControl: "input",
                sortable: true
            },
            {
                field: 'Model',
                title: 'MODEL',
                halign: 'center',
                align: 'center',
                rowspan: 2,
                class: 'text-nowrap',
                filterControl: "input",
                sortable: true
            },
            {
                field: 'SerialNumber',
                title: 'SN',
                halign: 'center',
                align: 'center',
                rowspan: 2,
                class: 'text-nowrap',
                filterControl: "input",
                sortable: true
            },
            {
                field: 'Origin',
                title: 'FROM (ORIGIN)',
                halign: 'center',
                align: 'left',
                class: 'text-nowrap',
                rowspan: 2,
                filterControl: "input",
                formatter: formatUpperCase,
                sortable: true
            },
            {
                field: '',
                title: 'DESTINATION',
                halign: 'center',
                colspan: 2,
                align: 'left',
                class: 'text-nowrap'
            },
            {
                field: '',
                title: 'RECEIVER',
                halign: 'center',
                colspan: 2,
                align: 'left',
                class: 'text-nowrap'
            },
            {
                field: 'ExpectedTimeLoading',
                title: 'ETD',
                halign: 'center',
                align: 'left',
                class: 'text-nowrap',
                rowspan: 2,
                sortable: true,
                filterControl: "datepicker",
                filterDatepickerOptions: { format: 'yyyy-mm-dd' },
                formatter: formatDateBT,
            },
        ],
        [
            {
                field: 'CustName',
                title: 'CUSTOMER NAME',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                filterControl: "input",
                formatter: formatUpperCase,
                sortable: true
            },
            {
                field: 'Kabupaten',
                title: 'DISTRICT',
                halign: 'center',
                align: 'left',
                class: 'text-nowrap',
                filterControl: "input",
                formatter: formatUpperCase,
                sortable: true,
            },
            {
                field: 'PicName',
                title: 'NAME',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                filterControl: "input",
                formatter: formatUpperCase,
                sortable: true
            },
            {
                field: 'PicHP',
                title: 'CONTACT',
                halign: 'center',
                align: 'left',
                class: 'text-nowrap',
                filterControl: "input",
                sortable: true
            },
        ]
    ],
    init: function () {
        this.table.bootstrapTable({
            cache: false,
            pagination: true,
            search: false,
            striped: true,
            clickToSelect: true,
            sidePagination: 'server',
            showColumns: false,
            showRefresh: true,
            smartDisplay: false,
            pageSize: '10',
            filterControl: true,
            formatNoMatches: function () {
                return '<span class="noMatches">-</span>';
            },
            rowStyle: function(row, index) {
                return {
                    css: { cursor: 'pointer'}
                }
            },
            onExpandRow: function (index, row, $detail) {
                $detail.html('<span class="text-center" style="font-size:16px;"><i class="fa fa-spinner fa-pulse fa-fw"></i></span> Loading, please wait...');
                $.ajax({
                    url: "/DTS/PartialListDetail",
                    dataType: "html",
                    method: 'GET',
                    data: { InboundID: row.AjuNo },
                    success: function (resultHtml) {
                        $detail.html(resultHtml);
                    },
                    error: function (e) {
                        $detail.html("Data not found");
                    }
                });
            },
            onClickRow: function (row, $element, field) {
                $("#searchKey").val(row.KeyCustom);
                $('#myModalFilterDR').modal("hide");
            },
            columns: this.columnList
        });
        window.pis.table({
            objTable: this.table,
            urlSearch: '/DTS/DeliveryRequisitionPage',
            urlPaging: '/DTS/DeliveryRequisitionPageXt',
            searchParams: {
                searchName: $("input[name=searchText]").val(),
                requestor: (userRoleName == 'DTSRequestor'), // true
            },
            autoLoad: true
        });

        $('button[name="CancelFilter"]').click(function () {
            $('#myModalFilterDR').modal("hide");
        });
    }
}

var outboundTrackingDestination = {
    selectedRow: null,
    init: function () {
        var SELF = this;
        $("#DCustID").select2({
            placeholder: 'Nama Customer',
            //dropdownParent: $('#myModalRequest'),
            minimumInputLength: 3,
            ajax: {
                url: myApp.root + 'DTS/getMasterCustomer',
                async: false,
                dataType: 'json',
                data: function (params) {
                    var query = {
                        key: params.term,
                        type: 'public'
                    };
                    return query;
                },
                processResults: function (data) {
                    var newData = $.map(data, function (obj) {
                        obj.id = obj.Customer_ID_SAP;
                        obj.text = obj.Customer_Full_Name;
                        return obj;
                    });
                    return {
                        results: newData
                    };
                }
            }
        });
        $('#DCustID').on('select2:select', function (e) {
            var data = e.params.data;
            $("div[for=DCustID]").hide();
            $("input[name=DCustName]").val(data.Customer_Full_Name);
            $("textarea[name=DCustAddress]").val(data.Jalan);
        });
        $formDestinationUnit = $('#formDestinationUnit');
        var formDesValidator = $formDestinationUnit.validate({
            highlight: function (element, errorClass, validClass) {
                $formDestinationUnit.find("div[for=" + element.name + "]").show();
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass(errorClass).addClass(validClass);
                $formDestinationUnit.find("div[for=" + element.name + "]").hide();
            }
        });
        $("#SaveDestination").click(function () {
            var isValid = $formDestinationUnit.valid();
            if (isValid) {
                var formData = SELF.selectedRow;
                formData.CustID = $('#DCustID').val();
                formData.CustName = $('#DCustName').val();
                formData.CustAddress = $('#DCustAddress').val();
                formData.PICName = $('#DPicName').val();
                formData.PICHp = $('#DPicHP').val();
                formData.Kecamatan = $('#DKecamatan').val();
                formData.Kabupaten = $('#DKabupaten').val();
                formData.Province = $('#DProvince').val();
                $.ajax({
                    type: "POST",
                    url: myApp.root + 'DTS/FormDestinationUnit',
                    beforeSend: function () {
                        ShowLoading();
                        $("button[name=SaveDestination]").attr("disabled", "disabled");
                    },
                    complete: function () {
                        HideLoading();
                        $("button[name=SaveDestination]").attr("disabled", false);
                    },
                    dataType: "json",
                    data: formData,
                    dataType: "json",
                    success: function (d) {
                        if (d.Status == 0) {
                            searchData();
                            sAlert('Success', 'Change Destination Success', 'success');
                        } else {
                            $("button[name=SaveDestination]").removeAttr("disabled");
                            sAlert('Error', d.Msg, 'error');
                        }
                        resetFormLog();
                        $("[name=refresh]").trigger('click');
                        hideModal('myModalDestination');

                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $("button[name=Save]").removeAttr("disabled");
                        sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
                    }
                });
            } else {
                $.each(formDesValidator.errorMap, function (index, value) {
                    console.log('Id: ' + index + ' Message: ' + value);
                });
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
            { field: 'DIDate', title: 'DI Date', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap', formatter: dateFormatterCAT },
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
<<<<<<< HEAD
<<<<<<< HEAD
            url: myApp.fullPath + "DTS/DownloadOutbound",
=======
            url: myApp.fullPath  + "DTS/DownloadOutbound",
>>>>>>> 639d8d0 (Intial commit)
=======
            url: myApp.fullPath + "DTS/DownloadOutbound",
>>>>>>> 93c2efe ([U] Update from client's TFS)
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
<<<<<<< HEAD
<<<<<<< HEAD
                window.open(myApp.root + 'DTS/DownloadToExcelOutbound?guid=' + guid, '_blank');
=======
                window.open(myApp.root +'DTS/DownloadToExcelOutbound?guid=' + guid, '_blank');
>>>>>>> 639d8d0 (Intial commit)
=======
                window.open(myApp.root + 'DTS/DownloadToExcelOutbound?guid=' + guid, '_blank');
>>>>>>> 93c2efe ([U] Update from client's TFS)
            },
            cache: false
        });
    });

    $("#searchKey").keypress(function (e) {
        if (e.which == 13) {
            searchData();
        }
    });

    $("#SupportingDocument").hover(function () {
        if ($(this).attr("download-url") !== "") {
            $(this).css("text-decoration", "underline");
        }
    }).mouseout(function () {
        $(this).css("text-decoration", "inherit");
    });
    $("#SupportingDocument").click(function () {
        if ($(this).attr("download-url") !== "") {
            var fileName = $(this).html();
            var url = $(this).attr("download-url");
            fetch(url).then(function (t) {
                return t.blob().then(function (b) {
                    var a = document.createElement("a");
                    a.href = URL.createObjectURL(b);
                    a.setAttribute("download", fileName);
                    a.click();
                });
            }).catch(function (e) {
                sAlert('Failed!', "File not found.", 'error');
            });
        }
    });

    $("#Action").select2();
    $("#Status").select2();

    $.ajax({
        type: "POST",
<<<<<<< HEAD
<<<<<<< HEAD
        url: myApp.fullPath + 'DTS/GetMasterAction',
=======
        url: myApp.fullPath  + 'DTS/GetMasterAction',
>>>>>>> 639d8d0 (Intial commit)
=======
        url: myApp.fullPath + 'DTS/GetMasterAction',
>>>>>>> 93c2efe ([U] Update from client's TFS)
        dataType: "json",
        success: function (result) {
            if (result && result.length > 0) {
                var items = [];
                for (var x in result) {
<<<<<<< HEAD
<<<<<<< HEAD
                    items.push({ id: result[x].Code, text: result[x].Description1 });
=======
                    items.push({ id: result[x].Code, text: result[x].Description1});
>>>>>>> 639d8d0 (Intial commit)
=======
                    items.push({ id: result[x].Code, text: result[x].Description1 });
>>>>>>> 93c2efe ([U] Update from client's TFS)
                }
                $("#Action").select2({ placeholder: 'Select Activity', data: items });
                $("#Action").val(null).trigger("change");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("button[name=Save]").removeAttr("disabled");
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });

    $.ajax({
        type: "POST",
<<<<<<< HEAD
<<<<<<< HEAD
        url: myApp.fullPath + 'DTS/GetMasterStatus',
=======
        url: myApp.fullPath  + 'DTS/GetMasterStatus',
>>>>>>> 639d8d0 (Intial commit)
=======
        url: myApp.fullPath + 'DTS/GetMasterStatus',
>>>>>>> 93c2efe ([U] Update from client's TFS)
        dataType: "json",
        success: function (result) {
            if (result && result.length > 0) {
                var items = [];
                for (var x in result) {
                    items.push({ id: result[x].Code, text: result[x].Description1 });
                }
                $("#Status").select2({ placeholder: 'Select Status', data: items });
                $("#Status").val(null).trigger("change");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("button[name=Save]").removeAttr("disabled");
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });

    outboundTracking.initTableUnit([]);
    $("#Save").click(function () {
        var isAnyUpdate = false;
        if ($('#Action').val() != null && $('#Action').val() != "") {
            isAnyUpdate = true;
            outboundTracking.selectedRow.Action = $('#Action').val();
        }
        if ($('#Status').val() != null && $('#Status').val() != "") {
            isAnyUpdate = true;
            outboundTracking.selectedRow.Status = $('#Status').val();
        }
        if ($('#Position').val() != "") {
            isAnyUpdate = true;
            outboundTracking.selectedRow.Position = $('#Position').val();
        }

        if ($('#EstTimeDeparture').val() != "" && $('#EstTimeDeparture').val() != formatDateLocal(outboundTracking.selectedRow.EstTimeDeparture)) {
            isAnyUpdate = true;
            outboundTracking.selectedRow.EstTimeDeparture = $('#EstTimeDeparture').val();
        } else {
            outboundTracking.selectedRow.EstTimeDeparture = null;
        }
        if ($('#ActTimeDeparture').val() != "" && $('#ActTimeDeparture').val() != formatDateLocal(outboundTracking.selectedRow.ActTimeDeparture)) {
            isAnyUpdate = true;
            outboundTracking.selectedRow.ActTimeDeparture = $('#ActTimeDeparture').val();
        } else {
            outboundTracking.selectedRow.ActTimeDeparture = null;
        }
        if ($('#EstTimeArrival').val() != "" && $('#EstTimeArrival').val() != formatDateLocal(outboundTracking.selectedRow.EstTimeArrival)) {
            isAnyUpdate = true;
            outboundTracking.selectedRow.EstTimeArrival = $('#EstTimeArrival').val();
        } else {
            outboundTracking.selectedRow.EstTimeArrival = null;
        }
        if ($('#ActTimeArrival').val() != "" && $('#ActTimeArrival').val() != formatDateLocal(outboundTracking.selectedRow.ActTimeArrival)) {
            isAnyUpdate = true;
            outboundTracking.selectedRow.ActTimeArrival = $('#ActTimeArrival').val();
        } else {
            outboundTracking.selectedRow.ActTimeArrival = null;
        }

        if ($('#Notes').val() != "") {
            isAnyUpdate = true;
            outboundTracking.selectedRow.Notes = $('#Notes').val();
        }

        if ($('#ApplyToAll').is(":checked") == true) {
            outboundTracking.selectedRow.VeselNoPolice = null;
            outboundTracking.selectedRow.DriverName = null;
            outboundTracking.selectedRow.DriverHp = null;
        }

        if ($DRData.header.ModaTransport == "SEA") {
            if ($('#VeselName').val() != "") {
                isAnyUpdate = true;
            }
            if ($('#PICName').val() != "") {
                isAnyUpdate = true;
            }
            if ($('#PICHp').val() != "") {
                isAnyUpdate = true;
            }
            outboundTracking.selectedRow.VeselName = $('#VeselName').val();
            outboundTracking.selectedRow.PICName = $('#PICName').val();
            outboundTracking.selectedRow.PICHp = $('#PICHp').val();

            if ($('#ApplyToAll').is(":checked") == false) {
                outboundTracking.selectedRow.VeselNoPolice = $('#sea-freight').find('input[name="VeselNoPolice"]').val();
                outboundTracking.selectedRow.DriverName = $('#sea-freight').find('input[name="DriverName"]').val();
                outboundTracking.selectedRow.DriverHp = $('#sea-freight').find('input[name="DriverHp"]').val();
            }

        } else if ($DRData.header.ModaTransport == "LAND") {
            outboundTracking.selectedRow.VeselNoPolice = $('#VeselNoPolice').val();
            outboundTracking.selectedRow.DriverName = $('#DriverName').val();
            outboundTracking.selectedRow.DriverHp = $('#DriverHp').val();
        } else if ($DRData.header.ModaTransport == "AIR") {
            if ($('#DANo').val() != "") {
                isAnyUpdate = true;
            }
            outboundTracking.selectedRow.DANo = $('#DANo').val();
            if ($('#ApplyToAll').is(":checked") == false) {
                outboundTracking.selectedRow.VeselNoPolice = $('#air-freight').find('input[name="VeselNoPolice"]').val();
                outboundTracking.selectedRow.DriverName = $('#air-freight').find('input[name="DriverName"]').val();
                outboundTracking.selectedRow.DriverHp = $('#air-freight').find('input[name="DriverHp"]').val();
            }
        }

        if ($('#ApplyToAll').is(":checked") == false) {
            if ($('#VeselNoPolice').val() != "") {
                isAnyUpdate = true;
            }
            if ($('#DriverName').val() != "") {
                isAnyUpdate = true;
            }
            if ($('#DriverHp').val() != "") {
                isAnyUpdate = true;
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD


        if ((outboundTracking.selectedRow.Action != 'PODC' && outboundTracking.selectedRow.Action != 'PODB') && outboundTracking.selectedRow.Status == 'BAST') {
            sAlert('Warning', "BAST status can be selected if the Activity is POD", 'warning');
            return;
        }
        if ((outboundTracking.selectedRow.Action == 'PODC' || outboundTracking.selectedRow.Action == 'PODB') && outboundTracking.selectedRow.Status != 'BAST') {
            sAlert('Warning', "Activity POD must selected BAST status ", 'warning');
            return;
        }
=======
=======


>>>>>>> 93c2efe ([U] Update from client's TFS)
        if ((outboundTracking.selectedRow.Action != 'PODC' && outboundTracking.selectedRow.Action != 'PODB') && outboundTracking.selectedRow.Status == 'BAST') {
            sAlert('Warning', "BAST status can be selected if the Activity is POD", 'warning');
            return;
        }
        if ((outboundTracking.selectedRow.Action == 'PODC' || outboundTracking.selectedRow.Action == 'PODB') && outboundTracking.selectedRow.Status != 'BAST') {
            sAlert('Warning', "Activity POD must selected BAST status ", 'warning');
            return;
        }
<<<<<<< HEAD
       
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)
        if (!isAnyUpdate) {
            sAlert('Warning', "no data changed", 'warning');
            return;
        }

        var isValid = $formLogStatus.valid();
        if (isValid) {
            var formData = new FormData();
            for (var name in outboundTracking.selectedRow) {
                var val = outboundTracking.selectedRow[name]; // == "null" ? null : dataForm[name];
                if (val != null && val != "null") {
                    formData.append(name, val);
                }
            }
            formData.append("Attachment1", $('#Attachment1')[0].files[0]);
            formData.append("IsApplyToAll", $('#ApplyToAll').is(":checked"));
            $.ajax({
                type: "POST",
                url: myApp.root + 'DTS/FormLogProcess',
                beforeSend: function () {
                    ShowLoading();
                    $("button[name=Save]").attr("disabled", "disabled");
                },
                complete: function () {
                    HideLoading();
                },
                data: formData,
                dataType: "json",
                contentType: false,
                processData: false,
                success: function (d) {
                    if (d.Status == 0) {
                        searchData();
                        sAlert('Success', 'Save data Success', 'success');
                    } else {
                        $("button[name=Save]").removeAttr("disabled");
                        sAlert('Error', d.Msg, 'error');
                    }
                    resetFormLog();
                    $("[name=refresh]").trigger('click');
                    hideModalLog();
                    
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("button[name=Save]").removeAttr("disabled");
                    sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
                }
            });
        } else {
            $.each(formLogValidator.errorMap, function (index, value) {
                console.log('Id: ' + index + ' Message: ' + value);
            });
        }
    });

    $('#flowCreated').click(function () {
        $("button[name=ReRoute]").hide();
        $form.find("input[name=formType]").val("V");
        $("button[name=Reject]").hide();
        $("button[name=Complete]").hide();
        $("button[name=ForceComplete]").hide();
        $("button[name=Approve]").hide();
        $("button[name=Revise]").hide();
        $("button[name=Cancel]").show();
        showModal();
        $('.btn-refnotype').removeClass('active');
        if ($DRData.header.RefNoType == 'SO') {
            $("#refSONo").parent().addClass('active')
        } else if ($DRData.header.RefNoType == 'STR') {
            $("#refSTRNo").parent().addClass('active')
        } else if ($DRData.header.RefNoType == 'DI') {
            $("#refDINo").parent().addClass('active')
        }
        referenceEvent.fillFromSo($DRData, "V");
        $("#myModalRequest .modal-title").html('VIEW - #' + $DRData.header.KeyCustom);
    });
    $('#flowProgress').click(function () {
        console.log($DRData.header);
        $('#myModalIconPreparation input[name="SoNo"]').val($DRData.header.SoNo);
        $('#myModalIconPreparation input[name="SoDate"]').val(formatDate($DRData.header.SoDate));
        $('#myModalIconPreparation input[name="STRNo"]').val($DRData.header.STRNo);
        $('#myModalIconPreparation input[name="STRDate"]').val(formatDate($DRData.header.STRDate));
        $('#myModalIconPreparation input[name="STONo"]').val($DRData.header.STONo);
        $('#myModalIconPreparation input[name="STODate"]').val(formatDate($DRData.header.STODate));
        $('#myModalIconPreparation input[name="DoNo"]').val($DRData.header.DoNo);
        $('#myModalIconPreparation input[name="OdDate"]').val(formatDate($DRData.header.OdDate));
        $('#myModalIconPreparation input[name="DINo"]').val($DRData.header.DINo);
        $('#myModalIconPreparation input[name="DIDate"]').val(formatDate($DRData.header.DIDate));

        showModal('myModalIconPreparation');
    });
    $('#flowPickup').click(function () {
        showModal('myModalIconPickup');
    });
    
    $('#BtnDownloadTpl').click(function () {
        window.open(myApp.fullPath  + 'DTS/DownloadTemplateUploadDR', '_blank');
    });
    $('#ApplyToAll').change(function () {
        if (this.checked) {
            if ($DRData.header.ModaTransport == "SEA") {
                $('#sea-freight .driver-info').addClass('hidden');
            } else if ($DRData.header.ModaTransport == "AIR") {
                $('#air-freight .driver-info').addClass('hidden');
            }
        } else {
            if ($DRData.header.ModaTransport == "SEA") {
                $('#sea-freight .driver-info').removeClass('hidden');
            } else if ($DRData.header.ModaTransport == "AIR") {
                $('#air-freight .driver-info').removeClass('hidden');
            }
        }
    });

    handleFileToTemp();
    outboundTrackingFilter.init();
    outboundTrackingDestination.init();
});



