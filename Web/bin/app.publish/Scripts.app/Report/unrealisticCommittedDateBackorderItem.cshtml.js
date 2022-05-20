var $table = $('#tabelUnrealisticCommittedDateBackorderItem');

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
            { field: 'ucdbi_Store', title: 'Store', align: 'center', width: '70px', sortable: true },
            { field: 'ucdbi_RefDoc', title: 'Reference Document', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'ucdbi_CustPONo', title: 'Customer PO Number', halign: 'center', align: 'left', width: '200px', sortable: true },
            { field: 'ucdbi_DocDate', title: 'Document Date', formatter: 'formatterDate', halign: 'center', align: 'right', width: '130px', sortable: true },
            { field: 'ucdbi_DocValue', title: 'Document Value', halign: 'center', formatter: 'formatNumber', align: 'right', width: '135px', sortable: true },
            { field: 'ucdbi_GapOfDate', title: 'Gap Of Date', align: 'center', width: '110px', sortable: true },
            { field: 'ucdbi_CommittedDate', title: 'Commited Date', halign: 'center', align: 'right', width: '135px', sortable: true, formatter: 'dateFormatter' },
            { field: 'ucdbi_SOS', title: 'SOS', halign: 'center', align: 'left', width: '65px', sortable: true },
            { field: 'ucdbi_PartNo', title: 'Part No', halign: 'center', align: 'left', width: '85px', sortable: true },
            { field: 'ucdbi_OrderMethod', title: 'Order Method', halign: 'center', align: 'left', width: '120px', sortable: true },
            { field: 'ucdbi_Facility', title: 'Facility', halign: 'center', align: 'left', width: '80px', sortable: true },
            { field: 'ucdbi_LeadTime', title: 'Lead Time', halign: 'center', align: 'left', width: '100px', sortable: true },
            { field: 'ucdbi_GapOfDate', title: 'Gap Of Date', halign: 'center', align: 'left', width: '110px', sortable: true },
            { field: 'ucdbi_CustID', title: 'Customer ID', halign: 'center', align: 'left', width: '110px', sortable: true },
            { field: 'ucdbi_CustName', title: 'Customer Name', halign: 'center', align: 'left', width: '380px', sortable: true }
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
            urlSearch: '/report/UnrealisticCommittedDateBackorderItemPage',
            urlPaging: '/report/UnrealisticCommittedDateBackorderItemPageXt',
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

        window.open("/Report/ExportToExcelCommitedDateBackorderIteme?" +
           "groupType=" + groupType +
            "&filterBy=" + filterBy +
            "&storeNumber=" + storeNumber +
            "&custId=" + custId
        );
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
    enableLink(true);
});

function formatterDate(dt) {
    return dt.replace(/(\d\d\d\d)(\d\d)(\d\d)/g, '$2/$3/$1');;
}

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
                    //$('#myModalContent').html(result);
                    //bindForm(dialog);
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
