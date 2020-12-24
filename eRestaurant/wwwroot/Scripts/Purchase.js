var PurchaseDatatable;

$(document).ready(function () {
     PurchaseDatatable = $('#PurchaseOrderDetails').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "searching":false,
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [5],
                "Data": "", "name": "Action", "defaultContent": '<a href="#" class="deleteItem">Delete Order</a>', "autoWidth": true
            }
            //,
            //{
            //    "targets": [6],
            //    "visible": false,
            //    "searchable": false
            //}
        ]
    });
    // $("#Date").val(new Date());
});

$('#addRow').on('click', function (e) {
    if ($("#purchase").valid()) {
        e.preventDefault();
        PurchaseDatatable.row.add([
            $("#IngredientId").val(),
            $('#IngredientId').children("option:selected").text(),
            $("#UnitPrice").val(),
            $("#Quantity").val(),
            $("#UnitPrice").val() * $("#Quantity").val(),
            '<td><a href="#" data-itemId="' + $("#IngredientId").val() + '" class="editItem fa fa-edit"></a> | <a href="#" data-itemId="' + $("#IngredientId").val() + '" class= "deleteItem fa fa-times"></a ></td > ',
            $("#PurchaseId").val()
        ]).draw(false);
        dataArr.push({
            ingredientId: $("#IngredientId").val(),
            unitPrice: $("#UnitPrice").val(),
            quantity: $("#Quantity").val(),
            total: $("#UnitPrice").val() * $("#Quantity").val(),
            purchaseId: $("#PurchaseId").val()
        });
        GrandTotal += $("#UnitPrice").val() * $("#Quantity").val();
        $("#GrandTotal").val(GrandTotal);
        DueAmount();
        clearItem();
    }
    else {
        return false;
    }
});

function saveOrder(data) {
    return $.ajax({
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        type: 'POST',
        beforeSend: function (xhr) { xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val()); },
        url: 'Purchase',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
}

$(function () {
    $('#saveOrder').click(function () {
        if ($("#purchase").valid()) {
            if (dataArr.length < 1) {
                $("#aModel").modal('show');
                $("#aModalMessage").val("Please enter altest one row!");
                $("#isValidate").removeClass("hide");
                return false;
            }
            else {
                $("#purchase").on("submit", function (e) {
                    e.preventDefault();
                    //console.log(dataArr);
                    var data = ({
                        Id:$("#Id").val(),
                        ReferenceNo: $("#ReferenceNo").val(),
                        SupplierId: $("#SupplierId").val(),
                        Date: $("#Date").val(),
                        GrandTotal: $("#GrandTotal").val(),
                        Due: $("#Paid").val(),
                        Paid: $("#Due").val(),
                        SupplierList: [],
                        IngredientList: [],
                        PurchaseDetails: dataArr
                    });
                    $.when(saveOrder(data)).then(function (response) {
                        if (response.status == "200") {
                            $("#aModalMessage").val(response.message);
                            $("#aModal").modal('show');
                        }
                        console.log(response);
                    }).fail(function (err) {
                        console.log(err);
                    });

                });
            }
        }
        else {
            return false;
        }
    });
})
$('#save').click(function () {
    window.location.href = "/Purchase/PurchaseList";
});

$('#ok').click(function () {
    window.location.href = "/Purchase/Purchase";
});

$(document).on('click', 'a.deleteItem', function (e) {
    debugger;
    e.preventDefault();
    var id = $(this).attr('data-itemId');
    $(this).parents('tr').css("background-color", "#FF3700").fadeOut(800, function () {
        for (var i = 0; i < dataArr.length; i++) {
            if (dataArr[i].ingredientId == id) {
                debugger;
                TotalAmount = dataArr[i].Total;
                GrandTotal -= TotalAmount;
                $("#GrandTotal").val(GrandTotal);
                DueAmount();
                dataArr.splice(i, 1);
                $(this).remove();
            }
        }
    });

});

$(document).on('click', 'a.editItem', function (e) {
    debugger;
    e.preventDefault();
    var id = $(this).attr('data-itemId');
    $(this).parents('tr').css("background-color", "#0000A0").fadeOut(800, function () {
        for (var i = 0; i < dataArr.length; i++) {
            if (dataArr[i].ingredientId == id) {
                $("#IngredientId").val(dataArr[i].ingredientId),
                    // $('#IngredientId').children("option:selected").text(),
                    $("#UnitPrice").val(dataArr[i].unitPrice),
                    $("#Quantity").val(dataArr[i].quantity),
                    $("#PurchaseId").val(dataArr[i].purchaseId)
                    //$("#UnitPrice").val() * $("#Quantity").val(),
                    GrandTotal = $("#GrandTotal").val();

                    TotalAmount = dataArr[i].total;
                GrandTotal -= TotalAmount;
                $("#GrandTotal").val(GrandTotal);
                DueAmount();
                dataArr.splice(i, 1);
                $(this).remove();
            }
        }
    });
});

$("#Paid").blur(function () {
    DueAmount();
});

function DueAmount() {
    var Due = 0
    Due = (GrandTotal - parseFloat($("#Paid").val()));
    $("#Due").val(Due);
}

function clearItem() {
    $("#IngredientId").val(''),
        $("#UnitPrice").val(''),
        $("#Quantity").val(''),
        $("#Amount").val(''),
        $("#PurchaseId").val('0')
}