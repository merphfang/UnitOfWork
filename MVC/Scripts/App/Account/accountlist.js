$(document).ready(function () {

    $('#lstAccounts').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": "http://localhost:38149/Account/GetAccounts",
        "columns": [
           
             { "data": "Id", "visible": false },
             { "data": "FirstName" },
             { "data": "LastName" },
             { "data": "Email" },
             { "data": "Customer" },
             { "data": "CreatedDate" },
             {
                   "data": "Id",
                   "render": function ( data, type, row ) {
                       return '<a href=""><i class="fas fa-edit"></i></a>';
                   } 
                 
             }
        ],
        "order": [[2, "asc"]],
        "language": {
            "emptyTable": "No records available!"
        }
    });
});