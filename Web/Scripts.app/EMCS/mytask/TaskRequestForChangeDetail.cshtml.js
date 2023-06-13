$tableChangeHistory = $('#tableRequestForChangeDetail');
$tableChangeItemList = $('#tableRequestForChangeList');
$tablepart = $('#tablepartCipl');
var $tableCargoForm = $('#tableCargoForm');
var $tableCargoItemHistory = $('#tableCargoItemHistory');
var $tablearmada = $('#tablearmada');
var $tablearmadaForRFC = $('#tablearmadaForRFC');
var $tableBlAwb = $('#tableBlAwbItems');
var $tableBlAwbRFC = $('#tableBlAwbItemForRFC');

window.operateEventRight = {
    'click .downloaddoc': function (e, value, row) {
        location.href = "/EMCS/DownloadGrDocument/" + row.Id;
    },
    'click .showDocumentdoc': function (e, value, row) {
        document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/GoodsReceive/" + row.Id + '/' + row.Filename;
    },
    'click .downloadarmadadoc': function (e, value, row) {

        location.href = "/EMCS/DownloadArmadaDocument/" + row.Id;
    },
    'click .showDocumentarmadadoc': function (e, value, row) {

        $.ajax({
            url: '/EMCS/GetListArmada?IdGr=0&Id=' + row.Id + '',
            success: function (data) {
                document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/GoodsReceive/" + data[0].FileName;

            }
        })
    },
    'click .downloadarmadadocForRFC': function (e, value, row) {
        
        location.href = "/EMCS/DownloadArmadaDocumentForRFC?FileName=" + row.FileName;
    },
    'click .showDocumentarmadadocForRFC': function (e, value, row) {
        
        //$.ajax({
        //    url: '/EMCS/GetListArmada?IdGr=0&Id=' + row.Id + '',
        //    success: function (data) {
        document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/GoodsReceive/" + row.FileName;

        //    }
        //})
    },
    'click .downloadBlAwb': function (e, value, row) {
        location.href = "/EMCS/BlAWBDocument/" + row.Id;
    },
    'click .showDocumentBlAwb': function (e, value, row) {
        document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/BLAWB/" + row.FileName;
    },
    'click .downloadBlAwbRFC': function (e, value, row) {
        location.href = "/EMCS/BlAWBRFCDocument/" + row.Id;
    }
};
var partOriginalCiplItems = [
    [
        {
            field: "Id",
            title: "Id Item",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "IdCipl",
            title: "Id Cipl",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "IdReference",
            title: "Id Reference",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: 'ReferenceNo',
            title: 'Reference No',
            rowspan: 2,
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            visible: false
        }, {
            field: 'IdCustomer',
            title: 'Id Customer',
            rowspan: 2,
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            visible: false
        }, {
            field: "Name",
            title: "Name",
            rowspan: 2,
            align: 'center',
            sortable: true,
            class: "text-nowrap"
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
            field: "CoO",
            title: "Country Of Origin",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "PartNumber",
            title: "Part Number",
            rowspan: 2,
            sortable: true,
            align: 'center'
        }, {
            field: "Sn",
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
            field: "ASNNumber",
            title: "ASN Number",
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
            field: "Type",
            title: "Type",
            rowspan: 2,
            align: 'center',
            sortable: true,
            class: 'text-nowrap',
            filterControl: true
        }, {
            field: "IdNo",
            title: "Id No",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "YearMade",
            title: "Year Made",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "Quantity",
            title: "Quantity",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "UnitPrice",
            title: "Unit Price",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "ExtendedValue",
            title: "Extended Value",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true
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
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "GrossWeight",
            title: "Gross Weight",
            colspan: 1,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "Currency",
            title: "Currency",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true,
            visible: false
        }, {
            field: "IdParent",
            title: "Id Parent",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true,
            visible: false
        }, {
            field: "SibNumber",
            title: "SIB Number",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "WoNumber",
            title: "WO Number",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "Claim",
            title: "Claim",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }],
    [{
        field: "Length",
        title: "Length",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.Length === '0.00' ? '' : row.Length : row.Length;
        }
    }, {
        field: "Width",
        title: "Width",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.Width === '0.00' ? '' : row.Width : row.Width;
        }
    }, {
        field: "Height",
        title: "Height",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.Height === '0.00' ? '' : row.Height : row.Height;
        }
    }, {
        field: "Volume",
        title: "(m3)",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.Volume === '0.000000' ? '' : row.Volume : row.Volume;
        }
    }, {
        field: "NetWeight",
        title: "in KGa",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.NetWeight === '0.00' ? '' : row.NetWeight : row.NetWeight;
        }
    }, {
        field: "GrossWeight",
        title: "in KGa",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.GrossWeight === '0.00' ? '' : row.GrossWeight : row.GrossWeight;
        }
    }
    ]
]

//$tablepart.bootstrapTable({
//    columns: partOriginalCiplItems,
//    cache: false,
//    search: false,
//    striped: true,
//    clickToSelect: true,
//    reorderableColumns: true,
//    toolbar: '.toolbarParts',
//    toolbarAlign: 'left',
//   /* onClickRow: selectRow,*/
//    showColumns: true,
//    showRefresh: false,
//    smartDisplay: false,
//    pagination: true,
//    sidePagination: 'client',
//    pageSize: '5',
//    formatNoMatches: function () {
//        return '<span class="noMatches">No data found</span>';
//    },
//});

var columnChangeHistory = [
    {
        field: 'FieldName',
        title: 'FieldName',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }
    , {
        field: 'CreateDate',
        title: 'Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY hh:mm:ss");
        }
    },
    {
        field: 'BeforeValue',
        title: 'Old Value',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'AfterValue',
        title: 'New Value',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    //{
    //    field: 'Reason',
    //    title: 'Reason',
    //    halign: 'center',
    //    align: 'left',
    //    class: 'text-nowrap',
    //    sortable: true
    //}
];
var part = [
    [
        //{
        //    field: "action",
        //    title: "Action",
        //    rowspan: 2,
        //    align: 'center',
        //    class: "text-nowrap",
        //    events: window.operateEvents,
        //    formatter: function (value, row, index) {
        //        return "<button class='btn btn-default btn-xs EditReferenceItem' type='button' data-toggle='modal' data-target='#ModalUpdateReference' value='Edit' title='Edit'><i class='fa fa-pencil'></i></button> <button class='btn btn-danger btn-xs DeleteReferenceItem' type='button' title='Delete'><i class='fa fa-trash'></i></button>";
        //    }
        //}, 
        {
            field: "Id",
            title: "Id Item",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "IdCipl",
            title: "Id Cipl",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        },
        //    {
        //    field: "IdReference",
        //    title: "Id Reference",
        //    rowspan: 2,
        //    align: 'center',
        //    sortable: true
        //},
        {
            field: 'ReferenceNo',
            title: 'Reference No',
            rowspan: 2,
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            visible: false
        }, {
            field: 'IdCustomer',
            title: 'Id Customer',
            rowspan: 2,
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            visible: false
        }, {
            field: "Name",
            title: "Name",
            rowspan: 2,
            align: 'center',
            sortable: true,
            class: "text-nowrap"
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
            field: "CoO",
            title: "Country Of Origin",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "PartNumber",
            title: "Part Number",
            rowspan: 2,
            sortable: true,
            align: 'center'
        }, {
            field: "Sn",
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
            field: "ASNNumber",
            title: "ASN Number",
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
            field: "Type",
            title: "Type",
            rowspan: 2,
            align: 'center',
            sortable: true,
            class: 'text-nowrap',
            filterControl: true
        }, {
            field: "IdNo",
            title: "Id No",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "YearMade",
            title: "Year Made",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "Quantity",
            title: "Quantity",
            rowspan: 2,
            align: 'center',
            sortable: true
        }, {
            field: "UnitPrice",
            title: "Unit Price",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "ExtendedValue",
            title: "Extended Value",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true
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
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "GrossWeight",
            title: "Gross Weight",
            colspan: 1,
            align: 'center',
            sortable: true,
            filterControl: true
        }, {
            field: "Currency",
            title: "Currency",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true,
            visible: false
        }, {
            field: "IdParent",
            title: "Id Parent",
            rowspan: 2,
            align: 'center',
            sortable: true,
            filterControl: true,
            visible: false
        }, {
            field: "SibNumber",
            title: "SIB Number",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "WoNumber",
            title: "WO Number",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        }, {
            field: "Claim",
            title: "Claim",
            rowspan: 2,
            align: 'center',
            sortable: true,
            visible: false
        },
        {
            field: "Status",
            title: "Status",
            rowspan: 2,
            align: 'center',
            sortable: true,

        }],
    [{
        field: "Length",
        title: "Length",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.Length === '0.00' ? '' : row.Length : row.Length;
        }
    }, {
        field: "Width",
        title: "Width",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.Width === '0.00' ? '' : row.Width : row.Width;
        }
    }, {
        field: "Height",
        title: "Height",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.Height === '0.00' ? '' : row.Height : row.Height;
        }
    }, {
        field: "Volume",
        title: "(m3)",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.Volume === '0.000000' ? '' : row.Volume : row.Volume;
        }
    }, {
        field: "NetWeight",
        title: "in KGa",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.NetWeight === '0.00' ? '' : row.NetWeight : row.NetWeight;
        }
    }, {
        field: "GrossWeight",
        title: "in KGa",
        sortable: true,
        align: 'right',
        filterControl: true,
        formatter: function (value, row, index) {
            var Category = GetCategoryUsed();
            return Category === 'PRA' || Category === 'REMAN' ? row.GrossWeight === '0.00' ? '' : row.GrossWeight : row.GrossWeight;
        }
    }
    ]
]
var tableCargoForm = [
    [{
        field: "",
        title: "NO",
        rowspan: 2,
        align: 'center',
        formatter: runningFormatterNoPaging
    },
    {
        field: "IdCipl",
        rowspan: 2,
        visible: false
    }, {
        field: "ContainerNumber",
        title: "CONTAINER",
        rowspan: 2,
        align: 'center',
        sortable: true
    }, {
        field: "IncoTerm",
        rowspan: 2,
        visible: false
    }, {
        field: "ContainerType",
        title: "CONTAINER TYPE",
        rowspan: 2,
        align: 'center',
        sortable: true,
        formatter: function (data, row, index) {
            if (data !== "") {
                return data;
            }
            return "-";
        }
    }, {
        field: "ContainerSealNumber",
        title: "SEAL NUMBER",
        rowspan: 2,
        align: 'center',
        sortable: true,
        formatter: function (data, row, index) {
            if (data !== "") {
                return data;
            }
            return "-";
        }
    }, {
        field: "CaseNumber",
        title: "CASE NUMBER",
        rowspan: 2,
        align: 'center',
        sortable: true,
        formatter: function (data, row, index) {
            if (data !== "") {
                return data;
            }
            return "-";
        }
    }, {
        field: "ItemName",
        title: "ITEM NAME",
        rowspan: 2,
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: function (data, row, index) {
            if (data !== "") {
                return data;
            }
            return "-";
        }
    }, {
        field: "Sn",
        title: "SN",
        rowspan: 2,
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: function (data, row, index) {
            if (data !== "") {
                return data;
            }
            return "-";
        }
    }, {
        field: "reference",
        title: "REFERENCE",
        colspan: 2,
        sortable: true,
        align: 'center'
    }, {
        field: "CargoDescription",
        title: "CARGO DESCRIPTION",
        rowspan: 2,
        align: 'center',
        sortable: true
    }, {
        field: "volume",
        title: "VOLUME",
        colspan: 3,
        halign: 'center',
        align: 'right',
        sortable: true,
        formatter: currencyFormatter
    }, {
        field: "NetWeight",
        title: "NET WEIGHT (KGS)",
        rowspan: 2,
        halign: 'center',
        align: 'right',
        sortable: true,
        formatter: currencyFormatter
    }, {
        field: "GrossWeight",
        title: "GROSS WEIGHT (KGS)",
        rowspan: 2,
        halign: 'center',
        align: 'right',
        sortable: true,
        formatter: currencyFormatter
    }],
    [{
        field: "EdoNo",
        title: "EDI Number",
        halign: 'center',
        align: 'left',
        sortable: true
    }, {
        field: "InboundDa",
        title: "INBOUND DA #",
        halign: 'center',
        align: 'left',
        sortable: true
    }, {
        field: "Length",
        title: "LENGTH",
        halign: 'center',
        align: 'right',
        sortable: true
    }, {
        field: "Width",
        title: "WIDTH",
        halign: 'center',
        align: 'right',
        sortable: true
    }, {
        field: "Height",
        title: "HEIGHT",
        halign: 'center',
        align: 'right',
        sortable: true
    }
    ]
]
var tableCargoItemHistory = [
    [{
        field: "",
        title: "NO",
        rowspan: 2,
        align: 'center',
        formatter: runningFormatterNoPaging
    },
    //{
    //    field: "State",
    //    title: "State",
    //    checkbox: false,
    //    class: "myclass",
    //    rowspan: 2,
    //    events: operateEvents,
    //    formatter: function (data, row, index) {
    //        return "<input type='checkbox' id='vehicle3' class='checkCargo'>";
    //    }
    //},
    //{
    //    field: "Id",
    //    title: "ACTION",
    //    align: "center",
    //    rowspan: 2,
    //    events: operateEvents,
    //    visible: true,
    //    formatter: function (data, row, index) {
    //        var divOpen = "<div style='width:100%;'>";
    //        var CargoItemId = row.Id;
    //        btnEditGroup = "";
    //        btnEdit = "<button type='button' class='btn btn-primary btn-xs edititem' id='edit'><i class='fa fa-edit'></i></button>";
    //        btnDelete = "<button type='button' class='btn btn-danger btn-xs deleteItem'><i class='fa fa-times'></i></button>";
    //        divClose = "</div>";
    //        htm = [divOpen, btnEditGroup, btnEdit, btnDelete, divClose];
    //        return htm.join(" ");
    //    }
    //},
    {
        field: "Id",
        rowspan: 2,
        visible: false
    },
    {
        field: "IdCargoItem",
        rowspan: 2,
        visible: false
    }, {
        field: "IdCipl",
        rowspan: 2,
        visible: false
    }, {
        field: "ContainerNumber",
        title: "CONTAINER",
        rowspan: 2,
        align: 'center',
        sortable: true
    }, {
        field: "IncoTerm",
        rowspan: 2,
        visible: false
    }, {
        field: "ContainerType",
        title: "CONTAINER TYPE",
        rowspan: 2,
        align: 'center',
        sortable: true,
        formatter: function (data, row, index) {
            if (data !== "") {
                return data;
            }
            return "-";
        }
    }, {
        field: "ContainerSealNumber",
        title: "SEAL NUMBER",
        rowspan: 2,
        align: 'center',
        sortable: true,
        formatter: function (data, row, index) {
            if (data !== "") {
                return data;
            }
            return "-";
        }
    }, {
        field: "CaseNumber",
        title: "CASE NUMBER",
        rowspan: 2,
        align: 'center',
        sortable: true,
        formatter: function (data, row, index) {
            if (data !== "") {
                return data;
            }
            return "-";
        }
    }, {
        field: "ItemName",
        title: "ITEM NAME",
        rowspan: 2,
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: function (data, row, index) {
            if (data !== "") {
                return data;
            }
            return "-";
        }
    }, {
        field: "Sn",
        title: "SN",
        rowspan: 2,
        halign: 'center',
        align: 'left',
        sortable: true,
        formatter: function (data, row, index) {
            if (data !== "") {
                return data;
            }
            return "-";
        }
    }, {
        field: "reference",
        title: "REFERENCE",
        colspan: 2,
        sortable: true,
        align: 'center'
    }, {
        field: "CargoDescription",
        title: "CARGO DESCRIPTION",
        rowspan: 2,
        align: 'center',
        sortable: true
    }, {
        field: "volume",
        title: "VOLUME",
        colspan: 3,
        halign: 'center',
        align: 'right',
        sortable: true,
        formatter: currencyFormatter
    }, {
        field: "Net",
        title: "NET WEIGHT (KGS)",
        rowspan: 2,
        halign: 'center',
        align: 'right',
        sortable: true,
        formatter: currencyFormatter
    }, {
        field: "Gross",
        title: "GROSS WEIGHT (KGS)",
        rowspan: 2,
        halign: 'center',
        align: 'right',
        sortable: true,
        formatter: currencyFormatter
    }, {
        field: "Status",
        title: "Status",
        rowspan: 2,
        halign: 'center',
        align: 'right',
        sortable: true,
    }],
    [{
        field: "EdoNo",
        title: "EDI Number",
        halign: 'center',
        align: 'left',
        sortable: true
    }, {
        field: "InboundDa",
        title: "INBOUND DA #",
        halign: 'center',
        align: 'left',
        sortable: true
    }, {
        field: "Length",
        title: "LENGTH",
        halign: 'center',
        align: 'right',
        sortable: true
    }, {
        field: "Width",
        title: "WIDTH",
        halign: 'center',
        align: 'right',
        sortable: true
    }, {
        field: "Height",
        title: "HEIGHT",
        halign: 'center',
        align: 'right',
        sortable: true
    }
    ]
]
var columnArmada = [

    //{
    //    field: '',
    //    title: 'Action',
    //    halign: 'center',
    //    align: 'center',
    //    class: 'text-nowrap',
    //    sortable: true,
    //    events: operateEvents,
    //    formatter: function () {
    //        var btnEdit = "<button type='button' class='btn btn-xs editarmada btn-primary'><i class='fa fa-edit'></i></button>";
    //        var btnRemove = "<button type='button' class='btn btn-xs removearmada btn-danger'><i class='fa fa-times'></i></button>";
    //        // var btnView = "<button type='button' class='btn viewarmada btn-xs btn-default'><i class='fa fa-search'></i></button>";
    //        var btnUpload = "<button type='button' class='btn btn-xs uploadarmadadocument btn-info' data-toggle='modal' data-target='#myModalUploadPlaceArmada' ><i class='fa fa-upload'></i></button>";
    //        var elm = ["<div>", btnEdit, btnRemove, btnUpload, "</div>"];
    //        return elm.join(" ");
    //    }
    //},
    {
        field: 'DoNo',
        title: 'Edi No.',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'PicName',
        title: 'PIC Name',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'PhoneNumber',
        title: 'Contact',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'SimExpiryDate',
        title: 'License Expiry Date#',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'StnkNumber',
        title: 'No STNK',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    //{
    //    field: 'Bast',
    //    title: 'Bast',
    //    halign: 'center',
    //    align: 'center',
    //    class: 'text-nowrap',
    //    sortable: true,
    //},
    {
        field: 'KirNumber',
        title: 'KIR Number',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'KirExpire',
        title: 'KIR Expiry Date',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'Bast',
        title: 'Bast',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'NopolNumber',
        title: 'Police Plate#',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'SimNumber',
        title: 'SIM',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        //formatter: function (data, row, index) {
        //    return moment(data).format("DD MMM YYYY");
        //}
    },
    {
        field: 'KtpNumber',
        title: 'KTP',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'Apar',
        title: 'APAR',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data) {
            if (data == false) {
                return "No";
            }
            else {
                return "Yes";
            }
        }

    },
    {
        field: 'EstimationTimePickup',
        title: 'ATP',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'Apd',
        title: 'APD',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            if (data == false) {
                return "No";
            }
            else {
                return "Yes";
            }
        }
    },
    {
        field: 'DaNo',
        title: 'DO Reference',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    //{
    //    field: '',
    //    title: 'AAA',
    //    halign: 'center',
    //    align: 'center',
    //    class: 'text-nowrap',
    //    sortable: true,
    //},
    //{
    //    field: '',
    //    title: 'Attachment',
    //    align: 'center',
    //    valign: 'center',
    //    halign: "center",
    //    class: 'text-nowrap',
    //    sortable: true,
    //},
    {
        field: 'FileName',
        title: 'Attachment',
        align: 'center',
        valign: 'center',
        halign: "center",
        sortable: true,
        events: operateEventRight,
        formatter: function (data, row) {
            if (row.FileName !== "" && row.FileName !== null) {
                var btnDownload = "<button class='btn btn-xs btn-success downloadarmadadoc' type='button'><i class='fa fa-download'></i></button>";
                var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocumentarmadadoc' data-toggle='modal' data-target='#myModalUploadPreview' type='button'><i class='fa fa-file-pdf-o'></i></button>";
                return [btnDownload, btnPreview].join(' ');
            } else {
                return "-";
            }
        },
        class: 'text-nowrap'

    }];
var columnArmadaForRFC = [

    //{
    //    field: '',
    //    title: 'Action',
    //    halign: 'center',
    //    align: 'center',
    //    class: 'text-nowrap',
    //    sortable: true,
    //    events: operateEvents,
    //    formatter: function (data, row, index) {
    //        //var btnEdit = "<button type='button' class='btn btn-xs editarmada btn-primary'><i class='fa fa-edit'></i></button>";
    //        //var btnRemove = "<button type='button' class='btn btn-xs removearmada btn-danger'><i class='fa fa-times'></i></button>";
    //        // var btnView = "<button type='button' class='btn viewarmada btn-xs btn-default'><i class='fa fa-search'></i></button>";
    //        if (row.Status == "Created") {
    //            var btnUpload = "<button type='button' class='btn btn-xs uploadarmadadocumentForRFC btn-info' value='true' data-toggle='modal' data-target='#myModalUploadPlaceArmadaForRFC' ><i class='fa fa-upload'></i></button>";
    //            var elm = ["<div>", btnUpload, "</div>"];
    //            return elm.join(" ");
    //        }
    //        else {
    //            return "-";
    //        }

    //    }
    //},
    {
        field: 'DoNo',
        title: 'Edi No.',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'PicName',
        title: 'PIC Name',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'PhoneNumber',
        title: 'Contact',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'SimExpiryDate',
        title: 'License Expiry Date#',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'StnkNumber',
        title: 'No STNK',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    //{
    //    field: 'Bast',
    //    title: 'Bast',
    //    halign: 'center',
    //    align: 'center',
    //    class: 'text-nowrap',
    //    sortable: true,
    //},
    {
        field: 'KirNumber',
        title: 'KIR Number',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'KirExpire',
        title: 'KIR Expiry Date',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'Bast',
        title: 'Bast',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'NopolNumber',
        title: 'Police Plate#',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'SimNumber',
        title: 'SIM',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        //formatter: function (data, row, index) {
        //    return moment(data).format("DD MMM YYYY");
        //}
    },
    {
        field: 'KtpNumber',
        title: 'KTP',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'Apar',
        title: 'APAR',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data) {
            if (data == false) {
                return "No";
            }
            else {
                return "Yes";
            }
        }

    },
    {
        field: 'EstimationTimePickup',
        title: 'ATP',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'Apd',
        title: 'APD',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            if (data == false) {
                return "No";
            }
            else {
                return "Yes";
            }
        }
    },
    {
        field: 'DaNo',
        title: 'DO Reference',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'Status',
        title: 'Status',
        align: 'center',
        valign: 'center',
        halign: "center",
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'FileName',
        title: 'Attachment',
        align: 'center',
        valign: 'center',
        halign: "center",
        sortable: true,
        events: operateEventRight,
        formatter: function (data, row) {
            if (row.FileName !== "" && row.FileName !== null && row.Status != "Deleted") {
                var btnDownload = "<button class='btn btn-xs btn-success downloadarmadadocForRFC' type='button'><i class='fa fa-download'></i></button>";
                var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocumentarmadadocForRFC' data-toggle='modal' data-target='#myModalUploadPreview' type='button'><i class='fa fa-file-pdf-o'></i></button>";
                return [btnDownload, btnPreview].join(' ');
            } else {
                return "-";
            }
        },
        class: 'text-nowrap'

    }];
var blawbcolumns = [
    //{
    //    field: "Id",
    //    title: "Action",
    //    align: "center",
    //    class: "text-nowrap",
    //    sortable: true,
    //    width: "125",
    //    formatter: function (data, row, index) {
    //        return operateFormatter({ Edit: Boolean($AllowUpdate), Delete: Boolean($AllowDelete), Data: row });
    //    },
    //    events: operateEvents
    //},
    {
        field: 'Number',
        title: 'Master BL/AWB Number',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'MasterBlDate',
        title: 'Master BL/AWB Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'HouseBlNumber',
        title: 'House BL/AWB Number',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'HouseBlDate',
        title: 'House BL/AWB Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'Publisher',
        title: 'Publish',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'FileName',
        title: 'Attachment',
        align: 'center',
        valign: 'center',
        halign: "center",
        class: 'text-nowrap',
        sortable: true,
        events: operateEventRight,
        formatter: function (data, row) {
            if (row.FileName !== "") {
                var btnDownload = "<button class='btn btn-xs btn-success downloadBlAwb' type='button'><i class='fa fa-download'></i></button>";
                var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocumentBlAwb' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='fa fa-file-pdf-o'></i></button>";
                return [btnDownload, btnPreview].join(' ');
            } else {
                return "-";
            }
        },
        class: 'text-nowrap'
    }
]
var blawbcolumnsRFC = [
    {
        field: 'Number',
        title: 'Master BL/AWB Number',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
    },
    {
        field: 'MasterBlDate',
        title: 'Master BL/AWB Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'HouseBlNumber',
        title: 'House BL/AWB Number',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'HouseBlDate',
        title: 'House BL/AWB Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'Publisher',
        title: 'Publish',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Status',
        title: 'Status',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'FileName',
        title: 'Attachment',
        align: 'center',
        valign: 'center',
        halign: "center",
        class: 'text-nowrap',
        sortable: true,
        events: operateEventRight,
        formatter: function (data, row) {
            if (row.FileName !== "" && row.FileName != null && row.Status != "Deleted") {
                var btnDownload = "<button class='btn btn-xs btn-success downloadBlAwbRFC' type='button'><i class='fa fa-download'></i></button>";
                var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocumentBlAwb' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='fa fa-file-pdf-o'></i></button>";
                return [btnDownload, btnPreview].join(' ');
            } else {
                return "-";
            }
        },
        class: 'text-nowrap'
    }
]
//$('#btnApproveChangeHistory').on('click', function () {
//    paramSearch = {
//        idTerm: $('#RFCId').val(),
//        formtype: 'CIPL',
//        formId: $('#formId').val(),
//    };
//    $.ajax({
//        url: '/EMCS/ApproveChangeHistory',
//        type: 'POST',
//        data: paramSearch,
//        success: function (guid) {

//        },
//        cache: false
//    });
//});
function ApproveRequestForChange() {
    
    var url = '';
    if ($('#FormType').val() === 'CIPL') {
        url = '/EMCS/ApproveChangeHistory';
    }
    else if ($('#FormType').val() === 'Cargo') {
        url = '/EMCS/ApproveChangeHistoryCl';
    }
    else if ($('#FormType').val() === 'ShippingInstruction') {
        url = '/EMCS/ApproveChangeHistoryCl';
    }
    else if ($('#FormType').val() === 'NpePeb') {
        url = '/EMCS/ApproveChangeHistoryCl';
    }
    else if ($('#FormType').val() === 'GoodsReceive') {
        url = '/EMCS/ApproveChangeHistoryCl';
    }
    else if ($('#FormType').val() === 'BlAwb') {
        url = '/EMCS/ApproveChangeHistoryCl';
    }
    paramSearch = {
        idTerm: $('#RFCId').val(),
        formtype: $('#FormType').val(),
        formId: $('#formId').val(),
    };
    
    $.ajax({
        url: url,
        type: 'POST',
        data: paramSearch,
        success: function (guid) {

        },
        cache: false
    });
}
function RejectRequestForChange(data) {
    paramSearch = {
        idTerm: data.Id,
        reason: data.Notes,
    };
    $.ajax({
        url: '/EMCS/RejectChangeHistory',
        type: 'POST',
        data: paramSearch,
        success: function (guid) {
            Swal.fire({
                type: 'success',
                title: 'Success'
                , text: 'Request rejected successfully',
            }).then((result) => {
                window.location.href = "/EMCS/Mytask";
            });
        },
        cache: false
    });
}
$("#btnApproveChangeHistory").on("click", function () {
    Swal.fire({
        title: 'Approve Confirmation',
        text: 'By approving this changes, you are responsible for the authenticity of the documents and data entered. Are you sure you want to process this request of change?',
        type: 'question',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonColor: '#3085d6',
        confirmButtonText: 'Yes, Approve!',
        allowEscapeKey: false,
        allowOutsideClick: false,
        showCloseButton: true
    }).then((result) => {
        if (result.value) {
            ApproveRequestForChange();
            Swal.fire({
                type: 'success',
                title: 'Success'
                , text: 'Saved, Your Data Has Been Saved',
            }).then((result) => {
                window.location.href = "/EMCS/Mytask";
            });
        }
        return false;
    });
});
$("#btnRejectChangeHistory").on("click", function () {
    Swal.fire({
        title: 'Reject this Request?',
        text: 'Are you sure you want to reject this request for change?',
        type: 'question',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonColor: '#3085d6',
        confirmButtonText: 'Yes, Reject!',
        allowEscapeKey: false,
        allowOutsideClick: false,
        showCloseButton: true
    }).then((result) => {
        if (result.value) {
            Swal.fire({
                input: 'textarea',
                allowEscapeKey: false,
                allowOutsideClick: false,
                inputPlaceholder: 'Please add reason for rejecting this request for change...',
                inputAttributes: {
                    'aria-label': 'Please add reason for rejecting this request for change...'
                },
                showCancelButton: false
            }).then((result) => {
                var Notes = result.value;
                var Status = "Approve";
                var Id = $('#RFCId').val();
                var data = { Notes: Notes, Status: Status, Id: Id };
                RejectRequestForChange(data);
            });
        }
        return false;
    });
});
function GetCategoryUsed() {
    var Category;
    $('#FormOldCore').hide();
    if ($('#jenisBarangCipl').val() === 'CATERPILLAR NEW EQUIPMENT') {
        Category = "PP";
    } else if ($('#jenisBarangCipl').val() === 'CATERPILLAR SPAREPARTS') {
        if ($('#sparepartsCipl').val() === 'PRA') {
            Category = "PRA";
            $('#FormOldCore, #div-CaseNumberParts').show();
            $('#div-SnItemCipl').hide();
        } else if ($('#sparepartsCipl').val() === 'Old Core') {
            Category = "REMAN";
            $('#FormOldCore, #div-SnItemCipl, #div-WidthParts, #div-HeightParts, #div-VolumeParts, #div-GrossWeightParts, #div-NetWeightParts').show();
            $('#div-CaseNumberParts').hide();
        } else if ($('#sparepartsCipl').val() === 'SIB') {
            Category = "SIB";
        }

    } else if ($('#jenisBarangCipl').val() === 'CATERPILLAR USED EQUIPMENT') {
        Category = "UE";
    } else {
        Category = "MISCELLANEOUS";
    }
    return Category;
}

$(function () {
    $.ajax({
        url: '/EMCS/GetChangeHistoryReason',
        type: 'POST',
        data: {
            IdTerm: $("#RFCId").val(),
            formtype: $('#FormType').val(),
        },
        cache: false,
        async: false,
        success: function (data, response) {
            $("#changehistory").text(data);
        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Update Data',
            })
        }
    });
    $.ajax({
        url: '/EMCS/CheckRequestExists',
        type: 'POST',
        data: {
            IdTerm: $("#RFCId").val(),
            formtype: $('#FormType').val(),
        },
        cache: false,
        async: false,
        success: function (data, response) {
            if (data == 0) {
                $("#btnRejectChangeHistory").hide();
                $("#btnApproveChangeHistory").hide();
            }
        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Update Data',
            })
        }
    });
    $tableChangeHistory.bootstrapTable({
        cache: false,
        url: "/Emcs/GetChangeHistoryList",
        pagination: true,
        search: false,
        striped: false,
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,
        queryParams: function (params) {
            return {
                limit: params.limit,
                offset: params.offset,
                IdTerm: $("#RFCId").val(),
                formType: $("#formType").val(),
                sort: params.sort,
                order: params.order
            };
        },
        searchOnEnterKey: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">Data Not Found</span>';
        },
        responseHandler: function (resp) {
            var data = {};
            $.map(resp, function (value, key) {
                data[value.Key] = value.Value;
            });


            return data;
        },
        columns: columnChangeHistory
    });
    $("#tableRequestForChangeList").bootstrapTable({
        url: "/EMCS/GetCiplItemChangeList",
        columns: part,
        cache: false,
        pagination: true,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: ".toolbar",
        toolbarAlign: "left",
        queryParams: function (params) {
            return {
                limit: params.limit,
                offset: params.offset,
                IdCipl: $('#formId').val(),
                sort: params.sort,
                order: params.order
            };
        },
        onClickRow: selectRow,
        sidePagination: "server",
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: "5",
        formatNoMatches: function () {
            return '<span class="noMatches">No task available</span>';
        }
    });

    $("#tablepartCipl").bootstrapTable({
        url: "/EMCS/CiplItemList?id=" + $('#formId').val(),
        columns: partOriginalCiplItems,
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
        formatNoMatches: function () {
            return '<span class="noMatches">No task available</span>';
        }
    });



    $tableCargoForm.bootstrapTable({
        columns: tableCargoForm,
        url: "/EMCS/GetCargoListItem",
        cache: false,
        sidePagination: 'client',
        pagination: true,
        search: false,
        queryParams: function (params) {
            var CargoId = $("#formId").val();
            return { "Id": CargoId }
        },
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        responseHandler: function (resp) {
            var data = {};
            $.map(resp, function (value, key) {
                data[value.Key] = value.Value;
            });
            return data.rows;
        },
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">No Item Found</span>';
        }

    });

    $tableCargoItemHistory.bootstrapTable({
        columns: tableCargoItemHistory,
        url: "/EMCS/GetCargoItemHistory",
        cache: false,
        sidePagination: 'client',
        pagination: true,
        search: false,
        queryParams: function (params) {
            var CargoId = $("#formId").val();
            return { "Cargoid": CargoId }
        },
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        responseHandler: function (resp) {
            return resp;
        },
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">No Item Found</span>';
        }

    });
    $tablearmada.bootstrapTable({
        url: "/EMCS/GetListArmada",
        columns: columnArmada,
        pagination: true,
        pagesize: '10',
        cache: false,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbarAlign: 'left',
        queryParams: function (params) {

            return {
                IdGr: $('#formId').val(),
                Id: 0,
            };
        },
        onClickRow: selectRow,
        showColumns: false,
        showRefresh: false,
        smartDisplay: false,
        formatNoMatches: function () {
            return '<span class="noMatches">Not Data Found</span>';
        },
        success: function (data) {

        }
    });
    $tablearmadaForRFC.bootstrapTable({
        url: "/EMCS/GetListArmadaForRFC",
        columns: columnArmadaForRFC,
        pagination: true,
        pagesize: '10',
        cache: false,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbarAlign: 'left',
        queryParams: function (params) {
            return {
                IdGr: $('#formId').val(),
            };
        },
        onClickRow: selectRow,
        showColumns: false,
        showRefresh: false,
        smartDisplay: false,
        formatNoMatches: function () {
            return '<span class="noMatches">Not Data Found</span>';
        },
        success: function (data) {

        }
    });
    $tableBlAwb.bootstrapTable(
        {
            url: "/EMCS/GetBlAwbListByCargo",
            columns: blawbcolumns,
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
            queryParams: function (params) {
                return {
                    limit: params.limit,
                    offset: params.offset,
                    Cargoid: $("#formId").val(),
                    sort: params.sort,
                    order: params.order,
                };
            },
            responseHandler: function (resp) {
                var data = {};
                $.map(resp, function (value, key) {
                    data[value.Key] = value.Value;
                });
                return data;
            },
            formatNoMatches: function () {
                return '<span class="noMatches">No Data Found</span>';
            },

        });
    $tableBlAwbRFC.bootstrapTable({
        url: "/EMCS/GetBlAwbRFCChangeList",
        columns: blawbcolumnsRFC,
        pagination: true,
        pagesize: '5',
        cache: false,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbarAlign: 'left',
        queryParams: function (params) {
            return {
                //limit: params.limit,
                //offset: params.offset,
                id: $("#formId").val(),
                //sort: params.sort,
                //order: params.order,
            };
        },
        onClickRow: selectRow,
        showColumns: false,
        showRefresh: false,
        smartDisplay: false,
        formatNoMatches: function () {
            return '<span class="noMatches">Not Data Found</span>';
        },
        responseHandler: function (resp) {
            //var data = {};
            //$.map(resp, function (value, key) {
            //    data[value.Key] = value.Value;
            //});
            return resp;
        },
        success: function (data) {

        }

    });

});
