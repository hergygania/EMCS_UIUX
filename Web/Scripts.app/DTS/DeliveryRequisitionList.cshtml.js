$table = $('#tableDeliveryRequisition');
$searchInput = $("#txtSearchData").val();
function statusFormatter(str, index, row) {
    color = '';
    text = '';

    switch (str) {
        case 'approve':
            color = 'primary';
            text = 'In Progress';
            icon = 'fa fa-gear';
            break;
        case 'reject':
            color = 'danger';
            text = 'Reject';
            icon = 'fa fa-remove';
            break;
        case 'revise':
            color = 'warning';
            text = 'Need Revision';
            icon = 'fa fa-edit';
            break;
        case 'revised':
            color = 'primary';
            text = 'Revised';
            icon = 'fa fa-check-circle';
            break;
        case 'submit':
            color = 'primary';
            text = 'Submited';
            icon = 'fa fa-paper-plane';
            break;
        case 'complete':
            color = 'success';
            text = 'Complete';
            icon = 'fa fa-check-circle';
            break;
        default:
            color = 'default';
            text = 'Draft';
            icon = 'fa fa-file';
            break;
    }
    return "<div class='label label-" + color + "' style='width:100px;'>" + "<i class='" + icon + "'></i> " + text + "</div>";
}
function ActionFormatter(value, row, index) {
    console.log(row);
    var htm = [];
    if (row.Status === 'draft' || row.Status === 'revise' || row.Status === 'complete') {
        if (allowUpdate === "True") htm.push('<button class="edit btn btn-primary btn-xs"><i class="fa fa-edit"></i></button> ');
        if (row.Status !== "complete") {
            if (allowDelete === "True") htm.push('<button class="remove btn btn-danger btn-xs"><i class="fa fa-remove"></i></button> ');
        }
    } else {
        htm.push('-');
    }
    return htm.join('');
}
function operateFormatter(options) {
    var btn = [];
    btn.push('<button onclick = "showHistoryTable()" class="btn btn-xs btn-primary" alt="Edit remarks">');
    btn.push('<i class="fa fa-edit"></i>');
    btn.push('</button>');
    return btn.join('');
}
function showModal() {
    $("#formRequest")[0].reset();
    $(".error").hide();
    $("#formRequest").find('input').attr('disabled', false);
    $("select[name=CustID]").attr("disabled", false);
    //$("#RequestID").attr("disabled", false);
    $form.find("textarea[name=CustAddress]").attr("disabled", false);
    $("button[name=Cancel]").show();
    $("button[name=SaveAsDraft]").show();
    $("button[name=SaveAsRevised]").hide();
    $("#myModalRequest").modal("show");
}
function deleteDeliveryRequisition(ID) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/DeliveryRequisitionDelete',
        beforeSend: function () {
            //showLoading();
        },
        complete: function () {
            //hideLoading();
        },
        data: { ID: ID },
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
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
}
window.EventsFormatter = {
    'click .remove': function (e, value, row, index) {
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
                return deleteDeliveryRequisition(row.ID);
            }
        });
    },
    'click .edit': function (e, value, row, index) {
        console.log(row);
        var isdisabled = row.Status === "complete" ? "disabled" : false;
        if (row.Status === "reject" || row.Status === "revise") {
            $("#RejectNoteSpace").show();
            $("textarea[name=RejectNote]").val(row.RejectNote);
        }
        $form.find("input[name=Origin]").val(row.Origin).attr("disabled", isdisabled);
        $form.find("input[name=Model]").val(row.Model).attr("disabled", isdisabled);
        $form.find("input[name=SerialNumber]").val(row.SerialNumber).attr("disabled", isdisabled);
        $form.find("input[name=Batch]").val(row.Batch).attr("disabled", isdisabled);
        $form.find("input[name=ReqHp]").val(row.ReqHp).attr("disabled", isdisabled);
        $form.find("textarea[name=CustAddress]").val(row.CustAddress).attr("disabled", isdisabled);
        $form.find("input[name=Kecamatan]").val(row.Kecamatan).attr("disabled", isdisabled);
        $form.find("input[name=Kabupaten]").val(row.Kabupaten).attr("disabled", isdisabled);
        $form.find("input[name=Province]").val(row.Province).attr("disabled", isdisabled);
        $form.find("input[name=PicName]").val(row.PicName).attr("disabled", isdisabled);
        $form.find("input[name=PicHP]").val(row.PicHP).attr("disabled", isdisabled);

        var TODs = row.TermOfDelivery.split(',');
        $.each(TODs, function (index, itemTod) {
            $form.find("input[name=TOD][value='" + itemTod + "']").prop("checked", true);
        });
        $form.find("input[name=TOD]").attr("disabled", isdisabled);

        var SODs = row.SupportingOfDelivery.split(',');
        $.each(SODs, function (index, itemSod) {
            $form.find("input[name=SOD][value='" + itemSod + "']").prop("checked", true);
        });
        $form.find("input[name=SOD]").attr("disabled", isdisabled);

        var INCTs = row.Incoterm.split(',');
        $.each(INCTs, function (index, itemInct) {
            $form.find("input[name=INCT][value='" + itemInct + "']").prop("checked", true);
        });
        $form.find("input[name=INCT]").attr("disabled", isdisabled);

        $form.find("input[name=ExpectedTimeArrival]").val(dateFormatterCAT(row.ExpectedTimeArrival)).attr("disabled", isdisabled);
        $form.find("input[name=ExpectedTimeLoading]").val(dateFormatterCAT(row.ExpectedTimeLoading)).attr("disabled", isdisabled);
        $form.find("input[name=ID]").val(row.ID).attr("disabled", isdisabled);
        $form.find("input[name=SoNo]").val(row.SoNo);
        $form.find("input[name=DoNo]").val(row.DoNo);

        //var newOption1 = new Option(row.ReqName, row.ReqID, false, false);
        //$('#RequestID').append(newOption1).trigger('change');
        //$("#RequestID").attr("disabled", isdisabled);

        var newOption2 = new Option(row.CustName, row.CustID, false, false);
        $('#CustomerID').append(newOption2).trigger('change');
        $("#CustomerID").attr("disabled", isdisabled);

        $form.find("select[name=CustID]").val(row.CustID).trigger("change");
        $("select[name=CustID]").attr("disabled", isdisabled);

        $form.find("input[name=formType]").val("U");
        $form.find("input[name=OdDate]").val(dateFormatterCAT(row.OdDate));

        if (row.PenaltyLateness) {
            $form.find("input[name=PenaltyLateness][value=true]").prop('checked', true).attr("disabled", isdisabled);
            $form.find("input[name=PenaltyLateness]").attr("disabled", isdisabled);
        } else {
            $form.find("input[name=PenaltyLateness][value=false]").prop('checked', true).attr("disabled", isdisabled);
            $form.find("input[name=PenaltyLateness]").attr("disabled", isdisabled);
        }

        //if (isdisabled === "disabled") {
        //    $("button[name=Cancel]").hide();
        //    $("button[name=SaveAsDraft]").hide();
        //} else {
        //    $("button[name=Cancel]").show();
        //    $("button[name=SaveAsDraft]").show();
        //}
        $form.find("input[name=unit][value=MACHINE]").prop('checked', false); 

        if (row.Unit=='MACHINE') {
            $form.find("input[name=unit][value=MACHINE]").prop('checked', true);
            $form.find("input[name=unit]").attr("disabled", isdisabled);
        } else if (row.Unit=='ENGINE') {
            $form.find("input[name=unit][value=ENGINE]").prop('checked', true);
            $form.find("input[name=unit]").attr("disabled", isdisabled);
        } else if (row.Unit=='FORKLIP') {
            $form.find("input[name=unit][value=FORKLIP]").prop('checked', true); 
            $form.find("input[name=unit]").attr("disabled", isdisabled);
        } else if (row.Unit=='RUE') {
            $form.find("input[name=unit][value=RUE]").prop('checked', true);    
            $form.find("input[name=unit]").attr("disabled", isdisabled);
        }

        $form.find("input[name=Transportation][value=PTTU]").prop('checked', false); 

        if (row.Transportation=='PTTU') {
            $form.find("input[name=Transportation][value='PTTU']").prop('checked', true).attr("disabled", isdisabled);
            $form.find("input[name=Transportation]").attr("disabled", isdisabled);
        } else if (row.Transportation == 'Customer'){
            $form.find("input[name=Transportation][value='Customer']").prop('checked', true).attr("disabled", isdisabled);
            $form.find("input[name=Transportation]").attr("disabled", isdisabled);
        }
        if (row.Status === "complete") {
            $("button[name=Cancel]").show();
            $("button[name=SaveAsDraft]").hide();
            $("button[name=SubmitForm]").show();
            $("button[name=SaveAsRevised]").hide();
        } else if (row.Status === "reject") {
            $("button[name=Cancel]").show();
            $("button[name=SaveAsDraft]").show();
            $("button[name=SubmitForm]").show();
            $("button[name=SaveAsRevised]").hide();
        } else if (row.Status === "revise") {
            $("button[name=Cancel]").show();
            $("button[name=SaveAsDraft]").hide();
            $("button[name=SubmitForm]").hide();
            $("button[name=SaveAsRevised]").show();
        }

        $("#myModalRequest").modal("show");
    }
};
function hideModal() {
    $("#myModalRequest").modal("hide");
}
var columnList = [
    [
        {
            field: 'ID',
            title: 'Action',
            halign: 'center',
            align: 'center',
            rowspan: 2,
            formatter: ActionFormatter,
            events: EventsFormatter,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'KeyCustom',
            title: 'ID',
            halign: 'center',
            align: 'center',
            rowspan: 2,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'Status',
            title: 'Status',
            halign: 'center',
            align: 'left',
            formatter: statusFormatter,
            rowspan: 2,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'Origin',
            title: 'Origin',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'Model',
            title: 'Model',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'SerialNumber',
            title: 'SN Unit',
            halign: 'center',
            rowspan: 2,
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'Batch',
            title: 'Batch',
            halign: 'center',
            rowspan: 2,
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: '',
            title: 'Requestor',
            halign: 'center',
            colspan: 2,
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: '',
            title: 'Destination',
            halign: 'center',
            colspan: 5,
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: '',
            title: 'PIC/ Consignee',
            halign: 'center',
            colspan: 2,
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'TermOfDelivery',
            title: 'Term of Delivery',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'SupportingOfDelivery',
            title: 'Supporting of Delivery',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'Incoterm',
            title: 'IncoTerm',
            halign: 'center',
            align: 'left',
            rowspan: 2,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'ExpectedTimeLoading',
            title: 'Expected time Loading',
            halign: 'center',
            formatter: dateFormatterCAT,
            align: 'center',
            rowspan: 2,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'ExpectedTimeArrival',
            title: 'Expected time Arrival',
            halign: 'center',
            rowspan: 2,
            align: 'center',
            formatter: dateFormatterCAT,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'Transportation',
            title: 'Transportation',
            halign: 'center',
            rowspan: 2,
            align: 'center',           
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'PenaltyLateness',
            title: 'Penalty Lateness',
            halign: 'center',
            align: 'center',
            formatter: function (data, row, index) {
                var stat = data ? '<span class="label label-success">Yes</span>' : '<span class="label label-danger">No</span>';
                return stat;
            },
            rowspan: 2,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: '',
            title: 'Document References',
            halign: 'center',
            align: 'left',
            colspan: 3,
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'CreateDate',
            title: 'Create Date',
            halign: 'center',
            align: 'center',
            rowspan: 2,
            class: 'text-nowrap',
            sortable: true,
            formatter: function (data, index, row) {
                if (data !== null) {
                    var formattedDate = moment(data).format('DD MMM YYYY');
                    return formattedDate;
                }
            }
        }, {
            field: 'RejectNote',
            title: 'Notes',
            class: 'text-nowrap',
            halign: 'center',
            rowspan: 2,
            formatter: function (data, row, index) {
                return data;
            }
        }, {
            field: 'Referrence',
            title: 'Referrence',
            class: 'text-nowrap',
            halign: 'center',
            rowspan: 2,
            formatter: function (data, row, index) {
                return data;
            }
        }
    ],
    [
        {
            field: 'ReqName',
            title: 'Name',
            halign: 'center',
            class: 'text-nowrap',
            align: 'left',
            sortable: true
        },
        {
            field: 'ReqHp',
            title: 'HP No',
            class: 'text-nowrap',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'CustName',
            title: 'Customer Name',
            halign: 'center',
            class: 'text-nowrap',
            align: 'left',
            sortable: true
        },
        {
            field: 'CustAddress',
            title: 'Address',
            class: 'text-nowrap',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Kecamatan',
            title: 'Kecamatan',
            halign: 'center',
            class: 'text-nowrap',
            align: 'left',
            sortable: true
        },
        {
            field: 'Kabupaten',
            title: 'Kabupaten',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true,
        },
        {
            field: 'Province',
            title: 'Province',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true,
        },
        {
            field: 'PicName',
            title: 'Name',
            halign: 'center',
            class: 'text-nowrap',
            align: 'left',
            sortable: true
        },
        {
            field: 'PicHP',
            title: 'HP No',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'SoNo',
            title: 'SO No',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'DoNo',
            title: 'OD No',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'OdDate',
            title: 'OD Date',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            formatter: dateFormatterCAT,
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

    window.pis.table({
        objTable: $table,
        urlSearch: '/DTS/DeliveryRequisitionPage',
        urlPaging: '/DTS/DeliveryRequisitionPageXt',
        searchParams: {
            searchName: $("input[name=searchText]").val(),
            requestor: true
        },
        autoLoad: true
    });

    $("#btnAddRequest").click(function (e) {
        resetForm();
        $("#RejectNoteSpace").hide();
        showModal();
    });

    $(".downloadDeliveryRequisition").click(function () {
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
            cache: false,
        });
    });
    $("#mySearch").insertBefore($("[name=refresh]"));
});