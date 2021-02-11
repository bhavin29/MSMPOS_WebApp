var editFoodMenuDataArr = [];
var foodMenuDeletedId = [];
var editIngredientDataArr = [];
var ingredientDeletedId = [];
$(document).ready(function () {
    $("#ProductionFormula").validate();
    FormulaFoodMenuTable = $('#formulaFoodMenu').DataTable({
        columnDefs: [
            { targets: [0], orderable: false, visible: false },
            { targets: [1], orderable: false, visible: false },
            { targets: [3], orderable: false, class: "text-right" }
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

    FormulaIngredientTable = $('#formulaIngredient').DataTable({
        columnDefs: [
            { targets: [0], orderable: false, visible: false },
            { targets: [1], orderable: false, visible: false },
            { targets: [3], orderable: false, class: "text-right" },
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

    if ($("#FoodmenuType").val() == 2) {
        $("#onthespot").prop('checked', true);
    }
    else {
        $("#onthespot").prop('checked', false);
    }
});


$('#addFoodMenuRow').on('click', function (e) {
    e.preventDefault();
    var message = validation(0);
    var rowId = "rowId" + $("#FoodMenuId").val();
    ExpectedOutput = $("#ExpectedOutput").val();
    if (message == '') {
        FormulaFoodMenuTable.row('.active').remove().draw(false);
        var rowNode = FormulaFoodMenuTable.row.add([
            $("#PFFoodMenuId").val(),
            $("#FoodMenuId").val(),
            $('#FoodMenuId').children("option:selected").text(),
            parseFloat(ExpectedOutput).toFixed(2),
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#FoodMenuId").val() + '" class="btn btn-link editFoodMenuItem">Edit</a></a><a href="#" data-toggle="modal" class="btn btn-link" data-target="#myModal0">Delete</a></div></td > ' +
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteFoodMenuOrder(0, ' + $("#FoodMenuId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
        ]).node().id = rowId;
        FormulaFoodMenuTable.draw(false);
        foodMenuDataArr.push({
            pfFoodMenuId: $("#PFFoodMenuId").val(),
            foodMenuId: $("#FoodMenuId").val(),
            expectedOutput: $("#ExpectedOutput").val(),
            foodMenuName: $('#FoodMenuId').children("option:selected").text()
        });
        $(rowNode).find('td').eq(1).addClass('text-right');
        $(rowNode).find('td').eq(2).addClass('text-right');
        clearFoodMenuItem();
        editFoodMenuDataArr = [];
        $("#ExpectedOutput").val(parseFloat(1.00).toFixed(2));
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
            foodMenuDeletedId.push(foodMenuDataArr[i].pfFoodMenuId);
            foodMenuDataArr.splice(i, 1);
            FormulaFoodMenuTable.row(rowId).remove().draw(false);
            jQuery.noConflict();
            $("#myModal" + foodMenuId).modal('hide');
        }
    }
};


$(document).on('click', 'a.editFoodMenuItem', function (e) {
    if (!FormulaFoodMenuTable.data().any() || FormulaFoodMenuTable.data().row == null) {
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
                $("#PFFoodMenuId").val(foodMenuDataArr[i].pfFoodMenuId);
                $("#FoodMenuId").val(foodMenuDataArr[i].foodMenuId);
                $("#ExpectedOutput").val(foodMenuDataArr[i].expectedOutput);
                editFoodMenuDataArr = foodMenuDataArr.splice(i, 1);
            }
        }
    }
});

function validation(id) {
    var message = '';
    if (id == 0) {
        if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == '0') {
            message = "Select food menu"
        }
        else if ($("#BatchSize").val() == '' || $("#BatchSize").val() == 0) {
            message = "Enter Batch Size"
        }
        else if ($("#FormulaName").val() == '' || $("#FormulaName").val() == 0) {
            message = "Enter Formula Name"
        }
        else if ($("#ExpectedOutput").val() == '' || $("#ExpectedOutput").val() == 0) {
            message = "Enter Expected Output"
        }

        if ($("#onthespot").prop("checked") == true) {
            if (foodMenuDataArr.length == 1) {
                message = "Only one food menu allows in on the spot food."
            }
        }

        for (var i = 0; i < foodMenuDataArr.length; i++) {
            if ($("#FoodMenuId").val() == foodMenuDataArr[i].foodMenuId) {
                message = "Food menu already selected!"
                break;
            }
        }
    }

    if (id == 1) {
        if (!FormulaFoodMenuTable.data().any() || FormulaFoodMenuTable.data().row == null) {
            var message = 'At least one menu item should be entered'
            return message;
        }

        if (!FormulaIngredientTable.data().any() || FormulaIngredientTable.data().row == null) {
            var message = 'At least one stock item should be entered'
            return message;
        }
    }

    if (id == 2) {
        if ($("#IngredientId").val() == '' || $("#IngredientId").val() == '0') {
            message = "Select ingredient"
        }
        else if ($("#IngredientQty").val() == '' || $("#IngredientQty").val() == 0) {
            message = "Enter ingredient qty"
        }

        for (var i = 0; i < ingredientDataArr.length; i++) {
            if ($("#IngredientId").val() == ingredientDataArr[i].ingredientId) {
                message = "Ingredient already selected!"
                break;
            }
        }
    }
    return message;
}

function clearFoodMenuItem() {
    $("#FoodMenuId").val('0');
    $("#PFFoodMenuId").val('0');    
    $("#ExpectedOutput").val('');
}


//Ingredient


$('#addIngredientRow').on('click', function (e) {
    e.preventDefault();
    var message = validation(2);
    var rowId = "rowId" + $("#IngredientId").val();
    IngredientQty = $("#IngredientQty").val();
    if (message == '') {
        FormulaIngredientTable.row('.active').remove().draw(false);
        var rowNode = FormulaIngredientTable.row.add([
            $("#PFIngredientId").val(),
            $("#IngredientId").val(),
            $('#IngredientId').children("option:selected").text(),
            parseFloat(IngredientQty).toFixed(2),
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#IngredientId").val() + '" class="btn btn-link editIngredientItem">Edit</a><a href="#" data-toggle="modal" class="btn btn-link" data-target="#myModal0">Delete</a></div></td > ' +
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#IngredientId").val() + '" onclick="deleteIngredientOrder(0, ' + $("#IngredientId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
        ]).node().id = rowId;
        FormulaIngredientTable.draw(false);
        ingredientDataArr.push({
            pfIngredientId: $("#PFIngredientId").val(),
            ingredientId: $("#IngredientId").val(),
            ingredientQty: $("#IngredientQty").val(),
            ingredientName: $('#IngredientId').children("option:selected").text()
        });
        $(rowNode).find('td').eq(1).addClass('text-right');
        $(rowNode).find('td').eq(2).addClass('text-right');
        clearIngredientItem();
        editIngredientDataArr = [];
        $("#IngredientQty").val(parseFloat(1.00).toFixed(2));
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

function deleteIngredientOrder(id, ingredientId, rowId) {

    for (var i = 0; i < ingredientDataArr.length; i++) {
        if (ingredientDataArr[i].ingredientId == ingredientId) {
            ingredientDeletedId.push(ingredientDataArr[i].pfIngredientId);
            ingredientDataArr.splice(i, 1);
            FormulaIngredientTable.row(rowId).remove().draw(false);
            jQuery.noConflict();
            $("#myModal" + foodMenuId).modal('hide');
        }
    }
};


$(document).on('click', 'a.editIngredientItem', function (e) {
    if (!FormulaIngredientTable.data().any() || FormulaIngredientTable.data().row == null) {
        var message = 'No data available!'
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
    }
    else {
        e.preventDefault();
        if (editIngredientDataArr.length > 0) {
            ingredientDataArr.push(editIngredientDataArr[0]);
        }
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
        }
        else {
            $(this).parents('tr').removeClass('active');
            $(this).parents('tr').addClass('active');
        }
        var id = $(this).attr('data-itemId');
        for (var i = 0; i < ingredientDataArr.length; i++) {
            if (ingredientDataArr[i].ingredientId == id) {
                $("#PFIngredientId").val(ingredientDataArr[i].pfIngredientId);
                $("#IngredientId").val(ingredientDataArr[i].ingredientId);
                $("#IngredientQty").val(ingredientDataArr[i].ingredientQty);
                editIngredientDataArr = ingredientDataArr.splice(i, 1);
            }
        }
    }
});

function clearIngredientItem() {
    $("#IngredientId").val('0');
    $("#PFIngredientId").val('0');
    $("#IngredientQty").val('');
}

function GetUnitNameByIngredientId() {
    $.ajax({
        url: "/FoodMenuIngredient/GetUnitNameByIngredientId?ingredientId=" + $("#IngredientId").val(),
        data: {},
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            $("#IngredientUnitName").text(obj.unitName);
        },
        error: function (data) {
            alert(data);
        }
    });
}

function GetUnitNameByFoodMenuId() {
    $.ajax({
        url: "/ProductionFormula/GetUnitNameByFoodMenuId?foodMenuId=" + $("#FoodMenuId").val(),
        data: {},
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            $("#FoodMenuUnitName").text(obj.unitName);
            $("#BatchSizeUnitName").text(obj.unitName);
            $("#BatchSizeUnitId").val(obj.id);
        },
        error: function (data) {
            alert(data);
        }
    });
}



function saveOrder(data) {
    console.log(data);
    return $.ajax({
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        type: 'POST',
        beforeSend: function (xhr) { xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val()); },
        url: 'ProductionFormula',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        var message = validation(1);
        if (message == '') {
            $("#productionFormulaForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    FoodmenuType: $("#FoodmenuType").val(),
                    FormulaName: $("#FormulaName").val(),
                    BatchSize: $("#BatchSize").val(),
                    BatchSizeUnitId: $("#BatchSizeUnitId").val(),
                    ExpectedOutput: $("#ExpectedOutput").val(),
                    IsActive: $("#IsActive").is(":checked"),
                    ProductionFormulaFoodMenuModels: foodMenuDataArr,
                    ProductionFormulaIngredientModels: ingredientDataArr,
                    FoodMenuDeletedId: foodMenuDeletedId,
                    IngredientDeletedId: ingredientDeletedId
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
    window.location.href = "/ProductionFormula/Index";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});