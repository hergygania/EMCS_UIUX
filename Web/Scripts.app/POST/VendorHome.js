var Role = $('#Role').val();
var btnActionIncoming = `<a href="DetailOutstanding?Id=" class="btn btn-default btn-xs" style="background:#fcba12;color:white;"><i class="fa fa-eye"></i></a>`;
var btnActionInprogress = `<a href="/Post/DetailProcess?Id=1" class="btn btn-default btn-xs " style="background:#fcba12;color:white!important;" title="view"><i class="fa fa-eye"></i></a>`;
var btnActionDone = `<a href="/Post/DetailPreview?Id=1" class="btn btn-default btn-xs " style="background:#fcba12;color:white!important;" title="view"><i class="fa fa-eye"></i></a>`;
var check = `<span data-toggle="tooltip" data-placement="right" title="-" class="fa fa-check-circle text-primary"></span>`;
var uncheck = `<span class="fa fa-minus-circle text-disabled"></span>`;
var current = `<span class="fa fa-map-marker text-success text-success fa-spinner fa-pulse"></span>`;
VendorNameVisible = false;
var VisibleColumnProgress = Role.includes('POSTFINANCE') || Role.includes('POSTFINANCEBRANCH') ? false : true;
var VisibleColumnFinance = Role.includes('POSTFINANCE') || Role.includes('POSTFINANCEBRANCH') ? true : false;
var VisibleColumnDeliveryStatusPO = Role.includes('POSTFINANCE') || Role.includes('POSTFINANCEBRANCH') ? false : true;

var VisibleColumnComment = Role.includes('POSTVENDOR') || Role.includes('POSTEXPEDITOR') || Role.includes('POSTTAX') || Role.includes('POST') ? true : false;
var tableUploadPo = $('#table-uploadPO');

$tableIncoming = $("#table-po");
$tableInProgress = $("#table-inprogress");
$tableDone = $("#table-done");
$tableReject = $("#table-reject");
var baseUrl = location.origin;

Dropzone.autoDiscover = false;

$(document).ready(function () {
<<<<<<< HEAD

    if (landingPage == 1) {
        CheckPopUpHomePage();
    }

    else {
        $("#idpopUp").hide();
        $("#idpopUpHardCopyInvoice").hide();
        $("#idpopUpPPN").hide();
        $("#idhome").show();
    }

=======
    CheckPopUpHomePage();
>>>>>>> 639d8d0 (Intial commit)
    initDateRange();
    InitTableViewHistory();
    $('[data-toggle="tooltip"]').tooltip()
    $(".showTab").on("click", function () {
        var elm = $(this).data('content');

        $(".tab-pane").each(function (index) {
            $(this).removeClass("active");
        });

        $(".showTab").each(function (idex) {
            $(this).removeClass("active");
        })
        $(this).addClass("active");
        $("#" + elm).addClass("show active");
        return false;
    });

    $tableIncoming.bootstrapTable({
        url: "GetListPOInComing",
        pagination: true,
        sidePagination: 'server',
        pageSize: 10,
        queryParams: function (p) {
            var param = {};
            var startDatePoReceipt = dateFormatter($("#poReceipt").data("daterangepicker").startDate._d) ?? "";
            var endDatePoReceipt = dateFormatter($("#poReceipt").data("daterangepicker").endDate._d) ?? "";
            var startDateDeliveryDate = dateFormatter($("#deliveryDate").data("daterangepicker").startDate._d) ?? "";
            var endDateDeliveryDate = dateFormatter($("#deliveryDate").data("daterangepicker").endDate._d) ?? "";

            var PoReceipt = $("#poReceipt").val();
            var DeliveryDate = $("#deliveryDate").val();
            if (PoReceipt == "") {
                startDatePoReceipt = "";
                endDatePoReceipt = "";
            }

            if (DeliveryDate == "") {
                startDateDeliveryDate = "";
                endDateDeliveryDate = "";
            }

            param.PoNo = $("#poNo").val();
            param.StartDatePoReceipt = startDatePoReceipt;
            param.EndDatePoReceipt = endDatePoReceipt;
            param.StartDateDeliveryDate = startDateDeliveryDate;
            param.EndDateDeliveryDate = endDateDeliveryDate;
            param.limit = p.limit;
            param.offset = p.offset;
            return param;
        },
        pageList: [10, 25, 50, 100],
        responseHandler: function (res) {
            var offset = res.offset;
            $.each(res.rows, function (i, row) {
                row.index = offset + (i + 1);
            });
            return res;
        },
        cache: false,
        smartDisplay: false,
        trimOnSearch: false,
        columns: [
            {
                title: 'No',
                field: 'No',
                class: 'text-center',
                align: 'center',
                width: '50',
                formatter: runningFormatter
            },
            {
                title: 'Action',
                field: 'Request_Id',
                class: 'text-center',
                dataAlign: 'center',
                width: '80',
             
                formatter: function (data, row, index) {
                    var button1 = `<a href="DetailOutstanding?Id=${data}" class="btn btn-default btn-xs" style="background:#fcba12;color:white;"><i class="fa fa-eye"></i></a>`;
                    var button2 = '';
                    if (Role.includes('POSTVENDOR') || Role.includes('POSTEXPEDITOR') || Role.includes('POSTTAX') || Role.includes('POSTFINANCEBRANCH')) {
                        button2 = `<a href="javascript:void(0);" onclick="ShowComment(` + row.Request_Id + `, ` + row.PO_No + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;"><i class="fa fa-comment-dots"></i><span style="display:none;" class="total-comment-` + row.Request_Id + `">&nbsp;0</span><span style="display:none;" class="badge-custom badge-custom-` + row.Request_Id + `">&nbsp;0</span></a>`;
                    }
                  
                    return [button1, button2].join(" ");
                }
            },
            {
                title: 'Company',
                field: 'Vendor_Name',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                visible: VendorNameVisible,
            },
            {
                title: 'PO No',
                field: 'PO_No',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: function (data, row, index) {
                    var buttons = `<a href="javascript:void(0);" onclick="InitModalUploadPO(` + row.Request_Id + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;">` + data + `</a>`;
                    return buttons;
                }
            },
            {
                title: 'Line Item',
                field: 'CountLineNumber',
                halign: 'center',
                align: 'center',
                width: '110'
            },
            {
                title: 'Subtotal',
                field: 'SubTotal',
                align: 'center',
                align: 'right',
                width: '110',
                formatter: currencyFormatter,
            },
            {
                title: 'Total Include Tax (10%)',
                field: 'SubTotalTax10',
                align: 'center',
                align: 'right',
                visible: false,
                width: '110',
                formatter: currencyFormatter,
            },
            {
                title: 'Delivery Date',
                field: 'Delivery_Date',
                class: 'text-nowrap',
                align: 'center',
                width: '110'

            },
            {
                title: 'Contact Name',
                field: 'PICName',
                class: 'text-nowrap',
                align: 'center',
                width: '250',

            },
            {
                title: 'Contact Email',
                field: 'PICEmail',
                class: 'text-nowrap',
                align: 'center',
                width: '250'
            },
            {
                title: 'PO Receipt Date',
                field: 'PO_Receipt_Date',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: false,
                formatter: dateSAPFormatter,
            }
        ],
        onLoadSuccess: function () {
            var PoList = [];
            var table = $tableIncoming.bootstrapTable('getData');
            if (table.length > 0) {
                jQuery.each(table, function (index, value) {
                    PoList.push(value.Request_Id);
                })
                GetTotalComment(PoList);
                GetTotalUnread(PoList);
            }
        }
    })

    $tableInProgress.bootstrapTable({
        url: "GetListPOInProgress",
        pagination: true,
        sidePagination: 'server',
        pageSize: 10,
        queryParams: function (p) {
            var param = {};

            var startDatePoReceipt = dateFormatter($("#poReceipt").data("daterangepicker").startDate._d) ?? "";
            var endDatePoReceipt = dateFormatter($("#poReceipt").data("daterangepicker").endDate._d) ?? "";
            var startDateDeliveryDate = dateFormatter($("#deliveryDate").data("daterangepicker").startDate._d) ?? "";
            var endDateDeliveryDate = dateFormatter($("#deliveryDate").data("daterangepicker").endDate._d) ?? "";

            var PoReceipt = $("#poReceipt").val();
            var DeliveryDate = $("#deliveryDate").val();
            if (PoReceipt == "") {
                startDatePoReceipt = "";
                endDatePoReceipt = "";
            }

            if (DeliveryDate == "") {
                startDateDeliveryDate = "";
                endDateDeliveryDate = "";
            }

            param.PoNo = $("#poNo").val();
            param.StartDatePoReceipt = startDatePoReceipt;
            param.EndDatePoReceipt = endDatePoReceipt;
            param.StartDateDeliveryDate = startDateDeliveryDate;
            param.EndDateDeliveryDate = endDateDeliveryDate;

            param.limit = p.limit;
            param.offset = p.offset;
            return param;
        },
        pageList: [10, 25, 50, 100],
        cache: false,
        smartDisplay: false,
        trimOnSearch: false,
        responseHandler: function (res) {
            var offset = res.offset;
            $.each(res.rows, function (i, row) {
                row.index = offset + (i + 1);
            });
            return res;
        },

        columns: [
            {
                title: 'No',
                field: 'No',
                class: 'text-center',
                align: 'center',
                width: '50',
                formatter: runningFormatter

            },
            {
                title: 'Log',
                field: 'Log',
                class: 'text-center',
                dataAlign: 'center',
                width: '20',
                formatter: function (data, row, index) {
                    var NextProcessName = row.NextProcessName;
                    var url = LinkUrlRequest(NextProcessName, row.Request_Id);
                    var button1 = `<a href="javascript:void(0);" onclick="ShowHistory(` + row.Request_Id + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;"><i class="fa fa-history"></i></a>`;
                    var buttons = [button1].join(" ");
                    return '<div style="width:20px;">' + buttons + '</div>';
                }
            },
            {
                title: 'View',
                field: 'Action',
                class: 'text-center',
                dataAlign: 'center',
                width: '40',
                formatter: function (data, row, index) {
                    var NextProcessName = row.NextProcessName;
                    var url = LinkUrlRequest(NextProcessName, row.Request_Id);
                    var button1 = `<a href="${url}" class="btn btn-default btn-xs " style="background:#fcba12;color:white!important;" title="view"><i class="fa fa-eye"></i></a>`;
                    var buttons = [button1].join(" ");
                    return '<div style="width:20px;">' + buttons + '</div>';
                }
            },
            {
                title: 'Comment',
                field: 'Comment',
                class: 'text-center',
                dataAlign: 'center',
                width: '40',
                visible: VisibleColumnComment,
                formatter: function (data, row, index) {
                    var NextProcessName = row.NextProcessName;
                    var url = LinkUrlRequest(NextProcessName, row.Request_Id);                    
                    var  button2 = `<a href="javascript:void(0);" onclick="ShowComment(` + row.Request_Id + `, ` + row.PO_No + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;"><i class="fa fa-comment-dots"></i><span style="display:none;" class="total-comment-` + row.Request_Id + `">&nbsp;0</span><span style="display:none;" class="badge-custom badge-custom-` + row.Request_Id + `">&nbsp;0</span></a>`;
                    var buttons = [button2].join(" ");
                    return '<div style="width:20px;">' + buttons + '</div>';
                }
            },
            {
                title: 'Company',
                field: 'Vendor_Name',
                class: 'text-nowrap',
                align: 'left',
                width: '110',

            },
            {
                title: 'PO No',
                field: 'PO_No',
                class: 'text-center',
                align: 'center',
                width: '110',
                formatter: function (data, row, index) {
                    var buttons = `<a href="javascript:void(0);" onclick="InitModalUploadPO(` + row.Request_Id + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;">` + data + `</a>`;
                    return buttons;
                }
            },

            {
                title: 'PrePayment',
                field: 'PrePayment',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: VisibleColumnProgress,
                formatter: GetProcessFlowChecklistPrePayment,
            },
            {
                title: 'Line Item',
                field: 'CountLineNumber',
                halign: 'center',
                align: 'center',
                width: '70'

            },
            {
                title: 'Subtotal',
                field: 'SubTotal',
                halign: 'center',
                align: 'right',
                width: '110',
                formatter: currencyFormatter,

            },
            {
                title: 'Total Include Tax (10%)',
                field: 'SubTotalTax10',
                halign: 'center',
                align: 'right',
                width: '110',
                visible: false,
                formatter: currencyFormatter,

            },
            {
                title: 'Delivery Date',
                field: 'Delivery_Date',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: VisibleColumnProgress,

            },
            {
                title: 'Contact Name',
                field: 'PICName',
                class: 'text-nowrap',
                align: 'left',
                halign: 'center',
                width: '250',
                visible: VisibleColumnProgress,

            },
            {
                title: 'Contact Email',
                field: 'PICEmail',
                class: 'text-nowrap',
                align: 'center',
                width: '250',
                visible: VisibleColumnProgress,
            },
            {
                title: 'PO Receipt Date',
                field: 'PO_Receipt_Date',
                class: 'text-center',
                align: 'center',
                width: '110',
                visible: VisibleColumnProgress,
                formatter: dateSAPFormatter
            },
            {
                title: 'Delivery Status PO',
                field: 'Status_PO',
                class: 'text-center',
                align: 'center',
                width: '110',
                visible: VisibleColumnDeliveryStatusPO,
                formatter: function (value, row, index) {
                    var Processing = row.Processing;
                    var data = "";
                    var color = "badge-danger";
                    if (value != null) {
                        data = value;
                        color = getBadgeColor(value);
                    }
                    else if (Processing == "current") {
                        data = "CONFIRMED";
                        color = "badge-warning";
                    }
                    else {
                        data = "OUTSTANDING";
                    }

                    if (data === "Delay" || data === "delay" || data === "DELAY") {
                        color = "badge-danger";
                    }
                    return `<span class='badge ` + color + `'>` + data + `</span>`;
                }
            },
            {
                title: 'Confirmation',
                field: 'Confirmation',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: VisibleColumnProgress,
                formatter: GetProcessFlowChecklistConfirmation,
            },
            {
                title: 'Processing',
                field: 'Processing',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: VisibleColumnProgress,
                formatter: GetProcessFlowChecklistProcessing,
            },
            {
                title: 'Delivering',
                field: 'Delivering',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: VisibleColumnProgress,
                formatter: GetDeliveryFlowChecklist,
            },
            {
                title: 'BAST',
                field: 'BAST',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: VisibleColumnProgress,
                formatter: GetProcessFlowChecklistBast,
            }, {
                title: 'GR/SA',
                field: 'GRSA',
                class: 'text-center',
                align: 'center',
                width: '100px',
                formatter: GetProcessFlowChecklistGRSA,
<<<<<<< HEAD
            },
            //{
            //    title: 'Document Finance',
            //    field: 'DocumentFinance',
            //    class: 'text-center',
            //    align: 'center',
            //    width: '100px',
            //    visible: VisibleColumnFinance,
            //    formatter: GetProcessFlowChecklistInvoiceDocument,
            //},
           
=======
            },  
>>>>>>> 639d8d0 (Intial commit)
            {
                title: 'Invoice Submit',
                field: 'Invoice_Date',
                class: 'text-center',
                align: 'center',
                width: '100px',

                formatter: GetProcessFlowChecklistInvoice,
            },
            {
                title: 'Submitted On',
                field: 'Invoice_Date',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: VisibleColumnFinance,              
            },
            {
<<<<<<< HEAD
                title: 'Invoice Date',
                field: 'Invoice_Date',
=======
                title: 'Invoice Submitted',
                field: 'Invoicing',
>>>>>>> 639d8d0 (Intial commit)
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: VisibleColumnFinance,  

                formatter: GetProcessFlowChecklistInvoice,
            },
            {
                title: 'Invoice Posted',
                field: '',
                class: 'text-center',
                align: 'center',
                width: '100px',
                formatter: GetProcessFlowChecklistInvoiceSAP,
            },
            {
                title: 'Invoice Paid',
                field: 'PAID',
                class: 'text-center',
                align: 'center',
                width: '100px',
                formatter: GetProcessFlowChecklistPaidPO,
            }

        ],
        onLoadSuccess: function () {
            var PoList = [];
            var table = $tableInProgress.bootstrapTable('getData');
            if (table.length > 0) {
                jQuery.each(table, function (index, value) {
                    PoList.push(value.Request_Id);
                })
                GetTotalComment(PoList);
                GetTotalUnread(PoList);
            }
        }
    })

    $tableDone.bootstrapTable({
        url: "GetListPODone",
        pagination: true,
        sidePagination: 'server',
        pageSize: 10,
        queryParams: function (p) {
            var param = {};
            var startDatePoReceipt = dateFormatter($("#poReceipt").data("daterangepicker").startDate._d) ?? "";
            var endDatePoReceipt = dateFormatter($("#poReceipt").data("daterangepicker").endDate._d) ?? "";
            var startDateDeliveryDate = dateFormatter($("#deliveryDate").data("daterangepicker").startDate._d) ?? "";
            var endDateDeliveryDate = dateFormatter($("#deliveryDate").data("daterangepicker").endDate._d) ?? "";

            var PoReceipt = $("#poReceipt").val();
            var DeliveryDate = $("#deliveryDate").val();
            if (PoReceipt == "") {
                startDatePoReceipt = "";
                endDatePoReceipt = "";
            }

            if (DeliveryDate == "") {
                startDateDeliveryDate = "";
                endDateDeliveryDate = "";
            }

            param.PoNo = $("#poNo").val();


            param.StartDatePoReceipt = startDatePoReceipt;
            param.EndDatePoReceipt = endDatePoReceipt;
            param.StartDateDeliveryDate = startDateDeliveryDate;
            param.EndDateDeliveryDate = endDateDeliveryDate;
            param.limit = p.limit;
            param.offset = p.offset;
            return param;
        },
        pageList: [10, 25, 50, 100],
        cache: false,
        smartDisplay: false,
        responseHandler: function (res) {
            var offset = res.offset;
            $.each(res.rows, function (i, row) {
                row.index = offset + (i + 1);
            });
            return res;
        },
        trimOnSearch: false,
        columns: [{
            title: 'No',
            field: 'No',
            class: 'text-center',
            align: 'center',
            width: '50',
            formatter: runningFormatter

        },
        {
            title: 'Action',
            field: 'Action',
            class: 'text-center',
            dataAlign: 'center',
            width: '80',
            formatter: function (data, row, index) {
                var button1 = `<a href="/Post/DetailDone?Id=${row.Request_Id}" class="btn btn-default btn-xs " style="background:#fcba12;color:white!important;" title="view"><i class="fa fa-eye"></i></a>`;
                var button2 = '';
                if (Role.includes('POSTVENDOR') || Role.includes('POSTEXPEDITOR') || Role.includes('POSTTAX') || Role.includes('POSTFINANCEBRANCH')) {
                    button2 = `<a href="javascript:void(0);" onclick="ShowComment(` + row.Request_Id + `, ` + row.PO_No + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;"><i class="fa fa-comment-dots"></i><span style="display:none;" class="total-comment-` + row.Request_Id + `">&nbsp;0</span><span style="display:none;" class="badge-custom badge-custom-` + row.Request_Id + `">&nbsp;0</span></a>`;
                }
                    var buttons = [button1, button2].join(" ");
                return '<div style="width:150px;">' + buttons + '</div>';
            }
        },
        {
            title: 'Company',
            field: 'Vendor_Name',
            class: 'text-nowrap',
            align: 'center',
            width: '110',
        },
        {
            title: 'PO No',
            field: 'PO_No',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: function (data, row, index) {
                var buttons = `<a href="javascript:void(0);" onclick="InitModalUploadPO(` + row.Request_Id + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;">` + data + `</a>`;
                return buttons;
            }
        },
        {
            title: 'Line Item',
            field: 'CountLineNumber',
            halign: 'center',
            align: 'center',
            width: '110',

        },
        {
            title: 'Subtotal',
            field: 'SubTotal',
            align: 'center',
            align: 'right',
            width: '110',
            formatter: currencyFormatter,
        },
        {
            title: 'Total Include Tax (10%)',
            field: 'SubTotalTax10',
            align: 'center',
            align: 'right',
            width: '110',
            visible: false,
            formatter: currencyFormatter,
        },
        {
            title: 'Delivery Date',
            field: 'Delivery_Date',
            class: 'text-center text-nowrap',
            align: 'center',
            width: '110',
            visible: VisibleColumnProgress,
        },
        {
            title: 'Contact Name',
            field: 'PICName',
            class: 'text-center',
            align: 'center',
            width: '110',
            visible: VisibleColumnProgress,
        },
        {
            title: 'Contact Email',
            field: 'PICEmail',
            class: 'text-center',
            align: 'center',
            width: '110',
            visible: VisibleColumnProgress,
        },
        {
            title: 'PO Receipt Date',
            field: 'PO_Receipt_Date',
            class: 'text-center',
            align: 'center',
            width: '110',
            visible: VisibleColumnProgress,
            formatter: dateSAPFormatter

        },
        {
            title: 'Delivery Status PO',
            field: 'Status_PO',
            class: 'text-center',
            align: 'center',
            width: '110',
            visible: VisibleColumnDeliveryStatusPO,
            formatter: function (value, row, index) {

                var data = "";
                var color = "badge-danger";
                if (value != null) {
                    data = value;
                    color = getBadgeColor(value);
                } else {
                    data = "OUTSTANDING";
                }
                return `<span class='badge ` + color + `'>` + data + `</span>`;
            }
        },
        {
            title: 'Confirmation',
            field: 'Confirmation',
            class: 'text-center',
            align: 'center',
            width: '100px',
            visible: VisibleColumnProgress,
            formatter: GetProcessFlowChecklistConfirmation,
        },
        {
            title: 'Processing',
            field: 'Processing',
            class: 'text-center',
            align: 'center',
            width: '100px',
            visible: VisibleColumnProgress,
            formatter: GetProcessFlowChecklistProcessing,
        },
        {
            title: 'Delivering',
            field: 'Delivering',
            class: 'text-center',
            align: 'center',
            width: '100px',
            visible: VisibleColumnProgress,
            formatter: GetDeliveryFlowChecklist,
        },
        {
            title: ' BAST',
            field: 'BAST',
            class: 'text-center',
            align: 'center',
            width: '100px',
            visible: VisibleColumnProgress,
            formatter: GetProcessFlowChecklistBast,
        },
        {
            title: 'GR/SA',
            field: 'GRSA',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetProcessFlowChecklistGRSA,
        },
        //{
        //    title: 'Document Finance',
        //    field: 'DocumentFinance',
        //    class: 'text-center',
        //    align: 'center',
        //    width: '100px',
        //    visible: VisibleColumnFinance,
        //    formatter: GetProcessFlowChecklistInvoice,
        //},
       
        {
            title: 'Invoice Submit',
            field: 'Invoicing',
            class: 'text-center',
            align: 'center',
            width: '100px',

            formatter: GetProcessFlowChecklistInvoice,
        },
        {
            title: 'Submitted On',
            field: 'Invoice_Date',
            class: 'text-center',
            align: 'center',
            width: '100px',
            visible: VisibleColumnFinance,
        },
        {
            title: 'Invoice Submitted',
            field: 'Invoicing',
            class: 'text-center',
            align: 'center',
            width: '100px',
            visible: VisibleColumnFinance,  
            formatter: GetProcessFlowChecklistInvoice,
        }, {
            title: 'Invoice Posted',
            field: '',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetProcessFlowChecklistInvoiceSAP,
        },
        {
            title: 'Invoice Paid',
            field: 'PAID',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetProcessFlowChecklistPaidPO,
        }],
        onLoadSuccess: function () {
            var PoList = [];
            var table = $tableDone.bootstrapTable('getData');
            if (table.length > 0) {
                jQuery.each(table, function (index, value) {
                    PoList.push(value.Request_Id);
                })
                GetTotalComment(PoList);
                GetTotalUnread(PoList);
            }
        }
    })

    $tableReject.bootstrapTable({
        url: "GetListPOReject",
        pagination: true,
        sidePagination: 'server',
        pageSize: 10,
        queryParams: function (p) {
            var param = {};
            var startDatePoReceipt = dateFormatter($("#poReceipt").data("daterangepicker").startDate._d) ?? "";
            var endDatePoReceipt = dateFormatter($("#poReceipt").data("daterangepicker").endDate._d) ?? "";
            var startDateDeliveryDate = dateFormatter($("#deliveryDate").data("daterangepicker").startDate._d) ?? "";
            var endDateDeliveryDate = dateFormatter($("#deliveryDate").data("daterangepicker").endDate._d) ?? "";

            var PoReceipt = $("#poReceipt").val();
            var DeliveryDate = $("#deliveryDate").val();
            if (PoReceipt == "") {
                startDatePoReceipt = "";
                endDatePoReceipt = "";
            }

            if (DeliveryDate == "") {
                startDateDeliveryDate = "";
                endDateDeliveryDate = "";
            }

            param.PoNo = $("#poNo").val();
            param.StartDatePoReceipt = startDatePoReceipt;
            param.EndDatePoReceipt = endDatePoReceipt;
            param.StartDateDeliveryDate = startDateDeliveryDate;
            param.EndDateDeliveryDate = endDateDeliveryDate;
            param.limit = p.limit;
            param.offset = p.offset;
            return param;
        },
        pageList: [10, 25, 50, 100],
        cache: false,
        smartDisplay: false,
        trimOnSearch: false,
        columns: [
            {
                title: 'No',
                field: 'No',
                class: 'text-center',
                align: 'center',
                width: '50',
                formatter: runningFormatterNoPaging

            },
            {
                title: 'Company',
                field: 'Vendor_Name',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: false,
            },
            {
                title: 'PO No',
                field: 'PO_No',
                class: 'text-center',
                align: 'center',
                width: '110'
            },
            {
                title: 'Line Item',
                field: 'CountLineNumber',
                halign: 'center',
                align: 'center',
                width: '110',

            },
            {
                title: 'Subtotal',
                field: 'SubTotal',
                align: 'center',
                align: 'right',
                width: '110',
                formatter: currencyFormatter,
            },
            {
                title: 'Total Include Tax (10%)',
                field: 'SubTotalTax10',
                align: 'center',
                align: 'right',
                width: '110',
                visible: false,
                formatter: currencyFormatter,
            },
            {
                title: 'Delivery Date',
                field: 'Delivery_Date',
                class: 'text-center text-nowrap',
                align: 'center',
                width: '110',
                visible: false
            },
            {
                title: 'Contact Name',
                field: 'PICName',
                class: 'text-center',
                align: 'center',
                width: '110'
            },
            {
                title: 'Contact Email',
                field: 'PICEmail',
                class: 'text-center',
                align: 'center',
                width: '110'
            },
            {
                title: 'PO Receipt Date',
                field: 'PO_Receipt_Date',
                class: 'text-center',
                align: 'center',
                width: '110',
                formatter: dateSAPFormatter

            },
            {
                title: 'Delivery Status PO',
                field: 'Status_PO',
                class: 'text-center',
                align: 'center',
                width: '110',
                visible: false,
                formatter: function (value, row, index) {

                    var data = "";
                    var color = "badge-danger";
                    if (value != null) {
                        data = value;
                        color = getBadgeColor(value);
                    } else {
                        data = "OUTSTANDING";
                    }
                    return `<span class='badge ` + color + `'>` + data + `</span>`;
                }
            },
            {
                title: 'Confirmation',
                field: 'Confirmation',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: false,
                formatter: GetProcessFlowChecklistConfirmation,
            },
            {
                title: 'Processing',
                field: 'Processing',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: false,
                formatter: GetProcessFlowChecklistProcessing,
            },
            {
                title: 'Delivering',
                field: 'Delivering',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: false,
                formatter: GetDeliveryFlowChecklist,
            },
            {
                title: ' BAST',
                field: 'BAST',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: false,
                formatter: GetProcessFlowChecklistBast,
            },
            {
                title: 'GR/SA',
                field: 'Invoicing',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: false,
                formatter: GetProcessFlowChecklistInvoice,
            },
            {
                title: 'Invoicing',
                field: 'Invoicing',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: false,
                formatter: GetProcessFlowChecklistInvoice,
            }, {
                title: 'Paid',
                field: 'PAID',
                class: 'text-center',
                align: 'center',
                width: '100px',
                visible: false,
                formatter: GetProcessFlowChecklistInvoice,
            }

        ],
        onLoadSuccess: function () {
            var PoList = [];
            var table = $tableReject.bootstrapTable('getData');
            if (table.length > 0) {
                jQuery.each(table, function (index, value) {
                    PoList.push(value.Request_Id);
                })
                GetTotalComment(PoList);
                GetTotalUnread(PoList);
            }
        }
    })

    tableUploadPo.bootstrapTable({
        url: '/Post/GetListAttachment',
        pagination: true,
        serverSort: false,
        loadingTemplate: "loadingTemplate",
        queryParams: function (params) {
            var query = {
                type: "PO",
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        pageSize: 10,
        pageList: [50, 100, 150, 200, 250],
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
                field: 'Action',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: function (row, data, index) {
                    return ` <a href="#" onclick="DownloadFileUpload(${data.ID});" class="btn btn-light btn-xs" title="Download"><span class="fa fa-download"></span></a>`
                }
            },
            {
                title: 'FileName',
                field: 'FileName',
                class: 'text-nowrap',
                halign: 'center',
                align: 'left',
                width: '450'

            },
        ]
    })
})


function InitTableViewHistory() {
    $('#table-history').bootstrapTable({
        detailView: false,
        url: '/Post/GetListHistory',
        queryParams: function (params) {
            var query = {
                id: params.id ?? 0,
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        columns: [{
            title: 'PO',
            field: 'PO_Number',
            class: 'text-center',
            align: 'center',
            width: '110'
        },
        {
            title: 'Step',
            field: 'ProcessName',
            class: 'text-center',
            align: 'center',
            width: '110'
        },
        {
            title: 'Status',
            field: 'StatusName',
            class: 'text-center',
            align: 'center',
            width: '110'
        },
        {
            title: 'Updated By',
            field: 'UpdatedBy',
            class: 'text-center',
            align: 'center',
            width: '110'
        },
        {
            title: 'Date',
            field: 'SubmitDate',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: dateFormattertime
        }]
    })
}

function InitTableViewNotes() {
    $('#table-viewnotes').bootstrapTable({
        detailView: false,
        url: '/Post/GetItemListByPO',
        queryParams: function (params) {
            var query = {
                id: params.id ?? 0,
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        columns: [{
            title: 'Item Description',
            field: 'Item_Description',
            class: 'text-center',
            align: 'center',
            width: '110'
        }, {
            title: 'Date',
            field: 'TimeStamp',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: dateSAPFormatter
        },
        {
            title: 'Step',
            field: 'Step',
            class: 'text-center',
            align: 'center',
            width: '110'
        },
        {
            title: 'Status',
            field: 'Status',
            class: 'text-center',
            align: 'center',
            width: '110'
        },
        {
            title: 'Notes',
            field: 'Notes',
            class: 'text-center',
            align: 'center',
            width: '110',
        },
        {
            title: 'ETA',
            field: 'ETA',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: dateSAPFormatter
        },
        {
            title: 'ATA',
            field: 'ATA',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: dateSAPFormatter
        },
        {
            title: 'ATD',
            field: 'ATD',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: dateSAPFormatter
        },
        {
            title: 'ETD',
            field: 'ETD',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: dateSAPFormatter
        },
        {
            title: 'Qty',
            field: 'Qty',
            class: 'text-center',
            align: 'center',
            width: '110'
        },
        {
            title: 'Qty Partial',
            field: 'QtyPartial',
            class: 'text-center',
            align: 'center',
            width: '110',
        }]
    })
}

function ShowHistory(id) {
    $.getJSON("GetListHistory?Id=" + id)
        .then(function (data) {
            $('#modalViewHistoryForm').modal();

            $("#table-history").bootstrapTable('refresh', {
                query: { id: id }
            });
        })

}
function getListComment(ReqId) {
    $.ajax({
        url: BaseUrl() + "ReadData",
        type: "GET",
        data: { RequestId: ReqId },
        success: function (resp) {
            var data = resp.result;
            renderComment(data);
        },
        error: function (err) {
            swal("Opps!", "Something Wrong when submit the comment!, Please Try Again", "error");
        },
        failed: function (fail) {
            swal("Opps!", "Something Wrong when submit the comment!, Please Try Again", "error");
        }
    })
}

function InitModalUploadPO(reqId) {
    var baseUrl = location.origin;
    document.getElementById("FormUploadPO").style.display = "none";
    $('#modalUploadPO').modal();
    tableUploadPo.bootstrapTable('refresh', {
        url: baseUrl + "/Post/GetListAttachment?id=" + reqId
    });

}

function InitTableUploadPO(reqId) {
    $('#table-uploadPO').bootstrapTable({
        pagination: true,
        serverSort: false,
        loadingTemplate: "loadingTemplate",
        url: '/Post/GetListAttachment',
        queryParams: function (params) {
            var query = {
                id: reqId,
                type: "PO",
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
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
                field: 'Action',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: function (row, data, index) {
                    return ` <a href="#" onclick="DownloadFileUpload(${data.ID});" class="btn btn-light btn-xs" title="Download"><span class="fa fa-download"></span></a>`
                }
            },
            {
                title: 'FileName',
                field: 'FileName',
                class: 'text-nowrap',
                halign: 'center',
                align: 'left',
                width: '450'

            },
        ]
    });
}

function DownloadFileUpload(id) {
    url = "/POST/DownloadFileRequest?id=" + id;
    window.open(url, '_blank');
}

function GetDeliveryFlowChecklist(data, row, index) {
    var PartialInProgress = `<span class="fa fa-check-circle text-warning"></span>`;
    var CurrentProgress = `<span class="fa fa-minus-circle text-disabled"></span>`;
    var Running = `<span class="fa fa-map-marker-alt text-success fa-spinner fa-pulse"></span>`;

    if (row.POType == "D") {
        if (data == 'running') {
            if (row.TotalComplete > 0 && row.TotalNotComplete > 0)
                return PartialInProgress
            else if (row.TotalComplete > 0 && row.TotalNotComplete == 0) {
                return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';
            }
            else
                return Running
        }
        else if (data == 'check') {
            return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';
        }
        else
            return CurrentProgress;


        // if (row.TotalNotComplete == 0) {
        // return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';
        // } else if (row.TotalNotComplete > 0) {
        // if (data == 'current') {
        // return current;
        // } else {
        // return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + uncheck + '</span>';
        // }
        // } else {
        // return current;
        // }
    } else {
        if (data == 'running') {
            if (row.TotalNotPOD > 0 && row.TotalPOD == 0)
                return Running
            else if (row.TotalNotPOD == 0 && row.TotalPOD > 0) {
                return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';
            }
            else
                return PartialInProgress
        }
        else if (data == 'check') {
            return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';
        }
        else
<<<<<<< HEAD
            return CurrentProgress;        
=======
            return CurrentProgress;


        // if (row.TotalNotPOD == 0) {
        // return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';

        // } else if (row.TotalNotPOD > 0) {
        // if (data == 'current') {
        // return PartialInProgress;
        // } else {
        // return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + uncheck + '</span>';
        // }
        // } else {
        // return current;
        // }
>>>>>>> 639d8d0 (Intial commit)
    }
}

function clearSearchHeader() {

}

function clearSearchFilter() {
    $("#poNo").val("");
    $("#poReceipt").val("");
    $("#deliveryDate").val("");
    if ($("#tabIncoming").hasClass("active")) {
        //$tableIncoming.bootstrapTable("refresh");
        $("#table-po").bootstrapTable("refresh");
    }

    if ($("#tabInprocess").hasClass("active")) {
        $("#table-inprogress").bootstrapTable("refresh");
    }

    if ($("#tabDone").hasClass("active")) {
        $("#table-done").bootstrapTable("refresh");
    }
    if ($("#tabReject").hasClass("active")) {
        $("#table-reject").bootstrapTable("refresh");
    }

}
function searchPO() {
    var PoReceipt = $("#poReceipt").val();
    var DeliveryDate = $("#deliveryDate").val();
    if (PoReceipt == "") {
        startDatePoReceipt = null;
        endDatePoReceipt = null;
    }

    if (DeliveryDate == "") {
        startDateDeliveryDate = null;
        endDateDeliveryDate = null;
    }



    if ($("#tabIncoming").hasClass("active")) {

        $("#table-po").bootstrapTable("refresh");
    }

    if ($("#tabInprocess").hasClass("active")) {
        $("#table-inprogress").bootstrapTable("refresh");
    }

    if ($("#tabDone").hasClass("active")) {
        $("#table-done").bootstrapTable("refresh");
    }

    if ($("#tabReject").hasClass("active")) {
        $("#table-reject").bootstrapTable("refresh");
    }

}

function showFileInvoice() {
    $("#modalViewInvoice").modal("show");
}

function showFileBast() {
    $("#modalViewBast").modal("show");
}

function initDateRange() {
    $('#poReceipt').daterangepicker({
        opens: 'right',
        locale: {
            format: 'DD.MMM.YYYY'
        }
    }, function (start, end) {
        var start_date = moment(start._d).format("DD.MM.YYYY");
        var end_date = moment(end._d).format("DD.MM.YYYY");

        $('#poReceipt').val(start_date + '-' + end_date);
    });

    $('#deliveryDate').daterangepicker({
        opens: 'right',
        locale: {
            format: 'DD.MMM.YYYY'
        },
    }, function (start, end) {
        var start_date = moment(start._d).format("DD.MM.YYYY");
        var end_date = moment(end._d).format("DD.MM.YYYY");

        $('#deliveryDate').val(start_date + '-' + end_date);
    });

}

function loadingTemplate(message) {
    return '<i class="fa fa-spinner fa-spin fa-fw fa-2x"></i>'
}


function CheckPopUpHomePage() {
    $.getJSON("CheckPopUp", function (data) {
        if (data.IsAgreeHomePage == true) {
            $("#idpopUp").hide();
<<<<<<< HEAD
            $("#idpopUpHardCopyInvoice").show();   
            $("#idpopUpPPN").hide();
            $("#idhome").hide();
=======
            $("#idhome").show();
>>>>>>> 639d8d0 (Intial commit)
        }
        else {
            $("#idpopUp").show();
            $("#idhome").hide();
<<<<<<< HEAD
             $("#idpopUpPPN").hide();
            $("#idhome").hide();
=======
>>>>>>> 639d8d0 (Intial commit)
        }
    });
}


function SavePopUpHome() {

    $.getJSON("SavePopUp?isChecked=true&description=HomePage", function (data) {
        $("#idpopUp").hide();
<<<<<<< HEAD
        $("#idpopUpHardCopyInvoice").hide();
        $("#idpopUpPPN").show();       
        $("#idhome").hide();
    });
}
function SavePopUpHardCopy() {   
        $("#idpopUp").hide();
        $("#idpopUpHardCopyInvoice").hide();
        $("#idpopUpPPN").show();
        $("#idhome").hide();
}
function SavePopUpPPN() {
    $("#idpopUp").hide();
    $("#idpopUpHardCopyInvoice").hide();
    $("#idpopUpPPN").hide();
    $("#idhome").show();
=======
        $("#idhome").show();

    });
>>>>>>> 639d8d0 (Intial commit)
}