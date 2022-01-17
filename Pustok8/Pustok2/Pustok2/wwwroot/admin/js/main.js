$(function () {
    $(document).on("click", ".delete-btn2", function (e) {
        e.preventDefault();
        let id = $(this).attr("data-id")
        let name = $(this).attr("data-name")
         Swal.fire({
            title: 'Razisiz?',
            text: "Data Silindikden sonra geri qaytarmaq mumkun olmayacaq!",
            icon: 'Tehluke',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Delete!'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(`/adminpanel/${name}/delete2?id=${id}`)
                    .then(Response => Response.text())
                    .then(data => {

                        window.location.reload(true);
                    })
            }
        })
    })
    

})













