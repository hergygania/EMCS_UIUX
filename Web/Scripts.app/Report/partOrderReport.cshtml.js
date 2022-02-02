var $table = $('#tabelPartOrderReport');

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
                field: 'JCode',
                title: 'JCode',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'StoreNumber',
                title: 'Store Number',
                halign: 'center',
                align: 'left',
                sortable: true,
            },
            {
                field: 'TotalAmount',
                title: 'Total Amount',
                halign: 'center',
                formatter: 'formatNumber',
                align: 'right',
                sortable: true,
            },
            {
                field: 'TotalFOB',
                title: 'Total FOB',
                halign: 'center',
                formatter: 'formatNumber',
                align: 'right',
                sortable: true,
            }, {
                field: 'IsHazardous',
                title: 'Is Hazardous',
                halign: 'center',
                align: 'left',
                sortable: true,
                formatter: yesNOFormatter,
            }, {
                field: 'ServiceCharges',
                title: 'Service Charges',
                halign: 'center',
                align: 'right',
                sortable: true,
            }, {
                field: 'CoreDeposit',
                title: 'Core Deposit',
                halign: 'center',
                formatter: 'formatNumber',
                align: 'right',
                sortable: true,
            }, {
                field: 'OtherCharges',
                title: 'Other Charges',
                halign: 'center',
                formatter: 'formatNumber',
                align: 'right',
                sortable: true,
            }, {
                field: 'FreightCharges',
                title: 'Freight Charges',
                halign: 'center',
                formatter: 'formatNumber',
                align: 'right',
                sortable: true,
            },
            {
                field: 'ShippingIDASN',
                title: 'Shipping IDASN',
                halign: 'center',
                align: 'left',
                sortable: true,
            }, {
                field: 'AgreementType',
                title: 'Agreement Type',
                halign: 'center',
                align: 'left',
                sortable: true,
            }, {
                field: 'VettingRoute',
                title: 'Vetting Route',
                halign: 'center',
                align: 'left',
                sortable: true,
            },
             {
                 field: 'SurveyDate',
                 title: 'Survey Date',
                 formatter: 'dateFormatter',
                 halign: 'center',
                 align: 'left',
                 sortable: true,
             }, {
                 field: 'Status',
                 title: 'Status',
                 formatter: statusFormatter,
                 halign: 'center',
                 align: 'left',
                 sortable: true,
             }, {
                 field: 'SOS',
                 title: 'SOS',
                 halign: 'center',
                 align: 'left',
                 sortable: true,
             },
            {
                field: 'HPLReceiptDate',
                title: 'HPL Receipt Date',
                formatter: 'dateFormatter',
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
        $('#JCode').val('', 'ALL').change();
        $('#AgreementType').val('', 'ALL').change();
        $('#ShippingInstruction').val('', 'ALL').change();
        $('#InvoiceNo').val('');
        $('#SOS').val('');
        $('#EntryDate').val('');
        $('#IsHazardous').val('', 'ALL').change();
    });

    $("#btnFilter").click(function () {
        var _starDate, _endDate, _groupType, _isHazardous;
        if ($('#EntryDate').val() != '') {
            _starDate = $('#EntryDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
            _endDate = $('#EntryDate').data('daterangepicker').endDate.format('MM/DD/YYYY');
        }
        if ($('#hub:checked').length > 0) {
            _groupType = $('#hub').val();
        } else {
            _groupType = $('#area').val();

        }
        var isHazard = $('#IsHazardous').val();
        if (isHazard != "") {
            if (isHazard == 1) {
                _isHazardous = true;
            } else {
                _isHazardous = false;
            }
        } else {
            _isHazardous = "";
        }
        var filterBy = $('#FilterBy').val();

        window.pis.table({
            objTable: $table,
            urlSearch: '/report/PartOrderReportPage',
            urlPaging: '/report/PartOrderReportPageXt',
            searchParams: {
                GroupType: _groupType,
                FilterBy: filterBy,
                StoreNumber: $('#StoreNumber').val(),
                JCode: $('#JCode').val(),
                ShippingInstruction: $('#ShippingInstruction').val(),
                AgreementType: $('#AgreementType').val(),
                SOS: $('#SOS').val(),
                InvoiceStartDate: _starDate,
                InvoiceEndDate: _endDate,
                IsHazardous: _isHazardous,
                InvoiceNo: $('#InvoiceNo').val()
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
        var storeNumber = $('#StoreNumber').val();
        var startDate = _starDate;
        var endDate = _endDate;
        var filterBy = $('#FilterBy').val();
        var  jCode=$('#JCode').val();
        var shippingInstruction = $('#ShippingInstruction').val();
        var invoiceNo = $('#InvoiceNo').val();
        var isHazard = $('#IsHazardous').val();
        if (isHazard != "") {
            if (isHazard == 1) {
                _isHazardous = true;
            } else {
                _isHazardous = false;
            }
        } else {
            _isHazardous = "";
        }
        var agreementType = $('#AgreementType').val();
        var SOS = $('#SOS').val();



        window.open("/Report/ExportToExcelPartOrder?" +
            "invoiceNo=" + invoiceNo +
            "&invoiceStartDate=" + startDate +
            "&invoiceEndDate=" + endDate +
            "&groupType=" + groupType +
            "&filterBy=" + filterBy +
            "&storeNumber=" + storeNumber +
            "&jCode=" + jCode +
            "&shippingInstruction=" + shippingInstruction +
            "&isHazardous=" + _isHazardous+
            "&agreementType=" + agreementType +
            "&sos=" + SOS
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