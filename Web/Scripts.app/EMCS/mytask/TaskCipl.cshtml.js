$(function () {
  const columnsCipl = [
    {
      //    field: "id",
      //    title: "No",
      //    halign: "center",
      //    align: "center",
      //    formatter: runningFormatter
      //}, {
      field: "",
      title: "Action",
      align: "left",
      class: "text-nowrap",
      sortable: false,
      width: "150",
      events: events,
      formatter: function (data, row) {
        console.log(row);
        const div1 = "<div class='flex gap-x-1.5 items-center'>";
        const id = row.IdCipl;
        var btnApprove =
          row.Status === "Approve" || row.Status.toLowerCase() === "submit"
            ? `<a href='/EMCS/CiplApprove/${row.IdCipl}' class='btn-action btn-green info' title="Approve"><i class="uil uil-check-circle"></i></a>`
            : "";
        var btnEdit = "";
        if (Boolean($("#IsEditAllowed").val())) {
          btnEdit = `<a href='/EMCS/CiplEdit?id=${row.IdCipl}&rfc=false' class='btn-action btn-amber' title="Edit"><i class="uil uil-edit"></i></a>`;
        }
        var btnRevise;
        if (row.Status === "Revise") {
          if (row.IdNextStep === 30066) {
            btnRevise = `<a href='/EMCS/CiplApprove/${row.IdCipl} ' class='btn-action btn-green' title="Approve"><i class="uil uil-check-circle"></i></a>`;
          } else {
            btnRevise =
              row.Status === "Revise"
                ? `<a href='/EMCS/CiplEdit/${row.IdCipl}' class='btn-action btn-red edit' title="Revise"><i class="uil uil-history"></i></a>`
                : "";
          }
        }

        if (
          (row.IdStep === 10033 ||
            row.IdStep === 10032 ||
            row.IdStep === 10035) &&
          row.Status === "Submit"
        ) {
          btnApprove = `<a href='/EMCS/CiplApproveReviseDimension/?id=${id}' class='btn-action btn-green' title="Approve"><i class="uil uil-check-circle"></i></a>`;
        }

        const div2 = "</div>";
        const btn = [div1, btnEdit, btnApprove, btnRevise, div2];
        return btn.join(" ");
      },
    },
    {
      field: "CreateDate",
      title: "Create Date",
      halign: "center",
      align: "center",
      sortable: true,
      formatter: function (data) {
        return moment(data).format("DD MMM YYYY");
      },
    },
    {
      field: "CiplNo",
      title: "Cipl Number",
      halign: "center",
      sortable: true,
    },
    {
      field: "Category",
      title: "Category",
      halign: "center",
      sortable: true,
    },
    {
      field: "ETA",
      title: "ETA",
      visible: false,
      halign: "center",
      sortable: true,
    },
    {
      field: "ShippingMethod",
      halign: "center",
      align: "center",
      title: "Shipment method",
      sortable: true,
    },
    {
      field: "Forwader",
      title: "Forwarder",
      align: "center",
      halign: "center",
      sortable: true,
    },
    {
      field: "Loading",
      title: "Loading Port",
      halign: "center",
      visible: false,
      sortable: true,
    },
    {
      field: "Destination",
      title: "Destination Port",
      halign: "center",
      visible: false,
      sortable: true,
    },
    {
      field: "NextStatusViewByUser",
      title: "Status",
      halign: "center",
      align: "center",
      sortable: true,
      filterControl: true,
    },
  ];

  $("#tbl-task-cipl").bootstrapTable({
    url: "/EMCS/GetTaskCiplData",
    columns: columnsCipl,
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
      return '<span class="noMatches">No task available</span>';
    },
  });
});
