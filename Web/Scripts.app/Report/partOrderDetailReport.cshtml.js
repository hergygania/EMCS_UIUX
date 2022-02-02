var $table = $('#tabelPartOrderDetailReport');

$(function () {
    enableLink(false);

    $('.cal').click(function () {
        $('#EntryDate').focus();
    });

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
                field: 'InvoiceNo',
                title: 'Invoice No',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'InvoiceDate',
                title: 'Invoice Date',
                halign: 'center',
                align: 'left',
                formatter: 'dateFormatter',
                sortable: true
            }, {
                field: 'PrimPSO',
                title: 'Prim PSO',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'PartsNumber',
                title: 'Parts Number',
                halign: 'center',
                align: 'left',
                sortable: true,
            },
            {
                field: 'COO',
                title: 'COO',
                halign: 'center',
                align: 'right',
                sortable: true,
            },
            {
                field: 'COODescription',
                title: 'COO Description',
                halign: 'center',
                align: 'left',
                sortable: true,
            }, {
                field: 'InvoiceItemNo',
                title: 'Invoice Item No',
                halign: 'center',
                align: 'left',
                sortable: true,
            }, {
                field: 'PartsDescriptionShort',
                title: 'PartsDescription Short',
                halign: 'center',
                align: 'left',
                sortable: true,
            }, {
                field: 'InvoiceItemQty',
                title: 'Invoice Item Qty',
              formatter: 'formatNumber',
                  halign: 'center',
                align: 'right',
                sortable: true,
            }, {
                field: 'CustomerReff',
                title: 'Customer Reff',
                halign: 'center',
                align: 'left',
                sortable: true,
            }, {
                field: 'PartGrossWeight',
                formatter: 'formatNumber',
                title: 'Part Gross Weight',
                halign: 'center',
                align: 'right',
                sortable: true,
            },
            {
                field: 'ChargesDiscountAmount',
                title: 'Charges Discount Amount',
                halign: 'center',
                align: 'right',
                sortable: true,
            }, {
                field: 'BECode',
                title: 'BE Code',
                halign: 'center',
                align: 'left',
                sortable: true,
            }, {
                field: 'OrderCLSCode',
                title: 'Order CLS Code',
                halign: 'center',
                align: 'left',
                sortable: true,
            },
             {
                 field: 'Profile',
                 title: 'Profile',
                 halign: 'center',
                 align: 'left',
                 sortable: true,
             }, {
                 field: 'UnitPrice',
                 title: 'Unit Price',
                 formatter: 'formatNumber',
                 halign: 'center',
                 align: 'right',
                 sortable: true,
             }, {
                 field: 'OMID',
                 title: 'OM ID',
                 halign: 'center',
                 align: 'left',
                 sortable: true,
             },
          
             {
                 field: 'StoreName',
                 title: 'Store Name',
                 halign: 'center',
                 align: 'left',
                 sortable: true,
             },
             {
                 field: 'HubName',
                 title: 'Hub Name',
                 halign: 'center',
                 align: 'left',
                 sortable: true,
             },
             {
                 field: 'AreaName',
                 title: 'Area Name',
                 halign: 'center',
                 align: 'left',
                 sortable: true,
             },
        ]
    });

    $('#btn-clear').click(function () {
        $('#FilterBy').val('', 'ALL').change();
        $('#StoreNumber').val('', 'ALL').change();
        $('#Coo').val('', 'ALL').change();
        $('#CaseNo').val('');
        $('#InvoiceNo').val('');
        $('#EntryDate').val('');
        $('#PartNumber').val('');
        $('#PartName').val('');
        $('#CustomerReff').val('');
        $('#SOS').val('');
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
        var caseNo = $('#CaseNo').val();
        var partNumber = $('#PartNumber').val();
        var partName = $('#PartName').val();
        var invoiceNo = $('#InvoiceNo').val();
        var customerReff = $('#CustomerReff').val();
        var sos = $('#SOS').val();
        var coo = $('#Coo').val();
        var storeNo = $('#StoreNumber').val();
        var startData = _starDate;
        var endDate = _endDate;

        window.pis.table({
            objTable: $table,
            urlSearch: '/report/PartOrderDetailReportPage',
            urlPaging: '/report/PartOrderDetailReportPageXt',
            searchParams: {
                groupType: _groupType,
                filterBy: filterBy,
                storeNo: storeNo,
                invoiceDateStart: startData,
                invoiceDateEnd: endDate,
                invoiceNo: invoiceNo,
                caseNo: caseNo,
                partNumber: partNumber,
                partName: partName,
                customerReff: customerReff,
                sos: sos,
                coo: coo,
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
        var filterBy = $('#FilterBy').val();
        var caseNo = $('#CaseNo').val();
        var partNumber = $('#PartNumber').val();
        var partName = $('#PartName').val();
        var invoiceNo = $('#InvoiceNo').val();
        var customerReff = $('#CustomerReff').val();
        var sos = $('#SOS').val();
        var coo = $('#Coo').val();
        var storeNo = $('#StoreNumber').val();
        var startData = _starDate;
        var endDate = _endDate;
        window.open("/Report/ExportToExcelPartOrderDetail?" +
            "groupType=" + _groupType +
            "&filterBy=" + filterBy +
            "&storeNo=" + storeNo +
            "&invoiceNo=" + invoiceNo +
            "&startDate=" + startData +
            "&endDate=" + endDate +
            "&caseNo=" + caseNo+
            "&partNo=" + partNumber +
            "&partName=" + partName+
            "&coo=" + coo+
            "&customerReff=" + customerReff+
            "&sos=" + sos
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

function statusFormatter(value, row, index) {
    if (value == "1") {
        return "<span class='label label-success'>ACTIVE</span>";
    } else {
        return "<span class='label label-danger'>DEACTIVE</span>";
    }
}
function yesNOFormatter(value, row, index) {
    if (value == "1") {
        return "Yes";
    } else {
        return "No";
    }
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