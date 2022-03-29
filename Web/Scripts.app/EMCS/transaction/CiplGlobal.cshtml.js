var $AllowDelete = $('#AllowDelete').val();
var $AllowUpdate = $('#AllowUpdate').val();

var $tablepart = $('#tablepartCipl');
var $tableunit = $('#tableunitCipl');
var $tablemisc = $('#tablemiscCipl');
var $tableemail = $('#tableemailCipl');
var $tablereference = $('#tablereference');
var $tableFormDocuments = $('#tableCiplFormDocuments');

var $tablepartcheck = $('#tablepartCipl');
var $tableunitcheck = $('#tableunitCipl');
var $tablemisccheck = $('#tablemiscCipl');
var $tableemailcheck = $('#tableemailCipl');

var $form = $("#FormCipl");

function load_data() {
    //ID

    $('#ConsolidateCipl').select2({
        placeholder: "Please Select Consolidate"
    })

    $('#sparepartsCipl, #permanentCipl, #remarkstextCipl, #unitCipl, .divCkbBranchCipl, .forwaderCompanyCipl, .forwaderForwadingCipl, .forwaderAddressCipl, .forwaderAreaCipl, .forwaderCityCipl, .forwaderPostalCodeCipl, .forwaderFaxCipl, .forwaderContactCipl, .forwaderAttentionCipl, .forwaderEmailCipl, #div-SIBNumberItemCipl, #div-WONumberItemCipl, #div-ClaimItemCipl').hide();
    $('.inpConsignee, .inpNotify, #refCipl, #lcnoCipl, #lcDateCipl, #freightCipl, #cabangCipl, #idCategoryReference').prop("disabled", true);
    $('#idCustomerCipl').prop('disabled', true);

    //CLASS
    $('/*.notify, .div-sales, .div-sales-consignee, .divStatusEdit */ .tableItemSpareparts, .tableItemUnit, .tableItemMisc, .tableItemEmail, .categoryspareparts, .categoryunit, .categorymisc, .exportRemarks').hide();
    $('#refCipl').on('change', function () {
        GetConsigneeName($(this).val());
        $('#idCustomerCipl').val(null);

        var Category = GetCategoryUsed();
        //Category === 'REMAN' ? $tablereference.bootstrapTable('hideColumn', 'JCode') : $tablereference.bootstrapTable('showColumn', 'JCode')
        if (Category === 'SIB' || Category === 'REMAN') {
            $tablepart.bootstrapTable('showColumn', 'SibNumber');
            $tablepart.bootstrapTable('showColumn', 'WoNumber')
            $tablepart.bootstrapTable('showColumn', 'Claim')
            $('#div-SIBNumberItemCipl, #div-WONumberItemCipl, #div-ClaimItemCipl').show();
        } else {
            $tablepart.bootstrapTable('hideColumn', 'SibNumber');
            $tablepart.bootstrapTable('hideColumn', 'WoNumber')
            $tablepart.bootstrapTable('hideColumn', 'Claim')
            $('#div-SIBNumberItemCipl, #div-WONumberItemCipl, #div-ClaimItemCipl').hide();
        }
    })
    if ($('#refCipl') !== null) {
        $('.btnAddItem').prop("disabled", false);
    }
    
    $('#jenisBarangCipl').on('change', function () {
        GetReferenceNo();

        $('#unitCipl, #permanentCipl, .divCkbBranchCipl, .forwaderCompanyCipl, .forwaderForwadingCipl, .forwaderAddressCipl, .forwaderAreaCipl, .forwaderCityCipl, .forwaderPostalCodeCipl, .forwaderFaxCipl, .forwaderContactCipl, .forwaderAttentionCipl, .forwaderEmailCipl').val(null).hide();
        $('.categoryspareparts, .categorymisc, .categoryunit, .tableItemSpareparts, .tableItemUnit, .tableItemMisc, .tableItemEmail').hide();
        //$('.div-sales, .div-sales-consignee').hide();
        $('#forwaderCipl, #sparepartsCipl, #unitCipl, #exportCipl').val(null).trigger('change.select2').prop('disabled', false);

        $('#idCustomerCipl').val(null);
        $('#refCipl, #exportCipl, #CkbBranchCipl, #idCategoryReference').val(null).trigger('change.select2');
        $('.tableItem, .div-idCustomerCipl').show();

        $("#exportCipl option[value='Non Sales - Exhibition (Temporary)']").remove();
     
        if ($(this).val() === 'CATERPILLAR SPAREPARTS') { // Spareparts
            $('#divReferenceNo, .div-idCustomerCipl').show();
            $('.tableItemSpareparts, .categoryspareparts').show();
            $('#forwaderCipl').val('CKB').trigger('change.select2').prop('disabled', false);
          
            //$('#forwaderAttentionCipl, #divCkbBranchCipl, #forwaderAddressCipl, #forwaderCityCipl, #forwaderPostalCodeCipl, #forwaderFaxCipl, #forwaderContactCipl, #forwaderEmailCipl').show("slow");
            $('.forwaderAttentionCipl, .divCkbBranchCipl, .forwaderAddressCipl, .forwaderCityCipl, .forwaderPostalCodeCipl, .forwaderFaxCipl, .forwaderContactCipl, .forwaderEmailCipl, .forwaderCompanyCipl, .forwaderForwadingCipl').show("slow");
        } else {
            if ($(this).val() === 'MISCELLANEOUS') { // MISC
                $('#permanentCipl, #refCipl, .tableItemMisc, .categorymisc').show();
                $('#divReferenceNo, .div-idCustomerCipl').hide();
            } else {
                $('#divReferenceNo, .div-idCustomerCipl').show();
                if ($(this).val() === 'CATERPILLAR NEW EQUIPMENT' || $(this).val() === 'CATERPILLAR USED EQUIPMENT') { // USED EQUIPMENT && UNIT
                    $('.tableItemUnit, .categoryunit').show();
                    $('#exportCipl').append('<option value="Non Sales - Exhibition (Temporary)" data-category="No Commercial Value for Exhibition Purposes Only">Non Sales - Exhibition (Temporary)</option>').val('').trigger("change");
                } else {
                    $('.tableItem').hide();
                }
            }
        }

    });

    $('#soldConsigneeCipl').on('change', function () {
        $("#shipDeliveryCipl option[value='Sold To']").remove();
        if ($(this).val() === 'Consignee') {
            $('#shipDeliveryCipl').append('<option value="Sold To">Sold To</option>').val(null).trigger("change");
            $('.notifySameCipl').text('').append('Same As ' + $(this).val());
        } else {
            $('#shipDeliveryCipl').val(null).trigger("change");
            $('.notifySameCipl').text('').append('Same As ' + $(this).val());
        }
    })

    $('#sparepartsCipl').on('change', function () {
        $('#refCipl').val(null);
        GetReferenceNo();
        if ($(this).val() === 'Old Core') {
            $('#exportCipl').val('Non Sales - Return (Permanent)').trigger('change').prop('disabled', true);

            $("#soldConsigneeCipl").val("Consignee").trigger('change');;
            $("#shipDeliveryCipl").val("Notify").trigger('change');;
            $("#consigneeNameCipl").val("Caterpillar S.A.R.L Singapore Branch ( Core )");
            $("#consigneeAddressCipl").val("5 Tukang Innovation Grove\nSingapore 618304");
            $("#consigneeCountryCipl").val("Attn : Chee Kwong Kwan");
            $("#consigneeTelpCipl").val("-");
            $("#consigneeFaxCipl").val("-");
            $("#consigneePicCipl").val("Mr Chee kwong kwan");
            $("#consigneeEmailCipl").val("kwan_chee_kwong@cat.com>;Choo_Tien_Tean@cat.com");

            $("#notifyNameCipl").val("DHL Global Forwarding (S) Pte Ltd");
            $("#notifyAddressCipl").val("No. 1 Changi South Street 2\n4th Floor, DHL Distribution Centre\nSingapore 486760");
            $("#notifyCountryCipl").val("Singapore");
            $("#notifyTelpCipl").val("-");
            $("#notifyFaxCipl").val("-");
            $("#notifyPicCipl").val("Jing Kang");
            $("#notifyEmailCipl").val("edgsg-dgf-catofrimport@dhl.com");

        } else if ($(this).val() === 'PRA') {
            $("#soldConsigneeCipl").val("Consignee").trigger('change');;
            $("#shipDeliveryCipl").val("Notify").trigger('change');;
            $("#consigneeNameCipl").val("Caterpillar S.A.R.L Singapore Branch");
            $("#consigneeAddressCipl").val("14 Tractor road Singapore 627973");
            $("#consigneeCountryCipl").val("Singapore");
            $("#consigneeTelpCipl").val("+65 68293195 / +65 68293192 / +65 68293018");
            $("#consigneeFaxCipl").val("+65 68200667");
            $("#consigneePicCipl").val("Nithya / Lim Beng Hwa / Jeffrey Wong (Freight management team)");
            $("#consigneeEmailCipl").val("Gunasekaran_Nithyalakshmi@cat.com;Lim_Beng_Hwa@cat.com;Wong_Jeffrey@cat.com");

            $("#notifyNameCipl").val("DHL Global Forwarding (S) Pte Ltd");
            $("#notifyAddressCipl").val("DHL Global Forwarding (S) Pte Ltd\n4th Floor, DHL Distribution Centre\nSingapore 486760");
            $("#notifyCountryCipl").val("Singapore");
            $("#notifyTelpCipl").val("+65 63186888");
            $("#notifyFaxCipl").val("+65 65429972");
            $("#notifyPicCipl").val("Import Ocean Team");
            $("#notifyEmailCipl").val("edgsg-dgf-catofrimport@dhl.com");
        } else {
            $('#exportCipl').val(null).trigger('change').prop('disabled', false);
        }
    })

    $('#exportCipl').on('change', function () {
        var selected = $(this).children("option:selected").data('data');
        $('.exportRemarks').hide();
        if ($('#sparepartsCipl').val() != 'PRA' && $('#sparepartsCipl').val() != 'Old Core')
        {
            $('.inpConsignee, .inpNotify').val(null).prop("disabled", true);
        }
        $('#refCipl, #idCustomer').val(null).trigger('change.select2').prop("disabled", true);
        $('.div-mandatory').removeClass("has-error");
        if ($(this).val() === 'Non Sales - Repair Return (Temporary)') { // Non Sales - Repair Return (Temporary)
            $('#remarksCipl').val(selected.desc).prop('disabled', true);
            //$('.inpConsignee, .inpNotify, #idCategoryReference').val(null).prop("disabled", false);
            $('.inpConsignee, .inpNotify').prop("disabled", false);
            $('#idCategoryReference').val(null).prop("disabled", false);
            if ($('#sparepartsCipl').val() != 'PRA' && $('#sparepartsCipl').val() != 'Old Core') {
                $('.inpConsignee, .inpNotify, #idCategoryReference').val(null).prop("disabled", false);
            }
            //$('#idCategoryReference').val(null).prop("disabled", false);
            //if ($('#sparepartsCipl').val() != 'PRA' && $('#sparepartsCipl').val() != 'Old Core') {
            //    $('.inpConsignee, .inpNotify').val(null).prop("disabled", false);
            //}
            $('#refCipl, #idCategoryReference').val(null).trigger('change.select2');
            $('.col-ship-to, .col-notify-party').removeClass("col-md-4").addClass("col-md-6");
            $('.div-mandatory').addClass("has-error");
        } else if ($(this).val() === 'Non Sales - Return (Permanent)') { // Non Sales - Return (Permanent)
            $('#remarksCipl').val(selected.desc).prop('disabled', true);
            //$('.inpConsignee, .inpNotify, #idCategoryReference').val(null).prop("disabled", false);
            $('.inpConsignee, .inpNotify').prop("disabled", false);
            $('#idCategoryReference').val(null).prop("disabled", false);
            if ($('#sparepartsCipl').val() != 'PRA' && $('#sparepartsCipl').val() != 'Old Core') {
                $('.inpConsignee, .inpNotify, #idCategoryReference').val(null).prop("disabled", false);
            }
            //$('#idCategoryReference').val(null).prop("disabled", false);
            //if ($('#sparepartsCipl').val() != 'PRA' && $('#sparepartsCipl').val() != 'Old Core') {
            //    $('.inpConsignee, .inpNotify').val(null).prop("disabled", false);
            //}
            $('#refCipl, #idCategoryReference').val(null).trigger('change.select2');
            $('.exportRemarks').show("slow");
            $('.col-ship-to, .col-notify-party').removeClass("col-md-4").addClass("col-md-6");
        } else if ($(this).val() === 'Non Sales - Personal Effect (Permanent)') { // Non Sales - Personal Effect (Permanent)
            $('#remarksCipl').val(selected.desc).prop('disabled', true);
            //$('.inpConsignee, .inpNotify, #idCategoryReference').val(null).prop("disabled", false);
            $('.inpConsignee, .inpNotify').prop("disabled", false);
            $('#idCategoryReference').val(null).prop("disabled", false);
            if ($('#sparepartsCipl').val() != 'PRA' && $('#sparepartsCipl').val() != 'Old Core') {
                $('.inpConsignee, .inpNotify, #idCategoryReference').val(null).prop("disabled", false);
            }
            //$('#idCategoryReference').val(null).prop("disabled", false);
            //if ($('#sparepartsCipl').val() != 'PRA' && $('#sparepartsCipl').val() != 'Old Core') {
            //    $('.inpConsignee, .inpNotify').val(null).prop("disabled", false);
            //}
            $('#refCipl, #idCategoryReference').val(null).trigger('change.select2');
            $('.col-ship-to, .col-notify-party').removeClass("col-md-4").addClass("col-md-6");
        } else if ($(this).val() === 'Non Sales - Exhibition (Temporary)') {
            $('#remarksCipl').val(selected.element.dataset.category).prop('disabled', true);
            //$('.inpConsignee, .inpNotify, #idCategoryReference').val(null).prop("disabled", false);
            $('.inpConsignee, .inpNotify').prop("disabled", false);
            $('#idCategoryReference').val(null).prop("disabled", false);
            if ($('#sparepartsCipl').val() != 'PRA' && $('#sparepartsCipl').val() != 'Old Core') {
                $('.inpConsignee, .inpNotify, #idCategoryReference').val(null).prop("disabled", false);
            }
            //$('#idCategoryReference').val(null).prop("disabled", false);
            //if ($('#sparepartsCipl').val() != 'PRA' && $('#sparepartsCipl').val() != 'Old Core') {
            //    $('.inpConsignee, .inpNotify').val(null).prop("disabled", false);
            //}
            $('#refCipl, #idCategoryReference').val(null).trigger('change.select2');
            $('.col-ship-to, .col-notify-party').removeClass("col-md-4").addClass("col-md-6");
            $('.div-mandatory').addClass("has-error");
        } else if ($(this).val() === 'Sales (Permanent)') { // Sales Permanent
            $('#notifySameCipl').val();
            $('#remarksCipl').val(null).prop('disabled', false);
            //$('.inpConsignee, .inpNotify, #idCategoryReference').val(null).prop("disabled", false);
            $('.inpConsignee, .inpNotify').prop("disabled", false);
            $('#idCategoryReference').val(null).prop("disabled", false);
            if ($('#sparepartsCipl').val() != 'PRA' && $('#sparepartsCipl').val() != 'Old Core') {
                $('.inpConsignee, .inpNotify, #idCategoryReference').val(null).prop("disabled", false);
            }
            //$('#idCategoryReference').val(null).prop("disabled", false);
            //if ($('#sparepartsCipl').val() != 'PRA' && $('#sparepartsCipl').val() != 'Old Core') {
            //    $('.inpConsignee, .inpNotify').val(null).prop("disabled", false);
            //}
            $('#refCipl, #idCategoryReference').val(null).trigger('change.select2');
        } else {
            $('.inpConsignee, .inpNotify').val(null).prop("disabled", true);
            $('#refCipl, #idCategoryReference').val(null).trigger('change.select2');
        }

    });

    $('#exportremarksCipl').on('change', function () {
        var selected = $(this).children("option:selected").data('data');
        if ($(this).val() === 'Shipped as Return Shipment') { // SHIPPED AS RETURN SHIPMENT
            $('#remarksCipl').val(selected.desc).prop('disabled', true);
        } else if ($(this).val() === 'Shipped for Tehnical Analysis Reason') { // SHIPPED FOR TECHNICAL ANALYSIS REASON
            $('#remarksCipl').val(selected.desc).prop('disabled', true);
        }
    })

    $('#idCategoryReference').on('change', function () {
        
        if ($(this).val() === null) {
            $('#refCipl').val(null).trigger('change.select2').prop('disabled', false);
            $('#divRefCipl').show();
            if ($('#jenisBarangCipl').val() === 'CATERPILLAR NEW EQUIPMENT' || $('#jenisBarangCipl').val() === 'CATERPILLAR USED EQUIPMENT') { // USED EQUIPMENT && UNIT
                $('.tableItemUnit, .categoryunit').show();
                $('#exportCipl').append('<option value="Non Sales - Exhibition (Temporary)" data-category="No Commercial Value for Exhibition Purposes Only">Non Sales - Exhibition (Temporary)</option>').val('').trigger("change");
            } else {
                $('.tableItem, .tableItemEmail').hide();
                $('.tableItemSpareparts').show();
            }
        } else if ($(this).val() === "Other") {
            $('#divRefCipl, .tableItemSpareparts, .categoryspareparts, .tableItemUnit, categoryunit').hide();
            $('.tableItemEmail').show();
            $('#refCipl').val(null).trigger('change.select2').prop('disabled', true);
        } else {
            $('#refCipl').val(null).trigger('change.select2').prop('disabled', false);
            $('#divRefCipl').show();
            if ($('#jenisBarangCipl').val() === 'CATERPILLAR NEW EQUIPMENT' || $('#jenisBarangCipl').val() === 'CATERPILLAR USED EQUIPMENT') { // USED EQUIPMENT && UNIT
                $('.tableItemUnit, .categoryunit').show();
                $('.tableItemSpareparts, .categoryspareparts, .tableItemEmail').hide();
            } else {
                $('.tableItemEmail, .tableItemUnit, .categoryunit').hide();
                $('.tableItemSpareparts, .categoryspareparts').show();
            }
        }

    })

    $('#forwaderCipl').on('change', function () {
        $('#CkbBranchCipl').val(null).trigger('change');
        $('#divCkbBranchCipl, #forwaderForwadingCipl, #forwaderAddressCipl, #forwaderAreaCipl, #forwaderCityCipl, #forwaderPostalCodeCipl, #forwaderFaxCipl, #forwaderContactCipl, #forwaderAttentionCipl, #forwaderEmailCipl').val(null);
        $('.divCkbBranchCipl, .forwaderCompanyCipl, .forwaderForwadingCipl, .forwaderAddressCipl, .forwaderAreaCipl, .forwaderCityCipl, .forwaderPostalCodeCipl, .forwaderFaxCipl, .forwaderContactCipl, .forwaderAttentionCipl, .forwaderEmailCipl').hide("slow");
        if ($(this).val() === 'CKB') {
            $('.divCkbBranchCipl, .forwaderCompanyCipl, .forwaderForwadingCipl, .forwaderAddressCipl, .forwaderCityCipl, .forwaderPostalCodeCipl, .forwaderFaxCipl, .forwaderContactCipl, .forwaderAttentionCipl, .forwaderEmailCipl').show("slow");
            //} else if ($(this).val() === 'Non CKB' && $('#jenisBarangCipl').val() === 'CATERPILLAR USED EQUIPMENT') {
            //$('').show("slow");
            $('#forwaderCompanyCipl').val("PT.Cipta Krida Bahari");
        } else if ($(this).val() === 'Non CKB'/* && $('#jenisBarangCipl').val() != 'CATERPILLAR USED EQUIPMENT'*/) {
            $('.forwaderCompanyCipl, .forwaderAddressCipl, .forwaderContactCipl, .forwaderFaxCipl, .forwaderEmailCipl, .forwaderAttentionCipl').show("slow");
            $('#forwaderCompanyCipl').val("");

        }
    })

    $('#div-radio-notify span').on('click', function () {
        var sel = $(this).data('value');
        var tog = $(this).data('toggle');
        var active = $(this).data('active');
        var classes = "btn-default btn-primary btn-success btn-info btn-warning btn-danger btn-link";
        var notactive = $(this).data('notactive-notify');
        $('#' + tog).val(sel);

        if (sel === 'L') {
            $('span[data-toggle="' + tog + '"]').not('[data-value="' + sel + '"]').removeClass('active ' + classes).addClass('notactive-notify ' + notactive);
            $('span[data-toggle="' + tog + '"][data-value="' + sel + '"]').removeClass('notactive-notify ' + classes).addClass('active-notify ' + active);
            $('#notifyNameCipl').val($('#consigneeNameCipl').val());
            $('#notifyAddressCipl').val($('#consigneeAddressCipl').val());
            $('#notifyCountryCipl').val($('#consigneeCountryCipl').val());
            $('#select2-notifyCountryCipl-container').text($('#notifyCountryCipl').val());
            $('#notifyTelpCipl').val($('#consigneeTelpCipl').val());
            $('#notifyFaxCipl').val($('#consigneeFaxCipl').val());
            $('#notifyPicCipl').val($('#consigneePicCipl').val());
            $('#notifyEmailCipl').val($('#consigneeEmailCipl').val());
            $('#NotifyPartySameConsignee').val(true);
            $('.notify').show();
        } else if (sel === 'D') {
            $('span[data-toggle="' + tog + '"]').not('[data-value="' + sel + '"]').removeClass('active ' + classes).addClass('notactive-notify ' + notactive);
            $('span[data-toggle="' + tog + '"][data-value="' + sel + '"]').removeClass('notactive-notify ' + classes).addClass('active-notify ' + active);
            $('#notifyNameCipl').val(null);
            $('#notifyAddressCipl').val(null);
            $('#notifyCountryCipl').val(null);
            $('#notifyTelpCipl').val(null);
            $('#notifyFaxCipl').val(null);
            $('#notifyPicCipl').val(null);
            $('#notifyEmailCipl').val(null);
            $('#NotifyPartySameConsignee').val(false);
            $('.notify').show();
        }
    });

    $('#paymentCipl').on('change', function () {
        $('#lcnoCipl, #lcDateCipl').prop('disabled', true);
        var value = "" + $(this).val() + "";
        //var n = str.includes("world");
        if (value.includes("LC")) {
            $('#lcnoCipl, #lcDateCipl').prop('disabled', false);
        }
    })

    $('#incoCipl').on('change', function () {
        var v = $(this).val();
        if (v === 'EXW' || v === 'FCA' || v === 'FAS' || v === 'FOB') {
            $('#freightCipl').val('Collect').trigger('change');
        } else if (v === 'CFR' || v === 'CIF' || v === 'CIP' || v === 'CPT' || v === 'DAT' || v === 'DAP' || v === 'DDP') {
            $('#freightCipl').val('Prepaid').trigger('change');
        }
    })

    $('#partAddButton, #unitAddButton').on('click', function () {

        if (($('#refCipl').val() === null || $('#refCipl').val() === '') && $('#jenisBarangCipl').val() !== 'MISCELLANEOUS' && $('#idCategoryReference').val() !== 'Other') {
            Swal.fire(
                'Warning!',
                'Please fill Reference No',
                'warning'
            );
            return false;
        }

        $('#IdReference, #ReferenceItemCipl, #IdCustomerItemCipl, #PartItemCipl, #JcodeItemCipl, #CcrItemCipl, #IdNoItemCipl, #CurrencyItemCipl, #UnitItemCipl, #ExtendedItemCipl').prop("disabled", true);

        $('.btnAddReference, .btnUpdateReference, .btnAddMisc').hide();
        if ($(this).val() === 'Add') {
            $('.btnAddReference').show();
        } else {
            $('.btnUpdateReference').show();
        }
        GetReferenceItem();
    })
 
    $('#miscAddButton').on('click', function () {
        $('.btnAddReference, .btnUpdateReference, .btnAddMisc').hide();
        $('#SnItemCipl, #CaseNumItemCipl, #ASNNumItemCipl, #CoOItemCipl, #TypeItemCipl, #YearMadeItemCipl').prop('disabled', true);
        $('#UnitItemCipl, #LengthItemCipl, #WidthItemCipl, #HeightItemCipl, #NetItemCipl, #GrossItemCipl').val(0);
        $('#PartItemCipl, #UnitItemCipl').prop('disabled', false);
        if ($(this).val() === 'Add') {
            $('.btnAddMisc').show();
        } else {
            $('.btnUpdateReference').show();
        }
    })

    $('#emailAddButton').on('click', function () {
        $('.btnAddReference, .btnUpdateReference, .btnAddMisc').hide();
        //$('#SnItemCipl').prop('disabled', true);
        $('#CcrItemCipl, #JcodeItemCipl, #IdCustomerItemCipl, #SnItemCipl').prop('disabled', false);
        $('#UnitItemCipl, #LengthItemCipl, #WidthItemCipl, #HeightItemCipl, #NetItemCipl, #GrossItemCipl').val(0);
        $('#PartItemCipl, #UnitItemCipl').prop('disabled', false);
        if ($(this).val() === 'Add') {
            $('.btnAddMisc').show();
        } else {
            $('.btnUpdateReference').show();
        }
    })

    $('#idPickupArea').on('change', function () {
        $('#idPickupPic').val(null).trigger('change');
    })

    $('#BtnSearchReference').on('click', function () {
        if ($('#Column').valid() && $('#Column').valid()) {
            $tablereference.bootstrapTable('refresh');
        }
    })
    $('#documentAddButton').on('click', function () {
        $('.modal-title-document').text("Add Document");
        $('.btnAddDocument').text("Add");
    })

    window.operateEvents = {
        'click .EditReferenceItem': function (e, value, row, index) {
            debugger;
            if ($('#jenisBarangCipl').val() === 'MISCELLANEOUS') {
                $('.btnAddReference, .btnUpdateReference, .btnAddMisc, #FormOldCore').hide();
                if ($(this).val() !== 'Add') {
                    $('.btnUpdateReference').show();
                }
                $('#PartItemCipl, #UnitItemCipl').prop('disabled', false);
                $('#SnItemCipl, #CaseNumItemCipl, #ASNNumItemCipl, #CoOItemCipl, #TypeItemCipl, #YearMadeItemCipl').prop('disabled', true);
                $('#IdItem').val(row.Id);
                $('#NameItemCipl').val(row.Name);
                $('#QuantityItemCipl').val(row.Quantity);
                $('#UomItemCipl').val(row.UnitUom == null ? uomtypes[0].id : uomtypes.find(x => x.text === row.UnitUom).id).trigger('change');
                $('#PartItemCipl').val(row.PartNumber);
                $('#LengthItemCipl').val(row.Length).prop('disabled', false);
                $('#WidthItemCipl').val(row.Width).prop('disabled', false);
                $('#HeightItemCipl').val(row.Height).prop('disabled', false);
                $('#ExtendedItemCipl').val(row.ExtendedValue);
                $('#UnitItemCipl').val(row.UnitPrice);
                $('#VolumeItemCipl').val(row.Volume);
                $('#NetItemCipl').val(row.NetWeight).prop('disabled', false);
                $('#GrossItemCipl').val(row.GrossWeight).prop('disabled', false);
                $('#IdParentItemCipl').val(row.IdParent);
            } else if ($('#idCategoryReference').val() === 'Other') {
                $('.btnAddReference, .btnUpdateReference, .btnAddMisc, #FormOldCore').hide();
                if ($(this).val() !== 'Add') {
                    $('.btnUpdateReference').show();
                }
                $('#SnItemCipl').prop('disabled', true);
                $('#CcrItemCipl, #JcodeItemCipl, #IdCustomerItemCipl').prop('disabled', false);
                $('#PartItemCipl, #UnitItemCipl').prop('disabled', false);
                $('#UomItemCipl').val(row.UnitUom == null ? uomtypes[0].id : uomtypes.find(x => x.text === row.UnitUom).id).trigger('change');
                $('#NameItemCipl').val(row.Name);
                $('#QuantityItemCipl').val(row.Quantity);
                $('#PartItemCipl').val(row.PartNumber);
                $('#LengthItemCipl').val(row.Length).prop('disabled', false);
                $('#WidthItemCipl').val(row.Width).prop('disabled', false);
                $('#HeightItemCipl').val(row.Height).prop('disabled', false);
                $('#ExtendedItemCipl').val(row.ExtendedValue);
                $('#UnitItemCipl').val(row.UnitPrice);
                $('#VolumeItemCipl').val(row.Volume);
                $('#NetItemCipl').val(row.NetWeight).prop('disabled', false);
                $('#GrossItemCipl').val(row.GrossWeight).prop('disabled', false);
                $('#IdParentItemCipl').val(row.IdParent);
                $('#JcodeItemCipl').val(row.JCode);
                $('#CcrItemCipl').val(row.Ccr);
                $('#CaseNumItemCipl').val(row.CaseNumber);
                $('#ASNNumItemCipl').val(row.ASNNumber);
                $('#CoOItemCipl').val(row.CoO);
                $('#YearMadeItemCipl').val(row.YearMade);
                $('#IdNoItemCipl').val(row.IdNo);
                $('#IdCustomerItemCipl').val(row.IdCustomer);
                $('#TypeItemCipl').val(row.Type);
            } else {
                CiplItemGetByIdReference(row.IdReference);
                $('.btnAddReference, .btnUpdateReference, .btnAddMisc, #FormOldCore').hide();
                $('#UnitCaseItemCipl').prop("disabled", true);
                if ($(this).val() === 'Add') {
                    $('.btnAddReference').show();
                } else {
                    $('.btnUpdateReference').show();
                }
                $('#UomItemCipl').val(null).trigger('change');
                $('#IdItem').val(row.Id);
                $('#IdReference').val(row.IdReference);
                $('#IdCustomerItemCipl').val(row.IdCustomer);
                $('#ReferenceItemCipl').val(row.ReferenceNo);
                $('#NameItemCipl').val(row.Name);
                $('#QuantityItemCipl').val(row.Quantity);
                $('#UomItemCipl').val(row.UnitUom == null ? uomtypes[0].id : uomtypes.find(x => x.text === row.UnitUom).id).trigger('change');
                $('#PartItemCipl').val(row.PartNumber);
                $('#SnItemCipl').val(row.Sn);
                $('#JcodeItemCipl').val(row.JCode);
                $('#CcrItemCipl').val(row.Ccr);
                $('#CaseNumItemCipl').val(row.CaseNumber);
                $('#ASNNumItemCipl').val(row.ASNNumber);
                $('#CoOItemCipl').val(row.CoO);
                $('#YearMadeItemCipl').val(row.YearMade);
                $('#IdNoItemCipl').val(row.IdNo);
                $('#CurrencyItemCipl').val(row.Currency);
                $('#TypeItemCipl').val(row.Type);
                $('#LengthItemCipl').val(row.Length).prop('disabled', false);
                $('#WidthItemCipl').val(row.Width).prop('disabled', false);
                $('#HeightItemCipl').val(row.Height).prop('disabled', false);
                $('#ExtendedItemCipl').val(row.ExtendedValue);
                $('#UnitItemCipl').val(row.UnitPrice);
                $('#VolumeItemCipl').val(row.Volume);
                $('#NetItemCipl').val(row.NetWeight).prop('disabled', false);
                $('#GrossItemCipl').val(row.GrossWeight).prop('disabled', false);
                $('#IdParentItemCipl').val(row.IdParent);
                $('#btnUpdateReference').val(index);
                $('#SIBNumberItemCipl').val(row.SibNumber);
                $('#WONumberItemCipl').val(row.WoNumber);
                $('#ClaimItemCipl').val(row.Claim);
                if (row.IdParent > 0) {
                    $('#QuantityItemCipl, #UomItemCipl, #CaseNumItemCipl, #ASNNumItemCipl, #CoOItemCipl, #TypeItemCipl').prop('disabled', true);
                }
                //event.stopPropagation();
            }

        },
        'click .DeleteReferenceItem': function (e, value, row, index) {
            CiplItemDeleteById(row.Id);
            get_ciplitem_by_id();
            var Data = new Array();
            Data.push({ 'Id': row.IdReference, 'Quantity': row.Quantity });
            UpdateQuantityReference(Data, 'Delete');
            SumReferenceItem();
        },
        'click .SplitReferenceItem': function (e, value, row, index) {
            var Data = new Array();
            Data.push({ 'IdCipl': row.IdCipl, 'Id': row.IdReference, 'IdParent': row.Id })
            CiplItemSplitById(Data);
        },
        'click .edit': function (e, value, row, index) {
            $('#IdDocument').val(row.Id);
            $('#inp-doc-date').val(row.DocumentDate);
            $('#DocumentName').val(row.DocumentName);
            $('.modal-title-document').text("Update Document");
            $('.btnAddDocument').text("Update");
            
        },
        'click .remove': function (e, value, row, index) {
            CiplDocumentDeleteById(row.Id);
            get_cipldocument_by_id();
        },
        'click .upload': function (e, value, row, index) {
            $('#IdDocumentUpload').val(row.Id);
            //$(".uploadRecord").attr('href', '/EMCS/CiplDocumentUpload/' + row.Id).trigger('click');
        }
    };

    $('.modal').on('hidden.bs.modal', function (e) {
        $(this).find("input,textarea").val(null);
        $(this).find("select").val(null).trigger("change");
    });

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
                return Category === 'PRA' || Category === 'REMAN'  ? row.Volume === '0.000000' ? '' : row.Volume : row.Volume;
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

    var unit = [
        [
            {
                field: "action",
                title: "Action",
                rowspan: 2,
                align: 'center',
                events: window.operateEvents,
                class: "text-nowrap",
                formatter: function (value, row, index) {
                    var button;
                    if (row.IdParent > 0) {
                        button = "<button class='btn btn-default btn-xs EditReferenceItem' type='button' data-toggle='modal' data-target='#ModalUpdateReference' value='Edit' title='Edit'><i class='fa fa-pencil'></i></button> <button class='btn btn-danger btn-xs DeleteReferenceItem' type='button' title='Delete'><i class='fa fa-trash'></i></button>";
                    } else {
                        button = "<button class='btn btn-default btn-xs EditReferenceItem' type='button' data-toggle='modal' data-target='#ModalUpdateReference' value='Edit' title='Edit'><i class='fa fa-pencil'></i></button> <button class='btn btn-danger btn-xs DeleteReferenceItem' type='button' title='Delete'><i class='fa fa-trash'></i></button> <button class='btn btn-warning btn-xs SplitReferenceItem' type='button' title='Split / Cut'><i class='fa fa-scissors'></i></button>";
                    }
                    return button;
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
                field: "CoO",
                title: "Country Of Origin",
                rowspan: 2,
                align: 'center',
                sortable: true
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
                field: "PartNumber",
                title: "Part Number",
                rowspan: 2,
                sortable: true,
                align: 'center',
                visible: false
            }, {
                field: "Sn",
                title: "SN",
                rowspan: 2,
                align: 'center',
                sortable: true,
                class: "text-nowrap"
            }, {
                field: "JCode",
                title: "J-Code",
                rowspan: 2,
                align: 'center',
                sortable: true,
                visible: false
            }, {
                field: "Ccr",
                title: "CCR",
                rowspan: 2,
                align: 'center',
                sortable: true,
                visible: false
            }, {
                field: "CaseNumber",
                title: "Case Number",
                rowspan: 2,
                align: 'center',
                sortable: true,
                visible: false
            }, {
                field: "Type",
                title: "Type",
                rowspan: 2,
                align: 'center',
                sortable: true,
                filterControl: true
            }, {
                field: "IdNo",
                title: "Id No",
                rowspan: 2,
                align: 'center',
                sortable: true
            }, {
                field: "YearMade",
                title: "Year Made",
                rowspan: 2,
                align: 'center',
                sortable: true
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
                filterControl: true
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
            }],
        [{
            field: "Length",
            title: "Length",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "Width",
            title: "Width",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "Height",
            title: "Height",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "Volume",
            title: "(m3)",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "NetWeight",
            title: "in KGa",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "GrossWeight",
            title: "in KGa",
            sortable: true,
            align: 'right',
            filterControl: true
        }
        ]
    ]

    $tableunit.bootstrapTable({
        columns: unit,
        cache: false,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbarUnit',
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
        }
    });

    var misc = [
        [
            {
                field: "action",
                title: "Action",
                rowspan: 2,
                align: 'center',
                events: window.operateEvents,
                class: "text-nowrap",
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
                sortable: true,
                visible: false
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
                sortable: true,
                visible: false
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
                sortable: true,
                visible: false
            }, {
                field: "JCode",
                title: "J-Code",
                rowspan: 2,
                align: 'center',
                sortable: true,
                visible: false
            }, {
                field: "Ccr",
                title: "CCR",
                rowspan: 2,
                align: 'center',
                sortable: true,
                visible: false
            }, {
                field: "CaseNumber",
                title: "Case Number",
                rowspan: 2,
                align: 'center',
                sortable: true,
                visible: false
            }, {
                field: "Type",
                title: "Type",
                rowspan: 2,
                align: 'center',
                sortable: true,
                filterControl: true,
                visible: false
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
            }],
        [{
            field: "Length",
            title: "Length",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "Width",
            title: "Width",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "Height",
            title: "Height",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "Volume",
            title: "(m3)",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "NetWeight",
            title: "in KGa",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "GrossWeight",
            title: "in KGa",
            sortable: true,
            align: 'right',
            filterControl: true
        }
        ]
    ]

    $tablemisc.bootstrapTable({
        columns: misc,
        cache: false,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbarMisc',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        pagination: true,
        sidePagination: 'client',
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
    });

    $tableemail.bootstrapTable({
        columns: misc,
        cache: false,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbarEmail',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        pagination: true,
        sidePagination: 'client',
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
    });

    var columnDocument = [
    {
        field: '',
        title: 'Action',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return operateFormatter();
        },
        events: operateEvents
    },
    {
        field: 'ID',
        title: 'No',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        visible: false
    },
    {
        field: 'DocumentDate',
        title: 'Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'DocumentName',
        title: 'Document Name',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Filename',
        title: 'Attachment',
        align: 'center',
        valign: 'center',
        halign: "center",
        class: 'text-nowrap',
        sortable: true,
        events: operateEventRight,
        formatter: function (data, row) {
            if (row.Filename !== "") {
                var btnDownload = "<button class='btn btn-xs btn-success download' type='button'><i class='fa fa-download'></i></button>";
                var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocument' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='fa fa-file-pdf-o'></i></button>";
                return [btnDownload, btnPreview].join(' ');
            } else {
                return "-";
            }
        },
        class: 'text-nowrap'
    }];

    function operateFormatter() {
        var btn = [];
        btn.push('<div class="btn-toolbar row">');
        btn.push('<button type="button" class="btn btn-info btn-xs edit" data-toggle="modal" data-target="#myModalDocument" title="Edit"><i class="fa fa-edit"></i></button>\
            <button type="button" class="btn btn-primary btn-xs upload" data-toggle="modal" data-target="#myModalUploadPlace" title="Upload"><i class="fa fa-upload"></i></button>');
        btn.push('<button type="button" class="btn btn-danger btn-xs remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
        btn.push('</div>');
        return btn.join('');
    }

    $tableFormDocuments.bootstrapTable({
        columns: columnDocument,
        cache: false,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbarDocument',
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

    Dropzone.autoDiscover = false;
}



$('.cal-doc-date').click(function () {
    $('#inp-doc-date').focus();
});

window.operateEventRight = {
    'click .download': function (e, value, row) {
        location.href = "/EMCS/DownloadCiplDocument/" + row.Id;
    },
    'click .showDocument': function (e, value, row) {
        document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/Cipl/" + row.Id + '/' + row.Filename;
    }
};

function ValidateAddReference() {
    var Category = GetCategoryUsed();
    var table = $tablereference.bootstrapTable('getSelections');

    if (table.length > 0) {
        if (Category === 'PRA') {
            if ($('#PackagingTypeParts').valid() && $('#CaseNumberParts').valid()) {
                return true;
            } else {
                Swal.fire({
                    type: 'warning',
                    title: 'Warning'
                    , text: 'Please, Complete All Form Before Add New Reference',
                })
                return false;
            }

        } else if (Category === 'REMAN') {
            var CaseNumber = 0;
            CaseNumber = removeSingleAttributeDuplicates(table, 'CaseNumber').length;
            if ($('#PackagingTypeParts').valid()) {
                if (CaseNumber > 1) {
                    Swal.fire({
                        type: 'warning',
                        title: 'Warning'
                        , text: 'Please, choose the same case number',
                    })
                    return false;
                } else {
                    return true;
                }

            } else {
                Swal.fire({
                    type: 'warning',
                    title: 'Warning'
                    , text: 'Please, Complete All Form Before Add New Reference',
                })
                return false;
            }
        } else {
            return true
        }
    } else {
        Swal.fire({
            type: 'warning',
            title: 'Warning'
            , text: 'Please Select The Item First ',
        })
    }
}
// ====================== TABLE ======================
$('#btnAddReference').on('click', function (e) {
    e.stopPropagation();
    var UnitName = $('#NameItemCipl').valid();
    var Unit = ValidateAddReference();
    if (Unit) {
        debugger;
        if ($('#idCipl').val() === null || $('#idCipl').val() === '' || $('#idCipl').val() === undefined) {
            post_insert_cipl('Draft');
        } else {
            post_insert_cipl_item($('#idCipl').val());
        }
        var data = $tablereference.bootstrapTable('getSelections');
        UpdateQuantityReference(data, 'Insert');
        get_ciplitem_by_id();
        SumReferenceItem();
        $('#ModalAddNewReference').modal('hide');
    }
    //if (UnitName) {
    //if ($('#PackagingTypeParts').valid() && $('#CaseNumberParts').valid()) {

    //}
    //if ($('#idCipl').val() === null || $('#idCipl').val() === '' || $('#idCipl').val() === undefined) {
    //    post_insert_cipl('Draft');
    //} else {
    //    post_insert_cipl_item($('#idCipl').val());
    //}
    //var data = $tablereference.bootstrapTable('getSelections');
    //UpdateQuantityReference(data, 'Insert');
    //get_ciplitem_by_id();
    //SumReferenceItem();
    //}


})
$('#btnAddMisc').on('click', function () {
    var UnitName = $('#NameItemCipl').valid();
    if (UnitName) {
        if ($('#idCipl').val() === null || $('#idCipl').val() === '' || $('#idCipl').val() === undefined) {
            post_insert_cipl('Draft');
        } else {
            post_insert_cipl_item($('#idCipl').val());
        }
        get_ciplitem_by_id();
        SumReferenceItem();
    }

})

function GetCategoryUsed() {
    var Category;
    $('#FormOldCore').hide();
    if ($('#jenisBarangCipl').val() === 'CATERPILLAR NEW EQUIPMENT') {
        Category = "PP";
    } else if ($('#jenisBarangCipl').val() === 'CATERPILLAR SPAREPARTS') {
        if ($('#sparepartsCipl').val() === 'PRA') {
            Category = "PRA";
            $('#FormOldCore, #div-CaseNumberParts').show();
            $('#div-SnItemCipl').hide();
        } else if ($('#sparepartsCipl').val() === 'Old Core') {
            Category = "REMAN";
            $('#FormOldCore, #div-SnItemCipl, #div-WidthParts, #div-HeightParts, #div-VolumeParts, #div-GrossWeightParts, #div-NetWeightParts').show();
            $('#div-CaseNumberParts').hide();
        } else if ($('#sparepartsCipl').val() === 'SIB') {
            Category = "SIB";
        }

    } else if ($('#jenisBarangCipl').val() === 'CATERPILLAR USED EQUIPMENT') {
        Category = "UE";
    } else {
        Category = "MISCELLANEOUS";
    }
    return Category;
}

function ajaxInsertUpdateDocument() {
    var json = new Array();

    json.push({
        Id: $('#IdDocument').val(),
        IdCipl: $('#idCipl').val(),
        DocumentDate: $('#inp-doc-date').val(),
        DocumentName: $('#DocumentName').val()
    });
    $.ajax({
        url: '/EMCS/CiplDocumentInsert',
        type: 'POST',
        data: {
            data: json
        },
        cache: false,
        async: false,
        success: function (data, response) {
            Swal.fire({
                type: 'success',
                title: 'Success'
                , text: 'Saved, Your Data Has Been Saved',
            })
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

function ajaxInsertUpdateReference() {
    var json = new Array();
    var Quantity = parseInt($('#MaxQuantityItemCipl').val()) - parseInt($('#QuantityItemCipl').val());
    json.push({
        Id: $('#IdItem').val(),
        IdItem: $('#IdItem').val(),
        IdCipl: $('#idCipl').val(),
        IdReference: $('#IdReference').val(),
        IdParent: $('#IdParentItemCipl').val(),
        ReferenceNo: $('#ReferenceItemCipl').val(),
        IdCustomer: $('#IdCustomerItemCipl').val(),
        Name: $('#NameItemCipl').val(),
        UnitUom: $('#UomItemCipl').val(),
        PartNumber: $('#PartItemCipl').val(),
        Sn: $('#SnItemCipl').val(),
        JCode: $('#JcodeItemCipl').val(),
        Ccr: $('#CcrItemCipl').val(),
        CaseNumber: $('#CaseNumItemCipl').val(),
        ASNNumber: $('#ASNNumItemCipl').val(),
        CoO: $('#CoOItemCipl').val(),
        Type: $('#TypeItemCipl').val(),
        IdNo: $('#IdNoItemCipl').val(),
        YearMade: $('#YearMadeItemCipl').val(),
        Quantity: $('#QuantityItemCipl').val(),
        UnitPrice: removeformatCurrency($('#UnitItemCipl').val()),
        ExtendedValue: removeformatCurrency($('#ExtendedItemCipl').val()),
        Length: removeformatCurrency($('#LengthItemCipl').val()),
        Width: removeformatCurrency($('#WidthItemCipl').val()),
        Height: removeformatCurrency($('#HeightItemCipl').val()),
        Volume: removeformatCurrency($('#VolumeItemCipl').val()),
        GrossWeight: removeformatCurrency($('#GrossItemCipl').val()),
        NetWeight: removeformatCurrency($('#NetItemCipl').val()),
        Currency: $('#currencyCipl').val(),
        AvailableQuantity: parseInt($('#QuantityItemCipl').val()),
        SibNumber: $('#SIBNumberItemCipl').val(),
        WoNumber: $('#WONumberItemCipl').val(),
        Claim: $('#ClaimItemCipl').val()
    });
    $.ajax({
        url: '/EMCS/CiplItemInsert',
        type: 'POST',
        data: {
            data: json,
            IdCipl: $('#idCipl').val()
        },
        cache: false,
        async: false,
        success: function (data, response) {
            var Data = new Array();
            var Quantity = parseInt($('#MaxQuantityItemCipl').val()) - parseInt($('#QuantityItemCipl').val());
            Data.push({ 'Id': $('#IdReference').val(), 'Quantity': Quantity });
            UpdateQuantityReference(Data, 'Update');
            Swal.fire({
                type: 'success',
                title: 'Success'
                , text: 'Saved, Your Data Has Been Saved',
            })
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
function CiplItemGetByIdReference(IdReference) {
    $.ajax({
        url: '/EMCS/CiplItemGetByIdReference',
        type: 'POST',
        data: {
            IdReference: IdReference,
        },
        cache: false,
        async: false,
        success: function (data, response) {
            $('#MaxQuantityItemCipl').val(data.Quantity);
            $('#AvailableQuantityItemCipl').val(data.AvailableQuantity);
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
function GetReferenceItem() {
    $tablereference.bootstrapTable('refresh');
    var reference = [
        [
            {
                field: "state",
                checkbox: true,
                rowspan: 2,
                align: 'center',
                formatter: function (value, row, index) {
                    if (row.AvailableQuantity === 0) {
                        return {
                            disabled: true
                        }
                    }
                    return value;
                }
            }, {
                field: "Id",
                title: "Id Item",
                rowspan: 2,
                align: 'center',
                sortable: true,
                visible: false
            }, {
                field: "IdItem",
                title: "Id Item",
                rowspan: 2,
                align: 'center',
                sortable: true,
                visible: false
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
                title: "Name Of Goods",
                rowspan: 2,
                align: 'center',
                sortable: true,
                class: 'text-nowrap',
                formatter: function (value, row, index) {
                    return row.UnitName;
                }
            }, {
                field: "CoO",
                title: "Country Of Origin",
                rowspan: 2,
                align: 'center',
                sortable: true
            }, {
                field: "UnitUom",
                title: "UOM",
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
                sortable: true,
                formatter: function (value, row, index) {
                    return row.UnitSn;
                }
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
                filterControl: true,
                visible: false
            }, {
                field: "IdNo",
                title: "Id No",
                rowspan: 2,
                align: 'center',
                sortable: true
            }, {
                field: "YearMade",
                title: "Year Made",
                rowspan: 2,
                align: 'center',
                sortable: true
            }, {
                field: "AvailableQuantity",
                title: "Quantity",
                rowspan: 2,
                align: 'center',
                sortable: true
            }, {
                field: "Quantity",
                title: "Quantity From SPA",
                rowspan: 2,
                align: 'center',
                sortable: true,
                visible: false
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
                filterControl: true
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
            filterControl: true
        }, {
            field: "Width",
            title: "Width",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "Height",
            title: "Height",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "Volume",
            title: "(m3)",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "NetWeight",
            title: "in KGa",
            sortable: true,
            align: 'right',
            filterControl: true
        }, {
            field: "GrossWeight",
            title: "in KGa",
            sortable: true,
            align: 'right',
            filterControl: true
        }
        ]
    ]

    $tablereference.bootstrapTable({
        url: '/EMCS/GetReferenceItem',
        columns: reference,
        checkboxHeader: true,
        queryParams: function (params) {
            var Category = GetCategoryUsed();
            params.Category = Category;
            params.Column = $('#Column').val() === '' || $('#Column').val() === null ? $('#idCategoryReference').val() : $('#Column').val();
            params.ColumnValue = $('#ColumnValue').val() === '' || $('#ColumnValue').val() === null ? $('#refCipl').val() === null ? "" : $('#refCipl').val().toString() : $('#ColumnValue').val();
            return params;
        },
        cache: false,
        UniqueId: "Id",
        pagination: true,
        search: true,        
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '#toolbar-reference',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'client',
        showColumns: true,
        responseHandler: function (resp) {
            var data = {};
            $.map(resp, function (value, key) {
                data[value.Key] = value.Value;
            });
            return data;
        },
        showRefresh: true,
        smartDisplay: true,
        pageSize: '10',
        pageList: [25, 50, 75, 100],
        responseHandler: function (resp) {
            var data = {};
            $.map(resp, function (value, key) {
                data[value.Key] = value.Value;
            });
            return data.rows;
        },
        formatNoMatches: function () {
            return '<span class="noMatches">No item found</span>';
        }
    });

    //Category === 'REMAN' ? $tablereference.bootstrapTable('hideColumn', 'JCode') : $tablereference.bootstrapTable('showColumn', 'JCode')
    var Category = GetCategoryUsed();
    if (Category === 'SIB' || Category === 'REMAN') {
        $tablereference.bootstrapTable('hideColumn', 'JCode');
        $tablereference.bootstrapTable('showColumn', 'Claim');
        $tablereference.bootstrapTable('showColumn', 'SibNumber');
        $tablereference.bootstrapTable('showColumn', 'WoNumber');
    } else {
        $tablereference.bootstrapTable('showColumn', 'JCode')
        $tablereference.bootstrapTable('hideColumn', 'Claim');
        $tablereference.bootstrapTable('hideColumn', 'SibNumber');
        $tablereference.bootstrapTable('hideColumn', 'WoNumber');
    }

}
//CiplDocumentDeleteById
function CiplDocumentDeleteById(id) {
    $.ajax({
        url: '/EMCS/CiplDocumentDeleteById',
        type: 'POST',
        data: {
            id: id,
        },
        cache: false,
        async: false,
        success: function (data, response) {
            Swal.fire({
                type: 'success',
                title: 'Success'
                , text: 'Success, Your Data Has Been Delete',
            })
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

function CiplItemDeleteById(id) {
    $.ajax({
        url: '/EMCS/CiplItemDeleteById',
        type: 'POST',
        data: {
            id: id,
        },
        cache: false,
        async: false,
        success: function (data, response) {
            Swal.fire({
                type: 'success',
                title: 'Success'
                , text: 'Success, Your Data Has Been Delete',
            })
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
function CiplItemSplitById(data) {
    $.ajax({
        url: '/EMCS/CiplItemInsert',
        type: 'POST',
        data: {
            data: data
        },
        cache: false,
        async: false,
        success: function (data, response) {
            Swal.fire({
                type: 'success',
                title: 'Success'
                , text: 'Saved, Split Data Has Been Saved',
            })
            get_ciplitem_by_id();
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
function removeSingleAttributeDuplicates(array, key) {
    let lookup = {};
    let result = [];
    for (let i = 0; i < array.length; i++) {
        if (!lookup[array[i][key]]) {
            lookup[array[i][key]] = true;
            result.push(array[i]);
        }
    }
    return result;
}
function removeSingleAttributeDuplicatesNew(array, key,key2) {
    let lookup = {};
    let result = [];
    for (let i = 0; i < array.length; i++) {
        if (!lookup[array[i][key]] && !lookup[array[i][key2]]) {
            lookup[array[i][key]] = true;
            lookup[array[i][key2]] = true;
            result.push(array[i]);
        }
    }
    return result;
}
function removeDoubleAttributeDuplicates(array) {
    jsonObject = array.map(JSON.stringify);
    uniqueSet = new Set(jsonObject);
    uniqueArray = Array.from(uniqueSet).map(JSON.parse);
    return uniqueArray;
}
function getUniqueNumberOfCollies(array) {
    result = array.filter(function (a) {
        var key = a.CaseNumber + '|' + a.Type;
        if (!this[key]) {
            this[key] = true;
            return true;
        }
    }, Object.create(null));

    return result;
}
function SumReferenceItem() {
    var SumGross = 0;
    var SumNet = 0;
    var SumQuantity = 0;
    var SumCollies = 0;
    var SumVolume = 0;

    var table = get_used_table_cipl_item();
    var data = selectedCiplCategory(table);
    var Category = GetCategoryUsed();

    var SumCollie = 0;
    if (Category === 'SIB') {
        var CountJCode = 0;
        $.map(data, function (elm, idx) {
            CountJCode = removeSingleAttributeDuplicates(data, 'JCode').length + removeSingleAttributeDuplicates(data, 'Type').length;
            SumGross = SumGross + removeformatCurrency(elm.GrossWeight);
            SumNet = SumNet + removeformatCurrency(elm.NetWeight);
            SumQuantity = SumQuantity + parseInt(elm.Quantity);

            SumVolume = SumVolume + removeformatCurrency(elm.Volume);
        })
        SumCollie = CountJCode;
    } else if (Category === 'PRA' || Category === 'REMAN') {
        var CountJCode = 0;
        $.map(data, function (elm, idx) {
            CountJCode = getUniqueNumberOfCollies(data).length;;
            SumGross = SumGross + removeformatCurrency(elm.GrossWeight);
            SumNet = SumNet + removeformatCurrency(elm.NetWeight);
            SumQuantity = SumQuantity + parseInt(elm.Quantity);

            SumVolume = SumVolume + removeformatCurrency(elm.Volume);
        })
        SumCollie = CountJCode;
    } else {
        var array = new Array();
        $.map(data, function (elm, idx) {
            if (elm.Sn != "")
            {
                array.push({ 'Name': elm.Name, 'Sn': elm.Sn });
            }            
            SumGross = SumGross + removeformatCurrency(elm.GrossWeight);
            SumNet = SumNet + removeformatCurrency(elm.NetWeight);
            SumQuantity = SumQuantity + parseInt(elm.Quantity);

            SumVolume = SumVolume + removeformatCurrency(elm.Volume);
        })
        var CountDuplicate = removeDoubleAttributeDuplicates(array).length;
        SumCollie = CountDuplicate;
    }

    if (Category === 'PP' || Category === 'UE') {
        SumCollie = '-';
    }

    $('#grossCipl').val(formatCurrency(SumGross, ".", ",", 2));
    $('#weightCipl').val(formatCurrency(SumNet, ".", ",", 2));
    $('#quantityCipl').val(SumQuantity);
    $('#colliesCipl').val(SumCollie);
    $('#volumeCipl').val(formatCurrency(SumVolume, ".", ",", 6));
}

function selectedCiplCategory(table) {
    return $.map(table.bootstrapTable('getData'), function (row) {
        return row
    })
}

$tablereference.on('check.bs.table uncheck.bs.table ', function () {
    SumReferenceItem();
})
$("#btnAddDocument").on('click', function () {
    var aaa = $('#idCipl').val();
    if ($('#idCipl').val() == null || $('#idCipl').val() == 0) {
        Swal.fire({
            type: 'error',
            title: 'Oops...',
            html: 'CIPL ID is not found, please save as draft before add documents',
        });
        return false;
    } else {
        ajaxInsertUpdateDocument();
        get_cipldocument_by_id();
    }
    
})

$("#btnUpdateReference").on('click', function () {
    if (removeformatCurrency($('#NetItemCipl').val()) > removeformatCurrency($('#GrossItemCipl').val())) {
        Swal.fire({
            type: 'error',
            title: 'Oops...',
            html: 'Net Weight cannot be greater than Gross Weight',
        });
        return false;
    }
    if ($('#jenisBarangCipl').val() === 'MISCELLANEOUS' || $('#idCategoryReference').val() === 'Other') {
        ajaxInsertUpdateReference();
        get_ciplitem_by_id();
        SumReferenceItem();
    } else {
        var table = get_used_table_cipl_item();
        var tabledata = table.bootstrapTable('getData');
        var CountQuantity = 0;
        var IdReferenceItem = parseInt($('#IdReference').val());

        for (var i = 0; i < tabledata.length; i++) {
            if (tabledata[i].IdReference === IdReferenceItem) {
                CountQuantity = CountQuantity + parseInt(tabledata[i].Quantity);
            }
        }
        var TotalQuantity = CountQuantity + parseInt($('#QuantityItemCipl').val());

        if (parseInt($('#QuantityItemCipl').val()) > parseInt($('#MaxQuantityItemCipl').val())) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                html: 'The quantity is greater than the specified quantity. <br> The maximum number of quantities for this item with ID ' + $('#IdReference').val() + ' is <b>' + $('#MaxQuantityItemCipl').val() + '</b>',
            })
            return false;
        } else {
            ajaxInsertUpdateReference();
            get_ciplitem_by_id();
            SumReferenceItem();
        }

    }

})

$('.SumVolumenOldCore').keyup(function () {
    var length = removeformatCurrency($('#LengthParts').val());
    var width = removeformatCurrency($('#WidthParts').val());
    var height = removeformatCurrency($('#HeightParts').val());
    var volume = (length * width * height) / 1000000;
    $('#VolumeParts').val(formatCurrency(volume, ".", ",", 6));
})
$('.SumVolume').keyup(function () {
    var length = removeformatCurrency($('#LengthItemCipl').val());
    var width = removeformatCurrency($('#WidthItemCipl').val());
    var height = removeformatCurrency($('#HeightItemCipl').val());
    var volume = (length * width * height) / 1000000;
    $('#VolumeItemCipl').val(formatCurrency(volume, ".", ",", 6));
})
$('#QuantityItemCipl, #UnitItemCipl').keyup(function () {
    var UnitPrice = removeformatCurrency($('#UnitItemCipl').val());
    var Quantity = parseInt($('#QuantityItemCipl').val());
    var ExtendedValue = UnitPrice * Quantity;
    $('#ExtendedItemCipl').val(formatCurrency(ExtendedValue, ".", ",", 2));
})
function GetDestinationPort() {
    var country = $("#countryCipl").val();
   
        $('#destinationCipl').select2({
            placeholder: "Select Destination Port",
            ajax: {
                url: "/emcs/GetPortData",
                dataType: 'json',
                data: function (params) {

                    return {
                        Country: country,
                        Name: params.term

                    };
                },
                success: function (data, response) {
                },
                processResults: function (data) {
                    return {
                        results: $.map(data.data, function (item) {
                            return {
                                text: item.Text,
                                id: item.Id
                            }
                        })
                    }
                }
            }
        })

    
}
function GetReferenceNo() {
    var Category = GetCategoryUsed();
    var item = {
        Category: Category,
        ReferenceNo: $('#refCipl').val()
    } 

    $('#refCipl').select2({
        minimumInputLength: 4,
        placeholder: "Select Reference No",
        ajax: {
            url: "/emcs/GetReference",
            data: function (params) {
                var lastReference = $("#refCipl").val();
                var idCustomer = $("#idCustomerCipl").val();

                if (lastReference) {
                    lastReference = lastReference.join(',');
                }
                return {
                    Category: Category,
                    ReferenceNo: params.term,
                    LastReference: lastReference,
                    IdCustomer: idCustomer,
                    CategoryReference: $('#idCategoryReference').val()
                };
            },
            method: 'POST',
            success: function (data, response) {
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.ReferenceNo,
                            id: item.ReferenceNo
                        }
                    })
                }
            }
        }
    })

    $('#idPickupPic').select2({
        placeholder: "Select Pick Up User",
        ajax: {
            url: "/emcs/GetPickUpPic",
            data: function (params) {
                return {
                    User: params.term //,
                    //Area: $('#idPickupArea').val()
                    //Area: $('#idPickupArea').val().substr($('#idPickupArea').val().length - 3)
                };
            },
            method: 'POST',
            success: function (data, response) {
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.Text + ' - ' + item.Extra,
                            id: item.Id
                        }
                    })
                }
            }
        }
    })

    $('#jenisBarangCipl, #refCipl, #exportCipl').on('select2:select', function (e) {
        var table = get_used_table_cipl_item().bootstrapTable('getData');
        if (table.length > 0) {
            CiplItemCancel();
        }
    });

    $('#loadingCipl').select2({
        placeholder: "Select Loading Port",
        ajax: {
            url: "/emcs/GetLocalPortData",
            dataType: 'json',
            data: function (params) {
               
                return {
                   
                    Name: params.term

                };
            },
            success: function (data, response) {
            },
            processResults: function (data) {
                return {
                    results: $.map(data.data, function (item) {
                        return {
                            text: item.Text,
                            id: item.Id
                        }
                    })
                }
            }
        }
    })

    
    
    $('#CkbBranchCipl').select2({
        placeholder: 'Select CKB Branch',
        width: "100%",
        ajax: {
            url: "/EMCS/GetBranchCkb",
            type: "get",
            data: function (params) {
                var query = {
                    searchName: params.term
                }

                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.data, function (item) {
                        return {
                            text: item.Name,
                            id: item.Name,
                            city: item.City,
                            area: item.Area,
                            address: item.Address,
                            postalcode: item.PostalCode,
                            phone: item.PhoneNumber,
                            fax: item.FaxNumber
                        }
                    })
                }
            }
        }
    }).on('select2:select', function (event) {
        $('#forwaderAddressCipl').val(event.params.data.address);
        $('#forwaderCityCipl').val(event.params.data.city);
        $('#forwaderAreaCipl').val(event.params.data.area);
        $('#forwaderPostalCodeCipl').val(event.params.data.postalcode);
        $('#forwaderContactCipl').val(event.params.data.phone);
        $('#forwaderFaxCipl').val(event.params.data.fax);
    })

}

function get_used_table_cipl_item() {
    var table;
    if ($('#jenisBarangCipl').val() === 'CATERPILLAR SPAREPARTS' && $('#idCategoryReference').val() !== 'Other') {
        table = $tablepart;
    } else if ($('#jenisBarangCipl').val() === 'MISCELLANEOUS') {
        table = $tablemisc;
    }
    
    else if (($('#jenisBarangCipl').val() === 'CATERPILLAR NEW EQUIPMENT' || $('#jenisBarangCipl').val() === 'CATERPILLAR USED EQUIPMENT') && $('#idCategoryReference').val() !== 'Other') {
        table = $tableunit;
    } else if ($('#idCategoryReference').val() === 'Other') {
        table = $tableemail;
    }
    return table;
}


function UpdateQuantityReference(Data, Status) {
    var Item = Data;
    $.ajax({
        url: '/EMCS/UpdateQuantityReference',
        type: 'POST',
        data: {
            Item: Item,
            Status: Status
        },
        cache: false,
        async: false,
        success: function (data, response) {
            return 'OK';
        },
        error: function (data, response) {
            console.log(data, response);
        }
    })
}

function CiplItemCancel() {
    $.ajax({
        url: '/EMCS/CiplItemCancel',
        type: 'POST',
        data: {
            IdCipl: $('#idCipl').val()
        },
        cache: false,
        async: false,
        success: function (data, response) {
            $tablepart.bootstrapTable('removeAll');
            $tablemisc.bootstrapTable('removeAll');
            $tableunit.bootstrapTable('removeAll');
            $tablereference.bootstrapTable('removeAll');
        },
        error: function (data, response) {
            console.log(data, response);
        }
    })
}

function Select2Parameter(data) {
    var array = new Array();
    $.each(data, function (index, element) {
        array.push({ 'id': element.Name, 'text': element.Name, 'desc': element.Description });
    });
    return array;
}

$("#FormCipl").validate({
    onkeyup: false,
    errorClass: "input-validation-error",
    //ignore: ':hidden:not(.do-not-ignore)',

    errorPlacement: function (error, element) {
        if (element.hasClass("select2") && element.hasClass("input-validation-error")) {
            element.next("span").addClass("input-validation-error");
        }
        else if (element.hasClass("select2-hidden-accessible")) {
            error.insertAfter(element.parent('span.select2'));
        }
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass(errorClass); //.removeClass(errorClass);
        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
    },
    //When removing make the same adjustments as when adding
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass(errorClass); //.addClass(validClass);
        $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
        $(element).next("span").removeClass("input-validation-error");
    },


});

function CiplItemConvert(data) {
    var array = new Array();
    $.each(data, function (index, element) {
        var arraydata = {
            ReferenceNo: element.ReferenceNo,
            Currency: element.Currency,
            Id: element.Id,
            IdCustomer: element.IdCustomer,
            IdCipl: element.IdCipl,
            IdReference: element.IdReference,
            Name: element.Name,
            Quantity: element.Quantity,
            UnitUom: element.UnitUom,
            Sn: element.Sn,
            Ccr: element.Ccr,
            IdNo: element.IdNo,
            YearMade: element.YearMade,
            CaseNumber: element.CaseNumber,
            PartNumber: element.PartNumber,
            JCode: element.JCode,
            Type: element.Type,
            UnitPrice: formatCurrency(element.UnitPrice, ".", ",", 2),
            ExtendedValue: formatCurrency(element.ExtendedValue, ".", ",", 2),
            Length: formatCurrency(element.Length, ".", ",", 2),
            Width: formatCurrency(element.Width, ".", ",", 2),
            Height: formatCurrency(element.Height, ".", ",", 2),
            Volume: formatCurrency(element.Volume, ".", ",", 6),
            NetWeight: formatCurrency(element.NetWeight, ".", ",", 2),
            GrossWeight: formatCurrency(element.GrossWeight, ".", ",", 2),
            CoO: element.CoO === '' || element.CoO === null ? $('#CoOItemCipl').val() : element.CoO,
            IdParent: element.IdParent,
            SibNumber: element.SibNumber,
            WoNumber: element.WoNumber,
            Claim: element.Claim,
            ASNNumber: element.ASNNumber
        }
        array.push(arraydata);
    })
    return array;
}

function CiplDocumentConvert(data) {
    var array = new Array();
    $.each(data, function (index, element) {
        var arraydata = {
            Id: element.Id,
            IdCipl: element.IdCipl,
            DocumentDate: moment(element.DocumentDate).format("DD MMM YYYY"),
            DocumentName: element.DocumentName,
            Filename: element.Filename
        }
        array.push(arraydata);
    })
    return array;
}

function ReferenceItemConvert(data) {
    var array = new Array();
    $.each(data, function (index, element) {
        var arraydata = {
            ReferenceNo: element.ReferenceNo,
            Currency: element.Currency,
            Id: element.Id,
            IdCustomer: element.IdCustomer,
            IdCipl: $('#idCipl').val(),
            IdReference: element.Id,
            Name: element.UnitName,
            Quantity: element.Quantity,
            UnitUom: element.UnitUom,
            Sn: element.UnitSn,
            Ccr: element.Ccr,
            IdNo: element.IDNo,
            YearMade: element.YearMade,
            CaseNumber: element.CaseNumber,
            PartNumber: element.PartNumber,
            JCode: element.JCode,
            UnitPrice: element.UnitPrice,
            ExtendedValue: element.ExtendedValue,
            Length: element.Length,
            Width: element.Width,
            Height: element.Height,
            Volume: element.Volume,
            NetWeight: element.NetWeight,
            GrossWeight: element.GrossWeight,
            CoO: element.CoO === '' || element.CoO === null ? $('#CoOItemCipl').val() : element.CoO,
            AvailableQuantity: element.AvailableQuantity,
            IdParent: element.IdParent,
            SibNumber: element.SibNumber,
            WoNumber: element.WoNumber,
            Claim: element.Claim
        }
        array.push(arraydata);
    })
    return array;
}
function AddNewOldCore(data) {
    var array = new Array();
    var category = GetCategoryUsed();
    $.each(data, function (index, element) {
        if (category === 'REMAN') {
            var arraydata = {
                ReferenceNo: element.ReferenceNo,
                Currency: element.Currency,
                Id: element.Id,
                IdCustomer: element.IdCustomer,
                IdCipl: $('#idCipl').val(),
                IdReference: element.Id,
                IdParent: element.IdParent,
                Name: element.UnitName,
                Quantity: element.Quantity,
                UnitUom: element.UnitUom,
                Sn: element.UnitSn,
                Ccr: element.Ccr,
                IdNo: element.IDNo,
                YearMade: element.YearMade,
                PartNumber: element.PartNumber,
                JCode: element.JCode,
                UnitPrice: element.UnitPrice,
                ExtendedValue: element.ExtendedValue,
                CoO: element.CoO === '' || element.CoO === null ? $('#CoOItemCipl').val() : element.CoO,
                AvailableQuantity: element.AvailableQuantity,
                IdParent: element.IdParent,
                SibNumber: element.SibNumber,
                WoNumber: element.WoNumber,
                Claim: element.Claim,
                CaseNumber: element.CaseNumber,
                Type: $('#PackagingTypeParts').val(),
                ASNNumber: $('#ASNNumberParts').val(),
                Length: index === 0 ? removeformatCurrency($('#LengthParts').val()) : 0,
                Width: index === 0 ? removeformatCurrency($('#WidthParts').val()) : 0,
                Height: index === 0 ? removeformatCurrency($('#HeightParts').val()) : 0,
                Volume: index === 0 ? removeformatCurrency($('#VolumeParts').val()) : 0,
                NetWeight: index === 0 ? removeformatCurrency($('#NetWeightParts').val()) : 0,
                GrossWeight: index === 0 ? removeformatCurrency($('#GrossWeightParts').val()) : 0
            }
        } else if (category === 'PRA') {
            var arraydata = {
                ReferenceNo: element.ReferenceNo,
                Currency: element.Currency,
                Id: element.Id,
                IdCustomer: element.IdCustomer,
                IdCipl: $('#idCipl').val(),
                IdReference: element.Id,
                IdParent: element.IdParent,
                Name: element.UnitName,
                Quantity: element.Quantity,
                UnitUom: element.UnitUom,
                Sn: element.UnitSn,
                Ccr: element.Ccr,
                IdNo: element.IDNo,
                YearMade: element.YearMade,
                PartNumber: element.PartNumber,
                JCode: element.JCode,
                UnitPrice: element.UnitPrice,
                ExtendedValue: element.ExtendedValue,
                CoO: element.CoO === '' || element.CoO === null ? $('#CoOItemCipl').val() : element.CoO,
                AvailableQuantity: element.AvailableQuantity,
                IdParent: element.IdParent,
                SibNumber: element.SibNumber,
                WoNumber: element.WoNumber,
                Claim: element.Claim,
                CaseNumber: $('#CaseNumberParts').val(),
                ASNNumber: $('#ASNNumberParts').val(),
                Type: $('#PackagingTypeParts').val(),
                Length: index === 0 ? removeformatCurrency($('#LengthParts').val()) : 0,
                Width: index === 0 ? removeformatCurrency($('#WidthParts').val()) : 0,
                Height: index === 0 ? removeformatCurrency($('#HeightParts').val()) : 0,
                Volume: index === 0 ? removeformatCurrency($('#VolumeParts').val()) : 0,
                NetWeight: index === 0 ? removeformatCurrency($('#NetWeightParts').val()) : 0,
                GrossWeight: index === 0 ? removeformatCurrency($('#GrossWeightParts').val()) : 0
            }
        }
        array.push(arraydata);
    })
    return array;
}
$('#btnSaveCipl').on('click', function () {
    event.preventDefault();
    var table = get_used_table_cipl_item().bootstrapTable('getData');
    var status = $(this).val();
    if (table.length === 0) {
        Swal.fire({
            type: 'error',
            title: 'Oops...',
            text: 'Please Fill Table Sales Order',
        })
        return false;
    } else {
        if ($('#noCipl').val() === null || $('#noCipl').val() === 'null' || $('#noCipl').val() === '') {
            post_insert_cipl(status);
           
        } else {
            post_update_cipl(status);
            if ($('#jenisBarangCipl').val() === 'CATERPILLAR SPAREPARTS') {
                $('#partUploadFile').show();
            }
        }
    }
})


$('#btnSubmitCipl').on('click', function (event) {
    event.preventDefault();
    $('input[name="query"]').each(function () {
        $(this).rules('add', {
            required: true,
            accept: "image/jpeg, image/pjpeg"
        })
    })
    var table = get_used_table_cipl_item().bootstrapTable('getData');
    var status = $(this).val();
    if (table.length === 0) {
        Swal.fire({
            type: 'error',
            title: 'Oops...',
            text: 'Please Fill Table Sales Order',
        })
        return false;
    } else if ($('#idCategoryReference').val() === 'Other' && $tableFormDocuments.bootstrapTable('getData').length === 0) {
        Swal.fire({
            type: 'error',
            title: 'Oops...',
            text: 'Please Fill And Upload Document',
        })
        return false;
    } else {
        var isValid = $("#FormCipl").valid();
        if (isValid) {
            if ($('#noCipl').val() === null || $('#noCipl').val() === 'null' || $('#noCipl').val() === '') {
                post_insert_cipl('Draft');
            } else {
                Swal.fire({
                    title: "Confirmation",
                    text: "By submitting, you are responsible for the authenticity of the documents and data entered. Are you sure you want to process this document?",
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, Submit it!'
                }).then((result) => {
                    if (result.value) {
                        $(this).prop('disabled', true);
                        post_update_cipl(status);
                        Swal.fire({
                            title: 'Submit!',
                            text: 'Data Confirmed Successfully',
                            type: 'success'
                        }).then((result) => {
                            window.location.href = "/EMCS/CiplList";
                        });
                    }
                })
            }
        } else {
            Swal.fire("Attention!", "Please complete all required field in this form before submit!", "error");
        }
    }
})

function post_insert_cipl(status) {
    if ($('#jenisBarangCipl').val() === 'CATERPILLAR SPAREPARTS') {
        var CategoryItem = $('#sparepartsCipl').val();
    } else if ($('#jenisBarangCipl').val() === 'MISCELLANEOUS') {
        var CategoryItem = $('#permanentCipl').val();
    } else if ($('#jenisBarangCipl').val() === 'CATERPILLAR NEW EQUIPMENT' || $('#jenisBarangCipl').val() === 'CATERPILLAR USED EQUIPMENT') {
        var CategoryItem = $('#unitCipl').val();
    }

    var item = {
        data: {
            Id: $('#idCipl').val(),
            CiplNo: $('#noCipl').val(),
            dateCipl: $('#dateCipl').val(),
            Category: $('#jenisBarangCipl').val(),
            ReferenceNo: $('#refCipl').val() === null ? "" : $('#refCipl').val().toString(),
            CategoriItem: CategoryItem,
            ExportType: $('#exportCipl').val(),
            ExportTypeItem: $('#exportremarksCipl').val(),
            refCipl: $('#refCipl').val(),
            SoldConsignee: $('#soldConsigneeCipl').val(),
            SoldToName: $('#consigneeNameCipl').val(),
            SoldToAddress: $('#consigneeAddressCipl').val(),
            SoldToCountry: $('#consigneeCountryCipl').val(),
            SoldToTelephone: $('#consigneeTelpCipl').val(),
            SoldToFax: $('#consigneeFaxCipl').val(),
            SoldToPic: $('#consigneePicCipl').val(),
            SoldToEmail: $('#consigneeEmailCipl').val(),
            ShipDelivery: $('#shipDeliveryCipl').val(),
            ConsigneeSameSoldTo: $('#ConsigneeSameSoldTo').val(),
            ConsigneeName: $('#consigneeNameCipl').val(),
            ConsigneeAddress: $('#consigneeAddressCipl').val(),
            ConsigneeCountry: $("#consigneeCountryCipl").val(),
            ConsigneeTelephone: $('#consigneeTelpCipl').val(),
            ConsigneeFax: $('#consigneeFaxCipl').val(),
            ConsigneePic: $('#consigneePicCipl').val(),
            ConsigneeEmail: $('#consigneeEmailCipl').val(),
            //consigneeCipl: $('#consigneeCipl').val(),
            NotifyPartySameConsignee: $('#NotifyPartySameConsignee').val(),
            NotifyName: $('#notifyNameCipl').val(),
            NotifyAddress: $('#notifyAddressCipl').val(),
            NotifyCountry: $('#notifyCountryCipl').val(),
            NotifyTelephone: $('#notifyTelpCipl').val(),
            NotifyFax: $('#notifyFaxCipl').val(),
            NotifyPic: $('#notifyPicCipl').val(),
            NotifyEmail: $('#notifyEmailCipl').val(),
            Area: $('#areaCipl').val(),
            Branch: $('#cabangCipl').val().split('-')[0].trim(),
            Currency: $('#currencyCipl').val(),
            Rate: $('#RateCipl').val(),
            LcNoDate: $('#lcnoCipl').val() + ' - ' + $('#lcDateCipl').val(),
            PaymentTerms: $('#paymentCipl').val(),
            LoadingPort: $('#loadingCipl').val(),
            DestinationPort: $('#destinationCipl').val(),
            IncoTerm: $('#incoCipl').val(),
            ShippingMethod: $('#shippingCipl').val(),
            FreightPayment: $('#freightCipl').val(),
            CountryOfOrigin: $('#countryCipl').val(),
            ShippingMarks: $('#shippingMarkCipl').val(),
            Remarks: $('#remarksCipl').val(),
            inspectionCipl: $('#inspectionCipl').val(),
            SpecialInstruction: $('#txtSpecialInscCipl').val(),
            Status: status,
            PickUpPic: $('#idPickupPic').val(),
            PickUpArea: $('#idPickupArea').val(),
            CategoryReference: $('#idCategoryReference').val(),
            Consolidate: $('#ConsolidateCipl').val()
        },
        Forwader: {
            Forwader: $('#forwaderCipl').val(),
            Type: $('#typeCipl').val(),
            Branch: $('#CkbBranchCipl').val(),
            Attention: $('#forwaderAttentionCipl').val(),
            Company: $('#forwaderCompanyCipl').val(),
            SubconCompany: $('#forwaderForwadingCipl').val(),
            Address: $('#forwaderAddressCipl').val(),
            Area: $('#forwaderAreaCipl').val(),
            City: $('#forwaderCityCipl').val(),
            PostalCode: $('#forwaderPostalCodeCipl').val(),
            Contact: $('#forwaderContactCipl').val(),
            FaxNumber: $('#forwaderFaxCipl').val(),
            Forwading: $('#forwaderForwadingCipl').val(),
            Email: $('#forwaderEmailCipl').val()
        }
    }

    $.ajax({
        url: '/EMCS/CiplInsert',
        type: 'POST',
        data: {
            item,
            Status: status
        },
        cache: false,
        async: false,
        success: function (data, response) {
            $('#idCipl').val(data[0]['Id']);
            $('#noCipl').val(data[0]['No']);
            $('#dateCipl').val(moment(data[0]['Createdate']).format("DD MMM YYYY"));
            post_insert_cipl_item(data[0]['ID']);
            $('#btnSubmitCipl').prop('disabled', false);
        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Insert Data',
            })
        }
    });
}

$("#RequestChange").click(function () {
    debugger;
    var modelObj = {
        FormType: "CIPL",
        FormNo: $('#idCipl').val(),
        Reason: $('#ReasonForChange').val(),
        /*rfcList: requestForChange*/
    }
    if ($('#jenisBarangCipl').val() === 'CATERPILLAR SPAREPARTS') {
        var CategoryItem = $('#sparepartsCipl').val();
    } else if ($('#jenisBarangCipl').val() === 'MISCELLANEOUS') {
        var CategoryItem = $('#permanentCipl').val();
    } else if ($('#jenisBarangCipl').val() === 'CATERPILLAR NEW EQUIPMENT' || $('#jenisBarangCipl').val() === 'CATERPILLAR USED EQUIPMENT') {
        var CategoryItem = $('#unitCipl').val();
    }
    var item = {
        Data: {
            Id: $('#idCipl').val(),
            CiplNo: $('#noCipl').val(),
            dateCipl: $('#dateCipl').val(),
            Category: $('#jenisBarangCipl').val(),
            ReferenceNo: $('#refCipl').val() === null ? "" : $('#refCipl').val().toString(),
            CategoriItem: CategoryItem,
            ExportType: $('#exportCipl').val(),
            ExportTypeItem: $('#exportremarksCipl').val(),
            refCipl: $('#refCipl').val(),
            SoldConsignee: $('#soldConsigneeCipl').val(),
            SoldToName: $('#consigneeNameCipl').val(),
            SoldToAddress: $('#consigneeAddressCipl').val(),
            SoldToCountry: $('#consigneeCountryCipl').val(),
            SoldToTelephone: $('#consigneeTelpCipl').val(),
            SoldToFax: $('#consigneeFaxCipl').val(),
            SoldToPic: $('#consigneePicCipl').val(),
            SoldToEmail: $('#consigneeEmailCipl').val(),

            //SoldToName: $('#salesNameCipl').val(),
            //SoldToAddress: $('#salesAddressCipl').val(),
            //SoldToCountry: $('#salesCountryCipl').val(),
            //SoldToTelephone: $('#salesTelpCipl').val(),
            //SoldToFax: $('#salesFaxCipl').val(),
            //SoldToPic: $('#salesPicCipl').val(),
            //SoldToEmail: $('#salesEmailCipl').val(),

            ShipDelivery: $('#shipDeliveryCipl').val(),
            ConsigneeSameSoldTo: $('#ConsigneeSameSoldTo').val(),
            ConsigneeName: $('#consigneeNameCipl').val(),
            ConsigneeAddress: $('#consigneeAddressCipl').val(),
            ConsigneeCountry: $("#consigneeCountryCipl").val(),
            ConsigneeTelephone: $('#consigneeTelpCipl').val(),
            ConsigneeFax: $('#consigneeFaxCipl').val(),
            ConsigneePic: $('#consigneePicCipl').val(),
            ConsigneeEmail: $('#consigneeEmailCipl').val(),
            //consigneeCipl: $('#consigneeCipl').val(),
            NotifyPartySameConsignee: $('#NotifyPartySameConsignee').val(),
            NotifyName: $('#notifyNameCipl').val(),
            NotifyAddress: $('#notifyAddressCipl').val(),
            NotifyCountry: $('#notifyCountryCipl').val(),
            NotifyTelephone: $('#notifyTelpCipl').val(),
            NotifyFax: $('#notifyFaxCipl').val(),
            NotifyPic: $('#notifyPicCipl').val(),
            NotifyEmail: $('#notifyEmailCipl').val(),
            Area: $('#areaCipl').val().split('-')[0].trim(),
            Branch: $('#cabangCipl').val().split('-')[0].trim(),
            Currency: $('#currencyCipl').val(),
            Rate: $('#RateCipl').val(),
            LcNoDate: $('#lcnoCipl').val() + ' - ' + $('#lcDateCipl').val(),
            PaymentTerms: $('#paymentCipl').val(),
            LoadingPort: $('#loadingCipl').val(),
            DestinationPort: $('#destinationCipl').val(),
            IncoTerm: $('#incoCipl').val(),
            ShippingMethod: $('#shippingCipl').val(),
            FreightPayment: $('#freightCipl').val(),
            CountryOfOrigin: $('#countryCipl').val(),
            ShippingMarks: $('#shippingMarkCipl').val(),
            Remarks: $('#remarksCipl').val(),
            inspectionCipl: $('#inspectionCipl').val(),
            SpecialInstruction: $('#txtSpecialInscCipl').val(),
            Status: status,
            PickUpPic: $('#idPickupPic').val() === null || $('#idPickupPic').val() === "" ? "" : $('#idPickupPic').val().split('-')[0].trim(),
            PickUpArea: $('#idPickupArea').val() === null || $('#idPickupArea').val() === "" ? "" : $('#idPickupArea').val().split('-')[0].trim(),
            CategoryReference: $('#idCategoryReference').val(),
            Consolidate: $('#ConsolidateCipl').val()
        },
        Forwader: {
            Forwader: $('#forwaderCipl').val(),
            Type: $('#typeCipl').val(),
            Branch: $('#CkbBranchCipl').val(),
            Attention: $('#forwaderAttentionCipl').val(),
            Company: $('#forwaderCompanyCipl').val(),
            SubconCompany: $('#forwaderForwadingCipl').val(),
            Address: $('#forwaderAddressCipl').val(),
            Area: $('#forwaderAreaCipl').val(),
            City: $('#forwaderCityCipl').val(),
            PostalCode: $('#forwaderPostalCodeCipl').val(),
            Contact: $('#forwaderContactCipl').val(),
            FaxNumber: $('#forwaderFaxCipl').val(),
            Forwading: $('#forwaderForwadingCipl').val(),
            Email: $('#forwaderEmailCipl').val()
        }
    }

    $.ajax({
        url: '/EMCS/SaveChangeHistory',
        type: 'POST',
        data: {
            form: modelObj,
            item: item
        },
        cache: false,
        async: false,
        success: function (data, response) {
            //var getstatus = status === 'Draft' ? ' Saved' : ' Submit';
            //if (data == 1) {
            //    Swal.fire({
            //        type: 'success',
            //        title: 'Success',
            //        text: 'Save, Your Data Has Been' + getstatus,
            //    })
            //} else {
            //    Swal.fire({
            //        type: 'warning',
            //        title: 'Update Failed !',
            //        text: 'Unauthorized to update this document !',
            //    })
            //}

            //post_update_cipl_item(data, status);
        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Update Data',
            })
        }
    });
    //$.ajax({
    //    url: '/EMCS/SaveChangeHistory',
    //    type: 'POST',
    //    data: modelObj,
    //    cache: false,
    //    async: false,
    //    success: function (data, response) {
    //        debugger;
    //    },
    //    error: function (e) {
    //        Swal.fire({
    //            type: 'error',
    //            title: 'Oops...',
    //            text: 'Something went wrong! Fail Insert Data',
    //        })
    //    }
    //});

   
});
function post_update_cipl(status) {
  
    if ($('#jenisBarangCipl').val() === 'CATERPILLAR SPAREPARTS') {
        var CategoryItem = $('#sparepartsCipl').val();
    } else if ($('#jenisBarangCipl').val() === 'MISCELLANEOUS') {
        var CategoryItem = $('#permanentCipl').val();
    } else if ($('#jenisBarangCipl').val() === 'CATERPILLAR NEW EQUIPMENT' || $('#jenisBarangCipl').val() === 'CATERPILLAR USED EQUIPMENT') {
        var CategoryItem = $('#unitCipl').val();
    }
    var item = {
        Data: {
            Id: $('#idCipl').val(),
            CiplNo: $('#noCipl').val(),
            dateCipl: $('#dateCipl').val(),
            Category: $('#jenisBarangCipl').val(),
            ReferenceNo: $('#refCipl').val() === null ? "" : $('#refCipl').val().toString(),
            CategoriItem: CategoryItem,
            ExportType: $('#exportCipl').val(),
            ExportTypeItem: $('#exportremarksCipl').val(),
            refCipl: $('#refCipl').val(),
            SoldConsignee: $('#soldConsigneeCipl').val(),
            SoldToName: $('#consigneeNameCipl').val(),
            SoldToAddress: $('#consigneeAddressCipl').val(),
            SoldToCountry: $('#consigneeCountryCipl').val(),
            SoldToTelephone: $('#consigneeTelpCipl').val(),
            SoldToFax: $('#consigneeFaxCipl').val(),
            SoldToPic: $('#consigneePicCipl').val(),
            SoldToEmail: $('#consigneeEmailCipl').val(),

            //SoldToName: $('#salesNameCipl').val(),
            //SoldToAddress: $('#salesAddressCipl').val(),
            //SoldToCountry: $('#salesCountryCipl').val(),
            //SoldToTelephone: $('#salesTelpCipl').val(),
            //SoldToFax: $('#salesFaxCipl').val(),
            //SoldToPic: $('#salesPicCipl').val(),
            //SoldToEmail: $('#salesEmailCipl').val(),

            ShipDelivery: $('#shipDeliveryCipl').val(),
            ConsigneeSameSoldTo: $('#ConsigneeSameSoldTo').val(),
            ConsigneeName: $('#consigneeNameCipl').val(),
            ConsigneeAddress: $('#consigneeAddressCipl').val(),
            ConsigneeCountry: $("#consigneeCountryCipl").val(),
            ConsigneeTelephone: $('#consigneeTelpCipl').val(),
            ConsigneeFax: $('#consigneeFaxCipl').val(),
            ConsigneePic: $('#consigneePicCipl').val(),
            ConsigneeEmail: $('#consigneeEmailCipl').val(),
            //consigneeCipl: $('#consigneeCipl').val(),
            NotifyPartySameConsignee: $('#NotifyPartySameConsignee').val(),
            NotifyName: $('#notifyNameCipl').val(),
            NotifyAddress: $('#notifyAddressCipl').val(),
            NotifyCountry: $('#notifyCountryCipl').val(),
            NotifyTelephone: $('#notifyTelpCipl').val(),
            NotifyFax: $('#notifyFaxCipl').val(),
            NotifyPic: $('#notifyPicCipl').val(),
            NotifyEmail: $('#notifyEmailCipl').val(),
            Area: $('#areaCipl').val().split('-')[0].trim(),
            Branch: $('#cabangCipl').val().split('-')[0].trim(),
            Currency: $('#currencyCipl').val(),
            Rate: $('#RateCipl').val(),
            LcNoDate: $('#lcnoCipl').val() + ' - ' + $('#lcDateCipl').val(),
            PaymentTerms: $('#paymentCipl').val(),
            LoadingPort: $('#loadingCipl').val(),
            DestinationPort: $('#destinationCipl').val(),
            IncoTerm: $('#incoCipl').val(),
            ShippingMethod: $('#shippingCipl').val(),
            FreightPayment: $('#freightCipl').val(),
            CountryOfOrigin: $('#countryCipl').val(),
            ShippingMarks: $('#shippingMarkCipl').val(),
            Remarks: $('#remarksCipl').val(),
            inspectionCipl: $('#inspectionCipl').val(),
            SpecialInstruction: $('#txtSpecialInscCipl').val(),
            Status: status,
            PickUpPic: $('#idPickupPic').val() === null || $('#idPickupPic').val() === "" ? "" : $('#idPickupPic').val().split('-')[0].trim(),
            PickUpArea: $('#idPickupArea').val() === null || $('#idPickupArea').val() === "" ? "" : $('#idPickupArea').val().split('-')[0].trim(),
            CategoryReference: $('#idCategoryReference').val(),
            Consolidate: $('#ConsolidateCipl').val()
        },
        Forwader: {
            Forwader: $('#forwaderCipl').val(),
            Type: $('#typeCipl').val(),
            Branch: $('#CkbBranchCipl').val(),
            Attention: $('#forwaderAttentionCipl').val(),
            Company: $('#forwaderCompanyCipl').val(),
            SubconCompany: $('#forwaderForwadingCipl').val(),
            Address: $('#forwaderAddressCipl').val(),
            Area: $('#forwaderAreaCipl').val(),
            City: $('#forwaderCityCipl').val(),
            PostalCode: $('#forwaderPostalCodeCipl').val(),
            Contact: $('#forwaderContactCipl').val(),
            FaxNumber: $('#forwaderFaxCipl').val(),
            Forwading: $('#forwaderForwadingCipl').val(),
            Email: $('#forwaderEmailCipl').val()
        }
    }

    $.ajax({
        url: '/EMCS/CiplUpdate',
        type: 'POST',
        data: {
            item,
            Status: status
        },
        cache: false,
        async: false,
        success: function (data, response) {
            var getstatus = status === 'Draft' ? ' Saved' : ' Submit';
            if (data == 1)
            {
                Swal.fire({
                    type: 'success',
                    title: 'Success',
                    text: 'Save, Your Data Has Been' + getstatus,
                })
            } else {
                Swal.fire({
                    type: 'warning',
                    title: 'Update Failed !',
                    text: 'Unauthorized to update this document !',
                })
            }
            
            //post_update_cipl_item(data, status);
        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Update Data',
            })
        }
    });
}

function ConvertCurrency(data) {
    var array = new Array();
    $.each(data, function (index, element) {
        var arraydata = {
            No: element.No,
            ReferenceNo: element.ReferenceNo,
            Currency: element.Currency,
            Id: element.Id,
            IdCustomer: element.IdCustomer,
            Name: element.Name,
            Quantity: element.Quantity,
            UnitUom: element.UnitUom,
            Sn: element.Sn,
            IdNo: element.IdNo,
            YearMade: element.YearMade,
            CaseNumber: element.CaseNumber,
            Type: element.Type,
            UnitPrice: removeformatCurrency(element.UnitPrice),
            ExtendedValue: removeformatCurrency(element.ExtendedValue),
            Length: removeformatCurrency(element.Length),
            Width: removeformatCurrency(element.Width),
            Height: removeformatCurrency(element.Height),
            Volume: removeformatCurrency(element.Volume),
            NetWeight: removeformatCurrency(element.NetWeight),
            GrossWeight: removeformatCurrency(element.GrossWeight)
        }
        array.push(arraydata);
    })
    return array;
}


function post_insert_cipl_item(id) {
    var Category = GetCategoryUsed();
    if (Category === 'MISCELLANEOUS' || $('#idCategoryReference').val() === 'Other') {
        ajaxInsertUpdateReference();
        return false;
    } else if (Category === 'REMAN' || Category === 'PRA') {
        var reference = $tablereference.bootstrapTable('getSelections');
        var data = AddNewOldCore(reference);
    } else {
        var reference = $tablereference.bootstrapTable('getSelections');
        var data = ReferenceItemConvert(reference);
    }
    $.ajax({
        url: '/EMCS/CiplItemInsert',
        type: 'POST',
        data: {
            data: data,
            idCipl: id
        },
        cache: false,
        async: false,
        success: function (data, response) {
            Swal.fire({
                type: 'success',
                title: 'Success'
                , text: 'Saved, Your Data Has Been Saved',
            })
        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Update Data',
            })
        }
    })
    //}
}


function post_update_cipl_item(id, status) {
    event.preventDefault();
    var data = ConvertCurrency(get_used_table_cipl_item().bootstrapTable('getData'));
    $.ajax({
        url: '/EMCS/CiplItemUpdate',
        type: 'POST',
        data: {
            data: data,
            id: $('#idCipl').val(),
            Status: status
        },
        cache: false,
        async: false,
        success: function (data, response) {
            Swal.fire({
                type: 'success',
                title: 'Success',
                text: 'Saved, Your Data Has Been Saved',
            })
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

