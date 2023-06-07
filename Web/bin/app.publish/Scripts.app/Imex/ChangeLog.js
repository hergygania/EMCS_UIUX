
$(document).ready(function () {
    initData();
});

function initData() {
    $('#tableNewest').bootstrapTable({
        method: 'GET',
        url: baseUrl + '/ChangeLog/GetChangeLogNewest',
        toolbar: '#table-toolbar',
        crossDomain: true,
        striped: true,
        pagination: true,
        pageSize: 10,
        pageList: [10, 25, 50, 100, 200],
        search: false,
        showColumns: false,
        showRefresh: false,
        minimumCountColumns: 2,
        clickToSelect: true,
        smartDisplay: false,
        rowStyle: function () {
            return {
                //classes: 'text-nowrap another-class',
                css: { "font-size": "14px", "padding": "15px 10px" }
            };
        },
        columns: [{
            field: 'PartsNumber',
            title: 'PartsNumber',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false,
            width: '15%'
        }, {
            field: 'HSCodeOld',
            title: 'HS Code Old',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
        },
        {
            field: 'HSCodeNew',
            title: 'HS Code New',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
        }, {
            field: 'OMOld',
            title: 'OM Old',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'OMNew',
            title: 'OM New',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
            }
            //, {
            //field: 'UpdatedBy',
            //title: 'Updated By',
            //halign: 'center',
            //align: 'right',
            //sortable: true,
            //width: '5%'
            //}
            , {
            field: 'UpdatedDate',
            title: 'Updated Date',
            halign: 'center',
            align: 'right',
            sortable: true
        }
            //, {
            //	field: 'machine',
            //	title: 'Machine',
            //	halign: 'center',
            //	align: 'left',
            //	sortable: false
            //}
        ]
    });

    $('#tableDaily').bootstrapTable({
        method: 'GET',
        url: baseUrl + '/ChangeLog/GetChangeLogDaily',
        toolbar: '#table-toolbar',
        crossDomain: true,
        striped: true,
        pagination: true,
        pageSize: 10,
        pageList: [10, 25, 50, 100, 200],
        search: false,
        showColumns: false,
        showRefresh: false,
        minimumCountColumns: 2,
        clickToSelect: true,
        smartDisplay: false,
        rowStyle: function () {
            return {
                //classes: 'text-nowrap another-class',
                css: { "font-size": "14px", "padding": "15px 10px" }
            };
        },
        columns: [{
            field: 'PartsNumber',
            title: 'PartsNumber',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false,
            width: '15%'
        }, {
            field: 'HSCodeOld',
            title: 'HS Code Old',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
        },
        {
            field: 'HSCodeNew',
            title: 'HS Code New',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
        }, {
            field: 'OMOld',
            title: 'OM Old',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'OMNew',
            title: 'OM New',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
            }
            //, {
            //field: 'UpdatedBy',
            //title: 'Updated By',
            //halign: 'center',
            //align: 'right',
            //sortable: true,
            //width: '5%'
            //}
            , {
            field: 'UpdatedDate',
            title: 'Updated Date',
            halign: 'center',
            align: 'right',
            sortable: true
        }
            //, {
            //	field: 'machine',
            //	title: 'Machine',
            //	halign: 'center',
            //	align: 'left',
            //	sortable: false
            //}
        ]
    });

    $('#tableWeekly').bootstrapTable({
        method: 'GET',
        url: baseUrl + '/ChangeLog/GetChangeLogWeekly',
        toolbar: '#table-toolbar',
        crossDomain: true,
        striped: true,
        pagination: true,
        pageSize: 10,
        pageList: [10, 25, 50, 100, 200],
        search: false,
        showColumns: false,
        showRefresh: false,
        minimumCountColumns: 2,
        clickToSelect: true,
        smartDisplay: false,
        rowStyle: function () {
            return {
                //classes: 'text-nowrap another-class',
                css: { "font-size": "14px", "padding": "15px 10px" }
            };
        },
        columns: [{
            field: 'PartsNumber',
            title: 'PartsNumber',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false,
            width: '15%'
        }, {
            field: 'HSCodeOld',
            title: 'HS Code Old',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
        },
        {
            field: 'HSCodeNew',
            title: 'HS Code New',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
        }, {
            field: 'OMOld',
            title: 'OM Old',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'OMNew',
            title: 'OM New',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
            }
            //, {
            //field: 'UpdatedBy',
            //title: 'Updated By',
            //halign: 'center',
            //align: 'right',
            //sortable: true,
            //width: '5%'
            //}
            , {
            field: 'UpdatedDate',
            title: 'Updated Date',
            halign: 'center',
            align: 'right',
            sortable: true
        }
            //, {
            //	field: 'machine',
            //	title: 'Machine',
            //	halign: 'center',
            //	align: 'left',
            //	sortable: false
            //}
        ]
    });

    $('#tableMonthly').bootstrapTable({
        method: 'GET',
        url: baseUrl + '/ChangeLog/GetChangeLogMonthly',
        toolbar: '#table-toolbar',
        crossDomain: true,
        striped: true,
        pagination: true,
        pageSize: 10,
        pageList: [10, 25, 50, 100, 200],
        search: false,
        showColumns: false,
        showRefresh: false,
        minimumCountColumns: 2,
        clickToSelect: true,
        smartDisplay: false,
        rowStyle: function () {
            return {
                //classes: 'text-nowrap another-class',
                css: { "font-size": "14px", "padding": "15px 10px" }
            };
        },
        columns: [{
            field: 'PartsNumber',
            title: 'PartsNumber',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false,
            width: '15%'
        }, {
            field: 'HSCodeOld',
            title: 'HS Code Old',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
        },
        {
            field: 'HSCodeNew',
            title: 'HS Code New',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
        }, {
            field: 'OMOld',
            title: 'OM Old',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'OMNew',
            title: 'OM New',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
        }
            //    , {
            //    field: 'UpdatedBy',
            //    title: 'Updated By',
            //    halign: 'center',
            //    align: 'right',
            //    sortable: true,
            //    width: '5%'
            //}
            , {
            field: 'UpdatedDate',
            title: 'Updated Date',
            halign: 'center',
            align: 'right',
            sortable: true
        }
            //, {
            //	field: 'machine',
            //	title: 'Machine',
            //	halign: 'center',
            //	align: 'left',
            //	sortable: false
            //}
        ]
    });

    $('#tableRange').bootstrapTable({
        method: 'GET',
        url: baseUrl + '/ChangeLog/GetChangeLogByDate',
        toolbar: '#table-toolbar',
        crossDomain: true,
        striped: true,
        pagination: true,
        pageSize: 10,
        pageList: [10, 25, 50, 100, 200],
        search: false,
        showColumns: false,
        showRefresh: false,
        minimumCountColumns: 2,
        queryParams: function (params) {
            return {
                'dateFromW': $('#dateFrom').val(),
                'dateToW': $('#dateTo').val()
            };
        },
        clickToSelect: true,
        smartDisplay: false,
        rowStyle: function () {
            return {
                //classes: 'text-nowrap another-class',
                css: { "font-size": "14px", "padding": "15px 10px" }
            };
        },
        columns: [{
            field: 'PartsNumber',
            title: 'PartsNumber',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false,
            width: '15%'
        }, {
            field: 'HSCodeOld',
            title: 'HS Code Old',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
        },
        {
            field: 'HSCodeNew',
            title: 'HS Code New',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
        }, {
            field: 'OMOld',
            title: 'OM Old',
            halign: 'center',
            align: 'left',
            sortable: true
        }, {
            field: 'OMNew',
            title: 'OM New',
            halign: 'center',
            align: 'left',
            sortable: true,
            switchable: false
        }
        //    , {
        //    field: 'UpdatedBy',
        //    title: 'Updated By',
        //    halign: 'center',
        //    align: 'right',
        //    sortable: true,
        //    width: '5%'
        //}
            , {
            field: 'UpdatedDate',
            title: 'Updated Date',
            halign: 'center',
            align: 'right',
            sortable: true
        }
            //, {
            //	field: 'machine',
            //	title: 'Machine',
            //	halign: 'center',
            //	align: 'left',
            //	sortable: false
            //}
        ]
    });
}

function getData() {
    $('#tableRange').bootstrapTable('refresh', { url: baseUrl + 'ChangeLog/GetChangeLogByDate' });

}

function DownloadChangeLog(spName) {
    data = {
        SPName: 'mp.spGetChangeLogNewest'
    };

    data = JSON.stringify(data);
    enableLink(false);
    $.ajax({
        url: baseUrl + '/ChangeLog/DownloadChangeLog' + '?SPName=' + spName,
        type: 'GET',
        success: function (guid) {
            enableLink(true);
            window.open('DownloadToExcel?guid=' + guid, '_blank');
        },
        cache: false,
        contentType: false,
        processData: false
    });
}

function DownloadChangeLogByDate() {
    enableLink(false);
    $.ajax({
        url: baseUrl + '/ChangeLog/DownloadChangeLogByDate' + '?dateFromW=' + $('#dateFrom').val() + '&dateToW=' + $('#dateTo').val(),
        type: 'GET',
        success: function (guid) {
            enableLink(true);
            window.open('DownloadToExcel?guid=' + guid, '_blank');
        },
        cache: false,
        contentType: false,
        processData: false
    });
}
