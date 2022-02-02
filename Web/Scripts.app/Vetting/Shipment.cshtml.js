window.eventsShipment = {
	'click .edit': function (e, value, row, index) {
		getShipmentPage('/vetting-process/ShipmentEdit?id=' + row.ShipmentID);
	}
};

$(function () {
	$.ajaxSetup({ cache: false });
	var $table = $('#tableShipment');
	var _staDate, _endDate;

	$('.cal').click(function () {
	    $('#ETD').datepicker('show');
	});

	$('.cal1').click(function () {
	    $('#ETA').datepicker('show');
	});

	$("#btnFilter").click(function () {
		_staDate = null; _endDate=null
		//if ($('#InvoiceDate').val() != '') {
		//	_staDate = $('#InvoiceDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
		//	_endDate = $('#InvoiceDate').data('daterangepicker').endDate.format('MM/DD/YYYY');
		//}

		/* 
			freight: get from index.cshtml
			vettingRoute: get from partial Shipment.cshtml
		*/
		refreshShipment();
	});

    $(".downloadExcel").click(function () {   
        enableLink(false);
        $.ajax({
            url: "DownloadShipmentToExcel",
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

	refreshShipment = function () {
		window.pis.table({
			objTable: $table,
			urlSearch: '/vetting-process/shipmentPage',
			urlPaging: '/vetting-process/shipmentPageXt',
			searchParams: {
				Freight: freight,
				vettingRoute: vettingRoute,
				BLAWB: $('#BLAWB').val(),
				ManifestNo: $('#ManifestNo').val(),
				ETA: $('#ETA').val(),
				ETD: $('#ETD').val(),
				//DateSta: _staDate,
				//DateFin: _endDate,
				Vessel: $('#Vessel').val(),
				LoadingPortDesc: $('#LoadingPortDesc').val(),
				DestinationPortDesc: $('#DestinationPortDesc').val()
			},
			dataHeight: 373,
			autoLoad: true
		});
	}

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
			width: '77px',
			align: 'center',
			formatter: operateFormatter({ Detail: false, Edit: true, Delete: false }),
			events: eventsShipment,
			switchable: false
		}, {
			title: 'No',
			halign: 'center',
			align: 'right',
			width: '3%',
			formatter: runningFormatter,
			switchable: false
		}, {
			field: 'BLAWB',
			title: 'BL/AWB',
			halign: 'center',
			align: 'left',
			sortable: true,
			switchable: false
		}, {
			field: 'Vessel',
			title: 'Vessel/Voyage',
			halign: 'center',
			align: 'left',
			sortable: true,
			switchable: false
		}, {
			field: 'LoadingPortDesc',
			title: 'Loading Port',
			halign: 'center',
			align: 'left',
			sortable: true
		}, {
			field: 'DestinationPortDesc',
			title: 'Destination Port',
			halign: 'center',
			align: 'left',
			sortable: true
		}, {
			field: 'ETD',
			title: 'ETD',
			halign: 'center',
			align: 'right',
			formatter: 'dateFormatter',
			sortable: true
		}, {
			field: 'ETA',
			title: 'ETA',
			halign: 'center',
			align: 'right',
			formatter: 'dateFormatter',
			sortable: true
		}, {
			field: 'totManifest',
			title: 'Manifest',
			halign: 'right',
			align: 'right',
			sortable: true
		}, {
			field: 'totPackage',
			title: 'Package',
			halign: 'right',
			align: 'right',
			sortable: true
		}
		]
	});

	$('#btn-clear').click(function () {
	    $('#BLAWB').val('');
	    $('#Vessel').val('');
	    $('#ETD').val('');
	    $('#ETA').val('');
	    $('#LoadingPortDesc').val('');
	    $('#DestinationPortDesc').val('');
	    $('#ManifestNo').val('');
	});

	$('#btnCreateShipment').click(function (e) {
		getShipmentPage('/vetting-process/ShipmentAddPage');
	});

	getShipmentPage = function (url) {
		var divId = 'Shipment_' + shipmentMode;
		$("#" + divId).empty();

		$.ajax({
			type: "GET",
			url: url,
			data: { freight: freight, vettingRoute: vettingRoute },
			dataType: "html",
			async: false,
			cache: false,
			beforeSend: function () {
				enableLink(false);
				$("#" + divId).html("<div style=\"text-align:center;color:red\"><img src='/Content/images/ajax-loading.gif' style=\"padding-right:3px\">...Loading page...</div>");
			},
			success: function (data) {
				$("#" + divId).empty();
				$("#" + divId).html(data);

				if (typeof window.rebindCSS == "undefined") {
					alert("rebindCSS")
					$.getScript("/scripts/script.js", function () {
					});
				}
				else {
					rebindCSS();
				}
			}
		});
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

	$(function () {
		if (myApp.isReadOnly == 'true') {
			//alert('hide')
			$('#btnCreate').hide();
			$('#btnCreateShipment').hide();
            $('#downloadExcel').hide();
			$(".table").bootstrapTable('hideColumn', 'action');
			$("button:contains('New')").hide();
			$("button:contains('Create')").hide();
			$("button:contains('Upload')").hide();
			$("a:contains('Upload')").hide();
			$("a:contains('Template')").hide();
			//$("a:contains('Upload')").parent('div').hide();
		}
	});

});

