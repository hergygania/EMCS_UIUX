$form = $("#formRequest");
var validator = $form.validate({ 
    ignore: ":hidden",
    highlight: function (element, errorClass, validClass) {
        $form.find("div[for=" + element.name + "]").show();
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass(errorClass).addClass(validClass);
        if (element.name && element.name != "") {
            $form.find("div[for=" + element.name + "]").hide();
        }
    }
});
function prepareModal() {
    var state = $('input[name=formType]').val();
    resetReference();
    resetFormRequisition();
}
function hideModal() {
    $("#myModalRequest").modal("hide");
}
function setFormSisable(isdisabled, formType) {
    if (isdisabled != false) {
        $('#btn-group-refnotype label.btn').addClass('hidden');
        if ($("#refSONo").parent().hasClass('active')) {
            $("#refSONo").parent().removeClass('hidden');
        } else if ($("#refSTRNo").parent().hasClass('active')) {
            $("#refSTRNo").parent().removeClass('hidden');
        } else if ($("#refPONo").parent().hasClass('active')) {
            $("#refPONo").parent().removeClass('hidden');
        } else if ($("#refDINo").parent().hasClass('active')) {
            $("#refDINo").parent().removeClass('hidden');
        }
    } 
<<<<<<< HEAD
    $("#newcustName").show();
    $("#oldcustNamegroup").hide();
=======
>>>>>>> 639d8d0 (Intial commit)
    $('.btn-refNo button').attr("disabled", isdisabled);
    $("#refNo").attr("disabled", isdisabled);
    $("#refDate").attr("disabled", isdisabled);
    $("#RequestHP").attr("disabled", isdisabled);
    $("#Sales1ID").attr("disabled", isdisabled);
    $("#Sales1Name").attr("disabled", isdisabled);
    $("#Sales1Hp").attr("disabled", isdisabled);
    $("#Sales2ID").attr("disabled", isdisabled);
    $("#Sales2Name").attr("disabled", isdisabled);
    $("#Sales2Hp").attr("disabled", isdisabled);

    $("#CustID").attr("disabled", isdisabled);
    $("#CustName").attr("disabled", isdisabled);
    $("#CustAddress").attr("disabled", isdisabled);
    $("#PicName").attr("disabled", isdisabled);
    $("#PicHP").attr("disabled", isdisabled);
    $("#Kecamatan").attr("disabled", isdisabled);
    $("#Kabupaten").attr("disabled", isdisabled);
    $("#Province").attr("disabled", isdisabled);
    $("#Origin").attr("disabled", isdisabled);
    $("#RequestNotes").attr("disabled", isdisabled);
<<<<<<< HEAD
    $("#btnhistory").hide();
=======
>>>>>>> 639d8d0 (Intial commit)
    if(formType === 'V')
    {
        $("#ExpectedTimeArrival").attr("disabled", isdisabled);
        $("#ExpectedTimeLoading").attr("disabled", isdisabled);
    }
    
    $("#ActualTimeArrival").attr("disabled", isdisabled);
    $("#ActualTimeDeparture").attr("disabled", isdisabled);

    $("#SoNo").attr("disabled", isdisabled);
    $("#SoDate").attr("disabled", isdisabled);
    $("#STRNo").attr("disabled", isdisabled);
    $("#STRDate").attr("disabled", isdisabled);
    $("#STONo").attr("disabled", isdisabled);
    $("#STODate").attr("disabled", isdisabled);
    $("#DoNo").attr("disabled", isdisabled);
    $("#OdDate").attr("disabled", isdisabled);
    $("#DINo").attr("disabled", isdisabled);
    $("#DIDate").attr("disabled", isdisabled);
    $("#DIDateSAP").attr("disabled", isdisabled);
    $("#u-weight").attr("disabled", isdisabled);
    $("#u-length").attr("disabled", isdisabled);
    $("#u-width").attr("disabled", isdisabled);
    $("#u-height").attr("disabled", isdisabled);

    $form.find("input[name=unit]").attr("disabled", isdisabled);
    $form.find("input[name=TermOfDelivery]").attr("disabled", isdisabled);
    $("#TODOthers").attr("disabled", isdisabled);
    $form.find("input[name=SupportingOfDelivery]").attr("disabled", isdisabled);
    $("#SODOthers").attr("disabled", isdisabled);
    $("#SODCrane_Forklift").attr("disabled", isdisabled);
    $form.find("input[name=Incoterm]").attr("disabled", isdisabled);
    $("#INCTOthers").attr("disabled", isdisabled);
    $form.find("input[name=Transportation]").attr("disabled", isdisabled);
    $("#TransportationOthers").attr("disabled", isdisabled);
    $form.find("input[name=IsDemob]").attr("disabled", isdisabled);
    $form.find("input[name=ModaTransport]").attr("disabled", isdisabled);
    $form.find("input[name=PenaltyLateness]").attr("disabled", isdisabled);

    if (formType == "R") {
        $("#refSONo").parent().removeClass('hidden');
        $('.btn-refNo button').attr("disabled", false);
        $("#refNo").attr("disabled", false);
    }

    $('div.checkbox.sendmail').find('input[type="checkbox"]').attr("disabled", isdisabled);
    if (['revised', 'submit'].indexOf(referenceEvent.dataRef.header.Status) > -1 && formType == "U") {
        //$form.find("input[name=SendEmailToCkb]").attr("disabled", false);
        $form.find("input[name=SendEmailToCkbSurabaya]").attr("disabled", false);
        $form.find("input[name=SendEmailToCkbMakassar]").attr("disabled", false);
        $form.find("input[name=SendEmailToCkbCakungStandartKit]").attr("disabled", false);
        $form.find("input[name=SendEmailToCkbBalikpapan]").attr("disabled", false);
        $form.find("input[name=SendEmailToCkbBanjarmasin]").attr("disabled", false);
  
    } else if (referenceEvent.dataRef.header.Status == 'approve' && formType == "U") {
        $('div.checkbox.sendmail').find('input[type="checkbox"]').attr("disabled", false);
        //$form.find("input[name=SendEmailToCkb]").attr("disabled", isdisabled);
        $form.find("input[name=SendEmailToCkbSurabaya]").attr("disabled", true);
        $form.find("input[name=SendEmailToCkbMakassar]").attr("disabled", true);
        $form.find("input[name=SendEmailToCkbCakungStandartKit]").attr("disabled", true);
        $form.find("input[name=SendEmailToCkbBalikpapan]").attr("disabled", true);
        $form.find("input[name=SendEmailToCkbBanjarmasin]").attr("disabled", true);
<<<<<<< HEAD
    } else if (referenceEvent.dataRef.header.Status == 'rerouted' && formType == "U") {
 
        $form.find("input[name=SendEmailToCkbSurabaya]").attr("disabled", true);
        $form.find("input[name=SendEmailToCkbMakassar]").attr("disabled", true);
        $form.find("input[name=SendEmailToCkbCakungStandartKit]").attr("disabled", true);
        $form.find("input[name=SendEmailToCkbBalikpapan]").attr("disabled", true);
        $form.find("input[name=SendEmailToCkbBanjarmasin]").attr("disabled", true);

        $form.find("input[name=SendEmailToServiceTUPalembang]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUPekanBaru]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUJambi]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUBengkulu]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUTanjungEnim]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUMedan]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUPadang]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUBangkaBelitung]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUBandarLampung]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUBSD]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUSurabaya]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUManado]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUJayapura]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUSorong]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUSamarinda]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUBalikpapan]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUMakassar]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUSemarang]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUPontianak]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUBatuLicin]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUSangatta]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUKendari]").attr("disabled", false);
        $form.find("input[name=SendEmailToServiceTUMeulaboh]").attr("disabled", false);    
=======
>>>>>>> 639d8d0 (Intial commit)
    }


}
function showFormRequisition() {
    $("#formRequisition").removeClass("hidden");
};
function invalidRef() {
    $("#refDate").hide();
    $("#refNo").removeProp('readonly', false);
}

<<<<<<< HEAD
function viewhistoryreroute() {
    var refNo = $("#refNo").val();
    var refUrl;   
    refUrl = myApp.root + 'DTS/GetHistoryReroute?number=' + refNo
   
    $.ajax({
        type: "GET",
        url: refUrl,
        beforeSend: function () { },
        complete: function () { },
        dataType: "json",
        success: function (d) {
            if (d && d.header.RefNo != null && d.header.RefNo != 'null' && d.header.RefNo != '') {
               
               
                $("#OldCustName").val(d.header.CustName || '');
                $("#OldCustName").attr("disabled", true);        
                $("#newcustName").hide();
                $("#oldcustNamegroup").show();
                $("#CustAddress").val(d.header.CustAddress);
                $("#PicName").val(d.header.PicName);
                $("#PicHP").val(d.header.PicHP);
                $("#Kecamatan").val(d.header.Kecamatan);
                $("#Kabupaten").val(d.header.Kabupaten);
                $("#Province").val(d.header.Province);
                $("#RefNo").val(d.header.RefNo);
                $("#SoNo").val(d.header.SoNo);
                $("#SoDate").val(formatDate(d.header.SoDate));
                $("button[name=ReRoute]").attr("disabled", false);
                $('.SDOC-container .row .upload').hide();
                $('.SDOC-container .row.preview').hide();                
            } else {
                invalidRef();
                sAlert('Error', 'REF NO ' + refNo + " Not Found!", "error");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            invalidRef();
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
           
        }
    });
}

=======
>>>>>>> 639d8d0 (Intial commit)
function referenceClick() {
    var refNo = $("#refNo").val();
    var refUrl;
    $("#refDate").html('');
    $("#refDate").show();
    $("#refNo").prop('readonly', true);

    if (!refNo || refNo == '') {
        invalidRef();
        sAlert('Error', "Please fill NO!", "error");
        return;
    }
    if (referenceEvent.dataRef.header.RefNo == refNo) {
        invalidRef();
        sAlert('Warning', "Please change your reference number!", "warning");
        return;
    }

    if ($("#refSONo").parent().hasClass('active')) {
        refUrl = myApp.root + 'DTS/GetDRReferenceNo?keyType=' + "SO" + "&number=" + refNo;
    }
    else if ($("#refSTRNo").parent().hasClass('active')) {
        refUrl = myApp.root + 'DTS/GetDRReferenceNo?keyType=' + "STR" + "&number=" + refNo;
    }
    else if ($("#refPONo").parent().hasClass('active')) {
        refUrl = myApp.root + 'DTS/GetDRReferenceNo?keyType=' + "PO" + "&number=" + refNo;
    }
    else if ($("#refDINo").parent().hasClass('active')) {
        refUrl = myApp.root + 'DTS/GetDRReferenceNo?keyType=' + "DI" + "&number=" + refNo;
    }
    else {
        invalidRef();
        return;
    }

    $.ajax({
        type: "GET",
        url: refUrl,
        beforeSend: function () { },
        complete: function () { },
        dataType: "json",
        success: function (d) {
            //console.log(d.header.RefNo);
            if (d && d.header.RefNo != null && d.header.RefNo != 'null' && d.header.RefNo != '') {
                var newOption2 = new Option(d.header.CustName || '', d.header.CustID, false, false);
                $('#CustID').append(newOption2).trigger('change');
                $("#CustID").val(d.header.CustID).trigger("change");
                $("#CustName").val(d.header.CustName || '');
                $("#CustAddress").val(d.header.CustAddress);
                $("#PicName").val(d.header.PicName);
                $("#PicHP").val(d.header.PicHP);
                $("#Kecamatan").val(d.header.Kecamatan);
                $("#Kabupaten").val(d.header.Kabupaten);
                $("#Province").val(d.header.Province);
                $("#RefNo").val(d.header.RefNo);
                $("#SoNo").val(d.header.SoNo);
                $("#SoDate").val(formatDate(d.header.SoDate));

                $("button[name=ReRoute]").attr("disabled", false);
                $("#CustID").attr("disabled", false);
                $("#CustName").attr("disabled", false);
                $("#CustAddress").attr("disabled", false);
                $("#PicName").attr("disabled", false);
                $("#PicHP").attr("disabled", false);
                $("#Kecamatan").attr("disabled", false);
                $("#Kabupaten").attr("disabled", false);
                $("#Province").attr("disabled", false);
            } else {
                invalidRef();
                sAlert('Error', 'REF NO ' + refNo + " Not Found!", "error");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            invalidRef();
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
            // sAlert('Error', "SO NO: " + refNo + " IS NOT VALID!", "error");
        }
    });
}
function referenceReset() {
    $('#refNo').val('');
}

var referenceEvent = {
    formType: "V",
    dataRef: null,
    fillFromSo: function (result, formType) {
        var SELF = referenceEvent;
        result.header.RefNoDateString = 'SO DATE: ' + result.header.RefNoDateString;
        SELF.fillData(result, formType);
    },
    fillData: function (dataRef, formType = 'I') {
        referenceEvent.dataRef = dataRef;
        var isdisabled = true;
        if (dataRef.header.Status === "reject" || dataRef.header.Status === "revise") {
            $("#RejectNoteSpace").show();
            $("textarea[name=RejectNote]").val(dataRef.header.RejectNote);
        }
        if (formType == 'I') {
            $form.find("input[name=formType]").val("I");
        } else {
            $("#refNo").val(dataRef.header.RefNo);
        }
        $("#ID").val(dataRef.header.ID);
        $("#KeyCustom").val(dataRef.header.KeyCustom);
        $("#refDate").html(dataRef.header.RefNoDateString);
        $("#ReqName").val(formatUpperCase(dataRef.header.ReqName));
        $("#RequestID").val(dataRef.header.RequestID);
        $("#RequestHP").val(dataRef.header.ReqHp);

        var sales1Op = new Option(formatUpperCase(dataRef.header.Sales1Name), dataRef.header.Sales1ID, false, false);
        $('#Sales1ID').append(sales1Op).trigger('change');
        $("#Sales1ID").val(dataRef.header.Sales1ID).trigger("change");
        $("#Sales1Name").val(formatUpperCase(dataRef.header.Sales1Name));
        $("#Sales1Hp").val(dataRef.header.Sales1Hp);
        var sales2Op = new Option(formatUpperCase(dataRef.header.Sales2Name), dataRef.header.Sales2ID, false, false);
        $('#Sales2ID').append(sales2Op).trigger('change');
        $("#Sales2ID").val(dataRef.header.Sales2ID).trigger("change");
        $("#Sales2Name").val(formatUpperCase(dataRef.header.Sales2Name));
        $("#Sales2Hp").val(dataRef.header.Sales2Hp);
        var newOption2 = new Option(formatUpperCase(dataRef.header.CustName) || '', dataRef.header.CustID, false, false);
        $('#CustID').append(newOption2).trigger('change');
        $("#CustID").val(dataRef.header.CustID).trigger("change");
        $("#CustName").val(formatUpperCase(dataRef.header.CustName) || '');
        $("#CustAddress").val(formatUpperCase(dataRef.header.CustAddress));
        $("#PicName").val(formatUpperCase(dataRef.header.PicName));
        $("#PicHP").val(formatUpperCase(dataRef.header.PicHP));
        $("#Kecamatan").val(formatUpperCase(dataRef.header.Kecamatan));
        $("#Kabupaten").val(formatUpperCase(dataRef.header.Kabupaten));
        $("#Province").val(formatUpperCase(dataRef.header.Province));
        $("#Origin").val(formatUpperCase(dataRef.header.Origin));
        $("#CreateDate").val(formatDateTimeLocal(formatUpperCase(dataRef.header.CreateDate)));
        $("#RequestNotes").val(dataRef.header.RequestNotes);

        $("#ExpectedTimeArrival").val(formatDateLocal(dataRef.header.ExpectedTimeArrival));
        $("#ExpectedTimeLoading").val(formatDateLocal(dataRef.header.ExpectedTimeLoading));
        $("#ActualTimeArrival").val(formatDateLocal(dataRef.header.ActualTimeArrival));
        $("#ActualTimeDeparture").val(formatDateLocal(dataRef.header.ActualTimeDeparture));

        if (dataRef.header.Unit && dataRef.header.Unit != 'null') {
            var Unit = dataRef.header.Unit.split(',');
            $.each(Unit, function (index, itemTod) {
                $form.find("input[name=unit][value='" + itemTod + "']").prop("checked", true);
            });
            if (dataRef.header.Unit == "ATTACHMENT") {
                //console.log(dataRef.header.Unit);
                $("#section-dimension").show();
                $('#u-weight').val(dataRef.header.UnitDimWeight);
                $('#u-length').val(dataRef.header.UnitDimLength);
                $('#u-width').val(dataRef.header.UnitDimWidth);
                $('#u-height').val(dataRef.header.UnitDimHeight);
                $('#u-vol').val(dataRef.header.UnitDimVol);
            } else {
                $("#section-dimension").hide();
            }
        }
        if (dataRef.header.TermOfDelivery && dataRef.header.TermOfDelivery != 'null') {
            var TODs = dataRef.header.TermOfDelivery.split(',');
            $.each(TODs, function (index, itemTod) {
                if (itemTod.search("OTHERS") > -1) {
                    var strSplit = itemTod.split('-');
                    $('#TODOthers').val((strSplit.length > 0) ? strSplit[1] : '');
                    $("#TODOthers").removeAttr('readonly');
                    $form.find("input[name=TermOfDelivery][value='" + strSplit[0] + "']").prop("checked", true);
                } else {
                    $form.find("input[name=TermOfDelivery][value='" + itemTod + "']").prop("checked", true);
                }
            });
        }
        if (dataRef.header.SupportingOfDelivery && dataRef.header.SupportingOfDelivery != 'null') {
            var SODs = dataRef.header.SupportingOfDelivery.split(',');
            $.each(SODs, function (index, itemSod) {
                if (itemSod.search("OTHERS") > -1) {
                    var strSplit = itemSod.split('-');
                    $('#SODOthers').val((strSplit.length > 0) ? strSplit[1] : '');
                    $("#SODOthers").removeAttr('readonly');
                    $form.find("input[name=SupportingOfDelivery][value='" + strSplit[0] + "']").prop("checked", true);
                } else if (itemSod.search("CRANE/FORKLIFT") > -1) {
                    var strSplit = itemSod.split('-');
                    $('#SODCrane_Forklift').val((strSplit.length > 0) ? strSplit[1] : '');
                    $("#SODCrane_Forklift").removeAttr('readonly');
                    $form.find("input[name=SupportingOfDelivery][value='" + strSplit[0] + "']").prop("checked", true);
                } else {
                    $form.find("input[name=SupportingOfDelivery][value='" + itemSod + "']").prop("checked", true);
                }

            });
        }
        if (dataRef.header.Incoterm && dataRef.header.Incoterm != 'null') {
            var INCTs = dataRef.header.Incoterm.split(',');
            $.each(INCTs, function (index, itemInct) {
                if (itemInct.search("OTHERS") > -1) {
                    var strSplit = itemInct.split('-');
                    $('#INCTOthers').val((strSplit.length > 0) ? strSplit[1] : '');
                    $("#INCTOthers").removeAttr('readonly');
                    $form.find("input[name=Incoterm][value='" + strSplit[0] + "']").prop("checked", true);
                } else {
                    $form.find("input[name=Incoterm][value='" + itemInct + "']").prop("checked", true);
                }
            });
        }
        if (dataRef.header.Transportation && dataRef.header.Transportation != 'null') {
            var Transportation = dataRef.header.Transportation.split(',');
            $.each(Transportation, function (index, itemInct) {
                if (itemInct.search("OTHERS") > -1) {
                    var strSplit = itemInct.split('-');
                    $('#TransportationOthers').val((strSplit.length > 0) ? strSplit[1] : '');
                    $("#TransportationOthers").removeAttr('readonly');
                    $form.find("input[name=Transportation][value='" + strSplit[0] + "']").prop("checked", true);
                } else {
                    $form.find("input[name=Transportation][value='" + itemInct + "']").prop("checked", true);
                }
            });
        }
        if (dataRef.header.ModaTransport && dataRef.header.ModaTransport != 'null') {
            var ModaTransport = dataRef.header.ModaTransport.split(',');
            $.each(ModaTransport, function (index, itemInct) {
                $form.find("input[name=ModaTransport][value='" + itemInct + "']").prop("checked", true);
            });
        }
<<<<<<< HEAD
       
      // CKB
        $form.find("input[name=SendEmailToCkbSurabaya]").prop("checked", dataRef.header.SendEmailToCkbSurabaya == true);      
=======
        //$form.find("input[name=SendEmailToCkb]").prop("checked", dataRef.header.SendEmailToCkb == true);
      // CKB
        $form.find("input[name=SendEmailToCkbSurabaya]").prop("checked", dataRef.header.SendEmailToCkbSurabaya == true);
        //$form.find("input[name=SendEmailToCkbSurabaya]").attr("disabled", isdisabled);
>>>>>>> 639d8d0 (Intial commit)
        $form.find("input[name=SendEmailToCkbMakassar]").prop("checked", dataRef.header.SendEmailToCkbMakassar == true);   
        $form.find("input[name=SendEmailToCkbCakungStandartKit]").prop("checked", dataRef.header.SendEmailToCkbCakungStandartKit == true);
        $form.find("input[name=SendEmailToCkbBalikpapan]").prop("checked", dataRef.header.SendEmailToCkbBalikpapan == true);
        $form.find("input[name=SendEmailToCkbBanjarmasin]").prop("checked", dataRef.header.SendEmailToCkbBanjarmasin == true);  
        //TU WAREHOUSE
        $form.find("input[name=SendEmailToCakung]").prop("checked", dataRef.header.SendEmailToCakung == true);
        $form.find("input[name=SendEmailToBalikPapan]").prop("checked", dataRef.header.SendEmailToBalikPapan == true);
        $form.find("input[name=SendEmailToMakasar]").prop("checked", dataRef.header.SendEmailToMakasar == true);
        $form.find("input[name=SendEmailToSurabaya]").prop("checked", dataRef.header.SendEmailToSurabaya == true);
        $form.find("input[name=SendEmailToBanjarMasin]").prop("checked", dataRef.header.SendEmailToBanjarMasin == true);       
        $form.find("input[name=SendEmailToCileungsi]").prop("checked", dataRef.header.SendEmailToCileungsi == true);
        // TU SERVICE
        $form.find("input[name=SendEmailToServiceTUPalembang]").prop("checked", dataRef.header.SendEmailToServiceTUPalembang == true);
        $form.find("input[name=SendEmailToServiceTUPekanBaru]").prop("checked", dataRef.header.SendEmailToServiceTUPekanBaru == true);
        $form.find("input[name=SendEmailToServiceTUJambi]").prop("checked", dataRef.header.SendEmailToServiceTUJambi == true);
        $form.find("input[name=SendEmailToServiceTUBengkulu]").prop("checked", dataRef.header.SendEmailToServiceTUBengkulu == true);
        $form.find("input[name=SendEmailToServiceTUTanjungEnim]").prop("checked", dataRef.header.SendEmailToServiceTUTanjungEnim == true);
        $form.find("input[name=SendEmailToServiceTUMedan]").prop("checked", dataRef.header.SendEmailToServiceTUMedan == true);
        $form.find("input[name=SendEmailToServiceTUPadang]").prop("checked", dataRef.header.SendEmailToServiceTUPadang == true);
        $form.find("input[name=SendEmailToServiceTUBangkaBelitung]").prop("checked", dataRef.header.SendEmailToServiceTUBangkaBelitung == true);
        $form.find("input[name=SendEmailToServiceTUBandarLampung]").prop("checked", dataRef.header.SendEmailToServiceTUBandarLampung == true);
        $form.find("input[name=SendEmailToServiceTUBSD]").prop("checked", dataRef.header.SendEmailToServiceTUBSD == true);
        $form.find("input[name=SendEmailToServiceTUSurabaya]").prop("checked", dataRef.header.SendEmailToServiceTUSurabaya == true);
        $form.find("input[name=SendEmailToServiceTUManado]").prop("checked", dataRef.header.SendEmailToServiceTUManado == true);
        $form.find("input[name=SendEmailToServiceTUJayapura]").prop("checked", dataRef.header.SendEmailToServiceTUJayapura == true);
        $form.find("input[name=SendEmailToServiceTUSorong]").prop("checked", dataRef.header.SendEmailToServiceTUSorong == true);
        $form.find("input[name=SendEmailToServiceTUSamarinda]").prop("checked", dataRef.header.SendEmailToServiceTUSamarinda == true);
        $form.find("input[name=SendEmailToServiceTUBalikpapan]").prop("checked", dataRef.header.SendEmailToServiceTUBalikpapan == true);
        $form.find("input[name=SendEmailToServiceTUMakassar]").prop("checked", dataRef.header.SendEmailToServiceTUMakassar == true);
        $form.find("input[name=SendEmailToServiceTUSemarang]").prop("checked", dataRef.header.SendEmailToServiceTUSemarang == true);
        $form.find("input[name=SendEmailToServiceTUPontianak]").prop("checked", dataRef.header.SendEmailToServiceTUPontianak == true);
        $form.find("input[name=SendEmailToServiceTUBatuLicin]").prop("checked", dataRef.header.SendEmailToServiceTUBatuLicin == true);
        $form.find("input[name=SendEmailToServiceTUSangatta]").prop("checked", dataRef.header.SendEmailToServiceTUSangatta == true);
        $form.find("input[name=SendEmailToServiceTUKendari]").prop("checked", dataRef.header.SendEmailToServiceTUKendari == true);
<<<<<<< HEAD
        $form.find("input[name=SendEmailToServiceTUMeulaboh]").prop("checked", dataRef.header.SendEmailToServiceTUMeulaboh == true);        
=======

>>>>>>> 639d8d0 (Intial commit)
        if (dataRef.header.IsDemob) {
            $form.find("input[name=IsDemob][value=true]").prop('checked', true).attr("disabled", isdisabled);
        } else {
            $form.find("input[name=IsDemob][value=false]").prop('checked', true).attr("disabled", isdisabled);
        }

        if (dataRef.header.PenaltyLateness) {
            $form.find("input[name=PenaltyLateness][value=true]").prop('checked', true).attr("disabled", isdisabled);
        } else {
            $form.find("input[name=PenaltyLateness][value=false]").prop('checked', true).attr("disabled", isdisabled);
        }
        $("#SoNo").val(dataRef.header.SoNo);
        $("#SoDate").val(formatDateLocal(dataRef.header.SoDate));
        $("#STRNo").val(dataRef.header.STRNo);
        $("#STRDate").val(formatDateLocal(dataRef.header.STRDate));
        $("#STONo").val(dataRef.header.STONo);
        $("#STODate").val(formatDateLocal(dataRef.header.STODate));
        $("#DoNo").val(dataRef.header.DoNo);
        $("#OdDate").val(formatDateLocal(dataRef.header.OdDate));
        $("#DINo").val(dataRef.header.DINo);
        $("#DIDate").val(formatDateLocal(dataRef.header.DIDate));
        if (dataRef.header.DIDateSAP !== null) {
            var data = dataRef.header.DIDateSAP;
            var n = data.includes("-");
            if (n !== true) {
                var arr = data.split(';');
                var DIDateSAP = "";
                for (var i = 0; i < arr.length; i++) {
                    var year = arr[i].substring(0, 4);
                    var month = arr[i].substring(4, 6);
                    var day = arr[i].substring(6, 8);
                    if (DIDateSAP == "") {
                        DIDateSAP = year + "-" + month + "-" + day;
                    }
                    else if (DIDateSAP !== "") {
                        DIDateSAP = DIDateSAP + ";" + year + "-" + month + "-" + day;
                    }

                }
                $("#DIDateSAP").val(DIDateSAP);
            }
            else {
                $("#DIDateSAP").val(dataRef.header.DIDateSAP);
            }
        }
        $('.SDOC-container').show();
        $('.SDOC-container .row .upload').show();
        $('.SDOC-container .row.preview').show();
        $('#SDOC-url').attr('href', myApp.root + dataRef.header.SupportingDocument);
        if (formType == 'R') {
            $('.SDOC-container .row .upload').show();
            $('.SDOC-container .row.preview').hide();
<<<<<<< HEAD

      
        } else if (formType == 'U' && dataRef.header.Status == "rerouted" && dataRef.header.RefNoType == "STR" && dataRef.header.ReRouted == true){
            $('.SDOC-container .row .upload').show();
            $('.SDOC-container .row.preview').show();
            showFilePreview(dataRef.header);
        }else {
            $('.SDOC-container .row .upload').show();
=======
            //showFilePreview(dataRef.header);
        } else {
            $('.SDOC-container .row .upload').hide();
>>>>>>> 639d8d0 (Intial commit)
            $('.SDOC-container .row.preview').show();
            showFilePreview(dataRef.header);
        }
        $('.fileinput-remove span').hide();
        setFormSisable(isdisabled, formType);

        $('#SendEmailNotes').val('');
        $('input[name="SendEmailCheckAll"]').prop("checked", false);

        if (formType == 'U' && dataRef.header.Status == "rerouted" && dataRef.header.RefNoType == "STR" && dataRef.header.ReRouted == true) {
<<<<<<< HEAD
            $("button[name=ReRoute]").attr("disabled", false);          
=======
            $("button[name=ReRoute]").attr("disabled", false);
            // $("button[name=ReRoute]").html('<i class="fa fa-check-circle"></i> Complete');
>>>>>>> 639d8d0 (Intial commit)
            $("#CustID").attr("disabled", false);
            $("#CustName").attr("disabled", false);
            $("#CustAddress").attr("disabled", false);
            $("#PicName").attr("disabled", false);
            $("#PicHP").attr("disabled", false);
<<<<<<< HEAD
            $("#ExpectedTimeArrival").attr("disabled", true);
            $("#ExpectedTimeLoading").attr("disabled", true);
            $("button[name=Reject]").show();
            $("button[name=Approve]").hide();
            $("button[name=Revise]").hide();
            $form.find("input[name=SupportingOfDelivery]").attr("disabled", false);
            $("#btnhistory").show();
        }
        if (formType == 'U' && dataRef.header.Status == "rerouted" && dataRef.header.RefNoType == "SO" && dataRef.header.ReRouted == true) {
            
            $("#ExpectedTimeArrival").attr("disabled", true);
            $("#ExpectedTimeLoading").attr("disabled", true);
         
        }
        if (['submit', 'approve', 'revised'].indexOf(dataRef.header.Status) > -1 && formType == 'U') {
            $form.find("input[name=ModaTransport]").attr("disabled", false);
        }
        if (formType == 'V' && dataRef.header.Status === "rerouted") {
            $("#btnhistory").show();
            $("#btnhistory").attr("disabled", false);
        }
=======
            $("button[name=Reject]").hide();
            $("button[name=Approve]").hide();
            $("button[name=Revise]").hide();
            //$("#Kecamatan").attr("disabled", false);
            //$("#Kabupaten").attr("disabled", false);
            //$("#Province").attr("disabled", false);
        }
     
        if (['submit', 'approve', 'revised'].indexOf(dataRef.header.Status) > -1 && formType == 'U') {
            $form.find("input[name=ModaTransport]").attr("disabled", false);
        }

>>>>>>> 639d8d0 (Intial commit)
      
        requestingForm.initTableUnit(dataRef.details, formType, isdisabled);
        showFormRequisition();
    }
};
function resetForm() {
    $form[0].reset();
    resetReference();
    resetFormRequisition();
<<<<<<< HEAD
    $("#btnhistory").hide();
=======
>>>>>>> 639d8d0 (Intial commit)
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
    if (referenceEvent.formType != "R") {
        $("#formRequest")[0].reset();
        $(".error").hide();
        $("#formRequest").find('input').attr('disabled', false);
        $("select[name=Sales1Name]").attr("disabled", false);
        $("select[name=Sales2Name]").attr("disabled", false);
        $("select[name=CustID]").attr("disabled", false);
        $form.find("textarea[name=CustAddress]").attr("disabled", false);
        $("#formRequisition").addClass('hidden');
        $("#refDate").html('');
        $("#refDate").hide();
        $("#refNo").removeProp('readonly', false);
        $("#section-dimension").hide();
        $('.btn-refNo button').removeAttr("disabled");
        $("div.button-action button").removeAttr("disabled");
        $("button[name=ReRoute]").attr("disabled", "disabled");
    }
}
var requestingForm = {
    initTableUnit: function (_data, formType, isdisabled) {
        var $tableUnit = $('#tableDeliveryRequisitionUnit');
        $tableUnit.bootstrapTable('destroy');
        $tableUnit.bootstrapTable({
            cache: false,
            pagination: false,
            search: false,
            striped: true,
            clickToSelect: true,
            sidePagination: 'server',
            showColumns: false,
            showRefresh: false,
            smartDisplay: false,
            pageSize: '10',
            formatNoMatches: function () {
                return '<span class="noMatches">-</span>';
            },
            data: _data,
            columns: [
                {
                    title: '#',
                    halign: 'center',
                    align: 'center',
                    checkbox: true,
                    formatter: function (value, row, index) {
                        return {
                            disabled: (row.Selectable == 0 || isdisabled == true),
                            checked: row.Checked == 1
                        }
                    }
                },
                {
                    field: 'RefNo',
                    title: 'RefNo',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    visible: false,
                    sortable: true
                },
                {
                    field: 'RefItemId',
                    title: 'RefItemId',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    visible: false,
                    sortable: true
                },
                {
                    field: 'Model',
                    title: 'MODEL',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    sortable: true
                },
                {
                    field: 'SerialNumber',
                    title: 'SN',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    sortable: true
                },
                {
                    field: 'Batch',
                    title: 'BATCH',
                    halign: 'left',
                    align: 'left',
                    class: 'text-nowrap',
                    sortable: true
                }
            ]
        });
    },
    initEventRadio: function () {
        $('input[type=radio][name=TermOfDelivery]').change(function () {
            $("#TODOthers").val('');
            if (this.value == 'OTHERS') {
                $("#TODOthers").removeAttr('readonly');
            }
            else {
                $("#TODOthers").attr('readonly', true);
            }
        });
        $('input[type=radio][name=SupportingOfDelivery]').change(function () {
            $("#SODOthers").val('');
            if (this.value == 'OTHERS') {
                $("#SODOthers").removeAttr('readonly');
            } else if (this.value == 'CRANE/FORKLIFT') {
                $("#SODCrane_Forklift").removeAttr('readonly');
            } else {
                $("#SODOthers").attr('readonly', true);
                $("#SODCrane_Forklift").attr('readonly', true);
            }
        });
        $('input[type=radio][name=Incoterm]').change(function () {
            $("#INCTOthers").val('');
            if (this.value == 'OTHERS') {
                $("#INCTOthers").removeAttr('readonly');
            }
            else {
                $("#INCTOthers").attr('readonly', true);
            }
        });
        $('input[type=radio][name=Transportation]').change(function () {
            $("#TransportationOthers").val('');
            if (this.value == 'OTHERS') {
                $("#TransportationOthers").removeAttr('readonly');
            }
            else {
                $("#TransportationOthers").attr('readonly', true);
            }
        });
        $("input[type=radio][name='unit']").change(function () {
            if (this.value === "ATTACHCMENT") {
                $("#section-dimension").show();
            } else {
                $("#section-dimension").hide();
            }
        });
        $("input[type=radio][name='reference']").change(function () {
<<<<<<< HEAD
         
=======
            //resetFormRequisition();
            //console.log($('input[name=formType]').val());
>>>>>>> 639d8d0 (Intial commit)
        });
    }
}

function showFilePreview(header) {
    var filesPreview = [];
    var filesPreviewConfig = [];
    if (header.SupportingDocument && header.SupportingDocument != "") {
        filesPreview.push(myApp.root + header.SupportingDocument);
    }
    if (header.SupportingDocument1 && header.SupportingDocument1 != "") {
        filesPreview.push(myApp.root + header.SupportingDocument1);
    }
    if (header.SupportingDocument2 && header.SupportingDocument2 != "") {
        filesPreview.push(myApp.root + header.SupportingDocument2);
    }
    if (header.SupportingDocument3 && header.SupportingDocument3 != "") {
        filesPreview.push(myApp.root + header.SupportingDocument3);
    }
    if (header.SupportingDocument4 && header.SupportingDocument4 != "") {
        filesPreview.push(myApp.root + header.SupportingDocument4);
    }
    if (header.SupportingDocument5 && header.SupportingDocument5 != "") {
        filesPreview.push(myApp.root + header.SupportingDocument5);
    }

    $("#SDOCPreview").fileinput('destroy');
    if (filesPreview.length > 0) {
        for (var x in filesPreview) {
            var filename = filesPreview[x].replace(/^.*[\\\/]/, '');
            var fileExtension = filename.replace(/^.*\./, '');
            if (['doc', 'docx', 'xls', 'xlsx', 'pdf'].indexOf(fileExtension) > -1) {
                filesPreviewConfig.push({ type: "office", caption: filename });
                filesPreview[x] = '<div class="file-preview-frame"><h3><i class="glyphicon glyphicon-file"></i></h3>' + filename + '</div>';
            } else {
                filesPreviewConfig.push({ caption: filename });
                filesPreview[x] = '<img src="' + filesPreview[x] + '" class="file-preview-image" style="width: 100%;" />';
            }
        }
        $("#SDOCPreview").fileinput({
            showUpload: false,
            previewFileType: 'any',
            initialPreview: filesPreview,
            initialPreviewAsData: false,
            showCaption: false,
            showBrowse: false,
            initialPreviewShowDelete: false,
            initialPreviewDownloadUrl: myApp.root + 'Upload/DTS/deliveryrequisition/' + header.ID + '/{filename}',
            initialPreviewConfig: filesPreviewConfig,
        });
    } else {
        $('.SDOC-container .row.preview').hide();
    }

}

$(function () {
    $("#CustID").select2({
        placeholder: 'Nama Customer (Sudah terisi otomatis daari SAP)',
        dropdownParent: $('#myModalRequest'),
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
    $('#CustID').on('select2:select', function (e) {
        var data = e.params.data;
        $("div[for=CustID]").hide();
        $("input[name=CustName]").val(data.Customer_Full_Name);
        $("textarea[name=CustAddress]").val(data.Jalan);
    });
    $("#Sales1ID").select2({
        placeholder: 'Nama sales yang meminta pengiriman',
        dropdownParent: $('#myModalRequest'),
        minimumInputLength: 3,
        ajax: {
            url: myApp.root + 'DTS/GetMasterEmployee',
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
                //console.log(data);
                var newData = $.map(data, function (obj) {
                    obj.id = obj.UserID;
                    obj.text = obj.FullName;
                    return obj;
                });
                // Tranforms the top-level key of the response object from 'items' to 'results'
                return {
                    results: newData
                };
            }
            // Additional AJAX parameters go here; see the end of this chapter for the full code of this example
        }
    });
    $('#Sales1ID').on('select2:select', function (e) {
        var data = e.params.data;
        //console.log(data);
        $("div[for=Sales1ID]").hide();
        $("input[name=Sales1Name]").val(data.FullName);
        $("input[name=Sales1Hp]").val(data.Phone);
    });
    $("#Sales2ID").select2({
        placeholder: 'Nama sales yang menerima unit',
        dropdownParent: $('#myModalRequest'),
        minimumInputLength: 3,
        ajax: {
            url: myApp.root + 'DTS/GetMasterEmployee',
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
                //console.log(data);
                var newData = $.map(data, function (obj) {
                    obj.id = obj.UserID;
                    obj.text = obj.FullName;
                    return obj;
                });
                // Tranforms the top-level key of the response object from 'items' to 'results'
                return {
                    results: newData
                };
            }
            // Additional AJAX parameters go here; see the end of this chapter for the full code of this example
        }
    });
    $('#Sales2ID').on('select2:select', function (e) {
        var data = e.params.data;
        //console.log(data);
        $("div[for=Sales2ID]").hide();
        $("input[name=Sales2Name]").val(data.FullName);
        $("input[name=Sales2Hp]").val(data.Phone);
    });
    $("input[name=PenaltyLateness]").click(function () {
        $('input[name=PenaltyLateness]').prop('checked', false);
        $(this).prop('checked', true);
    });
    $("input[name=Transportation]").click(function () {
        $('input[name=Transportation]').prop('checked', false);
        $(this).prop('checked', true);
    });
    $("input[name=unit]").click(function () {
        $('input[name=unit]').prop('checked', false);
        $(this).prop('checked', true);
    });

    $("#myModalRequest").on('show.bs.modal', function () {
        prepareModal();
        requestingForm.initEventRadio();
        $(".date").datepicker().on('show.bs.modal', function (event) {
            event.stopPropagation();
        });
    });
    $('.u-dimension').change(function (e) {
        var uLength = $('#u-length').val();
        var uWidth = $('#u-width').val();
        var uHeight = $('#u-height').val();

        uLength = parseFloat(uLength) || 0;
        uWidth = parseFloat(uWidth) || 0;
        uHeight = parseFloat(uHeight) || 0;
        $('#u-vol').val(uLength * uWidth * uHeight);
    });
});