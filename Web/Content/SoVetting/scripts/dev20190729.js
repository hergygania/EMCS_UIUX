$(document).ready(function() {

  var ajax_method_POST = 'POST'

  $.fn.serializeFormJSON = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
      // console.log(a)
      if (o[this.name]) {
        if (!o[this.name].push) {
          o[this.name] = [o[this.name]];
        }
        o[this.name].push(this.value || '');
      } else {
        o[this.name] = this.value || '';
      }
    });
    // console.log(o)
    return o;
  };
  
  function SubmitFieldset(fieldsetName) {    
    $('body').append('<form id="form-to-submit" style="visibility:hidden;"></form>');
  
    $('#form-to-submit').html($(fieldsetName).clone());
  
    var data = $('#form-to-submit').serializeFormJSON();
  
    $('#form-to-submit').remove();
  
    return data;
  }
  
  
  var inputAdvSrc             = '#input-search-filter';
  var tableAdvSrcResult       = '#table-adv-src-result';
  var dataTable_general_opts  = {
    destroy       : true,
    lengthChange  : false,
    //- ajax          : "data/advance-filter.json",
    deferRender   : true,
    fixedColumns  : {
      leftColumns : 2
    },
    scrollX       : true,
    fixedColumns  : true,
  };
  
  // -------------
  // DATATABLE QUICK SEARCH
  // -------------
  function quickTrackingSO() {
    var ajax_url    = $('#btn-track-now').data('ajax_url');
    var error_empty = $('#btn-track-now').data('ajax_empty');
  
    var $table_wrap = $('#table-quick-result-wrap')
  
    var $btn_refresh        = $table_wrap.find('.refresh-data-table');
    var $latest_date        = $table_wrap.find('.latest_date');
    var $latest_time        = $table_wrap.find('.latest_time');
    var $btn_export_excel   = $table_wrap.find('.btn-export-excel');
    var $showing_data       = $table_wrap.find('.showing-data');
  
    // $('body').addClass('preload-truck-show'); // PRELOADER
    Pace.start

    var $table_target   = $('#table-quick-result');

    $table_target.DataTable().clear().destroy()

    var data_obj    = $('#quick-search-form').serializeFormJSON();

    var xhr = $.ajax({
      type        : ajax_method_POST,
      url         : ajax_url,
      data        : JSON.stringify(data_obj),
      dataType    : 'json',
      contentType : 'application/json; charset=utf-8',
      success     : function (resp) {

        $('#quick-search-form').hide();
        $('#quick-search-result').show();

        setTimeout(function () {

          // ------------------
          // GENERATE COLUMN FILTER
          // ------------------
          $table_target.find('thead tr')
            .clone(true)
            .appendTo( $table_target.find('thead') );
          
          $table_target.find('thead tr:eq(1)').addClass('cloned')
          $table_target.find('thead tr:eq(1) th').each( function (i) {
            var title = $(this).text();
            $(this).html( '<input type="text" class="form-control rounded-0" placeholder="Find" />' );
      
            $( 'input', this ).on( 'keyup change', function () {
              if ( table_quick_result.column(i).search() !== this.value ) {
                table_quick_result
                  .column(i)
                  .search( this.value )
                  .draw();
              }
            });
          });
  
          // ------------------
          // DATATABLES
          // ------------------
          var opts = {
            data          : resp.data,
            dom           : 'Bfrtip',
            orderCellsTop : true,
            buttons       : [
              {
                extend  : 'excelHtml5',
                text    : '<span class="btn-generate-export-excel">Export to Excel</span>'
              }
            ],
            columns   : quickSearchColumns
          }
  
          var merge_opts = Object.assign(opts, dataTable_general_opts);

          var table_quick_result = $table_target.DataTable(merge_opts);

          var table_info = table_quick_result.page.info();

          // console.log(table_info);

          var row_txt      = ( table_info.recordsDisplay < 2 ) ? 'Row' : 'Rows';
          var showing_info = ( table_info.end < table_info.length) ? table_info.end : table_info.length

          $showing_data.html('Showing ' + showing_info + ' ' + row_txt + ' from Total ' + table_info.recordsTotal + ' ' + row_txt);
          var row_txt      = ( table_info.recordsDisplay < 2 ) ? 'Row' : 'Rows';
          var showing_info = ( table_info.end < table_info.length) ? table_info.end : table_info.length

          $showing_data.html('Showing ' + showing_info + ' ' + row_txt + ' from Total ' + table_info.recordsTotal + ' ' + row_txt);

          var data_latest_update = resp.latest_update;

          $latest_date.html(getDateDDMMYYY(data_latest_update));
          $latest_time.html(getTime(data_latest_update));
          
          // ------------------
          // BUTTON EXPORT
          // ------------------
          $btn_export_excel.on('click', function () {
            $table_wrap.find('.buttons-excel.buttons-html5').click()
          });
  
          // ------------------
          // REFRESH DATA TABLE
          // ------------------
          $btn_refresh.on('click', function() {
            $table_target.DataTable().clear().destroy();
            $table_target.find('thead .cloned').remove()
          
            $table_target.find('thead tr, thead th').each(function() {
              var attributes = $.map(this.attributes, function(item) {
                return item.name;
              });
              var img = $(this);
              $.each(attributes, function(i, item) {
                img.removeAttr(item);
              });
            });
          
            setTimeout(function () {
              quickTrackingSO();
            }, 500 );
          });
  
  
          // ------------------
          // OPEN ORDER TABLE
          // ------------------
          $table_wrap.find('.btn-open-order-table').on('click', function() {
            // console.log('klik')
            var order = $(this).data('order_item')
  
            quickOrderItem(order);
          });
        }, 500);
      }
    });
  
    // prevent submitting
    return false;
  }
  
  
  // -------------
  // DATATABLE QUICK ORDER ITEM
  // -------------
  function quickOrderItem(value) {
    var $table_wrap = $('#table-quick-order-wrap');
  
    var ajax_url    = $table_wrap.data('ajax_url');
  
    var $btn_refresh        = $table_wrap.find('.refresh-data-table');
    var $latest_date        = $table_wrap.find('.latest_date');
    var $latest_time        = $table_wrap.find('.latest_time');
    var $btn_export_excel   = $table_wrap.find('.btn-export-excel');
    var $showing_data       = $table_wrap.find('.showing-data');
  
  
    // $('body').addClass('preload-truck-show'); // PRELOADER
    Pace.start
  
    var $table_target   = $('#table-quick-order');
  
    $table_target.DataTable().clear().destroy()
  
    var data_obj    = {
      'order_item': value
    }
  
    var xhr = $.ajax({
      type      : ajax_method_POST,
      url       : ajax_url,
      data      : JSON.stringify(data_obj),
      dataType  : 'json',
      contentType : 'application/json; charset=utf-8',
      success   : function (resp) {
  
        $('#table-quick-result-wrap').hide();
        $('#table-quick-order-wrap').show();
  
        setTimeout(function () {
  
          // ------------------
          // GENERATE COLUMN FILTER
          // ------------------
          $table_target.find('thead tr')
            .clone(true)
            .appendTo( $table_target.find('thead') );
          
          $table_target.find('thead tr:eq(1)').addClass('cloned')
          $table_target.find('thead tr:eq(1) th').each( function (i) {
            var title = $(this).text();
            $(this).html( '<input type="text" class="form-control rounded-0" placeholder="Find" />' );
      
            $( 'input', this ).on( 'keyup change', function () {
              if ( table_quick_result.column(i).search() !== this.value ) {
                table_quick_result
                  .column(i)
                  .search( this.value )
                  .draw();
              }
            });
          });
  
          // ------------------
          // DATATABLES
          // ------------------
          var opts = {
            data          : resp.data,
            dom           : 'Bfrtip',
            orderCellsTop : true,
            buttons       : [
              {
                extend  : 'excelHtml5',
                text    : '<span class="btn-generate-export-excel">Export to Excel</span>'
              }
            ],
            columns   : quickOrderColumns
          }
  
          var merge_opts = Object.assign(opts, dataTable_general_opts);
  
          var table_quick_result = $table_target.DataTable(merge_opts);
  
          var table_info = table_quick_result.page.info();

                      // console.log(table_info);

            var row_txt      = ( table_info.recordsDisplay < 2 ) ? 'Row' : 'Rows';
            var showing_info = ( table_info.end < table_info.length) ? table_info.end : table_info.length

            $showing_data.html('Showing ' + showing_info + ' ' + row_txt + ' from Total ' + table_info.recordsTotal + ' ' + row_txt);

          var data_latest_update = resp.latest_update;
  
          $latest_date.html(getDateDDMMYYY(data_latest_update));
          $latest_time.html(getTime(data_latest_update));
            
          // ------------------
          // BUTTON EXPORT
          // ------------------
          $btn_export_excel.on('click', function () {
            $table_wrap.find('.buttons-excel.buttons-html5').click()
          });
  
          // ------------------
          // REFRESH DATA TABLE
          // ------------------
          $btn_refresh.on('click', function() {
            $table_target.DataTable().clear().destroy();
            $table_target.find('thead .cloned').remove()
          
            $table_target.find('thead tr, thead th').each(function() {
              var attributes = $.map(this.attributes, function(item) {
                return item.name;
              });
              var img = $(this);
              $.each(attributes, function(i, item) {
                img.removeAttr(item);
              });
            });
          
            setTimeout(function () {
              quickOrderItem();
            }, 500 );
          });
  
        }, 500);
      }
    });
  
  }
  
  
  $('#btn-track-now').on('click', function(e) {
    e.preventDefault()
    quickTrackingSO();
  });
  
  $(inputAdvSrc).keyup(function(e) {
    const code = e.which
  
    if (code === 13) {
      quickTrackingSO();
    }
  });
  
  // -------------
  // HIDE TRUCK PRELOADER
  // -------------
  var hide_truck_preload = function () {
    setTimeout(function() {
      $('body').removeClass('preload-truck-show');
    }, 1000);
  }
  
  // -------------
  // COLUMN SETTING
  // -------------
  window.checked_summary   = [];
  window.checked_order     = [];
  window.checked_source    = [];
  
  $('.chk-summary-checkall').change(function() {
    var target = $(this).data('checkall_target');
  
    checked_summary = [];
  
    if ($(this).is(':checked')) {
      target.forEach(function(e){
        checked_summary.push('checkedAll');
      })
    } else {
      target.forEach(function(e){
        checked_summary.remove('checkedAll');
      })
    }
  
    // console.log(checked_summary);
  })
  
  $('.chk-summary').on('change', function() {
    var item = $(this).val();
  
    if ($(this).is(':checked')) {
      checked_summary.push(item);
    } else {
      checked_summary.remove(item);
    }
  
    // console.log(checked_summary);
  });
  
  
  $('.chk-item-order-checkall').change(function() {
    var target = $(this).data('checkall_target');
  
    checked_order = [];
  
    if ($(this).is(':checked')) {
      target.forEach(function(e){
        checked_order.push('checkedAll');
      })
    } else {
      target.forEach(function(e){
        checked_order.remove('checkedAll');
      })
    }
  
    // console.log(checked_order);
  })
  
  $('.chk-item-order').on('change', function() {
    var item = $(this).val();
  
    if ($(this).is(':checked')) {
      checked_order.push(item);
    } else {
      checked_order.remove(item);
    }
  
    // console.log(checked_order);
  });
  
  $('.chk-item-source-checkall').change(function() {
    var target = $(this).data('checkall_target');
  
    checked_source = [];
  
    if ($(this).is(':checked')) {
      target.forEach(function(e){
        checked_source.push('checkedAll');
      })
    } else {
      target.forEach(function(e){
        checked_source.remove('checkedAll');
      })
    }
  
    // console.log(checked_source);
  })
  
  $('.chk-item-source').on('change', function() {
    var item = $(this).val();
  
    if ($(this).is(':checked')) {
      checked_source.push(item);
    } else {
      checked_source.remove(item);
    }
  
    // console.log(checked_source);
  });
  
  
  // --------------------
  // BUTTON SET TO DEFAULT
  // --------------------
  window.column_set_default_valid = false
  window.params_set_default_valid = false
  
  $('#btn-col-filter-setDefault').on('click', function() {
    var ajax_url      = $(this).data('ajax_url');
    var alert_error   = $(this).data('alert_error')
    var alert_success = $(this).data('alert_success')
  
  
    var set_as_default = function (collect_data, ajax_url, alert_success) {
      var xhr = $.ajax({
        type      : ajax_method_POST,
        url       : ajax_url,
        data      : JSON.stringify(collect_data),
        dataType  : 'json',
        contentType : 'application/json; charset=utf-8',
        success   : function (resp) {
          if (resp.is_valid === true) {
            Swal.fire({
              type  : 'success',
              html  : alert_success,
              timer : 1000
            });
          } else {
            Swal.fire({
              type  : 'error',
              html  : 'Penyimpanan gagal dilakukan'
            });
          }
        }
      });
    }
  
    if ( $(this).attr('id') == 'btn-col-filter-setDefault' ) {
      if ( checked_summary.length !== 0 || checked_order.length !== 0 || checked_source.length !== 0 ) {
        var collect_data = SubmitFieldset('#fieldset-column-setting')
        set_as_default(collect_data, ajax_url, alert_success)
        column_set_default_valid = true
      } else {
        Swal.fire({
          type : 'error',
          html : alert_error
        })
      }
    } else {
      var collect_data = SubmitFieldset('#fieldset-parameter-setting')
      set_as_default(collect_data, ajax_url, alert_success)
      params_set_default_valid = true
    }
  });
  
  // ----------
  // BTN NEXT
  // ----------
  $('#btn-adv-search-next, #btn-tab-filter-2').on('click', function() {
    var $this = $(this)
    var alert_error = $(this).data('alert_error')
  
    if ( checked_summary.length !== 0 || checked_order.length !== 0 || checked_source.length !== 0) {
      if ( column_set_default_valid == true ) {
        if ($this.prop('id') === 'btn-adv-search-next') {
          // console.log(column_set_default_valid)
          $('#btn-tab-filter-2').click()
        }
      } else {
        Swal.fire({
          type : 'error',
          html : 'Mohon klik tombol "Set as Default" terlebih dahulu'
        });
      }
  
    } else {
      Swal.fire({
        type : 'error',
        html : alert_error
      });
  
      return false
    }
  });
  

  // -------------
  // DATATABLE ADV SEARCH
  // -------------
  function advResultTable() {
    var $table_wrap         = $('#table-adv-src-result-wrap')
    var ajax_url            = $table_wrap.data('ajax_url');
    var ajax_url_order      = $table_wrap.data('ajax_table_order');
    var ajax_url_sourcing   = $table_wrap.data('ajax_table_sourcing');

    var error_empty         = $table_wrap.data('ajax_empty');

    var $btn_refresh        = $table_wrap.find('.refresh-data-table');
    var $latest_date        = $table_wrap.find('.latest_date');
    var $latest_time        = $table_wrap.find('.latest_time');
    var $btn_export_excel   = $table_wrap.find('.btn-export-excel');

    // $('body').addClass('preload-truck-show'); // PRELOADER
    Pace.start

    var $table_target   = $('#table-adv-src-result');

    $table_target.DataTable().clear().destroy()

    var data_obj    = $('#advance-src-form').serializeFormJSON();

    var xhr = $.ajax({
      type      : ajax_method_POST,
      url       : ajax_url,
      data      : JSON.stringify(data_obj),
      dataType  : 'json',
      contentType : 'application/json; charset=utf-8',
      success   : function (resp) {

        $('#advance-src-form').hide();
        $('#advanced-search-result').show();

        setTimeout(function () {

          // ------------------
          // GENERATE COLUMN FILTER
          // ------------------
          $table_target.find('thead tr')
            .clone(true)
            .appendTo( $table_target.find('thead') );
          
          $table_target.find('thead tr:eq(1)').addClass('cloned')
          $table_target.find('thead tr:eq(1) th').each( function (i) {
            var title = $(this).text();
            $(this).html( '<input type="text" class="form-control rounded-0" placeholder="Find" />' );
      
            $( 'input', this ).on( 'keyup change', function () {
              if ( table_quick_result.column(i).search() !== this.value ) {
                table_quick_result
                  .column(i)
                  .search( this.value )
                  .draw();
              }
            });
          });

          // console.log(resp)

          // ------------------
          // DATATABLES
          // ------------------
          var opts = {
            data          : resp.data,
            dom           : 'Bfrtip',
            orderCellsTop : true,
            buttons       : [
              {
                extend  : 'excelHtml5',
                text    : '<span class="btn-generate-export-excel">Export to Excel</span>'
              }
            ],
            columnDefs : resp.columnDefs,
            columns   : advanceAllcolumns
          }

          // console.log(opts)

          var merge_opts = Object.assign(opts, dataTable_general_opts);

          var table_quick_result = $table_target.DataTable(merge_opts);

          var data_latest_update = resp.latest_update;

          var table_info = table_quick_result.page.info();

          $latest_date.html(getDateDDMMYYY(data_latest_update));
          $latest_time.html(getTime(data_latest_update));
          
          // ------------------
          // BUTTON EXPORT
          // ------------------
          $btn_export_excel.on('click', function () {
            $table_wrap.find('.buttons-excel.buttons-html5').click()
          });

          // ------------------
          // REFRESH DATA TABLE
          // ------------------
          $btn_refresh.on('click', function() {
            $table_target.DataTable().clear().destroy();
            $table_target.find('thead .cloned').remove()
          
            $table_target.find('thead tr, thead th').each(function() {
              var attributes = $.map(this.attributes, function(item) {
                return item.name;
              });
              var img = $(this);
              $.each(attributes, function(i, item) {
                img.removeAttr(item);
              });
            });
          
            setTimeout(function () {
              advResultTable();
            }, 500 );
          });


          // TRACKING MODAL
          $('.btn-open-tracking-so-number').on('click', function() {
            var number    = $(this).data('number');
            var modal     = $(this).attr('href');
            // var ajax_url  = $('#table-adv-src-result').data('ajax_table_order');
        
            var ajax_url      = $('#tracking-modal').data('ajax_url');

            $(modal).modal('show');

            $('#tracking-so-loop').html('')

            var total_table     = 1
            var resp_data       = ''
        
            var i = 0
            setTimeout(() => {
              $.ajax({
                type      : ajax_method_POST,
                url       : ajax_url,
                data      : JSON.stringify({
                  track_number : number
                }),
                dataType  : 'json',
                contentType : 'application/json; charset=utf-8',
                success   : function (resp) {

                  // console.log(resp)

                  var so_number   = resp.so_number;
                  resp_data       = resp.data;
                  total_table     = resp_data.length;

                  $('#tracking-so-number').html(so_number);

                  // console.log(total_table)
                }
              });

              setTimeout(function () {
                for (i=0; i<total_table; i++) {
                  // console.log(resp_data[i])
                  // console.log(value)
                  var id = 'table-tracking-so-' + i
  
                  var div ='<div class="row no-gutters">' +
                              '<h6>DA NUMBER '+resp_data[i].da_number+'</h6>' +
                              '<div class="table-responsive mb-3">' +
                                '<table class="table table-striped table-bordered table-tracking table-text-nowrap" id="'+id+'">' +
                                  '<thead class="thead-light">' +
                                    '<tr class="small text-center align-middle">' +
                                      '<th>Tracking Status</th>' +
                                      '<th>Time</th>' +
                                      '<th>City</th>' +
                                      '<th>Invoice CKB</th>' +
                                      '<th>Invoice CAT</th>' +
                                    '</tr>' +
                                  '</thead>' +
                                  '<tbody>' +
                                    '<tr>' +
                                      '<td>' + resp_data[i].data.tracking_stat +'</td>' +
                                      '<td>' + resp_data[i].data.time +'</td>' +
                                      '<td>' + resp_data[i].data.city +'</td>' +
                                      '<td>' + resp_data[i].data.invoice_ckb +'</td>' +
                                      '<td>' + resp_data[i].data.invoice_cat +'</td>' +
                                    '</tr>' +
                                  '</tbody>' +
                                '</table>' +
                              '</div>' +
                            '</div>';
  
                  $('#tracking-so-loop').append(div);
                }
  
              }, 500);

              // setTimeout(function () {
              //   for (i=0; i<total_table; i++) {
              //     var id = 'table-tracking-so-' + i
              //     // console.log(i)
              //     // console.log(resp_data)
              //     console.log(resp_data[i].data)
              //     var opts = {
              //       data          : resp_data[i].data,
              //       dom           : 'Bfrtip',
              //       orderCellsTop : true,
              //       columns       : [
              //         { data : 'tracking_stat' },
              //         { data : 'time' },
              //         { data : 'city' },
              //         { data : 'invoice_ckb' },
              //         { data : 'invoice_cat' }
              //       ]
              //     }
  
              //     console.log(opts)
        
              //     var merge_opts = Object.assign(opts, dataTable_general_opts)
          
              //     var table_order = $(id).DataTable(merge_opts)
              //   }
              // }, 1000);
            }, 500);


          });

        }, 500);
      }
    });

    // prevent submitting
    return false;
  }

  // advResultTable()
  
  // -------------
  // ADVANCE SEARCH ON CLICK
  // -------------
  $('#btn-run-adv-search').on('click', function(e) {
    e.preventDefault()
  
    // var ajax_url      = $(this).data('ajax');
    advResultTable();
  
    // if (params_set_default_valid == true) {
    //   advResultTable();
    // } else {
    //   Swal.fire({
    //     type : 'error',
    //     html : 'Mohon klik tombol "Set as Default" terlebih dahulu'
    //   });
    // }
  });
  
  
  // --------------------
  // OPEN ORDER & STATIC SO NUMBER - DATATABLE WITHIN MODAL
  // --------------------
  $('#table-adv-src-result').DataTable().on('draw', function() {
    // ORDER MODAL
    $('.btn-open-order-modal').on('click', function() {
      var number    = $(this).data('number');
      var modal     = $(this).attr('href');
      // var ajax_url  = $('#table-adv-src-result').data('ajax_table_order');
  
      var ajax_url      = $('#table-adv-src-result-wrap').data('ajax_table_order');

      $(modal).modal('show')
  
      setTimeout(() => {
        $.ajax({
          type      : ajax_method_POST,
          url       : ajax_url,
          data      : JSON.stringify({
            so_number : number
          }),
          dataType  : 'json',
          contentType : 'application/json; charset=utf-8',
          success   : function (resp) {
  
            var opts = {
              data          : resp.data,
              columns        : advSrcModal_order_columns
            }
  
            var merge_opts = Object.assign(opts, dataTable_general_opts)
    
            var table_order = $('#table-item-order').DataTable(merge_opts)
          }
        })
      }, 500);
    });

  });
  
  
  // --------------------
  // OPEN SOURCING - DATATABLE WITHIN MODAL
  // --------------------
  $('#table-item-order').DataTable().on('draw', function() {
    $('.btn-open-sourcing-modal').on('click', function() {
      var number    = $(this).data('number');
      var modal     = $(this).attr('href');
      // var ajax_url  = $('#table-adv-src-result').data('ajax_table_sourcing');
  
      var ajax_url   = $('#table-adv-src-result-wrap').data('ajax_table_sourcing');
  
      $(modal).modal('show')
  
      setTimeout(() => {
        $.ajax({
          type      : ajax_method_POST,
          url       : ajax_url,
          data      : JSON.stringify({
            so_number : number
          }),
          dataType  : 'json',
          contentType : 'application/json; charset=utf-8',
          success   : function (resp) {
  
            var opts = {
              data          : resp.data,
              columns        : advSrcModal_sourcing_columns
            }
  
            var merge_opts = Object.assign(opts, dataTable_general_opts)
    
            var table_order = $('#table-item-sourcing').DataTable(merge_opts)
          }
        })
      }, 500);
    });
  });

  // $("#triger-menu").on("click", function() {
  //   $("#aass").toggleClass("invis show").queue(function(n) {
  //     $("#menu-cont").toggleClass("iin out");
  //     $("#menu-content-neo").toggleClass("iin out");
  //     n()
  //   })
  // });
  
  
  
  // // ------------------------
  // // COLLECTING DATA TO POST VIA AJAX
  // // ------------------------
  // var advSrcData = function() {
  //   return {
  //     sales_office : $('[name=sales-office]').val(),
  //     start_date_1 : $('[name=startDate1]').val(),
  //     end_date_1   : $('[name=endDate1]').val(),
  //     //- tambahin lagi dah nih apa aja yg mau diPOST
  //   };
  // }
  
  // -------------
  // ADVANCE SEARCH DATATABLE
  // -------------
  // var runFilterAjax = function (ajax_url, data) {
  //   $('body').addClass('preload-truck-show');
  //   $('#advance-src-form').hide();
  
  //   var xhr = $.ajax({
  //     type      : ajax_method_POST,
  //     url       : ajax_url,
  //     data      : JSON.stringify(data),
  //     dataType  : 'json',
  //     contentType : 'application/json; charset=utf-8',
  //     success   : function (resp) {
  //       hide_truck_preload();
  
  //       $('#quick-search-result').hide();
  //       $('#advanced-search-result').show();
  
  
  //       var opts = {
  //         data          : resp.data,
  //         dom           : 'Bfrtip',
  //         buttons       : [
  //           'excelHtml5'
  //         ],
  //         columns   : [
  //           { data    : 'action',
  //             render  : function ( data, type, row ) {
  //               var div = '<div class="btn-group text-nowrap">'+
  //                         '<a href="#tracking-modal" data-toggle="modal" data-tracking-id="' + data.action + '"  class="text-danger dt-open-modal">'+
  //                         '<i class="fa fa-search mr-2"></i>'+
  //                         '<span>Track</span></a></div>';
  //               return div;
  //             }
  //           },
  //           { data    : 'so_number',
  //             render  : function ( data, type, row ) {
  //               var div = '<a href="#item-order-modal" class="text-danger btn-open-order-modal" data-number="' + data.number + '">Detail</a>'+
  //                         '<span class="mx-2">|</span>'+
  //                         '<a href="#so-number-modal" data-toggle="modal" class="dt-open-modal text-danger" data-number="' + data.number + '">' + data.number + '</a>';
  //               return div;
  //             }
  //           },
  //           { data : 'sold_to_part_no' },
  //           { data : 'customer_name' },
  //           { data : 'ship_to_party_no' },
  //           { data : 'ship_to_party_name' },
  //           { data : 'payer_name' },
  //           { data : 'grace_period' },
  //           { data : 'grace_period_date' },
  //           { data : 'grace_period_notes' },
  //           { data : 'area' },
  //           { data : 'sales_office' },
  //           { data : 'sold_to_party_no' }
  //         ]
  //       }
  
  //       var merge_opts = Object.assign(opts, dataTable_general_opts)
  
  //       var tab1_table = $(tableAdvSrcResult).DataTable(merge_opts);
  //     }
  //   });
  
  //   if (ajax_url == 'abort') {
  //     xhr.abort();
  
  //     setTimeout(function() {
  //       hide_truck_preload();
  //     }, 1000);
  //   }
  // }
  
  
  // // -------------
  // // ADVANCE SEARCH - REFRESH
  // // -------------
  // $('#table-result-refresh').on('click', function() {
  //   var ajax_url = $('#btn-run-adv-search').data('ajax')
  //   $(tableAdvSrcResult).DataTable().clear().destroy()
  //   setTimeout(() => {
  //     runFilterAjax(ajax_url, advSrcData())
  //   }, 500);
  // });
  
  // -------------
  // ADVANCE SEARCH - CANCEL AJAX ADVANCE FILTER 
  // -------------
  $('#btn-cancel-filter').on('click', function() {
    runFilterAjax('abort');
  });
  
  
  // $('#btn-reset-filter').on('click', function() {
  //   var ajax_url      = $(this).data('ajax');
  //   runFilterAjax(ajax_url, {show_all: true});
  // });
});
