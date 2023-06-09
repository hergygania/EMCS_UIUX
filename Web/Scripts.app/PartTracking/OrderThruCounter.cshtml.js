var $table = $('#table-order');
var $AllowUpdate = $('#AllowUpdate').val();
setTimeout(function () { $(".model-parent").find(".select2").addClass("width30"); }, 300);

$(function () {
    reloadScripts("OrderThruCounter.cshtml.js", "token")
    clearScreen();
    $('#doc_date_s').removeAttr("disabled");
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');

    $('#order_status').val('all').change();

    $('#doc_date_s').daterangepicker({
        format: 'DD MMM YYYY',
        autoUpdateInput: true,
        "showDropdowns": true,
        "startDate": moment(),
        "endDate": moment()
    },
    function (start, end, label) {
        $('#doc_date_start').val(start.format('YYYY-MM-DD'));
        $('#doc_date_end').val(end.format('YYYY-MM-DD'));
    });

    $("#doc_date_s").change(function (e) {
        $('#doc_date_s').val('');
        $('#doc_date_start').val('');
        $('#doc_date_end').val('');
    });

    $('.cal').click(function () {
        $('#doc_date_s').focus();
    });

    $("#cust_po_no").on("keyup", function () {
        disableDate();
    });

    $("#ref_doc_no").on("keyup", function () {
        disableDate();
    });

    $("#wo_number").on("keyup", function () {
        disableDate();
    });

    $("#equip_number").on("keyup", function () {
        disableDate();
    });

    $("#part_number").on("keyup", function () {
        disableDate();
    });

    $("#part_desc").on("keyup", function () {
        disableDate();
    });

    $("#model").on("keyup", function () {
        disableDate();
    });

    $("#serial").on("keyup", function () {
        disableDate();
    });

    //$("#order_status").change(function () {
    //    var orderStatus = $("#order_status").val();
    //    var defaultCheck = $('input[name="defaultDate"]:checked').val();
    //    if (orderStatus != '' && orderStatus != 'Order Completed') {
    //        $('#on').removeAttr("disabled");
    //        $('#off').removeAttr("disabled");
    //    } else {
    //        $('#on').attr("disabled", "disabled");
    //        $('#off').attr("disabled", "disabled");
    //        $('#doc_date_s').removeAttr("disabled");
    //        $('#on').prop('checked', true);
    //    }
    //});

    $('input[name="defaultDate"]').on('change', function () {
        if ($('input[name="defaultDate"]:checked').val() == 'ON') {
            $('#doc_date_s').removeAttr("disabled");
        } else {
            $('#doc_date_s').attr("disabled", "disabled");
        }
    });

    $("#cust_po_date_s").change(function (e) {
        $('#cust_po_date_s').val('');
        $('#cust_po_date_start').val('');
        $('#cust_po_date_end').val('');
    });

    $("#filter_by").change(function (e) {
        enableLink(false);
        var val = $(this).val();
        $.getJSON("/partTracking/GetStore",
		{
		    filter_type: $("input[name='filter_type']:checked").val(),
		    id: val
		},
        function (results) {
            $('#selStoreList_Nos').empty();
            $('#selStoreList_Nos').append($("<option value=''>ALL</option>"));
            $.each(results, function (i, data) {
                $('#selStoreList_Nos').append($("<option value=" + data.Plant + ">" + data.Name + "</option>"));
            });
            enableLink(true);
        });

        $('#selStoreList_Nos').val('val', '').change();
    });

    $("#filter_by").change();

    if (userType != 'ext-part') {
        $("#cust_group_no").select2({
            minimumInputLength: 1,
            width: 'resolve',
            dropdownAutoWidth: 'false',
            placeholder: '',
            allowClear: true,
            ajax: {
                url: "/partTracking/OrderThruCounterGetCustomerGroupList",
                type: "POST",
                dataType: 'json',
                data: function (params) {
                    var queryParameters = {
                        term: params.term
                    }
                    return queryParameters;
                },
                processResults: function (data) {
                    return {
                        results: $.map(data.modelList,
							function (item) {
							    return {
							        text: item.CUNM,
							        id: item.CUNM
							    }
							})
                    };
                },
            },
        });
    };

    $("#selCustList_Nos").select2({
        minimumInputLength: 1,
        placeholder: '',
        width: 'resolve',
        dropdownAutoWidth: 'false',
        allowClear: true,
        ajax: {
            url: "/partTracking/GetCustomerListFilter",
            type: "POST",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    term: params.term,
                    cust_group: $('#cust_group_no').val()
                }
                return queryParameters;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.list,
                        function (item) {
                            return {
                                text: item.NAME1,
                                id: item.CustomerNo
                            }
                        })
                };
            },
        },
    });


    $table.bootstrapTable({
        cache: false,
        search: false,
        pagination: true,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        showExport: true,
        exportTypes: ['excel'],
        exportOptions: {
            ignoreColumn: [0],
            fileName: 'file.xls'
        },
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        //fixedColumns:true,
        //fixedNumber:4,
        columns: [
            { field: 'action', title: 'Action', width: '90px', align: 'center', formatter: operateFormatter, events: operateEvents, switchable: false },
            { field: 'cust_no', title: 'Cust. Id', halign: 'center', width: '90px', align: 'left', sortable: true, switchable: false },
            { field: 'cust_name', title: 'Customer Name', halign: 'center', width: '240px', align: 'left', sortable: true, switchable: false },
            { field: 'cust_po_no', title: 'PO Number', width: '180px', halign: 'center', align: 'left', sortable: true },
            { field: 'cust_po_date', title: 'PO Date', width: '100px', halign: 'center', align: 'left', formatter: dateFormatter, sortable: true },
            { field: 'orderd_by', title: 'Ordered By', halign: 'center', width: '240px', align: 'left', sortable: true },
            { field: 'wo_number', title: 'WO Number', width: '120px', halign: 'center', align: 'left', sortable: true },
            //{ field: 'seg_number', title: 'Seg', width: '70px', halign: 'center', align: 'center', sortable: true },
            { field: 'ref_doc_no', title: 'SO Number', halign: 'center', width: '90px', align: 'left', sortable: true, switchable: false },
            { field: 'doc_date', title: 'Doc Date', width: '100px', halign: 'center', align: 'right', formatter: dateFormatter, sortable: true, switchable: false },
            { field: 'model_serial', title: 'Model - Serial', width: '260px', halign: 'center', align: 'left', sortable: true },
            { field: 'equip_number', title: 'Equip Number', width: '130px', halign: 'center', align: 'left', sortable: true },
            { field: 'invoice_type', title: 'Invoice Type', width: '170px', halign: 'center', align: 'left', sortable: true },
            //{ field: 'shp_type', title: 'Shp Type', halign: 'center', width: '170px', align: 'left', sortable: true },
            { field: 'doc_status', title: 'Doc / WO Status', halign: 'center', width: '180px', align: 'left', sortable: true, switchable: false },
            {
                field: 'order_status', title: 'Order Status', width: '180px', halign: 'center', align: 'left', sortable: true, switchable: false
            },
    		{ field: 'need_by_date', title: 'Need By Date', halign: 'center', width: '120px', align: 'right', formatter: dateFormatter, sortable: true, switchable: false },
            { field: 'commited_date', title: 'Commited Date', width: '130px', halign: 'center', align: 'right', formatter: dateFormatter, sortable: true, switchable: false },
            { field: 'store_no', title: 'Plant', width: '90px', halign: 'center', align: 'center', sortable: true },
            { field: 'Overdue', title: 'Overdue', width: '90px', halign: 'center', align: 'center', sortable: true },
            { field: 'CreditLimit', title: 'Credit Limit', width: '90px', halign: 'center', align: 'center', sortable: true },
    		//{ field: 'update', title: 'Update Document', width: '120px', align: 'center', formatter: operateFormatter events: updateDocuments, switchable: false }
            { field: 'update', title: 'Update Document', width: '120px', align: 'center', 
            formatter: operateUpdate({ Edit: Boolean($AllowUpdate) }), events: updateDocuments, switchable: false
            }
        ]
    });
    $("[name=refresh]").css('display', 'none');
    $('#filter-bar').bootstrapTableFilter({
        filters: [
		    {field: 'cust_name', label: 'CustomerName',  type: 'search', enabled: true },
		    { field: 'cust_po_no', label: 'poNumber', type: 'search' },
    		{ field: 'order_status', label: 'orderStatus', type: 'select', values: [ { id: 'Outstanding', label: 'Outstanding' }, { id: 'BO Fill', label: 'BO Fill' } ] }
        ],
        onSubmit: function () {
            alert();
        }
    });

    $('#btn-filter').click(function () {
        var defaultDate = $('input[name="defaultDate"]:checked').val();
        var dateSetStart;
        var dateSetEnd;
        if (defaultDate == 'OFF') {
            dateSetStart = '';
            dateSetEnd = '';
        } else {
            dateSetStart = $('#doc_date_start').val();
            dateSetEnd = $('#doc_date_end').val();
        }
        window.pis.table({
            objTable: $table,
            urlSearch: '/parttracking/OrderThruCounterPage',
            urlPaging: '/parttracking/OrderThruCounterPageXt',
            searchParams: {
                filter_type: $("input[name='filter_type']:checked").val(),
                cust_group_no: $('#cust_group_no').val(),
                selCustList_Nos: $('#selCustList_Nos').val(),
                cust_po_no: $('#cust_po_no').val(),
                cust_po_date: $('#custpodate').val(),
                selStoreList_Nos: $('#selStoreList_Nos').val(),
                invoice_type: $('#invoice_type').val(),
                wo_number: $('#wo_number').val(),
                seg_number: $('#seg_number').val(),
                model_type: $('#model_type').val(),
                model: $('#model').val(),
                serial: $('#serial').val(),
                equip_number: $('#equip_number').val(),
                ref_doc_no: $('#ref_doc_no').val(),
                shp_type: $('#shp_type').val(),
                doc_date_start: dateSetStart,
                doc_date_end: dateSetEnd,
                filter_type: $("input[name=filter_type]:checked").val(),
                filter_by: $('#filter_by').val(),
                part_number: $('#part_number').val(),
                part_desc: $('#part_desc').val(),
                order_status: $('#order_status').val()
            },
            dataHeight: 412,
            autoLoad: true
        });
        //$('.fixed-table-body-columns').css('display', 'block');
    });

    $('#btn-clear').click(function () {
        clearScreen();
    });

    $(".downloadExcel").click(function () {
        $(".table2excel").table2excel({
            name: "Order Thru Counter & Service",
            filename: "Order Thru Counter & Service"
        });
    });

    $("#updateDocument").submit(function (e) {
        e.preventDefault();
        var form = $('#updateDocument').serialize();
        $.ajax({
            type: 'POST',
            url: '/partTracking/updateDocument',
            data: form,
            dataType: 'json',
            async: false,
            success: function (data) {
                if (data.result == 'success') {
                    swal({ title: 'Success', text: 'Documents <b>' + data.rfdcno + '</b> successfully ' + data.msg, html: true, timer: 2000, type: "success", showConfirmButton: false });
                } else {
                    swal({ title: 'Failed', text: 'Documents <b>' + data.rfdcno + '</b> failed to ' + data.msg, html: true, timer: 2000, type: "error", showConfirmButton: false });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                swal({ title: 'Error', text: jqXHR.status + " " + jqXHR.statusText, timer: 2000, type: "error", showConfirmButton: false });
            }
        }).then(function () {
            $("[name=refresh]").trigger('click');
//            $("#btn-filter").trigger('click');
//            $table.bootstrapTable('refresh');
            $('#updDoc').modal('hide');
        });
    });
});

function operateFormatter(value, row, index) {
	return [
        '<div class="btn-group">',
	        '<button type="button" class="btn btn-info detail" onclick="viewDetail(\'' + row.ref_doc_no + '\',\'' + row.store_no + '\',\'' + row.cust_name + '\',\'' + row.cust_po_no + '\',\'' + dateFormatter(row.doc_date) + '\',\'' + row.wo_number + '\',\'' + row.store_name + '\',\'' + dateFormatter(row.need_by_date) + '\',\'' + dateFormatter(row.commited_date) + '\',\'' + row.model + '\',\'' + row.serial + '\',\'' + row.doc_curr + '\',\'' + row.doc_value + '\',\'' + row.doc_status + '\')" title="Detail"><i class="fa fa-search-plus"></i> Detail</button>',
        '</div>'
	].join('');
}

function viewDetail(ref_doc_no, store_no, cust_name, cust_po_no, doc_date, wo_number, store_name, need_by_date, commited_date, model, serial, doc_curr, doc_value, doc_status) {
//    alert(doc_date);
    var url = '/partTracking/OrderThruCounterDetail?' +
        '&refdocno=' + ref_doc_no +
        '&storeno=' + store_no +
        '&custname=' + cust_name +
        '&custpono=' + cust_po_no +
//        '&custpodate=' + cust_po_date +
        '&docdate=' + doc_date +
        '&wonumber=' + wo_number +
        '&storename=' + store_name +
        '&needbydate=' + need_by_date +
        '&commiteddate=' + commited_date +
        '&model=' + model +
        '&serial=' + serial +
        '&doccurr=' + doc_curr +
        '&docvalue=' + doc_value +
        '&docstatus=' + doc_status;
    loadDetailPage(url);
}

window.operateEvents = {
    'click .detail': function (e, value, row, index) {
        //alert(row.doc_date);
		var url = '/partTracking/OrderThruCounterDetail?' +
			'&refdocno=' + row.ref_doc_no +
			'&storeno=' + row.store_no +
			'&custname=' + row.cust_name +
			'&custpono=' + row.cust_po_no +
			'&custpodate=' + dateFormatter(row.cust_po_date) +
			'&docdate=' + dateFormatter(row.doc_date) +
			'&wonumber=' + row.wo_number +
			'&storename=' + row.store_name +
			'&needbydate=' + dateFormatter(row.need_by_date) +
			'&commiteddate=' + dateFormatter(row.commited_date) +
			'&model=' + row.model +
			'&serial=' + row.serial +
			'&doccurr=' + row.doc_curr +
			'&docvalue=' + row.doc_value +
			'&docstatus=' + row.doc_status;
		loadDetailPage(url);
	}
};

function operateUpdate2(value, row, index) {
    return [
        '<div class="btn-group">',
	        '<button type="button" class="btn btn-info update" title="Update"><i class="fa fa-edit"></i> Update</button>',
        '</div>'
    ].join('');
}

function operateUpdate(options) {
    var btn = [];

    btn.push('<div class="btn-group">');
    if (options.Add == true)
        btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>');
    if (options.Edit == true)
        btn.push('<div class="btn-group"><button type="button" class="btn btn-info update" title="Update"><i class="fa fa-edit"></i> Update</button></div>');
    if (options.Delete == true)
        btn.push('<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    if (options.Info == true)
        btn.push('<button type="button" class="btn btn-info info" title="Info"><i class="fa fa-info-circle"></i></button>');

    btn.push('</div>');

    return btn.join('');
}

window.updateDocuments = {
    'click .update': function (e, value, row, index) {
        $("#updDoc").modal("show");
        $("#docNumber").val(row.ref_doc_no);
        if (row.order_status == "Awaiting Release Document") { $('#awt').prop('checked', true); }
        else if (row.order_status == "Outstanding Backorder") { $('#outBO').prop('checked', true); }
        else { $('#oC').prop('checked', true); }
    }
};

function gotoDetail(e) {
	var rowid = $(e).attr("data-rowid");
	var url = '/partTracking/OrderThruCounterDetail/' + rowid;
	window.location = url;

}

function progressFormat(value, row, index) {
	return [
        '<div class="progress progress-xs">',
	        '<div class="progress-bar progress-bar-yellow" style="width:' + row.progress + '%"></div>',
        '</div>'
	].join('');
}

function dateFormatter(dt) {
	if (dt != null) {
		var formattedDate = moment(dt).format('DD MMM YYYY');
		return formattedDate;
	}
};

function setFilter(index) {
	enableLink(false);
	$.getJSON("/partTracking/GetFilterBy", { index: index },
		function (results) {
			$('#filter_by').val('', 'ALL').change();
			$('#filter_by').empty();
			$('#filter_by').append($("<option value=''>ALL</option>"));
			$.each(results, function (i, data) {
				if (index == 1)
					$('#filter_by').append($("<option value=" + data.HubID + ">" + data.Name + "</option>"));
				else
					$('#filter_by').append($("<option value=" + data.AreaID + ">" + data.Name + "</option>"));
			});
		});
	$("#filter_by").select2();
}

function disableDate() {
    var serial = $("#serial").val();
    var model = $("#model").val();
    var partDesc = $("#part_desc").val();
    var partNumber = $("#part_number").val();
    var equipNumber = $("#equip_number").val();
    var woNumber = $("#wo_number").val();
    var refDocNo = $("#ref_doc_no").val();
    var custPoNo = $("#cust_po_no").val();
    if (partDesc.length > 0 || partNumber.length > 0 || equipNumber.length > 0 || woNumber.length > 0 || refDocNo.length > 0 || custPoNo.length > 0 || model.length > 0 || serial.length > 0) {
        $('#doc_date_s').attr("disabled", "disabled");
        $('#on').attr("disabled", "disabled");
        $('#off').attr("disabled", "disabled");
        $('#off').prop('checked', true);
        $('#order_status').attr("disabled", "disabled");
    } else {
        $('#doc_date_s').removeAttr("disabled");
        $('#on').removeAttr("disabled");
        $('#off').removeAttr("disabled");
        $('#on').prop('checked', true);
        $('#order_status').removeAttr("disabled");
    }
}

function clearScreen() {
    $('#hub').prop('checked', true);
    $('#filter_by').val('', 'ALL').change();
    $('#selStoreList_Nos').val('val', '').change();
    $('#cust_group_no').val('', 'ALL').change();
    $('#selCustList_Nos').val('val', '').change();
    $('#cust_po_no').val('');
    $('#custpodate').val('');
    $('#invoice_type').val('', 'ALL').change();
    $('#order_status').removeAttr("disabled");
    $('#order_status').val('all').change();
    $('#shp_type').val('', 'ALL').change();
    $('#on').prop('checked', true);
    $('input[name="doc_date_s"]').val(moment(new Date).format('DD MMM YYYY') + ' - ' + moment(new Date).format('DD MMM YYYY'));
    $('#doc_date_s').removeAttr("disabled");
    $('input[name="doc_date_start"]').val(moment(new Date).format('YYYY-MM-DD'));
    $('input[name="doc_date_end"]').val(moment(new Date).format('YYYY-MM-DD'));
    $('#ref_doc_no').val('');
    $('#wo_number').val('');
    $('#seg_number').val('');
    $('#model_type').val('', 'ALL').change();
    $('#model').val('');
    $('#serial').val('');
    $('#equip_number').val('');
    $('#part_number').val('');
    $('#part_desc').val('');
}