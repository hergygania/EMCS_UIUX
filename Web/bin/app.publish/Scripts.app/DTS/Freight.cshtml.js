$table = $('#tableFreightCost');

var columnList = [
    {
        field: 'Route',
        title: 'Route',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Origin',
        title: 'Origin',
        halign: 'center',
        align: 'left',
        editable: true,
        editableType: "text",
        editableMode: 'inline',
        editableEmptytext: "Set",
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Destination',
        title: 'Destination',
        halign: 'center',
        align: 'left',
        editable: true,
        editableType: "text",
        editableMode: 'inline',
        editableEmptytext: "Set",
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'ProductHierarcy',
        title: 'Product Hierarcy',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Description',
        title: 'Model',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'ValidFrom',
        title: 'Valid From',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: dateFormatterCAT
    },
    {
        field: 'ValidTo',
        title: 'Valid To',
        halign: 'center',
        class: 'text-nowrap',
        align: 'left',
        sortable: true,
        formatter: dateFormatterCAT
    },
    {
        field: 'Amount',
        title: 'Amount',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true,

        
    }];

function showList() {    
    var Route = $('#searchRoute').val();
    var Model = $('#searchModel').val()
    var Origin = $('#searchOrigin').val()
    var Destination = $('#searchDestination').val()
        window.pis.table({
            objTable: $table,
            urlSearch: '/DTS/FreighCostPage',
            urlPaging: '/DTS/FreighCostXt',

            searchParams: {
                Route: Route,
                Model: Model,
                Origin: Origin,
                Destination: Destination
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
        formatNoMatches: function () {
            return 'Please contact Distribution for estimate freight cost';
        },
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


$(document).ready(function () {
    console.log('masuk');
    $("#searchRoute").select2({
        placeholder: "Select a Route",
        minimumInputLength: 3,
        allowClear: true,
        ajax: {
            url: "/DTS/GetFreightRouteOption",
            type: "POST",
            data: function (params) {
                return {
                    type : 'Route',
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
                            text: list.Route,
                            id: list.Route
                        };
                    })
                };
            }
        }
    })
    $("#searchOrigin").select2({
        placeholder: "Select a Origin",
        minimumInputLength: 3,
        allowClear: true,
        ajax: {
            url: "/DTS/GetFreightRouteOption",
            type: "POST",
            data: function (params) {
                return {
                    type: 'Origin',
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
                            text: list.Origin,
                            id: list.Origin
                        };
                    })
                };
            }
        }
    })
    $("#searchDestination").select2({
        placeholder: "Select a Destination",
        minimumInputLength: 3,
        allowClear: true,
        ajax: {
            url: "/DTS/GetFreightRouteOption",
            type: "POST",
            data: function (params) {
                return {
                    type: 'Destination',
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
                            text: list.Destination,
                            id: list.Destination
                        };
                    })
                };
            }
        }
    })
    $("#searchModel").select2({
        placeholder: "Select a Model",
        minimumInputLength: 3,
        allowClear: true,
        ajax: {
            url: "/DTS/GetFreightRouteOption",
            type: "POST",
            data: function (params) {
                return {
                    type: 'Model',
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
                            text: list.Model,
                            id: list.Model
                        };
                    })
                };
            }
        }
    })
});


