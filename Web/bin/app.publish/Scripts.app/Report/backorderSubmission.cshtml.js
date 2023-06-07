var $table = $('#tabelbackorder');

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
            { field: 'bcksms_Store', title: 'Store', align: 'center', width: '70px', sortable: true },
            { field: 'bcksms_RefDoc', title: 'Reference Document', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'bcksms_CustPONo', title: 'Customer PO Number', halign: 'center', align: 'left', width: '200px', sortable: true },
            { field: 'bcksms_DocDate', title: 'Document Date', halign: 'center', align: 'right', width: '130px', sortable: true, formatter: 'dateFormatter' },
            { field: 'bcksms_SOS', title: 'SOS', halign: 'center', align: 'left', width: '65px', sortable: true },
            { field: 'bcksms_PartNo', title: 'Part No', halign: 'center', align: 'left', width: '85px', sortable: true },
            { field: 'bcksms_CustName', title: 'Customer Name', halign: 'center', align: 'left', width: '380px', sortable: true },
            { field: 'bcksms_Description', title: 'Description', halign: 'center', align: 'left', width: '150px', sortable: true },
            { field: 'bcksms_BackorderQty', title: 'Quantity', halign: 'center', align: 'right', width: '100px', sortable: true },
            { field: 'bcksms_Facility', title: 'Facility', halign: 'center', align: 'right', width: '85px', sortable: true },
            { field: 'bcksms_TransferOrdNo', title: 'Transfer Orde', halign: 'center', align: 'left', width: '120px', sortable: true },
            { field: 'bcksms_FacOrdNo', title: 'FAC Order No', halign: 'center', align: 'left', width: '120px', sortable: true },
            { field: 'bcksms_Comments', title: 'Comments', halign: 'center', align: 'left', width: '150px', sortable: true },
            { field: 'bcksms_CustID', title: 'Customer Id', halign: 'center', align: 'left', width: '110px', sortable: true }
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
            urlSearch: '/report/BackorderSubmissionPage',
            urlPaging: '/report/BackorderSubmissionPageXt',
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
        var filterBy = $('#FilterBy').val();
        var groupType = _groupType;
        var storeNumber = $('#StoreNumber').val() == null ? '' : $('#StoreNumber').val();

        var custId = $('#CustomerId').val();

        window.open("/Report/ExportToExcelBackorde?" +
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
