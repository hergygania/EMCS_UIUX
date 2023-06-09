$(function () {
    $.ajaxSetup({cache: false});
    var o = $.AdminLTE.options;
    _init();
    $.AdminLTE.tree('.sidebar');
    _fix();
    fix_sidebar();

    $(".row-offcanvas").click(function () {
        _fix();
        fix_sidebar();
    });

    $(document).on('keydown drop', '.page', function (event) {
        var charCode = (event.which) ? event.which : event.keyCode;
//        alert(charCode);
        if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 96 || charCode > 105)) {
            if (charCode == 190) {
                return true;
            } else if ((charCode == 8 || (charCode > 31 && charCode < 41))) {
                return true;
            } else {
                return false;
            }
        }
    });

    $.fn.modal.Constructor.DEFAULTS.backdrop = 'static';
    $.fn.modal.Constructor.DEFAULTS.keyboard = false;

    rebindCSS = function () {
        $('.date').datepicker({
            format: "dd M yyyy",
            keyboardNavigation: false,
            autoclose: true,
            clearBtn: true,
            todayHighlight: true
        });

        $('.daterange').daterangepicker({
            format: 'DD MMM YYYY',
            timePicker: false,
            minDate: false,
            maxDate: false,
            showDropdowns: true,
            startDate: myApp.firstMonth,
            endDate: myApp.lastMonth,
            autoUpdateInput: false,
            cancelClass: "daterange-clear",
            locale: {
                format: 'DD MMM YYYY',
                separator: ' - ',
                applyLabel: 'Apply',
                cancelLabel: 'Cancel',
                firstDay: 1
            }
        },
		function (start, end, label) {
		    console.log('New date range selected: ' + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD') + ' (predefined range: ' + label + ')');
		});

        $('.daterange').on('apply.daterangepicker', function (ev, picker) {
            $('#' + ev.currentTarget.id).val(picker.startDate.format('DD MMM YYYY') + ' - ' + picker.endDate.format('DD MMM YYYY'));
        });
        $('.daterange').on('cancel.daterangepicker', function (ev) {
            $('#' + ev.currentTarget.id).val('');
        });

        $('.daterange-clear').on('click', function (e) {
            $('.daterange').val('');
        });

        $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });
        var width = $(".select2-container--default").width() - 5;
        $(".select2-container--default").css('width', width + 'px');
        $('.select2-selection__arrow').show();
    };
    rebindCSS();
});

function _fix() {
    var height = $(window).height();
    $(".wrapper").css("min-height", height + "px");
    var content = $(".wrapper").height();
    if (content > height)
        $(".left-side, html, body").css("min-height", height + "px");
    else {
        $(".left-side, html, body").css("min-height", height + "px");
    }
}

function fix_sidebar() {
    $(".sidebar").slimscroll({
        height: "600px",
        color: "rgba(0,0,0,0.2)"
    });
}

function selectRow(e, row, $element) {
    //Add by Uti Ridwan Ali 31 Maret 2017 : Untuk fixing Bugs Detail View bootstrap-tabe
    var $rowDetailView = $(row).parent().find(".detail-view");
    
    if ($rowDetailView.length) {
        if ($(row).hasClass("selecteds") == true)
            $(row).removeClass("selecteds");
        else {
            $("tbody > tr").removeClass("selecteds");
            $(row).addClass("selecteds");
        }

        return;
    }
    //End Add

    if ($(row).hasClass("selecteds") == true) {
        $(row).removeAttr("class");
    } else {
        $("tbody > tr").removeAttr("class");
        $(row).attr("class", "selecteds");
    }
};

function runningFormatter(value, row, index) {
    var size = $('.page-size').html();
    var page = $('#page').val() - 1;
    return (parseInt(size) * parseInt(page)) + index + 1;
};

function statusFormatter(value, row, index) {
    if (value == "1") {
        return "<div class='label label-success' style='white-space:nowrap;'>Active</div>";
    } else {
        return "<div class='label label-danger' style='white-space:nowrap;'>Inactive</div>";
    }
}

function YesNoFormatter(value, row, index) {
    if (value == "1") {
        return "<div class='label label-success' style='white-space:nowrap;'>Yes</div>";
    } else {
        return "<div class='label label-danger' style='white-space:nowrap;'>No</div>";
    }
}

var formatNumber = function (value, row, index) {
    if (value != null) {
        var arrayNumber = value.toString().split(".");
        if (arrayNumber.length > 1) {
            if (parseInt(arrayNumber[1]) == 0) {
                var number = parseInt(arrayNumber[0]);
                var array = number.toString().split('');
                var index = -3;
                while (array.length + index > 0) {
                    array.splice(index, 0, '.');
                    index -= 4;
                }
                return array.join('');
            } else {
                number = parseInt(arrayNumber[0]);
                array = number.toString().split('');
                index = -3;
                while (array.length + index > 0) {
                    array.splice(index, 0, '.');
                    index -= 4;
                }
                var joinNumber = array.join('');
                return joinNumber + "," + arrayNumber[arrayNumber.length - 1];
            }
        } else {
            if (isNaN(value)) return "";
            var str = new String(value);
            var result = "", len = str.length;
            for (var i = len - 1; i >= 0; i--) {
                if ((i + 1) % 3 == 0 && i + 1 != len) result += ".";
                result += str.charAt(len - 1 - i);
            }
            var value = result;
            return value;
        }
        return value;
    } else {
        return 0;
    }
};

function julianToDateFormatter(value, row, index) {
    if (value != "" || value != null) {
        var n = parseInt(value);
        var a = n + 32044;
        var b = Math.floor(((4 * a) + 3) / 146097);
        var c = a - Math.floor((146097 * b) / 4);
        var d = Math.floor(((4 * c) + 3) / 1461);
        var e = c - Math.floor((1461 * d) / 4);
        var f = Math.floor(((5 * e) + 2) / 153);

        var D = e + 1 - Math.floor(((153 * f) + 2) / 5);
        var M = f + 3 - 12 - Math.round(f / 10);
        var Y = (100 * b) + d - 4800 + Math.floor(f / 10);
        return new Date(Y, M, D);     
    }
}

function formatErrorMessage(jqXHR, exception) {
    if (jqXHR.status === 0) {
        return ('Not connected.\nPlease verify your network connection.');
    } else if (jqXHR.status == 404) {
        return ('The requested page not found. [404]');
    } else if (jqXHR.status == 500) {
        return ('Internal Server Error [500].');
    } else if (exception === 'parsererror') {
        return ('Requested JSON parse failed.');
    } else if (exception === 'timeout') {
        return ('Time out error.');
    } else if (exception === 'abort') {
        return ('Ajax request aborted.');
    } else {
        return ('Uncaught Error.\n' + jqXHR.responseText);
    }
};

function newDateFormatter(dt) {
    if (dt != '' && dt != ' ' && dt != null && dt != 'null') {
//        console.log(dt);
        var year = dt.substring(0, 4);
        var month = dt.substring(4, 6);
        var date = dt.substring(6, 8);
        return date + '/' + month + '/' + year;
    } else {
        return '-';
    }
}
function dateFormatterV2(dt) {
    if (dt == undefined || dt == 'undefined' || dt == null || dt == 'null' || dt == '-') return '';
    jsonDate = dt;
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    var d = new Date(parseInt(jsonDate.substr(6)));
    var m, day, monthIndex = d.getMonth();
    var mmm = monthNames[monthIndex];

    m = monthIndex + 1;
    if (m < 10)
        m = '0' + m
    if (d.getDate() < 10)
        day = '0' + d.getDate()
    else
        day = d.getDate();

    //var formattedDate = m + "-" + day + "-" + d.getFullYear();
    var formattedDate = d.getFullYear() + "-" + m + "-" + day;
    var formattedString = day + " " + mmm + " " + d.getFullYear();
    var hours = (d.getHours() < 10) ? "0" + d.getHours() : d.getHours();
    var minutes = (d.getMinutes() < 10) ? "0" + d.getMinutes() : d.getMinutes();
    var formattedTime = hours + ":" + minutes + ":" + d.getSeconds();
    formattedDate = formattedDate;// + " " + formattedTime;
    return formattedDate == undefined ? '-' : formattedDate;
};
function dateFormatter(dt) {
    if (dt == undefined || dt == 'undefined' || dt == null || dt == 'null' || dt == '-') return '';
    jsonDate = dt;
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    var d = new Date(parseInt(jsonDate.substr(6)));
    var m, day, monthIndex = d.getMonth();
    var mmm = monthNames[monthIndex];

    m = monthIndex + 1;
    if (m < 10)
        m = '0' + m
    if (d.getDate() < 10)
        day = '0' + d.getDate()
    else
        day = d.getDate();

    var formattedDate = m + "/" + day + "/" + d.getFullYear();
    var formattedString = day + " " + mmm + " " + d.getFullYear();
    var hours = (d.getHours() < 10) ? "0" + d.getHours() : d.getHours();
    var minutes = (d.getMinutes() < 10) ? "0" + d.getMinutes() : d.getMinutes();
    var formattedTime = hours + ":" + minutes + ":" + d.getSeconds();
    formattedDate = formattedDate;// + " " + formattedTime;
    return formattedDate == undefined ? '-' : formattedDate;
};

function dateFormatterCAT(dt) {
    if (dt != null && dt !="") {
        var formattedDate = moment(dt).format('DD MMM YYYY');
        return formattedDate;
    }
};

function dateStringFormatterCAT(dt) {
    if (dt != null && dt != "") {
        var Year = dt.substring(0, 4);
        var Month = dt.substring(6,4);
        var Day = dt.substring(8,6);
        return moment(Year + '-' + Month + '-' + Day).format('DD MMM YYYY');
    }
};

function dateTimeFormatterCAT(dt) {
    if (dt != null) {
        var formattedDate = moment(dt).format('DD MMM YYYY HH:mm:ss');
        return formattedDate;
    }
};

function sAlert(title, text, type) {
    swal({
        title: title,
        text: text,
        type: type,
        html: true
    });
};

function showMainScrollbar(v) {
    if (v) {
        $('html').css('overflow', '');
        $('body').css('overflow', '');
        $(".modal-dialog").css('width', '').css('top', '');
        $(".modal-dialog").removeClass("width").removeClass("top");
    }
    else {
        $('html').css('overflow', 'hidden');
        $('body').css('overflow', 'hidden');
        $('#myModalPlace').css('overflow', 'auto');
    }
};

function loadModal(url) {
    enableLink(false);
    $('#myModalContent').load(url, function () {
        $('html').css('overflow', 'hidden');
        $('body').css('overflow', 'hidden');

        $('#myModalPlace').modal({ keyboard: true }, 'show');
        bindForm(this);
        enableLink(true);
    });

    $('#myModalPlace').on('shown.bs.modal', function (e) {
        showMainScrollbar(false);
        $.fn.modal.Constructor.prototype.enforceFocus = function () { };
    });
    $('#myModalPlace').on('hidden.bs.modal', function (e) {
        showMainScrollbar(true);
    });
};

loadDetailPage = function (url, parentId, detailId) {
    var _prn = (parentId == undefined ? 'parent' : parentId);
    var _det = (detailId == undefined ? 'detail' : detailId);
    $(window).scrollTop(0);

    $("#" + _prn).focus();
    $("#" + _det).empty();
    $("#" + _det).show();
    $("#" + _det).focus();
    $("#" + _prn).hide();

    $.ajax({
        type: "GET",
        url: url,
        dataType: "html",
        async: false,
        cache: false,
        beforeSend: function () {
            enableLink(false);
            $("#" + _det).html("<div style=\"text-align:center;color:red\"><img src='/Content/images/ajax-loading.gif' style=\"padding-right:3px\">...Loading page...</div>");
        },
        success: function (data) {
            var _br = $('.box-header').length ? "" : "<br/>";
            $("#" + _det).empty();
            $("#" + _det).html(_br + data);

            if (typeof window.rebindCSS == "undefined") {
                alert("rebindCSS")
                $.getScript("/scripts/script.js", function () {
                });
            }
            else {
                rebindCSS();
            }
        }
    });
};

function enableLink(value) {
//    alert(value);
    if (value == true) {
        $('html, body').removeClass('wait');
        $('button').prop('disabled', false)
        $('.btn').removeClass('disabled');
        $('.fixed-table-loading').hide();
        $('.fixed-table-body-columns').removeClass('didi');
    }
    else {
        $('button').prop('disabled', true)
        $('.btn').addClass('disabled');

        if ($('.loadingImg', '.fixed-table-loading').length == 0) {
//            alert('a');
            $('.fixed-table-loading').prepend('<img class="loadingImg" src="/Content/images/ajax-loading.gif"/>&nbsp;')
            $('.fixed-table-body-columns').addClass('didi');
        }
        $('.fixed-table-loading').css('color', 'red');
        $('.fixed-table-loading').show();
    }
};

function reloadScripts(toRefreshList, key) {
    var scripts = document.getElementsByTagName('script');
    for (var i = 0; i < scripts.length; i++) {
        var aScript = scripts[i];
        for (var j = 0; j < toRefreshList.length; j++) {
            var toRefresh = toRefreshList[j];
            if (aScript.src && (aScript.src.indexOf(toRefresh) > -1)) {
                new_src = aScript.src.replace(toRefresh, toRefresh + '?k=' + key);
                aScript.src = new_src;
            }
        }
    }
}

(function (f) {
    jQuery.fn.extend({
        slimScroll: function (h) {
            var a = f.extend({ width: "auto", height: "250px", size: "7px", color: "#000", position: "right", distance: "1px", start: "top", opacity: 0.4, alwaysVisible: !1, disableFadeOut: !1, railVisible: !1, railColor: "#333", railOpacity: 0.2, railDraggable: !0, railClass: "slimScrollRail", barClass: "slimScrollBar", wrapperClass: "slimScrollDiv", allowPageScroll: !1, wheelStep: 20, touchScrollStep: 200, borderRadius: "0px", railBorderRadius: "0px" }, h);
            this.each(function () {
                function r(d) {
                    if (s) {
                        d = d ||
                                window.event;
                        var c = 0;
                        d.wheelDelta && (c = -d.wheelDelta / 120);
                        d.detail && (c = d.detail / 3);
                        f(d.target || d.srcTarget || d.srcElement).closest("." + a.wrapperClass).is(b.parent()) && m(c, !0);
                        d.preventDefault && !k && d.preventDefault();
                        k || (d.returnValue = !1)
                    }
                }
                function m(d, f, h) {
                    k = !1;
                    var e = d, g = b.outerHeight() - c.outerHeight();
                    f && (e = parseInt(c.css("top")) + d * parseInt(a.wheelStep) / 100 * c.outerHeight(), e = Math.min(Math.max(e, 0), g), e = 0 < d ? Math.ceil(e) : Math.floor(e), c.css({ top: e + "px" }));
                    l = parseInt(c.css("top")) / (b.outerHeight() - c.outerHeight());
                    e = l * (b[0].scrollHeight - b.outerHeight());
                    h && (e = d, d = e / b[0].scrollHeight * b.outerHeight(), d = Math.min(Math.max(d, 0), g), c.css({ top: d + "px" }));
                    b.scrollTop(e);
                    b.trigger("slimscrolling", ~~e);
                    v();
                    p()
                }
                function C() {
                    window.addEventListener ? (this.addEventListener("DOMMouseScroll", r, !1), this.addEventListener("mousewheel", r, !1), this.addEventListener("MozMousePixelScroll", r, !1)) : document.attachEvent("onmousewheel", r)
                }
                function w() {
                    u = Math.max(b.outerHeight() / b[0].scrollHeight * b.outerHeight(), D);
                    c.css({ height: u + "px" });
                    var a = u == b.outerHeight() ? "none" : "block";
                    c.css({ display: a })
                }
                function v() {
                    w();
                    clearTimeout(A);
                    l == ~~l ? (k = a.allowPageScroll, B != l && b.trigger("slimscroll", 0 == ~~l ? "top" : "bottom")) : k = !1;
                    B = l;
                    u >= b.outerHeight() ? k = !0 : (c.stop(!0, !0).fadeIn("fast"), a.railVisible && g.stop(!0, !0).fadeIn("fast"))
                }
                function p() {
                    a.alwaysVisible || (A = setTimeout(function () {
                        a.disableFadeOut && s || (x || y) || (c.fadeOut("slow"), g.fadeOut("slow"))
                    }, 1E3))
                }
                var s, x, y, A, z, u, l, B, D = 30, k = !1, b = f(this);
                if (b.parent().hasClass(a.wrapperClass)) {
                    var n = b.scrollTop(),
                            c = b.parent().find("." + a.barClass), g = b.parent().find("." + a.railClass);
                    w();
                    if (f.isPlainObject(h)) {
                        if ("height" in h && "auto" == h.height) {
                            b.parent().css("height", "auto");
                            b.css("height", "auto");
                            var q = b.parent().parent().height();
                            b.parent().css("height", q);
                            b.css("height", q)
                        }
                        if ("scrollTo" in h)
                            n = parseInt(a.scrollTo);
                        else if ("scrollBy" in h)
                            n += parseInt(a.scrollBy);
                        else if ("destroy" in h) {
                            c.remove();
                            g.remove();
                            b.unwrap();
                            return
                        }
                        m(n, !1, !0)
                    }
                } else {
                    a.height = "auto" == a.height ? b.parent().height() : a.height;
                    n = f("<div></div>").addClass(a.wrapperClass).css({
                        position: "relative",
                        overflow: "hidden", width: a.width, height: a.height
                    });
                    b.css({ overflow: "hidden", width: a.width, height: a.height });
                    var g = f("<div></div>").addClass(a.railClass).css({ width: a.size, height: "100%", position: "absolute", top: 0, display: a.alwaysVisible && a.railVisible ? "block" : "none", "border-radius": a.railBorderRadius, background: a.railColor, opacity: a.railOpacity, zIndex: 90 }), c = f("<div></div>").addClass(a.barClass).css({
                        background: a.color, width: a.size, position: "absolute", top: 0, opacity: a.opacity, display: a.alwaysVisible ?
                                    "block" : "none", "border-radius": a.borderRadius, BorderRadius: a.borderRadius, MozBorderRadius: a.borderRadius, WebkitBorderRadius: a.borderRadius, zIndex: 99
                    }), q = "right" == a.position ? { right: a.distance } : { left: a.distance };
                    g.css(q);
                    c.css(q);
                    b.wrap(n);
                    b.parent().append(c);
                    b.parent().append(g);
                    a.railDraggable && c.bind("mousedown", function (a) {
                        var b = f(document);
                        y = !0;
                        t = parseFloat(c.css("top"));
                        pageY = a.pageY;
                        b.bind("mousemove.slimscroll", function (a) {
                            currTop = t + a.pageY - pageY;
                            c.css("top", currTop);
                            m(0, c.position().top, !1)
                        });
                        b.bind("mouseup.slimscroll", function (a) {
                            y = !1;
                            p();
                            b.unbind(".slimscroll")
                        });
                        return !1
                    }).bind("selectstart.slimscroll", function (a) {
                        a.stopPropagation();
                        a.preventDefault();
                        return !1
                    });
                    g.hover(function () {
                        v()
                    }, function () {
                        p()
                    });
                    c.hover(function () {
                        x = !0
                    }, function () {
                        x = !1
                    });
                    b.hover(function () {
                        s = !0;
                        v();
                        p()
                    }, function () {
                        s = !1;
                        p()
                    });
                    b.bind("touchstart", function (a, b) {
                        a.originalEvent.touches.length && (z = a.originalEvent.touches[0].pageY)
                    });
                    b.bind("touchmove", function (b) {
                        k || b.originalEvent.preventDefault();
                        b.originalEvent.touches.length &&
                                (m((z - b.originalEvent.touches[0].pageY) / a.touchScrollStep, !0), z = b.originalEvent.touches[0].pageY)
                    });
                    w();
                    "bottom" === a.start ? (c.css({ top: b.outerHeight() - c.outerHeight() }), m(0, !0)) : "top" !== a.start && (m(f(a.start).position().top, null, !0), a.alwaysVisible || c.hide());
                    C()
                }
            });
            return this
        }
    });
    jQuery.fn.extend({ slimscroll: jQuery.fn.slimScroll })
})(jQuery);
