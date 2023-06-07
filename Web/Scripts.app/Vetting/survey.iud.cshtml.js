window.operateSurvey = {
    'click .remove': function (e, value, row, index) {
        $.ajax({
            url: '/vetting-process/SurveyDelete',
            type: 'post',
            data: { id: row.SurveyDetailID },
            success: function (result) {
                refreshSurveyPart(row.SurveyID);
            }
        });
    },
    'click .remove2': function (e, value, row, index) {
        $.ajax({
            url: '/vetting-process/SurveyAttachmentDelete',
            type: 'post',
            data: { id: row.SurveyDocumentID },
            success: function (result) {
                refreshSurveyDocument(row.SurveyID);
            }
        });
    }

};


$(function () {
    var $tblSurveyPart = $('#tableSurveyPart');
    var $tbltableAttachment = $('#tableAttachment');

    var $tablePart = $('#tablePartsOrder'); //table in PartsOrder.listCheck.hs.cshtml
    var checkedRows = [];

    $('.cal').click(function () {
        $('#VRDate').datepicker('show');
    });

    $('#modalPartOrderList').on('shown.bs.modal', function (e) {
        if ($('#CommodityID').val() == '') {
            $('#modalPartOrderList').modal('toggle');
            alert('Please select commodity ..!')
            return;
        }
        showMainScrollbar(false);
        checkedRows = [];
    });


    $tablePart.on('check-all.bs.table', function (e, object) {
        $.each(object, function (index, row) {

            checkedRows.push({ DetailID: row.DetailID });
            //checkedRows.push({ DetailID: row.DetailID, InvoiceNo: row.InvoiceNo, InvoiceDate: dateFormatter(row.InvoiceDate), HSCode: row.HSCode, PartsNumber: row.PartsNumber });
        });
        console.log('check-all.bs.table:');
        console.log(checkedRows);
    });

    $tablePart.on('uncheck-all.bs.table', function (e, object) {
        $.each(object, function (index, row) {

            $.each(checkedRows, function (index, value) {
                if (value != null && value.DetailID === row.DetailID) {
                    checkedRows.splice(index, 1);
                }
            });

        });
        console.log('uncheck-all.bs.table:');
        console.log(checkedRows);
    });

    $tablePart.on('check.bs.table', function (e, row) {
        checkedRows.push({ DetailID: row.DetailID });
        console.log(checkedRows);
    });

    $tablePart.on('uncheck.bs.table', function (e, row) {
        $.each(checkedRows, function (index, value) {
            if (value != null && value.DetailID === row.DetailID) {
                checkedRows.splice(index, 1);
            }
        });
        console.log(checkedRows);
    });

    /* trigger when click from child modal (PartsOrder.listCheck.cshtml) */
    $('#modalPartOrderList #bntSelectSx').click(function () {
        showMainScrollbar(true);
        enableLink(false);

        $.ajax({
            url: '/vetting-process/SurveyGetParts',
            type: 'post',
            data: { arrObject: JSON.stringify(checkedRows) }, ///$(frm).serialize(),
            success: function (result) {
                refreshSurveyPart();
                checkedRows = [];
            }
        });
    });



    refreshSurveyPart = function (surveyId) {
        enableLink(false);
        createTableSurveyPart();
        surveyId = (surveyId == undefined ? -1 : surveyId);

        window.pis.table({
            objTable: $tblSurveyPart,
            urlSearch: '/vetting-process/SurveyPartPage',
            urlPaging: '/vetting-process/SurveyPartPageXt',
            searchParams: {
                surveyId: surveyId,
            },
            autoLoad: true
        });
    };

    refreshSurveyDocument = function (surveyId) {
        enableLink(false);
        createTableAttachment();
        surveyId = (surveyId == undefined ? -1 : surveyId);

        window.pis.table({
            objTable: $tbltableAttachment,
            urlSearch: '/vetting-process/SurveyDocumentPage',
            urlPaging: '/vetting-process/SurveyDocumentPageXt',
            searchParams: {
                surveyId: surveyId,
            },
            autoLoad: true
        });
    };

    createTableSurveyPart = function () {
        $tblSurveyPart.bootstrapTable({
            cache: false,
            pagination: true,
            search: false,
            striped: true,
            clickToSelect: true,
            reorderableColumns: true,
            toolbarAlign: 'left',
            onClickRow: selectRow,
            sidePagination: 'server',
            showColumns: true,
            showRefresh: false,
            smartDisplay: false,
            toolbar: '.toolbarSurvey',
            pageSize: '5',
            formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
            columns: [{
                field: 'action',
                title: 'Action',
                width: '99px',
                align: 'center',
                formatter: operateFormatter({ Delete: true }),
                events: operateSurvey,
                class: 'noExl',
                switchable: false
            }, {
                field: 'no',
                title: 'No',
                halign: 'center',
                align: 'center',
                width: '77px',
                formatter: runningFormatter
            }, {
                field: 'CaseNo',
                title: 'Case Number',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'PartsNumber',
                title: 'Parts No',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'PartsName',
                title: 'Parts Description',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'InvoiceItemQty',
                title: 'Qty',
                halign: 'right',
                align: 'right',
                sortable: true
            }, {
                field: 'HSCode',
                title: 'HS Code',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'PartGrossWeight',
                title: 'Nett WT',
                halign: 'right',
                align: 'right',
                sortable: true
            }, {
                field: 'TotalWeight',
                title: 'Total WT',
                halign: 'right',
                align: 'right',
                sortable: true
            }, {
                field: 'InvoiceNo',
                title: 'Invoice No',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'InvoiceDate',
                title: 'Invoice Date',
                halign: 'right',
                align: 'right',
                sortable: true,
                width: '101px',
                formatter: 'dateFormatter'
            }
            ]
        });
    };

    createTableAttachment = function () {
        $tbltableAttachment.bootstrapTable({
            cache: false,
            pagination: true,
            search: false,
            striped: true,
            clickToSelect: true,
            reorderableColumns: true,
            toolbarAlign: 'left',
            onClickRow: selectRow,
            sidePagination: 'server',
            showColumns: true,
            showRefresh: false,
            smartDisplay: false,
            toolbar: '.toolbarAttachment',
            pageSize: '5',
            formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
            columns: [{
                field: 'action',
                title: 'Action',
                width: '150px',
                align: 'center',
                formatter: operateFormatter({ Delete2: true }),
                events: operateSurvey,
                switchable: false
            }, {
                field: 'no',
                title: 'No',
                halign: 'center',
                align: 'left',
                sortable: true,
                formatter: runningFormatter
            }, {
                field: 'DocumentName',
                title: 'Document Name',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'FileName',
                title: 'Attachment File',
                halign: 'center',
                align: 'left',
                sortable: true,
                formatter: attachmentFormatter
            }
            ]
        });
    };

    createTableSurveyPart();
    createTableAttachment();

    //edit mode
    if ($('#SurveyID').val() != '' && $('#SurveyID').val() != -1) {
        refreshSurveyPart($('#SurveyID').val());
        refreshSurveyDocument($('#SurveyID').val());
    };

    $(".downloadExcel").click(function () {
        //var id = $('#SurveyID').val();		
        $('#xls [name=VRNo]').val($('#VRNo').val());
        $('#xls [name=VRDate]').val($('#VRDate').val());
        $('#xls [name=CommodityID]').val($('#CommodityID').val());
        //$(".table2excel").table2excel({exclude: ".noExl",filename: "survey"});
        return $('#xls').submit();
    });

});

function attachmentFormatter(value, row, index) {
    return '<a href="' + folderdoc + row.DocumentTypeID + '/' + value + '" target="_blank">' + value + " " + '</a>' + (row.dml == '(duplicate)' ? '<span style="color:red"> (duplicate)</span>' : '');
};

function operateFormatter(options) {
    var btn = [];
    btn.push('<div class="btn-group">');
    if (options.Add == true)
        btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>')
    if (options.Edit == true)
        btn.push('<button type="button" class="btn btn-xs btn-info edit" title="Edit"><i class="fa fa-edit"></i></button>');
    if (options.Delete == true)
        btn.push('<button type="button" class="btn btn-xs btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    if (options.Delete2 == true)
        btn.push('<button type="button" class="btn btn-xs btn-danger remove2" title="Delete"><i class="fa fa-trash-o"></i></button>');
    if (options.Detail == true)
        btn.push('<button type="button" class="btn btn-xs btn-info detail" title="Detail" data-toggle="modal" data-target="#mymodal"><i class="fa fa-search-plus"></i> Detail</button>')
    if (options.Detail2 == true)
        btn.push('<button type="button" class="btn btn-xs btn-info detail" title="Detail" data-toggle="modal" data-target="#mdlDetail"><i class="fa fa-search-plus"></i> Detail</button>')

    btn.push('</div>');

    return btn.join('');
}

