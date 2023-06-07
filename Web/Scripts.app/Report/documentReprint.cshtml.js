var $table = $('#tabelDocumentReprint');

$(function () {
    enableLink(false);
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
        columns: [
            { title: 'No', align: 'center', width: '45px', formatter: runningFormatter },
            { field: 'docrep_Store', title: 'Store', align: 'center', width: '70px', sortable: true },
            { field: 'docrep_RefDoc', title: 'Reference Document', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'docrep_CustPONo', title: 'Customer PO Number', halign: 'center', align: 'left', width: '200px', sortable: true },
            { field: 'docrep_DocDate', title: 'Document Date', halign: 'center', align: 'right', sortable: true, width: '130px', formatter: 'dateFormatter' },
            { field: 'docrep_DocValue', title: 'Document Value', halign: 'center', align: 'right', width: '150px', formatter: 'formatNumber', sortable: true },
            { field: 'docrep_CustID', title: 'Customer ID', halign: 'center', align: 'left', width: '110px', sortable: true },
            { field: 'docrep_CustName', title: 'Customer Name', halign: 'center', align: 'left', width: '380px', sortable: true },
            { field: 'docrep_PrintCount', title: 'Print Count', halign: 'center', align: 'right', width: '120px', sortable: true },
            { field: 'docrep_Remarks', title: 'Remarks', halign: 'center', align: 'left', width: '150px', sortable: true }
        ]
    });
    
    $("#btnFilter").click(function () {
        var _starDate, _endDate, _groupType;       
        if ($('#hub:checked').length > 0) {
            _groupType = $('#hub').val();
        } else {
            _groupType = $('#area').val();
        }
        var filterBy = $('#FilterBy').val();
        window.pis.table({
            objTable: $table,
            urlSearch: '/report/DocumentReprintPage',
            urlPaging: '/report/DocumentReprintPageXt',
            searchParams: {
                GroupType: _groupType,
                FilterBy: filterBy,
                StoreNumber: $('#StoreNumber').val(),
                CustomerId: $('#CustomerId').val(),
            },
            autoLoad: true
        });
    });

    $(".downloadExcel").click(function () {
        var _starDate, _endDate, _groupType;       
        if ($('#hub:checked').length > 0) {
            _groupType = $('#hub').val();
        } else {
            _groupType = $('#area').val();
        }
        var groupType = _groupType;
        var storeNumber = $('#StoreNumber').val() == null ? '' : $('#StoreNumber').val();       
        var custId = $('#CustomerId').val();
        var filterBy = $('#FilterBy').val();

        window.open("/Report/ExportToExcelDocReprint?" +
           "groupType=" + groupType +
            "&filterBy=" + filterBy +
            "&storeNumber=" + storeNumber +
            "&custId=" + custId
        );
    });

    $("#mySearch").insertBefore($("[name=refresh]"))
    enableLink(true);
});

$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
        enableLink(false);
        $('#myModalContent').load(this.href, function () {
            $('#myModalPlace').modal({ keyboard: true }, 'show');
            bindForm(this);
            enableLink(true);
        });
        return false;
    });
});

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        if (!$("form#" + this.id).valid()) { return false; }
        $('#progress').show();
        enableLink(false);
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                enableLink(true);
                if (result.Status == 0) {
                    if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
                    $('#myModalPlace').modal('hide');
                    $('#progress').hide();
                    $("#btnFilter").trigger('click');
                }
                else {
                    if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
                    $('#progress').hide();
                }
            }
        });
        return false;
    });
};

function excelSubmit() {
    $('.fixed-table-loading').show();
    enableLink(false);
    $('#submitExcel').submit();
}
