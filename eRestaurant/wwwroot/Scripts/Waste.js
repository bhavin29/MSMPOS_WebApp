﻿var WasteDatatable;

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

$('#addRow').on('click', function (e) {
    e.preventDefault();
    var message = validation();
    debugger;
    if (message == '') {
        WasteDatatable.row('.active').remove().draw(false);
        var rowNode = WasteDatatable.row.add([
            $("#FoodMenuId").val(),
            $('#FoodMenuId').children("option:selected").text(),
            $("#IngredientId").val(),
            $('#IngredientId').children("option:selected").text(),
            '<td class="text-right">' + $("#Quantity").val() + ' </td>',
            '<td class="text-right">' + $("#LossAmount").val() + ' </td>',
            $("#WasteId").val(),
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#IngredientId").val() + '" class="btn btn-link editItem"><i class="fa fa-edit"></i></a><a href="#" class="btn btn-link btn-danger" data-toggle="modal" data-target="#myModal0"><i class="fa fa-times"></i></a></div></td > ' +
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#IngredientId").val() + '" onclick="deleteOrder(0, ' + $("#IngredientId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-default" data-dismiss="modal">Close</button></div></div></div ></div >'
        ]).draw(false);
        dataArr.push({
            foodMenuId: $("#FoodMenuId").val(),
            ingredientId: $("#IngredientId").val(),
            qty: $("#Quantity").val(),
            lossAmount: $("#LossAmount").val(),
            wasteId: $("#WasteId").val(),
            foodMenuName: $('#FoodMenuId').children("option:selected").text(),
            ingredientName: $('#IngredientId').children("option:selected").text()
        });
        $(rowNode).find('td').eq(1).addClass('text-right');
        $(rowNode).find('td').eq(2).addClass('text-right');
        clearItem();
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
        url: 'Waste',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        if (!WasteDatatable.data().any() || WasteDatatable.data().row == null) {
            var message = 'At least one order should be entered'
            $(".modal-body").text(message);
            $("#save").hide();
            jQuery.noConflict();
            $("#aModal").modal('show');
        }
        else {
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
                    WasteDetail: dataArr
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
    window.location.href = "/Waste/WasteList";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

function deleteOrderItem(id) {
    debugger;
    return $.ajax({
        dataType: "json",
        //contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        type: "GET",
        // beforeSend: function (xhr) { xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val()); },
        url: "/Waste/DeleteWasteDetail",
        data: "wasteId=" + id
        //headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
}

function deleteOrder(wasteId, ingredientId, rowId) {
    var id = wasteId;
    if (ingredientId == "0") {
        for (var i = 0; i < dataArr.length; i++) {
            if (dataArr[i].ingredientId == id) {
                TotalAmount = dataArr[i].total;
                GrandTotal -= TotalAmount;
                $("#TotalLossAmount").val(GrandTotal);
                dataArr.splice(i, 1);
                WasteDatatable.row(rowId).remove().draw(false);
                jQuery.noConflict();
                $("#myModal0").modal('hide');
            }
        }
    }
    else {
        $.when(deleteOrderItem(ingredientId).then(function (res) {
            console.log(res)
            if (res.status == 200) {
                debugger;
                for (var i = 0; i < dataArr.length; i++) {
                    if (dataArr[i].ingredientId == id) {
                        TotalAmount = dataArr[i].total;
                        GrandTotal -= TotalAmount;
                        $("#TotalLossAmount").val(GrandTotal);
                        dataArr.splice(i, 1);
                        WasteDatatable.row(rowId).remove().draw(false);
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
    if (!WasteDatatable.data().any() || WasteDatatable.data().row == null) {
        var message = 'No data available!'
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
    }
    else {
        debugger;
  
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
                    $("#FoodMenuId").val(dataArr[i].foodMenuId),
                    $("#Quantity").val(dataArr[i].qty),
                    $("#LossAmount").val(dataArr[i].lossAmount),
                    $("#WasteId").val(dataArr[i].wasteId)
                dataArr.splice(i, 1);
                $(this).remove();
            }
        }
    }
    debugger;

});


function validation() {
    debugger;
    var message = '';

    if ($("#IngredientId").val() == '' || $("#IngredientId").val() == '0') {
        message = "Select ingredient"
    }
    else if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == 0) {
        message = "Select Food Menu"
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
        $("#FoodMenuId").val(''),
        $("#Quantity").val(''),
        $("#LossAmount").val(''),
        $("#WasteId").val('0')
}