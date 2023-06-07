function displayNotification() {
    $.ajax({
        url: myApp.fullPath + "/EMCS/GetNotificationList?isRead=0&isDelete=0",
        success: function (unread) {
            if (unread.length > 0) {
                $('.menu-enhance li a:contains("Notification")').append("<span class='badge pull-right'>" + unread.length + "</span>");

                $('.content-header ol').prepend("<a href='" + myApp.fullPath + "/EMCS/Notification'><i class='fa fa-bell-o'></i><span class='label label-warning'>" + unread.length + "</span></a>&nbsp;&nbsp;|&nbsp;&nbsp;");
            }
        }
    });
};

$(function () {
    displayNotification();
    // ReSharper disable once StatementTermination
});