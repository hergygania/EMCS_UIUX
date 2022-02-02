var visible = false;
if (CargoTypeName === "Full Container Load") {
    visible = true;
}

function getTotalData() {
    var IdCargo = $("#CargoID").val();
    $.get(myApp.fullPath + "EMCS/GetTotalDataCargo/" + IdCargo, function (resp) {
        var data = {};
        $.map(resp, function (value, key) {
            data[value.Key] = value.Value;
        });
        $("#topackCl").val(formatCurrency(data.totalPackage, ".", ",", 2));
        $("#togroswegCl").val(formatCurrency(data.totalGrossWeight, ".", ",", 2));
        $("#tonetwegCl").val(formatCurrency(data.totalNetWeight, ".", ",", 2));
        //$("#tovolumeCl").val(formatCurrency(data.totalVolume, ".", ",", 2));
        $("#tovolumeCl").val(data.totalVolume);
    });
}
getTotalData();


function ReviseProblem(Status, IdRequest, Notes, statusName) {
    $.ajax({
        url: myApp.fullPath + "EMCS/CargoApprove",
        type: "Post",
        async: false,
        data: {
            Status: Status,
            Id: IdRequest,
            Notes: Notes
        },
        success: function (resp) {
            console.log(resp);
            Swal.fire({
                title: statusName + '!',
                text: 'Data Confirmed Successfully',
                type: 'success',
            }).then((result) => {
                window.location.href = "/EMCS/MyTask";
            });
        },
        error: function (resp) {
            Swal.fire({
                title: 'Error',
                text: 'Data Error. Please Try Again !',
                type: 'error',
            })
        }
    })
}

function submitProblem(statusName) {
    var status = false;
    var data = $("#myModalProblemContent form").serializeArray();
    data.push({ name: "Status", value: statusName });
    console.log(data);
    debugger;
    $.ajax({
        url: myApp.fullPath + "EMCS/SaveProblemHistory",
        type: "Post",
        async: false,
        data: data,
        success: function (resp) {
            var result = resp.result;
            status = true;
            ReviseProblem(result.Status, result.IdRequest, result.Comment, statusName);
        },
        error: function (resp) {
            Swal.fire({
                title: 'Error',
                text: 'Data Error. Please Try Again !',
                type: 'error',
            })
        }
    })
    return status;
}

$("#YesRejectBtn").on("click", function () {
    var status = $("#myModalProblemContent form").valid();
    var StatusName = "Reject";
    if (status) {
        submitProblem(StatusName);
    }
})

$("#YesReviseBtn").on("click", function () {
    var status = $("#myModalProblemContent form").valid();
    var StatusName = "Revise";
    if (status) {
        submitProblem(StatusName);
    }
})

var tableCargoForm = [
    [{
        field: "Id",
        title: "NO",
        rowspan: 2,
        align: 'center',
        formatter: runningFormatterNoPaging
    }, {
        field: "IdCipl",
        rowspan: 2,
        visible: false
    }, {
        field: "ContainerNumber",
        title: "CONTAINER",
        rowspan: 2,
        visible: visible,
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
        class: "text-nowrap",
        align: 'center',
        visible: visible,
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
        visible: visible,
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
        visible: visible,
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
        align: 'center',
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
        class: "text-nowrap",
        align: 'center',
        sortable: true
    }, {
        field: "volume",
        title: "CIPL VOLUME",
        colspan: 3,
        align: 'right',
        formatter: currencyFormatter,
        sortable: true
    }, {
        field: "weight",
        title: "CIPL Weight",
        colspan: 2,
        align: 'right',
        formatter: currencyFormatter,
        sortable: true
    }, {
        field: "newvolume",
        title: "New VOLUME",
        colspan: 3,
        align: 'right',
        formatter: currencyFormatter,
        sortable: true
    },  {
        field: "newweight",
        title: "New Weight",
        colspan: 2,
        align: 'right',
        formatter: currencyFormatter,
        sortable: true
    }],
    [{
        field: "EdoNo",
        title: "EDI Number #",
        align: 'center',
        sortable: true
    }, {
        field: "InboundDa",
        title: "INBOUND DA #",
        halign: 'center',
        align: 'left',
        sortable: true
    }, {
        field: "Length",
        title: "Cipl Length",
        formatter: currencyFormatter,
        align: 'right',
        sortable: true
    }, {
        field: "Width",
        title: "Cipl Width",
        align: 'right',
        formatter: currencyFormatter,
        sortable: true
    }, {
        field: "Height",
        title: "Cipl Height",
        align: 'right',
        formatter: currencyFormatter,
        sortable: true
    }, {
        field: "NetWeight",
        title: "CIPL NET WEIGHT (KGS)",
        align: 'right',
        formatter: currencyFormatter,
        sortable: true
    }, {
        field: "GrossWeight",
        align: 'right',
        title: "CIPL GROSS WEIGHT (KGS)",
        formatter: currencyFormatter,
        sortable: true
    }, {
        field: "NewLength",
        title: "New Length",
        formatter: currencyFormatter,
        align: 'right',
        sortable: true
    }, {
        field: "NewWidth",
        title: "New Width",
        align: 'right',
        formatter: currencyFormatter,
        sortable: true
    }, {
        field: "NewHeight",
        title: "New Height",
        align: 'right',
        formatter: currencyFormatter,
        sortable: true
    }, {
        field: "NewNetWeight",
        title: "New NET WEIGHT (KGS)",
        align: 'right',
        formatter: currencyFormatter,
        sortable: true
    }, {
        field: "NewGrossWeight",
        align: 'right',
        title: "New GROSS WEIGHT (KGS)",
        formatter: currencyFormatter,
        sortable: true
    }
    ]
]

$tableCargoForm = $("#tableCargoView");
$tableCargoForm.bootstrapTable({
    columns: tableCargoForm,
    url: myApp.fullPath + "/EMCS/GetCargoListItemApproval",
    cache: false,
    sidePagination: 'server',
    pagination: true,
    search: false,
    queryParams: function (params) {
        var CargoID = $("#CargoID").val();
        return { "Id": CargoID }
    },
    striped: false,
    clickToSelect: true,
    reorderableColumns: true,
    responseHandler: function (resp) {
        var data = {};
        $.map(resp, function (value, key) {
            data[value.Key] = value.Value;
        });
        return data;
    },
    toolbar: '.toolbar',
    toolbarAlign: 'left',
    onClickRow: selectRow,
    showColumns: true,
    showRefresh: true,
    smartDisplay: false,
    pageSize: '10',
    rowStyle: 'rowStyle',
    formatNoMatches: function () {
        return '<span class="noMatches">No Item Found</span>';
    },

});

function rowStyle(row, index) {

    if (row.NewLength != null || row.NewWidth != null || row.NewHeight != null || row.NewNetWeight != null || row.NewGrossWeight != null) { // possibly you map another property, it depends on your setting.

        return { classes: "bg-yellow" }
    }
    return {};
}

// #region Approve Function
function ApproveCargo(obj) {
    var Id = $("#CargoID").val();
    $.ajax({
        url: "/EMCS/CargoApprove",
        type: "POST",
        data: {
            Id: Id,
            Status: obj.Status,
            Notes: obj.Notes,
            Detail: {
                ReqType: "CL",
                IdRequest: Id,
                Category: obj.Category,
                Case: obj.Case,
                Causes: obj.Causes,
                Impact: obj.Impact,
                CaseDate: "20 Jan 2019"
            }
        },
        success: function (resp) {
            location.href = "/EMCS/CargoList";
        }
    });
}
// #endregion

// #region Btn Action Region
$("#btnApprove").on("click", function () {
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
                var Notes = result.value;
                var Status = "Approve";
                var IdCargo = $("#CargoID").val();
                var data = { Notes: Notes, Status: Status, IdCargo: IdCargo };

                ApproveCargo(data);
            });
        }
        return false;
    });
});
