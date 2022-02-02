$table = $('#tableOutboundNonCkb');

var columnList = [
    {
        field: 'statuseViz',
        title: 'Status Viz',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
       
    },
    {
        field: 'startOnlineDate',
        title: 'Start Online Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: dateFormatter
    },
    {
        field: 'shipToDealerCd',
        title: 'ShipTo Dealer',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'shipSourceName',
        title: 'Ship Source Name Hierarcy',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'shipSourceCode',
        title: 'Ship Source Code',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'serialNumber',
        title: 'Serial Number',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true      
    },
    {
        field: 'salesModel',
        title: 'Sales Model',
        halign: 'center',
        class: 'text-nowrap',
        align: 'left',
        sortable: true    
    },
    {
        field: 'readyToShipDate',
        title: 'ReadyTo Ship Date',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: dateFormatter
    },
    {
        field: 'productLineOrGrpAbr',
        title: 'Product Line Or Grp Abr',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'productLineOrGrp',
        title: 'product Line Or Grp',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true

    },
    {
        field: 'prevApproxOrderRdyToShpDate',
        title: 'Prev Approx Order ReadyTo Ship Date',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: dateFormatter
    },
    {
        field: 'predictedETA',
        title: 'ETA',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: dateFormatter
    },
    {
        field: 'plantShipDate',
        title: 'Plant Ship Date',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: dateFormatter
    },
    {
        field: 'originalDeliver',
        title: 'Original Delivery',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'orderReceiptDate',
        title: 'Order Receipt Date',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: dateFormatter
    },
    {
        field: 'origApproxOrderRdyToShpDate',
        title: 'Origin Approx Order Ready To ShipDate',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: dateFormatter
    },
    {
        field: 'orderReceiptDate',
        title: 'Order Receipt Date',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: dateFormatter
    },
    {
        field: 'orderPromiseDate',
        title: 'Order Promise Date',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: dateFormatter
    },
    {
        field: 'orderNumber',
        title: 'Order Number',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'mnfctrngSrcFacilityName',
        title: 'Manufacture Facility Name',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'mnfctrngSrcFacilityCode',
        title: 'Manufacture Facility Code',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'marketingRegion',
        title: 'Marketing Region',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'lastUpdatedTimestamp',
        title: 'Last Updated',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: dateFormatter
    },
    {
        field: 'lane',
        title: 'Lane',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'hotShipment',
        title: 'Shipment',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'divisionOrBusinessUnitAbr',
        title: 'Division Or BusinessUnit Abr',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'divisionOrBusinessUnit',
        title: 'Division Or BusinessUnit',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'district',
        title: 'District',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'deliveryType',
        title: 'Delivery Type',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'deliveryLocation',
        title: 'Delivery Location',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'deliveryEta',
        title: 'Delivery ETA',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
         formatter: dateFormatter
    },
    {
        field: 'delivered',
        title: 'Delivered',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'dealerOrderNumber',
        title: 'Dealer Order Number',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'dealerName',
        title: 'Dealer Name',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'dealerDeliveryRequirement',
        title: 'Dealer Delivery Requirement',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'dealerCode',
        title: 'Dealer Code',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'buildDate',
        title: 'Build Date',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
         formatter: dateFormatter
    },
    {
        field: 'approxOrderRdyToShpDate',
        title: 'Approx Order Ready To ShipDate',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: dateFormatter
    },
    {
        field: 'approxBuildDate',
        title: 'Approx Build Date',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: dateFormatter
    }];

function showList() {    


    var SalesModel = $('#searchSalesModel').val();
    var SerialNumber = $('#searchSerialNumber').val();

    var ShipSourceName = $('#searchShipSource').val();
    var StartDate = $('#StartDate').val()
    var EndDate = $('#EndDate').val()
    var ETAStartDate = $('#ETAStartDate').val()
    var ETAEndDate = $('#ETAEndDate').val()

        window.pis.table({
            objTable: $table,
            urlSearch: '/DTS/InboundEvizPage',
            urlPaging: '/DTS/InboundEvizPageXt',

            searchParams: {
                SalesModel: SalesModel,
                SerialNumber: SerialNumber,
                ShipSourceName: ShipSourceName,
                StartDate: StartDate,
                EndDate: EndDate,
                ETAStartDate: ETAStartDate,
                ETAEndDate: ETAEndDate
            },
            

            autoLoad: true

        })
    

    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: false,
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,        
        smartDisplay: false,
        pageSize: '10',
        //formatNoMatches: function () {
        //    return 'Please contact Distribution for estimate freight cost';
        //},
        responseHandler: function (resp) {
            var data = {};
            $.map(resp, function (value, key) {
                data[value.Key] = value.Value;
            });
            return data;
        },
        filterControl: true,
        columns: columnList
       
    });
   
}

function dateFormatter(value, row, index) {
    if (value) {
        return moment(value).format("DD-MM-YYYY");
    } else {
        return "-";
    }
}
$(document).ready(function () {
    showList()
    
    $("#searchSalesModel").select2({
        placeholder: "Select a Sales Model",
        minimumInputLength: 3,
        allowClear: true,
        ajax: {
            url: "/DTS/GetSalesModel",
            type: "POST",
            data: function (params) {
                return {
                    type: 'SalesModel',
                    key: params.term
                };
            },
            dataType: 'json',
            processResults: function (data) {
                // Tranforms the top-level key of the response object from 'items' to 'results'
                console.log(data);
                var list = [];
                for (var i = 0; i < data.length; i++) {
                    list.push(data[i]);
                }

                return {
                    results: $.map(data, function (list) {
                        return {
                            text: list.SalesModel,
                            id: list.SalesModel
                        };
                    })
                };
            }
        }
    })
    $("#searchShipSource").select2({
        placeholder: "Select a Ship Source",
        minimumInputLength: 3,
        allowClear: true,
        ajax: {
            url: "/DTS/GetShipSource",
            type: "POST",
            data: function (params) {
                return {
                    type: 'ShipSource',
                    key: params.term
                };
            },
            dataType: 'json',
            processResults: function (data) {
                // Tranforms the top-level key of the response object from 'items' to 'results'
                console.log(data);
                var list = [];
                for (var i = 0; i < data.length; i++) {
                    list.push(data[i]);
                }

                return {
                    results: $.map(data, function (list) {
                        return {
                            text: list.ShipSourceName,
                            id: list.ShipSourceName
                        };
                    })
                };
            }
        }
    })
    $("#searchSerialNumber").select2({
        placeholder: "Select a Serial Number",
        minimumInputLength: 3,
        allowClear: true,
        ajax: {
            url: "/DTS/GetSerialNumber",
            type: "POST",
            data: function (params) {
                return {
                    type: 'SerialNumber',
                    key: params.term
                };
            },
            dataType: 'json',
            processResults: function (data) {
                // Tranforms the top-level key of the response object from 'items' to 'results'
                console.log(data);
                var list = [];
                for (var i = 0; i < data.length; i++) {
                    list.push(data[i]);
                }

                return {
                    results: $.map(data, function (list) {
                        return {
                            text: list.SerialNumber,
                            id: list.SerialNumber
                        };
                    })
                };
            }
        }
    })
 
});


