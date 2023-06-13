var $table = $('#tableKppbc');
var $searchInput = $("#txtSearchData").val();
var $AllowDelete = $('#AllowDelete').val();
var $AllowUpdate = $('#AllowUpdate').val();

function operateFormatter(options) {
    var btn = [];

    btn.push('<div class="btn-group">');
    if (options.Edit === true)
        btn.push('<button type="button" class="btn btn-sm btn-info edit" title="Edit"><i class="fa fa-edit"></i></button>');
    if (options.Delete === true)
        btn.push('<button type="button" class="btn btn-sm btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
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

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'EMCS/KppbcDelete',
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
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });

}

window.operateEvents = {
    'click .edit': function (e, value, row, index) {
        $(".editRecord").attr('href', '/emcs/KppbcEdit/' + row.Id).trigger('click');
        //$(".editRecord").attr('href', '/master/MenuEdit/' + row.ID).trigger('click');
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

var columnList = [
    {
        field: 'ID',
        title: 'Actions',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        formatter: operateFormatter({ Edit: Boolean($AllowUpdate), Delete: Boolean($AllowDelete) }),
        events: operateEvents,
        sortable: true
    }, {
        field: 'BAreaName',
        title: 'Area',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'Code',
        title: 'Code',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'Name',
        title: 'Name',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'Propinsi',
        title: 'Province',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'Kab',
        title: 'Districts',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'CreateBy',
        title: 'Created By',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'CreateDate',
        title: 'Created Date',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    }];

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        console.log($(this).serialize());
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

$(function () {
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,
        toolbar: '.toolbar',
        searchOnEnterKey: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        queryParams: function (params) {
        },
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: columnList
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/EMCS/KppbcPage',
        urlPaging: '/EMCS/KppbcPageXt',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
});