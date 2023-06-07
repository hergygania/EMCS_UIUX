$table = $('#tableDeliveryRequisition');
$searchInput = $("#txtSearchData").val();
function statusFormatter(str, index, row) {
    color = '';
    icon = '';
    text = formatUpperCase(str);
    switch (str) {
        case 'Delayed':
            color = 'danger';
            //icon = 'fa fa-times';
            break;
        case 'Risk Of Delayed':
            color = 'warning';
            //icon = 'fa fa-edit';
            break;
        case 'On Schedule':
            color = 'primary';
            //icon = 'fa fa-paper-plane';
            break;
        default:
            color = 'default';
            //icon = 'fa fa-file';
            break;
    }
    return "<div class='label label-" + color + "' style='width:100px;'>" + "<i class='" + icon + "'></i> " + text + "</div>";
}
function activityFormatter(str, index, row) {
    color = '';
    text = formatUpperCase(str);
    switch (str) {
        case 'DR Creation':
            color = 'default';
            icon = 'fa fa-file-medical';
            break;
        case 'Preparation':
            color = 'default';
            icon = 'fa fa-hourglass-half';
            break;
        case 'Pick Up':
            color = 'default';
            icon = 'fa fa-truck-pickup';
            break;
        case 'Instransit':
            color = 'default';
            icon = 'fa fafa-shipping-fast';
            break;
        case 'POD':
            color = 'default';
            icon = 'fa fa-handshake';
            break;
        case 'Rejected':
            color = 'danger';
            icon = 'fa fa-ban';
            break;
        default:
            color = 'default';
            icon = 'fa fa-file';
            break;
    }
    return "<div class='label label-" + color + "' style='width:100px;'>" + "<i class='" + icon + "'></i> " + text + "</div>";
}
function ActionFormatter(value, row, index) {
    var htm = [];
    htm.push('<button class="view btn btn-info btn-xs"><i class="fa fa-eye"></i></button> ');
    if (row.Status === 'draft' || row.Status === 'revise') {
        if (allowUpdate === "True") htm.push('<button class="edit btn btn-primary btn-xs"><i class="fa fa-edit"></i></button> ');
        if (row.Status !== "complete") {
            if (allowDelete === "True") htm.push('<button class="remove btn btn-danger btn-xs"><i class="fa fa-trash"></i></button> ');
        }
    }
    return htm.join('');
}
function showModal() {
    $("#myModalRequest").modal("show");
}

function hideModal() {
    $("#myModalRequest").modal("hide");
}

function loadTable() {
    window.pis.table({
        objTable: $table,
        urlSearch: '/DTS/ReportDeliveryRequisitionPage',
        urlPaging: '/DTS/ReportDeliveryRequisitionPageXt',
        searchParams: {
            Activity: $('#filter-activity').val(),
            Status: '',
            requestor: true
        },
        autoLoad: true
    });
}

var columnList = [
    [
        {
            field: 'KeyCustom',
            title: 'DR NO.',
            halign: 'center',
            align: 'center',
            rowspan: 2,
            class: 'text-nowrap',
            //filterControl: "input",
            sortable: true
        },
        {
            field: 'Activity_DR',
            title: 'ACTIVITY',
            halign: 'center',
            align: 'left',
            formatter: activityFormatter,
            rowspan: 2,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'Status_DR',
            title: 'STATUS',
            halign: 'center',
            align: 'left',
            formatter: statusFormatter,
            rowspan: 2,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: '',
            title: 'REQUESTER',
            halign: 'center',
            colspan: 2,
            align: 'left',
            class: 'text-nowrap'
        },
        {
            field: 'Unit',
            title: 'TYPE',
            class: 'text-nowrap',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            //filterControl: "select",
            sortable: true
        },
        {
            field: 'Model',
            title: 'MODEL',
            class: 'text-nowrap',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            sortable: true
        },
        {
            field: 'SerialNumber',
            title: 'SN',
            class: 'text-nowrap',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            sortable: true
        },
        {
            field: 'Batch',
            title: 'BATCH',
            class: 'text-nowrap',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            sortable: true
        },
        {
            field: 'Origin',
            title: 'FROM (ORIGIN)',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            rowspan: 2,
            //filterControl: "input",
            formatter: formatUpperCase,
            sortable: true
        },
        {
            field: '',
            title: 'DESTINATION',
            halign: 'center',
            colspan: 2,
            align: 'left',
            class: 'text-nowrap'
        },
        {
            field: '',
            title: 'RECEIVER',
            halign: 'center',
            colspan: 2,
            align: 'left',
            class: 'text-nowrap'
        },
        {
            field: 'ExpectedTimeLoading',
            title: 'ETD',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            rowspan: 2,
            sortable: true,
            filterControl: "datepicker",
            filterDatepickerOptions: {format: 'yyyy-mm-dd'},
            formatter: formatDateBT,
        },
    ],
    [
        {
            field: 'ReqName',
            title: 'NAME',
            halign: 'center',
            class: 'text-nowrap',
            align: 'left',
            //filterControl: "input",
            formatter: formatUpperCase,
            sortable: true
        },
        {
            field: 'ReqHp',
            title: 'PHONE NO',
            class: 'text-nowrap',
            halign: 'center',
            align: 'left',
            //filterControl: "input",
            sortable: true
        },
        {
            field: 'CustName',
            title: 'CUSTOMER NAME',
            halign: 'center',
            class: 'text-nowrap',
            align: 'left',
            //filterControl: "input",
            formatter: formatUpperCase,
            sortable: true
        },
        {
            field: 'Kabupaten',
            title: 'DISTRICT',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            //filterControl: "input",
            formatter: formatUpperCase,
            sortable: true,
        },
        {
            field: 'PicName',
            title: 'NAME',
            halign: 'center',
            class: 'text-nowrap',
            align: 'left',
            //filterControl: "input",
            formatter: formatUpperCase,
            sortable: true
        },
        {
            field: 'PicHP',
            title: 'CONTACT',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            //filterControl: "input",
            sortable: true
        },
    ]
];
$(function () {
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        sidePagination: 'server',
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        //filterControl: true,
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        onExpandRow: function (index, row, $detail) {
            $detail.html('<span class="text-center" style="font-size:16px;"><i class="fa fa-spinner fa-pulse fa-fw"></i></span> Loading, please wait...');
            $.ajax({
                url: "/DTS/PartialListDetail",
                dataType: "html",
                method: 'GET',
                data: { InboundID: row.AjuNo },
                success: function (resultHtml) {
                    $detail.html(resultHtml);
                },
                error: function (e) {
                    $detail.html("Data not found");
                }
            });
        },
        columns: columnList
    });
    loadTable();
    $("#btnExportDR").click(function () {
        var options = $table.bootstrapTable('getOptions');
        enableLink(false);
        $.ajax({
            url: "/DTS/DownloadReportDR",
            type: 'GET',
            data: {
                Activity: $('#filter-activity').val(),
                Status: '',
                filterColumns: options.valuesFilterControl,
            },
            success: function (guid) {
                enableLink(true);
                window.open('/DTS/DownloadToExcelReportDR?guid=' + guid, '_blank');
            },
            cache: false
        });
    });

    $('#BtnFilter').click(function (e) {
        console.log(e);
        loadTable();
    });
});
