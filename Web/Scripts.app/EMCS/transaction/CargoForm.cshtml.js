var $tableCargoForm = $('#tableCargoForm');
var $tableAddCargo = $('#tableAddCargo');

var $AllowDelete = $('#AllowDelete').val();
var $AllowUpdate = $('#AllowUpdate').val();

var $form = $("#formRequest");
var consignee_list = [], notify_list = [], ciplno_list = [];
$("#editItemCargoAll").hide();
var cargoIds = [];
Dropzone.autoDiscover = false;
$(function () {
    //$('#RequestForChangeHistoryCL').hide();
    //if ($("#CargoID").val() > 0) {
    //    $('#RequestForChangeHistoryCL').show();
    //    //if ($('#jenisBarangCipl').val() === 'CATERPILLAR SPAREPARTS') {
    //    //    $('#partUploadFile').show();
    //    //}

    //}
    // #region Table Cargo    
    $("#editItemCargoAll").on("click", function (row) {
       
        if (cargoIds.length > 0) {
            var objModel = {};
            objModel.cargoIds = cargoIds;
            $(".editItemCargo").attr('href', '/EMCS/EditCargoitem?cargoids=' + JSON.stringify(objModel)).trigger('click');
            
        }
        else {
            Swal.fire({
                title: "Warning",
                text: "Please select any one cargo item to update.",
                type: "warning"
            });
        }
 
    });
    initForm();
    setForm(CargoID);

    var tableCargoForm = [
        [{
            field: "",
            title: "NO",
            rowspan: 2,
            align: 'center',
            formatter: runningFormatterNoPaging
        },
        {
            field: "State",
            title: "State",
            checkbox: false,
            class: "myclass",
            rowspan: 2,
            events: operateEvents,
            formatter: function (data, row, index) {
                return "<input type='checkbox' id='vehicle3' class='checkCargo'>";
            }
        },
        {
            field: "Id",
            title: "ACTION",
            align: "center",
            rowspan: 2,
            events: operateEvents,
            visible: true,
            formatter: function (data, row, index) {
                var divOpen = "<div style='width:100%;'>";
                var CargoItemId = row.Id;
                btnEditGroup = "";
                btnEdit = "<button type='button' class='btn btn-primary btn-link btn-xs edititem' id='edit'><i class='tim-icons icon-cloud-upload-94'></i></button>";
                btnDelete = "<button type='button' class='btn btn-danger btn-link btn-xs deleteItem'><i class='tim-icons icon-simple-remove'></i></button>";
                divClose = "</div>";
                htm = [divOpen, btnEditGroup, btnEdit, btnDelete, divClose];
                return htm.join(" ");
            }
        }, {
            field: "IdCipl",
            rowspan: 2,
            visible: false
        }, {
            field: "ContainerNumber",
            title: "CONTAINER",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "IncoTerm",
            rowspan: 2,
            visible: false
        }, {
            field: "ContainerType",
            title: "CONTAINER TYPE",
            rowspan: 2,
            align: 'center',
            sortable: true,
            formatter: function (data, row, index) {
                if (data !== "") {
                    return data;
                }
                return "-";
            }
        }, {
            field: "ContainerSealNumber",
            title: "SEAL NUMBER",
            rowspan: 2,
            align: 'center',
            sortable: true,
            formatter: function (data, row, index) {
                if (data !== "") {
                    return data;
                }
                return "-";
            }
        }, {
            field: "CaseNumber",
            title: "CASE NUMBER",
            rowspan: 2,
            align: 'center',
            sortable: true,
            formatter: function (data, row, index) {
                if (data !== "") {
                    return data;
                }
                return "-";
            }
        }, {
            field: "ItemName",
            title: "ITEM NAME",
            rowspan: 2,
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: function (data, row, index) {
                if (data !== "") {
                    return data;
                }
                return "-";
            }
        }, {
            field: "Sn",
            title: "SN",
            rowspan: 2,
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: function (data, row, index) {
                if (data !== "") {
                    return data;
                }
                return "-";
            }
        }, {
            field: "reference",
            title: "REFERENCE",
            colspan: 2,
            sortable: true,
            align: 'center'
        }, {
            field: "CargoDescription",
            title: "CARGO DESCRIPTION",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "volume",
            title: "VOLUME",
            colspan: 3,
            halign: 'center',
            align: 'right',
            sortable: true,
            formatter: currencyFormatter
        }, {
            field: "NetWeight",
            title: "NET WEIGHT (KGS)",
            rowspan: 2,
            halign: 'center',
            align: 'right',
            sortable: true,
            formatter: currencyFormatter
        }, {
            field: "GrossWeight",
            title: "GROSS WEIGHT (KGS)",
            rowspan: 2,
            halign: 'center',
            align: 'right',
            sortable: true,
            formatter: currencyFormatter
        }],
        [{
            field: "EdoNo",
            title: "EDI Number",
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: "InboundDa",
            title: "INBOUND DA #",
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: "Length",
            title: "LENGTH",
            halign: 'center',
            align: 'right',
            sortable: true
        }, {
            field: "Width",
            title: "WIDTH",
            halign: 'center',
            align: 'right',
            sortable: true
        }, {
            field: "Height",
            title: "HEIGHT",
            halign: 'center',
            align: 'right',
            sortable: true
        }
        ]
    ]

    $tableCargoForm.bootstrapTable({
        columns: tableCargoForm,
        url: "/EMCS/GetCargoListItem",
        cache: false,
        sidePagination: 'client',
        pagination: true,
        search: false,
        queryParams: function (params) {
            var CargoId = $("#CargoID").val();
            return { "Id": CargoId }
        },
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        responseHandler: function (resp) {
            var data = {};
            $.map(resp, function (value, key) {
                data[value.Key] = value.Value;
            });
            
            if (data.total > 0) {
                $("#editItemCargoAll").show();
            }
            return data.rows;
        },
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">No Item Found</span>';
        }

    });
    $("#inpReference").on("change", function (elm) {
        console.log("ali mutasal 207 : ", elm);
    })
    // #endregion
    $("#RequestForChangeHistoryCL").click(function () {
        Swal.fire({
            title: 'Request this change?',
            text: 'By approving this changes, you are responsible for the authenticity of the documents and data entered. Are you sure you want to process this request of change?',
            type: 'question',
            showCancelButton: true,
            cancelButtonColor: '#d33',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Yes, Request!',
            allowEscapeKey: false,
            allowOutsideClick: false,
            showCloseButton: true
        }).then((result) => {
            if (result.value) {
                Swal.fire({
                    input: 'textarea',
                    allowEscapeKey: false,
                    allowOutsideClick: false,
                    inputPlaceholder: 'Please add reason for this request for change...',
                    inputAttributes: {
                        'aria-label': 'Please add reason for this request for change...'
                    },
                    showCancelButton: false
                }).then((result) => {
                    if (result.value !== '') {
                        var Notes = result.value;
                        var Status = "Approve";
                        var Id = $('#RFCId').val();
                        var formdata = { Notes: Notes, Status: Status, Id: Id };
                        RequestForChangeCL(Status,formdata);
                    }
                    else {
                        Swal.fire({
                            type: 'error',
                            title: 'Oops...',
                            text: 'Please add request for change reason',
                        })
                    }
                });
            }
            return false;
        });


    });
    function RequestForChangeCL(status, formdata) {
        debugger;
        var modelObj = {
            FormType: "Cargo",
            FormId: $("#CargoID").val(),
            Reason: formdata.Notes,
            /*rfcList: requestForChange*/
        }
        var result = "";
        var data = {
            Id: $('#CargoID').val(),
            Consignee: $('#Consignee').val(),
            NotifyParty: $('#NotifyParty').val(),
            ExportType: $('#ExportType').val(),
            Category: $('#Category').val(),
            Referrence: $('#inpReference').val(),
            Incoterms: $('#Incoterms').val(),
            StuffingDateStarted: $('#inpStartStuffDateCl').val(),
            StuffingDateFinished: $('#inpFinishStuffDateCl').val(),
            //ETA: $('#inpETA').val(),
            //ETD: $('#inpETD').val(),
            VesselFlight: $('#inpVeselCl').val(),
            ConnectingVesselFlight: $('#inpConnectVeselCl').val(),
            VoyageVesselFlight: $('#inpVoyageCl').val(),
            VoyageConnectingVessel: $('#inpVoyageConnectCl').val(),
            PortOfLoading: $('#inpPortLoadCl').val(),
            PortOfDestination: $('#inpPortDiscCl').val(),
            SailingSchedule: $('#inpFlightCl').val(),
            ArrivalDestination: $('#inpArrivalCl').val(),
            BookingNumber: $('#inpBookNumCl').val(),
            BookingDate: $('#inpBookDateCl').val(),
            CargoType: $('#CargoType').val(),
            ShippingMethod: $('#ShippingMethod').val(),
            Liner: $('#inpLinerCl').val(),
            Status: status
        }

        $.ajax({
            url: '/emcs/SaveChangeHistoryCL',
            type: 'POST',
            data: {
                form: modelObj,
                item: data
            },
            cache: false,
            success: function (data, response) {
                if (data.Status === 0) {
                    $("#btnAddCIPL").removeAttr("disabled");
                    Swal.fire("Success", data.Msg, "success");
                    var detail = data.result.cargoData;
                    $("#CargoID").val(detail.Id);
                    $("#CreateDate").val(moment(detail.CreateDate).format("DD MMM YYYY"));
                    $("#inpClNo").val(detail.ClNo);
                    $("#inpPreparedCl").val(detail.PreparedBy);
                    if (status == "Submit") {
                        location.href = myApp.fullPath + "EMCS/CargoList";
                    }
                } else {
                    Swal.fire("Failed Submit", "Cargo Item is not complete !", "warning");
                }
                return false;
            },
            error: function (e) {
                Swal.fire("Error", e.statusText, "error");
            }
        });
    }
    
    $('#inpPortLoadCl').select2({
        placeholder: "Select Loading Port",
        ajax: {
            url: "/emcs/GetLocalPortData",
            dataType: 'json',
            data: function (params) {
                return {
                    Name: params.term
                };
            },
            success: function (data, response) {
            },
            processResults: function (data) {
                return {
                    results: $.map(data.data, function (item) {
                        console.log(item);
                        return {
                            text: item.Text,
                            id: item.Id
                        }
                    })
                }
            }
        }
    })

    $('#inpPortDiscCl').select2({
        placeholder: "Select Destination / Discharge Port",
        ajax: {
            url: "/emcs/GetPortData",
            dataType: 'json',
            data: function (params) {
                return {
                    Name: params.term
                };
            },
            success: function (data, response) {
            },
            processResults: function (data) {
                return {
                    results: $.map(data.data, function (item) {
                        return {
                            text: item.Text,
                            id: item.Id
                        }
                    })
                }
            }
        }
    })
});

function removeItem() {
    Swal.fire({
        title: "Warning",
        text: "Please complete the form required before add items.",
        type: "warning"
    });
}

//function getTotalData() {
//    var IdCargo = $("#CargoID").val();
//    $.get(myApp.fullPath + "EMCS/GetTotalDataCargo/" + IdCargo, function (resp) {
//        var data = {};
//        $.map(resp, function (value, key) {
//            data[value.Key] = value.Value;
//        });
//        $("#inpTotalPackageCl").val(formatCurrency(data.totalPackage, ".", ",", 2));
//        $("#inpTotalGrossWeightCl").val(formatCurrency(data.totalGrossWeight, ".", ",", 2));
//        $("#inpTotalNetWeightCl").val(formatCurrency(data.totalNetWeight, ".", ",", 2));
//        $("#inpTotalVolumeCl").val(data.totalVolume);
//    });
//}
function getTotalData() {
    var IdCargo = $("#CargoID").val();
    var selectvalue = $('#select').val();
    $.get(myApp.fullPath + "EMCS/GetTotalDataCargo?Id=" + IdCargo + "&selectvalue=" + selectvalue, function (resp) {
        var data = {};
        $.map(resp, function (value, key) {
            data[value.Key] = value.Value;
        });
        $("#inpTotalPackageCl").val(formatCurrency(data.totalPackage, ".", ",", 2));
        $("#inpTotalGrossWeightCl").val(formatCurrency(data.totalGrossWeight, ".", ",", 2));
        $("#inpTotalNetWeightCl").val(formatCurrency(data.totalNetWeight, ".", ",", 2));
        $("#inpTotalVolumeCl").val(data.totalVolume);
    });
}

$("#CargoType").on("change", function () {
    $(".btnAddCIPL").prop("disabled", false);
    var CargoType = $("#CargoType").val();
    if (CargoType === "FCL") {
        $tableCargoForm.bootstrapTable('showColumn', 'CaseNumber');
        $tableCargoForm.bootstrapTable('showColumn', 'ContainerNumber');
        $tableCargoForm.bootstrapTable('showColumn', 'ContainerType');
        $tableCargoForm.bootstrapTable('showColumn', 'ContainerSealNumber');
        visible = true;
    } else {
        $tableCargoForm.bootstrapTable('hideColumn', 'CaseNumber');
        $tableCargoForm.bootstrapTable('hideColumn', 'ContainerNumber');
        $tableCargoForm.bootstrapTable('hideColumn', 'ContainerType');
        $tableCargoForm.bootstrapTable('hideColumn', 'ContainerSealNumber');
        visible = false;
    }
});

// #region Btn Action
$(".btnAddCIPL").on("click", function () {
    
    var CargoType = $("#CargoType").val();
    var Consignee = $("#Consignee").valid();
    var NotifyParty = $("#NotifyParty").valid();
    var ExportType = $("#ExportType").valid();
    var Category = $("#Category").valid();
    var Incoterms = $("#Incoterms").valid();
    var ShippingMethod = $("#ShippingMethod").valid();
    var Reference = $("#inpReference").valid();

    if (CargoType && Consignee && NotifyParty && ExportType && Category && Incoterms && ShippingMethod && Reference) {
        var CargoId = $("#CargoID").val() ? $("#CargoID").val() : "0";
        if (CargoId === "0") {
            CargoId = post_insert_cargo_normal("Draft", CargoId);
            if (CargoType === "FCL") {
                $(".linkCreateCargo").attr("href", "/EMCS/CreateCargoContainer/?Id=" + CargoId).trigger("click");
            } else {
                $(".linkCreateCargo").attr("href", "/EMCS/CreateCargoItem/?Id=" + CargoId).trigger("click");
            }
        } else {
            CargoId = post_insert_cargo_normal("Draft", CargoId);
            if (CargoType === "FCL") {
                $(".linkCreateCargo").attr("href", "/EMCS/CreateCargoContainer/?Id=" + CargoId).trigger("click");
            } else {
                $(".linkCreateCargo").attr("href", "/EMCS/CreateCargoItem/?Id=" + CargoId).trigger("click");
            }
        }
    } else {
        Swal.fire({
            title: "Warning",
            text: "Please complete the form required before add items.",
            type: "warning"
        });
    }
});
// #endregion

// #region Validation Form 
$("#formRequest").validate({
    onkeyup: false,
    errorClass: "input-validation-error",
    //put error message behind each form element
    errorPlacement: function (error, element) {
        if (element.hasClass("select2") && element.hasClass("input-validation-error")) {
            element.next("span").addClass("input-validation-error");
        }
        else if (element.hasClass("select2-hidden-accessible")) {
            error.insertAfter(element.parent('span.select2'));
        }
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

$(".select2, .date").on("change", function (e) {
    var id = $(this).attr("id");
    $("#" + id).valid();
});
// #endregion

// #region Select2 Initialize
//$("#IdKppbc").select2({
//    //minimumInputLength: 3,
//    placeholder: 'Search for Kppbc',
//    maximumInputLength: 20,
//    tags: false, //prevent free text entry
//    width: "100%",
//    ajax: {
//        url: "/EMCS/GetKppbcList",
//        type: "get",
//        data: function (params) {
//            var query = {
//                searchName: params.term
//            }

//            return query;
//        },
//        processResults: function (data) {
//            // Transforms the top-level key of the response object from 'items' to 'results'
//            console.log(data.data);
//            var options = [];
//            $.map(data.data, function (obj) {
//                var item = {};
//                item.id = obj.ID;
//                item.text = obj.Name;
//                item.kab = obj.Kab;
//                item.prop = obj.Propinsi;
//                item.code = obj.Code;
//                options.push(item);
//            });
//            console.log(options);
//            return {
//                results: options
//            };
//        }
//    },
//    templateResult: function (repo) {
//        var htm = repo.code + " - " + repo.text;
//        return htm;
//    }
//});

$(".Select2Port").select2({
    //minimumInputLength: 3,
    placeholder: 'Search for Port',
    maximumInputLength: 20,
    tags: false, //prevent free text entry
    width: "100%",
    ajax: {
        url: "/EMCS/GetPortData",
        type: "get",
        data: function (params) {
            var type = $(".typePort").val() ? $(".typePort").val() : "SeaPort";

            var query = {
                Type: type,
                Name: params.term
            }

            return query;
        },
        processResults: function (data) {
            // Transforms the top-level key of the response object from 'items' to 'results'
            console.log(data.data);
            var options = [];
            $.map(data.data, function (obj) {
                var item = {};
                item.id = obj.id;
                item.text = obj.text;
                item.country = obj.country;
                item.code = obj.code;
                options.push(item);
            });

            return {
                results: options
            };
        }
    },
    templateResult: function (repo) {
        var htm = repo.code + " - " + repo.text;
        return htm;
    }
});

//$("#ShippingMethod").select2({
//    //minimumInputLength: 3,
//    placeholder: 'Search for Shipping Method',
//    maximumInputLength: 20,
//    tags: false, //prevent free text entry
//    width: "100%",
//    ajax: {
//        url: "/EMCS/GetOptionItem",
//        type: "get",
//        data: function (params) {
//            var query = {
//                searchCode: "ShippingMethod",
//                searchName: params.term
//            }

//            return query;
//        },
//        processResults: function (data) {
//            // Transforms the top-level key of the response object from 'items' to 'results'
//            console.log(data.data);
//            var options = [];
//            $.map(data.data, function (obj) {
//                var item = {};
//                item.id = obj.id;
//                item.text = obj.text;
//                options.push(item);
//            });
//            console.log(options);
//            return {
//                results: options
//            };
//        }
//    }
//});

$("#CargoType").select2({
    //minimumInputLength: 3,
    placeholder: 'Search for Cargo Type',
    maximumInputLength: 20,
    tags: false, //prevent free text entry
    width: "100%",
    ajax: {
        url: "/EMCS/GetParamItems",
        type: "get",
        data: function (params) {
            var query = {
                searchCode: "CargoType",
                searchName: params.term
            }

            return query;
        },
        processResults: function (data) {
            var options = [];
            $.map(data.data, function (obj) {
                var item = {};
                item.id = obj.Id;
                item.text = obj.Text;
                options.push(item);
            });
            console.log(options);
            return {
                results: options
            };
        }
    }
});

$("#inpReference").on("change", function () {
    var inpRef = $("#inpReference").val();
    console.log(inpRef);
    if (inpRef === null) {
        $("#Consignee").val(null).trigger("change");
        $("#NotifyParty").val(null).trigger("change");
        $("#ExportType").val(null).trigger("change");
        $("#Incoterms").val(null).trigger("change");
        $("#Category").val(null).trigger("change");
        $("#ShippingMethod").val(null).trigger("change");
    }
    //removeItem();
});

$("#inpReference").select2({
    //minimumInputLength: 3,
    placeholder: 'Search for reference',
    maximumInputLength: 20,
    tags: false, //prevent free text entry
    width: "100%",
    ajax: {
        url: "/emcs/GetCiplAvailable",
        type: "get",
        data: function (params) {
            var IdCipl = $("#inpReference").val();
            var IdCargo = $("#CargoID").val();

            if (IdCipl) {
                IdCipl = IdCipl.join(',');
            }

            //var query = {
            //    Search: params.term,
            //    ciplno: IdCipl,
            //    cargoid: IdCargo,
            //    ConsigneeName: $("#Consignee").val(),
            //    NotifyParty: $("#NotifyParty").val(),
            //    ExportType: $("#ExportType").val(),
            //    Incoterms: $("#Incoterms").val(),
            //    Category: $("#Category").val(),
            //    ShippingMethod: $("#ShippingMethod").val()
            //}

            return {
                Search: params.term,
                ciplno: params.term,
                cargoid: IdCargo,
                ConsigneeName: $("#Consignee").val(),
                NotifyParty: $("#NotifyParty").val(),
                ExportType: $("#ExportType").val(),
                Incoterms: $("#Incoterms").val(),
                Category: $("#Category").val(),
                ShippingMethod: $("#ShippingMethod").val()
            };

            //return query;
        },
        processResults: function (data) {
            // Transforms the top-level key of the response object from 'items' to 'results'
            console.log(data.data);
            var options = [];
            $.map(data.data, function (obj) {
                var item = {};
                item.id = obj.CiplId;
                item.text = obj.DoNo;
                item.CategoriItem = obj.CategoriItem;
                item.Category = obj.Category;
                item.ConsigneeCountry = obj.ConsigneeCountry;
                item.ConsigneeName = obj.ConsigneeName;
                item.ExportType = obj.ExportType;
                item.ExportTypeItem = obj.ExportTypeItem;
                item.IncoTerm = obj.IncoTerm;
                item.NotifyName = obj.NotifyName;
                item.ShippingMethod = obj.ShippingMethod;
                options.push(item);
            });

            return {
                results: options
            };
        }
    }
});

//$("#ShippingMethod").on("change", function (e) {
//    var value = $("#ShippingMethod").val();
//    if (value === "Sea") {
//        $("#inpVeselCl").val("Vessel");
//    } else {
//        $("#inpVeselCl").val("Flight");
//    }
//});

$('#inpReference').on('select2:select', function (e) {
    var data = e.params.data;

    //var objConsignee = new Option(data.ConsigneeName, data.ConsigneeName, true, true);
    //var objNotifyParty = new Option(data.NotifyName, data.NotifyName, true, true);
    //var objExportType = new Option(data.ExportType, data.ExportType, true, true);
    //var objCategory = new Option(data.Category, data.Category, true, true);
    //var objIncoterms = new Option(data.IncoTerm, data.IncoTerm, true, true);
    //var objShippingMethod = new Option(data.ShippingMethod, data.ShippingMethod, true, true);

    //$("#Consignee").append(objConsignee).trigger("change");
    //$("#NotifyParty").append(objNotifyParty).trigger("change");
    //$("#ExportType").append(objExportType).trigger("change");
    //$("#Category").append(objCategory).trigger("change");
    //$("#Incoterms").append(objIncoterms).trigger("change");
    //$("#ShippingMethod").append(objShippingMethod).trigger("change");

    $("#Consignee").val(data.ConsigneeName);
    $("#NotifyParty").val(data.NotifyName);
    $("#ExportType").val(data.ExportType);
    $("#Category").val(data.Category);
    $("#Incoterms").val(data.IncoTerm);
    $("#ShippingMethod").val(data.ShippingMethod);


    console.log(data);
});

$("#CIPLNo").select2({
    width: '100%',
    placeholder: 'Please Select CIPL No',
    multiple: true,
    ajax: {
        url: "/EMCS/GetCIPLNoList",
        type: "GET",
        data: function (params) {
            var query = {
                searchName: params.term,
                consignee: $('#Consignee').val(),
                notify: $('#NotifyParty').val(),
                exporttype: $('#ExportType').val(),
                category: $('#Category').val(),
                incoterms: $('#Incoterms').val()
            }
            return query;
        },
        processResults: function (data) {
            return {
                results: data
            };
        }
    }
});
// #endregion

// #region Item Cipl Table
$("#CIPLNo").on("select2:select", function () {
    $tableAddCargo.bootstrapTable("refresh");
});
// #endregion

// #region SetForm Value
function initForm() {
    $('.calStuffStart').click(function () {
        $('#inpStartStuffDateCl').focus().datepicker({
            format: "mm/DD/YYYY",
            autoclose: true
        });
    });
    $('.calStuffFinish').click(function () {
        $('#inpFinishStuffDateCl').focus().datepicker({
            format: "mm/DD/YYYY",
            autoclose: true
        });
    });
    $('.calFlightCl').click(function () {
        $('#inpFlightCl').focus().datepicker({
            format: "mm/DD/YYYY",
            autoclose: true
        });
    });
    $('.calArrivalCl').click(function () {
        $('#inpArrivalCl').focus().datepicker({
            format: "mm/DD/YYYY",
            autoclose: true
        });
    });

    $('.calBookDateCl').click(function () {
        $('#inpBookDateCl').focus().datepicker({
            format: "mm/DD/YYYY",
        });
    });
    //$('.calETACl').click(function () {
    //    $('#inpETA').focus().datepicker({
    //        format: "mm/DD/YYYY",
    //        autoclose: true
    //    });
    //});
    //$('.calETDCl').click(function () {
    //    $('#inpETD').focus().datepicker({
    //        format: "mm/DD/YYYY",
    //        autoclose: true
    //    });
    //});

}


//$("#checkcn").on('click', function () {

//    var cargoid = $('#CargoID').val();
//    var cnno = $('#containernumber').val();

//    if (cargoid != 0 || cargoid != null && cnno != 0 || cnno != null) {
//        $.ajax({
//            url: "/emcs/CheckCNNo",
//            success: function (data) {

//            }
//        })

//    }
//})

function setForm(CargoID) {
    $('#CargoID').val(CargoID);
    if (CargoID > 0) {
        $.ajax({
            url: "/emcs/GetCargoData?CargoID=" + CargoID,
            success: function (data) {

                console.log(data);
                $(".btnAddCIPL").removeAttr("disabled");
                $('#inpClNo').val(data.header.ClNo);
                $('#Consignee').val(data.header.Consignee).attr("readonly", "readonly");
                $("#NotifyParty").val(data.header.NotifyParty).attr("readonly", "readonly");
                $('#ExportType').val(data.header.ExportType).attr("readonly", "readonly");
                $('#Category').val(data.header.Category).attr("readonly", "readonly");
                $('#Incoterms').val(data.header.IncotermsDesc).attr("readonly", "readonly");
                $('#CreateDate').val(moment(data.header.CreateDate).format("DD MMM YYYY"));
                $('#inpPreparedCl').val(data.header.PreparedBy);

                if (data.cipllist) {
                    if (data.cipllist.length > 0) {
                        jQuery.each(data.cipllist, function (index, value) {
                            $("#inpReference").append(new Option(value.EdoNumber, value.IdCipl, false, true)).trigger("change");
                        });
                    }
                }

                $('#CargoType').append(new Option(data.header.CargoTypeName, data.header.CargoType, true, true)).trigger("change");
                $('#ShippingMethod').val(data.header.ShippingMethod).attr("readonly", "readonly");
                $('#inpContainerNumCl').val(data.header.Container);
                $('#inpCargoDescCl').val(data.header.CargoDescription);
                (data.header.StuffingDateStarted) ? $('#inpStartStuffDateCl').val(moment(data.header.StuffingDateStarted).format('DD MMM YYYY')) : "";
                (data.header.StuffingDateFinished) ? $('#inpFinishStuffDateCl').val(moment(data.header.StuffingDateFinished).format('DD MMM YYYY')) : "";

                $('#inpVeselCl').val(data.header.VesselFlight);
                $('#inpConnectVeselCl').val(data.header.ConnectingVesselFlight);
                $('#inpVoyageCl').val(data.header.VoyageVesselFlight);
                $('#inpVoyageConnectCl').val(data.header.VoyageConnectingVessel);

                var inpPortLoadClOption = new Option(data.header.PortOfLoading, data.header.PortOfLoading, true, true);
                $('#inpPortLoadCl').append(inpPortLoadClOption).trigger("change");

                var inpPortDiscClOption = new Option(data.header.PortOfDestination, data.header.PortOfDestination, true, true);
                $('#inpPortDiscCl').append(inpPortDiscClOption).trigger("change");

                (data.header.SailingSchedule) ? $('#inpFlightCl').val(moment(data.header.SailingSchedule).format('DD MMM YYYY')) : "";
                (data.header.ArrivalDestination) ? $('#inpArrivalCl').val(moment(data.header.ArrivalDestination).format('DD MMM YYYY')) : "";

                $('#inpBookNumCl').val(data.header.BookingNumber);
                (data.header.BookingDate) ? $('#inpBookDateCl').val(moment(data.header.BookingDate).format('DD MMM YYYY')) : "";
                $('#inpLinerCl').val(data.header.Liner);
                //$tableCargoForm.bootstrapTable("refresh");

                getTotalData();
            }
        });
    }
    var editable = window.location.pathname.split('/')[2] !== "ViewCargo";
    setViewForm(editable);
}

function setViewForm(editable) {
    $('#Consignee').prop('disabled', !editable);
    $('#NotifyParty').prop('disabled', !editable);
    $('#ExportType').prop('disabled', !editable);
    $('#Category').prop('disabled', !editable);
    $('#Incoterms').prop('disabled', !editable);

    $('#inpStartStuffDateCl').prop('disabled', !editable);
    $('#inpFinishStuffDateCl').prop('disabled', !editable);
    $('#inpVeselCl').prop('readonly', !editable);
    $('#inpConnectVeselCl').prop('readonly', !editable);
    $('#inpVoyageCl').prop('readonly', !editable);
    $('#inpVoyageConnectCl').prop('readonly', !editable);
    $('#inpPortLoadCl').prop('readonly', !editable);
    $('#inpPortDiscCl').prop('readonly', !editable);
    $('#inpFlightCl').prop('disabled', !editable);
    $('#inpArrivalCl').prop('disabled', !editable);
    $('#inpBookDateCl').prop('disabled', !editable);
    $('#inpBookNumCl').prop('readonly', !editable);
    $('#inpLinerCl').prop('readonly', !editable);

    if (editable) {
        $("#btnAddCIPL").show();
        $("#btnSaveCargo").show();
        $("#btnSubmitCargo").show();
        $("#btnCancelCargo").hide();
    } else {
        $("#btnAddCIPL").hide();
        $("#btnSaveCargo").hide();
        $("#btnSubmitCargo").hide();
        $("#btnCancelCargo").show();
    }
}
// #endregion 

function refreshCIPLNo() {
    var consignee = $('#Consignee :selected').text().trim();
    var notify = $('#NotifyParty :selected').text().trim();
    var exporttype = $('#ExportType').val();
    var category = $('#Category').val();
    var incoterms = $('#Incoterms').val();
    if (consignee !== "" && notify !== "" && exporttype !== null && category != null && incoterms != null) {
        $.ajax({
            url: "/emcs/GetCIPLNoList?consignee=" + consignee + "&notify=" + notify + "&exporttype=" + exporttype + "&category=" + category + "&incoterms=" + incoterms,
            success: function (data) {
                $("#CIPLNo").select2({
                    data: data,
                    width: '100%',
                    placeholder: 'Please Select CIPL No',
                }).on("change", function () {
                    $tableAddCargo.bootstrapTable('removeAll');
                    if ($('#CIPLNo').val() != null) {
                        $.ajax({
                            url: "/emcs/GetCIPLforCargo?ciplno=" + $('#CIPLNo').val().join(',') + "&cargoid=" + $('#CargoID').val(),
                            success: function (data) {
                                $tableAddCargo.bootstrapTable('load', data);
                            }
                        });
                    }
                });
                $('#CIPLNo').val(ciplno_list.length > 0 ? ciplno_list : null).trigger('change');
            }
        });
    }
}

function addContainer() {
    var container = $('#inpContainerNumCl').val();
    var cargoDesc = $('#inpCargoDescCl').val();
    var result = [];

    var addCIPL = $tableAddCargo.bootstrapTable('getData');
    addCIPL.map(function (data, i) {
        result.push({
            ID: data.ID,
            IdCipl: data.IdCipl,
            Container: container,
            IncoTerm: data.IncoTerm,
            IncoTermNumber: data.IncoTermNumber,
            CaseNumber: data.CaseNumber,
            EdoNo: data.EdoNo,
            InBoundDa: data.InBoundDa,
            CargoDescription: cargoDesc,
            Length: data.Length,
            Width: data.Width,
            Height: data.Height,
            Net: data.NetWeight,
            Gross: data.GrossWeight
        });
    });
    $tableCargoForm.bootstrapTable('removeAll');
    $tableCargoForm.bootstrapTable('load', result);
}

function post_cargo(status) {
    if (status === 'Submit') {
        var isValid = $("#formRequest").valid();
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
                    post_insert_cargo(status);
                }
            });
        } else {
            Swal.fire("Attention!", "Please complete all required field in this form, before submit!", "error");
        }
    } else {
        post_insert_cargo(status);
    }
}

function post_insert_cargo_normal(status, CargoId) {
    var data = {
        Id: CargoId,
        Consignee: $('#Consignee').val(),
        NotifyParty: $('#NotifyParty').val(),
        ExportType: $('#ExportType').val(),
        Category: $('#Category').val(),
        Referrence: $('#inpReference').val(),
        Incoterms: $('#Incoterms').val(),
        StuffingDateStarted: $('#inpStartStuffDateCl').val(),
        StuffingDateFinished: $('#inpFinishStuffDateCl').val(),
        VesselFlight: $('#inpVeselCl').val(),
        ConnectingVesselFlight: $('#inpConnectVeselCl').val(),
        VoyageVesselFlight: $('#inpVoyageCl').val(),
        VoyageConnectingVessel: $('#inpVoyageConnectCl').val(),
        PortOfLoading: $('#inpPortLoadCl').val(),
        PortOfDestination: $('#inpPortDiscCl').val(),
        SailingSchedule: $('#inpFlightCl').val(),
        ArrivalDestination: $('#inpArrivalCl').val(),
        BookingNumber: $('#inpBookNumCl').val(),
        BookingDate: $('#inpBookDateCl').val(),
        CargoType: $('#CargoType').val(),
        ShippingMethod: $('#ShippingMethod').val(),
        Liner: $('#inpLinerCl').val(),
        Status: status
    }
/*    var CargoId = 0;*/
    $.ajax({
        url: '/EMCS/InsertCargo',
        type: 'POST',
        data: data,
        async: false,
        cache: false,
        success: function (data, response) {
            if (data.Status === 0) {
                $("#btnAddCIPL").removeAttr("disabled");
                var detail = data.result.cargoData;
                $("#CargoID").val(detail.Id);
                $("#CreateDate").val(moment(detail.CreateDate).format("DD MMM YYYY"));
                $("#inpClNo").val(detail.ClNo);
                $("#inpPreparedCl").val(detail.PreparedBy);
                return CargoId = detail.Id;
            }
            return CargoId;
        },
        error: function (e) {
            swal.fire("Error", e.statusText, "error");
        }
    });
    return CargoId;
}

function post_insert_cargo(status) {
    var result = "";
    var data = {
        Id: $('#CargoID').val(),
        Consignee: $('#Consignee').val(),
        NotifyParty: $('#NotifyParty').val(),
        ExportType: $('#ExportType').val(),
        Category: $('#Category').val(),
        Referrence: $('#inpReference').val(),
        Incoterms: $('#Incoterms').val(),
        StuffingDateStarted: $('#inpStartStuffDateCl').val(),
        StuffingDateFinished: $('#inpFinishStuffDateCl').val(),
        //ETA: $('#inpETA').val(),
        //ETD: $('#inpETD').val(),
        VesselFlight: $('#inpVeselCl').val(),
        ConnectingVesselFlight: $('#inpConnectVeselCl').val(),
        VoyageVesselFlight: $('#inpVoyageCl').val(),
        VoyageConnectingVessel: $('#inpVoyageConnectCl').val(),
        PortOfLoading: $('#inpPortLoadCl').val(),
        PortOfDestination: $('#inpPortDiscCl').val(),
        SailingSchedule: $('#inpFlightCl').val(),
        ArrivalDestination: $('#inpArrivalCl').val(),
        BookingNumber: $('#inpBookNumCl').val(),
        BookingDate: $('#inpBookDateCl').val(),
        CargoType: $('#CargoType').val(),
        ShippingMethod: $('#ShippingMethod').val(),
        Liner: $('#inpLinerCl').val(),
        Status: status
    }

    $.ajax({
        url: '/EMCS/InsertCargoAtBottom',
        type: 'POST',
        data: data,
        cache: false,
        success: function (data, response) {
            if (data.Status === 0) {
                $("#btnAddCIPL").removeAttr("disabled");
                Swal.fire("Success", data.Msg, "success");
                var detail = data.result.cargoData;
                $("#CargoID").val(detail.Id);
                $("#CreateDate").val(moment(detail.CreateDate).format("DD MMM YYYY"));
                $("#inpClNo").val(detail.ClNo);
                $("#inpPreparedCl").val(detail.PreparedBy);
                if (status == "Submit") {
                    location.href = myApp.fullPath + "EMCS/CargoList";
                }
            } else {
                Swal.fire("Failed Submit", "Cargo Item is not complete !", "warning");
            }
            return false;
        },
        error: function (e) {
            Swal.fire("Error", e.statusText, "error");
        }
    });
}

function post_insert_cargo_item(id) {
    $.ajax({
        url: '/EMCS/InsertCargoItem',
        type: 'POST',
        data: {
            data: $('#tableCargoForm').bootstrapTable('getData'),
            id: id,
        },
        cache: false,
        async: false,
        success: function (data, response) {
            Swal.fire({
                title: 'Saved',
                text: 'Your data has been saved.',
                type: 'success',
                showCancelButton: false
            }).then((result) => {
                if (result.value) {
                    window.location.href = '/EMCS/CargoList';
                }
            });
        },
        error: function (e) {
            Swal.fire("error!");
            e.preventDefault();
            Swal.fire("Error", e.statusText, "error");
        }
    })
}

function operateFormatter(options) {
    var btn = [];
    console.log(options);
    btn.push('<div class="btn-group">');
    if (options.Add == true)
        btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>')
    if (options.Edit == true && options.Data.status === 'Draft')
        btn.push('<button type="button" class="btn btn-primary edit" title="Edit"><i class="fa fa-edit"></i></button>');
    if (options.Delete == true && options.Data.status === 'Draft')
        btn.push('<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    //if (options.Info == true)
    btn.push('<button type="button" class="btn btn-info info" title="Info"><i class="fa fa-info-circle"></i></button>')

    btn.push('</div>');

    return btn.join('');
}

operateFormatter.DEFAULTS = {
    Add: false,
    Edit: false,
    Delete: false,
    Info: false,
    View: false,
    History: false
}

window.operateEvents = {
    'click .edititem': function (e, value, row, index) {
        
        $('.checkCargo').prop('checked', false);
        cargoIds = [];
        cargoIds.push(row.Id);
        var objModel = {};
        objModel.cargoIds = cargoIds;
        $(".editItemCargo").attr('href', '/EMCS/EditCargoitem?cargoids=' + JSON.stringify(objModel)).trigger('click');
        cargoIds = [];
    },
    'click .checkCargo': function (e, value, row, index) {
        
        newCargoIds = [];
        IsFound = false;
        if (cargoIds.length > 0) {
            for (var i = 0; i < cargoIds.length; i++) {
                if (cargoIds[i] == row.Id) {
                    IsFound = true;
                }
                else {
                    newCargoIds.push(cargoIds[i]);
                }
            }
        }
        else {
            newCargoIds.push(row.Id);
        }
        if (IsFound == false && cargoIds.length > 0) {
            newCargoIds.push(row.Id);
        }
        
        cargoIds = newCargoIds;
    },
    'click .editContainer': function (e, value, row, index) {

        console.log(row);
        console.log(value);
        $(".editContainerData").attr('href', '/EMCS/EditContainerCargo/' + row.ContainerId).trigger('click');
    },
    'click .deleteItem': function (e, value, row, index) {

        Swal.fire({
            title: 'Confirmation',
            text: 'Are you sure want to submit data?',
            type: 'question',
            showCancelButton: true,
            cancelButtonColor: '#d33',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Yes, Remove!',
            allowEscapeKey: false,
            allowOutsideClick: false,
            showCloseButton: true
        }).then((result) => {
            if (result.value) {
                console.log(row);
                deleteThis(row.Id);
                //post_insert_cargo(status);
                //location.href = myApp.fullPath + "EMCS/CargoList";
            }
        });
        return false;
    },
    'click .edit': function (e, value, row, index) {
        
        $('#Id').val(row.Id);
        $('#inp-doc-date').val(row.DocumentDate);
        $('#DocumentName').val(row.DocumentName);
    },
    'click .remove': function (e, value, row, index) {
        
        CargoDocumentDeleteById(row.Id);
        get_cargodocumentlist();
    },
    'click .upload': function (e, value, row, index) {
        
        $('#IdDocumentUpload').val(row.Id);
        //$(".uploadRecord").attr('href', '/EMCS/CiplDocumentUpload/' + row.Id).trigger('click');
    }
};

function getdataforedit(id) {
    
    $.ajax({
        url: '/EMCS/EditCargoItemById/' + id,
        type: 'Get',
        data: {
            Id: id,
        },
        success: function (data, response) {
            Swal.fire({
                type: 'success',
                title: 'Success'
                , text: 'Success, Your Data Has Been Delete',
            })
        }
    })
}
function CargoDocumentDeleteById(id) {
    
    $.ajax({
        url: '/EMCS/CargoDocumentDeleteById',
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

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'EMCS/DeleteCargoItem',
        beforeSend: function () {
            $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show();
        },
        complete: function () {
            $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide();
        },
        data: { Id: id },
        dataType: "JSON",
        success: function (d) {
            if (d.Msg !== undefined) {
                Swal.fire({
                    title: "Success",
                    text: "Delete cargo item succeed.",
                    type: 'success'
                });
            }
            getTotalData();
            $("[name=refresh]").trigger('click');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });

};

$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
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
        var formName = $(this).prop("name") ? $(this).prop("name") : "";
        var CargoID = $("#CargoID").val();
        var input = $(this).serializeArray();
        var category = $("#Category").val();
        var cargoType = $("#CargoType").val();
        var validationStat = true;
        var ids = [];
        var Url = this.action;

        //selection = $tableContainer.bootstrapTable("getData");
        //var selection = $tableContainer.bootstrapTable('getSelections');
        var selection = $tableContainer.bootstrapTable('getSelections');

        if (cargoType === "LCL" && formName === "FormUpdateItemCargo") {
            Url = "EditCargoItem";
        } else {
            if (cargoType !== "FCL") {
                var selection = $tableContainer.bootstrapTable("getAllSelections");
                var IdCargoItem = $(this).find("#IdCargoItem").val();

                Url = "InsertNoContainerItems";
                input.push({ name: 'Id', value: IdCargoItem });
            } else {
                validationStat = $(this).valid();
            }
        }

        jQuery.each(selection, function (index, value) {
            ids.push(value.Id);
        });

        if (validationStat) {
            if (formName !== "FormUpdateItemCargo") {
                var Description = $("#Description") ? $("#Description").val() : "";
                var ContainerNumber = $("#ContainerNumber") ? $("#ContainerNumber").val() : "";
                var ContainerType = $("#ContainerType") ? $("#ContainerType").val() : "";
                var ContainerSealNumber = $("#ContainerSealNumber") ? $("#ContainerSealNumber").val() : "";

                input.push({ name: 'Category', value: category });
                input.push({ name: 'Description', value: Description });
                input.push({ name: 'ContainerNumber', value: ContainerNumber });
                input.push({ name: 'ContainerType', value: ContainerType });
                input.push({ name: 'ContainerSealNumber', value: ContainerSealNumber });
            }

            input.push({ name: 'Items', value: ids.join(',') });
            input.push({ name: 'CargoId', value: CargoID });
            $('#progress').show();
            $.ajax({
                url: Url,
                type: this.method,
                data: input,
                success: function (result) {
                    if (result.Status === 0) {
                        if (result.Msg !== undefined)
                            Swal.fire('Success', result.Msg, 'success');
                        $('#myModalPlace').modal('hide');
                        $('#progress').hide();
                        $("[name=refresh]").trigger('click');
                    } else {
                        if (result.Msg !== undefined) Swal.fire('Failed', result.Msg, 'error');
                        $('#progress').hide();
                    }

                    getTotalData();
                    $('#progress').hide();
                }
            });
        } else {
            Swal.fire({
                title: "Form Failed",
                type: 'warning',
                text: "Please complete the form."
            });
        }
        return false;
    });
};
window.operateEventRight = {
    'click .download': function (e, value, row) {
        location.href = "/EMCS/DownloadCargoDocument/" + row.Id;
    },
    'click .showDocument': function (e, value, row) {
        document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/Cargo/" + row.Id + '/' + row.Filename;
    }
};

var myDropzoneDocument = new Dropzone("#FormUploadDocumentContainer", { // Make the bodyFormUpload a dropzone

    url: "/EMCS/CargoDocumentUpload", // Set the url

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
            get_cargodocumentlist();
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
