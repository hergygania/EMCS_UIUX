var $table = $("#TblGoodReceive");
var $IdGr = $("#IdGr").val();
var $tablearmadaForRFC = $('#tablearmadaForRFC');
var ciplitemids = [];
var ArmadaList = [];
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
        $('#IdGrDocument').val(row.Id);
        $('#inp-doc-date').val(row.DocumentDate);
        $('#DocumentName').val(row.DocumentName);
    },
    'click .removeDocument': function (e, value, row, index) {
        GrDocumentDeleteById(row.Id);
        get_grdocumentlist();
    },
    'click .uploadDocument': function (e, value, row, index) {
        $('#IdDocumentUpload').val(row.Id);
    },
    'click .checkitem': function (e, value, row, index) {

        newciplitem = [];
        IsFound = false;
        if (ciplitemids.length > 0) {
            for (var i = 0; i < ciplitemids.length; i++) {
                if (ciplitemids[i] == row.Id) {
                    IsFound = true;
                }
                else {
                    newciplitem.push(ciplitemids[i]);
                }
            }
        }
        else {
            newciplitem.push(row.Id);
        }
        if (IsFound == false && ciplitemids.length > 0) {
            newciplitem.push(row.Id);
        }

        ciplitemids = newciplitem;
    },
    'click .removeItem': function (e, value, row, index) {
        console.log(row);

        $.ajax({
            url: '/EMCS/DeleteItem?id=' + row.IdShippingFleet + '&idCiplItem=' + row.Id,
            type: 'POST',
            success: function (data, response) {
                Swal.fire({
                    type: 'success',
                    title: 'Success',
                    text: 'Success, Your Data Has Been Delete',
                })
                ciplitemids = [];
                ShippingFleetItemTable();
            },
            error: function (e) {
                Swal.fire({
                    type: 'error',
                    title: 'Oops...',
                    text: 'Something went wrong! Fail Update Data',
                })
            }
        })

    },
    'click .viewarmada': function (e, value, row, index) {
        $('#saveitem').hide();
        ViewItemTable(row.Id);
    },
    'click .editarmada': function (e, value, row) {

        editArmada(row);
    },
    'click .removearmada': function (e, value, row) {

        deletearmada(row.Id, row.IdGr);
    },
    'click .uploadarmadadocument': function (e, value, row, index) {

        $('#IdDocumentUpload').val(row.Id);
    },
    'click .uploadarmadadocumentForRFC': function (e, value, row, index) {

        $('#IdDocumentUpload').val(row.Id);
    }

};

window.operateEventRight = {
    'click .downloaddoc': function (e, value, row) {
        location.href = "/EMCS/DownloadGrDocument/" + row.Id;
    },
    'click .showDocumentdoc': function (e, value, row) {
        document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/GoodsReceive/" + row.Id + '/' + row.Filename;
    },
    'click .downloadarmadadoc': function (e, value, row) {

        location.href = "/EMCS/DownloadArmadaDocument/" + row.Id;
    },
    'click .showDocumentarmadadoc': function (e, value, row) {

        $.ajax({
            url: '/EMCS/GetListArmada?IdGr=0&Id=' + row.Id + '',
            success: function (data) {
                document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/GoodsReceive/" + data[0].FileName;

            }
        })
    },
    'click .downloadarmadadocForRFC': function (e, value, row) {

        location.href = "/EMCS/DownloadArmadaDocumentForRFC?FileName=" + row.FileName;
    },
    'click .showDocumentarmadadocForRFC': function (e, value, row) {
        $.ajax({
            url: '/EMCS/GetRFCitemDataById?Id=' + row.Id + '',
            success: function (data) {
                document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/GoodsReceive/" + data.FileName;

            }
        })
    }
};

var columns = [
    //{
    //    field: "",
    //    title: "No",
    //    align: "center",
    //    width: "40",
    //    formatter: runningFormatterC
    //},
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

                sAlert('Success', "Data Updated SuccessFully", 'success')
                //setTimeout($("[name=refresh]").trigger('click'), 3000);
                /*$("[name=refresh]").trigger('click')*/


            }
            $("[name=refresh]").trigger('click');
        },
        error: function (jqXhr) {
            Swal.fire('Error', jqXhr.status + " " + jqXhr.statusText, "error");
        }
    });

};
function deletepopup(d) {

    swal.fire({
        title: 'Success',
        text: 'Data Updated SuccessFully',
        type: 'success'
    }).then((result) => {
        $("[name=refresh]").trigger('click');
    })


}

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

$('#btnApproveSave').on('click', function () {
    var idgr = $('#IdGr').val();

    $.ajax({
        url: "/EMCS/GetCheckarmadaDetail?Id=" + idgr,
        success: function (data) {

            var FileName = true;
            for (var i = 0; i < data.FileName.length; i++) {

                if (data.FileName[i] == null) {
                    Swal.fire({
                        title: '',
                        text: 'Please Upload DA document in all armada',
                        type: 'warning'
                    })
                    FileName = false;
                    break;
                }

            }

            if (FileName == true) {
                Swal.fire({
                    title: 'Approve Confirmation',
                    text: 'By approving this document, you are responsible for the authenticity of the documents and data entered. Are you sure you want to process this document?',
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
                            var Notes = result.value;
                            var Status = "Approve";
                            var Id = $('#IdGr').val();
                            var data = { Notes: Notes, Status: Status, Id: Id };
                            SaveChangeHistoryAndApproveRG(data);

                        });

                    }
                    return false;
                });
            }
        }


    })
})

function SaveChangeHistoryAndApproveRG(data) {

    var modelObj = {
        FormType: "GoodsReceive",
        FormId: $('#IdGr').val(),
        Reason: data.Notes,
        Status: status
    }

    var item = {
        Id: $('#IdGr').val(),
        GrNo: $('#GrNo').val(),
        Vendor: $('#Vendor').val(),
        VehicleType: $('#VehicleType').val(),
        VehicleMerk: $('#VehicleBrand').val(),
        PickupPoint: $('#PickupPoint').val(),
        PickupPic: $('#PickupPic').val(),
        Notes: $('#Note').val(),
    }
    $.ajax({
        url: '/EMCS/SaveHistoryAndApproveForGR',
        type: 'POST',
        data: {
            form: modelObj,
            item: item,
        },
        cache: false,
        async: true,
        success: function (data, response) {

            Swal.fire({
                title: 'Approve!',
                text: 'Data Confirmed Successfully',
                type: 'success'
            }).then((result) => {
                window.location.href = "/EMCS/Mytask";
            });
        },
        error: function (e) {
            return false;
        }
    });


}

function SaveArmadaByApprover() {
    var dono = $('#DoNo').val();
    if (dono == '0' && dono == null) {
        Swal.fire({
            title: 'Warning',
            text: 'Please Select Edo No Of Armada',
            type: 'warning',
            showCancelButton: true,
            allowEscapeKey: false,
            allowOutsideClick: false,
            showCloseButton: true
        });
    }
    else if ($('#PicName').val() == '' || $('#PhoneNumber').val() == undefined || $('#KtpNumber').val() == '' || $('#SimNumber').val() == '' || $('#SimExpiryDate').val() == '' || $('#StnkNumber').val() == '' ||
        $('#KirNumber').val() == '' || $('#KirExpire').val() == '' || $('#NopolNumber').val() == '' || $('#EstimationTimePickup').val() == '') {
        Swal.fire({
            title: 'Warning',
            text: 'Please Fill All Details OF Armada',
            type: 'warning',
            showCancelButton: true,
            allowEscapeKey: false,
            allowOutsideClick: false,
            showCloseButton: true
        });
    }
    else {
        text = [];
        if (dono != undefined && dono != null && dono != 0) {
            for (var i = 0; i < dono.length; i++) {
                var edono = dono[i];
                text.push($('#DoNo option[value="' + edono + '"]').text())

            }
            text = text.join(',');
        }
        else {
            text = $('#DoNoText').val();
        }
        var objModel = {};
        objModel.Id = $('#IdArmada').val();
        objModel.IdGr = $('#IdGr').val(),
            objModel.IdCipl = parseInt(0),
            objModel.DoNo = text,
            objModel.PicName = $('#PicName').val(),
            objModel.PhoneNumber = $('#PhoneNumber').val(),
            objModel.KtpNumber = $('#KtpNumber').val(),
            objModel.SimNumber = $('#SimNumber').val(),
            objModel.SimExpiryDate = $('#SimExpiryDate').val(),
            objModel.StnkNumber = $('#StnkNumber').val(),
            objModel.KirNumber = $('#KirNumber').val(),
            objModel.KirExpire = $('#KirExpire').val(),
            objModel.NopolNumber = $('#NopolNumber').val(),
            objModel.EstimationTimePickup = $('#EstimationTimePickup').val(),
            objModel.Apar = $('#Apar').val(),
            objModel.Apd = $('#Apd').val(),
            objModel.DaNo = $('#DaNo').val(),
            objModel.Bast = $('#Bast').val();
        objModel.FileName = null;

        $.ajax({
            url: "/EMCS/SaveHistory",
            type: 'Post',
            async: false,
            data: objModel,
            success: function (data) {
                popupsave == false;
                Swal.fire("Success", "Data Saved SuccessFully", "success");
                $('#IdArmada').val(null);
                $('#DoNoText').val(null);
                $('#IdSaveChange').val(null);
                text = [];
                remove();
                tablearmada();
            },
        });
    }
}


$("#BtnDraft").on("click", function () {

    var display = 0;
    var resultmsg = 0;
    $("#Status").val("Draft");
    var PickupPoint = $("#PickupPoint").val();
    var PickupPic = $("#PickupPic").val();
    if (PickupPoint != null && PickupPic != null) {

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
    var FileName = "";
    var showpopup = true;
    var IdGr = $('#IdGr').val();
    $.ajax({
        url: "/EMCS/GetCheckarmadaDetail?Id=" + $('#IdGr').val(),
        success: function (data) {

            if (data.itemlistingrcount != 0) {
                var FileName = true;
                for (var i = 0; i < data.FileName.length; i++) {
                    if (data.FileName[i] == null) {
                        Swal.fire({
                            title: '',
                            text: 'Please Upload DA document in all armada',
                            type: 'warning'
                        })
                        FileName = false;
                        break;
                    }
                }
                if (FileName == true) {
                    if (isValid) {
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
                                Swal.fire('Success', 'Data Added Successfully!', 'success').then((result) => {
                                    if (result.value) {
                                        SubmitData(IdGr);
                                        location.href = myApp.fullPath + "EMCS/Grlist";
                                    }
                                })
                            }
                        });
                    }
                }
            }
            else {
                swal.fire({
                    text: 'Please Add At least One Armada',
                    title: '',
                    type: 'warning'
                })
            }

        }
    })


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
                if ($("#BtnDraft").val() != "0") {
                    if (result.Msg !== undefined) {
                        Swal.fire({
                            title: 'Success',
                            text: result.Msg,
                            type: 'success',
                            allowEscapeKey: false,
                            allowOutsideClick: false,
                            showCloseButton: true
                        });
                        $('#BtnDraft').val("1");
                    }

                }

                var data = result.result.Data;
                $IdGr = data.Id;
                $("#Id").val(data.Id);
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

function SubmitData(Id) {

    var form = $("#FormGR").serialize();
    $.extend(form, { 'Data.Id': Id });
    $.ajax({
        url: "/emcs/creategr",
        type: "POST",
        data: form,
        success: function (result) {

            if (result.Status === 0) {
                var stat = $("#Status").val();

                var data = result.result.Data;
                $IdGr = data.Id;
                $("#IdGr").val(data.Id);
                $("#GrNo").val(data.GrNo);
                $('#progress').hide();
                $("[name=refresh]").trigger('click');

                if (stat.toLowerCase() === "submit") {
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

    $("#btnAddItem").on("click", function () {

        var id = $("#IdGr").val();
        if (id !== "0") {
            $("#linkAddItem").attr("href", "/EMCS/CreateGrItem/?IdGr=" + id).trigger("click");
        } else {
            var picName = $("#PicName").valid();
            if (picName) {
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

                var reduced = options.reduce(function (filtered, option) {
                    var comp = $("#PickupPoint").data("select2").dropdown.$search;
                    if (comp != undefined) {
                        var term = $("#PickupPoint").data("select2").dropdown.$search.val();
                        if (option.text.toLowerCase().includes(term)) {
                            var newVal = { id: option.id, text: option.text, BAreaCode: option.BAreaCode, BAreaName: option.BAreaName }
                            filtered.push(newVal);
                        }
                        return filtered;
                    } else {
                        return reduced = [];
                    }
                }, []);

                if (reduced.length > 0) {
                    return {
                        results: reduced
                    };
                } else {
                    return {
                        results: options
                    };
                }
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

                var reduced = options.reduce(function (filtered, option) {
                    var comp = $("#PickupPic").data("select2").dropdown.$search;
                    if (comp != undefined) {
                        var term = $("#PickupPic").data("select2").dropdown.$search.val();
                        if (option.text.toLowerCase().includes(term)) {
                            var newVal = { id: option.id, text: option.text, PickUpArea: option.PickUpArea, BAreaName: option.BAreaName }
                            filtered.push(newVal);
                        }
                        return filtered;
                    } else {
                        return reduced = [];
                    }
                }, []);

                if (reduced.length > 0) {
                    return {
                        results: reduced
                    };
                } else {
                    return {
                        results: options
                    };
                }
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
                var query = {
                    searchName: params.term
                };
                return query;
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
    $tablearmada = $('#tablearmada');

});

function ViewItemTable(Id) {

    if (Id != 0) {
        $('#Method').val('View');
        var ciplids = [];
        $.ajax({
            url: '/EMCS/GetListArmada?IdGr=0&Id=' + Id + '',
            success: function (getdata) {
                if (getdata.length != 0 && getdata.length != null && getdata.length != undefined) {
                    $.ajax({
                        url: '/EMCS/GetCiplIdFromDoNo?DoNo=' + getdata[0].DoNo,
                        success: function (data1) {

                            for (var i = 0; i < data1.length; i++) {
                                ciplids.push(data1[i].Id);
                            }
                            $('#IdArmada').val(getdata[0].Id);
                            $('#IdCipl').val(ciplids.join(','));
                            text = [];
                            getid = [];
                            $tablearmadaitemview = $('#tablearmadaitem');

                            var columnarmadaitemview = [
                                [


                                    {
                                        field: "State",
                                        title: "Select Item",
                                        rowspan: 2,
                                        events: operateEvents,
                                        formatter: function (data, row, index) {

                                            if ($('#Method').val() == "Edit") {

                                                if (row.IdShippingFleet > 0) {

                                                    return "<button type='button' class='btn btn-xs btn-danger removeItem' value='" + row.Id + "' title='Delete'><i class='fa fa-trash-o'></i></button>";
                                                }
                                                else {
                                                    return "<input type='checkbox' value='" + row.Id + "' class='checkitem' >";
                                                }
                                            }
                                            if ($('#Method').val() == "View") {
                                                $tablearmadaitemview.bootstrapTable('hideColumn', 'State')
                                            }

                                        }
                                    },
                                    {
                                        field: "CiplNo",
                                        title: "No.",
                                        align: "center",
                                        halign: "center",
                                        rowspan: 2,
                                        sortable: true,
                                        formatter: runningFormatterNoPaging
                                    },
                                    {
                                        field: "Name",
                                        title: "Name",
                                        rowspan: 2,
                                        align: 'center',
                                        sortable: true
                                    }, {
                                        field: "Quantity",
                                        title: "Quantity",
                                        rowspan: 2,
                                        align: 'center',
                                        sortable: true
                                    }, {
                                        field: "UnitUom",
                                        title: "UOM",
                                        rowspan: 2,
                                        align: 'center',
                                        sortable: true,
                                        formatter: function (value, row, index) {
                                            return row.UnitUom;
                                        }
                                    }, {
                                        field: "PartNumber",
                                        title: "Part Number",
                                        rowspan: 2,
                                        align: 'center',
                                        sortable: true
                                    },
                                    {
                                        field: "Sn",
                                        title: "Sn",
                                        rowspan: 2,
                                        align: 'center',
                                        sortable: true
                                    },
                                    {
                                        field: "JCode",
                                        title: "J-Code",
                                        rowspan: 2,
                                        align: 'center',
                                        sortable: true
                                    },
                                    {
                                        field: "Ccr",
                                        title: "CCR",
                                        rowspan: 2,
                                        align: 'center',
                                        sortable: true
                                    },
                                    {
                                        field: "CaseNumber",
                                        title: "Case Number",
                                        rowspan: 2,
                                        align: 'center',
                                        sortable: true
                                    },
                                    {
                                        field: "ASNNumber",
                                        title: "ASN Number",
                                        rowspan: 2,
                                        align: 'center',
                                        sortable: true
                                    },
                                    {
                                        field: "Category",
                                        title: "Type",
                                        rowspan: 2,
                                        align: 'center',
                                        sortable: true,
                                    }, {
                                        field: "UnitPrice",
                                        title: "Unit Price",
                                        rowspan: 2,
                                        align: 'center',
                                        sortable: true,
                                        formatter: function (value, row, index) {
                                            return row.UnitPrice;
                                        }
                                    }, {
                                        field: "ExtendedValue",
                                        title: "Extended Value",
                                        rowspan: 2,
                                        align: 'center',
                                        sortable: true,
                                        filterControl: true,
                                        formatter: function (value, row, index) {
                                            return row.ExtendedValue;
                                        }
                                    }, {
                                        field: "dimension",
                                        title: "Dimension (In CM)",
                                        colspan: 3,
                                        align: 'center',
                                        sortable: true,
                                        filterControl: true
                                    }, {
                                        field: "Volume",
                                        title: "Volume",
                                        colspan: 1,
                                        align: 'center',
                                        sortable: true,
                                        filterControl: true
                                    }, {
                                        field: "NetWeight",
                                        title: "Net Weight",
                                        colspan: 1,
                                        align: 'right',
                                        sortable: true,
                                        filterControl: true
                                    }, {
                                        field: "GrossWeight",
                                        title: "Gross Weight",
                                        colspan: 1,
                                        align: 'right',
                                        sortable: true,
                                        filterControl: true
                                    }],
                                [{
                                    field: "Length",
                                    title: "Length",
                                    sortable: true,
                                    align: 'center',
                                    filterControl: true,
                                    formatter: function (value, row, index) {
                                        return row.Length;
                                    }
                                }, {
                                    field: "Width",
                                    title: "Width",
                                    sortable: true,
                                    align: 'center',
                                    filterControl: true,
                                    formatter: function (value, row, index) {
                                        return row.Width;
                                    }
                                }, {
                                    field: "Height",
                                    title: "Height",
                                    sortable: true,
                                    align: 'center',
                                    filterControl: true,
                                    formatter: function (value, row, index) {
                                        return row.Height;
                                    }
                                }, {
                                    field: "m3",
                                    title: "(m3)",
                                    sortable: true,
                                    align: 'center',
                                    filterControl: true,
                                    formatter: function (value, row, index) {
                                        return row.Volume;
                                    }
                                }, {
                                    field: "NetWeight",
                                    title: "in KGa",
                                    sortable: true,
                                    align: 'right',
                                    filterControl: true,
                                    formatter: function (value, row, index) {
                                        return row.NetWeight;
                                    }
                                }, {
                                    field: "GrossWeight",
                                    title: "in KGa",
                                    sortable: true,
                                    align: 'right',
                                    filterControl: true,
                                    formatter: function (value, row, index) {
                                        return row.GrossWeight;
                                    }
                                }
                                ]
                            ];
                            if ($('#Method').val() == "View") {
                                $tablearmadaitemview.bootstrapTable('hideColumn', 'State')
                            }

                            $tablearmadaitemview.bootstrapTable({
                                url: "/EMCS/CiplItemAvailableInArmada",
                                columns: columnarmadaitemview,
                                pagination: false,
                                //pagesize: '10',
                                cache: false,
                                search: false,
                                striped: false,
                                clickToSelect: true,
                                reorderableColumns: true,
                                toolbar: '.toolbararmadaitem',
                                toolbarAlign: 'left',
                                queryParams: function (params) {

                                    return {
                                        Method: $('#Method').val(),
                                        IdCipl: $('#IdCipl').val(),
                                        IdGr: getdata[0].IdGr,
                                        IdShippingFleet: $('#IdArmada').val(),
                                    };
                                },
                                onClickRow: selectRow,
                                showColumns: false,
                                showRefresh: false,
                                smartDisplay: false,
                                formatNoMatches: function () {
                                    return '<span class="noMatches">Not Data Found</span>';
                                },
                                success: function (data) {

                                }
                            });
                            $($tablearmadaitemview).bootstrapTable('refresh');
                            $('#myModalItem').modal('show')
                        }
                    })

                }


            }
        })
    }
}

function tablearmada() {
    $tablearmada = $('#tablearmada');
    var columnArmada = [

        {
            field: '',
            title: 'Action',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            events: operateEvents,
            formatter: function () {
                var btnEdit = "<button type='button' class='btn btn-xs editarmada btn-primary'><i class='fa fa-edit'></i></button>";
                var btnRemove = "<button type='button' class='btn btn-xs removearmada btn-danger'><i class='fa fa-times'></i></button>";
                // var btnView = "<button type='button' class='btn viewarmada btn-xs btn-default'><i class='fa fa-search'></i></button>";
                var btnUpload = "<button type='button' class='btn btn-xs uploadarmadadocument btn-info' data-toggle='modal' data-target='#myModalUploadPlaceArmada' ><i class='fa fa-upload'></i></button>";
                var elm = ["<div>", btnEdit, btnRemove, btnUpload, "</div>"];
                return elm.join(" ");
            }
        },
        {
            field: 'DoNo',
            title: 'Edi No.',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
        },
        {
            field: 'PicName',
            title: 'PIC Name',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
        },
        {
            field: 'PhoneNumber',
            title: 'Contact',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
        },
        {
            field: 'SimExpiryDate',
            title: 'License Expiry Date#',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            formatter: function (data, row, index) {
                return moment(data).format("DD MMM YYYY");
            }
        },
        {
            field: 'StnkNumber',
            title: 'No STNK',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
        },
        //{
        //    field: 'Bast',
        //    title: 'Bast',
        //    halign: 'center',
        //    align: 'center',
        //    class: 'text-nowrap',
        //    sortable: true,
        //},
        {
            field: 'KirNumber',
            title: 'KIR Number',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
        },
        {
            field: 'KirExpire',
            title: 'KIR Expiry Date',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            formatter: function (data, row, index) {
                return moment(data).format("DD MMM YYYY");
            }
        },
        {
            field: 'Bast',
            title: 'Bast',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
        },
        {
            field: 'NopolNumber',
            title: 'Police Plate#',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
        },
        {
            field: 'SimNumber',
            title: 'SIM',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            //formatter: function (data, row, index) {
            //    return moment(data).format("DD MMM YYYY");
            //}
        },
        {
            field: 'KtpNumber',
            title: 'KTP',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
        },
        {
            field: 'Apar',
            title: 'APAR',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            formatter: function (data) {
                if (data == false) {
                    return "No";
                }
                else {
                    return "Yes";
                }
            }

        },
        {
            field: 'EstimationTimePickup',
            title: 'ATP',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            formatter: function (data, row, index) {
                return moment(data).format("DD MMM YYYY");
            }
        },
        {
            field: 'Apd',
            title: 'APD',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            formatter: function (data, row, index) {
                if (data == false) {
                    return "No";
                }
                else {
                    return "Yes";
                }
            }
        },
        {
            field: 'DaNo',
            title: 'DO Reference',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
        },
        //{
        //    field: '',
        //    title: 'AAA',
        //    halign: 'center',
        //    align: 'center',
        //    class: 'text-nowrap',
        //    sortable: true,
        //},
        //{
        //    field: '',
        //    title: 'Attachment',
        //    align: 'center',
        //    valign: 'center',
        //    halign: "center",
        //    class: 'text-nowrap',
        //    sortable: true,
        //},
        {
            field: 'FileName',
            title: 'Attachment',
            align: 'center',
            valign: 'center',
            halign: "center",
            sortable: true,
            events: operateEventRight,
            formatter: function (data, row) {
                if (row.FileName !== "" && row.FileName !== null) {
                    var btnDownload = "<button class='btn btn-xs btn-success downloadarmadadoc' type='button'><i class='fa fa-download'></i></button>";
                    var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocumentarmadadoc' data-toggle='modal' data-target='#myModalUploadPreview' type='button'><i class='fa fa-file-pdf-o'></i></button>";
                    return [btnDownload, btnPreview].join(' ');
                } else {
                    return "-";
                }
            },
            class: 'text-nowrap'

        }];

    $tablearmada.bootstrapTable({
        url: "/EMCS/GetListArmada",
        columns: columnArmada,
        pagination: true,
        pagesize: '10',
        cache: false,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbarAlign: 'left',
        queryParams: function (params) {
            return {
                IdGr: $('#IdGr').val(),
                Id: 0,
            };
        },
        onClickRow: selectRow,
        showColumns: false,
        showRefresh: false,
        smartDisplay: false,
        formatNoMatches: function () {
            return '<span class="noMatches">Not Data Found</span>';
        },
        success: function (data) {

        }
    });
    $($tablearmada).bootstrapTable('refresh')

}


var myDropzoneDocument1 = new Dropzone("#FormUploadDocumentArmadaContainer", { // Make the bodyFormUpload a dropzone

    url: "/EMCS/GrDocumentUploadArmada", // Set the url

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
    previewTemplate: $("#template-dropzone1").html(),
    uploadMultiple: false
    //previewsContainer: "#FormUploadMaterial", // Define the container to display the previews
    //clickable: ".fileinput-button" // Define the element that should be used as click trigger to select files.
}
);

myDropzoneDocument1.on("addedfile", function (file) {

    // Hookup the start button
    $("#actions .start").on("click", function () {
        myDropzone.enqueueFile(file);
    });
    $("#placeholderUpload").hide();
});

myDropzoneDocument1.on("totaluploadprogress", function (progress) {

    $("#total-progress .progress-bar").css("width", progress + "%");
    $("#progressPercent").html(progress + "%");
});

myDropzoneDocument1.on("sending", function (file, xhr, formData) {


    formData.append("id", $("#IdDocumentUpload").val());
    formData.append("IsRFC", $("#IsRFC").val());
    formData.append("butttonRFC", false);
    // Show the total progress bar when upload starts
    $("#total-progress").css("opacity", 1);
    // And disable the start button
    $("#actions .delete").attr("disabled", "disabled");
    $(".start").attr("disabled", "disabled");

});

myDropzoneDocument1.on("complete", function (resp) {


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
            tablearmada();
            tablearmadaForRFC();
        }
    }
});

myDropzoneDocument1.on("queuecomplete", function (progress) {

    $("#total-progress").css("opacity", "0");
    setTimeout(function () {
        myDropzoneDocument1.removeAllFiles(true);
    }, 400);
});

$("#actions .start").on("click", function () {
    myDropzoneDocument1.enqueueFiles(myDropzoneDocument1.getFilesWithStatus(Dropzone.QUEUED));
});

$("#actions .cancel").on("click", function () {
    myDropzoneDocument1.removeAllFiles(true);
    $("#placeholderUpload").hide();
});

// For RFC Document Upload
var myDropzoneDocument2 = new Dropzone("#FormUploadDocumentArmadaContainerForRFC", { // Make the bodyFormUpload a dropzone

    url: "/EMCS/GrDocumentUploadArmada", // Set the url

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
    previewTemplate: $("#template-dropzone1").html(),
    uploadMultiple: false
    //previewsContainer: "#FormUploadMaterial", // Define the container to display the previews
    //clickable: ".fileinput-button" // Define the element that should be used as click trigger to select files.
}
);

myDropzoneDocument2.on("addedfile", function (file) {

    // Hookup the start button
    $("#actions .start").on("click", function () {
        myDropzone.enqueueFile(file);
    });
    $("#placeholderUpload").hide();
});

myDropzoneDocument2.on("totaluploadprogress", function (progress) {

    $("#total-progress .progress-bar").css("width", progress + "%");
    $("#progressPercent").html(progress + "%");
});

myDropzoneDocument2.on("sending", function (file, xhr, formData) {


    formData.append("id", $("#IdDocumentUpload").val());
    formData.append("IsRFC", $("#IsRFC").val());
    formData.append("buttonRFC", true);
    // Show the total progress bar when upload starts
    $("#total-progress").css("opacity", 1);
    // And disable the start button
    $("#actions .delete").attr("disabled", "disabled");
    $(".start").attr("disabled", "disabled");

});

myDropzoneDocument2.on("complete", function (resp) {


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
            tablearmada();
            tablearmadaForRFC();
        }
    }
});

myDropzoneDocument2.on("queuecomplete", function (progress) {

    $("#total-progress").css("opacity", "0");
    setTimeout(function () {
        myDropzoneDocument2.removeAllFiles(true);
    }, 400);
});

$("#actions .start").on("click", function () {
    myDropzoneDocument2.enqueueFiles(myDropzoneDocument2.getFilesWithStatus(Dropzone.QUEUED));
});

$("#actions .cancel").on("click", function () {
    myDropzoneDocument2.removeAllFiles(true);
    $("#placeholderUpload").hide();
});
