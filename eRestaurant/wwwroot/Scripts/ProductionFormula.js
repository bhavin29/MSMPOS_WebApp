﻿var editFoodMenuDataArr = [];
var foodMenuDeletedId = [];
var editIngredientDataArr = [];
var ingredientDeletedId = [];
$(document).ready(function () {
    $('#ProductionFormula').validate();
    FormulaFoodMenuTable = $('#formulaFoodMenu').DataTable({
        columnDefs: [
            { targets: [0], orderable: false, visible: false },
            { targets: [1], orderable: false, visible: false },
            { targets: [3], orderable: false, class: "text-right" }
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

    FormulaIngredientTable = $('#formulaIngredient').DataTable({
        columnDefs: [
            { targets: [0], orderable: false, visible: false },
            { targets: [1], orderable: false, visible: false },
            { targets: [3], orderable: false, class: "text-right" },
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
    if ($('#FoodmenuType').val() == 2) {
        $('#onthespot').prop('checked', true);
        if ($('#FormulaName').val() == '') { $('#FormulaName').val('Formula name as menu item'); }
        $('#FormulaName').attr("disabled", "disabled"); 
    }
    else {
        $('#onthespot').prop('checked', false);
    }
    $('#IngredientId').select2();
    $('#FoodMenuId').select2();
    $('#BatchSizeUnitId').select2();
    $('#FoodMenuId').focus();
});


$('#addFoodMenuRow').on('click', function (e) {
    e.preventDefault();
    var message = validation(0);
    var rowId = "rowId" + $("#FoodMenuId").val();
    ExpectedOutput = $("#ExpectedOutput").val();

    if (message == '') {
        FormulaFoodMenuTable.row('.active').remove().draw(false);
        var rowNode = FormulaFoodMenuTable.row.add([
            $('#PFFoodMenuId').val(),
            $('#FoodMenuId').val(),
            $('#FoodMenuId').children("option:selected").text(),
            parseFloat(ExpectedOutput).toFixed(2) + ' ' + $("#FoodMenuUnitName").text() ,
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#FoodMenuId").val() + '" class=" editFoodMenuItem">Edit</a></a> / <a href="#" data-toggle="modal" data-target="#myModal0">Delete</a></div></td > ' +
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteFoodMenuOrder(0, ' + $("#FoodMenuId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
        ]).node().id = rowId;

        FormulaFoodMenuTable.draw(false);

        foodMenuDataArr.push({
            pfFoodMenuId: $("#PFFoodMenuId").val(),
            foodMenuId: $("#FoodMenuId").val(),
            expectedOutput: $("#ExpectedOutput").val(),
            foodMenuUnitName: $("#FoodMenuUnitName").text(),
            foodMenuName: $('#FoodMenuId').children("option:selected").text()
        });

        $(rowNode).find('td').eq(1).addClass('text-right');
        $(rowNode).find('td').eq(2).addClass('text-right');


        if ($('#FoodmenuType').val() == 2) {
            $('#FormulaName').val($('#FoodMenuId').children("option:selected").text());
        }

        clearFoodMenuItem();
        editFoodMenuDataArr = [];
        
     //   $('#FoodMenuId').val(0).trigger('change');
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

    debugger;
    if (!FormulaFoodMenuTable.data().any() || FormulaFoodMenuTable.data().row == null) {
        debugger;

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
                $("#ExpectedOutput").val(foodMenuDataArr[i].expectedOutput);
                $('#FoodMenuId').val(foodMenuDataArr[i].foodMenuId).trigger('change');
                editFoodMenuDataArr = foodMenuDataArr.splice(i, 1);
            }
        }
    }
});

function validation(id) {
    var message = '';
    if (id == 0) {
        if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == '0') {
            message = "Select menu item"
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
        if ($("#BatchSizeUnitId").val() == '' || $("#BatchSizeUnitId").val() == '0') {
            message = "Select Batch Size Unit"
        }
        else if ($("#BatchSize").val() == '' || $("#BatchSize").val() == 0) {
            message = "Enter Batch Size"
        }
        else if (($("#FormulaName").val() == '' || $("#FormulaName").val() == 0) && ($("#FoodmenuType").val() == 3)) {
            message = "Enter Formula Name"
        }
        else if (!FormulaFoodMenuTable.data().any() || FormulaFoodMenuTable.data().row == null) {
            message = 'At least one menu item should be entered'
        }
        else if ($("#BatchSize").val() == '' || $("#BatchSize").val() == 0) {
            message = "Enter Batch Size"
        }
        else if (!FormulaIngredientTable.data().any() || FormulaIngredientTable.data().row == null) {
             message = 'At least one stock item should be entered'
        }
    }

    if (id == 2) {
        if ($("#IngredientId").val() == '' || $("#IngredientId").val() == '0') {
            message = "Select stock item"
        }
        else if ($("#IngredientQty").val() == '' || $("#IngredientQty").val() == 0) {
            message = "Enter stock qty"
        }

        for (var i = 0; i < ingredientDataArr.length; i++) {
            if ($("#IngredientId").val() == ingredientDataArr[i].ingredientId) {
                message = "stock item already selected!"
                break;
            }
        }
    }
    return message;
}

function clearFoodMenuItem() {
    $("#PFFoodMenuId").val('0');    
    $("#ExpectedOutput").val(parseFloat(1.00).toFixed(2));
    $("#FoodMenuUnitName").html('');
    $('#FoodMenuId').val(0).trigger('change');
}

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
            parseFloat(IngredientQty).toFixed(2) + ' ' + $("#IngredientUnitName").text(),
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#IngredientId").val() + '" class=" editIngredientItem">Edit</a> / <a href="#" data-toggle="modal"  data-target="#myModal0">Delete</a></div></td > ' +
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
              //  $("#IngredientId").val(ingredientDataArr[i].ingredientId);
                $("#IngredientQty").val(ingredientDataArr[i].ingredientQty);
                $('#IngredientId').val(ingredientDataArr[i].ingredientId).trigger('change');

                editIngredientDataArr = ingredientDataArr.splice(i, 1);
            }
        }
    }
});

function clearIngredientItem() {
    $("#PFIngredientId").val('0');
    $("#IngredientQty").val(parseFloat(1.00).toFixed(2));
    $("#IngredientUnitName").html('');
    $('#IngredientId').val(0).trigger('change');
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
            //$('#BatchSizeUnitId').val(obj.id).trigger('change');
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
        debugger;
        if (message == '') {
            $("#productionFormulaForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    FoodmenuType: $("#FoodmenuType").val(),
                    FormulaName: $("#FormulaName").val(),
                    BatchSize: $("#BatchSize").val(),
                    BatchSizeUnitId: $("#BatchSizeUnitId").val(),
                    StoreId: $("#StoreId").val(),
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