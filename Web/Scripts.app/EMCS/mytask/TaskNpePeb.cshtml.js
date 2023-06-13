$(function () {
  window.operateEvents = {
    "click .btn-create-npe": function (e, data, row, index) {
      location.href = "/EMCS/CreatePebNpe?Id=" + parseInt(row.IdCl);
    },
    "click .btn-approve-npe": function (e, data, row, index) {
      location.href = "/EMCS/PebNpeApproval?Id=" + parseInt(row.IdCl);
    },
    "click .btn-revise-npe": function (e, data, row, index) {
      location.href = "/EMCS/RevisePebNpe?Id=" + parseInt(row.IdCl);
    },
    "click .btn-view-npe": function (e, data, row, index) {
      location.href = "/EMCS/ViewPebNpe?Id=" + parseInt(row.IdCl);
    },
  };

  var columns_peb = [
    {
      //    field: "id",
      //    title: "No",
      //    formatter: runningFormatter,
      //    align: 'center'
      //},{
      field: "IdCl",
      title: "IdCl",
      visible: false,
      align: "center",
    },
    {
      field: "",
      title: "Action",
      align: "left",
      sortable: false,
      class: "text-nowrap",
      width: "160",
      events: window.operateEvents,
      formatter: function (data, row, index) {
        var btn = [];
        btn.push('<div class="flex gap-x-1.5 items-center">');
        if (
          row.StatusViewByUser === "Waiting NPE & PEB approval" ||
          row.StatusViewByUser === "Waiting approval draft PEB" ||
          row.StatusViewByUser === "Waiting approval NPE"
        ) {
          btn.push(
            "<button class='btn-action btn-green btn-approve-npe' data-toggle='tooltip' data-placement='top' title='Approve & Reject Npe Peb'><i class='uil uil-check-circle'></i></button>"
          );
          btn.push(
            "<button class='btn-action btn-blue btn-view-npe' data-toggle='tooltip' data-placement='top' title='View'><i class='uil uil-search'></i></button>"
          );
          //btn.push("<button class='btn btn-xs btn-warning btn-revise-npe' data-toggle='tooltip' data-placement='top' title='Revise Npe Peb'><i class='fa fa-pencil'></i></button>")
        } else if (
          row.StatusViewByUser === "Waiting for NPE & PEB" ||
          row.StatusViewByUser === "Need revision review by imex" ||
          row.StatusViewByUser === "Waiting NPE document"
        ) {
          btn.push(
            "<button class='btn-action btn-amber btn-create-npe' data-toggle='tooltip' data-placement='top' title='Create Npe Peb'><i class='uil uil-plus-circle'></i></button>"
          );
        } else {
          btn.push(
            "<button class='btn-actin btn-blue btn-view-npe' data-toggle='tooltip' data-placement='top' title='View'><i class='uil uil-search'></i></button>"
          );
        }

        btn.push("</div>");
        return btn.join(" ");
      },
    },
    {
      field: "SlNo",
      title: "SI Number",
      sortable: true,
    },
    {
      field: "ClNo",
      title: "Cargo Number",
      class: "text-nowrap",
      halign: "center",
      sortable: true,
    },
    {
      field: "CreateDate",
      title: "Date",
      align: "center",
      halign: "center",
      sortable: true,
      class: "text-nowrap",
      formatter: function (data, row, index) {
        if (data) {
          return moment(data).format("DD MMM YYYY");
        } else {
          return "-";
        }
      },
    },
    {
      field: "PreparedBy",
      halign: "center",
      title: "Prepared By",
      class: "text-nowrap",
      sortable: true,
    },
    {
      field: "Consignee",
      align: "left",
      title: "Consignee Name",
      halign: "center",
      class: "text-nowrap",
      sortable: true,
    },
    {
      field: "StuffingDateStarted",
      title: "Stuffing Date (Start)",
      halign: "center",
      class: "text-nowrap",
      sortable: true,
      align: "center",
      formatter: function (data, row, index) {
        if (data) {
          return moment(data).format("DD MMM YYYY");
        } else {
          return "-";
        }
      },
    },
    {
      field: "StuffingDateFinished",
      title: "Stuffing Date (Finished)",
      halign: "center",
      class: "text-nowrap",
      align: "center",
      sortable: true,
      formatter: function (data, row, index) {
        if (data) {
          return moment(data).format("DD MMM YYYY");
        } else {
          return "-";
        }
      },
    },
    {
      field: "Etd",
      title: "ETD",
      class: "text-nowrap",
      align: "center",
      halign: "center",
      sortable: true,
      filterControl: true,
      formatter: function (data, row, index) {
        if (data) {
          return moment(data).format("DD MMM YYYY");
        } else {
          return "-";
        }
      },
    },
    {
      field: "Eta",
      title: "ETA",
      class: "text-nowrap",
      align: "center",
      halign: "center",
      sortable: true,
      filterControl: true,
      formatter: function (data, row, index) {
        if (data) {
          return moment(data).format("DD MMM YYYY");
        } else {
          return "-";
        }
      },
    },
    {
      field: "StatusViewByUser",
      title: "Status",
      class: "text-nowrap",
      align: "center",
      halign: "center",
      sortable: true,
      filterControl: true,
    },
  ];

  $("#tbl-task-peb").bootstrapTable({
    url: "/EMCS/GetTaskNpePebData",
    columns: columns_peb,
    cache: false,
    pagination: true,
    search: false,
    striped: false,
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
      return '<span class="noMatches">No data found.</span>';
    },
  });
});
