var $table = $('#tableProblemCategory');

$(function () {

    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'ID',
            title: 'Action',
            width: '180px',
            align: 'center',
            formatter: operateFormatter({ Edit: true, Delete: true, Preview: true }),
            events: operateEvents
        },
        {
            field: 'Category',
            title: 'Category',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Case',
            title: 'Case',
            halign: 'center',
            align: 'left',
            sortable: true
        }]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/emcs/ProblemCategoryPage',
        urlPaging: '/emcs/ProblemCategoryPageXt',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"));
    // ReSharper disable once StatementTermination

});


function operateFormatter(options) {
    var btn = [];
    btn.push('<div class="btn-group">');
    if (options.Add === true)
        btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>');
    if (options.Edit === true)
        btn.push('<button type="button" class="btn btn-info edit" title="Edit"><i class="fa fa-edit"></i></button>');
    //if (options.Preview === true)
    //    btn.push('<button type="button" class="btn btn-default info" title="Info"><i class="fa fa-eye"></i></button>');
    if (options.Upload === true)
        btn.push('<button type="button" class="btn btn-primary upload" title="Upload"><i class="fa fa-upload"></i></button>');
    if (options.Delete === true)
        btn.push('<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
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
};

window.operateEvents = {
    'click .edit': function (e, value, row) {
        $(".editRecord").attr('href', '/EMCS/ProblemCategoryEdit/' + row.Id).trigger('click');
    },
    'click .remove': function (e, value, row) {
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
            return false;
        });
    }
};

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'EMCS/ProblemCategoryDelete',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: { ID: id },
        dataType: "json",
        success: function (d) {
            if (d.Msg != undefined) {
                sAlert('Success', d.Msg, 'success');
            }

            $("[name=refresh]").trigger('click');
        },
        error: function (jqXhr) {
            sAlert('Error', jqXhr.status + " " + jqXhr.statusText, "error");
        }
    });

};

$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function () {
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
