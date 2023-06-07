var $table = $('#tableCategoryCode');
var $AllowDelete = $('#AllowDelete').val();
var $AllowUpdate = $('#AllowUpdate').val();

$(function () {
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
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'action',
            title: 'Action',
            width: '180px',
            align: 'center',
            formatter: operateFormatter({ Edit: Boolean($AllowUpdate), Delete: Boolean($AllowDelete) }),
            events: operateEvents,
            switchable: false
        },
        {
            field: 'Category',
            title: 'Category',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Code',
            title: 'Code',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Description1',
            title: 'Description 1',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Description2',
            title: 'Description 2',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Ordering',
            title: 'Ordering',
            halign: 'center',
            align: 'left',
            sortable: true
        }]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/DTS/CategoryCodePage',
        urlPaging: '/DTS/CategoryCodePageXt',
        autoLoad: true
    });

    $("#mySearch").insertBefore($("[name=refresh]"));

});

function operateFormatter(options) {
    var btn = [];

    btn.push('<div class="btn-group">');
    if (options.Add === true)
        btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>')
    if (options.Edit === true)
        btn.push('<button type="button" class="btn btn-info edit" title="Edit"><i class="fa fa-edit"></i></button>');
    if (options.Delete === true)
        btn.push('<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    if (options.Info === true)
        btn.push('<button type="button" class="btn btn-info info" title="Info"><i class="fa fa-info-circle"></i></button>')

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
    'click .edit': function (e, value, row, index) {
        $(".editRecord").attr('href', '/DTS/CategoryCodeEdit/' + row.Category + '/' + row.Code).trigger('click');
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
                return deleteThis(row.Category, row.Code);
            }
        });
    }
};

function deleteThis(cat, code) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/CategoryCodeDelete',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: { cat: cat, code: code },
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