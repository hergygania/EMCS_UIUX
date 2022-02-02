var $tableGr = $("#TblGoodReceive");

window.operateEventRight = {
    'click .download': function (e, value, row) {
        e.preventDefault();
        location.href = myApp.fullPath + "/EMCS/DownloadGrItem/" + row.Id;
    },
    'click .ShowDocument': function (e, value, row) {
        e.preventDefault();
        const url = `/Upload/EMCS/GR/${row.IdGr}/${value}`;
        // ReSharper disable once UseOfImplicitGlobalInFunctionScope
        showPreviewDocument(url);
    }
};

var columns = [
    {
        field: "Id",
        visible: true,
        align: "center",
        width: "20",
        title: "No",
        formatter: runningFormatter
    }, {
        field: "Category",
        visible: true,
        title: "Category",
        halign: "center",
        formatter: function (data, row) {
            return data + " - " + row.CategoriItem;
        }
    }, {
        field: "DoNo",
        visible: true,
        title: "EDI Number",
        halign: "center"
    }, {
        field: "DaNo",
        title: "DO Reference",
        align: "left",
        halign: "center",
        valign: "center",
        sortable: true,
        class: "text-nowrap"

    }, {
        field: "FileName",
        title: "Attachment",
        align: "center",
        valign: "center",
        halign: "center",
        sortable: true,
        events: operateEventRight,
        formatter: function (data, row) {
            if (row.FileName !== "") {
                const divOpen = "<div class='text-center' style='width:100%;'>";
                const btnDownload =
                    "<button class='btn btn-xs btn-success download' style='display:inline;' type='button'><i class='fa fa-download'></i></button>";
                const btnPreview =
                    "<button class='btn btn-xs btn-primary ShowDocument' style='display:inline; type='button'><i class='fa fa-file-pdf-o'></i></button>";
                const divClose = "</div>";
                return [divOpen, btnDownload, btnPreview, divClose].join(" ");
            } else {
                return "-";
            }
        },
        class: "text-nowrap"
    }
];

function operateFormatter(options) {
    var btn = [];
    btn.push('<div class="btn-group">');
    if (options.Add === true)
        btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>');
    if (options.Edit === true)
        btn.push('<button type="button" class="btn btn-info edit" title="Edit"><i class="fa fa-edit"></i></button>');
    if (options.Upload === true)
        btn.push('<button type="button" class="btn btn-primary upload" title="Upload"><i class="fa fa-upload"></i></button>');
    if (options.Delete === true)
        btn.push('<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    btn.push('</div>');
    return btn.join('');
}

operateFormatter.DEFAULTS = {
    Add: false,
    Edit: false,
    Delete: false,
    Info: false,
    View: false,
    History: false,
    Preview: false,
    Upload: false
};

window.operateEvents = {
    'click .edit': function (e, value, row) {
        $(".editRecord").attr('href', '/EMCS/BannerEdit/' + row.ID).trigger('click');
    },
    'click .preview': function (e, value, row) {
        $(".previewImages").attr('href', '/EMCS/PreviewImage/' + row.ID).trigger('click');
    },
    'click .upload': function (e, value, row) {
        $(".uploadRecord").attr('href', '/EMCS/BannerUpload/' + row.ID).trigger('click');
    },
    'click .remove': function (e, value, row) {
        swal({
            title: "Are you sure want to delete this data?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#F56954",
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            closeOnConfirm: false,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                sweetAlert.close();
                return deleteThis(row.ID);
            }
            return false;
        });
    }
};

$(function () {
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $tableGr.bootstrapTable({
        url: myApp.fullPath + "EMCS/GetItemGr",
        cache: false,
        pagination: true,
        search: false,
        striped: false,
        showRefresh: true,
        queryParams: function () {
            return { Id: $("#IdGr").val() };
        },
        clickToSelect: false,
        sidePagination: 'server',
        responseHandler: function (resp) {
            console.log(resp);
            var data = {};
            $.map(resp.data, function (value) {
                data[value.Key] = value.Value;
            });
            return data;
        },
        showColumns: false,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">No data found</span>';
        },
        columns: columns
    });

    $("#mySearch").insertBefore($("[name=refresh]"));

});

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'EMCS/BannerDelete',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: { ID: id },
        dataType: "json",
        success: function (d) {
            if (d.Msg !== undefined) {
                sAlert('Success', d.Msg, 'success');
            }

            $("[name=refresh]").trigger('click');
        },
        error: function (jqXhr) {
            sAlert('Error', jqXhr.status + " " + jqXhr.statusText, "error");
        }
    });

};

$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function () {
        $('#myModalContent').load(this.href, function () {
            $('#myModalPlace').modal({
                keyboard: true
            }, 'show');

            bindForm(this);
        });
        return false;
    });
});

function ApproveGR(obj) {
    var id = $("#IdGr").val();
    $.ajax({
        url: "/EMCS/GRApprove",
        type: "POST",
        data: {
            data: {
                Id: id,
                Status: obj.Status,
                Notes: obj.Notes
            },
            Detail: {
                ReqType: "GR",
                IdRequest: id,
                Category: obj.Category,
                Case: obj.Case,
                Causes: obj.Causes,
                Impact: obj.Impact,
                CaseDate: "20 Jan 2019"
            }
        },
        success: function () {
            location.href = "/EMCS/GrList";
        }
    });
}

//#region Button Action
$("#BtnApprove").on("click", function () {
    Swal.fire({
        title: 'Approve Confirmation',
        text: 'Do you want to continue ?',
        type: 'question',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonColor: '#3085d6',
        confirmButtonText: 'Yes, Approve!',
        allowEscapeKey: false,
        allowOutsideClick: false,
        showCloseButton: true
    }).then((result) => {
        if (result.value) {
            Swal.fire({
                input: 'textarea',
                allowEscapeKey: false,
                allowOutsideClick: false,
                inputPlaceholder: 'Type your notes here...',
                inputAttributes: {
                    'aria-label': 'Type your notes here'
                },
                showCancelButton: false
            }).then((result) => {
                var notes = result.value;
                var status = "Approve";
                var idGr = $("#IdGr").val();
                var data = { Notes: notes, Status: status, IdGR: idGr };

                ApproveGR(data);
            });
        }
        return false;
    });
});
//#endregion

function SubmitData() {
    $.ajax({
        url: "/emcs/creategr",
        type: "POST",
        data: $("#FormGR").serialize(),
        success: function (result) {
            if (result.Status === 0) {
                if (result.Msg !== undefined) sAlert('Success', result.Msg, 'success');
                var data = result.result;
                console.log(data);
                $("#GrNo").val(data.GrNo);
                $("#IdGr").val(data.Id);
                $('#progress').hide();
                $("[name=refresh]").trigger('click');

                var stat = $("#Status").val();
                if (stat.toLowerCase() === "submit") {
                    location.href = "/emcs/grlist";
                }
            }
            else {
                if (result.Msg !== undefined) sAlert('Failed', result.Msg, 'error');
                $('#progress').hide();
            }
            return false;
        }
    });
}

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $('#progress').show();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.Status === 0) {
                    if (result.Msg !== undefined) sAlert('Success', result.Msg, 'success');
                    $('#myModalPlace').modal('hide');
                    $('#progress').hide();
                    $("[name=refresh]").trigger('click');
                }
                else {
                    if (result.Msg !== undefined) sAlert('Failed', result.Msg, 'error');
                    $('#progress').hide();
                }
            }
        });
        return false;
    });
};

function ShowModal() {
    $("#myModal").modal("show");
}

$(document).ready(function () {
    $(".date").datepicker({
        autoclose: true,
        format: "dd M yyyy"
    });
    $("form").removeData("validator");
    $("form").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse("form");
});