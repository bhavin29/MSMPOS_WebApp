var editFoodMenuDataArr = [];
var foodMenuDeletedId = [];
var FoodMenuPurchasePrice = 0;
var IngredientPurchasePrice = 0;
var AssetItemPurchasePrice = 0;
var InventoryType;
$(document).ready(function () {
    $("#InventoryAlteration").validate();
    InventoryType = $("#InventoryType").val();
    FoodMenuTable = $('#FoodMenuTable').DataTable({
        columnDefs: [
            { targets: [0,6], orderable: false, visible: false },
            { targets: [2, 3, 4], orderable: false, class: "text-right" }
        ],
        "paging": false,
        "bLengthChange": true,
        "bInfo": false,
        "bFilter": false,
        "ordering": true,
        "autoWidth": false,
        "orderCellsTop": true,
        "stateSave": false,
        "pageLength": 200,
        "lengthMenu": [
            [200, 500, 1000],
            ['200', '500', '1000']
        ],
    });
    $("#StoreId").select2();
    $("#FoodMenuId").select2();
    $("#IngredientId").select2();
    $("#AssetItemId").select2();
    $("#StoreId").focus();
});


$('#addFoodMenuRow').on('click', function (e) {
    e.preventDefault();
    var message = validation(0);
    var rowNode;
    var rowId;
    if (message == '') {
        debugger;

        if (InventoryType == 1) {
            rowId =  $("#FoodMenuId").val();
        }
        else if (InventoryType == "2") {
            rowId = $("#IngredientId").val();
        }
        else if (InventoryType == 3) {
            rowId =  $("#AssetItemId").val();
        }

        FoodMenuTable.row('.active').remove().draw(false);

        if (InventoryType == "1") {
            rowNode = FoodMenuTable.row.add([
                $("#FoodMenuId").val(),
                $('#FoodMenuId').children("option:selected").text(),
                $("#Qty").val(),
                $("#Amount").val(),
                $("#InventoryStockQty").val(),
                '<td><div class="form-button-action"><a href="#" data-itemId="' + rowId + '></a><a href="#" data-toggle="modal" data-target="#myModal' + rowId + '">Delete</a></div></td > ' +
                '<div class="modal fade" id="myModal' + rowId + '" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
                '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
                'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteFoodMenuOrder(0, ' + $("#FoodMenuId").val() + ',' + rowId + ')" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
                rowId
            ]).node().id = rowId;
        }
        if (InventoryType == "2") {
            rowNode = FoodMenuTable.row.add([
                $("#IngredientId").val(),
                $('#IngredientId').children("option:selected").text(),
                $("#Qty").val(),
                $("#Amount").val(),
                $("#InventoryStockQty").val(),
                '<td><div class="form-button-action"><a href="#" data-itemId="' + rowId + '></a><a href="#" data-toggle="modal" data-target="#myModal' + rowId + '">Delete</a></div></td > ' +
                '<div class="modal fade" id="myModal' + rowId + '" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
                 '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
                'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#IngredientId").val() + '" onclick="deleteFoodMenuOrder(0, ' + $("#IngredientId").val() + ',' + rowId + ')" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
                rowId
            ]).node().id = rowId;
        }

        if (InventoryType == "3") {
            rowNode = FoodMenuTable.row.add([
                    $("#AssetItemId").val(),
                $('#AssetItemId').children("option:selected").text(),
                $("#Qty").val(),
                $("#Amount").val(),
                $("#InventoryStockQty").val(),
                '<td><div class="form-button-action"><a href="#" data-itemId="' + rowId + '></a><a href="#" data-toggle="modal" data-target="#myModal' + rowId + '">Delete</a></div></td > ' +
                '<div class="modal fade" id="myModal' + rowId + '" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
                '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
                'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#AssetItemId").val() + '" onclick="deleteFoodMenuOrder(0, ' + $("#AssetItemId").val() + ',' + rowId + ')" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
                rowId
            ]).node().id = rowId;
        }

        FoodMenuTable.draw(false);

        if (InventoryType == "1") {
            foodMenuDataArr.push({
                id: $("#Id").val(),
                inventoryAlterationId: $("#InventoryAlterationId").val(),
                foodMenuId: $("#FoodMenuId").val(),
                foodMenuName: $('#FoodMenuId').children("option:selected").text(),
                qty: $("#Qty").val(),
                inventoryStockQty: $("#InventoryStockQty").val(),
                amount: $("#Amount").val()
            });
        } else if (InventoryType == "2") {
            foodMenuDataArr.push({
                id: $("#Id").val(),
                inventoryAlterationId: $("#InventoryAlterationId").val(),
                ingredientId: $("#IngredientId").val(),
                ingredientName: $('#IngredientId').children("option:selected").text(),
                qty: $("#Qty").val(),
                inventoryStockQty: $("#InventoryStockQty").val(),
                amount: $("#Amount").val()
            });
        } else if (InventoryType == "3") {
            foodMenuDataArr.push({
                id: $("#Id").val(),
                inventoryAlterationId: $("#InventoryAlterationId").val(),
                assetItemId: $("#AssetItemId").val(),
                assetItemName: $('#AssetItemId').children("option:selected").text(),
                qty: $("#Qty").val(),
                inventoryStockQty: $("#InventoryStockQty").val(),
                amount: $("#Amount").val()
            });
        }

        $(rowNode).find('td').eq(2).addClass('text-right');
        $(rowNode).find('td').eq(3).addClass('text-right');

        clearFoodMenuItem();
        editFoodMenuDataArr = [];

        if (InventoryType == "1") {
            $('#FoodMenuId').val(0).trigger('change');
        }
        if (InventoryType == "2") {
            $('#IngredientId').val(0).trigger('change');
        }
        if (InventoryType == "3") {
            $('#AssetItemId').val(0).trigger('change');
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

function deleteFoodMenuOrder(id, foodMenuId, rowId) {

    for (var i = 0; i < foodMenuDataArr.length; i++) {
        debugger;
        if (InventoryType == "1") {
            if (foodMenuDataArr[i].foodMenuId == foodMenuId) {
                foodMenuDeletedId.push(foodMenuDataArr[i].foodMenuId);
                foodMenuDataArr.splice(i, 1);
                FoodMenuTable.row('#'+rowId).remove().draw(false);
                jQuery.noConflict();
                $("#myModal" + foodMenuId).modal('hide');
            }
        }

        if (InventoryType == "2") {
            if (foodMenuDataArr[i].ingredientId == foodMenuId) {
                foodMenuDeletedId.push(foodMenuDataArr[i].ingredientId);
                foodMenuDataArr.splice(i, 1);
                FoodMenuTable.row('#' + rowId).remove().draw(false);
                jQuery.noConflict();
                $("#myModal" + foodMenuId).modal('hide');
            }
        }

        if (InventoryType == "3") {
            if (foodMenuDataArr[i].assetItemId == foodMenuId) {
                foodMenuDeletedId.push(foodMenuDataArr[i].assetItemId);
                foodMenuDataArr.splice(i, 1);
                FoodMenuTable.row('#' + rowId).remove().draw(false);
                jQuery.noConflict();
                $("#myModal" + foodMenuId).modal('hide');
            }
        }
    }
};


$(document).on('click', 'a.editFoodMenuItem', function (e) {
    if (!FoodMenuTable.data().any() || FoodMenuTable.data().row == null) {
        var message = 'No data available!'
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
    }
    else {
        e.preventDefault();
        if (editFoodMenuDataArr.length > 0) {
            foodMenuDataArr.push(editFoodMenuDataArr[0]);
        }
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
        }
        else {
            $(this).parents('tr').removeClass('active');
            $(this).parents('tr').addClass('active');
        }
        var id = $(this).attr('data-itemId');
        for (var i = 0; i < foodMenuDataArr.length; i++) {

            if (InventoryType == "1") {
                if (foodMenuDataArr[i].foodMenuId == id) {
                    $('#FoodMenuId').val(foodMenuDataArr[i].foodMenuId).trigger('change');
                    $("#Qty").val(foodMenuDataArr[i].qty);
                    $("#Amount").val(foodMenuDataArr[i].amount);
                    $("#InventoryStockQty").val(foodMenuDataArr[i].inventoryStockQty);
                    editFoodMenuDataArr = foodMenuDataArr.splice(i, 1);
                }
            }
            if (InventoryType == "2") {
                if (foodMenuDataArr[i].ingredientId == id) {
                    $('#IngredientId').val(foodMenuDataArr[i].ingredientId).trigger('change');
                    $("#Qty").val(foodMenuDataArr[i].qty);
                    $("#Amount").val(foodMenuDataArr[i].amount);
                    $("#InventoryStockQty").val(foodMenuDataArr[i].inventoryStockQty);
                    editFoodMenuDataArr = foodMenuDataArr.splice(i, 1);
                }
            }
            if (InventoryType == "3") {
                if (foodMenuDataArr[i].assetItemId == id) {
                    $('#AssetItemId').val(foodMenuDataArr[i].assetItemId).trigger('change');
                    $("#Qty").val(foodMenuDataArr[i].qty);
                    $("#Amount").val(foodMenuDataArr[i].amount);
                    $("#InventoryStockQty").val(foodMenuDataArr[i].inventoryStockQty);
                    editFoodMenuDataArr = foodMenuDataArr.splice(i, 1);
                }
            }
        }
    }
});

function validation(id) {
    var message = '';
    if (id == 0) {
        if (InventoryType == "1") {
            if ($("#StoreId").val() == '' || $("#StoreId").val() == '0') {
                message = "Select Store"
            }
            else if ($("#Qty").val() == '' || $("#Qty").val() == '0') {
                message = "Select Quantity"
            }
            else if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == 0) {
                message = "Select Menu item"
            }
            else {
                for (var i = 0; i < foodMenuDataArr.length; i++) {
                    if ($("#FoodMenuId").val() == foodMenuDataArr[i].foodMenuId) {
                        message = "Menu item already selected!"
                        break;
                    }
                }
            }
        }

        if (InventoryType == "2") {
            if ($("#StoreId").val() == '' || $("#StoreId").val() == '0') {
                message = "Select Store"
            }
            else if ($("#Qty").val() == '' || $("#Qty").val() == '0') {
                message = "Select Quantity"
            }
            else if ($("#IngredientId").val() == '' || $("#IngredientId").val() == 0) {
                message = "Select stock item"
            }
            else {
                for (var i = 0; i < foodMenuDataArr.length; i++) {
                    if ($("#IngredientId").val() == foodMenuDataArr[i].foodMenuId) {
                        message = "Stock item already selected!"
                        break;
                    }
                }
            }
        }

        if (InventoryType == "3") {
            if ($("#StoreId").val() == '' || $("#StoreId").val() == '0') {
                message = "Select Store"
            }
            else if ($("#Qty").val() == '' || $("#Qty").val() == '0') {
                message = "Select Quantity"
            }
            else if ($("#AssetItemId").val() == '' || $("#AssetItemId").val() == 0) {
                message = "Select asset item"
            }
            else {
                for (var i = 0; i < foodMenuDataArr.length; i++) {
                    if ($("#AssetItemId").val() == foodMenuDataArr[i].assetItemId) {
                        message = "Asset item already selected!"
                        break;
                    }
                }
            }
        }
    }

    if (id == 1) {
        if (!FoodMenuTable.data().any() || FoodMenuTable.data().row == null) {
            var message = 'At least one menu item should be entered'
            return message;
        }

        if ($("#StoreId").val() == '') {
            message = "Select Store"
            $("#StoreId").focus();
            return message;
        }
    }
    return message;
}

function clearFoodMenuItem() {
    $("#Qty").val(parseFloat(1.00).toFixed(2));
    $("#InventoryStockQty").val(parseFloat(0.00).toFixed(2));
    $("#Amount").val('0');

    $('#IngredientId').val(0).trigger('change');
    $('#AssetItemId').val(0).trigger('change');
    $('#FoodMenuId').val(0).trigger('change');
}



function saveOrder(data) {
    console.log(data);
    return $.ajax({
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        type: 'POST',
        beforeSend: function (xhr) { xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val()); },
        url: 'InventoryAlteration',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        var message = validation(1);
        if (message == '') {
            $("#inventoryAlterationForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    InventoryType: $("#InventoryType").val(),
                    Notes: $("#Notes").val(),
                    InventoryAlterationDetails: foodMenuDataArr,
                    StoreId: $("#StoreId").val()
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

$('#save').click(function () {
    window.location.href = "/InventoryAlteration/Index";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

function GetFoodMenuPurchasePrice() {
    GetInventoryStockQty();
    $.ajax({
        url: "/Waste/GetFoodMenuPurchasePrice",
        data: { "id": $("#FoodMenuId").val() },
        async: false,
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            FoodMenuPurchasePrice = obj.foodMenuPurchasePrice;
            FoodMenuPurchasePrice = parseFloat(FoodMenuPurchasePrice).toFixed(2);
            $("#Amount").val(FoodMenuPurchasePrice);
        }
    });
}

$("#Qty").change(function () {
    if (InventoryType == "1") {
        $("#Amount").val(parseFloat(FoodMenuPurchasePrice) * parseFloat($("#Qty").val()));
    }
    if (InventoryType == "2") {
        $("#Amount").val(parseFloat(IngredientPurchasePrice) * parseFloat($("#Qty").val()));
    }
    if (InventoryType == "3") {
        $("#Amount").val(parseFloat(AssetItemPurchasePrice) * parseFloat($("#Qty").val()));
    }
});

function GetInventoryStockQty() {
    $.ajax({
        url: "/InventoryAlteration/GetInventoryStockQty",
        data: { "storeId": $("#StoreId").val(), "foodMenuId": $("#FoodMenuId").val() },
        async: false,
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            $("#InventoryStockQty").val(parseFloat(obj.stockQty).toFixed(2));
        }
    });
}

function GetInventoryStockQtyForIngredient() {
    $.ajax({
        url: "/InventoryAlteration/GetInventoryStockQtyForIngredient",
        data: { "storeId": $("#StoreId").val(), "ingredientId": $("#IngredientId").val() },
        async: false,
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            $("#InventoryStockQty").val(parseFloat(obj.stockQty).toFixed(2));
        }
    });
}

function GetIngredientPurchasePrice() {
    GetInventoryStockQtyForIngredient();
    $.ajax({
        url: "/Waste/GetIngredientPurchasePrice",
        data: { "id": $("#IngredientId").val() },
        async: false,
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            IngredientPurchasePrice = obj.ingredientPurchasePrice;
            IngredientPurchasePrice = parseFloat(IngredientPurchasePrice).toFixed(2);
            $("#Amount").val(IngredientPurchasePrice);
        }
    });
}

function GetFoodMenuLastPrice(foodMenuId) {
    var itemType = 2;

    $.ajax({
        url: "/PurchaseInvoiceFoodMenu/GetFoodMenuLastPrice",
        data: { "itemType": itemType, "foodMenuId": foodMenuId.value },
        type: "GET",
        dataType: "text",
        success: function (data) {
            $("#UnitPrice").val('');
            var obj = JSON.parse(data);
            AssetItemPurchasePrice = parseFloat(obj.unitPrice);
            $("#Amount").val(AssetItemPurchasePrice);
        },
        error: function (data) {
            alert(data);
        }
    });
}