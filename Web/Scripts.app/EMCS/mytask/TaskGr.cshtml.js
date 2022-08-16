$(function () {
<<<<<<< HEAD
  var columns_gr = [
    {
      //    field: "id",
      //    title: "No",
      //    halign: "center",
      //    align: 'center',
      //    formatter: runningFormatter
      //}, {
      field: "",
      title: "Action",
      align: "left",
      class: "text-nowrap",
      sortable: false,
      width: "150",
      events: events,
      formatter: function (data, row, index) {
        var btn = [];
        btn.push('<div class="flex gap-x-1.5 items-center">');
        btn.push(
          '<a href="/EMCS/EditGRForm/' +
            row.Id +
            '" class="btn-action btn-amber" title="Edit"><i class="uil uil-edit"></i></a>'
        );
        if (row.Status === "Waiting Approval") {
          btn.push(
            '<a href="/EMCS/ApprovalGR/' +
              row.Id +
              '" class="btn-action btn-green" title="Approve"><i class="uil uil-check-circle"></i></a>'
          );
        } else if (row.Status === "Revise") {
          btn.push(
            '<a href="/EMCS/EditGRForm/' +
              row.Id +
              '" class="btn-action btn-red edit" title="Revise"><i class="uil uil-edit"></i></a>'
          );
        } else {
          btn.push("-");
        }
        btn.push("</div>");
        return btn.join("");
      },
    },
    {
      field: "GrNo",
      title: "GR Number",
      halign: "center",
      sortable: true,
    },
    {
      field: "PicName",
      halign: "center",
      title: "Pic Name",
      sortable: true,
    },
    {
      field: "PhoneNumber",
      halign: "center",
      title: "Phone Number",
      sortable: true,
    },
    {
      field: "EstimationTimePickup",
      title: "ETP",
      visible: true,
      align: "center",
      halign: "center",
      sortable: true,
      formatter: function (data, row, index) {
        return moment(data).format("DD MMM YYYY");
      },
    },
    {
      field: "NopolNumber",
      title: "Police Number",
      halign: "center",
      sortable: true,
    },
    {
      field: "Status",
      title: "Status",
      halign: "center",
      sortable: true,
      filterControl: true,
    },
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
    toolbar: ".toolbar",
    toolbarAlign: "left",
    onClickRow: selectRow,
    sidePagination: "server",
    showColumns: true,
    showRefresh: true,
    smartDisplay: false,
    pageSize: "5",
    loadingFontSize: "16px",
    classes: "",
    icons: {
      paginationSwitchDown: "uil uil-arrow-circle-down",
      paginationSwitchUp: "uil uil-arrow-circle-up",
      refresh: "uil uil-sync",
      toggleOff: "uil uil-toggle-off",
      toggleOn: "uil uil-toggle-on",
      columns: "uil uil-columns",
      fullscreen: "uil uil-expand-arrows-alt",
      detailOpen: "uil uil-plus",
      detailClose: "uil uil-minus",
    },
    formatNoMatches: function () {
      return '<span class="noMatches">-</span>';
    },
  });
});
=======
    var columns_gr = [
        {
        //    field: "id",
        //    title: "No",
        //    halign: "center",
        //    align: 'center',
        //    formatter: runningFormatter
        //}, {
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
                btn.push('<a href="/EMCS/EditGRForm/' + row.Id + '" class="btn btn-xs btn-primary"><i class="fa fa-pencil" alt="Edit GR"></i></a>');
                if (row.Status === "Waiting Approval") {
                    btn.push('<a href="/EMCS/ApprovalGR/' + row.Id + '" class="btn btn-success btn-link btn-sm"><i class="tim-icons icon-zoom-split" alt="Show GR"></i></a>');
                } else if (row.Status === "Revise") {
                    btn.push('<a href="/EMCS/EditGRForm/' + row.Id + '" class="btn btn-primary btn-link btn-sm"><i class="tim-icons icon-pencil" alt="Edit GR"></i></a>');
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
>>>>>>> 639d8d0 (Intial commit)
