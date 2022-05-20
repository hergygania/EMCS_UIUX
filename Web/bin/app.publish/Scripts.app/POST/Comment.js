function BaseUrl() {
    var baseUrl = $("#BaseUrl").val();
    return baseUrl;
}

function renderComment(data) {
    var listComment = "";
    jQuery.each(data, function (index, element) {
        var waktu = moment(element.CreatedOn).format("MMM D, YYYY, h:mm:ss a");
        var commentItem = `<article class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <header class="text-left">
                                <div class="comment-user"><h5><b><i class="fa fa-user"></i>&nbsp;<span class="comment-user-content">`+ element.Username + `</span></b></h5></div>
                                <time class="comment-date" datetime="`+ waktu + `"><i class="fa fa-clock"></i> ` + waktu + `</time>
                            </header>
                            <div class="comment-post">
                                <p>`+ element.Comment + `</p>
                            </div>
                        </div>
                    </div>
                </div></article>`;
        listComment += commentItem;
    })
    $(".comment-list").html(listComment);
}

function getListComment(ReqId) {
    $.ajax({
        url: BaseUrl() + "ReadData",
        type: "GET",
        data: { RequestId: ReqId },
        success: function (resp) {
            var data = resp.result;
            renderComment(data);
        },
        error: function (err) {
            swal("Opps!", "Something Wrong when submit the comment!, Please Try Again", "error");
        },
        failed: function (fail) {
            swal("Opps!", "Something Wrong when submit the comment!, Please Try Again", "error");
        }
    })
}

function submitComment() {
    tinyMCE.triggerSave(true, true);
    var comment = '';
  
    comment = tinyMCE.editors.comment_content.getContent();
    var data = {
        Comment: comment,
        RequestId: $("#form_req_id").val()
    }
    $.ajax({
        url: BaseUrl() + "CreateComment",
        method: 'POST',
        contentType: 'application/json;charset=utf-8',
        data: JSON.stringify(data),
        type: 'json',
       
        success: function (resp) {
            var RequestId = $("#form_req_id").val();
            getListComment(RequestId);
            UpdateTotalComment(RequestId);
            //$("#comment_content").val("");
            tinyMCE.editors.comment_content.setContent('');
        },
        error: function (err) {
            swal("Opps!", "Something Wrong when submit the comment!, Please Try Again", "error");
        },
        failed: function (fail) {
            swal("Opps!", "Something Wrong when submit the comment!, Please Try Again", "error");
        }
    })
}

function GetTotalUnread(PoList) {
    var ReqIds = PoList;
    $.ajax({
        url: BaseUrl() + "GetTotalUnread",
        type: "POST",
        data: { RequestId: ReqIds },
        success: function (resp) {
            if (resp.status === "SUCCESS") {
                if (resp.result.length > 0) {
                    jQuery.each(resp.result, function (index, value) {
                        if (value.TotalUnread > 0) {
                            var item = $(".badge-custom-" + value.RequestId);
                            item.html("&nbsp" + value.TotalUnread);
                            item.show();
                        }
                    });
                }
            }
        },
        error: function (err) {
            swal("Opps!", "Something Wrong when submit the comment!, Please Try Again", "error");
        },
        failed: function (fail) {
            swal("Opps!", "Something Wrong when submit the comment!, Please Try Again", "error");
        }
    })
}

function GetTotalComment(PoList) {
    var ReqIds = PoList;
    $.ajax({
        url: BaseUrl() + "GetTotalComment",
        type: "POST",
        data: { RequestId: ReqIds },
        success: function (resp) {
            if (resp.status === "SUCCESS") {
                if (resp.result.length > 0) {
                    jQuery.each(resp.result, function (index, value) {
                        if (value.TotalComment > 0) {
                            var itemTotal = $(".total-comment-" + value.RequestId);
                            itemTotal.html("&nbsp" + value.TotalComment);
                            itemTotal.show();
                        }
                    });
                }
            }
        },
        error: function (err) {
            swal("Opps!", "Something Wrong when submit the comment!, Please Try Again", "error");
        },
        failed: function (fail) {
            swal("Opps!", "Something Wrong when submit the comment!, Please Try Again", "error");
        }
    })
}

function UpdateTotalComment(Id) {
    $.ajax({
        url: BaseUrl() + "GetTotalCommentById",
        type: "POST",
        data: {
            RequestId: Id
        },
        success: function (resp) {
            var RequestId = $("#form_req_id").val();
            getListComment(RequestId);
            if (resp.status === "SUCCESS") {
                $(".total-comment-" + RequestId).html("&nbsp;" + resp.result);
            }
            $("#comment_content").val("");
        },
        error: function (err) {
            swal("Opps!", "Something Wrong when submit the comment!, Please Try Again", "error");
        },
        failed: function (fail) {
            swal("Opps!", "Something Wrong when submit the comment!, Please Try Again", "error");
        }
    })
}

function ShowComment(row, po_no, type) {
    getListComment(row);
    $(".badge-custom-" + row).hide();
    $("#form_req_id").val(row);
    $("#modal_po_number").html(po_no);
    $("#modalComment").modal("show");
}

$(function () {    
    $("#submit_comment").on("click", function () {
        submitComment();
        return false;
    })

})
tinymce.init({
    selector: "textarea#comment_content",  // change this value according to your HTML
    plugins: "table",
    menubar: "table",
    //toolbar: "table",
    table_default_attributes: {
        border: '1'
    },
    setup: function (editor) {
        editor.on('init', function (e) {
            tinyMCE.editors.comment_content.setContent('');
        });
    }

})
