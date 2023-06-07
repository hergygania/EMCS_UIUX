var $table = $('#tbl-destination-sailing');

$(function () {
    getOriginList();
    getDestinationList();

    var columns = [
        {
            field: "ClNo",
            title: "CL No",
            sortable: true,
            rownspan: 2,
            align: "left",
            halign: "center",
            class: "text-nowrap"
        }, {        
            field: "DestinationCountry",
            title: "DESTINATION COUNTRY",
            sortable: true,
            rownspan: 2,
            align: "left",
            halign: "center",
            class: "text-nowrap"
        }, {
          
            field: "PortDestination",
            title: "Port",
            sortable: true,
            rownspan: 2,
            align: "left",
            halign: "center",
            class: "text-nowrap"
        }, {
            field: "OriginCity",
            title: "ORIGIN CITY",
            sortable: true,
            rownspan: 2,
            align: "left",
            halign: "center",
            class: "text-nowrap"
        }, {

            field: "PortOrigin",
            title: "Port",
            sortable: true,
            rownspan: 2,
            align: "left",
            halign: "center",
            class: "text-nowrap"        
        }, {
            field: "ShippingMethod",
            title: "SHIPPING METHOD",
            sortable: true,
            rownspan: 2,
            align: "left",
            halign: "center",
            class: "text-nowrap"
        }, {            
            field: "Etd",
            title: "ETD",
            sortable: true,
            align: "left",
            halign: "center",
            class: "text-nowrap",
            formatter: function (data, row, index) {
                return moment(data).format("DD MMM YYYY");
            },
        }, {
            field: "Eta",
            title: "ETA",
            sortable: true,
            align: "left",
            halign: "center",
            class: "text-nowrap",
            formatter: function (data, row, index) {
                return moment(data).format("DD MMM YYYY");
            },
        }, {
            field: "Estimation",
            title: "ESTIMATION DAY",
            sortable: true,
            align: "left",
            halign: "center",
            class: "text-nowrap"
        }
    ]


    $table.bootstrapTable({
        //url: "/EMCS/GetEstimationList",
        columns: columns,
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        searchOnEnterKey: true,
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        }
    });

    var Origin = "";
    var Destination = "";
    reportEstimation(Origin, Destination);

});

function reportEstimation(Origin, Destination) {

    window.pis.table({
        objTable: $table,
        urlSearch: '/EMCS/RSailingEstimateListPage?Origin=' + Origin + '&Destination=' + Destination,
        urlPaging: '/EMCS/RSailingEstimatePageXt',
        autoLoad: true
    });

}
function getOriginList() {
    $.ajax({
        url: "/emcs/GetAreaList",
        success: function (data) {
            var origin = new Array();
            $.each(data, function (index, element) {
                origin.push({ 'id': element.Id, 'text': element.Text === 'CATERPILLAR NEW EQUIPMENT' ? 'CATERPILLAR NEW EQUIPMENT' : element.Text });
            });
            $("#origin").select2({
                data: origin,
                width: '100%',
                placeholder: 'All',
            })
            $('#origin').val(null).trigger('change');
        }
    });
}

function getDestinationList() {
    $.ajax({
        url: "/emcs/GetCountryList",
        success: function (data) {
            var destination = new Array();
            $.each(data, function (index, element) {
                destination.push({ 'id': element.Id, 'text': element.Text === 'CATERPILLAR NEW EQUIPMENT' ? 'CATERPILLAR NEW EQUIPMENT' : element.Text });
            });
            $("#destination").select2({
                data: destination,
                width: '100%',
                placeholder: 'All',
            })
            $('#destination').val(null).trigger('change');
        }
    });
}

function searchDataReport() {
    Origin = $('#origin').val() === null || $('#origin').val() === '' ? '' : $('#origin').val();
    Destination = $('#destination').val() === null || $('#destination').val() === '' ? '' : $('#destination').val();
    reportEstimation(Origin, Destination);
}

function exportDataReport() {
    var origin = $('#origin').val() === null || $('#origin').val() === '' ? '' : $('#origin').val();
    var destination = $('#destination').val() === null || $('#destination').val() === '' ? '' : $('#destination').val();
    window.open('/EMCS/DownloadEstimationList?Origin=' + origin + '&Destination=' + destination, '_blank');
}