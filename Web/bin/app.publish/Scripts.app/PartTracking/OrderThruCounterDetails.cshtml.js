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
    reloadScripts("OrderThruCounterDetails.cshtml.js", "token")
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
            { field: 'part_no', title: 'Part No', width: '85px', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'part_desc', title: 'Part Desc', width: '230px', halign: 'center', align: 'left', sortable: true, switchable: false },
		    { field: 'progress', title: 'Progress', width: '100px', halign: 'center', align: 'left', formatter: progressFormat, switchable: false },
    		{ field: 'status', title: 'Status', width: '180px', halign: 'center', align: 'left', sortable: true },
    		{ field: 'qty_order', title: 'Qty<br/>Order', width: '90px', halign: 'center', align: 'right', sortable: true, footerFormatter: function totQty(data) { return '<span class="totalQty"></span>'; }, switchable: false, },
            { field: 'qty_shipped', title: 'Qty<br/>Shipped', width: '90px', halign: 'center', align: 'right', sortable: true, footerFormatter: function totQty2(data) { return '<span class="totalQtyShi"></span>'; } },
		    { field: 'eta', title: 'ETA', width: '110px', halign: 'right', align: 'right', formatter: dateFormatter, sortable: true, cellStyle: function cellStyle(value, row, index) { return { classes: 'text-nowrap', css: { "color": "#000000" } }; } }
		]
	});

	window.pis.table({
		objTable: $tableDet,
		urlSearch: '/parttracking/OrderThruCounterDetailsPage',
		urlPaging: '/parttracking/OrderThruCounterDetailsPageXt',
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

		$.getJSON("/partTracking/GetOrderThruCounterDetailsSum", function (v) {
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

