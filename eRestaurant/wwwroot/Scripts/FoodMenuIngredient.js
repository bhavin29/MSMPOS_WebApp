var FoodMenuIngredient;
var editDataArr = [];
var deletedId = [];
$(document).ready(function () {
    $("#foodMenu").validate();
    FoodMenuIngredientTable = $('#FoodMenuIngredientTable').DataTable({
        columnDefs: [
            { targets: [0], orderable: false, visible: false },
            { targets: [1], orderable: false, visible: false },
            { targets: [2], orderable: false, visible: false },
            { targets: [4], orderable: false, class:"text-right"}
        ],
        "paging": false,
        "bLengthChange": true,
        "bInfo": true,
        "bFilter": true,
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

function GetFoodMenuByCategory() {
    $.ajax({
        url: "/FoodMenuIngredient/GetFoodMenuByCategory?categoryId=" + $("#FoodCategoryId").val(),
        data: {},
        type: "GET",
        dataType: "text",
        success: function (data) {
            $("#FoodMenuId").empty();
            var obj = JSON.parse(data);
            for (var i = 0; i < obj.foodMenuList.length; ++i) {
                $("#FoodMenuId").append('<option value="' + obj.foodMenuList[i].value + '">' + obj.foodMenuList[i].text + '</option>');
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

function GetUnitNameByIngredientId() {
    $.ajax({
        url: "/FoodMenuIngredient/GetUnitNameByIngredientId?ingredientId=" + $("#IngredientId").val(),
        data: {},
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            $("#UnitName").text(obj.unitName);
        },
        error: function (data) {
            alert(data);
        }
    });
}

$('#addRow').on('click', function (e) {
    e.preventDefault();
    var message = validation(0);
    var rowId = "rowId" + $("#IngredientId").val();
    var Consumption = $("#Consumption").val();
    var UnitName = $("#UnitName").text();
    Consumption = parseFloat(Consumption).toFixed(2);
    if (message == '') {
        FoodMenuIngredientTable.row('.active').remove().draw(false);
        var rowNode = FoodMenuIngredientTable.row.add([
            $("#Id").val(),
            $("#IngredientId").val(),
            $("#FoodMenuId").val(),
            $('#IngredientId').children("option:selected").text(),
            Consumption,
            UnitName,
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#IngredientId").val() + '" class="btn btn-link editItem">Edit</a><a href="#" data-itemId="' + $("#IngredientId").val() + '"" ></a><a href="#" data-toggle="modal" class="btn btn-link" data-target="#myModal0">Delete</a></div></td > ' +
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#IngredientId").val() + '" onclick="deleteOrder(0, ' + $("#IngredientId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
        ]).node().id = rowId;
        FoodMenuIngredientTable.draw(false);
        dataArr.push({
            id: $("#Id").val(),
            ingredientId: $("#IngredientId").val(),
            foodMenuId: $("#FoodMenuId").val(),
            Consumption: $("#Consumption").val(),
            UnitName: $("#UnitName").text(),
            ingredientName: $('#IngredientId').children("option:selected").text()
        });
        $(rowNode).find('td').eq(1).addClass('text-right');
        $(rowNode).find('td').eq(2).addClass('text-right');
        clearItem();
        editDataArr = [];
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
    console.log(data);
    return $.ajax({
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        type: 'POST',
        beforeSend: function (xhr) { xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val()); },
        url: 'FoodMenuIngredient',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        var message = validation(1);
        if (message == '') {
            $("#foodMenu").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    FoodMenuId: $("#FoodMenuId").val(),
                    FoodMenuIngredientDetails: dataArr,
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

$('#save').click(function () {
    window.location.href = "Index";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

function deleteOrder(id, ingredientId, rowId) {

    for (var i = 0; i < dataArr.length; i++) {
        if (dataArr[i].ingredientId == ingredientId) {
            deletedId.push(dataArr[i].id);
            dataArr.splice(i, 1);
            FoodMenuIngredientTable.row(rowId).remove().draw(false);
            jQuery.noConflict();
            $("#myModal" + ingredientId).modal('hide');
        }
    }
};

function validation(id) {
    var message = '';
    if (id == 0) {
        if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == '0') {
            message = "Select food menu"
        }
        else if ($("#IngredientId").val() == '' || $("#IngredientId").val() == '0') {
            message = "Select ingredient"
        }
        else if ($("#Consumption").val() == '' || $("#Consumption").val() == 0) {
            message = "Enter Consumption"
        }

        for (var i = 0; i < dataArr.length; i++) {
            if ($("#IngredientId").val() == dataArr[i].ingredientId) {
                message = "Ingredient already selected!"
                break;
            }
        }
    }

    if (id == 1) {
        if (!FoodMenuIngredientTable.data().any() || FoodMenuIngredientTable.data().row == null) {
            var message = 'At least one detail should be entered'
            return message;
        }
    }

    if (id == 2) {
        if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == '0') {
            message = "Select food menu"
        }
    }
    return message;
}

function loadFoodMenuIngredient() {
    var message = validation(2);
    if (message == '') {
        window.location.href = "/FoodMenuIngredient/Index?foodMenuId=" + $("#FoodMenuId").val();
    }
    else {
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
        return false;
    }
    
}

function clearItem() {
    $("#IngredientId").val('0');
    $("#UnitName").text('');
    $("#Consumption").val('');
}


$(document).on('click', 'a.editItem', function (e) {
    if (!FoodMenuIngredientTable.data().any() || FoodMenuIngredientTable.data().row == null) {
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
            if (dataArr[i].ingredientId == id) {
                $("#Id").val(dataArr[i].id);
                $("#IngredientId").val(dataArr[i].ingredientId);
                $("#UnitName").text(dataArr[i].unitName);
                $("#Consumption").val(dataArr[i].consumption);
                $("#FoodMenuId").val(dataArr[i].foodMenuId);
                editDataArr = dataArr.splice(i, 1);
            }
        }
    }
});

$('#cancel').on('click', function (e) {
    e.preventDefault();
    clearItem();
    dataArr.push(editDataArr[0]);
    editDataArr = [];
});