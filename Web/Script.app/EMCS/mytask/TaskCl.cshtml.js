$(function () {
    var columnsCargo = [
        {
            field: "Id",
            title: "No",
            align: 'center',
            formatter: runningFormatter
        }, {
            field: "IdCl",
            title: "IdCl",
            visible: false,
            align: 'center'
        }, {
            field: "Id",
            title: "Action",
            align: "center",
            sortable: true,
            width: "170",
            events: events,
            formatter: function (data, row) {
                console.log(row);
                var btn = [];
                btn.push('<div class="btn-group">');
                btn.push('<div class="btn-group">');
                 
                if (row.Status === "Revise") {
                    btn.push('<a href="/EMCS/UpdateCargo?CargoID=' + row.IdCl + '" class="btn btn-info btn-xs"><i class="fa fa-edit" alt="Show GR"></i></a>');
                } else {
                    if (Boolean($("#IsEditAllowed").val())) {
                        btn.push('<a href="/EMCS/UpdateCargo?CargoID=' + row.IdCl + '&rfc=false" class="btn btn-xs btn-primary"><i class="fa fa-edit" alt="Show GR"></i></a>');
                    }
                    btn.push('<a href="/EMCS/CargoApproval/?Id=' + parseInt(row.IdCl) + '" class="btn btn-info btn-xs"><i class="fa fa-search" alt="Show GR"></i></a>');
                }
                
                btn.push('</div>');
                btn.push('</div>');
                return btn.join('');
            }
        }, {
            field: "ClNo",
            title: "Cargo Number",
            class: "text-nowrap",
            halign: "center",
            sortable: true
        }, {
            field: "CreateDate",
            title: "Date",
            align: "center",
            halign: "center",
            sortable: true,
            class: "text-nowrap",
            formatter: function (data) {
                if (data) {
                    return moment(data).format("DD MMM YYYY");
                } else {
                    return "-";
                }
            }
        }, {
            field: "PreparedBy",
            halign: "center",
            title: "Prepared By",
            class: "text-nowrap",
            sortable: true
        }, {
            field: "Consignee",
            align: "left",
            title: "Consignee Name",
            halign: "center",
            class: "text-nowrap",
            sortable: true
        }, {
            field: "StuffingDateStarted",
            title: "Stuffing Date (Start)",
            halign: "center",
            class: "text-nowrap",
            sortable: true,
            align: "center",
            formatter: function (data) {
                if (data) {
                    return moment(data).format("DD MMM YYYY");
                } else {
                    return "-";
                }
            }
        }, {
            field: "StuffingDateFinished",
            title: "Stuffing Date (Finished)",
            halign: "center",
            class: "text-nowrap",
            align: "center",
            sortable: true,
            formatter: function (data) {
                if (data) {
                    return moment(data).format("DD MMM YYYY");
                } else {
                    return "-";
                }
            }
        }, {
            field: "SailingSchedule",
            title: "ETD",
            class: "text-nowrap",
            align: "center",
            halign: "center",
            sortable: true,
            filterControl: true,
            formatter: function (data) {
                if (data) {
                    return moment(data).format("DD MMM YYYY");
                } else {
                    return "-";
                }
            }
        }, {
            field: "ArrivalDestination",
            title: "ETA",
            class: "text-nowrap",
            align: "center",
            halign: "center",
            sortable: true,
            filterControl: true,
            formatter: function (data) {
                if (data) {
                    return moment(data).format("DD MMM YYYY");
                } else {
                    return "-";
                }
            }
        }, {
            field: "StatusViewByUser",
            title: "Status",
            class: "text-nowrap",
            align: "center",
            halign: "center",
            sortable: true,
            filterControl: true
        }
    ];

    $("#tbl-task-cargo").bootstrapTable({
        url: "/EMCS/GetTaskClData",
        columns: columnsCargo,
        cache: false,
        pagination: true,
        search: false,
        striped: false,
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
        formatNoMatches: function () {
            return '<span class="noMatches">No task available</span>';
        }
    });
})