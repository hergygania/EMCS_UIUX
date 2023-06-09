$tableHistory = $('#tableCiplHistory');

var columnHistory = [
    {
        field: 'IdCipl',
        title: 'No',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: runningFormatterNoPaging
    },
    {
        field: 'CreateDate',
        title: 'Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY HH:mm:ss");
        }
    },
    //{
    //    field: 'Flow',
    //    title: 'Flow',
    //    halign: 'center',
    //    align: 'left',
    //    class: 'text-nowrap',
    //    sortable: true
    //},
    {
        field: 'Step',
        title: 'Step',
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
        field: 'ViewByUser',
        title: 'View By User',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Notes',
        title: 'Notes',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'CreateBy',
        title: 'PIC',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }];

$(function () {
    $tableHistory.bootstrapTable({
        cache: false,
        url: "/Emcs/GetHistoryList",
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
                IdTerm: $("#idCipl").val(),
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
            var url = window.location.href;
            if (url.indexOf("CiplApprove")) {
                $.each(data.rows, function (index, element) {
                    if (element.Status === 'SUBMIT' || element.Status === 'Submit') {
                        $('#SubmitDate').val(moment(element.CreateDate).format("DD MMM YYYY hh:mm:ss"));
                    }
                })

                var SubmitDate = new Date($('#SubmitDate').val());
                var DateTimeNow = new Date();
                var DifferenceTime = diff_hours(DateTimeNow, SubmitDate);
                if (DifferenceTime > 24) {
                    Swal.fire(
                        'Warning!',
                        'This Data Has Been Delayed Approve / Overdue Approve.',
                        'warning'
                    );
                }
            }

            return data;
        },
        columns: columnHistory
    });

    //window.pis.table({
    //    objTable: $tableHistory,
    //    urlSearch: '/EMCS/CiplHistoryPage/'+$("#idCipl").val(),
    //    urlPaging: '/EMCS/CiplHistoryPageXt',
    //    autoLoad: true
    //});

});