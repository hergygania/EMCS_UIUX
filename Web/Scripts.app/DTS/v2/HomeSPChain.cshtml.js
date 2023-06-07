var GroupStatus = [
    { Status: 'DR Submit', Count: 0 },
    { Status: 'DR Approved', Count: 0 },
    { Status: 'DR Revise', Count: 0 },
    { Status: 'DR Rejected', Count: 0 },
];
function GeDRtGroupByStatus() {
    paramSearch = {
        isSPChain: true
    }
    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/GetDRGroupByStatus',
        data: paramSearch,
        beforeSend: function () {
            ShowLoading();
        },
        complete: function () {
            HideLoading();
        },
        dataType: "json",
        success: function (d) {
            if (d.message !== undefined) {
                sAlert('Error', d.message, 'error');
            } else {
                for (var ix in d.result) {
                    if (['submit'].indexOf(d.result[ix].Status) > -1) {                      
                        $("#drsubmit").text(parseInt(d.result[ix].Count_Id));
                    }
                    if (d.result[ix].Status == 'approve') {                      
                        $("#drapprove").text(parseInt(d.result[ix].Count_Id));
                    }
                    if (d.result[ix].Status == 'revise') {                      
                        $("#drrevise").text(parseInt(d.result[ix].Count_Id));
                    }
                    if (d.result[ix].Status == 'reject') {                      
                        $("#drreject").text(parseInt(d.result[ix].Count_Id));
                    }
                }
                
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
}
function GetDRCount() {
    paramSearch = {
        isSPChain: true
    }
    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/GetDRGroupByStatus',
        data: paramSearch,
        beforeSend: function () {
            ShowLoading();
        },
        complete: function () {
            HideLoading();
        },
        dataType: "json",
        success: function (d) {
            var countDR = 0;
            if (d.message !== undefined) {
                sAlert('Error', d.message, 'error');
            } else {
                for (var ix in d.result) {
                    if (['submit'].indexOf(d.result[ix].Status) > -1) {
                        countDR = parseInt(countDR|| 0) + parseInt(d.result[ix].Count_Id || 0);
                    }
                    if (d.result[ix].Status == 'approve') {
                        countDR = parseInt(countDR || 0) + parseInt(d.result[ix].Count_Id || 0);
                    }
                    if (d.result[ix].Status == 'revise') {
                        countDR = parseInt(countDR|| 0) + parseInt(d.result[ix].Count_Id || 0);
                    }
                    if (d.result[ix].Status == 'reject') {
                        countDR = parseInt(countDR || 0) + parseInt(d.result[ix].Count_Id || 0);
                    }
                }
                $("#countnotif").text(countDR);               
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
}
function GetDRCountToday() {
    paramSearch = {
        isSPChain: true
    }
    $.ajax({
        type: "POST",
       
        url: myApp.root + 'DTS/GetDRGroupByStatusToday',
        data: paramSearch,
        beforeSend: function () {
            ShowLoading();
        },
        complete: function () {
            HideLoading();
        },
        dataType: "json",
        success: function (d) {
            var countDR = 0;
            if (d.message !== undefined) {
                sAlert('Error', d.message, 'error');
            } else {               
                countDR = d.result;               
                $("#countnotifTodaySPChain").text(countDR);
                $("#countnotifTodaySPChainClick").text(countDR);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
}
function GetChartDR() {
    paramSearch = {
        isSPChain: true
    }
    $.ajax({
        method: 'POST',
        url: myApp.fullPath + 'DTS/ChartDR',
        data: paramSearch,
        success: function (result) {
            
            var result = result;           
           
            DataDR = [];
            // Chart Categories DR
            var categoriesDR = [];
            Array.from(result).forEach(function (item) {
                var temp1 = [];
                var seriesPhase = [];
              
                Array.from(item.ListDate).forEach(function (ele) {

                    var temp2 = [];
                    temp2 = [ele.MonthName, ele.Count];
                    categoriesDR.push(ele.MonthName);                   
                    seriesPhase.push(temp2);
                   
                });
                temp1 = { name: item.status, data: seriesPhase, /*stack: item.location,*/ };

                DataDR.push(temp1);
            });
           
            ChartDR(DataDR, categoriesDR)
        }
    })
}
function ChartDR(DataDR, categoriesDR) {    
    Highcharts.chart('ChartDR', {
            exporting: { enabled: false },
            chart: {
                type: 'column'
            },
            title: {
                text: 'DELIVERY REQUEST',
                style: {
                    color: "#333333",
                    cursor: "default",
                    fontSize: "16px",
                    whiteSpace: "nowrap"
                }
            },
          
            xAxis: {
                categories: categoriesDR
            },
            yAxis: {
                min: 1,
                title: {
                    text: ''
                },
                stackLabels: {
                    enabled: false,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                    }
                }
            },
            tooltip: {
                headerFormat: '<b>{point.x}</b><br/>',
                pointFormat: '{series.name}: {point.y}'
            },
            plotOptions: {
                column: {
                    dataLabels: {
                        enabled: true,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'black',
                        formatter: function () {
                            if (this.y != 0) {
                                return this.y;
                            }
                        }
                    }
                }
            },
            series: DataDR,
            
            column: {
                dataLabels: {
                    enabled: true
                },
                showInLegend: true,
                stacking: 'normal'
            },

            colors: ['#13C9F5', '#F5B813','#13F54D']
        });
    
}
$(function () {
    GetDRCountToday();
    GetDRCount();
    GeDRtGroupByStatus();  
    GetChartDR();
    $('#imageHome').click(function () {
        window.location = myApp.fullPath + 'DTS/Home';
    });
    $('#imageDeliveryTracking').click(function () {
        window.location.replace(myApp.fullPath + 'DTS/Outbound');       
    });
    $('#imageInboundTracking').click(function () {
        window.location.replace(myApp.fullPath + 'DTS/Inbound');
    });
    $('#imageDailyReportInbound').click(function () {
        window.location.replace(myApp.fullPath + 'DTS/DailyReportInbound');
    });
    $('#imageDailyReportOutbound').click(function () {
        window.location.replace(myApp.fullPath + 'DTS/ReportDeliveryRequisition');
    });
    $('#imageDailyReport').click(function () {
        window.location.replace(myApp.fullPath + 'DTS/ReportDeliveryRequisition');       
    });
    $('#freight').click(function () {
        window.location.replace(myApp.fullPath + 'DTS/Freight');
    });
    $('#userguide').click(function () {
        window.location.replace(myApp.fullPath + 'Content/DTS/UserManual.pptx', '_blank');

    });
    $('#InboundEviz').click(function () {
        window.location.replace(myApp.fullPath + 'DTS/InboundEviz');
    });
    $('#shipmentinbound').click(function () {
        window.location.replace(myApp.fullPath + 'DTS/InboundNonCKB');
    });
    $('#shipmentoutbound').click(function () {
        window.location.replace(myApp.fullPath + 'DTS/OutboundNonCKB');
    });
<<<<<<< HEAD
<<<<<<< HEAD
 
=======
    //$('#createDR').click(function () {       
    //    window.open(myApp.fullPath + 'DTS/DeliveryRequisitionList?c=1&b=Home', '_blank');
    //});
>>>>>>> 639d8d0 (Intial commit)
=======
 
>>>>>>> 93c2efe ([U] Update from client's TFS)
    $('#createDRnew').click(function () {
        window.location.replace(myApp.fullPath + 'DTS/DeliveryRequisitionList');
    });
     
    $('#DRApprove').click(function () {
<<<<<<< HEAD
<<<<<<< HEAD
        var today = ''
        localStorage.setItem("today", today);
        window.location.replace(myApp.fullPath + 'DTS/DeliveryRequisitionListAcc');
    });

=======
        window.location.replace(myApp.fullPath + 'DTS/DeliveryRequisitionListAcc');
    });
    //$('#createDI').click(function () {
    //    window.open(myApp.fullPath + 'DTS/Home?c=1&b=Home');
    //});
>>>>>>> 639d8d0 (Intial commit)
=======
        var today = ''
        localStorage.setItem("today", today);
        window.location.replace(myApp.fullPath + 'DTS/DeliveryRequisitionListAcc');
    });

>>>>>>> 93c2efe ([U] Update from client's TFS)
    $('#createDInew').click(function () {
        window.location.replace(myApp.fullPath + 'DTS/DeliveryInstructionList');
    });
    $('#DIApprove').click(function () {
        window.location.replace(myApp.fullPath + 'DTS/DeliveryInstructionListAcc');
    });
<<<<<<< HEAD
<<<<<<< HEAD
=======
    //$('#DRList').click(function () {
    //    window.location.replace(myApp.fullPath + 'DTS/DeliveryRequisitionList');        
    //});
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)

    $('#imageDR').click(function () {
        $('#DRView').show()
    });
    $('#drsubmit').click(function () {
        var status = 'submit';
        var homenotif = 'notif';
        var today = ''
        localStorage.setItem("homenotif", homenotif);
        localStorage.setItem("status", status);
        localStorage.setItem("today", today);
        window.location.replace(myApp.fullPath + 'DTS/DeliveryRequisitionList');   
    });
    $('#countTodaySPChainClick').click(function () {
        var status = 'submit';
        var homenotif = 'notif';
        var today ='today'
        localStorage.setItem("homenotif", homenotif);
        localStorage.setItem("status", status);
        localStorage.setItem("today", today);
        window.location.replace(myApp.fullPath + 'DTS/DeliveryRequisitionListAcc');
    });
    $('#drapprove').click(function () {
        var status = 'approve';
        var homenotif = 'notif';
        var today = ''
        localStorage.setItem("homenotif", homenotif);
        localStorage.setItem("status", status);      
        localStorage.setItem("today", today);
        window.location.replace(myApp.fullPath + 'DTS/DeliveryRequisitionList');
    });
    $('#drrevise').click(function () {
        var status = 'revise';
        var homenotif = 'notif';
        var today = ''
        localStorage.setItem("homenotif", homenotif);
        localStorage.setItem("status", status);      
        localStorage.setItem("today", today);
        window.location.replace(myApp.fullPath + 'DTS/DeliveryRequisitionList');
    });
    $('#drreject').click(function () {
        var status = 'reject';
        var homenotif = 'notif';
        var today = ''
        localStorage.setItem("homenotif", homenotif);
        localStorage.setItem("status", status);
        localStorage.setItem("today", today);
        window.location.replace(myApp.fullPath + 'DTS/DeliveryRequisitionList');
    });
});



