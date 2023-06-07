var $table = $('#tabelWHDocumentReprint');

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
            field: 'whdocrep_DocNo',
            title: 'Document Number',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'whdocrep_SOS',
            title: 'SOS',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'whdocrep_PartNo',
            title: 'Part Number',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'whdocrep_Description',
            title: 'Description',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'whdocrep_PackageQty',
            title: 'Document Date',
            halign: 'center',
            align: 'left',
            sortable: true,
        }, {
            field: 'whdocrep_Qty',
            title: 'Quantity',
            halign: 'center',
            align: 'left',
            sortable: true,
        }, {
            field: 'whdocrep_Binloc',
            title: 'Bin Loc',
            halign: 'center',
            align: 'left',
            sortable: true,
        }, {
            field: 'whdocrep_SellingPrice',
            title: 'Selling Price',
            halign: 'center',
            align: 'left',
            sortable: true,
        }, {
            field: 'whdocrep_ReprintDate',
            title: 'Reprint Date',
            halign: 'center',
            align: 'left',
            formatter: 'dateFormatter',
            sortable: true,
        }, {
            field: 'whdocrep_UserID',
            title: 'User Id',
            halign: 'center',
            align: 'left',
            sortable: true,
        }]
    });


    $("#btnFilter").click(function () {
        var _starDate, _endDate;
        if ($('#ReprintDate').val() != '') {
            _starDate = $('#ReprintDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
            _endDate = $('#ReprintDate').data('daterangepicker').endDate.format('MM/DD/YYYY');
        }
        window.pis.table({
            objTable: $table,
            urlSearch: '/report/WHDocumentReprintPage',
            urlPaging: '/report/WHDocumentReprintPageXt',
            searchParams: {
                DocNo: $('#DocNo').val(),
                Sos: $('#Sos').val(),
                PartNo: $('#PartNo').val(),
                StartDate: _starDate,
                EndDate: _endDate,

            },
            autoLoad: true
        });
    });

    $(".downloadExcel").click(function () {
        $table.pagination = false;
        $(".table2excel").table2excel({
            exclude: ".noExl",
            name: "WHDocumentReprint",
            filename: "WHDocumentReprint",
            
        });
    });

    //$('.downloadExcel').on('click', function () {
    //    $table.bootstrapTable('refreshOptions', {
    //        exportDataType: "all"
    //    });
        
    //    $(".table2excel").table2excel({
    ////        exclude: ".noExl",
    ////        name: "WHDocumentAwaiting",
    ////        filename: "WHDocumentAwaiting"
    ////    });
    //});

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
