$(function () {

    $(document).on("click", ".single-btn-eye", function () {
        let id = $(this).data("id")
/*        alert("salam")
*/        fetch(`/home/getbook/${id}`)
            .then(Response => Response.text())
            .then(data => {
                console.log(data)
                $("#detailModal .modal-content").html(data)
            })
        $("#detailModal").modal("show")
    })
 
})
let icon = document.getElementById("pagenation-icon")

icon.addEventListener("click", function () {
    alert("salam")

    icon.className="active"
})


$(function () {

    $(document).on("click", ".cross-btn", function () {
        let id = $(this).data("id")
        alert("salam")
        fetch(`/book/removebasket/${id}`)
            .then(Response => Response.text())
    .then(data => {
        alert(data)
            })
      
    })

})