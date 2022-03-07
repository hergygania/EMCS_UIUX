$form = $("#formRequest");
$tableUnit = $('#tableDeliveryInstructionUnit'); 

$form.validate({
    ignore: ":hidden",
    highlight: function (element) {
        $form.find("div[for=" + element.name + "]").show();
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass(errorClass).addClass(validClass);
        if (element.name && element.name !== "") {
            if (element.name === 'SDOC[]') {
                $form.find("div[for=SDOC]").hide();
            } else {
                $form.find("div[for=" + element.name + "]").hide();
            }
        }
    }
});
// ReSharper disable once UnusedParameter
function formatCurrency(amount, decimalSeparator, nDecimalDigits) {
    decimalSeparator =  '.';
    var thousandsSeparator = ',';
    nDecimalDigits = typeof nDecimalDigits !== 'undefined' ? nDecimalDigits : 0;

// ReSharper disable ConditionIsAlwaysConst
// ReSharper disable once HeuristicallyUnreachableCode
    amount = typeof amount !== 'undefined' || amount !== 'null' ? amount : 0;
// ReSharper restore ConditionIsAlwaysConst

    var num = parseFloat(amount);
    if (isNaN(num)) {
        return 0;
    }

    var fixed = num.toFixed(nDecimalDigits);
    var parts = new RegExp('^(-?\\d{1,3})((?:\\d{3})+)(\\.(\\d{' + nDecimalDigits + '}))?$').exec(fixed);
    if (parts) {
        console.log(parts[1] + ' - ' + parts[2].replace(/\d{3}/g, thousandsSeparator + '$&') + ' - ' + (parts[4] ? decimalSeparator + parts[4] : ''));
        return parts[1] + parts[2].replace(/\d{3}/g, thousandsSeparator + '$&');
    } else {
        return fixed.replace('.', decimalSeparator);
    }

}

function showFilePreview(header) {
    var filesPreview = [];
    var filesPreviewConfig = [];

    if (header.SupportingDocument1 && header.SupportingDocument1 !== "") {
        filesPreview.push(myApp.root + header.SupportingDocument1);
    }
    if (header.SupportingDocument2 && header.SupportingDocument2 !== "") {
        filesPreview.push(myApp.root + header.SupportingDocument2);
    }
    if (header.SupportingDocument3 && header.SupportingDocument3 !== "") {
        filesPreview.push(myApp.root + header.SupportingDocument3);
    }
    if (header.SupportingDocument4 && header.SupportingDocument4 !== "") {
        filesPreview.push(myApp.root + header.SupportingDocument4);
    }
    if (header.SupportingDocument5 && header.SupportingDocument5 !== "") {
        filesPreview.push(myApp.root + header.SupportingDocument5);
    }
  
  
    $("#SDOCPreview").fileinput('destroy');
    if (filesPreview.length > 0) {
        // ReSharper disable once MissingHasOwnPropertyInForeach
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
            initialPreviewDownloadUrl: myApp.root + 'Upload/DTS/DeliveryInstructionList/' + header.ID + '/{filename}',
            initialPreviewConfig: filesPreviewConfig
        });
    } else {
        $('.SDOC-container .row.preview').hide();
    }
}

var DIForm = {
    ID: 0,
    data: null,
    mode: "I", // I = Insert, U = Edit, V = View, A = Approval
    title: "CREATE DI",
    show: function () {
        var data = this.data || {};

        $("#formRequest")[0].reset();
        $form.find("input[name=formType]").val(this.mode);
        $(".error").hide();

        $("#ID").val(data.ID);
        if (data.RequestorHp && data.RequestorHp !== '') {
            $("#RequestorHp").val(data.RequestorHp);
        }
        var sales1Op = new Option(data.Sales1Name || "", data.Sales1ID || "", false, false);
        $('#Sales1ID').append(sales1Op).trigger('change');
        $("#Sales1ID").val(data.Sales1ID).trigger("change");
        $("#Sales1Name").val(data.Sales1Name);

        var sales2Op = new Option(data.Sales2Name || "", data.Sales2ID || "", false, false);
        $('#Sales2ID').append(sales2Op).trigger('change');
        $("#Sales2ID").val(data.Sales2ID).trigger("change");
        $("#Sales2Name").val(data.Sales2Name);

        var newOption2 = new Option(data.CustName || "", data.CustID || "", false, false);
        $('#CustID').append(newOption2).trigger('change');
        $("#CustID").val(data.CustID).trigger("change");
        $("#CustName").val(data.CustName);
        $("select[id=CustID]").attr("disabled", false);
        $("#Sales1Hp").val(data.Sales1Hp);
        $("#Sales2Hp").val(data.Sales2Hp);
        $("#CustAddress").val(data.CustAddress);
        $("#PicName").val(data.PicName);
        $("#PicHP").val(data.PicHP);
        $("#Kecamatan").val(data.Kecamatan);
        $("#Kabupaten").val(data.Kabupaten);
        $("#Province").val(data.Province);
        $("#Origin").val(data.Origin);
        $("#Remarks").val(data.Remarks);
        $("#ChargeofAccount").val(data.ChargeofAccount);
        $("#ApprovalNote").val(data.ApprovalNote);
        $("#ExpectedDeliveryDate").val(formatDate(data.ExpectedDeliveryDate));
        $("#PickUpPlanDate").val(formatDate(data.PickUpPlanDate));
        $("#PromisedDeliveryDate").val(formatDate(data.PromisedDeliveryDate));
        $("#ModaTransport").val(data.ModaTransport);
        $("#Incoterm").val(data.Incoterm);
        $("#SupportingOfDelivery").val(data.SupportingOfDelivery);
        $("#vendorname").val(data.VendorName);
        $("#ForwarderName").val(data.ForwarderName);
        $("#section-approval-note").hide();
        $("#chkvendor").prop("checked", false);
        $("button[name=Cancel]").show();
        $("button[name=SubmitForm]").show();
        $("button[name=SaveAsDraft]").show();
        $("button[name=SaveAsRevised]").hide();
        $("#btnAddUnit").hide();

        $("button[name=Reject]").show();
        $("button[name=Revise]").show();
        $("button[name=Approve]").show();

        $('.SDOC-container').show();
        $('.SDOC-container .row .upload').show();
        $('.SDOC-container .row.preview').show();
        var modaTransport;
        if (this.mode === "I" || this.mode === "U") {
            $("#section-approval-note").show();
            $("#formRequest").find('input').attr('disabled', false);
            $("#formRequest").find('textarea').attr('disabled', false);
            $("#ExpectedDeliveryDate").attr("disabled", true);
            $("#PromisedDeliveryDate").attr("disabled", true);
            $("#vendorname").attr("disabled", true);
            if (this.mode === "U") {
              
                if (this.data["VendorName"] === '-' ) {
                    $("select[name=CustID]").attr("disabled", true);
                    $("#chkvendor").prop("checked", true);
                    $("#vendorname").attr("disabled", false);
                } else {
                    $("select[name=CustID]").attr("disabled", false);
                    $("#chkvendor").prop("checked", false);
                    $("#vendorname").attr("disabled", true);
                }
                if (this.data["ModaTransport"] && this.data !== 'null') {
                    modaTransport = this.data["ModaTransport"].split(',');
                    $.each(modaTransport, function (index, itemInct) {
                        $form.find("input[name=ModaTransport][value='" + itemInct + "']").prop("checked", true);
                    });
                }
                if (this.data["Incoterm"] && this.data !== 'null') {
                    modaTransport = this.data["Incoterm"].split(',');
                    $.each(modaTransport, function (index, itemInct) {
                        $form.find("input[name=Incoterm][value='" + itemInct + "']").prop("checked", true);
                    });
                }
                if (this.data["SupportingOfDelivery"] && this.data !== 'null') {
                    modaTransport = this.data["SupportingOfDelivery"].split(',');
                    $.each(modaTransport, function (index, itemInct) {
                        $form.find("input[name=SupportingOfDelivery][value='" + itemInct + "']").prop("checked", true);
                    });
                }
            }          
         
            $("select[name=Sales1ID]").attr("disabled", false);
            $("select[name=Sales2ID]").attr("disabled", false);
          
            if (this.mode === "U" && data.Status === "revise") {             
                $("button[name=SubmitForm]").hide();
                $("button[name=SaveAsDraft]").hide();
                $("button[name=SaveAsRevised]").show();
            }
          
            if (this.mode === "I") {
                $("#btnAddUnit").show();
                $("#ForwarderName").attr("disabled", true);
              
            }
            $('.SDOC-container .row .upload').show();
            $('.SDOC-container .row.preview').hide();
        } else {          
            $("#formRequest").find('input').attr('disabled', true);
            $("#formRequest").find('textarea').attr('disabled', true);
            $("select[name=CustID]").attr("disabled", true);
            $("select[name=Sales1ID]").attr("disabled", true);
            $("select[name=Sales2ID]").attr("disabled", true);
            $("#section-approval-note").show();
            $("#ApprovalNote").attr("disabled", false);
            $("#ForwarderName").attr("disabled", false);
            $("button[name=Cancel]").show();
            $("button[name=SubmitForm]").hide();
            $("button[name=SaveAsDraft]").hide();
            $("button[name=SaveAsRevised]").hide();          

            if (this.mode !== "A") {
                $("button[name=Cancel]").hide();
                $("button[name=Reject]").hide();
                $("button[name=Revise]").hide();
                $("button[name=Approve]").hide();
                $("#ApprovalNote").attr("disabled", true);
                $("#ForwarderName").attr("disabled", true);
                $("#btnAddUnit").hide();
            }
            if (data.ModaTransport && this.header !== 'null') {
                modaTransport = data.ModaTransport.split(',');
                $.each(modaTransport, function (index, itemInct) {
                    $form.find("input[name=ModaTransport][value='" + itemInct + "']").prop("checked", true);                  
                });
            }
            if (this.data["Incoterm"] && this.data !== 'null') {
                modaTransport = this.data["Incoterm"].split(',');
                $.each(modaTransport, function (index, itemInct) {
                    $form.find("input[name=Incoterm][value='" + itemInct + "']").prop("checked", true);
                });
            }
            if (this.data["SupportingOfDelivery"] && this.data !== 'null') {
                modaTransport = this.data["SupportingOfDelivery"].split(',');
                $.each(modaTransport, function (index, itemInct) {
                    $form.find("input[name=SupportingOfDelivery][value='" + itemInct + "']").prop("checked", true);
                });
            }
            if (this.mode === "V") {
                $("#ExpectedDeliveryDate").attr("disabled", true);
                $("#PromisedDeliveryDate").attr("disabled", true);
                if (this.data["VendorName"] !== null) {
                    if (this.data["VendorName"] !== "-") {
                        $("select[name=CustID]").attr("disabled", true);
                        $("#chkvendor").prop("checked", true);
                        $("#vendorname").attr("disabled", true);
                    }                   
                }
            }
            if (this.mode === "A") {        
                $("button[name=Reject]").hide();
                $("button[name=Revise]").hide();                
                $form.find("input[name=ModaTransport]").attr("disabled", false);
                $("#Remarks").attr("disabled", false);
                $("#ChargeofAccount").attr("disabled", false);
                $("#ExpectedDeliveryDate").attr("disabled", false);
                $("#PromisedDeliveryDate").attr("disabled", false);
                $("#PickUpPlanDate").attr("disabled", false);
                if (this.data["VendorName"] !== null) {
             
                    if (this.data["VendorName"] !== "-") {
                        $("select[name=CustID]").attr("disabled", true);
                        $("#chkvendor").prop("checked", true);
                        $("#vendorname").attr("disabled", true);
                    }               
                } else {
                    $("select[name=CustID]").attr("disabled", true);
                    $("#chkvendor").prop("checked", false);
                    $("#vendorname").attr("disabled", true);
                }
            }
            if (this.mode !== "V" && this.mode !== "A" ) {                
                $form.find("input[name=ModaTransport]").attr("disabled", false);
            }
            $('.SDOC-container .row .upload').hide();
            $('.SDOC-container .row.preview').show();
            showFilePreview(data);
        }
        $("#myModalRequest .modal-title").html("Delivery Instruction [" + this.title + "]");
        $("#myModalRequest").modal("show");
    },
    hide: function () {
        $("#formRequest")[0].reset();
        $form.find("input[name=formType]").val("I");
        $(".error").hide();

        $("#formRequest").find('input').attr('disabled', false);
        $("#formRequest").find('textarea').attr('disabled', false);
        $("select[name=CustID]").attr("disabled", false);
        $("select[name=Sales1ID]").attr("disabled", false);
        $("select[name=Sales2ID]").attr("disabled", false);

        $("button[name=Cancel]").show();
        $("button[name=SaveAsDraft]").show();
        $("button[name=SaveAsRevised]").hide();

        $("#myModalRequest").modal("hide");
    },
    resetForm: function () {
        $form[0].reset();
        DIForm.KeyCustom = "";
        DIForm.refreshTableUnit();
    },
    submitForm: function (actType) {
        var self = this;

        var dataForm = $form.serializeArray().reduce(function (obj, item) {
            obj[item.name] = item.value;
            return obj;
        }, {});
// ReSharper disable once AssignmentInConditionExpression
        if (dataForm['vendorname'] === "undefined" && dataForm['CustName'] === "") {
            sAlert('Error', "Please Fill Customer Name", "error");
            return;
        } 
        
        dataForm['Status'] = actType;
        if (this.mode === "I" ||this.mode ==="U") {
            var detailUnits = $('#tableDeliveryInstructionUnit').bootstrapTable('getData');
            console.log(detailUnits);
            if (detailUnits.length <= 0) {
                sAlert('Error', "Please input data unit.", "error");
                return;
            } else {
                var errMsg = "";
                var errMsgVal = "";

                detailUnits.map(function (data) {
                    if (data.Model === "") {
                        errMsg = "Model";
                    }
                    if (data.SerialNumber === "") {
                        errMsg += ", Serial No.";
                    }
                    if (data.Batch === "") {
                        errMsg += ", Batch";
                    }
                    //if (data.FreightCost === "") {
                    //    errMsg += ", FreightCost";
                    //}                    
                });

                if (errMsg !== "") {
                    sAlert('Error', errMsg + " is required", "error");
                    return;
                }

                detailUnits.forEach(dataunit => {
                    //String model = dataunit.Model.toString();
                    //String SerialNumber = dataunit.SerialNumber.toString();
                    //String Batch = dataunit.Batch.toString();
                     
                    if (dataunit.Model.toString().match('<') || dataunit.Model.toString().match('>')) {
                        errMsgVal += ", Model Unit invalid.";
                    }
                    if (dataunit.SerialNumber.toString().match('<') || dataunit.SerialNumber.toString().match('>')) {
                        errMsgVal += ", Serial No Unit invalid.";
                    }
                    if (dataunit.Batch.toString().match('<') || dataunit.Batch.toString().match('>')) {
                        errMsgVal += ", Batch Unit invalid.";
                    }
                });

                //for (var key in detailUnits) {
                //    //var model = key.Model;
                //    //if (model.match('<>')) {
                //    //    errMsgVal += ", Model invalid.";
                //    //}
                //    sAlert('Error', detailUnits[key.Model], "error");
                    
                //    return;
                //}

                if (errMsgVal !== "") {
                    sAlert('Error', errMsgVal, "error");
                    return;
                }
            }}
        

        var formData = new FormData();
        // ReSharper disable once MissingHasOwnPropertyInForeach
        for (var name in dataForm) {
            var val = dataForm[name]; // == "null" ? null : dataForm[name];
            if (val !== null && val !== "null") {
                if (val === false || val === "false") {
                    val = '-';
                }
                htmlEncode(val);
                formData.append(name, val);
            }
        }

        formData.append('detailUnits', JSON.stringify(detailUnits));
        formData.append("SDOC", $('#SDOC')[0].files[0]);
        formData.append("SDOC1", $('#SDOC1')[0].files[0]);
        formData.append("SDOC2", $('#SDOC2')[0].files[0]);
        

        $.ajax({
            type: "POST",
            url: myApp.root + 'DTS/DeliveryInstructionProccessUpload',
            data: formData,
                    
            dataType: "json",
            contentType: false,
            processData: false,

            beforeSend: function () {
                ShowLoading();
                $("button[name=SubmitForm]").prop("disabled", true);
                $("button[name=Cancel]").prop("disabled", true);
                $("button[name=SaveAsRevised]").prop("disabled", true);
                $("button[name=SaveAsDraft]").prop("disabled", true);
            },
            complete: function () {
                HideLoading();
                $("button[name=SubmitForm]").prop("disabled", false);
                $("button[name=Cancel]").prop("disabled", false);
                $("button[name=SaveAsRevised]").prop("disabled", false);
                $("button[name=SaveAsDraft]").prop("disabled", false);
            },
            success: function (d) {
                if (d.Status === 0) {
                    sAlert('Success', 'Modify data Success!', 'success');
                    $("[name=refresh]").trigger('click');
                    self.resetForm();
                    self.hide();
                } else {
                    sAlert('Error', d.Msg, "error");
                }
            },
            error: function (jqXhr) {
                sAlert('Error', jqXhr.status + " " + jqXhr.statusText, "error");
            }
        });

    },
    submitFormApproval: function (actType) {
        var self = this;
        var inputValue = $("#ApprovalNote").val();
        var forwarderName = $("#ForwarderName").val();
        var ChargeofAccount = $("#ChargeofAccount").val();
        var ExpectedDeliveryDate = $("#ExpectedDeliveryDate").val();
        var PromisedDeliveryDate = $("#PromisedDeliveryDate").val();
        
        var modaTransport = $form.find("input[name=ModaTransport]:checked").val();
        var Incoterm = $form.find("input[name=Incoterm]:checked").val();
        var SupportingOfDelivery = $form.find("input[name=SupportingOfDelivery]:checked").val();
        var remarks = $("#Remarks").val();
        var $tableUnit = $('#tableDeliveryInstructionUnit');

        
        var detailunit = $tableUnit.bootstrapTable('getData');
        if (actType === "reject" || actType === "revise") {
            swal({
                title: actType === "revise" ? "Please Input Revise Note" : "Please Input Reject Note",
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

                self.sendResponseApproval(actType, remarks, inputValue, forwarderName, ChargeofAccount, modaTransport, Incoterm, SupportingOfDelivery, ExpectedDeliveryDate, PromisedDeliveryDate, detailunit);
                // ReSharper disable once NotAllPathsReturnValue
            });
        } else if (actType === "approve", modaTransport, Incoterm, SupportingOfDelivery, detailunit) {
        
            self.sendResponseApproval(actType, remarks, inputValue, forwarderName, ChargeofAccount, modaTransport,Incoterm, SupportingOfDelivery,ExpectedDeliveryDate, PromisedDeliveryDate, detailunit);
        }
    },
    sendResponseApproval: function (actType, remarks, approvalNote, forwarderName, ChargeofAccount, modaTransport, Incoterm, SupportingOfDelivery, ExpectedDeliveryDate, PromisedDeliveryDate, detailunit) {
        var self = this;

        $.ajax({
            type: "POST",
            url: myApp.root + 'DTS/DeliveryInstructionProccessApproval',
            data: {
                ID: self.ID, actType: actType, Remarks: remarks, ApprovalNote: approvalNote, ForwarderName: forwarderName, ChargeofAccount: ChargeofAccount, ModaTransport: modaTransport,
                Incoterm: Incoterm, SupportingOfDelivery: SupportingOfDelivery, ExpectedDeliveryDate: ExpectedDeliveryDate, PromisedDeliveryDate: PromisedDeliveryDate,  detailUnits: detailunit
            },
            dataType: "json",
            beforeSend: function () {
                $("button[name=Reject]").prop("disabled", true);
                $("button[name=Cancel]").prop("disabled", true);
                $("button[name=Revise]").prop("disabled", true);
                $("button[name=Approve]").prop("disabled", true);
            },
            success: function (d) {
                if (d.Status === 0) {
                    sAlert('Success', 'Modify data Success!', 'success');
                    $("[name=refresh]").trigger('click');
                    self.resetForm();
                    self.hide();
                } else {
                    sAlert('Error', d.Msg, "error");
                }
            },
            error: function (jqXhr) {
                sAlert('Error', jqXhr.status + " " + jqXhr.statusText, "error");
            }
        }).then(function () {
            $("button[name=Reject]").prop("disabled", false);
            $("button[name=Cancel]").prop("disabled", false);
            $("button[name=Revise]").prop("disabled", false);
            $("button[name=Approve]").prop("disabled", false);
        });
    },
    initTableUnit: function () {
        $tableUnit.bootstrapTable({
            cache: false,
            url: "/DTS/GetDeliveryInstructionUnitList",
            pagination: false,
            search: false,
            striped: true,
            clickToSelect: true,
            showColumns: false,
            showRefresh: false,
            smartDisplay: false,
            pageSize: '10',
            uniqueId: 'uid',
            editable: true,
            queryParams: function () {
                return {
                    HeaderID: DIForm.ID
                };
            },
            formatNoMatches: function () {
                return '<span class="noMatches">Set</span>';
            },
            onAll: function () {
                if (DIForm.mode === "I" ) {
                    $('[data-name="Model"]').editable('enable');
                    $('[data-name="SerialNumber"]').editable('enable');
                    $('[data-name="Batch"]').editable('enable');
                    $('[data-name="FreightCost"]').editable('disable');
                    
                }
                else if (DIForm.mode === "U") {
                    $('[data-name="Model"]').editable('enable');
                    $('[data-name="SerialNumber"]').editable('enable');
                    $('[data-name="Batch"]').editable('enable');
                    $('[data-name="FreightCost"]').editable('disable');
                }
                else if (DIForm.mode === "A") {
                    $('[data-name="Model"]').editable('disable');
                    $('[data-name="SerialNumber"]').editable('disable');
                    $('[data-name="Batch"]').editable('disable');
                    $('[data-name="FreightCost"]').editable('enable');
             
                }
                else {
                    $('[data-name="Model"]').editable('disable');
                    $('[data-name="SerialNumber"]').editable('disable');
                    $('[data-name="Batch"]').editable('disable');
                    $('[data-name="FreightCost"]').editable('disable');
            
                }
            },
            columns: [
                {
                    field: 'ID',
                    title: 'ACTION',
                    halign: 'center',
                    align: 'center',
                    width: '10%',
                    formatter: function () {
                        if (DIForm.mode === "I" || DIForm.mode === "U") {
                            var htm = [];
                            htm.push('<button class="removeUnit btn btn-danger btn-xs"><i class="fa fa-trash"></i></button> ');
                            return htm.join('');
                        }
                        return '';
                    },
                    events: {
                        // ReSharper disable once UnusedParameter
                        'click .removeUnit': function (e, value, row, index) {
                            e.preventDefault();
                            if (row.ID) {
                                $tableUnit.bootstrapTable('remove', { field: 'ID', values: [row.ID] });
                            } else {
                                $tableUnit.bootstrapTable('remove', { field: 'uid', values: [row.uid] });
                            }
                        }
                    },
                    "class": 'text-nowrap',
                    sortable: true
                },
                {
                    field: 'Model',
                    title: 'MODEL',
                    halign: 'left',
                    align: 'left',
                    "class": 'text-nowrap',
                    editable: true,
                    editableType: "text",
                    editableMode: 'inline',
                    editableEmptytext: "Set",
                    sortable: true
                },
                {
                    field: 'SerialNumber',
                    title: 'SERIAL NO',
                    halign: 'left',
                    align: 'left',
                    "class": 'text-nowrap',
                    editable: true,
                    editableType: "text",
                    editableMode: 'inline',
                    editableEmptytext: "Set",
                    sortable: true
                },
                {
                    field: 'Batch',
                    title: 'BATCH',
                    halign: 'left',
                    align: 'left',
                    "class": 'text-nowrap',
                    editable: true,
                    editableType: "text",
                    editableMode: 'inline',
                    editableEmptytext: "Set",
                    sortable: true
                },
                {
                    field: 'FreightCost',
                    title: 'FREIGHT COST (RP)',
                    halign: 'left',
                    align: 'left',
                    "class": 'text-nowrap',
                    editable: true,
                    editableType: "number",
                    editableMode: 'inline',
                    editableEmptytext: "Set",
                    sortable: true,
                    formatter: formatCurrency
                }
            ]
        });
    },
    refreshTableUnit: function () {
        $tableUnit.bootstrapTable('refresh');
    },
    initSelect2: function () {
        $("#CustID").select2({
            placeholder: 'Nama Customer',
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

    }
};
function htmlEncode(str) {
    return String(str).replace(/[^a-zA-Z 0-9'.<>]+/g, '').replace(/^\s+|\s+$/g, '')
}
$(function () {
    DIForm.initSelect2();
 
    $("button[name=Cancel]").click(function () {
        $form[0].reset();
        DIForm.hide();
    });

    $("button[name=SubmitForm]").click(function () {
        var isValid = $form.valid();
        if (isValid) {
            DIForm.submitForm("submit");
        }
    });
    $("button[name=SaveAsDraft]").click(function () {
        var isValid = $form.valid();
        if (isValid) {
            DIForm.submitForm("draft");
        }
    });
    $("button[name=SaveAsRevised]").click(function () {
        var isValid = $form.valid();
        if (isValid) {
            DIForm.submitForm("revised");
        }
    });

    $("button[name=Reject]").click(function () {
        DIForm.submitFormApproval("reject");
    });
    $("button[name=Revise]").click(function () {
        DIForm.submitFormApproval("revise");
    });
    $("button[name=Approve]").click(function () {
        DIForm.submitFormApproval("approve");
    });

    $("#myModalRequest").on('shown.bs.modal', function () {
        DIForm.initEventRadio();
        DIForm.refreshTableUnit();
    });

    $("#btnAddUnit").on('click', function () {
        var $tableUnit = $('#tableDeliveryInstructionUnit');
        var data = $tableUnit.bootstrapTable('getData');
        $tableUnit.bootstrapTable('insertRow', {
            index: data.length || 0,
            row: {
                uid: data.length || 0,
                HeaderID: DIForm.ID,
                Model: '',
                SerialNumber: '',
                Batch: '',
                FreightCost: ''
            }
        });
    });
   
    $('#chkvendor').change(function () {
      
        if ($('input[id=chkvendor]').is(':checked')) {
            $("select[id=CustID]").attr("disabled", true);
            $('#CustAddress').val("");
            $("#CustID").select2("val", "");
            $("#CustName").text("", "");
            $("#vendorname").attr("disabled", false);
         
        } else {
            $("select[id=CustID]").attr("disabled", false);
            $("#vendorname").attr("disabled", true);
            $("#vendorname").val("");
        }
    });
    DIForm.initTableUnit();
});