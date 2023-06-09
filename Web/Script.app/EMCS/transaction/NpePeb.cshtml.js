var $table = $('#tableNpePeb');
var $AllowDelete = $('#AllowDelete').val();
var $AllowUpdate = $('#AllowUpdate').val();


$(function () {
    var columns = [
        {
            field: "Id",
            title: "Action",
            align: "center",
            class: "text-nowrap",
            sortable: true,
            width: "125",
            formatter: function (data, row, index) {
                return operateFormatter({ Edit: Boolean($AllowUpdate), Delete: Boolean($AllowDelete), Data: row });
            },
            events: operateEvents
        },
        {
            field: "RegistrationNumber",
            title: "No Pen.",
            sortable: true
        },
        {
            field: "AjuNumber",
            title: "Aju Number.",
            sortable: true,

        },
        {
            field: "ClNo",
            title: "CL No.",
            sortable: true,

        },
        {
            field: "AjuDate",
            title: "AjuDate",
            sortable: true,
            formatter: function (data, row, index) {
                if (data) {
                    if (data) {
                        return moment(data).format("DD MMM YYYY");
                    } else {
                        return "-";
                    }
                } else {
                    return "-";
                }
            }
        },
        {
            field: "NpeNumber",
            title: "NpeNumber",
            sortable: true
        },
        {
            field: "NpeDate",
            title: "NpeDate.",
            sortable: true,
            formatter: function (data, row, index) {
                if (data) {
                    if (data) {
                        return moment(data).format("DD MMM YYYY");
                    } else {
                        return "-";
                    }
                } else {
                    return "-";
                }
            }
        },
        //{
        //    field: "PebNumber",
        //    title: "PebNumber.",
        //    sortable: true
        //},
        {
            field: "PassPabeanOffice",
            title: "PassPabeanOffice.",
            sortable: true
        },
        {
            field: "Valuta",
            title: "Type Of Currency(Valuta).",
            sortable: true
        },
        {
            field: "StatusViewByUser",
            title: "Status",
            sortable: true
        }
    ]

    $table.bootstrapTable(
        {
            url: "/EMCS/GetNpePebList",
            columns: columns,
            cache: false,
            pagination: true,
            search: false,
            striped: true,
            clickToSelect: true,
            reorderableColumns: true,
            toolbar: '.toolbar',
            toolbarAlign: 'left',
            onClickRow: selectRow,
            sidePagination: 'server',
            showColumns: true,
            showRefresh: true,
            smartDisplay: false,
            pageSize: '5',
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
            },
            formatNoMatches: function () {
                return '<span class="noMatches">No Data Found</span>';
            },

        });

    $("#mySearch").insertBefore($("[name=refresh]"));

});


function operateFormatter(options) {
    
    var btn = [];
    btn.push('<div class="btn-group">');
    if (options.Data.StatusViewByUser != null && options.Data.StatusViewByUser != '') {
        if (options.Edit == true && options.Data.PendingRFC == 0 && (options.Data.StatusViewByUser.replace(/\s/g, '') == "WaitingforNPE&PEB" || options.Data.StatusViewByUser.replace(/\s/g, '') == "WaitingNPE&PEBapproval" || options.Data.StatusViewByUser.replace(/\s/g, '') == "Needrevisionreviewbyimex" || options.Data.StatusViewByUser.replace(/\s/g, '') == "WaitingapprovaldraftPEB"
            || options.Data.StatusViewByUser.replace(/\s/g, '') == "Finish" || options.Data.StatusViewByUser.replace(/\s/g, '') == "WaitingforBLorAWB" || options.Data.StatusViewByUser.replace(/\s/g, '') == "BLorAWBneedrevision"))
            btn.push('<a type="button" class="btn btn-xs btn-primary edit" title="Edit"><i class="fa fa-edit"></i></a>');
    }
    btn.push('<a type="button"  class="btn btn-xs btn-info info" title="Info"><i class="fa fa-search""></i></a>')
    btn.push('</div>');

    return btn.join('');
}

operateFormatter.DEFAULTS = {
    Add: false,
    Edit: true,
    Delete: false,
    Info: false,
    View: false,
    History: false
}

window.operateEvents = {
    'click .info': function (e, data, row, index) {
        
        //var page = "cargo";
        //location.href = "/EMCS/ShowNpePeb?page=" + page + "&Id=" + row.IdCl;
        location.href = "/EMCS/ViewPebNpe?Id=" + parseInt(row.IdCl) + "&IdNpePeb=" + row.Id;
    },
    'click .edit': function (e, value, row, index) {
        location.href = "/EMCS/CreatePebNpe?Id=" + row.IdCl+"&rfc=true";
    },
};

