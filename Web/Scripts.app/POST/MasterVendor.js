var $table = $('#table-vendor');
var AtdVisible = AtdVisible ?? true;
var canEdit = canEdit ?? false;
var canDelete = canDelete ?? false;
//var btnEditParameter = `<a class="btn btn-light btn-xs editParameter-`+data+`" href="#" title="Edit"><i class="fa fa-edit" title="Edit"></i></a>`;
//var btnDeleteParameter = `<a class="btn btn-light btn-xs deleteParameter-`+data+`" href="#" title="Delete"><i class="fa fa-trash" title="Delete"></i></a>`;
//var ActionItem = [btnEditParameter, btnDeleteParameter].join('&nbsp;');
var checkItem = `<input type='checkbox'`;
var urlCreate = 'MasterVendorCreate';
var urlEdit = 'MasterVendorEdit';
var urlDelete = 'MasterVendorDeleteById?id=';
var methodPost = 'POST';

$(document).ready(function () {
    initTableVendor();

})

function initModalCreateVendor() {
    $("#modalVendor").modal('show');
    $("#titleVendor").text('Form Create Master Vendor');
    $(`#id`).val('');
    $(`#code`).val('');
    $(`#name`).val('');
    $(`#address`).val('');
    $(`#city`).val('');
    $(`#npwp`).val('');
    $(`#telephone`).val('');

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

function initTableVendor() {
    $table.bootstrapTable({
        ajax: "getDataVendor",
        pagination: true,
        serverSort: false,
        sidePagination:"server",
        pageSize: '5',
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
                align: 'center',
                width: '110',
                formatter: function (data, row, index) {
                    return `<button class="btn btn-light btn-xs" onClick ="initModalEditVendor(${data})" title="Edit" type="button"><i class="fa fa-edit" title="Edit"></i></button>&nbsp;
                    <button class="btn btn-light btn-xs" onClick="deleteVendor(${data})" data-id='${data}' type="button" title="Delete"><i class="fa fa-trash" title="Delete"></i></button>`;

                }
            },
            {
                title: 'Code',
                field: 'Code',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450'

            },
            {
                title: 'Name',
                field: 'Name',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450'

            },
            {
                title: 'NPWP',
                field: 'NPWP',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450'

            },
            {
                title: 'City',
                field: 'City',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450'

            },
            {
                title: 'Address',
                field: 'Address',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450'

            },
            {
                title: 'Telephone',
                field: 'Telephone',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450'

            },
            ]
    });
}


function getDataVendor(params) {
    var url = 'GetDataVendor'
    $.get(url+"?"+$.param(params.data)).then(function (res) {
        params.success(res.result)
    })
}


function initModalEditVendor(id) {
    $("#modalVendor").modal('show');
    $("#titleVendor").text('Form Edit Vendor');
    if (id != null || id != '') {
        var ids = parseInt(id);
        $.getJSON(`/Post/GetDataVendorById?id=${ids}`).then(function (res) {
            $(`#id`).val(res.result.Id);
            $(`#name`).val(res.result.Name);
            $(`#code`).val(res.result.Code);
            $(`#address`).text(res.result.Address);
            $(`#city`).val(res.result.City);
            $(`#telephone`).val(res.result.Telephone);
            $(`#npwp`).val(res.result.NPWP);

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
            $("#modalVendor").modal('hide');
            $table.bootstrapTable('refresh')
        },
        error: function (err) {

            swalError("Master Vendor failed save");
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