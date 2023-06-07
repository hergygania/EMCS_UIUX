$table = $('#tableDeliveryRequisition');
$form = $("#formRequest");
function statusFormatter(str, index, row) {
    color = '';
    text = '';

    switch (str) {
        case 'approve':
            color = 'info';
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
            icon = 'fa fa-remove';
            break;
        case 'revised':
            color = 'primary';
            text = 'Revised';
            icon = 'fa fa-edit';
            break;
        case 'submit':
            color = 'primary';
            text = 'New';
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
function ActionFormatter(data, row, index) {
    var htm = [];
    console.log('alimutasal', row);
    console.log('alimutasal', data);
    if (row.Status !== "reject" && row.Status !== "complete" && row.Status !== "revise") {
        htm.push('<button class="approve btn btn-primary btn-xs"><i class="fa fa-external-link"></i></button> ');
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
    //$("#formRequest")[0].reset();
    //$(".error").hide();

    $("#myModalRequest").modal("show");
}
function hideModal() {
    $("#myModalRequest").modal("hide");
}
window.EventsFormatter = {
    'click .approve': function (e, value, row, index) {
        if (row.Status === 'complete') {
            $("button[name=Reject]").hide();
            $("button[name=Complete]").hide();
            $("button[name=Approve]").hide();
            $("button[name=Revise]").hide();
            $("button[name=Close]").show();
        } else if (row.Status === "submit") {
            $("button[name=Reject]").show();
            $("button[name=Approve]").show();
            $("button[name=Close]").show();
            $("button[name=Revise]").show();
            $("button[name=Complete]").hide();
        } else if (row.Status === "revised") {
            $("button[name=Reject]").show();
            $("button[name=Approve]").show();
            $("button[name=Close]").show();
            $("button[name=Revise]").show();
            $("button[name=Complete]").hide();
        } else if (row.Status === "approve") {
            $("button[name=Reject]").show();
            $("button[name=Close]").show();
            $("button[name=Complete]").show();
            $("button[name=Approve]").hide();
            $("button[name=Revise]").hide();
        }
        console.log(row);
        $form.find("#ID").val(row.ID);
        $form.find("#KeyCustom").val(row.KeyCustom);
        $form.find("input[name=Origin]").val(row.Origin);
        $form.find("input[name=Model]").val(row.Model);
        $form.find("input[name=Model]").val(row.Model);
        $form.find("input[name=SerialNumber]").val(row.SerialNumber);
        $form.find("input[name=Batch]").val(row.Batch);
        $form.find("input[name=ReqHp]").val(row.ReqHp);
        $form.find("textarea[name=CustAddress]").val(row.CustAddress);
        $form.find("input[name=Kecamatan]").val(row.Kecamatan);
        $form.find("input[name=Kabupaten]").val(row.Kabupaten);
        $form.find("input[name=Province]").val(row.Province);
        $form.find("input[name=PicName]").val(row.PicName);
        $form.find("input[name=PicHP]").val(row.PicHP);

        
        var TODs = row.TermOfDelivery.split(',');
        $.each(TODs, function (index, itemTod) {
            $form.find("input[name=TOD][value='DOOR TO DOOR (DTD)']").prop("checked", false);
            $form.find("input[name=TOD][value='PORT TO PORT (PTP)']").prop("checked", false);
            $form.find("input[name=TOD][value='DOOR TO PORT OR PORT TO DOOR (DTP OR PTD)']").prop("checked", false);
            $form.find("input[name=TOD][value='CONTAINER YARD  TO CONTAINER YARD ( CY TO CY )']").prop("checked", false);
            $form.find("input[name=TOD][value='" + itemTod + "']").prop("checked", true);
        });
        //$form.find("input[name=TOD][value='" + row.TermOfDelivery + "']").prop("checked", true);

        var SODs = row.SupportingOfDelivery.split(',');
        $.each(SODs, function (index, itemSod) {
            $form.find("input[name=SOD][value='OPERATOR LOADING & UNLOADING']").prop("checked", false);
            $form.find("input[name=SOD][value='LIFTING TOOLS']").prop("checked", false);
            $form.find("input[name=SOD][value='ESCORTED BY POLICE']").prop("checked", false);
            $form.find("input[name=SOD][value='ESCORTED BY MECHANIC']").prop("checked", false);
            $form.find("input[name=SOD][value='" + itemSod + "']").prop("checked", true);
        });
        //$form.find("input[name=SOD][value='" + row.SupportingOfDelivery + "']").prop("checked", true);

        var INCTs = row.Incoterm.split(',');
        $.each(INCTs, function (index, itemInct) {
            $form.find("input[name=INCT][value='FOT ( FREE ON TRUCK )']").prop("checked", false);
            $form.find("input[name=INCT][value='ON PONDATION']").prop("checked", false);
            $form.find("input[name=INCT][value='FOB ( FREE ON BOARD )']").prop("checked", false);
            $form.find("input[name=INCT][value='ON GROUND']").prop("checked", false);
            $form.find("input[name=INCT][value='FAS ( FREE ALONGSIDE SHIP)']").prop("checked", false);
            $form.find("input[name=INCT][value='" + itemInct + "']").prop("checked", true);
        });
        //$form.find("input[name=INCT][value='" + row.Incoterm + "']").prop("checked", true);

        $form.find("input[name=ExpectedTimeArrival]").val(dateFormatterCAT(row.ExpectedTimeArrival));
        $form.find("input[name=ExpectedTimeLoading]").val(dateFormatterCAT(row.ExpectedTimeLoading));
        $form.find("input[name=ID]").val(row.ID);
        $form.find("input[name=SoNo]").val(row.SoNo);
        $form.find("input[name=DoNo]").val(row.DoNo);
        $form.find("input[name=DINo]").val(row.DINo);
        $form.find("input[name=Referrence]").val(row.Referrence);
        $('#RequestID').val(row.ReqName);
        $('#CustomerID').val(row.CustName);
        $form.find("input[name=formType]").val("U");
        $form.find("input[name=OdDate]").val(dateFormatterCAT(row.OdDate));

        $form.find("input[name=PenaltyLateness][value=true]").prop('checked', false);
        $form.find("input[name=PenaltyLateness][value=false]").prop('checked', false);
        if (row.PenaltyLateness) {
            $form.find("input[name=PenaltyLateness][value=true]").prop('checked', true);
        } else {
            $form.find("input[name=PenaltyLateness][value=false]").prop('checked', true);
        }
      
        $form.find("input[name=unit][value=MACHINE]").prop('checked', false);
        $form.find("input[name=unit][value=ENGINE]").prop('checked', false);
        $form.find("input[name=unit][value=FORKLIP]").prop('checked', false);
        $form.find("input[name=unit][value=RUE]").prop('checked', false);
        if (row.Unit == 'MACHINE') {
            $form.find("input[name=unit][value=MACHINE]").prop('checked', true);
        } else if (row.Unit == 'ENGINE') {
            $form.find("input[name=unit][value=ENGINE]").prop('checked', true);
        } else if (row.Unit == 'FORKLIP') {
            $form.find("input[name=unit][value=FORKLIP]").prop('checked', true);
        } else if (row.Unit == 'RUE') {
            $form.find("input[name=unit][value=RUE]").prop('checked', true);
        }
        $form.find("input[name=Transportation][value=PTTU]").prop('checked', false);
        $form.find("input[name=Transportation][value=Customer]").prop('checked', false);
        if (row.Transportation=='PTTU') {
            $form.find("input[name=Transportation][value=PTTU]").prop('checked', true);
            //$form.find("input[name=Transportation]").attr("disabled", isdisabled);
        } else {
            $form.find("input[name=Transportation][value=Customer]").prop('checked', true);
            //$form.find("input[name=Transportation]").attr("disabled", isdisabled);
        }


        showModal();
    }
};
var columnList = [
    [
        {
            field: 'ID',
            title: '#',
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
            formatter: dateFormatterCAT,
            sortable: true
        },
        {
            field: 'Referrence',
            title: 'Referrence',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            rowspan: 2,
            sortable: true
        },
        {
            field: 'RejectNote',
            title: 'Notes',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            rowspan: 2,
            sortable: true
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
            sortable: true
        },
        {
            field: 'Province',
            title: 'Province',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
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

function sendResponse(dataForm) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/DeliveryValidateProccess',
        beforeSend: function () {
            $("button[name=Reject]").attr("disabled", "disabled");
            $("button[name=Approve]").attr("disabled", "disabled");
            $("button[name=Complete]").attr("disabled", "disabled");
        },
        complete: function () {
        },
        data: dataForm,
        dataType: "json",
        success: function (d) {
            if (d.Msg !== undefined) {
                sAlert('Success', 'Modify data Success!', 'success');
            }
            $("[name=refresh]").trigger('click');
            
            hideModal();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
}

function submitForm(ActType) {
    var dataForm = $form.serializeArray();
    dataForm.push({ name: 'type', value: ActType });
    if (ActType === "Reject" || ActType === "Revise") {
        swal({
            title: ActType === "Revise" ? "Silahkan isi Revise Note" : "Silahkan isi Reject Note",
            type: "input",
            showCancelButton: true,
            closeOnConfirm: false,
            animation: "slide-from-top",
            inputPlaceholder: "Tuliskan keterangan"
        }, function (inputValue) {
            if (inputValue === false) return false;

            if (inputValue === "") {
                swal.showInputError("Anda Perlu menuliskan sesuatu!");
                return false;
            }
            dataForm.push({ name: 'RejectNote', value: inputValue });
            sendResponse(dataForm);
        });
    } else if (ActType === "Complete") {
        //swal({
        //    title: "Silahkan isi Referrence",
        //    type: "input",
        //    showCancelButton: true,
        //    closeOnConfirm: false,
        //    animation: "slide-from-top",
        //    inputPlaceholder: "Tuliskan Referrence"
        //}, function (inputValue) {
        //    if (inputValue === false) return false;

        //    if (inputValue === "") {
        //        swal.showInputError("Referrence Wajib diisi!");
        //        return false;
        //    }
        //    dataForm.push({ name: 'Referrence', value: inputValue });
        //    sendResponse(dataForm);
        sendResponse(dataForm);
    
        //});
    } else {
        sendResponse(dataForm);
    }


}

$("button[name=Approve]").click(function () {
    var isValid = $form.valid();

    if (isValid) {
        submitForm("Complete");
    }
});
$("button[name=Complete]").click(function () {
    var isValid = $form.valid();
 
    if (isValid) {
        submitForm("Complete");
    }
});
$form.validate({
    highlight: function (element, errorClass, validClass) {
        //$(element).addClass(errorClass).removeClass(validClass);
        $form.find("div[for=" + element.name + "]").show();
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass(errorClass).addClass(validClass);
        //$(element.form).find("div[for=" + element.name + "]").removeClass(errorClass);
        $form.find("div[for=" + element.name + "]").hide();
    }
});
//function resetForm() {
//    $form[0].reset();
//    //$("#RequestID").select2("val", "");
//    $("#CustomerID").select2("val", "");
//    //$("[name=refresh]").trigger('click');
//}

$(function () {
    $(".downloadExcelDeliveryRequisition").click(function () {
        enableLink(false);
        $.ajax({
            url: "/DTS/DownloadInbound",
            type: 'GET',
            data: {
                //IdString: $('#txtlistidinbound').val(),
                //RTSFrom: RTSFrom,
                //RTSTo: RTSEnd,
                //OnBoardVesselFrom: VesselFrom,
                //OnBoardVesselTo: VesselEnd,
                //PortInFrom: PortinFrom,
                //PortInTo: PortinEnd,
                //PortOutFrom: PortoutFrom,
                //PortOutTo: PortoutEnd,
                //Status: $('#Status').val(),
                //Position: $('#Position').val()
            },
            success: function (guid) {
                enableLink(true);
                window.open('/DTS/DownloadToExcelInbound?guid=' + guid, '_blank');
            },
            cache: false,
        });
    });
    $("button[name=SubmitForm]").click(function () {
        var isValid = $form.valid();
        if (isValid) {
            processForm("submit");
        }
    });
    $(".processForm").click(function () {
        var act = $(this).attr('name');
        if (act == 'Reject' || act == 'Revise') {
            submitForm(act);
            hideModal();
        }
        else {
            var isValid = $form.valid();
            if (isValid) {
                submitForm(act);
                hideModal();
            }
        }
    });
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        sidePagination: 'server',
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        ageSize: '10',
        fixedNumber: '3',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: columnList
    });
    window.pis.table({
        objTable: $table,
        urlSearch: '/DTS/DeliveryRequisitionPage',
        urlPaging: '/DTS/DeliveryRequisitionPageXt',
        searchParams: {
            typeData: 'validation',
            requestor: false
            //custName:'',
            //origin:''
        },
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"));
});