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
	},
	'click .imexInvoice': function (e, value, row, index) {
		$('#imex_sxNo').val(row.InvoiceNo);
		$("form#submitImex").submit();
	}
};


$(function () {
	enableLink(false);
	var $tblManifest = $('#tblManifest');

	if (typeof window.rebindCSS == "undefined")
		$.getScript("/scripts/script.js");
	else
		rebindCSS();

	$('#detBLAWB').text($('#_blawb').val())
	$('#detVessel').text($('#Vessel').val())

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
				sortable: true,
				formatter: operateFormatterInvNo,
				events: operateManifestDetail
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

	};

	createTableManifest();

	if (crudMode == 'U') {
		refreshManifest()
	}
	else {
		enableLink(true);
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
	function operateFormatterInvNo(value, row, index) {
		return '<a class="imexInvoice" style="cursor:pointer;text-decoration:underline">' + row.InvoiceNo + '</a>';
	};

	$("form").removeData("validator");
	$("form").removeData("unobtrusiveValidation");
	$.validator.unobtrusive.parse("form");
});

function frmManifestSubmit(frm) {

	if (!$("form#" + frm.id).valid()) {
		return;
	}
	$('#progress').show();
	enableLink(false);

	$.ajax({
		url: frm.action,
		type: frm.method,
		data: $(frm).serialize(),
		success: function (result) {

			enableLink(true);

			if (result.Status == 0) {
				$('#progress').hide();
				//if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
				cancelManifest();
				refreshShipmentManifest();
			}
			else if (result.Status == 2) {
				$('#progress').hide();
				if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
			}
			else {
				//if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
				$('#progress').hide();
			}
		}
	});
	return false;
}

function cancelManifest() {
	$("#detail").empty();
	$("#detail").hide();
	$("#parent").show();
};
