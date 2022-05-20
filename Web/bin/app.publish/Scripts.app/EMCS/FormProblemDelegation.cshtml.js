function initProblemCaseDelegation(cat) {
    $("#ProblemCasesDelegation").val("").trigger("change");
    $("#ProblemCasesDelegation").select2({
        placeholder: "Search for Problem Cases",
        tags: false, //prevent free text entry
        width: "100%",
        ajax: {
            url: "/EMCS/GetProblemCase",
            type: "get",
            data: function(params) {
                const query = {
                    cat: cat,
                    term: params.term ? params.term : ""
                };

                return query;
            },
            processResults: function(data) {
                // Transforms the top-level key of the response object from 'items' to 'results'
                var options = [];
                $.map(data.data,
                    function(obj) {
                        const item = {};
                        item.id = obj.text;
                        item.text = obj.text;
                        options.push(item);
                    });
                console.log(options);
                return {
                    results: options
                };
            }
        }
    });
}

function initProblemCategoryDelegation() {
    $("#ProblemCategoryDelegation").select2({
        placeholder: 'Search for Problem Category',
        tags: false, //prevent free text entry
        width: "100%",
        ajax: {
            url: "/EMCS/GetProblemCategory",
            type: "get",
            data: function (params) {
                var query = {
                    term: params.term ? params.term : ""
                };

                return query;
            },
            processResults: function (data) {
                // Transforms the top-level key of the response object from 'items' to 'results'
                console.log(data.data);
                var options = [];
                $.map(data.data, function (obj) {
                    var item = {};
                    item.id = obj.text;
                    item.text = obj.text;
                    options.push(item);
                });
                console.log(options);
                return {
                    results: options
                };
            }
        }
    });
}

function initCausesAutocompleteDelegation(cat, cas) {
    var options = {
        url: myApp.fullPath + "EMCS/GetProblemCauses",
        listLocation: "data",
        getValue: "Causes",
        template: {
            type: "Causes",
            fields: {
                description: "Causes"
            }
        },
        list: {
            match: {
                enabled: true
            }
            , onChooseEvent: function () {
                console.log(this);
            }
        },
        theme: "plate-dark",
        ajaxSettings: {
            dataType: "json",
            method: "GET",
            data: {
                dataType: "json",
                term: function () {
                    return $("#ProblemCauses").val();
                },
                cat: cat,
                cas: cas
            }
        }
    };

    $("#ProblemCausesDelegation").easyAutocomplete(options);
}

function initImpactAutocompleteDelegation(cat, cas) {
    var options = {
        url: myApp.fullPath + "EMCS/GetProblemImpact",
        listLocation: "data",
        getValue: "Impact",
        template: {
            type: "Impact",
            fields: {
                description: "Impact"
            }
        },
        list: {
            match: {
                enabled: true
            }
            , onChooseEvent: function () {
                console.log(this);
                var value = $("#consigneeNameCipl").getSelectedItemData();
                $("#consigneeAddressCipl").val(value.ConsigneeCountry);
                $("#consigneeCountryCipl").val(value.ConsigneeEmail);
                $("#consigneeTelpCipl").val(value.ConsigneeFax);
                $("#consigneeFaxCipl").val(value.ConsigneeName);
                $("#consigneePicCipl").val(value.ConsigneePic);
                $("#consigneeEmailCipl").val(value.ConsigneeTelephone);
            }
        },
        theme: "plate-dark",
        ajaxSettings: {
            dataType: "json",
            method: "GET",
            data: {
                dataType: "json",
                term: function () {
                    return $("#ProblemImpact").val();
                },
                cat: cat,
                cas: cas
            }
        }
    };

    $("#ProblemImpactDelegation").easyAutocomplete(options);
}

function initDelegationTo() {
    $("#DelegationTo").val("").trigger("change");
    $("#DelegationTo").select2({
        placeholder: 'Search for Delegation Person',
        tags: false, //prevent free text entry
        width: "100%",
        ajax: {
            url: "/EMCS/GetNextSuperior",
            type: "get",
            data: function (params) {
                return params;
            },
            processResults: function (data) {
                // Transforms the top-level key of the response object from 'items' to 'results'
                console.log(data.data);
                var options = [];
                $.map(data.data, function (obj) {
                    var item = {};
                    item.id = obj.AdUser;
                    item.text = obj.EmployeeName;
                    item.Employee_Id = obj.EmployeeId;
                    options.push(item);
                });
                console.log(options);
                return {
                    results: options
                };
            }
        }
    });
}

function DelegationCipl(idRequest, delegationTo) {
    $.ajax({
        url: myApp.fullPath + "EMCS/CiplDelegation",
        type: "Post",
        async: false,
        data: {
            id: idRequest,
            delegationTo: delegationTo
        },
        success: function () {
            Swal.fire({
                title: 'Delegation Submit',
                text: 'Delegation Sucessfully submit',
                type: 'success'
            }).then((result) => {
                console.log(result);
                location.reload();
            });
        },
        error: function () {
            Swal.fire({
                title: 'Error',
                text: 'Data Error. Please Try Again !',
                type: 'error'
            });
        }
    });
}

function submitProblemDelegation() {
    var status = false;
    //var data = $("#myModalProblemDelegationContent form").serializeArray();
    //data.push({ name: "Status", value: statusName });
    const data = {
        ReqType: "CIPL",
        IdRequest: $("input[name=IdRequest]").val(),
        IdStep: $("input[name=IdStep]").val(),
        Status: "Approve",
        Category: "Delegation",
        Case: "CIPL",
        CaseDate: $("#ProblemDateDelegation").val(),
        Causes: $("#ProblemNoteDelegation").val(),
        Impact: "Pending",
        Comment: $("#ProblemNoteDelegation").val()
    };
    $.ajax({
        url: myApp.fullPath + "EMCS/SaveProblemHistory",
        type: "Post",
        async: false,
        data: data,
        success: function (resp) {
            var delegationTo = $("#DelegationTo").val();
            var result = resp.result;
            status = true;
            DelegationCipl(result.IdRequest, delegationTo);
        },
        error: function () {
            Swal.fire({
                title: "Error",
                text: "Data Error. Please Try Again !",
                type: "error"
            });
        }
    });
    return status;
}

initProblemCategoryDelegation();
initDelegationTo();

$("#myModalProblemDelegationContent form").validate({
    onkeyup: false,
    errorClass: "input-validation-error",

    //put error message behind each form element
    errorPlacement: function (error, element) {
        if (element.hasClass("select2") && element.hasClass("input-validation-error")) {
            element.next("span").addClass("input-validation-error");
        }
        else if (element.hasClass("select2-hidden-accessible")) {
            error.insertAfter(element.parent('span.select2'));
        }
    },
    highlight: function (element, errorClass) {
        $(element).addClass(errorClass); //.removeClass(errorClass);
        $(element).closest(".form-group").removeClass("has-success").addClass("has-error");
    },
    //When removing make the same adjustments as when adding
    unhighlight: function(element, errorClass) {
        $(element).removeClass(errorClass); //.addClass(validClass);
        $(element).closest(".form-group").removeClass("has-error").addClass("has-success");
        $(element).next("span").removeClass("input-validation-error");
    }
});

$("#myModalProblemDelegation").on("hidden.bs.modal",
    function() {
        $("#myModalProblemDelegationContent form")[0].reset();
        $("#ProblemCasesDelegation").val("").trigger("change");
        $("#ProblemCategoryDelegation").val("").trigger("change");
    });

$(".select2, .date").on("change", function () {
    var id = $(this).attr("id");
    $("#" + id).valid();
});
// #endregion

$("#myModalProblemDelegationContent form").find("input").removeAttr("disabled");

$("#myModalProblemDelegationContent form").find("textarea").removeAttr("disabled");

$("#myModalProblemDelegationContent form").find("select").removeAttr("disabled");

$("#ProblemCategoryDelegation").on("select2:select", function () {
    var cat = $(this).val();
    initProblemCaseDelegation(cat);
});

$("#ProblemCasesDelegation").on("select2:select", function () {
    var cat = $("#ProblemCategoryDelegation").val();
    var cas = $("#ProblemCasesDelegation").val();
    initCausesAutocompleteDelegation(cat, cas);
    initImpactAutocompleteDelegation(cat, cas);
});

$("#btnDelegation").on("click", function () {
    $("#myModalProblemDelegation").modal("show");
    $("#myModalProblemDelegationContent form").find("input").removeAttr("disabled");
    $("#myModalProblemDelegationContent form").find("textarea").removeAttr("disabled");
    $("#myModalProblemDelegationContent form").find("select").removeAttr("disabled");
});

$("#YesSubmitBtn").on("click", function () {
    submitProblemDelegation();
});