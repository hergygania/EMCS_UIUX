$table = $('#TblGoodReceive');
$searchInput = $("#txtSearchData").val();

window.operateEvents = {
    'click .edit': function (e, value, row, index) {
        location.href = "/emcs/EditGRForm/" + row.Id;
    },
    'click .preview': function (e, value, row, index) {
        location.href = "/emcs/PreviewGR/" + row.Id;
    },
    'click .remove': function (e, value, row, index) {
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
        });
    }
};

function deleteThis(id) {
    debugger;
    $.ajax({
        type: "POST",
        url: myApp.root + 'EMCS/RemoveGR',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: { Id: id },
        dataType: "json",
        success: function (d) {
            if (d.Msg !== undefined) {
                sAlert('Success', d.Msg, 'success');
            }

            $("[name=refresh]").trigger('click');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });

};

function formatterGR(data, row, index) {
    var btnEdit = "";
    var btnDelete = "";
    var btnPreview = "";
    if (row.Status === "Draft" || row.Status === "Revise") {
<<<<<<< HEAD
        btnEdit = "<button class='btn edit btn-xs btn-link btn-info'><i class='tim-icons icon-pencil'></i></button>";
        btnDelete = "<button class='btn delete btn-xs btn-link btn-danger remove'><i class='tim-icons icon-simple-remove'></i></button>";
    } else {
        btnPreview = "<button class='btn preview btn-xs btn-link btn-success'><i class='tim-icons icon-zoom-split'></i></button>";
=======
        btnEdit = "<button class='btn edit btn-xs btn-primary'><i class='fa fa-edit'></i></button>";
        btnDelete = "<button class='btn delete btn-xs btn-danger remove'><i class='fa fa-times'></i></button>";
    } else {
        btnPreview = "<button class='btn preview btn-xs btn-default'><i class='fa fa-search'></i></button>";
>>>>>>> 639d8d0 (Intial commit)
    }
    return ["<div>", btnEdit, btnDelete, btnPreview, "</div>"].join(" ");
}

var columnList = [
    {
<<<<<<< HEAD
    //    field: '',
    //    title: 'No',
    //    halign: 'center',
    //    align: 'center',
    //    class: 'text-nowrap',
    //    formatter: runningFormatter,
    //    sortable: true
    //}, {
=======
        field: '',
        title: 'No',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        formatter: runningFormatter,
        sortable: true
    }, {
>>>>>>> 639d8d0 (Intial commit)
        field: 'Id',
        title: 'Action',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        events: operateEvents,
        formatter: formatterGR,
        sortable: true
    }, {
        field: 'GrNo',
        title: 'GR No',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'PicName',
        title: 'PIC Name',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }, {
        field: 'PhoneNumber',
        title: 'Phone Number',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'NopolNumber',
        title: 'Nopol Number',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'KtpNumber',
        title: 'KTP Number',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'SimNumber',
        title: 'SIM Number',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'SimNumber',
        title: 'SIM Number',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'EstimationTimePickup',
        title: 'ETP',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        },
        sortable: true
    },
    {
        field: 'Status',
        title: 'Status',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return row.StatusViewByUser;
        }
    },
    {
        field: 'PickupDate',
        title: 'ATP',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        visible: false,
        sortable: true
    }
];

$(function () {
    $table.bootstrapTable({
        cache: false,
        url: "/Emcs/GetGrList",
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,
        searchOnEnterKey: true,
        sortOrder: 'DESC',
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        queryParams: function (params) {
            params.term = $("#mySearch input[name=searchText]").val();
            return params;
        },
        responseHandler: function (resp) {
            var data = {};
            $.map(resp, function (value, key) {
                data[value.Key] = value.Value;
            });
            return data;
            //return res.messages
        },
        formatNoMatches: function () {
            return '<span class="noMatches"> No data Match! </span>';
        },
        columns: columnList
    });

    $("#mySearch").insertBefore($("[name=refresh]"));
});