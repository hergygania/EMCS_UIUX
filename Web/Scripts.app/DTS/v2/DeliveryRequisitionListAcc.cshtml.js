$table = $('#tableDeliveryRequisition'); 
$form = $("#formRequest");
var $ActType = '';
var rowSelected;
function statusFormatter(str, index, row) {
    color = '';
    text = '';
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
            icon = 'fa fa-reply';
            break;
        case 'revised':
            color = 'primary';
            text = 'REVISED';
            icon = 'fa fa-edit';
            break;
        case 'submit':
            color = 'primary';
            text = 'NEW';
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

function ActionFormatter(data, row, index) {
    var htm = [];
    htm.push('<button class="view btn btn-info btn-xs" data-toggle="tooltip" data-placement="bottom" title="View"><i class="fa fa-eye"></i></button> ');
<<<<<<< HEAD
<<<<<<< HEAD
    if (['reject', 'revise','request rerouted'].indexOf(row.Status) <= -1) {
        if (allowUpdate === "True") htm.push('<button class="approve btn btn-primary btn-xs" data-toggle="tooltip" data-placement="bottom" title="Edit"><i class="fa fa-edit"></i></button> ');
    }
    //if (row.Status == "rerouted" && row.RefNoType == "SO" ) {
    //    if (allowUpdate === "True") htm.push('<button class="reroute btn btn-warning btn-xs" data-toggle="tooltip" data-placement="bottom" title="Reroute"><i class="fa fa-route"></i></button> ');
    //}

=======
    if (['reject', 'revise'].indexOf(row.Status) <= -1) {
        if (allowUpdate === "True") htm.push('<button class="approve btn btn-primary btn-xs" data-toggle="tooltip" data-placement="bottom" title="Edit"><i class="fa fa-edit"></i></button> ');
    }
    if (row.Status == "rerouted" && row.RefNoType == "SO" ) {
        if (allowUpdate === "True") htm.push('<button class="reroute btn btn-warning btn-xs" data-toggle="tooltip" data-placement="bottom" title="Reroute"><i class="fa fa-route"></i></button> ');
    }
>>>>>>> 639d8d0 (Intial commit)
=======
    if (['reject', 'revise','request rerouted'].indexOf(row.Status) <= -1) {
        if (allowUpdate === "True") htm.push('<button class="approve btn btn-primary btn-xs" data-toggle="tooltip" data-placement="bottom" title="Edit"><i class="fa fa-edit"></i></button> ');
    }
    //if (row.Status == "rerouted" && row.RefNoType == "SO" ) {
    //    if (allowUpdate === "True") htm.push('<button class="reroute btn btn-warning btn-xs" data-toggle="tooltip" data-placement="bottom" title="Reroute"><i class="fa fa-route"></i></button> ');
    //}

>>>>>>> 93c2efe ([U] Update from client's TFS)
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
function showModal(id = 'myModalRequest') {
    $('#' + id).modal("show");
}
function hideModal(id = 'myModalRequest') {
    $('#' + id).modal("hide");
}
window.EventsFormatter = {
    'click .approve': function (e, value, row, index) {
        $form.find("input[name=formType]").val("U");
        $("#myModalRequest .modal-title").html('APPROVAL - #' + row.KeyCustom);
        $("button[name=ReRoute]").hide();
        if (row.Status === 'complete') {
            $("button[name=Reject]").hide();
            $("button[name=Complete]").show();
            $("button[name=ForceComplete]").hide();
            $("button[name=Approve]").hide();
            $("button[name=Revise]").hide();
            $("button[name=Cancel]").show();
        } else if (row.Status === "submit") {
            $("button[name=Complete]").hide();
            $("button[name=Reject]").show();
            $("button[name=Approve]").show();
            $("button[name=Cancel]").show();
            $("button[name=Revise]").show();
            $("button[name=ForceComplete]").hide();
        } else if (row.Status === "revised") {
            $("button[name=Complete]").hide();
            $("button[name=Reject]").show();
            $("button[name=Approve]").show();
            $("button[name=Cancel]").show();
            $("button[name=Revise]").show();
            $("button[name=ForceComplete]").hide();
        } else if (row.Status === "approve" || row.Status === "booked") {
            $("button[name=Reject]").show();
            $("button[name=Cancel]").show();
            $("button[name=Complete]").show();
            $("button[name=ForceComplete]").show();
            $("button[name=Approve]").hide();
            $("button[name=Revise]").hide();
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)
        } else if (row.Status === "rerouted") {
            $("button[name=Reject]").show();
            $("button[name=Cancel]").show();
            $("button[name=Complete]").show();
            $("button[name=ForceComplete]").hide();
            $("button[name=Approve]").hide();
            $("button[name=Revise]").show();
        }

<<<<<<< HEAD
=======
        }
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)
        $.ajax({
            type: "GET",
            url: myApp.root + 'DTS/GetDRDetails?number=' + row.ID,
            beforeSend: function () { ShowLoading() },
            complete: function () { HideLoading()},
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
                    referenceEvent.fillFromSo(d, "U");
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
    'click .view': function (e, value, row, index) {
        $("#myModalRequest .modal-title").html('VIEW - #' + row.KeyCustom);
        $form.find("input[name=formType]").val("V");
        //$("myModalRequest .modal-title").html('VIEW DR');
        $("button[name=Reject]").hide();
        $("button[name=Complete]").hide();
        $("button[name=ForceComplete]").hide();
        $("button[name=Approve]").hide();
        $("button[name=Revise]").hide();
        $("button[name=ReRoute]").hide();
        $("button[name=Cancel]").show();
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
        $("button[name=Reject]").hide();
        $("button[name=Complete]").hide();
        $("button[name=ForceComplete]").hide();
        $("button[name=Approve]").hide();
        $("button[name=Revise]").hide();
        $("button[name=ReRoute]").show();
        $("button[name=Cancel]").show();

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

function sendReRoute() {
    var dataForm = $form.serializeArray().reduce(function (obj, item) {
        obj[item.name] = item.value;
        return obj;
    }, {});
    var formData = new FormData();
    formData.append("ID", $('#ID').val());
    formData.append("KeyCustom", $('#KeyCustom').val());
    formData.append("formType", "U");
    formData.append("CustID", $('#CustID').val());
    formData.append("CustName", $('#CustName').val());
    formData.append("CustAddress", $('#CustAddress').val());
    formData.append("PicName", $('#PicName').val());
    formData.append("PicHP", $('#PicHP').val());
    formData.append("Kecamatan", $('#Kecamatan').val());
    formData.append("Kabupaten", $('#Kabupaten').val());
    formData.append("Province", $('#Province').val());
    formData.append("RefNo", $('#refNo').val());

    if ($("#refSONo").parent().hasClass('active')) {
        formData.append("RefNoType", "SO");
        formData.append("SoNo", $('#SoNo').val());
        formData.append("SoDate", $('#SoDate').val());
    }
    else if ($("#refSTRNo").parent().hasClass('active')) {
        formData.append("RefNoType", "STR");
        formData.append("STRNo", $('#STRNo').val());
        formData.append("STRDate", $('#STRDate').val());
    }
    else if ($("#refPONo").parent().hasClass('active')) {
        formData.append("RefNoType", "PO");
        //formData.append("STRNo", $('#STRNo').val());
        //formData.append("STRDate", $('#STRDate').val());
    }
    else if ($("#refDINo").parent().hasClass('active')) {
        formData.append("RefNoType", "DI");
        formData.append("DINo", $('#DINo').val());
        formData.append("DIDate", $('#DIDate').val());
    }
    formData.append("Status", 'complete');
    formData.append("SDOC", $('#SDOC')[0].files[0]);
    formData.append("SDOC1", $('#SDOC1')[0].files[0]);
    formData.append("SDOC2", $('#SDOC2')[0].files[0]);

    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/DeliveryRequisitionReRouteForm',
        beforeSend: function () {
            ShowLoading();
            $("button[name=ReRoute]").attr("disabled", "disabled");
            $("button[name=Cancel]").attr("disabled", "disabled");
        },
        complete: function () {
            HideLoading();
        },
        data: formData,
        dataType: "json",
        contentType: false,
        processData: false,
        success: function (d) {
            HideLoading();
            if (d.Status == 0) {
                var message = '';
                var DRNo = dataForm['KeyCustom'];
                if (d.result != null) {
                    DRNo = d.result.KeyCustom;
                }
                sAlert('Success', 'Reroute NO ' + DRNo + ' Success', 'success');
                resetForm();
                hideModal();
            } else {
                $("button[name=ReRoute]").removeAttr("disabled");
                $("button[name=Cancel]").removeAttr("disabled");
                sAlert('Error', d.Msg, 'error');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HideLoading();
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
            $("button[name=ReRoute]").removeAttr("disabled");
            $("button[name=Cancel]").removeAttr("disabled");
        }
    });
}
function htmlEncode(str) {
    return String(str).replace(/[^a-zA-Z 0-9'.<>]+/g, '').replace(/^\s+|\s+$/g, '')
}
function sendResponse(dataForm) {
    for (var i = 0; i < dataForm.length; i++) {
        htmlEncode(dataForm[i]);
    }

    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/DeliveryValidateProccess',
        beforeSend: function () {
            ShowLoading();
            $("button[name=Reject]").attr("disabled", "disabled");
            $("button[name=Approve]").attr("disabled", "disabled");
            $("button[name=Complete]").attr("disabled", "disabled");
        },
        complete: function () {
            HideLoading();
        },
        data: dataForm,
        dataType: "json",
        success: function (d) {
            if (d.Status == 0) {
                if (d.result && d.result.Status == 'booked') {
                    sAlert('Warning', rowSelected.header.RefNoType + " status is still booking", 'warning');
                } else {
                    var message = (d.Msg !== undefined) ? d.Msg : '';
                    sAlert('Success', message, 'success');
                }
            } else {
                sAlert('Error', d.Msg, 'error');
            }
            $("[name=refresh]").trigger('click');
            hideModal();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
}

function submitForm(ActType) {
    var dataForm = $form.serializeArray();
    
    if (ActType === "Reject" || ActType === "Revise") {
        dataForm.push({ name: 'type', value: ActType });
        swal({
            title: ActType === "Revise" ? "Please Input Revise Note" : "Please Input Reject Note",
            type: "input",
            showCancelButton: true,
            closeOnConfirm: false,
            animation: "slide-from-top",
            inputPlaceholder: "Type a description"
        }, function (inputValue) {
            if (inputValue === false) return false;

            if (inputValue === "") {
                swal.showInputError("You must type something!");
                return false;
            }
            dataForm.push({ name: 'RejectNote', value: inputValue });
            sendResponse(dataForm);
        });
    } else if (ActType === "Complete") {
        if (rowSelected.header.RefNoStatus == "PENDING") {
            if (rowSelected.header.Status == 'approve') {
                dataForm.push({ name: 'type', value: 'Booked' });
                sendResponse(dataForm);
                return;
            } else {
                sAlert('Warning', rowSelected.header.RefNoType + " status is still booking", 'warning');
                return;
            }
        } else {
            dataForm.push({ name: 'type', value: ActType });
<<<<<<< HEAD
<<<<<<< HEAD
            requestingFormModalComplete.initShow(rowSelected.header, rowSelected.details, rowSelected.header.Status);
=======
            requestingFormModalComplete.initShow(rowSelected.header, rowSelected.details);
>>>>>>> 639d8d0 (Intial commit)
=======
            requestingFormModalComplete.initShow(rowSelected.header, rowSelected.details, rowSelected.header.Status);
>>>>>>> 93c2efe ([U] Update from client's TFS)
           //Set Trigger SendEmail
               //SEND TO TU WAREHOUSE
            setInitValCheckSendEmail('SendEmailToCakung');
            setInitValCheckSendEmail('SendEmailToBalikPapan');
            setInitValCheckSendEmail('SendEmailToMakasar');
            setInitValCheckSendEmail('SendEmailToSurabaya');
            setInitValCheckSendEmail('SendEmailToBanjarMasin');
            //setInitValCheckSendEmail('SendEmailToPalembang');
            //setInitValCheckSendEmail('SendEmailToPekanBaru');
            setInitValCheckSendEmail('SendEmailToCileungsi');
            //Send TO TU Service
            setInitValCheckSendEmail('SendEmailToServiceTUPalembang');
            setInitValCheckSendEmail('SendEmailToServiceTUPekanBaru');
            setInitValCheckSendEmail('SendEmailToServiceTUJambi');
            setInitValCheckSendEmail('SendEmailToServiceTUBengkulu');
            setInitValCheckSendEmail('SendEmailToServiceTUTanjungEnim');
            setInitValCheckSendEmail('SendEmailToServiceTUMedan');
            setInitValCheckSendEmail('SendEmailToServiceTUPadang');
            setInitValCheckSendEmail('SendEmailToServiceTUBangkaBelitung');
            setInitValCheckSendEmail('SendEmailToServiceTUBandarLampung');
            setInitValCheckSendEmail('SendEmailToServiceTUBSD');
            setInitValCheckSendEmail('SendEmailToServiceTUSurabaya');
            setInitValCheckSendEmail('SendEmailToServiceTUManado');
            setInitValCheckSendEmail('SendEmailToServiceTUJayapura');
            setInitValCheckSendEmail('SendEmailToServiceTUSorong');
            setInitValCheckSendEmail('SendEmailToServiceTUSamarinda');
            setInitValCheckSendEmail('SendEmailToServiceTUBalikpapan');
            setInitValCheckSendEmail('SendEmailToServiceTUMakassar');
            setInitValCheckSendEmail('SendEmailToServiceTUSemarang');
            setInitValCheckSendEmail('SendEmailToServiceTUPontianak');
            setInitValCheckSendEmail('SendEmailToServiceTUBatuLicin');
            setInitValCheckSendEmail('SendEmailToServiceTUSangatta');
            setInitValCheckSendEmail('SendEmailToServiceTUKendari');
<<<<<<< HEAD
<<<<<<< HEAD
            setInitValCheckSendEmail('SendEmailToServiceTUMeulaboh');            
=======

            //Send TO CKB
            //setInitValCheckSendEmail('SendEmailToCkbAllArea');
            //setInitValCheckSendEmail('SendEmailToCkbCakung');
            //setInitValCheckSendEmail('SendEmailToCkbSurabaya');
            //setInitValCheckSendEmail('SendEmailToCkbMakassar');
            //setInitValCheckSendEmail('SendEmailToCkbCakungStandartKit');
            //setInitValCheckSendEmail('SendEmailToCkbBalikpapan');
            //setInitValCheckSendEmail('SendEmailToCkbBanjarmasin');

            //setInitValCheckSendEmail('SendEmailToCkb');
>>>>>>> 639d8d0 (Intial commit)
=======
            setInitValCheckSendEmail('SendEmailToServiceTUMeulaboh');            
>>>>>>> 93c2efe ([U] Update from client's TFS)
            $("#myModalRequestComplete").modal("show");
        }
    } else if (ActType === "ForceComplete") {
        dataForm.push({ name: 'type', value: ActType });
        requestingFormModalComplete.initShow(rowSelected.header, rowSelected.details);
        //SEND TO WAREHOUSER
        setInitValCheckSendEmail('SendEmailToCakung');
        setInitValCheckSendEmail('SendEmailToBalikPapan');
        setInitValCheckSendEmail('SendEmailToMakasar');
        setInitValCheckSendEmail('SendEmailToSurabaya');
        setInitValCheckSendEmail('SendEmailToBanjarMasin');
        //setInitValCheckSendEmail('SendEmailToPalembang');
        //setInitValCheckSendEmail('SendEmailToPekanBaru');
        setInitValCheckSendEmail('SendEmailToCileungsi');
        // SEND TO TU SERVICE
        setInitValCheckSendEmail('SendEmailToServiceTUPalembang');
        setInitValCheckSendEmail('SendEmailToServiceTUPekanBaru');
        setInitValCheckSendEmail('SendEmailToServiceTUJambi');
        setInitValCheckSendEmail('SendEmailToServiceTUBengkulu');
        setInitValCheckSendEmail('SendEmailToServiceTUTanjungEnim');
        setInitValCheckSendEmail('SendEmailToServiceTUMedan');
        setInitValCheckSendEmail('SendEmailToServiceTUPadang');
        setInitValCheckSendEmail('SendEmailToServiceTUBangkaBelitung');
        setInitValCheckSendEmail('SendEmailToServiceTUBandarLampung');
        setInitValCheckSendEmail('SendEmailToServiceTUBSD');
        setInitValCheckSendEmail('SendEmailToServiceTUSurabaya');
        setInitValCheckSendEmail('SendEmailToServiceTUManado');
        setInitValCheckSendEmail('SendEmailToServiceTUJayapura');
        setInitValCheckSendEmail('SendEmailToServiceTUSorong');
        setInitValCheckSendEmail('SendEmailToServiceTUSamarinda');
        setInitValCheckSendEmail('SendEmailToServiceTUBalikpapan');
        setInitValCheckSendEmail('SendEmailToServiceTUMakassar');
        setInitValCheckSendEmail('SendEmailToServiceTUSemarang');
        setInitValCheckSendEmail('SendEmailToServiceTUPontianak');
        setInitValCheckSendEmail('SendEmailToServiceTUBatuLicin');
        setInitValCheckSendEmail('SendEmailToServiceTUSangatta');
        setInitValCheckSendEmail('SendEmailToServiceTUKendari');
<<<<<<< HEAD
<<<<<<< HEAD
        setInitValCheckSendEmail('SendEmailToServiceTUMeulaboh');
    
        $("#myModalRequestComplete").modal("show");
    } else if (ActType === "Approve") {
        dataForm.push({ name: 'type', value: ActType });
       
        $('#myModalCkb').modal("show");        
=======

         //Send TO CKB
        //setInitValCheckSendEmail('SendEmailToCkbAllArea');
        //setInitValCheckSendEmail('SendEmailToCkbCakung');
        //setInitValCheckSendEmail('SendEmailToCkbSurabaya');
        //setInitValCheckSendEmail('SendEmailToCkbMakassar');
        //setInitValCheckSendEmail('SendEmailToCkbCakungStandartKit');
        //setInitValCheckSendEmail('SendEmailToCkbBalikpapan');
        $("#myModalRequestComplete").modal("show");
    } else if (ActType === "Approve") {
        dataForm.push({ name: 'type', value: ActType });
        //sendResponse(dataForm);
        //showModal('myModalCkb');
        $('#myModalCkb').modal("show");
        //$('#myModalCkb input[name="SendEmailToCkb"]').prop("checked", $('#formRequest input[name="SendEmailToCkb"]').is(":checked"));
>>>>>>> 639d8d0 (Intial commit)
=======
        setInitValCheckSendEmail('SendEmailToServiceTUMeulaboh');
    
        $("#myModalRequestComplete").modal("show");
    } else if (ActType === "Approve") {
        dataForm.push({ name: 'type', value: ActType });
       
        $('#myModalCkb').modal("show");        
>>>>>>> 93c2efe ([U] Update from client's TFS)
        $('#myModalCkb input[name="SendEmailToCkbSurabaya"]').prop("checked", $('#formRequest input[name="SendEmailToCkbSurabaya"]').is(":checked"));
        $('#myModalCkb input[name="SendEmailToCkbMakassar"]').prop("checked", $('#formRequest input[name="SendEmailToCkbMakassar"]').is(":checked"));
        $('#myModalCkb input[name="SendEmailToCkbCakungStandartKit"]').prop("checked", $('#formRequest input[name="SendEmailToCkbCakungStandartKit"]').is(":checked"));
        $('#myModalCkb input[name="SendEmailToCkbBalikpapan"]').prop("checked", $('#formRequest input[name="SendEmailToCkbBalikpapan"]').is(":checked"));
        $('#myModalCkb input[name="SendEmailToCkbBanjarmasin"]').prop("checked", $('#formRequest input[name="SendEmailToCkbBanjarmasin"]').is(":checked"));
        $('#CkbNotes').val('');
    } else if (ActType === "ReRoute") {
        sendReRoute();
    }
}

var requestingFormModalComplete = {
    $formEl: null,
    data: null,
<<<<<<< HEAD
<<<<<<< HEAD
    initShow: function (header, units,status) {
=======
    initShow: function (header, units) {
>>>>>>> 639d8d0 (Intial commit)
=======
    initShow: function (header, units,status) {
>>>>>>> 93c2efe ([U] Update from client's TFS)
        $('.freight').addClass('hidden');
        var SELF = requestingFormModalComplete;
        SELF.data = {
            header: header,
            details: units.filter(function (item) {
                return item.Checked == 1;
            })
        };
        var modaTransport = $('#formRequest input[name="ModaTransport"]:checked').val();
        if (modaTransport == "SEA") {
            SELF.$formEl = $('#form-sea-freight');
            $('#myModalRequestComplete .modal-title').html('COMPLETE E-DELIVERY (SEA FREIGHT)');
            $('#sea-freight').removeClass('hidden');
        } else if (modaTransport == "LAND") {
            SELF.$formEl = $('#form-land-freight');
            $('#myModalRequestComplete .modal-title').html('COMPLETE E-DELIVERY (INLAND FREIGHT)');
            $('#land-freight').removeClass('hidden');
<<<<<<< HEAD
<<<<<<< HEAD
            SELF.initTableUnitInfo(SELF.data.details, status);
=======
            SELF.initTableUnitInfo(SELF.data.details);
>>>>>>> 639d8d0 (Intial commit)
=======
            SELF.initTableUnitInfo(SELF.data.details, status);
>>>>>>> 93c2efe ([U] Update from client's TFS)
        } else if (modaTransport == "AIR") {
            SELF.$formEl = $('#form-air-freight');
            $('#myModalRequestComplete .modal-title').html('COMPLETE E-DELIVERY (AIR FREIGHT)');
            $('#air-freight').removeClass('hidden');
        }

    },
<<<<<<< HEAD
<<<<<<< HEAD
    initTableUnitInfo: function (units, status) {
        var $tableCUnit = $('#tableDRAccComplteUnit');

        if (status = 'rerouted') {
            for (var x in units) {
                units[x].VeselNoPolice = units[0].VeselNoPolice;
                units[x].DriverName = units[0].DriverName;
                units[x].DriverHp = units[0].DriverHp;
                units[x].PickUpPlan = dateFormatterV2(units[0].PickUpPlan);
                units[x].EstTimeArrival = dateFormatterV2(units[0].EstTimeArrival);
                units[x].EstTimeDeparture = dateFormatterV2(units[0].EstTimeDeparture);
            }
        }
        else {
            for (var x in units) {
                units[x].VeselNoPolice = '-';
                units[x].DriverName = '-';
                units[x].DriverHp = '-';
            }
        }

=======
    initTableUnitInfo: function (units) {
=======
    initTableUnitInfo: function (units, status) {
>>>>>>> 93c2efe ([U] Update from client's TFS)
        var $tableCUnit = $('#tableDRAccComplteUnit');

        if (status = 'rerouted') {
            for (var x in units) {
                units[x].VeselNoPolice = units[0].VeselNoPolice;
                units[x].DriverName = units[0].DriverName;
                units[x].DriverHp = units[0].DriverHp;
                units[x].PickUpPlan = dateFormatterV2(units[0].PickUpPlan);
                units[x].EstTimeArrival = dateFormatterV2(units[0].EstTimeArrival);
                units[x].EstTimeDeparture = dateFormatterV2(units[0].EstTimeDeparture);
            }
        }
<<<<<<< HEAD
>>>>>>> 639d8d0 (Intial commit)
=======
        else {
            for (var x in units) {
                units[x].VeselNoPolice = '-';
                units[x].DriverName = '-';
                units[x].DriverHp = '-';
            }
        }

>>>>>>> 93c2efe ([U] Update from client's TFS)
        $tableCUnit.bootstrapTable('destroy');
        $tableCUnit.bootstrapTable({
            cache: false,
            pagination: true,
            search: false,
            striped: true,
            clickToSelect: true,
            showColumns: false,
            showRefresh: false,
            smartDisplay: false,
            pageSize: '10',
            formatNoMatches: function () {
                return '<span class="noMatches">Set</span>';
            },
            data: units,
            editable: true,
        });
    },
}
function submitFormComplete() {
    var _FM = requestingFormModalComplete;
    var formData = new FormData();
    var detailUnits = [];
    var modaTransport = $('#formRequest input[name="ModaTransport"]:checked').val();
    if (modaTransport == "SEA") {
        detailUnits = _FM.data.details;
        for (var x in detailUnits) {
            detailUnits[x].VeselName = _FM.$formEl.find('input[name=VeselName]').val();
            detailUnits[x].PICName = _FM.$formEl.find('input[name=PICName]').val();
            detailUnits[x].PICHp = _FM.$formEl.find('input[name=PICHp]').val();
            detailUnits[x].PickUpPlan = _FM.$formEl.find('input[name=PickUpPlan]').val();
            detailUnits[x].EstTimeDeparture = _FM.$formEl.find('input[name=EstTimeDeparture]').val();
            detailUnits[x].EstTimeArrival = _FM.$formEl.find('input[name=EstTimeArrival]').val();
        }
        formData.append("Attachment1", $('#Attachment1-sea')[0].files[0]);
        formData.append("Attachment2", $('#Attachment2-sea')[0].files[0]);
    } else if (modaTransport == "LAND") {
        formData.append("Attachment1", $('#Attachment1-land')[0].files[0]);
        formData.append("Attachment2", $('#Attachment2-land')[0].files[0]);
        detailUnits = _FM.data.details;
        if (_FM.$formEl.find('input[Name="ApplyToAll"]').is(":checked")) {
            if (_FM.$formEl.find('input[name=VeselNoPolice]').val()=='') {
                sAlert('warning', "Please Fill textbox No.Polisi", 'warning')
                return;
            }
            if (_FM.$formEl.find('input[name=DriverName]').val() == '') {
                sAlert('warning', "Please Fill textbox Driver Name", 'warning')
                return;
            }
            if (_FM.$formEl.find('input[name=DriverHp]').val() == '') {
                sAlert('warning', "Please Fill textbox Driver HP", 'warning')
                return;
            }
            if (_FM.$formEl.find('input[name=PickUpPlan]').val() == '') {
                sAlert('warning', "Please Fill textbox PickUp Plan", 'warning')
                return;
            }
            if (_FM.$formEl.find('input[name=EstTimeDeparture]').val() == '') {
                sAlert('warning', "Please Fill textbox EstTimeDeparture ", 'warning')
                return;
            }
            if (_FM.$formEl.find('input[name=EstTimeArrival]').val() == '') {
                sAlert('warning', "Please Fill textbox EstTimeArrival ", 'warning')
                return;
            }
            for (var x in detailUnits) {
                detailUnits[x].VeselNoPolice = _FM.$formEl.find('input[name=VeselNoPolice]').val();
                detailUnits[x].DriverName = _FM.$formEl.find('input[name=DriverName]').val();
                detailUnits[x].DriverHp = _FM.$formEl.find('input[name=DriverHp]').val();
                detailUnits[x].PickUpPlan = _FM.$formEl.find('input[name=PickUpPlan]').val();
                detailUnits[x].EstTimeDeparture = _FM.$formEl.find('input[name=EstTimeDeparture]').val();
                detailUnits[x].EstTimeArrival = _FM.$formEl.find('input[name=EstTimeArrival]').val();
            }
        }
        else {
            detailUnits = $('#tableDRAccComplteUnit').bootstrapTable('getData');
        }
        
        
    } else if (modaTransport == "AIR") {
        detailUnits = _FM.data.details;
        for (var x in detailUnits) {
            detailUnits[x].DANo = _FM.$formEl.find('input[name=DANo]').val();
            detailUnits[x].EstTimeDeparture = _FM.$formEl.find('input[name=EstTimeDeparture]').val();
            detailUnits[x].EstTimeArrival = _FM.$formEl.find('input[name=EstTimeArrival]').val();
        }
        formData.append("Attachment1", $('#Attachment1-air')[0].files[0]);
        formData.append("Attachment2", $('#Attachment2-air')[0].files[0]);
    }
    //_FM.data.header.SendEmailToCKB = $('#formRequest input[name="SendEmailToCKB"]').is(":checked");
    _FM.data.header.ModaTransport = $('#formRequest input[name="ModaTransport"]:checked').val();
    _FM.data.header['SendEmailNotes'] = $('textarea[name="SendEmailNotes"]').val();
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)
    if (_FM.data.header.Status = 'rerouted') {
        formData.append("SDOC", $('#SDOC')[0].files[0]);
        formData.append("SDOC1", $('#SDOC1')[0].files[0]);
        formData.append("SDOC2", $('#SDOC2')[0].files[0]);
      
    }
<<<<<<< HEAD
=======
    
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)
    _FM.data.header['ExpectedTimeArrival'] = $('#formRequest input[name="ExpectedTimeArrival"]').val()
    _FM.data.header['ExpectedTimeLoading'] = $('#formRequest input[name="ExpectedTimeLoading"]').val()
    // TU WAREHOUSE
    _FM.data.header.SendEmailToCakung = $('#formRequest input[name="SendEmailToCakung"]').is(":checked");
    _FM.data.header.SendEmailToBalikPapan = $('#formRequest input[name="SendEmailToBalikPapan"]').is(":checked");
    _FM.data.header.SendEmailToMakasar = $('#formRequest input[name="SendEmailToMakasar"]').is(":checked");
    _FM.data.header.SendEmailToSurabaya = $('#formRequest input[name="SendEmailToSurabaya"]').is(":checked");
    _FM.data.header.SendEmailToBanjarMasin = $('#formRequest input[name="SendEmailToBanjarMasin"]').is(":checked");  
    _FM.data.header.SendEmailToCileungsi = $('#formRequest input[name="SendEmailToCileungsi"]').is(":checked");
    // TU SERVICE
    _FM.data.header.SendEmailToServiceTUPalembang = $('#formRequest input[name="SendEmailToServiceTUPalembang"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUPekanBaru = $('#formRequest input[name="SendEmailToServiceTUPekanBaru"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUPalembang = $('#formRequest input[name="SendEmailToServiceTUPalembang"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUJambi = $('#formRequest input[name="SendEmailToServiceTUJambi"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUBengkulu = $('#formRequest input[name="SendEmailToServiceTUBengkulu"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUTanjungEnim = $('#formRequest input[name="SendEmailToServiceTUTanjungEnim"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUMedan = $('#formRequest input[name="SendEmailToServiceTUMedan"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUPadang = $('#formRequest input[name="SendEmailToServiceTUPadang"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUBangkaBelitung = $('#formRequest input[name="SendEmailToServiceTUBangkaBelitung"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUBandarLampung = $('#formRequest input[name="SendEmailToServiceTUBandarLampung"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUBSD = $('#formRequest input[name="SendEmailToServiceTUBSD"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUSurabaya = $('#formRequest input[name="SendEmailToServiceTUSurabaya"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUManado = $('#formRequest input[name="SendEmailToServiceTUManado"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUJayapura = $('#formRequest input[name="SendEmailToServiceTUJayapura"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUSorong = $('#formRequest input[name="SendEmailToServiceTUSorong"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUSamarinda = $('#formRequest input[name="SendEmailToServiceTUSamarinda"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUBalikpapan = $('#formRequest input[name="SendEmailToServiceTUBalikpapan"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUMakassar = $('#formRequest input[name="SendEmailToServiceTUMakassar"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUSemarang = $('#formRequest input[name="SendEmailToServiceTUSemarang"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUPontianak = $('#formRequest input[name="SendEmailToServiceTUPontianak"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUBatuLicin = $('#formRequest input[name="SendEmailToServiceTUBatuLicin"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUSangatta = $('#formRequest input[name="SendEmailToServiceTUSangatta"]').is(":checked");
    _FM.data.header.SendEmailToServiceTUKendari = $('#formRequest input[name="SendEmailToServiceTUKendari"]').is(":checked");
<<<<<<< HEAD
<<<<<<< HEAD
    _FM.data.header.SendEmailToServiceTUMeulaboh = $('#formRequest input[name="SendEmailToServiceTUMeulaboh"]').is(":checked");
    
=======
>>>>>>> 639d8d0 (Intial commit)
=======
    _FM.data.header.SendEmailToServiceTUMeulaboh = $('#formRequest input[name="SendEmailToServiceTUMeulaboh"]').is(":checked");
    
>>>>>>> 93c2efe ([U] Update from client's TFS)
   // TU CKB
    _FM.data.header.SendEmailToCkbSurabaya = $('#formRequest input[name="SendEmailToCkbSurabaya"]').is(":checked");
    _FM.data.header.SendEmailToCkbMakassar = $('#formRequest input[name="SendEmailToCkbMakassar"]').is(":checked");
    _FM.data.header.SendEmailToCkbCakungStandartKit = $('#formRequest input[name="SendEmailToCkbCakungStandartKit"]').is(":checked");
    _FM.data.header.SendEmailToCkbBalikpapan = $('#formRequest input[name="SendEmailToCkbBalikpapan"]').is(":checked");
    _FM.data.header.SendEmailToCkbBanjarmasin = $('#formRequest input[name="SendEmailToCkbBanjarmasin"]').is(":checked");


    _FM.data.header.ForceComplete = ($ActType == 'ForceComplete') ? true : false;
    for (var name in _FM.data.header) {
        var val = _FM.data.header[name]; // == "null" ? null : dataForm[name];
        if (val != null && val != "null") {
            formData.append(name, val);
        }
    }
    if (modaTransport == "LAND") {
        for (var nameunit in detailUnits) {
            if (_FM.$formEl.find('input[Name="ApplyToAll"]').is(":checked")== false) {
                var val = detailUnits[nameunit]; // == "null" ? null : dataForm[name];
                if (val.VeselNoPolice == "-" || val.VeselNoPolice == null) {
                    sAlert('warning', "Please Fill No.Polisi", 'warning')
                    return;
                }
                if (val.DriverName == "-" || val.DriverName == null) {
                    sAlert('warning', "Please Fill Driver Name", 'warning')
                    return;
                }
                if (val.DriverHp == "-" || val.DriverHp == null) {
                    sAlert('warning', "Please Fill Driver HP", 'warning')
                    return;
                }
<<<<<<< HEAD
<<<<<<< HEAD
            } 
=======
            }
>>>>>>> 639d8d0 (Intial commit)
=======
            } 
>>>>>>> 93c2efe ([U] Update from client's TFS)
        }
    }
    formData.append('detailUnits', JSON.stringify(detailUnits));
    
    
    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/DeliveryCompleteProccess',
        beforeSend: function () {
            ShowLoading();
            $("button[name=SaveComplete]").attr("disabled", "disabled");
        },
        complete: function () {
            HideLoading();
            $("button[name=SaveComplete]").removeAttr("disabled");
        },
        data: formData,
        dataType: "json",
        contentType: false,
        processData: false,
        success: function (d) {
            HideLoading();
            if (d.Status == 0) {
                sAlert('Success', 'Modify data Success!', 'success');
                resetForm();
                hideModal('myModalRequestComplete');
                hideModal();
            } else {
                sAlert('Error', d.Msg, 'error');
                $("button[name=Save]").removeAttr("disabled");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
}

function setActionCheckSendEmail(mailName) {
    $('input[name="' + mailName + '"]').change(function (e) {
        var value = $(this).is(":checked");
        $('input[name="' + mailName + '"]').each(function () {
            $(this).prop("checked", value);
        });
    });
}
function setInitValCheckSendEmail(mailName) {
    $('#myModalRequestComplete input[name="' + mailName + '"]')
        .prop("checked", $('#formRequest input[name="' + mailName + '"]').is(":checked"));
}

var filterStatus = {
    "submit": "NEW",
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
            //filterData: 'KeyCustom',
            sortable: true
        },
        {
            field: 'CreateDate',
            title: 'CREATE DATE',
            halign: 'center',
            align: 'center',
            rowspan: 2,
            class: 'text-nowrap',
            filterControl: "input",
            formatter: dateFormatter,        
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
            title: 'KABUPATEN',
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
    $(".processForm").click(function () {
        var act = $(this).attr('name');
        $ActType = act;
        if (act == 'Reject' || act == 'Revise') {
            submitForm(act);
        }
        else {
            var isValid = $form.valid();
            if (isValid) {
                submitForm(act);
            }
        }
    });
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        sidePagination: 'server',
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        ageSize: '10',
        fixedNumber: '3',
        filterControl: true,
        searchOnEnterKey: false,
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        rowStyle: function rowStyle(row, index) {
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
        columns: columnList
    });
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)
    var today = localStorage.getItem("today")
    if (today == 'today') {
        window.pis.table({
            objTable: $table,
            urlSearch: '/DTS/DeliveryRequisitionIncomingPage',
            urlPaging: '/DTS/DeliveryRequisitionIncomingPageXt',
            searchParams: {
                typesearch: 'incoming',
                requestor: false,
                today: today
                //custName:'',
                //origin:'',
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
                typeData: 'validation',
                requestor: false,
                //custName:'',
                //origin:'',
            },
            autoLoad: true
        });
    }
 
<<<<<<< HEAD
=======
    window.pis.table({
        objTable: $table,
        urlSearch: '/DTS/DeliveryRequisitionPage',
        urlPaging: '/DTS/DeliveryRequisitionPageXt',
        searchParams: {
            typeData: 'validation',
            requestor: false,
            //custName:'',
            //origin:'',
        },
        autoLoad: true
    });
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)
    $("#mySearch").insertBefore($("[name=refresh]"));

    $("#btnExportDR").click(function () {
        var options = $table.bootstrapTable('getOptions');
        enableLink(false);
        $.ajax({
            url: "/DTS/DownloadDeliveryRequisition",
            type: 'POST',
            dataType: "json",
            data: {
                requestor: false,
                filterColumns: options.valuesFilterControl,
            },
            success: function (guid) {
                enableLink(true);
                window.open('/DTS/DownloadToExcelDeliveryRequisition?guid=' + guid, '_blank');
            },
            cache: false
        });
    });

    //$('input[name="SendEmailToCkb"]').change(function (e) {
    //    var value = $(this).is(":checked");
    //    $('input[name="SendEmailToCkb"]').each(function () {
    //        $(this).prop("checked", value);
    //    });
    //});
    $('input[name="SendEmailToCkbSurabaya"]').change(function (e) {
        var value = $(this).is(":checked");
        $('input[name="SendEmailToCkbSurabaya"]').each(function () {
            $(this).prop("checked", value);
        });
    });
    $('input[name="SendEmailToCkbMakassar"]').change(function (e) {
        var value = $(this).is(":checked");
        $('input[name="SendEmailToCkbMakassar"]').each(function () {
            $(this).prop("checked", value);
        });
    });
    $('input[name="SendEmailToCkbCakungStandartKit"]').change(function (e) {
        var value = $(this).is(":checked");
        $('input[name="SendEmailToCkbCakungStandartKit"]').each(function () {
            $(this).prop("checked", value);
        });
    });
    $('input[name="SendEmailToCkbBalikpapan"]').change(function (e) {
        var value = $(this).is(":checked");
        $('input[name="SendEmailToCkbBalikpapan"]').each(function () {
            $(this).prop("checked", value);
        });
    });
    $('input[name="SendEmailToCkbBanjarmasin"]').change(function (e) {
        var value = $(this).is(":checked");
        $('input[name="SendEmailToCkbBanjarmasin"]').each(function () {
            $(this).prop("checked", value);
        });
    });

    setActionCheckSendEmail('SendEmailToCakung');
    setActionCheckSendEmail('SendEmailToBalikPapan');
    setActionCheckSendEmail('SendEmailToMakasar');
    setActionCheckSendEmail('SendEmailToSurabaya');
    setActionCheckSendEmail('SendEmailToBanjarMasin');
    //setActionCheckSendEmail('SendEmailToPalembang');
    //setActionCheckSendEmail('SendEmailToPekanBaru');
    setActionCheckSendEmail('SendEmailToCileungsi');


    setActionCheckSendEmail('SendEmailToServiceTUPalembang');
    setActionCheckSendEmail('SendEmailToServiceTUPekanBaru');
    setActionCheckSendEmail('SendEmailToServiceTUJambi');
    setActionCheckSendEmail('SendEmailToServiceTUBengkulu');
    setActionCheckSendEmail('SendEmailToServiceTUTanjungEnim');
    setActionCheckSendEmail('SendEmailToServiceTUMedan');
    setActionCheckSendEmail('SendEmailToServiceTUPadang');
    setActionCheckSendEmail('SendEmailToServiceTUBangkaBelitung');
    setActionCheckSendEmail('SendEmailToServiceTUBandarLampung');
    setActionCheckSendEmail('SendEmailToServiceTUBSD');
    setActionCheckSendEmail('SendEmailToServiceTUSurabaya');
    setActionCheckSendEmail('SendEmailToServiceTUManado');
    setActionCheckSendEmail('SendEmailToServiceTUJayapura');
    setActionCheckSendEmail('SendEmailToServiceTUSorong');
    setActionCheckSendEmail('SendEmailToServiceTUSamarinda');
    setActionCheckSendEmail('SendEmailToServiceTUBalikpapan');  
    setActionCheckSendEmail('SendEmailToServiceTUMakassar');
    setActionCheckSendEmail('SendEmailToServiceTUSemarang');
    setActionCheckSendEmail('SendEmailToServiceTUPontianak');
    setActionCheckSendEmail('SendEmailToServiceTUBatuLicin');
    setActionCheckSendEmail('SendEmailToServiceTUSangatta');
    setActionCheckSendEmail('SendEmailToServiceTUKendari');
<<<<<<< HEAD
<<<<<<< HEAD
    setActionCheckSendEmail('SendEmailToServiceTUMeulaboh');
=======
>>>>>>> 639d8d0 (Intial commit)
=======
    setActionCheckSendEmail('SendEmailToServiceTUMeulaboh');
>>>>>>> 93c2efe ([U] Update from client's TFS)
    
    //setActionCheckSendEmail('SendEmailToCkb');
    //setActionCheckSendEmail('SendEmailToCkbAllArea');
    //setActionCheckSendEmail('SendEmailToCkbCakung');
    //setActionCheckSendEmail('SendEmailToCkbSurabaya');
    //setActionCheckSendEmail('SendEmailToCkbMakassar');
    //setActionCheckSendEmail('SendEmailToCkbCakungStandartKit');
    //setActionCheckSendEmail('SendEmailToCkbBalikpapan');
    //setActionCheckSendEmail('SendEmailToCkbBanjarmasin');

    $('input[name="SendEmailCheckAll"]').change(function (e) {
        var value = $(this).is(":checked");
        $('div.checkbox.sendmail').find('input[type="checkbox"]').prop("checked", value);
        $('div.checkbox.sendmail').find('input[type="checkbox"]').trigger('change');
    });
    
    $('#ApproveOk').click(function () {
        var dataForm = $form.serializeArray();
        dataForm.push({ name: 'type', value: "Approve" });
        dataForm.push({ name: 'RejectNote', value: $('#CkbNotes').val() });
        sendResponse(dataForm);
        hideModal('myModalCkb');
    });

    $("button[name=SaveComplete]").on('click', function () {
        var _FM = requestingFormModalComplete;
        var validator = _FM.$formEl.validate({
            ignore: ":hidden",
            highlight: function (element, errorClass, validClass) {
                _FM.$formEl.find("div[for=" + element.name + "]").show();
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass(errorClass).addClass(validClass);
                if (element.name && element.name != "") {
                    _FM.$formEl.find("div[for=" + element.name + "]").hide();
                }
            }
        });
        var isValid = _FM.$formEl.valid();
        if (isValid) {
            submitFormComplete();
        } else {
            $.each(validator.errorMap, function (index, value) {
                console.log('Id: ' + index + ' Message: ' + value);
            });
        }
    });
});