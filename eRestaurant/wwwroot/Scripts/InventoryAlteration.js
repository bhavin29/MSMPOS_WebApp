var editFoodMenuDataArr = [];
var foodMenuDeletedId = [];
var FoodMenuPurchasePrice = 0;
$(document).ready(function () {
    $("#InventoryAlteration").validate();
    FoodMenuTable = $('#FoodMenuTable').DataTable({
        columnDefs: [
            { targets: [0], orderable: false, visible: false },
            { targets: [2,3], orderable: false, class: "text-right" }
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
});


$('#addFoodMenuRow').on('click', function (e) {
    e.preventDefault();
    var message = validation(0);
    var rowId = "rowId" + $("#FoodMenuId").val();
    
    var Qty = $("#Qty").val();
    var Amount = $("#Amount").val();
    if (message == '') {
        FoodMenuTable.row('.active').remove().draw(false);
        var rowNode = FoodMenuTable.row.add([
            $("#FoodMenuId").val(),
            $('#FoodMenuId').children("option:selected").text(),
            $("#Qty").val(),
            $("#Amount").val(),
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#FoodMenuId").val() + '" class=" editFoodMenuItem">Edit</a></a> / <a href="#" data-toggle="modal" data-target="#myModal0">Delete</a></div></td > ' +
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteFoodMenuOrder(0, ' + $("#FoodMenuId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
        ]).node().id = rowId;
        FoodMenuTable.draw(false);
        foodMenuDataArr.push({
            id: $("#Id").val(),
            inventoryAlterationId: $("#InventoryAlterationId").val(),
            foodMenuId: $("#FoodMenuId").val(),
            foodMenuName: $('#FoodMenuId').children("option:selected").text(),
            qty: $("#Qty").val(),
            amount: $("#Amount").val()
        });
        $(rowNode).find('td').eq(2).addClass('text-right');
        $(rowNode).find('td').eq(3).addClass('text-right');

        clearFoodMenuItem();
        editFoodMenuDataArr = [];

        $("#FoodMenuId").focus();
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
        if (foodMenuDataArr[i].foodMenuId == foodMenuId) {
            foodMenuDeletedId.push(foodMenuDataArr[i].foodMenuId);
            foodMenuDataArr.splice(i, 1);
            FoodMenuTable.row(rowId).remove().draw(false);
            jQuery.noConflict();
            $("#myModal" + foodMenuId).modal('hide');
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
            if (foodMenuDataArr[i].foodMenuId == id) {
                $("#FoodMenuId").val(foodMenuDataArr[i].foodMenuId);
                $("#Qty").val(foodMenuDataArr[i].qty);
                $("#Amount").val(foodMenuDataArr[i].amount);
                editFoodMenuDataArr = foodMenuDataArr.splice(i, 1);
            }
        }
    }
});

function validation(id) {
    var message = '';
    if (id == 0) {
        if ($("#StoreId").val() == '' || $("#StoreId").val() == '0') {
            message = "Select Store"
        }

        if ($("#Qty").val() == '' || $("#Qty").val() == '0') {
            message = "Select Quantity"
        }
        else if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == 0) {
            message = "Select Menu item"
        }

        for (var i = 0; i < foodMenuDataArr.length; i++) {
            if ($("#FoodMenuId").val() == foodMenuDataArr[i].foodMenuId) {
                message = "Menu item already selected!"
                break;
            }
        }
    }

    if (id == 1) {
        if (!FoodMenuTable.data().any() || FoodMenuTable.data().row == null) {
            var message = 'At least one menu item should be entered'
            return message;
        }
    }
    return message;
}

function clearFoodMenuItem() {
    $("#FoodMenuId").val('0');
    $("#Qty").val(parseFloat(1.00).toFixed(2));
    $("#Amount").val('0');
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
    $("#Amount").val(parseFloat(FoodMenuPurchasePrice) * parseFloat($("#Qty").val()));
});