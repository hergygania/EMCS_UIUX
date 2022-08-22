function ApproveDelegation(obj) {
  $.ajax({
    url: "/EMCS/BlAwbApproval",
    type: "POST",
    data: {
      Id: obj.Id,
      Status: obj.Status,
      Notes: obj.Notes,
    },
    success: function (resp) {
      console.log(resp);
      //location.href = "/EMCS/PreviewGR/" + IdPrev;
    },
  });
}

window.operateDelegationEvents = {
  "click .btn-revise-bl": function (e, data, row) {
    location.href = "/EMCS/BlAwbRevise?Id=" + row.Id;
  },
  "click .btn-view-bl": function (e, data, row) {
    location.href = "/EMCS/BlAwbView?Id=" + row.Id;
  },
};

var columns_delegation = [
  {
    //    field: "id",
    //    title: "No",
    //    align: 'center',
    //    formatter: runningFormatter
    //}, {
    field: "",
    title: "Action",
    align: "center",
    sortable: false,
    width: "160",
    events: window.operateEvents,
    formatter: function (data, row) {
      var btn = [];
      btn.push('<div class="flex gap-x-1.5 items-center">');
      if (
        row["StatusViewByUser"] === "Waiting for BL or AWB" ||
        row["StatusViewByUser"] === "Need revision review by imex"
      ) {
        btn.push(
          "<button class='btn-action btn-amber btn-create-bl' title='Revise'><i class='uil uil-history'></i></button>"
        );
      } else if (row["StatusViewByUser"] === "Waiting for BL or AWB approval") {
        btn.push(
          "<button class='btn-action btn-green btn-approval-bl' data-toggle='tooltip' title='Approve'><i class='uil uil-check-circle'></i></button>"
        );
        btn.push(
          "<button class='btn-action btn-blue btn-view-bl' data-toggle='tooltip' data-placement='top' title='View'><i class='uil uil-search'></i></button>"
        );
      } else {
        btn.push(
          "<button class='btn-action btn-blue btn-view-bl' data-toggle='tooltip' data-placement='top' title='View'><i class='uil uil-search'></i></button>"
        );
      }

      btn.push("</div>");
      return btn.join(" ");
    },
  },
  {
    field: "FlowType",
    title: "Flow Type",
    sortable: true,
  },
  {
    field: "ReqNo",
    title: "Request Number",
    sortable: true,
  },
  {
    field: "ReqDate",
    title: "Request Date",
    sortable: true,
  },
];

$("#tbl-task-delegation").bootstrapTable({
  url: "/EMCS/GetTaskBlData",
  columns: columns_delegation,
  cache: false,
  pagination: true,
  search: false,
  striped: true,
  clickToSelect: true,
  reorderableColumns: true,
  toolbar: ".toolbar",
  toolbarAlign: "left",
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
