var $table = $('#tableLogImportFCC');

$(function () {
	$table.bootstrapTable({
		cache: false,
		pagination: true,
		search: false,
		striped: true,
		clickToSelect: true,
		reorderableColumns: true,
		onClickRow: selectRow,
		sidePagination: 'server',
		showColumns: true,
		showRefresh: true,
		smartDisplay: false,
		pageSize: '5',
		formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
		columns: [
            { field: 'FileName', title: 'File Name', halign: 'center', width: '400px', align: 'left', formatter: 'linkFormatter', sortable: true },
            { field: 'EntryDate', title: 'Upload Date', width: '200px', halign: 'center', align: 'right', formatter: dateFormatter, sortable: true },
            { field: 'EntryBy', title: 'Upload By', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, },
            { field: 'Status', title: 'Status', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, }
		]
	});

	window.pis.table({
		objTable: $table,
		urlSearch: '/Master/IndexPageLogImport',
		urlPaging: '/Master/GetLogImportFCC',
		autoLoad: true
	});
	$("#mySearch").insertBefore($("[name=refresh]"));
});

function linkFormatter(value, row, index) {
    return "<a href='" + row.URL + "'>" + value + "</a>";
}

$(function () {
	$.ajaxSetup({ cache: false });
	$("a[data-modal]").on("click", function (e) {
		$('#myModalContent').load(this.href, function () {
			$('#myModalPlace').modal({
				keyboard: true
			}, 'show');
			bindForm(this);
		});
		return false;
	});
});

function bindForm(dialog) {
	$('form', dialog).submit(function () {
		$('#progress').show();
		$.ajax({
			url: this.action,
			type: this.method,
			data: $(this).serialize(),
			success: function (result) {

				if (result.Status == 0) {
					if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
					$('#myModalPlace').modal('hide');
					$('#progress').hide();
					$("[name=refresh]").trigger('click');
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
		async: true,
		success: function (result) {
			enableLink(true);

			if (result.Status == 0) {
				if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
				$("[name=refresh]").trigger('click');
			}
			else {
				if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
				if (result.Data != undefined) {
					$('#uploadResult').html(result.Data);
					$('#uploadResult').show();
					$('#divResult').show();
				}
				$("[name=refresh]").trigger('click');
			}
			$("#filexls").val("");
		},
		cache: false,
		contentType: false,
		processData: false
	});

	return false;
});