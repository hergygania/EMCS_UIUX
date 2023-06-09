$tableunit = $('#tableunitCipl');
$tablepart = $('#tablepartCipl');
$tablemisc = $('#tablemiscCipl');
$tableemail = $('#tableemailCipl');

var columnCiplItem = [
    [{
        field: "CiplNo",
        title: "No.",
        align: "center",
        halign: "center",
        rowspan: 2,
        sortable: true,
        formatter: runningFormatterNoPaging
    }, {
        field: "Name",
        title: "Name/Model",
        rowspan: 2,
        halign: "center",
        sortable: true
    }, {
        field: "Quantity",
        title: "Quantity",
        rowspan: 2,
        halign: "center",
        sortable: true
    }, {
        field: "UnitUom",
        title: "UOM",
        rowspan: 2,
        halign: "center",
        sortable: true
    }, {
        field: "Sn",
        title: "SN",
        rowspan: 2,
        halign: "center",
        sortable: true,
        class: "text-nowrap"
    }, {
        field: "Id",
        title: "ID No.",
        align: "center",
        rowspan: 2,
        halign: "center",
        sortable: true,
        class: "text-nowrap"
    },
    {
        field: "YearMade",
        title: "Year Made",
        sortable: true,
        rowspan: 2,
        align: "center",
        halign: "center",
        class: "text-nowrap"
    },
    {
        field: "UnitPrice",
        title: "Unit Price",
        sortable: true,
        align: "right",
        halign: "center",
        rowspan: 2,
        filterControl: true,
        class: "text-nowrap",
        formatter: currencyFormatter
    },
    {
        field: "ExtendedValue",
        title: "Extended Value",
        sortable: true,
        filterControl: true,
        halign: "center",
        rowspan: 2,
        align: "right",
        class: "text-nowrap",
        formatter: currencyFormatter
    },
    {
        field: "",
        title: "Dimension (in CM)",
        sortable: true,
        colspan: 3,
        filterControl: true,
        halign: "center",
        class: "text-nowrap"
    },
    {
        field: "Type",
        title: "Type",
        sortable: true,
        align: "center",
        rowspan: 2,
        halign: "center",
        filterControl: true,
        class: "text-nowrap",
        formatter: function (data, row, index) {
            if (data) {
                return data;
            }
            return "-";
        }
    },
    {
        field: "NetWeight",
        title: "Net Weight",
        align: "right",
        halign: "center",
        sortable: true,
        rowspan: 2,
        filterControl: true,
        class: "text-nowrap",
        formatter: currencyFormatter
    },
    {
        field: "GrossWeight",
        title: "Gross Weight",
        sortable: true,
        align: "right",
        halign: "center",
        rowspan: 2,
        filterControl: true,
        class: "text-nowrap",
        formatter: currencyFormatter
    }], [
        {
            field: "Length",
            title: "Length",
            sortable: true,
            halign: "center",
            align: "right",
            filterControl: true,
            class: "text-nowrap"
        },
        {
            field: "Width",
            title: "Width",
            sortable: true,
            align: "right",
            halign: "center",
            filterControl: true,
            class: "text-nowrap"
        },
        {
            field: "Height",
            title: "Height",
            sortable: true,
            halign: "center",
            align: "right",
            filterControl: true,
            class: "text-nowrap"
        },
    ]
];

var part = [
    [
        {
            field: "CiplNo",
            title: "No.",
            align: "center",
            halign: "center",
            rowspan: 2,
            sortable: true,
            formatter: runningFormatterNoPaging
        },
        {
            field: "Name",
            title: "Name",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "Quantity",
            title: "Quantity",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "UnitUom",
            title: "UOM",
            rowspan: 2,
            align: 'center',
            sortable: true,
            formatter: function (value, row, index) {
                return row.UnitUom;
            }
        }, {
            field: "PartNumber",
            title: "Part Number",
            rowspan: 2,
            sortable: true,
            align: 'center'
        }, {
            field: "UnitSN",
            title: "SN",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "JCode",
            title: "J-Code",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "Ccr",
            title: "CCR",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "CaseNumber",
            title: "Case Number",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "ASNNumber",
            title: "ASN Number",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "Type",
            title: "Type",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "UnitPrice",
            title: "Unit Price",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true,
            formatter: function (value, row, index) {
                return row.UnitPrice;
            }
        }, {
            field: "ExtendedValue",
            title: "Extended Value",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true,
            formatter: function (value, row, index) {
                return row.ExtendedValue;
            }
        }, {
            field: "dimension",
            title: "Dimension (In CM)",
            colspan: 3,
            align: 'center',
            sortable: true,
            filterControl: true,
            valign: 'middle'
        }, {
            field: "Volume",
            title: "Volume",
            colspan: 1,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "NetWeight",
            title: "Net Weight",
            colspan: 1,
            align: 'right',
            sortable: true,
            filterControl: true
        }, {
            field: "GrossWeight",
            title: "Gross Weight",
            colspan: 1,
            align: 'right',
            sortable: true,
            filterControl: true
        }],
    [{
        field: "Length",
        title: "Length",
        sortable: true,
        align: 'center',
        filterControl: true,
        formatter: function (value, row, index) {
            return row.Length;
        }
    }, {
        field: "Width",
        title: "Width",
        sortable: true,
        align: 'center',
        filterControl: true,
        formatter: function (value, row, index) {
            return row.Width;
        }
    }, {
        field: "Height",
        title: "Height",
        sortable: true,
        align: 'center',
        filterControl: true,
        formatter: function (value, row, index) {
            return row.Height;
        }
    }, {
        field: "Volume",
        title: "(m3)",
        sortable: true,
        align: 'center',
        filterControl: true,
        formatter: function (value, row, index) {
            return row.Volume;
        }
    }, {
        field: "NetWeight",
        title: "in KGa",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            return row.NetWeight;
        }
    }, {
        field: "GrossWeight",
        title: "in KGa",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            return row.GrossWeight;
        }
    }
    ]
];

var misc = [
    [
        {
            field: "CiplNo",
            title: "No.",
            align: "center",
            halign: "center",
            rowspan: 2,
            sortable: true,
            formatter: runningFormatterNoPaging
        },
        {
            field: "Name",
            title: "Name / Model",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "Quantity",
            title: "Quantity",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "Uom",
            title: "UOM",
            rowspan: 2,
            align: 'center',
            sortable: true,
            formatter: function (value, row, index) {
                return row.UnitUom;
            }
        }, {
            field: "PartNumber",
            title: "Part Number",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "UnitPrice",
            title: "Unit Price",
            rowspan: 2,
            align: 'center',
            sortable: true,
            formatter: function (value, row, index) {
                return row.UnitPrice;
            }
        }, {
            field: "ExtendedValue",
            title: "Extended Value",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true,
            formatter: function (value, row, index) {
                return row.ExtendedValue;
            }
        }, {
            field: "dimension",
            title: "Dimension (In CM)",
            colspan: 3,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "Category",
            title: "Type",
            colspan: 1,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "NetWeight",
            title: "Net Weight",
            colspan: 1,
            align: 'right',
            sortable: true,
            filterControl: true
        }, {
            field: "GrossWeight",
            title: "Gross Weight",
            colspan: 1,
            align: 'right',
            sortable: true,
            filterControl: true
        }],
    [{
        field: "Length",
        title: "Length",
        sortable: true,
        align: 'center',
        filterControl: true,
        formatter: function (value, row, index) {
            return row.Length;
        }
    }, {
        field: "Width",
        title: "Width",
        sortable: true,
        align: 'center',
        filterControl: true,
        formatter: function (value, row, index) {
            return row.Width;
        }
    }, {
        field: "Height",
        title: "Height",
        sortable: true,
        align: 'center',
        filterControl: true,
        formatter: function (value, row, index) {
            return row.Height;
        }
    }, {
        field: "m3",
        title: "(m3)",
        sortable: true,
        align: 'center',
        filterControl: true,
        formatter: function (value, row, index) {
            return row.Volume;
        }
    }, {
        field: "NetWeight",
        title: "in KGa",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            return row.NetWeight;
        }
    }, {
        field: "GrossWeight",
        title: "in KGa",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            return row.GrossWeight;
        }
    }
    ]
];

$(function () {
    $tableunit.bootstrapTable({
        url: "/EMCS/CiplItemGetById",
        columns: columnCiplItem,
        cache: false,
        clickToSelect: false,
        search: false,
        pagination: true,
        striped: false,
        reorderableColumns: false,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        queryParams: function (params) {
            return { Id: $("#idCipl").val() };
        },
        sidePagination: 'client',
        onClickRow: selectRow,
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        formatNoMatches: function () {
            return '<span class="noMatches">No item available</span>';
        }
    });

    $tablepart.bootstrapTable({
        url: "/EMCS/CiplItemGetById",
        columns: part,
        cache: false,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbarParts',
        toolbarAlign: 'left',
        queryParams: function (params) {
            return { Id: $("#idCipl").val() };
        },
        onClickRow: selectRow,
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        formatNoMatches: function () {
            return '<span class="noMatches">No data found</span>';
        },
    });

    $tablemisc.bootstrapTable({
        url: "/EMCS/CiplItemGetById",
        columns: misc,
        cache: false,
        //pagination: true,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbarMisc',
        toolbarAlign: 'left',
        queryParams: function (params) {
            return { Id: $("#idCipl").val() };
        },
        onClickRow: selectRow,
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        //pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
    });

    //window.pis.table({
    //    objTable: $tableCiplItem,
    //    urlSearch: '/EMCS/CiplListPage/' + $("#idCipl").val(),
    //    urlPaging: '/EMCS/CiplListPageXt',
    //    autoLoad: true
    //});

});


function get_ciplitem_by_id() {
    $.ajax({
        url: '/EMCS/CiplItemGetById',
        type: 'POST',
        data: {
            id: $('#idCipl').val(),
        },
        cache: false,
        async: false,
        success: function (data, response) {
            var convert = CiplItemConvert(data);
            if (convert.length > 0) {
                if ($('#jenisBarangCipl').val() === 'CATERPILLAR SPAREPARTS') {
                    $tablepart.bootstrapTable('removeAll');
                    $tablepart.bootstrapTable('load', convert);
                    SumReferenceItem();
                } else if ($('#jenisBarangCipl').val() === 'MISCELLANEOUS') {
                    $tablemisc.bootstrapTable('removeAll');
                    $tablemisc.bootstrapTable('load', convert);
                    SumReferenceItem();
                } else if ($('#jenisBarangCipl').val() === 'CATERPILLAR NEW EQUIPMENT' || $('#jenisBarangCipl').val() === 'CATERPILLAR USED EQUIPMENT') {
                    $tableunit.bootstrapTable('removeAll');
                    $tableunit.bootstrapTable('load', convert);
                    SumReferenceItem();
                }
            } else {
                $tablepart.bootstrapTable('removeAll');
                $tablemisc.bootstrapTable('removeAll');
                $tableunit.bootstrapTable('removeAll');
            }

        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Get Data',
            })
        }
    })
}