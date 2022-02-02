$(function () {
    $('#btn-export').click(function () {
        pisProgressBar(25, true);
        resetFormatValue();
        var data = $('#frmCalculator').serialize();
        SetNumericValue();
        
        if ($("#ModaFactor").val() != "") data = data.replace("ModaFactor=" + $("#ModaFactor").val(), "ModaFactor=" + $("#ModaFactor").text());
        if ($("#ModaFleet").val() != "") data = data.replace("ModaFleet=" + $("#ModaFleet").val(), "ModaFleet=" + $("#ModaFleet")[0].selectedOptions[0].text);
        if ($("#ddlOrigin").val() != "") data = data.replace("Origin=" + $("#ddlOrigin").val(), "Origin=" + $("#ddlOrigin")[0].selectedOptions[0].text);
        if ($("#ddlDestination").val() != "") data = data.replace("Destination=" + $("#ddlDestination").val(), "Destination=" + $("#ddlDestination")[0].selectedOptions[0].text);

        $.ajax({
            type: "POST",
            url: myApp.root + 'FreightCost/ExportToPDF',
            dataType: "json",
            data: data,
            cache: false,
            progress: function (e) {
                console.log(e);
                if (e.lengthComputable) {
                    var pct = (e.loaded / e.total) * 100;
                    pisProgressBar(pct, true);
                }
                else
                    console.warn('Content Length not reported!');
            }
        }).done(function (result) {
            pisProgressBar(false);
            if (result != "Failed") {
                if (result.fileName != "")
                    window.location.href = myApp.root + 'FreightCost/Download/?file=' + result.fileName;
                else
                    sAlert('Failed', "No File PDF", 'error');
            }
            else
                sAlert('Failed', result.Msg, 'error');

            return false;
        });
    });

    spanHideCurrency();
    initNumeric(); 
    //DDl Services
    $("#ddlService").change(function (e) {
        var ddlService = $('#ddlService').val();
        var ddlOrigin = $('#ddlOrigin').val();
        var ddlDestination = $('#ddlDestination').val();
        var txtActualWeight = $('#ActualWeight').val();
        var ModaFactor = $("#ModaFactor option:selected").text();

        if (ddlService != '') {
            if (ddlOrigin != '' && ddlDestination != '') {
                setAll(e, ddlService, ddlOrigin, ddlDestination, txtActualWeight, ModaFactor);
            }
        }
    });

    //DDL Origin
    $("#ddlOrigin").change(function (e) {

        var ddlService = $('#ddlService').val();
        var ddlOrigin = $('#ddlOrigin').val();
        var ddlDestination = $('#ddlDestination').val();
        var txtActualWeight = $('#ActualWeight').val();
        var ModaFactor = $("#ModaFactor option:selected").text();

        if (ddlDestination != '') {
            setAll(e, ddlService, ddlOrigin, ddlDestination, txtActualWeight, ModaFactor);
        }
        $('#TruckingRate').val('0');
    });

    //DDL Destination
    $("#ddlDestination").change(function (e) {

        var ddlService = $('#ddlService').val();
        var ddlOrigin = $('#ddlOrigin').val();
        var ddlDestination = $('#ddlDestination').val();
        var txtActualWeight = $('#ActualWeight').val();
        var ModaFactor = $("#ModaFactor option:selected").text();

        if (ddlOrigin != '') {
            setAll(e, ddlService, ddlOrigin, ddlDestination, txtActualWeight, ModaFactor);
        }
        $('#TruckingRate').val('0');
    });

    //DDL Moda Factor
    $("#ModaFactor").change(function (e) {
        resetFormatValue();

        var ddlModaFactor = $("#ModaFactor option:selected").text();
        var ddlService = $('#ddlService').val();
        var ddlOrigin = $('#ddlOrigin').val();
        var ddlDestination = $('#ddlDestination').val();
        if (ddlModaFactor != null || ddlModaFactor != '') {
            //setSurcharge(e, ddlService, ddlOrigin, ddlDestination, ddlModaFactor);
            //DimWeight();
        }
        else
            $('#DimWeight').val('0');

        SetNumericValue();
    });

    //DDL Moda Feet
    $("#ModaFleet").change(function (e) {
        resetFormatValue();

        var ddlModaFleet = $('#ModaFleet').val();
        if (ddlModaFleet != null)
            $('#TruckingRate').val($('#ModaFleet').val());
        else
            $('#TruckingRate').val('0');

        SetNumericValue();
    });

    //Set All 
    function setAll(e, ServiceCode, Origin, Destination, ActualWeight, ModaFactor) {
        setModaFactor(e, Origin, Destination, ServiceCode);
        setFleetModa(e, Origin, Destination);
    }

    //Get Moda Factor
    function setModaFactor(e, Origin, Destination, ServiceCode) {
        $.getJSON("/FreightCost/GetModaFactor",
                    {
                        Origin: Origin, Destination: Destination, ServiceCode: ServiceCode
                    },
            function (results) {
                $('#ModaFactor').empty();
                $('#ModaFactor').append($("<option value=''></option>"));
                $.each(results, function (i, data) {
                    $('#ModaFactor').append($("<option value=" + data.ValueModa + ">" + data.ModaName + "</option>"));
                });
            });
        $('#ModaFactor').val('val', '').change();
    }

    //get Fleet Moda
    function setFleetModa(e, Origin, Destination) {
        $.getJSON("/FreightCost/GetFleetModaByCity",
                    {
                        Origin: Origin, Destination: Destination
                    },
            function (results) {
                $('#ModaFleet').empty();
                $('#ModaFleet').append($("<option value=''></option>"));
                $.each(results, function (i, data) {
                    $('#ModaFleet').append($("<option value=" + data.RatePerTrip + ">" + data.ModaName + "</option>"));
                });
            });
        $('#ModaFleet').val('val', '').change();
    }

    $('input, textarea, select').on('blur focusout focusin keyup keydown', function () {
        $("#ActualWeight").removeClass("input-validation-error");
        $("#Lenght").removeClass("input-validation-error");
        $("#Wide").removeClass("input-validation-error");
        $("#Height").removeClass("input-validation-error");
    });

    $("select.mandatory").next().find(".select2-selection--single").addClass("mandatory").find(".select2-selection__rendered").css("color", "#FFF !important");
});

function toggleIcon(e) {
    $(e.target)
        .prev('.panel-heading')
        .find(".more-less")
        .toggleClass('glyphicon-plus glyphicon-minus');
}
$('.panel-group').on('hidden.bs.collapse', toggleIcon);
$('.panel-group').on('shown.bs.collapse', toggleIcon);

(function ($) {
    var originalVal = $.fn.val;
    $.fn.val = function () {
        var prev;
        if (arguments.length > 0) {
            prev = originalVal.apply(this, []);
        }
        var result = originalVal.apply(this, arguments);
        if (arguments.length > 0)
            $(this).change();
        return result;
    };
})(jQuery);

function spanShowCurrency(Currency) {
    $('#spanRate').show();
    $('#spanMinRate').show();
    $('#spanCostSurcharge').show();
    $('#spanCostRA').show();
    $('#spanDomm').show();
    $('#spanInter').show();

    $('#spanMinRate').text(Currency);
    $('#spanRate').text(Currency);
    $('#spanCostSurcharge').text(Currency);
    $('#spanCostRA').text(Currency);
    $('#spanDomm').text(Currency);
    $('#spanInter').text(Currency);

    $('[name=CurrencyRate]').val(Currency);
    $('[name=CSR]').val(Currency);
    $('[name=CRA]').val(Currency);
}

function spanHideCurrency() {
    $('#spanRate').hide();
    $('#spanMinRate').hide();
    $('#spanCostSurcharge').hide();
    $('#spanCostRA').hide();
    $('#spanDomm').hide();
    $('#spanInter').hide();
    $('#CurrInboundUSD').hide();
    $('#CurrInRate').hide();
}

function hitungCalculator() {
    resetFormatValue();
    var data = $('#frmCalculator').serialize();
    SetNumericValue();
    if ($("#ModaFactor").val() != "") data = data + "&ModaFactorName=" + $("#ModaFactor").text();

    $.ajax({
        type: "POST",
        url: myApp.root + 'FreightCost/HitungCalculator',
        dataType: "json",
        data: data,
        cache: false
    }).done(function (data) {
        $("#Currency").val(data.Currency);
        $("#Rate").val(data.Rate);
        $("#MinRate").val(data.MinRate);
        $("#MinWeight").val(data.MinWeight);
        $("#InRate").val(data.InRate);
        $("#DimWeight").val(data.DimWeight);
        $("#ChargWeight").val(data.ChargWeight);
        $("#TruckingRate").val(data.TruckingRate);
        $("#CostCBM").val(data.CostCBM);
        $("#CostPacking").val(data.CostPacking);
        $("#CostSurcharge").val(data.CostSurcharge);
        $("#CostRA").val(data.CostRA);
        $("#RA").val(data.RA);
        $("#LeadTime").val(data.LeadTime)

        $("#InboundUSD").val(data.InboundUSD);
        $("#InboundIDR").val(data.InboundIDR);
        $("#TotalInternational").val(data.TotalInternational);
        $("#TotalDomestic").val(data.TotalDomestic);

        if (data.CurrencyInRate != null) {
            $("#CurrInRate").show();
            $("#CurrInRate").text(data.CurrencyInRate);
            $("[name=CurrencyInRate]").val(data.CurrencyInRate);
        }
        if (data.CurrencyRate != null) spanShowCurrency(data.CurrencyRate);

        $(".panel-collapse").collapse();
    });
}

function initNumeric() {
    SetFormatValue($("#ActualWeight"));
    SetFormatValue($("#Lenght"));
    SetFormatValue($("#Wide"));
    SetFormatValue($("#Height"));

    SetFormatValue($("#Currency"));
    SetFormatValue($("#Rate"));
    SetFormatValue($("#MinRate"));
    SetFormatValue($("#MinWeight"));
    SetFormatValue($("#InRate"));
    SetFormatValue($("#DimWeight"));
    SetFormatValue($("#ChargWeight"));
    SetFormatValue($("#TruckingRate"));
    SetFormatValue($("#CostCBM"));
    SetFormatValue($("#CostPacking"));
    SetFormatValue($("#CostSurcharge"));
    SetFormatValue($("#CostRA"));

    SetFormatValue($("#InboundUSD"));
    SetFormatValue($("#InboundIDR"));
    SetFormatValue($("#TotalInternational"));
    SetFormatValue($("#TotalDomestic"));
}
function SetNumericValue() {
    setNumeric($("#ActualWeight"), $("#ActualWeight").val());
    setNumeric($("#Lenght"), $("#Lenght").val());
    setNumeric($("#Wide"), $("#Wide").val());
    setNumeric($("#Height"), $("#Height").val());

    setNumeric($("#Currency"), $("#Currency").val());
    setNumeric($("#Rate"), $("#Rate").val());
    setNumeric($("#MinRate"), $("#MinRate").val());
    setNumeric($("#MinWeight"), $("#MinWeight").val());
    setNumeric($("#InRate"), $("#InRate").val());
    setNumeric($("#DimWeight"), $("#DimWeight").val());
    setNumeric($("#ChargWeight"), $("#ChargWeight").val());
    setNumeric($("#TruckingRate"), $("#TruckingRate").val());
    setNumeric($("#CostCBM"), $("#CostCBM").val());
    setNumeric($("#CostPacking"), $("#CostPacking").val());
    setNumeric($("#CostSurcharge"), $("#CostSurcharge").val());
    setNumeric($("#CostRA"), $("#CostRA").val());

    setNumeric($("#InboundUSD"), $("#InboundUSD").val());
    setNumeric($("#InboundIDR"), $("#InboundIDR").val());
    setNumeric($("#TotalInternational"), $("#TotalInternational").val());
    setNumeric($("#TotalDomestic"), $("#TotalDomestic").val());
}
function resetFormatValue() {
    GetFormatValue($("#ActualWeight"));
    GetFormatValue($("#Lenght"));
    GetFormatValue($("#Wide"));
    GetFormatValue($("#Height"));

    GetFormatValue($("#Currency"));
    GetFormatValue($("#Rate"));
    GetFormatValue($("#MinRate"));
    GetFormatValue($("#MinWeight"));
    GetFormatValue($("#InRate"));
    GetFormatValue($("#DimWeight"));
    GetFormatValue($("#ChargWeight"));
    GetFormatValue($("#TruckingRate"));
    GetFormatValue($("#CostCBM"));
    GetFormatValue($("#CostPacking"));
    GetFormatValue($("#CostSurcharge"));
    GetFormatValue($("#CostRA"));

    GetFormatValue($("#InboundUSD"));
    GetFormatValue($("#InboundIDR"));
    GetFormatValue($("#TotalInternational"));
    GetFormatValue($("#TotalDomestic"));
}

function setNumeric(e, value) {
    if (value == "") value = "0";
    if (value == "Infinity") return;
    if (value.indexOf(",") <= 0) e.autoNumeric('set', value);
}
function SetFormatValue(e) {
    e.autoNumeric("init", {
        aSep: ',',
        aDec: '.',
        mDec: '2',
        aSign: ''
    });
}
function GetFormatValue(e) {
    e.val(e.autoNumeric('get'));
}
