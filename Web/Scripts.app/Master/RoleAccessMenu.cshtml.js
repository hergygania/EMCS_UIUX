
var json;
var dataOld;
var $table = $('#tableRoleMenu');

$(function () {

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');

    $("[name=refresh]").trigger('click');
    $("#ddlRole").select2({
        placeholder: 'Select Role',
        data: json,
        width: "245px"
    }).on("change", function (e) {
        var RoleID = $('#ddlRole').val();
        $('input:text[name=searchText]').val(RoleID);
        $("[name=refresh]").trigger('click');
        $table.bootstrapTable('refresh');
    });

    $table.bootstrapTable({
        //url: 'Pages/ReportProject/ReportProject.php',
        cache: false,
        pagination: true,
        search: true,
        striped: true,
        clickToSelect: true,
        onClickRow: selectRow,
        toolbarAlign: 'left',
        sidePagination: 'server',
        showColumns: false,
        showRefresh: true,
        //		showAdvSearch	: true,
        smartDisplay: false,
        detailView: true,
        pageSize: '5',
        onExpandRow: function (index, row, $detail) { expandTable($detail, row); },
        columns: [
            {
                field: 'SelectedMenu', title: 'Name Menu', halign: 'center', align: 'left', sortable: true
            },
            {
                field: 'IsRead', title: 'Is Read', halign: 'center', formatter: formatCheckBoxIsRead,
                editor: 'checkbox', align: 'center', sortable: false
            },
            {
                field: 'IsCreate', title: 'Is Created', halign: 'center', formatter: formatCheckBoxIsCreate,
                editor: 'checkbox', align: 'center', sortable: false
            },
            {
                field: 'IsUpdated', title: 'Is Updated', halign: 'center', formatter: formatCheckBoxIsUpdate,
                editor: 'checkbox', align: 'center', sortable: false
            },
            {
                field: 'IsDeleted', title: 'Is Deleted', halign: 'center', formatter: formatCheckBoxIsDelete,
                editor: 'checkbox', align: 'center', sortable: false
            }
        ]
    });


    window.pis.table({
        objTable: $table,
        urlSearch: '/master/RoleAccessMenuPage',
        urlPaging: '/master/RoleAccessMenuPageXt',
        autoLoad: true
    });

});


function expandTable($detail, row) {
    reportID = row.reportID;
    buildTable($detail.html('<table></table>').find('table'), reportID);
}

function buildTable($ext, reportID) {
    $ext.bootstrapTable({
        method: 'GET',
        //url: '/master/RoleAccessMenuPageXt',
        onClickRow: selectRow,
        columns: [
			{
			    field: 'SelectedMenu', title: 'Name Menu', halign: 'center', align: 'left', sortable: true
			},
            {
                field: 'IsRead', title: 'Is Read', halign: 'center', formatter: formatCheckBoxIsRead,
                editor: 'checkbox', align: 'center', sortable: false
            },
            {
                field: 'IsCreate', title: 'Is Created', halign: 'center', formatter: formatCheckBoxIsCreate,
                editor: 'checkbox', align: 'center', sortable: false
            },
            {
                field: 'IsUpdated', title: 'Is Updated', halign: 'center', formatter: formatCheckBoxIsUpdate,
                editor: 'checkbox', align: 'center', sortable: false
            },
            {
                field: 'IsDeleted', title: 'Is Deleted', halign: 'center', formatter: formatCheckBoxIsDelete,
                editor: 'checkbox', align: 'center', sortable: false
            }
        ]
    });
}
function formatCheckBoxIsRead(value, row) {
    var s;
    var i = 0;
    if (value) {
        s = '<div>' +
            '<label><input type="checkbox" id="IsRead" value="' + 1 + '" checked"></label>';
        return s;
    } else {
        s = '<div>' +
            '<label><input type="checkbox" id="IsRead" value="' + 0 + '" "></label></div>';
        return s;
    }
}

function formatCheckBoxIsCreate(value, row) {
    var s;
    var i = 0;
    if (value) {
        s = '<div>' +
                '<label><input class="chk" type="checkbox" id="IsCreate" value="' + 1 + '" checked onclick="Onclick(this, ' + row.ID + ');"></label>'
        '</div>';
        return s;
    } else {
        s = '<div>' +
                '<label><input class="chk" type="checkbox" id="IsCreate" value="' + 0 + '" onclick="Onclick(this, ' + row.ID + ');"></label>'
        '</div>';
        return s;
    }
}

function formatCheckBoxIsUpdate(value, row) {
    var s;
    var i = 0;
    if (value) {
        s = '<div >' +
                '<label><input  type="checkbox" id="IsUpdate" value="' + 1 + '" checked onclick="Onclick(this, ' + row.ID + ');"></label>'
        '</div>';
        return s;
    } else {
        s = '<div >' +
                '<label><input  type="checkbox" id="IsUpdate" value="' + 0 + '" onclick="Onclick(this, ' + row.ID + ');"></label>'
        '</div>';
        return s;
    }
}

function formatCheckBoxIsDelete(value, row) {
    var s;
    var i = 0;
    if (value) {
        s = '<div >' +
                '<label><input  type="checkbox" id="IsDelete" value="' + 1 + '" checked onclick="Onclick(this, ' + row.ID + ');"></label>'
        '</div>';
        return s;
    } else {
        s = '<div >' +
                '<label><input  type="checkbox" id="IsDelete" value="' + 0 + '" onclick="Onclick(this, ' + row.ID + ');"></label>'
        '</div>';
        return s;
    }
}

//function Onclick(e, ID) {
//    switch (e.id) {
//        case 'IsRead':
//            $('#tg').treegrid('update', {
//                id: ID,
//                row: {
//                    IsRead: !e.checked
//                }
//            });
//            break;
//        case 'IsCreated':
//            $('#tg').treegrid('update', {
//                id: ID,
//                row: {
//                    IsCreate: !e.checked
//                }
//            });
//            break;
//        case 'IsUpdated':
//            $('#tg').treegrid('update', {
//                id: ID,
//                row: {
//                    IsUpdate: !e.checked
//                }
//            });
//            break;
//        case 'IsDeleted':
//            $('#tg').treegrid('update', {
//                id: ID,
//                row: {
//                    IsDelete: !e.checked
//                }
//            });
//            break;
//    }
//    json = $('#tg').treegrid('getData');
//    $(".checkbox").removeClass("disabled");
//}

var editingId;
function edit(e) {
    //dataOld = $('#tg').treegrid('getData');
    EventEdit()
}

function save(e) {
    $('#tg').treegrid('loading');
    EventCancel();
    json = $('#tg').treegrid('getData');

    $.ajax({
        type: 'POST',
        url: '@Url.Action("UpdateGroupDeatil", "MasterGroupsDetail")',
        data: JSON.stringify(json),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        error: function (e) {
            if (e.statusText != "OK") {
                swal("Error", e.statusText, "error");
            } else {
                swal("Unauthorised", "Your don't have permission !, please contact your Administrator", "error");
            }
            $('#tg').treegrid('loaded');
        },
        success: function (result) {
            $('#tg').treegrid('loaded');
            if (typeof (result) == "string") {
                swal("Error", result, "error");
            } else {
                $('#tg').treegrid('loadData', result);
                swal("Saved!", "Your data has been Saved.", "success");
            }
        }
    });
}

function cancel(e) {
    //$('#tg').treegrid('loadData', dataOld);
    EventCancel();
}

function EventEdit() {
    $(".checkbox").removeClass("disabled");
    $("#cancel").removeClass("disabled");
    $("#edit").addClass("disabled");
    $("#btnSave").removeClass("disabled");
}

function EventCancel() {
    $("#btnSave").addClass("disabled");
    $("#edit").removeClass("disabled");
    $("#cancel").addClass("disabled");
    $(".checkbox").addClass("disabled");
}
