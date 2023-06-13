var $table = $('#table-ebupot');
var checkItem = `<input type='checkbox'`;
var urlCreate = 'EbupotCreate';
var methodPost = 'POST';
var _dataSelection = [];
var btn = $("#submit");

$(document).ready(function () {
    $('.select2').select2();
    $('#cabangRow').attr("disabled", false);
    SetFormatValue($('#tarif'));
    SetFormatValue($('#bruto'));
    SetFormatValue($('#pph'));
    InitSelect2Branch();
    InitSelect2Vendor();
    InitDateRange();
    InitCheckboxEvents();
    initTableEbupot();
    //initTableEbupotDetail();
})

function initModalCreateEbupot() {
    $("#modalEbupot").modal('show');
    $("#titleEbupot").text('Upload Data E-Bupot');
    $("#select2VendorModal").val('').trigger('change');;
    $("#npwpVendorModal").val('');
    $("#select2BranchModal").val('').trigger('change');;
    $("#select2BranchModal2").val('').trigger('change');;
    $("#dateModal").val('');
    $("#masaPajakModal").val('');
    $("#invoiceNoModal").val('');
    $("#noBuktiPotongModal").val('');
    $("#kode").val('');
    $("#tarif").val('');
    $("#bruto").val('');
    $("#pph").val('');
    $("#nik").val('');
    $("#tin").val('');
    $("#pembetulan").val(0);
    $('#FileUpload').val('');
}

function initModalEbupotDetail(data) {
    $("#modalEbupotDetail").modal('show');
}

function InitDateRange() {
    var now = new Date();
    $('#dateModal').datepicker({
        format: 'dd.mm.yyyy',
        autoclose: true,
        clearBtn: true,
        orientation: "top"
    });
    $('#masaPajakModal').datepicker({
        format: 'mm.yyyy',
        autoclose: true,
        clearBtn: true,
        orientation: "top",
        viewMode: "months",
        minViewMode: "months"
    });

    $('.masaPajak').daterangepicker({
        opens: 'right',
        locale: {
            format: 'MMM.YYYY',
            viewMode: "months",
            minViewMode: "months"
        },

    }, function (start, end) {
        console.log('start', start._d);
        var start_date = moment(start._d).format("MM.YYYY");
        var end_date = moment(end._d).format("MM.YYYY");

        $('.masaPajak').val(start_date + '-' + end_date);
    });
    //$('.masaPajak').val(moment(now).format("DD.MM.YYYY") + '-' + moment(now).format("DD.MM.YYYY"));
    $('.masaPajak').val('');
}

function submitForm() {
    var isHO = $('#role').val() == 'POST';
    var branch = $("#select2BranchModal").val();
    if (!isHO) {
        branch = $("#select2BranchModal2").val()
    }
    var formData = {
        "NamaVendor": $("#select2VendorModal").val(),
        "NpwpVendor": $("#npwpVendorModal").val(),
        "Cabang": branch,
        "Date": moment($("#dateModal").datepicker("getDate")).format(),
        "MasaPajak": moment($("#masaPajakModal").datepicker("getDate")).format(),
        "InvoiceNo": $("#invoiceNoModal").val(),
        "NoBuktiPotong": $("#noBuktiPotongModal").val(),
        "Kode": $("#kode").val(),
        "Tarif": $('#tarif').autoNumeric('get'),
        "Bruto": $('#bruto').autoNumeric('get'),
        "PPH": $('#pph').autoNumeric('get'),
        "NIK": $('#nik').val(),
        "Tin": $('#tin').val(),
        "Pembetulan": $('#pembetulan').val(),
    };

    sendItem(urlCreate, methodPost, formData);
}

function initTableEbupot() {
    $table.bootstrapTable('destroy');
    $table.bootstrapTable({
        url: "GetEbupot",
        pagination: true,
        serverSort: true,
        sidePagination: "server",
        pageSize: 5,
        pageList: [5, 10, 25, 50, 100, 200],
        sortOrder: 'desc',
        responseHandler: "responseHandler",
        onLoadSuccess: function (resp) {
            UpdateDataPageNumber(resp.rows);
        },
        queryParams: function (p) {
            jQuery.ajaxSettings.traditional = true;
            var dariMasa = null;
            var keMasa = null;
            if ($('#masaPajak').val()) {
                dariMasa = moment($("#masaPajak").data("daterangepicker").startDate._d).format("YYYYMM") ?? "";
                keMasa = moment($("#masaPajak").data("daterangepicker").endDate._d).format("YYYYMM") ?? "";
            }
            return {
                Limit: p.limit,
                Offset: p.offset,
                Sort: p.sort,
                Order: p.order,
                Cabang: $("#select2Branch").val(),
                Vendor: $("#select2Vendor").val(),
                NpwpVendor: $("#npwpVendor").val(),
                NoBuktiPotong: $("#noBuktiPotong").val(),
                DariMasa: dariMasa,
                KeMasa: keMasa,
            };
        },
        columns: [
            {
                field: 'state',
                checkbox: true,
                align: 'center',
                valign: 'middle',
            },
            {
                title: 'Action',
                field: 'ID',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: function (data, row, index) {
                    if (row.Path)
                        return `<button class="btn btn-light btn-xs" onClick ="DownloadFileUpload('${row.FileNameOri}', '${row.Path}')" title="Download" type="button"><i class="fa fa-download" title="Download"></i></button>`;

                    return ''
                }
            },
            {
                title: 'Status SPT',
                field: 'status',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Nama Cabang',
                field: 'namaCabang',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'NPWP Cabang',
                field: 'npwpCabang',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Masa',
                field: 'masa',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Tahun',
                field: 'tahun',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Pembetulan',
                field: 'pembetulan',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Pesan',
                field: 'message',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'No Bupot',
                field: 'noBupot',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Rev Bupot',
                field: 'revBupot',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Status Bupot',
                field: 'statusBupot',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Cetak',
                field: 'cetak',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Pesan',
                field: 'pesanBupot',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Tin',
                field: 'tin1',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'NPWP Vendor',
                field: 'npwpVendor',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'NIK',
                field: 'nik',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Nama Vendor',
                field: 'namaVendor',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Email',
                field: 'emailVendor',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Invoice No',
                field: 'invoiceNo',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Kode',
                field: 'kode',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Bruto',
                field: 'bruto',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                formatter: function (value) {
                    if (value == null || value == 0) {
                        return "0.00";
                    }
                    return formatMoney(value, 2, '.', ',');
                },
                sortable: true
            },
            {
                title: 'PPH',
                field: 'pph',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                formatter: function (value) {
                    if (value == null || value == 0) {
                        return "0.00";
                    }
                    return formatMoney(value, 2, '.', ',');
                },
                sortable: true
            },
            {
                title: 'XML ID',
                field: 'refXmlId',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Ref Log File Id',
                field: 'refLogFileId',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Tanggal BP',
                field: 'tgl',
                formatter: function (value) {

                    if (value == null) {
                        return '-';
                    }
                    return moment(value).format('DD-MM-YYYY');
                },
            },
            {
                title: 'Pembuat',
                field: 'createdBy',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Dibuat',
                field: 'createdDate',
                formatter: function (value) {

                    if (value == null) {
                        return '-';
                    }
                    return moment(value).format('DD-MM-YYYY');
                },
            },
            {
                title: 'Diupdate Oleh',
                field: 'lastModifiedBy',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true
            },
            {
                title: 'Terakhir Update',
                field: 'lastModifiedDate',
                formatter: function (value) {

                    if (value == null) {
                        return '-';
                    }
                    return moment(value).format('DD-MM-YYYY');
                },
            },
        ]
    });
}


function getDataEbupot(params) {
    var url = 'GetDataEbupot'
    $.get(url + "?" + $.param(params.data)).then(function (res) {
        params.success(res.result)
    })
}

function sendItem(url, method, data) {
    var fileUpload = $('#FileUpload').get(0);
    var files = fileUpload.files;
    if (files.length > 0) {
        var validext = ['pdf'];
        var ext = files[0].name.substring(files[0].name.lastIndexOf('.') + 1).toLowerCase();
        if ($.inArray(ext, validext) == -1) {
            return swalInfo("Please upload only pdf format file");
        }

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
                uploadFile(res.result);
            },
            error: function (err) {
                btn.html(btn.data('original-text'));
                swalError("Ebupot failed save");
            }
        })
    } else {
        swalWarning("Attachment is required");
    }
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

function swalInfo(message) {
    swal({
        title: "Information",
        text: message,
        type: "info",
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



function InitSelect2Branch() {
    $('#select2Branch').select2({
        placeholder: "Cabang",
        id: "id",
        minimumInputLength: 3,
        allowClear: true,
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
        return;
    });

    $('#select2BranchModal').select2({
        placeholder: "Cabang",
        id: "id",
        minimumInputLength: 3,
        allowClear: true,
        dropdownParent: $("#modalEbupot"),
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
    });

    $('#select2BranchModal2').select2({
        placeholder: "Cabang",
        id: "id",
        allowClear: true,
        dropdownParent: $("#modalEbupot"),
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
    });
}

function InitSelect2Vendor() {
    $('#select2Vendor').select2({
        placeholder: "Vendor",
        id: "id",
        allowClear: true,
        minimumInputLength: 3,
        ajax: {
            url: '/Post/GetSelectVendor',
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
    }).on("change", function (e) {
        var data = $("#select2Vendor").select2("data");
        $('#npwpVendor').val(data[0].extra);
    });

    $('#select2VendorModal').select2({
        placeholder: "Vendor",
        id: "id",
        allowClear: true,
        minimumInputLength: 3,
        dropdownParent: $("#modalEbupot"),
        ajax: {
            url: '/Post/GetSelectVendor',
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
    }).on("change", function (e) {
        var data = $("#select2VendorModal").select2("data");
        $('#npwpVendorModal').val(data[0].extra);
    });

    //$.ajax({
    //    url: '/Post/GetSelectVendor',
    //    data: function (params) {
    //        var query = {
    //            search: params.term,
    //            type: 'public'
    //        }
    //        return query;
    //    },
    //    dataType: 'json',
    //    delay: 250,
    //    processResults: function (data) {
    //        var newData = $.map(data.result, function (obj) {
    //            return obj;
    //        });
    //        return {
    //            results: newData
    //        };
    //    },
    //    cache: true,
    //    success: function (result) {
    //        $("#select2Vendor").select2({
    //            allowClear: true,
    //            id: "id",
    //            placeholder: "Vendor",
    //            data: [{ id: '', text: '' }].concat(result.result),
    //            width: "100%"
    //        }).on("select2:select", function (obj) {
    //            return;
    //        });
    //    },
    //    error: function (e) {
    //        swal("Error", "Load vendor failed", "error");
    //    },
    //});
}

function search() {
    initTableEbupot();
}
function download() {
    window.open("/Content/E-BUPOT/Manual/UserManual_EBUPOT.PDF", "_blank");
}
function clearSearchFilter() {
    $("#select2Branch").val('').trigger('change');
    $("#select2Vendor").val('').trigger('change');
    $("#noBuktiPotong").val('');
    $('#masaPajak').val('');
    initTableEbupot();
}

function uploadFile(id) {
    var fileUpload = $('#FileUpload').get(0);
    var files = fileUpload.files;

    var fileData = new FormData();

    fileData.append(files[0].name, files[0]);
    fileData.append('code', "Ebupot");
    fileData.append('ID', id);

    $.ajax({
        url: 'UploadFileBupot',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: fileData,
        success: function (result) {
            swalSuccess('Data saved');
            $("#modalEbupot").modal('hide');
            $table.bootstrapTable('refresh');
        },
        error: function (err) {
            $('#FileUpload').val('');
            swal("Error", err.statusText, "error");
        },
        complete: function () {
            btn.html(btn.data('original-text'));
        },
    });
}

function DownloadFileUpload(fileName, path) {
    var url = `/Post/DownloadFileBupot?fileName=${fileName}&path=${path}`;
    window.open(url, '_blank');
}

//function DownloadMultiFile() {
//    var ids = GetIdSelections();
//    url = "/POST/DownloadMultiFileBupot?ids=" + ids;
//    window.open(url, '_blank');
//}

function DownloadMultiFile() {
    var ids = GetIdSelections();
    if (ids.length == 0) {
        swalWarning("No bupot selected");
        return;
    }
    $.ajax({
        url: "/Post/ThrowDownloadId",
        contentType: 'application/json;charset=utf-8',
        type: 'json',
        method: 'POST',
        data: '{id: ' + JSON.stringify(ids) + '}',
        success: function (guid) {
            window.open('/POST/DownloadMultiFileBupot?guid=' + guid, '_blank');
        },
        cache: false
    });
}

function GetFormatValue(e) {
    return e.val(e.autoNumeric('get'));
}

function SetFormatValue(e) {
    e.autoNumeric("init", {
        aSep: ',',
        aDec: '.',
        aSign: ''
    });
}

function InitCheckboxEvents() {
    $table.on('check.bs.table uncheck.bs.table check-all.bs.table uncheck-all.bs.table', function () {
        ClearDataCurrentPage();
        GetSelectionCurrentPage();
    });
}

function GetSelectionCurrentPage() {
    $.map($table.bootstrapTable('getSelections'), function (row) {
        _dataSelection.push({
            "ID": row.ID,
            "Page": $table.bootstrapTable('getOptions').pageNumber
        });
    });
}

function ClearDataCurrentPage() {
    var dataPage = _dataSelection.filter((_dataSelection) => {
        return _dataSelection.Page == $table.bootstrapTable('getOptions').pageNumber
    });
    $.each(dataPage, function (i, value) {
        var index = _dataSelection.findIndex(obj => obj.ID == value.ID);
        _dataSelection.splice(index, 1);
    });
}

function responseHandler(res) {
    var _selections = _dataSelection.map((_dataSelection) => {
        return _dataSelection.ID
    });

    $.each(res.rows, function (i, row) {
        row.state = $.inArray(row.ID, _selections) !== -1;
    });
    return res;
}

function UpdateDataPageNumber(_data) {
    var _dataCbu = _data.map((_data) => {
        return _data.ID
    });


    $.each(_dataSelection, function (index, row) {

        if ($.inArray(row.ID, _dataCbu) >= 0) {
            _dataSelection[index].Page = $table.bootstrapTable('getOptions').pageNumber;
        }

    });
}

function GetIdSelections() {
    return _dataSelection.map((_dataSelection) => {
        return _dataSelection.ID
    });
}

function formatMoney(amount, decimalCount = 2, decimal = ".", thousands = ",") {
    try {
        decimalCount = Math.abs(decimalCount);
        decimalCount = isNaN(decimalCount) ? 2 : decimalCount;

        const negativeSign = amount < 0 ? "-" : "";

        let i = parseInt(amount = Math.abs(Number(amount) || 0).toFixed(decimalCount)).toString();
        let j = (i.length > 3) ? i.length % 3 : 0;

        return negativeSign + (j ? i.substr(0, j) + thousands : '') + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousands) + (decimalCount ? decimal + Math.abs(amount - i).toFixed(decimalCount).slice(2) : "");
    } catch (e) {
        console.log(e)
    }
}