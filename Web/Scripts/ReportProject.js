$(function() {
	initTable();
	userList();
	$("textarea").wysihtml5({lists:false,image:false,link:false});

	$('.cal1').click(function () {$('#startPlannedDate').focus();});
	$('.cal2').click(function () {$('#endPlannedDates').focus();});
	$('.cal3').click(function () {$('#startActualDate').focus();});
	$('.cal4').click(function () {$('#endActualDate').focus();});

	$("#updateProgress").bootstrapValidator({
		excluded: [':disabled'],
		fields: {
			projectName: {
				validators: {
					notEmpty: {
						message: 'Project Name field is required'
					}
				}
			},
			startDate: {
				validators: {
					notEmpty: {
						message: 'Start Date field is required'
					}
				}
			}
		}
	}).on('success.form.bv', function (e) {
		e.preventDefault();
		var form = $('#updateProgress').serialize();
		showLoading();
		$.ajax({
			type 		: 'POST',
			url 		: 'Pages/ReportProject/ReportProject.php?task=updateProject',
			data 		: form,
			dataType	: 'json',
			async		: false,
			success: function (data) {
				closeLoading();
				if (data.result == 'true') {
					swal({title:'Success', text:'Project Name <b>' + $("#projectNames").val() + '</b> successfully ' + data.msg, html: true, timer:2000, type:"success", showConfirmButton:false});
				} else {
					swal({title:'Failed', text:'Project Name <b>' + $("#projectNames").val() + '</b> failed to ' + data.msg, html: true, timer:2000, type:"error", showConfirmButton:false});
				}
			},
			error: function (jqXHR, textStatus, errorThrown) {
				closeLoading();
				swal({title:'Error', text:jqXHR.status + " " + jqXHR.statusText, timer:2000, type:"error", showConfirmButton:false});
			}
		}).then(function () {
			$("#reportProject").bootstrapTable('refresh');
			$('#update-progress').modal('hide');
		});
	});

	$("#saveAssignTo").bootstrapValidator({
		fields: {
			assignToPM: {
				validators: {
					notEmpty: {
						message: 'Project Manager field is required'
					}
				}
			}
		}
	}).on('success.form.bv', function (e) {
		e.preventDefault();
		submitAssign();
	});
});

function userList() {
    $("#empName").select2({
        minimumInputLength:0,
        ajax: {
            url			: "library/_ListUser.inc.php",
            dataType	: 'json',
            delay		: 250,
            data		: function (params) {
				var queryParameters = {
					term: params.term
				}
				return queryParameters;
            },
            processResults: function (data) {
				return {
					results: $.map(data, function (item) {
						return {
							text		: item.text,
							id			: item.id
						}
					})
				};
            },
            cache: true
		}
    });
}

var $table = $('#reportProject');
function initTable() {
	$table.bootstrapTable({
		method			: 'GET',
		url				: 'Pages/ReportProject/ReportProject.php',
		cache			: false,
		pagination		: true,
		search			: true,
		striped			: true,
		clickToSelect	: true,
		onClickRow		: selectRow,
		toolbarAlign	: 'left',
		sidePagination	: 'server',
		showColumns		: true,
		showRefresh		: true,
//		showAdvSearch	: true,
		smartDisplay	: false,
		detailView		: true,
		onExpandRow		: function (index, row, $detail) {expandTable($detail, row);},
		columns			: [
			{field:null, title:'#', width:'40px', align:'center', formatter:runningFormatter, switchable:false},
			{field:'action', title:'Action', width:'120px', align:'center', formatter:operateFormatter, events:operateEvents, switchable:false},
			{field:'projectNo', title:'Project No', width:'100px', halign:'center', align:'left', sortable:true},
			{field:'projectName', title:'Project Name', width:'170px', halign:'center', align:'left', sortable:true},
			{field:'name', title:'PIC', width:'120px', halign:'center', align:'left', sortable:true},
			{field:'plannedStartDate',title:'Planned Start Date<br>Actual Start Date',width:'150px',halign:'center',align:'left',sortable:true,formatter:startDates},
			{field:'plannedEndDate',title:'Planned End Date<br>Actual End Date',width:'140px',halign:'center',align:'left',sortable:true,formatter:endDates},
			{field:'statusProject',title:'Status',width:'90px',halign:'center',align:'left',sortable:true,formatter:formatText},
			{field:'progressName',title:'Progress',width:'110px',halign:'center',align:'left',sortable:true},
			{field:'description',title:'Description',halign:'center',align:'left',sortable:true}
		]
	});
}

function expandTable($detail, row) {
	reportID = row.reportID;
	buildTable($detail.html('<table></table>').find('table'), reportID);
}

function buildTable($ext, reportID) {
	$ext.bootstrapTable({
		method			: 'GET',
		url				: 'Pages/ReportProject/ReportProject.php?task=expandTable&reportID='+reportID,
		onClickRow: selectRow,
		showColumns: false,
		columns 		: [
			{field:'progressName',title:'Progress',width:'230px',halign:'center',align:'left'},
			{field:'plannedStartDate',title:'Planned Start Date<br>Actual Start Date',width:'130px',halign:'center',align:'left',formatter:startDates},
			{field:'plannedEndDate',title:'Planned End Date<br>Actual End Date',width:'130px',halign:'center',align:'left',formatter:endDates},
			{field:'statusProject',title:'Status',width:'120px',halign:'center',align:'right',formatter:formatText},
			{field:'description',title:'Description',width:'250px',halign:'center',align:'left'},
			{field:'attachmentFile',title:'Document',halign:'center',align:'left',formatter:formatAttachment}
		]
	});
}

function formatAttachment(value, row, index) {
	var attachFormat;
	if ( row.attachmentFile == '' ) {
		attachFormat = '-';
	} else {
		attachFormat = '<a href="attachFile/'+row.users+'/'+row.attachmentFile+'" target="_blank">'+row.attachmentFile+'</a>';
	}
	return	attachFormat;
}

function changeFormat(value, row, index) {
	var endDate;
	if( row.endDate == '1900-01-01' ) {
		endDate = '-';
	} else {
		endDate = row.endDate
	}
	return endDate;
}

function formatText (value, row, index) { 
	var progress;
	if( row.statusProject == null ) {
		progress = 0;
	} else {
		progress = row.statusProject;		
	}
	return [
		'<div class="progress">',
			'<div class="progress-bar progress-bar-aqua"',
				' role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100"',
				' style="width: ' + progress + '%">',
				'<span> ' + progress + '%</span>',
			'</div>',
		'</div>'
	].join('');
}

function startDates(value, row, index) {
	return row.plannedStartDate + '<br>' + row.actualStartDate;
}

function endDates(value, row, index) {
	return row.plannedEndDate + '<br>' + row.actualEndDate;
}

function operateFormatter(value, row, index) {
	return [
		'<div class="btn-group">',
			'<button type="button" class="btn btn-info edit" data-toggle="tooltip" data-placement="right" title="Update Report"><i class="fa fa-edit"></i></button>',
			'<button type="button" class="btn btn-success assign" data-toggle="tooltip" data-placement="right" title="Assign To"><i class="fa fa-exchange"></i></button>',
		'</div>'
	].join('');
}

window.operateEvents = {
	'click .edit': function (e, value, row, index) {
		var endDate;
		if( row.attachmentFile == ' ' ) {$('.doc').hide();}
		else {$('.doc').show();}
		$("#update-progress").modal("show");
		$('#projectName').val( row.projectName );
		$('#startPlannedDate').val( row.plannedStartDate );
		$('#endPlannedDate').val( row.plannedEndDate );
		$('#startActualDate').val( row.actualStartDate );
		$('#endActualDate').val( row.actualEndDate );
		intiSelectEdit(row.progressID, row.progressName, row.orderingNo);
		$("#progressProject").append("<option value='"+row.progressID+"' selected='selected'>"+row.progressName+"</opiton>");
		$('#statusProject').val( row.statusProject );
		$('#statusProject').spinedit({maximum:100,minimum:row.statusProject,step:5});
		$('#description').data("wysihtml5").editor.setValue( row.description );
		$('#attachmentFile').html( '<a href="attachFile/'+row.attachmentFile+'" target="_blank">'+row.attachmentFile+'</a>' );		
		startDate();
		$('#reportID').val( row.reportID );
		$('#statusProgress').val( row.progressID );
/*
		if( row.statusProject >= 100 ) {
			$('#startPlannedDates').attr( 'disabled','disabled' );
			$('#endPlannedDates').attr( 'disabled','disabled' );
			$('#startActualDates').attr( 'disabled','disabled' );
			$('#endActualDates').attr( 'disabled','disabled' );
			$('#statusProjects').attr( 'disabled','disabled' );
			$('#progressProjects').attr( 'disabled','disabled' );
			$('#descriptions').attr( 'disabled','disabled' );
			$('#btnUpdate').attr( 'disabled','disabled' );
		} else {
			$('#startPlannedDates').removeAttr( 'disabled' );
			$('#endPlannedDates').removeAttr( 'disabled' );
			$('#startActualDates').removeAttr( 'disabled' );
			$('#endActualDates').removeAttr( 'disabled' );
			$('#statusProjects').removeAttr( 'disabled' );
			$('#progressProjects').removeAttr( 'disabled' );
			$('#descriptions').removeAttr( 'disabled' );
			$('#btnUpdate').removeAttr( 'disabled' );
		}
*/
	},
	'click .assign': function (e, value, row, index) {
		$("#assignTo").modal("show");
		intiPM();
		$('#projectNoS').val( row.projectNo );
	}
};

function intiPM() {
    $("#assignToPM").select2({
        minimumInputLength:0,
        ajax: {
            url			: "library/_ListPm.inc.php?task=PM",
            dataType	: 'json',
            delay		: 250,
            data		: function (params) {
				var queryParameters = {
					term: params.term
				}
				return queryParameters;
            },
            processResults: function (data) {
				return {
					results: $.map(data, function (item) {
						return {
							text		: item.text,
							id			: item.id
						}
					})
				};
            },
            cache: true
		}
    });
}

function intiSelectEdit(progressID, progressName, orderingNo) {
    $("#progressProject").select2({
        minimumInputLength:0,
        ajax: {
            url			: "library/_ListProgres.inc.php?task=edit&orderingNo="+orderingNo,
            dataType	: 'json',
            delay		: 250,
            data		: function (params) {
				var queryParameters = {
					term: params.term
				}
				return queryParameters;
            },
            processResults: function (data) {
				return {
					results: $.map(data, function (item) {
						return {
							text		: item.progressName,
							id			: item.id
						}
					})
				};
            },
            cache: true
        },
		initSelection: function(element, callback) {
			callback({id: progressID, text: progressName });
		}
    });
}

function startDate() {
	$('input[name="startPlannedDate"]').daterangepicker({
        singleDatePicker: true,
        format: "YYYY-MM-DD"
	});
	$('input[name="endPlannedDate"]').daterangepicker({
        singleDatePicker: true,
        format: "YYYY-MM-DD"
	});
	$('input[name="startActualDate"]').daterangepicker({
        singleDatePicker: true,
        format: "YYYY-MM-DD"
	});
	$('input[name="endActualDate"]').daterangepicker({
        singleDatePicker: true,
        format: "YYYY-MM-DD"
	});
}

function submitAssign() {
	var formData = $('#saveAssignTo').serialize();
	$.ajax({
		type 		: 'POST',
		url 		: 'Pages/ReportProject/ReportProject.php?task=saveAssignTo',
		data 		: formData,
		dataType	: 'json',
		async		: false,
		success 	: function (data) {
			closeLoading();
			if (data.result == 'true') {
				swal({title:'Success', text:'Request No <b>' + data.requestNo + '</b> successfully assign to ' + data.user , html: true, timer:2000, type:"success", showConfirmButton:false});
			} else {
				swal({title:'Failed', text:'Request No <b>' + data.requestNo + '</b> failed assign to ' + data.user , html: true, timer:2000, type:"error", showConfirmButton:false});
			}
		},
		error: function (jqXHR, textStatus, errorThrown) {
			closeLoading();
			swal({title:'Error', text:jqXHR.status + " " + jqXHR.statusText, timer:2000, type:"error", showConfirmButton:false});
		}
	}).then(function () {
		$table.bootstrapTable('refresh');
		$('#assignTo').modal('hide');
		$('#detailRequest').modal('hide');
	});
}