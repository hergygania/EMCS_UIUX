var $table = $('#table-mapping');
var AtdVisible = AtdVisible ?? true;
var canEdit = canEdit ?? false;
var canDelete = canDelete ?? false;
//var btnEditParameter = `<a class="btn btn-light btn-xs editParameter-`+data+`" href="#" title="Edit"><i class="fa fa-edit" title="Edit"></i></a>`;
//var btnDeleteParameter = `<a class="btn btn-light btn-xs deleteParameter-`+data+`" href="#" title="Delete"><i class="fa fa-trash" title="Delete"></i></a>`;
//var ActionItem = [btnEditParameter, btnDeleteParameter].join('&nbsp;');
var checkItem = `<input type='checkbox'`;
var urlCreate = 'MasterMappingUserCreate';
var urlEdit = 'MasterMappingUserEdit';
var urlDelete = 'MasterMappingUserDeleteById?id=';
var methodPost = 'POST';

$(document).ready(function () {
    $('.select2').select2();
    InitSelect2Branch();
    InitSelect2User();
    initTableMappingUser();
})

function initModalCreateMappingUser() {
    $("#modalMappingUser").modal('show');
    $("#titleMappingUser").text('Form Create Mapping User');
    $('#id').val('');
    $('#select2User').val('').trigger('change');
    $('#select2Branch').val('').trigger('change');
    $('#npwp').val('');

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

function initTableMappingUser() {
    $table.bootstrapTable({
        ajax: "getDataMappingUser",
        search: true,
        pagination: true,
        serverSort: false,
        sidePagination: "client",
        sortable: true,
        sortStable: true,
        pageSize: '5',
        pageList: [5, 10, 15, 20, 50],
        columns: [
            {
                title: 'No',
                align: 'center',
                halign: 'center',
                class: 'text-nowrap',
                formatter: runningFormatterNoPaging
            },
            {
                title: 'User ID',
                field: 'UserID',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450'
            },
            {
                title: 'FullName',
                field: 'FullName',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450'
            },
            {
                title: 'Nama Cabang',
                field: 'NamaCabang',
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
                title: 'Delete',
                field: 'ID',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: function (data, row, index) {
                    return `<button class="btn btn-danger btn-xs" onClick="deleteMappingUser(${data})" data-id='${data}' type="button" title="Delete"><i class="fa fa-trash" title="Delete"></i></button>`;

                }
            },
        ]
    });
}


function getDataMappingUser(params) {
    var url = 'GetDataMappingUser'
    $.get(url + "?" + $.param(params.data)).then(function (res) {
        params.success(res.result)
    })
}


function initModalEditMappingUser(id) {
    $("#modalMappingUser").modal('show');
    $("#titleMappingUser").text('Form Edit Mapping User');
    if (id != null || id != '') {
        var ids = parseInt(id);
        $.getJSON(`/Post/GetDataMappingUserById?id=${ids}`).then(function (res) {
            $('#id').val(res.result.ID);
            $('#select2User').val(res.result.UserID).trigger('change');
            $('#select2Branch').val(res.result.NamaCabang).trigger('change');
            $('#npwp').val(res.result.NPWP);
        })
    }
}

function sendItem(url, method, data) {
    var btn = $("#submit");
    $.ajax({
        cache: false,
        async: false,
        url: url,
        method: method,
        data: data,
        beforeSend: function () {
            var loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i> Processing...';
            if (btn.html() !== loadingText) {
                btn.data('original-text', btn.html());
                btn.html(loadingText);
            }
        },
        success: function (res) {
            if (res.status == 'SUCCESS') {
                swalSuccess('Data saved');
                $("#modalMappingUser").modal('hide');
                $table.bootstrapTable('refresh')
            } else {
                swalError(res.result);
            }
        },
        error: function (err) {
            swalError("Mapping User failed save");
        },
        complete: function () {
            btn.html(btn.data('original-text'));
        },
    })
}

function deleteMappingUser(id) {
    var obj = {}
    obj.Id = parseInt(id);
    swalConfirmDelete(obj, "want to delete it?")
}

function swalSuccess(message) {
    swal({
        title: "Success",
        text: message,
        type: "success",
        showCancelButton: false,
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
        showCancelButton: false,
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
        showCancelButton: false,
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

function InitSelect2User() {
    $('#select2User').select2({
        placeholder: "User",
        id: "id",
        minimumInputLength: 3,
        dropdownParent: $("#modalMappingUser"),
        ajax: {
            url: '/Post/GetSelectUser',
            data: function (params) {
                var query = {
                    search: params.term,
                    type: 'public'
                }
                return query;
            },
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                var newData = $.map(data.result, function (obj) {
                    return obj;
                });
                return {
                    results: newData
                };
            },
            cache: true
        }
    }).on("select2:select", function (obj) {
        var data = $("#select2User").select2("data");
        $('#fullName').val(data[0].text);
    });
}

function InitSelect2Branch() {
    $('#select2Branch').select2({
        placeholder: "Cabang",
        id: "id",
        minimumInputLength: 3,
        dropdownParent: $("#modalMappingUser"),
        ajax: {
            url: '/Post/GetSelectCabang',
            data: function (params) {
                var query = {
                    search: params.term,
                    type: 'public'
                }
                return query;
            },
            dataType: 'json',
            allowClear: true,
            delay: 250,
            processResults: function (data) {
                var newData = $.map(data.result, function (obj) {
                    return obj;
                });
                return {
                    results: newData
                };
            },
            cache: true
        }
    }).on("select2:select", function (e) {
        var data = e.params.data;
        $('#npwp').val(data.extra);
    });
}