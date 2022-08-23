const getExportToday = (date1, date2) => {
  return new Promise((resolve, reject) => {
    $.ajax({
      url: "/emcs/ExportToday",
      data: {
        date1: date1,
        date2: date2,
        searchCode: $("#Area").val(),
      },
    })
      .done(function (data) {
        resolve(data);
      })
      .fail(function (error) {
        reject(error);
      });
  });
};

const getTotalNetWeight = (date1, date2) => {
  return new Promise((resolve, reject) => {
    $.ajax({
      url: "/emcs/TotalNetWeight",
      data: {
        date1: date1,
        date2: date2,
        searchCode: $("#Area").val(),
      },
    })
      .done(function (data) {
        resolve(data);
      })
      .fail(function (error) {
        reject(error);
      });
  });
};

const getTotalExportValue = (date1, date2) => {
  return new Promise((resolve, reject) => {
    $.ajax({
      url: "/emcs/TotalExportValue",
      data: {
        date1: date1,
        date2: date2,
        searchCode: $("#Area").val(),
      },
    })
      .done(function (data) {
        resolve(data);
      })
      .fail(function (error) {
        reject(error);
      });
  });
};

const getExchangeRate = (date1, date2) => {
  return new Promise((resolve, reject) => {
    $.ajax({
      url: "/emcs/ExchangeRateToday",
      data: {
        date1: date1,
        date2: date2,
        searchCode: $("#Area").val(),
      },
    })
      .done(function (data) {
        resolve(data);
      })
      .fail(function (error) {
        reject(error);
      });
  });
};

const loadExportToday = (date1, date2) => {
  //console.log("loadExportToday Called");
  $("#slider-export-today .loading-spinner").show();
  $("#summary-export-today").html(null);
  $("#ul-export-today").html(null);
  getExportToday(date1, date2)
    .then((data) => {
      //console.log(data);
      let object = data.model;
      let object2 = data.model2[0];
      let objectLength = object.length + Object.keys(object2).length;
      let summaryDataContainer = "#summary-export-today";
      let summaryDataUl = "#ul-export-today";
      if (object.length > 0) {
        $(summaryDataContainer).append(
          "<div class='summary-number-item'>" +
            "<span class='summary-number-amount'>" +
            object[0].Total +
            "</span>" +
            "<span class='summary-number-label'>" +
            object[0].Category +
            "</span>" +
            "</div>"
        );
        $.each(object, function (index, element) {
          let html = '<li title="' + element.Desc + '">' + element.Total + "  " + element.Category + "</li>";
          $(summaryDataUl).append(html);
        });
        if (Object.keys(object2).length > 0) {
          $.each(object2, function (index, element) {
            let html = "<li>" + element + "  " + index + "</li>";
            $(summaryDataUl).append(html);
          });
        }
      }
      rotateSummaryContent(objectLength, summaryDataContainer, summaryDataUl, "");
      //console.log("loadExportToday Done");
      $("#slider-export-today .loading-spinner").hide();
    })
    .catch((err) => {
      console.log(err);
    });
};

const loadTotalNetWeight = (date1, date2) => {
  //console.log("loadTotalNetWeight Called");
  $("#slider-net-weight .loading-spinner").show();
  $("#summary-net-weight").html(null);
  $("#ul-net-weight").html(null);
  getTotalNetWeight(date1, date2)
    .then((data) => {
      //console.log(data);
      let objectLength = data.length;
      let summaryDataContainer = "#summary-net-weight";
      let summaryDataUl = "#ul-net-weight";
      if (data.length > 0) {
        $(summaryDataContainer).append(
          "<div class='summary-number-item'>" +
            "<span class='summary-number-amount'>" +
            data[0].Total +
            "</span>" +
            "<span class='summary-number-label'>" +
            "Ton " +
            data[0].Desc +
            "</span>" +
            "</div>"
        );
        $.each(data, function (index, element) {
          let html = '<li title="' + element.Desc + '">' + element.Total + " Ton " + element.Desc + "</li>";
          $(summaryDataUl).append(html);
        });
      }
      rotateSummaryContent(objectLength, summaryDataContainer, summaryDataUl, "Ton ");
      //console.log("loadTotalNetWeight Done");
      $("#slider-net-weight .loading-spinner").hide();
    })
    .catch((err) => {
      console.log(err);
    });
};

const loadTotalExportValue = (date1, date2) => {
  //console.log("loadTotalExportValue Called");
  $("#slider-export-value .loading-spinner").show();
  $("#summary-export-value").html(null);
  $("#ul-export-value").html(null);
  getTotalExportValue(date1, date2)
    .then((data) => {
      //console.log(data);
      let objectLength = data.length;
      let summaryDataContainer = "#summary-export-value";
      let summaryDataUl = "#ul-export-value";
      let firstword = [];
      let desc1 = data[0].Desc.substr(0, data[0].Desc.indexOf("("));
      let desc2 = desc1.split(" ");
      $.each(desc2, function (index, element) {
        let first = element.substring(0, 1);
        firstword.push(first);
      });

      $(summaryDataContainer).append(
        "<div class='summary-number-item'>" +
          "<span class='summary-number-amount'>" +
          "$" +
          formatCurrency(data[0].Total, ".", ",", 2) +
          "</span>" +
          "<span class='summary-number-label'>" +
          " Total " +
          firstword.join("") +
          "</span>" +
          "</div>"
      );

      $.each(data, function (index, element) {
        let firstword2 = [];
        let desc3 = element.Desc.substr(0, element.Desc.indexOf("("));
        let desc4 = desc3.split(" ");
        $.each(desc4, function (index2, element) {
          let first2 = element.substring(0, 1);
          firstword2.push(first2);
        });
        let html = "<li>$" + formatCurrency(element.Total, ".", ",", 2) + "  Total " + firstword2.join("") + "</li>";
        $(summaryDataUl).append(html);
      });
      rotateSummaryContent(objectLength, summaryDataContainer, summaryDataUl, "");
      //console.log("loadTotalExportValue Done");
      $("#slider-export-value .loading-spinner").hide();
    })
    .catch((err) => {
      console.log(err);
    });
};

const loadExchangeRateToday = (date1, date2 = "") => {
  $("#slider-exchange-rate .loading-spinner").show();
  $("#summary-exchange-rate").html(null);
  $("#ul-exchange-rate").html(null);
  getExchangeRate(date1, date2)
    .then((data) => {
      //console.log(data);
      let objectLength = data.length;
      let summaryDataContainer = "#summary-exchange-rate";
      let summaryDataUl = "#ul-exchange-rate";
      if (data.length > 0) {
        $(summaryDataContainer).append(
          "<div class='summary-number-item'>" +
            "<span class='summary-number-amount'>" +
            formatCurrency(data[0].Rate, ".", ",", 2) +
            "</span>" +
            "<span class='summary-number-label'>" +
            "IDR / " +
            data[0].Curr +
            "</span>" +
            "</div>"
        );
        $.each(data, function (index, element) {
          let html = "<li>" + formatCurrency(element.Rate, ".", ",", 2) + " IDR / " + element.Curr + "</li>";
          $(summaryDataUl).append(html);
        });
      }
      rotateSummaryContent(objectLength, summaryDataContainer, summaryDataUl, "IDR ");
      $("#slider-exchange-rate .loading-spinner").hide();
    })
    .catch((err) => {
      console.log(err);
    });
};

const rotateSummaryContent = (objectLength, summaryDataContainer, summaryDataUl, unitSuffix) => {
  let counter = 2;
  const rotator = setInterval(function () {
    if (counter > objectLength) {
      counter = 1;
    }
    let nextDataLi = $(summaryDataUl + " li:nth-child(" + counter + ")")[0].textContent;
    if (nextDataLi) {
      let nextDataArray = nextDataLi.split(" ");
      $(summaryDataContainer).append(
        "<div class='summary-number-item'>" +
          "<span class='summary-number-amount'>" +
          nextDataArray.slice(0, 1) +
          "</span>" +
          "<span class='summary-number-label'>" +
          unitSuffix +
          nextDataArray.slice(2).join(" ") +
          "</span>" +
          "</div>"
      );
      $(summaryDataContainer + " .summary-number-item:first-child").addClass("remove");
      setTimeout(function () {
        $(summaryDataContainer + " .summary-number-item:nth-child(2)").addClass("active");
      }, "150");
      setTimeout(function () {
        $(summaryDataContainer + " .summary-number-item:first-child").remove();
      }, "1000");
    }
    counter++;
  }, 5000);
};

const loadMap = (type) => {
  //console.log("loadMap Called");
  $("#map-loader").show();
  $("#container-map").hide();
  $("#map-title").text("");
  var title = "";
  if (type === "Branch") {
    title = "Export Current Activity by Branch";
  } else {
    title = "Export Current Activity by Port";
  }

  Highcharts.SVGRenderer.prototype.symbols.cross = function (x, y, w, h) {
    var returnArray = [];
    var startPoint = x - 5 * w;
    var endPoint = x + 5 * w;
    var dashWidth = w;
    var gapWidth = 0.5 * w;

    var yValWithOffset = y + w / 2; //account for marker radius when moving to the right Y position

    var currentPoint = startPoint;
    for (var currentPoint = startPoint; currentPoint <= endPoint; currentPoint += dashWidth + gapWidth /*jump forward one location*/) {
      returnArray.push("M", currentPoint, yValWithOffset, "L", currentPoint + dashWidth, yValWithOffset);
    }
    returnArray.push("z"); //end drawing
    return returnArray;
  };

  if (Highcharts.VMLRenderer) {
    Highcharts.VMLRenderer.prototype.symbols.cross = Highcharts.SVGRenderer.prototype.symbols.cross;
  }

  var h = Highcharts,
    map = h.maps["countries/id/id-all"],
    chart;

  Highcharts.getJSON("/EMCS/GetMapData?type=" + type + "&user" + $("#Area").val(), function (json) {
    $("#map-loader").hide();
    $("#container-map").show();
    $("#map-title").text(title);
    var data = [];
    json.data.forEach(function (p) {
      p.lat = p.Lat;
      p.lon = p.Lon;
      p.province = p.Province;
      p.data = p.Data;
      p.z = p.Total;
      data.push(p);
    });
    //console.log(data);
    data = [
      {
        Lat: "-8.584429999999999",
        Lon: "120.689",
        Province: "NTT",
        Data: "- E.0512220244HW Ariliago<br>- E.2411220243HW Ariliago<br>- E.2311220242HW Ariliago<br>- E.2311220241HW Ariliago<br>- E.2111220240HW Ariliago<br>- E.1611220239HW Ariliago<br>- E.1511220238HW Ariliago<br>- E.1111220237HW Ariliago<br>- E.1011220236HW Ariliago<br>- E.0711220235HW Ariliago<br>- E.0411220234HW Ariliago<br>- E.2410220233HW Ariliago<br>- E.2010220231HW Ariliago<br>- E.1910220230HW Ariliago<br>- E.1810220228HW Ariliago<br>- E.1710220227HW Ariliago<br>- E.1410220226HW Ariliago<br>- E.1310220225HW Ariliago<br>- E.1210220224JP Asmat Awaluddin<br>- E.1210220223HW Ariliago<br>- E.1210220222HW Ariliago<br>- E.0610220218HW Ariliago<br>- E.0510220217HW Ariliago<br>- E.2909220213HW Ariliago<br>- E.2909220212HW Ariliago<br>- E.2209220211HW Ariliago<br>- E.2109220210HW Ariliago<br>- E.1309220207HW Ariliago<br>- E.0909220206HW Ariliago<br>- E.0909220205HW Ariliago<br>- E.0509220199HW Ariliago<br>- E.3003220066HW Ariliago<br>- E.2403220053HW Ariliago<br>- E.2103220046JP Asmat Awaluddin",
        Total: 0,
        lat: "-8.584429999999999",
        lon: "120.689",
        province: "NTT",
        data: "- E.0512220244HW Ariliago<br>- E.2411220243HW Ariliago<br>- E.2311220242HW Ariliago<br>- E.2311220241HW Ariliago<br>- E.2111220240HW Ariliago<br>- E.1611220239HW Ariliago<br>- E.1511220238HW Ariliago<br>- E.1111220237HW Ariliago<br>- E.1011220236HW Ariliago<br>- E.0711220235HW Ariliago<br>- E.0411220234HW Ariliago<br>- E.2410220233HW Ariliago<br>- E.2010220231HW Ariliago<br>- E.1910220230HW Ariliago<br>- E.1810220228HW Ariliago<br>- E.1710220227HW Ariliago<br>- E.1410220226HW Ariliago<br>- E.1310220225HW Ariliago<br>- E.1210220224JP Asmat Awaluddin<br>- E.1210220223HW Ariliago<br>- E.1210220222HW Ariliago<br>- E.0610220218HW Ariliago<br>- E.0510220217HW Ariliago<br>- E.2909220213HW Ariliago<br>- E.2909220212HW Ariliago<br>- E.2209220211HW Ariliago<br>- E.2109220210HW Ariliago<br>- E.1309220207HW Ariliago<br>- E.0909220206HW Ariliago<br>- E.0909220205HW Ariliago<br>- E.0509220199HW Ariliago<br>- E.3003220066HW Ariliago<br>- E.2403220053HW Ariliago<br>- E.2103220046JP Asmat Awaluddin",
        z: 0,
      },
      {
        Lat: "-7.88129",
        Lon: "112.616",
        Province: "Jawa Timur",
        Data: "- E.0310220215HW Ariliago<br>- E.2408220181HW Ariliago<br>- E.0706220134HW Ariliago<br>- E.0206220133HW Ariliago<br>- E.2505220128HW Ariliago<br>- E.1205220116HW Ariliago<br>- E.3103220075HW Ariliago<br>- E.2403220052HW Ariliago",
        Total: 0,
        lat: "-7.88129",
        lon: "112.616",
        province: "Jawa Timur",
        data: "- E.0310220215HW Ariliago<br>- E.2408220181HW Ariliago<br>- E.0706220134HW Ariliago<br>- E.0206220133HW Ariliago<br>- E.2505220128HW Ariliago<br>- E.1205220116HW Ariliago<br>- E.3103220075HW Ariliago<br>- E.2403220052HW Ariliago",
        z: 0,
      },
      {
        Lat: "-1.84217",
        Lon: "113.286",
        Province: "Kalimantan Tengah",
        Data: "- E.0909220204HW Ariliago<br>- E.0509220200HW Ariliago<br>- E.0509220198HW Ariliago<br>- E.0509220197HW Ariliago<br>- E.1108220146JP Asmat Awaluddin<br>- E.2305220127HW Ariliago<br>- E.0804220084JP Asmat Awaluddin<br>- E.2303220051HW Ariliago<br>- E.1603220041JP Asmat Awaluddin<br>- E.1301220006HW Ariliago<br>- E.0501220002HW Ariliago",
        Total: 0,
        lat: "-1.84217",
        lon: "113.286",
        province: "Kalimantan Tengah",
        data: "- E.0909220204HW Ariliago<br>- E.0509220200HW Ariliago<br>- E.0509220198HW Ariliago<br>- E.0509220197HW Ariliago<br>- E.1108220146JP Asmat Awaluddin<br>- E.2305220127HW Ariliago<br>- E.0804220084JP Asmat Awaluddin<br>- E.2303220051HW Ariliago<br>- E.1603220041JP Asmat Awaluddin<br>- E.1301220006HW Ariliago<br>- E.0501220002HW Ariliago",
        z: 0,
      },
      {
        Lat: "-7.2901",
        Lon: "109.896",
        Province: "Jawa Tengah",
        Data: "- E.0909220203HW Ariliago<br>- E.1108220145JP Asmat Awaluddin<br>- E.0808220143HW Ariliago<br>- E.0308220141JP Asmat Awaluddin<br>- E.3005220131HW Ariliago<br>- E.3005220130HW Ariliago<br>- E.0704220083HW Ariliago<br>- E.0704220082HW Ariliago<br>- E.1102220022HW Ariliago<br>- E.1002220020HW Ariliago",
        Total: 0,
        lat: "-7.2901",
        lon: "109.896",
        province: "Jawa Tengah",
        data: "- E.0909220203HW Ariliago<br>- E.1108220145JP Asmat Awaluddin<br>- E.0808220143HW Ariliago<br>- E.0308220141JP Asmat Awaluddin<br>- E.3005220131HW Ariliago<br>- E.3005220130HW Ariliago<br>- E.0704220083HW Ariliago<br>- E.0704220082HW Ariliago<br>- E.1102220022HW Ariliago<br>- E.1002220020HW Ariliago",
        z: 0,
      },
      {
        Lat: "-6.90763",
        Lon: "107.638",
        Province: "Jawa Barat",
        Data: "- E.0809220202HW Ariliago<br>- E.0709220201HW Ariliago<br>- E.0209220196HW Ariliago<br>- E.2308220180HW Ariliago<br>- E.1908220177HW Ariliago<br>- E.2706220138HW Ariliago<br>- E.2106220137HW Ariliago<br>- E.1306220135HW Ariliago<br>- E.0206220132HW Ariliago<br>- E.3005220129HW Ariliago<br>- E.2005220126HW Ariliago<br>- E.2005220125HW Ariliago<br>- E.2005220124HW Ariliago<br>- E.2005220122HW Ariliago<br>- E.1905220121HW Ariliago<br>- E.1705220120HW Ariliago<br>- E.1705220119HW Ariliago<br>- E.1305220118HW Ariliago<br>- E.1305220117HW Ariliago<br>- E.1205220115HW Ariliago<br>- E.1205220114HW Ariliago<br>- E.1205220113HW Ariliago<br>- E.1105220112HW Ariliago<br>- E.0905220111HW Ariliago<br>- E.2804220109HW Ariliago<br>- E.2804220107HW Ariliago<br>- E.2804220106HW Ariliago<br>- E.2604220105HW Ariliago<br>- E.2604220104HW Ariliago<br>- E.2504220103HW Ariliago<br>- E.2104220100HW Ariliago<br>- E.2104220098HW Ariliago<br>- E.2004220097HW Ariliago<br>- E.2004220096HW Ariliago<br>- E.1904220095HW Ariliago<br>- E.1804220094HW Ariliago<br>- E.1304220093HW Ariliago<br>- E.1204220092HW Ariliago<br>- E.1104220085HW Ariliago<br>- E.0704220081HW Ariliago<br>- E.0704220080HW Ariliago<br>- E.0704220079HW Ariliago<br>- E.0604220078HW Ariliago<br>- E.0104220077HW Ariliago<br>- E.0104220076HW Ariliago<br>- E.3103220067HW Ariliago<br>- E.2903220061HW Ariliago<br>- E.2903220060HW Ariliago<br>- E.2903220059HW Ariliago<br>- E.2503220054HW Ariliago<br>- E.2303220048HW Ariliago<br>- E.1603220042HW Ariliago<br>- E.1603220040HW Ariliago<br>- E.0402220011HW Ariliago<br>- E.0402220010HW Ariliago",
        Total: 0,
        lat: "-6.90763",
        lon: "107.638",
        province: "Jawa Barat",
        data: "- E.0809220202HW Ariliago<br>- E.0709220201HW Ariliago<br>- E.0209220196HW Ariliago<br>- E.2308220180HW Ariliago<br>- E.1908220177HW Ariliago<br>- E.2706220138HW Ariliago<br>- E.2106220137HW Ariliago<br>- E.1306220135HW Ariliago<br>- E.0206220132HW Ariliago<br>- E.3005220129HW Ariliago<br>- E.2005220126HW Ariliago<br>- E.2005220125HW Ariliago<br>- E.2005220124HW Ariliago<br>- E.2005220122HW Ariliago<br>- E.1905220121HW Ariliago<br>- E.1705220120HW Ariliago<br>- E.1705220119HW Ariliago<br>- E.1305220118HW Ariliago<br>- E.1305220117HW Ariliago<br>- E.1205220115HW Ariliago<br>- E.1205220114HW Ariliago<br>- E.1205220113HW Ariliago<br>- E.1105220112HW Ariliago<br>- E.0905220111HW Ariliago<br>- E.2804220109HW Ariliago<br>- E.2804220107HW Ariliago<br>- E.2804220106HW Ariliago<br>- E.2604220105HW Ariliago<br>- E.2604220104HW Ariliago<br>- E.2504220103HW Ariliago<br>- E.2104220100HW Ariliago<br>- E.2104220098HW Ariliago<br>- E.2004220097HW Ariliago<br>- E.2004220096HW Ariliago<br>- E.1904220095HW Ariliago<br>- E.1804220094HW Ariliago<br>- E.1304220093HW Ariliago<br>- E.1204220092HW Ariliago<br>- E.1104220085HW Ariliago<br>- E.0704220081HW Ariliago<br>- E.0704220080HW Ariliago<br>- E.0704220079HW Ariliago<br>- E.0604220078HW Ariliago<br>- E.0104220077HW Ariliago<br>- E.0104220076HW Ariliago<br>- E.3103220067HW Ariliago<br>- E.2903220061HW Ariliago<br>- E.2903220060HW Ariliago<br>- E.2903220059HW Ariliago<br>- E.2503220054HW Ariliago<br>- E.2303220048HW Ariliago<br>- E.1603220042HW Ariliago<br>- E.1603220040HW Ariliago<br>- E.0402220011HW Ariliago<br>- E.0402220010HW Ariliago",
        z: 0,
      },
      {
        Lat: "-6.25794",
        Lon: "106.167",
        Province: "Banten",
        Data: "- E.2908220189HW Ariliago",
        Total: 0,
        lat: "-6.25794",
        lon: "106.167",
        province: "Banten",
        data: "- E.2908220189HW Ariliago",
        z: 0,
      },
      {
        Lat: "-1.65497",
        Lon: "102.823",
        Province: "Jambi",
        Data: "- E.2508220188HW Ariliago<br>- E.2508220187HW Ariliago<br>- E.2508220186HW Ariliago<br>- E.2508220185HW Ariliago",
        Total: 0,
        lat: "-1.65497",
        lon: "102.823",
        province: "Jambi",
        data: "- E.2508220188HW Ariliago<br>- E.2508220187HW Ariliago<br>- E.2508220186HW Ariliago<br>- E.2508220185HW Ariliago",
        z: 0,
      },
      {
        Lat: "0.853039",
        Lon: "124.446",
        Province: "Sulawesi Utara",
        Data: "- E.2408220184HW Ariliago",
        Total: 0,
        lat: "0.853039",
        lon: "124.446",
        province: "Sulawesi Utara",
        data: "- E.2408220184HW Ariliago",
        z: 0,
      },
      {
        Lat: "1.28915",
        Lon: "116.354",
        Province: "Kalimantan Timur",
        Data: "- E.2208220178HW Ariliago",
        Total: 0,
        lat: "1.28915",
        lon: "116.354",
        province: "Kalimantan Timur",
        data: "- E.2208220178HW Ariliago",
        z: 0,
      },
      {
        Lat: "-6.22462",
        Lon: "106.837",
        Province: "Jakarta Raya",
        Data: "- E.3103220074HW Ariliago<br>- E.3103220073HW Ariliago<br>- E.3103220072HW Ariliago<br>- E.3103220071HW Ariliago<br>- E.3103220070HW Ariliago<br>- E.3103220069HW Ariliago<br>- E.3103220068HW Ariliago<br>- E.3003220064HW Ariliago<br>- E.2903220062HW Ariliago<br>- E.1703220043HW Ariliago<br>- E.1603220038HW Ariliago<br>- E.2402220031HW Ariliago<br>- E.2202220030HW Ariliago<br>- E.2202220029HW Ariliago<br>- E.0702220013HW Ariliago",
        Total: 0,
        lat: "-6.22462",
        lon: "106.837",
        province: "Jakarta Raya",
        data: "- E.3103220074HW Ariliago<br>- E.3103220073HW Ariliago<br>- E.3103220072HW Ariliago<br>- E.3103220071HW Ariliago<br>- E.3103220070HW Ariliago<br>- E.3103220069HW Ariliago<br>- E.3103220068HW Ariliago<br>- E.3003220064HW Ariliago<br>- E.2903220062HW Ariliago<br>- E.1703220043HW Ariliago<br>- E.1603220038HW Ariliago<br>- E.2402220031HW Ariliago<br>- E.2202220030HW Ariliago<br>- E.2202220029HW Ariliago<br>- E.0702220013HW Ariliago",
        z: 0,
      },
    ];
    chart = Highcharts.mapChart("container-map", {
      credits: false,
      chart: {
        map: "countries/id/id-all",
        backgroundColor: "none",
        //height: "56.25%",
        style: {
          fontFamily: "Arial",
        },
        events: {
          render: function () {
            const chart = this,
              group = chart.series[0].group,
              bBox = group.getBBox(),
              ratio = bBox.width / bBox.height;

            if (!chart.allowUpdate) {
              chart.allowUpdate = true;
              chart.setSize(null, (chart.plotSizeX + 20) / ratio, false);
              chart.allowUpdate = false;
            }
          },
        },
      },
      title: {
        //text: title,
        text: "",
        alignt: "left",
      },
      tooltip: {
        useHTML: true,
        pointFormat: "<div><b>{point.province}</b></div>",
      },
      plotOptions: {
        series: {
          point: {
            events: {
              click: function () {
                const pointData = {
                  province: this.province,
                  data: this.data,
                  lat: this.lat,
                  lon: this.lon,
                  total: this.Total,
                };
                openMapModal(pointData);
              },
            },
          },
        },
      },
      xAxis: {
        crosshair: {
          zIndex: 5,
          dashStyle: "dot",
          snap: false,
          color: "gray",
        },
      },
      yAxis: {
        crosshair: {
          zIndex: 5,
          dashStyle: "dot",
          snap: false,
          color: "gray",
        },
      },
      legend: {
        enabled: false,
      },
      series: [
        {
          name: "Basemap",
          mapData: map,
          // borderColor: "#f0f0f0",
          // backgroundColor: "#D2D2D2",
          // nullColor: "#D2D2D2",
          borderWidth: 2,
          borderColor: "#f8fafc",
          backgroundColor: "#cbd5e1",
          nullColor: "#cbd5e1",
          showInLegend: false,
        },
        {
          name: "Separators",
          type: "mapline",
          data: h.geojson(map, "mapline"),
          color: "#101010",
          enableMouseTracking: false,
          showInLegend: false,
        },
        {
          cursor: "pointer",
          type: "mapbubble",
          name: "Province",
          data: data,
          //maxSize: "7%",
          maxSize: "8%",
          allowHTML: true,
          useHTML: true,
          nullColor: "#535353",
          marker: {
            //symbol: "url(https://upload.wikimedia.org/wikipedia/commons/b/b2/%EB%8D%95%EC%9D%B4%EB%91%98%EC%9D%B4.png)",
            symbol: "url(../../../Images/pin.png)",
            useHTML: true,
            style: {
              useHTML: true,
            },
          },
          //text: '<div><svg width="15px" height="15px" viewBox="0 0 256 256" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"><title>s</title><desc></desc><defs></defs><g id="black-icon-copy-6" stroke="none" stroke-width="1" fill="none" fill-rule="evenodd" stroke-linecap="round" stroke-linejoin="round"><g id="Download" transform="translate(8.000000, 8.000000)" stroke="#000000" stroke-width="16"><path d="M240.463235,188.294116 L240.463235,209.163945 C240.463235,226.444557 226.444557,240.463235 209.163945,240.463235 L31.7540284,240.463235 C14.4625072,240.463235 0.454738562,226.444557 0.454738562,209.163945 L0.454738562,188.294116" id="Shape"></path><path d="M120.458987,187.094073 L120.458987,0.454738562" id="Shape"></path><polyline id="Shape" points="196.825327 110.727733 120.458987 187.094073 44.0926471 110.727733"></polyline></g></g></svg></div>',
          borderColor: "#F8F8F8",
          states: {
            hover: {
              color: "#BADA55",
            },
          },
        },
      ],
    });
    //AddElementSvg();
    //console.log("loadMap Done");
  });

  document.getElementById("container-map").addEventListener("mouseout", function () {
    if (chart && chart.lab) {
      chart.lab.destroy();
      chart.lab = null;
    }
  });
};

const loadMapModal = (type) => {
  //console.log("loadMap Called");
  //$("#map-loader").show();
  //$("#map-popup-map").hide();
  // $("#map-title").text("");
  // var title = "";
  // if (type === "Branch") {
  //   title = "Export Current Activity by Branch";
  // } else {
  //   title = "Export Current Activity by Port";
  // }

  Highcharts.SVGRenderer.prototype.symbols.cross = function (x, y, w, h) {
    var returnArray = [];
    var startPoint = x - 5 * w;
    var endPoint = x + 5 * w;
    var dashWidth = w;
    var gapWidth = 0.5 * w;

    var yValWithOffset = y + w / 2; //account for marker radius when moving to the right Y position

    var currentPoint = startPoint;
    for (var currentPoint = startPoint; currentPoint <= endPoint; currentPoint += dashWidth + gapWidth /*jump forward one location*/) {
      returnArray.push("M", currentPoint, yValWithOffset, "L", currentPoint + dashWidth, yValWithOffset);
    }
    returnArray.push("z"); //end drawing
    return returnArray;
  };

  if (Highcharts.VMLRenderer) {
    Highcharts.VMLRenderer.prototype.symbols.cross = Highcharts.SVGRenderer.prototype.symbols.cross;
  }

  var h = Highcharts,
    map = h.maps["countries/id/id-all"],
    chart;

  //Highcharts.getJSON("/EMCS/GetMapData?type=" + type + "&user" + $("#Area").val(), function (json) {
  //$("#map-loader").hide();
  //$("#map-popup-map").show();
  //$("#map-title").text(title);
  var data = [];
  // json.data.forEach(function (p) {
  //   p.lat = p.Lat;
  //   p.lon = p.Lon;
  //   p.province = p.Province;
  //   p.data = p.Data;
  //   p.z = p.Total;
  //   data.push(p);
  // });
  //console.log(data);
  data = [
    {
      Lat: "-8.584429999999999",
      Lon: "120.689",
      Province: "NTT",
      Data: "- E.0512220244HW Ariliago<br>- E.2411220243HW Ariliago<br>- E.2311220242HW Ariliago<br>- E.2311220241HW Ariliago<br>- E.2111220240HW Ariliago<br>- E.1611220239HW Ariliago<br>- E.1511220238HW Ariliago<br>- E.1111220237HW Ariliago<br>- E.1011220236HW Ariliago<br>- E.0711220235HW Ariliago<br>- E.0411220234HW Ariliago<br>- E.2410220233HW Ariliago<br>- E.2010220231HW Ariliago<br>- E.1910220230HW Ariliago<br>- E.1810220228HW Ariliago<br>- E.1710220227HW Ariliago<br>- E.1410220226HW Ariliago<br>- E.1310220225HW Ariliago<br>- E.1210220224JP Asmat Awaluddin<br>- E.1210220223HW Ariliago<br>- E.1210220222HW Ariliago<br>- E.0610220218HW Ariliago<br>- E.0510220217HW Ariliago<br>- E.2909220213HW Ariliago<br>- E.2909220212HW Ariliago<br>- E.2209220211HW Ariliago<br>- E.2109220210HW Ariliago<br>- E.1309220207HW Ariliago<br>- E.0909220206HW Ariliago<br>- E.0909220205HW Ariliago<br>- E.0509220199HW Ariliago<br>- E.3003220066HW Ariliago<br>- E.2403220053HW Ariliago<br>- E.2103220046JP Asmat Awaluddin",
      Total: 0,
      lat: "-8.584429999999999",
      lon: "120.689",
      province: "NTT",
      data: "- E.0512220244HW Ariliago<br>- E.2411220243HW Ariliago<br>- E.2311220242HW Ariliago<br>- E.2311220241HW Ariliago<br>- E.2111220240HW Ariliago<br>- E.1611220239HW Ariliago<br>- E.1511220238HW Ariliago<br>- E.1111220237HW Ariliago<br>- E.1011220236HW Ariliago<br>- E.0711220235HW Ariliago<br>- E.0411220234HW Ariliago<br>- E.2410220233HW Ariliago<br>- E.2010220231HW Ariliago<br>- E.1910220230HW Ariliago<br>- E.1810220228HW Ariliago<br>- E.1710220227HW Ariliago<br>- E.1410220226HW Ariliago<br>- E.1310220225HW Ariliago<br>- E.1210220224JP Asmat Awaluddin<br>- E.1210220223HW Ariliago<br>- E.1210220222HW Ariliago<br>- E.0610220218HW Ariliago<br>- E.0510220217HW Ariliago<br>- E.2909220213HW Ariliago<br>- E.2909220212HW Ariliago<br>- E.2209220211HW Ariliago<br>- E.2109220210HW Ariliago<br>- E.1309220207HW Ariliago<br>- E.0909220206HW Ariliago<br>- E.0909220205HW Ariliago<br>- E.0509220199HW Ariliago<br>- E.3003220066HW Ariliago<br>- E.2403220053HW Ariliago<br>- E.2103220046JP Asmat Awaluddin",
      z: 0,
    },
    {
      Lat: "-7.88129",
      Lon: "112.616",
      Province: "Jawa Timur",
      Data: "- E.0310220215HW Ariliago<br>- E.2408220181HW Ariliago<br>- E.0706220134HW Ariliago<br>- E.0206220133HW Ariliago<br>- E.2505220128HW Ariliago<br>- E.1205220116HW Ariliago<br>- E.3103220075HW Ariliago<br>- E.2403220052HW Ariliago",
      Total: 0,
      lat: "-7.88129",
      lon: "112.616",
      province: "Jawa Timur",
      data: "- E.0310220215HW Ariliago<br>- E.2408220181HW Ariliago<br>- E.0706220134HW Ariliago<br>- E.0206220133HW Ariliago<br>- E.2505220128HW Ariliago<br>- E.1205220116HW Ariliago<br>- E.3103220075HW Ariliago<br>- E.2403220052HW Ariliago",
      z: 0,
    },
    {
      Lat: "-1.84217",
      Lon: "113.286",
      Province: "Kalimantan Tengah",
      Data: "- E.0909220204HW Ariliago<br>- E.0509220200HW Ariliago<br>- E.0509220198HW Ariliago<br>- E.0509220197HW Ariliago<br>- E.1108220146JP Asmat Awaluddin<br>- E.2305220127HW Ariliago<br>- E.0804220084JP Asmat Awaluddin<br>- E.2303220051HW Ariliago<br>- E.1603220041JP Asmat Awaluddin<br>- E.1301220006HW Ariliago<br>- E.0501220002HW Ariliago",
      Total: 0,
      lat: "-1.84217",
      lon: "113.286",
      province: "Kalimantan Tengah",
      data: "- E.0909220204HW Ariliago<br>- E.0509220200HW Ariliago<br>- E.0509220198HW Ariliago<br>- E.0509220197HW Ariliago<br>- E.1108220146JP Asmat Awaluddin<br>- E.2305220127HW Ariliago<br>- E.0804220084JP Asmat Awaluddin<br>- E.2303220051HW Ariliago<br>- E.1603220041JP Asmat Awaluddin<br>- E.1301220006HW Ariliago<br>- E.0501220002HW Ariliago",
      z: 0,
    },
    {
      Lat: "-7.2901",
      Lon: "109.896",
      Province: "Jawa Tengah",
      Data: "- E.0909220203HW Ariliago<br>- E.1108220145JP Asmat Awaluddin<br>- E.0808220143HW Ariliago<br>- E.0308220141JP Asmat Awaluddin<br>- E.3005220131HW Ariliago<br>- E.3005220130HW Ariliago<br>- E.0704220083HW Ariliago<br>- E.0704220082HW Ariliago<br>- E.1102220022HW Ariliago<br>- E.1002220020HW Ariliago",
      Total: 0,
      lat: "-7.2901",
      lon: "109.896",
      province: "Jawa Tengah",
      data: "- E.0909220203HW Ariliago<br>- E.1108220145JP Asmat Awaluddin<br>- E.0808220143HW Ariliago<br>- E.0308220141JP Asmat Awaluddin<br>- E.3005220131HW Ariliago<br>- E.3005220130HW Ariliago<br>- E.0704220083HW Ariliago<br>- E.0704220082HW Ariliago<br>- E.1102220022HW Ariliago<br>- E.1002220020HW Ariliago",
      z: 0,
    },
    {
      Lat: "-6.90763",
      Lon: "107.638",
      Province: "Jawa Barat",
      Data: "- E.0809220202HW Ariliago<br>- E.0709220201HW Ariliago<br>- E.0209220196HW Ariliago<br>- E.2308220180HW Ariliago<br>- E.1908220177HW Ariliago<br>- E.2706220138HW Ariliago<br>- E.2106220137HW Ariliago<br>- E.1306220135HW Ariliago<br>- E.0206220132HW Ariliago<br>- E.3005220129HW Ariliago<br>- E.2005220126HW Ariliago<br>- E.2005220125HW Ariliago<br>- E.2005220124HW Ariliago<br>- E.2005220122HW Ariliago<br>- E.1905220121HW Ariliago<br>- E.1705220120HW Ariliago<br>- E.1705220119HW Ariliago<br>- E.1305220118HW Ariliago<br>- E.1305220117HW Ariliago<br>- E.1205220115HW Ariliago<br>- E.1205220114HW Ariliago<br>- E.1205220113HW Ariliago<br>- E.1105220112HW Ariliago<br>- E.0905220111HW Ariliago<br>- E.2804220109HW Ariliago<br>- E.2804220107HW Ariliago<br>- E.2804220106HW Ariliago<br>- E.2604220105HW Ariliago<br>- E.2604220104HW Ariliago<br>- E.2504220103HW Ariliago<br>- E.2104220100HW Ariliago<br>- E.2104220098HW Ariliago<br>- E.2004220097HW Ariliago<br>- E.2004220096HW Ariliago<br>- E.1904220095HW Ariliago<br>- E.1804220094HW Ariliago<br>- E.1304220093HW Ariliago<br>- E.1204220092HW Ariliago<br>- E.1104220085HW Ariliago<br>- E.0704220081HW Ariliago<br>- E.0704220080HW Ariliago<br>- E.0704220079HW Ariliago<br>- E.0604220078HW Ariliago<br>- E.0104220077HW Ariliago<br>- E.0104220076HW Ariliago<br>- E.3103220067HW Ariliago<br>- E.2903220061HW Ariliago<br>- E.2903220060HW Ariliago<br>- E.2903220059HW Ariliago<br>- E.2503220054HW Ariliago<br>- E.2303220048HW Ariliago<br>- E.1603220042HW Ariliago<br>- E.1603220040HW Ariliago<br>- E.0402220011HW Ariliago<br>- E.0402220010HW Ariliago",
      Total: 0,
      lat: "-6.90763",
      lon: "107.638",
      province: "Jawa Barat",
      data: "- E.0809220202HW Ariliago<br>- E.0709220201HW Ariliago<br>- E.0209220196HW Ariliago<br>- E.2308220180HW Ariliago<br>- E.1908220177HW Ariliago<br>- E.2706220138HW Ariliago<br>- E.2106220137HW Ariliago<br>- E.1306220135HW Ariliago<br>- E.0206220132HW Ariliago<br>- E.3005220129HW Ariliago<br>- E.2005220126HW Ariliago<br>- E.2005220125HW Ariliago<br>- E.2005220124HW Ariliago<br>- E.2005220122HW Ariliago<br>- E.1905220121HW Ariliago<br>- E.1705220120HW Ariliago<br>- E.1705220119HW Ariliago<br>- E.1305220118HW Ariliago<br>- E.1305220117HW Ariliago<br>- E.1205220115HW Ariliago<br>- E.1205220114HW Ariliago<br>- E.1205220113HW Ariliago<br>- E.1105220112HW Ariliago<br>- E.0905220111HW Ariliago<br>- E.2804220109HW Ariliago<br>- E.2804220107HW Ariliago<br>- E.2804220106HW Ariliago<br>- E.2604220105HW Ariliago<br>- E.2604220104HW Ariliago<br>- E.2504220103HW Ariliago<br>- E.2104220100HW Ariliago<br>- E.2104220098HW Ariliago<br>- E.2004220097HW Ariliago<br>- E.2004220096HW Ariliago<br>- E.1904220095HW Ariliago<br>- E.1804220094HW Ariliago<br>- E.1304220093HW Ariliago<br>- E.1204220092HW Ariliago<br>- E.1104220085HW Ariliago<br>- E.0704220081HW Ariliago<br>- E.0704220080HW Ariliago<br>- E.0704220079HW Ariliago<br>- E.0604220078HW Ariliago<br>- E.0104220077HW Ariliago<br>- E.0104220076HW Ariliago<br>- E.3103220067HW Ariliago<br>- E.2903220061HW Ariliago<br>- E.2903220060HW Ariliago<br>- E.2903220059HW Ariliago<br>- E.2503220054HW Ariliago<br>- E.2303220048HW Ariliago<br>- E.1603220042HW Ariliago<br>- E.1603220040HW Ariliago<br>- E.0402220011HW Ariliago<br>- E.0402220010HW Ariliago",
      z: 0,
    },
    {
      Lat: "-6.25794",
      Lon: "106.167",
      Province: "Banten",
      Data: "- E.2908220189HW Ariliago",
      Total: 0,
      lat: "-6.25794",
      lon: "106.167",
      province: "Banten",
      data: "- E.2908220189HW Ariliago",
      z: 0,
    },
    {
      Lat: "-1.65497",
      Lon: "102.823",
      Province: "Jambi",
      Data: "- E.2508220188HW Ariliago<br>- E.2508220187HW Ariliago<br>- E.2508220186HW Ariliago<br>- E.2508220185HW Ariliago",
      Total: 0,
      lat: "-1.65497",
      lon: "102.823",
      province: "Jambi",
      data: "- E.2508220188HW Ariliago<br>- E.2508220187HW Ariliago<br>- E.2508220186HW Ariliago<br>- E.2508220185HW Ariliago",
      z: 0,
    },
    {
      Lat: "0.853039",
      Lon: "124.446",
      Province: "Sulawesi Utara",
      Data: "- E.2408220184HW Ariliago",
      Total: 0,
      lat: "0.853039",
      lon: "124.446",
      province: "Sulawesi Utara",
      data: "- E.2408220184HW Ariliago",
      z: 0,
    },
    {
      Lat: "1.28915",
      Lon: "116.354",
      Province: "Kalimantan Timur",
      Data: "- E.2208220178HW Ariliago",
      Total: 0,
      lat: "1.28915",
      lon: "116.354",
      province: "Kalimantan Timur",
      data: "- E.2208220178HW Ariliago",
      z: 0,
    },
    {
      Lat: "-6.22462",
      Lon: "106.837",
      Province: "Jakarta Raya",
      Data: "- E.3103220074HW Ariliago<br>- E.3103220073HW Ariliago<br>- E.3103220072HW Ariliago<br>- E.3103220071HW Ariliago<br>- E.3103220070HW Ariliago<br>- E.3103220069HW Ariliago<br>- E.3103220068HW Ariliago<br>- E.3003220064HW Ariliago<br>- E.2903220062HW Ariliago<br>- E.1703220043HW Ariliago<br>- E.1603220038HW Ariliago<br>- E.2402220031HW Ariliago<br>- E.2202220030HW Ariliago<br>- E.2202220029HW Ariliago<br>- E.0702220013HW Ariliago",
      Total: 0,
      lat: "-6.22462",
      lon: "106.837",
      province: "Jakarta Raya",
      data: "- E.3103220074HW Ariliago<br>- E.3103220073HW Ariliago<br>- E.3103220072HW Ariliago<br>- E.3103220071HW Ariliago<br>- E.3103220070HW Ariliago<br>- E.3103220069HW Ariliago<br>- E.3103220068HW Ariliago<br>- E.3003220064HW Ariliago<br>- E.2903220062HW Ariliago<br>- E.1703220043HW Ariliago<br>- E.1603220038HW Ariliago<br>- E.2402220031HW Ariliago<br>- E.2202220030HW Ariliago<br>- E.2202220029HW Ariliago<br>- E.0702220013HW Ariliago",
      z: 0,
    },
  ];
  chart = Highcharts.mapChart("map-popup-map", {
    credits: false,
    chart: {
      map: "countries/id/id-all",
      backgroundColor: "none",
      //height: "56.25%",
      style: {
        fontFamily: "Arial",
      },
      events: {
        render: function () {
          const chart = this,
            group = chart.series[0].group,
            bBox = group.getBBox(),
            ratio = bBox.width / bBox.height;

          if (!chart.allowUpdate) {
            chart.allowUpdate = true;
            chart.setSize(null, (chart.plotSizeX + 20) / ratio, false);
            chart.allowUpdate = false;
          }
        },
      },
    },
    title: {
      //text: title,
      text: "",
      alignt: "left",
    },
    tooltip: {
      useHTML: true,
      pointFormat: "<div><b>{point.province}</b></div>",
    },
    plotOptions: {
      series: {
        point: {
          events: {
            click: function () {
              const pointData = {
                province: this.province,
                data: this.data,
                lat: this.lat,
                lon: this.lon,
                total: this.Total,
              };
              openMapModal(pointData);
            },
          },
        },
      },
    },
    xAxis: {
      crosshair: {
        zIndex: 5,
        dashStyle: "dot",
        snap: false,
        color: "gray",
      },
    },
    yAxis: {
      crosshair: {
        zIndex: 5,
        dashStyle: "dot",
        snap: false,
        color: "gray",
      },
    },
    legend: {
      enabled: false,
    },
    series: [
      {
        name: "Basemap",
        mapData: map,
        // borderColor: "#f0f0f0",
        // backgroundColor: "#D2D2D2",
        // nullColor: "#D2D2D2",
        borderWidth: 2,
        borderColor: "#f8fafc",
        backgroundColor: "#cbd5e1",
        nullColor: "#cbd5e1",
        showInLegend: false,
      },
      {
        name: "Separators",
        type: "mapline",
        data: h.geojson(map, "mapline"),
        color: "#101010",
        enableMouseTracking: false,
        showInLegend: false,
      },
      {
        cursor: "pointer",
        type: "mapbubble",
        name: "Province",
        data: data,
        //maxSize: "7%",
        maxSize: "8%",
        allowHTML: true,
        useHTML: true,
        nullColor: "#535353",
        marker: {
          //symbol: "url(https://upload.wikimedia.org/wikipedia/commons/b/b2/%EB%8D%95%EC%9D%B4%EB%91%98%EC%9D%B4.png)",
          symbol: "url(../../../Images/pin.png)",
          useHTML: true,
          style: {
            useHTML: true,
          },
        },
        //text: '<div><svg width="15px" height="15px" viewBox="0 0 256 256" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"><title>s</title><desc></desc><defs></defs><g id="black-icon-copy-6" stroke="none" stroke-width="1" fill="none" fill-rule="evenodd" stroke-linecap="round" stroke-linejoin="round"><g id="Download" transform="translate(8.000000, 8.000000)" stroke="#000000" stroke-width="16"><path d="M240.463235,188.294116 L240.463235,209.163945 C240.463235,226.444557 226.444557,240.463235 209.163945,240.463235 L31.7540284,240.463235 C14.4625072,240.463235 0.454738562,226.444557 0.454738562,209.163945 L0.454738562,188.294116" id="Shape"></path><path d="M120.458987,187.094073 L120.458987,0.454738562" id="Shape"></path><polyline id="Shape" points="196.825327 110.727733 120.458987 187.094073 44.0926471 110.727733"></polyline></g></g></svg></div>',
        borderColor: "#F8F8F8",
        states: {
          hover: {
            color: "#BADA55",
          },
        },
      },
    ],
  });
  //AddElementSvg();
  //console.log("loadMap Done");
  //});

  document.getElementById("map-popup-map").addEventListener("mouseout", function () {
    if (chart && chart.lab) {
      chart.lab.destroy();
      chart.lab = null;
    }
  });
};

const loadOutstanding = (id, rows) => {
  var outstandingData = [];
  $.ajax({
    url: "/emcs/Oustanding",
    //url: "/emcs/Oustanding?searchId=29&searchId2=1&searchCode=&_=1668387433729",
    //async: false,
    data: {
      // searchId: id,
      // searchId2: rows,
      searchCode: $("#Area").val(),
      searchId: 29,
      searchId2: 1,
    },
  }).done(function (data) {
    var data = {
      branch: [
        {
          No: "E.0401220004BM",
          Branch: "Martabe",
          PortOfLoading: "IDCGK - Soekarno Hatta",
          PortOfDestination: "BMI - Bloomington, IL",
          ETD: "/Date(1642352400000)/",
          ETA: "/Date(1642784400000)/",
          ViewByUser: "Finish",
        },
        {
          No: "E.0501220007HW",
          Branch: "Batam",
          PortOfLoading: "BTH - Batam, Indonesia",
          PortOfDestination: "SGSIN - Singapore",
          ETD: null,
          ETA: null,
          ViewByUser: "Waiting for pickup goods",
        },
        {
          No: "E.0701220013BM",
          Branch: "Pekanbaru",
          PortOfLoading: "IDCGK - Soekarno Hatta",
          PortOfDestination: "ORD - Chicago, IL - O'Hare",
          ETD: "/Date(1642957200000)/",
          ETA: "/Date(1643389200000)/",
          ViewByUser: "Finish",
        },
        {
          No: "E.1101220018SM",
          Branch: "Central Yard Cakung - Berikat",
          PortOfLoading: "IDTPP - Jakarta (Tanjung Priok)",
          PortOfDestination: "SGSIN - Singapore",
          ETD: null,
          ETA: null,
          ViewByUser: "Waiting for Pickup Goods",
        },
        {
          No: "E.1201220022HG",
          Branch: "Batu Hijau",
          PortOfLoading: "IDCGK - Soekarno Hatta",
          PortOfDestination: "LAF - Lafayette, IN",
          ETD: "/Date(1643302800000)/",
          ETA: "/Date(1643734800000)/",
          ViewByUser: "Approve Revise Cipl",
        },
      ],
      port: [
        {
          No: "000000-000660-20220112-001089",
          Branch: "Balikpapan",
          PortOfLoading: "IDTPP - Jakarta (Tanjung Priok",
          PortOfDestination: "AEJEA - Jebel Ali",
          ETD: "/Date(1642438800000)/",
          ETA: "/Date(1644944400000)/",
          ViewByUser: "Finish",
        },
        {
          No: "000000-000660-20220115-001090",
          Branch: "Samarinda",
          PortOfLoading: "IDTPP - Jakarta (Tanjung Priok",
          PortOfDestination: "AEJEA - Jebel Ali",
          ETD: "/Date(1642611600000)/",
          ETA: "/Date(1644598800000)/",
          ViewByUser: "Finish",
        },
        {
          No: "000000-002582-20220117-616527",
          Branch: "Rantau",
          PortOfLoading: "IDCGK - Soekarno Hatta",
          PortOfDestination: "SJC - San Jose, CA",
          ETD: "/Date(1643907600000)/",
          ETA: "/Date(1644339600000)/",
          ViewByUser: "Finish",
        },
        {
          No: "000000-002582-20220215-616933",
          Branch: "CRC Balikpapan",
          PortOfLoading: "IDCGK - Soekarno Hatta",
          PortOfDestination: "MKE - Milwaukee, WI",
          ETD: "/Date(1645549200000)/",
          ETA: "/Date(1645981200000)/",
          ViewByUser: "Finish",
        },
        {
          No: "000000-002582-20220406-617457",
          Branch: "Tabang",
          PortOfLoading: "IDCGK - Soekarno Hatta",
          PortOfDestination: "ORD - Chicago, IL - O'Hare",
          ETD: "/Date(1649178000000)/",
          ETA: "/Date(1652202000000)/",
          ViewByUser: "Finish",
        },
      ],
    };
    console.log(data);
    //   var branch = data.branch;
    //   if (branch.length > 0) {
    //     $("#tbody-branch tr").remove();
    //     $.each(branch, function (index, element) {
    //       var html1 = "<tr>";
    //       html1 += "<td></td>";
    //       html1 += "<td>" + element.No + "</td>";
    //       html1 += "<td>" + element.Branch + "</td>";
    //       html1 += "<td>" + element.PortOfLoading.split("-")[1] + "</td>";
    //       html1 += "<td>" + element.PortOfDestination.split("-")[1] + "</td>";
    //       if (element.ETD == null) {
    //         element.ETD == "-";
    //         html1 += "<td>-</td>";
    //       } else {
    //         html1 += "<td>" + moment(element.ETD).format("DD MMM YY") + "</td>";
    //       }
    //       if (element.ETA == null) {
    //         element.ETA == "-";
    //         html1 += "<td>-</td>";
    //       } else {
    //         html1 += "<td>" + moment(element.ETA).format("DD MMM YY") + "</td>";
    //       }

    //       html1 += "<td>" + element.ViewByUser + "</td>";
    //       html1 += "</tr>";
    //       $("#tbody-branch").append(html1);

    //       outstandingData.push(element);
    //     });
    //   } else {
    //     $("#tbody-branch tr").remove();
    //     var html2 = '<tr><td colspan="7">No Data Found</td></tr>';
    //     $("#tbody-branch").append(html2);
    //   }

    //   var port = data.port;
    //   if (port.length > 0) {
    //     $("#tbody-port tr").remove();
    //     $.each(port, function (index, element) {
    //       var html3 = "<tr>";
    //       html3 += "<td>" + element.No + "</td>";
    //       html3 += "<td>" + element.Branch + "</td>";
    //       html3 += "<td>" + element.PortOfLoading.split("-")[1] + "</td>";
    //       html3 += "<td>" + element.PortOfDestination.split("-")[1] + "</td>";
    //       if (element.ETD == null) {
    //         html3 += "<td>-</td>";
    //       } else {
    //         html3 += "<td>" + moment(element.ETD).format("DD MMM YY") + "</td>";
    //       }
    //       if (element.ETA == null) {
    //       } else {
    //         html3 += "<td>" + moment(element.ETA).format("DD MMM YY") + "</td>";
    //       }

    //       html3 += "<td>" + element.ViewByUser + "</td>";
    //       html3 += "</tr>";
    //       $("#tbody-port").append(html3);
    //     });
    //   } else {
    //     $("#tbody-port tr").remove();
    //     var html4 = '<tr><td colspan="7">No Data Found</td></tr>';
    //     $("#tbody-port").append(html4);
    //   }
  });
  // success: function (data) {
  //   var branch = data.branch;
  //   if (branch.length > 0) {
  //     $("#tbody-branch tr").remove();
  //     $.each(branch, function (index, element) {
  //       var html1 = "<tr>";
  //       html1 += "<td></td>";
  //       html1 += "<td>" + element.No + "</td>";
  //       html1 += "<td>" + element.Branch + "</td>";
  //       html1 += "<td>" + element.PortOfLoading.split("-")[1] + "</td>";
  //       html1 += "<td>" + element.PortOfDestination.split("-")[1] + "</td>";
  //       if (element.ETD == null) {
  //         element.ETD == "-";
  //         html1 += "<td>-</td>";
  //       } else {
  //         html1 += "<td>" + moment(element.ETD).format("DD MMM YY") + "</td>";
  //       }
  //       if (element.ETA == null) {
  //         element.ETA == "-";
  //         html1 += "<td>-</td>";
  //       } else {
  //         html1 += "<td>" + moment(element.ETA).format("DD MMM YY") + "</td>";
  //       }

  //       html1 += "<td>" + element.ViewByUser + "</td>";
  //       html1 += "</tr>";
  //       $("#tbody-branch").append(html1);

  //       outstandingData.push(element);
  //     });
  //   } else {
  //     $("#tbody-branch tr").remove();
  //     var html2 = '<tr><td colspan="7">No Data Found</td></tr>';
  //     $("#tbody-branch").append(html2);
  //   }

  //   var port = data.port;
  //   if (port.length > 0) {
  //     $("#tbody-port tr").remove();
  //     $.each(port, function (index, element) {
  //       var html3 = "<tr>";
  //       html3 += "<td>" + element.No + "</td>";
  //       html3 += "<td>" + element.Branch + "</td>";
  //       html3 += "<td>" + element.PortOfLoading.split("-")[1] + "</td>";
  //       html3 += "<td>" + element.PortOfDestination.split("-")[1] + "</td>";
  //       if (element.ETD == null) {
  //         html3 += "<td>-</td>";
  //       } else {
  //         html3 += "<td>" + moment(element.ETD).format("DD MMM YY") + "</td>";
  //       }
  //       if (element.ETA == null) {
  //       } else {
  //         html3 += "<td>" + moment(element.ETA).format("DD MMM YY") + "</td>";
  //       }

  //       html3 += "<td>" + element.ViewByUser + "</td>";
  //       html3 += "</tr>";
  //       $("#tbody-port").append(html3);
  //     });
  //   } else {
  //     $("#tbody-port tr").remove();
  //     var html4 = '<tr><td colspan="7">No Data Found</td></tr>';
  //     $("#tbody-port").append(html4);
  //   }
  // },
  return outstandingData;
};

const loadOutstandingSingle = (id, rows) => {
  var outstandingData = [];
  $.ajax({
    url: "/emcs/Oustanding",
    async: false,
    data: {
      searchId: id,
      searchId2: rows,
      searchCode: $("#Area").val(),
    },
    success: function (data) {
      var data = {
        branch: [
          {
            No: "E.0401220004BM",
            Branch: "Martabe",
            PortOfLoading: "IDCGK - Soekarno Hatta",
            PortOfDestination: "BMI - Bloomington, IL",
            ETD: "/Date(1642352400000)/",
            ETA: "/Date(1642784400000)/",
            ViewByUser: "Finish",
          },
          {
            No: "E.0501220007HW",
            Branch: "Batam",
            PortOfLoading: "BTH - Batam, Indonesia",
            PortOfDestination: "SGSIN - Singapore",
            ETD: null,
            ETA: null,
            ViewByUser: "Waiting for pickup goods",
          },
          {
            No: "E.0701220013BM",
            Branch: "Pekanbaru",
            PortOfLoading: "IDCGK - Soekarno Hatta",
            PortOfDestination: "ORD - Chicago, IL - O'Hare",
            ETD: "/Date(1642957200000)/",
            ETA: "/Date(1643389200000)/",
            ViewByUser: "Finish",
          },
          {
            No: "E.1101220018SM",
            Branch: "Central Yard Cakung - Berikat",
            PortOfLoading: "IDTPP - Jakarta (Tanjung Priok)",
            PortOfDestination: "SGSIN - Singapore",
            ETD: null,
            ETA: null,
            ViewByUser: "Waiting for Pickup Goods",
          },
          {
            No: "E.1201220022HG",
            Branch: "Batu Hijau",
            PortOfLoading: "IDCGK - Soekarno Hatta",
            PortOfDestination: "LAF - Lafayette, IN",
            ETD: "/Date(1643302800000)/",
            ETA: "/Date(1643734800000)/",
            ViewByUser: "Approve Revise Cipl",
          },
          {
            No: "E.0401220004BM",
            Branch: "Martabe",
            PortOfLoading: "IDCGK - Soekarno Hatta",
            PortOfDestination: "BMI - Bloomington, IL",
            ETD: "/Date(1642352400000)/",
            ETA: "/Date(1642784400000)/",
            ViewByUser: "Finish",
          },
          {
            No: "E.0501220007HW",
            Branch: "Batam",
            PortOfLoading: "BTH - Batam, Indonesia",
            PortOfDestination: "SGSIN - Singapore",
            ETD: null,
            ETA: null,
            ViewByUser: "Waiting for pickup goods",
          },
          {
            No: "E.0701220013BM",
            Branch: "Pekanbaru",
            PortOfLoading: "IDCGK - Soekarno Hatta",
            PortOfDestination: "ORD - Chicago, IL - O'Hare",
            ETD: "/Date(1642957200000)/",
            ETA: "/Date(1643389200000)/",
            ViewByUser: "Finish",
          },
          {
            No: "E.1101220018SM",
            Branch: "Central Yard Cakung - Berikat",
            PortOfLoading: "IDTPP - Jakarta (Tanjung Priok)",
            PortOfDestination: "SGSIN - Singapore",
            ETD: null,
            ETA: null,
            ViewByUser: "Waiting for Pickup Goods",
          },
          {
            No: "E.1201220022HG",
            Branch: "Batu Hijau",
            PortOfLoading: "IDCGK - Soekarno Hatta",
            PortOfDestination: "LAF - Lafayette, IN",
            ETD: "/Date(1643302800000)/",
            ETA: "/Date(1643734800000)/",
            ViewByUser: "Approve Revise Cipl",
          },
          {
            No: "E.0401220004BM",
            Branch: "Martabe",
            PortOfLoading: "IDCGK - Soekarno Hatta",
            PortOfDestination: "BMI - Bloomington, IL",
            ETD: "/Date(1642352400000)/",
            ETA: "/Date(1642784400000)/",
            ViewByUser: "Finish",
          },
          {
            No: "E.0501220007HW",
            Branch: "Batam",
            PortOfLoading: "BTH - Batam, Indonesia",
            PortOfDestination: "SGSIN - Singapore",
            ETD: null,
            ETA: null,
            ViewByUser: "Waiting for pickup goods",
          },
          {
            No: "E.0701220013BM",
            Branch: "Pekanbaru",
            PortOfLoading: "IDCGK - Soekarno Hatta",
            PortOfDestination: "ORD - Chicago, IL - O'Hare",
            ETD: "/Date(1642957200000)/",
            ETA: "/Date(1643389200000)/",
            ViewByUser: "Finish",
          },
          {
            No: "E.1101220018SM",
            Branch: "Central Yard Cakung - Berikat",
            PortOfLoading: "IDTPP - Jakarta (Tanjung Priok)",
            PortOfDestination: "SGSIN - Singapore",
            ETD: null,
            ETA: null,
            ViewByUser: "Waiting for Pickup Goods",
          },
          {
            No: "E.1201220022HG",
            Branch: "Batu Hijau",
            PortOfLoading: "IDCGK - Soekarno Hatta",
            PortOfDestination: "LAF - Lafayette, IN",
            ETD: "/Date(1643302800000)/",
            ETA: "/Date(1643734800000)/",
            ViewByUser: "Approve Revise Cipl",
          },
        ],
        port: [
          {
            No: "000000-000660-20220112-001089",
            Branch: "Balikpapan",
            PortOfLoading: "IDTPP - Jakarta (Tanjung Priok",
            PortOfDestination: "AEJEA - Jebel Ali",
            ETD: "/Date(1642438800000)/",
            ETA: "/Date(1644944400000)/",
            ViewByUser: "Finish",
          },
          {
            No: "000000-000660-20220115-001090",
            Branch: "Samarinda",
            PortOfLoading: "IDTPP - Jakarta (Tanjung Priok",
            PortOfDestination: "AEJEA - Jebel Ali",
            ETD: "/Date(1642611600000)/",
            ETA: "/Date(1644598800000)/",
            ViewByUser: "Finish",
          },
          {
            No: "000000-002582-20220117-616527",
            Branch: "Rantau",
            PortOfLoading: "IDCGK - Soekarno Hatta",
            PortOfDestination: "SJC - San Jose, CA",
            ETD: "/Date(1643907600000)/",
            ETA: "/Date(1644339600000)/",
            ViewByUser: "Finish",
          },
          {
            No: "000000-002582-20220215-616933",
            Branch: "CRC Balikpapan",
            PortOfLoading: "IDCGK - Soekarno Hatta",
            PortOfDestination: "MKE - Milwaukee, WI",
            ETD: "/Date(1645549200000)/",
            ETA: "/Date(1645981200000)/",
            ViewByUser: "Finish",
          },
          {
            No: "000000-002582-20220406-617457",
            Branch: "Tabang",
            PortOfLoading: "IDCGK - Soekarno Hatta",
            PortOfDestination: "ORD - Chicago, IL - O'Hare",
            ETD: "/Date(1649178000000)/",
            ETA: "/Date(1652202000000)/",
            ViewByUser: "Finish",
          },
        ],
      };
      var branch = data.branch;
      var port = data.port;
      if ($("#map-category").val() === "Branch") {
        if (branch.length > 0) {
          $.each(branch, function (index, element) {
            outstandingData.push(element);
          });
        }
      } else {
        if (port.length > 0) {
          $.each(port, function (index, element) {
            outstandingData.push(element);
          });
        }
      }
    },
  });
  return outstandingData;
};

const loadTotalDataCard = () => {
  //console.log("loadTotalDataCard Called");
  $(".total-cards .loading-spinner").show();
  $(".total-cards .total-card-counter").hide();
  $.ajax({
    url: myApp.fullPath + "/EMCS/GetContent5",
    type: "GET",
    dataType: "json",
  }).done(function (resp) {
    let problem = resp.problem;
    let outstandingExport = resp.outstandingExport;
    let viewer = resp.viewer;
    $(".total-problem").find("total-card-counter").attr("data-count", problem);
    $(".total-visit").find(".total-card-counter").attr("data-count", viewer);
    $(".total-outstanding").find(".total-card-counter").attr("data-count", outstandingExport);
    $(".total-cards .loading-spinner").hide();
    $(".total-cards .total-card-counter").show();
    totalCardCounter();
    //console.log("loadTotalDataCard Done");
  });
};

const totalCardCounter = () => {
  $(".total-card-counter").each(function () {
    var $this = $(this);
    var countTo = $this.attr("data-count");
    $({
      countNum: $this.text(),
    }).animate(
      {
        countNum: countTo,
      },
      {
        duration: 1500,
        easing: "linear",
        step: function () {
          $this.text(Math.floor(this.countNum));
        },
        complete: function () {
          $this.text(this.countNum);
        },
      }
    );
  });
};

const loadShipment = () => {
  //console.log("loadShipment Called");
  $.ajax({
    url: "/emcs/Shipment",
    async: true,
    data: {
      searchCode: $("#Area").val(),
    },
  }).done(function (data) {
    //console.log(data);
    var parts = data[3].Total;
    var machine = data[1].Total;
    var engine = data[0].Total;
    var forklift = data[2].Total;
    $("#cat-parts, #cat-machine, #cat-engine, #cat-forklift").html(null);
    circliful.newCircle({
      percent: parts,
      id: "cat-parts",
      type: "simple",
      animationStep: 3,
      strokeLinecap: "round",
      backgroundCircleWidth: 6,
      foregroundCircleWidth: 10,
    });
    circliful.newCircle({
      percent: machine,
      id: "cat-machine",
      type: "simple",
      animationStep: 3,
      strokeLinecap: "round",
      backgroundCircleWidth: 6,
      foregroundCircleWidth: 10,
    });
    circliful.newCircle({
      percent: engine,
      id: "cat-engine",
      type: "simple",
      animationStep: 3,
      strokeLinecap: "round",
      backgroundCircleWidth: 6,
      foregroundCircleWidth: 10,
    });
    circliful.newCircle({
      percent: forklift,
      id: "cat-forklift",
      type: "simple",
      animationStep: 3,
      strokeLinecap: "round",
      backgroundCircleWidth: 6,
      foregroundCircleWidth: 10,
    });
    //console.log("loadShipment Done");
  });
};

const openMapModal = (data) => {
  $("#map-popup-province").html(data.province);
  let map_img = "";
  if (data.province == "Jakarta Raya") {
    map_img = '<img src="/Images/EMCS/Dashboard/map-jakarta-raya.png" />';
  } else if (data.province == "Jawa Barat") {
    map_img = '<img src="/Images/EMCS/Dashboard/map-jawa-barat.png" />';
  } else if (data.province == "Jawa Tengah") {
    map_img = '<img src="/Images/EMCS/Dashboard/map-jawa-tengah.png" />';
  } else if (data.province == "Jawa Timur") {
    map_img = '<img src="/Images/EMCS/Dashboard/map-jawa-timur.png" />';
  } else if (data.province == "Kalimantan Tengah") {
    map_img = '<img src="/Images/EMCS/Dashboard/map-kalimantan-tengah.png" />';
  } else if (data.province == "NTT") {
    map_img = '<img src="/Images/EMCS/Dashboard/map-ntt.png" />';
  }
  $(".map-popup-image").html(map_img);
  const info = data.data;
  const infoArray = info.split("<br>");
  $(".map-popup-data--table-data").empty();
  for (let i = 0; i < infoArray.length; i++) {
    const str = infoArray[i].replace("- ", "");
    const cipl = str.slice(0, 15);
    const pic = str.slice(15);
    const branch = "Nama Cabang";
    $(".map-popup-data--table-data").append("<tr><td>" + cipl + "</td><td>" + pic + "</td><td>" + branch + "</td></tr>");
  }
  $(".map-popup").toggleClass("open");
};

const loadExportCycle = () => {
  //console.log("loadExportCycle Called");
  $("#cycle-steps .loading-spinner").show();
  var dateNow = new Date();
  var startDate = moment(dateNow).format("YYYY-MM-DD");
  var endDate = dateNow.getFullYear() + "-01-01";

  $.ajax({
    url: "RAchievementListPage?StartDate=" + startDate + "&EndDate=" + endDate,
    type: "get",
    dataType: "json",
    cache: false,
  }).done(function (data) {
    //var object = data.Data.result;
    // Below object is for demo only. Use above in production.
    var object = [
      { Cycle: "CIPL Approved", Target: "2 Days", Actual: "1.0 Days", Achievement: "100.0", TotAchievement: 0 },
      { Cycle: "Pickup Goods", Target: "2 Days", Actual: "1.2 Days", Achievement: "100.0", TotAchievement: 0 },
      { Cycle: "PEB Approved", Target: "3 Days", Actual: "1.5 Days", Achievement: "100.0", TotAchievement: 0 },
      { Cycle: "Shipped on Board", Target: "2 Days", Actual: "0.5 Days", Achievement: "100.0", TotAchievement: 0 },
    ];
    //console.log(object);
    $("#cycle-steps .loading-spinner").hide();
    if (object.length > 0) {
      let i = 1;
      let array = [];
      $.each(object, function (index, element) {
        let actual = element.Actual.split(" ");
        let target = element.Target.split(" ");
        let html =
          "<div id='step-bar-" +
          i +
          "' class='step-bar'>" +
          '<div class="flex justify-between text-sm text-gray-600 dark:text-white mb-1">' +
          "<div>" +
          element.Cycle +
          "</div>" +
          "<div>" +
          actual[0] +
          "/" +
          target[0] +
          "</div>" +
          "</div>" +
          '<div class="step-bar-bg">' +
          '<div id="step-bar-progress-' +
          i +
          '" class="step-bar-progress color-' +
          i +
          '"></div>' +
          "</div>" +
          "</div>";
        //array.push(html);
        $("#cycle-steps").append(html);
        $("#step-bar-progress-" + i).animate(
          {
            width: (actual[0] / target[0]) * 100 + "%",
          },
          1000
        );
        i++;
      });
      //console.log("loadExportCycle Done");
    }
  });
};

const loadTrendExport = (startYear, endYear) => {
  $.ajax({
    url: "getTrendExport?startYear=" + startYear + "&endYear=" + endYear,
    type: "get",
    dataType: "json",
  }).done(function (data) {
    console.log(data);
    $("#trend-export").append('<div id="container_trend">');

    var categoriesYearly = [];
    var dataTotExport = [];
    var dataTotalPeb = [];

    $.each(data, function (i, element) {
      categoriesYearly.push(element.Year);
      dataTotExport.push(element.TotalExport);
      dataTotalPeb.push(element.TotalPEB);
    });

    var trendExportChart = Highcharts.chart("container_trend", {
      chart: {
        type: "column",
        height: 66 + "%",
      },
      title: {
        text: "Trend Export",
      },
      xAxis: {
        categories: categoriesYearly,
      },
      yAxis: [
        {
          min: 0,
          title: {
            text: "Total Export Value",
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
        shadow: false,
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
          data: dataTotExport,
          tooltip: {
            valuePrefix: "$",
            valueSuffix: " M",
          },
          pointPadding: 0.3,
          pointPlacement: -0.2,
        },
        {
          name: "Total PEB",
          color: "#000000",
          data: dataTotalPeb,
          pointPadding: 0.3,
          pointPlacement: 0.2,
          yAxis: 1,
        },
      ],
    });
    trendExportChart.reflow();
    $(".highcharts-xaxis-labels text").click(function () {
      trendExportChart(this.innerHTML, this.innerHTML);
    });
  });
};

const loadBiggestCommodities = (year, exportType) => {
  let date = new Date();
  let lastYear = date.getFullYear();
  $.ajax({
    url: "/emcs/Shipment",
    //async: false,
    data: {
      searchId: year,
      searchName: exportType,
    },
  }).done(function (data) {
    //console.log(data);
    $("#export-biggest-commodities").append('<div id="container_biggest">');

    let category = new Array();
    $.each(data, function (index, element) {
      category.push({
        name: element.Category,
        y: element.Total,
      });
    });
    let big5 = Highcharts.chart("container_biggest", {
      chart: {
        //type: "column",
        //height: 66 + "%",
        plotBackgroundColor: null,
        plotBorderWidth: null,
        plotShadow: false,
        type: "pie",
        height: 240,
        style: {
          fontFamily: "sans-serif",
        },
      },
      //colors: ["#ffca22", "#9dd45d", "#05beff", "#ff696a", "#c63bff"],
      colors: ["#fbbf24", "#60a5fa", "#f87171", "#c084fc", "#4ade80"],
      title: {
        text: "",
      },
      // subtitle: {
      //   text: lastYear,
      // },
      credits: {
        enabled: false,
      },
      tooltip: {
        pointFormat: "<b>{point.percentage:.1f}%</b>",
      },
      plotOptions: {
        pie: {
          innerSize: 125,
          allowPointSelect: false,
          cursor: "pointer",
          animation: {
            duration: 2000,
          },
          borderColor: "#ffffff",
          borderWidth: 2,
          dataLabels: {
            enabled: false,
          },
          showInLegend: true,
        },
      },
      legend: {
        borderWidth: 0,
        itemStyle: {
          fontSize: "11px",
        },
        itemMarginBottom: 3,
      },
      // tooltip: {
      //   headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
      //   pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>',
      // },
      series: [
        {
          name: "Commodity",
          colorByPoint: true,
          data: category,
        },
      ],
    });
    $("#export-biggest-commodities .loading-spinner").hide();
    big5.reflow();
  });
};

const loadExportByCategory = (startYear, endYear) => {
  $.ajax({
    url: "getExportByCategory?startYear=" + startYear + "&endYear=" + endYear,
  }).done(function (data) {
    //console.log(data);
    $("#export-by-category").append('<div id="container_export_by_category">');
    let byCategory = [
      {
        name: "Category",
        colorByPoint: true,
        data: [],
      },
    ];

    $.each(data, function (i, element) {
      byCategory[0]["data"].push({
        name: element.Category,
        y: element.TotalPercentage * 100,
      });
    });

    let exportByCategoryChart = Highcharts.chart("container_export_by_category", {
      chart: {
        plotBackgroundColor: null,
        plotBorderWidth: null,
        plotShadow: false,
        type: "pie",
        height: 240,
        style: {
          fontFamily: "sans-serif",
        },
      },
      //colors: ["#ffca22", "#9dd45d", "#05beff", "#ff696a", "#c63bff"],
      colors: ["#fbbf24", "#60a5fa", "#f87171"],
      title: {
        text: "",
      },
      tooltip: {
        pointFormat: "<b>{point.percentage:.1f}%</b>",
      },
      legend: {
        borderWidth: 0,
        itemStyle: {
          fontSize: "11px",
        },
        itemMarginBottom: 3,
      },
      plotOptions: {
        pie: {
          innerSize: 110,
          allowPointSelect: false,
          cursor: "pointer",
          animation: {
            duration: 2000,
          },
          borderColor: "#ffffff",
          borderWidth: 2,
          dataLabels: {
            enabled: false,
          },
          showInLegend: true,
        },
      },
      credits: {
        enabled: false,
      },
      series: byCategory,
    });
    $("#export-by-category .loading-spinner").hide();
    exportByCategoryChart.reflow();
  });
};

const loadSalesVSNonSales = (startYear, endYear) => {
  $.ajax({
    url: "getSalesVSNonSales?startYear=" + startYear + "&endYear=" + endYear,
  }).done(function (data) {
    //console.log(data);
    $("#export-sales-vs-nonsales").append('<div id="container_compare_sales" class="!pt-4 !pb-2 !px-2">');

    //let startYear = parseInt($("#SalesVSNonSalesStartYear").val());
    let startYear = parseInt(data[0].Year);
    //console.log(startYear);

    let byExpType = [
      {
        name: "Sales",
        data: [],
      },
      {
        name: "Non Sales",
        data: [],
      },
    ];

    $.each(data, function (i, data) {
      byExpType[0].data.push(data.Sales);
      byExpType[1].data.push(data.NonSales);
    });

    let salesVsNonSalesChart = Highcharts.chart("container_compare_sales", {
      colors: ["#ffca22", "#00000"],
      chart: {
        //height: 66 + "%",
        height: 206,
      },
      title: {
        text: "",
      },
      yAxis: {
        title: {
          //text: "Amount",
          text: "",
        },
      },
      legend: {
        layout: "horizontal",
        align: "center",
        verticalAlign: "bottom",
      },
      credits: {
        enabled: false,
      },
      plotOptions: {
        series: {
          pointStart: startYear,
        },
      },
      series: byExpType,
      // responsive: {
      //   rules: [
      //     {
      //       condition: {
      //         maxWidth: 500,
      //       },
      //       chartOptions: {
      //         legend: {
      //           layout: "horizontal",
      //           align: "center",
      //           verticalAlign: "bottom",
      //         },
      //       },
      //     },
      //   ],
      // },
    });
    $("#export-sales-vs-nonsales .loading-spinner").hide();
    salesVsNonSalesChart.reflow();
  });
};

const loadBreakingNews = () => {
  $.ajax({
    url: myApp.fullPath + "/EMCS/getRss",
    //async: false,
  }).done(function (data) {
    var json = data.data;
    $.each(json, function (index, element) {
      var icon = "/Images/favicon.ico";
      switch (element.Source) {
        case "detik":
          icon = "/Images/detik.png";
          break;
        case "cnn":
          icon = "/Images/emcs/cnn_badge.png";
          break;
      }

      var html =
        '<li><span class="bn-logo"><img class="news-logo" src="' +
        icon +
        '" style="width:20px;" /></span><span class="bn-text">' +
        element.Title +
        "</span></li>";
      $("#ul-news").append(html);
    });
    $("#newsTicker").jConveyorTicker();
  });
};

const loadTrakindoVideo = () => {
  $.ajax({
    url: "/emcs/GetVideoTrakindo",
    async: true,
    success: function (data) {
      $("#iframe-trakindo-video").attr("src", "/File/EMCS/Video/" + data.Video);
    },
  });
};

$(function () {
  // Load Map Outstanding
  // let countOutstanding = 1;
  // let rowOutstanding = 5;
  // loadOutstanding(countOutstanding, rowOutstanding);
  var countOutstandingSingle = 1;
  var outstandingDataSingle = loadOutstandingSingle(countOutstandingSingle, 1);
  //console.log(outstandingDataSingle);
  if (outstandingDataSingle.length > 0) {
    //for (let i = 0; i < outstandingDataSingle.length; i++) {
    for (let i = 0; i < 8; i++) {
      var branch = outstandingDataSingle[i].Branch.split("-");
      var branchName = outstandingDataSingle[i].Branch;
      var lastNameBranch = outstandingDataSingle[i].Branch.split(" ");
      var getBranch =
        branchName.includes("Transit") || branchName.includes("Yard")
          ? lastNameBranch[lastNameBranch.length - 1]
          : branch[0].substring(0, 15);
      var portOfDestination = outstandingDataSingle[i].PortOfDestination.split("-");
      var category = $("#map-category").val() === "Branch" ? "CIPL" : "NO AJU";
      var ETD = outstandingDataSingle[i].ETD;
      var ETA = outstandingDataSingle[i].ETA;
      const dateFormat = { year: "numeric", month: "short", day: "numeric" };
      if (ETD == null) {
        ETD = "-";
      } else {
        ETD = moment(outstandingDataSingle[i].ETD).format("DD MMM YY");
      }
      if (ETA == null) {
        ETA = "-";
      } else {
        ETA = moment(outstandingDataSingle[i].ETA).format("DD MMM YY");
      }
      var isActive = "";
      if (i == 0) {
        isActive = "active";
      }
      $("#outstanding-carousel .carousel-inner").append(
        '<button class="map-outstanding-popup--open carousel-item ' +
          isActive +
          '">' +
          '<div class="grid grid-cols-6 gap-x-1 w-full">' +
          '<span class="truncate">' +
          outstandingDataSingle[i].No +
          "</span>" +
          '<span class="truncate">' +
          getBranch +
          "</span>" +
          '<span class="truncate">' +
          ETD +
          "</span>" +
          '<span class="truncate">' +
          ETA +
          "</span>" +
          '<span class="truncate">' +
          portOfDestination[0].trim() +
          "</span>" +
          '<span class="truncate">' +
          outstandingDataSingle[i].ViewByUser +
          "</span>" +
          "</div>" +
          "</button>"
      );
    }
  }

  // Load Map Outstanding Popup
  if (outstandingDataSingle.length > 0) {
    // for (let i = 0; i < outstandingDataSingle.length; i++) {
    for (let i = 0; i < 10; i++) {
      var branch = outstandingDataSingle[i].Branch.split("-");
      var branchName = outstandingDataSingle[i].Branch;
      var lastNameBranch = outstandingDataSingle[i].Branch.split(" ");
      var getBranch =
        branchName.includes("Transit") || branchName.includes("Yard")
          ? lastNameBranch[lastNameBranch.length - 1]
          : branch[0].substring(0, 15);
      var portOfLoading = outstandingDataSingle[i].PortOfLoading.split("-");
      var portOfDestination = outstandingDataSingle[i].PortOfDestination.split("-");
      //var category = $("#map-category").val() === "Branch" ? "CIPL" : "NO AJU";
      var ETD = outstandingDataSingle[i].ETD;
      var ETA = outstandingDataSingle[i].ETA;
      const dateFormat = { year: "numeric", month: "short", day: "numeric" };
      if (ETD == null) {
        ETD = "-";
      } else {
        ETD = moment(outstandingDataSingle[i].ETD).format("DD MMM YY");
      }
      if (ETA == null) {
        ETA = "-";
      } else {
        ETA = moment(outstandingDataSingle[i].ETA).format("DD MMM YY");
      }
      var isActive = "";
      if (i == 0) {
        isActive = "active";
      }
      $(".map-outstanding-popup--table-data").append(
        "<tr>" +
          '<td><span class="truncate">' +
          outstandingDataSingle[i].No +
          "</span></td>" +
          '<td><span class="truncate">' +
          getBranch +
          "</span></td>" +
          '<td><span class="truncate">' +
          portOfLoading[1].trim() +
          "</span></td>" +
          '<td><span class="truncate">' +
          portOfDestination[1].trim() +
          "</span></td>" +
          '<td><span class="truncate">' +
          ETD +
          "</span></td>" +
          '<td><span class="truncate">' +
          ETA +
          "</span></td>" +
          '<td><span class="truncate">' +
          outstandingDataSingle[i].ViewByUser +
          "</span></td>" +
          "</tr>"
      );
    }
  }

  // Load Cards (4 cards on top)
  let date1 = "";
  let date2 = "";
  let dateTimeNow = new Date();
  let yearNow = dateTimeNow.getFullYear();
  loadExportToday(date1, date2);
  loadTotalNetWeight(date1, date2);
  loadExchangeRateToday(date1, date2);
  loadTotalExportValue(date1, date2);

  // Load Map
  loadMap("Branch");
  loadMapModal("Branch");

  // Load Data Cards
  loadTotalDataCard();
  loadShipment();
  loadExportCycle();
  //loadTrendExport(yearNow - 2, yearNow); -- Error, 302 Status
  loadExportByCategory(yearNow - 2, yearNow);
  loadBiggestCommodities(yearNow, "");
  //loadSalesVSNonSales(yearNow, yearNow);
  loadSalesVSNonSales("2020", "2022");

  // Load Breaking News
  loadBreakingNews();

  // Button Interactions
  $("#map-popup-close").on("click", function () {
    $(".map-popup").toggleClass("open");
  });
  $(".map-outstanding-popup--open").on("click", function () {
    $(".map-outstanding-popup").toggleClass("open");
  });
  $("#map-outstanding-popup--close").on("click", function () {
    $(".map-outstanding-popup").toggleClass("open");
  });

  setTimeout(function () {
    let tabs = $("#btn-map-outstanding-switch");
    let activeItem = tabs.find(".active");
    let activeWidth = activeItem.outerWidth();
    $("#btn-map-outstanding-switch .selector").css({
      left: activeItem.position.left + "px",
      width: activeWidth + "px",
    });
    $("#btn-map-outstanding-switch").on("click", "label", function (e) {
      e.preventDefault();
      $("#btn-map-outstanding-switch label").removeClass("active");
      $(this).addClass("active");
      var activeWidth = $(this).outerWidth();
      var itemPos = $(this).position();
      $("#btn-map-outstanding-switch .selector").css({
        left: itemPos.left + "px",
        width: activeWidth + "px",
      });
    });
  }, 500);
  $("#btn-branch-outstanding-map").on("click", function () {
    $("#map-outstanding-category").val("Branch");
  });
  $("#btn-port-outstanding-map").on("click", function () {
    $("#map-outstanding-category").val("Port");
  });

  setTimeout(function () {
    var tabs = $("#btn-map-switch");
    var activeItem = tabs.find(".active");
    var activeWidth = activeItem.outerWidth();
    $("#btn-map-switch .selector").css({
      left: activeItem.position.left + "px",
      width: activeWidth + "px",
    });
    $("#btn-map-switch").on("click", "label", function (e) {
      e.preventDefault();
      $("#btn-map-switch label").removeClass("active");
      $(this).addClass("active");
      var activeWidth = $(this).outerWidth();
      var itemPos = $(this).position();
      $("#btn-map-switch .selector").css({
        left: itemPos.left + "px",
        width: activeWidth + "px",
      });
    });
  }, 500);
  $("#btn-branch-container-map").on("click", function () {
    loadMap("Branch");
    $("#map-category").val("Branch");
  });
  $("#btn-port-container-map").on("click", function () {
    loadMap("Port");
    $("#map-category").val("Port");
  });

  $("#news-collapse").on("click", function () {
    $(".breaking-news-ticker").toggleClass("news-collapsed");
  });

  $("#btn-video-trakindo").on("click", function () {
    $(".dashboardTv").removeClass("active");
    $(".dashboardVideoTrakindo").addClass("active");
    $("#iframe-tv").removeAttr("src");
    if ($(".dashboardVideoTrakindo").not().hasClass("active")) {
      loadTrakindoVideo();
    }
  });
  $("#btn-tv").on("click", function () {
    $(".dashboardVideoTrakindo").removeClass("active");
    $(".dashboardTv").addClass("active");
    $("#iframe-trakindo-video").removeAttr("src");
    $("#iframe-tv").attr("src", "https://www.cnnindonesia.com/tv/embed?ref=transmedia&smartautoplay=true");
  });
  $("#btn-video-trakindo-close").on("click", function () {
    $(".dashboardVideoTrakindo").removeClass("active");
    setTimeout(() => {
      $("#iframe-trakindo-video").removeAttr("src");
    }, 2000);
  });
  $("#btn-tv-close").on("click", function () {
    $(".dashboardTv").removeClass("active");
    setTimeout(() => {
      $("#iframe-tv").removeAttr("src");
    }, 2000);
  });
});