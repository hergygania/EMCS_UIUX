var $table = $('#tabelPartOrderCaseReport');

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
                field: 'CaseNo',
                title: 'Case No',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'ShippingIDASN',
                title: 'Shipping IDASN',
                halign: 'center',
                align: 'left',
                sortable: true,
            },
            {
                field: 'CaseType',
                title: 'Case Type',
                halign: 'center',
                align: 'left',
                sortable: true,
            },
            {
                field: 'CaseDescription',
                title: 'Case Description',
                halign: 'center',
                align: 'left',
                sortable: true,
            }, {
                field: 'WeightKG',
                title: 'Weight KG',
                halign: 'center',
                formatter: 'formatNumber',
                align: 'rigth',
                sortable: true,
            }, {
                field: 'LengthCM',
                title: 'Length CM',
                halign: 'center',
                formatter: 'formatNumber',
                align: 'right',
                sortable: true,
            }, {
                field: 'WideCM',
                title: 'Wide CM',
                formatter: 'formatNumber',
                halign: 'center',
                align: 'right',
                sortable: true,
            }, {
                field: 'HeightCM',
                title: 'Height CM',
                formatter: 'formatNumber',
                halign: 'center',
                align: 'right',
                sortable: true,
            }, {
                field: 'RouteID',
                title: 'Route ID',
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
        $('#CaseNo').val('');
        $('#CaseType').val('', 'ALL').change();
        $('#CaseDescription').val('', 'ALL').change();
        $('#InvoiceNo').val('');
        $('#EntryDate').val('');
        $('#WeightFrom').val('');
        $('#WeightTo').val('');
        $('#LengthFrom').val('');
        $('#LengthTo').val('');
        $('#WideFrom').val('');
        $('#WideTo').val('');
        $('#HeightFrom').val('');
        $('#HeightTo').val('');
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
        var caseType = $('#CaseType').val();
        var caseDesc = $('#CaseDescription').val();
        var invoiceNo = $('#InvoiceNo').val();
        var weightFrom = $('#WeightFrom').val();
        var weightTo = $('#WeightTo').val();
        var lengthFrom = $('#LengthFrom').val();
        var lengthTo = $('#LengthTo').val();
        var wideFrom = $('#WideFrom').val();
        var wideTo = $('#WideTo').val();
        var heightFrom = $('#HeightFrom').val();
        var heightTo = $('#HeightTo').val();
        var storeNo = $('#StoreNumber').val();
        window.pis.table({
            objTable: $table,
            urlSearch: '/report/PartOrderCaseReportPage',
            urlPaging: '/report/PartOrderCaseReportPageXt',
            searchParams: {
                GroupType: _groupType,
                FilterBy: filterBy,
                StoreNumber: storeNo,
                InvoiceStartDate: _starDate,
                InvoiceEndDate: _endDate,
                InvoiceNo: invoiceNo,
                CaseNo: caseNo,
                CaseType: caseType,
                CaseDescription: caseDesc,
                WeightFrom: weightFrom,
                WeightTo: weightTo,
                LengthFrom: lengthFrom,
                LengthTo: lengthTo,
                WideFrom: wideFrom,
                WideTo: wideTo,
                HeightFrom: heightFrom,
                HeightTo: heightTo,
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
        var caseType = $('#CaseType').val();
        var caseDesc = $('#CaseDescription').val();
        var invoiceNo = $('#InvoiceNo').val();
        var weightFrom = $('#WeightFrom').val();
        var weightTo = $('#WeightTo').val();
        var lengthFrom = $('#LengthFrom').val();
        var lengthTo = $('#LengthTo').val();
        var wideFrom = $('#WideFrom').val();
        var wideTo = $('#WideTo').val();
        var heightFrom = $('#HeightFrom').val();
        var heightTo = $('#HeightTo').val();
        var storeNo = $('#StoreNumber').val();

        window.open("/Report/ExportToExcelPartOrderCase?" +
            "groupType=" + _groupType +
            "&filterBy=" + filterBy +
            "&storeNo=" + storeNo +
            "&caseNo=" + caseNo +
            "&caseType=" + caseType +
            "&caseDescription=" + caseDesc +
            "&invoiceNo=" + invoiceNo +
            "&invoiceStartDate=" + _starDate +
            "&invoiceEndDate=" + _endDate +
            "&weightFrom=" + weightFrom +
            "&weightTo=" + weightTo +
            "&lengthFrom=" + lengthFrom +
            "&lengthTo=" + lengthTo +
            "&wideFrom=" + wideFrom +
            "&wideTo=" + wideTo +
            "&heightFrom=" + heightFrom +
            "&heightTo=" + heightTo);
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