﻿var $table = $('#tabelLongPod');

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
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [
        {
            title: 'No',
            halign: 'center',
            align: 'right',
            width: '3%',
            formatter: runningFormatter
        }, {
            field: 'lpod_StoreNo',
            title: 'Plant',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'lpod_SOS',
            title: 'SLoc',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'lpod_PartsNumber',
            title: 'Parts Number',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'lpod_Description',
            title: 'Description',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'lpod_Document',
            title: 'PO Number',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'lpod_QTY',
            title: 'QTY',
            halign: 'center',
            formatter: 'formatNumber',
            align: 'right',
            sortable: true
        }, {
            field: 'lpod_OrderDate',
            title: 'Order Date',
            formatter: 'dateFormatter',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'lpod_InvoiceNo',
            title: 'Invoice No',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'lpod_InvoiceDate',
            formatter: 'dateFormatter',
            title: 'Invoice Date',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'lpod_PODDate',
            title: 'POD Date',
            formatter: 'dateFormatter',
            halign: 'center',
            align: 'left',
            sortable: true
        },  {
            field: 'lpod_UNCS',
            title: 'Dealer Net',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'lpod_Weight',
            title: 'Weight',
            halign: 'center',
            formatter: 'formatNumber',
            align: 'right',
            sortable: true
        }, {
            field: 'lpod_ASNNo',
            title: 'ASN No',
            halign: 'center',
            align: 'left',
            sortable: true
        }]
    });


    $("#btnFilter").click(function () {
        var _starDate, _endDate, _groupType;
        if ($('#EntryDate').val() != '') {
            _starDate = $('#EntryDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
            _endDate = $('#EntryDate').data('daterangepicker').endDate.format('MM/DD/YYYY');
        }
        if ($('#hub:checked').length > 0) {
            _groupType = $('#hub').val();
        } else {
            _groupType = $('#area').val();

        }
        var filterBy = $('#FilterBy').val();
        window.pis.table({
            objTable: $table,
            urlSearch: '/report/LongPodPage',
            urlPaging: '/report/LongPodPageXt',
            searchParams: {
                GroupType: _groupType,
                FilterBy: filterBy,
                StoreNumber: $('#StoreNumber').val(),
                StartDate: _starDate,
                EndDate: _endDate,
            },
            autoLoad: true
        });
    });

    $(".downloadExcel").click(function () {
        var _starDate, _endDate, _groupType;
        if ($('#EntryDate').val() != '') {
            _starDate = $('#EntryDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
            _endDate = $('#EntryDate').data('daterangepicker').endDate.format('MM/DD/YYYY');
        }
        if ($('#hub:checked').length > 0) {
            _groupType = $('#hub').val();
        } else {
            _groupType = $('#area').val();

        }
        var groupType = _groupType;
        var storeNumber = $('#StoreNumber').val() == null ? '' : $('#StoreNumber').val();
        var startDate = _starDate;
        var endDate = _endDate;
        var filterBy = $('#FilterBy').val();
        window.open("/Report/ExportToExcelLongPod?" +
            "groupType=" + groupType +
            "&filterBy=" + filterBy +
            "&storeNumber=" + storeNumber +
            "&startDate=" + startDate +
            "&endDate=" + endDate
        );
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

