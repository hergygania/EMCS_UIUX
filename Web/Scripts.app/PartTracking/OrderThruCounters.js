"use strict";
var $table = $('#table-order');
setTimeout(function () { $(".model-parent").find(".select2").addClass("width30"); }, 300);

$(function () {
    reloadScripts("OrderThruCounters.js", "token")
    $('#doc_date_s').removeAttr("disabled");
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');

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

    $("#order_status").change(function () {
        var orderStatus = $("#order_status").val();
        var defaultCheck = $('input[name="defaultDate"]:checked').val();
        if (orderStatus != '' && orderStatus != 'Order Completed') {
            $('#on').removeAttr("disabled");
            $('#off').removeAttr("disabled");
        } else {
            $('#on').attr("disabled", "disabled");
            $('#off').attr("disabled", "disabled");
            $('#doc_date_s').removeAttr("disabled");
            $('#on').prop('checked', true);
        }
    });

    $('input[name="defaultDate"]').on('change', function () {
        if ($('input[name="defaultDate"]:checked').val() == 'ON') {
            $('#doc_date_s').removeAttr("disabled");
        } else {
            $('#doc_date_s').attr("disabled", "disabled");
        }
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
                $('#selStoreList_Nos').append($("<option value=" + data.StoreNo + ">" + data.Name + "</option>"));
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
							        id: item.CUNO
							    }
							})
                    };
                },
            },
        });
    };

    $("#selCustList_Nos").select2({
        minimumInputLength: 1,
        width: 'resolve',
        dropdownAutoWidth: 'false',
        ajax: {
            url: "/partTracking/GetCustomerList",
            type: "POST",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    term: params.term,
                    prcuno: $('#cust_group_no').val()
                }
                return queryParameters;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.modelList,
					function (item) {
					    return {
					        text: item.CUNM,
					        id: item.CUNO
					    }
					})
                };
            },
        }
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
        showRefresh: false,
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
        columns: [
            { field: null, title: 'Action', width: '90px', align: 'center', formatter: operateFormatter, events: operateEvents, switchable: false },
            { field: 'cust_no', title: 'Cust Id', halign: 'center', width: '80px', align: 'left', sortable: true, switchable: false },
            { field: 'cust_name', title: 'Cust Name', halign: 'center', width: '200px', align: 'left', sortable: true, switchable: false },
            { field: 'cust_po_no', title: 'PO Number', width: '180px', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'orderd_by', title: 'Ordered By', halign: 'center', width: '220px', align: 'left', sortable: true },
            { field: 'ref_doc_no', title: 'Ref Doc', halign: 'center', width: '100px', align: 'left', sortable: true, switchable: false },
            { field: 'doc_date', title: 'Doc Date', width: '110px', halign: 'center', align: 'right', formatter: dateFormatter, sortable: true, switchable: false },
            { field: 'model_serial', title: 'Model - Serial', width: '190px', halign: 'center', align: 'left', sortable: true },
            { field: 'equip_number', title: 'Equip Number', width: '130px', halign: 'center', align: 'left', sortable: true },
            { field: 'order_status', title: 'Order Status', width: '170px', halign: 'center', align: 'left', sortable: true, switchable: false },
    		{ field: 'need_by_date', title: 'Need By Date', halign: 'center', width: '120px', align: 'right', formatter: dateFormatter, sortable: true, switchable: false },
            { field: 'commited_date', title: 'Commited Date', width: '140px', halign: 'center', align: 'right', formatter: dateFormatter, sortable: true, switchable: false },
            { field: 'store_name', title: 'Store', width: '150px', halign: 'center', align: 'left', sortable: true },
            { field: 'Overdue', title: 'Overdue', width: '150px', halign: 'center', align: 'left', sortable: true },
            { field: 'CreditLimit', title: 'Credit Limit', width: '150px', halign: 'center', align: 'left', sortable: true }
        ]
    });

    $('#filter-bar').bootstrapTableFilter({
        filters: [
            { field: 'cust_name', label: 'CustomerName', type: 'search', enabled: true },
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
            urlSearch: '/parttracking/OrderThruCountersPage',
            urlPaging: '/parttracking/OrderThruCountersPageXt',
            searchParams: {
                filter_type: $("input[name='filter_type']:checked").val(),
                cust_group_no: $('#cust_group_no').val(),
                selCustList_Nos: $('#selCustList_Nos').val(),
                cust_po_no: $('#cust_po_no').val(),
                selStoreList_Nos: $('#selStoreList_Nos').val(),
                model_type: $('#model_type').val(),
                model: $('#model').val(),
                serial: $('#serial').val(),
                equip_number: $('#equip_number').val(),
                ref_doc_no: $('#ref_doc_no').val(),
                doc_date_start: dateSetStart,
                doc_date_end: dateSetEnd,
                filter_type: $("input[name=filter_type]:checked").val(),
                filter_by: $('#filter_by').val(),
                part_number: $('#part_number').val(),
                part_desc: $('#part_desc').val(),
                order_status: $('#order_status').val()
            },
            dataHeight: 212,
            autoLoad: true
        });
    });

    $('#btn-clear').click(function () {
        $('#filter_by').val('', 'ALL').change();
        $('#selStoreList_Nos').val('val', '').change();
        $('#cust_group_no').val('', 'ALL').change();
        $('#selCustList_Nos').val('val', '').change();
        $('#cust_po_no').val('');
        $('#order_status').val('', 'ALL').change();
        $('#ref_doc_no').val('');
        $('#model_type').val('', 'ALL').change();
        $('#model').val('');
        $('#serial').val('');
        $('#equip_number').val('');
        $('#part_number').val('');
        $('#part_desc').val('');
    });

    $(".downloadExcel").click(function () {
        $(".table2excel").table2excel({
            name: "Order Thru Counter & Service",
            filename: "Order Thru Counter & Service"
        });
    });
});

function operateFormatter(value, row, index) {
    return [
        '<div class="btn-group">',
	        '<button type="button" class="btn btn-info detail" title="Detail"><i class="fa fa-search-plus"></i> Detail</button>',
        '</div>'
    ].join('');
}

window.operateEvents = {
    'click .detail': function (e, value, row, index) {
        var url = '/partTracking/OrderThruCounterDetails?' +
			'&refdocno=' + row.ref_doc_no +
			'&storeno=' + row.store_no +
			'&custname=' + row.cust_name +
			'&custpono=' + row.cust_po_no +
//			'&custpodate=' + dateFormatter(row.cust_po_date) +
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
