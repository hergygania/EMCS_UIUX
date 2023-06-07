window.eventsShipment = {
	'click .edit': function (e, value, row, index) {
		loadDetailPage('/imex-data/ShipmentEdit?id=' + row.ShipmentID);
	}
};

$(function () {
	$.ajaxSetup({ cache: false });
	var $table = $('#tableShipment');
	var _etdSta, _etdFin, _etaSta, _etaFin, _atdSta, _atdFin;

	helpers.buildDropdown('/Picker/GetShippingType', $('#selFreight'), true, null);

	$('.cal').click(function () {
	    $('#ETD').focus();
	});

	$('.cal1').click(function () {
	    $('#ETA').focus();
	});

	$('.cal2').click(function () {
	    $('#ATD').focus();
	});

	$("#btnFilter").click(function () {
		_etdSta = null; _etdFin = null; _etaSta = null; _etaFin = null; _atdSta = null; _atdFin = null;
		if ($('#ETD').val() != '') {
			_etdSta = $('#ETD').data('daterangepicker').startDate.format('MM/DD/YYYY');
			_etdFin = $('#ETD').data('daterangepicker').endDate.format('MM/DD/YYYY');
		}
		if ($('#ETA').val() != '') {
			_etaSta = $('#ETA').data('daterangepicker').startDate.format('MM/DD/YYYY');
			_etaFin = $('#ETA').data('daterangepicker').endDate.format('MM/DD/YYYY');
		}
		if ($('#ATD').val() != '') {
			_atdSta = $('#ATD').data('daterangepicker').startDate.format('MM/DD/YYYY');
			_atdFin = $('#ATD').data('daterangepicker').endDate.format('MM/DD/YYYY');
		}

		refreshShipment();
	});

	$('#btn-clear').click(function () {
	    $('#BLAWB').val('');
	    $('#Vessel').val('');
	    $('#ETD').val('');
	    $('#ETA').val('');
	    $('#ATD').val('');
	    $('#LoadingPortDesc').val('');
	    $('#DestinationPortDesc').val('');
	    $('#selFreight').val('val', '').change();
	});

	$(".downloadExcel").click(function () {
		//$('[name=selectFreight]').val(($('#selFreight').val() == null ? '' : $('#selFreight').val().toString()));
		//return $('#parent').submit();
        enableLink(false);
        $.ajax({
            url: "DownloadShippingDataToExcel",
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

	formSubmit = function (obj) {
		_etdSta = null; _etdFin = null; _etaSta = null; _etaFin = null; _atdSta = null; _atdFin = null;
		if ($('#ETD').val() != '') {
			_etdSta = $('#ETD').data('daterangepicker').startDate.format('MM/DD/YYYY');
			_etdFin = $('#ETD').data('daterangepicker').endDate.format('MM/DD/YYYY');
		}
		if ($('#ETA').val() != '') {
			_etaSta = $('#ETA').data('daterangepicker').startDate.format('MM/DD/YYYY');
			_etaFin = $('#ETA').data('daterangepicker').endDate.format('MM/DD/YYYY');
		}
		if ($('#ATD').val() != '') {
			_atdSta = $('#ATD').data('daterangepicker').startDate.format('MM/DD/YYYY');
			_atdFin = $('#ATD').data('daterangepicker').endDate.format('MM/DD/YYYY');
		}

			$('[name=EtdSta]').val(_etdSta);
			$('[name=EtdFin]').val(_etdFin);
			$('[name=EtaSta]').val(_etaSta);
			$('[name=EtaFin]').val(_etaFin);
			$('[name=AtdSta]').val(_atdSta);
			$('[name=AtdFin]').val(_atdFin);
	};


	refreshShipment = function () {
		window.pis.table({
			objTable: $table,
			urlSearch: '/imexdata/shipmentPage',
			urlPaging: '/imexdata/shipmentPageXt',
			searchParams: {
				BLAWB: $('#BLAWB').val(),
				Vessel: $('#Vessel').val(),
				EtdSta: _etdSta,
				EtdFin: _etdFin,
				EtaSta: _etaSta,
				EtaFin: _etaFin,
				AtdSta: _atdSta,
				AtdFin: _atdFin,
				LoadingPortDesc: $('#LoadingPortDesc').val(),
				DestinationPortDesc: $('#DestinationPortDesc').val(),
				selectFreight: ($('#selFreight').val() == null ? '' : $('#selFreight').val().toString())
			},
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
			width: '99px',
			align: 'center',
			formatter: operateFormatter({ Detail: false, Edit: true, Delete: false }),
			events: eventsShipment,
			switchable: false
		}, {
			field: 'BLAWB',
			title: 'BL/AWB',
			halign: 'center',
			align: 'left',
			sortable: true
		}, {
			field: 'Vessel',
			title: 'Vessel/Voyage',
			halign: 'center',
			align: 'left',
			sortable: true
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
			title: 'Packages',
			halign: 'right',
			align: 'right',
			sortable: true
		}
		]
	});


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

	enableLink(true);
});

