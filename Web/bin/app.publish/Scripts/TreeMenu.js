'use strict';
$.AdminLTE = {};

$.AdminLTE.options = {
    navbarMenuSlimscroll: true,
    navbarMenuSlimscrollWidth: "3px",
    navbarMenuHeight: "200px",
    animationSpeed: 500,
    sidebarToggleSelector: "[data-toggle='offcanvas']",
    sidebarPushMenu: true,
    sidebarSlimScroll: true,
    sidebarExpandOnHover: false,
    enableBoxRefresh: true,
    enableBSToppltip: true,
    BSTooltipSelector: "[data-toggle='tooltip']",
    enableFastclick: true,
    enableControlSidebar: true,
    controlSidebarOptions: {
        toggleBtnSelector: "[data-toggle='control-sidebar']",
        selector: ".control-sidebar",
        slide: true
    }
};

function _init() {
    $.AdminLTE.tree = function (menu) {
        var _this = this;
        var animationSpeed = $.AdminLTE.options.animationSpeed;

        var checkCookie = $.cookie("nav-item");
        $.cookie("nav-item", null);

        $('.sidebar-menu > li > a').click(function () {
            var navIndex = $('.sidebar-menu > li > a').index(this);
            $.cookie("nav-item", navIndex);
        });
        /*
        if (checkCookie != "") {
            $('.sidebar-menu > li:eq(' + checkCookie + ')').addClass('active').next().show();
        }
        */
        if (checkCookie != "") {
            if (checkCookie == null) {
                $('.sidebar-menu > li:eq(1)').addClass('active').next().show();
            } else {
                $('.sidebar-menu > li:eq(' + (parseInt(checkCookie) + 1) + ')').addClass('active').next().show();
            }
        }

        $("li a", $(menu)).on('click', function (e) {
            var $this = $(this);
            var checkElement = $this.next();

            if ((checkElement.is('.treeview-menu')) && (checkElement.is(':visible'))) {
                checkElement.slideUp(animationSpeed, function () {
                    checkElement.removeClass('menu-open');
                });
                checkElement.parent("li").removeClass("active");
            }
            else if ((checkElement.is('.treeview-menu')) && (!checkElement.is(':visible'))) {
                var parent = $this.parents('ul').first();
                var ul = parent.find('ul:visible').slideUp(animationSpeed);
                ul.removeClass('menu-open');
                var parent_li = $this.parent("li");
                checkElement.slideDown(animationSpeed, function () {
                    checkElement.addClass('menu-open');
                    parent.find('li.active').removeClass('active');
                    parent_li.addClass('active');
                });
            }
            if (checkElement.is('.treeview-menu')) {
                e.preventDefault();
            }
        });
    };
}