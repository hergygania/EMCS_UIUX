var $table = $('#tblHs');
var $AllowDelete = $('#AllowDelete').val();
var $AllowUpdate = $('#AllowUpdate').val();
$(function () {
	enableLink(false);
	//helpers.buildDropdown('/Picker/GetListRegulation', $('#selRegulation'), true, null);
	helpers.buildDropdown('/Picker/GetListRegulationIssue', $('#selIssueBy'), true, null);
	helpers.buildDropdown('/Picker/GetListLartas', $('#selLartas'), true, null);
    helpers.buildDropdown('/Picker/GetListOMCode', $('#selOrderMethods'), true, null);
	//helpers.buildDropdown('/Picker/GetListHSCodeId', $('#selHSCode'), true, null);
	
	var regulation = new mySelect2({
		id: 'selRegulation',
		url: '/Picker/Select2Regulation',
		minimumInputLength: 1
	}).load();

	var hsCode = new mySelect2({
		id: 'selHSCode',
        url: '/Picker/Select2HsCodeName'
	}).load();
    
	$('.cal').click(function () {
	    $('#IssuedDateSta').datepicker('show');
	});

	$('.cal1').click(function () {
	    $('#IssuedDateFin').datepicker('show');
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
		columns: [
        //    {
		//	field: 'action',
		//	title: 'Action',
		//	width: '70px',
		//	align: 'center',
		//	formatter: operateFormatter({ Edit: Boolean($AllowUpdate)}),
		//	events: operateEvents,
		//	class: 'noExl',
		//	switchable: false
		//},
        {
			field: 'HSCode',
			title: 'HS Code',
			halign: 'center',
			align: 'left',
			sortable: true
		}, {
			field: 'HSDescription',
			title: 'HS Description',
			halign: 'center',
			align: 'left',
            width : '70%',
			sortable: true
		},
        {
            field: 'BeaMasuk',
            title: 'Bea Masuk (Duty)',
            halign: 'center',
            align: 'right',
            sortable: true
        },
		//{
		//	field: 'LicenseNumber',
		//	title: 'License',
		//	halign: 'center',
		//	align: 'left',
		//	sortable: true
		//},
		//{
		//	field: 'LartasDesc',
		//	title: 'Lartas',
		//	halign: 'center',
		//	align: 'left',
		//	sortable: true
		//},
		//{
		//	field: 'QtyOfParts',
		//	title: '∑ Parts',
		//	halign: 'center',
		//	align: 'right',
		//	sortable: true
		//},
		{
			field: 'OMCode',
			title: 'OM',
			halign: 'center',
			align: 'center',
			sortable: true
		},
		{
			field: 'Regulation',
			title: 'Regulation',
			halign: 'center',
			align: 'left',
			sortable: true,
			visible: false
		},{
			field: 'ModifiedBy',
			title: 'ModifiedBy',
			halign: 'left',
			align: 'left',
			sortable: true,
			visible: false
		}, {
			field: 'ModifiedDate',
			title: 'ModifiedDate',
			halign: 'left',
			align: 'left',
			sortable: true,
			formatter: 'dateFormatter',
			visible: false
		}, {
			field: 'DetailID',
			title: 'Id',
			sortable: true,
			visible: false
		}]
	});

	$('#btn-clear').click(function () {
	    $('#selRegulation').val('val', '').change();
	    $('#selIssueBy').val('val', '').change();
	    //$('#IssuedDateSta').val('');
	    //$('#IssuedDateFin').val('');
	    $('#selHSCode').val('val', '').change();
	    //$('#selLartas').val('val', '').change();
	    $('#selOrderMethods').val('val', '').change();
	    $('#HSDescription').val('');
	});

	$("#btnFilter").click(function () {
		//if ($('#IssuedDateSta').val() != '' && $('#IssuedDateFin').val() != '') {
		//	$.validator.addMethod("greaterThan",
		//		function (value, element, params) {
		//			var prm = $('#' + params).val();

		//			if (!/Invalid|NaN/.test(new Date(value))) {
		//				return new Date(value) >= new Date(prm);
		//			}
		//			return isNaN(value) && isNaN(prm) || (Number(value) > Number(prm));
		//		}, 'Issued Finish must be greater than Issued Start.');
		//	$("#IssuedDateFin").rules('add', { greaterThan: "IssuedDateSta" });
		//}

		//if (!$("form#frmSrc").valid()) { $("#IssuedDateFin").rules('remove'); return; }

		//if (($('#IssuedDateSta').val() != '' && $('#IssuedDateFin').val() == '') || $('#IssuedDateSta').val() == '' && $('#IssuedDateFin').val() != '') {
		//	$('#IssuedDateSta').val('');
		//	$('#IssuedDateFin').val('');
		//}

		$('#divResult').hide();
		$('#uploadResult').hide();
		$('#uploadResult').empty();

		window.pis.table({
			objTable: $table,
			urlSearch: '/imex/HsRegulationMappingPage',
			urlPaging: '/imex/HsRegulationMappingPageXt',
			searchParams: {
				//selRegulation: $('#selRegulation').val(),
				selIssueBy: $('#selIssueBy').val(),
				//IssuedDateSta: $('#IssuedDateSta').val(),
				//IssuedDateFin: $('#IssuedDateFin').val(),
				selHSCode: $('#selHSCode').val(),
				HSDescription: $('#HSDescription').val(),
				//Status: $('#Status').val(),
				//selLartas: $('#selLartas').val(),
				selOrderMethods: $('#selOrderMethods').val()
			},
			autoLoad: true
		});
	});

	$(".downloadExcel").click(function () {
		//$(".table2excel").table2excel({
		//	exclude: ".noExl",
		//	filename: "HsRegulation.xls"
		//});
        enableLink(false);
        $.ajax({
            url: "DownloadHSRegulationToExcel",
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

	$(".table2excel2").click(function () { $(".table2excel2").table2excel({ filename: "unMapping.xls" }); });

	$("#mySearch").insertBefore($("[name=refresh]"))
	enableLink(true);
});


function operateFormatter(value, row, index) {
	return [
			'<div style="width:70px;white-space:nowrap;text-align:center">',
					'<button type="button" class="btn btn-xs btn-primary edit" title="Edit"><i class="fa fa-pencil"></i> Edit</button>',
			'</div>'
	].join('');
}

function operateFormatter(options) {
    var btn = [];

    btn.push('<div style="width:70px;white-space:nowrap;text-align:center">');
    if (options.Add == true)
        btn.push('<button type="button" class="btn btn-xs btn-danger extend" title="Extend"><i class="fa fa-check-circle"></i> Extend</button>')
    if (options.Edit == true)
        btn.push('<button type="button" class="btn btn-xs btn-primary edit" title="Edit"><i class="fa fa-pencil"></i> Edit</button>');
    if (options.Delete == true)
        btn.push('<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    if (options.Info == true)
        btn.push('<button type="button" class="btn btn-xs btn-info detail" title="Detail"><i class="fa fa-search-plus"></i> Detail</button>')

    btn.push('</div>');

    return btn.join('');
}

window.operateEvents = {
	'click .edit': function (e, value, row, index) {
		loadModal('/imex/RegulationManagDetailEdit?id=' + row.DetailID);
	}
};



$(function () {
	$.ajaxSetup({ cache: false });

	$("#myModalPlace").on('hide.bs.modal', function () {
		//recall select2 ajax after return from modal (bugs)
		var _hsCode = new mySelect2({ id: 'selHSCode', url: '/Picker/Select2HsCode' }).load();
		var _regulation = new mySelect2({ id: 'selRegulation', url: '/Picker/Select2Regulation', minimumInputLength: 1 }).load();
	});

	$("a[data-modal]").on("click", function (e) {
		enableLink(false);

		$('#myModalContent').load(this.href, function () {

			$('#myModalPlace').modal({ keyboard: true }, 'show');

			bindForm(this);

			enableLink(true);
		});
		return false;
	});


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
					if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
					$('#myModalPlace').modal('hide');
					$('#progress').hide();
					$("#btnFilter").trigger('click');
				}
				else {
					if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
					$('#progress').hide();
					//$('#myModalContent').html(result);
					//bindForm(dialog);
				}
			}
		});
		return false;
	});
};

$("form#submitExcel").submit(function () {
	enableLink(false);
	var dt = { "rows": {}, "total": 0 };
	$table.bootstrapTable('load', dt);
	$('#uploadResult').empty();

	var formData = new FormData($(this)[0]);

	$.ajax({
		url: $(this).attr("action"),
		type: 'POST',
		data: formData,
		async: false,
		success: function (result) {
			enableLink(true);

			if (result.Status == 0) {
				if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
				$("#btnFilter").trigger('click');
			}
			else {
				if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
				if (result.Data != undefined) {
					$('#uploadResult').html(result.Data);
					$('#uploadResult').show();
					$('#divResult').show();
				}
			}

		},
		cache: false,
		contentType: false,
		processData: false
	});

	return false;
});

function detailFormatter(index, row) {
	enableLink(false);
	var html = [];
	html.push('<div id="_child' + index + '" class="bootstrap-table fixed-table-body"></div>')

	$.ajax({
		type: 'POST',
		//url: '/Imex/RegulationList',
        url: '/Imex/RegulationListNew',
		//data: { id: row.RegulationManagementID, hsid: row.HSID },
        data: { HSCode: row.HSCode },
		success: function (d) {
		    if (d != null) {
				var tbl = [];
				tbl.push('<table style="width:100%;" class="table table-bordered table-hover table-striped">' +
				'<thead><tr style="text-align:center;font-weight: bold;">' +
				' <td>Regulation </td>' +
				' <td>Description </td>' +
				' <td>Issued By </td>' +
				//' <td>Issued Date </td>' +
				' <td>Default OM </td>' +
				'</tr></thead><tbody>');

				if (d.Result != undefined) {
				    $.each(d.Result, function (i, v) {
				        tbl.push('<tr>' +
                            '<td>' + v.CodePermitCategory + ' - ' + v.PermitCategoryName + '</td>' +
                            '<td>' + v.Description + '</td>' +
                            '<td>' + v.Permit + '</td>' +
                            //'<td style="text-align:right;">' + dateFormatter(v.IssuedDate) + '</td>' +
                            '<td>' + v.OMCode || "-" + '</td>' +
                            '</tr>');
				    });
				} else {
				    tbl.push('<tr class="no-records-found"><td colspan="4"><span class="noMatches">No data found</span></td></tr>');
				}

				tbl.push('</tbody></table>');
				$('#_child' + index + '').html(tbl.join(''));
			}
		},
		complete: function () { enableLink(true); },
		error: function (xhr, error, errorThrown) { }
	});

	return html.join('');
}

