$(document).ready(function () {
<<<<<<< HEAD
  //function getDataSummary() {
  //    $.ajax({
  //        type: 'GET',
  //        url: "~/Json/ExportTransaction.json",
  //        dataType: JSON
  //    })
  //    .done(function (data, textStatus, jqXHR) {
  //            getHighChart();
  //    })
  //}

  $("#TrendExportButton").on("click", function (e) {
    $("#TrendExportFilter").toggle("show");
  });
  $("#TrendExportFilter").on("click", function (e) {
    e.stopPropagation();
  });

  $("#btn-big-5-commodities").on("click", function (e) {
    $("#ul-big-5-commodities").toggle("show");
  });
  $("#ul-big-5-commodities").on("click", function (e) {
    e.stopPropagation();
  });

  $("#ExportByCategoryButton").on("click", function (e) {
    $("#ExportByCategoryFilter").toggle("show");
  });
  $("#ExportByCategoryFilter").on("click", function (e) {
    e.stopPropagation();
  });

  $("#SalesVSNonSalesButton").on("click", function (e) {
    $("#SalesVSNonSalesFilter").toggle("show");
  });
  $("#SalesVSNonSalesFilter").on("click", function (e) {
    e.stopPropagation();
  });

  $("#TotalExportButton").on("click", function (e) {
    $("#TotalExportFilter").toggle("show");
  });
  $("#TotalExportFilter").on("click", function (e) {
    e.stopPropagation();
  });

  var intervals = [
    { id: 2, text: "2 Years" },
    { id: 3, text: "3 Years" },
    { id: 4, text: "4 Years" },
    { id: 5, text: "5 Years" },
    { id: 6, text: "6 Years" },
    { id: 7, text: "7 Years" },
    { id: 8, text: "8 Years" },
    { id: 9, text: "9 Years" },
    { id: 10, text: "10 Years" },
  ];

  $("#TrendExportYearInterval")
    .select2({
      data: intervals,
      width: "100%",
    })
    .on("select2:select", function () {
      var endYear = (parseInt($("#TrendExportStartYear").val()) + parseInt($(this).val())).toString();

      $("#TrendExportEndYear").val(endYear);
      $("#ExportByCategoryEndYear").val(endYear);

      $("#ExportByCategoryYearInterval").val($(this).val()).trigger("change");
    });
  $("#TrendExportYearInterval").val(intervals[0].id).trigger("change");
  $("#TrendExportStartYear").val(new Date().getFullYear() - 2);
  $("#TrendExportEndYear").val(new Date().getFullYear());

  $("#ExportByCategoryYearInterval")
    .select2({
      data: intervals,
      width: "100%",
    })
    .on("select2:select", function () {
      var endYear = (parseInt($("#ExportByCategoryStartYear").val()) + parseInt($(this).val())).toString();

      $("#TrendExportEndYear").val(endYear);
      $("#ExportByCategoryEndYear").val(endYear);

      $("#TrendExportYearInterval").val($(this).val()).trigger("change");
    });
  $("#ExportByCategoryYearInterval").val(intervals[0].id).trigger("change");
  $("#ExportByCategoryStartYear").val(new Date().getFullYear() - 2);
  $("#ExportByCategoryEndYear").val(new Date().getFullYear());

  $("#TrendExportStartYear")
    .datepicker({
      format: "yyyy",
      viewMode: "years",
      minViewMode: "years",
    })
    .on("changeYear", function (e) {
      $(this).datepicker("hide");

      $("#ExportByCategoryStartYear").val(e.date.getFullYear());
      if ($("#TrendExportYearInterval").val() !== null && $("#TrendExportYearInterval").val() !== "") {
        var endYear = (parseInt(e.date.getFullYear()) + parseInt($("#TrendExportYearInterval").val())).toString();
        $("#TrendExportEndYear").val(endYear);
        $("#ExportByCategoryEndYear").val(endYear);
      }
    });

  $("#ExportByCategoryStartYear")
    .datepicker({
      format: "yyyy",
      viewMode: "years",
      minViewMode: "years",
    })
    .on("changeYear", function (e) {
      $(this).datepicker("hide");

      $("#TrendExportStartYear").val(e.date.getFullYear());
      if ($("#ExportByCategoryYearInterval").val() !== null && $("#ExportByCategoryYearInterval").val() !== "") {
        var endYear = (parseInt(e.date.getFullYear()) + parseInt($("#ExportByCategoryYearInterval").val())).toString();
        $("#TrendExportEndYear").val(endYear);
        $("#ExportByCategoryEndYear").val(endYear);
      }
    });

  $("#SalesVSNonSalesStartYear")
    .datepicker({
      format: "yyyy",
      viewMode: "years",
      minViewMode: "years",
    })
    .on("changeYear", function (e) {
      $(this).datepicker("hide");
      $("#SalesVSNonSalesEndYear").datepicker("setStartDate", e.date.getFullYear().toString());
    });
  $("#SalesVSNonSalesStartYear").val(new Date().getFullYear());

  $("#SalesVSNonSalesEndYear")
    .datepicker({
      format: "yyyy",
      viewMode: "years",
      minViewMode: "years",
    })
    .on("changeYear", function (e) {
      $(this).datepicker("hide");
      $("#SalesVSNonSalesStartYear").datepicker("setEndDate", e.date.getFullYear().toString());
    });
  $("#SalesVSNonSalesEndYear").val(new Date().getFullYear());

  $("#SalesVSNonSalesEndYear").datepicker("setStartDate", $("#SalesVSNonSalesStartYear").val());
  $("#SalesVSNonSalesStartYear").datepicker("setEndDate", $("#SalesVSNonSalesEndYear").val());

  $("#TotalExportYear")
    .datepicker({
      format: "yyyy",
      viewMode: "years",
      minViewMode: "years",
    })
    .on("changeYear", function (e) {
      $(this).datepicker("hide");
    });
  $("#TotalExportYear").val(new Date().getFullYear());

  $(".year").datepicker({
    format: "yyyy",
    viewMode: "years",
    minViewMode: "years",
  });

  getHighChart();
});

function TrendExportSearch() {
  var startYear = $("#TrendExportStartYear").val();
  var endYear = $("#TrendExportEndYear").val();
  if (startYear !== null && startYear !== "" && endYear !== null && endYear !== "") {
    getTrendExport();
    getExportByCategory(startYear, endYear);
  }
}

function TrendExportDownload() {
  var startYear = $("#TrendExportStartYear").val();
  var endYear = $("#TrendExportEndYear").val();
  window.open("/EMCS/DownloadTrendExport?startYear=" + startYear + "&endYear=" + endYear, "_blank");
}

function ExportByCategorySearch() {
  var startYear = $("#ExportByCategoryStartYear").val();
  var endYear = $("#ExportByCategoryEndYear").val();
  if (startYear !== null && startYear !== "" && endYear !== null && endYear !== "") {
    getExportByCategory(startYear, endYear);
  }
}

function ExportByCategoryDownload() {
  var startYear = $("#ExportByCategoryStartYear").val();
  var endYear = $("#ExportByCategoryEndYear").val();
  window.open("/EMCS/DownloadExportByCategory?startYear=" + startYear + "&endYear=" + endYear, "_blank");
}

function SalesVSNonSalesSearch() {
  var startYear = $("#SalesVSNonSalesStartYear").val();
  var endYear = $("#SalesVSNonSalesEndYear").val();
  if (startYear !== null && startYear !== "" && endYear !== null && endYear !== "") {
    getSalesVSNonSales();
  }
}

function SalesVSNonSalesDownload() {
  var startYear = $("#SalesVSNonSalesStartYear").val();
  var endYear = $("#SalesVSNonSalesEndYear").val();
  window.open("/EMCS/DownloadSalesVSNonSales?startYear=" + startYear + "&endYear=" + endYear, "_blank");
}

function TotalExportSearch() {
  var year = $("#TotalExportYear").val();
  if (year !== null && year !== "") {
    getTotalExport();
  }
}

function TotalExportDownload() {
  var year = $("#TotalExportYear").val();
  window.open("/EMCS/DownloadTotalExport?year=" + year, "_blank");
}

$("#form-export-today").on("submit", function (e) {
  e.preventDefault();
  BigesCommoditiesCategory();
});
$("#btn-big-5-commodities-download").on("click", function () {
  var year = $("#date1-export-type").val();
  var exporttype = $("#slc-export-type").val();
  window.open("/EMCS/DownloadBig5Commodities?searchId=" + year + "&searchName=" + exporttype, "_blank");
});

function GetDataSummary() {
  //var newData = {
  //    "Perioddatefrom": dtfrom,
  //    "Perioddateto": dtto,
  //    "BranchCC100": BranchCC100
  //}

  //$.ajax({
  //    //type: 'GET',
  //    //url: "~/Json/ExportTransaction.json",
  //    dataType: JSON,
  //    data: newData
  //})
  //.done(function (data, textStatus, jqXHR) {
  //   getHighChart();
  //})
  //.fail(function (jqXHR, textStatus, errorThrown) {
  //    var result = $.parseJSON(jqXHR.responseText);
  //    sAlert(result.Message,
  //        result.ExceptionMessage + "&#13;&#10;" +
  //        result.ExceptionType + "&#13;&#10;" +
  //        result.StackTrace, "error");
  //});
  getHighChart();
}

function getHighChart() {
  getTrendExport();
  getExportByCategory($("#TrendExportStartYear").val(), $("#TrendExportEndYear").val());
  getSalesVSNonSales();
  getTotalExport();
  BigesCommoditiesCategory();

  //Highcharts.chart('container_bigest', {
  //    chart: {
  //        type: 'column'
  //    },
  //    colors: ['#ffca22', '#9dd45d', '#05beff', '#ff696a', '#c63bff'],
  //    title: {
  //        text: '5 Bigest Commodities Category'
  //    },
  //    subtitle: {
  //        text: '2019'
  //    },
  //    credits: {
  //        enabled: false
  //    },
  //    xAxis: {
  //        type: 'category'
  //    },
  //    yAxis: {
  //        title: {
  //            text: 'Total percent commodities'
  //        }

  //    },
  //    legend: {
  //        enabled: false
  //    },
  //    credits: {
  //        enabled: false
  //    },
  //    plotOptions: {
  //        series: {
  //            borderWidth: 0,
  //            dataLabels: {
  //                enabled: true,
  //                format: '{point.y:.1f}%'
  //            }
  //        }
  //    },

  //    tooltip: {
  //        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
  //        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
  //    },

  //    series: [
  //        {
  //            name: "Browsers",
  //            colorByPoint: true,
  //            data: [
  //                {
  //                    name: "Parts",
  //                    y: 62.74
  //                },
  //                {
  //                    name: "Machine",
  //                    y: 10.57
  //                },
  //                {
  //                    name: "Engine",
  //                    y: 7.23
  //                },
  //                {
  //                    name: "Forklift",
  //                    y: 5.58
  //                },
  //                {
  //                    name: "Miscellenious",
  //                    y: 4.02
  //                }
  //            ]
  //        }
  //    ],

  //});
}

function getTrendExport() {
  $.ajax({
    //url: "getTrendExport?startYear=" + parseInt($("#TrendExportStartYear").val()) + "&endYear=" + parseInt($("#TrendExportEndYear").val()),
    url: "getTrendExport?startYear=2020&endYear=2022",
    success: function (data) {
      console.log(data);
      var categories_yearly = [];
      var data_totExport = [];
      var data_totalPeb = [];

      $.each(data, function (i, data) {
        categories_yearly.push(data.Year);
        data_totExport.push(data.TotalExport);
        data_totalPeb.push(data.TotalPeb);
      });

      var TrendExportChart = Highcharts.chart("container_trend", {
        chart: {
          type: "column",
          height: 300,
        },
        title: {
          text: "Trend Export",
          align: "left",
        },
        xAxis: {
          categories: categories_yearly,
        },
        yAxis: [
          {
            min: 0,
            title: {
              text: "Total Export Value in USD",
            },
            labels: {
              formatter: function () {
                if (this.value > 1000) return "USD " + Highcharts.numberFormat(this.value / 1000, 1) + "K"; // maybe only switch if > 1000
                return "USD " + Highcharts.numberFormat(this.value, 0);
              },
            },
          },
          {
            title: {
              text: "Total PEB",
            },
            opposite: true,
          },
        ],
        legend: {
          layout: "horizontal",
          align: "right",
          verticalAlign: "top",
        },
        tooltip: {
          shared: true,
        },
        plotOptions: {
          column: {
            grouping: false,
            shadow: false,
            borderWidth: 0,
          },
          series: {
            cursor: "pointer",
            point: {
              events: {
                click: function () {
                  getExportByCategory(this.category, this.category);
                },
              },
            },
          },
        },
        series: [
          {
            name: "Total Export Value",
            color: "#FF9900",
            data: data_totExport,
            tooltip: {
              valuePrefix: "$",
              valueSuffix: " M",
            },
            pointPadding: 0.3,
            pointPlacement: -0.2,
            borderRadius: 10,
          },
          {
            name: "Total PEB",
            color: "#FF9900",
            data: data_totalPeb,
            pointPadding: 0.3,
            pointPlacement: 0.2,
            yAxis: 1,
            borderRadius: 10,
          },
        ],
      });
      TrendExportChart.reflow();
      $(".highcharts-xaxis-labels text").click(function () {
        getExportByCategory(this.innerHTML, this.innerHTML);
      });
    },
  });
}

function getExportByCategory(startYear, endYear) {
  $.ajax({
    url: "getExportByCategory?startYear=" + parseInt(startYear) + "&endYear=" + parseInt(endYear),
    success: function (data) {
      var byCategory = [
        {
          name: "Category",
          colorByPoint: true,
          data: [],
        },
      ];

      $.each(data, function (i, data) {
        byCategory[0].data.push({
          name: data.Category,
          y: data.TotalPercentage * 100,
          //sliced: true
        });
      });

      var ExportByCategoryChart = Highcharts.chart("container_trend_pie", {
        chart: {
          plotBackgroundColor: null,
          plotBorderWidth: null,
          plotShadow: false,
          type: "pie",
          height: 300,
          //options3d: {
          //    enabled: true,
          //    alpha: 45,
          //    beta: 0
          //}
        },
        //colors: ["#ffca22", "#f7fbfb", "#05beff", "#ff696a", "#c63bff"],
        colors: ["#2F88FC", "#FF9C02", "#C22C2C"],
        title: {
          //text: "Export by Category",
          //align: "left",
          text: "",
        },
        // subtitle: {
        //   text: "<b>65%</b><br>Total new<br>customers",
        //   align: "left",
        //   verticalAlign: "middle",
        //   x: 170,
        //   y: 30,
        // },
        tooltip: {
          pointFormat: "{series.name}: <b>{point.percentage:.1f}%</b>",
        },
        plotOptions: {
          pie: {
            innerSize: 160,
            allowPointSelect: false,
            cursor: "pointer",
            //depth: 35,
            dataLabels: {
              enabled: false,
              //format: '{point.name}'
            },
            showInLegend: true,
          },
        },
        legend: {
          layout: "vertical",
          align: "center",
          verticalAlign: "bottom",
          itemStyle: {
            fontSize: "11px",
          },
          itemMarginBottom: 3,
        },
        credits: {
          enabled: false,
        },
        series: byCategory,
      });
      $("#container_trend_pie .loading-spinner").hide();
      ExportByCategoryChart.reflow();
    },
  });
}

function getSalesVSNonSales() {
  $.ajax({
    url:
      // "getSalesVSNonSales?startYear=" +
      // parseInt($("#SalesVSNonSalesStartYear").val()) +
      // "&endYear=" +
      // parseInt($("#SalesVSNonSalesEndYear").val()),
      "getSalesVSNonSales?startYear=2020&endYear=2022",
    success: function (data) {
      //var startYear = parseInt($("#SalesVSNonSalesStartYear").val());
      //console.log("Sales no Sales", data);
      var startYear = 2020;
      var byExpType = [
        { name: "Sales", type: "spline", data: [] },
        { name: "Non Sales", type: "spline", data: [] },
      ];

      $.each(data, function (i, data) {
        byExpType[0].data.push(data.Sales);
        byExpType[1].data.push(data.NonSales);
      });

      var SalesVSNonSalesChart = Highcharts.chart("container_compare_sales", {
        chart: {
          height: 300,
        },
        colors: ["#7f39fb", "#ffca22"],
        title: {
          //text: "Sales vs Non Sales",
          //align: "left",
          text: "",
        },
        yAxis: {
          title: {
            //text: "Total Amount",
            text: "",
          },
        },
        legend: {
          layout: "horizontal",
          align: "center",
          verticalAlign: "bottom",
          borderWidth: 0,
          itemStyle: {
            fontSize: "12px",
          },
          itemMarginBottom: 3,
        },
        credits: {
          enabled: false,
        },
        plotOptions: {
          //series: {
          //    label: {
          //        connectorAllowed: false
          //    },
          //    pointStart: startYear
          //}
          column: {
            pointPadding: 0.2,
            borderWidth: 0,
          },
        },
        series: byExpType,
        //responsive: {
        //    rules: [{
        //        condition: {
        //            maxWidth: 500
        //        },
        //        chartOptions: {
        //            legend: {
        //                layout: 'horizontal',
        //                align: 'center',
        //                verticalAlign: 'bottom'
        //            }
        //        }
        //    }]
        //}
      });
      $("#container_compare_sales .loading-spinner").hide();
      SalesVSNonSalesChart.reflow();
    },
  });
}

function getTotalExport() {
  $.ajax({
    url: "getTotalExport?year=" + parseInt($("#TotalExportYear").val()),
    success: function (data) {
      var datasales = [0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
      var datanonsales = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

      var outstandingCipl = [
        { name: "Total Invoice", type: "spline", data: [] },
        //{ name: 'Total PEB', type: 'column', data: [] },
        { name: "Outstanding", type: "spline", data: [] },
        { name: "Sales", type: "column", data: datasales, borderRadius: 10 },
        { name: "Non Sales", type: "column", data: datanonsales, borderRadius: 10 },
      ];

      var x_categories = [];

      $.each(data, function (i, data) {
        x_categories.push(data.Month);
        outstandingCipl[0].data.push(data.Invoice);
        //outstandingCipl[1].data.push(data.Peb);
        outstandingCipl[1].data.push(data.Outstanding);
      });
      //console.log(outstandingCipl);

      var TotalExportChart = Highcharts.chart("container", {
        chart: {
          type: "column",
          height: 300,
        },
        colors: ["#7f39fb", "#ffca22", "#a478f1", "#a478f1"],
        title: {
          //text: "Total Export Transaction",
          //align: "left",
          text: "",
        },
        subtitle: {
          //text: 'Source: WorldClimate.com'
        },
        credits: {
          enabled: false,
        },
        xAxis: {
          categories: x_categories,
          crosshair: true,
        },
        yAxis: {
          min: 0,
          title: {
            text: "Transaction",
          },
        },
        legend: {
          layout: "horizontal",
          align: "right",
          verticalAlign: "top",
        },
        //tooltip: {
        //    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
        //    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
        //        '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
        //    footerFormat: '</table>',
        //    shared: true,
        //    useHTML: true
        //},
        plotOptions: {
          column: {
            pointPadding: 0.2,
            borderWidth: 0,
          },
        },
        series: outstandingCipl,
      });
      $("#container .loading-spinner").hide();
      TotalExportChart.reflow();
    },
  });
}

function BigesCommoditiesCategory() {
  var year = $("#date1-export-type").val();
  var exporttype = $("#slc-export-type").val();
  $.ajax({
    url: "/emcs/TotalBig5CommoditiesList",
    async: true,
    data: {
      searchId: year,
      searchName: exporttype,
    },
    success: function (data) {
      var category = new Array();
      $.each(data, function (index, element) {
        category.push({ name: element.Desc, y: element.Total });
      });
      //console.log(category);
      var big5 = Highcharts.chart("container_bigest", {
        chart: {
          type: "column",
          height: 300,
        },
        //colors: ["#ffeed5", "#ffeed5", "#ffeed5", "#ffeed5", "#ffeed5"],
        colors: ["#fbbf24", "#60a5fa", "#f87171", "#c084fc", "#4ade80"],
        title: {
          //text: "5 Biggest Commodities Category",
          //align: "left",
          text: "",
        },
        subtitle: {
          //text: '2019'
        },
        credits: {
          enabled: false,
        },
        xAxis: {
          type: "category",
        },
        yAxis: {
          //title: {
          //text: "Total Percentage",
          //},
          title: {
            text: "",
          },
        },
        legend: {
          enabled: false,
        },
        plotOptions: {
          series: {
            borderWidth: 0,
            dataLabels: {
              enabled: true,
              format: "{point.y:.1f}%",
            },
            pointWidth: 16,
            borderRadius: 5,
          },
        },
        tooltip: {
          headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
          pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>',
        },
        series: [
          {
            //name: "Browsers",
            colorByPoint: true,
            data: category,
          },
        ],
      });
      $("#container_bigest .loading-spinner").hide();
      big5.reflow();
    },
  });
}
=======
    //function getDataSummary() {
    //    $.ajax({
    //        type: 'GET',
    //        url: "~/Json/ExportTransaction.json",
    //        dataType: JSON
    //    })
    //    .done(function (data, textStatus, jqXHR) {
    //            getHighChart();
    //    })
    //}

    $('#TrendExportButton').on('click', function (e) {
        $("#TrendExportFilter").toggle("show");
    });
    $("#TrendExportFilter").on("click", function (e) {
        e.stopPropagation();
    });

    $('#btn-big-5-commodities').on('click', function (e) {
        $("#ul-big-5-commodities").toggle("show");
    });
    $("#ul-big-5-commodities").on("click", function (e) {
        e.stopPropagation();
    });

    $('#ExportByCategoryButton').on('click', function (e) {
        $("#ExportByCategoryFilter").toggle("show");
    });
    $("#ExportByCategoryFilter").on("click", function (e) {
        e.stopPropagation();
    });

    $('#SalesVSNonSalesButton').on('click', function (e) {
        $("#SalesVSNonSalesFilter").toggle("show");
    });
    $("#SalesVSNonSalesFilter").on("click", function (e) {
        e.stopPropagation();
    });

    $('#TotalExportButton').on('click', function (e) {
        $("#TotalExportFilter").toggle("show");
    });
    $("#TotalExportFilter").on("click", function (e) {
        e.stopPropagation();
    });

    var intervals = [
        { id: 2, text: "2 Years" },
        { id: 3, text: "3 Years" },
        { id: 4, text: "4 Years" },
        { id: 5, text: "5 Years" },
        { id: 6, text: "6 Years" },
        { id: 7, text: "7 Years" },
        { id: 8, text: "8 Years" },
        { id: 9, text: "9 Years" },
        { id: 10, text: "10 Years" }
    ];

    $("#TrendExportYearInterval").select2({
        data: intervals,
        width: '100%'
    }).on("select2:select", function () {
        var endYear = (parseInt($("#TrendExportStartYear").val()) + parseInt($(this).val())).toString();

        $("#TrendExportEndYear").val(endYear);
        $("#ExportByCategoryEndYear").val(endYear);

        $('#ExportByCategoryYearInterval').val($(this).val()).trigger('change');
    });
    $('#TrendExportYearInterval').val(intervals[0].id).trigger('change');
    $("#TrendExportStartYear").val(new Date().getFullYear() - 2);
    $("#TrendExportEndYear").val(new Date().getFullYear());

    $("#ExportByCategoryYearInterval").select2({
        data: intervals,
        width: '100%'
    }).on("select2:select", function () {
        var endYear = (parseInt($("#ExportByCategoryStartYear").val()) + parseInt($(this).val())).toString();

        $("#TrendExportEndYear").val(endYear);
        $("#ExportByCategoryEndYear").val(endYear);

        $('#TrendExportYearInterval').val($(this).val()).trigger('change');
    });
    $('#ExportByCategoryYearInterval').val(intervals[0].id).trigger('change');
    $("#ExportByCategoryStartYear").val(new Date().getFullYear() - 2);
    $("#ExportByCategoryEndYear").val(new Date().getFullYear());


    $("#TrendExportStartYear").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    }).on('changeYear', function (e) {
        $(this).datepicker('hide');

        $("#ExportByCategoryStartYear").val(e.date.getFullYear());
        if (($('#TrendExportYearInterval').val() !== null && $('#TrendExportYearInterval').val() !== '')) {
            var endYear = (parseInt(e.date.getFullYear()) + parseInt($('#TrendExportYearInterval').val())).toString();
            $('#TrendExportEndYear').val(endYear);
            $('#ExportByCategoryEndYear').val(endYear);
        }
    });

    $("#ExportByCategoryStartYear").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    }).on('changeYear', function (e) {
        $(this).datepicker('hide');

        $("#TrendExportStartYear").val(e.date.getFullYear());
        if (($('#ExportByCategoryYearInterval').val() !== null && $('#ExportByCategoryYearInterval').val() !== '')) {
            var endYear = (parseInt(e.date.getFullYear()) + parseInt($('#ExportByCategoryYearInterval').val())).toString();
            $('#TrendExportEndYear').val(endYear);
            $('#ExportByCategoryEndYear').val(endYear);
        }
    });

    $("#SalesVSNonSalesStartYear").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    }).on('changeYear', function (e) {
        $(this).datepicker('hide');
        $('#SalesVSNonSalesEndYear').datepicker('setStartDate', (e.date.getFullYear()).toString());
    });
    $("#SalesVSNonSalesStartYear").val(new Date().getFullYear());

    $("#SalesVSNonSalesEndYear").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    }).on('changeYear', function (e) {
        $(this).datepicker('hide');
        $('#SalesVSNonSalesStartYear').datepicker('setEndDate', (e.date.getFullYear()).toString());
    });
    $("#SalesVSNonSalesEndYear").val(new Date().getFullYear());

    $('#SalesVSNonSalesEndYear').datepicker('setStartDate', ($("#SalesVSNonSalesStartYear").val()));
    $('#SalesVSNonSalesStartYear').datepicker('setEndDate', $("#SalesVSNonSalesEndYear").val());

    $("#TotalExportYear").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    }).on('changeYear', function (e) {
        $(this).datepicker('hide');
    });
    $("#TotalExportYear").val(new Date().getFullYear());

    $(".year").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });

    getHighChart();

});

function TrendExportSearch() {
    var startYear = $('#TrendExportStartYear').val();
    var endYear = $('#TrendExportEndYear').val();
    if (startYear !== null && startYear !== '' && endYear !== null && endYear !== '') {
        getTrendExport();
        getExportByCategory(startYear, endYear);
    }
}

function TrendExportDownload() {
    var startYear = $('#TrendExportStartYear').val();
    var endYear = $('#TrendExportEndYear').val();
    window.open('/EMCS/DownloadTrendExport?startYear=' + startYear + '&endYear=' + endYear, '_blank');
}

function ExportByCategorySearch() {
    var startYear = $('#ExportByCategoryStartYear').val();
    var endYear = $('#ExportByCategoryEndYear').val();
    if (startYear !== null && startYear !== '' && endYear !== null && endYear !== '') {
        getExportByCategory(startYear, endYear);
    }
}

function ExportByCategoryDownload() {
    var startYear = $('#ExportByCategoryStartYear').val();
    var endYear = $('#ExportByCategoryEndYear').val();
    window.open('/EMCS/DownloadExportByCategory?startYear=' + startYear + '&endYear=' + endYear, '_blank');
}

function SalesVSNonSalesSearch() {
    var startYear = $('#SalesVSNonSalesStartYear').val();
    var endYear = $('#SalesVSNonSalesEndYear').val();
    if (startYear !== null && startYear !== '' && endYear !== null && endYear !== '') {
        getSalesVSNonSales();
    }
}

function SalesVSNonSalesDownload() {
    var startYear = $('#SalesVSNonSalesStartYear').val();
    var endYear = $('#SalesVSNonSalesEndYear').val();
    window.open('/EMCS/DownloadSalesVSNonSales?startYear=' + startYear + '&endYear=' + endYear, '_blank');
}

function TotalExportSearch() {
    var year = $('#TotalExportYear').val();
    if (year !== null && year !== '') {
        getTotalExport();
    }
}

function TotalExportDownload() {
    var year = $('#TotalExportYear').val();
    window.open('/EMCS/DownloadTotalExport?year=' + year, '_blank');
}

$('#form-export-today').on('submit', function (e) {
    e.preventDefault();
    BigesCommoditiesCategory();
})
$('#btn-big-5-commodities-download').on('click', function () {
    var year = $('#date1-export-type').val();
    var exporttype = $('#slc-export-type').val();
    window.open('/EMCS/DownloadBig5Commodities?searchId=' + year + '&searchName=' + exporttype, '_blank');
})

function GetDataSummary() {

    //var newData = {
    //    "Perioddatefrom": dtfrom,
    //    "Perioddateto": dtto,
    //    "BranchCC100": BranchCC100
    //}

    //$.ajax({
    //    //type: 'GET',
    //    //url: "~/Json/ExportTransaction.json",
    //    dataType: JSON,
    //    data: newData
    //})
    //.done(function (data, textStatus, jqXHR) {
    //   getHighChart();
    //})
    //.fail(function (jqXHR, textStatus, errorThrown) {
    //    var result = $.parseJSON(jqXHR.responseText);
    //    sAlert(result.Message,
    //        result.ExceptionMessage + "&#13;&#10;" +
    //        result.ExceptionType + "&#13;&#10;" +
    //        result.StackTrace, "error");
    //});
    getHighChart();
}

function getHighChart() {
    getTrendExport();
    getExportByCategory($("#TrendExportStartYear").val(), $("#TrendExportEndYear").val());
    getSalesVSNonSales();
    getTotalExport();
    BigesCommoditiesCategory();

    //Highcharts.chart('container_bigest', {
    //    chart: {
    //        type: 'column'
    //    },
    //    colors: ['#ffca22', '#9dd45d', '#05beff', '#ff696a', '#c63bff'],
    //    title: {
    //        text: '5 Bigest Commodities Category'
    //    },
    //    subtitle: {
    //        text: '2019'
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    xAxis: {
    //        type: 'category'
    //    },
    //    yAxis: {
    //        title: {
    //            text: 'Total percent commodities'
    //        }

    //    },
    //    legend: {
    //        enabled: false
    //    },
    //    credits: {
    //        enabled: false
    //    },
    //    plotOptions: {
    //        series: {
    //            borderWidth: 0,
    //            dataLabels: {
    //                enabled: true,
    //                format: '{point.y:.1f}%'
    //            }
    //        }
    //    },

    //    tooltip: {
    //        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
    //        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
    //    },

    //    series: [
    //        {
    //            name: "Browsers",
    //            colorByPoint: true,
    //            data: [
    //                {
    //                    name: "Parts",
    //                    y: 62.74
    //                },
    //                {
    //                    name: "Machine",
    //                    y: 10.57
    //                },
    //                {
    //                    name: "Engine",
    //                    y: 7.23
    //                },
    //                {
    //                    name: "Forklift",
    //                    y: 5.58
    //                },
    //                {
    //                    name: "Miscellenious",
    //                    y: 4.02
    //                }
    //            ]
    //        }
    //    ],

    //});

}

function getTrendExport() {
    $.ajax({
        url: 'getTrendExport?startYear=' + parseInt($("#TrendExportStartYear").val()) + '&endYear=' + parseInt($("#TrendExportEndYear").val()),
        success: function (data) {
            var categories_yearly = [];
            var data_totExport = [];
            var data_totalPeb = [];

            $.each(data, function (i, data) {
                categories_yearly.push(data.Year);
                data_totExport.push(data.TotalExport);
                data_totalPeb.push(data.TotalPeb);
            })

            var TrendExportChart =
                Highcharts.chart('container_trend', {
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'Trend Export'
                    },
                    xAxis: {
                        categories: categories_yearly
                    },
                    yAxis: [{
                        min: 0,
                        title: {
                            text: 'Total Export Value in USD'
                        },
                        labels: {
                            formatter: function () {
                                if (this.value > 1000) return 'USD ' + Highcharts.numberFormat(this.value / 1000, 1) + "K";  // maybe only switch if > 1000
                                return 'USD ' + Highcharts.numberFormat(this.value, 0);
                            }
                        }
                    }, {
                        title: {
                            text: 'Total PEB'
                        },
                        opposite: true
                    }],
                    legend: {
                        shadow: false
                    },
                    tooltip: {
                        shared: true
                    },
                    plotOptions: {
                        column: {
                            grouping: false,
                            shadow: false,
                            borderWidth: 0
                        },
                        series: {
                            cursor: 'pointer',
                            point: {
                                events: {
                                    click: function () {
                                        getExportByCategory(this.category, this.category);
                                    }
                                }
                            }
                        }
                    },
                    series: [
                        {
                            name: 'Total Export Value',
                            color: '#FF9900',
                            data: data_totExport,
                            tooltip: {
                                valuePrefix: '$',
                                valueSuffix: ' M'
                            },
                            pointPadding: 0.3,
                            pointPlacement: -0.2
                        }, {
                            name: 'Total PEB',
                            color: '#000000',
                            data: data_totalPeb,
                            pointPadding: 0.3,
                            pointPlacement: 0.2,
                            yAxis: 1
                        }]
                });
            TrendExportChart.reflow();
            $('.highcharts-xaxis-labels text').click(function () {
                getExportByCategory(this.innerHTML, this.innerHTML);
            });
        }
    });
}

function getExportByCategory(startYear, endYear) {
    $.ajax({
        url: 'getExportByCategory?startYear=' + parseInt(startYear) + '&endYear=' + parseInt(endYear),
        success: function (data) {
            var byCategory = [{
                name: 'Category',
                colorByPoint: true,
                data: []
            }];

            $.each(data, function (i, data) {
                byCategory[0].data.push({
                    name: data.Category,
                    y: data.TotalPercentage * 100
                    //sliced: true
                });
            });

            var ExportByCategoryChart = Highcharts.chart('container_trend_pie', {
                chart: {
                    type: 'pie',
                    options3d: {
                        enabled: true,
                        alpha: 45,
                        beta: 0
                    }
                },
                colors: ['#ffca22', '#9dd45d', '#05beff', '#ff696a', '#c63bff'],
                title: {
                    text: 'Export by Category'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        depth: 35,
                        dataLabels: {
                            enabled: true,
                            format: '{point.name}'
                        }
                    }
                },
                credits: {
                    enabled: false
                },
                series: byCategory
            });
            ExportByCategoryChart.reflow();
        }
    });
}

function getSalesVSNonSales() {
    $.ajax({
        url: 'getSalesVSNonSales?startYear=' + parseInt($("#SalesVSNonSalesStartYear").val()) + '&endYear=' + parseInt($("#SalesVSNonSalesEndYear").val()),
        success: function (data) {
            var startYear = parseInt($("#SalesVSNonSalesStartYear").val());
            var byExpType =
                [
                    { name: 'Sales', data: [] },
                    { name: 'Non Sales', data: [] }
                ];

            $.each(data, function (i, data) {
                byExpType[0].data.push(data.Sales);
                byExpType[1].data.push(data.NonSales);
            })

            var SalesVSNonSalesChart = Highcharts.chart('container_compare_sales', {
                colors: ['#ffca22', '#000'],
                title: {
                    text: 'Sales vs Non Sales'
                },
                yAxis: {
                    title: {
                        text: 'Total Amount'
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },
                credits: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        },
                        pointStart: startYear
                    }
                },
                series: byExpType,
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                }

            });
            SalesVSNonSalesChart.reflow();
        }
    });
}



function getTotalExport() {
    $.ajax({
        url: 'getTotalExport?year=' + parseInt($("#TotalExportYear").val()),
        success: function (data) {
            var outstandingCipl = [
                { name: 'Total Invoice & Packing List', type: 'column', data: [] },
                { name: 'Total PEB', type: 'column', data: [] },
                { name: 'Outstanding', type: 'spline', data: [] }
            ];
            var x_categories = [];

            $.each(data, function (i, data) {
                x_categories.push(data.Month);
                outstandingCipl[0].data.push(data.Invoice);
                outstandingCipl[1].data.push(data.Peb);
                outstandingCipl[2].data.push(data.Outstanding);
            });

            var TotalExportChart = Highcharts.chart('container', {
                chart: {
                    type: 'column'
                },
                colors: ['#ffca22', '#000'],
                title: {
                    text: 'Total Export Transaction'
                },
                subtitle: {
                    //text: 'Source: WorldClimate.com'
                },
                credits: {
                    enabled: false
                },
                xAxis: {
                    categories: x_categories,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Transaction'
                    }
                },
                //tooltip: {
                //    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                //    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                //        '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
                //    footerFormat: '</table>',
                //    shared: true,
                //    useHTML: true
                //},
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: outstandingCipl
            });
            TotalExportChart.reflow();
        }
    });
}

function BigesCommoditiesCategory() {
    var year = $('#date1-export-type').val();
    var exporttype = $('#slc-export-type').val();
    $.ajax({
        url: "/emcs/TotalBig5CommoditiesList",
        async: false,
        data: {
            searchId: year,
            searchName: exporttype
        },
        success: function (data) {
            var category = new Array;
            $.each(data, function (index, element) {
                category.push({ name: element.Desc, y: element.Total });
            });
            console.log(category);
            var big5 = Highcharts.chart('container_bigest', {
                chart: {
                    type: 'column'
                },
                colors: ['#ffca22', '#9dd45d', '#05beff', '#ff696a', '#c63bff'],
                title: {
                    text: '5 Bigest Commodities Category'
                },
                subtitle: {
                    //text: '2019'
                },
                credits: {
                    enabled: false
                },
                xAxis: {
                    type: 'category'
                },
                yAxis: {
                    title: {
                        text: 'Total Percentage'
                    }

                },
                legend: {
                    enabled: false
                },
                credits: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:.1f}%'
                        }
                    }
                },

                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
                },

                series: [
                    {
                        name: "Browsers",
                        colorByPoint: true,
                        data: category
                    }
                ],

            });
            big5.reflow();
        }
    });
}
>>>>>>> 639d8d0 (Intial commit)
