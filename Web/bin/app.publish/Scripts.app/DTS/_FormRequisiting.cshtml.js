$form = $("#formRequest");
$table = $("#tableDeliveryRequisition");

function hideModal() {
    $("#myModalRequest").modal("hide");
}

function resetForm() {
    $form[0].reset();
    //$("#RequestID").select2("val", "");
    $("#CustomerID").select2("val", "");
    $("[name=refresh]").trigger('click');
}
function getCheckboxValues(name) {
    var dataCheckbox = $("input[name=" + name + "]:checked");
    var dataArray = [];
    var dataString = "";

    if (dataCheckbox.length > 0) {
        $.each(dataCheckbox, function (index, item) {
            dataArray.push($(item).val());
        });
        dataString = dataArray.join(",");
    }
    return dataString;
}
window.pis.table({
    objTable: $table,
    urlSearch: '/DTS/DeliveryRequisitionPage',
    urlPaging: '/DTS/DeliveryRequisitionPageXt',
    searchParams: {
        //IdString: $('#txtlistidinbound').val(),
        //custName:'',
        //origin:''
    },
    autoLoad: true
});

function submitForm(ActType) {
    var reqData = $("#RequestName").val();
    var cusData = $("#CustomerID").select2('data');
    var dataForm = $form.serializeArray();

    //dataForm.push({ name: 'SOD', value: getCheckboxValues("SOD") });
    //dataForm.push({ name: 'TOD', value: getCheckboxValues("TOD") });
    //dataForm.push({ name: 'INCT', value: getCheckboxValues("INCT") });
    dataForm.push({ name: 'type', value: ActType });
    dataForm.push({ name: 'reqName', value: reqData });
    dataForm.push({ name: 'cusName', value: cusData[0].text });

    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/DeliveryRequisitionProccess',
        beforeSend: function () {
            $("button[name=SubmitForm]").attr("disabled", "disabled");
            $("button[name=Cancel]").attr("disabled", "disabled");
            $("button[name=SaveAsRevised]").attr("disabled", "disabled");
            $("button[name=SaveAsDraft]").attr("disabled", "disabled");
        },
        complete: function () {
            //hideLoading();
        },
        data: dataForm,
        dataType: "json",
        success: function (d) {
            console.log(d);
            //hideLoading();
            if (d.Msg !== undefined) {
                sAlert('Success', 'Modify data Success!', 'success');
            }
            resetForm();
            hideModal();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });

}

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

$(function () {
    $(".select2Field").select2();
    //$("#RequestID").select2({
    //    placeholder: 'Select Requester',
    //    ajax: {
    //        url: myApp.root + 'DTS/getMasterUser',
    //        async: false,
    //        dataType: 'json',
    //        data: function (params) {
    //            var query = {
    //                key: params.term,
    //                type: 'public'
    //            };
    //            return query;
    //        },
    //        processResults: function (data) {
    //            console.log(data);
    //            var newData = $.map(data, function (obj) {
    //                obj.id = obj.UserID;
    //                obj.text = obj.FullName;
    //                return obj;
    //            });
    //            // Tranforms the top-level key of the response object from 'items' to 'results'
    //            return {
    //                results: newData
    //            };
    //        }
    //        // Additional AJAX parameters go here; see the end of this chapter for the full code of this example
    //    }
    //});
    $("#CustomerID").select2({
        placeholder: 'Select Customer',
        minimumInputLength: 3,
        ajax: {
            url: myApp.root + 'DTS/getMasterCustomer',
            async: false,
            dataType: 'json',
            data: function (params) {
                var query = {
                    key: params.term,
                    type: 'public'
                };
                return query;
            },
            processResults: function (data) {
                console.log(data);
                var newData = $.map(data, function (obj) {
                    obj.id = obj.Customer_ID_SAP;
                    obj.text = obj.Customer_Full_Name;
                    return obj;
                });
                // Tranforms the top-level key of the response object from 'items' to 'results'
                return {
                    results: newData
                };
            }
            // Additional AJAX parameters go here; see the end of this chapter for the full code of this example
        }
    });
    $('#CustomerID').on('select2:select', function (e) {
        var data = e.params.data;
        var element = $(this);
        console.log(element.name);
        $("div[for=CustID]").hide();
        $("textarea[name=CustAddress]").val(data.Jalan);
    });
    //$('#RequestID').on('select2:select', function (e) {
    //    var data = e.params.data;
    //    var element = $(this);
    //    $("div[for=ReqID]").hide();
    //    $("input[name=ReqHp]").val(data.Phone);
    //});
    $("button[name=SubmitForm]").click(function () {
        var isValid = $form.valid();
        if (isValid) {
            submitForm("submit");
        }
    });
    $("button[name=Cancel]").click(function () {
        $form[0].reset();
        //$("#RequestID").select2("val", "");
        $("#CustomerID").select2("val", "");
        hideModal();
    });
    $("button[name=SaveAsDraft]").click(function () {
        var isValid = $form.valid();
        console.log(isValid);
        if (isValid) {
            submitForm("draft");
        }
    });
    $("button[name=SaveAsRevised]").click(function () {
        var isValid = $form.valid();
        console.log(isValid);
        if (isValid) {
            submitForm("revised");
        }
    });
    $("input[name=PenaltyLateness]").click(function () {
        $('input[name=PenaltyLateness]').prop('checked', false);
        $(this).prop('checked', true);
    });
    $("input[name=Transportation]").click(function () {
        $('input[name=Transportation]').prop('checked', false);
        $(this).prop('checked', true);
    });
    $("input[name=unit]").click(function () {
        $('input[name=unit]').prop('checked', false);
        $(this).prop('checked', true);
    });
    //$(".INCT").click(function () {
    //    $('.INCT').prop('checked', false);
    //    $(this).prop('checked', true);
    //});

    //$(".TOD").click(function () {
    //    $('.TOD').prop('checked', false);
    //    $(this).prop('checked', true);
    //});

    //$(".SOD").click(function () {
    //    $('.SOD').prop('checked', false);
    //    $(this).prop('checked', true);
    //});
});