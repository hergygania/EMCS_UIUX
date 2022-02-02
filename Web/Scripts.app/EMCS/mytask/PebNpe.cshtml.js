$(function () {

    $(".date").on("change", function () {
        console.log(this);
        var id = $(this).attr("id");
        $("#" + id).valid();
    })
    $('.DownloadDocument, .preview-document, #DocumentShow').hide();
    dropzone();

    getPasswordPabeanOffice();
    getLastestCurrency();
    getLastestKurs();

    $('#trCompleteDoc, #trDraftPeb').hide();


    if ($("input[name='flagCompleteDoc']:checked").val() === 'true') {
        $('#trCompleteDoc').show();
    }
    if ($("input[name='flagDraftPeb']:checked").val() === 'true') {
        $('#trDraftPeb').show();
    }

    var PasswordPabean = $("#GetPasswordPabean").val();
    $('#NpePass').val(PasswordPabean);
    var consExpType = new Option(PasswordPabean, PasswordPabean, true, true);
    $('#NpePass').append(consExpType).trigger("change");

    var Valuta = $("#GetNpeValuta").val();
    var consExpType = new Option(Valuta, Valuta, true, true);
    $('#NpeValuta').append(consExpType).trigger("change");

    var ajudate = $('#AjuDate').val();
    if (ajudate) {
        $('#AjuDate').val(moment(ajudate).format("DD MMM YYYY"));
    } else {
        $('#AjuDate').val(null);
    }

    var npedate = $('#NpeDate').val();
    if (npedate) {
        $('#NpeDate').val(moment(npedate).format("DD MMM YYYY"));
    } else {
        $('#NpeDate').val(null);
    }

    GetDocumentPebNpe();

})

$("input[name='flagCompleteDoc'], input[name='flagDraftPeb']").change(function () {
    $('#DocumentShow').removeAttr('src').hide();
});

function getPasswordPabeanOffice() {
    $('#NpePass').select2({
        placeholder: 'Please Select Password Pabean Office',
        ajax: {
            url: "/emcs/KppbcList",
            dataType: 'json',
            async: false,
            data: function (params) {
                var query = {
                    SearchName: params.term
                }
                return query;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data.data, function (item) {
                        console.log(item);
                        return {
                            text: item.Code + ' - ' + item.Name,
                            id: item.Code + ' - ' + item.Name,
                            desc: item.Address
                        }
                    })
                }
            }
        }
    }).on('select2:select', function (event) {
        $('#NpeDescPass').val(event.params.data.desc).text(event.params.data.desc);
    })
}

function Select2Parameter(data) {
    var array = new Array();
    $.each(data, function (index, element) {
        array.push({ 'id': element.Name, 'text': element.Name, 'desc': element.Description });
    });
    return array;
}

function getLastestCurrency() {
    $.ajax({
        url: "/emcs/GetLastestCurrency",
        success: function (data) {
            console.log(data);
            var array = new Array();
            $.each(data.data, function (index, element) {
                array.push({ 'id': element.Id, 'text': element.Text });
            });

            $("#NpeValuta").select2({
                data: array,
                width: '100%',
                placeholder: 'Please Select Export Type',
            })
        }
    })
}

function getLastestKurs() {
    $.ajax({
        url: "/emcs/GetLastestKurs",
        success: function (data) {
            $('#NpeRate').val(formatCurrency(data, ".", ",", 2)).prop('disabled', true);
        }
    })
}

$("#FormUploadNpePeb").validate({
    onkeyup: false,
    errorClass: "error",
    ignore: ':hidden:not(.do-not-ignore)',

    //put error message behind each form element
    errorPlacement: function (error, element) {
        if (element.hasClass("select2") && element.hasClass("input-validation-error")) {
            element.next("span").addClass("input-validation-error");
        } else if (element.hasClass("select2-hidden-accessible")) {
            error.insertAfter(element.parent('span.select2'));
        }
    },
    errorElement: 'span',
    onError: function () {
        $('.input-group.error').find('.help-block.form-error').each(function () {
            $(this).closest('.form-group').addClass('error-class').append($(this));
        });
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass(errorClass); //.removeClass(errorClass);
        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
    },
    //When removing make the same adjustments as when adding
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass(errorClass); //.addClass(validClass);
        $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
        $(element).next("span").removeClass("input-validation-error");
    },
});

$('.preview-document').on('click', function () {
    $('#DocumentShow').attr("src", "/Upload/EMCS/NPEPEB/" + $(this).val()).show();
})

$('.flagCompleteDoc').on('click', function () {
    $('#trCompleteDoc').hide();
    var value = $(this).val();
    if (value === 'true') {
        $('#url-document-COMPLETEDOCUMENT').addClass('do-not-ignore');
        $('#trCompleteDoc').show();
    } else if (value === 'false') {
        $('#trCompleteDoc').hide();
        $('#url-document-COMPLETEDOCUMENT').removeClass('do-not-ignore');
    }
})
$('.flagDraftPeb').on('click', function () {
    $('#trDraftPeb').hide();
    var value = $(this).val();
    if (value === 'true') {
        $('#url-document-DRAFTPEB').addClass('do-not-ignore');
        $('#trDraftPeb').show();
    } else if (value === 'false') {
        $('#trDraftPeb').hide();
        $('#url-document-DRAFTPEB').removeClass('do-not-ignore');
    }
})

function GetDocumentPebNpe() {
    $.ajax({
        url: '/EMCS/GetDocumentPebNpe',
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
    id.attr("href", "/Upload/EMCS/NPEPEB/" + filename);
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
            url: '/emcs/UploadDocumentNpePeb',
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
                    formData.append("AjuNumber", $('#AjuNumber').val());
                    formData.append("typeDoc", Name);
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
                    GetDocumentPebNpe();
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