var $table = $('#tableIncoterms');
var $searchInput = $("#txtSearchData").val();
var $AllowDelete = $('#AllowDelete').val();
var $AllowUpdate = $('#AllowUpdate').val();

$(".rssItem").on("change", function () {
    var val = $(this).val();
    var checked = $("#source_" + val).prop("checked");
    $.ajax({
        url: myApp.fullPath + "/EMCS/SetRunningTextSource",
        type: "POST",
        data: {
            Id: val,
            status: (checked ? 0 : 1)
        },
        success: function (resp) {
            if (resp.status === 1) {
                swal("Success", "Success to update status", "success");
            } else {
                swal("Failed!", "Failed to update status!", "error");
            }
        },
        error: function () {
            swal("Failed!", "Failed to update status!", "error");
        }
    });
});

function operateFormatter(options) {
    var btn = [];

    btn.push('<div class="btn-group">');
    if (options.Edit === true)
        btn.push('<button type="button" class="btn btn-sm btn-link btn-info edit" title="Edit"><i class="tim-icons icon-pencil"></i></button>');
    if (options.Delete === true)
        btn.push('<button type="button" class="btn btn-sm btn-link btn-danger remove" title="Delete"><i class="tim-icons icon-simple-remove"></i></button>');
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

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'EMCS/RunningTextDeleteById',
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
        error: function (jqXhr) {
            sAlert('Error', jqXhr.status + " " + jqXhr.statusText, "error");
        }
    });

}

window.operateEvents = {
    'click .edit': function (e, value, row) {
        $(".editRecord").attr('href', '/EMCS/RunningTextEdit/' + row.Id).trigger('click');
        //$(".editRecord").attr('href', '/master/MenuEdit/' + row.ID).trigger('click');
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
        field: 'Content',
        title: 'Content',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data) {
            if (data.length > 80) {
                return data.substring(1, 80) + "...";
            } else {
                return data;
            }
        }
    }, {
        field: 'StartDate',
        title: 'Start Date',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data) {
            return moment(data).format("DD MMM YYYY");
        }
    }, {
        field: 'EndDate',
        title: 'End Date',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data) {
            return moment(data).format("DD MMM YYYY");
        }
    }, {
        field: 'IsDelete',
        title: 'Status',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data) {
            if (data === true) {
                return "<span class='label label-danger'>No</span>";
            } else {
                return "<span class='label label-success'>Yes</span>";
            }
        }
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
        formatter: function (data) {
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
            params.SearchName = $("#mySearch input[name=searchText]").val();
            return params;
        },
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: columnList
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/EMCS/RunningTextPage',
        urlPaging: '/EMCS/RunningTextPageXt',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"));
});