
$(function () {
	enableLink(true);

	$('form#formExtend').submit(function () {
		var isValid = $('form#formExtend').valid();
		if (isValid == false) return isValid;

		enableLink(false);
		$('#progress').show();

		$.ajax({
			url: this.action,
			type: this.method,
			data: $(this).serialize(),
			success: function (result) {

				enableLink(true);

				if (result.Status == 0) {
					if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
					$('#progress').hide();
					$('#btnCancel').html('Close');
					$('#btnUpdate').html('License Updated', true)
					$('#btnUpdate').prop('disabled', true)
				}
				else {
					if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
					$('#progress').hide();
				}
			}
		});
		return false;
	});


	$('form#frmComment').submit(function () {
		var isValid = $('form#frmComment').valid();
		if (isValid == false) return isValid;

		enableLink(false);
		$('#progress').show();

		$.ajax({
			url: this.action,
			type: this.method,
			data: $(this).serialize(),
			success: function (result) {

				enableLink(true);

				if (result.Status == 0) {
					refreshComment();
					if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
					$('#progress').hide();
				}
				else {
					if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
					$('#progress').hide();
				}
			}
		});
		return false;
	});


	function refreshComment() {
		enableLink(false);
		$('#comment').val('');

		$.ajax({
			type: 'get',
			url: '/Imex/LicenseExtendCommentList',
			data: { id: $('#LicenseManagementID').val() },
			success: function (d) {
				if (d != null && d.Result.length>0) {
					var tbl = [];
					tbl.push('');

					$.each(d.Result, function (i, v) {
						tbl.push('<li class="'+ (i % 2 == 0 ? 'in' : 'out') + '">' +
								'<img class="avatar img-responsive" alt="">' + 
								'<div class="message">' +
								'	<span class="arrow"></span>' + v.StringId + '  -  ' + v.EntryBy +
								'	&nbsp;<span class="datetime"><small><i>'+v.DayDesc+ '</i></small></span>' +
								'	<span class="body">' + v.Comment + '</span>' +
								'</div>' +
							'</li>');
					});

					$('.chats').html(tbl.join(''));
				}
			},
			complete: function () { enableLink(true); },
			error: function (xhr, error, errorThrown) { }
		});
	};

	refreshComment();
});

