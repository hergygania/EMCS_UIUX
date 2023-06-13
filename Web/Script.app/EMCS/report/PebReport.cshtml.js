var $table = $('#tbl-pebreport');
var startDate = '';
var endDate = '';
var paramName = '';
var paramValue = '';


$(function () {
    $table.bootstrapTable('refresh', function () {
        alert('Test');
    });

    $('#category').on('change', function () {

        $('.categoryspareparts, .categoryunit, .categorymisc').hide();

        if ($(this).val() === 'CATERPILLAR SPAREPARTS') { // Spareparts
            $('.categoryspareparts').show();
        } else {
            if ($(this).val() === 'MISCELLANEOUS') { // MISC
                $('#permanentCipl, .categorymisc').show();
            } else {
                if ($(this).val() === 'CATERPILLAR NEW EQUIPMENT' || $(this).val() === 'CATERPILLAR USED EQUIPMENT') { // USED EQUIPMENT && UNIT
                    $('.categoryunit').show();
                }
            }
        }
    });
     startDate = '';
     endDate = '';

    //$.ajax({
    //    url: "/EMCS/GetDetailsTrackingFilter",
    //    success: function (data) {

    //        var category = new Array();
    //        $.each(data.category, function (index, element) {
    //            category.push({ 'id': element.Id, 'text': element.Text });
    //        });
    //        $("#category").select2({
    //            data: category,
    //            placeholder: 'Please Select Filter',
    //        }).on("select2:select", function () {
    //            $('#filter-value').val('');
    //        });
    //        $('#category').val(null).trigger('change');


    //        var unit = new Array();
    //        $.each(data.categoryunit, function (index, element) {
    //            unit.push({ 'id': element.Id, 'text': element.Text });
    //        });
    //        $("#unitCipl").select2({
    //            data: unit,
    //            width: '100%',
    //            placeholder: 'Please Select Unit',
    //        });
    //        $('#unitCipl').val(null).trigger('change');

    //        var spareparts = new Array();
    //        $.each(data.categoryspareparts, function (index, element) {
    //            spareparts.push({ 'id': element.Id, 'text': element.Text === 'PRA' ? 'PRA / RMA' : element.Text });
    //        });

    //        $("#sparepartsCipl").select2({
    //            data: spareparts,
    //            width: '100%',
    //            placeholder: 'Please Select Spareparts',
    //        });
    //        $('#sparepartsCipl').val(null).trigger('change');
    //    }
    //});
 


    $('.cal-start-date').click(function () {
        $('#inp-start-date').focus();
    });
    //$('#inp-start-date').focus().datepicker({
    //    format: "mm/yyyy",
    //    viewMode: "months",
    //    minViewMode: "months",
    //    //autoclose: true
    //}).on('changeMonth', function (e) {
    //    //    $('#inp-end-date').datepicker('setDate', '');
    //    //    $('#inp-end-date').datepicker('setStartDate', moment(e.date).format('MM/YYYY'));
    //    //    $('#inp-end-date').datepicker('setEndDate', '12/' + moment(e.date).format('YYYY'));
    //});
    $('#inp-start-date').datepicker({
        format: "dd/mm/yyyy"
    });
    $('#inp-end-date').datepicker({
        format: "dd/mm/yyyy"
    });
    $('.cal-end-date').click(function () {
        $('#inp-end-date').focus();
    });
    //$('#inp-end-date').focus().datepicker({
    //    format: "mm/yyyy",
    //    viewMode: "months",
    //    minViewMode: "months",
    //    //autoclose: true
    //}).on('changeMonth', function (e) {
    //    //$('#inp-start-date').datepicker('setEndDate', moment(e.date).format('MM/YYYY'));
    //});

    var columns = [ 
        [
            {
                field: "PebMonth",
                title: "&nbsp;MONTH &nbsp;&nbsp;",
                rowspan: 3,
                valign: "middle",
                align: "center",
                width: '200px'
            }, {
                field:'RowNumber',
                title: "NO",
                rowspan: 3,
                valign: "middle",
                align: "center"
            },
            {
                title: "PEB",
                rowspan: 2,
                colspan: 3,
                sortable: true,
                align: "center",
            },
            //{
            //    field: "ReferenceNo",
            //    title: "Reference No",
            //    rowspan: 3,
            //    sortable: true,
            //    align: "left",
            //    valign: "middle"
            //},
            {
                field: "PPJK",
                title: "CUSTOMS BROKER  <br/> (PPJK) ",
                rowspan: 3,
                sortable: true,
                align: "center",
            }, {
                field: "ShippingMethod",
                title: "SHIPMENT BY",
                rowspan: 3,
                halign: "center",
                align: "left",
                class: "text-nowrap"
            }, {
                field: "CargoType",
                title: "LOADING TYPE",
                rowspan: 3,
                sortable: true,
                align: "center",
            }, {
                field: "Container",
                title: "CONTAINER",
                rowspan: 3,
                sortable: true,
                align: "center",
            },
            {
                field: "Packages",
                title: "PACKAGES",
                rowspan: 3,
                sortable: true,
                align: "left",
                halign: "center",
                class: "text-nowrap"
            },
            {
                field: 'Gross',
                title: "GROSS WEIGHT",
                rowspan: 2,
                colspan: 2,
                sortable: true,
                align: "center",
            },
            {
                title: "TYPE OF EXPORT",
                rowspan: 2,
                colspan: 2,
                sortable: true,
                align: "center",
            },
            {
                title: "PORT",
                rowspan: 2,
                colspan: 2,
                sortable: true,
                align: "center",
            },
            {
                title: "ESTIMATED SCHEDULE",
                rowspan: 2,
                colspan: 2,
                sortable: true,
                align: "center",
            },
            {
                title: "BL/AWB",
                rowspan: 2,
                colspan: 2,
                sortable: true,
                align: "center",
            },
            {
                title: "VALUE IN PEB",
                rowspan: 2,
                colspan: 8,
                sortable: true,
                align: "center",
            },
            {
                title: "TRAKINDO CIPL",
                rowspan: 2,
                colspan: 4,
                sortable: true,
                align: "center",
            },
            {
                title: "CONSIGNEE",
                rowspan: 2,
                colspan: 2,
                sortable: true,
                align: "center",
            },
            {
                title: "CUSTOMER",
                rowspan: 2,
                colspan: 2,
                sortable: true,
                align: "center",
            },
            {
                field: 'Note',
                title: "NOTE",
                rowspan: 3,
                sortable: true,
                align: "center",
            },
            {
                title: "DESCRIPTION OF GOODS",
                rowspan: 2,
                colspan: 7,
                sortable: true,
                align: "center",
            },
            {
                field:'IncoTerms',
                title: "INCOTERMS",
                rowspan: 3,
                sortable: true,
                align: "center",
            },
            {
                title: "TOTAL PRICE",
                rowspan: 2,
                colspan: 2,
                sortable: true,
                align: "center",
            },
            {
                field: "FreightPayment",
                title: "FREIGHT",
                rowspan: 3,
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },

            {
                field: "InsuranceAmount",
                title: "INSURANCE",
                rowspan: 3,
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: 'TOTALEXPORTVALUE',
                title: "TOTAL EXPORT VALUE",
                rowspan: 3,
                sortable: true,
                align: "center",
                formatter: function (data) {
                    return '$' + '    '+ data
                }
            },
            {
                title: "EXCHANGE RATE",
                rowspan: 2,
                colspan: 1,
                sortable: true,
                align: "center",
            },
            {
                field: 'Ammount',
                title: "TOTAL EXPORT VALUE IN USD",
                rowspan: 3,
                sortable: true,
                align: "center",
                formatter: function (data) {
                    return 'USD' + '    ' + data
                }
            }, {
                field:'TOTALVALUEPERSHIPMENT',
                title: "TOTAL VALUE PERSHIPMENT",
                rowspan: 3,
                sortable: true,
                align: "center",
            },
            {
                field : 'Balanced',
                title: "BALANCED",
                rowspan: 3,
                sortable: true,
                align: "center",
            },
            {
                field: 'TOTALEXPORTVALUEINIDR',
                title: "TOTAL EXPORT VALUE IN IDR",
                rowspan: 3,
                sortable: true,
                align: "center",
                formatter: function (data) {
                    if (data == null) {
                        return "-";
                    }
                    else {
                        return 'IDR' + '        ' + data

                    }
                }
            },
            {
                field: 'Sales',
                title: "SALES",
                rowspan: 3,
                sortable: true,
                align: "center",
            }, {
                field: 'NonSales',
                title: "NON SALES",
                rowspan: 3,
                sortable: true,
                radio: false,
                checkbox: false,
                filterControl: true,
                align: "center",
            },
            {
                title: "Exporter",
                rowspan: 3,
                sortable: true,
                align: "center",
                formatter:function(){
                    return  'PT. Trakindo Utama'
                },
            },
        ],
        [],
        [
            {
                field: "AjuNumber",
                title: "AJU NO.",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                halign: "center",
                align: "center"
            }, {
                field: "Nopen",
                title: "NOPEN",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            }, {
                field: "NopenDate",
                title: "NO PEN DATE",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center",
                formatter: function (data) {
                    if (data != null) {
                        return moment(data).format('DD-MM-YYYY');
                    }
                    else {
                        return '-';
                    }
                }
            },
            {
                field: "Gross",
                title: "TOTAL",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center",    
            },
            {
                field:'GrossWeightUom',
                title: "UOM",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center",
                //formatter: function () {
                //    return "KGS"
                //}
            },
            {
                field: "Type",
                title: "PERMANENT / TEMPORARY",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"

            }, {
                field: "Note",
                title: "SALES / NON SALES",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            }, {
                field: "PortOfLoading",
                title: "LOADING",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            }, {
                field: "PortOfDestination",
                title: "DESTINATION",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            }, {
                field: "Etd",
                title: "ETD",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            }, {
                field: "Eta",
                title: "ETA",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            }, {
                field: "MasterBlAwbNumber",
                title: "NO.",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            }, {
                field: "BlDate",
                title: "DATE",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center",
                //formatter: function (data) {
                //    debugger;
                //    if (data != null) {
                //        return data;
                //    }
                //    else {
                //        return '-';
                //    }
                //}
            }, {
                field: "",
                title: "N0. INVOICE CONSOLIDATION",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center",
                formatter: function () {
                    return '-'
                }

            }, {
                field: "",
                title: "DATE",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center",
                formatter: function (data) {
                    if (data != null) {
                        return moment(data).format('DD-MM-YYYY');
                    }
                    else {
                        return '-';
                    }
                }
            },
            {
                field: "IncoTerms",
                title: "INCOTERMS",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: "Valuta",
                title: "CURRENCY",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: "Rate",
                title: "AMMOUNT",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"

            },
            {
                field: "FreightPayment",
                title: "FREIGHT",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },

            {
                field: "InsuranceAmount",
                title: "INSURANCE",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: "TotalAmount",
                title: "TOTAL AMOUNT",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },

            {
                field: "CiplNo",
                title: "NO",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: "Branch",
                title: "BRANCH",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: "CiplDate",
                title: "DATE",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center",
                formatter: function (data) {
                    if (data != null) {
                        return moment(data).format('DD-MM-YYYY');
                    }
                    else {
                        return '-';
                    }
                }
            },
            {
                field: "Remarks",
                title: "REMARKS",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: "ConsigneeName",
                title: "NAME",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: "ConsigneeCountry",
                title: "COUNTRY",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: "ConsigneeName",
                title: "NAME",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            }, {
                field: "ConsigneeCountry",
                title: "COUNTRY",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: "Type",
                title: "TYPES",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            }, {
                field: "CategoryName",
                title: "NAME",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: "Colli",
                title: "COLLI",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: "CiplQty",
                title: "QTY",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            }, {
                field: "CiplUOM",
                title: "UOM",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            }, {
                field: "CiplWeight",
                title: "WEIGHT",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            }, {
                field: "GrossWeightUom",
                title: "UOM",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center",
                //formatter: function () {
                //    return "KGS"
                //}
            }, {
                field: "Valuta",
                title: "CURRENCY",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            {
                field: "Ammount",
                title: "AMMOUNT",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: "center"
            },
            //{
            //    field: "PebNpeRate",
            //    title: "IDR / USD",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //},
            {
                field: "PebNpeRate",
                title: "IDR / USD",
                sortable: true,
                filterControl: true,
                class: 'text-nowrap',
                align: 'left',
                visible: true,
                formatter: function (data) {
                    return 'IDR' + '    '+ data
                }
            }
            //{
            //    field: "",
            //    title: "IDR / CUR",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: 'center',
            //    visible: true,
            //}
        ]


    ]

    $table.bootstrapTable({
        columns: columns,
        cache: false,
        pagination: true,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'server',
        sortOrder:'DESC',
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        pageSize: [10],
        responseHandler : function (data) {
            //;
            //var settings = {
            //    "url": "https://api.exchangerate.host/convert?from=USD&to=EUR&amount="+data[0].Balanced,
            //    "method": "GET",
            //    "timeout": 0,
            //};

            //$.ajax(settings).done(function (response) {
            //    console.log(response);
            //});
        },
        onRefresh: function () {
            debugger;
            searchDataReport(null, null, null, null, null, false);
        },
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
    });
    /*$($table).bootstrapTable('refresh');*/
    searchDataReport(false);
});

function reportDetailTracking(startDate, endDate, paramName, paramValue, keynum, isclick) {
    if (isclick == undefined || isclick == null || isclick == false) {
        $('#inp-start-date').val(null);
        $('#inp-end-date').val(null);
        window.pis.table({
            objTable: $table,
            urlSearch: '/EMCS/RPebNpePage?isclick='+ false,
            urlPaging: '/EMCS/RPebNpePageXt',
            autoLoad: true
        });
    }
    else {
        window.pis.table({
            objTable: $table,
            urlSearch: '/EMCS/RPebNpePage?startMonth=' + startDate + '&endMonth=' + endDate + '&ParamName=' + paramName + '&ParamValue=' + paramValue + '&Keynum=' + keynum + '&isclick=' + isclick,
            urlPaging: '/EMCS/RPebNpePageXt',
            autoLoad: true
        });
    }
    

}

function searchDataReport(isclick) {
    var keynum = '';
    if ($('#inp-start-date').val() !== undefined && $('#inp-start-date').val() !== null && $('#inp-start-date').val() !== '') {
        var date = $('#inp-start-date').val().split('/');
        startDate = date[2] + '-' + date[1] + '-' + date[0];
    }
    if ($('#inp-end-date').val() !== undefined && $('#inp-end-date').val() !== null && $('#inp-end-date').val() !== '') {
        var date = $('#inp-end-date').val().split('/');
        endDate = date[2] + '-' + date[1] + '-' + date[0];
    }
    if ($('#category').val() !== undefined && $('#category').val() !== null && $('#category').val() !== '') {
        paramName = $('#category').val();
        if ($('#unitCipl').val() != undefined)
            paramValue = $('#unitCipl').val();
        else if ($('#sparepartsCipl').val() != undefined)
            paramValue = $('#sparepartsCipl').val();
    }
    if ($('#keynum').val() !== undefined && $('#keynum').val() !== null && $('#keynum').val() !== '') {
        keynum = $('#keynum').val();
    }
    reportDetailTracking(startDate, endDate, paramName, paramValue, keynum, isclick);

}

function exportDataReport() {
    var keynum = '';
    if ($('#inp-start-date').val() !== undefined && $('#inp-start-date').val() !== null && $('#inp-start-date').val() !== '') {
        var date = $('#inp-start-date').val().split('/');
        startDate = date[2] + '-' + date[1] + '-' + date[0];
    }
    if ($('#inp-end-date').val() !== undefined && $('#inp-end-date').val() !== null && $('#inp-end-date').val() !== '') {
        var date = $('#inp-end-date').val().split('/');
        endDate = date[2] + '-' + date[1] + '-' + date[0];
    }
    if ($('#category').val() !== undefined && $('#category').val() !== null && $('#category').val() !== '') {
        paramName = $('#category').val();
        if ($('#unitCipl').val() != undefined)
            paramValue = $('#unitCipl').val();
        else if ($('#sparepartsCipl').val() != undefined)
            paramValue = $('#sparepartsCipl').val();
    }
    if ($('#keynum').val() !== undefined && $('#keynum').val() !== null && $('#keynum').val() !== '') {
        keynum = $('#keynum').val();
    }

    window.open('/EMCS/DownloadPebReport?startDate=' + startDate + '&endDate=' + endDate + '&ParamName=' + paramName + '&ParamValue=' + paramValue + '&Keynum=' + keynum, '_blank');
}
