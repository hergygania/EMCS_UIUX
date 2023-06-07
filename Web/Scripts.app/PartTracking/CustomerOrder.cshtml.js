var $table = $('#table-client');
$(function () {
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'true' });


    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');

    
    $('#cust_po_date').daterangepicker({
        format: 'DD MMM YYYY',
        autoUpdateInput: true,
        "showDropdowns": true,
        "startDate": moment(),
        "endDate": moment()
    },
    function (start, end, label) {
        $('#cust_po_date_start').val(start.format('YYYY-MM-DD'));
        $('#cust_po_date_end').val(end.format('YYYY-MM-DD'));
    });
    var today = moment().format('DD MMM YYYY');
    $('#cust_po_date').val(today + ' - ' + today);
    $("#cust_po_date").change(function (e) {
        $('#cust_po_date').val('');
        $('#cust_po_date_start').val('');
        $('#cust_po_date_end').val('');
    });
    $('#cust_po_date_start').val(moment().format('YYYY-MM-DD'));
    $('#cust_po_date_end').val(moment().format('YYYY-MM-DD'));

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
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
        	return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'action',
            title: 'Action',
            width: '100px',
            align: 'center',
            formatter: operateFormatter,
            events: operateEvents
        }, {
            field: 'cust_name',
            title: 'Customer',
            //            width: '20%',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'cust_po_no',
            title: 'PO Number',
                        width: '10%',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'cust_po_date',
            title: 'PO Date',
            //            width: '10%',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'orderd_by',
            title: 'Ordered By',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'ref_doc_no',
            title: 'TU Ref Doc',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'doc_date',
            title: 'Doc Date',
            halign: 'center',
            align: 'left',
            formatter: dateFormatter,
            sortable: true
        }, {
            field: 'model_serial',
            title: 'Unit Model / Serial No',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'equip_number',
            title: 'Equip Number',
            width: '110px',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'order_status',
            title: 'Order Status',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'need_by_date',
            title: 'Need By Date',
            halign: 'center',
            align: 'left',
            formatter: dateFormatter,
            sortable: true
        }, {
            field: 'commited_date',
            title: 'Commited Date',
            halign: 'center',
            align: 'left',
            formatter: dateFormatter,
            sortable: true
        }, {
            field: 'eta',
            title: 'ETA',
            //            width: '7%',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'ata',
            title: 'ATA',
            //            width: '7%',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'Overdue',
            title: 'Overdue',
            //            width: '7%',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'CreditLimit',
            title: 'Credit Limit',
            //            width: '7%',
            halign: 'center',
            align: 'left',
            sortable: true
        }]
    });
});

$('#btn-filter').click(function () {
    window.pis.table({
        objTable: $table,
        urlSearch: '/parttracking/CustomerOrderPage',
        urlPaging: '/parttracking/CustomerOrderPageXt',
        searchParams: {
            selCustList_Nos: $('#selCustList_Nos').val(),
            cust_po_no: $('#cust_po_no').val(),
            model: $('#model').val(),
            serial: $('#serial').val(),
            equip_number: $('#equip_number').val(),
            ref_doc_no: $('#ref_doc_no').val(),            
            part_number: $('#part_number').val(),
            part_desc: $('#part_desc').val()
        },
        autoLoad: true
    });
});
function operateFormatter(value, row, index) {
    return [
		'<div class="btn-group">',
			'<button type="button" class="btn btn-xs btn-info detail" title="Detail"><i class="fa fa-search-plus"></i> Detail</button>',
		'</div>'
    ].join('');
}

window.operateEvents = {
    'click .detail': function (e, value, row, index) {
        
        window.location = '/partTracking/CustomerOrderDetail/' + row.ref_doc_no;
        //		alert('You click edit icon, row: ' + JSON.stringify(row));
        console.log(value, row, index);
    },
    'click .imex': function (e, value, row, index) {
        alert('You click remove icon, row: ' + JSON.stringify(row));
        console.log(value, row, index);
    }
};

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