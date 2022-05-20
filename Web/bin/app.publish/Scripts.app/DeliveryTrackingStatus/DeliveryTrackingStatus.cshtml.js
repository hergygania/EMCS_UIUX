var $table = $('#tableDeliveryTrackingStatus');

$(function () {
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        showExport: true,
        exportTypes: ['excel'],
        exportOptions: {
            ignoreColumn: [0],
            fileName: 'file.xls'
        },
        formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
        columns: [
            { field: 'Action', title: 'Action', halign: 'center', width: '150px', align: 'left', formatter: operateFormatter({ Edit: true, Delete: true })},
            { field: 'SN', title: 'Serial Number', width: '200px', halign: 'center', align: 'right', formatter: dateFormatter, sortable: true },
            { field: 'Model', title: 'Model', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, },
            { field: 'From ', title: 'Origin', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, },
            { field: 'TO', title: 'Destination', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, },
            { field: 'ATD', title: 'Act. Depature', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, },
            { field: 'ATA', title: 'Act. Arrival', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, }
        ]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/DeliveryTrackingStatus/IndexPageLogImport',
        urlPaging: '/DeliveryTrackingStatus/GetLogImportPrimeProduct',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
});

function operateFormatter(options) {
    var btn = [];

    btn.push('<div class="btn-group">');
    //if (options.Add == true)
    //    btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>')
    if (options.Edit == true)
        btn.push('<button type="button" class="btn btn-info edit" title="Edit"><i class="fa fa-edit"></i></button>');
    if (options.Delete == true)
        btn.push('<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    //if (options.Info == true)
    //    btn.push('<button type="button" class="btn btn-info info" title="Info"><i class="fa fa-info-circle"></i></button>')

    btn.push('</div>');

    return btn.join('');
}

operateFormatter.DEFAULTS = {
    Add: false,
    Edit: false,
    Delete: false,
    Info: false,
    View: false,
    History: false
}

window.operateEvents = {
    'click .edit': function (e, value, row, index) {
        //alert('You click edit icon, row: ' + JSON.stringify(row.CommodityNo) + ' row.CommodityNo:' + row.CommodityNo);
        //console.log(value, row, index);
        $(".editRecord").attr('href', '/master/CommodityManagementEdit/' + row.CommodityNo).trigger('click');
    },
    'click .remove': function (e, value, row, index) {
        //$(".editRecord").attr('href', '/master/CommodityManagementDelete/' + row.CommodityNo).trigger('click');
        //console.log(value, row, index);
        swal({
            title: "Are you sure want to delete this data?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#F56954",
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            closeOnConfirm: false,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                sweetAlert.close();
                return deleteThis(row.CommodityNo);
            }
        });
    }
};

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'Master/CommodityManagementDeleteById',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: { CommodityNo: id },
        dataType: "json",
        success: function (d) {
            if (d.Msg != undefined) {
                sAlert('Success', d.Msg, 'success');
            }

            $("[name=refresh]").trigger('click');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });

};



$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
        $('#myModalContent').load(this.href, function () {
            $('#myModalPlace').modal({
                keyboard: true
            }, 'show');
            bindForm(this);
        });
        return false;
    });
});

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $('#progress').show();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {

                if (result.Status == 0) {
                    if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
                    $('#myModalPlace').modal('hide');
                    $('#progress').hide();
                    $("[name=refresh]").trigger('click');
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