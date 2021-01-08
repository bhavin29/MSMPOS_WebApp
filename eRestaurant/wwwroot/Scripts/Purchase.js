var PurchaseDatatable;

$(document).ready(function () {
    $("#purchase").validate();
    PurchaseDatatable = $('#PurchaseOrderDetails').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "oLanguage": {
            "sEmptyTable": "No data available"
        },
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
            ,
            {
                "targets": [6],
                "visible": false,
                "searchable": false
            }
        ]
    });
});

$('#addRow').on('click', function (e) {
    e.preventDefault();
    var message = validation();
    if (message == '') {
        PurchaseDatatable.row('.active').remove().draw(false);
        var rowNode = PurchaseDatatable.row.add([
            '<td class="text-right">' + $("#IngredientId").val() + ' </td>',
            $('#IngredientId').children("option:selected").text(),
            '<td class="text-right">' + $("#UnitPrice").val()  + ' </td>',
            '<td class="text-right">' + $("#Quantity").val() + ' </td>',
            '<td class="text-right">' + $("#UnitPrice").val() * $("#Quantity").val() + ' </td>',
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#IngredientId").val() + '" class="btn btn-link editItem"><i class="fa fa-edit"></i></a><a href="#" class="btn btn-link btn-danger" data-toggle="modal" data-target="#myModal0"><i class="fa fa-times"></i></a></div></td > '+
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">'+
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">'+
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#IngredientId").val() + '" onclick="deleteOrder(0, ' + $("#IngredientId").val() +',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-default" data-dismiss="modal">Close</button></div></div></div ></div >',
            $("#PurchaseId").val()
        ]).draw(false).nodes();
        dataArr.push({
            ingredientId: $("#IngredientId").val(),
            unitPrice: $("#UnitPrice").val(),
            quantity: $("#Quantity").val(),
            total: $("#UnitPrice").val() * $("#Quantity").val(),
            purchaseId: $("#PurchaseId").val()
        });
        $(rowNode).find('td').eq(1).addClass('text-right');
        $(rowNode).find('td').eq(2).addClass('text-right');
        $(rowNode).find('td').eq(3).addClass('text-right');
        GrandTotal += $("#UnitPrice").val() * $("#Quantity").val();
        $("#GrandTotal").val(GrandTotal);
        DueAmount();
        clearItem();
        $("#IngredientId").focus();
    }
    else if (message != '') {
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
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
};

$(function () {
    $('#saveOrder').click(function (e) {
        e.preventDefault();
        if (!PurchaseDatatable.data().any() || PurchaseDatatable.data().row == null) {
            var message = 'At least one order should be entered'
            $(".modal-body").text(message);
            $("#save").hide();
            jQuery.noConflict();
            $("#aModal").modal('show');
        }
        else {
            $("#purchase").on("submit", function (e) {
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    SupplierId: $("#SupplierId").val(),
                    Date: $("#Date").val(),
                    GrandTotal: $("#GrandTotal").val(),
                    Due: $("#Due").val(),
                    Paid: $("#Paid").val(),
                    SupplierList: [],
                    IngredientList: [],
                    PurchaseDetails: dataArr
                });
                $.when(saveOrder(data)).then(function (response) {
                    if (response.status == "200") {
                        $(".modal-body").text(response.message);
                        $("#save").show();
                        $("#ok").hide();
                        jQuery.noConflict();
                        $("#aModal").modal('show');
                    }
                    else {
                        $(".modal-body").text(response.message);
                        $("#ok").show();
                        $("#save").hide();
                        jQuery.noConflict();
                        $("#aModal").modal('show');
                    }
                    console.log(response);
                }).fail(function (err) {
                    console.log(err);
                });

            });
        }
    })
});
$("#save").click(function () {
    window.location.href = "/Purchase/PurchaseList";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

function deleteOrderItem(id) {
    return $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Purchase/DeletePurchaseDetails",
        data: "purchaseId=" + id
    });
}

function deleteOrder(purchaseId, ingredientId, rowId) {
    var id = ingredientId;
    if (purchaseId == "0") {
        for (var i = 0; i < dataArr.length; i++) {
            if (dataArr[i].ingredientId == id) {
                TotalAmount = dataArr[i].total;
                GrandTotal -= TotalAmount;
                $("#GrandTotal").val(GrandTotal);
                DueAmount();
                dataArr.splice(i, 1);
                PurchaseDatatable.row(rowId).remove().draw(false);
                jQuery.noConflict();
                $("#myModal0").modal('hide');
            }
        }
    }
    else {
        $.when(deleteOrderItem(purchaseId).then(function (res) {
            console.log(res)
            if (res.status == 200) {
                debugger;
                for (var i = 0; i < dataArr.length; i++) {
                    if (dataArr[i].ingredientId == id) {
                        TotalAmount = dataArr[i].total;
                        GrandTotal -= TotalAmount;
                        $("#GrandTotal").val(GrandTotal);
                        DueAmount();
                        dataArr.splice(i, 1);
                        PurchaseDatatable.row(rowId).remove().draw(false);
                        jQuery.noConflict();
                        $("#myModal2" + rowId).modal('hide');
                    }
                }
                console.log(res);
            }
        }).fail(function (err) {
            console.log(err);
        }));
    }
};

$(document).on('click', 'a.editItem', function (e) {
    if (!PurchaseDatatable.data().any() || PurchaseDatatable.data().row == null) {
        var message = 'No data available!'
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
    }
    else {
        e.preventDefault();
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
        }
        else {
            $(this).parents('tr').removeClass('active');
            $(this).parents('tr').addClass('active');
        }
        var id = $(this).attr('data-itemId');
        for (var i = 0; i < dataArr.length; i++) {
            if (dataArr[i].ingredientId == id) {
                $("#IngredientId").val(dataArr[i].ingredientId),
                    $("#UnitPrice").val(dataArr[i].unitPrice),
                    $("#Quantity").val(dataArr[i].quantity),
                    $("#PurchaseId").val(dataArr[i].purchaseId)
                GrandTotal = $("#GrandTotal").val();

                TotalAmount = dataArr[i].total;
                GrandTotal -= TotalAmount;
                $("#GrandTotal").val(GrandTotal);
                DueAmount();
                dataArr.splice(i, 1);
                $(this).remove();
            }
        }
    }
});

$("#Paid").blur(function () {
    DueAmount();
});

function DueAmount() {
    var Due = 0
    Due = (GrandTotal - parseFloat($("#Paid").val()));
    $("#Due").val(Due);
}

function validation() {
    var message = '';
    if ($("#IngredientId").val() == '' || $("#IngredientId").val() == '0') {
        message = "Select ingredient"
    }
    else if ($("#UnitPrice").val() == '' || $("#UnitPrice").val() == 0) {
        message = "Enter unit price"
    }
    else if ($("#Quantity").val() == '' || $("#Quantity").val() == 0) {
        message = "Enter quantity"
    }
    for (var i = 0; i < dataArr.length; i++) {
        if ($("#IngredientId").val() == dataArr[i].ingredientId) {
            message = "Ingredient already selected!"
            break;
        }
    }
    return message;
}

function clearItem() {
    $("#IngredientId").val(''),
        $("#UnitPrice").val(''),
        $("#Quantity").val(''),
        $("#Amount").val(''),
        $("#PurchaseId").val('0')
}