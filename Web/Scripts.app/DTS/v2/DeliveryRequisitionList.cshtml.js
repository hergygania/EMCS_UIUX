$table = $('#tableDeliveryRequisition'); 
$searchInput = $("#txtSearchData").val();
function statusFormatter(str, index, row) {
    color = '';
    text = '';
    //console.log(str);
    switch (str) {
        case 'approve':
            color = 'info';
            text = 'IN PROGRESS';
            icon = 'fa fa-hourglass-start';
            break;
        case 'reject':
            color = 'danger';
            text = 'REJECTED';
            icon = 'fa fa-times';
            break;
        case 'revise':
            color = 'warning';
            text = 'NEED REVISION';
            icon = 'fa fa-edit';
            break;
        case 'revised':
            color = 'primary';
            text = 'REVISED';
            icon = 'fa fa-reply';
            break;
        case 'submit':
            color = 'primary';
            text = 'SUBMITED';
            icon = 'fa fa-paper-plane';
            break;
        case 'complete':
            color = 'success';
            text = 'COMPLETED';
            icon = 'fa fa-check-circle';
            break;
        case 'booked':
            color = 'warning';
            text = 'BOOKED';
            icon = 'fa fa-hourglass-start';
            break;
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)
        case 'request rerouted':
            color = 'warning';
            text = 'REQUEST REROUTED';
            icon = 'fa fa-hourglass-start';
            break;
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)
        case 'rerouted':
            color = 'warning';
            text = 'REROUTED';
            icon = 'fa fa-hourglass-start';
            break;
        default:
            color = 'default';
            text = 'DRAFT';
            icon = 'fa fa-file';
            break;
    }
    return "<div class='label label-" + color + "' style='width:100px;'>" + "<i class='" + icon + "'></i> " + text + "</div>";
}

function tooltip() {
    $('[data-toggle="tooltip"]').tooltip()
}

function ActionFormatter(value, row, index) {
    var htm = [];
    htm.push('<button class="view btn btn-info btn-xs" data-toggle="tooltip" data-placement="bottom" title="View"><i class="fa fa-eye"></i></button> ');
<<<<<<< HEAD
<<<<<<< HEAD
    if (row.Status === 'draft' || row.Status === 'revise' || row.Status === 'request rerouted') {
=======
    if (row.Status === 'draft' || row.Status === 'revise' || row.Status === 'rerouted') {
>>>>>>> 639d8d0 (Intial commit)
=======
    if (row.Status === 'draft' || row.Status === 'revise' || row.Status === 'request rerouted') {
>>>>>>> 93c2efe ([U] Update from client's TFS)
        if (allowUpdate === "True") htm.push('<button class="edit btn btn-primary btn-xs" data-toggle="tooltip" data-placement="bottom" title="Edit"><i class="fa fa-edit"></i></button> ');
        if (row.Status !== "complete") {
            if (allowDelete === "True") htm.push('<button class="remove btn btn-danger btn-xs" data-toggle="tooltip" data-placement="bottom" title="Delete"><i class="fa fa-trash"></i></button> ');
        }
    }
    if (row.Status === "complete" && row.RefNoType === "STR") {
<<<<<<< HEAD
<<<<<<< HEAD
        if (allowUpdate === "True") htm.push('<button class="reroute btn btn-warning btn-xs" data-toggle="tooltip" data-placement="bottom" title="Request Reroute"><i class="fa fa-route"></i></button> ');
=======
        if (allowUpdate === "True") htm.push('<button class="reroute btn btn-warning btn-xs" data-toggle="tooltip" data-placement="bottom" title="Reroute"><i class="fa fa-route"></i></button> ');
>>>>>>> 639d8d0 (Intial commit)
=======
        if (allowUpdate === "True") htm.push('<button class="reroute btn btn-warning btn-xs" data-toggle="tooltip" data-placement="bottom" title="Request Reroute"><i class="fa fa-route"></i></button> ');
>>>>>>> 93c2efe ([U] Update from client's TFS)
    }
    return htm.join('');
}
function ActionFormatterUnit(value, row, index) {
    var htm = [];
    htm.push('<button class="show-unit btn btn-info btn-xs" data-toggle="tooltip" data-placement="bottom" title="Show Unit"><i class="fa fa-list-ol"></button>');
    return htm.join('');
}
function operateFormatter(options) {
    var btn = [];
    btn.push('<button onclick = "showHistoryTable()" class="btn btn-xs btn-primary" alt="Edit remarks">');
    btn.push('<i class="fa fa-edit"></i>');
    btn.push('</button>');
    return btn.join('');
}
function showModal() {
    $("#myModalRequest").modal("show");
}
function deleteDeliveryRequisition(ID) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/DeliveryRequisitionDelete',
        beforeSend: function () {
            ShowLoading();
        },
        complete: function () {
            HideLoading();
        },
        data: { ID: ID },
        dataType: "json",
        success: function (d) {
            if (d.Msg !== undefined) {
                sAlert('Success', d.Msg, 'success');
            }
            $("[name=refresh]").trigger('click');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
}
window.EventsFormatter = {
    'click .remove': function (e, value, row, index) {
        referenceEvent.formType = "D";
        swal({
            title: "Are you sure want to delete this data?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#F56954",
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            closeOnConfirm: false,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                sweetAlert.close();
                return deleteDeliveryRequisition(row.ID);
            }
        });
    },
    'click .edit': function (e, value, row, index) {
        $(".modal-title").html('EDIT - #' + row.KeyCustom);
        $form.find("input[name=formType]").val("U");
        referenceEvent.formType = "U";
        $("button[name=ReRoute]").hide();
        if (row.Status === "draft") {
            $("button[name=Cancel]").show();
            $("button[name=SaveAsDraft]").show();
            $("button[name=SubmitForm]").show();
            $("button[name=SaveAsRevised]").hide();
        } else if (row.Status === "revise") {
            $("button[name=Cancel]").show();
            $("button[name=SaveAsDraft]").hide();
            $("button[name=SubmitForm]").hide();
            $("button[name=SaveAsRevised]").show();
        } else if (row.Status === "complete") {
            $("button[name=Cancel]").show();
            $("button[name=SaveAsDraft]").hide();
            $("button[name=SubmitForm]").hide();
            $("button[name=SaveAsRevised]").hide();
        } else {
            $("button[name=Cancel]").show();
            $("button[name=SaveAsDraft]").show();
            $("button[name=SubmitForm]").show();
            $("button[name=SaveAsRevised]").hide();
        }

        $.ajax({
            type: "GET",
            url: myApp.root + 'DTS/GetDRDetails?number=' + row.ID,
            beforeSend: function () { ShowLoading() },
            complete: function () { HideLoading() },
            dataType: "json",
            success: function (d) {
                if (d) {
                    showModal();
                    $('.btn-refnotype').removeClass('active');
                    if (d.header.RefNoType == 'SO') {
                        $("#refSONo").parent().addClass('active')
                    } else if (d.header.RefNoType == 'STR') {
                        $("#refSTRNo").parent().addClass('active')
                    } else if (d.header.RefNoType == 'PO') {
                        $("#refPONo").parent().addClass('active')
                    } else if (d.header.RefNoType == 'DI') {
                        $("#refDINo").parent().addClass('active')
                    }
                    referenceEvent.fillFromSo(d,"U");
                    //showFormRequisition();
                } else {
                    invalidRef();
                    sAlert('Error',"Data not found!", "error");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                invalidRef();
                sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
                // sAlert('Error', "SO NO: " + refNo + " IS NOT VALID!", "error");
            }
        });
    },
    'click .view': function (e, value, row, index) {
        $(".modal-title").html('VIEW - #' + row.KeyCustom);
        $form.find("input[name=formType]").val("V");
        referenceEvent.formType = "V";
        $("button[name=Cancel]").show();
        $("button[name=SaveAsDraft]").hide();
        $("button[name=SubmitForm]").hide();
        $("button[name=SaveAsRevised]").hide();
        $("button[name=ReRoute]").hide();
        $.ajax({
            type: "GET",
            url: myApp.root + 'DTS/GetDRDetails?number=' + row.ID,
            beforeSend: function () { ShowLoading() },
            complete: function () { HideLoading() },
            dataType: "json",
            success: function (d) {
                if (d) {
                    showModal();
                    $('.btn-refnotype').removeClass('active');
                    if (d.header.RefNoType === 'SO') {
                        $("#refSONo").parent().addClass('active');
                    } else if (d.header.RefNoType === 'STR') {
                        $("#refSTRNo").parent().addClass('active');
                    } else if (d.header.RefNoType === 'PO') {
                        $("#refPONo").parent().addClass('active');
                    }else if (d.header.RefNoType === 'DI') {
                        $("#refDINo").parent().addClass('active');
                    }
                    referenceEvent.fillFromSo(d, "V");
                } else {
                    invalidRef();
                    sAlert('Error', "Data not found!", "error");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                invalidRef();
                sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
                // sAlert('Error', "SO NO: " + refNo + " IS NOT VALID!", "error");
            }
        });
    },
    'click .show-unit': function (e, value, row, index) {
        $(".modal-title").html('Show Unit - #' + row.KeyCustom);
        $.ajax({
            type: "GET",
            url: myApp.root + 'DTS/GetDRUnits?number=' + row.ID,
            beforeSend: function () { ShowLoading() },
            complete: function () { HideLoading() },
            dataType: "json",
            success: function (d) {
                if (d) {
                    requestingFormModalUnit.initTableUnitInfo(d);
                    $("#myModalUnitOnly").modal("show");
                } else {
                    invalidRef();
                    sAlert('Error', "Data not found!", "error");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                invalidRef();
                sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
            }
        });
    },
    'click .reroute': function (e, value, row, index) {
        $form.find("input[name=formType]").val("R");
        $("#myModalRequest .modal-title").html('REROUTE - #' + row.KeyCustom);
        $("button[name=Cancel]").show();
        $("button[name=SaveAsDraft]").hide();
        $("button[name=SubmitForm]").hide();
        $("button[name=SaveAsRevised]").hide();
        $("button[name=ReRoute]").show();
        $("button[name=ReRoute]").attr("disabled", true);

        $.ajax({
            type: "GET",
            url: myApp.root + 'DTS/GetDRDetails?number=' + row.ID,
            beforeSend: function () { ShowLoading() },
            complete: function () { HideLoading() },
            dataType: "json",
            success: function (d) {
                if (d) {
                    rowSelected = d;
                    showModal();
                    $('.btn-refnotype').removeClass('active');
                    if (d.header.RefNoType == 'SO') {
                        $("#refSONo").parent().addClass('active')
                    } else if (d.header.RefNoType == 'STR') {
                        $("#refSTRNo").parent().addClass('active')
                    } else if (d.header.RefNoType == 'PO') {
                        $("#refPONo").parent().addClass('active')
                    } else if (d.header.RefNoType == 'DI') {
                        $("#refDINo").parent().addClass('active')
                    }
                    referenceEvent.formType = "R";
                    referenceEvent.fillFromSo(d, "R");
                } else {
                    invalidRef();
                    sAlert('Error', "Data not found!", "error");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                invalidRef();
                sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
            }
        });
    }
};
function resetForm() {
    $("#formRequest")[0].reset();
    resetReference();
    resetFormRequisition();
    $("[name=refresh]").trigger('click');
}
function resetReference() {
    $("#refSONo").parent().addClass('active');
    $("#refSTRNo").parent().removeClass('active');
    $("#refPONo").parent().removeClass('active');
    $("#refDINo").parent().removeClass('active');

    $("#refSONo").parent().removeClass('hidden');
    $("#refSTRNo").parent().removeClass('hidden');
    $("#refPONo").parent().removeClass('hidden');
    $("#refDINo").parent().removeClass('hidden');
    $('#btn-group-refnotype label.btn').removeClass('hidden');
}
function resetFormRequisition() {
    $("#formRequest")[0].reset();
    $(".error").hide();
    $("#formRequest").find('input').attr('disabled', false);
    $("select[name=Sales1Name]").attr("disabled", false);
    $("select[name=Sales2Name]").attr("disabled", false);
    $("select[name=CustID]").attr("disabled", false);
    $("select[name=ProvinceName]").attr("disabled", false);
    $("select[name=DistrictName]").attr("disabled", false);
    $("select[name=SubDistrictName]").attr("disabled", false);
    $form.find("textarea[name=CustAddress]").attr("disabled", false);
    //$("#ProvinceID").select2("val", "");
    //$("#DistrictID").select2("val", "");
    //$("#SubDistrictID").select2("val", "");
    $("#formRequisition").addClass('hidden');
    $("button[name=SaveAsBooking]").hide();
    $("#refDate").html('');
    $("#refDate").hide();

    $("#refNo").removeProp('readonly', false);
    setTimeout(function () {
        $('.modal').data('bs.modal').handleUpdate();
    }, 80);
    $("#section-dimension").hide();
    $("#on_pondation").hide();

    $("button[name=SubmitForm]").removeAttr("disabled");
    $("button[name=Cancel]").removeAttr("disabled");
    $("button[name=SaveAsRevised]").removeAttr("disabled");
    $("button[name=SaveAsDraft]").removeAttr("disabled");
    $('.btn-refNo button').removeAttr("disabled");
}
function hideModal() {
    $("#myModalRequest").modal("hide");
}

var filterStatus = {
    "draft": "DRAFT",
    "submit": "SUBMITED",
    "revise": "NEED REVISION",
    "revised": "REVISED",
    "approve": "IN PROGRESS",
    "booked": "BOOKED",
    "complete": "COMPLETED",
    "rerouted": "REROUTED",
    "reject": "REJECTED"
};
var columnList = [
    [
        {
            field: 'ID',
            title: 'ACTION',
            halign: 'center',
            valign: 'middle',
            align: 'left',
            rowspan: 2,
            formatter: ActionFormatter,
            events: EventsFormatter,
            class: 'text-nowrap'
            //filterControl: "input",
            //sortable: true
        },
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
            field: 'Status',
            title: 'STATUS',
            halign: 'center',
            align: 'left',
            formatter: statusFormatter,
            rowspan: 2,
            class: 'text-nowrap',
            filterControl: "select",
            filterData: 'var:filterStatus',
            sortable: true
        },
        {
            field: '',
            title: 'REQUESTER',
            halign: 'center',
            colspan: 2,
            align: 'left',
            class: 'text-nowrap'
        },
        {
            field: 'Unit',
            title: 'TYPE',
            class: 'text-nowrap',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            filterControl: "select",
            sortable: true
        },
        {
            field: 'DetailUnit',
            title: 'UNIT',
            halign: 'center',
            valign: 'middle',
            align: 'center',
            rowspan: 2,
            formatter: ActionFormatterUnit,
            events: EventsFormatter,
            class: 'text-nowrap'
            //filterControl: "input",
            //sortable: true
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
            filterDatepickerOptions: {format: 'yyyy-mm-dd'},
            formatter: formatDateBT,
        },
    ],
    [
        {
            field: 'ReqName',
            title: 'NAME',
            halign: 'center',
            class: 'text-nowrap',
            align: 'left',
            filterControl: "input",
            formatter: formatUpperCase,
            sortable: true
        },
        {
            field: 'ReqHp',
            title: 'PHONE NO',
            class: 'text-nowrap',
            halign: 'center',
            align: 'left',
            filterControl: "input",
            sortable: true
        },
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
];

$(function () {

    var url_string = window.location.href //"http://localhost:1400/DTS/DeliveryRequisitionList.html?c=1&b=DI-000179-REV-00" 
    var url = new URL(url_string);
    var FromEmail = url.searchParams.get("c");
    var b = url.searchParams.get("b");
    if (FromEmail === '1') {

        resetForm();
        $("#RejectNoteSpace").hide();
        $form.find("input[name=formType]").val("I");
        referenceEvent.formType = "I";
        $("button[name=Cancel]").hide();
        $("button[name=SaveAsDraft]").hide();
        $("button[name=SubmitForm]").hide();
        $("button[name=SaveAsRevised]").hide();
        $("button[name=ReRoute]").hide();
        $("#myModalRequest").modal("show");
        // Click Not From Home Page Requestor 
        if (b != 'Home') {
            $("#refSONo").parent().removeClass('active');
            $("#refDINo").parent().addClass('active');
            $("#refNo").val(b);
            referenceClick();
        }
        
    }

    $table.bootstrapTable({
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
        rowStyle: function rowStyle(row, index) {
            //console.log(row);
            var css = '';
            if (row.Status == 'submit' || row.Status == 'approve') {
                var a = moment(row.UpdateDate);
                var b = moment();
                if (b.diff(a, 'days') > 1) {
                    css = {
                        css: {
                            background: 'rgba(245, 105, 84, 0.4)',
                            border: '#f4543c',
                        }
                    };
                }
            }
            return css;
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
        columns: columnList
    });   
  
    var homenotif = localStorage.getItem("homenotif")    
    var status = localStorage.getItem("status") 
    var today = localStorage.getItem("today")
    if (homenotif == 'notif') {     
            window.pis.table({
                objTable: $table,
                urlSearch: '/DTS/DeliveryRequisitionPage',
                urlPaging: '/DTS/DeliveryRequisitionPageXt',
                searchParams: {
                    requestor: true,
                    status: status,
                    today: today
                },
                autoLoad: true
            });
    }   
    else {
        window.pis.table({

            objTable: $table,
            urlSearch: '/DTS/DeliveryRequisitionPage',
            urlPaging: '/DTS/DeliveryRequisitionPageXt',
            searchParams: {
                searchName: $("input[name=searchText]").val(),
                requestor: true,
                today: false
            },
            autoLoad: true
        });
    }
       
    
        
    

    $("#btnAddRequest").click(function (e) {
        resetForm();
        $("#RejectNoteSpace").hide();
        $form.find("input[name=formType]").val("I");
        referenceEvent.formType = "I";
        $("button[name=Cancel]").hide();
        $("button[name=SaveAsDraft]").hide();
        $("button[name=SubmitForm]").hide();
        $("button[name=SaveAsRevised]").hide();
        $("button[name=ReRoute]").hide();
        showModal();
    });

    $("#mySearch").insertBefore($("[name=refresh]"));

    $("#btnExportDR").click(function () {
        var options = $table.bootstrapTable('getOptions');
        //enableLink(false);
        $.ajax({
            url: "/DTS/DownloadDeliveryRequisition",
            type: 'POST',
            dataType: "json",
            data: {
                requestor: true,
                filterColumns: options.valuesFilterControl                
            },
            success: function (guid) {
                //enableLink(true);
                window.open('/DTS/DownloadToExcelDeliveryRequisition?guid=' + guid, '_blank');
            },
            cache: false
        });
    });
});
