var InventoryTransferDatatable;
var editDataArr = [];
var deletedId = [];
var InventoryType;
$(document).ready(function () {
    $("#InventoryTransfer").validate();
    InventoryType = $("#InventoryType").val();
    InventoryTransferDatatable = $('#InventoryTransferOrderDetail').DataTable({
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
                "Data": "", "name": "Action", "defaultContent": '<a href="#" class="deleteItem">Delete</a>', "autoWidth": true
            },
            {
                "targets": [4,6],
                "visible": false,
                "searchable": false
            }

        ]
    });
    $("#FromStoreId").focus();
});

$('#cancel').on('click', function (e) {
    e.preventDefault();
    clearItem();
    dataArr.push(editDataArr[0]);
    editDataArr = [];
});

$('#addRow').on('click', function (e) {
    e.preventDefault();
    var rowId;
    var rowNode;
    var message = validation(0);

    if (InventoryType == "2") {
        rowId = "rowId" + $("#IngredientId").val();
    }
    if (InventoryType == 1) {
        rowId = "rowId" + $("#FoodMenuId").val();
    }

    var Qty = $("#Quantity").val();
    Qty = parseFloat(Qty).toFixed(2);
    var CurrentStock = $("#StockQty").val();
    CurrentStock = parseFloat(CurrentStock).toFixed(2);

    var ProductUnit = $("#ProductUnit").val();
    ProductUnit = "PC";
    if (message == '') {
        InventoryTransferDatatable.row('.active').remove().draw(false);
        if (InventoryType == "2") {
            rowNode = InventoryTransferDatatable.row.add([
                '<td class="text-right">' + $("#IngredientId").val() + ' </td>',
                $('#IngredientId').children("option:selected").text(),
                '<td class="text-right">' + CurrentStock + ' </td>',
                '<td class="text-right">' + Qty + ' </td>',
                '<td class="text-right">' + ProductUnit + ' </td>',
                '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#IngredientId").val() + '">Delete</a><a href="#" data-toggle="modal" data-target="#myModal' + $("#IngredientId").val() + '"></a></div></td > ' +
                '<div class="modal fade" id=myModal' + $("#IngredientId").val() + ' tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
                '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">' +
                'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#IngredientId").val() + '" onclick="deleteOrder(' + $("#IngredientId").val() + ',' + rowId + ')" class="btn bg-danger mr-1" data-dismiss="modal">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
                $("#InventoryTransferId").val()
            ]).node().id = rowId;
        }

        if (InventoryType == "1") {
            rowNode = InventoryTransferDatatable.row.add([
                '<td class="text-right">' + $("#FoodMenuId").val() + ' </td>',
                $('#FoodMenuId').children("option:selected").text(),
                '<td class="text-right">' + CurrentStock + ' </td>',
                '<td class="text-right">' + Qty + ' </td>',
                '<td class="text-right">' + ProductUnit + ' </td>',
                '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#FoodMenuId").val() + '"></a><a href="#" data-toggle="modal" data-target="#myModal' + $("#FoodMenuId").val() + '">Delete</a></div></td > ' +
                '<div class="modal fade" id=myModal' + $("#FoodMenuId").val() + ' tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
                '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">' +
                'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteOrder(0, ' + $("#FoodMenuId").val() + ',' + rowId + ')" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
              $("#InventoryTransferId").val()
            ]).node().id = rowId;
        }
        debugger;
        $(rowNode).find('td').eq(1).addClass('text-right');
        $(rowNode).find('td').eq(2).addClass('text-right');
        $(rowNode).find('td').eq(3).addClass('text-right');
  
        InventoryTransferDatatable.draw(false);

        if (InventoryType == "2") {
            dataArr.push({
                ingredientId: $("#IngredientId").val(),
                currentStock: $("#StockQty").val().toFixed(2),
                quantity: $("#Quantity").val().toFixed(2),
                productUnit: $("#ProductUnit").val(),
                //consumpationStatus: $("#ConsumpationStatus").val(),
                InventoryTransferId: $("#InventoryTransferId").val(),
                ingredientName: $('#IngredientId').children("option:selected").text()
            });
        }

        if (InventoryType == "1") {
            dataArr.push({
                foodMenuId: $("#FoodMenuId").val(),
                currentStock: $("#StockQty").val(),
                quantity: $("#Quantity").val(),
                productUnit: $("#ProductUnit").val(),
                //consumpationStatus: $("#ConsumpationStatus").val(),
                InventoryTransferId: $("#InventoryTransferId").val(),
                foodMenuName: $('#FoodMenuId').children("option:selected").text()
            });
        }

  
        clearItem();
        editDataArr = [];

        if (InventoryType == "2") {
            $("#IngredientId").focus();
        }

        if (InventoryType == "1") {
            $("#FoodMenuId").focus();
        }
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
        url: 'InventoryTransfer',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        var message = validation(1);
        if (message == '') {
            $("#InventoryTransfer").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    InventoryType: InventoryType,
                    FromStoreId: $("#FromStoreId").val(),
                    ToStoreId: $("#ToStoreId").val(),
                    EmployeeId: $("#EmployeeId").val(),
                    IngredientId: $("#IngredientId").val(),
                    Date: $("#Date").val(),
                    Notes: $("#Notes").val(),
                    EmployeeList: [],
                    IngredientList: [],
                    FromStoreList: [],
                    ToStoreList: [],
                    InventoryTransferDetail: dataArr,
                    DeletedId: deletedId
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
        else {
            $(".modal-body").text(message);
            $("#save").hide();
            jQuery.noConflict();
            $("#aModal").modal('show');
            return false;
        }
    })


});

$("#save").click(function () {
    window.location.href = "/InventoryTransfer/InventoryTransferList";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

//function deleteOrderItem(id) {
//    return $.ajax({
//        dataType: "json",
//        type: "GET",
//        url: "/InventoryTransfer/DeleteInventoryTransferDetails",
//        data: "InventoryTransferId=" + id
//    });
//}

function deleteOrder(id, rowId) {
    debugger;
    for (var i = 0; i < dataArr.length; i++) {
        if (InventoryType == "2") {
            if (dataArr[i].ingredientId == id) {
                deletedId.push(dataArr[i].InventoryTransferId);
                dataArr.splice(i, 1);
                InventoryTransferDatatable.row(rowId).remove().draw(false);
                jQuery.noConflict();
                $("#myModal" + id).modal('hide');
            }
        }
        if (InventoryType == "1") {
            if (dataArr[i].foodMenuId == id) {
                deletedId.push(dataArr[i].inventoryTransferId);
                dataArr.splice(i, 1);
                InventoryTransferDatatable.row(rowId).remove().draw(false);
                jQuery.noConflict();
                $("#myModal" + id).modal('hide');
            }
        }
    }
};

$(document).on('click', 'a.editItem', function (e) {
    if (!InventoryTransferDatatable.data().any() || InventoryTransferDatatable.data().row == null) {
        var message = 'No data available!'
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
    }
    else {
        e.preventDefault();
        if (editDataArr.length > 0) {
            dataArr.push(editDataArr[0]);
        }
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
        }
        else {
            $(this).parents('tr').removeClass('active');
            $(this).parents('tr').addClass('active');
        }
        var id = $(this).attr('data-itemId');
        for (var i = 0; i < dataArr.length; i++) {
            if (InventoryType == "2") {
                if (dataArr[i].ingredientId == id) {
                    $("#IngredientId").val(dataArr[i].ingredientId),
                        $("#Quantity").val(dataArr[i].quantity),
                        //$("#ConsumpationStatus").val(dataArr[i].consumpationStatus),
                        $("#InventoryTransferId").val(dataArr[i].InventoryTransferId);
                    editDataArr = dataArr.splice(i, 1);
                }
            }
            if (InventoryType == "1") {
                if (dataArr[i].foodMenuId == id) {
                    $("#FoodMenuId").val(dataArr[i].foodMenuId),
                        $("#Quantity").val(dataArr[i].quantity),
                        //$("#ConsumpationStatus").val(dataArr[i].consumpationStatus),
                        $("#InventoryTransferId").val(dataArr[i].InventoryTransferId);
                    editDataArr = dataArr.splice(i, 1);
                }
            }
        }
    }
});

function validation(id) {
    var message = '';

    if ($("#FromStoreId").val() == '' || $("#FromStoreId").val() == '0') {
        message = "Select From Store"
    }
    else if ($("#ToStoreId").val() == '' || $("#ToStoreId").val() == '0') {
        message = "Select To Store"
    }
    else if ($("#FromStoreId").val() == $("#ToStoreId").val()) {
        message = "Select From Store And To Store Different"
    }
    else if ($("#EmployeeId").val() == '' || $("#EmployeeId").val() == '0') {
        message = "Select Supplier"
    }
    else {
        if (id == 1) {
            if (!InventoryTransferDatatable.data().any() || InventoryTransferDatatable.data().row == null) {
                var message = 'At least one detail should be entered'
                return message;
            }
        }
        else {
            if (id == 0) {
                if (InventoryType == "2") {
                    if ($("#IngredientId").val() == '' || $("#IngredientId").val() == '0') {
                        message = "Select ingredient"
                    }
                    else if ($("#Quantity").val() == '' || $("#Quantity").val() == 0) {
                        message = "Enter Quantity"
                    }
                    //else if ($("#ConsumpationStatus").val() == '' || $("#ConsumpationStatus").val() == 0) {
                    //    message = "Select Comsumption Status"
                    //}

                    for (var i = 0; i < dataArr.length; i++) {
                        if ($("#IngredientId").val() == dataArr[i].ingredientId) {
                            message = "Ingredient already selected!"
                            break;
                        }
                    }
                }
                if (InventoryType == "1") {
                    if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == '0') {
                        message = "Select Product"
                    }
                    else if ($("#Quantity").val() == '' || $("#Quantity").val() == 0) {
                        message = "Enter Quantity"
                    }
                    //else if ($("#ConsumpationStatus").val() == '' || $("#ConsumpationStatus").val() == 0) {
                    //    message = "Select Comsumption Status"
                    //}

                    for (var i = 0; i < dataArr.length; i++) {
                        if ($("#FoodMenuId").val() == dataArr[i].foodMenuId) {
                            message = "Food already selected!"
                            break;
                        }
                    }
                }
            }
        }
    }
    return message;

}

function clearItem() {
    $("#IngredientId").val('0'),
        $("#FoodMenuId").val('0'),
        $("#CurrentStock").val('0'),
        $("#ProductUnit").val('0'),
        //$("#ConsumpationStatus").val(''),
        $("#Quantity").val('1'),
        $("#InventoryTransferId").val('0')

    $("#FoodMenuId").focus()
}


function GetFoodMenuStock(foodMenuId, storeId) {
    $.ajax({
        url: "/InventoryTransfer/GetFoodMenuStock",
        data: { "foodMenuId": foodMenuId.value, "storeId": storeId.value },
        type: "GET",
        dataType: "text",
        success: function (data) {
            $("#StockQty").val('');
            var obj = JSON.parse(data);
            $("#StockQty").val(parseFloat(obj.stockQty));
        },
        error: function (data) {
            alert(data);
        }
    });
}
