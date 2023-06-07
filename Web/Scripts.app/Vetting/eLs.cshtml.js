window.eventsSurvey = {
	'click .edit': function (e, value, row, index) {
		loadDetailPage('/vetting-process/eLsEdit?id=' + row.SurveyID);
	}
};

$(function () {
	$.ajaxSetup({ cache: false });
	var $table = $('#tableSurvey');

	$("#btnFilter").click(function () {

		refreshShipment();
	});

	$(".downloadExcel").click(function () {
		$('[name=selectFreight]').val(($('#selFreight').val() == null ? '' : $('#selFreight').val().toString()));
		return $('#parent').submit();
	});

	refreshShipment = function () {
		if (($('#DateSta').val() != '' && $('#DateFin').val() == '') || $('#DateSta').val() == '' && $('#DateFin').val() != '') {
			$('#DateSta').val('');
			$('#DateFin').val('');
			$('.field-validation-error').html('');
		}

		if ($('#DateSta').val() != '' && $('#DateFin').val() != '') {
			$.validator.addMethod("greaterThan",
				function (value, element, params) {
					var prm = $('#' + params).val();

					if (!/Invalid|NaN/.test(new Date(value))) {
						return new Date(value) >= new Date(prm);
					}
					return isNaN(value) && isNaN(prm) || (Number(value) > Number(prm));
				}, 'Finish date must be greater than Start date.');
			$("#DateFin").rules('add', { greaterThan: "DateSta" });
		}

		if (!$("form#parent").valid()) { $("#DateFin").rules('remove'); return; }

		window.pis.table({
			objTable: $table,
			urlSearch: '/vetting-process/eLsList',
			urlPaging: '/vetting-process/eLsListXt',
			searchParams: {
				vettingRoute: 2,
				Id: $('#SurveyID').val(),
				VRNo: $('#VRNo').val(),
				VONo: $('#VONo').val(),
				DateSta: $('#DateSta').val(),
				DateFin: $('#DateFin').val()
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
		showColumns: false,
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
			events: eventsSurvey
		}, {
			field: 'no',
			title: 'No',
			halign: 'center',
			align: 'center',
			width: '90px',
			formatter: runningFormatter
		}, {
			field: 'SurveyID',
			title: 'Batch No',
			halign: 'center',
			align: 'left',
			width: '90px',
			sortable: true
		}, {
			field: 'VRNo',
			title: 'VR Number',
			halign: 'center',
			align: 'left',
			sortable: true
		}, {
			field: 'VRDate',
			title: 'VR Date',
			halign: 'right',
			align: 'right',
			width: '111px',
			formatter: 'dateFormatter',
			sortable: true
		}, {
			field: 'CommodityName',
			title: 'Commodity',
			sortable: true,
			visible: true
		}, {
			field: 'VONo',
			title: 'VONo',
			sortable: true,
			visible: true
		}, {
			field: 'RFIDate',
			title: 'RFI Date',
			halign: 'center',
			align: 'center',
			formatter: 'dateFormatter',
			sortable: true
		},
		{
			field: 'SurveyDate',
			title: 'Survey Date',
			halign: 'center',
			align: 'center',
			formatter: 'dateFormatter',
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

