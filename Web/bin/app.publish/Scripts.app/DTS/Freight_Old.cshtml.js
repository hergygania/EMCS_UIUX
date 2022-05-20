$table = $("#tableFreight");
$form = $("#freightFormSearch");
var columnList;
async function populateColumn() {
    var columns = await getModelList();
    columnList = columns;
    return columns;
}

function formatMoney(amount, decimalCount = 2, decimal = ".", thousands = ",") {
    try {
        decimalCount = Math.abs(decimalCount);
        decimalCount = isNaN(decimalCount) ? 2 : decimalCount;

        const negativeSign = amount < 0 ? "-" : "";

        let i = parseInt(amount = Math.abs(Number(amount) || 0).toFixed(decimalCount)).toString();
        let j = (i.length > 3) ? i.length % 3 : 0;

        return negativeSign + (j ? i.substr(0, j) + thousands : '') + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousands) + (decimalCount ? decimal + Math.abs(amount - i).toFixed(decimalCount).slice(2) : "");
    } catch (e) {
        console.log(e)
    }
}

function getModelList() {
    $.ajax({
        type: "POST",
        async: false,
        url: myApp.root + 'DTS/GetUnitModel',
        dataType: "json",
        success: function (resp) {
            console.log(resp);
            columnList = [
                {
                    field: 'origin',
                    title: 'Origin',
                    halign: 'center',
                    align: 'left'
                },
                {
                    field: 'area',
                    title: 'Area',
                    align: 'left',
                    halign: 'center'
                },
                {
                    field: 'provinsi',
                    title: 'Provinsi',
                    halign: 'center',
                    align: 'left'
                },
                {
                    field: 'kabupatenkota',
                    title: 'Kabupaten Kota',
                    halign: 'center',
                    align: 'left'
                },
                {
                    field: 'ibukotakabupaten',
                    title: 'Ibu Kota Kab.',
                    halign: 'center',
                    align: 'left'
                }
            ];
            $.each(resp, function (index, item) {
                console.log('freight : ', item.Model.toLowerCase());

                columnList.push({
                    field: item.Model.toLowerCase(),
                    title: item.Model,
                    formatter: function (data, index, row) {
                        var newVal = formatMoney(data, 0, ',', '.');
                        console.log(newVal);
                        if (newVal === "0") {
                            return "n/a";
                        } else {
                            return newVal;
                        }
                    },
                    halign: 'center',
                    align: 'right'
                });
            });
            console.log('hasil adding : ', columnList);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });

    columnList = columnList;
    return columnList;
}

function getData(params) {
    console.log(params);
    var dataList;
    $.ajax({
        type: "POST",
        url: "/DTS/FreightCalculatorPage",
        dataType: "json",
        async: false,
        success: function (data) {
            console.log('dari url ', data);
            dataList = data.Data.result;
            console.log('line 85 : ', dataList);
        },
        error: function (er) {
            params.error(er);
        }
    });
    return dataList;
}

function formatRepo(data) {
    console.log('alimutasal line 117 : ', data);
}

$(document).ready(function () {
    $("#searchOrigin").select2({
        placeholder: "Select a Origin",
        minimumInputLength: 3,
        allowClear: true,
        ajax: {
            url: "/DTS/GetFreightOption",
            type: "POST",
            data: function (params) {
                return {
                    key: params.term, // search term
                    type: 'origin'
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
    });

    $("#searchDestination").select2({
        placeholder: "Select a Destination",
        minimumInputLength: 3,
        allowClear: true,
        ajax: {
            url: "/DTS/GetFreightOption",
            type: "POST",
            dataType: 'json',
            data: function (params) {
                return {
                    key: params.term, // search term
                    type: 'destination'
                };
            },
            processResults: function (data) {
                // Tranforms the top-level key of the response object from 'items' to 'results'
                console.log(data);
                var list = [];
                for (var i = 0; i < data.length; i++) {
                    list.push(data[i]);
                }

                return {
                    results: $.map(data, function (list) {
                        console.log('ali mutasal : ', data);
                        return {
                            text: list.Destination,
                            id: list.Destination
                        };
                    })
                };
            }
        }
    });

    $("#searchModel").select2({
        placeholder: "Select a Model",
        minimumInputLength: 3,
        allowClear: true,
        ajax: {
            url: "/DTS/GetFreightOption",
            type: "POST",
            dataType: 'json',
            data: function (params) {
                return {
                    key: params.term, // search term
                    type: 'model'
                };
            },
            processResults: function (data) {
                // Tranforms the top-level key of the response object from 'items' to 'results'
                console.log(data);
                var list = [];
                for (var i = 0; i < data.length; i++) {
                    list.push(data[i]);
                }

                return {
                    results: $.map(data, function (list) {
                        console.log('ali mutasal : ', data);
                        return {
                            text: list.Model,
                            id: list.Model
                        };
                    })
                };
            }
        }
    });

    $("#searchETD").datepicker({
        autoclose: true,
        format: 'dd M yyyy'
    });

    $("#searchFreight").on('click', function () {
        var origin = $("#searchOrigin").val();
        var destination = $("#searchDestination").val();
        var model = $("#searchModel").val();
        var etd = $("#searchETD").val();
        console.log('origin : ', origin);
        console.log('destination : ', destination);
        console.log('model : ', model);
        console.log('etd : ', etd);

        if (origin === "" || destination === "" || model === "" || etd === "") {
            swal("Information!", "Silahkan lengkapi Form Pencarian!", "error");
        } else {
            $.ajax({
                url: "/DTS/GetFreightDetail",
                type: "POST",
                dataType: 'json',
                data: {
                    origin: origin,
                    destination: destination,
                    model: model,
                    etd: etd
                },
                success: function (data) {
                    console.log('alimutasal ', data.length);
                    if (data.ID) {
                        var origin = data.Origin;
                        var total = formatMoney(data.Value, 0, ',', '.');
                        if (total === '0') {
                            total = 'N/A';
                        } else {
                            total = "IDR " + total;
                        }

                        $("#ResultETD").html(etd === "" ? "-" : etd);
                        $("#ResultCost").html(total);
                        $("#ResultOrigin").html(origin);
                        $("#ResultModel").html(model);
                        $("#ResultDestination").html(destination);
                    } else {
                        console.log('tidak ada data ditemukan');

                        sAlert('Error', "Data not found!", "error");
                    }
                },
                error: function (err) {
                    console.log('tidak ada data ditemukan', err);
                }
            })
        }

    });

    populateColumn();
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        queryParams: function (params) {
            return {
                origin: $("input[name=origin]").val(),
                destination: $("input[name=destination]").val(),
                limit: params.limit,
                order: params.order,
                offset: params.offset,
                unit: $("input[name=unit]").val()
            };
        },
        method: 'GET',
        url: "/DTS/FreightCalculatorPageXt",
        clickToSelect: true,
        reorderableColumns: false,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        //onClickRow: selectRow,
        sidePagination: 'server',
        fixedColumns: true,
        fixedNumber: '5',
        showColumns: false,
        showRefresh: true,
        columns: columnList,
        smartDisplay: false,
        pageSize: '10'


    });
    $(window).resize(function () {
        $table.bootstrapTable('resetView');
    });
});

$("#displayData").click(function () {
    var dataSerialize = $form.serialize();
    console.log(dataSerialize);
    $table.bootstrapTable('refresh');
});

$("#btnExportFreight").click(function () {
    enableLink(false);
    $.ajax({
        url: "/DTS/DownloadFreight",
        type: 'GET',
        success: function (guid) {
            enableLink(true);
            window.open('/DTS/DownloadToExcelFreight?guid=' + guid, '_blank');
        },
        cache: false
    });
});