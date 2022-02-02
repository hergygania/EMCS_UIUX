$(function () {
    var columns_gr = [
        {
            field: "id",
            title: "No",
            halign: "center",
            align: 'center',
            formatter: runningFormatter
        }, {
            field: "",
            title: "Action",
            align: "center",
            class: "text-nowrap",
            sortable: true,
            width: "150",
            events: events,
            formatter: function (data, row, index) {
                var btn = [];
                btn.push('<div class="btn-toolbar row">');
                if (row.Status === "Waiting Approval") {
                    btn.push('<a href="/EMCS/ApprovalGR/' + row.Id + '" class="btn btn-xs btn-default"><i class="fa fa-search" alt="Show GR"></i></a>');
                } else if (row.Status === "Revise") {
                    btn.push('<a href="/EMCS/EditGRForm/' + row.Id + '" class="btn btn-xs btn-primary"><i class="fa fa-pencil" alt="Edit GR"></i></a>');
                } else {
                    btn.push("-");
                }
                btn.push('</div>');
                return btn.join('');
            }
        }, {
            field: "GrNo",
            title: "GR Number",
            halign: "center",
            sortable: true
        }, {
            field: "PicName",
            halign: "center",
            title: "Pic Name",
            sortable: true
        }, {
            field: "PhoneNumber",
            halign: "center",
            title: "Phone Number",
            sortable: true
        }, {
            field: "EstimationTimePickup",
            title: "ETP",
            visible: true,
            align: "center",
            halign: "center",
            sortable: true,
            formatter: function (data, row, index) {
                return moment(data).format("DD MMM YYYY");
            }
        }, {
            field: "NopolNumber",
            title: "Police Number",
            halign: "center",
            sortable: true
        }, {
            field: "Status",
            title: "Status",
            halign: "center",
            sortable: true,
            filterControl: true
        }
    ];

    $("#tbl-task-gr").bootstrapTable({
        url: "/EMCS/GetTaskGrData",
        columns: columns_gr,
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
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        }
    });
});