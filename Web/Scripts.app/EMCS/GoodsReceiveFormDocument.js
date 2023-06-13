$tablegrdocument = $('#tablegrDocuments');

//function load_data_tablegr() {
var columnDocument = [
    {
        field: '',
        title: 'Action',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        events: operateEvents,
        formatter: function (data, row, index) {
            return operateFormatter(data, row, index);
        },

    },
    {
        field: 'Id',
        title: 'No',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        visible: false
    },
    {
        field: 'IdGr',
        title: 'RGBastid',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        visible: false
    },
    {
        field: 'DocumentDate',
        title: 'Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'DocumentName',
        title: 'Document Name',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Filename',
        title: 'Attachment',
        align: 'center',
        valign: 'center',
        halign: "center",
        class: 'text-nowrap',
        sortable: true,
        events: operateEventRight,
        formatter: function (data, row) {
            if (row.Filename !== "") {
                var btnDownload = "<button class='btn btn-xs btn-success downloaddoc' type='button'><i class='fa fa-download'></i></button>";
                var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocumentdoc' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='fa fa-file-pdf-o'></i></button>";
                return [btnDownload, btnPreview].join(' ');
            } else {
                return "-";
            }
        },
        class: 'text-nowrap'
    }];
$tablegrdocument.bootstrapTable({
    columns: columnDocument,
    cache: false,
    search: false,
    striped: false,
    clickToSelect: true,
    reorderableColumns: true,
    toolbar: '.toolbarDocument',
    toolbarAlign: 'left',
    onClickRow: selectRow,
    showColumns: true,
    showRefresh: false,
    smartDisplay: false,
    pagination: true,
    sidePagination: 'client',
    pageSize: '5',
    formatNoMatches: function () {
        return '<span class="noMatches">No data found</span>';
    },
});
//window.operateEventRight = {
//    'click .downloaddoc': function (e, value, row) {
//        location.href = "/EMCS/DownloadArmadaDocument/" + row.Id;
//    },
//    'click .showDocumentdoc': function (e, value, row) {
//        document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/GoodsReceive/" + row.Id + '/' + row.Filename;
//    }
//};

operateFormatter.DEFAULTS = {
    Add: false,
    Edit: false,
    Delete: false,
    Info: false,
    View: false,
    History: false
}

function operateFormatter(data, row, index) {
    var btn = [];
    btn.push('<div class="btn-toolbar row">');
    btn.push('<button type="button" class="btn btn-info btn-xs editDocument" id="editDocument" data-toggle="modal" data-target="#myModalDocument" title="Edit" > <i class="fa fa-edit"></i></button >\
            <button type="button" class="btn btn-primary btn-xs uploadDocument" data-toggle="modal" data-target="#myModalUploadPlace" title="Upload"><i class="fa fa-upload"></i></button>');
    btn.push('<button type="button" class="btn btn-danger btn-xs removeDocument" id="removeDocument" title="Delete"><i class="fa fa-trash-o"></i></button>');
    btn.push('</div>');
    return btn.join('');
}
/*}*/
//function operateEvents() {
//    $('.editDocument').on('click', function (e, value, row, index) {
//        alert();
//        $('#Id').val(row.Id);
//        $('#inp-doc-date').val(row.DocumentDate);
//        $('#DocumentName').val(row.DocumentName);
//    })
//    $('.removeDocument').on('click', function (e, value, row, index) {

//        GrDocumentDeleteById(row.Id);
//        get_grdocumentlist();
//    })
//    $('.uploadDocument').on('click ', function (e, value, row, index) {

//        $('#IdDocumentUpload').val(row.Id);
//        //$(".uploadRecord").attr('href', '/EMCS/CiplDocumentUpload/' + row.Id).trigger('click');
//    })
//}
//function operateEventRight() {
//    $('.downloaddoc').on('click', function (e, value, row) {
//        location.href = "/EMCS/DownloadGrItem/" + row.Id;
//    })
//    $('.showDocumentdoc').on('click', function (e, value, row) {
//        $(".PreviewFile").attr('href', '/EMCS/PreviewGrItem?Id=' + row.Id).trigger('click');
//    })
//}

function ajaxInsertUpdateDocument() {
    
    var json = new Array();
    json.push({
        Id: $('#IdGrDocument').val(),
        IdGr: $('#IdGr').val(),
        DocumentDate: $('#inp-doc-date').val(),
        DocumentName: $('#DocumentName').val()
    });
    $.ajax({
        url: '/EMCS/GRDocumentInsert',
        type: 'POST',
        data: {
            data: json
        },
        cache: false,
        async: false,
        success: function (data, response) {
            Swal.fire({
                type: 'success',
                title: 'Success'
                , text: 'Saved, Your Data Has Been Saved',
            })
        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Update Data',
            })
        }
    })

}
function get_grdocumentlist() {
    $.ajax({
        url: '/EMCS/GetGRDocumentList',
        type: 'GET',
        data: {
            IdGr: $('#IdGr').val(),

        },

        cache: false,
        async: false,
        success: function (data, response) {
            var convert = GRDocumentConvert(data);
            convert.Id = 0;

            if (convert.length > 0) {
                $tablegrdocument.bootstrapTable('removeAll');
                $tablegrdocument.bootstrapTable('load', convert);
                //$tablegrdocument.bootstrapTable('remove', convert.Id);


                //$tableDocuments.bootstrapTable('refresh');
            } else {
                $tablegrdocument.bootstrapTable('removeAll');
            }
            convert.Id = 0;
        },

        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Get Data',
            })
        }

    })

}
function GRDocumentConvert(data) {

    var array = new Array();
    $.each(data, function (index, element) {
        var arraydata = {
            Id: element.Id,
            IdGr: element.IdGr,
            DocumentDate: moment(element.DocumentDate).format("DD MMM YYYY"),
            DocumentName: element.DocumentName,
            Filename: element.Filename
        }
        array.push(arraydata);
    })
    return array;
}
function GrDocumentDeleteById(id) {
    $.ajax({
        url: '/EMCS/GrDocumentDeleteById',
        type: 'POST',
        data: {
            id: id,
        },
        cache: false,
        async: false,
        success: function (data, response) {
            Swal.fire({
                type: 'success',
                title: 'Success',
                text: 'Success, Your Data Has Been Delete',
            })
        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Update Data',
            })
        }
    })
    $("#Id").val("0");
}

$("#documentAddButton").on('click', function () {
    $('#Id').val(0),
        $('#DocumentName').val(null),
        $('#DocumentDate').val(null)
})
$("#btnAddDocument").on("click", function () {
    var id = $("#IdGr").val();

    if ($("#IdGr").val() == null || $("#IdGr").val() == 0) {
        Swal.fire({
            type: 'error',
            title: 'Opps..',
            html: 'Good Receive Id Is Not Found,Please save as Draft before Add Document'
        });
        return false;
    }
    else {
        ajaxInsertUpdateDocument();
        get_grdocumentlist();
    }


})
var myDropzoneDocument = new Dropzone("#FormUploadDocument", { // Make the bodyFormUpload a dropzone

    url: "/EMCS/GrDocumentUpload", // Set the url

    thumbnailHeight: 100,
    thumbnailWeight: 100,
    timeout: "80000",
    method: 'POST',
    dictDefaultMessage: "<h4>Click this Section for Browse the Import File.</h4>",
    acceptedFiles: '.pdf, .jpeg, .jpg, .png',
    filesizeBase: 1024,
    autoProcessQueue: true,
    maxFiles: 1,
    maxFilesize: 100, // MB
    parallelUploads: 1,
    previewTemplate: $("#template-dropzone").html(),
    uploadMultiple: false
    //previewsContainer: "#FormUploadMaterial", // Define the container to display the previews
    //clickable: ".fileinput-button" // Define the element that should be used as click trigger to select files.
}
);

myDropzoneDocument.on("addedfile", function (file) {

    // Hookup the start button
    $("#actions .start").on("click", function () {
        myDropzone.enqueueFile(file);
    });
    $("#placeholderUpload").hide();
});

myDropzoneDocument.on("totaluploadprogress", function (progress) {

    $("#total-progress .progress-bar").css("width", progress + "%");
    $("#progressPercent").html(progress + "%");
});

myDropzoneDocument.on("sending", function (file, xhr, formData) {


    formData.append("id", $("#IdDocumentUpload").val());
    // Show the total progress bar when upload starts
    $("#total-progress").css("opacity", 1); 
    // And disable the start button
    $("#actions .delete").attr("disabled", "disabled");
    $(".start").attr("disabled", "disabled");

});

myDropzoneDocument.on("complete", function (resp) {


    if (resp.status === "success") {
        $("#actions .delete").prop("disabled", false);
        if (resp.size >= 9785 && resp.size <= 9800) {
            swal.fire("Upload Status", "Empty files will not be uploaded.", "error");
        }
        else {
            var respText = resp.xhr.response;
            var respData = JSON.parse(respText);
            console.log(respData);
            var type = respData.status ? "success" : "error";
            swal.fire("Upload Status", respData.msg, type);
            get_grdocumentlist();
        }
    }
});

myDropzoneDocument.on("queuecomplete", function (progress) {
    $("#total-progress").css("opacity", "0");
    setTimeout(function () {
        myDropzoneDocument.removeAllFiles(true);
    }, 400);
});

$("#actions .start").on("click", function () {
    myDropzoneDocument.enqueueFiles(myDropzoneDocument.getFilesWithStatus(Dropzone.QUEUED));
});

$("#actions .cancel").on("click", function () {
    myDropzoneDocument.removeAllFiles(true);
    $("#placeholderUpload").hide();
});

/*-------------------------------------------------------------------------------------*/

$("#listgetforview").on('click', function () {

    get_Grdocumentviewlist();
})
$tableDocuments1 = $('#tableGrDocuments1');
var columnDocument1 = [
    {
        field: '',
        title: 'Action',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return "-";
        },

    },
    {
        field: 'Id',
        title: 'No',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        visible: false
    },
    {
        field: 'IdGr',
        title: 'RG Bast Id',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        visible: false
    },
    {
        field: 'DocumentDate',
        title: 'Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'DocumentName',
        title: 'Document Name',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Filename',
        title: 'Attachment',
        align: 'center',
        valign: 'center',
        halign: "center",
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row) {
            //if (row.Filename !== "") {
            //    var btnDownload = "<button class='btn btn-xs btn-success download' type='button'><i class='fa fa-download'></i></button>";
            //    var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocument' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='fa fa-file-pdf-o'></i></button>";
            //    return [btnDownload, btnPreview].join(' ');
            //} else {
            return "-";
            //}
        },
        class: 'text-nowrap'
    }];
$tableDocuments1.bootstrapTable({
    columns: columnDocument1,
    cache: false,
    search: false,
    striped: false,
    clickToSelect: true,
    reorderableColumns: true,
    toolbar: '.toolbarDocument',
    toolbarAlign: 'left',
    onClickRow: selectRow,
    showColumns: true,
    showRefresh: false,
    smartDisplay: false,
    pagination: true,
    sidePagination: 'client',
    pageSize: '5',
    formatNoMatches: function () {
        return '<span class="noMatches">No data found</span>';
    },
});
function get_Grdocumentviewlist() {

    $.ajax({
        url: '/EMCS/GetGRDocumentList',
        type: 'POST',
        data: {
            IdGr: $('#IdGr').val(),

        },

        cache: false,
        async: false,
        success: function (data, response) {

            var convert = GRDocumentConvert(data);
            convert.Id = 0;
            if (convert.length > 0) {
                $tableDocuments1.bootstrapTable('removeAll');
                $tableDocuments1.bootstrapTable('load', convert);
                $tableDocuments1.bootstrapTable('remove', convert.Id);


                //$tableDocuments.bootstrapTable('refresh');
            } else {
                $tableDocuments1.bootstrapTable('removeAll');
            }
            convert.Id = 0;
        },

        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Get Data',
            })
        }

    })

}



