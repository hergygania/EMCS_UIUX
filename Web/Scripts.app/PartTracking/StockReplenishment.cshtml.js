var $table = $('#table-stock');
$(function () {
    reloadScripts("StockReplenishment.cshtml.js", "token")
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'true' });

    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'true' });
    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');

    $('.cal').click(function () {
        $('#doc_date').focus();
    });

    $('.cal1').click(function () {
        $('#supply_docinv_date').datepicker('show');
    });


    var MaterialType = new mySelect2({
        id: 'MaterialType',
        url: '/partTracking/GetMaterialDescription',
        minimumInputLength: 1
    }).load();


    $("#filter_by").change(function (e) {
        enableLink(false);
        var val = $(this).val();
        $.getJSON("/partTracking/GetStore",
				{
				    filter_type: $("input[name='filter_type']:checked").val(),
				    id: val
				},
		function (results) {
		    $('#selStoreList_Nos').empty();
		    $('#selStoreList_Nos').append($("<option value=''>ALL</option>"));
		    $.each(results, function (i, data) {
		        $('#selStoreList_Nos').append($("<option value=" + data.Plant + ">" + data.Name + "</option>"));
		    });
		    enableLink(true);
		});
        $('#selStoreList_Nos').val('val', '').change();

    });
    $("#filter_by").change();

    $("#sr_type").change(function (e) {
        var val = $(this).val();

        $('#doc_status').empty();
        $('#doc_status').append($("<option value=''>ALL</option>"));
        if (val == 'O' || val == '') {
            $('#doc_status').append('<option value="ACK">Acknowledgment Status</option>');
            $('#doc_status').append('<option value="Outstanding Supplier">Outstanding Supplier</option>');
            $('#doc_status').append('<option value="Outstanding Supplier > 15 Days">Outstanding Supplier &gt; 15 Days</option>');
            $('#doc_status').append('<option value="In Transit">In Transit</option>');
            $('#doc_status').append('<option value="POD">POD</option>');
            $('#doc_status').append('<option value="Receipt Goods">Receipt Goods</option>');

        }
        else {
            $('#doc_status').append('<option value="In Transit">In Transit</option>');
            $('#doc_status').append('<option value="POD">POD</option>');
            $('#doc_status').append('<option value="Receipt Goods">Receipt Goods</option>');
        }
        $('#doc_status').val('', 'ALL').change();
    });

    $("#order_class").change(function (e) {
        //var val = $(this).val();
        //$.getJSON("/partTracking/StockReplenishmentGetOrderProfile",
		//		{
		//		    orderClass: val
		//		},
		//function (results) {
		//    $('#order_profile').empty();
		//    $('#order_profile').append($("<option value=''>ALL</option>"));
		//    $.each(results, function (i, data) {
		//        $('#order_profile').append($("<option value=" + data.ProfileNumber + ">" + data.ProfileDescription + "</option>"));
		//    });
		//});
        //$('#order_profile').val('val', '').change();
    });

    $table.bootstrapTable({
        cache: false,
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
        showExport: true,
        exportTypes: ['excel'],
        exportOptions: {
            ignoreColumn: [0],
            fileName: 'file.xls'
        },
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [
            { field: 'action', title: 'Action', width: '180px', align: 'center', formatter: operateFormatter, events: operateEvents, switchable: false },
    		{ field: 'ORDSOS', title: 'Material Type', width: '150px', halign: 'center', align: 'left', sortable: true, switchable: false },
    		{ field: 'store_name', title: 'Store', width: '350px', halign: 'center', align: 'left', sortable: true },
		    { field: 'order_number', title: 'Order Number', halign: 'center', width: '120px', align: 'left', sortable: true, switchable: false },
            { field: 'doc_date', title: 'Doc Date', halign: 'center', width: '105px', align: 'right', formatter: dateFormatter, sortable: true, switchable: false },
            { field: 'supply_docinv', title: 'Invoice No', halign: 'center', width: '150px', align: 'left', sortable: true },
            { field: 'supply_docinv_date', title: 'Invoce Date', halign: 'center', width: '105px', align: 'right', formatter: dateFormatter, sortable: true },
    		{ field: 'progress', title: 'Progress', width: '150px', halign: 'center', align: 'left', formatter: progressFormat },
            //{ field: 'cols', title: 'progress_leg', halign: 'center', align: 'left', formatter: progressFormat2, visible: false },
    		{ field: 'doc_status', title: 'Doc Status', halign: 'center', align: 'left', width: '220px', sortable: true },
    		{ field: 'eta', title: 'ETA', halign: 'right', width: '105px', align: 'right', formatter: dateFormatter, sortable: true },
    		{ field: 'ata', title: 'ATA', halign: 'right', width: '105px', align: 'right', formatter: dateFormatter, sortable: true },
		    { field: 'RPORNE', title: 'RPORNE', width: '220px', sortable: true }
    		//{ field: 'store_no', title: 'store_no', sortable: true, visible: false },
    		//{ field: 'sos_group_type', title: 'sos_group_type', sortable: true, visible: false },
    		//{ field: 'endDate', title: 'end_date', formatter: dateFormatter, sortable: true, visible: false },
    		//{ field: 'origin', title: 'origin', sortable: true, visible: false },
    		//{ field: 'destination', title: 'destination', sortable: true, visible: false }
        ]
    });

    $('#btn-clear').click(function () {
        $('#filter_by').val('', 'ALL').change();
        $('#selStoreList_Nos').val('val', '').change();
        //$('#sr_type').val('', 'O').change();
        $('#order_class').val('', 'ALL').change();
        $('#order_profile').val('', 'ALL').change();
        $('#shp_type').val('', 'ALL').change();
        $('#agreement').val('', 'ALL').change();
        $('#doc_status').val('', 'ALL').change();
        //$('#doc_date').val('');
        $('#order_number').val('');
        $('#supply_docinv').val('');
        $('#supply_docinv_date').val('');
        $('#case_no').val('');
        $('#part_number').val('');
        $('#part_desc').val('');
        $('#MaterialType').val('', 'ALL').change();
    });

    $('#btn-filter').click(function () {
        var _staDate, _endDate;
        if ($('#doc_date').val() != '') {
            _staDate = $('#doc_date').data('daterangepicker').startDate.format('MM/DD/YYYY');
            _endDate = $('#doc_date').data('daterangepicker').endDate.format('MM/DD/YYYY');
        }
        
        window.pis.table({
            objTable: $table,
            urlSearch: '/parttracking/StockReplenishmentPage',
            urlPaging: '/parttracking/StockReplenishmentPageXt',
            searchParams: {
                filter_type: $("input[name='filter_type']:checked").val(),
                filter_by: $('#filter_by').val(),
                selStoreList_Nos: $('#selStoreList_Nos').val(),
                sr_type: $('#sr_type').val(),
                order_class: $('#order_class').val(),
                order_profile: $('#order_profile').val(),
                shp_type: $('#shp_type').val(),
                agreement: $('#agreement').val(),
                doc_date_start: _staDate,
                doc_date_end: _endDate,
                order_number: $('#order_number').val(),
                supply_docinv: $('#supply_docinv').val(),
                supply_docinv_date: $('#supply_docinv_date').val(),
                case_no: $('#case_no').val(),
                part_number: $('#part_number').val(),
                part_desc: $('#part_desc').val(),
                MaterialType: $('#MaterialType').val(),
                doc_status: $('#doc_status').val()
            },
            dataHeight: 412,
            autoLoad: true
        });
    });

    $(".downloadExcel").click(function () {
        $(".table2excel").table2excel({
            name: "Stock Replenishment",
            filename: "Stock Replenishment.xls"
        });
    });
    //  $('#btn-filter').click();
    enableLink(true);
});


function operateFormatter(value, row, index) {
    var btn = '', width = 'width:121px;';
    if (row.supply_docinv == null || row.supply_docinv == '')
        btn = '<button type="button" class="btn btn-primary leaddis" style="cursor:not-allowed;filter: alpha(opacity=65); -webkit-box-shadow: none; box-shadow: none;opacity: .65;" title="N/A"><i class="fa fa-check-circle"></i> IMEX</button>';
    else
        btn = '<button type="button" class="btn btn-danger imex" title="Imex"><i class="fa fa-check-circle"></i> IMEX</button>';

    if ($('#sr_type').val() == 'T') {
        btn = ''; width = '';
    }

    return [
	    '<div class="btn-group">',
		    '<button type="button" class="btn btn-info detail" title="Detail"><i class="fa fa-search-plus"></i> Detail</button>',
		    btn,
	    '</div>'
    ].join('');
}

window.operateEvents = {
    'click .detail': function (e, value, row, index) {
        var docDate = dateFormatter(row.doc_date);
        var invno = row.supply_docinv;
        var invDate = dateFormatter(row.supply_docinv_date);
        var recvDate = dateFormatter(row.receiveDate);
        var trackingid = row.tracking_id;
        var pod = dateFormatter(row.podDate);
        var pup = dateFormatter(row.pupDate);

        loadDetailPage('/partTracking/StockReplenishmentDetail?rporne=' + row.RPORNE + '&ordsos=' + row.ORDSOS + '&stno=' + row.store_no + '&orderNo=' + row.order_number +
			'&invNo=' + invno + '&docDate=' + docDate + '&invDate=' + invDate + '&recvDate=' + recvDate + '&docSts=' + row.doc_status +
			'&trackingid=' + trackingid + '&podDt=' + pod + '&pupDt=' + pup);
        //window.location = '/partTracking/StockReplenishmentDetail?rporne=' + row.RPORNE + '&ordsos=' + row.ORDSOS + '&stno=' + row.store_no;
        //console.log(value, row, index);
    },
    'click .imex': function (e, value, row, index) {
        $('#imex_sxNo').val(row.supply_docinv);
        $('#imex_partNo').val('');
        $('#imex_caseNo').val('');
        $("form#submitImex").submit();
        //var url = '/detail-part-and-case-' + row.supply_docinv + '--';
        //var params = ['height='+screen.height,'width='+screen.width,'fullscreen=yes'].join(',');
        //var popup = window.open(url, '_blank');//, params); 
        //popup.moveTo(0, 0);
    }
};

function progressFormat(value, row, index) {
    var perc = row.percentage;
    var col = '#0DBB0D';
    if (perc >= 91) col = '#0DBB0D';
    else if (perc > 79 && perc < 91) col = 'yellow';
    else col = 'red';

    return [
	'<div class="progress progress-xs">',
		'<div class="progress-bar" style="background-color:' + col + ';width:' + row.progress + '%"></div>',
		//'<div class="progress-bar progress-bar-' + col + '" style="width:' + row.progress + '%"></div>',
	'</div>'
    ].join('');
}
function progressFormat2(value, row, index) {
    var sAct = row.sum_actual;
    var sBm = row.sum_bm;
    var seq = row.Sequence;
    var dly = row.delay;
    return '<div style="white-space:nowrap">B:' + sBm + ' A:' + sAct + ' D:' + dly + ' T:' + seq + '</di>';
}
function dateFormatter(dt) {
    if (dt != null || dt != undefined) {
        var formattedDate = moment(dt).format('DD MMM YYYY');
        return formattedDate;
    }
};

function setFilter(index) {
    enableLink(false);
    $.getJSON("/partTracking/GetFilterBy", { index: index },
			function (results) {
			    $('#filter_by').val('', 'ALL').change();
			    $('#filter_by').empty();
			    $('#filter_by').append($("<option value=''>ALL</option>"));
			    $.each(results, function (i, data) {
			        if (index == 1)
			            $('#filter_by').append($("<option value=" + data.HubID + ">" + data.Name + "</option>"));
			        else
			            $('#filter_by').append($("<option value=" + data.AreaID + ">" + data.Name + "</option>"));
			    });
			});
    $("#filter_by").select2();
}



