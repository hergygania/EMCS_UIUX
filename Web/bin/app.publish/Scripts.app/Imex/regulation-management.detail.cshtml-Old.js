
window.operateEvents = {
	'click .edit': function (e, value, row, index) {
		loadModal('/imex/RegulationManagDetailEdit?id=' + row.DetailID);
	},
	'click .remove': function (e, value, row, index) {
		swal({
			title: "Are you sure want to delete this data?",
			type: "warning",
			showCancelButton: true,
			confirmButtonColor: "#F56954",
			confirmButtonText: "Yes",
			cancelButtonText: "No",
			closeOnConfirm: false,
			closeOnCancel: true
		}, function (isConfirm) {
			if (isConfirm) {
				sweetAlert.close();
				return deleteThis(row.DetailID);
			}
		});
	}
};

$(function () {
	var $tableDetail = $('#tblDetail');
	enableLink(false);

	refreshDetail = function () {
		$('#divResult2').hide();
		$('#uploadResult2').hide();
		$('#uploadResult2').empty();

		enableLink(false);

		window.pis.table({
			objTable: $tableDetail,
			urlSearch: '/imex/RegulationManagDetailPage',
			urlPaging: '/imex/RegulationManagDetailPageXt',
			searchParams: {
				RegulationManagementID: regId
			},
			autoLoad: true
		});
	}

	$tableDetail.bootstrapTable({
		cache: false,
		pagination: true,
		search: false,
		striped: true,
		clickToSelect: true,
		reorderableColumns: true,
		toolbar: '.toolbarDet',
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
			width: '125px',
			align: 'center',
			formatter: operateFormatter,
			events: operateEvents,
			class: 'noExl',
			switchable: false
		}, {
			field: 'Regulation',
			title: 'Regulation',
			halign: 'center',
			align: 'left',
			sortable: true,
			visible: false
		}, {
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
			sortable: true
		},
		{
			field: 'LartasDesc',
			title: 'Lartas',
			halign: 'center',
			align: 'left',
			sortable: true
		},
		{
			field: 'LicenseNumber',
			title: 'License',
			halign: 'center',
			align: 'left',
			sortable: true
		}, {
			field: 'QtyOfParts',
			title: '∑ Parts',
			halign: 'center',
			align: 'right',
			sortable: true
		}, {
			field: 'OMCode',
			title: 'OM',
			halign: 'center',
			align: 'center',
			sortable: true
		}, {
			field: 'Status',
			title: '<div style="white-space:nowrap;">Status</div>',
			halign: 'center',
			align: 'center',
			sortable: true,
			formatter: statusFormatter
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
			field: 'RegulationManagementID',
			title: 'RegulationId',
			sortable: true,
			visible: false
		}, {
			field: 'DetailID',
			title: 'Id',
			sortable: true,
			visible: false
		}]
	});

	refreshDetail();

	$(".download2Excel").click(function () {
		$(".table2excel2").table2excel({
			exclude: ".noExl",
			filename: "regulationDet.xls"
		});
	});

	$(".downloadExcel3").click(function () { $(".table2excel3").table2excel({ filename: "unMapping.xls" }); });

	$("#mySearch").insertBefore($("[name=refresh]"))
});


function operateFormatter(value, row, index) {
	return [
			'<div class="btn-group" style="width:123px;white-space:nowrap; text-align:center">',
					'<button type="button" class="btn btn-xs btn-primary edit" title="Edit"><i class="fa fa-pencil"></i> Edit</button>',
					'<button type="button" class="btn btn-xs btn-danger remove" title="Remove"><i class="fa fa-times"></i> Remove</button>',
			'</div>'
	].join('');
}


function deleteThis(id) {
	$.ajax({
		type: "POST",
		url: myApp.root + 'imex/RegulationManagDetailDeleteId',
		beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
		complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
		data: { id: id },
		dataType: "json",
		success: function (d) {
			if (d.Msg != undefined) {
				sAlert('Success', d.Msg, 'success');
			}

			refreshDetail();
		},
		error: function (jqXHR, textStatus, errorThrown) {
			sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
		}
	});

};


//$(function () {
//	$.ajaxSetup({ cache: false });
//	$("a[data-modal]").on("click", function (e) {
//		enableLink(false);
//		$('#myModalContent').load(this.href, function () {
//			$('#myModalPlace').modal({ keyboard: true }, 'show');
//			bindForm(this);
//			enableLink(true);
//		});
//		return false;
//	});
//});

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
					refreshDetail();
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


$("form#submit2Excel").submit(function () {
	enableLink(false);
	$('#uploadResult2').empty();

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
				refreshDetail();
			}
			else {
				if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
				if (result.Data != undefined) {
					$('#uploadResult2').html(result.Data);
					$('#uploadResult2').show();
					$('#divResult2').show();
				}
			}

		},
		cache: false,
		contentType: false,
		processData: false
	});

	return false;
});

function cancelDetail() {
	$("#detail").empty();
	$("#detail").hide();
	$("#parent").show();
}
