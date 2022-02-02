var $table = $('#tabelBORespondTime');

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
                field: 'borsp_PartNo',
                title: 'Part No',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'borsp_Description',
                title: 'Description',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'borsp_PackageQty',
                title: 'Package Qty',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'borsp_Qty',
                title: 'Quantity',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'borsp_Binloc',
                title: 'Binloc',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'borsp_Weight',
                title: 'Total Weight',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'borsp_Width',
                title: 'Width',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'borsp_Height',
                title: 'Height',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'borsp_Length',
                title: 'Length',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'borsp_BOSubmissionDatetime',
                title: 'BO Submission',
                halign: 'center',
                align: 'left',
                sortable: true,
                formatter: 'dateFormatter'
            }, {
                field: 'borsp_PickupDate',
                title: 'Pickup Date',
                halign: 'center',
                align: 'left',
                sortable: true,
                formatter: 'dateFormatter'
            }, {
                field: 'borsp_Leadtime',
                title: 'Leadtime',
                halign: 'center',
                align: 'left',
                sortable: true,
            }, {
                field: 'borsp_BM',
                title: 'BM',
                halign: 'center',
                align: 'left',
                sortable: true,
            }]
    });


    $("#btnFilter").click(function () {
        var _starDate, _endDate;
        if ($('#PickupDate').val() != '') {
            _starDate = $('#PickupDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
            _endDate = $('#PickupDate').data('daterangepicker').endDate.format('MM/DD/YYYY');
        }
        window.pis.table({
            objTable: $table,
            urlSearch: '/report/BORespondTimePage',
            urlPaging: '/report/BORespondTimePageXt',
            searchParams: {
                PartNo: $('#PartNo').val(),
                Quantity: $('#Quantity').val(),
                BinLoc: $('#BinLoc').val(),
                Weight: $('#Weight').val(),
                Length: $('#Length').val(),
                Width: $('#Width').val(),
                Height: $('#Height').val(),
                PickupStartDate: _starDate,
                PickupEndDate: _endDate
            },
            autoLoad: true
        });
    });

    $(".downloadExcel").click(function () {
        var _starDate, _endDate;
        if ($('#PickupDate').val() != '') {
            _starDate = $('#PickupDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
            _endDate = $('#PickupDate').data('daterangepicker').endDate.format('MM/DD/YYYY');
        }

        var partNo = $('#PartNo').val();

        var quantity = $('#Quantity').val();

        var pickupStartDate = _starDate;

        var pickupEndDate = _endDate;

        var binLoc = $('#BinLoc').val();
        var weight = $('#Weight').val();
        var width = $('#Width').val();
        var height = $('#Height').val();


        window.open("/Report/ExportToExcelBORespondTime?" +
            "partNo=" + partNo +
            "&quantity=" + quantity +
            "&binLoc=" + binLoc +
            "&weight=" + weight +
            "&length=" + length +
            "&width=" + width +
            "&pickupStartDate=" + pickupStartDate +
            "&pickupStartDate=" + pickupEndDate
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