function deletepopup(d) {
    Swal.fire({
        title: 'Success',
        text: 'Data Updated SuccessFully',
        type: 'success'
    }).then((result) => {
        $("[name=refresh]").trigger('click');
    })


}