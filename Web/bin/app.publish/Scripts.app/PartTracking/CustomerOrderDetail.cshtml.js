$(function () {
	var $tableDet = $('#table');
	$(".js-states").select2();
    //Date picker
    $('#datePicker').daterangepicker();

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');

    $tableDet.bootstrapTable({
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
            field: 'SOS',
            title: 'SOS',
            //            width: '10%',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'part_no',
            title: 'Part No',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'part_desc',
            title: 'Part Desc',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'progress',
            title: 'Progress',
            halign: 'center',
            align: 'left',
            formatter: progressFormat
        }, {
            field: 'status',
            title: 'Status',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'cust_line',
            title: 'Cust Line',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'source',
            title: 'Source',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'doc_invoice',
            title: 'BO<br />Doc/Invoice',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'doc_invoice_date',
            title: 'BO/Inv<br />Date',
            halign: 'center',
            align: 'left',
            formatter: dateFormatter,
            sortable: true
        }, {
            field: 'qty_order',
            title: 'QTY<br />Order',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'qty_shipped',
            title: 'QTY<br />Shipped',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'qty_bO',
            title: 'QTY BO',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'eta',
            title: 'ETA',
            halign: 'center',
            align: 'left',
            formatter: dateFormatter,
            sortable: true
        }, {
            field: 'ata',
            title: 'ATA',
            halign: 'center',
            align: 'left',
            formatter: dateFormatter,
            sortable: true
        }, {
            field: 'commodity_name',
            title: 'Commodity',
            //            width: '10%',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'weight',
            title: 'Weight',
            //            width: '10%',
            halign: 'center',
            align: 'left',
            sortable: true
        }]
    });

    window.pis.table({
        objTable: $tableDet,
        urlSearch: '/parttracking/OrderThruCounterDetailPage',
        urlPaging: '/parttracking/OrderThruCounterDetailPageXt',
        searchParams: $('#ref_doc_no').val(),
        autoLoad: true
    });


});



function operateFormatter(value, row, index) {
    return [
		'<div class="btn-group">',
			'<button type="button" class="btn btn-info detail" title="Detail" data-toggle="modal" data-target="#modal" ><i class="fa fa-search-plus"></i> Detail</button>',
			'<button type="button" class="btn btn-primary leadtime" title="Leadtime"><i class="fa fa-clock-o"></i> Milestone</button>',
			'<button type="button" class="btn btn-danger imex" title="Imex"><i class="fa fa-check-circle"></i> IMEX</button>',
		'</div>'
    ].join('');
}

window.operateEvents = {
    'click .detail': function (e, value, row, index) {

        //		window.location = 'index.php?page=CustomerOrderDetail';
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
			'<div class="progress-bar progress-bar-danger" style="width:' + row.progress + '%"></div>',
		'</div>'
    ].join('');
}

function dateFormatter(dt) {
    if (dt != null) {
        var formattedDate = moment(dt).format('DD MMM YYYY');
        return formattedDate;
    }
};