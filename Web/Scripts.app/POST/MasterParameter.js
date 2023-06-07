var $table = $('#table-parameter');
var AtdVisible = AtdVisible ?? true;
var canEdit = canEdit ?? false;
var canDelete = canDelete ?? false;
var checkItem = `<input type='checkbox'`;
var urlCreate = 'MasterParameterCreate';
var urlEdit = 'MasterParameterEdit';
var urlDelete = 'MasterParameterDeleteById?id=';
var methodPost = 'POST';

$(document).ready(function () {
    $table.bootstrapTable({
        url: 'GetDataParameter',
        pagination: true,
        serverSort: false,
        search: true,
        pageSize: '5',
        toolbar: ".toolbar",
        responseHandler: function (data) {
            return data.result;
        },
        columns: [
            {
                title: 'No',
                align: 'center',
                halign: 'center',
                class: 'text-nowrap',
                formatter: runningFormatterNoPaging
            },
            {
                title: 'Action',
                field: 'Id',
                class: 'text-nowrap',
                halign: 'center',
                align: 'center',
                width: '110',
                formatter: function (data, row, index) {
                    return `<button class="btn btn-light btn-xs" onClick ="initModalEditParameter(${data})" title="Edit" type="button"><i class="fa fa-edit" title="Edit"></i></button>&nbsp;
                    <button class="btn btn-light btn-xs" onClick="deleteParameter(${data})" data-id='${data}' type="button" title="Delete"><i class="fa fa-trash" title="Delete"></i></button>`;

                }
            },
            {
                title: 'Group',
                field: 'Group',
                class: 'text-nowrap',
                halign: 'center',
                align: 'left',
                width: '450'

            },
            {
                title: 'Name',
                field: 'Name',
                halign: 'center',
                align: 'left',
                class: 'text-nowrap',
                halign: 'center',
                width: '450'

            },
            {
                title: 'Value',
                field: 'Group',
                class: 'text-nowrap',
                halign: 'center',
                align: 'left',
                width: '450'

            },
            {
                title: 'Sort',
                field: 'Sort',
                class: 'text-nowrap',
                halign: 'center',
                align: 'center',
                width: '100'

            },
            {
                title: 'Description',
                field: 'Description',
                halign: 'center',
                align: 'left',
                width: '450',
                formatter: function (data, row, index) {
                    if (data) {
                        return data;
                    } else {
                        return "-";
                    }
                }
            }]
    })
})

function initModalCreateParameter() {
    $("#modalParameter").modal('show');
    $("#titleParameter").text('Form Create Master Parameter');
    $(`#id`).val('');
    $(`#name`).val('');
    $(`#value`).val('');
    $(`#group`).val('');
    $(`#description`).val('');
    $(`#public`).val(true);

}

function submitForm() {
    var form = $('form').serialize();
    var id = $('#id').val();
    if (id == null || id == "") {
        sendItem(urlCreate, methodPost, form);
    } else {
        sendItem(urlEdit, methodPost, form);
    }
}

function getDataParameter(params) {
    var url = 'GetDataParameter'
    $.get(url).then(function (res) {
        params.success(res.result)
    })
}


function initModalEditParameter(id) {
    $("#modalParameter").modal('show');
    $("#titleParameter").text('Form Edit Master Parameter');
    if (id != null || id != '') {
        var ids = parseInt(id);
        $.getJSON(`/Post/GetParameterById?id=${ids}`).then(function (res) {
            $(`#id`).val(res.result.Id);
            $(`#name`).val(res.result.Name);
            $(`#value`).val(res.result.Value);
            $(`#group`).val(res.result.Group);
            $(`#Sort`).val(res.result.Sort);
            $(`#description`).val(res.result.Description);
            $(`#public`).val(true);

        })
    }
}

function sendItem(url, method, data) {
    $.ajax({
        cache: false,
        async: false,
        url: url,
        method: method,
        data: data,
        success: function (res) {
            $("#modalParameter").modal('hide');
            $table.bootstrapTable('refresh');
            if (res.status) {
                swalSuccess("Update data berhasil");
            } else {
                swalSuccess("Update data gagal");
            }
        },
        error: function (err) {
            swalError("Master Parameter failed save");
        }
    })
}

function deleteParameter(id) {
    var obj = {}
    obj.Id = parseInt(id);
    swalConfirmDelete(obj, "want to delete it?")
}

function swalSuccess(message) {
    swal({
        title: "Success",
        text: message,
        type: "success",
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonColor: "#00c0ef",
        confirmButtonText: "OK",
        //cancelButtonText: "No",
        showCloseButton: false
    }, function (isConfirm) {
        if (isConfirm) {
            return;
        }
    })
}

function swalConfirmDelete(data, message) {
    swal({
        title: "Are you sure?",
        text: message,
        type: "info",
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonColor: "#00c0ef",
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        showCloseButton: true

    }, function (isConfirm) {
        if (isConfirm) {
            var url = urlDelete + data.Id
            sendItem(url, methodPost, data)
        }

    })

}

function swalWarning(message) {
    swal({
        title: "Warning",
        text: message,
        type: "warning",
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonColor: "#00c0ef",
        confirmButtonText: "OK",
        //cancelButtonText: "No",
        showCloseButton: false,

    }, function (isConfirm) {
        if (isConfirm) {
            return;
        }
    })
}

function swalError(message) {
    swal({
        title: "Failed",
        text: message,
        type: "error",
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonColor: "#00c0ef",
        confirmButtonText: "OK",
        //cancelButtonText: "No",
        showCloseButton: false

    }, function (isConfirm) {
        if (isConfirm) {
            return;
        }
    })
}