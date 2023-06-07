$(function () {

   
        $('#StagingDateid').focus().datepicker({
            format: " mm/dd/yyyy",
            minDate: 0//,
            //startDate: "today"
        });


});

$("#selectstagingtable").change(function () {
    var $table = $('#tableStaging');
    $table.bootstrapTable('destroy');



    StagingHistory($("#selectstagingtable").val())
});


function StagingHistory(StagingShow) {
   
    if (StagingShow == "Staging4Bn48R") {
        BoostraptableStaging4Bn48R();
    } else if (StagingShow == "StagingCORE") {
        BoostraptableStagingCORE();
    } else if (StagingShow == "StagingInventoryAdjustment") {
        BoostraptableStagingIA();
    } else if (StagingShow == "StagingCreateBER") {
        BoostraptableStagingCreateBER();
    } else if (StagingShow == "StagingCreateJC") {
        BoostraptableStagingCreateJC();
    } else if (StagingShow == "StagingCreateMO") {
        BoostraptableStagingCreateMO();
    } else if (StagingShow == "StagingCreateSQ") {
        BoostraptableStagingCreateSQ();
    } else if (StagingShow == "StagingCreateST") {
        BoostraptableStagingCreateST();
    } else if (StagingShow == "StagingCreateWIP") {
        BoostraptableStagingCreateWIP();
    } else if (StagingShow == "StagingDeleteDocRW") {
        BoostraptableStagingDeleteDocRW();
    } else if (StagingShow == "StagingDeleteMO") {
        BoostraptableStagingDeleteMO();
    } else if (StagingShow == "StagingReceivedMO") {
        BoostraptableStagingReceivedMO();
    } else if (StagingShow == "StagingReceivedST") {
        BoostraptableStagingReceivedST();
    } else if (StagingShow == "StagingSales500") {
        BoostraptableStagingSales500();
    } else if (StagingShow == "StagingSales800") {
        BoostraptableStagingSales800();
    } else
    {

    }

   
}

function hideAlldiv() {
    $("#div4Bn48R").hide();
    $("#divIA").hide();
    $("#divCore").hide();
    $("#divBER").hide();
    $("#divJC").hide();
    $("#divMO").hide();
    $("#divSQ").hide();
    $("#divST").hide();
    $("#divWIP").hide();
    $("#divDelRW").hide();
    $("#divDelMO").hide();
    $("#divReceivedMO").hide();
    $("#divReceivedST").hide();
    $("#div500").hide();
    $("#div800").hide();
}

function BoostraptableStaging4Bn48R() {
    hideAlldiv();
    $("#div4Bn48R").show();
    var $table = $('#tableStaging');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'StoreNo',
            title: 'Store No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'SOS',
		    title: 'SOS',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'PartNo',
		    title: 'Part No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Description',
		    title: 'Description',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Qty',
		    title: 'Qty',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Dlrinfofield',
		    title: 'Dlrinfofield',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'TranCode',
		    title: 'Transaction Code',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'ReceivedDate',
		    title: 'Received Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'DOC_C',
		    title: 'Doc C',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'LastUpdateDate',
		    title: 'Last Update Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'Flag',
		    title: 'Flag',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: operateFormatterReadDoc
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStaging4Bn48R',
        urlPaging: '/cat/InitilizeStaging4Bn48R',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingIA() {
    hideAlldiv();
    $("#divIA").show();
    var $table = $('#tableIA');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'StoreNo',
            title: 'Store No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'SOS',
		    title: 'SOS',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'PartNo',
		    title: 'Part No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Description',
		    title: 'Description',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Qty',
		    title: 'Qty',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'WorkorderNo',
		    title: 'Work Order',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'DlrInfoField',
		    title: 'Dlr Info Field',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Notes',
		    title: 'Notes',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'TranCode',
		    title: 'Trancode',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'ReceivedDate',
		    title: 'Received Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'LastUpdateDate',
		    title: 'Last Update Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'Flag',
		    title: 'Flag',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: operateFormatterReadDoc
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingIA',
        urlPaging: '/cat/InitilizeStagingIA',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingCORE() {
    hideAlldiv();
    $("#divCore").show();
    var $table = $('#tableCORE');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'StoreNo',
            title: 'Store No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'SOS',
		    title: 'SOS',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'PartNo',
		    title: 'Part No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Description',
		    title: 'Description',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Qty',
		    title: 'Qty',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'RefDoc',
		    title: 'Doc Sales',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'ReturnDoc',
		    title: 'Doc Return',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'WCSL',
		    title: 'WCSL',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'DateSale',
		    title: 'Date Doc Sales',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateStringFormatterCAT
		},
		{
		    field: 'DateReturn',
		    title: 'Date Doc Return',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateStringFormatterCAT
		},
        {
            field: 'DateWCSL',
            title: 'Date Doc WCSL',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateStringFormatterCAT
        },
        {
            field: 'WorkorderNo',
            title: 'Workorder',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Seg',
            title: 'Segment',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'ManualOrder',
            title: 'Manual Order',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'LastUpdateDate',
		    title: 'Last Update Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'Flag',
		    title: 'Flag',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: operateFormatterReadDoc
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingCORE',
        urlPaging: '/cat/InitilizeStagingCORE',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingCreateBER() {
    hideAlldiv();
    $("#divBER").show();
    var $table = $('#tableBER');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'CRC_PCD',
            title: 'CRC PCD',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'JobLoc',
		    title: 'Job Location',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Job Code',
		    title: 'Job Code',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'RetrunAsZeroHour',
		    title: 'Retrun As Zero Hour',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'TUID',
		    title: 'TUID',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'DateRecived',
		    title: 'Date Received',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'JobInstruction',
		    title: 'Job Instruction',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'StandID',
		    title: 'Stand ID',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'NewWO',
		    title: 'New Workorder',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'WO1K',
		    title: 'Workorder 1 K',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'OldWO',
            title: 'Old Workorder',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'WCSL',
            title: 'WCSL',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Notes',
            title: 'Notes',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'CRR_ID',
            title: 'CRR ID',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'WIP_ID',
            title: 'WIP ID',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'StatusCMS',
            title: 'Status CMS',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'LastUpdateCMS',
            title: 'Last Update CMS',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
        {
            field: 'StartDate',
            title: 'Start Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
        {
            field: 'DaNumber',
            title: 'DA Number',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'ComponentCondition',
            title: 'Component Condition',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'RebuildStatus',
            title: 'Rebuid Status',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'CurrentRebuildActivity',
            title: 'Current Rebuild Activity',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Flag',
            title: 'Flag',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: operateFormatterReadDoc
        },
        {
            field: 'EntryDate',
            title: 'Entry Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
        {
            field: 'EntryBy',
            title: 'Entry By',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'ModifiedDate',
            title: 'Modified Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
		{
		    field: 'ModifiedBy',
		    title: 'Modified By',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingCreateBER',
        urlPaging: '/cat/InitilizeStagingCreateBER',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingCreateJC() {
    hideAlldiv();
    $("#divJC").show();
    var $table = $('#tableJC');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'CRC_PCD',
            title: 'CRC PCD',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'JobLoc',
		    title: 'Job Location',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'JobCode',
		    title: 'Job Code',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'RetrunAsZeroHour',
		    title: 'Retrun As Zero Hour',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'TUID',
		    title: 'TUID',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'DateRecived',
		    title: 'Date Received',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'JobInstruction',
		    title: 'Job Instruction',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'StandID',
		    title: 'Stand ID',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'NewWO',
		    title: 'New Workorder',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'WO1K',
		    title: 'Workorder 1 K',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'OldWO',
            title: 'Old Workorder',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'WCSL',
            title: 'WCSL',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Notes',
            title: 'Notes',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'CRR_ID',
            title: 'CRR ID',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'WIP_ID',
            title: 'WIP ID',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'StatusCMS',
            title: 'Status CMS',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'LastUpdateCMS',
            title: 'Last Update CMS',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
        {
            field: 'StartDate',
            title: 'Start Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
        {
            field: 'DaNumber',
            title: 'DA Number',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'ComponentCondition',
            title: 'Component Condition',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'RebuildStatus',
            title: 'Rebuid Status',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'CurrentRebuildActivity',
            title: 'Current Rebuild Activity',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'CRC_Completion',
            title: 'CRC Completion',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Flag',
            title: 'Flag',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: operateFormatterReadDoc
        },
        {
            field: 'EntryDate',
            title: 'Entry Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
        {
            field: 'EntryBy',
            title: 'Entry By',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'ModifiedDate',
            title: 'Modified Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
		{
		    field: 'ModifiedBy',
		    title: 'Modified By',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingCreateJC',
        urlPaging: '/cat/InitilizeStagingCreateJC',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingCreateMO() {
    hideAlldiv();
    $("#divMO").show();
    var $table = $('#tableMO');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'StoreNo',
            title: 'Store No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'SOS',
		    title: 'SOS',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'PartNo',
		    title: 'Part No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Description',
		    title: 'Description',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Qty',
		    title: 'Qty',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Morder',
		    title: 'M Order',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'WorkorderNo',
		    title: 'Workorder No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'DOCC',
            title: 'Doc Sales',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'DOCR',
            title: 'Doc Return',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'DOCW',
		    title: 'WCSL',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'ReceivedDate',
            title: 'Received Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
		{
		    field: 'LastUpdateDate',
		    title: 'Last Update Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'Flag',
		    title: 'Flag',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: operateFormatterReadDoc
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingCreateMO',
        urlPaging: '/cat/InitilizeStagingCreateMO',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingCreateSQ() {
    hideAlldiv();
    $("#divSQ").show();
    var $table = $('#tableSQ');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'CRC_PCD',
            title: 'CRC PCD',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'JobLoc',
		    title: 'Job Location',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'JobCode',
		    title: 'Job Code',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'RetrunAsZeroHour',
		    title: 'Retrun As Zero Hour',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'TUID',
		    title: 'TUID',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'DateRecived',
		    title: 'Date Received',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'JobInstruction',
		    title: 'Job Instruction',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'StandID',
		    title: 'Stand ID',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'NewWO',
		    title: 'New Workorder',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'WO1K',
		    title: 'Workorder 1 K',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'OldWO',
            title: 'Old Workorder',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'WCSL',
            title: 'WCSL',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Notes',
            title: 'Notes',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'CRR_ID',
            title: 'CRR ID',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'WIP_ID',
            title: 'WIP ID',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'StatusCMS',
            title: 'Status CMS',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'LastUpdateCMS',
            title: 'Last Update CMS',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
        {
            field: 'StartDate',
            title: 'Start Date',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'DaNumber',
            title: 'DA Number',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'ComponentCondition',
            title: 'Component Condition',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'RebuildStatus',
            title: 'Rebuid Status',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'CurrentRebuildActivity',
            title: 'Current Rebuild Activity',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Flag',
            title: 'Flag',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: operateFormatterReadDoc
        },
        {
            field: 'EntryDate',
            title: 'Entry Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
        {
            field: 'EntryBy',
            title: 'Entry By',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'ModifiedDate',
            title: 'Modified Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
		{
		    field: 'ModifiedBy',
		    title: 'Modified By',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingCreateSQ',
        urlPaging: '/cat/InitilizeStagingCreateSQ',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingCreateST() {
    hideAlldiv();
    $("#divST").show();
    var $table = $('#tableST');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'StoreNo',
            title: 'Store No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'SOS',
		    title: 'SOS',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'PartNo',
		    title: 'Part No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Description',
		    title: 'Description',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Qty',
		    title: 'Qty',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'WorkorderNo',
            title: 'Workorder',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'StoreTo',
            title: 'Store To',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'DlrInfoField',
            title: 'Dlr Info Field',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'SNUnit',
            title: 'SN Unit',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'UnitNo',
            title: 'Unit No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'TranCode',
            title: 'Tran Code',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'ReceivedDate',
            title: 'Received Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
		{
		    field: 'LastUpdateDate',
		    title: 'Last Update Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'Flag',
		    title: 'Flag',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: operateFormatterReadDoc
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingCreateST',
        urlPaging: '/cat/InitilizeStagingCreateST',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingCreateWIP() {
    hideAlldiv();
    $("#divWIP").show();
    var $table = $('#tableWIP');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'CRC_PCD',
            title: 'CRC PCD',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
		{
		    field: 'JobLoc',
		    title: 'Job Location',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'JobCode',
		    title: 'Job Code',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'RetrunAsZeroHour',
		    title: 'Retrun As Zero Hour',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'TUID',
		    title: 'TUID',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'DateRecived',
		    title: 'Date Received',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'JobInstruction',
		    title: 'Job Instruction',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'StandID',
		    title: 'Stand ID',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'NewWO',
		    title: 'New Workorder',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'WO1K',
		    title: 'Workorder 1 K',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'OldWO',
            title: 'Old Workorder',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'WCSL',
            title: 'WCSL',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Notes',
            title: 'Notes',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'CRR_ID',
            title: 'CRR ID',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'WIP_ID',
            title: 'WIP ID',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'StatusCMS',
            title: 'Status CMS',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'LastUpdateCMS',
            title: 'Last Update CMS',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'StartDate',
            title: 'Start Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
        {
            field: 'DaNumber',
            title: 'DA Number',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'ComponentCondition',
            title: 'Component Condition',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'RebuildStatus',
            title: 'Rebuid Status',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'CurrentRebuildActivity',
            title: 'Current Rebuild Activity',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Flag',
            title: 'Flag',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: operateFormatterReadDoc
        },
        {
            field: 'EntryDate',
            title: 'Entry Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
        {
            field: 'EntryBy',
            title: 'Entry By',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'ModifiedDate',
            title: 'Modified Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
		{
		    field: 'ModifiedBy',
		    title: 'Modified By',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingCreateWIP',
        urlPaging: '/cat/InitilizeStagingCreateWIP',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingDeleteDocRW() {
    hideAlldiv();
    $("#divDelRW").show();
    var $table = $('#tableDelRW');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'StoreNo',
            title: 'Store No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'SOS',
		    title: 'SOS',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'PartNo',
		    title: 'Part No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Description',
		    title: 'Description',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Qty',
		    title: 'Qty',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Morder',
		    title: 'M Order',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'WorkorderNo',
		    title: 'Workorder No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'DOCC',
            title: 'Doc Sales',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'DOCR',
            title: 'Doc Return',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'DOCW',
		    title: 'WCSL',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'ReceivedDate',
            title: 'Received Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
		{
		    field: 'LastUpdateDate',
		    title: 'Last Update Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'Flag',
		    title: 'Flag',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: operateFormatterReadDoc
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingDeleteDocRW',
        urlPaging: '/cat/InitilizeStagingDeleteDocRW',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingDeleteMO() {
    hideAlldiv();
    $("#divDelMO").show();
    var $table = $('#tableDelMO');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'StoreNo',
            title: 'Store No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'SOS',
		    title: 'SOS',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'PartNo',
		    title: 'Part No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Description',
		    title: 'Description',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Qty',
		    title: 'Qty',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Morder',
		    title: 'M Order',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'WorkorderNo',
		    title: 'Workorder No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'DOCC',
            title: 'Doc Sales',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'DOCR',
            title: 'Doc Return',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'DOCW',
		    title: 'WCSL',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'ReceivedDate',
            title: 'Received Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateFormatterCAT
        },
		{
		    field: 'LastUpdateDate',
		    title: 'Last Update Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'Flag',
		    title: 'Flag',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: operateFormatterReadDoc
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingDeleteMO',
        urlPaging: '/cat/InitilizeStagingDeleteMO',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingReceivedMO() {
    hideAlldiv();
    $("#divReceivedMO").show();
    var $table = $('#tableReceivedMO');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'StoreNo',
            title: 'Store No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'SOS',
		    title: 'SOS',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'PartNo',
		    title: 'Part No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Description',
		    title: 'Description',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Qty',
		    title: 'Qty',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'WorkorderNo',
            title: 'Workorder',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'DlrInfoField',
            title: 'Dlr Info Field',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'TranCode',
            title: 'Transaction Code',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'ReceivedDate',
		    title: 'Received Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'Flag',
		    title: 'Flag',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: operateFormatterReadDoc
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingReceivedMO',
        urlPaging: '/cat/InitilizeStagingReceivedMO',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function operateFormatterReadDoc(value, row, index) {
    if (row.Flag == 5 && row.Remainingqty > 0) {
        return "Document sisa " + row.Remainingqty + " Qty.";
    }
   
};

function BoostraptableStagingReceivedST() {
    hideAlldiv();
    $("#divReceivedST").show();
    var $table = $('#tableReceivedST');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'StoreNo',
            title: 'Store No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'SOS',
		    title: 'SOS',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'PartNo',
		    title: 'Part No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Description',
		    title: 'Description',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Qty',
		    title: 'Qty',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'WorkorderNo',
            title: 'Workorder',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'FromStore',
            title: 'From Store',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'DlrInfoField',
            title: 'Dlr Info Field',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'TranCode',
            title: 'Transaction Code',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'ReceivedDate',
		    title: 'Received Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'Flag',
		    title: 'Flag',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: operateFormatterReadDoc
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingReceivedST',
        urlPaging: '/cat/InitilizeStagingReceivedST',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingSales500() {
    hideAlldiv();
    $("#div500").show();
    var $table = $('#table500');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'StoreNo',
            title: 'Store No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'SOS',
		    title: 'SOS',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'PartNo',
		    title: 'Part No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Description',
		    title: 'Description',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Qty',
		    title: 'Qty',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'EquipmentNumber',
            title: 'Equipment Number',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Customer_ID',
            title: 'Customer ID',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'CustomerName',
            title: 'Customer Name',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'DocDate',
            title: 'Doc Date',
            halign: 'center',
            align: 'left',
            sortable: true,
            formatter: dateStringFormatterCAT
        },
        {
            field: 'WorkorderNo',
            title: 'Workorder',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'SEG',
            title: 'Segment',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'DocSale',
            title: 'Doc Sale',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'TranCode',
            title: 'Tran Code',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'LastUpdateDate',
		    title: 'Last Update Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'Flag',
		    title: 'Flag',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: operateFormatterReadDoc
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingSales500',
        urlPaging: '/cat/InitilizeStagingSales500',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}

function BoostraptableStagingSales800() {
    hideAlldiv();
    $("#div800").show();
    var $table = $('#table800');
    $table.bootstrapTable('destroy');
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');


    $table.bootstrapTable({
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
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'StoreNo',
            title: 'Store No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'SOS',
		    title: 'SOS',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'PartNo',
		    title: 'Part No',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Description',
		    title: 'Description',
		    halign: 'center',
		    align: 'left',
		    sortable: true
		},
		{
		    field: 'Qty',
		    title: 'Qty',
		    halign: 'right',
		    align: 'left',
		    sortable: true
		},
        {
            field: 'SNUnit',
            title: 'SN Unit',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'UnitNo',
            title: 'Unit No',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'DlrInfoField',
            title: 'Dlr Info Field',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Customer_ID',
            title: 'Customer ID',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'CustomerName',
            title: 'Customer Name',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'TranCode',
            title: 'Tran Code',
            halign: 'center',
            align: 'left',
            sortable: true
        },
		{
		    field: 'LastUpdateDate',
		    title: 'Last Update Date',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: dateFormatterCAT
		},
		{
		    field: 'Flag',
		    title: 'Flag',
		    halign: 'center',
		    align: 'left',
		    sortable: true,
		    formatter: operateFormatterReadDoc
		}]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/cat/InitilizeStagingSales800',
        urlPaging: '/cat/InitilizeStagingSales800',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
}