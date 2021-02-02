﻿var InventoryAdjustmentDatatable;
var editDataArr = [];
var deletedId = [];
var InventoryType;
$(document).ready(function () {
    $("#InventoryAdjustment").validate();
    InventoryType = $("#InventoryType").val();
    InventoryAdjustmentDatatable = $('#InventoryAdjustmentOrderDetails').DataTable({
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
                "targets": [6],
                "visible": false,
                "searchable": false
            }

        ]
    });
    $("#FoodMenuId").focus();
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
    var Price;
    var message = validation(0);

    if (InventoryType == "2") {
        rowId = "rowId" + $("#IngredientId").val();
    }
    if (InventoryType == 1) {
        rowId = "rowId" + $("#FoodMenuId").val();
    }

    $.ajax({
        url: "/InventoryAdjustment/GetFoodMenuPurchasePrice",
        data: { "foodMenuId": $("#FoodMenuId").val() },
        async: false,
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            Price = obj.purchasePrice;
            Price = parseFloat(Price).toFixed(2);
        }
    });

    var Qty = $("#Quantity").val();
    //var Price = 1;//$("#Price").val('0');
    var TotalAmount = $("#TotalAmount").val('0');
    $("#Price").val(Price);
    Qty = parseFloat(Qty).toFixed(4);
    Price = parseFloat(Price).toFixed(2);
    TotalAmount = parseFloat(Qty * Price).toFixed(2);
    $("#TotalAmount").val(TotalAmount);

    if (message == '') {
        InventoryAdjustmentDatatable.row('.active').remove().draw(false);

        if (InventoryType == "2") {
            rowNode = InventoryAdjustmentDatatable.row.add([
                '<td class="text-right">' + $("#IngredientId").val() + ' </td>',
                $('#IngredientId').children("option:selected").text(),
                '<td class="text-right">' + Qty + ' </td>',
                '<td class="text-right">' + Price + ' </td>',
                '<td class="text-right">' + Total + ' </td>',
                '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#IngredientId").val() + '" ">Delete</a><a href="#"  data-toggle="modal" data-target="#myModal' + $("#IngredientId").val() + '"></a></div></td > ' +
                '<div class="modal fade" id=myModal' + $("#IngredientId").val() + ' tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
                '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">' +
                'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#IngredientId").val() + '" onclick="deleteOrder(' + $("#IngredientId").val() + ',' + rowId + ')" class="btn bg-danger mr-1" data-dismiss="modal">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
                $("#InventoryAdjustmentId").val()
            ]).node().id = rowId;
        }
        if (InventoryType == "1") {
            rowNode = InventoryAdjustmentDatatable.row.add([
                '<td class="text-right">' + $("#FoodMenuId").val() + ' </td>',
                $('#FoodMenuId').children("option:selected").text(),
                '<td class="text-right">' + Qty + ' </td>',
                '<td class="text-right">' + Price + ' </td>',
                '<td class="text-right">' + TotalAmount + ' </td>',
                '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#FoodMenuId").val() + '" ">Delete</a><a href="#" data-toggle="modal" data-target="#myModal' + $("#FoodMenuId").val() + '"></a></div></td > ' +
                '<div class="modal fade" id=myModal' + $("#FoodMenuId").val() + ' tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
                '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">' +
                'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteOrder(' + $("#FoodMenuId").val() + ',' + rowId + ')" class="btn bg-danger mr-1" data-dismiss="modal">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
                $("#InventoryAdjustmentId").val()
            ]).node().id = rowId;
        }
        InventoryAdjustmentDatatable.draw(false);

        if (InventoryType == "2") {
            dataArr.push({
                ingredientId: $("#IngredientId").val(),
                quantity: $("#Quantity").val(),
                price: $("#Price").toFixed(2),
                totalAmount: $("#TotalAmount").toFixed(2),
                //consumpationStatus: $("#ConsumpationStatus").val(),
                inventoryAdjustmentId: $("#InventoryAdjustmentId").val(),
                ingredientName: $('#IngredientId').children("option:selected").text()
            });
        }

        if (InventoryType == "1") {
            dataArr.push({
                foodMenuId: $("#FoodMenuId").val(),
                quantity: $("#Quantity").val(),
                price: $("#Price").val(),
                totalAmount: $("#TotalAmount").val(),
                //consumpationStatus: $("#ConsumpationStatus").val(),
                inventoryAdjustmentId: $("#InventoryAdjustmentId").val(),
                foodMenuName: $('#FoodMenuId').children("option:selected").text()
            });
        }
        debugger;
        $(rowNode).find('td').eq(1).addClass('text-right');
        $(rowNode).find('td').eq(2).addClass('text-right');
        $(rowNode).find('td').eq(3).addClass('text-right');

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
        url: 'InventoryAdjustment',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        var message = validation(1);
        if (message == '') {
            $("#InventoryAdjustment").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    InventoryType: InventoryType,
                    StoreId: $("#StoreId").val(),
                    EmployeeId: $("#EmployeeId").val(),
                    Date: $("#Date").val(),
                    Notes: $("#Notes").val(),
                    StoreList: [],
                    EmployeeList: [],
                    IngredientList: [],
                    InventoryAdjustmentDetail: dataArr,
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
    window.location.href = "/InventoryAdjustment/InventoryAdjustmentList";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

function deleteOrder(id, rowId) {
    for (var i = 0; i < dataArr.length; i++) {
        if (InventoryType == "2") {
            if (dataArr[i].ingredientId == id) {
                deletedId.push(dataArr[i].inventoryAdjustmentId);
                dataArr.splice(i, 1);
                InventoryAdjustmentDatatable.row(rowId).remove().draw(false);
                jQuery.noConflict();
                $("#myModal" + id).modal('hide');
            }
        }
        if (InventoryType == "1") {
            if (dataArr[i].foodMenuId == id) {
                deletedId.push(dataArr[i].inventoryAdjustmentId);
                dataArr.splice(i, 1);
                InventoryAdjustmentDatatable.row(rowId).remove().draw(false);
                jQuery.noConflict();
                $("#myModal" + id).modal('hide');
            }
        }
    }
};

$(document).on('click', 'a.editItem', function (e) {
    if (!InventoryAdjustmentDatatable.data().any() || InventoryAdjustmentDatatable.data().row == null) {
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
            if (InventoryType == "2") {
                if (dataArr[i].ingredientId == id) {
                    $("#IngredientId").val(dataArr[i].ingredientId),
                        $("#Quantity").val(dataArr[i].quantity),
                        $("#Price").val(dataArr[i].price),
                        $("#TotalAmount").val(dataArr[i].totalAmount),
                        //$("#ConsumpationStatus").val(dataArr[i].consumpationStatus),
                        $("#InventoryAdjustmentId").val(dataArr[i].inventoryAdjustmentId);
                    editDataArr = dataArr.splice(i, 1);
                }
            }
            if (InventoryType == "1") {
                if (dataArr[i].foodMenuId == id) {
                    $("#FoodMenuId").val(dataArr[i].foodMenuId),
                        $("#Quantity").val(dataArr[i].quantity),
                        $("#Price").val(dataArr[i].price),
                        $("#TotalAmount").val(dataArr[i].totalAmount),
                        //$("#ConsumpationStatus").val(dataArr[i].consumpationStatus),
                        $("#InventoryAdjustmentId").val(dataArr[i].inventoryAdjustmentId);
                    editDataArr = dataArr.splice(i, 1);
                }
            }
        }
    }
});

function validation(id) {
    var message = '';

    if (id == 1) {
        if (!InventoryAdjustmentDatatable.data().any() || InventoryAdjustmentDatatable.data().row == null) {
            var message = 'At least one order should be entered'
            return message;
        }
        if ($("#StoreId").val() == '') {
            message = "Select Store"
            $("#StoreId").focus();
            return message;
        }
    }

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
                $("#FoodMenuId").focus();
            }
            else if ($("#Quantity").val() == '' || $("#Quantity").val() == 0) {
                message = "Enter Quantity"
                $("#Quantity").focus();
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
    return message;
}

function clearItem() {
    $("#IngredientId").val('0'),
        $("#FoodMenuId").val('0'),
        //$("#ConsumpationStatus").val(''),
        $("#Quantity").val('1'),
        $("#Price").val(''),
        $("#TotalAmount").val(''),
        $("#InventoryAdjustmentId").val('0')
}