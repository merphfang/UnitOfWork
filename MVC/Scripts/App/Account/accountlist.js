$(document).ready(function () {

    $('#lstAccounts').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": listRequestUrl,
        "columns": [
           
             { "name": "Id", "data": "Id", "visible": false },
             { "name": "FirstName","data": "FirstName" },
             { "name": "LastName", "data": "LastName" },
             { "name": "Email", "data": "Email" },
             { "name": "Customer.Name", "data": "Customer" },
             { "name": "CreatedDate", "data": "CreatedDate" },
             {
                   "data": "Id",
                   "render": function ( data, type, row ) {
                       return '<a href=""><i class="fas fa-edit"></i></a>';
                   },
                   "orderable": false
                 
             }
        ],
        "order": [[2, "asc"]],
        "language": {
            "emptyTable": "No records available!"
        }
    });

    var dtable = $("#lstAccounts").dataTable().api();
    $(".dataTables_filter input")
        .unbind() 
        .bind("input", function (e) {
            if (this.value.length >= 3 || e.keyCode == 13) {
                dtable.search(this.value).draw();
            }
            if (this.value == "") {
                dtable.search("").draw();
            }
            return;
        });

});