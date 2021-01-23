var WasteDatatable;
var FoodManuLostAmount = 0;
var IngredientLostAmount = 0;
var editDataArr = [];
var deletedId = [];
$(document).ready(function () {
    $("#Waste").validate();
    WasteDatatable = $('#WasteOrderDetails').DataTable({
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
                "targets": [2],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [6],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [7],
                "Data": "", "name": "Action", "defaultContent": '<a href="#" class="deleteItem">Delete Order</a>', "autoWidth": true
            }
        ]
    });
});

$('#Cancle').on('click', function (e) {
    e.preventDefault();
    TotalLossAmount += editDataArr[0].lossAmount;
    $("#TotalLossAmount").val(parseFloat(TotalLossAmount).toFixed(4));
    clearItem();
    $("#FoodMenuId").focus();
})

$('#addRow').on('click', function (e) {
    e.preventDefault();
    var message = validation(0);
    if (message == '') {
        if ($('#FoodMenuId').children("option:selected").text() == "--Select--") {
            $('#FoodMenuId').val('0');
        }
        if ($('#IngredientId').children("option:selected").text() == "--Select--") {
            $("#IngredientId").val('0')
        }
        var dataItemId = $("#FoodMenuId").val() + "|" + $("#IngredientId").val();
        var myModal = $("#FoodMenuId").val() + $("#IngredientId").val()
        var rowId = "rowId" + $("#FoodMenuId").val() + $("#IngredientId").val()
        var FoodMenuName = '';
        var IngredietnName = '';
        if ($('#FoodMenuId').val() != 0) {
            FoodMenuName = $('#FoodMenuId').children("option:selected").text();
        }
        if ($('#IngredientId').val() != 0) {
            IngredietnName = $('#IngredientId').children("option:selected").text();
        }

        var Qty = parseFloat($("#Quantity").val()).toFixed(2);
        var LossAmount = parseFloat($("#LossAmount").val()).toFixed(4);

        WasteDatatable.row('.active').remove().draw(false);
        var rowNode = WasteDatatable.row.add([
            '<td>' + $("#FoodMenuId").val() + '</td>',
            '<td>' + FoodMenuName + '</td>',
            '<td>' + $("#IngredientId").val() + '</td>',
            '<td>' + IngredietnName + '</td>',
            '<td class="text-right">' + Qty + ' </td>',
            '<td class="text-right">' + LossAmount + ' </td>',
            '<td>' + $("#WasteId").val() + '</td>',
            '<td><div class="form-button-action"><a href="#" data-itemId="' + dataItemId + '" class="btn btn-link editItem"><i class="fa fa-edit"></i></a><a href="#" class="btn btn-link btn-danger" data-toggle="modal" data-target="#myModal' + myModal + '"><i class="fa fa-times"></i></a></div></td > ' +
            '<div class="modal fade" id="myModal' + myModal + '" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + dataItemId + '" onclick="deleteOrder(0, ' + $("#IngredientId").val() + ', ' + $("#FoodMenuId").val() + ',' + rowId + ')" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >'
        ]).node().id = rowId;
        WasteDatatable.draw(false);
        dataArr.push({
            foodMenuId: $("#FoodMenuId").val(),
            ingredientId: $("#IngredientId").val(),
            qty: $("#Quantity").val(),
            lossAmount: $("#LossAmount").val(),
            wasteId: $("#WasteId").val(),
            foodMenuName: $('#FoodMenuId').children("option:selected").text(),
            ingredientName: $('#IngredientId').children("option:selected").text()
        });

        $(rowNode).find('td').eq(2).addClass('text-right');
        $(rowNode).find('td').eq(3).addClass('text-right');

        TotalLossAmount += ($("#LossAmount").val() * 1);
        $("#TotalLossAmount").val(parseFloat(TotalLossAmount).toFixed(4));
        clearItem();
        $("#FoodMenuId").focus();
    }
    else {
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
        url: 'Waste',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        var message = validation(1);
        if (message == '') {
            $("#waste").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNumber: $("#ReferenceNumber").val(),
                    EmployeeId: $("#EmployeeId").val(),
                    OutletId: $("#OutletId").val(),
                    TotalLossAmount: $("#TotalLossAmount").val(),
                    ReasonForWaste: $("#ReasonForWaste").val(),
                    WasteDateTime: $("#WasteDateTime").val(),
                    WasteStatus: $("#WasteStatus").val(),
                    EmployeeList: [],
                    IngredientList: [],
                    WasteDetail: dataArr,
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
    window.location.href = "/Waste/WasteList";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

function deleteOrderItem(id) {
    debugger;
    return $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Waste/DeleteWasteDetails",
        data: "wasteId=" + id
    });
}

function deleteOrder(wasteId, ingredientId, foodMenuId, rowId) {
    for (var i = 0; i < dataArr.length; i++) {
        if (dataArr[i].ingredientId == ingredientId && dataArr[i].foodMenuId == foodMenuId) {
            LossAmount = dataArr[i].lossAmount;
            TotalLossAmount -= LossAmount;
            $("#TotalLossAmount").val(TotalLossAmount);
            deletedId.push(wasteId + "|" + foodMenuId + "|" + ingredientId);
            dataArr.splice(i, 1);
            WasteDatatable.row(rowId).remove().draw(false);
            jQuery.noConflict();
            $("#myModal" + wasteId).modal('hide');
        }
    }
};
//else {
//    $.when(deleteOrderItem(wasteId + "|" + foodMenuId + "|" + ingredientId).then(function (res) {
//        console.log(res)
//        if (res.status == 200) {
//            for (var i = 0; i < dataArr.length; i++) {
//                if (dataArr[i].ingredientId == ingredientId && dataArr[i].foodMenuId == foodMenuId) {
//                    TotalAmount = dataArr[i].lossAmount;
//                    TotalLossAmount -= TotalAmount;
//                    $("#TotalLossAmount").val(TotalLossAmount);
//                    dataArr.splice(i, 1);
//                    WasteDatatable.row(rowId).remove().draw(false);
//                    jQuery.noConflict();
//                    $("#myModal" + i).modal('hide');
//                }
//            }
//            console.log(res);
//        }
//    }).fail(function (err) {
//        console.log(err);
//    }));
//}
//};

$(document).on('click', 'a.editItem', function (e) {
    if (!WasteDatatable.data().any() || WasteDatatable.data().row == null) {
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
        var val = id.split("|");
        for (var i = 0; i < dataArr.length; i++) {
            if (dataArr[i].ingredientId == val[1] && dataArr[i].foodMenuId == val[0]) {
                $("#IngredientId").val(dataArr[i].ingredientId),
                    $("#FoodMenuId").val(dataArr[i].foodMenuId),
                    $("#IngredientIdForLostAmount").val(dataArr[i].ingredientId)
                $("#FoodMenuIdForLostAmount").val(dataArr[i].foodMenuId);
                $("#Quantity").val(dataArr[i].qty),
                    $("#LossAmount").val(dataArr[i].lossAmount),
                    $("#WasteId").val(dataArr[i].wasteId)
                TotalLossAmount = $("#TotalLossAmount").val();
                LossAmount = dataArr[i].lossAmount;
                TotalLossAmount -= LossAmount;
                $("#TotalLossAmount").val(TotalLossAmount);
                dataArr.splice(i, 1);
                break;
            }
        }
        if ($("#IngredientId").val() > 0) {
            DropdownValueChange(1);
        }
        else {
            DropdownValueChange(0);
        }
        if (WasteModelId > 0) {
            $("#FoodMenuId").attr("disabled", "disabled");
            $("#IngredientId").attr("disabled", "disabled");
        }
    }
});

$(function () {
    $("#IngredientId").change(function () {
        DropdownValueChange(1);
        $("#Quantity").val('');
        $("#LossAmount").val('');
    });
});
$(function () {
    $("#FoodMenuId").change(function () {
        DropdownValueChange(0);
        $("#Quantity").val('');
        $("#LossAmount").val('');
    });
});
function DropdownValueChange(id) {
    debugger;
    $("#FoodMenuIdForLostAmount").val("0");
    $("#IngredientIdForLostAmount").val("0");
    FoodManuLostAmount = 0;
    IngredientLostAmount = 0;
    if (id == 1) {
        var value = $('select[name=IngredientId]').val();
        if (value != "0") {
            $("#FoodMenuId").attr("disabled", "disabled");
            $("#IngredientIdForLostAmount").val(value);
            IngredientLostAmount = $('#IngredientIdForLostAmount').children("option:selected").text()
        }
        else {
            $("#FoodMenuId").removeAttr("disabled");
        }
    }
    else {
        var value = $('select[name=FoodMenuId]').val();
        if (value != "0") {
            $("#IngredientId").attr("disabled", "disabled");
            $("#FoodMenuIdForLostAmount").val(value);
            FoodManuLostAmount = $('#FoodMenuIdForLostAmount').children("option:selected").text()
        }
        else {
            $("#IngredientId").removeAttr("disabled");
        }
    }

}


$("#Quantity").blur(function () {
    debugger;
    if (FoodManuLostAmount > 0) {
        var Amount = $("#Quantity").val() * FoodManuLostAmount;
    }
    else {
        var Amount = $("#Quantity").val() * IngredientLostAmount;
    }
    $("#LossAmount").val(Amount);

});

function validation(id) {
    var message = '';
    if ($("#OutletId").val() == '' || $("#OutletId").val() == 0) {
        message = "Select Outlet"
        return message;
    }

    if ($("#EmployeeId").val() == '' || $("#EmployeeId").val() == 0) {
        message = "Select Employee"
        return message;
    }

    if (id == 1) {
        if (!WasteDatatable.data().any() || WasteDatatable.data().row == null) {
            var message = 'At least one order should be entered'
            return message;
        }
        if ($("#ReasonForWaste").val() == '') {
            message = "Enter waste reason"
            return message;
        }
    }

    if (id == 0) {
        if ($("#IngredientId").val() == '' || $("#IngredientId").val() == '0') {
            if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == '0') {
                message = "Select Food Menu OR Ingredient"
                return message;
            }
        }
        if ($("#Quantity").val() == '' || $("#Quantity").val() == 0) {
            message = "Enter quantity"
            return message;
        }

        for (var i = 0; i < dataArr.length; i++) {
            if ($("#IngredientId").val() == dataArr[i].ingredientId && $("#FoodMenuId").val() == dataArr[i].foodMenuId) {
                message = "Item already selected!"
                break;
            }
        }
    }
    return message;
}

function clearItem() {
    $("#IngredientId").val('0'),
        $("#FoodMenuId").val('0'),
        $("#Quantity").val(''),
        $("#LossAmount").val(''),
        $("#WasteId").val('0')
    $("#IngredientId").removeAttr("disabled");
    $("#FoodMenuId").removeAttr("disabled");
    $("#FoodMenuIdForLostAmount").val('0');
    $("#IngredientIdForLostAmount").val('0');
    FoodManuLostAmount = 0;
    IngredientLostAmount = 0;
}
