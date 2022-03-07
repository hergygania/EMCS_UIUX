var Role = $('#Role').val();

var tableUploadPo = $('#table-uploadPO');

$tableInvoiceIncoming = $("#table-InvoiceIncoming");
$tableInvoiceInProgress = $("#table-InvoiceInprogress");
$tableInvoiceDone = $("#table-InvoiceDone");
$tableInvoiceReject = $("#table-InvoiceReject");

var baseUrl = location.origin;

Dropzone.autoDiscover = false;
function InitTablehardcopyInvoice(InvoiceId) {
    $('#table-hardcopyinvoice').bootstrapTable({
        cache: false,
        async: false,
        detailView: false,
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
        detailView: false,
        url: '/Post/GetHardCopyInvoiceByInvoiceId',
        queryParams: function (params) {
            var query = {
                Id: InvoiceId,
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        columns: [
        {
            title: 'InvoiceId',
            field: 'InvoiceId',
            align: 'center',
            width: '120',
            visible: false
        },
        {
            title: 'PO_Number',
            field: 'PO_Number',
            align: 'center',
            width: '120'
        },
        {
            title: 'FileName',
            field: 'FileNameOri',
            align: 'center',
            width: '120'
        },
        {
            title: 'Recipients Name/ Receipt Number',
            field: 'ReceiptNameOrNumber',
            align: 'center',
            width: '120'
        },
        {
            title: 'Submission Type',
            field: 'SubmissionType',
            align: 'center',
            width: '120'
        },
        {
            title: 'Submission Date',
            field: 'SubmissionDate',
            align: 'center',
            width: '120',
            formatter: dateSAPFormatter
        }
        ]
    })
}

$(document).ready(function () {
   
    initDateRange();
    InitTableViewHistory();
    InitTablehardcopyInvoice(0);
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

    $tableInvoiceIncoming.bootstrapTable({
        url: "GetListInvoiceInComing",
        pagination: true,
        sidePagination: 'server',
        pageSize: 10,
        queryParams: function (p) {
            var param = {};
            var startInvoiceUploadDate = dateFormatter($("#InvoiceUploadDate").data("daterangepicker").startDate._d) ?? "";
            var endInvoiceUploadDate = dateFormatter($("#InvoiceUploadDate").data("daterangepicker").endDate._d) ?? "";
            var startInvoicePostingDate = dateFormatter($("#InvoicePostingDate").data("daterangepicker").startDate._d) ?? "";
            var endDateInvoicePostingDate = dateFormatter($("#InvoicePostingDate").data("daterangepicker").endDate._d) ?? "";

            var InvoiceUploadDate = $("#InvoiceUploadDate").val();
            var InvoicePostingDate = $("#InvoicePostingDate").val();
            if (InvoiceUploadDate == "") {
                startInvoiceUploadDate = "";
                endInvoiceUploadDate = "";
            }

            if (InvoicePostingDate == "") {
                startInvoicePostingDate = "";
                endDateInvoicePostingDate = "";
            }

            param.PoNo = $("#poNo").val();
            param.startInvoiceUploadDate = startInvoiceUploadDate;
            param.endInvoiceUploadDate = endInvoiceUploadDate;
            param.StartDateInvoicePostingDate = startInvoicePostingDate;
            param.EndDateInvoicePostingDate = endDateInvoicePostingDate;
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
                title: '',
                field: '',
                class: 'text-center',
                dataAlign: 'center',
                width: '20',
                formatter: function (data, row, index) {
                    var NextProcessName = row.NextProcessName;
                    var url = LinkUrlRequest(NextProcessName, row.Request_Id);
                    var button1 = `<a href="javascript:void(0);" onclick="DownloadFileUpload(` + row.InvoiceId + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;"><i class="fa fa-file-pdf"></i></a>`;
                    var buttons = [button1].join(" ");
                    return '<div style="width:20px;">' + buttons + '</div>';
                }
            },   
            {
                title: 'Action',
                field: 'Request_Id',
                class: 'text-center',
                dataAlign: 'center',
                width: '80',
                visible: ActionVisible,
                formatter: function (data, row, index) {
                    if (row.PlantCodeKOFAX == '') {
                        var button1 = `<a href="DetailOutstanding?Id=${data}" class="btn btn-default btn-xs" style="background:#fcba12;color:white;"><i class="fa fa-eye"></i></a>`;
                    }                    
                    return [button1];
                }
            },
            {
                title: 'Company',
                field: 'VendorName',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
             
            },
            {
                title: 'PO No',
                field: 'PO_Number',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: function (data, row, index) {
                    var buttons = `<a href="javascript:void(0);" onclick="InitModalUploadPO(` + row.Request_Id + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;">` + data + `</a>`;
                    return buttons;
                }
            },         
            {
                title: 'FileName Invoice',
                field: 'FileNameOri',
                halign: 'center',
                align: 'center',
                width: '110',

            },
            {
                title: 'Invoice Upload Date',
                field: 'InvoiceUploadedDate',
                class: 'text-nowrap',
                align: 'center',
                width: '250'
            },
            {
                title: 'GR/SA List',
                field: 'GR',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: function (data, row, index) {
                    var htm = "<button title='Show GR List' alt='Show GR List' data-toggle='tooltip' data-placement='top' class='btn btn-xs btn-default GrListItem'><i class='fa fa-list'></i></button>";
                    return htm;
                },
                events: operateEventHeader
            },            
            {
                title: 'Hard Copy Invoice',
                field: 'hardcopy',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: function (data, row, index) {
                    var htm = "<button title='Show Hardcopy Invoice List' alt='Show Hardcopy Invoice List' data-toggle='tooltip' data-placement='top' class='btn btn-xs btn-default HardcopyInvoice'><i class='fa fa-list'></i></button>";
                    return htm;
                },
                events: operateEventHeader
            },
        ],
        onLoadSuccess: function () {
            var PoList = [];
            var table = $tableInvoiceIncoming.bootstrapTable('getData');
            if (table.length > 0) {
                jQuery.each(table, function (index, value) {
                    PoList.push(value.Request_Id);
                })
                GetTotalComment(PoList);
                GetTotalUnread(PoList);
            }
        }
    })

    $tableInvoiceInProgress.bootstrapTable({
        url: "GetListInvoiceProgress",
        pagination: true,
        sidePagination: 'server',
        pageSize: 10,
        queryParams: function (p) {
            var param = {};
            var startInvoiceUploadDate = dateFormatter($("#InvoiceUploadDate").data("daterangepicker").startDate._d) ?? "";
            var endInvoiceUploadDate = dateFormatter($("#InvoiceUploadDate").data("daterangepicker").endDate._d) ?? "";
            var startInvoicePostingDate = dateFormatter($("#InvoicePostingDate").data("daterangepicker").startDate._d) ?? "";
            var endDateInvoicePostingDate = dateFormatter($("#InvoicePostingDate").data("daterangepicker").endDate._d) ?? "";

            var InvoiceUploadDate = $("#InvoiceUploadDate").val();
            var InvoicePostingDate = $("#InvoicePostingDate").val();
            if (InvoiceUploadDate == "") {
                startInvoiceUploadDate = "";
                endInvoiceUploadDate = "";
            }

            if (InvoicePostingDate == "") {
                startInvoicePostingDate = "";
                endDateInvoicePostingDate = "";
            }

            param.PoNo = $("#poNo").val();
            param.startInvoiceUploadDate = startInvoiceUploadDate;
            param.endInvoiceUploadDate = endInvoiceUploadDate;
            param.startInvoicePostingDate = startInvoicePostingDate;
            param.endDateInvoicePostingDate = endDateInvoicePostingDate;
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
                title: '',
                field: '',
                class: 'text-center',
                dataAlign: 'center',
                width: '20',
                formatter: function (data, row, index) {
                    var NextProcessName = row.NextProcessName;
                    var url = LinkUrlRequest(NextProcessName, row.Request_Id);
                    var button1 = `<a href="javascript:void(0);" onclick="DownloadFileUpload(` + row.InvoiceId + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;"><i class="fa fa-file-pdf"></i></a>`;
                    var buttons = [button1].join(" ");
                    return '<div style="width:20px;">' + buttons + '</div>';
                }
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
                title: 'Company',
                field: 'VendorName',
                class: 'text-nowrap',
                align: 'left',
                width: '110',

            },
            {
                title: 'PO No',
                field: 'PO_Number',
                class: 'text-center',
                align: 'center',
                width: '110',
                formatter: function (data, row, index) {
                    var buttons = `<a href="javascript:void(0);" onclick="InitModalUploadPO(` + row.Request_Id + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;">` + data + `</a>`;
                    return buttons;
                }
            },         
            {
                title: 'FileName Invoice',
                field: 'FileNameOri',
                halign: 'center',
                align: 'center',
                width: '110',

            },
            {
                title: 'Invoice Upload Date',
                field: 'InvoiceUploadedDate',
                halign: 'center',
                align: 'right',
                width: '110'

            },          
            {
                title: 'GR/SA List',
                field: 'GR',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: function (data, row, index) {
                    var htm = "<button title='Show GR List' alt='Show GR List' data-toggle='tooltip' data-placement='top' class='btn btn-xs btn-default GrListItem'><i class='fa fa-list'></i></button>";
                    return htm;
                },
                events: operateEventHeader
            },
            {
                title: 'Hard Copy Invoice',
                field: 'hardcopy',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: function (data, row, index) {
                    var htm = "<button title='Show Hardcopy Invoice List' alt='Show Hardcopy Invoice List' data-toggle='tooltip' data-placement='top' class='btn btn-xs btn-default HardcopyInvoice'><i class='fa fa-list'></i></button>";
                    return htm;
                },
                events: operateEventHeader
            },
            {
                title: 'Status',
                field: 'StatusInvoice',
                class: 'text-nowrap',
                align: 'center',
                width: '100',
               
            },
            {
                title: 'Message',
                field: 'MessageInvoice',
                class: 'text-center',
                align: 'center',
                width: '110'               
            }     
        ],
        
        onLoadSuccess: function () {
            var PoList = [];
            var table = $tableInvoiceInProgress.bootstrapTable('getData');
            if (table.length > 0) {
                jQuery.each(table, function (index, value) {
                    PoList.push(value.Request_Id);
                })
                GetTotalComment(PoList);
                GetTotalUnread(PoList);
            }
        }
    })

    $tableInvoiceDone.bootstrapTable({
        url: "GetListInvoiceComplete",
        pagination: true,
        sidePagination: 'server',
        pageSize: 10,
        queryParams: function (p) {
            var param = {};
            var startInvoiceUploadDate = dateFormatter($("#InvoiceUploadDate").data("daterangepicker").startDate._d) ?? "";
            var endInvoiceUploadDate = dateFormatter($("#InvoiceUploadDate").data("daterangepicker").endDate._d) ?? "";
            var startInvoicePostingDate = dateFormatter($("#InvoicePostingDate").data("daterangepicker").startDate._d) ?? "";
            var endDateInvoicePostingDate = dateFormatter($("#InvoicePostingDate").data("daterangepicker").endDate._d) ?? "";

            var InvoiceUploadDate = $("#InvoiceUploadDate").val();
            var InvoicePostingDate = $("#InvoicePostingDate").val();
            if (InvoiceUploadDate == "") {
                startInvoiceUploadDate = "";
                endInvoiceUploadDate = "";
            }

            if (InvoicePostingDate == "") {
                startInvoicePostingDate = "";
                endDateInvoicePostingDate = "";
            }

            param.PoNo = $("#poNo").val();
            param.startInvoiceUploadDate = startInvoiceUploadDate;
            param.endInvoiceUploadDate = endInvoiceUploadDate;
            param.startInvoicePostingDate = startInvoicePostingDate;
            param.endDateInvoicePostingDate = endDateInvoicePostingDate;
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
            title: '',
            field: '',
            class: 'text-center',
            dataAlign: 'center',
            width: '20',
            formatter: function (data, row, index) {
                var NextProcessName = row.NextProcessName;
                var url = LinkUrlRequest(NextProcessName, row.Request_Id);
                var button1 = `<a href="javascript:void(0);" onclick="DownloadFileUpload(` + row.InvoiceId + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;"><i class="fa fa-file-pdf"></i></a>`;
                var buttons = [button1].join(" ");
                return '<div style="width:20px;">' + buttons + '</div>';
            }
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
            title: 'Company',
            field: 'VendorName',
            class: 'text-nowrap',
            align: 'center',
            width: '110',
        },
        {
            title: 'PO No',
            field: 'PO_Number',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: function (data, row, index) {
                var buttons = `<a href="javascript:void(0);" onclick="InitModalUploadPO(` + row.Request_Id + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;">` + data + `</a>`;
                return buttons;
            }
        },      
        {
            title: 'FileName Invoice',
            field: 'FileNameOri',
            halign: 'center',
            align: 'center',
            width: '110',

            },
        {
            title: 'InvoiceNo',
            field: 'InvoiceNo',
            align: 'center',
            align: 'right',
            width: '110'

        },
        {
            title: 'Invoice Upload Date',
            field: 'InvoiceUploadedDate',
            align: 'center',
            align: 'right',
            width: '110'
           
        },       
        {
            title: 'GR/SA List',
            field: 'GR',
            class: 'text-center text-nowrap',
            align: 'center',
            width: '110',
            formatter: function (data, row, index) {
                var htm = "<button title='Show GR List' alt='Show GR List' data-toggle='tooltip' data-placement='top' class='btn btn-xs btn-default GrListItem'><i class='fa fa-list'></i></button>";
                return htm;
            },
            events: operateEventHeader
        },
        {
            title: 'Hard Copy Invoice',
            field: 'hardcopy',
            class: 'text-nowrap',
            align: 'center',
            width: '110',
            formatter: function (data, row, index) {
                var htm = "<button title='Show Hardcopy Invoice List' alt='Show Hardcopy Invoice List' data-toggle='tooltip' data-placement='top' class='btn btn-xs btn-default HardcopyInvoice'><i class='fa fa-list'></i></button>";
                return htm;
            },
            events: operateEventHeader
        },
        {
            title: 'SAP Doc No',
            field: 'SAPDocNo',
            class: 'text-center',
            align: 'center',
            width: '110',
            
        },
        {
            title: 'Status',
            field: 'StatusInvoice',
            class: 'text-center',
            align: 'center',
            width: '110',         

        },    
        {
            title: 'Message',
            field: 'MessageInvoice',
            class: 'text-center',
            align: 'center',
            width: '100px'            
        },
        ],
        onLoadSuccess: function () {
            var PoList = [];
            var table = $tableInvoiceDone.bootstrapTable('getData');
            if (table.length > 0) {
                jQuery.each(table, function (index, value) {
                    PoList.push(value.Request_Id);
                })
                GetTotalComment(PoList);
                GetTotalUnread(PoList);
            }
        }
    })

    $tableInvoiceReject.bootstrapTable({
        url: "GetListInvoiceReject",
        pagination: true,
        sidePagination: 'server',
        pageSize: 10,
        queryParams: function (p) {
            var param = {};
            var startInvoiceUploadDate = dateFormatter($("#InvoiceUploadDate").data("daterangepicker").startDate._d) ?? "";
            var endInvoiceUploadDate = dateFormatter($("#InvoiceUploadDate").data("daterangepicker").endDate._d) ?? "";
            var startInvoicePostingDate = dateFormatter($("#InvoicePostingDate").data("daterangepicker").startDate._d) ?? "";
            var endDateInvoicePostingDate = dateFormatter($("#InvoicePostingDate").data("daterangepicker").endDate._d) ?? "";

            var InvoiceUploadDate = $("#InvoiceUploadDate").val();
            var InvoicePostingDate = $("#InvoicePostingDate").val();
            if (InvoiceUploadDate == "") {
                startInvoiceUploadDate = "";
                endInvoiceUploadDate = "";
            }

            if (InvoicePostingDate == "") {
                startInvoicePostingDate = "";
                endDateInvoicePostingDate = "";
            }

            param.PoNo = $("#poNo").val();
            param.startInvoiceUploadDate = startInvoiceUploadDate;
            param.endInvoiceUploadDate = endInvoiceUploadDate;
            param.startInvoicePostingDate = startInvoicePostingDate;
            param.endDateInvoicePostingDate = endDateInvoicePostingDate;
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
                title: '',
                field: '',
                class: 'text-center',
                dataAlign: 'center',
                width: '20',
                formatter: function (data, row, index) {
                    var NextProcessName = row.NextProcessName;
                    var url = LinkUrlRequest(NextProcessName, row.Request_Id);
                    var button1 = `<a href="javascript:void(0);" onclick="DownloadFileUpload(` + row.InvoiceId + `)" class="btn btn-light notification btn-comment btn-xs" style="color:black;"><i class="fa fa-file-pdf"></i></a>`;
                    var buttons = [button1].join(" ");
                    return '<div style="width:20px;">' + buttons + '</div>';
                }
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
                title: 'Company',
                field: 'VendorName',
                class: 'text-nowrap',
                align: 'center',
                width: '110',               
            },
            {
                title: 'PO No',
                field: 'PO_Number',
                class: 'text-center',
                align: 'center',
                width: '110'
            },          
            {
                title: 'File Name',
                field: 'FileNameOri',
                halign: 'center',
                align: 'center',
                width: '110',

            },
            {
                title: 'Invoice Upload Date',
                field: 'InvoiceUploadedDate',
                align: 'center',
                align: 'right',
                width: '110'

            },          
            {
                title: 'GR/SA List',
                field: 'GR',
                align: 'center',
                align: 'right',
                width: '110',
                formatter: function (data, row, index) {
                    var htm = "<button title='Show GR List' alt='Show GR List' data-toggle='tooltip' data-placement='top' class='btn btn-xs btn-default GrListItem'><i class='fa fa-list'></i></button>";
                    return htm;
                },
                events: operateEventHeader
            },
            {
                title: 'Hard Copy Invoice',
                field: 'hardcopy',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: function (data, row, index) {
                    var htm = "<button title='Show Hardcopy Invoice List' alt='Show Hardcopy Invoice List' data-toggle='tooltip' data-placement='top' class='btn btn-xs btn-default HardcopyInvoice'><i class='fa fa-list'></i></button>";
                    return htm;
                },
                events: operateEventHeader
            },
            {
                title: 'Status',
                field: 'StatusInvoice',
                class: 'text-center',
                align: 'center',
                width: '110'
            },
            {
                title: 'Message',
                field: 'MessageInvoice',
                class: 'text-center',
                align: 'center',
                width: '110'
            }

        ],
        onLoadSuccess: function () {
            var PoList = [];
            var table = $tableInvoiceReject.bootstrapTable('getData');
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

var ActionVisible = false;

if (Role.includes('POSTFINANCEBRANCH') || Role.includes('POSTPLANTVIEWERFINANCE')) {
    ActionVisible = true;
}
function DownloadFileUpload(id) {
    url = "/POST/DownloadFileRequest?id=" + id;
    window.open(url, '_blank');
}

window.operateEventHeader = {
    'click .GrListItem': function (e, value, row, index) {
        if (row.TypeTab == 'Incoming') {
            var data = $("#table-InvoiceIncoming").bootstrapTable("getData");
        }
        else if (row.TypeTab == 'Progress') {
            var data = $("#table-InvoiceInprogress").bootstrapTable("getData");
        }
        else if (row.TypeTab == 'Complete') {
            var data = $("#table-InvoiceDone").bootstrapTable("getData");
        }
        else if (row.TypeTab == 'Reject') {
            var data = $("#table-InvoiceReject").bootstrapTable("getData");
        }
       
        var PoNo = row.PO_Number;
        $("#PoNo").val(PoNo);    
        InitTableGrGet(PoNo);
        $("#table-gr-list").bootstrapTable('refresh');
        $("#modalGrList").modal("show");
    },
    'click .HardcopyInvoice': function (e, value, row, index) {
        if (row.TypeTab == 'Incoming') {
            var data = $("#table-InvoiceIncoming").bootstrapTable("getData");
        }
        else if (row.TypeTab == 'Progress') {
            var data = $("#table-InvoiceInprogress").bootstrapTable("getData");
        }
        else if (row.TypeTab == 'Complete') {
            var data = $("#table-InvoiceDone").bootstrapTable("getData");
        }
        else if (row.TypeTab == 'Reject') {
            var data = $("#table-InvoiceReject").bootstrapTable("getData");
        }

        var PoNo = row.PO_Number;
        $("#PoNo").val(PoNo);
      
        InitTablehardcopy(row.InvoiceId);
          
        $("#modalhardCopyInvoiceForm").modal("show");
        $("#submithardcopyinvoice").hide();
        $("#btnSave").hide();
 
    },
}
function InitTablehardcopy(InvoiceId) {
    $("#table-hardcopyinvoice").bootstrapTable('destroy');
    $("#table-hardcopyinvoice").bootstrapTable({
        cache: false,
        async: false,
        detailView: false,
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
        url: 'GetHardCopyInvoiceByInvoiceId',
        queryParams: function (params) {
            var query = {
                Id: InvoiceId                
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        columns: columnsHC
    });
}
function InitModalHardCopy(InvoiceId) {
    $('#modalhardCopyInvoiceForm').modal('show');
    $('#modalhardCopyInvoiceForm').modal({ backdrop: "static" });  

    $("#table-hardcopyinvoice").bootstrapTable('refresh', {
        query: { id: InvoiceId }
    });
}

function InitTableGrGet(PoNo) {
    $("#table-gr-list").bootstrapTable('destroy');
    $("#table-gr-list").bootstrapTable({
        cache: false,
        async: false,
        detailView: false,
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
        url: 'GetDataGRByPO',
        queryParams: function (params) {
            var query = {
                poNo: PoNo,          
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        columns: columnsGr
    });
}
var columnsGr = [{
    title: 'No',
    align: 'center',
    halign: 'center',
    class: 'text-nowrap',
    width: '10',
    formatter: runningFormatterNoPaging
}, {
    title: 'GR Number',
    field: 'GRNo',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '100'
},
{
    title: 'GR Date',
    field: 'GRDate',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter

},
{
    title: 'GR Posting Date',
    field: 'GRPostingDate',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter
},
{
    title: 'Amount',
    field: 'GRValue',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: currencyFormatter
    }]
var columnsHC = [{
    title: 'InvoiceId',
    field: 'InvoiceId',
    align: 'center',
    width: '120',
    visible: false
},
    {
        title: 'PO_Number',
        field: 'PO_Number',
        align: 'center',
        width: '120'
    },
    {
        title: 'FileName',
        field: 'FileNameOri',
        align: 'center',
        width: '120'
    },
    {
        title: 'Recipients Name/ Receipt Number',
        field: 'ReceiptNameOrNumber',
        align: 'center',
        width: '120'
    },
    {
        title: 'Submission Type',
        field: 'SubmissionType',
        align: 'center',
        width: '120'
    },
    {
        title: 'Submission Date',
        field: 'SubmissionDate',
        align: 'center',
        width: '120',
        formatter: dateSAPFormatter
    }]
function InitTableGr() {
    $("#table-gr-list").bootstrapTable({
        detailView: false,
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
        ajax: GetListGrByRequestId,
        columns: columnsGr
    });
}
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

function clearSearchFilter() {
    $("#poNo").val("");
    $("#InvoiceUploadDate").val("");
    $("#InvoicePostingDate").val("");
    if ($("#tabInvoiceIncoming").hasClass("active")) {    
        $("#table-InvoiceIncoming").bootstrapTable("refresh");
    }

    if ($("#tabInvoiceInprogress").hasClass("active")) {
        $("#table-InvoiceInprogress").bootstrapTable("refresh");
    }

    if ($("#tabInvoiceDone").hasClass("active")) {
        $("#table-InvoiceDone").bootstrapTable("refresh");
    }
    if ($("#tabInvoiceReject").hasClass("active")) {
        $("#table-InvoiceReject").bootstrapTable("refresh");
    }

}
function clearSearchHeader() {

}
function searchInvoice() {
    var InvoiceUploadDate = $("#InvoiceUploadDate").val();
    var InvoicePostingDate = $("#InvoicePostingDate").val();
    if (InvoiceUploadDate == "") {
        startInvoiceUploadDate = null;
        endInvoiceUploadDate = null;
    }

    if (InvoicePostingDate == "") {
        startInvoicePostingDate = null;
        endDateInvoicePostingDate = null;
    }
    if ($("#tabInvoiceIncoming").hasClass("active")) {

        $("#table-InvoiceIncoming").bootstrapTable("refresh");
    }

    if ($("#tabInvoiceInprogress").hasClass("active")) {
        $("#table-InvoiceInprogress").bootstrapTable("refresh");
    }

    if ($("#tabInvoiceDone").hasClass("active")) {
        $("#table-InvoiceDone").bootstrapTable("refresh");
    }

    if ($("#tabInvoiceReject").hasClass("active")) {
        $("#table-InvoiceReject").bootstrapTable("refresh");
    }

}


function InitModalUploadPO(reqId) {
    var baseUrl = location.origin;
    document.getElementById("FormUploadPO").style.display = "none";
    $('#modalUploadPO').modal();
    tableUploadPo.bootstrapTable('refresh', {
        url: baseUrl + "/Post/GetListAttachment?id=" + reqId
    });

}
function showFileInvoice() {
    $("#modalViewInvoice").modal("show");
}

function showFileBast() {
    $("#modalViewBast").modal("show");
}

function initDateRange() {
    $('#InvoiceUploadDate').daterangepicker({
        opens: 'right',
        locale: {
            format: 'DD.MMM.YYYY'
        }
    }, function (start, end) {
        var start_date = moment(start._d).format("DD.MM.YYYY");
        var end_date = moment(end._d).format("DD.MM.YYYY");

        $('#InvoiceUploadDate').val(start_date + '-' + end_date);
    });

    $('#InvoicePostingDate').daterangepicker({
        opens: 'right',
        locale: {
            format: 'DD.MMM.YYYY'
        },
    }, function (start, end) {
        var start_date = moment(start._d).format("DD.MM.YYYY");
        var end_date = moment(end._d).format("DD.MM.YYYY");

        $('#InvoicePostingDate').val(start_date + '-' + end_date);
    });

}


function loadingTemplate(message) {
    return '<i class="fa fa-spinner fa-spin fa-fw fa-2x"></i>'
}

