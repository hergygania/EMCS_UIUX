window.operateEvents = {
	'click .detail': function (e, value, row, index) {
		loadModal('/imex-data/ReturnToVendorEdit?id=' + row.PartsOrderID)
	}
};

window.operateEventsOM = {
	'click .editOm': function (e, value, row, index) {
		$('#DetailID').val(row.DetailID);
		$('#OMID').val(row.OMID).change();
		$('#PartsNumber').val(row.PartsNumber);
		$('#CaseNo').val(row.CaseNo);
		$("#modalOM").modal('show');
	}
};

$(function () {

	$.ajaxSetup({ cache: false });

	$('.cal').click(function () {
	    $('#InvoiceDate').focus();
	});

	var $table = $('#tablePartsOrder');

	$("#btnFilter").click(function () {
		var _staDate, _endDate;
		if ($('#InvoiceDate').val() != '') {
			_staDate = $('#InvoiceDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
			_endDate = $('#InvoiceDate').data('daterangepicker').endDate.format('MM/DD/YYYY');
		}

		window.pis.table({
			objTable: $table,
			urlSearch: '/imexdata/partsOrderPage',
			urlPaging: '/imexdata/partsOrderPageXt',
			searchParams: {
				InvoiceNo: $('#InvoiceNo').val(),
				DateSta: _staDate,
				DateFin: _endDate,
				JCode: $('#JCode').val(),
				AgreementType: $('#AgreementType').val(),
				StoreNumber: $('#StoreNumber').val(),
				DANumber: $('#DANumber').val()
			},
			autoLoad: true
		});
	});

	$table.bootstrapTable({
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
		showRefresh: false,
		smartDisplay: false,
		pageSize: '5',
		formatNoMatches: function () {
			return '<span class="noMatches">-</span>';
		},
		columns: [{
			field: 'action',
			title: 'Action',
			width: '99x',
			align: 'center',
			formatter: operateFormatter,
			events: operateEvents,
			class: 'noExl',
			switchable: false
		}, {
			field: 'InvoiceNo',
			title: 'Invoice No',
			halign: 'center',
			align: 'left',
			sortable: true
		}, {
			field: 'InvoiceDate',
			title: 'Invoice Date',
			halign: 'center',
			align: 'left',
			sortable: true,
			formatter: 'dateFormatter'
		}, {
			field: 'AgreementType',
			title: 'Agreement Type',
			halign: 'center',
			align: 'left',
			sortable: true
		}, {
			field: 'JCode',
			title: 'J-Code',
			halign: 'center',
			align: 'left',
			sortable: true
		}, {
			field: 'StoreNumber',
			title: 'Store Number',
			halign: 'center',
			align: 'left',
			sortable: true
		}, {
			field: 'DA',
			title: 'DA Number',
			halign: 'center',
			align: 'left',
			sortable: true
		}, {
			field: 'ModifiedBy',
			title: 'ModifiedBy',
			halign: 'center',
			align: 'left',
			sortable: true,
			visible: false
		}, {
			field: 'ModifiedDate',
			title: 'ModifiedDate',
			halign: 'center',
			align: 'left',
			sortable: true,
			formatter: 'dateFormatter',
			visible: false
		}, {
			field: 'PartsOrderID',
			title: 'Id',
			sortable: true,
			visible: false
		}
		]
	});

	$('#btn-clear').click(function () {
	    $('#InvoiceNo').val('');
	    $('#InvoiceDate').val('');
	    $('#AgreementType').val('');
	    $('#JCode').val('');
	    $('#StoreNumber').val('');
	    $('#DANumber').val('');
	});

	function operateFormatter() {
		var btn = [];
		btn.push('<div class="btn-group">');
		btn.push('<button type="button" class="btn btn-xs btn-info detail" title="Detail" data-toggle="modal" data-target="#mymodal"><i class="fa fa-search-plus"></i> Detail</button>')
		btn.push('</div>');
		return btn.join('');
	};

	$(".downloadExcel").click(function () {
		//$(".table2excel").table2excel({
		//	exclude: ".noExl",
		//	filename: "returnToVendor.xls"
		//});
        enableLink(false);
        $.ajax({
            url: "DownloadReturnToVendorToExcel",
            type: 'GET',
            success: function (guid) {
                enableLink(true);
                window.open('DownloadToExcel?guid=' + guid, '_blank');
            },
            cache: false,
            contentType: false,
            processData: false
        });
	});

	enableLink(true);
});



function bindForm(dialog) {
	return;
};
