$(function () {

    $('.DownloadDocument, .preview-document, #DocumentShow').hide();
    dropzone();

    $(".date").on("change", function () {
        console.log(this);
        var id = $(this).attr("id");
        $("#" + id).valid();
    })

    

    GetDocumentBlAwb();

})

$('.preview-document').on('click', function () {
    $('#DocumentShow').attr("src", "/Upload/EMCS/BLAWB/" + $(this).val()).show();
})

function GetDocumentBlAwb() {
    $.ajax({
        url: '/EMCS/GetDocumentBlAwb',
        type: 'POST',
        data: {
            IdRequest: $('#CargoID').val(),
        },
        cache: false,
        async: false,
        success: function (data, response) {
            $.each(data, function (index, element) {
                console.log(element.Tag);
                var id_doc = $('#download-document-' + element.Tag).show();
                var url_doc = $('#url-document-' + element.Tag).removeClass('do-not-ignore');
                var label_doc = $('#label-document-' + element.Tag).removeClass('error');
                var preview_doc = $('#preview-document-' + element.Tag).show();
                download(element.FileName, id_doc, url_doc, preview_doc);
            });
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
function download(filename, id, url, preview) {
    id.attr("href", "/Upload/EMCS/BLAWB/" + filename);
    url.val(filename);
    preview.val(filename);
}
function dropzone() {
    Dropzone.autoDiscover = false;
    var previewNode = document.querySelector(".template");
    previewNode.id = "";
    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    $('.dz-clickable').each(function () {
        var options = $(this).attr('id').split('-');
        var Name = options[1];
        $(this).dropzone({
            url: '/emcs/UploadDocumentBlAwb',
            //paramName: Name,
            uploadMultiple: false,
            parallelUploads: 1,
            clickable: true,
            maxFiles: 1,
            acceptedFiles: ".jpeg,.jpg,.pdf",
            previewTemplate: previewTemplate,
            init: function () {
                this.on('sending', function (file, xhr, formData) {
                    formData.append("IdCargo", $('#CargoID').val());
                    formData.append("BlAwbNo", $('#Number').val());
                    formData.append("TypeDoc", Name);
                });
                this.on("addedfile", function (file) {
                    if (this.files.length > 1) {
                        this.files = this.files.slice(1, 2);
                    }
                });
            },

            success: function (file, response) {
                $('.dz-image-preview').remove();
                if (file.status === "success") {
                    Swal.fire(
                        'Processing Document!',
                        'Document Uploaded Successfully!',
                        'success'
                    );
                    GetDocumentBlAwb();
                } else {
                    Swal.fire(
                        'Processing Document!',
                        'Error Upload!',
                        'error'
                    )
                }
            },
            complete: function (file, response) {
                console.log(file, response);
                //location.reload();
            },
            // Rest of the configuration equal to all dropzones
        });

    });
}
