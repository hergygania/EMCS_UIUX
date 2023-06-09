$tablePo = $('#table-po');
var baseUrl = location.origin;
function showLoading() {
    $("#loadingModal").modal({ backdrop: 'static', keyboard: false });
    $("#loadingModal").modal("show");
}

function closeLoading() {
    $("#loadingModal").modal("hide");
}

$(document).ready(function () {
    $('.select2').select2();
    InitSelect2Branch();
    InitSelect2StatusPO();
    InitSelect2Supplier();
    InitSelect2UserPIC();
    InitDateRangePODate();
    InitDateRangedeliveryDate();
    InitSelectSort();
    InitSelect2PO();
    InitSelect2InvoiceNo();
    $tablePo.bootstrapTable({
        url: '/Post/GetListReportSla',
        toolbar: '.toolbar',
        queryParams: function (p) {
            var poDate = $(".poDate").val();
            var deliveryDate = $(".deliveryDate").val();
            var poDateStart = "", poDateEnd = "", deliveryDateStart = "", deliveryDateEnd = "";

            if (poDate != "") {
                poDateStart = dateFormatter($(".poDate").data("daterangepicker").startDate._d);
                poDateEnd = dateFormatter($(".poDate").data("daterangepicker").endDate._d);
            }

            if (deliveryDate != "") {
                deliveryDateStart = dateFormatter($(".deliveryDate").data("daterangepicker").startDate._d);
                deliveryDateEnd = dateFormatter($(".deliveryDate").data("daterangepicker").endDate._d);
            }

            var param = {
                "statusPO": $("#select2StatusPO option:selected").val() ?? "",
                "startDeliveryDate": deliveryDateStart ?? "",
                "endDeliveryDate": deliveryDateEnd ?? "",
                "branch": $("#select2Branch option:selected").val() ?? "",
                "supplier": $("#select2Supplier option:selected").val() ?? "",
                "userPIC": $("#select2UserPIC option:selected").val() ?? "",
                "pono": $("#select2PO option:selected").val() ?? "",
                "invoiceno": $("#select2Invoice option:selected").val() ?? "",
                "startPODate": poDateStart ?? "",
                "endPODate": poDateEnd ?? "",
                "limit": p.limit,
                "offset": p.offset,
                "search": p.search,
                "sort": $("#sortBy").val(),
                "order": $("#orderBy").val()
            };

            return param;
        },
        pageSize: 10,
        pageList: [10, 25, 50, 100],
        cache: false,
        pagination: true,
        sidePagination: 'server',
        smartDisplay: false,
        trimOnSearch: false,
        columns: [
            {
                title: 'No',
                field: 'No',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                formatter: runningFormatterNoPaging
            },
            {
                title: 'PO No',
                field: 'PO_Number',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '350'

            },
            {
                title: 'Qty PO',
                field: 'QtyPO',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '350'

            },
            {
                title: 'Qty Done',
                field: 'QtyDone',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '350'

            },
            {
                title: 'Qty Outstanding',
                field: 'QtyOutstanding',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '350'

            },
            {
                title: 'PO Date',
                field: 'PO_Date',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '350',
                formatter: dateFormatter,
            },
            {
                title: 'PO Line Item',
                field: 'PO_lineitem',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '350'
            }, {
                title: 'Item Desc',
                field: 'ItemDescription',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '350'
            }, {
                title: 'Plant',
                field: 'Plant',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '350'
            }, {
                title: 'Vendor',
                field: 'VendorName',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '350'
            }, {
                title: 'PO sent to Vendor',
                field: 'POsenttoVendor',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '350',
                visible: false,
            }, {
                title: 'PO Confirm Date',
                field: 'PO_ConfirmDate',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '350',
                formatter: dateFormatter,
            }, {
                title: 'PR Number',
                field: 'PR_Number',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '350'
            },
            {
                title: 'PR line item',
                field: 'PR_lineitem',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '350'

            },
            {
                title: 'PR Date',
                field: 'PR_Date',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '350',
                formatter: dateFormatter,
            },
            {
                title: 'PR Creator',
                field: 'PR_Creator',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '110',

            },
            {
                title: 'Ordering By',
                field: 'OrderingBy',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '300',

            },
            {
                title: 'Request Date',
                field: 'RequestDate',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: dateFormatter,
            },
            {
                title: ' Promise Date',
                field: 'PromiseDate',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: dateFormatter,
            },
            {
                title: 'Aging',
                field: 'Aging',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '110',

            },
            {
                title: 'ETD',
                field: 'ETD',
                halign: 'center',
                class: 'text-nowrap',
                align: 'right',
                width: '150px',
                formatter: dateFormatter,
            },
            {
                title: 'ATD',
                field: 'ATD',
                halign: 'center',
                class: 'text-nowrap',
                align: 'right',
                width: '150px',
                formatter: dateFormatter,
            }, {
                title: 'ETA',
                field: 'ETA',
                halign: 'center',
                class: 'text-nowrap',
                align: 'right',
                width: '150px',
                formatter: dateFormatter,
            },
            {
                title: 'ATA',
                field: 'ATA',
                halign: 'center',
                class: 'text-nowrap',
                align: 'center',
                width: '300',
                formatter: dateFormatter,
            }, {
                title: 'GR/SA Number',
                field: 'SA_Number',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '110',

            },
            {
                title: 'GR/SA Posting Date',
                field: 'SA_PostingDate',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: dateFormatter,
            },
            {
                title: 'GR/SA Document Date',
                field: 'SA_DocumentDate',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: dateFormatter,
            },
            {
                title: 'GR/SA Amount',
                field: 'SA_Amount',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '110'

            }, {
                title: 'Cost Center',
                field: 'CostCenter',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                visible: false
            }, {
                title: 'Invoice Number',
                field: 'Invoice_Number',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
            }, {
                title: 'Invoice Posting Date',
                field: 'Invoice_PostingDate',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: dateFormatter,
            }, {
                title: 'Invoice Date',
                field: 'Invoice_Date',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: dateFormatter,
            }, {
                title: 'PO Status',
                field: 'POStatus',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
            },
            {
                title: 'SLA',
                field: 'SLA',
                halign: 'center',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
            }
        ]
    });
})

function InitSelect2Branch() {
    $('#select2Branch').select2({
        placeholder: "Branch",
        id: "id",
        ajax: {
            url: '/Post/GetSelectBranch',
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
    }).on("select2:select", function (obj) {
        return;
    });
}

function InitSelect2InvoiceNo() {
    $('#select2Invoice').select2({
        placeholder: "Invoice Number",
        id: "id",
        ajax: {
            url: '/Post/GetSelectInvoice',
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
    }).on("select2:select", function (obj) {
        return;
    });
}
function InitSelect2PO() {
    $('#select2PO').select2({
        placeholder: "PO",
        id: "id",
        ajax: {
            url: '/Post/GetSelectPO',
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
    }).on("select2:select", function (obj) {
        return;
    });
}
function InitSelect2StatusPO() {
    var StatusPO = ["On Schedule"
        , "Risk Delay"
        , "Delay"
        , "POD"
        , "In Transit"
        , "BAST"
        , "On Progress"
        , "Complete"
    ]
    $('#select2StatusPO').select2({
        placeholder: "Status PO",
        allowClear: true,
        data: StatusPO,
    }).on("select2:select", function (obj) {
        return;
    });
}

function InitSelect2DeliveryStatus() {
    $('#select2DeliveryStatus').select2({
        placeholder: "Delivery Status",
        id: "id",
        ajax: {
            url: '/Post/GetSelectDeliveryStatus',
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
}

function InitSelect2Supplier() {
    $('#select2Supplier').select2({
        placeholder: "Supplier",
        id: "id",
        allowClear: true,
        ajax: {
            url: '/Post/GetSelectSupplier',
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
}

function InitSelect2UserPIC() {
    $('#select2UserPIC').select2({
        placeholder: "User",
        id: "id",
        ajax: {
            url: '/Post/GetSelectUserPIC',
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
}

function RefreshTableAdvanceSearch() {
    $tablePo.bootstrapTable("refresh");
    //$('#table-po').bootstrapTable('destroy');
    //InitTableReport();
}

function DownloadAdvanceSearch(e) {
    showLoading();
    var poDate = $(".poDate").val();
    var deliveryDate = $(".deliveryDate").val();
    var poDateStart = "", poDateEnd = "", deliveryDateStart = "", deliveryDateEnd = "";

    if (poDate != "") {
        poDateStart = dateFormatter($(".poDate").data("daterangepicker").startDate._d);
        poDateEnd = dateFormatter($(".poDate").data("daterangepicker").endDate._d);
        //poDateStart = $(".poDate").data("daterangepicker").startDate._d;
        //poDateEnd = $(".poDate").data("daterangepicker").endDate._d;
    }

    if (deliveryDate != "") {
        deliveryDateStart = dateFormatter($(".deliveryDate").data("daterangepicker").startDate._d);
        deliveryDateEnd = dateFormatter($(".deliveryDate").data("daterangepicker").endDate._d);
        //deliveryDateStart = $(".deliveryDate").data("daterangepicker").startDate._d;
        //deliveryDateEnd = $(".deliveryDate").data("daterangepicker").endDate._d;
    }

    var param = {
        "statusPO": $("#select2StatusPO option:selected").val() ?? "",
        "startDeliveryDate": deliveryDateStart ?? "",
        "endDeliveryDate": deliveryDateEnd ?? "",
        "branch": $("#select2Branch option:selected").val() ?? "",
        "supplier": $("#select2Supplier option:selected").val() ?? "",
        "userPIC": $("#select2UserPIC option:selected").val() ?? "",
        "startPODate": poDateStart ?? "",
        "endPODate": poDateEnd ?? "",
        "pono": $("#select2PO option:selected").val() ?? "",
        "invoiceno": $("#select2Invoice option:selected").val() ?? "",
    };

    $.ajax({
        cache: false,
        //async: false,
        url: 'DownloadReportExcelSLA',
        method: 'GET',
        data: param,
        success: function (res) {
            setTimeout(function () {
                closeLoading();
            }, 3000);
            url = "/POST/DownloadResultReportExcelSLA?guid=" + res
            window.open(url, '_blank');
        },
        error: function (err) {
            setTimeout(function () {
                closeLoading();
            }, 3000);
            swalSuccess(' failed Download!');
        }
    })
}

function InitDateRangePODate() {
    $('.poDate').daterangepicker({
        opens: 'right',
        locale: {
            format: 'DD.MMM.YYYY'
        },

    }, function (start, end) {
        return;
    });
}

function InitDateRangedeliveryDate() {
    $('.deliveryDate').daterangepicker({
        opens: 'right',
        locale: {
            format: 'DD.MMM.YYYY'
        },

    }, function (start, end) {
        return;
    });
}

function InitSelectSort() {
    $.get(baseUrl + "/Post/GetParamByGroup?groupName=SortingReport", function (data) {
        if (data.status === "SUCCESS") {
            var options = "";
            jQuery.each(data.result, function (index, element) {
                options += "<option value='" + element.Value + "' " + ((element.Sort === 1) ? "selected" : "") + ">" + element.Name + "</option>";
            })

            $("#sortBy").append(options);
        }
    })
}