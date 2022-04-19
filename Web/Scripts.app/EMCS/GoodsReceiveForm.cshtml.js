var $table = $("#TblGoodReceive");
var $IdGr = $("#IdGr").val();

window.operateEvents = {
    'click .edit': function (e, value, row) {
        $(".editRecord").attr("href", `/EMCS/UpdateGrItem/?Id=${row.Id}&IdGr=${row.IdGr}`).trigger("click");
    },
    'click .upload': function (e, value, row) {
        $(".uploadRecord").attr("href", `/EMCS/UploadGrItem/${row.Id}`).trigger("click");
    },
    'click .remove': function (e, value, row) {
        Swal.fire({
            title: "Confirmation",
            text: "Are you sure want to remove this data?",
            type: "question",
            showCancelButton: true,
            cancelButtonColor: "#d33",
            confirmButtonColor: "#3085d6",
            confirmButtonText: "Yes, Remove!",
            allowEscapeKey: false,
            allowOutsideClick: false,
            showCloseButton: true
        }).then((result) => {
            if (result.value) {
                sweetAlert.close();
                return deleteThis(row.Id);
            }
            return false;
        });
    },
    'click .editDocument': function (e, value, row, index) {
        
        $('#Id').val(row.Id);
        $('#inp-doc-date').val(row.DocumentDate);
        $('#DocumentName').val(row.DocumentName);
    },
    'click .removeDocument': function (e, value, row, index) {
        
        GrDocumentDeleteById(row.Id);
        get_grdocumentlist();
    },
    'click .uploadDocument': function (e, value, row, index) {
        
        $('#IdDocumentUpload').val(row.Id);
        //$(".uploadRecord").attr('href', '/EMCS/CiplDocumentUpload/' + row.Id).trigger('click');
    }
};

window.operateEventRight = {
    'click .download': function (e, value, row) {
        location.href = "/EMCS/DownloadGrItem/" + row.Id;
    },
    'click .showDocument': function (e, value, row) {
        $(".PreviewFile").attr('href', '/EMCS/PreviewGrItem?Id=' + row.Id).trigger('click');
    },
    'click .downloaddoc': function (e, value, row) {
        
        location.href = "/EMCS/DownloadGrDocument/" + row.Id;
    },
    'click .showDocumentdoc': function (e, value, row) {
        document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/GoodsReceive/" + row.Id + '/' + row.Filename;
    }
};

var columns = [
    {
        field: "",
        title: "No",
        align: "center",
        width: "40",
        formatter: runningFormatterC
    },
    {
        field: 'operate',
        title: 'Action',
        align: 'center',
        width: '10%',
        events: operateEvents,
        formatter: function () {
            var btnEdit = "<button type='button' class='btn btn-xs edit btn-primary'><i class='fa fa-edit'></i></button>";
            var btnUpload = "<button type='button' class='btn btn-xs upload btn-info'><i class='fa fa-upload'></i></button>";
            var btnRemove = "<button type='button' class='btn btn-xs remove btn-danger'><i class='fa fa-times'></i></button>";
            var elm = ["<div>", btnEdit, btnUpload, btnRemove, "</div>"];
            return elm.join(" ");
        }
    }, {
        field: 'id',
        visible: false
    }, {
        field: 'DoNo',
        visible: true,
        title: "EDI Number",
        halign: "center"
    }, {
        field: 'DaNo',
        //title: 'DA Number',
        title: 'DO Reference',
        align: 'left',
        halign: "center",
        valign: 'center',
        sortable: true,
        class: 'text-nowrap'

    }, {
        field: 'FileName',
        title: 'Attachment',
        align: 'center',
        valign: 'center',
        halign: "center",
        sortable: true,
        events: operateEventRight,
        formatter: function (data, row) {
            if (row.FileName !== "") {
                var btnDownload = "<button class='btn btn-xs btn-success download' type='button'><i class='fa fa-download'></i></button>";
                var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocument' type='button'><i class='fa fa-file-pdf-o'></i></button>";
                return [btnDownload, btnPreview].join(' ');
            } else {
                return "-";
            }
        },
        class: 'text-nowrap'

    }];

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
        url: myApp.fullPath + "EMCS/GetItemGr",
        cache: false,
        pagination: true,
        search: true,
        queryParams: function (params) {
            params.Id = $("#IdGr").val();
            return params;
        },
        striped: true,
        sidePagination: 'server',
        clickToSelect: false,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        responseHandler: function (resp) {
            console.log(resp);
            var data = {};
            $.map(resp.data, function (value) {
                data[value.Key] = value.Value;
            });
            return data;
        },
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">No data found</span>';
        },
        columns: columns
    });
});

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

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'EMCS/RemoveGrItem',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: { Id: id },
        dataType: "json",
        success: function (d) {
            if (d.Msg !== undefined) {
                Swal.fire('Success', d.Msg, 'success');
            }

            $("[name=refresh]").trigger('click');
        },
        error: function (jqXhr) {
            Swal.fire('Error', jqXhr.status + " " + jqXhr.statusText, "error");
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

    $('#myModalPlace').on('hidden.bs.modal', function () {
        $table.bootstrapTable("refresh");
    });
});

$("#BtnDraft").on("click", function () {
    $("#Status").val("Draft");
    var picName = $("#PicName").valid();

    if (picName) {
        SubmitDataNormal();
    } else {
        scrollToError(".input-validation-error");
    }
});

$("#BtnSubmit").on("click", function (e) {
    e.preventDefault();
    $("#Status").val("Submit");
    $('#progress').show();
    var item = $("#TblGoodReceive").bootstrapTable("getData");
    var totalItem = item.length;
    var isValid = $("#FormGR").valid();
    if (isValid) {
        if (totalItem > 0) {
            Swal.fire({
                title: 'Confirmation',
                text: 'By submitting, you are responsible for the authenticity of the documents and data entered. Are you sure you want to process this document?',
                type: 'question',
                showCancelButton: true,
                cancelButtonColor: '#d33',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'Yes, submit!',
                allowEscapeKey: false,
                allowOutsideClick: false,
                showCloseButton: true
            }).then((result) => {
                if (result.value) {
                    SubmitData();
                    location.href = myApp.fullPath + "EMCS/Grlist";
                }
            });
        } else {
            Swal.fire({
                title: 'Warning',
                text: 'Please Add an item to pickup',
                type: 'warning',
                showCancelButton: true,
                allowEscapeKey: false,
                allowOutsideClick: false,
                showCloseButton: true
            });
        }
    }
});

function SubmitDataNormal() {
    $.ajax({
        url: "/emcs/creategr",
        type: "POST",
        async: false,
        data: $("#FormGR").serialize(),
        success: function (result) {
            if (result.Status === 0) {
                var stat = $("#Status").val();

                if (result.Msg !== undefined) {
                    Swal.fire({
                        title: 'Success',
                        text: result.Msg,
                        type: 'success',
                        allowEscapeKey: false,
                        allowOutsideClick: false,
                        showCloseButton: true
                    });
                }

                var data = result.result.Data;
                $IdGr = data.Id;
                $("#IdGr").val(data.Id);
                $("#GrNo").val(data.GrNo);
                $('#progress').hide();
                $("[name=refresh]").trigger('click');
                if (stat.toLowerCase() === "submit") {
                    location.href = "/emcs/grlist";
                }
            }
            else {
                if (result.Msg !== undefined) Swal.fire('Failed', result.Msg, 'error');
                $('#progress').hide();
            }
            return false;
        }
    });
}

function SubmitData() {
    $.ajax({
        url: "/emcs/creategr",
        type: "POST",
        data: $("#FormGR").serialize(),
        success: function (result) {
            if (result.Status === 0) {
                var stat = $("#Status").val();

                if (result.Msg !== undefined) {
                    if (stat.toLowerCase() === "submit") {
                        Swal.fire('Success', result.Msg, 'success');
                    }
                }
                var data = result.result.Data;
                $IdGr = data.Id;
                $("#IdGr").val(data.Id);
                $("#GrNo").val(data.GrNo);
                $('#progress').hide();
                $("[name=refresh]").trigger('click');

                if (stat.toLowerCase() === "submit") {
                    location.href = "/emcs/grlist";
                    return false;
                }
            }
            else {
                if (result.Msg !== undefined) Swal.fire('Failed', result.Msg, 'error');
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
                    if (result.Msg !== undefined) Swal.fire('Success', result.Msg, 'success');
                    $('#myModalPlace').modal('hide');
                    $('#progress').hide();
                    $("[name=refresh]").trigger('click');
                }
                else {
                    if (result.Msg !== undefined) Swal.fire('Failed', result.Msg, 'error');
                    $('#progress').hide();
                }
            }
        });
        return false;
    });
}

function ShowModal() {
    $("#myModal").modal("show");
}

function initVehicleAutocomplete() {
    var options = {
        url: myApp.fullPath + "EMCS/GetVehicle",
        listLocation: "data",
        getValue: "text",
        template: {
            type: "description",
            fields: {
                description: "text"
            }
        },
        list: {
            match: {
                enabled: true
            }
            , onChooseEvent: function () {
                console.log(this);
            }
        },
        theme: "plate-dark",
        ajaxSettings: {
            dataType: "json",
            method: "GET",
            data: {
                dataType: "json",
                term: function () {
                    return $("#data_VehicleType").val();
                }
            }
        }
    };

    $("#data_VehicleType").easyAutocomplete(options);
}

function initVehicleMerkAutocomplete() {
    var options = {
        url: myApp.fullPath + "EMCS/GetVehicleMerk",
        listLocation: "data",
        getValue: "text",
        template: {
            type: "description",
            fields: {
                description: "text"
            }
        },
        list: {
            match: {
                enabled: true
            }
            , onChooseEvent: function () {
                console.log(this);
            }
        },
        theme: "plate-dark",
        ajaxSettings: {
            dataType: "json",
            method: "GET",
            data: {
                dataType: "json",
                term: function () {
                    return $("#data_VehicleMerk").val();
                }
            }
        }
    };

    $("#data_VehicleMerk").easyAutocomplete(options);
}

$(document).ready(function () {
    //$(".EdoNo").select2();

    $("#btnAddItem").on("click", function () {
        var id = $("#IdGr").val();
        if (id !== "0") {
            $("#linkAddItem").attr("href", "/EMCS/CreateGrItem/?IdGr=" + id).trigger("click");
        } else {
            var picName = $("#PicName").valid();
            if (picName) {
                SubmitDataNormal();
            } else {
                scrollToError(".input-validation-error");
            }
        }
    });

    $(".date").datepicker({
        autoclose: true,
        format: "dd M yyyy"
    });

    $("#PickupPoint").select2({
        placeholder: 'Search for Pickup Point',
        //maximumInputLength: 20,
        //minimumInputLength: 3,
        tags: false, //prevent free text entry
        width: "100%",
        ajax: {
            url: "/EMCS/GetCiplAreaAvailable",
            type: "GET",
            processResults: function (data) {
                // Transforms the top-level key of the response object from 'items' to 'results'
                var options = [];
                $.map(data.data, function (obj) {
                    var item = {};
                    item.id = obj.BAreaCode;
                    item.text = obj.BAreaCode + " - " + obj.BAreaName;
                    item.BAreaCode = obj.BAreaCode;
                    item.BAreaName = obj.BAreaName;
                    options.push(item);
                });
                return {
                    results: options
                };
            }
        }
    });

    $("#PickupPic").select2({
        placeholder: 'Search for Pickup Pic',
        tags: false, //prevent free text entry
        width: "100%",
        ajax: {
            url: "/EMCS/GetCiplPicAvailable",
            type: "GET",
            data: function (params) {
                var area = $("#PickupPoint").val() ? $("#PickupPoint").val() : "";
                var query = {
                    area: area,
                    search: params.term
                };
                return query;
            },
            processResults: function (data) {
                // Transforms the top-level key of the response object from 'items' to 'results'
                var options = [];
                $.map(data.data, function (obj) {
                    var item = {};
                    item.id = obj.PickUpPic;
                    item.text = obj.PickUpPic + " - " + obj.EmployeeName;
                    item.PickUpArea = obj.PickUpArea;
                    item.BAreaName = obj.BAreaName;
                    options.push(item);
                });
                return {
                    results: options
                };
            }
        }
    });

    $("#Vendor").select2({
        placeholder: 'Search for Vendor',
        maximumInputLength: 20,
        tags: false, //prevent free text entry
        width: "100%",
        ajax: {
            url: "/EMCS/GetVendorList",
            type: "GET",
            data: function (params) {
                return params;
            },
            processResults: function (data) {
                // Transforms the top-level key of the response object from 'items' to 'results'
                var options = [];
                $.map(data.data, function (obj) {
                    var item = {};
                    item.id = obj.Code;
                    item.text = obj.Code + " - " + obj.Name;
                    item.City = obj.City;
                    item.Address = obj.Address;
                    item.Telephone = obj.Telephone;
                    options.push(item);
                });
                return {
                    results: options
                };
            }
        }
    });

    $('#Vendor').on('select2:select', function (e) {
        var data = e.params.data;
        if (data.selected) {
            $("#VendorAddress").val(data.Address + " - " + data.City);
        }
    });

    $("form").removeData("validator");
    $("form").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse("form");
    initVehicleAutocomplete();
    initVehicleMerkAutocomplete();
});

var myDropzoneDocument = new Dropzone("#FormUploadDocumentContainer", { // Make the bodyFormUpload a dropzone
    
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