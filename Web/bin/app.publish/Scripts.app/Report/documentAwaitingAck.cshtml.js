var $table = $('#tabelDocument');

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
            { field: 'rack_Store', title: 'Store', align: 'center', width: '70px', sortable: true },
            { field: 'rack_RefDoc', title: 'Reference Document', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'rack_CustPONo', title: 'Customer PO Number', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'rack_CustName', title: 'Customer Name', halign: 'center', align: 'left', width: '320px', sortable: true },
            { field: 'rack_DocValue', title: 'Document Value', formatter: 'formatNumber', halign: 'center', align: 'left', width: '135px', sortable: true },
            { field: 'rack_DocDate', title: 'Document Date', halign: 'center', align: 'right', width: '130px', sortable: true, formatter: 'dateFormatter' }
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
            urlSearch: '/report/DocumentAwaitingAckPage',
            urlPaging: '/report/DocumentAwaitingAckPageXt',
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

        window.open("/Report/ExportToExcelDocument?" +
            "groupType=" + groupType +
            "&filterBy=" + filterBy +
            "&storeNumber=" + storeNumber +
            "&custId=" + custId);

    });





    $("#mySearch").insertBefore($("[name=refresh]"));
    enableLink(true);
});


function operateFormatter(value, row, index) {
    return [
        '<div class="btn-group" style="width:123px;white-space:nowrap; text-align:center">',
        '<button type="button" class="btn btn-xs btn-primary edit" title="Edit"><i class="fa fa-pencil"></i> Edit</button>',
        '<button type="button" class="btn btn-xs btn-info detail" title="Detail"><i class="fa fa-search-plus"></i> Detail</button>',
        '</div>'
    ].join('');
}


window.operateEvents = {

};


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

        if (!$("form#" + this.id).valid()) {
            return false;
        }

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
                } else {
                    if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
                    $('#progress').hide();
                    //$('#myModalContent').html(result);
                    //bindForm(dialog);
                }
            }
        });
        return false;
    });
}

;

function excelSubmit() {
    $('.fixed-table-loading').show();
    enableLink(false);
    $('#submitExcel').submit();
}

