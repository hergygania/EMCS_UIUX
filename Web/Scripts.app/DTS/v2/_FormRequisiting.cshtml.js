$form = $("#formRequest");
$table = $("#tableDeliveryRequisition");

var validator = $form.validate({
    ignore: ":hidden",
    highlight: function (element, errorClass, validClass) {
        $form.find("div[for=" + element.name + "]").show();
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass(errorClass).addClass(validClass);
        if (element.name && element.name != "") { 
            if (element.name == 'SDOC[]') {
                $form.find("div[for=SDOC]").hide();
            } else {
                $form.find("div[for=" + element.name + "]").hide();
            } 
        }
    }
});

function prepareModal() {
    var state = $('input[name=formType]').val();
    resetReference();
    resetFormRequisition();
    if (state == 'I') {
        $(".modal-title").html('NEW DR');
    }
    //else if (state == 'V')
    //    $(".modal-title").html('VIEW DR');
    //else if (state == "U")
    //    $(".modal-title").html('EDIT DR');
    
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
    //$("#Kecamatan").attr("disabled", isdisabled);
    //$("#Kabupaten").attr("disabled", isdisabled);
    //$("#Province").attr("disabled", isdisabled);
    $("#ProvinceID").attr("disabled", isdisabled);
    $("#SubDistrictID").attr("disabled", isdisabled);
    $("#DistrictID").attr("disabled", isdisabled);  
    $("#Origin").attr("disabled", isdisabled);
    $("#RequestNotes").attr("disabled", isdisabled);

    $("#ExpectedTimeArrival").attr("disabled", isdisabled);
    $("#ExpectedTimeLoading").attr("disabled", isdisabled);
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

    $form.find("input[name=ModaTransport]").attr("disabled", true);
    $form.find("input[name=PenaltyLateness]").attr("disabled", isdisabled);
    $form.find("input[name=IsDemob]").attr("disabled", isdisabled);
    $form.find("input[name=SendEmailToCkbSurabaya]").attr("disabled", isdisabled);
    $form.find("input[name=SendEmailToCkbMakassar]").attr("disabled", isdisabled);
    $form.find("input[name=SendEmailToCkbCakungStandartKit]").attr("disabled", isdisabled);
    $form.find("input[name=SendEmailToCkbBalikpapan]").attr("disabled", isdisabled);
    $form.find("input[name=SendEmailToCkbBanjarmasin]").attr("disabled", isdisabled);
    //$form.find("input[name=SendEmailToCkb]").attr("disabled", "disabled");

    if (formType == 'R') {
        $("#refSONo").parent().removeClass('hidden');
        //$("#refSTRNo").parent().removeClass('hidden');
        $("button[name=ReRoute]").attr("disabled", false);
        $('.btn-refNo button').attr("disabled", false);
        $("#refNo").attr("disabled", false);
    }
}

function showFormRequisition() {
    $("#formRequisition").removeClass("hidden");
    setTimeout(function () {
        $('.modal').data('bs.modal').handleUpdate();
    }, 80);
}
var referenceEvent = {
    formType: '',
    dataRef: '',
    fillFromSo: function (result, formType) {
        var SELF = referenceEvent;
        result.header.RefNoDateString = 'SO DATE: ' + result.header.RefNoDateString;
        SELF.fillData(result, formType);
    },
    fillFromStr: function (result, formType) {
        var SELF = referenceEvent;
        result.header.RefNoDateString = result.header.RefType + ' DATE: ' + result.header.RefNoDateString;
        SELF.fillData(result, formType);
    },
    fillFromDI: function (result, formType) {
        
        var SELF = referenceEvent;
        result.header.RefNoDateString = 'DI DATE: ' + result.header.RefNoDateString;
        SELF.fillData(result, formType);
    },
    fillData: function (dataRef, formType = 'I') {
        referenceEvent.formType = formType;
        referenceEvent.dataRef = dataRef;
        var isdisabled = dataRef.header.Status === "complete" || dataRef.header.Status === "reject" ? "disabled" : false;
        if (formType === "V" || formType === "R") {
            isdisabled = true; // "disabled";
        }
        if (dataRef.header.Status === "reject" || dataRef.header.Status === "revise") {
            $("#RejectNoteSpace").show();
            $("textarea[name=RejectNote]").val(dataRef.header.RejectNote);
        }
        //console.log(formType);
        if (formType == 'I') {
            $form.find("input[name=formType]").val("I");
        } else {
            $("#refNo").val(dataRef.header.RefNo);
        }
        $("#ID").val(dataRef.header.ID);
        $("#KeyCustom").val(dataRef.header.KeyCustom);
        $("#refDate").html(dataRef.header.RefNoDateString);
        if (dataRef.header.ReqHp !== null && dataRef.header.ReqHp !== '') {
            $("#RequestHP").val(dataRef.header.ReqHp);
        }
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
        var newOption3 = new Option(formatUpperCase(dataRef.header.Province) || '', dataRef.header.ProvinsiId, false, false);
        $('#ProvinceID').append(newOption3).trigger('change');
        $("#ProvinceID").val(dataRef.header.ProvinsiId).trigger("change");                
        $("#ProvinceName").val(formatUpperCase(dataRef.header.Province) || '');

        var newOption4 = new Option(formatUpperCase(dataRef.header.Kabupaten) || '', dataRef.header.KabupatenId, false, false);
        $('#DistrictID').append(newOption4).trigger('change');
        $("#DistrictID").val(dataRef.header.KabupatenId).trigger("change");        
        $("#DistrictName").val(formatUpperCase(dataRef.header.Kabupaten));

        var newOption5 = new Option(formatUpperCase(dataRef.header.Kecamatan) || '', dataRef.header.KecamatanId, false, false);
        $('#SubDistrictID').append(newOption5).trigger('change');
        $("#SubDistrictID").val(dataRef.header.KecamatanId).trigger("change");
        $("#SubDistrictName").val(formatUpperCase(dataRef.header.Kecamatan));    
        
        $("#Origin").val(formatUpperCase(dataRef.header.Origin));        
        $("#ExpectedTimeArrival").val(formatDate(dataRef.header.ExpectedTimeArrival));
        $("#ExpectedTimeLoading").val(formatDate(dataRef.header.ExpectedTimeLoading));
        if (formatDate(dataRef.header.ActualTimeArrival) !== '01 JAN 1900') {
            $("#ActualTimeArrival").val(formatDate(dataRef.header.ActualTimeArrival));
        }
        if (formatDate(dataRef.header.ActualTimeDeparture) !== '01 JAN 1900') {
            $("#ActualTimeDeparture").val(formatDate(dataRef.header.ActualTimeDeparture));
        }
        //$("#ActualTimeDeparture").val(formatDate(dataRef.header.ActualTimeDeparture));

        $("#RequestNotes").val(dataRef.header.RequestNotes);

        if (dataRef.header.Unit && dataRef.header.Unit != 'null') {
            var Unit = dataRef.header.Unit.split(',');
            $.each(Unit, function (index, itemTod) {
                $form.find("input[name=unit][value='" + itemTod + "']").prop("checked", true);
            });
            if (dataRef.header.Unit === "ATTACHMENT CAT" || dataRef.header.Unit === "ATTACHMENT NON CAT") {
                //console.log(dataRef.header.Unit);
                $("#section-dimension").show();
                $("#on_pondation").hide();
                $('#u-weight').val(dataRef.header.UnitDimWeight);
                $('#u-length').val(dataRef.header.UnitDimLength);
                $('#u-width').val(dataRef.header.UnitDimWidth);
                $('#u-height').val(dataRef.header.UnitDimHeight);
                $('#u-vol').val(dataRef.header.UnitDimVol);
            } else if (dataRef.header.Unit === "ENGINE") {
                $("#section-dimension").hide();
                $("#on_pondation").show();
            }else {
                $("#section-dimension").hide();
                $("#on_pondation").hide();
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
                    strSplit = itemSod.split('-');
                    $('#SODCrane_Forklift').val((strSplit.length > 0) ? strSplit[1] : '');
                    $("#SODCrane_Forklift").removeAttr('readonly');
                    $form.find("input[name=SupportingOfDelivery][value='" + strSplit[0] + "']").prop("checked", true);
                } else {
                    $form.find("input[name=SupportingOfDelivery][value='" + itemSod + "']").prop("checked", true);
                }

            });
        }

        //console.log(dataRef.header.Incoterm);
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
        if (dataRef.header.SendEmailToCkbSurabaya && dataRef.header.SendEmailToCkbSurabaya == true) {
            $form.find("input[name=SendEmailToCkbSurabaya]").prop("checked", true);
        }
        if (dataRef.header.SendEmailToCkbMakassar && dataRef.header.SendEmailToCkbMakassar == true) {
            $form.find("input[name=SendEmailToCkbMakassar]").prop("checked", true);
        }
        if (dataRef.header.SendEmailToCkbCakungStandartKit && dataRef.header.SendEmailToCkbCakungStandartKit == true) {
            $form.find("input[name=SendEmailToCkbCakungStandartKit]").prop("checked", true);
        }
        if (dataRef.header.SendEmailToCkbBalikpapan && dataRef.header.SendEmailToCkbBalikpapan == true) {
            $form.find("input[name=SendEmailToCkbBalikpapan]").prop("checked", true);
        }
        if (dataRef.header.SendEmailToCkbBanjarmasin && dataRef.header.SendEmailToCkbBanjarmasin == true) {
            $form.find("input[name=SendEmailToCkbBanjarmasin]").prop("checked", true);
        }

        if (dataRef.header.PenaltyLateness) {
            $form.find("input[name=PenaltyLateness][value=true]").prop('checked', true).attr("disabled", isdisabled);
        } else {
            $form.find("input[name=PenaltyLateness][value=false]").prop('checked', true).attr("disabled", isdisabled);
        }
        if (dataRef.header.IsDemob) {
            $form.find("input[name=IsDemob][value=true]").prop('checked', true).attr("disabled", isdisabled);
        } else {
            $form.find("input[name=IsDemob][value=false]").prop('checked', true).attr("disabled", isdisabled);
        }

        //console.log(dataRef.header);
        $("#SoNo").val(dataRef.header.SoNo);
        $("#SoDate").val(formatDate(dataRef.header.SoDate));
        $("#STRNo").val(dataRef.header.STRNo);
        $("#STRDate").val(formatDate(dataRef.header.STRDate));
        $("#STONo").val(dataRef.header.STONo);
        $("#STODate").val(formatDate(dataRef.header.STODate));
        $("#DoNo").val(dataRef.header.DoNo);
        $("#OdDate").val(formatDate(dataRef.header.OdDate));
        $("#DINo").val(dataRef.header.DINo);
        $("#DIDate").val(formatDate(dataRef.header.DIDate));
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
       
        //$("#DIDateSAP").val(dataRef.header.DIDateSAP);
        $('.SDOC-container').show();
        $('.SDOC-container .row .upload').show();
        $('.SDOC-container .row.preview').show();
        $("button[name=ReRoute]").attr("disabled", true);

        if (['U', 'I'].indexOf(formType) > -1) {
            $('.SDOC-container .row .upload').show();
            $('.SDOC-container .row.preview').hide();
        } else if (formType === 'R') {
            $('.SDOC-container .row .upload').show();
            $('.SDOC-container .row.preview').show();
            showFilePreview(dataRef.header);
        } else if (formType === 'V') {
            $('.SDOC-container .row .upload').hide();
            $('.SDOC-container .row.preview').show();
            //$('.SDOC-container').hide();
            showFilePreview(dataRef.header);
        }
        $('.fileinput-remove span').hide();
        setFormSisable(isdisabled, formType);
        requestingForm.initTableUnit(dataRef.details, formType, isdisabled);
        showFormRequisition();
    }
};
function invalidRef() {
    $("#refDate").hide();
    $("#refNo").removeProp('readonly', false);
}
function viewDR(refNo,refUrl) {
     $.ajax({
        type: "GET",
        url: refUrl,
        beforeSend: function () { },
        complete: function () { },
        dataType: "json",

        success: function (d) {
            if (d && d.header.RefNo != null && d.header.RefNo !== 'null' && d.header.RefNo != '') {
                var formType = $form.find("input[name=formType]").val();
                if (formType === 'R') {
                    $("#refSONo").parent().removeClass('hidden');

                    var newOption2 = new Option(d.header.CustName || '', d.header.CustID, false, false);
                    $('#CustID').append(newOption2).trigger('change');
                    $("#CustID").val(d.header.CustID).trigger("change");
                    $("#CustName").val(formatUpperCase(d.header.CustName || ''));
                    $("#CustAddress").val(formatUpperCase(d.header.CustAddress));
                    //$("#ActualTimeArrival").val(formatUpperCase(d.header.ATA));
                    //$('#ProvinceID').append(newOption2).trigger('change');
                    //$("#ProvinceID").val(d.header.ProvinceID).trigger("change");
                    //$("#ProvinceName").val(formatUpperCase(d.header.Province || ''));
                    //$('#DistrictID').append(newOption2).trigger('change');
                    //$("#DistrictID").val(d.header.ProvinceID).trigger("change");
                    //$("#DistrictName").val(formatUpperCase(d.header.Kabupaten || ''));               
                    //$('#SubDistrictID').append(newOption2).trigger('change');
                    //$("#SubDistrictID").val(d.header.SubDistrictID).trigger("change");
                    //$("#SubdistrictName").val(formatUpperCase(d.header.Kabupaten || ''));
                    $("#PicName").val(formatUpperCase(d.header.PicName));
                    $("#PicHP").val(formatUpperCase(d.header.PicHP));
                    //$("#SubDistrictID").val(formatUpperCase(d.header.Kecamatan));
                    //$("#DistrictID").val(formatUpperCase(d.header.Kabupaten));
                    //$("#ProvinceID").val(formatUpperCase(d.header.Province));
                    $("#RefNo").val(d.header.RefNo);
                    $("#SoNo").val(d.header.SoNo);
                    $("#SoDate").val(formatDate(d.header.SoDate));

                    $("button[name=ReRoute]").attr("disabled", false);
                    $("#CustID").attr("disabled", false);
                    $("#CustName").attr("disabled", false);
                    $("#CustAddress").attr("disabled", false);
                    $("#PicName").attr("disabled", false);
                    $("#PicHP").attr("disabled", false);
                    $("#SubDistrictID").attr("disabled", false);
                    $("#DistrictID").attr("disabled", false);
                    $("#ProvinceID").attr("disabled", false);
                    $("#ActualTimeArrival").attr("disabled", false);
                    $("#ActualTimeDeparture").attr("disabled", false);
                    $('.btn-refNo button').attr("disabled", false);
                    $("#refNo").attr("disabled", false);
                } else {
                    if ($("#refSONo").parent().hasClass('active')) {
                        d.header.ExpectedTimeArrival = "";
                        d.header.ExpectedTimeLoading = "";
                        referenceEvent.fillFromSo(d, $form.find("input[name=formType]").val());
                    } else if ($("#refSTRNo").parent().hasClass('active')) {
                        d.header.ExpectedTimeArrival = "";
                        d.header.ExpectedTimeLoading = "";
                        referenceEvent.fillFromStr(d, $form.find("input[name=formType]").val());
                    } else if ($("#refPONo").parent().hasClass('active')) {
                        d.header.ExpectedTimeArrival = "";
                        d.header.ExpectedTimeLoading = "";
                        referenceEvent.fillFromStr(d, $form.find("input[name=formType]").val());
                    } else if ($("#refDINo").parent().hasClass('active')) {
                        d.header.ExpectedTimeArrival = "";
                        referenceEvent.fillFromDI(d, $form.find("input[name=formType]").val());
                    }
                    $("button[name=Cancel]").show();
                    $("button[name=SaveAsDraft]").show();
                    $("button[name=SubmitForm]").show();
                    $("button[name=SaveAsRevised]").hide();
                    $("button[name=ReRoute]").hide();
                }
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
function referenceClick() {
    var refNo = $("#refNo").val();
    var refUrl;
    $("#ProvinceID").select2("val", "");
    $("#DistrictID").select2("val", "");
    $("#SubDistrictID").select2("val", "");
    $("#refDate").html('');
    $("#refDate").show();
    $("#refNo").prop('readonly', true);   
   
    if (!refNo || refNo === '') {
        invalidRef();
        sAlert('Error', "Please fill NO!", "error");
        return;
    }

    if ($form.find("input[name=formType]").val() === "U") {
        if (referenceEvent.dataRef && referenceEvent.dataRef.header && referenceEvent.dataRef.header.RefNo
            && referenceEvent.dataRef.header.RefNo === $("#refNo").val()) {
            invalidRef();
            sAlert('Warning', "Please change your reference number!", "warning");
            return;
        }
    }    
 
    if ($("#refSONo").parent().hasClass('active')) {
      
        refUrl = myApp.root + 'DTS/GetDRReferenceNo?keyType=' + "SO" + "&number=" + refNo;
    }
    else if ($("#refSTRNo").parent().hasClass('active')) {      
        
        refUrl = myApp.root + 'DTS/GetDRReferenceNo?keyType=' + "STR" + "&number=" + refNo;
    }
    else if ($("#refPONo").parent().hasClass('active')) {      
        
        isExist = refUrl = myApp.root + 'DTS/GetDRReferenceNo?keyType=' + "PO" + "&number=" + refNo;
    }
    else if ($("#refDINo").parent().hasClass('active')) {        
        refUrl = myApp.root + 'DTS/GetDRReferenceNo?keyType=' + "DI" + "&number=" + refNo;
    }
    else {
        invalidRef();
        return;
    }
    var CheckUrl;
    CheckUrl = myApp.root + 'DTS/GetDRExist?refNo=' + refNo;
    $.ajax({
        type: "GET",
        url: CheckUrl,
        beforeSend: function () { },
        complete: function () { },
        dataType: "json",
        success: function (d) {
            if (d.header != null) {
                sAlert('Warning', "SO/RO/STR had been assigned on " + d.header.KeyCustom + "", "warning");
                return;
            }
            else {
                viewDR(refNo,refUrl);
            }            
        }
    })
}
function resetForm() {
    $form[0].reset();
    resetReference();
    resetFormRequisition();
    
    $("[name=refresh]").trigger('click');
}
function getCheckboxValues(name) {
    var dataCheckbox = $("input[name=" + name + "]:checked");
    var dataArray = [];
    var dataString = "";

    if (dataCheckbox.length > 0) {
        $.each(dataCheckbox, function (index, item) {
            if ($(item).val() === 'OTHERS') {
                dataArray.push($(item).val() + '-' + $('#INCTOthers').val());
            } else {
                dataArray.push($(item).val());
            }
            
        });
        dataString = dataArray.join(",");
    }
    return dataString;
}
function submitForm(ActType) {
    var dataForm = $form.serializeArray().reduce(function (obj, item) {
        obj[item.name] = item.value;
        return obj;
    }, {});
    if (dataForm['SupportingOfDelivery'] === 'OTHERS') {
        if ($("#SODOthers").val() === '') {
            sAlert('Error', "Please fill Supporting Of Delivery - OTHER", "error");
            return;
        }
    }
    dataForm.Incoterm = getCheckboxValues("Incoterm");
    if (dataForm.Incoterm == 'OTHERS-') {
        if($("#INCTOthers").val() ==='')
        {
            sAlert('Error', "Please fill Transportation Service Arrangement - OTHER", "error");
            return;
        }
    }
   
    if (dataForm['Transportation'] === 'OTHERS') {
        if ($("#TransportationOthers").val() === '') {
            sAlert('Error', "Please fill Transportation Obligation- OTHER", "error");
            return;
        }
    }
    dataForm['RefNoType']="";
    if ($("#refSONo").parent().hasClass('active')) {
        dataForm['RefNoType'] = "SO";
    }
    else if ($("#refSTRNo").parent().hasClass('active')) {
        dataForm['RefNoType'] = "STR";
    }
    else if ($("#refPONo").parent().hasClass('active')) {
        dataForm['RefNoType'] = "PO";
    }
    else if ($("#refDINo").parent().hasClass('active')) {
        dataForm['RefNoType'] = "DI";
    }

    if (!$("#refNo").val() || $("#refNo").val() === '') {
        invalidRef();
        sAlert('Error', "Please fill " + dataForm['RefNoType'] + " Number", "error");
        return;
    }
    if ($("#PicHP").val().length > 16) {
        alert("Maximum of 16 characters for HP PIC");
        return;
    }

    dataForm['status'] = htmlEncode(ActType);
    
    dataForm['Province'] = htmlEncode($("#ProvinceID").text().trim().replace('-', ''));
    dataForm['Kabupaten'] = htmlEncode($("#DistrictID").text().trim().replace('-', ''));
    dataForm['Kecamatan'] = htmlEncode($("#SubDistrictID").text().trim().replace('-', ''));
    dataForm['ProvinceName'] = htmlEncode($("#ProvinceID").text().replace('-', ''));
    dataForm['DistrictName'] = htmlEncode($("#DistrictID").text().replace('-', ''));
    dataForm['SubDistrictName'] = htmlEncode($("#SubDistrictID").text().replace('-',''));
    dataForm['UnitDimWeight'] = htmlEncode($('#u-weight').val());
    dataForm['UnitDimWidth'] = htmlEncode($('#u-width').val());
    dataForm['UnitDimLength'] = htmlEncode($('#u-length').val());
    dataForm['UnitDimHeight'] = htmlEncode($('#u-height').val());
    dataForm['UnitDimVol'] = htmlEncode($('#u-vol').val());

    var detailUnits = $('#tableDeliveryRequisitionUnit').bootstrapTable('getSelections');
    if (!detailUnits || detailUnits.length <= 0) {
        sAlert('Warning', 'Unit items should not be empty', 'warning');
        return;
    }
    if (dataForm['TermOfDelivery'] === 'OTHERS') {
        dataForm['TermOfDelivery'] += '-' + htmlEncode($('#TODOthers').val());
    }
    if (dataForm['SupportingOfDelivery'] === 'OTHERS') {
        dataForm['SupportingOfDelivery'] += '-' + htmlEncode($('#SODOthers').val());
    }
    if (dataForm['SupportingOfDelivery'] === 'CRANE/FORKLIFT') {
        dataForm['SupportingOfDelivery'] += '-' + htmlEncode($('#SODCrane_Forklift').val());
    }
    if (dataForm['Transportation'] === 'OTHERS') {
        dataForm['Transportation'] += '-' + htmlEncode($('#TransportationOthers').val());
    }
    var formData = new FormData();
    for (var name in dataForm) {
        var val = dataForm[name]; // == "null" ? null : dataForm[name];
        if (val !== null && val !== "null") {
            formData.append(name, val);
        }
    }   
    
    for (var x in detailUnits) {
        detailUnits[x].CustID = htmlEncode(dataForm.CustID);
        detailUnits[x].CustName = htmlEncode(dataForm.CustName);
        detailUnits[x].CustAddress = htmlEncode(dataForm.CustAddress);
        detailUnits[x].Kecamatan = htmlEncode(dataForm.SubDistrictName.trim());
        detailUnits[x].Kabupaten = htmlEncode(dataForm.DistrictName.trim().replace('-', '').replace('-', ''));
        detailUnits[x].Province = htmlEncode(dataForm.ProvinceName.trim().replace('-', '').replace('-', ''));
    }

    formData.append('detailUnits', JSON.stringify(detailUnits));
    formData.append("SDOC", $('#SDOC')[0].files[0]);
    formData.append("SDOC1", $('#SDOC1')[0].files[0]);
    formData.append("SDOC2", $('#SDOC2')[0].files[0]);

    $.ajax({
        type: "POST",
        url: myApp.fullPath  + 'DTS/DeliveryRequisitionProccessForm',
        beforeSend: function () {
            ShowLoading();
            $("button[name=SubmitForm]").attr("disabled", "disabled");
            $("button[name=ReRoute]").attr("disabled", "disabled");
            $("button[name=Cancel]").attr("disabled", "disabled");
            $("button[name=SaveAsRevised]").attr("disabled", "disabled");
            $("button[name=SaveAsDraft]").attr("disabled", "disabled");
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
            if (d.Status === 0) {
                var message = '';
                var DRNo = dataForm['KeyCustom'];
                if (d.result !== null) {
                    DRNo = d.result.KeyCustom;
                }
                if (ActType === 'draft') {
                    message = 'Save as Draft NO ' + DRNo + ' Success';
                } else if (ActType === 'submit') {
                    message = 'Submit NO ' + DRNo + ' Success';
                } else if (ActType === 'revised') {
                    message = 'Save as Revised NO ' + DRNo + ' Success';
                }
                sAlert('Success', message, 'success');
                resetForm();
                hideModal();
            } else {
                $("button[name=SubmitForm]").removeAttr("disabled");
                $("button[name=Cancel]").removeAttr("disabled");
                $("button[name=SaveAsRevised]").removeAttr("disabled");
                $("button[name=SaveAsDraft]").removeAttr("disabled");
                $("button[name=ReRoute]").attr("disabled", false);
                sAlert('Error', d.Msg, 'error');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            HideLoading();
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
            $("button[name=SubmitForm]").removeAttr("disabled");
            $("button[name=Cancel]").removeAttr("disabled");
            $("button[name=SaveAsRevised]").removeAttr("disabled");
            $("button[name=SaveAsDraft]").removeAttr("disabled");
        }
    });
    
}

//function htmlEncode(str) {
//    return String(str).replace(/[^\w. ]/gi, function (c) {
//        return '&#' + c.charCodeAt(0) + ';';
//    });
//}
function htmlEncode(str) {
    return String(str).replace(/[^a-zA-Z 0-9'.<>]+/g, '').replace(/^\s+|\s+$/g, '')   
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
    
    $("#ProvinceID").select2("val", "");
    $("#DistrictID").select2("val", "");
    $("#SubDistrictID").select2("val", "");
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
function clearClick() {
    //console.log(referenceEvent.formType);
    if (referenceEvent.formType === "I") {
        $("button[name=Cancel]").hide();
        $("button[name=SaveAsDraft]").hide();
        $("button[name=SubmitForm]").hide();
        $("button[name=SaveAsRevised]").hide();
        $("button[name=ReRoute]").hide();
        resetFormRequisition();
    } else {
        $("#refDate").html('');
        $("#refDate").hide();
        $("#refNo").val('');
        $("#refNo").removeProp('readonly', false);
    }
}
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
    formData.append("ProvinceID", $('#ProvinceID').val());
    formData.append("DistrictID", $('#DistrictID').val());
    formData.append("SubDistrictID", $('#SubDistrictID').val());
    formData.append("CustName", $('#CustName').val());
    formData.append("CustAddress", $('#CustAddress').val());
    formData.append("PicName", $('#PicName').val());
    formData.append("PicHP", $('#PicHP').val());
    formData.append("Kecamatan", $('#SubDistrictID').text());
    formData.append("Kabupaten", $('#DistrictID').text());
    formData.append("Province", $('#ProvinceID').text());
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
    }
    else if ($("#refDINo").parent().hasClass('active')) {
        formData.append("RefNoType", "DI");
        formData.append("DINo", $('#DINo').val());
        formData.append("DIDate", $('#DIDate').val());
    }
    formData.append("Status", 'rerouted');
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
            //console.log(d);
            HideLoading();
            if (d.Status === 0) {
                var message = '';
                var DRNo = dataForm['KeyCustom'];
                if (d.result !== null) {
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
var requestingForm = {
    initTableUnit: function (_data, formType, isdisabled) {
        var $tableUnitForm = $('#tableDeliveryRequisitionUnit');
        $tableUnitForm.bootstrapTable('destroy');
        $tableUnitForm.bootstrapTable({
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
                            disabled: (row.Selectable === 0 || isdisabled === true),
                            checked: row.Checked === 1
                        };
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
            if (this.value === 'OTHERS') {
                $("#TODOthers").removeAttr('readonly');
            }
            else {
                $("#TODOthers").attr('readonly', true);
            }
        });

        $('input[type=radio][name=SupportingOfDelivery]').change(function () {
            $("#SODOthers").val('');
            $("#FOT").attr("disabled", false);
            $("#FOB").attr("disabled", false);
            $("#ONGROUND").attr("disabled", false);
            $("#ONTOCONTAINER").attr("disabled", false);
            $("#INCTOTHER").attr("disabled", false);
            $("#on_pondation").attr("disabled", false);
            $form.find("input[name=Incoterm][value='FOT ( ON TRUCT EXCLUDE UNLOADING )']").prop("checked", false);
            $form.find("input[name=Incoterm][value='FOB ( ON BOARD INCLUDE LOADING & UNLOADING )']").prop("checked", false);
            $form.find("input[name=Incoterm][value='ON GROUND ( INCLUDE UNLOADING )']").prop("checked", false);
            $form.find("input[name=Incoterm][value='ONTO CONTAINER ( INCLUDE LOADING & UNLOADING )']").prop("checked", false);
            $form.find("input[name=Incoterm][value='ON PONDATION ( INCLUDE LOADING & UNLOADING )']").prop("checked", false);
            $form.find("input[name=Incoterm][value='OTHERS']").prop("checked", false);

            if (this.value === 'OTHERS') {
                $("#SODOthers").removeAttr('readonly');
                $("#SODCrane_Forklift").attr('readonly', true);
            } else if (this.value === 'CRANE/FORKLIFT') {
                $("#SODOthers").attr('readonly', true);
                $("#SODCrane_Forklift").removeAttr('readonly');
            } else if (this.value === 'UNLOADING BY CUSTOMER') {
                //$("#FOT").attr("disabled", true);
                $form.find("input[name=Incoterm][value='FOT ( ON TRUCT EXCLUDE UNLOADING )']").prop("checked", true);

                $("#FOB").attr("disabled", true);
                $("#ONGROUND").attr("disabled", true);
                $("#ONTOCONTAINER").attr("disabled", true);
                $("#INCTOTHER").attr("disabled", true);
                $("#on_pondation").attr("disabled", true);
            } else if (this.value === 'UNLOADING BY VENDOR') {
                $("#FOT").attr("disabled", true);
            } else if (this.value === 'UNLOADING BY PTTU') {
                $form.find("input[name=Incoterm][value='ON GROUND ( INCLUDE UNLOADING )']").prop("checked", true);

                $("#FOT").attr("disabled", true);
                $("#FOB").attr("disabled", true);
                $("#ONTOCONTAINER").attr("disabled", true);
                $("#INCTOTHER").attr("disabled", true);
                $("#on_pondation").attr("disabled", true);
            } else {
                $("#SODOthers").attr('readonly', true);
                $("#SODCrane_Forklift").attr('readonly', true);
            }
        });

        $('input[type=checkbox][name=Incoterm]').change(function () {
            $("#INCTOthers").val('');
            if (this.value === 'OTHERS') {
                $("#INCTOthers").removeAttr('readonly');
            }
            else {
                $("#INCTOthers").attr('readonly', true);
            }
        });

        $('input[type=radio][name=Transportation]').change(function () {
            $("#TransportationOthers").val('');
            if (this.value === 'OTHERS') {
                $("#TransportationOthers").removeAttr('readonly');
            }
            else {
                $("#TransportationOthers").attr('readonly', true);
            }
        });

        $("input[type=radio][name='unit']").change(function () {
            if (this.value === "ATTACHMENT CAT" || this.value === "ATTACHMENT NON CAT") {
                $("#section-dimension").show();
                $("#on_pondation").hide();
            } else if (this.value === "ENGINE") {
                $("#on_pondation").show();
                $("#section-dimension").hide();
            } else {
                $("#section-dimension").hide();
                $("#on_pondation").hide();
            }
        });

        $("input[type=radio][name='reference']").change(function () {
            if (referenceEvent.formType === "I") {
                resetFormRequisition();
                $("button[name=Cancel]").hide();
                $("button[name=SaveAsDraft]").hide();
                $("button[name=SubmitForm]").hide();
                $("button[name=SaveAsRevised]").hide();
                $("button[name=ReRoute]").hide();
            } else if (referenceEvent.formType == "U") {
                $("#refNo").val('');
            }
        });
    }
};

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
                // Tranforms the top-level key of the response object from 'items' to 'results'
                return {
                    results: newData
                };
            }
            // Additional AJAX parameters go here; see the end of this chapter for the full code of this example
        }
    });
    $('#CustID').on('select2:select', function (e) {
        var data = e.params.data;    
        console.log(data);
        $("div[for=CustID]").hide();
        $("input[name=CustName]").val(data.Customer_Full_Name || "");
        $("textarea[name=CustAddress]").val(data.Jalan);
        $("input[name=Kabupaten]").val(data.Kota);
        $("input[name=Province]").val('');
     
    });
    $("#ProvinceID").select2({
        placeholder: 'Select Provinsi',
        minimumInputLength: 3,
        ajax: {
            url: myApp.root + 'DTS/getMasterProvince',
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
                console.log(data);
                var newData = $.map(data, function (obj) {
                    obj.id = obj.ProvinsiId;
                    obj.text = obj.ProvinsiName;
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
    $("#ProvinceID").select2({
        placeholder: 'Select Provinsi',
        minimumInputLength: 3,
        ajax: {
            url: myApp.root + 'DTS/getMasterProvince',
            async: false,
            dataType: 'json',
            data: null,
            data: function (params) {
                var query = {
                    key: params.term,
                    type: 'public'
                };
                return query;
            },
            processResults: function (data) {
                console.log(data);
                var newData = $.map(data, function (obj) {
                    obj.id = obj.ProvinsiId;
                    obj.text = obj.ProvinsiName;
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
    $('#ProvinceID').on('select2:select', function (e) {
        var element = $(this);
        console.log(element.ProvinsiName);
        $("div[for=ProvinceID]").hide();
    });

    $("#DistrictID").select2({
        placeholder: 'Select Kabupaten',
        minimumInputLength: 3,
        ajax: {
            url: myApp.root + 'DTS/getMasterDistrict',
            async: false,
            dataType: 'json',
            data: null,
            data: function (params) {             
                console.log(params);
                var query = {
                    key: params.term
                };
                    type: 'public';
                return query;
            },
            processResults: function (data) {
        
                var newData = $.map(data, function (obj) {
                    obj.id = obj.KabupatenId;
                    obj.text = obj.KabupatenName;
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
    $('#DistrictID').on('select2:select', function (e) {
        var element = $(this);
        console.log(element.name);
        $("div[for=DistrictID]").hide();
    });

    $("#SubDistrictID").select2({
        
        placeholder: 'Select Kecamatan',
        minimumInputLength: 3,
        ajax: {
            url: myApp.root + 'DTS/getMasterSubDistrict',
            async: false,
            dataType: 'json',
            data :null,
            data: function (params) {
                var query = {
                    key: params.term,
                    type: 'public'
                };
                return query;
            },
            processResults: function (data) {
                console.log(data);
                var newData = $.map(data, function (obj) {
                    obj.id = obj.KecamatanId;
                    obj.text = obj.KecamatanName;
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
    $('#SubDistrictID').on('select2:select', function (e) {
        var element = $(this);
        console.log(element.name);
        $("div[for=SubDistrictID]").hide();
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
                var newData = $.map(data, function (obj) {
                    obj.id = obj.UserID;
                    obj.text = obj.FullName;
                    return obj;
                });
                return {
                    results: newData
                };
            }
        }
    });
    $('#Sales1ID').on('select2:select', function (e) {
        var data = e.params.data;
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
                var newData = $.map(data, function (obj) {
                    obj.id = obj.UserID;
                    obj.text = obj.FullName;
                    return obj;
                });
                return {
                    results: newData
                };
            }
        }
    });
    $('#Sales2ID').on('select2:select', function (e) {
        var data = e.params.data;
        $("div[for=Sales2ID]").hide();
        $("input[name=Sales2Name]").val(data.FullName);
        $("input[name=Sales2Hp]").val(data.Phone);
    });
    $("button[name=SubmitForm]").click(function () {
        var isValid = $form.valid();
        if (isValid) {
            submitForm("submit");
        } else {
            $.each(validator.errorMap, function (index, value) {
                console.log('Id: ' + index + ' Message: ' + value);
            });
        }
    });
    $("button[name=Cancel]").click(function () {
        $form[0].reset();
        $("#CustID").select2("val", "");
       
        $("#Sales1ID").select2("val", "");
        $("#Sales2ID").select2("val", "");
        hideModal();
    });
    $("button[name=SaveAsDraft]").click(function () {
        var isValid = $form.valid();
        if (isValid) {
            submitForm("draft");
        } else {
            $.each(validator.errorMap, function (index, value) {
                console.log('Id: ' + index + ' Message: ' + value);
            });
        }
    });
    $("button[name=SaveAsRevised]").click(function () {
        var isValid = $form.valid();
        if (isValid) {
            submitForm("revised");
        } else {
            $.each(validator.errorMap, function (index, value) {
                console.log('Id: ' + index + ' Message: ' + value);
            });
        }
    });
    $("button[name=ReRoute]").click(function () {
        var isValid = $form.valid();
        if (isValid) {
            sendReRoute();
        } else {
            $.each(validator.errorMap, function (index, value) {
                console.log('Id: ' + index + ' Message: ' + value);
            });
        }
    });
    $("input[name=PenaltyLateness]").click(function () {
        $('input[name=PenaltyLateness]').prop('checked', false);
        $(this).prop('checked', true);
    });
    $("input[name=IsDemob]").click(function () {
        $('input[name=IsDemob]').prop('checked', false);
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

