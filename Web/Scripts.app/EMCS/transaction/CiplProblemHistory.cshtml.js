﻿$tableProblemHistory = $("#tableCiplProblemHistory");

var columnProblemHistory = [
  {
    field: "",
    title: "No",
    halign: "center",
    align: "center",
    class: "text-nowrap",
    sortable: true,
    formatter: runningFormatterNoPaging,
  },
  {
    field: "Category",
    title: "Category",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
  },
  {
    field: "Case",
    title: "Case",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
  },
  {
    field: "Causes",
    title: "Causes",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
  },
  {
    field: "Impact",
    title: "Impact",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
  },
  {
    field: "Comment",
    title: "Note",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
  },
  {
    field: "CaseDate",
    title: "Case Date",
    halign: "center",
    align: "center",
    class: "text-nowrap",
    sortable: true,
    formatter: function (data, row, index) {
      return moment(data).format("DD MMM YYYY");
    },
  },
  {
    field: "PIC",
    title: "PIC",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
  },
];

$(function () {
  $tableProblemHistory.bootstrapTable({
    cache: false,
    url: "/Emcs/GetProbemList",
    pagination: true,
    search: false,
    data: {
      Id: $("#idCipl").val(),
      Cat: "CIPL",
    },
    striped: false,
    queryParams: function (params) {
      params.Id = $("#idCipl").val();
      params.Cat = "CIPL";
      return params;
    },
    clickToSelect: false,
    sidePagination: "server",
    reorderableColumns: true,
    toolbar: ".toolbar",
    toolbarAlign: "left",
    showColumns: true,
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
      return '<span class="noMatches">No data found</span>';
    },
    responseHandler: function (resp) {
      var data = {};
      $.map(resp.data, function (value, key) {
        data[value.Key] = value.Value;
      });
      // BUAT CIPL EDIT
      var url = window.location.href;
      if (url.includes("CiplEdit")) {
        if (data.total > 0) {
          $("#li-problem-cipl").removeClass("hide");
        }
      }

      // CIPL EDIT
      return data;
    },
    columns: columnProblemHistory,
  });
});
