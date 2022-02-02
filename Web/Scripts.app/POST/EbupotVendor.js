var $table = $('#table-ebupot');
var _dataSelection = [];

$(document).ready(function () {
    $('.select2').select2();
    InitDateRange();
    InitCheckboxEvents();
    initTableEbupot();
})

function InitDateRange() {
    $('.masaPajak').daterangepicker({
        opens: 'right',
        locale: {
            format: 'MMM.YYYY'
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

function initTableEbupot() {
    $table.bootstrapTable('destroy');
    $table.bootstrapTable({
        url: "GetEbupotVendor",
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
                title: 'No Bupot',
                field: 'noBupot',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'Rev Bupot',
                field: 'revBupot',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'Status Bupot',
                field: 'statusBupot',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'Cetak',
                field: 'cetak',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'NPWP Profile',
                field: 'npwpCabang',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'Nama Profile',
                field: 'namaCabang',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'Masa',
                field: 'masa',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'Tahun',
                field: 'tahun',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'Pembetulan',
                field: 'pembetulan',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'Pesan',
                field: 'pesanBupot',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'Tin',
                field: 'tin',
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
                sortable: true,
            },
            {
                title: 'NIK',
                field: 'nik',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'Nama Vendor',
                field: 'namaVendor',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'Email',
                field: 'emailVendor',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
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
                sortable: true,
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
                sortable: true,
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
                sortable: true,
            },
            {
                title: 'XML ID',
                field: 'refXmlId',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
            },
            {
                title: 'Ref Log File Id',
                field: 'refLogFileId',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
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
                sortable: true,
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
                sortable: true,
            },
            {
                title: 'Diupdate Oleh',
                field: 'lastModifiedBy',
                class: 'text-nowrap text-center',
                halign: 'center',
                align: 'left',
                width: '450',
                sortable: true,
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
                sortable: true,
            },
        ]
    });
}

function search() {
    initTableEbupot();
}
function download() {  
    window.open("/Content/E-BUPOT/Manual/UserManual_EBUPOT.PDF", "_blank");    
}
function clearSearchFilter() {
    $('#masaPajak').val('');
    $("#noBuktiPotong").val('');
    initTableEbupot();
}

function DownloadFileUpload(fileName, path) {
    var url = `/Post/DownloadFileBupot?fileName=${fileName}&path=${path}`;
    window.open(url, '_blank');
}

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