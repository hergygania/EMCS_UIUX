loadStyle();
loadScript();
$(document).ready(function () {
    //initChart();

    $("#btnFilter").click(function () {
        initChart();
    });

    $("#btn-clear").click(function () {
        $("#ModaList").val("").trigger("change");
        $("#OriginList").val("").trigger("change");
        $("#DestinationList").val("").trigger("change");
        $("#StatusList").val("").trigger("change");
        $("#UnitTypeList").val("").trigger("change");
        $("#NODA").val("").trigger("change");
        $("#ETD").val("").trigger("change");
        $("#ATD").val("").trigger("change");
        $("#ETA").val("").trigger("change");
        $("#ATA").val("").trigger("change");
    });
});

$(document).ajaxStart(function () {
    showLoading();
}).ajaxStop(function () {
    hideLoading();
});

function initChart() {
    var Params = {
        Moda: $("#ModaList").val() != "" ? $("#ModaList").val() : "0",
        From: $("#OriginList").val(),
        To: $("#DestinationList").val(),
        Status: $("#StatusList").val() != "" ? $("#StatusList").val() : "0",
        UnitType: $("#UnitTypeList").val() != "" ? $("#UnitTypeList").val() : "0",
        ETD: " ",
        ATD: $("#ATD").val(),
        ETA: " ",
        ATA: $("#ATA").val(),
        NODA: $("#NODA").val()
    };
    $.ajax({
        type: "GET",
        url: baseUrl + 'Report/_deliveryTrackingStatus',
        contentType: 'application/json; charset=utf-8',
        data: Params,
        success: function (result) {
            $("#chart").html("");
            $("#chart").html(result);
            setTimeout(function () {
                $('html, body').animate({
                    scrollTop: $("#chart").offset().top
                }, 2000);
            }, 100);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            hideLoading();
            sAlert('Error', "load chart error during : " + xhr.status + " " + thrownError, "error");
        }
    });
}

function includeScript(scriptUrl) {
    document.write("<script src='" + baseUrl + scriptUrl + "' type='text/javascript'><\/script>");
}

function includeStyle(styleUrl) {
    document.write("<link href='" + baseUrl + styleUrl + "' type='text/javascript' rel='stylesheet' type='text/css' \/>");
}

function loadScript() {
    includeScript("Scripts/plugins/Highcharts-4.0.1/js/highcharts-all.js");
    includeScript("Scripts/plugins/Highcharts-4.0.1/js/modules/funnel.js");
    includeScript("Scripts/plugins/core.js");
    includeScript("Scripts/plugins/prettify-small-1-Jun-2011/google-code-prettify/prettify.js");
}

function loadStyle() {
    includeStyle("Scripts/plugins/prettify-small-1-Jun-2011/google-code-prettify/prettify.css")
    includeStyle("Content/Highcharts/Highcharts.css")
}

function showLoading() {
    $('#chart').html("<div id=\"loading-image\" style=\"text-align:center\"><img src=" + baseUrl + "Content/Images/ajax-loading.gif\ alt=\"Loading...\" /></div>");
}

function hideLoading() {
    $('#loading-image').remove();
}