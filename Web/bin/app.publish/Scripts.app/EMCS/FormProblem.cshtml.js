function initProblemCase(cat) {
    $("#ProblemCases").val("").trigger("change");
    $("#ProblemCases").select2({
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
                console.log(data.data);
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

function initProblemCategory() {
    $("#ProblemCategory").select2({
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

function initCausesAutocomplete(cat, cas) {
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

    $("#ProblemCauses").easyAutocomplete(options);
}

function initImpactAutocomplete(cat, cas) {
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

    $("#ProblemImpact").easyAutocomplete(options);
}

$("#myModalProblemContent form").validate({
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
        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
    },
    //When removing make the same adjustments as when adding
    unhighlight: function (element, errorClass) {
        $(element).removeClass(errorClass); //.addClass(validClass);
        $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
        $(element).next("span").removeClass("input-validation-error");
    }
});

$('#myModalProblem').on('hidden.bs.modal', function () {
    $("#myModalProblemContent form")[0].reset();
    $("#ProblemCases").val("").trigger("change");
    $("#ProblemCategory").val("").trigger("change");
});

$(".select2, .date").on("change", function () {
    var id = $(this).attr("id");
    $("#" + id).valid();
});
// #endregion
$("#myModalProblemContent form").find("input").removeAttr("disabled");
$("#myModalProblemContent form").find("textarea").removeAttr("disabled");
$("#myModalProblemContent form").find("select").removeAttr("disabled");
initProblemCategory();
$("#ProblemCategory").on("select2:select", function () {
    var cat = $(this).val();
    initProblemCase(cat);
});

$("#ProblemCases").on("select2:select", function () {
    var cat = $("#ProblemCategory").val();
    var cas = $("#ProblemCases").val();
    initCausesAutocomplete(cat, cas);
    initImpactAutocomplete(cat, cas);
});

$("#btnRevise").on("click", function () {
    $("#myModalProblem").modal("show");
    $("#YesApproveBtn").hide();
    $("#YesRejectBtn").hide();
    $("#YesReviseBtn").show();
    $("#YesCancelBtn").hide();
    $("#myModalProblemContent form").find("input").removeAttr("disabled");
    $("#myModalProblemContent form").find("textarea").removeAttr("disabled");
    $("#myModalProblemContent form").find("select").removeAttr("disabled");
});

$("#btnReject").on("click", function () {
    $("#myModalProblem").modal("show");
    $("#YesApproveBtn").hide();
    $("#YesRejectBtn").show();
    $("#YesReviseBtn").hide();
    $("#YesCancelBtn").hide();
    $("#myModalProblemContent form").find("input").removeAttr("disabled");
    $("#myModalProblemContent form").find("textarea").removeAttr("disabled");
    $("#myModalProblemContent form").find("select").removeAttr("disabled");
    //$("#myModalContent").find("input").removeAttr("disabled");
    //$("#myModalContent").find("textarea").removeAttr("disabled");
    //$("#myModalContent").find("select").removeAttr("disabled");
});

$("#btnCancel").on("click", function () {
    $("#myModalProblem").modal("show");
    $("#YesApproveBtn").hide();
    $("#YesRejectBtn").hide();
    $("#YesReviseBtn").hide();
    $("#YesCancelBtn").show();
    $("#myModalProblemContent form").find("input").removeAttr("disabled");
    $("#myModalProblemContent form").find("textarea").removeAttr("disabled");
    $("#myModalProblemContent form").find("select").removeAttr("disabled");
});