var $table = $('#tabelDocumentAmend');

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
            field: 'amdoc_Type',
            title: 'Type',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'amdoc_ST1',
            title: 'Source WO',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'amdoc_QYHNDST1',
            title: 'Qty On Hand',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'amdoc_TAST1',
            title: 'Stock',
            halign: 'center',
            align: 'left',
            sortable: true,
        }, {
            field: 'amdoc_StatusStockST1',
            title: 'Status Stock',
            halign: 'center',
            align: 'left',
            sortable: true,
        }, {
            field: 'amdoc_STNo',
            title: 'Plant',
            halign: 'center',
            align: 'left',
            sortable: true,
        }, {
            field: 'amdoc_DocNo',
            title: 'PO Number',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'amdoc_SOS',
            title: 'Dealer Net',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'amdoc_PartNo',
            title: 'Part No',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'amdoc_Qty1',
            title: 'Quantity',
            halign: 'center',
            formatter: 'formatNumber',
            align: 'right',
            sortable: true
        },
        //{
        //    field: 'amdoc_StatusStockSTNo',
        //    title: 'Status Stock ST No',
        //    halign: 'center',
        //    align: 'left',
        //    sortable: true
        //},
        {
            field: 'amdoc_LSACJ8',
            title: 'Activity Date',
            halign: 'center',
            //formatter: 'julianToDateFormatter',
           // align: 'right',
            sortable: true
        }, {
            field: 'amdoc_QYHND',
            title: 'Stock QTY On Hand',
            halign: 'center',
            formatter: 'formatNumber',
            align: 'right',
            sortable: true
        }, {
            field: 'amdoc_QYOR',
            title: 'Outstanding QTY',
            halign: 'center',
            formatter: 'formatNumber',
            align: 'right',
            sortable: true
        }, {
            field: 'amdoc_QYPCS',
            title: 'QTY In Process',
            halign: 'center',
            formatter: 'formatNumber',
            align: 'right',
            sortable: true
        }, {
            field: 'amdoc_RTQYPC',
            title: 'Return QTY In Process',
            halign: 'center',
            formatter: 'formatNumber',
            align: 'right',
            sortable: true
        }, {
            field: 'amdoc_Note',
            title: 'Note',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'amdoc_UserID',
            title: 'SO Created By',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'amdoc_PXQY2',
            title: 'Pack QTY',
            halign: 'center',
            formatter: 'formatNumber',
            align: 'right',
            sortable: true
        }, ]
    });


    $("#btnFilter").click(function () {
        var _starDate, _endDate, _groupType;
        //if ($('#EntryDate').val() != '') {
        //    _starDate = $('#EntryDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
        //    _endDate = $('#EntryDate').data('daterangepicker').endDate.format('MM/DD/YYYY');
        //}
        if ($('#hub:checked').length > 0) {
            _groupType = $('#hub').val();
        } else {
            _groupType = $('#area').val();

        }
        var filterBy = $('#FilterBy').val();
        
        window.pis.table({
            objTable: $table,
            urlSearch: '/report/DocumentAmendPage',
            urlPaging: '/report/DocumentAmendPageXt',
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

        window.open("/Report/ExportToExcelDocAmend?" +
            "groupType=" + groupType +
            "&filterBy=" + filterBy +
            "&storeNumber=" + storeNumber +
            "&custId=" + custId
        );
    });



    $("#mySearch").insertBefore($("[name=refresh]"))
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

function excelSubmit() {
    $('.fixed-table-loading').show();
    enableLink(false);
    $('#submitExcel').submit();
}
