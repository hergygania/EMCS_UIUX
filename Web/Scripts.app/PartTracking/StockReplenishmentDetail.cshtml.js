window.operateEventsDet = {
    'click .detail': function (e, value, row, index) {
        bindPopup(row);
    },
    'click .leadtime': function (e, value, row, index) {
        bindPopupMil(row);
    },
    'click .imex': function (e, value, row, index) {
        $('#imex_sxNo').val(row.invoice_no);
        $('#imex_partNo').val(row.part_no);
        $('#imex_caseNo').val('');
        $("form#submitImex").submit();
    },
    'click .imexPartNo': function (e, value, row, index) {
        $('#imex_sxNo').val('');
        $('#imex_partNo').val(row.part_no);
        $('#imex_caseNo').val('');
        $("form#submitImex").submit();
    },
    'click .imexInvoice': function (e, value, row, index) {
        $('#imex_sxNo').val(row.invoice_no);
        $('#imex_partNo').val('');
        $('#imex_caseNo').val('');
        $("form#submitImex").submit();
    },
    'click .imexCaseNo': function (e, value, row, index) {
        $('#imex_sxNo').val('');
        $('#imex_partNo').val('');
        $('#imex_caseNo').val(row.case_number);
        $("form#submitImex").submit();
    }
};

$(function () {
    var $tableDet = $('#tableDet');
    enableLink(false);

    $('#modal').modal({
        backdrop: true,
        keyboard: false,
        show: false
    });

    $('#modalmil').modal({
        backdrop: true,
        keyboard: false,
        show: false
    });

    $tableDet.bootstrapTable({
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        pageSize: '5',
        exportTypes: ['excel'],
        exportOptions: {
            ignoreColumn: [0],
            fileName: 'file.xls'
        },
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [
            { field: 'action', title: 'Action', width: '255px', align: 'center', formatter: operateFormatterDet, events: operateEventsDet, switchable: false },
            { field: 'sos', title: 'Material Type', width: '150px', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'part_no', title: 'Material No', width: '130px', halign: 'center', align: 'left', formatter: operateFormatterPartNo, events: operateEventsDet, sortable: true, switchable: false },
            { field: 'part_desc', title: 'Material Desc', width: '200px', halign: 'center', align: 'left', sortable: true, switchable: false },
    		{ field: 'progress', title: 'Progress', width: '100px', halign: 'center', align: 'left', formatter: progressFormat, switchable: false },
		    //{ field: 'cols', title: 'progress_leg', halign: 'center', align: 'left', formatter: progressFormat2, visible: false },
		    { field: 'doc_status', title: 'Doc Status', width: '200px', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'doc_line', title: 'Doc Line', width: '200px', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'invoice_no', title: 'Invoice No', width: '110px', halign: 'center', align: 'left', formatter: operateFormatterInvNo, events: operateEventsDet, sortable: true, switchable: false },
            { field: 'case_number', title: 'Case', width: '110px', halign: 'center', align: 'left', formatter: operateFormatterCaseNo, events: operateEventsDet, sortable: true, switchable: false },
    		{ field: 'qty', title: 'Qty', width: '60px', halign: 'right', align: 'right', sortable: true, switchable: false, footerFormatter: function totQty(data) { return '<span class="totalQty"></span>'; } },
    		{ field: 'weight', title: 'Weight', width: '80px', halign: 'right', align: 'right', sortable: true, switchable: false, footerFormatter: function totWt(data) { return '<span class="totWeight"></span>'; } },
		    { field: 'eta', title: 'ETA', titleTooltip: 'Estimated Time Arrival', width: '110px', halign: 'center', align: 'left', formatter: dateFormatter, sortable: true, switchable: false, cellStyle: function cellStyle(value, row, index) { return { classes: 'text-nowrap' }; } },
            { field: 'ata', title: 'ATA', titleTooltip: 'Actual Time Arrival', width: '110px', halign: 'center', align: 'left', formatter: dateFormatter, sortable: true, switchable: false, cellStyle: function cellStyle(value, row, index) { return { classes: 'text-nowrap' }; } },
    		{ field: 'om', title: 'OM', width: '65px', halign: 'center', align: 'left', sortable: true },
		    { field: 'source', title: 'Source', width: '180px', halign: 'center', align: 'left', sortable: true },
		    { field: 'asn_number', title: 'ASN', width: '110px', halign: 'center', align: 'center', sortable: true },
		    { field: 'ack_status', title: 'ACK', width: '110px', halign: 'center', align: 'left', sortable: true },
		    { field: 'comm_code', title: 'Commodity', width: '200px', halign: 'center', align: 'left', sortable: true },
		    { field: 'dn', title: 'DN ($)', width: '80px', halign: 'right', align: 'right', sortable: true, switchable: false },
            { field: 'ext_dn', title: 'Ext DN ($)', width: '80px', halign: 'right', align: 'right', sortable: true, switchable: false }
		    //{ field: 'trilc', title: 'trilc', sortable: true, visible: false },
		    //{ field: 'Etl_Date', title: 'Etl_Date', formatter: dateFormatter, sortable: true, visible: false },
		    //{ field: 'JCode', title: 'JCode', sortable: true, visible: false },
		    //{ field: 'order_class', title: 'order_class', sortable: true, visible: false }
        ]
    });

    if (myApp.isAdmin == 'false') {
        $tableDet.bootstrapTable('hideColumn', 'dn');
        $tableDet.bootstrapTable('hideColumn', 'ext_dn');
    }

    window.pis.table({
        objTable: $tableDet,
        urlSearch: '/parttracking/StockReplenishmentDetailPage',
        urlPaging: '/parttracking/StockReplenishmentDetailPageXt',
        searchParams: {
            RPORNE: $('#RPORNE').val(),
            ORDSOS: $('#ORDSOS').val(),
            store_no: $('#store_no').val(),
            order_number: $('#order_number_det').val(),
            supply_docinv: $('#supply_docinv_det').val(),
            doc_date: $('#doc_date_det').val(),
            supply_docinv_date: $('#supply_docinv_date_det').val(),
            receiveDate: $('#receiveDate_det').val(),
            tracking_id: $('#tracking_id_det').val(),
            podDate: $('#podDate_det').val(),
            pupDate: $('#pupDate_det').val()
        },
        autoLoad: true,
        dataHeight: 499,
        afterComplete: function (v) {
            getSumaryDet();
        }
    });

    var isgetSumaryDet = 0;

    getSumaryDet = function () {
        if (isgetSumaryDet == 1) {
            $('.totalQty').html($('#det_qty').val());
            $('.totWeight').html($('#det_weight').val());
            $(".fixed-table-body").scrollLeft(0);
            return;
        }
        isgetSumaryDet = 1;

        $.getJSON("/partTracking/GetStockReplenishmentSum", function (v) {
            var qty = v.qty, wt = v.weight, rows = v.rows;

            $('#det_qty').val('Qty:<br>' + qty);
            $('#det_weight').val('Weight:<br>' + wt);

            $('.totalQty').html($('#det_qty').val());
            $('.totWeight').html($('#det_weight').val());

            if (rows == 1)
                $('.fixed-table-footer').hide();
        });
    };


    $(".downloadExcel2").click(function () {
        $(".table2excel2").table2excel({
            name: "Stock Replenishment Detail",
            filename: "Stock Replenishment Detail.xls"
        });
    });
});

function currencyFormatter(dt) {
    if (dt != null) {
        var formattedCurr = '$ ' + dt;
        return formattedCurr;
    }
};

function dateFormatter(dt) {
    if (dt != null) {
        var formattedDate = moment(dt).format('DD MMM YYYY');
        return formattedDate;
    }
};

function operateFormatterDet(value, row, index) {
    var btnDet = '', btnMil = '', btnTrn = '', width = '191';
    if (row.sosGroupType == 'N/A') {
        btnDet = '';//'<button type="button" class="btn btn-xs btn-primary leaddis" style="cursor: not-allowed;filter: alpha(opacity=65); -webkit-box-shadow: none; box-shadow: none;opacity: .65;" title="N/A"><i class="fa fa-search-plus"></i>Detail</button>';
        btnMil = '-';//'<button type="button" class="btn btn-xs btn-primary leaddis" style="cursor: not-allowed;filter: alpha(opacity=65); -webkit-box-shadow: none; box-shadow: none;opacity: .65;" title="N/A"><i class="fa fa-clock-o"></i> Milestone</button>';
        btnTrn = '';//'<button type="button" class="btn btn-xs btn-primary leaddis" style="cursor: not-allowed;filter: alpha(opacity=65); -webkit-box-shadow: none; box-shadow: none;opacity: .65;" title="N/A"><i class="fa fa-check-circle"></i> IMEX</button>';
        width = '44';
    }
    else {
        btnDet = '<button type="button" class="btn btn-info detail" title="Detail" href="#modal"><i class="fa fa-search-plus"></i> Detail</button>';
        btnMil = '<button type="button" class="btn btn-primary leadtime" title="Leadtime" hreft="#modalmil"><i class="fa fa-clock-o"></i> Milestone</button>';
        btnTrn = '<button type="button" class="btn btn-danger imex" title="Imex"><i class="fa fa-check-circle"></i> IMEX</button>';
    }

    if (row.RCDCD == 'T') {
        btnTrn = '';
        width = '155';
    }

    return [
	    '<div class="btn-group">',
		    btnDet,
		    btnMil,
		    btnTrn,
	    '</div>'
    ].join('');
}


function operateFormatterPartNo(value, row, index) {
    var s = row.RCDCD == 'T' || row.sosGroupType == 'N/A' ? row.part_no : '<a class="imexPartNo" style="cursor:pointer;text-decoration:underline">' + row.part_no + '</a>';
    return s;
};

function operateFormatterInvNo(value, row, index) {
    var s = row.RCDCD == 'T' || row.sosGroupType == 'N/A' || row.invoice_no == '-' ? row.invoice_no : '<a class="imexInvoice" style="cursor:pointer;text-decoration:underline">' + row.invoice_no + '</a>';
    return s;
};

function operateFormatterCaseNo(value, row, index) {
    var s = row.RCDCD == 'T' || row.sosGroupType == 'N/A' || row.case_number == '000000000' || row.case_number == '-' ? row.case_number : '<a class="imexCaseNo" style="cursor:pointer;text-decoration:underline">' + row.case_number + '</a>';
    return s;
};

function progressFormat(value, row, index) {
    var dt = row.progress;
    var sAct = parseInt(row.sum_actual);
    var sBm = parseInt(row.sum_bm);
    var seq = parseInt(row.Sequence);
    var percentage = row.percentage;
    if (sBm == 0 && sAct == 0) {
        sBm = 1; sAct = 1;
    }
    var perc = (sBm / sAct) * 100;
    var col = '#0DBB0D';
    if (perc >= 100) col = '#0DBB0D';
    else if (perc >= 80 && perc < 100) col = 'yellow';
    else col = 'red';

    if (row.RCDCD == 'T' && seq == 4) seq = 5;
    if (row.sosGroupType == 'N/A') {
        percentage = 100; col = '#ffffff';
        return "";
    }

    var progFormat = '<div class="progress progress-xs">';
    if (seq < 2) {
        percentage = 20;
        progFormat += '<div class="progress-bar" style="background-color:' + col + ';width:' + percentage + '%"></div>';
    }
    else if (seq <= 3) {
        percentage = 50;
        progFormat += '<div class="progress-bar" style="background-color:' + col + ';width:' + percentage + '%"></div>';
    }
    else if (seq <= 4) {
        percentage = 80;
        progFormat += '<div class="progress-bar" style="background-color:' + col + ';width:' + percentage + '%"></div>';
    }
    else {
        percentage = 100;
        progFormat += '<div class="progress-bar" style="background-color:' + col + ';width:' + percentage + '%"></div>';
    }
    progFormat += '</div>';
    return progFormat;
}

function progressFormat2(value, row, index) {
    var dt = row.progress;
    var sAct = row.sum_actual;
    var sBm = row.sum_bm;
    var seq = row.Sequence;
    var dly = row.delay;
    var percentage = row.percentage;
    var progFormat = '<div class="progress progress-xs">';
    if (dt < 80)
        progFormat += '<div class="progress-bar" style="background-color:red;width:' + percentage + '%"></div>';
    else if (dt < 100)
        progFormat += '<div class="progress-bar" style="background-color:yellow;width:' + percentage + '%"></div>';
    else if (dt >= 100)
        progFormat += '<div class="progress-bar" style="background-color:#0DBB0D;width:' + percentage + '%"></div>';

    progFormat += '</div>';
    return '<div style="white-space:nowrap">B:' + sBm + ' A:' + sAct + ' D:' + dly + ' T:' + seq + '</di>'; //progFormat;
}

function bindPopupMil(row) {
    $('#modalmil').modal({ show: true });
    $('#divMilestone').empty();
    $('#divMilestone').html("<div style=\"text-align:center;color:red\"><img src='/Content/images/ajax-loading.gif' style=\"padding-right:3px\">...Loading...</div>");
    enableLink(false);

    $('#m_part_no').val(row.part_no);
    $('#m_part_desc').val(row.part_desc);
    $('#m_invoice_no').val(row.invoice_no);
    if (row.eta != null && row.eta != '')
        $('#m_eta').val(moment(row.eta).format('DD MMM YYYY'));
    $('#m_source').val(row.source);
    $('#m_doc_number').val(row.doc_number);

    var data = new Object();
    data.type = row.doc_type;
    data.sos = row.sos;
    data.order_class = 'S';
    data.trilc = row.trilc;
    if (row.doc_date != null)
        data.doc_date = moment(row.doc_date, 'YYYY-MM-DD');
    data.store_no = $('#store_no').val();
    if (row.invoice_date != null)
        data.invoice_date = moment(row.invoice_date, 'YYYY-MM-DD');
    data.case_number = row.case_number
    data.RPORNE = $('#RPORNE').val();
    data.part_no = row.part_no;
    data.doc_number = row.doc_number;
    if (row.pupDate != null)
        data.pupDate = moment(row.pupDate, 'YYYY-MM-DD');
    if (row.podDate != null)
        data.podDate = moment(row.podDate, 'YYYY-MM-DD');
    if (row.receiveDate != null)
        data.receiveDate = moment(row.receiveDate, 'YYYY-MM-DD');

    var isRunning = 0;
    $("#modalmil").on('shown.bs.modal', function (e) {
        if (isRunning == 1) return;
        isRunning = 1;
        showMainScrollbar(false);

        $.getJSON("/partTracking/GetMilestoneList", {
            params: JSON.stringify(data)
        },
		function (results) {
		    setData(results);
		    enableLink(true);
		    isRunning = 1;
		});
    });

    $('#modalmil').on('hidden.bs.modal', function (e) {
        showMainScrollbar(true);
    });
}

function bindPopup(row) {
    enableLink(false);
    $('#d_part_no').val(row.part_no);
    $('#d_part_desc').val(row.part_desc);
    $('#d_invoice_no').val(row.invoice_no);
    if (row.invoice_date != null && row.invoice_date != '')
        $('#d_invoice_date').val(moment(row.invoice_date).format('DD MMM YYYY'));
    $('#d_source').val(row.source);
    $('#d_doc_number').val(row.doc_number);

    $('#modal').modal({
        backdrop: true,
        keyboard: false,
        show: true,
    });

    refreshDetailTable = function () {
        enableLink(false);

        if (row.doc_date != null && row.doc_date != '')
            var docDate = moment(row.doc_date.replace('-', '/').replace('-', '/')).format('MM/DD/YYYY');
        else
            docDate = null;

        enableLink(false);
        window.pis.table({
            objTable: $("#trakStatus"),
            urlSearch: '/parttracking/DetailTrakindoPage',
            urlPaging: '/parttracking/DetailTrakindoPageXt',
            searchParams: {
                part_no: row.part_no,
                case_number: row.case_number,
                JCode: row.JCode,
                sos: row.sos,
                RCDCD: row.RCDCD,
                doc_date: docDate,
                type: 'StockReplenishment',
                RPORNE: $('#RPORNE').val(),
                ORDSOS: $('#ORDSOS').val(),
                store_no: $('#store_no').val()
            },
            enableLink: false,
            autoLoad: true
        });

        window.pis.table({
            objTable: $("#forwStatus"),
            urlSearch: '/parttracking/DetailForwarderPage',
            urlPaging: '/parttracking/DetailForwarderPageXt',
            searchParams: {
                RCDCD: row.RCDCD,
                case_number: row.case_number,
                RPORNE: $('#RPORNE').val(),
                trilc: row.trilc,
                type: 'StockReplenishment'
            },
            enableLink: false,
            autoLoad: true
        });

        setTimeout(function () {
            enableLink(false);
            $("#supStatus").find(".noMatches").html('<span style="color:red">..loading ..!</span>');
            $('.fixed-table-loading').css('color', 'red');
            $('.fixed-table-loading').show();
        }, 5);

        window.pis.table({
            objTable: $("#supStatus"),
            urlSearch: '/parttracking/DetailSupplierPage',
            urlPaging: '/parttracking/DetailSupplierPageXt',
            searchParams: {
                order_number: $('#order_number').val(),
                part_no: row.part_no,
                case_number: row.case_number,
                JCode: row.JCode,
                sos: row.sos,
                type: 'StockReplenishment'
            },
            enableLink: true,
            autoLoad: true
        });
    };

    var isrunning = false;
    $("#modal").on('shown.bs.modal', function (e) {
        showMainScrollbar(false);
        if (isrunning == false) {
            isrunning = true;
            enableLink(false);
            $('.fixed-table-loading').css('color', 'red');
            $('.fixed-table-loading').show();
            refreshDetailTable();
        }
    });
    $('#modal').on('hidden.bs.modal', function (e) {
        showMainScrollbar(true);
    });
};

function cancelDetail() {
    $("#detail").empty();
    $("#detail").hide();
    $("#parent").show();
}

