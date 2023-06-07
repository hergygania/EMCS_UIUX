$(function () {
    var $tableDetail = $('#tableRMDetail');
    enableLink(false);
    initFilterSelect2();

    $("#btnFilter").click(function () {
        refreshDetail();
    });

    $("#btn-clear").click(function () {
        ClearFilter();
    });

    $(".detailDownloadExcel").click(function () {
        enableLink(false);
        $.ajax({
            url: "DownloadRegulationDeatilToExcel",
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

    refreshDetail = function () {
        enableLink(false);

        window.pis.table({
            objTable: $tableDetail,
            urlSearch: '/imex/RegulationManagDetailPage',
            urlPaging: '/imex/RegulationManagDetailPageXt',
            searchParams: {
                ListCodePermitCategory: $("#ListCodePermitCategory").val() == null ? [] : $("#ListCodePermitCategory").val(),
                ListHSCode: $("#HSCode").val() == null ? [] : $("#HSCode").val(),
                ListOM: $("#OM").val() == null ? [] : $("#OM").val(),
                Regulation: $("#Regulation").val()
            },
            autoLoad: true
        });
    }

    $tableDetail.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbarDetail',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: false,
        showRefresh: false,
        smartDisplay: false,
        pageSize: '5',
        showExport: true,
        exportTypes: ['excel'],
        exportOptions: {
            ignoreColumn: [0],
            fileName: 'file.xls'
        },
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'NoPermitCategory',
            title: 'No Permit Category',
            halign: 'center',
            align: 'left',
            sortable: true,
            visible: false
        }, {
            field: 'CodePermitCategory',
            title: 'Code Permit Category',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'PermitCategoryName',
            title: 'Permit Category',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'HSCode',
		    title: 'HS Code',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Lartas',
		    title: 'Lartas',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		}, {
		    field: 'Permit',
		    title: 'Permit',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		}, {
		    field: 'Regulation',
		    title: 'Regulation',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		}, {
		    field: 'Date',
		    title: 'Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: 'dateFormatter',
		}, {
		    field: 'Description',
		    title: 'Description',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		}, {
		    field: 'OMCode',
		    title: 'Order Method',
		    sortable: true,
		    visible: true,
		    align: 'center',
		    formatter: formatterOM
		}, {
		    field: 'ID',
		    title: 'Id',
		    sortable: true,
		    visible: false
		}]
    });
});

function toggleIcon(e) {
    $(e.target)
        .prev('.panel-heading')
        .find(".more-less")
        .toggleClass('fa-chevron-down');
}
$('.panel-group').on('hidden.bs.collapse', toggleIcon);
$('.panel-group').on('shown.bs.collapse', toggleIcon);

function initFilterSelect2() {
    var HSCode = $("#HSCode").val();
    var OM = $("#OM").val();
    var CodePermitCategory = $("#CodePermitCategory").val();

    $("#OM").select2();
    $("#ListCodePermitCategory").select2();

    var hscode_iud = new mySelect2({
        id: 'HSCode',
        url: '/Picker/Select2HsCodeName',
        minimumInputLength: 1
    }).load();
    
    $.getJSON("GetDataFilterSelect2", function (json) {
        $("#OM").select2({
            width: '95%',
            placeholder: '-- Choose Business --',
            data: json.dataOM,
            width: "100%"
        });

        $("#ListCodePermitCategory").select2({
            width: '95%',
            placeholder: '-- Choose Business --',
            data: json.dataPermitCategory,
            width: "95%"
        });

        $("#HSCode").val(HSCode).change();
        $("#OM").val(OM).change();
        $("#ListCodePermitCategory").val(CodePermitCategory).change();
    }).done(function (data) {
        $(".more-less").click()
        refreshDetail();
    });
}

function ClearFilter() {
    $("#ListCodePermitCategory").val(null).change();
    $("#HSCode").val(null).change();
    $("#OM").val(null).change();
    $("#Regulation").val('');
}

function formatterOM(value, row, index) {
    return '<div style="color:red;">' + value + '</div>'
}