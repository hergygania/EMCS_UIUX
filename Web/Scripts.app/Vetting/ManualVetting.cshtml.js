var $table = $('#tableManualVetting');

$(function () {
    $("#btnFilter").click(function () {
        refreshDetail();
    });

    $("#btn-clear").click(function () {
        ClearFilter();
    });

    $(".downloadExcel").click(function () {    
        enableLink(false);
        $.ajax({
            url: "DownloadManualVettingToExcel",
            type: 'GET',
            success: function (guid) {
                enableLink(true);
                window.open('DownloadToExcel?guid=' + guid, '_blank');
            },
            cache: false,
            contentType: false,
            processData: false
        });
    });

    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
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
        formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
        columns: [
            { field: 'PRIMPSO', title: 'PRIMPSO', halign: 'center', align: 'center', sortable: true },
            { field: 'PartNumber', title: 'Part Number', halign: 'center', align: 'left', sortable: true },
            { field: 'ManufacturingCode', title: 'Manufacturing</br>Code', halign: 'center', align: 'left', sortable: false },
            { field: 'PartName', title: 'Part Name', halign: 'center', align: 'center', sortable: false, },
            { field: 'CustomerRef', title: 'Customer Ref', halign: 'center', align: 'center', sortable: false, },
            { field: 'CustomerCode', title: 'Customer Code', halign: 'center', align: 'center', sortable: false, },
            { field: 'Status', title: 'Status', halign: 'center', align: 'center', sortable: false, },
            { field: 'OrderClassCode', title: 'Order</br>Class Code', halign: 'center', align: 'center', sortable: false, },
            { field: 'ProfileNumber', title: 'Profile Number', halign: 'center', align: 'center', sortable: false, },
            { field: 'OMCode', title: 'Order Method', halign: 'center', align: 'center', sortable: false, },
            { field: 'RemarksName', title: 'Remarks', halign: 'center', align: 'center', sortable: false, }
        ]
    });

    refreshDetail = function () {
        window.pis.table({
            objTable: $table,
            urlSearch: '/vetting-process/ManualVettingPage',
            urlPaging: '/vetting-process/ManualVettingPageXt',
            autoLoad: true,
            searchParams: {
                selHSCodeList_Ids: $("#selHSCodeList_Ids").val() == null ? [] : $("#selHSCodeList_Ids").val(),
                selPartsList_Ids: $("#selPartsList_Ids").val() == null ? [] : $("#selPartsList_Ids").val(),
                selOrderMethods: $("#selOrderMethods").val() == null ? [] : $("#selOrderMethods").val()
            },
        });
    }

    //refreshDetail();
    initFilterSelect2();
});

function initFilterSelect2() {
    $("#OM").select2();
    $("#HSID").select2();
    helpers.buildDropdown('/Picker/GetListOMCode', $('#selOrderMethods'), true, null);

    var partNumber = new mySelect2({
        id: 'selPartsList_Ids',
        url: '/Picker/SelectToPartNumber'
    }).load();

    var hscode_iud = new mySelect2({
        id: 'selHSCodeList_Ids',
        url: '/Picker/Select2HsCodeName',
        minimumInputLength: 1
    }).load();

    $(".more-less").click();
}

function toggleIcon(e) {
    $(e.target)
        .prev('.panel-heading')
        .find(".more-less")
        .toggleClass('fa-chevron-down');
}

$('.panel-group').on('hidden.bs.collapse', toggleIcon);
$('.panel-group').on('shown.bs.collapse', toggleIcon);

function ClearFilter() {
    $("#selPartsList_Ids").val(null).change();
    $("#Select2HSCode").val(null).change();
    $("#selOrderMethods").val(null).change();
}


$("form#submitExcel").submit(function () {
    var dt = { "rows": {}, "total": 0 };
    $table.bootstrapTable('load', dt);
    $('#uploadResult').empty();

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
                //$("#btnFilter").trigger('click');
                //$("[name=refresh]").trigger('click');
                refreshDetail();
            }
            else {
                if (result.Msg != undefined)
                    sAlert('Failed', result.Msg, 'error');
                if (result.Data != undefined) {

                }
                $("[name=refresh]").trigger('click');
            }

            $("#filexls").val("");
        },
        cache: false,
        contentType: false,
        processData: false
    });

    return false;
});