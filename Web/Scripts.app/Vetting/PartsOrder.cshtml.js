
window.operateEvents = {
	'click .detail': function (e, value, row, index) {
		showMainScrollbar(false);
        loadModal('/vetting-process/partsOrderEdit?id=' + row.PartsOrderID);
    }
};

window.operateEventsOM = {
	'click .editOm': function (e, value, row, index) {
		$("#modalOM").modal('show');
		$('#DetailID').val(row.DetailID);
		$('#OMID').val(row.OMID).change();
		$('#PartsNumber').val(row.PartsNumber);
		$('#CaseNo').val(row.CaseNo);
		$('#Remark').val('');
		//resolve bugid 1153
		if (vettingRoute == 3 && vettingRoute == row.VettingRoute) {
			$('.divReturntToVendor').show();
		}
		else {
			$('.divReturntToVendor').hide();
		}
		
		$('#ReturnToVendor').val(row.ReturnToVendor);
		if (row.ReturnToVendor == null || row.ReturnToVendor == '0')
			$('#return-n').click();
		else
			$('#return-y').click();
	}
};

$(function () {
	enableLink(false);

	helpers.buildDropdown('/Picker/GetAgreementType', $('#selAgreementType'), true, null);
	helpers.buildDropdown('/Picker/GetJCode', $('#selJCode'), true, null);

	$.ajaxSetup({ cache: false });
	var $table = $('#tablePartsOrder');

	$('.cal').click(function () {
	    $('#InvoiceDate').focus();
	});

	$("#btnFilter").click(function () {
		var _staDate, _endDate;
		if ($('#InvoiceDate').val() != '') {
			_staDate = $('#InvoiceDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
			_endDate = $('#InvoiceDate').data('daterangepicker').endDate.format('MM/DD/YYYY');
		}

		/* 
			freight: get from index.cshtml
			$('#DefaultShippId').val(): get from index.cshtml
			vettingRoute: get from partial partsOrder.cshtml
		*/


		window.pis.table({
			objTable: $table,
			urlSearch: '/vetting-process/partsOrderPage',
			urlPaging: '/vetting-process/partsOrderPageXt',
			searchParams: {
				Freight: freight,
				FreightShippId: $('#DefaultShippId').val(),
				vettingRoute: vettingRoute,
				InvoiceNo: $('#InvoiceNo').val(),
				DateSta: _staDate,
				DateFin: _endDate,
				selJCode: $('#selJCode').val(),
				selAgreementType: $('#selAgreementType').val(),
				StoreNumber: $('#StoreNumber').val()
			},
			dataHeight: 373,
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
		showExport: true,
		exportTypes: ['excel', 'csv', 'json', 'txt'],
		exportOptions: {
			ignoreColumn: [0],
			fileName: 'file.xls'
		},
		pageSize: '25',
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
			class: 'noExl,no_export',
			switchable: false
		}, {
			title: 'No',
			halign: 'center',
			align: 'right',
			width: '3%',
			formatter: runningFormatter,
			switchable: false
		}, {
			field: 'InvoiceNo',
			title: 'Invoice No',
			halign: 'center',
			align: 'left',
			sortable: true,
			switchable: false
		}, {
			field: 'InvoiceDate',
			title: 'Invoice Date',
			halign: 'right',
			align: 'right',
			sortable: true,
			switchable: false,
			formatter: 'dateFormatter'
		}, {
			field: 'AgreementType',
			title: 'Agreement Type',
			halign: 'center',
			align: 'center',
			sortable: true
		}, {
			field: 'JCode',
			title: 'J-Code',
			halign: 'center',
			align: 'center',
			sortable: true,
			switchable: false,
		}, {
			field: 'StoreNumber',
			title: 'Store Number',
			halign: 'center',
			sortable: true,
			switchable: false,
		}, {
			field: 'ShippingInstruction',
			title: 'Ship-Instruction',
			halign: 'center',
			align: 'left',
			sortable: true,
			visible: false
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
	});

	function operateFormatter() {
		var btn = [];
		btn.push('<div class="btn-group">');
		btn.push('<button type="button" class="btn btn-xs btn-info detail" title="Detail" data-toggle="modal" data-target="#mymodal"><i class="fa fa-search-plus"></i> Detail</button>')
		btn.push('</div>');
		return btn.join('');
	};

	

	$(".downloadExcel").click(function () {
		//$table.tableExport({ type: 'excel', escape: 'false', ignoreColumn: [0], tableName: 'PartsOrder' });
		//$(".table2excel").table2excel({
		//	exclude: ".noExl",
		//	filename: "vetting-PartsOrder.xls"
		//});
        enableLink(false);
        $.ajax({
            url: "DownloadPartsOrderToExcel",
            type: 'GET',
            data: "freight=" + freight + "&vettingRoute=" + vettingRoute,
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
