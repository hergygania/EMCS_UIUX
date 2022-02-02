'use strict';
var $table = $('#tableLeadTime');
var storeData = {};

$(function () {
    initTable();
    $('#addLeadTime').on('show.bs.modal', function () {
        $.ajax({
            type: 'GET',
            url: '/master/getStore/',
            datatype: 'json',
            success: function (data) {
                storeData = data;
            }
        }).then(appendModel);
    });

    $('#addLeadTime').on('hidden.bs.modal', clearForm);
    $('#editLeadTime').on('hidden.bs.modal', clearFormEdit);
    $('#appendLeadTime').on('click', appendModel);

    $("#saveLeadTime").submit(function (e) {
        e.preventDefault();
        var form = $('#saveLeadTime').serialize();
        $.ajax({
            type: 'POST',
            url: '/master/saveLeadTime',
            data: form,
            dataType: 'json',
            async: false,
            success: function (data) {
                if (data.result == 'success') {
                    swal({ title: 'Success', text: 'Data successfully ' + data.msg, html: true, timer: 2000, type: "success", showConfirmButton: false });
                } else {
                    swal({ title: 'Failed', text: 'Data failed to ' + data.msg, html: true, timer: 2000, type: "error", showConfirmButton: false });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                swal({ title: 'Error', text: jqXHR.status + " " + jqXHR.statusText, timer: 2000, type: "error", showConfirmButton: false });
            }
        }).then(function () {
            $table.bootstrapTable('refresh');
            $('#addLeadTime').modal('hide');
        });
    });

    $("#updateLeadTime").submit(function (e) {
        e.preventDefault();
        var form = $('#updateLeadTime').serialize();
        $.ajax({
            type: 'POST',
            url: '/master/updateLeadTime',
            data: form,
            dataType: 'json',
            async: false,
            success: function (data) {
                if (data.result == 'success') {
                    swal({ title: 'Success', text: 'Data successfully ' + data.msg, html: true, timer: 2000, type: "success", showConfirmButton: false });
                } else {
                    swal({ title: 'Failed', text: 'Data failed to ' + data.msg, html: true, timer: 2000, type: "error", showConfirmButton: false });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                swal({ title: 'Error', text: jqXHR.status + " " + jqXHR.statusText, timer: 2000, type: "error", showConfirmButton: false });
            }
        }).then(function () {
            $table.bootstrapTable('refresh');
            $('#editLeadTime').modal('hide');
        });
    });
});

function initTable() {
    $table.bootstrapTable({
        url: '/master/listLeadTime',
        cache: false,
        pagination: true,
        search: true,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        columns: [
            { field: '', title: '#', align: 'center', width: '70px', formatter: runningFormatter },
            { field: 'action', title: 'Action', width: '180px', align: 'center', formatter: operateFormatter, events: operateEvents, switchable: false },
    		{ field: 'STNAME', title: 'Store Name', halign: 'center', align: 'left', sortable: true, formatter: function storeName(value, row, index) { return row.STNO + ' - ' + row.STNAME; } },
		    {
		        field: 'LEADTIME', title: 'Lead Time RG', width: '140px', halign: 'center', align: 'right',
		        formatter: function leadTime(value, row, index) {
		            if (row.FILTERTYPE == 'Day') {
		                return parseInt(row.LEADTIME / 24) + ' ' + row.FILTERTYPE;
		            } else {
		                return row.LEADTIME + ' ' + row.FILTERTYPE;
		            }
		        }
		    },
            {
                field: 'PICKUPTIME1', title: 'Pick up Time 1', width: '140px', halign: 'center', align: 'right', formatter: function pickUpTime1(value, row, index) {
                    if (row.PICKUPTIME1 == '') {
                        return '-';
                    } else {
                        return row.PICKUPTIME1;
                    }
                }
            },
            {
                field: 'PICKUPTIME2', title: 'Pick up Time 2', width: '140px', halign: 'center', align: 'right', formatter: function pickUpTime2(value, row, index) {
                    if (row.PICKUPTIME2 == '') {
                        return '-';
                    } else {
                        return row.PICKUPTIME2;
                    }
                }
            },
            { field: 'ISACTIVE', title: 'Active', width: '100px', align: 'center', formatter: flagActive, events: operateActive }
        ]
    });
}

function flagActive(value, row, index) {
    if (row.ISACTIVE == true) {
        return '<a class="btn btn-success isActive" title="Active"><i class="glyphicon glyphicon-ok"></i></a>'
    } else {
        return '<a class="btn btn-danger isActive"><i class="glyphicon glyphicon-remove"></i></a>'
    }
}

window.operateActive = {
    'click .isActive': function (e, value, row, index) {
        var isActiveData = row.ISACTIVE;
        var msg = "";
        if (isActiveData == false) {
            msg = "Are you sure want to active this data?";
        } else {
            msg = "Are you sure want to inactive this data?";
        }
        swal({
            title: msg,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#F56954",
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            closeOnConfirm: true,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: 'POST',
                    url: '/master/setActiveLeadTime/',
                    data: {
                        "id": row.id,
                        "ISACTIVE": row.ISACTIVE
                    },
                    dataType: 'json',
                    async: false,
                    success: function (data) {
                        if (data.result = 'success')
                            $table.bootstrapTable('refresh');
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        swal({ title: 'Error', text: jqXHR.status + " " + jqXHR.statusText, timer: 2000, type: "error", showConfirmButton: false });
                    }
                });
            }
        });

        //$.ajax({
        //    type: 'POST',
        //    url: '/master/setActiveLeadTime/',
        //    data: {
        //        "id": row.id,
        //        "ISACTIVE": row.ISACTIVE
        //    },
        //    datatype: 'json',
        //    async: false,
        //    success: function (data) {
        //        if (data.result = 'success')
        //            $table.bootstrapTable('refresh');
        //    },
        //    error: function (jqXHR, textStatus, errorThrown) {
        //        sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        //    }
        //});
    }
};

function operateFormatter(value, row, index) {
    var btn = "";
    btn = '<div class="btn-group">' +
            '<button type="button" class="btn btn-info edit" title="Edit"><i class="fa fa-edit"></i> Edit</button>' +
            '<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash"></i> Delete</button>' +
    '</div>';
    return btn;
}

window.operateEvents = {
    'click .edit': function (e, value, row, index) {
        $("#editLeadTime").modal("show");
        $(".timepicker").timepicker({
            showInputs: false,
            defaultTime: ''
        });
        $('#storeID').append("<option value='" + row.STNO + "' selected='selected'>" + row.STNO + " - " + row.STNAME +"</opiton>");
        initStore(row.STNO, row.STNAME);
        if (row.FILTERTYPE == "Day") {
            $('#days').prop('checked', true);
        } else {
            $('#hours').prop('checked', true);
        }
        var newLeadTime;
        if (row.FILTERTYPE == 'Day') {
            newLeadTime = parseInt(row.LEADTIME / 24);
        } else {
            newLeadTime = row.LEADTIME;
        }
        $("#leadTime").val(newLeadTime);
        $("#pickUpTime1").val(row.PICKUPTIME1);
        $("#pickUpTime2").val(row.PICKUPTIME2);
        $("#id").val(row.id);
    },
    'click .remove': function (e, value, row, index) {
        deleteLeadTime(row.id);
    }
};

function initStore(STNO, STNAME) {
    $(".select2-editLeadTime").select2({
        placeholder: 'SELECT STORE',
        ajax: {
            url: '/master/getStoreID/',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                var queryParameters = {
                    stno: STNO,
                    term: params.term
                }
                return queryParameters;
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.STNAME,
                            id: item.STNO
                        }
                    })
                };
            },
            cache: false
        },
        initSelection: function (element, callback) {
            callback({ id: STNO, text: STNO + ' - ' + STNAME });
        }
    });
}

function deleteLeadTime(id) {
    swal({
        title: "Are you sure want to remove this data?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#F56954",
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: true,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            $.ajax({
                type: 'POST',
                url: '/master/deleteLeadTime',
                data: { "id": id },
                dataType: 'json',
                async: false,
                success: function (data) {
                    if (data.result == 'success') {
                        swal({ title: 'Success', text: 'Data successfully deleted. ' + data.msg, html: true, timer: 2000, type: "success", showConfirmButton: false });
                    } else {
                        swal({ title: 'Failed', text: 'failed to ' + data.msg, html: true, timer: 2000, type: "error", showConfirmButton: false });
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    swal({ title: 'Error', text: jqXHR.status + " " + jqXHR.statusText, timer: 2000, type: "error", showConfirmButton: false });
                }
            }).then(function () {
                $table.bootstrapTable('refresh');
                $('#editLeadTime').modal('hide');
            });
        }
    });
}

function newLeadTime() {
    $("#addLeadTime").modal("show");
}

function appendModel() {
    var model;
    var countLine = Object($('#leadTimeRows > tr')).length;
    $("#appendLeadTime").attr("disabled", "disabled");
    var no = parseFloat(countLine);
    model = '<tr id="tr' + no + '">';
    model += '<td style="text-align:center;padding:3px 8px;border:1px solid #ddd;width:50px;" id="no' + no + '">' + (no + 1) + '</td>';
    model += '<td style="text-align:center;padding:3px 8px;border:1px solid #ddd;width:70px;">';
        model += '<button type="button" class="btn btn-danger remove" id="remove'+no+'" title="Delete" onclick="deleteNewLeadTime(' + no + ');" ><i class="fa fa-trash-o"></i></button>';
    model += '</td>';
    model += '<td style="padding:3px 8px;border:1px solid #ddd;">';
        model += '<div class="form-group" style="margin-bottom:0px !important">';
          model += '<select class="select2-leadTime" name="[' + no + '].storeID" id="storeID' + no + '" style="width:100% !important;"></select>';
        model += '</div>';
    model += '</td>';
    model += '<td style="text-align:center;padding:3px 8px;border:1px solid #ddd;width:280px;">';
        model += '<div class="input-group" style="margin-bottom:0px !important">';
            model += '<div class="control-label col-lg-8 col-xs-8" style="margin-top:-7px">';
                model += '<div class="switch-toggle switch-2 well" style="margin-bottom:0px;height:30px;width:100%;">';
                    model += '<input id="hours' + no + '" name="[' + no + '].filter_type" type="radio" checked value="Hours">';
                    model += '<label for="hours' + no + '">Hours</label>';
                    model += '<input id="days' + no + '" name="[' + no + '].filter_type" type="radio" value="Day">';
                    model += '<label for="days' + no + '">Day</label>';
                    model += '<a class="btn btn-primary"></a>';
                model += '</div>';
            model += '</div>';
            model += '<div class="input-group col-lg-4 col-xs-4">';
                model += '<input type="text" class="form-control page" name="[' + no + '].leadTime" id="leadTime' + no + '" />';
            model += '</div>';
        model += '</div>';
    model += '</td>';
    model += '<td style="text-align:center;padding:3px 8px;border:1px solid #ddd;width:170px;">';
        model += '<div class="bootstrap-timepicker">';
            model += '<div class="input-group" style="margin-bottom:0px !important">';
                model += '<input type="text" class="form-control timepicker" name="[' + no + '].pickUpTime1" id="pickUpTime1' + no + '"/>';
                model += '<div class="input-group-addon"><i class="fa fa-clock-o"></i></div>';
            model += '</div>';
        model += '</div>';
    model += '</td>';
    model += '<td style="text-align:center;padding:3px 8px;border:1px solid #ddd;width:153px;">';
        model += '<div class="bootstrap-timepicker">';
            model += '<div class="input-group" style="margin-bottom:0px !important">';
                model += '<input type="text" class="form-control timepicker" name="[' + no + '].pickUpTime2" id="pickUpTime2' + no + '"/>';
                model += '<div class="input-group-addon"><i class="fa fa-clock-o"></i></div>';
            model += '</div>';
        model += '</div>';
    model += '</td>';
    model += '</tr>';
//    $('#modelRows').prepend(model);
    $('#leadTimeRows').append(model);
    var con = document.getElementById("boxpesan");
    con.scrollTop = con.scrollHeight;

    $('.select2-leadTime').select2({
        data: storeData,
        placeholder: 'SELECT STORE'
    });
    $('#storeID' + no).val('val', '').change();
    $(".timepicker").timepicker({
        showInputs: false
//        defaultTime: false
    });
    $("#leadTime" + no).val("");
    $("#pickUpTime1" + no).val("");
    $("#pickUpTime2" + no).val("");
    if (countLine == 0) {
        $("#remove0").attr("disabled", "disabled");
        //$("#appendLeadTime").attr("disabled", "disabled");
        //$("#btnSaveLead").attr("disabled", "disabled");
    } else {
        $("#remove0").removeAttr("disabled");
        //$("#appendLeadTime").removeAttr("disabled");
        //$("#btnSaveLead").removeAttr("disabled");
    }
    $('#storeID' + no).on('change', function () {
        formValidation(no);
    });

    $('#leadTime' + no).on('change', function () {
        formValidation(no);
    });
}

function cekInput(no) {
    var leadTime = $("#leadTime" + no).val();
    var newLeadTime;
    if (leadTime >= 24) {
        $('#days' + no).prop('checked', true);
        newLeadTime = parseFloat(leadTime / 24).toFixed(2);
//        console.log(newLeadTime)
        $("#leadTime" + no).val(newLeadTime);
    } else {
        $('#hours' + no).prop('checked', true);
    }
//    alert(leadTime);
}

function validateForm() {
    $('#leadTimeRows').on('change', validateSouvenir);
}

function formValidation( no ) {
    var stno = $('#storeID' + no).val();
    var leadTime = $('#leadTime' + no).val();
    if (stno != '') {
        $("#appendLeadTime").removeAttr("disabled");
//        $("#btnSaveLead").removeAttr("disabled");
    }
}

function deleteNewLeadTime(index) {
    swal({
        title: "Are you sure want to remove this data?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#F56954",
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: true,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            var countLine = Object($('#leadTimeRows > tr')).length - 1;
            for (var i = index ; i < countLine  ; i++) {
                var no = parseFloat(i) + 1;
                var storeID = $("#storeID" + no).val();
                var leadTime = $("#leadTime" + no).val();
                var pickUpTime1 = $("#pickUpTime1" + no).val();
                var pickUpTime2 = $("#pickUpTime2" + no).val();

                $("#no" + i).html(i + 1);
                $("#storeID" + i).val(storeID).trigger('change');
                $("#leadTime" + i).val(leadTime);
                $("#pickUpTime1" + i).val(pickUpTime1);
                $("#pickUpTime2" + i).val(pickUpTime2);
            }
            $("#tr" + countLine).fadeOut('fast', function () {
                $("#tr" + countLine).remove();
            });
        }
    });
}

function clearForm() {
    var countLine = Object($('#leadTimeRows tr')).length;
    for (var i = 0; i < countLine; i++) {
        $("#tr" + i).remove();
    }
    storeData = null;
}

function clearFormEdit() {
    $('form')[1].reset();
    $("#storeID").val("", "").trigger("change");
}