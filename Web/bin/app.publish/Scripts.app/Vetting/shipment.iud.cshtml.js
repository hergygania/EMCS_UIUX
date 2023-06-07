window.operateManifest = {
	'click .edit': function (e, value, row, index) {
		//$('a').attr("disabled","disabled");
		//$('.content-header,.nav-tabs-custom,.nav-pills').css('opacity', '0.2');
		loadDetailPage('/vetting-process/ManifestEdit?id=' + row.ShipmentManifestID);
		//loadModal('/vetting-process/ManifestEdit?id=' + row.ShipmentManifestID)
	},
	'click .remove': function (e, value, row, index) {
		$.ajax({
			url: '/vetting-process/ManifestDelete',
			type: 'post',
			data: { id: row.ShipmentManifestID },
			success: function (result) {
				refreshShipmentManifest(row.ShipmentID);
			}
		});
	},
	'click .remove2': function (e, value, row, index) {
		$.ajax({
			url: '/vetting-process/ShipmentDocumentDelete',
			type: 'post',
			data: { id: row.ShipmentDocumentID },
			success: function (result) {
				refreshShipmentDocument(row.ShipmentID);
			}
		});
	}

};

$(function () {
	if (typeof window.rebindCSS == "undefined")
		$.getScript("/scripts/script.js");
	else
		rebindCSS();

	var $tblShipmentManifest = $('#tableManifest');
	var $tbltableAttachment = $('#tableAttachment');

	refreshShipmentManifest = function (shipmentId) {
		enableLink(false);
		createTableShipmentManifest();
		shipmentId = (shipmentId == undefined ? -1 : shipmentId);

		window.pis.table({
			objTable: $tblShipmentManifest,
			urlSearch: '/vetting-process/ShipmentManifestPage',
			urlPaging: '/vetting-process/ShipmentManifestPageXt',
			searchParams: {
				ShipmentID: shipmentId,
			},
			autoLoad: true
		});

		$.ajax({
			url: '/vetting-process/ShipmentManifestTotal',
			type: "POST",
			dataType: "json",
			success: function (d) {
				$('[name=totPackage]').val(d.result.TotalPackage);
			}
		});

	};

	refreshShipmentDocument = function (shipmentId) {
		enableLink(false);
		createTableAttachment();
		shipmentId = (shipmentId == undefined ? -1 : shipmentId);

		window.pis.table({
			objTable: $tbltableAttachment,
			urlSearch: '/vetting-process/ShipmentDocumentPage',
			urlPaging: '/vetting-process/ShipmentDocumentPageXt',
			searchParams: {
				ShipmentID: shipmentId,
			},
			autoLoad: true
		});
	};

	createTableShipmentManifest = function () {
		$tblShipmentManifest.bootstrapTable({
			cache: false,
			pagination: true,
			search: false,
			striped: true,
			clickToSelect: true,
			reorderableColumns: true,
			toolbarAlign: 'left',
			onClickRow: selectRow,
			sidePagination: 'server',
			showColumns: true,
			showRefresh: false,
			smartDisplay: false,
			toolbar: '.toolbarShipmentManifest',
			pageSize: '5',
			formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
			columns: [{
				field: 'action',
				title: 'Action',
				width: '101px',
				align: 'center',
				formatter: operateFormatter({ Edit: true, Delete: true }),
				events: operateManifest,
				switchable: false
			}, {
				field: 'no',
				title: 'No',
				halign: 'center',
				align: 'center',
				formatter: runningFormatter
			}, {
				field: 'ManifestNumber',
				title: 'Manifest No',
				halign: 'left',
				align: 'left',
				sortable: true
			}, {
				field: 'ContainerNumber',
				title: 'Container No',
				halign: 'left',
				align: 'left',
				sortable: true
			}, {
				field: 'totPackage',
				title: 'Total Package',
				halign: 'right',
				align: 'right',
				sortable: true,
				formatter: packageFormatter
			}
			]
		});
	};

	
	createTableAttachment = function () {
		$tbltableAttachment.bootstrapTable({
			cache: false,
			pagination: true,
			search: false,
			striped: true,
			clickToSelect: true,
			reorderableColumns: true,
			toolbarAlign: 'left',
			onClickRow: selectRow,
			sidePagination: 'server',
			showColumns: true,
			showRefresh: false,
			smartDisplay: false,
			toolbar: '.toolbarAttachment',
			pageSize: '5',
			formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
			columns: [{
				field: 'action',
				title: 'Action',
				width: '150px',
				align: 'center',
				formatter: operateFormatter({ Delete2: true }),
				events: operateManifest,
				switchable: false
			}, {
				field: 'no',
				title: 'No',
				halign: 'center',
				align: 'center',
				sortable: true,
				formatter: runningFormatter
			}, {
				field: 'DocumentName',
				title: 'Document Name',
				halign: 'center',
				align: 'left',
				sortable: true
			}, {
				field: 'FileName',
				title: 'Attachment File',
				halign: 'center',
				align: 'left',
				sortable: true,
				formatter: attachmentFormatter
			}
			]
		});
	};

	createTableShipmentManifest();
	createTableAttachment();

	//edit mode
	if ($('#ShipmentID').val() != '' && $('#ShipmentID').val() != -1) {
		refreshShipmentManifest($('#ShipmentID').val());
		refreshShipmentDocument($('#ShipmentID').val());	
	};

	function attachmentFormatter(value, row, index) {
		return '<a href="' + folderdoc + row.DocumentTypeID + '/' + value + '" target="_blank">' + value + " " + '</a>' +(row.dml == '(duplicate)' ? '<span style="color:red"> (duplicate)</span>' : '');
	};

	function packageFormatter(value, row, index) {
		return row.totPackage + '&nbsp;&nbsp;<a href="/vetting-process/detailmanifest-' + row.ShipmentManifestID+ '-- " target="_blank">View</a>&nbsp;&nbsp;';
	};
	

	function operateFormatter(options) {
		var btn = [];

		btn.push('<div class="btn-group">');
		if (options.Add == true)
			btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>')
		if (options.Edit == true)
			btn.push('<button type="button" class="btn btn-xs btn-info edit" title="Edit"><i class="fa fa-edit"></i></button>');
		if (options.Delete == true)
			btn.push('<button type="button" class="btn btn-xs btn-danger remove" title="Delete" style="margin-left:2px"><i class="fa fa-trash-o"></i></button>');
		if (options.Delete2 == true)
			btn.push('<button type="button" class="btn btn-xs btn-danger remove2" title="Delete"><i class="fa fa-trash-o"></i></button>');
		if (options.Detail == true)
			btn.push('<button type="button" class="btn btn-xs btn-info detail" title="Detail" data-toggle="modal" data-target="#mymodal"><i class="fa fa-search-plus"></i> Detail</button>')
		if (options.Detail2 == true)
			btn.push('<button type="button" class="btn btn-xs btn-info detail" title="Detail" data-toggle="modal" data-target="#mdlDetail"><i class="fa fa-search-plus"></i> Detail</button>')
		btn.push('</div>');
		return btn.join('');
	}

});


function bindForm(dialog) {
	$('form', dialog).submit(function () {
		$('#progress').show();
		enableLink(false);

		$.ajax({
			url: this.action,
			type: this.method,
			data: $(this).serialize(),
			success: function (result) {

				enableLink(true);

				if (result.Status == 0) {
					//if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
					$('#myModalPlace').modal('hide');
					$('#progress').hide();
					refreshShipmentManifest();
				}
				else {
					if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
					$('#progress').hide();
				}
			}
		});
		return false;
	});
};
