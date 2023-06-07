window.operateManifestDetail = {
	'click .remove': function (e, value, row, index) {
		$.ajax({
			url: '/vetting-process/ManifestDetailDelete',
			type: 'post',
			data: { id: row.DetailID },
			success: function (result) {
				refreshManifest();
			}
		});

	}
};


$(function () {
	enableLink(false);
	var $tblManifest = $('#tblManifest');
	var $tablePart = $('#tablePartsOrder'); //table in PartsOrder.listCheck.cshtml
	var checkedRows = [];

	if (typeof window.rebindCSS == "undefined")
		$.getScript("/scripts/script.js");
	else
		rebindCSS();


	$('#btnAddSX').on('click', function () {
		checkedRows = [];
	})

	/* trigger when click from child modal (PartsOrder.listCheck.cshtml) */
	$('#modalPartOrderList #bntSelectSx').click(function () {
		showMainScrollbar(false);
		enableLink(false);

		$.ajax({
			url: '/vetting-process/ManifestGetSx',
			type: 'post',
			data: { arrObject: JSON.stringify(checkedRows),		
				ShipmentManifestID : $('#ShipmentManifestID').val()
			},
			success: function (result) {
				refreshManifest();
				checkedRows = [];
			}
		});
	});


	$tablePart.on('check-all.bs.table', function (e, object) {
		$.each(object, function (index, row) {

			//$.each(checkedRows, function (i, value) {
			//	if (value != null && value.PartsOrderID == row.PartsOrderID) // delete index
			//	{
			//		delete checkedRows[i];
			//		alert(i +' del ' + row.InvoiceNo)
			//	}
			//});

			checkedRows.push({ PartsOrderID: row.PartsOrderID, InvoiceNo: row.InvoiceNo, InvoiceDate: dateFormatter(row.InvoiceDate), JCode: row.JCode, StoreNumber: row.StoreNumber, AgreementType: row.AgreementType, DA: row.DA, ModifiedBy: row.ModifiedBy, ModifiedDate: dateFormatter(row.ModifiedDate) });
			//checkedRowsPut(row);
		});
		console.log('check-all.bs.table:');
		console.log(checkedRows);
	});

	$tablePart.on('uncheck-all.bs.table', function (e, object) {
		$.each(object, function (index, row) {

			$.each(checkedRows, function (index, value) {
				if (value != null && value.PartsOrderID === row.PartsOrderID) {
					checkedRows.splice(index, 1);
				}
			});

		});
		console.log('uncheck-all.bs.table:');
		console.log(checkedRows);
	});

	$tablePart.on('check.bs.table', function (e, row) {
		checkedRows.push({ PartsOrderID: row.PartsOrderID, InvoiceNo: row.InvoiceNo, InvoiceDate: dateFormatter(row.InvoiceDate), JCode: row.JCode, StoreNumber: row.StoreNumber, AgreementType: row.AgreementType, DA: row.DA, ModifiedBy: row.ModifiedBy, ModifiedDate: dateFormatter(row.ModifiedDate) });
		console.log(checkedRows);
	});

	$tablePart.on('uncheck.bs.table', function (e, row) {
		$.each(checkedRows, function (index, value) {
			if (value != null && value.PartsOrderID === row.PartsOrderID) {
				checkedRows.splice(index, 1);
			}
		});
		console.log(checkedRows);
	});


	refreshManifest = function () {
		enableLink(false);
		createTableManifest();

		window.pis.table({
			objTable: $tblManifest,
			urlSearch: '/vetting-process/ManifestDetailMemory',
			urlPaging: '/vetting-process/ManifestDetailMemoryXt',
			searchParams: {
				ShipmentManifestID: $('#ShipmentManifestID').val(),
			},
			autoLoad: true
		});

	};
	
	createTableManifest = function () {
		$tblManifest.bootstrapTable({
			cache: false,
			pagination: true,
			search: false,
			striped: true,
			clickToSelect: true,
			reorderableColumns: true,
			toolbar: '.toolbarManifes',
			toolbarAlign: 'left',
			onClickRow: selectRow,
			sidePagination: 'server',
			showColumns: true,
			showRefresh: false,
			smartDisplay: false,
			pageSize: '5',
			checkboxHeader: true,
			formatNoMatches: function () {
				return '<span class="noMatches">-</span>';
			},
			columns: [{
				field: 'action',
				title: 'Action',
				width: '99px',
				align: 'center',
				formatter: operateFormatter({ Delete: true }),
				events: operateManifestDetail,
				visible: (isView == 'true' ? false : true),
				switchable: false
			}, {
				field: 'no',
				title: 'No',
				halign: 'center',
				align: 'center',
				formatter: runningFormatter
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
				field: 'totPackage',
				title: 'Packages',
				halign: 'right',
				align: 'right',
				sortable: true,
				formatter: packageFormatter
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

		//$tblManifest.bootstrapTable({
		//	cache: false,
		//	pagination: true,
		//	search: false,
		//	striped: true,
		//	clickToSelect: true,
		//	reorderableColumns: true,
		//	toolbar: '.toolbarManifes',
		//	toolbarAlign: 'left',
		//	onClickRow: selectRow,
		//	sidePagination: 'server',
		//	showColumns: true,
		//	showRefresh: false,
		//	smartDisplay: false,
		//	pageSize: '5',
		//	formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
		//	columns: [{
		//		field: 'action',
		//		title: 'Action',
		//		width: '99px',
		//		align: 'center',
		//		formatter: operateFormatter({ Delete: true }),
		//		events: operateManifestDetail
		//	}, {
		//		field: 'no',
		//		title: 'No',
		//		halign: 'center',
		//		align: 'left',
		//		formatter: runningFormatter
		//	}, {
		//		field: 'CaseNo',
		//		title: 'Case No',
		//		halign: 'center',
		//		align: 'left',
		//		sortable: true
		//	}, {
		//		field: 'CaseType',
		//		title: 'Packing Type',
		//		halign: 'center',
		//		align: 'left',
		//		sortable: true
		//	}, {
		//		field: 'LengthCM',
		//		title: 'Len',
		//		halign: 'center',
		//		align: 'left',
		//		sortable: true
		//	}, {
		//		field: 'WeightKG',
		//		title: 'Weight',
		//		halign: 'center',
		//		align: 'left',
		//		sortable: true
		//	}, {
		//		field: 'HeightCM',
		//		title: 'Height',
		//		halign: 'center',
		//		align: 'left',
		//		sortable: true
		//	}, {
		//		field: 'WideCM',
		//		title: 'Vol',
		//		halign: 'center',
		//		align: 'left',
		//		sortable: true
		//	}, {
		//		field: 'JCode',
		//		title: 'J Code',
		//		halign: 'center',
		//		align: 'left',
		//		sortable: true
		//	}, {
		//		field: 'InvoiceNo',
		//		title: 'SX Inv #',
		//		halign: 'center',
		//		align: 'left',
		//		sortable: true
		//	}
		//	//, {
		//	//	field: 'EndDestination',
		//	//	title: 'End Dest',
		//	//	halign: 'center',
		//	//	align: 'left',
		//	//	sortable: true
		//	//}
		//	]
		//});

	};

	createTableManifest();

	if (crudMode == 'U') {
		refreshManifest()
	};

	function operateFormatter(options) {
		var btn = [];

		btn.push('<div class="btn-group">');
		if (options.Add == true)
			btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>')
		if (options.Edit == true)
			btn.push('<button type="button" class="btn btn-xs btn-info edit" title="Edit"><i class="fa fa-edit"></i></button>');
		if (options.Delete == true)
			btn.push('<button type="button" class="btn btn-xs btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
		if (options.Info == true)
			btn.push('<button type="button" class="btn btn-xs btn-info info" title="Info"><i class="fa fa-info-circle"></i></button>')
		if (options.Detail == true)
			btn.push('<button type="button" class="btn btn-xs btn-info detail" title="Detail" data-toggle="modal" data-target="#mymodal"><i class="fa fa-search-plus"></i> Detail</button>')
		if (options.Detail2 == true)
			btn.push('<button type="button" class="btn btn-xs btn-info detail" title="Detail" data-toggle="modal" data-target="#mdlDetail"><i class="fa fa-search-plus"></i> Detail</button>')

		btn.push('</div>');

		return btn.join('');
	}

	function packageFormatter(value, row, index) {
		return row.totPackage + '&nbsp;&nbsp;<a href="/vetting-process/detailmanifest-' + row.ShipmentManifestID + '-' + row.PartsOrderID + '" target="_blank">View</a>&nbsp;&nbsp;';
	};


	$("form").removeData("validator");
	$("form").removeData("unobtrusiveValidation");
	$.validator.unobtrusive.parse("form");
});
