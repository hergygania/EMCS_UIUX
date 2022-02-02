window.operateEventsDet = {
	'click .detail': function (e, value, row, index) {
		bindPopup(row);
	},
	'click .leadtime': function (e, value, row, index) {
		bindPopupMil(row);
	},
	'click .imex': function (e, value, row, index) {
		alert('You click remove icon, row: ' + JSON.stringify(row));
		console.log(value, row, index);
	}
};

$(function () {
    reloadScripts("OrderThruCounterDetail.cshtml.js", "token")
	enableLink(false);
	var $tableDet = $('#tableDet');
	$(".js-states").select2();
	$('#datePicker').daterangepicker();

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

	var width = $(".select2-container--default").width() - 5;
	$(".select2-container--default").css('width', width + 'px');

	$tableDet.bootstrapTable({
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
		exportTypes: ['excel'],
		exportOptions: {
			ignoreColumn: [0],
			fileName: 'file.xls'
		},
		formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
		columns: [
            { field: 'action', title: 'Action', width: '200px', align: 'center', formatter: operateFormatter, events: operateEventsDet, switchable: false },
            { field: 'SOS', title: 'SOS', width: '65px', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'part_no', title: 'Part No', width: '85px', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'part_desc', title: 'Part Desc', width: '230px', halign: 'center', align: 'left', sortable: true, switchable: false },
		    { field: 'progress', title: 'Progress', width: '100px', halign: 'center', align: 'left', formatter: progressFormat, switchable: false },
    		//{ field: 'cols', title: 'progress_leg', halign: 'center', align: 'left', formatter: progressFormat2, visible: false },
    		{ field: 'status', title: 'Status', width: '180px', halign: 'center', align: 'left', sortable: true },
            { field: 'cust_line', title: 'Cust<br />Line', width: '60px', halign: 'center', align: 'right', sortable: false },
            { field: 'line_item_no', title: 'Doc<br/>Line', width: '70px', halign: 'center', align: 'right', sortable: true, },
		    { field: 'doc_invoice', title: 'Invoice No', width: '100px', halign: 'center', align: 'left', sortable: true },
            { field: 'doc_inv_date', title: 'Inv Date', width: '110px', halign: 'center', align: 'right', formatter: dateFormatter, sortable: true, cellStyle: function cellStyle(value, row, index) { return { classes: 'text-nowrap' }; } },
    		//{ field: 'ack_date', title: 'Ack_Date', halign: 'center', align: 'right', formatter: dateFormatter, sortable: true, visible: false },
    		{ field: 'qty_order', title: 'Qty<br/>Order', width: '90px', halign: 'center', align: 'right', sortable: true, footerFormatter: function totQty(data) { return '<span class="totalQty"></span>'; }, switchable: false, },
            { field: 'qty_shipped', title: 'Qty<br/>Shipped', width: '90px', halign: 'center', align: 'right', sortable: true, footerFormatter: function totQty2(data) { return '<span class="totalQtyShi"></span>'; } },
            { field: 'qty_bo', title: 'Qty<br/>BO', width: '90px', halign: 'center', align: 'right', sortable: true, footerFormatter: function totQty3(data) { return '<span class="totalQtyBo"></span>'; }, switchable: false },
		    { field: 'eta', title: 'ETA', width: '110px', halign: 'right', align: 'right', formatter: dateFormatter, sortable: true, cellStyle: function cellStyle(value, row, index) { return { classes: 'text-nowrap', css: { "color": "#000000" } }; } },
		    { field: 'ata', title: 'ATA', width: '110px', halign: 'right', align: 'right', formatter: dateFormatter, sortable: true, switchable: false, cellStyle: function cellStyle(value, row, index) { return { classes: 'text-nowrap', css: { "color": "#000000" } }; } },
		    { field: 'weight', title: 'Weight', width: '80px', halign: 'center', align: 'right', sortable: true, footerFormatter: function totWt(data) { return '<span class="totalWeight"></span>'; } },
		    { field: 'source', title: 'Source', width: '80px', halign: 'center', align: 'left', sortable: true },
    		{ field: 'profile', title: 'Profile', width: '100px', halign: 'center', align: 'left', sortable: true },
            { field: 'agreement', title: 'Agreement', width: '150px', halign: 'center', align: 'left', sortable: true },
    		{ field: 'commodity_name', title: 'Commodity', width: '150px', halign: 'center', align: 'left', sortable: true },
    		{ field: 'act_ind', title: 'Activity<br>Ind', width: '85px', halign: 'center', align: 'center', sortable: true },
    		{ field: 'stock_ind', title: 'Stock<br>Ind', width: '85px', halign: 'center', align: 'center', sortable: true },
            { field: 'order_method', title: 'OM', width: '65px', halign: 'center', align: 'center', sortable: true }
		]
	});

	window.pis.table({
		objTable: $tableDet,
		urlSearch: '/parttracking/OrderThruCounterDetailPage',
		urlPaging: '/parttracking/OrderThruCounterDetailPageXt',
		searchParams: $('#ref_doc_no_det').val(),
		autoLoad: true,
		afterLoadData: function (v) {
		},
		afterComplete: function (v) {
			getAtaDate();
		},
		dataHeight: 412
	});

	var isGetAtaDate = 0;

	getAtaDate = function () {
		if (isGetAtaDate == 1) {
			$('.totalQty').html($('#det_qtyOrder').val());
			$('.totalQtyShi').html($('#det_qtyShipped').val());
			$('.totalQtyBo').html($('#det_qtyBo').val());
			$('.totalWeight').html($('#det_weight').val());
			$(".fixed-table-body").scrollLeft(0);
			return;
		}
		isGetAtaDate = 1;

		$.getJSON("/partTracking/GetOrderThruCounterDetailSum", function (v) {
			var ata = v.ataDate, qOrd = v.qtyOrder, qShi = v.qtyShipped, wt = v.weight, rows = v.rows,
				need = $('#det_need_by_date').val(),
				comm = $('#det_commited_date').val();

			ata = ata == '' ? null : ata;
			need = need == '' ? null : need;
			comm = comm == '' ? null : comm;

			if ((need != null && ata != null) && ata <= need)
				$('.needbydate_up').show();
			else if ((need != null && ata != null) && ata > need)
				$('.needbydate_down').show();
			else
				$('.needbydate_left').show();

			if ((comm != null && ata != null) && ata <= comm)
				$('.commited_up').show();
			else if ((comm != null && ata != null) && ata > comm)
				$('.commited_down').show();
			else
				$('.commited_left').show();

			$('#det_qtyOrder').val('Order:<br>' + qOrd);
			$('#det_qtyShipped').val('Shipped:<br>' + qShi);
			$('#det_qtyBo').val('Bo:<br>' + (qOrd - qShi));
			$('#det_weight').val('Weight:<br>' + wt);

			$('.totalQty').html($('#det_qtyOrder').val());
			$('.totalQtyShi').html($('#det_qtyShipped').val());
			$('.totalQtyBo').html($('#det_qtyBo').val());
			$('.totalWeight').html($('#det_weight').val());

			if (rows==1)
				$('.fixed-table-footer').hide();
		});
	};

	$(".downloadExcel").click(function () {
		$(".table2excel2").table2excel({
			filename: "Order Thru Counter & Service Detail"
		});
	});

});

function operateFormatter(value, row, index) {
	var btn = '';
	if (row.status == 'Supplied Ex-stock') {
	    btn = '<div class="btn-group"> - </div>';
	}
	else {
	    btn = '<div class="btn-group">' +
			'<button type="button" class="btn btn-info detail" title="Detail" href="#modal"><i class="fa fa-search-plus"></i> Detail</button>' +
			'<button type="button" class="btn btn-primary leadtime" title="Leadtime" href="#modalmil"><i class="fa fa-clock-o"></i> Milestone</button>' +
	'</div>';
	}
	return btn;
}

function progressFormat(value, row, index) {
	var dt = row.progress;
	var sAct = parseInt(row.sum_actual);
	var sBm = parseInt(row.sum_bm);
	var seq = parseInt(row.Sequence);
	var percentage = row.percentage;

	var perc = (sBm / sAct) * 100;
	var col = '#0DBB0D';
	if (perc >= 100) col = '#0DBB0D';
	else if (perc >= 80 && perc < 100) col = 'yellow';
	else col = 'red';

	if (row.status == 'Filled') {
		seq = 5;
		perc = 100;
	} else if (row.status == 'Invoicing Only') {
	    seq = 5;
	    perc = 100;
	}

	var progFormat = '<div class="progress progress-xs">';
	if (row.status == 'Invoicing Only') {
	    percentage = 100;
	    progFormat += '<div class="progress-bar" style="background-color:#0DBB0D;width:' + percentage + '%"></div>';
	}
	else if (seq < 2) {
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
	var percentage = row.percentage;
	var progFormat = '<div class="progress progress-xs">';
	if (row.status == 'Supplied Ex-stock') {
		percentage = 100;
		progFormat += '<div class="progress-bar" style="background-color:#0DBB0D;width:' + percentage + '%"></div>';
	}
	else {
		if (dt < 80)
			progFormat += '<div class="progress-bar" style="background-color:red;width:' + percentage + '%"></div>';
		else if (dt < 100)
			progFormat += '<div class="progress-bar" style="background-color:yellow;width:' + percentage + '%"></div>';
		else if (dt >= 100)
			progFormat += '<div class="progress-bar" style="background-color:#0DBB0D;width:' + percentage + '%"></div>';
	}

	progFormat += '</div>';
	var sAct = parseInt(row.sum_actual);
	var sBm = parseInt(row.sum_bm);
	var seq = parseInt(row.Sequence);
	return ' B:' + sBm + ' A:' + sAct + ' T:' + seq; //progFormat;
}

function dateFormatter(dt) {
	if (dt != null) {
		var formattedDate = moment(dt).format('DD MMM YYYY');
		return formattedDate;
	}
};

function bindPopupMil(row) {
	$('#modalmil').modal({ show: true });
	$('#divMilestone').empty();
	$('#divMilestone').html("<div style=\"text-align:center;color:red\"><img src='/Content/images/ajax-loading.gif' style=\"padding-right:3px\">...Loading...</div>");
	enableLink(false);

	$('#m_part_no').val(row.part_no);
	$('#m_part_desc').val(row.part_desc);
	$('#m_invoice_no').val(row.doc_invoice);
	if (row.eta != null && row.eta != '')
		$('#m_eta').val(moment(row.eta).format('DD MMM YYYY'));
	$('#m_source').val(row.source);
	$('#m_doc_number').val(row.RFDCNO);

	var data = new Object();
	data.type = row.doc_type;
	if (row.source != null) {
		var source = row.source.replace(' ', '');
		if (source.length >= 3)
			data.store_no = (source[0] == 'S' ? (isNaN(source.replace('S', '')) ? '' : source.replace('S', '')) : '');
	}
	data.sos = row.SOS;
	data.trilc = row.trilc;
	data.doc_date = $('#doc_date').val();
	if (row.doc_inv_date != null)
		data.invoice_date = moment(row.doc_inv_date).format('YYYY-MM-DD');
	data.doc_invoice = row.doc_invoice;
	data.cmnt1 = row.cmnt1;
	data.RPORNE = $('#ref_doc_no_det').val();
	data.part_no = row.part_no;
	if (row.ack_date != null)
		data.ackDate = moment(row.ack_date).format('YYYY-MM-DD');
	if (row.pupDate != null)
		data.pupDate = moment(row.pupDate).format('YYYY-MM-DD');
	if (row.podDate != null)
		data.podDate = moment(row.podDate).format('YYYY-MM-DD');
	if (row.receiveDate != null)
		data.receiveDate = moment(row.receiveDate).format('YYYY-MM-DD');

	var isRunning = 0;
	$("#modalmil").on('shown.bs.modal', function (e) {
		if (isRunning == 1) return;
		isRunning = 1;
		showMainScrollbar(false);
		$.getJSON("/partTracking/GetMilestoneList",
			{
				params: JSON.stringify(data)
			},
			function (results) {
				setData(results);
				enableLink(true);
			});
	});

	$('#modalmil').on('hidden.bs.modal', function (e) {
		showMainScrollbar(true);
	});
}


function bindPopup(row) {
	$('#modal').modal({
		backdrop: true,
		keyboard: false,
		show: true,
	});

	$('#d_part_no').val(row.part_no);
	$('#d_part_desc').val(row.part_desc);
	$('#d_invoice_no').val(row.doc_invoice);
	if (row.doc_inv_date != null)
		$('#d_invoice_date').val(moment(row.doc_inv_date).format('DD MMM YYYY'));
	$('#d_source').val(row.source);
	$('#d_doc_number').val(row.RFDCNO);


	refreshDetailTable = function () {
		window.pis.table({
			objTable: $("#supStatus"),
			urlSearch: '/parttracking/DetailSupplierPage',
			urlPaging: '/parttracking/DetailSupplierPageXt',
			searchParams: {
				part_no: row.part_no,
				JCode: row.JCode,
				sos: row.SOS,
				rfdcno: row.doc_invoice,
				qty_bo: row.qty_bo,
				type: 'OrderThruCounter'
			},
			enableLink: false,
			autoLoad: true
		});

		window.pis.table({
			objTable: $("#forwStatus"),
			urlSearch: '/parttracking/DetailForwarderPage',
			urlPaging: '/parttracking/DetailForwarderPageXt',
			searchParams: {
				cmnt1: row.doc_invoice,
				trilc: row.trilc,
				type: 'OrderThruCounter'
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
			objTable: $("#trakStatus"),
			urlSearch: '/parttracking/DetailTrakindoPage',
			urlPaging: '/parttracking/DetailTrakindoPageXt',
			searchParams: {
				part_no: row.part_no,
				JCode: row.JCode,
				sos: row.sos,
				type: 'OrderThruCounter',
				rfdcno: row.RFDCNO,
				case_number: row.doc_invoice
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
}
