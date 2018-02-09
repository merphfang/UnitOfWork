$(document).ready(function () {

    $('#lstAccounts').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": "http://localhost:38149/Account/GetAccounts",
        "columns": [
             { "data": "Id" },
             { "data": "FirstName" },
             { "data": "LastName" },
             { "data": "Email" }
        ],
        "order": [[2, "asc"]],
        "language": {
            "emptyTable": "No records available!"
        }
    });
});