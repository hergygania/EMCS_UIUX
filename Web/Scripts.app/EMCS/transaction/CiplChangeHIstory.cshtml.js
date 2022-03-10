$tableChangeHistory = $('#tableCiplChangeHistory');

var columnChangeHistory = [
    {
        field: 'FieldName',
        title: 'FieldName',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }
    ,{
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
$('#btnApproveChangeHistory').on('click', function () {
    paramSearch = {
        idTerm: $('#idCipl').val(),
        formtype:'CIPL'
    };
    $.ajax({
        url: '/EMCS/ApproveChangeHistory',
        type: 'POST',
        data: paramSearch,
        success: function (guid) {

        },
        cache: false
    });
});

$(function () {
    $.ajax({
        url: '/EMCS/GetChangeHistoryReason',
        type: 'POST',
        data: {
            IdTerm: $("#idCipl").val(),
            formtype:"CIPL"
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
                IdTerm: $("#idCipl").val(),
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
        columns: columnChangeHistory
    });
});