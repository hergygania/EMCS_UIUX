var isRead = -1, isDelete = 0;

$(document).ready(function () {
    getNotificationCount();

    $("#tbl-notification").bootstrapTable({
        url: myApp.fullPath + "/EMCS/GetNotificationList",
        pagination: true,
        cache: false,
        showRefresh: true,
        smartDisplay: false,
        search: true,
        pageSize: 10,
        striped: true,
        theadClasses: "hidden",
        sidePagination: "client",
        toolbar: ".notification-toolbar",
        queryParams: function (param) {
            param.isRead = isRead;
            param.isDelete = isDelete;
            return param;
        },
        formatNoMatches: function () {
            return '<span class="noMatches">No Item Found</span>';
        },
        columns: [
            {
                field: "NotificationCheck",
                checkbox: true
            }, {
                field: "Module",
                width: '30%'
            }, {
                field: "NotificationSubject",
                width: '70%',
                events: operateEvents,
                formatter: function (data, row) {
                    return "<a href='#' class='redirect'>" + row.NotificationSubject + " - " + $(row.NotificationContent).text().substring(0, 50) + "... </a>";
                }
            }
        ]
    });
});

window.operateEvents = {
    'click .redirect': function (e, value, row) {
        var dataIDs = [];
        dataIDs.push(row.Id);
        $.ajax({
            url: '/EMCS/UpdateNotification?Id=' + dataIDs + '&isRead=1&isDelete=' + (row.IsDelete ? 1 : 0),
            success: function () {
                $('#NotificationList').addClass('hidden');
                $('#NotificationDetail').removeClass('hidden');
                $(".mailbox-read-message").html("");
                $(".mailbox-read-message").append(row.NotificationContent);

                $('#NotificationID').val(row.Id);
                $('#NotificationSubject').text(row.NotificationSubject);
                $('#NotificationDate').text(moment(row.CreatedDate).format('DD MMM YYYY HH:MM'));
                getNotificationCount();
            }
        });
    }
};

function backToList() {
    $('#tbl-notification').bootstrapTable('uncheckAll');
    $('#NotificationList').removeClass('hidden');
    $('#NotificationDetail').addClass('hidden');

    $.each($(".folder"), function (i, item) {
        if ($(item).hasClass('active'))
            reloadData($(item).data('value'));
    });
}

function remove() {
    var dataIDs = [];

    if ($('#NotificationList').hasClass('hidden')) {
        dataIDs.push($('#NotificationID').val());
    } else {
        $.each($('#tbl-notification').bootstrapTable('getSelections'), function (i, data) {
            dataIDs.push(data.Id);
        });
    }

    if (dataIDs.length > 0) {
        swal({
            title: "Are you sure want to delete the item(s)?",
            type: "warning",
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            showCancelButton: true,
            closeOnConfirm: false,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: 'POST',
                    url: '/EMCS/UpdateNotification',
                    data: {
                        Id: dataIDs,
                        isRead: 0,
                        isDelete: 1
                    }
                })
                    .done(function () {
                        swal("Deleted!", "Your data has been deleted.", "success");
                    })
                    .fail(function (jqXhr) {
                        var result = $.parseJSON(jqXhr.responseText);
                        sweetAlert("Failed", result.ExceptionMessage + "&#13;&#10;", "error");
                    })
                    .always(function () {
                        $('#NotificationList').removeClass('hidden');
                        $('#NotificationDetail').addClass('hidden');

                        $.each($(".folder"), function (i, item) {
                            if ($(item).hasClass('active'))
                                reloadData($(item).data('value'));
                        });
                        getNotificationCount();
                        $('#tbl-notification').bootstrapTable('uncheckAll');
                    });
            }
        });
    }
}

function showNotificationControl(isShow) {
    if (isShow) {
        $('.notification-control').removeClass('hidden');
        //$('.trash-control').addClass('hidden');
    }
    else {
        $('.notification-control').addClass('hidden');
        //$('.trash-control').removeClass('hidden');
    }
}

$(".checkbox-toggle").click(function () {
    var clicks = $(this).data('clicks');
    if (clicks) {
        //Uncheck all checkboxes
        $('#tbl-notification').bootstrapTable('uncheckAll');
        $(".fa", this).removeClass("fa-check-square-o").addClass('fa-square-o');
    } else {
        //Check all checkboxes
        $('#tbl-notification').bootstrapTable('checkAll');
        $(".fa", this).removeClass("fa-square-o").addClass('fa-check-square-o');
    }
    $(this).data("clicks", !clicks);
});

$(".folder").click(function (e) {
    e.stopPropagation();
    e.preventDefault();

    $(".folder").removeClass('active');
    $(this).addClass('active');

    $('#tbl-notification').bootstrapTable($(this).data("value") !== 'trash' ? 'showColumn' : 'hideColumn', "NotificationCheck");
    reloadData($(this).data("value"));

    $('#NotificationList').removeClass('hidden');
    $('#NotificationDetail').addClass('hidden');
});

function reloadData(folder) {

    switch (folder) {
        case 'read':
            showNotificationControl(true);
            isRead = 1, isDelete = 0;
            break;
        case 'unread':
            showNotificationControl(true);
            isRead = 0, isDelete = 0;
            break;
        case 'trash':
            showNotificationControl(false);
            isRead = -1, isDelete = 1;
            break;
        default:
            showNotificationControl(true);
            isRead = -1, isDelete = 0;
    }
    var url = myApp.fullPath + "/EMCS/GetNotificationList?isRead=" + isRead + "&isDelete=" + isDelete;

    $.ajax({
        url: url,
        success: function (data) {
            $('#tbl-notification').bootstrapTable('removeAll');
            $("#tbl-notification").bootstrapTable('load', data);
            getNotificationCount();
        }
    });
}

function getNotificationCount() {
    $.ajax({
        url: myApp.fullPath + "/EMCS/GetNotificationList?isRead=-1&isDelete=0",
        success: function (notification) {
            $.ajax({
                url: myApp.fullPath + "/EMCS/GetNotificationList?isRead=0&isDelete=0",
                success: function (unread) {
                    $('#TotalNotification').text(notification.length);
                    $('#TotalUnread').text(unread.length);
                }
            });
        }
    });
}

