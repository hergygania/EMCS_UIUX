$table = $('#TblGoodReceive');
$searchInput = $("#txtSearchData").val();

window.operateEvents = {
    //'click .edit': function (e, value, row, index) {
    //    location.href = "/emcs/EditGRForm/" + row.Id;
    //},
    'click .edit': function (e, value, row, index) {
        if (this.value == "rfc")
            location.href = "/emcs/EditGRForm?id=" + row.Id + "&rfc=true";
        else
            location.href = "/emcs/EditGRForm?id=" + row.Id;
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

    $.ajax({
        type: "POST",
        url: myApp.root + 'EMCS/RemoveGR',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: { Id: id },
        dataType: "json",
        success: function (d) {

            /*if (d.Msg !== undefined) {*/
            swal('Success', "Data Updated SuccessFully", 'success')
            /*}*/
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
        btnEdit = "<button class='btn edit btn-xs btn-primary'><i class='fa fa-edit'></i></button>";
        btnDelete = "<button class='btn delete btn-xs btn-danger remove'><i class='fa fa-times'></i></button>";
    }
    else if (row.Status === "Approve" && row.PendingRFC == 0) {
              debugger;
                if (row.RoleID == 26 || row.RoleID == 15) {
                    btnEdit = "";
                }
                else {
                    btnEdit = "<button class='btn edit btn-xs btn-primary' value='rfc'><i class='fa fa-edit'></i></button>";
                }
        
        btnPreview = "<button class='btn preview btn-xs btn-default'><i class='fa fa-search'></i></button>";
    }
    else {
        btnPreview = "<button class='btn preview btn-xs btn-default'><i class='fa fa-search'></i></button>";
    }
    return ['<div>', btnEdit, btnDelete, btnPreview, '</div>'].join(' ');

       //if (options.Data.Status === 'Draft' || options.Data.Status === "Revise" || options.Data.StatusViewByUser === "Approve Revise By Imex") // (options.Data.Status === "Revise" && options.Data.StatusViewByUser !== "Pickup Goods")
    //{ btn.push('<button type="button" class="btn btn-xs btn-primary edit"  title="Edit"><i class="fa fa-edit"></i></button>'); }
    //else if (options.Data.StatusViewByUser !== null) {
    //        if (Boolean($("#IsCKB").val()) == true) {
    //             btnEdit = '<button type="button" value="rfc" class="btn btn-xs btn-primary edit"  title="Edit"><i class="fa fa-edit"></i></button>';
    //        }
        

    //}
    
    
}

var columnList = [
    {
        field: '',
        title: 'No',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        formatter: runningFormatter,
        sortable: true
    }, {
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
        field: 'EstimationTimePickup',
        title: 'ETP',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            if (data == null) {
                data = '/Date(1661884200000)/';
            }
            return moment(data).format("DD MMM YYYY");

        }

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

