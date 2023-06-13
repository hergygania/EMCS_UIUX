var $tableSIB = $("#tableSIB");
var visibleTools = ((AllowUpdate === "True" || AllowDelete === "True") ? false : true);

function editRow(target) {
    $tableSIB.datagrid("beginEdit", target, 'abc');
}

$(".sidebar-toggle").on("click", function () {
    setTimeout(function () {
        $(".easyui-tabs").tabs('resize');
        $tableSIB.datagrid('resize');
    }, 400);
});

function cancelrow(target) {
    $tableSIB.datagrid("cancelEdit", target);
    $tableSIB.datagrid("reload", target);
}

function getDataByIndex(index) {
    var data = $tableSIB.datagrid("getRows")[index];
    return data;
}

function doDelete(index) {
    var dataRow = getDataByIndex(index);
    $.ajax({
        url: "/Master/DeleteSIB",
        type: "POST",
        data: { ID: dataRow.ID },
        success: function (resp) {
            var type = resp.status ? "success" : "error";
            swal("Action Status", resp.msg, type);
            $tableSIB.edatagrid("reload");
        },
        error: function (err) {
            swal("Action Status", err.msg, "error");
        }
    });
}

function deleteRow(target) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this data!",
        icon: "/Media/Img/question.png",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            doDelete(target);
        }
    });
}

function ActionFormatter(data, row, index) {
    var htm = "<div style='width:100%;' class='text-center'>";
    if (AllowUpdate === "True") {
        htm += "<button class='btn btn-danger btn-xs delete' onClick='deleteRow(" + index + ");'><i class='fa fa-times'></i></button>&nbsp;";
    }

    if (AllowDelete === "True") {
        htm += "<button class='btn btn-primary btn-xs edit-item' onClick='editRow(" + index + ");'><i class='fa fa-edit'></i></button>";
    }

    htm += "</div>";
    return htm;
}

function searchTable() {
    var search = $("#search_field_material").val();
    $tableSIB.datagrid("load", {
        search: $.trim(search)
    });
    $tableSIB.datagrid("resize");
}

$.extend($.fn.datagrid.defaults.editors, {
    tools: {
        init: function (container, options) {
            var htm = "<div style='width:100%;' class='text-center'>";
            htm += "<button class='btn btn-default btn-xs remove btn-remove-item'><i class='fa fa-undo-alt'></i></button>&nbsp;";
            htm += "<button class='btn btn-success btn-xs save btn-save-item'><i class='fa fa-check-circle'></i></button>";
            htm += "</div>";
            var input = $(htm).appendTo(container);
            return input;
        },
        destroy: function (target) {
            $(target).remove();
        },
        getValue: function (target) {
            return $(target).val();
        },
        setValue: function (target, value) {
            $(target).val(value);
        },
        resize: function (target, width) {
            $(target)._outerWidth(width);
        }
    }
});

var $table = $('#tableSIB');
var $searchInput = $("#txtSearchData").val();
var $AllowDelete = $('#AllowDelete').val();
var $AllowUpdate = $('#AllowUpdate').val();

Dropzone.autoDiscover = false;

var myDropzone = new Dropzone("#FormUploadSIB", { // Make the bodyFormUpload a dropzone
    url: "/EMCS/UploadSIB", // Set the url
    thumbnailHeight: 100,
    thumbnailWeight: 100,
    timeout: "80000",
    method: 'POST',
    dictDefaultMessage: "<h4>Drop the Import File Here or Click this Section for Browse the Import File.</h4>",
    acceptedFiles: '.xlsx',
    filesizeBase: 1024,
    autoProcessQueue: true,
    maxFiles: 1,
    maxFilesize: 100, // MB
    parallelUploads: 1,
    previewTemplate: $("#template-dropzone").html(),
    uploadMultiple: false
    //previewsContainer: "#FormUploadMaterial", // Define the container to display the previews
    //clickable: ".fileinput-button" // Define the element that should be used as click trigger to select files.
});

myDropzone.on("addedfile", function (file) {
    // Hookup the start button
    $("#actions .start").on("click", function () {
        myDropzone.enqueueFile(file);
    });
    $("#placeholderUpload").hide();
});

myDropzone.on("totaluploadprogress", function (progress) {
    $("#total-progress .progress-bar").css("width", progress + "%");
    $("#progressPercent").html(progress + "%");
});

myDropzone.on("sending", function (file, xhr, formData) {
    // Show the total progress bar when upload starts
    $("#total-progress").css("opacity", 1);
    // And disable the start button
    $("#actions .delete").attr("disabled", "disabled");
    $(".start").attr("disabled", "disabled");
});

myDropzone.on("complete", function (resp) {
    if (resp.status === "success") {
        var respText = resp.xhr.response;
        var respData = JSON.parse(respText);
        console.log(respData);

        $("#actions .delete").prop("disabled", false);
        var type = respData.status ? "success" : "error";
        swal("Upload Status", respData.msg, type);
        location.reload();
    }
    
});

myDropzone.on("queuecomplete", function (progress) {
    $("#total-progress").css("opacity", "0");
    setTimeout(function () {
        myDropzone.removeAllFiles(true);
    }, 400);
});

$("#actions .start").on("click", function () {
    myDropzone.enqueueFiles(myDropzone.getFilesWithStatus(Dropzone.QUEUED));
});

$("#actions .cancel").on("click", function () {
    myDropzone.removeAllFiles(true);
    $("#placeholderUpload").hide();
});

//--------------------------//

function operateFormatter(options) {
    var btn = [];

    btn.push('<div class="btn-toolbar row">');
    if (options.Edit === true)
        btn.push('<button type="button" class="btn btn-sm btn-info btn-xs edit" title="Edit"><i class="fa fa-edit"></i></button>');
    if (options.Delete === true)
        btn.push('<button type="button" class="btn btn-sm btn-danger btn-xs remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
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
        url: myApp.root + 'EMCS/MenuDeleteById',
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
        $(".editRecord").attr('href', '/emcs/IncotermsEdit/' + row.ID).trigger('click');
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
                return deleteThis(row.ID);
            }
        });
    }
};

var columnList = [
    //{
    //    field: 'ID',
    //    title: 'Actions',
    //    halign: 'center',
    //    align: 'center',
    //    class: 'text-nowrap',
    //    formatter: operateFormatter({ Edit: Boolean($AllowUpdate), Delete: Boolean($AllowDelete) }),
    //    events: operateEvents,
    //    sortable: true
    //},
    {
        field: 'ReqNumber',
        title: 'Req Number',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },{
        field: 'DlrWO',
        title: 'DlrWO',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },{
        field: 'DlrClm',
        title: 'DlrClm',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },{
        field: 'SvcClm',
        title: 'SvcClm',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'PartNo',
        title: 'Part No',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'SerialNumber',
        title: 'Serial Number',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'Description',
        title: 'Description',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'DlrCode',
        title: 'DlrCode',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'UnitPrice',
        title: 'Unit Price',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'Currency',
        title: 'Currency',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'Qty',
        title: 'Quantity',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'CreateBy',
        title: 'Create By',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'CreateDate',
        title: 'Create Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            if (data) {
                if (data) {
                    return moment(data).format("DD MMM YYYY");
                } else {
                    return "-";
                }
            } else {
                return "-";
            }
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
        search: true,
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
        urlSearch: '/EMCS/MasterSIBPage',
        urlPaging: '/EMCS/MasterSIBPageXt',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
});