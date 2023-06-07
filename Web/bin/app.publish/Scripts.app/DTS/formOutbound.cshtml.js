$btn = $("#btnAddHistory");
$form = $("#formOutbound");
$historyList = [];
$requiredMsg = "<i class='fa fa-remove'></i> This field is required.";

function resetForm() {
    $form[0].reset();
}

function addDataHistory(data) {
    var tempData = {
        "RTSActual": "",
        "RTSPlan": "",
        "VesselActual": "",
        "VesselPlan": "",
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
    $historyList.push(tempData);
    $table.bootstrapTable('load', $historyList);
}

function showLoading() {
    $.LoadingOverlay("show", {
        image: "",
        fontawesome: "fa fa-cog fa-spin"
    });
}

function hideLoading() {
    $.LoadingOverlay("hide");
}

$(function () {
    $.ajaxSetup({ cache: false });

    $form.validate({
        rules: {
            DI: {
                required: true
            },
            DA: {
                required: true,
                //remote: {
                //    url: myApp.root + 'DTS/isDaExists',
                //    type: "post",
                //    data: {
                //        DA: function () {
                //            return $("#DANo").val();
                //        }
                //    }
                //}
            },
            Origin: {
                required: true
            },
            Moda: {
                required: true
            },
            UnitModa: {
                required: true
            },
            UnitType: {
                required: true
            },
            SerialNumber: {
                required: true
            },
            Model: {
                required: true
            },
            Status: {
                required: true
            },
            Position: {
                required: true
            },
            //ETD: {
            //    required: true
            //},
            //ATD: {
            //    required: true
            //},
            Destination: {
                required: true
            },
            //ETA: {
            //    required: true
            //},
            //ATA: {
            //    required: true
            //}
        },
        messages: {
            DI: $requiredMsg,
            DA: {
                required: $requiredMsg,
                //remote: '<i class="fa fa-remove"></i> Sorry this DA is already Exists'
            },
            Origin: $requiredMsg,
            Moda: $requiredMsg,
            UnitModa: $requiredMsg,
            UnitType: $requiredMsg,
            SerialNumber: $requiredMsg,
            Model: $requiredMsg,
            Status: $requiredMsg,
            Position: $requiredMsg,
            ETD: $requiredMsg,
            ATD: $requiredMsg,
            Destination: $requiredMsg,
            ETA: $requiredMsg,
            ATA: $requiredMsg
        }
    });

    $("#submitFormOutbound").click(function () {
        var isValid = $form.valid();
        var formType = $("#formType").attr('data-value');
        var id = 0;

        if (isValid) {
            var dataForm = $form.serializeArray();
            dataForm.push({ name: 'formType', value: formType });

            //showLoading();
            console.log(dataForm);
            $.ajax({
                type: "POST",
                url: myApp.root + 'DTS/FormOutbound',
                beforeSend: function () {
                    //showLoading();
                },
                complete: function () {
                    //hideLoading();
                },
                data: dataForm,
                dataType: "json",
                success: function (d) {
                    console.log(d);
                    //hideLoading();
                    d.Msg = 'Modify Data Success';
                    if (d.Msg !== undefined) {
                        sAlert('Success', d.Msg, 'success');
                    }
                    location.href = myApp.root + 'DTS/OutboundNonCKB';
                    //$("[name=refresh]").trigger('click');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
                }
            });
        }
    });

    $('.date').datepicker({
        container: '#boxdate',
        format: " mm/dd/yyyy",
        minDate: new Date()
    });
});