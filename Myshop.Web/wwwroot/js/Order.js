var dtble;
$(document).ready(function () {
    loaddata();
});

function loaddata() {
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Order/GetData",
            "dataSrc": "data"
            
        },
        "columns" : [
            { "data": "id" },
            { "data": "name" },
            { "data": "applicationUser.phoneNumber" },
            { "data": "applicationUser.email" },
          
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <a href = "/Admin/Order/Details?orderid=${data}" class="btn btn-success">View</a> 
                            

                    `;
                }

            }
        ]
    });
}

//function DeleteItem(url) {
//    Swal.fire({
//        title: "Are you sure?",
//        text: "You won't be able to revert this!",
//        icon: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#3085d6",
//        cancelButtonColor: "#d33",
//        confirmButtonText: "Yes, delete it!"
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: url,
//                type: "DELETE",
//                success: function (data) {
//                    console.log("Response from server:", data);

//                    if (data.success ) {
//                        dtble.ajax.reload();
//                        toaster.success(data.message);
//                    } else {
//                        toaster.error(data.message);
//                    }
//                }

//            });
//            Swal.fire({
//                title: "Deleted!",
//                text: "Your file has been deleted.",
//                icon: "success"
//            });
//        }
//    });
//}