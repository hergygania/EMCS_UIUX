

$(function () {
    loadingPage = function (url) {
        $('html, body').addClass('wait');
        $('.content').empty();
        $('.box-title').text('');
        $('.content').html('<div style="text-align:center;margin-top:5%;"><img class="loadingImg" src="/Content/images/ajax-loading.gif"/>&nbsp;...Loading...</div>')
        $('.content').css('color', 'red');
        location.replace(url);
    }
});

(function addXhrProgressEvent($) {
    var originalXhr = $.ajaxSettings.xhr;

    $.ajaxSetup({
        progress: $.noop,
        xhr: function () {
            var xhr = originalXhr(), that = this;
            if (xhr) {
                if (typeof xhr.addEventListener == "function") {
                    xhr.addEventListener("progress", function (event) {
                        that.progress(event);
                        if (that.global) {
                            var event = $.Event('ajaxProgress', event);
                            event.type = 'ajaxProgress';
                            $(document).trigger(event, [xhr]);
                        }
                    }, false);
                }
            }
            return xhr;
        }
    });
})(jQuery);

function formatDateLocal(string, format = "DD MMM YYYY") {
    if (string !== null) {
        var newFormat = moment(string).format(format);
        return newFormat;
    } else {
        return "-";
    }
}
function formatDateTimeLocal(string, format = "DD MMM YYYY hh:mm:ss") {
    if (string !== null) {
        var newFormat = moment(string).format(format);
        return newFormat;
    } else {
        return "-";
    }
}
function formatDate(date, row, index, field) {
    if (date && date != null && date != '') {
        return moment(date).format('DD MMM YYYY').toUpperCase()
    }
    return '';
}
function formatDateBT(date, row, index, field) {
    if (date && date!= null && date != '') {
        return moment(date).format('DD MMM YYYY').toUpperCase();
    }
    return '';
}


function formatUpperCase(value, row, index, field) {
    if (value && value != null && value != '' && value != 'null') {
        return value.toUpperCase();
    }

    return '-';
}

function ShowLoading() {
    var customElement = $("<div>", {
        "class": "col-md-12 text-center",
        "html": '<img src="' + myApp.fullPath + '/Content/Images/ajax-loading.gif" />',
        //"html"  : '<span class="text-center" style="font-size:16px;"><i class="fa fa-spinner fa-pulse fa-fw"></i></span> Loading, please wait...'
    });

    //$.LoadingOverlaySetup({
    //    background      : "rgba(0, 0, 0, 0.5)",
    //    //image           : "img/custom.svg",
    //    imageAnimation  : "1.5s fadein",
    //    imageColor      : "#ffcc00",
    //});
    //$.LoadingOverlay("show");
    $.LoadingOverlay("show", {
        image: "",
        //fontawesome : "fa fa-cog fa-spin",
        custom: customElement
    });

    if (myApp.Loading == null) {
        myApp.Loading = setTimeout(function () {
            $.LoadingOverlay("hide");
        }, 30000);
    }
}
function HideLoading() {
    $.LoadingOverlay("hide");
    if (myApp.Loading != null) {
        clearTimeout(myApp.Loading);
    }
}

