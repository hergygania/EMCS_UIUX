var $table = $('#tableVendor');

$(function () {
    
    var columns = [{
        field: 'Id',
        title: 'Action',
        width: '180px',
        align: 'center',
        formatter: function (data, row, index) {
            
            var btn = [];
            btn.push('<div class="btn-toolbar row">');
            if (row.IsManualEntry === false) {
                btn.push('<button type="button" class="btn btn-default info" title="Info"><i class="fa fa-eye"></i></button>');
            }
            else {
                btn.push('<button type="button" class="btn btn-info btn-xs edit" title="Edit"><i class="fa fa-edit"></i></button>');
                btn.push('<button type="button" class="btn btn-danger btn-xs remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
            }
            btn.push('</div>');
            return btn.join('');
        },
        events: operateEvents
    },
    {
        field: 'Name',
        title: 'Name',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'Address',
        title: 'Address',
        halign: 'center',
        align: 'left',
        sortable: true
        },
        {
            field: 'Telephone',
            title: 'Telephone',
            halign: 'center',
            align: 'left',
            sortable: true
        },
    {
        field: 'CreateBy',
        title: 'CreateBy',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'CreateDate',
        title: 'CreateDate',
        halign: 'center',
        align: 'center',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
        },
        {
            field: 'IsManualEntry',
            title: 'IsManualEntry',
            halign: 'center',
            align: 'left',
            formatter: function (data, row, index) {
                if(row.IsManualEntry === true)
                return "Manual Entry";
                else
                return "SAP Entry";
            },
            sortable: true
        },
    ]


    $table.bootstrapTable({
        columns: columns,
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: false,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        sidePagination: 'server',
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        //fixedColumns: true,
        //fixedNumber: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">No Data Found</span>';
        },

    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/emcs/VendorPage',
        urlPaging: '/emcs/VendorPageXt',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))

});


function operateFormatter(options) {
    
    var btn = [];
    btn.push('<div class="btn-toolbar row">');
    if (options.Add === true)
        btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>')
    if (options.Edit === true)
        btn.push('<button type="button" class="btn btn-info btn-xs edit" title="Edit"><i class="fa fa-edit"></i></button>');
    if (options.Preview === true)
        btn.push('<button type="button" class="btn btn-default info" title="Info"><i class="fa fa-eye"></i></button>');
    //if (options.Upload === true)
    //    btn.push('<button type="button" class="btn btn-primary upload" title="Upload"><i class="fa fa-upload"></i></button>');
    if (options.Delete === true)
        btn.push('<button type="button" class="btn btn-danger btn-xs remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    btn.push('</div>');
    return btn.join('');
}

operateFormatter.DEFAULTS = {
    Add: false,
    Edit: false,
    Delete: false,
    Info: false,
    View: false,
    History: false,
    Preview: false,
    Upload: false
}

window.operateEvents = {
    'click .edit': function (e, value, row, index) {
        $(".editRecord").attr('href', '/EMCS/VendorEdit/' + row.Id).trigger('click');
    },
    'click .info': function (e, value, row, index) {
        $(".preview").attr('href', '/EMCS/VendorView/' + row.Id).trigger('click');
    },
    'click .remove': function (e, value, row, index) {
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
                return deleteThis(row.Id);
            }
        });
    }
};

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'EMCS/VendorDelete',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: { ID: id },
        dataType: "json",
        success: function (d) {
            if (d.Msg !== undefined) {
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
                if (result.Status === 0) {
                    if (result.Msg !== undefined) sAlert('Success', result.Msg, 'success');
                    $('#myModalPlace').modal('hide');
                    $('#progress').hide();
                    $("[name=refresh]").trigger('click');
                }
                else {
                    if (result.Msg !== undefined) sAlert('Failed', result.Msg, 'error');
                    $('#progress').hide();
                }
            }
        });
        return false;
    });
};

