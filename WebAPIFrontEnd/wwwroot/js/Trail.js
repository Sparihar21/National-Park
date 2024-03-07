var dataTable;

$(function () {
    loadTableData();
});

function loadTableData() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Trail/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "nationalPark.name", "width": "20%" },
            { "data": "name", "width": "20%" },
            { "data": "distance", "width": "20%" },
            { "data": "elevation", "width": "20%" },
            {
                "data": "id", "render": function (data) {
                    return `
                        <div class="text-center">
                        <a href="Trail/Upsert/${data}" class="btn btn-info"><i class="fas fa-edit"></i></a>
                        <a class="btn btn-danger" onclick="Delete('/Trail/Delete/${data}')"><i class="fas fa-trash-alt"></i></a>
                        </div>
                        `;
                }
            }
        ]
    })
}


function Delete(url) {
    swal({
        title: "Want To Delete Category?",
        text: "Are You Sure?",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        $('#tblData').DataTable().ajax.reload();
                    }
                    else
                        toastr.error(data.message);
                }
            })
        }
    })
}