var $table = $('#tableShippingInstruction');
var $AllowDelete = $('#AllowDelete').val();
var $AllowUpdate = $('#AllowUpdate').val();

$(function () {
    var columns = [
        {
            field: "",
            title: "Action",
            align: "center",
            sortable: true,
            width: "125",
            formatter: function (data, row, index) {
                return operateFormatter({ Edit: Boolean($AllowUpdate), Delete: Boolean($AllowDelete), Data: row});
            },
            events: operateEvents
        },
        {
            field: "cipl",
            title: "CIPL No.",
            sortable: true
        }, {
            field: "cipl",
            title: "Category",
            sortable: true
        }, {
            field: "cipl",
            title: "Gross Weight",
            sortable: true
        }, {
            field: "consignee",
            title: "Consignee",
            sortable: true,
            class: "text-nowrap"
        }, {
            field: "consignee",
            title: "DA No.",
            sortable: true,
            class: "text-nowrap"
        }, {
            field: "branch",
            title: "ETD",
            sortable: true,
            class: "text-nowrap"
        }, {
            field: "status",
            title: "Status",
            sortable: true,
            filterControl: true,
            class: "text-nowrap"
        }
    ]

    $table.bootstrapTable({
        url: "/Json/Transaction/CiplData.json",
        columns: columns,
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
        
    });

    window.pis.table({
        objTable: $table,
        //urlSearch: '/Json/Transaction/Table.json',
        //urlPaging: '/Json/Transaction/Table.json',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"));

});


function operateFormatter(options) {
    var btn = [];
    console.log(options);
    btn.push('<div class="btn-group">');
    if (options.Add == true)
        btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>')
    if (options.Edit == true && options.Data.status === 'Draft')
        btn.push('<button type="button" class="btn btn-primary edit" title="Edit"><i class="fa fa-edit"></i></button>');
    if (options.Delete == true && options.Data.status === 'Draft')
        btn.push('<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    //if (options.Info == true)
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
}

window.operateEvents = {
    'click .edit': function (e, value, row, index) {
        //alert('You click edit icon, row: ' + JSON.stringify(row.EmailRecipientID) + ' row.EmailRecipientID:' + row.EmailRecipientID);
        //console.log(value, row, index);
        $(".editRecord").attr('href', '/master/EmailRecipientManagementEdit/' + row.EmailRecipientID).trigger('click');
    },
    'click .remove': function (e, value, row, index) {
        //$(".editRecord").attr('href', '/master/EmailRecipientManagementDelete/' + row.EmailRecipientID).trigger('click');
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
                return deleteThis(row.EmailRecipientID);
            }
        });
    }
};

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'Master/EmailRecipientManagementDeleteById',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: { EmailRecipientId: id },
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


function addEmail(tableId) {
    console.log(tableId);
    var i = parseInt($('#' + tableId + ' tr:last').data('row')) + 1;
    console.log(i);

    var strTr = "<tr data-row=" + i + ">";
    strTr += "<td><input type=\"email\" name=\"email\" id=\"email_" + i + "\" " +
        "required=\"required\" class=\"form-control required\" " +
        "style=\"width: 250px;\"></td>";
    strTr += "<td><button type=\"button\" value=\"Delete\" class=\"btn-danger btn-md\" onClick='removeTr(this)' style=\"margin-left: 10px;\">Delete</button></td>";
    strTr += "</tr>";
    $('#' + tableId + ' tr:last').after(strTr);
}


function removeTr(obj) {
    event.preventDefault();
    $(obj).parent().parent().remove();

}
