var dtble;
$(document).ready(function () {
    loaddata();
});

function loaddata() {
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetData",
            "dataSrc": "data"

        },
        "columns": [
            { "data": "name" },
            { "data": "description" },
            { "data": "price" },
            { "data": "category.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
        <div class="d-flex gap-2">
            <a href="/Admin/Product/Edit/${data}" 
               class="btn btn-outline-primary btn-sm d-flex align-items-center shadow-sm">
                <i class="bi bi-pencil-square me-1"></i> Edit
            </a>
            <a href="/Admin/Product/Delete/${data}" 
               class="btn btn-outline-danger btn-sm d-flex align-items-center shadow-sm">
                <i class="bi bi-trash me-1"></i> Delete
            </a>
        </div>
    `;
                }


            }
        ]
    });
}

