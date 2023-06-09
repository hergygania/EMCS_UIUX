var $table = $('#tableVideo');

$(function () {

    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: false,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        sidePagination: 'server',
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        //fixedColumns: true,
        //fixedNumber: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'ID',
            title: 'Action',
            //width: '180px',
            align: 'center',
            formatter: operateFormatter({ Edit: true, Delete: true, Preview: true }),
            events: operateEvents
        },
        {
            field: 'Name',
            title: 'Name',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'StartedDate',
            title: 'Start Date',
            halign: 'center',
            align: 'center',
            sortable: true,
            formatter: function (data) {
                return moment(data).format("DD MMM YYYY");
            }
        },
        {
            field: 'FinishedDate',
            title: 'Finish Date',
            halign: 'center',
            align: 'center',
            sortable: true,
            formatter: function (data) {
                return moment(data).format("DD MMM YYYY");
            }
        },
        {
            field: 'IsActive',
            title: 'Is Active',
            halign: 'center',
            align: 'center',
            sortable: true,
            formatter: function (data) {
                if (data) {
                    return "<span class='label label-success'>Yes</span>";
                } else {
                    return "<span class='label label-danger'>No</span>";
                }
            }
        },
        {
            field: 'Video',
            title: 'Video',
            halign: 'center',
            align: 'center',
            sortable: true,
            events: operateEvents,
            formatter: function (data) {
                var htm = "<div class='center'>";
                if (data !== "") {
                    htm += "<button class='btn btn-danger btn-link preview'><i class='tim-icons icon-video-66'></i></button>";
                } else {
                    htm += "-";
                }
                htm += "</div>";
                return htm;
            }
        }]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/emcs/VideoPage',
        urlPaging: '/emcs/VideoPageXt',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"));

});


function operateFormatter(options) {
    var btn = [];
    btn.push('<div class="">');
    if (options.Add === true)
        btn.push('<button type="button" class="btn btn-success btn-link new" title="Add"><i class="tim-icons icon-simple-add"></i></button>');
    if (options.Edit === true)
        btn.push('<button type="button" class="btn btn-info btn-link btn-xs edit" title="Edit"><i class="tim-icons icon-pencil"></i></button>\
            <button type="button" class="btn btn-success btn-link btn-xs upload" title="Upload"><i class="tim-icons icon-cloud-upload-94"></i></button>');
    //if (options.Preview === true)
    //    btn.push('<button type="button" class="btn btn-default info" title="Info"><i class="fa fa-eye"></i></button>');
    //if (options.Upload === true)
    //    btn.push('<button type="button" class="btn btn-primary upload" title="Upload"><i class="fa fa-upload"></i></button>');
    if (options.Delete === true)
        btn.push('<button type="button" class="btn btn-danger btn-link btn-xs remove" title="Delete"><i class="tim-icons icon-simple-remove"></i></button>');
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
        $(".editRecord").attr('href', '/EMCS/VideoEdit/' + row.Id).trigger('click');
    },
    'click .preview': function (e, value, row) {
        $(".previewVideo").attr('href', '/EMCS/PreviewVideo/' + row.Id).trigger('click');
    },
    'click .upload': function (e, value, row) {
        $(".uploadRecord").attr('href', '/EMCS/VideoUpload/' + row.Id).trigger('click');
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
                return deleteThis(row.Id);
            }
            return false;
        });
    }
};

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'EMCS/VideoDelete',
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


//Dropzone.autoDiscover = false;

//var myDropzone = new Dropzone("#FormUploadVideo", { // Make the bodyFormUpload a dropzone
//    url: "/EMCS/UploadVideo", // Set the url
//    thumbnailHeight: 100,
//    thumbnailWeight: 100,
//    timeout: "80000",
//    method: 'POST',
//    dictDefaultMessage: "<h4>Drop the Import File Here or Click this Section for Browse the Import Video.</h4>",
//    acceptedFiles: '.mpg, .wmv, .mov, .rm, .avi, .flv, .mp4',
//    filesizeBase: 1024,
//    autoProcessQueue: true,
//    maxFiles: 1,
//    maxFilesize: 100, // MB
//    parallelUploads: 1,
//    previewTemplate: $("#template-dropzone").html(),
//    uploadMultiple: false
//    //previewsContainer: "#FormUploadMaterial", // Define the container to display the previews
//    //clickable: ".fileinput-button" // Define the element that should be used as click trigger to select files.
//});

//myDropzone.on("addedfile", function (file) {
//    // Hookup the start button
//    $("#actions .start").on("click", function () {
//        myDropzone.enqueueFile(file);
//    });
//    $("#placeholderUpload").hide();
//});

//myDropzone.on("totaluploadprogress", function (progress) {
//    $("#total-progress .progress-bar").css("width", progress + "%");
//    $("#progressPercent").html(progress + "%");
//});

//myDropzone.on("sending", function (file, xhr, formData) {
//    // Show the total progress bar when upload starts
//    $("#total-progress").css("opacity", 1);
//    // And disable the start button
//    $("#actions .delete").attr("disabled", "disabled");
//    $(".start").attr("disabled", "disabled");
//});

//myDropzone.on("complete", function (resp) {
//    if (resp.status === "success") {
//        var respText = resp.xhr.response;
//        var respData = JSON.parse(respText);
//        var failedList = respData.failedList;
//        console.log(respData);

//        $("#actions .delete").prop("disabled", false);
//        var type = respData.status ? "success" : "error";
//        swal("Upload Status", respData.msg, type);
//    }
//});

//myDropzone.on("queuecomplete", function (progress) {
//    $("#total-progress").css("opacity", "0");
//    setTimeout(function () {
//        myDropzone.removeAllFiles(true);
//    }, 400);
//});

//$("#actions .start").on("click", function () {
//    myDropzone.enqueueFiles(myDropzone.getFilesWithStatus(Dropzone.QUEUED));
//});

//$("#actions .cancel").on("click", function () {
//    myDropzone.removeAllFiles(true);
//    $("#placeholderUpload").hide();
//});
