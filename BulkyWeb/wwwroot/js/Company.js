$(document).ready(function () {
    //loadDataTable();
    $('#tblData').DataTable({
        ajax: {
            url: '/Admin/Company/GetAll'
        },
        "columns": [
            { "data": "name" },
            { "data": "streetAddress" },
            { "data": "city" },
            { "data": "state" },
            { "data": "phoneNumber" }
        ]
    });
});
//function loadDataTable() {
//    debugger;
//    dataTable = $('#tblData1').DataTable({
//        "ajax": { url: '/admin/product/getall' },
//        "columns": [
//            { data: 'title', "width": "15%" },
//            { data: 'isbn', "width": "15%" },
//            { data: 'author', "width": "15%" },
//            { data: 'listprice', "width": "15%" },
//            { data: 'category.name', "width": "15%" }
//         ]
//        })

//}
function Delete(url){
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);

                }
            })
        }
        });
}


