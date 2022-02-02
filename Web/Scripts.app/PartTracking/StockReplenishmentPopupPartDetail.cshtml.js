$(function () {
    $("#supStatus").bootstrapTable({
    //    method: 'get',
      //  url: '/json/data3.json',
        cache: false,
        pagination: true,
        search: true,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination	: 'server',
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        // fixedColumns	: true,
        // fixedNumber	: '3',
        columns: [
        [
        {
            field: 'case_number',
            title: 'Case Number',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            sortable: true
        }, {
            field: 'asn_number',
            title: 'ASN Number',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            sortable: true
        }, {
            field: 'case_type',
            title: 'Case Type',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            sortable: true
        }, {
            field: 'case_desc',
            title: 'Case Desc',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            sortable: true
        }, {
            field: 'weight',
            title: 'Weight (Kg)',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            sortable: true
        }, {
            field: 'ship_via',
            title: 'Ship VIA',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            sortable: true
        }, {
            field: 'status_bo',
            title: 'Status BO',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            sortable: true
        }, {
            field: 'facility_bo',
            title: 'Facility BO',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            sortable: true
        }, {
            title: 'Material in Transit',
            align: 'center',
            colspan: 5
        }
        ], [
        {
            field: 'total',
            title: 'Total',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'next_rcvd_qty',
            title: 'Next Rcvd QTY',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'nect_rcvd_date',
            title: 'Next Rcvd Date',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'total_bo',
            title: 'Total BO',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'freeze',
            title: 'Freeze',
            halign: 'center',
            align: 'left',
            sortable: true
        }]
        ]
    });

    window.pis.table({
        objTable: $("#supStatus"),
        urlSearch: '/parttracking/DetailSupplierPage',
        urlPaging: '/parttracking/DetailSupplierPageXt',
        //searchParams: $('#order_number').val(),
        autoLoad: true
    });

    $("#forwStatus").bootstrapTable({
       // method: 'get',
        //url: '/json/data4.json',
        cache: false,
        pagination: true,
        search: true,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        // fixedColumns	: true,
        // fixedNumber	: '3',
        columns: [
        {
            field: 'case_number',
            title: 'Case Number',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'da_number',
            title: 'DA Number',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'last_location',
            title: 'Last Location',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'origin',
            title: 'Origin',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'destination',
            title: 'Destination',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'service_type',
            title: 'Service Type',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'status',
            title: 'Status',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'status_date',
            title: 'Status Date',
            halign: 'center',
            align: 'left',
            sortable: true
        }
        ]
    });

    window.pis.table({
        objTable: $("#forwStatus"),
        urlSearch: '/parttracking/DetailForwarderPage',
        urlPaging: '/parttracking/DetailForwarderPageXt',
        //searchParams: $('#order_number').val(),
        autoLoad: true
    });

    $("#trakStatus").bootstrapTable({
        //method: 'get',
        //url: '/json/data5.json',
        cache: false,
        pagination: true,
        search: true,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        // fixedColumns	: true,
        // fixedNumber	: '3',
        columns: [
        {
            field: 'store_number',
            title: 'Store Number',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'case_number',
            title: 'Case Number',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'order_qty',
            title: 'Order QTY',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'order_date',
            title: 'Order Date',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'supply_qty',
            title: 'Supply QTY',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'supply_date',
            title: 'Supply Date',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'bo_qty',
            title: 'BO QTY',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'bo_to_fill',
            title: 'BO Fill/ RG',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'bo_to_date',
            title: 'BO Fill/ RG Date',
            halign: 'center',
            align: 'left',
            sortable: true
        }
        ]
    });

    window.pis.table({
        objTable: $("#trakStatus"),
        urlSearch: '/parttracking/DetailTrakindoPage',
        urlPaging: '/parttracking/DetailTrakindoPageXt',
        //searchParams: $('#order_number').val(),
        autoLoad: true
    });
})