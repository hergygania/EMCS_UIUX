var lastpath_id = window.location.href.split("/");
var getlast_id = Number(lastpath_id[lastpath_id.length - 1]);
var uomtypes = new Array();
// =====================================================================================
// CIPL FORM
$(function () {


    get_parameter_select();
    //GetCustomer();
    load_data();

    get_cipl_by_id();

    var last_approve = lastpath_id[lastpath_id.length - 2];
    if (lastpath_id[4] === "CiplApprove" || last_approve === "CiplView") {
        $("input, textarea, select").prop('disabled', true);
        $('#partAddButton').hide();
    }

})

// BOOTSTRAP TABLE
var $tablepart = $('#tablepartCipl');
var $tableunit = $('#tableunitCipl');
var $tablemisc = $('#tablemiscCipl');


function get_parameter_select() {
    
    $.ajax({
        url: "/emcs/GetSelectItemCipl",
        async: false,
        success: function (data) {
            var exporttype = new Array();
            $.each(data.exporttype, function (index, element) {
                exporttype.push({ 'id': element.Name, 'text': element.Name, 'desc': element.Description });
            });
            $("#exportCipl").select2({
                data: exporttype,
                width: '100%',
                placeholder: 'Please Select Export Type',
            })
            $('#exportCipl').val(null).trigger('change');

            var exportremarks = new Array();
            $.each(data.exportremarks, function (index, element) {
                exportremarks.push({ 'id': element.Name, 'text': element.Name, 'desc': element.Description });
            });
            $("#exportremarksCipl").select2({
                data: exportremarks,
                width: '100%',
                placeholder: 'Please Select Export Remarks',
            })
            $('#exportremarksCipl').val(null).trigger('change');

            
            $.each(data.uomtypes, function (index, element) {
                uomtypes.push({ 'id': element.Value, 'text': element.Name });
            });
          
            $("#UomItemCipl").select2({
                data: uomtypes,
                width: '100%',
                placeholder: 'Please Select UOM Types',
            })
            $('#UomItemCipl').val(null).trigger('change');

            var category = new Array();
            $.each(data.category, function (index, element) {
                category.push({ 'id': element.Id, 'text': element.Text === 'CATERPILLAR NEW EQUIPMENT' ? 'CATERPILLAR NEW EQUIPMENT' : element.Text });
            });
            $("#jenisBarangCipl").select2({
                data: category,
                width: '100%',
                placeholder: 'Please Select Category',
            })
            $('#jenisBarangCipl').val(null).trigger('change');

            var categoryunit = new Array();
            $.each(data.categoryunit, function (index, element) {
                categoryunit.push({ 'id': element.Id, 'text': element.Text });
            });
            $("#unitCipl").select2({
                data: categoryunit,
                width: '100%',
                placeholder: 'Please Select Unit',
            });
            $('#unitCipl').val(null).trigger('change');

            var spareparts = new Array();
            $.each(data.categoryspareparts, function (index, element) {
                spareparts.push({ 'id': element.Id, 'text': element.Text === 'PRA' ? 'PRA / RMA' : element.Text });
            });
            $("#sparepartsCipl").select2({
                data: spareparts,
                width: '100%',
                placeholder: 'Please Select Spareparts',
            });
            $('#sparepartsCipl').val(null).trigger('change');

            var categoryreference = new Array();
            $.each(data.categoryreference, function (index, element) {
                categoryreference.push({ 'id': element.Id, 'text': element.Text });
            });
            $("#idCategoryReference").select2({
                data: categoryreference,
                width: '100%',
                placeholder: 'Please Select Category Reference',
            })
            $('#idCategoryReference').val(null).trigger('change');

            var sc = Select2Parameter(data.soldconsignee);
            $("#soldConsigneeCipl").select2({
                data: sc,
                width: '100%',
                placeholder: 'Please Select Sold To / Consignee To',
            });
            $('#soldConsigneeCipl').val(null).trigger('change');

            var sd = Select2Parameter(data.shipdelivery);
            $("#shipDeliveryCipl").select2({
                data: sd,
                width: '100%',
                placeholder: 'Please Select Ship To / Delivery To',
            });
            $('#shipDeliveryCipl').val(null).trigger('change');

            var incoterms = new Array();
            $.each(data.incoterms, function (index, element) {
                incoterms.push({ 'id': element.Number, 'text': element.Number + ' - ' + element.Name, 'desc': element.Description });
            });
            $("#incoCipl").select2({
                data: incoterms,
                width: '100%',
                placeholder: 'Please Select Incoterms',
            }).on('select2:select', function (event) {
                $('.descriptionIncoTerms').text(event.params.data.desc);
            })
            $('#incoCipl').val(null).trigger('change');

            var paymentterms = new Array();
            $.each(data.paymentterms, function (index, element) {
                paymentterms.push({ 'id': element.Id, 'text': element.Text });
            });
            $("#paymentCipl").select2({
                data: paymentterms,
                width: '100%',
                placeholder: 'Please Select Payment Terms',
            });
            $('#paymentCipl').val(null).trigger('change');

            var shippingmethod = new Array();
            $.each(data.shippingmethod, function (index, element) {
                shippingmethod.push({ 'id': element.Id, 'text': element.Text });
            });
            $("#shippingCipl").select2({
                data: shippingmethod,
                width: '100%',
                placeholder: 'Please Select Shipping Method',
            })
            $('#shippingCipl').val(null).trigger('change');

            var freightpayment = new Array();
            $.each(data.freightpayment, function (index, element) {
                freightpayment.push({ 'id': element.Id, 'text': element.Text });
            });
            $("#freightCipl").select2({
                data: freightpayment,
                width: '100%',
                placeholder: 'Please Select Freight Payment',
            })
            $('#freightCipl').val(null).trigger('change');

            var country = new Array();
            $.each(data.country, function (index, element) {
                country.push({ 'id': element.Id, 'text': element.Text });
            });
            $("#countryCipl").select2({
                data: country,
                allowClear: true,
                width: '100%',
                placeholder: 'Please Select Country',
            }).on("change", function () {
                $('#destinationCipl').val(null);
                GetDestinationPort();
            });
            $('#countryCipl').val(null).trigger('change');

            var forwader = new Array();
            $.each(data.forwader, function (index, element) {
                forwader.push({ 'id': element.Id, 'text': element.Text });
            });
            $("#forwaderCipl").select2({
                data: forwader,
                width: '100%',
                placeholder: 'Please Select Forwader',
            })
            $('#forwaderCipl').val(null).trigger('change');

            var type = new Array();
            $.each(data.type, function (index, element) {
                type.push({ 'id': element.Id, 'text': element.Text });
            });
            $("#typeCipl").select2({
                data: type,
                width: '100%',
                placeholder: 'Please Select Type',
            })
            $('#typeCipl').val(null).trigger('change');

            var currency = new Array();
            $.each(data.currency, function (index, element) {
                currency.push({ 'id': element.Id, 'text': element.Text });
            });
            $("#currencyCipl").select2({
                data: currency,
                width: '100%',
                placeholder: 'Please Select Currency',
            })
            $('#currencyCipl').val(null).trigger('change');

            var packagingtype = new Array();
            $.each(data.packagingtype, function (index, element) {
                packagingtype.push({ 'id': element.Id, 'text': element.Text });
            });
            $("#PackagingTypeParts").select2({
                data: packagingtype,
                width: '100%',
                placeholder: 'Select Packaging Type',
            })
            $('#PackagingTypeParts').val(null).trigger('change');
        }
    });

    $('#areaCipl').select2({
        placeholder: 'Please Select Area',
        ajax: {
            url: "/emcs/GetPlantList",
            dataType: 'json',
            async: false,
            data: function (params) {
                var query = {
                    searchName: params.term,
                }
                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.data, function (item) {
                        return {
                            text: item.PlantCode + ' - ' + item.PlantName,
                            id: item.PlantCode,
                            desc: item.BAreaCode + ' - ' + item.BAreaName
                        }
                    })
                }
            }
        }
    }).on('select2:select', function (event) {
        $('#cabangCipl').val(event.params.data.desc);
    })

    $('#idPickupArea').select2({
        placeholder: 'Please Select Area',
        ajax: {
            url: "/emcs/GetPlantList",
            dataType: 'json',
            data: function (params) {
                var query = {
                    searchName: params.term,
                }
                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.data, function (item) {
                        return {
                            text: item.BAreaCode + ' - ' + item.PlantName,
                            id: item.BAreaCode,
                            desc: item.BAreaCode + ' - ' + item.BAreaName
                        }
                    })
                }
            }
        }
    }).on('select2:select', function (event) {
        $('#idPickupPic').val(null).trigger('change');
    })

    $('#idPickupArea').select2({
        placeholder: 'Please Select Area',
        ajax: {
            url: "/emcs/GetPlantList",
            dataType: 'json',
            data: function (params) {
                var query = {
                    searchName: params.term,
                }
                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.data, function (item) {
                        return {
                            text: item.BAreaCode + ' - ' + item.PlantName,
                            id: item.BAreaCode,
                            desc: item.BAreaCode + ' - ' + item.BAreaName
                        }
                    })
                }
            }
        }
    }).on('select2:select', function (event) {
        $('#idPickupPic').val(null).trigger('change');
    })

}

function GetConsigneeName(id) {
    var getcategory = GetCategoryUsed();
    if (getcategory !== 'MISCELLANEOUS') {
        $.ajax({
            url: "/emcs/GetConsigneeName",
            type: 'POST',
            data: {
                id: id === null ? "" : id.toString(),
                category: getcategory,
                categoryreference: $('#idCategoryReference').val()
            },
            async: false,
            success: function (data) {
                if (data.length > 0) {
                    var json = data;

                    $('#idCustomerCipl').val(json[0].IdCustomer);
                    var last_reference = id[id.length - 1];
                    var checkduplicate = removeSingleAttributeDuplicates(json, 'ConsigneeName').length;
                    if (checkduplicate > 1) {
                        Swal.fire({
                            type: 'warning',
                            title: 'Oops...',
                            text: 'The Reference Number has a different Consignee Name.',
                        })
                        var $select = $('#refCipl');
                        var idToRemove = last_reference;

                        var values = $select.val();
                        if (values) {
                            var i = values.indexOf(idToRemove);
                            if (i >= 0) {
                                values.splice(i, 1);
                                $select.val(values).change();
                            }
                        }
                        return false;
                    } else {
                        $('#consigneeNameCipl').val(json[0].ConsigneeName);
                        $('#consigneeAddressCipl').val(json[0].Street);
                        $('#consigneeCountryCipl').val(json[0].City);
                        $('#consigneeTelpCipl').val(json[0].Telephone);
                        $('#consigneeFaxCipl').val(json[0].Fax);
                        $('#consigneePicCipl').val((json[0].PIC === '-' || json[0].PIC === null) ? "" : json[0].PIC);
                        $('#consigneeEmailCipl').val((json[0].Email === '-' || json[0].Email === null) ? "" : json[0].Email);

                    }

                    $('#currencyCipl').val(json[0].Currency).trigger('change');;
                }

            }
        });
    }
    //return item;
}

function GetCustomer() {
    //GetReferenceNo();

    //$('#idCustomerCipl').select2({
    //    placeholder: "Select Customer ID",
    //    ajax: {
    //        url: "/emcs/GetCustomerForSelect2",
    //        dataType: 'json',
    //        async: false,
    //        data: function (params) { },
    //        processResults: function (data) {
    //            return {
    //                results: $.map(data.data, function (item) {
    //                    return {
    //                        text: item.CustNr,
    //                        id: item.CustNr
    //                    }
    //                })
    //            }
    //        }
    //    }
    //}).on("change", function () {
    //    GetReferenceNo();
    //});
}

function get_cipl_by_id() {
    $.ajax({
        url: '/EMCS/CiplGetById',
        type: 'POST',
        data: {
            id: getlast_id,
        },
        cache: false,
        async: false,
        success: function (data, response) {
            if (data.model.CiplNo != '') {
                $('#partDownloadFile').show();
            }
            else {
                $('#partDownloadFile').hide();
            }
            
            var cipl = data.model;
            var forwader = data.forwader;
            $('#idCipl').val(cipl.Id);
            $('#noCipl').val(cipl.CiplNo);
            $('#jenisBarangCipl').val(cipl.Category).trigger('change');

            $('#dateCipl').val(moment(cipl.CreateDate).format("DD MMM YYYY"));
            $('.tableItem').show();
            if (cipl.Category === 'CATERPILLAR SPAREPARTS') { // Spareparts
                $('.tableItemSpareparts, .categoryspareparts').show();
                $('#forwaderCipl').val('CKB').trigger('change.select2').prop('disabled', true);
                $('#sparepartsCipl').val(cipl.CategoriItem).trigger('change');
                //$table = $('#tablepartCipl').show();
            } else {
                if (cipl.Category === 'MISCELLANEOUS') { // MISC
                    $('#permanentCipl, #refCipl, .tableItemMisc, .categorymisc').show();
                    $('#permanentCipl').val(cipl.CategoriItem).trigger('change');
                    $('.div-idCustomerCipl').hide();

                    //$table = $('#tablemiscCipl').show();
                } else {
                    if (cipl.Category === 'CATERPILLAR NEW EQUIPMENT' || cipl.Category === 'CATERPILLAR USED EQUIPMENT') { // USED EQUIPMENT && UNIT
                        $('.tableItemUnit, .categoryunit').show();
                        $('#unitCipl').val(cipl.CategoriItem).trigger('change');
                        //$table = $('#tableunitCipl').show();
                    } else {
                        $('.tableItem').hide();
                    }
                }
            }
            $('#exportCipl').val(cipl.ExportType).trigger('change');
            $('#idCategoryReference').val(cipl.CategoryReference).trigger('change');

            jQuery.each(cipl.ReferenceNo.split(','), function (index, value) {
                $("#refCipl").append(new Option(value, value, false, true)).trigger("change").prop('disabled', false);
            });

            $('#exportremarksCipl').val(cipl.ExportTypeItem).trigger('change');
            if (cipl.ExportType === 'Non Sales - Return (Permanent)') { // Spareparts
                $('#exportremarksCipl').show();
                $('#exportremarksCipl').val(cipl.ExportTypeItem);
            }
            if (cipl.ExportType === 'Sales (Permanent)') {
                $('#remarksCipl').prop("disabled", false);
            }
            $('#soldConsigneeCipl').val(cipl.SoldConsignee).trigger('change');;
            $('#shipDeliveryCipl').val(cipl.ShipDelivery).trigger('change');;

            $('#consigneeNameCipl').val(cipl.ConsigneeName);
            $('#consigneeAddressCipl').val(cipl.ConsigneeAddress);
            $('#consigneeCountryCipl').val(cipl.ConsigneeCountry);
            $('#consigneeTelpCipl').val(cipl.ConsigneeTelephone);
            $('#consigneeFaxCipl').val(cipl.ConsigneeFax);
            $('#consigneePicCipl').val(cipl.ConsigneePic);
            $('#consigneeEmailCipl').val(cipl.ConsigneeEmail);

            if (cipl.NotifyPartySameConsignee === true) {
                $('.notify').show();
                $('.notifySameCipl').removeClass('btn-default').addClass('btn-success');
            } else {
                $('.notify').show();
                $('.notifyDifferentCipl').removeClass('btn-default').addClass('btn-info');
            }
            $('#notifyNameCipl').val(cipl.NotifyName);
            $('#notifyAddressCipl').val(cipl.NotifyAddress);
            $('#notifyCountryCipl').val(cipl.NotifyCountry);
            $('#notifyTelpCipl').val(cipl.NotifyTelephone);
            $('#notifyFaxCipl').val(cipl.NotifyFax);
            $('#notifyPicCipl').val(cipl.NotifyPic);
            $('#notifyEmailCipl').val(cipl.NotifyEmail);
            
            var ValueArea = {
                id: cipl.Area,
                text: cipl.Area
            };
            var AreaOption = new Option(ValueArea.text, ValueArea.id, false, false);
            $('#areaCipl').append(AreaOption).trigger('change');

            if (cipl.PickUpArea !== null) {
                var SplitPickUpArea = cipl.PickUpArea.split("-");
                var ValuePickUp = {
                    id: SplitPickUpArea[0].trim(),
                    text: SplitPickUpArea[0] + ' - ' + SplitPickUpArea[1]
                };
                var PickUpAreaOption = new Option(ValuePickUp.text, ValuePickUp.id, false, false);
                $('#idPickupArea').append(PickUpAreaOption).trigger('change');
            }

            if (cipl.PickUpPic !== null) {
                var SplitPickUpPic = cipl.PickUpPic.split("-");
                var ValuePickUp = {
                    id: SplitPickUpPic[0],
                    text: SplitPickUpPic[1] + ' - ' + SplitPickUpPic[2]
                };
                var PickUpPicOption = new Option(ValuePickUp.text, ValuePickUp.id, false, false);
                $('#idPickupPic').append(PickUpPicOption).trigger('change');
            }

            $('#areaCipl').val(cipl.Area).trigger('change');
            $('#cabangCipl').val(cipl.Branch).trigger('change.select2');
            $('#ConsolidateCipl').val((cipl.Consolidate === true) ? "true" : "false").trigger('change');
            $('#currencyCipl').val(cipl.Currency).trigger('change');
            $('#RateCipl').val(cipl.Rate);
            $('#paymentCipl').val(cipl.PaymentTerms).trigger('change');
            $('#countryCipl').val(cipl.CountryOfOrigin).trigger('change');
            $('#shippingCipl').val(cipl.ShippingMethod).trigger('change');
            $('#loadingCipl').val(cipl.ExportType);
            $('#destinationCipl').val(cipl.ExportType);
            $('#lcnoCipl').val(cipl.LcNoDate.split('-')[0]);
            $('#lcDateCipl').val(cipl.LcNoDate.split('-')[1]);
            $('#incoCipl').val(cipl.IncoTerm).trigger('change');
            $('#freightCipl').val(cipl.FreightPayment).trigger('change');
            $('#incoCipl').val(cipl.IncoTerm);

            var loadingCipl = new Option(cipl.LoadingPort, cipl.LoadingPort, false, false);
            $('#loadingCipl').append(loadingCipl).trigger('change');

            var destinationCipl = new Option(cipl.DestinationPort, cipl.DestinationPort, false, false);
            $('#destinationCipl').append(destinationCipl).trigger('change');

            $('#shippingMarkCipl').val(cipl.ShippingMarks);
            $('#txtSpecialInscCipl').val(cipl.SpecialInstruction);
            $('#remarksCipl').val(cipl.Remarks);
           

            // CIPL FORWADER
            $('#forwaderCipl').val(forwader.Forwader).trigger('change');
            $('#typeCipl').val(forwader.Type).trigger('change');

            var CkbBranch = new Option(forwader.Branch, forwader.Branch, false, false);
            $('#CkbBranchCipl').append(CkbBranch).trigger('change');
            //$('#CkbBranchCipl').val(forwader.Branch).trigger('change');
            $('#forwaderCompanyCipl').val(forwader.Company);
            $('#forwaderSubconCompanyCipl').val(forwader.SubconCompany);
            $('#forwaderAddressCipl').val(forwader.Address);
            $('#forwaderAreaCipl').val(forwader.Area);
            $('#forwaderCityCipl').val(forwader.City);
            $('#forwaderPostalCodeCipl').val(forwader.PostalCode);
            $('#forwaderForwadingCipl').val(forwader.Forwading);
            $('#forwaderContactCipl').val(forwader.Contact);
            $('#forwaderFaxCipl').val(forwader.FaxNumber);
            $('#forwaderAttentionCipl').val(forwader.Attention);
            $('#forwaderEmailCipl').val(forwader.Email);

            get_ciplitem_by_id();
            get_cipldocument_by_id();
        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Update Data',
            })
        }
    })
}

function get_cipldocument_by_id() {
    $.ajax({
        url: '/EMCS/GetCiplDocumentList',
        type: 'POST',
        data: {
            IdCipl: $('#idCipl').val(),
        },
        cache: false,
        async: false,
        success: function (data, response) {
            var convert = CiplDocumentConvert(data);
            if (convert.length > 0) {
                $tableFormDocuments.bootstrapTable('removeAll');
                $tableFormDocuments.bootstrapTable('load', convert);
            } else {
                $tableFormDocuments.bootstrapTable('removeAll');
            }

        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Get Data',
            })
        }
    })
}

function get_ciplitem_by_id() {
    $.ajax({
        url: '/EMCS/CiplItemGetById',
        type: 'POST',
        data: {
            id: getlast_id,
        },
        //cache: false,
        //async: false,
        success: function (data, response) {
            var convert = CiplItemConvert(data);
            if (convert.length > 0) {
                $('#idCustomerCipl').val(convert[0].IdCustomer);
                if ($('#jenisBarangCipl').val() === 'CATERPILLAR SPAREPARTS' && $('#idCategoryReference').val() !== 'Other') {
                    $tablepart.bootstrapTable('removeAll');
                    $tablepart.bootstrapTable('load', convert);
                    SumReferenceItem();
                } else if ($('#jenisBarangCipl').val() === 'MISCELLANEOUS') {
                    $tablemisc.bootstrapTable('removeAll');
                    $tablemisc.bootstrapTable('load', convert);
                    SumReferenceItem();
                } else if (($('#jenisBarangCipl').val() === 'CATERPILLAR NEW EQUIPMENT' || $('#jenisBarangCipl').val() === 'CATERPILLAR USED EQUIPMENT') && $('#idCategoryReference').val() !== 'Other') {
                    $tableunit.bootstrapTable('removeAll');
                    $tableunit.bootstrapTable('load', convert);
                    SumReferenceItem();
                } else if ($('#idCategoryReference').val() === 'Other') {
                    $tableemail.bootstrapTable('removeAll');
                    $tableemail.bootstrapTable('load', convert);
                    SumReferenceItem();
                }
            } else {
                $tablepart.bootstrapTable('removeAll');
                $tablemisc.bootstrapTable('removeAll');
                $tableunit.bootstrapTable('removeAll');
                $tableemail.bootstrapTable('removeAll');
            }

        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Get Data',
            })
        }
    })
}

//  ==================================================================== VIEW APPROVE ====================================================================
function ApproveCipl(obj) {
    $.ajax({
        url: "/EMCS/CiplApproval",
        type: "POST",
        data: {
            Id: obj.Id,
            Status: obj.Status,
            Notes: obj.Notes,
        },
        success: function (resp) {
            Swal.fire(
                'Approve!',
                'Data Confirmed Successfully',
                'success'
            )
            location.href = "/EMCS/Mytask";

        }
    });
}

$("#btnApproveCipl").on("click", function () {
    Swal.fire({
        title: 'Approve Confirmation',
        text: 'By approving this document, you are responsible for the authenticity of the documents and data entered. Are you sure you want to process this document?',
        type: 'question',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonColor: '#3085d6',
        confirmButtonText: 'Yes, Approve!',
        allowEscapeKey: false,
        allowOutsideClick: false,
        showCloseButton: true
    }).then((result) => {
        if (result.value) {
            Swal.fire({
                input: 'textarea',
                allowEscapeKey: false,
                allowOutsideClick: false,
                inputPlaceholder: 'Type your notes here...',
                inputAttributes: {
                    'aria-label': 'Type your notes here'
                },
                showCancelButton: false
            }).then((result) => {
                var Notes = result.value;
                var Status = "Approve";
                var Id = $('#idCipl').val();
                var data = { Notes: Notes, Status: Status, Id: Id };
                ApproveCipl(data);
            });
        }
        return false;
    });
});

$("#btnReviseCipl").on("click", function () {
    Swal.fire({
        title: 'Revise Confirmation',
        text: 'Do you want to Revise this CIPL Data ?',
        type: 'question',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonColor: '#3085d6',
        html:
            '<select id="swal-input1" class="swal2-input" placeholder="Select Category"></select>' +
            '<select id="swal-input2" class="swal2-input" placeholder="Select Case"><option value="" selected>Select Case</option></select>' +
            '<input id="swal-input3" placeholder="Causes" class="swal2-input">' +
            '<input id="swal-input4" placeholder="Impact" class="swal2-input">' +
            '<textarea id="swal-input5" placeholder="Notes" class="swal2-textarea"></textarea>',
        focusConfirm: false,
        preConfirm: () => {
            var result = {
                Category: $("#swal-input1").val(),
                Case: $("#swal-input2").val(),
                Causes: $("#swal-input3").val(),
                Impact: $("#swal-input4").val(),
                Notes: $("#swal-input5").val(),
                Id: $("#idCipl").val(),
                Status: "Revise"
            };
            return result;
        },
        onBeforeOpen: () => {
            Swal.showLoading();
            $.get("/EMCS/GetListProblem", function (resp) {
                var data = resp.data;
                var options = "<option value='' selected>Select Category</option>";
                data.forEach(function (val, index) {
                    options += "<option value='" + val.Category + "'>" + val.Category + "</option>";
                });
                $("#swal-input1").html(options);
                Swal.disableLoading();
            });
        },
        onOpen: function () {
            $("#swal-input1").on("change", function (e) {
                Swal.showLoading();
                var val = $("#swal-input1").val();
                $.get("/EMCS/GetListCaseByProblemId/?Category=" + val, function (resp) {
                    var data = resp.data;
                    var options = "<option value='' selected>Select Case</option>";
                    data.forEach(function (val, index) {
                        options += "<option value='" + val.Case + "'>" + val.Case + "</option>";
                    });
                    $("#swal-input2").html(options);
                    Swal.disableLoading();
                });
            });
        },
        confirmButtonText: 'Yes, Revise!',
        allowEscapeKey: false,
        allowOutsideClick: false,
        showCloseButton: true
    }).then((result) => {
        ApproveCipl(result.value);
        return false;
    });
});

$("#btnRejectCipl").on("click", function () {
    Swal.fire({
        title: 'Reject Confirmation',
        text: 'Do you want to Reject this GR ?',
        type: 'question',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonColor: '#3085d6',
        html:
            '<select id="swal-input1" class="swal2-input" placeholder="Select Category"></select>' +
            '<select id="swal-input2" class="swal2-input" placeholder="Select Case"><option value="" selected>Select Case</option></select>' +
            '<input id="swal-input3" placeholder="Causes" class="swal2-input">' +
            '<input id="swal-input4" placeholder="Impact" class="swal2-input">' +
            '<textarea id="swal-input5" placeholder="Notes" class="swal2-textarea"></textarea>',
        focusConfirm: false,
        preConfirm: () => {
            var result = {
                Category: $("#swal-input1").val(),
                Case: $("#swal-input2").val(),
                Causes: $("#swal-input3").val(),
                Impact: $("#swal-input4").val(),
                Notes: $("#swal-input5").val(),
                Id: $("#idCipl").val(),
                Status: "Reject"
            };
            return result;
        },
        onBeforeOpen: () => {
            Swal.showLoading();
            $.get("/EMCS/GetListProblem", function (resp) {
                var data = resp.data;
                var options = "<option value='' selected>Select Category</option>";
                data.forEach(function (val, index) {
                    options += "<option value='" + val.Category + "'>" + val.Category + "</option>";
                });
                $("#swal-input1").html(options);
                Swal.disableLoading();
            });
        },
        onOpen: function () {
            $("#swal-input1").on("change", function (e) {
                Swal.showLoading();
                var val = $("#swal-input1").val();
                $.get("/EMCS/GetListCaseByProblemId/?Category=" + val, function (resp) {
                    var data = resp.data;
                    var options = "<option value='' selected>Select Case</option>";
                    data.forEach(function (val, index) {
                        options += "<option value='" + val.Case + "'>" + val.Case + "</option>";
                    });
                    $("#swal-input2").html(options);
                    Swal.disableLoading();
                });
            });
        },
        confirmButtonText: 'Yes, Revise!',
        allowEscapeKey: false,
        allowOutsideClick: false,
        showCloseButton: true
    }).then((result) => {
        ApproveCipl(result.value);
        return false;
    });
});

$('#partDownloadFile').on('click', function () {
    paramSearch = {
        id: $('#idCipl').val()
    };    
    $.ajax({
        url: '/Download/DownloadExcel',       
        type: 'POST',
        data: paramSearch,
        success: function (guid) {
            
            window.open('/Download/DownloadToExcel?guid=' + guid, '_blank');
        },
        cache: false
    });
});


var myDropzone = new Dropzone("#partUploadFile", { // Make the bodyFormUpload a dropzone

    url: "/Download/UploadCIPLItem", // Set the url

    thumbnailHeight: 100,
    thumbnailWeight: 100,
    timeout: "80000",
    method: 'POST',
    dictDefaultMessage: "<h4>Drop the Import File Here or Click this Section for Browse the Import File.</h4>",
    acceptedFiles: '.xlsx',
    filesizeBase: 1024,
    autoProcessQueue: true,
    maxFiles: 1,
    maxFilesize: 100, // MB
    parallelUploads: 1,
    previewTemplate: $("#template-dropzone").html(),
    uploadMultiple: false
    //previewsContainer: "#FormUploadMaterial", // Define the container to display the previews
    //clickable: ".fileinput-button" // Define the element that should be used as click trigger to select files.
});

myDropzone.on("addedfile", function (file) {
    // Hookup the start button
    $("#actions .start").on("click", function () {
        myDropzone.enqueueFile(file);
    });
    $("#placeholderUpload").hide();
});

myDropzone.on("totaluploadprogress", function (progress) {
    $("#total-progress .progress-bar").css("width", progress + "%");
    $("#progressPercent").html(progress + "%");
});

myDropzone.on("sending", function (file, xhr, formData) {
    formData.append("idCIPL", $("#idCipl").val());      
    formData.append("idReference", $("#refCipl").val());
    formData.append("refCipl", $("#refCipl").text());
    // Show the total progress bar when upload starts
    $("#total-progress").css("opacity", 1);
    // And disable the start button
    $("#actions .delete").attr("disabled", "disabled");
    $(".start").attr("disabled", "disabled");
});

myDropzone.on("complete", function (resp) {
if (resp.status === "success") {
    $("#actions .delete").prop("disabled", false);
    if (resp.size >= 9785 && resp.size <= 9800) {
        swal.fire("Upload Status", "Empty files will not be uploaded.", "error");
    }
    else {
        var respText = resp.xhr.response;
        var respData = JSON.parse(respText);
        console.log(respData);
        var type = respData.status ? "success" : "error";
        swal.fire("Upload Status", respData.msg, type);
        $("#tablepartCipl").bootstrapTable('destroy');
        reload_tablepart();
        get_ciplitem_by_id();          
    }
}
});

myDropzone.on("queuecomplete", function (progress) {
    $("#total-progress").css("opacity", "0");
    setTimeout(function () {
        myDropzone.removeAllFiles(true);
    }, 400);
});

$("#actions .start").on("click", function () {
    myDropzone.enqueueFiles(myDropzone.getFilesWithStatus(Dropzone.QUEUED));
});

$("#actions .cancel").on("click", function () {
    myDropzone.removeAllFiles(true);
    $("#placeholderUpload").hide();
});

var myDropzoneDocument = new Dropzone("#FormUploadDocumentContainer", { // Make the bodyFormUpload a dropzone
    url: "/EMCS/CiplDocumentUpload", // Set the url
    thumbnailHeight: 100,
    thumbnailWeight: 100,
    method: 'POST',
    acceptedFiles: '.pdf, .jpeg, .jpg, .png',
    filesizeBase: 1024,
    dictDefaultMessage: "<h4>Click this Section for Browse the Image File.</h4>",
    autoProcessQueue: true,
    maxFiles: 1,
    maxFilesize: 10, // MB
    parallelUploads: 1,
    previewTemplate: $("#template-dropzone").html(),
    uploadMultiple: false,
});

myDropzoneDocument.on("addedfile", function (file) {

    // Hookup the start button
    $("#actions .start").on("click", function () {
        myDropzone.enqueueFile(file);
    });
    $("#placeholderUpload").hide();
});

myDropzoneDocument.on("totaluploadprogress", function (progress) {
    $("#total-progress .progress-bar").css("width", progress + "%");
    $("#progressPercent").html(progress + "%");
});

myDropzoneDocument.on("sending", function (file, xhr, formData) {
    formData.append("id", $("#IdDocumentUpload").val());
    // Show the total progress bar when upload starts
    $("#total-progress").css("opacity", 1);
    // And disable the start button
    $("#actions .delete").attr("disabled", "disabled");
    $(".start").attr("disabled", "disabled");

});

myDropzoneDocument.on("complete", function (resp) {
    if (resp.status === "success") {
        $("#actions .delete").prop("disabled", false);
        if (resp.size >= 9785 && resp.size <= 9800) {
            swal.fire("Upload Status", "Empty files will not be uploaded.", "error");
        }
        else {
            var respText = resp.xhr.response;
            var respData = JSON.parse(respText);
            console.log(respData);
            var type = respData.status ? "success" : "error";
            swal.fire("Upload Status", respData.msg, type);
            get_cipldocument_by_id();
        }
    }
});

myDropzoneDocument.on("queuecomplete", function (progress) {
    $("#total-progress").css("opacity", "0");
    setTimeout(function () {
        myDropzoneDocument.removeAllFiles(true);
    }, 400);
});

$("#actions .start").on("click", function () {
    myDropzoneDocument.enqueueFiles(myDropzoneDocument.getFilesWithStatus(Dropzone.QUEUED));
});

$("#actions .cancel").on("click", function () {
    myDropzoneDocument.removeAllFiles(true);
    $("#placeholderUpload").hide();
});

function reload_tablepart() {

var part = [
    [
        {
            field: "action",
            title: "Action",
            rowspan: 2,
            align: 'center',
            class: "text-nowrap",
            events: window.operateEvents,
            formatter: function (value, row, index) {
                return "<button class='btn btn-default btn-xs EditReferenceItem' type='button' data-toggle='modal' data-target='#ModalUpdateReference' value='Edit' title='Edit'><i class='fa fa-pencil'></i></button> <button class='btn btn-danger btn-xs DeleteReferenceItem' type='button' title='Delete'><i class='fa fa-trash'></i></button>";
            }
        }, {
            field: "Id",
            title: "Id Item",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "IdCipl",
            title: "Id Cipl",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "IdReference",
            title: "Id Reference",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: 'ReferenceNo',
            title: 'Reference No',
            rowspan: 2,
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            visible: false
        }, {
            field: 'IdCustomer',
            title: 'Id Customer',
            rowspan: 2,
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            visible: false
        }, {
            field: "Name",
            title: "Name",
            rowspan: 2,
            align: 'center',
            sortable: true,
            class: "text-nowrap"
        }, {
            field: "UnitUom",
            title: "UOM",
            rowspan: 2,
            align: 'center',
            sortable: true,
            formatter: function (value, row, index) {
                return row.UnitUom;
            }
        }, {
            field: "CoO",
            title: "Country Of Origin",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "PartNumber",
            title: "Part Number",
            rowspan: 2,
            sortable: true,
            align: 'center'
        }, {
            field: "Sn",
            title: "SN",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "JCode",
            title: "J-Code",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "Ccr",
            title: "CCR",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "ASNNumber",
            title: "ASN Number",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "CaseNumber",
            title: "Case Number",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "Type",
            title: "Type",
            rowspan: 2,
            align: 'center',
            sortable: true,
            class: 'text-nowrap',
            filterControl: true
        }, {
            field: "IdNo",
            title: "Id No",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "YearMade",
            title: "Year Made",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "Quantity",
            title: "Quantity",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "UnitPrice",
            title: "Unit Price",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "ExtendedValue",
            title: "Extended Value",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "dimension",
            title: "Dimension (In CM)",
            colspan: 3,
            align: 'center',
            sortable: true,
            filterControl: true,
            valign: 'middle'
        }, {
            field: "Volume",
            title: "Volume",
            colspan: 1,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "NetWeight",
            title: "Net Weight",
            colspan: 1,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "GrossWeight",
            title: "Gross Weight",
            colspan: 1,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "Currency",
            title: "Currency",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true,
            visible: false
        }, {
            field: "IdParent",
            title: "Id Parent",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true,
            visible: false
        }, {
            field: "SibNumber",
            title: "SIB Number",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "WoNumber",
            title: "WO Number",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "Claim",
            title: "Claim",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }],
    [{
        field: "Length",
        title: "Length",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.Length === '0.00' ? '' : row.Length : row.Length;
        }
    }, {
        field: "Width",
        title: "Width",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.Width === '0.00' ? '' : row.Width : row.Width;
        }
    }, {
        field: "Height",
        title: "Height",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.Height === '0.00' ? '' : row.Height : row.Height;
        }
    }, {
        field: "Volume",
        title: "(m3)",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.Volume === '0.000000' ? '' : row.Volume : row.Volume;
        }
    }, {
        field: "NetWeight",
        title: "in KGa",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.NetWeight === '0.00' ? '' : row.NetWeight : row.NetWeight;
        }
    }, {
        field: "GrossWeight",
        title: "in KGa",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.GrossWeight === '0.00' ? '' : row.GrossWeight : row.GrossWeight;
        }
    }
    ]
]

$tablepart.bootstrapTable({
    columns: part,
    cache: false,
    search: false,
    striped: true,
    clickToSelect: true,
    reorderableColumns: true,
    toolbar: '.toolbarParts',
    toolbarAlign: 'left',
    onClickRow: selectRow,
    showColumns: true,
    showRefresh: false,
    smartDisplay: false,
    pagination: true,
    sidePagination: 'client',
    pageSize: '5',
    formatNoMatches: function () {
        return '<span class="noMatches">No data found</span>';
    },
});
}
