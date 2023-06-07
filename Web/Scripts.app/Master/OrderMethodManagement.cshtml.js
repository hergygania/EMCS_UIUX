var $table = $('#tableOrderMethod');
var $AllowDelete = $('#AllowDelete').val();
var $AllowUpdate = $('#AllowUpdate').val();

$(function() {

    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');

    $(".downloadExcel").click(function () {

        enableLink(false);
        $.ajax({
            url: "DownloadOrderMethodToExcel",
            type: 'GET',
            success: function (guid) {
                enableLink(true);
                window.open('DownloadToExcel?guid=' + guid, '_blank');
            },
            cache: false,
            contentType: false,
            processData: false
        });
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
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        //fixedColumns: true,
        //fixedNumber: '5',
        formatNoMatches: function() {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
                field: 'action',
                title: 'Action',
                width: '180px',
                align: 'center',
                formatter: operateFormatter({ Edit: Boolean($AllowUpdate), Delete: Boolean($AllowDelete) }),
                events: operateEvents
            }, {
                field: 'OMCode',
                title: 'Order Method Code',
                halign: 'center',
                align: 'left',
                sortable: true
            },
            {
                field: 'Description',
                title: 'Description',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'OMRank',
                title: 'Rank',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'VettingRoute',
                title: 'Vetting Route',
                halign: 'center',
                align: 'left',
                formatter: vettingRouteFormatter,
                sortable: true
            },
            {
                field: 'Status',
                title: 'Status',
                halign: 'center',
                align: 'center',
                sortable: false,
                formatter: statusFormatter
            }
        ]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/master/OrderMethodManagementPage',
        urlPaging: '/master/OrderMethodManagementPageXt',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"));
});

function statusFormatter(value, row, index) {
    if (value == 1) {
        return "<span class='label label-success'>ACTIVE</span>";
    } else {
        return "<span class='label label-danger'>INACTIVE</span>";
    }
}

function vettingRouteFormatter(value, row, index) {
    switch (value) {
    case 1:
        return "Normal";
    case 2:
        return "Survey";
    default:
        return "Mix";
    }

}

function operateFormatter(options) {
    var btn = [];

    btn.push('<div class="btn-group">');
    if (options.Add == true)
        btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>');
    if (options.Edit == true)
        btn.push('<button type="button" class="btn btn-info edit" title="Edit"><i class="fa fa-edit"></i></button>');
    if (options.Delete == true)
        btn.push('<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    if (options.Info == true)
        btn.push('<button type="button" class="btn btn-info info" title="Info"><i class="fa fa-info-circle"></i></button>');
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
};
window.operateEvents = {
    'click .edit': function(e, value, row, index) {
        $(".editRecord").attr('href', '/master/OrderMethodManagementEdit/' + row.OMID).trigger('click');
    },
    'click .remove': function(e, value, row, index) {
        swal({
                title: "Are you sure want to delete this data?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#F56954",
                confirmButtonText: "Yes",
                cancelButtonText: "No",
                closeOnConfirm: false,
                closeOnCancel: true
            }, function(isConfirm) {
                if (isConfirm) {
                    sweetAlert.close();
                    return deleteThis(row.OMID);
                }
            });
    }
};

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'Master/OrderMethodManagementDeleteById',
        beforeSend: function() {
            $('.fixed-table-toolbar').hide();
            $('.fixed-table-loading').show();
        },
        complete: function() {
            $('.fixed-table-toolbar').show();
            $('.fixed-table-loading').hide();
        },
        data: { orderMethodId: id },
        dataType: "json",
        success: function(d) {
            if (d.Msg != undefined) {
                sAlert('Success', d.Msg, 'success');
            }

            $("[name=refresh]").trigger('click');
        },
        error: function(jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });

}

;

$(function() {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function(e) {
        $('#myModalContent').load(this.href, function() {
            $('#myModalPlace').modal({
                keyboard: true
            }, 'show');

            bindForm(this);
        });
        return false;
    });


});

function bindForm(dialog) {
    $('form', dialog).submit(function() {
        $('#progress').show();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function(result) {

                if (result.Status == 0) {
                    if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
                    $('#myModalPlace').modal('hide');
                    $('#progress').hide();
                    $("[name=refresh]").trigger('click');
                } else {
                    if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
                    $('#progress').hide();
                }
            }
        });
        return false;
    });
}

