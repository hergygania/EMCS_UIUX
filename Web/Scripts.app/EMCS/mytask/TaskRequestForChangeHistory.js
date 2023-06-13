$tableChangeHistory = $("#tbl-task-rfc");

var columnChangeHistory = [
  {
    field: "",
    title: "Action",
    align: "left",
    class: "text-nowrap",
    sortable: true,
    width: "150",
    formatter: function (data, row) {
      console.log(row);
      const div1 = '<div class="flex gap-x-1.5 items-center">';

      var btnApprove = `<a href='/EMCS/RequestForChangeDetail?formtype=${row.FormType}&id=${row.Id}&formid=${row.FormId}' class='btn-action btn-amber'><i class='uil uil-search'></i></a>`;
      const div2 = "</div>";
      const btn = [div1, btnApprove, div2];
      return btn.join(" ");
    },
  },
  {
    field: "FormType",
    title: "Request for change on",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
  },
  {
    field: "RFCNumber",
    title: "RFCNumber",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
  },
  {
    field: "FormNo",
    title: "FormNo",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
  },

  {
    field: "CreateDate",
    title: "Date",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
    formatter: function (data, row, index) {
      return moment(data).format("DD MMM YYYY hh:mm:ss");
    },
  },
  {
    field: "Status",
    title: "Status",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
    formatter: function (data, row, index) {
      if (data == 1) return "Approved";
      else if (data == 2) return "Rejected";
      else return "Waiting for approval";
    },
  },
  //{
  //    field: 'BeforeValue',
  //    title: 'Old Value',
  //    halign: 'center',
  //    align: 'left',
  //    class: 'text-nowrap',
  //    sortable: true
  //},
  //{
  //    field: 'AfterValue',
  //    title: 'New Value',
  //    halign: 'center',
  //    align: 'left',
  //    class: 'text-nowrap',
  //    sortable: true
  //},
];
$("#btnApproveChangeHistory").on("click", function () {
  paramSearch = {
    idTerm: $("#idCipl").val(),
    formId: $("#formId").val(),
    formtype: "CIPL",
  };
  $.ajax({
    url: "/EMCS/ApproveChangeHistory",
    type: "POST",
    data: paramSearch,
    success: function (guid) {},
    cache: false,
  });
});

$(function () {
  $tableChangeHistory.bootstrapTable({
    cache: false,
    url: "/Emcs/GetRequestForChangeList",
    pagination: true,
    search: false,
    striped: false,
    clickToSelect: false,
    sidePagination: "server",
    showColumns: false,
    queryParams: function (params) {
      return {
        limit: params.limit,
        offset: params.offset,
        sort: params.sort,
        order: params.order,
      };
    },
    searchOnEnterKey: true,
    showRefresh: true,
    smartDisplay: false,
    pageSize: "10",
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
      return '<span class="noMatches">Data Not Found</span>';
    },
    responseHandler: function (resp) {
      var data = {};
      $.map(resp, function (value, key) {
        data[value.Key] = value.Value;
      });
      var url = window.location.href;

      return data;
    },
    columns: columnChangeHistory,
  });
});
