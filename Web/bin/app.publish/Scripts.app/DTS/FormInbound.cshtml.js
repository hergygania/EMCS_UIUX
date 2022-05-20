$btn = $("#btnAddHistory");
$form = $("#formCreateInbound");
$formAdd = $("#formAddHistory");
$historyForm = $("#formAddHistory");
$table = $("#tableFormInbound");
$modal = $("#myModalPlace");
$historyList = [];

function getTimeStamp() {
    var dateData = new Date();
    return dateData.getTime();
}

function getIndexByKey(dtArray, value) {
    var idx = dtArray.findIndex(x => x.ID === value);
    return idx;
}

function pad(str, max) {
    str = str.toString();
    return str.length < max ? pad("0" + str, max) : str;
}

function convertDateFormat(string) {
    if (string) {
        return moment(string).format("DD MMM YYYY");
    }
    return "";
}

function showDetailForm() {
    $modal.modal({
        keyboard: true
    }, "show");
    return false;
}

function showHistoryForm() {
    var datas = $table.bootstrapTable('getData', { useCurrentPage: false });
    var RTSActual = convertDateFormat(datas[0].RTSActual);
    var RTSPlan = convertDateFormat(datas[0].RTSPlan);
    var PortOutPlan = convertDateFormat(datas[0].PortOutPlan);
    var PortOutActual = convertDateFormat(datas[0].PortOutActual);
    var PortInPlan = convertDateFormat(datas[0].PortInPlan);
    var PortInActual = convertDateFormat(datas[0].PortInActual);
    var PLBOutActual = convertDateFormat(datas[0].PLBOutActual);
    var PLBOutPlan = convertDateFormat(datas[0].PLBOutPlan);
    var PLBInActual = convertDateFormat(datas[0].PLBInActual);
    var PLBInPlan = convertDateFormat(datas[0].PLBInPlan);
    var OnBoardVesselActual = convertDateFormat(datas[0].OnBoardVesselActual);
    var OnBoardVesselPlan = convertDateFormat(datas[0].OnBoardVesselPlan);
    var YardInActual = convertDateFormat(datas[0].YardInActual);
    var YardInPlan = convertDateFormat(datas[0].YardInPlan);
    var YardOutActual = convertDateFormat(datas[0].YardOutActual);
    var YardOutPlan = convertDateFormat(datas[0].YardOutPlan);

    $formAdd.find("input[name=RTSPlan]").val(RTSPlan);
    $formAdd.find("input[name=RTSActual]").val(RTSActual);
    $formAdd.find("input[name=OnBoardVesselActual]").val(OnBoardVesselActual);
    $formAdd.find("input[name=OnBoardVesselPlan]").val(OnBoardVesselPlan);
    $formAdd.find("input[name=PortInPlan]").val(PortInPlan);
    $formAdd.find("input[name=PortInActual]").val(PortInActual);
    $formAdd.find("input[name=PortOutActual]").val(PortOutActual);
    $formAdd.find("input[name=PortOutPlan]").val(PortOutPlan);
    $formAdd.find("input[name=PLBInPlan]").val(PLBInPlan);
    $formAdd.find("input[name=PLBInActual]").val(PLBInActual);
    $formAdd.find("input[name=PLBOutPlan]").val(PLBOutPlan);
    $formAdd.find("input[name=PLBOutActual]").val(PLBOutActual);
    $formAdd.find("input[name=YardInPlan]").val(YardInPlan);
    $formAdd.find("input[name=YardInActual]").val(YardInActual);
    $formAdd.find("input[name=YardOutPlan]").val(YardOutPlan);
    $formAdd.find("input[name=YardOutActual]").val(YardOutActual);

    $modal.modal({
        keyboard: true
    }, "show");
    return false;
}

function resetForm() {
    $form[0].reset();
}

function dateFormatter(data, index, row) {
    console.log(data);
    if (data) {
        var date = moment(data).format('DD MMM YYYY');
        return date;
    }
    return "-";
}

function setDate(stringDate) {
    console.log(stringDate);
    typeof stringDate;
    if (stringDate !== null) {
        var date = moment(stringDate).format('DD MMM YYYY');
        return date;
    }
    return "";
}

function addDataHistory(data) {
    var tempData = {
        "ID": "",
        "RTSActual": "",
        "RTSPlan": "",
        "OnBoardVesselActual": "",
        "OnBoardVesselPlan": "",
        "PortInActual": "",
        "PortInPlan": "",
        "PortOutActual": "",
        "PortOutPlan": "",
        "PLBInActual": "",
        "PLBInPlan": "",
        "PLBOutActual": "",
        "PLBOutPlan": "",
        "YardInActual": "",
        "YardInPlan": "",
        "YardOutActual": "",
        "YardOutPlan": ""
    };

    $.each(data, function (index, item) {
        var stringKey = item.name;
        tempData[stringKey] = item.value;
    });
    tempData["ID"] = getTimeStamp();
    $historyList.push(tempData);
    $table.bootstrapTable('load', $historyList);
}

function getIndexNo(ID) {
    return $historyList.findIndex(x => x.ID === ID);
}

function getDetail() {
    var PONo = $("#PONo").val();
    var SN = $("#SerialNumber").val();
    window.pis.table({
        objTable: $table,
        urlSearch: '/DTS/InboundDetailPage',
        urlPaging: '/DTS/InboundDetailPageXt',
        searchParams: {
            PoNumber: PONo,
            SerialNumber: SN
        },
        autoLoad: true
    });
}

function cekType() {
    var result = $("#formType").attr('data-value');
    console.log(result);
    if (result === 'edit') {
        getDetail();
        $("#btnAddDetail").hide();
    } else {
        $("#btnAddHistory").hide();
    }
}

function ActionFormatter() {
    var btn = [];
    var result = $("#formType").attr('data-value');
    btn.push('<div>');
    //btn.push('<button type="button" class="remove btn btn-danger btn-xs"><i class="fa fa-remove"></i></button>');
    if (result !== "edit") {
        //btn.push('<button type="button" class="edit btn btn-primary btn-xs"><i class="fa fa-edit"></i></button>');
        btn.push('<button type="button" class="remove btn btn-danger btn-xs"><i class="fa fa-remove"></i></button>');
    } else {
        btn.push('-');
    }
    btn.push('</div>');
    return btn.join('');
}

window.eventsFormatter = {
    'click .remove': function (e, value, row, index) {
        $historyList.splice(index, 1);
        var table_data_array = $table.bootstrapTable('getData');
        console.log(table_data_array);
        table_data_array.splice(index, 1);

        $table.bootstrapTable('load', table_data_array);
        e.preventDefault();
    },
    'click .edit': function (e, value, row, index) {
        $modal.modal({ keyboard: true }, "show");
        var dataList = $historyList[0];
        $("input[name=RTSPlan]").val(dataList.RTSPlan);
        $("input[name=RTSActual]").val(dataList.RTSActual);
        $("input[name=OnBoardVesselActual]").val(dataList.OnBoardVesselActual);
        $("input[name=OnBoardVesselPlan]").val(dataList.OnBoardVesselPlan);
        $("input[name=PortInActual]").val(dataList.PortInActual);
        $("input[name=PortInPlan]").val(dataList.PortInPlan);
        $("input[name=PortOutActual]").val(dataList.PortOutActual);
        $("input[name=PortOutPlan]").val(dataList.PortOutPlan);
        $("input[name=PLBInActual]").val(dataList.PLBInActual);
        $("input[name=PLBInPlan]").val(dataList.PLBInPlan);
        $("input[name=PLBOutActual]").val(dataList.PLBOutActual);
        $("input[name=PLBOutPlan]").val(dataList.PLBOutPlan);
        $("input[name=YardInActual]").val(dataList.YardInActual);
        $("input[name=YardInPlan]").val(dataList.YardInPlan);
        $("input[name=YardOutActual]").val(dataList.YardOutActual);
        $("input[name=YardOutPlan]").val(dataList.YardOutPlan);

        e.preventDefault();
    }
};

$("#formCreateInbound button[name=refresh]").click(function () {
    alert("wow");
})

function submitDetail(dataForm) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/SubmitDetailInbound',
        beforeSend: function () {
            $('.fixed-table-toolbar').hide();
            $('.fixed-table-loading').show();
        },
        complete: function () {
            $('.fixed-table-toolbar').show();
            $('.fixed-table-loading').hide();
        },
        data: dataForm,
        success: function (d) {
            if (d.Msg !== undefined) {
                sAlert('Success', d.Msg, 'success');
            }
            resetForm();
            //$table.bootstrapTable("removeAll")
            //$table.bootstrapTable("refresh");
            //$table.bootstrapTable('append', dataDetail);;
            //window.location.reload();
            //$table.bootstrapTable('refreshOptions', );
            getDetail();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });

}

$(function () {
    cekType();
    $.ajaxSetup({ cache: false });
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        sidePagination: 'server',
        pageSize: '10',
        columns: [
            [
                { field: 'ID', title: '#', width: '', align: 'center', switchable: false, rowspan: 2, formatter: ActionFormatter, events: eventsFormatter },
                { field: '', title: 'RTS', width: '', align: 'center', switchable: false, colspan: 2 },
                { field: '', title: 'On Board Vessel', width: '', align: 'center', switchable: false, colspan: 2 },
                { field: '', title: 'Port In', width: '', align: 'center', switchable: false, colspan: 2 },
                { field: '', title: 'Port Out', width: '', align: 'center', switchable: false, colspan: 2 },
                { field: '', title: 'PLB In', width: '', align: 'center', switchable: false, colspan: 2 },
                { field: '', title: 'PLB Out', width: '', align: 'center', switchable: false, colspan: 2 },
                { field: '', title: 'Yard In', width: '', align: 'center', switchable: false, colspan: 2 },
                { field: '', title: 'Yard Out', width: '', align: 'center', switchable: false, colspan: 2 },
            ],
            [
                { field: 'RTSActual', class: 'text-nowrap', title: 'Actual', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'RTSPlan', class: 'text-nowrap', title: 'Plan', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'OnBoardVesselActual', class: 'text-nowrap', title: 'Actual', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'OnBoardVesselPlan', class: 'text-nowrap', title: 'Plan', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'PortInActual', class: 'text-nowrap', title: 'Actual', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'PortInPlan', class: 'text-nowrap', title: 'Plan', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'PortOutActual', class: 'text-nowrap', title: 'Actual', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'PortOutPlan', class: 'text-nowrap', title: 'Plan', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'PLBInActual', class: 'text-nowrap', title: 'Actual', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'PLBInPlan', class: 'text-nowrap', title: 'Plan', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'PLBOutActual', class: 'text-nowrap', title: 'Actual', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'PLBOutPlan', class: 'text-nowrap', title: 'Plan', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'YardInActual', class: 'text-nowrap', title: 'Actual', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'YardInPlan', class: 'text-nowrap', title: 'Plan', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'YardOutActual', class: 'text-nowrap', title: 'Actual', width: '', align: 'center', switchable: false, formatter: dateFormatter },
                { field: 'YardOutPlan', class: 'text-nowrap', title: 'Plan', width: '', align: 'center', switchable: false, formatter: dateFormatter }
            ]
        ],
        //data: $historyList,
    });
    $("#submitShipmentInbound").click(function () {
        var requiredField = "<i class='fa fa-remove'></i> This field is required.";
        $form.validate({
            rules: {
                AjuNo: {
                    required: true
                },
                MSONo: {
                    required: true
                },
                PONoAjuNo: {
                    required: true
                },
                PONo: {
                    required: true
                },
                DischargePort: {
                    required: true
                },
                SerialNumber: {
                    required: true
                },
                BatchNumber: {
                    required: true
                },
                //ATAPort: {
                //    required: true
                //},
                //ETAPort: {
                //    required: true
                //},
                //ATACakung: {
                //    required: true
                //},
                //ETACakung: {
                //    required: true
                //},
                Position: {
                    required: true
                },
                Remark: {
                    required: true
                },
                LoadingPort: {
                    required: true
                }
            },
            messages: {
                AjuNo: requiredField,
                MSONo: requiredField,
                PONoAjuNo: requiredField,
                PONo: requiredField,
                DischargePort: requiredField,
                SerialNumber: requiredField,
                BatchNumber: requiredField,
                //ATAPort: requiredField,
                //ETAPort: requiredField,
                //ATACakung: requiredField,
                //ETACakung: requiredField,
                Position: requiredField,
                Remark: requiredField,
                LoadingPort: requiredField
            }
        });
        var formType = $("#formType").attr("data-value");
        var dataList = $("#tableFormInbound").bootstrapTable().data('bootstrap.table').data;
        var isValid = $form.valid();
        var id = 0;

        if (isValid) {
            var dataForm = $form.serializeArray();
            dataForm.push({ name: 'details', value: JSON.stringify(dataList) });
            dataForm.push({ name: 'formType', value: $("#formType").attr("data-value") });
            if (dataList.length > 0) {
                $.ajax({
                    type: "POST",
                    url: myApp.root + 'DTS/SubmitInbound',
                    beforeSend: function () {
                        $('.fixed-table-toolbar').hide();
                        $('.fixed-table-loading').show();
                    },
                    complete: function () {
                        $('.fixed-table-toolbar').show();
                        $('.fixed-table-loading').hide();
                    },
                    data: dataForm,
                    success: function (d) {
                        if (d.Msg !== undefined) {
                            sAlert('Success', d.Msg, 'success');
                        }
                        window.location.reload();
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
                    }
                });
            } else {
                sAlert('Failed', 'Please insert detail history', 'error');
            }
        }
    });
    $("#btnSubmitHistory").click(function () {

        var result = $("#formType").attr('data-value');
        if (result === "edit") {
            var dataForm = $formAdd.serializeArray();
            dataForm.push({ name: "AjuNo", value: $("#AjuNo").val() });
            dataForm.push({ name: "PONo", value: $("#PONo").val() });
            dataForm.push({ name: "MSONo", value: $("#MSONo").val() });
            dataForm.push({ name: "SerialNumber", value: $("#SerialNumber").val() });
            console.log('data dari Form add history : ', dataForm);
            submitDetail(dataForm);
        } else {
            var dataHistory = $historyForm.serializeArray();
            var dataDetail = {
                "RTSPlan": $("input[name=RTSPlan]").val(),
                "RTSActual": $("input[name=RTSActual]").val(),
                "OnBoardVesselActual": $("input[name=OnBoardVesselActual]").val(),
                "OnBoardVesselPlan": $("input[name=OnBoardVesselPlan]").val(),
                "PortInActual": $("input[name=PortInActual]").val(),
                "PortInPlan": $("input[name=PortInPlan]").val(),
                "PortOutActual": $("input[name=PortOutActual]").val(),
                "PortOutPlan": $("input[name=PortOutPlan]").val(),
                "PLBInActual": $("input[name=PLBInActual]").val(),
                "PLBInPlan": $("input[name=PLBInPlan]").val(),
                "PLBOutActual": $("input[name=PLBOutActual]").val(),
                "PLBOutPlan": $("input[name=PLBOutPlan]").val(),
                "YardInActual": $("input[name=YardInActual]").val(),
                "YardInPlan": $("input[name=YardInPlan]").val(),
                "YardOutActual": $("input[name=YardOutActual]").val(),
                "YardOutPlan": $("input[name=YardOutPlan]").val()
            };
            $historyForm[0].reset();
            $table.bootstrapTable('append', dataDetail);
        }
        $modal.modal("hide");
    });

    $('.date').datepicker({
        container: '#boxdate',
        format: " mm/dd/yyyy",
        minDate: new Date()
    });
});