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
    // Call datatables, and return the API to the variable for use in our code
    // Binds datatables to all elements with a class of datatable
    var dtable = $("#lstAccounts").dataTable().api();

    // Grab the datatables input box and alter how it is bound to events
    $(".dataTables_filter input")
        .unbind() // Unbind previous default bindings
        .bind("input", function (e) { // Bind our desired behavior
            // If the length is 3 or more characters, or the user pressed ENTER, search
            if (this.value.length >= 3 || e.keyCode == 13) {
                // Call the API search function
               
                dtable.search(this.value).draw();
            }
            // Ensure we clear the search if they backspace far enough
            if (this.value == "") {
                dtable.search("").draw();
            }
            return;
        });

});