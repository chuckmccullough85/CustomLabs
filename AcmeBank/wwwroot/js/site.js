// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$("#billpay_form").submit(function (event) {
    event.preventDefault();
    $.post("/account/AddBillPayItem", $("#billpay_form").serialize())
        .done(function (data) {
            var list = $("#bill_list")
            list.load("/account/getScheduledBills", function () {
                //alert("done");
            });
        });
});

function DeleteBill(id) {

    $.post("/account/DeleteScheduledBillPay", { id: id })
        .done(function (data) {
            $("#bill_list").html(data);
        });
}

