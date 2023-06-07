var $table = $('#table-inventory-list');
setTimeout(function () { $(".model-parent").find(".select2").addClass("width30"); }, 300);

$(function () {
    clearScreen();
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');

    var IsAdminCAT = $("#IsAdminCAT").val();

    $table.bootstrapTable({
        cache: false,
        search: false,
        pagination: true,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: false,
        showRefresh: false,
        smartDisplay: false,
        pageSize: '5',
        showExport: false,
        exportTypes: ['excel'],
        exportOptions: {
            ignoreColumn: [0],
            fileName: 'file.xls'
        },
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns:
        [
            [
                { field: 'action', title: 'Action', width: '5%', align: 'center', rowspan: 2, formatter: operateFormatter, switchable: false },
                { field: 'Note', title: 'Note', align: 'center', rowspan: 2, formatter: operateFormatterNote, switchable: false, visible: Boolean(IsAdminCAT) },
                { field: 'RefPartNo', title: 'Ref. Part Number', halign: 'center', align: 'left', rowspan: 2, sortable: true, switchable: false },
                { field: 'AlternetPartNumber', title: 'Alternative<br>Part Number', halign: 'center', align: 'left', rowspan: 2, sortable: true, switchable: false },
                { field: 'ApplicableModel', title: 'Applicable<br>Model', halign: 'center', align: 'left', rowspan: 2, sortable: true, switchable: false },
                { field: 'Prefix', title: 'Prefix', halign: 'center', align: 'left', rowspan: 2, sortable: true },
                { field: 'Component', title: 'Component', halign: 'center', align: 'left', rowspan: 2, sortable: true },
                //{ field: 'Section', title: 'Section', halign: 'center', align: 'left', rowspan: 2, sortable: true },
                { field: 'MOD', title: 'MOD', halign: 'center', align: 'center', rowspan: 2, sortable: true },
                { field: 'LastStatus', title: 'Status', halign: 'center', align: 'center', rowspan: 2, sortable: true, switchable: false },
                { field: 'SOS', title: 'SOS', halign: 'center', align: 'left', rowspan: 2, sortable: true },
                { field: 'StoreNumber', title: 'Store', halign: 'center', align: 'left', rowspan: 2, sortable: true },
                { field: 'KAL', title: 'Component<br>Inventory Number', halign: 'center', align: 'left', rowspan: 2, sortable: true },
                { title: 'CYCLE 1', halign: 'center', align: 'center', colspan: 6, },
                { title: 'UNDER REBUILD', halign: 'center', align: 'center', colspan: 7, },
            ],
            [
                { field: 'PONumber', title: 'PO Number', halign: 'center', align: 'center', sortable: true },
                //{ field: 'Original', title: 'Original', width: '170px', halign: 'center', align: 'left', sortable: true },
                { field: 'Schedule', title: 'Schedule', halign: 'center', align: 'center', sortable: true },
                { field: 'UnitNumber', title: 'Unit No.', halign: 'center', align: 'left', sortable: true, switchable: false },
                { field: 'EquipmentNumber', title: 'Serial Number', halign: 'center', align: 'center', sortable: true },
                { field: 'Location', title: 'Location', halign: 'center', width: '120px', align: 'center', sortable: true, switchable: false },
                { field: 'Customer', title: 'Customer', halign: 'center', align: 'right', sortable: true, switchable: false },

                { field: 'DocTransfer', title: 'Stock Transfer', halign: 'center', align: 'center', sortable: true },
                { field: 'DocSales', title: 'Doc Sales', halign: 'center', align: 'center', sortable: true },
                { field: 'DocWCSL', title: 'WCSL', halign: 'center', align: 'center', sortable: true },
                { field: 'NewWO6F', title: 'New WO 6F', halign: 'center', align: 'center', sortable: true },
                //{ field: 'WO1K', title: 'WO 1K', halign: 'center', align: 'center', sortable: true },
                //{ field: 'OldWO6F', title: 'Old WO 6F', halign: 'center', align: 'center', sortable: true },
                { field: 'MO', title: 'MO', halign: 'center', align: 'center', sortable: true },
                { field: 'RebuildStatus', title: 'Rebuild Status', halign: 'center', align: 'center', sortable: true },
                { field: 'CRC_PCD', title: 'CRC Promised<br>Completion Date', halign: 'center', align: 'center', formatter: dateFormatterCAT, sortable: true }
            ]
        ]
    });
    $("[name=refresh]").css('display', 'none');

    $('#btn-filters').click(function () {
        window.pis.table({
            objTable: $table,
            urlSearch: '/cat/InventoryPage',
            urlPaging: '/cat/InventoryPageXt',
            searchParams: {
                ref_part_no: $('#ref_part_no').val(),
                alt_part_no: $('#alt_part_no').val(),
                comp_inv_no: $('#comp_inv_no').val(),
                app_model: $('#app_model').val(),
                component: $('#component').val(),
                doctransfer: $('#doctransfer').val(),
                wcsl: $('#wcsl').val(),
                WO: $('#WO').val(),
                rebuildstatuscms: $('#rebuildstatuscms').val(),
                //prefix: $('#prefix').val(),
                //smcs_code: $('#smcs_code').val(),

                //mod: $('#mod').val(),
                StoreID: $('#StoreID').val(),
                //core_model: $('#core_model').val(),
                SOSID: $('#SOSID').val(),
                originalschedule: $('#originalschedule').val(),
                unitno: $('#unitno').val(),
                serialno: $('#serialno').val(),
                location: $('#location').val(),
                customer: $('#customer').val(),
                OriginalSchedule: $('#OriginalSchedule').val(),
                //family: $('#family').val(),
                //crc_tat: $('#crc_tat').val(),
                //SectionID: $('#SectionID').val(),
                surplus: $("[name='surplus']:checked").val(),
                StatusID: $('#statusid').val() == "" ? 0 : $('#statusid').val()
            },
            dataHeight: 412,
            autoLoad: true
        });
        $('.fixed-table-body-columns').css('display', 'block');
    });

    $('#btn-clear').click(function () {
        clearScreen();
    });
});

function operateFormatter(value, row, index) {
    return [
        '<div class="btn-group">',
        '<button type="button" class="btn btn-info edit" onclick="btneditinventory(\''
        + row.ID + '\')" title="Edit"><i class="fa fa-edit"></i> Edit</button>',
        '<button type="button" class="btn btn-info detail" onclick="viewDetail(\''
        + row.ID + '\')" title="Detail"><i class="fa fa-search-plus"></i> Detail</button>',
        '</div>'
    ].join('');
}

function operateFormatterNote(value, row, index) {
    if (row.NewWO6F != row.WO_CMS) {
        return 'WO DBS : ' + row.NewWO6F + '<br> WO CMS : ' + row.WO_CMS;
    }

    return '';
}

function viewDetail(InventoryID) {
    var url = '/cat/InventoryDetail?&InventoryID=' + InventoryID;
    goToDetail(url, 'parent', 'detail');

}

goToDetail = function (url, parentId, detailId) {
    $("#" + detailId).empty();
    $("#" + detailId).show();
    $("#" + detailId).focus();
    $("#" + parentId).hide();
    enableFilter(false);

    $.ajax({
        type: "GET",
        url: url,
        dataType: "html",
        async: false,
        cache: false,
        beforeSend: function () {
            enableLink(false);
            $("#" + detailId).html("<div style=\"text-align:center;color:red\"><img src='/Content/images/ajax-loading.gif' style=\"padding-right:3px\">...Loading page...</div>");
        },
        success: function (data) {
            var _br = $('.box-header').length ? "" : "<br/>";
            $("#" + detailId).empty();
            $("#" + detailId).html(_br + data);

            if (typeof window.rebindCSS == "undefined") {
                alert("rebindCSS")
                $.getScript("/scripts/script.js", function () {
                });
            }
            else {
                rebindCSS();
            }
        }
    });
}

function enableFilter(isEnable) {
    $('#filter').prop('disabled', !isEnable);
    $("#StoreID").prop("disabled", !isEnable);
    $("#SOSID").prop("disabled", !isEnable);
    $("#SectionID").prop("disabled", !isEnable);
    $("#statusid").prop("disabled", !isEnable);
}

$(".downloadExcel").click(function () {
    enableLink(false);
    $.ajax({
        url: "DownloadInventory",
        type: 'GET',
        data: {
            ref_part_no: $('#ref_part_no').val(),
            alt_part_no: $('#alt_part_no').val(),
            comp_inv_no: $('#comp_inv_no').val(),
            app_model: $('#app_model').val(),
            prefix: $('#prefix').val(),
            smcs_code: $('#smcs_code').val(),
            component: $('#component').val(),
            mod: $('#mod').val(),
            StoreID: $('#StoreID').val() == "" ? 0 : $('#StoreID').val(),
            core_model: $('#core_model').val(),
            SOSID: $('#SOSID').val(),
            family: $('#family').val(),
            crc_tat: $('#crc_tat').val(),
            SectionID: $('#SectionID').val() == "" ? 0 : $('#SectionID').val(),
            surplus: $("[name='surplus']:checked").val(),
            StatusID: $('#statusid').val() == "" ? 0 : $('#statusid').val()
        },
        success: function (guid) {
            enableLink(true);
            window.open('DownloadToExcel?guid=' + guid, '_blank');
        },
        cache: false,
    });
});

$(".downloadExcelforEdit").click(function () {
    enableLink(false);
    $.ajax({
        url: "DownloadInventoryforEdit",
        type: 'GET',
        success: function (guid) {
            enableLink(true);
            window.open('DownloadToExcel?guid=' + guid, '_blank');
        },
        cache: false,
    });
});

function clearScreen() {
    $('#ref_part_no').val('');
    $('#alt_part_no').val('');
    $('#comp_inv_no').val('');
    $('#app_model').val('');
    $('#prefix').val('');
    $('#smcs_code').val('');
    $('#component').val('');
    $('#mod').val('');
    $('#StoreID').val('', 'ALL').change();
    $('#core_model').val('');
    $('#SOSID').val('', 'ALL').change();
    $('#family').val('');
    $('#crc_tat').val('');
    $('#SectionID').val('', 'ALL').change();
    $('#statusid').val('', 'ALL').change();
    $('#rebuildstatuscms').val('', 'ALL').change();
    $('#blank').prop('checked', false);
    $('#yes').prop('checked', false);
    $('#no').prop('checked', false);

    //New Filter
    $('#location').val('', 'ALL').change();
    $('#doctransfer').val('');
    $('#wcsl').val('');
    $('#WO').val('');
    //$('#rebuildstatuscms').val('');

    $('#unitno').val('');
    $('#serialno').val('');
    $('#customer').val('');

}

$("form#submitInventoryEdit").submit(function () {
    var formData = new FormData($(this)[0]);
    enableLink(false);

    $.ajax({
        url: $(this).attr("action"),
        type: 'POST',
        data: formData,
        async: true,
        success: function (result) {
            enableLink(true);

            if (result.Status == 0) {
                if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
                $("#btn-filters").trigger('click');
            }
            else {
                if (result.Msg != undefined)
                    sAlert('Failed', result.Msg, 'error');
                $("#btn-filters").trigger('click');
            }

            $("#InventoryEditxls").val("");
        },
        cache: false,
        contentType: false,
        processData: false
    });

    return false;
});

$("form#submitPartInfoDetail").submit(function () {
    var formData = new FormData($(this)[0]);
    enableLink(false);

    $.ajax({
        url: $(this).attr("action"),
        type: 'POST',
        data: formData,
        async: true,
        success: function (result) {
            enableLink(true);

            if (result.Status == 0) {
                if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
                $("#btn-filters").trigger('click');
            }
            else {
                if (result.Msg != undefined)
                    sAlert('Failed', result.Msg, 'error');
                $("#btn-filters").trigger('click');
            }

            $("#PartInfoDetailxls").val("");
        },
        cache: false,
        contentType: false,
        processData: false
    });

    return false;
});

$("form#submitInvAllocation").submit(function () {
    var formData = new FormData($(this)[0]);
    enableLink(false);

    $.ajax({
        url: $(this).attr("action"),
        type: 'POST',
        data: formData,
        async: true,
        success: function (result) {
            enableLink(true);

            if (result.Status == 0) {
                if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
                $("#btn-filters").trigger('click');
            }
            else {
                if (result.Msg != undefined)
                    sAlert('Failed', result.Msg, 'error');
                $("#btn-filters").trigger('click');
            }

            $("#InvAllocationxls").val("");
        },
        cache: false,
        contentType: false,
        processData: false
    });

    return false;
});

$(".popupeditinventoryexcel").click(function () {
    $('#popupeditinventory').modal('show');
});

$('.calstoresupplydate').click(function () {
    $('#EditStoreSupplyDate').focus().datepicker({
        format: " mm/dd/yyyy",
        minDate: 0//,
        //startDate: "today"
    });
});

$('.calstocktranferdate').click(function () {
    $('#EditStockTransferDate').focus().datepicker({
        format: " mm/dd/yyyy",
        minDate: 0
        //startDate: "today"
    });
});

$('.calInv').click(function () {
    $('#OriginalSchedule').focus().datepicker({
        format: " mm/dd/yyyy",
        //minDate: '0'
        startDate: "today"
    });
});

//add new inventory
$(".InsertInventory").click(function () {
    GetlistSOS();
    GetlistStore();
    $('#popupNewinventory').modal('show');
})

$("#btnNewInventory").click(function () {


    if ($('#InsertKAL').val() == "") {
        sAlert('Validation Info', 'Please fill KAL', 'info');
        return;
    }
    if ($('#InsertStore').val() == "") {
        sAlert('Validation Info', 'Please Select Store', 'info');
        return;
    }
    if ($('#InsertAltPN').val() == "") {
        sAlert('Validation Info', 'Please fill Alternate Part Number', 'info');
        return;
    }
    if ($('#InsertSOS').val() == "") {
        sAlert('Validation Info', 'Please Select SOS', 'info');
        return;
    }
    if ($('#InsertRGNumber').val() == "") {
        sAlert('Validation Info', 'Please fill RG Number', 'info');
        return;
    }

    var data = {
        'kal': $('#InsertKAL').val(),
        'store': $('#InsertStore').val(),
        'altpn': $('#InsertAltPN').val(),
        'sos': $("#InsertSOS option:selected").text(),
        'RGNumber': $('#InsertRGNumber').val()
    };

    $.ajax({
        type: "POST",
        url: myApp.root + 'cat/AddNewInventory',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: data,
        dataType: "json",
        success: function (d) {
            if (d.Msg != undefined) {
                sAlert('Success', d.Msg, 'success');

            }
            $("[name=refresh]").trigger('click');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });


})

function GetlistSOS() {
    var listcust = [];
    $.ajax({
        type: 'GET',
        url: myApp.root + 'cat/MasterSOSPage',
        async: false,
        dataType: 'json'
    })
    .done(function (data, textStatus, jqXHR) {
        if (data != null) {
            listcust = [];
            $.each(data.Data.result, function (index, value) {
                listcust.push({ id: value.ID, value: value.ID, text: value.SOS })
            });
        }

        $('#InsertSOS').text('');
        $('#InsertSOS').select2({
            data: listcust
        });
        $('#InsertSOS').val(null).trigger("change");

    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        var result = $.parseJSON(jqXHR.responseText);
        sAlert(result.Message,
            result.ExceptionMessage + "&#13;&#10;" +
            result.ExceptionType + "&#13;&#10;" +
            result.StackTrace, "error");
    })
    .then(function () {

    });
}

function GetlistStore() {
    var listcust = [];
    $.ajax({
        type: 'GET',
        url: myApp.root + 'cat/GetListStore',
        async: false,
        dataType: 'json'
    })
    .done(function (data, textStatus, jqXHR) {
        if (data != null) {
            listcust = [];
            $.each(data, function (index, value) {
                listcust.push({ id: value.StoreNo.trim(), value: value.StoreNo.trim(), text: value.Name })
            });
        }

        $('#InsertStore').text('');
        $('#InsertStore').select2({
            data: listcust
        });
        $('#InsertStore').val(null).trigger("change");

    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        var result = $.parseJSON(jqXHR.responseText);
        sAlert(result.Message,
            result.ExceptionMessage + "&#13;&#10;" +
            result.ExceptionType + "&#13;&#10;" +
            result.StackTrace, "error");
    })
    .then(function () {

    });
}
//end new inventory

//add edit inventory
function GetlistStoreEdit() {
    var listcust = [];
    $.ajax({
        type: 'GET',
        url: myApp.root + 'cat/GetListStore',
        async: false,
        dataType: 'json'
    })
    .done(function (data, textStatus, jqXHR) {
        if (data != null) {
            listcust = [];
            $.each(data, function (index, value) {
                listcust.push({ id: value.StoreNo.trim(), value: value.StoreNo.trim(), text: value.Name })
            });
        }

        $('#EditStore').text('');
        $('#EditStore').select2({
            data: listcust
        });
        $('#EditStore').val(null).trigger("change");

    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        var result = $.parseJSON(jqXHR.responseText);
        sAlert(result.Message,
            result.ExceptionMessage + "&#13;&#10;" +
            result.ExceptionType + "&#13;&#10;" +
            result.StackTrace, "error");
    })
    .then(function () {

    });
}
function GetStoreByID(id) {
    var listcust = [];
    var data = {
        'ID': id
    };
    $.ajax({
        type: 'GET',
        data: data,
        url: myApp.root + 'cat/GetListStorebyID',
        async: false,
        dataType: 'json'
    })
    .done(function (data, textStatus, jqXHR) {


        if (data != null) {
            $("#EditStore").val(data);
            listcust = [];
            $("#EditStore").select2('data', data);
            $("#EditStore").val(data[0].ID).trigger('change');
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        var result = $.parseJSON(jqXHR.responseText);
        sweetAlert("Failed", result.ExceptionMessage + "&#13;&#10;", "error");
    })
}

function GetlistSOSEdit() {
    var listcust = [];
    $.ajax({
        type: 'GET',
        url: myApp.root + 'cat/MasterSOSPage',
        async: false,
        dataType: 'json'
    })
    .done(function (data, textStatus, jqXHR) {
        if (data != null) {
            listcust = [];
            $.each(data.Data.result, function (index, value) {
                listcust.push({ id: value.ID, value: value.ID, text: value.SOS })
            });
        }

        $('#EditSOS').text('');
        $('#EditSOS').select2({
            data: listcust
        });
        $('#EditSOS').val(null).trigger("change");

    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        var result = $.parseJSON(jqXHR.responseText);
        sAlert(result.Message,
            result.ExceptionMessage + "&#13;&#10;" +
            result.ExceptionType + "&#13;&#10;" +
            result.StackTrace, "error");
    })
    .then(function () {

    });
}
function GetSOSByID(sos) {
    var listcust = [];
    var data = {
        'sos': sos
    };
    $.ajax({
        type: 'GET',
        data: data,
        url: myApp.root + 'cat/MasterSOSbyID',
        async: false,
        dataType: 'json'
    })
    .done(function (data, textStatus, jqXHR) {


        if (data != null) {
            $("#EditSOS").val(data);
            listcust = [];
            $("#EditSOS").select2('data', data);
            $("#EditSOS").val(data.ID).trigger('change');
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        var result = $.parseJSON(jqXHR.responseText);
        sweetAlert("Failed", result.ExceptionMessage + "&#13;&#10;", "error");
    })
}

function btneditinventory(id) {
    GetDataEditInventory(id)
    $('#popupEditinventory').modal('show');
}

function GetDataEditInventory(invid) {
    $('#InventoryIDEdit').val(invid);
    $.ajax({
        type: 'GET',
        url: myApp.root + 'cat/GetDataEditInventory?InventoryID=' + invid,
        //data: { InventoryID: invid },
        async: false,
        dataType: 'json'
    })
    .done(function (data, textStatus, jqXHR) {
        if (data != null) {
            GetlistStoreEdit();
            GetlistSOSEdit();
            //GetStoreByID(data.StoreID);
            $("#EditStore").val(data.StoreID).trigger('change');
            GetSOSByID(data.SOS);

            $("#EditNewStatus").val(data.LastStatus).trigger('change');
            $('#EditKAL').val(data.KAL);
            $('#EditAltPN').val(data.AlternetPartNumber);
            $('#EditRGNumber').val(data.RGNumber);
            $('#EditWCSL').val(data.DocWCSL);
            $('#EditUsedSN').val(data.UnitNumber);
            $('#EditTUID').val(data.TUID);
            $('#EditOldStatus').val(data.LastStatus);
            $('#EditDocSales').val(data.DocSales);
            $('#EditStoreSupplyDate').val(data.DocDate);
            $('#EditEquipmentNo').val(data.EquipmentNumber);
            $('#EditMO').val(data.MO);
            $('#EditDocR').val(data.DocReturn);
            $('#EditWO').val(data.NewWO6F);
            $('#EditCustomer').val(data.CUSTOMER_ID);
            $('#EditRefDocTransfer').val(data.DocTransfer);
            $('#EditStockTransferDate').val(data.DocDateTransfer);

            //disabledstatus('blank');
        }



    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        var result = $.parseJSON(jqXHR.responseText);
        sAlert(result.Message,
            result.ExceptionMessage + "&#13;&#10;" +
            result.ExceptionType + "&#13;&#10;" +
            result.StackTrace, "error");
    })
    .then(function () {

    });
}

$("#btnUserEditInv").click(function () {
    var status = $('#EditNewStatus').val();

    if (status == 'OH') {
        if ($('#EditKAL').val() == "") {
            sAlert('Validation Info', 'Please fill KAL', 'info');
            return;
        }
        if ($('#EditAltPN').val() == "") {
            sAlert('Validation Info', 'Please Alternate Part Number', 'info');
            return;
        }
        if ($('#EditNewStatus').val() == "") {
            sAlert('Validation Info', 'Please fill Select New Status', 'info');
            return;
        }
        if ($('#EditStore').val() == "") {
            sAlert('Validation Info', 'Please Select Store', 'info');
            return;
        }
        if ($('#EditSOS').val() == "") {
            sAlert('Validation Info', 'Please select SOS', 'info');
            return;
        }
        if ($('#EditRGNumber').val() == "") {
            sAlert('Validation Info', 'Please fill RG Number', 'info');
            return;
        }
    }
    else if (status == 'ST') {
        if ($('#EditKAL').val() == "") {
            sAlert('Validation Info', 'Please fill KAL', 'info');
            return;
        }
        if ($('#EditAltPN').val() == "") {
            sAlert('Validation Info', 'Please Alternate Part Number', 'info');
            return;
        }
        if ($('#EditNewStatus').val() == "") {
            sAlert('Validation Info', 'Please fill Select New Status', 'info');
            return;
        }
        if ($('#EditStore').val() == "") {
            sAlert('Validation Info', 'Please Select Store', 'info');
            return;
        }
        if ($('#EditSOS').val() == "") {
            sAlert('Validation Info', 'Please select SOS', 'info');
            return;
        }
        if ($('#EditRefDocTransfer').val() == "") {
            sAlert('Validation Info', 'Please fill Ref Doc Transfer', 'info');
            return;
        }
        if ($('#EditStockTransferDate').val() == "") {
            sAlert('Validation Info', 'Please fill Stock Transfer Date', 'info');
            return;
        }
    }
    else if (status == 'WOC') {
        if ($("#EditSOS option:selected").text() != "800")
        {
            if ($('#EditKAL').val() == "") {
                sAlert('Validation Info', 'Please fill KAL', 'info');
                return;
            }
            if ($('#EditAltPN').val() == "") {
                sAlert('Validation Info', 'Please Alternate Part Number', 'info');
                return;
            }
            if ($('#EditNewStatus').val() == "") {
                sAlert('Validation Info', 'Please fill Select New Status', 'info');
                return;
            }
            if ($('#EditStore').val() == "") {
                sAlert('Validation Info', 'Please Select Store', 'info');
                return;
            }
            if ($('#EditSOS').val() == "") {
                sAlert('Validation Info', 'Please select SOS', 'info');
                return;
            }
            if ($('#EditDocSales').val() == "") {
                sAlert('Validation Info', 'Please fill Doc Sales', 'info');
                return;
            }
            if ($('#EditWO').val() == "") {
                sAlert('Validation Info', 'Please fill Workorder', 'info');
                return;
            }
        }
        else
        {
            if ($('#EditKAL').val() == "") {
                sAlert('Validation Info', 'Please fill KAL', 'info');
                return;
            }
            if ($('#EditAltPN').val() == "") {
                sAlert('Validation Info', 'Please Alternate Part Number', 'info');
                return;
            }
            if ($('#EditNewStatus').val() == "") {
                sAlert('Validation Info', 'Please fill Select New Status', 'info');
                return;
            }
            if ($('#EditStore').val() == "") {
                sAlert('Validation Info', 'Please Select Store', 'info');
                return;
            }
            if ($('#EditSOS').val() == "") {
                sAlert('Validation Info', 'Please select SOS', 'info');
                return;
            }
            if ($('#EditDocSales').val() == "") {
                sAlert('Validation Info', 'Please fill Doc Sales', 'info');
                return;
            }
        }
       
    }
    else if (status == 'TTC') {
        if ($('#EditKAL').val() == "") {
            sAlert('Validation Info', 'Please fill KAL', 'info');
            return;
        }
        if ($('#EditAltPN').val() == "") {
            sAlert('Validation Info', 'Please Alternate Part Number', 'info');
            return;
        }
        if ($('#EditNewStatus').val() == "") {
            sAlert('Validation Info', 'Please fill Select New Status', 'info');
            return;
        }
        if ($('#EditStore').val() == "") {
            sAlert('Validation Info', 'Please Select Store', 'info');
            return;
        }
        if ($('#EditSOS').val() == "") {
            sAlert('Validation Info', 'Please select SOS', 'info');
            return;
        }
        if ($('#EditDocSales').val() == "") {
            sAlert('Validation Info', 'Please fill Doc Sales', 'info');
            return;
        }
        if ($("#EditSOS").val() == "800") {
            if ($('#EditWCSL').val() == "") {
                sAlert('Validation Info', 'Please fill Doc WCSL', 'info');
                return;
            }
        }
       
    }
    else if (status == 'WIP') {
        if ($('#EditKAL').val() == "") {
            sAlert('Validation Info', 'Please fill KAL', 'info');
            return;
        }
        if ($('#EditAltPN').val() == "") {
            sAlert('Validation Info', 'Please Alternate Part Number', 'info');
            return;
        }
        if ($('#EditNewStatus').val() == "") {
            sAlert('Validation Info', 'Please fill Select New Status', 'info');
            return;
        }
        if ($('#EditStore').val() == "") {
            sAlert('Validation Info', 'Please Select Store', 'info');
            return;
        }
        if ($('#EditSOS').val() == "") {
            sAlert('Validation Info', 'Please select SOS', 'info');
            return;
        }
        if ($('#EditDocSales').val() == "") {
            sAlert('Validation Info', 'Please fill Doc Sales', 'info');
            return;
        }
        if ($('#EditMO').val() == "") {
            sAlert('Validation Info', 'Please fill MO', 'info');
            return;
        }
        if ($("#EditSOS").val() != "800") {
            if ($('#EditWO').val() == "") {
                sAlert('Validation Info', 'Please fill Workorder', 'info');
                return;
            }
        }
        else
        {
            if ($('#EditWCSL').val() == "") {
                sAlert('Validation Info', 'Please fill Doc WCSL', 'info');
                return;
            }
        }
       
       
    }
    else if (status == 'SQ') {
        if ($('#EditKAL').val() == "") {
            sAlert('Validation Info', 'Please fill KAL', 'info');
            return;
        }
        if ($('#EditAltPN').val() == "") {
            sAlert('Validation Info', 'Please Alternate Part Number', 'info');
            return;
        }
        if ($('#EditNewStatus').val() == "") {
            sAlert('Validation Info', 'Please fill Select New Status', 'info');
            return;
        }
        if ($('#EditStore').val() == "") {
            sAlert('Validation Info', 'Please Select Store', 'info');
            return;
        }
        if ($('#EditSOS').val() == "") {
            sAlert('Validation Info', 'Please select SOS', 'info');
            return;
        }
        if ($('#EditDocSales').val() == "") {
            sAlert('Validation Info', 'Please fill Doc Sales', 'info');
            return;
        }
        if ($('#EditMO').val() == "") {
            sAlert('Validation Info', 'Please fill MO', 'info');
            return;
        }
        if ($("#EditSOS").val() != "800") {
            if ($('#EditWO').val() == "") {
                sAlert('Validation Info', 'Please fill Workorder', 'info');
                return;
            }
        }
        else {
            if ($('#EditWCSL').val() == "") {
                sAlert('Validation Info', 'Please fill Doc WCSL', 'info');
                return;
            }
        }
    }
    else if (status == 'BER') {
        if ($('#EditKAL').val() == "") {
            sAlert('Validation Info', 'Please fill KAL', 'info');
            return;
        }
        if ($('#EditAltPN').val() == "") {
            sAlert('Validation Info', 'Please Alternate Part Number', 'info');
            return;
        }
        if ($('#EditNewStatus').val() == "") {
            sAlert('Validation Info', 'Please fill Select New Status', 'info');
            return;
        }
        if ($('#EditStore').val() == "") {
            sAlert('Validation Info', 'Please Select Store', 'info');
            return;
        }
        if ($('#EditSOS').val() == "") {
            sAlert('Validation Info', 'Please select SOS', 'info');
            return;
        }
        if ($('#EditDocSales').val() == "") {
            sAlert('Validation Info', 'Please fill Doc Sales', 'info');
            return;
        }
        if ($('#EditMO').val() == "") {
            sAlert('Validation Info', 'Please fill MO', 'info');
            return;
        }
        if ($("#EditSOS").val() != "800") {
            if ($('#EditWO').val() == "") {
                sAlert('Validation Info', 'Please fill Workorder', 'info');
                return;
            }
        }
        else {
            if ($('#EditWCSL').val() == "") {
                sAlert('Validation Info', 'Please fill Doc WCSL', 'info');
                return;
            }
        }
    }
    else if (status == 'JC') {
        if ($('#EditKAL').val() == "") {
            sAlert('Validation Info', 'Please fill KAL', 'info');
            return;
        }
        if ($('#EditAltPN').val() == "") {
            sAlert('Validation Info', 'Please Alternate Part Number', 'info');
            return;
        }
        if ($('#EditNewStatus').val() == "") {
            sAlert('Validation Info', 'Please fill Select New Status', 'info');
            return;
        }
        if ($('#EditStore').val() == "") {
            sAlert('Validation Info', 'Please Select Store', 'info');
            return;
        }
        if ($('#EditSOS').val() == "") {
            sAlert('Validation Info', 'Please select SOS', 'info');
            return;
        }
        if ($('#EditDocSales').val() == "") {
            sAlert('Validation Info', 'Please fill Doc Sales', 'info');
            return;
        }
        if ($('#EditMO').val() == "") {
            sAlert('Validation Info', 'Please fill MO', 'info');
            return;
        }
        if ($("#EditSOS").val() != "800") {
            if ($('#EditWO').val() == "") {
                sAlert('Validation Info', 'Please fill Workorder', 'info');
                return;
            }
        }
        else {
            if ($('#EditWCSL').val() == "") {
                sAlert('Validation Info', 'Please fill Doc WCSL', 'info');
                return;
            }
        }
    }
    else
    {
        sAlert('Validation Info', 'Please select Last status', 'info');
        return;
    }

    var data = {
        'InventoryID': $('#InventoryIDEdit').val(),
        'kal': $('#EditKAL').val(),
        'altpn': $('#EditAltPN').val(),
        'rgnumber': $('#EditRGNumber').val(),
        'wcsl': $('#EditWCSL').val(),
        'usedSN': $('#EditUsedSN').val(),
        'tuid': $('#EditTUID').val(),
        'DocSales': $('#EditDocSales').val(),
        'DocDate': $('#EditStoreSupplyDate').val(),
        'EquipmentNumber': $('#EditEquipmentNo').val(),
        'MO': $('#EditMO').val(),
        'DocReturn': $('#EditDocR').val(),
        'NewWO6F': $('#EditWO').val(),
        'CUSTOMER_ID': $('#EditCustomer').val(),
        'DocTransfer': $('#EditRefDocTransfer').val(),
        'DocDateTransfer': $('#EditStockTransferDate').val(),
        'SOS': $('#EditSOS').val(),
        'Store': $('#EditStore').val(),
        'LastStatus': $('#EditNewStatus').val()
    };


    $.ajax({
        type: "POST",
        url: myApp.root + 'cat/AddEditInventory',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: data,
        dataType: "json",
        success: function (d) {
            if (d.Msg != undefined) {
                sAlert('Success', d.Msg, 'success');

            }
            $("[name=refresh]").trigger('click');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });


})

$('#EditNewStatus').on('select2:select', function (e) {
    //disabledstatus($('#EditNewStatus').val());
});

function disabledstatus(status) {

    $('#EditKAL').prop('disabled', true);
    $('#EditAltPN').prop('disabled', true);
    $('#EditRGNumber').prop('disabled', true);
    $('#EditWCSL').prop('disabled', true);
    $('#EditUsedSN').prop('disabled', true);
    $('#EditTUID').prop('disabled', true);
    $('#EditOldStatus').prop('disabled', true);
    $('#EditDocSales').prop('disabled', true);
    $('#EditStoreSupplyDate').prop('disabled', true);
    $('#EditEquipmentNo').prop('disabled', true);
    $('#EditMO').prop('disabled', true);
    $('#EditDocR').prop('disabled', true);
    $('#EditWO').prop('disabled', true);
    $('#EditCustomer').prop('disabled', true);
    $('#EditRefDocTransfer').prop('disabled', true);
    $('#EditStockTransferDate').prop('disabled', true);
    $('#EditSOS').prop('disabled', true);
    $('#EditStore').prop('disabled', true);

    if (status == "OH") {
        $('#EditAltPN').prop('disabled', false);
        $('#EditSOS').prop('disabled', false);
        $('#EditStore').prop('disabled', false);
    }


}

function ValidationOH() {

    valcantempty($('#EditKAL').val(), 'Please fill KAL');
    valcantempty($('#EditAltPN').val(), 'Please fill Alternate Part Number');
    valcantempty($('#EditNewStatus').val(), 'Please Select New Status');
    valcantempty($('#EditStore').val(), 'Please select Store');
    valcantempty($('#EditSOS').val(), 'Please select SOS');
    valcantempty($('#EditRGNumber').val(), 'Please fill RG Number');
    //valmustempty($('#EditDocSales').val(), 'Please empty Doc Sales');
    //valmustempty($('#EditDocR').val(), 'Please empty Doc Return');
    //valmustempty($('#EditWCSL').val(), 'Please empty Doc WCSL');
    //valmustempty($('#EditStoreSupplyDate').val(), 'Please empty Store Supply Date');
    //valmustempty($('#EditWO').val(), 'Please empty workorder');
    //valmustempty($('#EditWCSL').val(), 'Please empty Doc WCSL');

}

function ValidationWOCSales500() {
    //valcantempty($('#EditKAL').val(), 'Please fill KAL');
    //valcantempty($('#EditAltPN').val(), 'Please fill Alternate Part Number');
    //valcantempty($('#EditNewStatus').val(), 'Please Select New Status');
    //valcantempty($('#EditStore').val(), 'Please select Store');
    //valcantempty($('#EditSOS').val(), 'Please select SOS');
    //valcantempty($('#EditDocSales').val(), 'Please fill Doc Sales');
    //valcantempty($('#EditWO').val(), 'Please fill Workorder');

  
}

function ValidationWOCSales800() {
    valcantempty($('#EditKAL').val(), 'Please fill KAL');
    valcantempty($('#EditAltPN').val(), 'Please fill Alternate Part Number');
    valcantempty($('#EditNewStatus').val(), 'Please Select New Status');
    valcantempty($('#EditStore').val(), 'Please select Store');
    valcantempty($('#EditSOS').val(), 'Please select SOS');
    valcantempty($('#EditDocSales').val(), 'Please fill Doc Sales');
}

function ValidationTTC() {
    valcantempty($('#EditKAL').val(), 'Please fill KAL');
    valcantempty($('#EditAltPN').val(), 'Please fill Alternate Part Number');
    valcantempty($('#EditNewStatus').val(), 'Please Select New Status');
    valcantempty($('#EditStore').val(), 'Please select Store');
    valcantempty($('#EditSOS').val(), 'Please select SOS');
    valcantempty($('#EditDocSales').val(), 'Please fill Doc Sales');
    valcantempty($('#EditWCSL').val(), 'Please fill Doc WCSL');

}

function ValidationWIPSQJC() {
    valcantempty($('#EditKAL').val(), 'Please fill KAL');
    valcantempty($('#EditAltPN').val(), 'Please fill Alternate Part Number');
    valcantempty($('#EditNewStatus').val(), 'Please Select New Status');
    valcantempty($('#EditStore').val(), 'Please select Store');
    valcantempty($('#EditSOS').val(), 'Please select SOS');
    valcantempty($('#EditMO').val(), 'Please fill MO');
    valcantempty($('#EditDocSales').val(), 'Please fill Doc Sales');
    valcantempty($('#EditWCSL').val(), 'Please fill Doc WCSL');
    valcantempty($('#EditWO').val(), 'Please fill Workorder');

}

function ValidationST() {
    valcantempty($('#EditKAL').val(), 'Please fill KAL');
    valcantempty($('#EditAltPN').val(), 'Please fill Alternate Part Number');
    valcantempty($('#EditNewStatus').val(), 'Please Select New Status');
    valcantempty($('#EditStore').val(), 'Please select Store');
    valcantempty($('#EditSOS').val(), 'Please select SOS');
    valcantempty($('#EditRefDocTransfer').val(), 'Please fill Ref Doc Transfer');
    valcantempty($('#EditStockTransferDate').val(), 'Please fill Stock Transfer Date');

}
//end edit inventory

function valcantempty(value, msg) {
    if (value == "") {
        return true;
    }
}

function valmustempty(value, msg) {
    if (value != "") {
        sAlert('Validation Info', msg, 'info');
        return;
    }
}

function ClearForm() {
    $('#EditKAL').val('');
    $('#EditAltPN').val('');
    $('#EditRGNumber').val('');
    $('#EditWCSL').val('');
    $('#EditUsedSN').val('');
    $('#EditTUID').val('');
    $('#EditOldStatus').val('');
    $('#EditDocSales').val('');
    $('#EditStoreSupplyDate').val('');
    $('#EditEquipmentNo').val('');
    $('#EditMO').val('');
    $('#EditDocR').val('');
    $('#EditWO').val('');
    $('#EditCustomer').val('');
    $('#EditRefDocTransfer').val('');
    $('#EditStockTransferDate').val('');
}