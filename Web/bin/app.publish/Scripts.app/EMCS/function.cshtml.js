// Parameter : 

$(function () {
    $(".datepicker").datepicker({
        format: "dd M yy",
        autoclose: true,
        endDate: new Date()
    });
});

function ConvertDate(value) {
    if (value != null) return moment(value).format("DD MMM YYYY");
    return "-";
};

function getDataSelect2(controller, id) {
    // ReSharper disable once UseOfImplicitGlobalInFunctionScope
    $.getJSON(baseUrl + "/" + controller,
        function (json) {
            $(id).select2({
                placeholder: "   --Please Select--",
                data: json,
                escapeMarkup: function (markup) { return markup; },
                templateResult: formatDataSelect2,
                templateSelection: formatRepoSelection,
                width: "100%"
            });
            $(id).val(null).trigger("change");
        });
}

function formatDataSelect2(data) {
    //if (data.loading) return data.desc;
    var $state = "<div class='select2-result-repository-content'>";
    $state += "<div class='select2-result-repository-content-name'>" + data.id + " - " + data.desc + "</div>";
    $state += "</div>";
    return $state;
}

function formatRepoSelection(data) {
    return data.desc;
}

function runningFormatter(value, row, index) {
    var page = parseInt($("li.page-number.active a").html()) - 1;
    var size = $("span.page-list span.dropup ul.dropdown-menu li.active a").html();
    return (parseInt(size) * parseInt(page)) + index + 1;
}

//-- Henry: Running number for no paging table
function runningFormatterNoPaging(value, row, index) {
    return index + 1;
}

//--Henry: Running Formatter Simple
function runningFormatterC(value, row, index) {
    return index + 1;
}

function formatCurrency(amount = 0, decimalSeparator, thousandsSeparator, nDecimalDigits) {
    decimalSeparator = typeof decimalSeparator !== 'undefined' ? decimalSeparator : '.';
    thousandsSeparator = typeof thousandsSeparator !== 'undefined' ? thousandsSeparator : ',';
    nDecimalDigits = typeof nDecimalDigits !== 'undefined' ? nDecimalDigits : 0;
    var num = parseFloat(amount);
    if (isNaN(num)) {
        return 0;
    }

    var fixed = num.toFixed(nDecimalDigits);
    var parts = new RegExp('^(-?\\d{1,3})((?:\\d{3})+)(\\.(\\d{' + nDecimalDigits + '}))?$').exec(fixed);
    if (parts) {
        console.log(parts[1] + ' - ' + parts[2].replace(/\d{3}/g, thousandsSeparator + '$&') + ' - ' + (parts[4] ? decimalSeparator + parts[4] : ''));
        return parts[1] + parts[2].replace(/\d{3}/g, thousandsSeparator + '$&') + (parts[4] ? decimalSeparator + parts[4] : '');
    } else {
        return fixed.replace('.', decimalSeparator);
    }
}

function removeformatCurrency(value) {
    var num = 0;
    if (value === null || value === '' || value === undefined) {
        return num;
    } else {
        var remove = value.replace(/,/g, '');
        var check = parseFloat(remove);
        return check;
    }
}

function currencyFormatter(value) {
    if (!value) {
        return 0;
    }
    var data = formatCurrency(value, '.', ',', 2);
    return data;
}

function scrollToError(elm = ".error") {
    var cord = ($(elm).offset().top) ? parseInt($(elm).offset().top) - 100 : 0;
    $('html, body').animate({
        scrollTop: cord
    }, 2000);
}

function diff_hours(dt2, dt1) {
    var diff = (dt2.getTime() - dt1.getTime()) / 1000;
    diff /= (60 * 60);
    return Math.abs(Math.round(diff));
}

$(".decimal").blur(function () {
    var id = $(this).attr('id');
    var value = $(this).val();
    var convert = formatCurrency(value, ".", ",", 2);
    $('#' + id).val(convert);
});
$(".decimal").focus(function () {
    var id = $(this).attr('id');
    var value = $(this).val();
    var convert = removeformatCurrency(value);

    $('#' + id).val(convert);
});

$(".decimal4").blur(function () {
    var id = $(this).attr('id');
    var value = $(this).val();
    var convert = formatCurrency(value, ".", ",", 4);
    $('#' + id).val(convert);
});
$(".decimal4").focus(function () {
    var id = $(this).attr('id');
    var value = $(this).val();
    var convert = removeformatCurrency(value);

    $('#' + id).val(convert);
});

var currencyData = $.find(".formatCurrency");
currencyData.forEach(function (data) {
    var idItem = $(data).attr("id");
    var value = $("#" + idItem).val();
    var convert = formatCurrency(value, ".", ",", 2);
    $("#" + idItem).val(convert);
});


function get_extension_file(filename) {
    return filename.split('.').pop();
}