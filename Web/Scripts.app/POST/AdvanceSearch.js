$(document).ready(function () {
    $('.select2').select2();
    InitSelect2Branch();
    InitSelect2StatusPO();
    //InitSelect2DeliveryStatus();
    InitSelect2Supplier();
    InitSelect2UserPIC();
    InitDateRangePODate();
    InitDateRangedeliveryDate();
    InitTableAdvanceSearch();
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
        , "BAST"]
    $('#select2StatusPO').select2({
        placeholder: "Status PO",
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
        $("#table-po").bootstrapTable("refresh");
}

function DownloadAdvanceSearch() {
    var poDate = $(".poDate").val();
    var deliveryDate = $(".deliveryDate").val();
    var poDateStart = "", poDateEnd = "", deliveryDateStart = "", deliveryDateEnd = "";

    if (poDate != "") {
        poDateStart = $(".poDate").data("daterangepicker").startDate._d;
        poDateEnd = $(".poDate").data("daterangepicker").endDate._d;
    }

    if (deliveryDate != "") {
        deliveryDateStart = $(".deliveryDate").data("daterangepicker").startDate._d;
        deliveryDateEnd = $(".deliveryDate").data("daterangepicker").endDate._d;
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
    };


    $.ajax({
        cache: false,
        async: false,
        url: 'DownloadReportExcelSLA',
        method: 'GET',
        data: param,
        success: function (res) {
            url = "/POST/DownloadResultReportExcelSLA?guid=" + res
            window.open( url ,'_blank');
        },
        error: function (err) {
            swalSuccess(' failed Download!');
        }
    })
}

function InitTableAdvanceSearch() {
    $('#table-po').bootstrapTable({
        detailView: false,
        url: '/Post/GetListAdvanceSearch',
        method: 'post',
        contentType: 'application/x-www-form-urlencoded',
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
                "startPODate": poDateStart ?? "",
                "endPODate": poDateEnd ?? "",
            };


            return {
                model: param,
            };
        },
        pagination: true,
        serverSort: false,
        responseHandler: function (res) {
            return res.result;
        },
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
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '350'

            },
            {
                title: 'PO Date',
                field: 'PO_Date',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '350',
                formatter: dateSAPFormatter,
            },
            {
                title: 'Item Category',
                field: 'Item_Category',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '350'
            }, {
                title: 'PO Line Item',
                field: 'PO_lineitem',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '350'
            }, {
                title: 'Item Desc',
                field: 'ItemDescription',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '350'
            }, {
                title: 'Plant',
                field: 'Plant',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '350'
            }, {
                title: 'Vendor',
                field: 'VendorName',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '350'
            }, {
                title: 'PO sent to Vendor',
                field: 'POsenttoVendor',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                visible : false,
                align: 'center',
                width: '350'
            }, {
                title: 'PO Confirm Date',
                field: 'PO_ConfirmDate',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '350',
                formatter: dateSAPFormatter,
            }, {
                title: 'PR Number',
                field: 'PR_Number',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '350'
            },
            {
                title: 'PR line item',
                field: 'PR_lineitem',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '350'

            },
            {
                title: 'PR Date',
                field: 'PR_Date',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '350',
                formatter: dateSAPFormatter,
            },
            {
                title: 'PR Creator',
                field: 'PR_Creator',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'left',
                width: '110',

            },
            {
                title: 'Ordering By',
                field: 'OrderingBy',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                visible : false,
                align: 'center',
                width: '300',

            },

            {
                title: 'Request Date',
                field: 'RequestDate',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: dateSAPFormatter,
            },
            {
                title: ' Promise Date',
                field: 'PromiseDate',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: dateSAPFormatter,
            },
            {
                title: 'Aging',
                field: 'Aging',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '110',

            },
            //{
            //    title: 'Currency',
            //    field: 'Currency',
            //    halign: 'center',
            //    sortable: true,
            //    class: 'text-nowrap',
            //    align: 'center',
            //    width: '110',
            //    formatter: function (data, row, index) {
            //        return "IDR";
            //    }

            //},
            {
                title: 'ETD',
                field: 'ETD',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'right',
                width: '150px',
                formatter: dateSAPFormatter,
            },
            {
                title: 'ATD',
                field: 'ATD',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'right',
                width: '150px',
                formatter: dateSAPFormatter,
            }, {
                title: 'ATD',
                field: 'ATD',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'right',
                width: '150px',
                formatter: dateSAPFormatter,
            }, {
                title: 'ETA',
                field: 'ETA',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'right',
                width: '150px',
                formatter: dateSAPFormatter,
            },
            {
                title: 'ATA',
                field: 'ATA',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'center',
                width: '300',
                formatter: dateSAPFormatter,
            },
            {
                title: 'M-L',
                field: 'M_L',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'left',
                visible : false,
                width: '110',
            }, {
                title: 'P-O',
                field: 'P_O',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                visible : false,
                align: 'left',
                width: '110',
            }, {
                title: 'SA Number',
                field: 'SA_Number',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'left',
                width: '110',
            }, {
                title: 'SA Posting Date',
                field: 'SA_PostingDate',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: dateSAPFormatter,
            }, {
                title: 'SA Document Date',
                field: 'SA_DocumentDate',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: dateSAPFormatter,
            }, {
                title: 'Cost Center',
                field: 'CostCenter',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'left',
                width: '110',
            }, {
                title: 'Invoice Number',
                field: 'Invoice_Number',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'left',
                width: '110',
            }, {
                title: 'Invoice Posting Date',
                field: 'Invoice_PostingDate',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: dateSAPFormatter,
            }, {
                title: 'Invoice Date',
                field: 'Invoice_Date',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: dateSAPFormatter,
            }, {
                title: 'PO Status',
                field: 'PO_Status',
                halign: 'center',
                sortable: true,
                class: 'text-nowrap',
                align: 'left',
                width: '110',
            }
        ]
    });
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
