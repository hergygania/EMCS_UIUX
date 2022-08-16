
$(function () {
    //$(".datepickr").datepicker({
    //    format: "yyyy",
    //    viewMode: "years",
    //    minViewMode: "years",
    //    autoClose: true
    //});
    $table = $("#tbl-tax-audit");

    var columns = [
        [{
            field: "no",
            title: "No",
            align: 'center',
            rowspan: 2,
            valign: "middle",
            
        }, {
            field: "Name",
            title: "Buyers of Export Goods",
            sortable: true,
            align: "center",
            colspan: 2
        }, {
            field: "",
            title: "PEB",
            sortable: true,
            align: "center",
            colspan: 8
        }, {
            field: "",
            title: "PPJK",
            sortable: true,
            align: "center",
            colspan: 3
        }, {
            field: "",
            title: "Shipping from site",
            sortable: true,
            align: "center",
            colspan: 2
        }, {
            field: "",
            title: "Receipt at The Pile",
            sortable: true,
            align: "center",
            colspan: 2
        }, {
            field: "",
            title: "Loading of Goods",
            sortable: true,
            align: "center",
            colspan: 3
        }, {
            field: "",
            title: "Invoice",
            sortable: true,
            align: "center",
            colspan: 2
        }, {
            field: "",
            title: "Bill of Lading",
            sortable: true,
            align: "center",
            colspan: 3
        }, {
            field: "LoadingPort",
            title: "Port of Loading ",
            sortable: true,
            align: "center",
            class: "text-nowrap",
            rowspan: 2,
            valign: "middle"
        }, {
            field: "",
            title: "Proof of Transfer Payment",
            sortable: true,
            align: "center",
            colspan: 3
        }, {
            field: "Remarks",
            title: "Information",
            sortable: true,
            align: "left",
            rowspan: 2,
            class: "text-nowrap",
            valign: "middle",
            halign: "center"
        }],
        [{
            field: "Name",
            title: "Buyer's Name",
            sortable: true,
            align: "left",
            class: "text-nowrap",
            valign: "middle",
            halign: "center"
        },
        {
            field: "Address",
            title: "ADDRESS",
            sortable: true,
            align: "left",
            class: "text-nowrap",
            valign: "middle",
            halign: "center"
        }, {
            field: "PebNo",
            title: "PEB NUMBER",
            sortable: true,
            align: "center",
            class: "text-nowrap",
            valign: "middle"
        }, {
            field: "PebDate",
            title: "PEB DATE",
            sortable: true,
            class: "text-nowrap",
            align: "center",
            valign: "middle"
        }, {
            field: "CurrInvoice",
            title: "CURRENCY INVOICE",
            sortable: true,
            align: "center",
            valign: "middle"
        }, {
            field: "CurrValue",
            title: "INVOICE VALUE",
            sortable: true,
            align: "center",
            valign: "middle"
        }, {
            field: "Rate",
            title: "TAX RATE",
            sortable: true,
            align: "right",
            valign: "middle"
        }, {
            field: "DppExport",
            title: "DPP Export",
            sortable: true,
            align: "right",
            valign: "middle",
            halign: "center"
        }, {
            field: "FilePeb",
            title: "FILE PEB",
            align: 'center',
            valign: 'center',
            halign: "center",
            class: 'text-nowrap',
            sortable: true,
            events: operateEventPeb,
            formatter: function (data, row) {
                if (row.FilePeb !== "") {
                    var btnDownload = "<button class='btn btn-xs btn-link btn-success download' type='button'><i class='tim-icons icon-cloud-download-93'></i></button>";
                    var btnPreview = "<button class='btn btn-xs btn-link btn-info btn-outline showDocument' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='tim-icons icon-paper'></i></button>";
                    return [btnDownload, btnPreview].join(' ');
                } else {
                    return "-";
                }
            },
            class: 'text-nowrap'
        }, {
            field: "FileBlAwb",
            title: "FILE BLAWB",
            align: 'center',
            valign: 'center',
            halign: "center",
            class: 'text-nowrap',
            sortable: true,
            events: operateEventBlAwb,
            formatter: function (data, row) {
                if (row.FileBlAwb !== "") {
                    var btnDownload = "<button class='btn btn-xs btn-link btn-success download' type='button'><i class='tim-icons icon-cloud-download-93'></i></button>";
                    var btnPreview = "<button class='btn btn-xs btn-link btn-info btn-outline showDocument' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='tim-icons icon-paper'></i></button>";
                    return [btnDownload, btnPreview].join(' ');
                } else {
                    return "-";
                }
            },
            class: 'text-nowrap'
        }, {
            field: "PpjkName",
            title: "PPJK NAME",
            sortable: true,
            align: "left",
            class: "text-nowrap",
            valign: "middle",
            halign: "center"
        }, {
            field: "PPJKAddress",
            title: "PPJK ADDRESS",
            sortable: true,
            align: "left",
            class: "text-nowrap",
            valign: "middle",
            halign: "center"
        }, {
            field: "DANo",
            title: "DA",
            sortable: true,
            align: "center",
            valign: "middle"
        }, {
            field: "DoNo",
            title: "DO NUMBER",
            sortable: true,
            align: "center",
            valign: "middle"
        }, {
            field: "DoDate",
            title: "DATE",
            sortable: true,
            class: "text-nowrap",
            align: "center",
            valign: "middle"
           
        }, {
            field: "WarehouseLoc",
            title: "Pile Place",
            sortable: true,
            align: "center",
            class: "text-nowrap"
        }, {
            field: "NpeNo",
            title: "Receipt",
            sortable: true,
            align: "center",
            valign: "middle"
        }, {
            field: "LoadingPort",
            title: "Loading Port",
            sortable: true,
            align: "center",
            valign: "middle",
            class: "text-nowrap"
        }, {
            field: "NpeNo",
            title: "Export Approval",
            sortable: true,
            align: "center",
            valign: "middle"
        }, {
            field: "NpeDate",
            title: "Date",
            sortable: true,
            class: "text-nowrap",
            align: "center",
            valign: "middle"
        }, {
            field: "InvoiceNo",
            title: "Invoice Number",
            sortable: true,
            align: "left",
            class: "text-nowrap",
            valign: "middle",
            halign: "center"
        }, {
            field: "InvoiceDate",
            title: "Date",
            sortable: true,
            class: "text-nowrap",
            align: "center",
            valign: "middle"
        }, {
            field: "Publisher",
            title: "Publisher",
            sortable: true,
            align: "center",
            class: "text-nowrap",
            valign: "middle"
        }, {
            field: "BlAwbNo",
            title: "Number",
            sortable: true,
            align: "center",
            class: "text-nowrap",
            valign: "middle"
        }, {
            field: "BlAwbDate",
            title: "Date",
            sortable: true,
            class: "text-nowrap",
            align: "center",
            valign: "middle"
        }, {
            field: "pelabuhan_bongkar",
            title: "Bank",
            sortable: true,
            align: "center",
            valign: "middle"
        }, {
            field: "no_voucher",
            title: "Voucher Number",
            sortable: true,
            align: "center",
            valign: "middle"
        }, {
            field: "tanggal_transfer",
            title: "Date",
            sortable: true,
            class: "text-nowrap",
            align: "center",
            valign: "middle"
        }]

    ]

    $table.bootstrapTable({
        //url: "/EMCS/GetTaxAuditList",
        columns: columns,
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
    });

    var StartDate = $("#inp-start-date").val();
    var EndDate = $("#inp-end-date").val();

    reportTaxAudit(StartDate, EndDate);

});

window.operateEventPeb = {
    'click .download': function (e, value, row) {
        location.href = "/EMCS/DownloadPebDocument?filename=" + row.FilePeb;
        //value.attr("href", "/Upload/EMCS/BLAWB/" + row.FilePeb);
    },
    'click .showDocument': function (e, value, row) {
        document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/NPEPEB/" + row.FilePeb;
    }
};

window.operateEventBlAwb = {
    'click .download': function (e, value, row) {
        location.href = "/EMCS/DownloadBlAwbDocument?filename=" + row.FileBlAwb;
        //value.attr("href", "/Upload/EMCS/BLAWB/" + row.FileBlAwb);
    },
    'click .showDocument': function (e, value, row) {
        document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/BLAWB/" + row.FileBlAwb;
    }
};

function reportTaxAudit(StartDate, EndDate) {

    window.pis.table({
        objTable: $table,
        urlSearch: '/EMCS/RTaxAuditListPage?StartDate=' + StartDate + '&EndDate=' + EndDate,
        urlPaging: '/EMCS/RTaxAuditPageXt?StartDate=' + StartDate + '&EndDate=' + EndDate,
        autoLoad: true
    });
}

function searchDataReport() {
    StartDate = $('#inp-start-date').val() === null || $('#inp-start-date').val() === '' ? '' : moment($('#inp-start-date').val()).format('YYYY-MM-DD');
    EndDate = $('#inp-end-date').val() === null || $('#inp-end-date').val() === '' ? '' : moment($('#inp-end-date').val()).format('YYYY-MM-DD');

    reportTaxAudit(StartDate, EndDate);
}

function exportDataReport() {
    var startDate = $('#inp-start-date').val() === null || $('#inp-start-date').val() === '' ? '' : moment($('#inp-start-date').val()).format('YYYY-MM-DD');
    var endDate = $('#inp-end-date').val() === null || $('#inp-end-date').val() === '' ? '' : moment($('#inp-end-date').val()).format('YYYY-MM-DD');
    window.open('/EMCS/DownloadTaxAudit?StartDate=' + startDate + '&EndDate=' + endDate, '_blank');
}

