var $table = $('#tableSubcon');

$(function () {
   var  columns= [{
        field: 'Id',
        title: 'Action',
        width: '180px',
        align: 'center',
        formatter: operateFormatter({ Edit: true, Delete: true}),
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
        field: 'Value',
        title: 'Value',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'CreatedBy',
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
    }
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
        urlSearch: '/emcs/SubconPage',
        urlPaging: '/emcs/SubconPageXt',
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
    //if (options.Preview === true)
    //    btn.push('<button type="button" class="btn btn-default info" title="Info"><i class="fa fa-eye"></i></button>');
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
        $(".editRecord").attr('href', '/EMCS/SubconEdit/' + row.Id).trigger('click');
    },
    'click .preview': function (e, value, row, index) {
        $(".previewImages").attr('href', '/EMCS/PreviewImage/' + row.Id).trigger('click');
    },
    'click .upload': function (e, value, row, index) {
        $(".uploadRecord").attr('href', '/EMCS/BannerUpload/' + row.Id).trigger('click');
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
        url: myApp.root + 'EMCS/SubconDelete',
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
$('#Add').on('click', function () {
    var Id = $('#Id').val();
    if (Id == undefined || Id == null || Id == "") {
        Id = 0;
    }
    var subconmodel = {};
    subconmodel.Id = Id;
    subconmodel.Name = $('#Name').val();
    $.ajax({
        url: '/EMCS/SubconCreate',
        data: {
            subconmodel
        },
        success: function (data) {
            swal.fire({
                type: 'success',
                title: 'Sucess',
                text: 'Data Saved SuccessFully'
            }).then((result) => {
                location.href = "/emcs/Subcon"
            })

        }
    })

})
$('#Update').on('click', function () {
    var Id = $('#Id').val();
    if (Id == undefined || Id == null || Id == "") {
        Id = 0;
    }
    var subconmodel = {};
    subconmodel.Id = Id;
    subconmodel.Name = $('#Name').val();
    $.ajax({
        url: '/EMCS/SubconEdit',
        type: 'POST',
        data: {
            subconmodel
        },
        success: function (data) {
            swal.fire({
                type: 'success',
                title: 'Sucess',
                text: 'Data Updated SuccessFully'
            }).then((result) => {
                location.href = "/emcs/Subcon"
            })

        }
    })

})
