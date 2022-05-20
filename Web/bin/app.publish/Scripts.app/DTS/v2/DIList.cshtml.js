$table = $('#tableDeliveryInstruction');
$tableUnitInfo = $('#tableDeliveryInstructionUnitInfo');
$searchInput = $("#txtSearchData").val();

function statusFormatter(str) {
    var color;
    var text;
    var icon;
    switch (str) {
        case 'approve':
            color = 'success';
            text = 'APPROVED';
            icon = 'fa fa-cog';
            break;
        case 'reject':
            color = 'danger';
            text = 'REJECTED';
            icon = 'fa fa-times';
            break;
        case 'revise':
            color = 'warning';
            text = 'NEED REVISION';
            icon = 'fa fa-edit';
            break;
        case 'revised':
            color = 'primary';
            text = 'REVISED';
            icon = 'fa fa-reply';
            break;
        case 'submit':
            color = 'primary';
            text = 'SUBMITTED';
            icon = 'fa fa-paper-plane';
            break;
        case 'complete':
            color = 'success';
            text = 'COMPLETED';
            icon = 'fa fa-check-circle';
            break;
        default:
            color = 'default';
            text = 'DRAFT';
            icon = 'fa fa-file';
            break;
    }
    return "<div class='label label-" + color + "' style='width:100px;'>" + "<i class='" + icon + "'></i> " + text + "</div>";
}

function tooltip() {
    $('[data-toggle="tooltip"]').tooltip();
}

// ReSharper disable once UnusedParameter
function ActionFormatter(value, row, index) {
    var htm = [];
    htm.push('<button class="view btn btn-info btn-xs" data-toggle="tooltip" data-placement="bottom" title="View"><i class="fa fa-eye"></i></button>');
    if (row.Status === 'draft' || row.Status === 'revise' || row.Status === 'complete' || row.Status ==='submit') {
        if (allowUpdate === "True") htm.push('<button class="edit btn btn-primary btn-xs" data-toggle="tooltip" data-placement="bottom" title="Edit"><i class="fa fa-edit"></i></button>');
        if (row.Status !== "complete") {
            if (allowDelete === "True") htm.push('<button class="remove btn btn-danger btn-xs" data-toggle="tooltip" data-placement="bottom" title="Delete"><i class="fa fa-trash"></i></button>');
        }
      
    }
    if (row.Status === "approve") {
        htm.push('<button class="createdr btn btn-success btn-xs" data-toggle="tooltip" data-placement="bottom" title="Create DR"><i class="fa fa-file"></i></button>');
    }
    
    return htm.join('');
}

function deleteDeliveryInstruction(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/DeliveryInstructionDelete',
        beforeSend: function () {
            //showLoading();
        },
        complete: function () {
            //hideLoading();
        },
        data: { ID: id },
        dataType: "json",
        success: function (d) {
            console.log(d);
            //hideLoading();
            if (d.Msg !== undefined) {
                sAlert('Success', d.Msg, 'success');
            }
            //window.location.reload();
            $("[name=refresh]").trigger('click');
        },
        error: function (jqXhr) {
            sAlert('Error', jqXhr.status + " " + jqXhr.statusText, "error");
        }
    });
}

window.EventsFormatter = {
// ReSharper disable once UnusedParameter
    'click .remove': function (e, vaule, row, index) {
        swal({
            title: "Are you sure want to delete this data?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#F56954",
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            closeOnConfirm: false,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                sweetAlert.close();
                return deleteDeliveryInstruction(row.ID);
            }
// ReSharper disable once NotAllPathsReturnValue
        });
    },
// ReSharper disable once UnusedParameter
    'click .edit': function (e, value, row, index) {
        DIForm.ID = row.ID;
        DIForm.data = row;
        DIForm.mode = "U";
        DIForm.title = "EDIT DI";
        DIForm.show();
    },
// ReSharper disable once UnusedParameter
    'click .view': function (e, value, row, index) {
        DIForm.ID = row.ID;
        DIForm.data = row;
        DIForm.mode = "V";
        DIForm.title = "VIEW DI";
        DIForm.show();
    },
    'click .createdr': function (e, value, row, index) {
        window.open('/DTS/DeliveryRequisitionList?c=1&b='+ row.KeyCustom, '_blank');     
    }
    //'click .export-pdf': function (e, value, row, index) {
    //    $.ajax({
    //        url: "/DTS/ExportToPDFDeliveryInstruction/" + row.ID,
    //        type: 'GET',
    //        beforeSend: function () {
    //            ShowLoading();
    //        },
    //        complete: function () {
    //            HideLoading();
    //        },
    //        success: function (d) {
    //            if (d.errorMessage !== undefined) {
    //                sAlert('Error', d.errorMessage, 'error');
    //            } else {
    //                window.open('/DTS/DownloadPDFDeliveryInstruction?filePath=' + d.fileName, '_blank');
    //            }
    //        },
    //        cache: false
    //    });
    //}
};

var columnList = [
    [
        {
            field: 'ID',
            title: 'ACTION',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            formatter: ActionFormatter,
            events: EventsFormatter,
            "class": 'text-nowrap'
           
        },
        {
            field: 'KeyCustom',
            title: 'DI NO.',
            halign: 'center',
            align: 'center',
            rowspan: 2,
            "class": 'text-nowrap',
            filterControl: "input",
            sortable: true
        },
        {
            field: 'Status',
            title: 'STATUS',
            halign: 'center',
            align: 'left',
            formatter: statusFormatter,
            rowspan: 2,
            "class": 'text-nowrap',
            filterControl: "input",
            sortable: true
        },
        {
            field: '',
            title: 'REQUESTER',
            halign: 'center',
            colspan: 2,
            align: 'left',
            "class": 'text-nowrap'
        },
        {
            field: 'Unit',
            title: 'UNIT',
            "class": 'text-nowrap',
            halign: 'center',
            align: 'center',
            //filterControl: "input",
            rowspan: 2,
            //sortable: true,
            formatter: function () {
                var htm = [];
                htm.push('<button class="show-unit btn btn-info btn-xs" data-toggle="tooltip" data-placement="bottom" title="Show Unit"><i class="fa fa-list-ol"></button>');
                return htm.join('');
            },
            events: EventsFormatter = {
// ReSharper disable once UnusedParameter
                'click .show-unit': function (e, value, row, index) {
                    $tableUnitInfo.bootstrapTable('refreshOptions', {
                        queryParams: function () {
                            return {
                                HeaderID: row.ID
                            };
                        }
                    });
                    $("#myModalUnitInfo").modal("show");
                }
            }
        },
        {
            field: 'ExpectedDeliveryDate',
            title: 'EDD',
            halign: 'center',
            align: 'left',
            "class": 'text-nowrap',
            rowspan: 2,
            sortable: true,
            filterControl: "datepicker",
            formatter: formatDateBT
        },
        {
            field: 'PromisedDeliveryDate',
            title: 'PDD',
            halign: 'center',
            align: 'left',
            "class": 'text-nowrap',
            rowspan: 2,
            sortable: true,
            filterControl: "datepicker",
            formatter: formatDateBT
        },
        {
            field: 'PickUpPlanDate',
            title: 'PPD',
            halign: 'center',
            align: 'left',
            "class": 'text-nowrap',
            rowspan: 2,
            sortable: true,
            filterControl: "datepicker",
            formatter: formatDateBT
        }

    ],
    [
        {
            field: 'RequestorName',
            title: 'NAME',
            halign: 'center',
            "class": 'text-nowrap',
            align: 'left',
            filterControl: "input",
            formatter: formatUpperCase,
            sortable: true
        },
        {
            field: 'RequestorHp',
            title: 'PHONE NO',
            "class": 'text-nowrap',
            halign: 'center',
            align: 'left',
            filterControl: "input",
            formatter: formatUpperCase,
            sortable: true
        }
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
        filterControl: true,
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
                error: function () {
                    $detail.html("Data not found");
                }
            });
        },
        columns: columnList
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/DTS/DeliveryInstructionPage',
        urlPaging: '/DTS/DeliveryInstructionPageXt',
        searchParams: {
            searchName: $("input[name=searchText]").val(),
            requestor: true
        },
        autoLoad: true
    });
    var url_string = window.location.href
    var url = new URL(url_string);
    var FromHome = url.searchParams.get("c");
    if (FromHome === '1') {
        DIForm.resetForm();
        DIForm.data = null;
        DIForm.ID = 0;
        DIForm.mode = "I";
        DIForm.title = "CREATE DI";
        DIForm.show();
    }
    $tableUnitInfo.bootstrapTable({
        cache: false,
        url: "/DTS/GetDeliveryInstructionUnitList",
        pagination: false,
        search: false,
        striped: true,
        clickToSelect: true,
        showColumns: false,
        showRefresh: false,
        smartDisplay: false,
        pageSize: '10',
        uniqueId: 'uid',
        editable: true,
        queryParams: function () {
            return {
                HeaderID: 0
            };
        },
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [
            {
                field: 'Model',
                title: 'MODEL',
                halign: 'left',
                align: 'left',
                "class": 'text-nowrap',
                sortable: true
            },
            {
                field: 'SerialNumber',
                title: 'SERIAL NO',
                halign: 'left',
                align: 'left',
                "class": 'text-nowrap',
                sortable: true
            },
            {
                field: 'Batch',
                title: 'BATCH',
                halign: 'left',
                align: 'left',
                "class": 'text-nowrap',
                sortable: true
            },
            {
                field: 'FreightCost',
                title: 'FREIGHT COST',
                halign: 'left',
                align: 'left',
                "class": 'text-nowrap',
                sortable: true
            }
        ]
    });

    $("#btnAddRequest").click(function () {
        DIForm.resetForm();
        DIForm.data = null;
        DIForm.ID = 0;
        DIForm.mode = "I";
        DIForm.title = "CREATE DI";
        DIForm.show();
    });

    $(".downloadDeliveryInstruction").click(function () {
        enableLink(false);
        $.ajax({
            url: "/DTS/DownloadInbound",
            type: 'GET',
            data: {

            },
            success: function (guid) {
                enableLink(true);
                window.open('/DTS/DownloadToExcelInbound?guid=' + guid, '_blank');
            },
            cache: false
        });
    });
    $("#mySearch").insertBefore($("[name=refresh]"));
});