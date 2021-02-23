var editAssetEventFoodmenuDataArr = [];
var assetEventfoodMenuDeletedId = [];
var editAssetEventItemDataArr = [];
var assetEventItemDeletedId = [];
var editAssetEventIngredientDataArr = [];
var assetEventIngredientDeletedId = [];
var GrossAmount = 0;
var TotalFoodVatAmount = 0;
var TotalFoodTaxAmount = 0;
var NetAmountTotal = 0;
var AssetItemNetAmountTotal = 0;
var IngredientNetAmountTotal = 0;
$(document).ready(function () {
    $("#assetEventForm").validate();

    AssetEventItem = $('#AssetEventItem').DataTable({
        "columnDefs": [
            { targets: [0, 1], visible: false },
            { targets: [3, 4, 5], class: "text-right" },
        ],
        "paging": false,
        "bLengthChange": true,
        "bInfo": false,
        "bFilter": false,
        "ordering": false,
        "autoWidth": false,
        "orderCellsTop": false,
        "stateSave": false,
        "pageLength": 200,
        "lengthMenu": [
            [200, 500, 1000],
            ['200', '500', '1000']
        ],
    });

    AssetEventFoodmenu = $('#AssetEventFoodmenu').DataTable({
        "columnDefs": [
            { targets: [0, 1], visible: false },
            { targets: [3], class: "text-right" },
        ],
        "paging": false,
        "bLengthChange": true,
        "bInfo": false,
        "bFilter": false,
        "ordering": false,
        "autoWidth": false,
        "orderCellsTop": false,
        "stateSave": false,
        "pageLength": 200,
        "lengthMenu": [
            [200, 500, 1000],
            ['200', '500', '1000']
        ],
    });

    AssetIngredientItem = $('#AssetIngredientItem').DataTable({
        columnDefs: [
            { targets: [1, 2], visible: false },
            { targets: [3, 4, 5], class: "text-right" },
        ],
        "paging": false,
        "bLengthChange": true,
        "bInfo": false,
        "bFilter": false,
        "ordering": false,
        "autoWidth": false,
        "orderCellsTop": false,
        "stateSave": false,
        "pageLength": 200,
        "lengthMenu": [
            [200, 500, 1000],
            ['200', '500', '1000']
        ],
    });

    if ($("#Status").val() == 0) {
        AssetEventItem.columns([7, 8, 9, 10]).visible(false);
    }
    if ($("#Status").val() == 1) {
        AssetEventItem.columns([8, 9, 10]).visible(false);
    }

    if ($("#Status").val() == 0) {
        AssetIngredientItem.columns([7, 8]).visible(false);
    }
    if ($("#Status").val() == 1) {
        AssetIngredientItem.columns([7, 8]).visible(false);
    }
});

//AssetEventItem
$('#addRowItem').on('click', function (e) {
    e.preventDefault();
    var message = validation(3);
    var rowId = $("#AssetItemId").val();
    // var rowId = "rowId" + $("#AssetItemId").val();
    var AssetItemCostPrice;
    var AssetItemTotalPrice = 0;

    var missingNotes = '';
    $.ajax({
        url: "/AssetEvent/GetCateringFoodMenuGlobalStatus",
        data: {},
        type: "GET",
        async: false,
        dataType: "text",
        success: function (data) {
            missingNotes = '<select id="MissingNote' + rowId + '" class="form-control">';
            var obj = JSON.parse(data);
            for (var i = 0; i < obj.foodMenuGlobalStatus.length; ++i) {
                missingNotes += '<option value="' + obj.foodMenuGlobalStatus[i].value + '">' + obj.foodMenuGlobalStatus[i].text + '</option>';
            }
        },
        error: function (data) {
            alert(data);
        }
    });

    AssetItemCostPrice = parseFloat($("#AssetItemCostPrice").val()).toFixed(2);

    var EventQty = parseFloat($("#EventQty").val()).toFixed(2);
    var StockQty = parseFloat($("#StockQty").val()).toFixed(2);
    var AllocatedQty = parseFloat($("#AllocatedQty").val()).toFixed(2);
    var ReturnQty = parseFloat($("#ReturnQty").val()).toFixed(2);
    var MissingQty = parseFloat((parseFloat(AllocatedQty) - parseFloat(ReturnQty))).toFixed(2);

    AssetItemTotalPrice = parseFloat(EventQty * AssetItemCostPrice).toFixed(2);

    if (message == '') {
        AssetEventItem.row('.active').remove().draw(false);

        var rowNode = AssetEventItem.row.add([
            '<td>' + $("#AssetEventItemId").val() + ' </td>',
            '<td>' + $("#AssetItemId").val() + ' </td>',
            $('#AssetItemId').children("option:selected").text(),
            '<td class="text-right">' + StockQty + '</td>',
            '<td class="text-right"><label>' + EventQty + '</label> <label>' + $("#AssetItemUnitName").text() + '</label></td>',
            '<td class="text-right">' + AssetItemCostPrice + ' </td>',
            '<td class="text-right">' + AssetItemTotalPrice + ' </td>',
            '<td class="text-right"><input type="number" id="allocatedQty' + $("#AssetItemId").val() + '" class="form-control col-sm-11 text-right" min="0" max="99999" value="' + AllocatedQty + '" name="item.AllocatedQty"> </td>',
            '<td class="text-right"><input type="number" id="returnQty' + $("#AssetItemId").val() + '" class="form-control col-sm-11 text-right" min="0" max="99999" value="' + ReturnQty + '" name="item.ReturnQty"> </td>',
            '<td class="text-right"><input type="number" id="missingQty' + $("#AssetItemId").val() + '" class="form-control col-sm-11 text-right" min="0" max="99999" value="' + MissingQty + '" name="item.MissingQty"> </td>',
            '<td class="text-right">' + missingNotes + ' </td>',
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#AssetItemId").val() + '" class=" editAssetItem">Edit</a></a> / <a href="#" data-toggle="modal" data-target="#myAModal0">Delete</a></div></td > ' +
            '<div class="modal fade" id=myAModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#AssetItemId").val() + '" onclick="deleteAssetOrder(0, ' + $("#AssetItemId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
        ]).draw(false).nodes();
        assetEventItemDataArr.push({
            assetItemId: $("#AssetItemId").val(),
            stockQty: StockQty,
            eventQty: EventQty,
            allocatedQty: AllocatedQty,
            returnQty: ReturnQty,
            missingQty: MissingQty,
            costPrice: AssetItemCostPrice,
            totalAmount: AssetItemTotalPrice,
            missingNote: '',
            assetEventItemId: $("#AssetEventItemId").val()
        });
        $(rowNode).find('td').eq(2).addClass('text-right');
        $(rowNode).find('td').eq(3).addClass('text-right');
        $(rowNode).find('td').eq(4).addClass('text-right');
        $(rowNode).find('td').eq(5).addClass('text-right');

        AssetItemNetAmountTotal = calculateAssetItemColumn(6);
        $("#AssetItemNetAmount").val(parseFloat(AssetItemNetAmountTotal).toFixed(2));

        clearAssetItem();
        editAssetEventItemDataArr = [];
        $("#AssetItemId").focus()
    }
    else if (message != '') {
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
        return false;
    }
});

function clearAssetItem() {
    $("#AssetItemId").val('0');
    $("#AssetItemCostPrice").val('');
    $("#EventQty").val('1');
}

function deleteAssetOrder(id, assetItemId, rowId) {

    for (var i = 0; i < assetEventItemDataArr.length; i++) {
        if (assetEventItemDataArr[i].assetItemId == assetItemId) {
            assetEventItemDeletedId.push(assetEventItemDataArr[i].assetEventItemId);
            assetEventItemDataArr.splice(i, 1);
            AssetEventItem.row(rowId).remove().draw(false);
            jQuery.noConflict();
            $("#myAModal" + assetItemId).modal('hide');
        }
    }
    $("#AssetItemNetAmount").val(parseFloat(calculateAssetItemColumn(6)).toFixed(2));
};



$(document).on('click', 'a.editAssetItem', function (e) {
    if (!AssetEventItem.data().any() || AssetEventItem.data().row == null) {
        var message = 'No data available!'
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
    }
    else {
        e.preventDefault();
        if (editAssetEventItemDataArr.length > 0) {
            assetEventItemDataArr.push(editAssetEventItemDataArr[0]);
        }
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
        }
        else {
            $(this).parents('tr').removeClass('active');
            $(this).parents('tr').addClass('active');
        }
        var id = $(this).attr('data-itemId');
        for (var i = 0; i < assetEventItemDataArr.length; i++) {
            if (assetEventItemDataArr[i].assetItemId == id) {
                $("#AssetEventItemId").val(assetEventItemDataArr[i].assetEventItemId);
                $("#AssetItemId").val(assetEventItemDataArr[i].assetItemId);
                $("#StockQty").val(assetEventItemDataArr[i].stockQty);
                $("#EventQty").val(assetEventItemDataArr[i].eventQty);
                $("#AllocatedQty").val(assetEventItemDataArr[i].allocatedQty);
                $("#ReturnQty").val(assetEventItemDataArr[i].returnQty);
                $("#MissingQty").val(assetEventItemDataArr[i].missingQty);
                $("#AssetItemCostPrice").val(assetEventItemDataArr[i].costPrice);
                //$("#MissingNote").val(assetEventItemDataArr[i].missingNote);
                editAssetEventItemDataArr = assetEventItemDataArr.splice(i, 1);
            }
        }
    }
});

function GetAssetItemPriceById() {
    GetUnitNameByAssetItemId();
    $.ajax({
        url: "/AssetEvent/GetAssetItemPriceById",
        data: { "id": $("#AssetItemId").val() },
        async: false,
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            AssetItemCostPrice = obj.assetItemCostPrice;
            AssetItemCostPrice = parseFloat(AssetItemCostPrice).toFixed(2);
        }
    });
    $("#AssetItemCostPrice").val(AssetItemCostPrice);
}


//FoodMenu
$('#addRowFood').on('click', function (e) {
    e.preventDefault();
    var message = validation(1);
    var rowId = "rowId" + $("#FoodMenuId").val();
    var Price;
    var TaxPercentage;
    var foodVatAmount = 0;
    var foodTaxAmount = 0;
    var TotalPrice = 0;

    $.ajax({
        url: "/AssetEvent/GetFoodMenuPriceTaxDetailById",
        data: { "id": $("#FoodMenuId").val() },
        async: false,
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            Price = obj.salesPrice;
            TaxPercentage = obj.taxPercentage;
            Price = parseFloat(Price).toFixed(2);
        }
    });

    Price = $("#SalesPrice").val();
    var Qty = $("#Quantity").val();
    Qty = parseFloat(Qty).toFixed(2);
    TotalPrice = parseFloat(Qty * Price).toFixed(2);

    if (parseFloat(TaxPercentage).toFixed(2) > 0) {
        foodTaxAmount = (TotalPrice * TaxPercentage) / 100;
    }
    else {
        foodVatAmount = TotalPrice;
    }

    foodTaxAmount = parseFloat(foodTaxAmount).toFixed(2);
    foodVatAmount = parseFloat(foodVatAmount).toFixed(2);


    if (message == '') {
        AssetEventFoodmenu.row('.active').remove().draw(false);
        var rowNode = AssetEventFoodmenu.row.add([
            $("#AssetEventFoodmenuId").val(),
            '<td class="text-right">' + $("#FoodMenuId").val() + ' </td>',
            $('#FoodMenuId').children("option:selected").text(),
            '<td class="text-right">' + Price + ' </td>',
            '<td class="text-right">' + Qty + ' ' + $("#FoodMenuUnitName").text() + ' </td>',
            '<td class="text-right">' + foodVatAmount + ' </td>',
            '<td class="text-right">' + foodTaxAmount + ' </td>',
            '<td class="text-right">' + TotalPrice + ' </td>',
            '<td class="text-right"><div class="form-button-action"><a href="#" data-itemId="' + $("#FoodMenuId").val() + '" class=" editFoodMenuItem">Edit</a></a> / <a href="#" data-toggle="modal" data-target="#myModal0">Delete</a></div></td > ' +
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteFoodMenuOrder(0, ' + $("#FoodMenuId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
        ]).draw(false).nodes();
        assetEventFoodmenuDataArr.push({
            foodMenuId: $("#FoodMenuId").val(),
            qunatity: Qty,
            salesPrice: Price,
            foodVatAmount: foodVatAmount,
            foodTaxAmount: foodTaxAmount,
            totalPrice: TotalPrice,
            assetEventFoodmenuId: $("#AssetEventFoodmenuId").val()
        });
        $(rowNode).find('td').eq(2).addClass('text-right');
        $(rowNode).find('td').eq(3).addClass('text-right');
        $(rowNode).find('td').eq(4).addClass('text-right');
        $(rowNode).find('td').eq(5).addClass('text-right');
        $(rowNode).find('td').eq(6).addClass('text-right');


        TotalFoodVatAmount = calculateColumn(3);
        TotalFoodTaxAmount = calculateColumn(4);
        GrossAmount = calculateColumn(5);

        $("#FoodVatAmount").val(parseFloat(TotalFoodVatAmount).toFixed(2));
        $("#FoodTaxAmount").val(parseFloat(TotalFoodTaxAmount).toFixed(2));
        $("#FoodGrossAmount").val(parseFloat(GrossAmount).toFixed(2));
        $("#FoodNetAmount").val(parseFloat(GrossAmount + TotalFoodTaxAmount).toFixed(2));
        clearFoodMenuItem();
        editAssetEventFoodmenuDataArr = [];
        $("#FoodMenuId").focus()
    }
    else if (message != '') {
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
        return false;
    }
});

function clearFoodMenuItem() {
    $("#FoodMenuId").val('0');
    $("#SalesPrice").val('0');
    $("#Quantity").val('1');
}

function deleteFoodMenuOrder(id, foodMenuId, rowId) {

    for (var i = 0; i < assetEventFoodmenuDataArr.length; i++) {
        if (assetEventFoodmenuDataArr[i].foodMenuId == foodMenuId) {
            assetEventfoodMenuDeletedId.push(assetEventFoodmenuDataArr[i].assetEventFoodmenuId);
            assetEventFoodmenuDataArr.splice(i, 1);
            AssetEventFoodmenu.row(rowId).remove().draw(false);
            jQuery.noConflict();
            $("#myModal" + foodMenuId).modal('hide');
        }
    }

    var totalGrossAmount = calculateColumn(5);
    var totalFoodVatAmount = calculateColumn(3);
    var totalFoodTaxAmount = calculateColumn(4);
    $("#FoodGrossAmount").val(parseFloat(totalGrossAmount).toFixed(2));
    $("#FoodNetAmount").val(parseFloat(totalGrossAmount + totalFoodTaxAmount).toFixed(2));
    $("#FoodVatAmount").val(parseFloat(totalFoodVatAmount).toFixed(2));
    $("#FoodTaxAmount").val(parseFloat(totalFoodTaxAmount).toFixed(2));
};


$(document).on('click', 'a.editFoodMenuItem', function (e) {
    if (!AssetEventFoodmenu.data().any() || AssetEventFoodmenu.data().row == null) {
        var message = 'No data available!'
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
    }
    else {
        e.preventDefault();
        if (editAssetEventFoodmenuDataArr.length > 0) {
            assetEventFoodmenuDataArr.push(editAssetEventFoodmenuDataArr[0]);
        }
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
        }
        else {
            $(this).parents('tr').removeClass('active');
            $(this).parents('tr').addClass('active');
        }
        var id = $(this).attr('data-itemId');
        for (var i = 0; i < assetEventFoodmenuDataArr.length; i++) {
            if (assetEventFoodmenuDataArr[i].foodMenuId == id) {
                $("#AssetEventFoodmenuId").val(assetEventFoodmenuDataArr[i].assetEventFoodmenuId);
                $("#FoodMenuId").val(assetEventFoodmenuDataArr[i].foodMenuId);
                $("#Quantity").val(assetEventFoodmenuDataArr[i].qunatity);
                $("#SalesPrice").val(assetEventFoodmenuDataArr[i].salesPrice);
                editAssetEventFoodmenuDataArr = assetEventFoodmenuDataArr.splice(i, 1);
            }
        }
    }
});

function GetFoodMenuPriceTaxDetailById() {
    GetUnitNameByFoodMenuId();
    var Price;
    var TaxPercentage;
    $.ajax({
        url: "/AssetEvent/GetFoodMenuPriceTaxDetailById",
        data: { "id": $("#FoodMenuId").val() },
        async: false,
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            Price = obj.salesPrice;
            TaxPercentage = obj.taxPercentage;
            Price = parseFloat(Price).toFixed(2);
        }
    });
    $("#SalesPrice").val(Price);
}


//Ingredient 


$('#addRowIngredient').on('click', function (e) {
    e.preventDefault();
    var message = validation(4);
    var rowId = "rowId" + $("#IngredientId").val();
    var chkId = $("#AssetEventIngredientId").val();

    var IngredientCostPrice;
    var IngredientTotalPrice = 0;

    IngredientCostPrice = parseFloat($("#AssetIngredientCostPrice").val()).toFixed(2);

    var IngredientEventQty = parseFloat($("#IngredientEventQty").val()).toFixed(2);
    var IngredientStockQty = parseFloat($("#IngredientStockQty").val()).toFixed(2);
    var IngredientReturnQty = parseFloat($("#IngredientReturnQty").val()).toFixed(2);
    var IngredientActualQty = parseFloat($("#IngredientActualQty").val()).toFixed(2);

    IngredientTotalPrice = parseFloat(IngredientEventQty * IngredientCostPrice).toFixed(2);

    if (message == '') {
        AssetIngredientItem.row('.active').remove().draw(false);

        var rowNode = AssetIngredientItem.row.add([
            '<td><input type="checkbox" id="' + chkId + '" /></td>',
            '<td>' + $("#AssetEventIngredientId").val() + '</td>',
            '<td>' + $("#IngredientId").val() + ' </td>',
            $('#IngredientId').children("option:selected").text(),
            '<td class="text-right">' + IngredientStockQty + '</td>',
            '<td class="text-right"><label>' + IngredientEventQty + '</label> <label>' + $("#IngredientUnitName").text() + '</label>  </td>',
            '<td class="text-right">' + IngredientCostPrice + ' </td>',
            '<td class="text-right">' + IngredientTotalPrice + '</td>',
            '<td class="text-right"><input type="number" id="IngredientRQty' + $("#IngredientId").val() + '" class="form-control col-sm-11 text-right" min="0" max="99999" value="' + IngredientReturnQty + '" name="item.ReturnQty"> </td>',
            '<td class="text-right"><input type="number" id="IngredientAQty' + $("#IngredientId").val() + '" class="form-control col-sm-11 text-right" min="0" max="99999" value="' + IngredientActualQty + '" name="item.ActualQty"> </td>',
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#IngredientId").val() + '" class="editIngredientItem">Edit</a> / <a href="#" data-toggle="modal" data-target="#myBModal0">Delete</a></div></td > ' +
            '<div class="modal fade" id=myBModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#IngredientId").val() + '" onclick="deleteIngredientOrder(0, ' + $("#IngredientId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
        ]).draw(false).nodes();

        assetEventIngredientDataArr.push({
            ingredientId: $("#IngredientId").val(),
            stockQty: IngredientStockQty,
            eventQty: IngredientEventQty,
            returnQty: IngredientReturnQty,
            actualQty: IngredientActualQty,
            costPrice: IngredientCostPrice,
            totalAmount: IngredientTotalPrice,
            assetEventIngredientId: $("#AssetEventIngredientId").val()
        });

        //$(rowNode).find('td').eq(1).addClass('text-right');
        //$(rowNode).find('td').eq(2).addClass('text-right');
        //$(rowNode).find('td').eq(3).addClass('text-right');
        $(rowNode).find('td').eq(4).addClass('text-right');
        $(rowNode).find('td').eq(5).addClass('text-right');
        $(rowNode).find('td').eq(6).addClass('text-right');

        IngredientNetAmountTotal = calculateIngredientColumn(6);
        $("#IngredientNetAmount").val(parseFloat(IngredientNetAmountTotal).toFixed(2));

        clearIngredientItem();
        editAssetEventIngredientDataArr = [];
        $("#IngredientId").focus()
    }
    else if (message != '') {
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
        return false;
    }
});

function clearIngredientItem() {
    $("#IngredientId").val('0');
    $("#AssetIngredientCostPrice").val('0');
    $("#IngredientEventQty").val('1');
}

function deleteIngredientOrder(id, ingredientId, rowId) {

    for (var i = 0; i < assetEventIngredientDataArr.length; i++) {
        if (assetEventIngredientDataArr[i].ingredientId == ingredientId) {
            assetEventIngredientDeletedId.push(assetEventIngredientDataArr[i].assetEventIngredientId);
            assetEventIngredientDataArr.splice(i, 1);
            AssetIngredientItem.row(rowId).remove().draw(false);
            jQuery.noConflict();
            $("#myBModal" + ingredientId).modal('hide');
        }
    }
    $("#IngredientNetAmount").val(parseFloat(calculateIngredientColumn(6)).toFixed(2));
};



$(document).on('click', 'a.editIngredientItem', function (e) {
    if (!AssetIngredientItem.data().any() || AssetIngredientItem.data().row == null) {
        var message = 'No data available!'
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
    }
    else {


        e.preventDefault();
        if (editAssetEventIngredientDataArr.length > 0) {
            assetEventIngredientDataArr.push(editAssetEventIngredientDataArr[0]);
        }
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
        }
        else {
            $(this).parents('tr').removeClass('active');
            $(this).parents('tr').addClass('active');
        }
        var id = $(this).attr('data-itemId');
        debugger;
        for (var i = 0; i < assetEventIngredientDataArr.length; i++) {
            if (assetEventIngredientDataArr[i].ingredientId == id) {
                $("#AssetEventIngredientId").val(assetEventIngredientDataArr[i].assetEventIngredientId);
                $("#IngredientId").val(assetEventIngredientDataArr[i].ingredientId);
                $("#IngredientStockQty").val(assetEventIngredientDataArr[i].stockQty);
                $("#IngredientEventQty").val(assetEventIngredientDataArr[i].eventQty);
                $("#IngredientReturnQty").val(assetEventIngredientDataArr[i].returnQty);
                $("#IngredientActualQty").val(assetEventIngredientDataArr[i].actualQty);
                $("#AssetIngredientCostPrice").val(assetEventIngredientDataArr[i].costPrice);
                editAssetEventIngredientDataArr = assetEventIngredientDataArr.splice(i, 1);
            }
        }
    }
});

function GetIngredientPriceById() {
    GetUnitNameByIngredientId();
    var IngredientCostPrice;
    $.ajax({
        url: "/AssetEvent/GetIngredientPriceById",
        data: { "id": $("#IngredientId").val() },
        async: false,
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            IngredientCostPrice = obj.ingredientPrice;
            IngredientCostPrice = parseFloat(IngredientCostPrice).toFixed(2);
        }
    });
    $("#AssetIngredientCostPrice").val(IngredientCostPrice);
}

//////Save Order

function saveOrder(data) {
    console.log(data);
    return $.ajax({
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        type: 'POST',
        beforeSend: function (xhr) { xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val()); },
        url: 'AssetEvent',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

function GetAssetItemArray() {
    assetEventItemDataArr = [];
    $("#AssetEventItem tbody tr").each(function () {
        var tds = $(this).find("td");
        var currentRow = $(this).closest("tr");
        //var dataline = $("#AssetEventItem").DataTable().row(currentRow).data();

        if (tds.length > 1) {
            if ($("#Status").val() == 0) {
                assetEventItemDataArr.push({
                    assetEventItemId: $(currentRow).find("td:eq(0) input[type='hidden']").val(),
                    assetItemId: $(currentRow).find("td:eq(1) input[type='hidden']").val(),
                    stockQty: tds[3].textContent,
                    eventQty: tds[4].children[0].textContent,
                    costPrice: tds[5].textContent,
                    totalAmount: tds[6].innerHTML,
                    allocatedQty: tds[4].children[0].textContent,
                    returnQty: 0.0,
                    missingQty: 0.0,
                    missingNote: ''
                });
            } else if ($("#Status").val() == 1) {
                assetEventItemDataArr.push({
                    assetEventItemId: $(currentRow).find("td:eq(0) input[type='hidden']").val(),
                    assetItemId: $(currentRow).find("td:eq(1) input[type='hidden']").val(),
                    stockQty: tds[3].textContent,
                    eventQty: tds[4].children[0].textContent,
                    costPrice: tds[5].textContent,
                    totalAmount: tds[6].innerHTML,
                    allocatedQty: $(currentRow).find("td:eq(7) input[type='number']").val(),
                    returnQty: $(currentRow).find("td:eq(7) input[type='number']").val(),
                    missingQty: 0.0,
                    missingNote: ''
                });
            }
            else {
                assetEventItemDataArr.push({
                    assetEventItemId: $(currentRow).find("td:eq(0) input[type='hidden']").val(),
                    assetItemId: $(currentRow).find("td:eq(1) input[type='hidden']").val(),
                    stockQty: tds[3].textContent,
                    eventQty: tds[4].children[0].textContent,
                    costPrice: tds[5].textContent,
                    totalAmount: tds[6].innerHTML,
                    allocatedQty: $(currentRow).find("td:eq(7) input[type='number']").val(),
                    returnQty: $(currentRow).find("td:eq(8) input[type='number']").val(),
                    missingQty: tds[9].textContent,
                    missingNote: $(currentRow).find("td:eq(10) select[id='item_MissingNote']").val()
                });
            }
        }
    });
}

function GetAssetIngredientArray() {
    assetEventIngredientDataArr = [];
    $("#AssetIngredientItem tbody tr").each(function () {
        var tds = $(this).find("td");
        var currentRow = $(this).closest("tr");
        //var dataline = $("#AssetIngredientItem").DataTable().row(currentRow).data();

        if (tds.length > 1) {
            if ($("#Status").val() == 0) {
                assetEventIngredientDataArr.push({
                    assetEventIngredientId: $(currentRow).find("td:eq(0) input[type='hidden']").val(),
                    ingredientId: $(currentRow).find("td:eq(1) input[type='hidden']").val(),
                    stockQty: tds[3].textContent,
                    eventQty: tds[4].children[0].textContent,
                    costPrice: tds[5].textContent,
                    totalAmount: tds[6].innerHTML,
                    returnQty: tds[4].children[0].textContent,
                    actualQty: 0.0
                });
            } else if ($("#Status").val() == 1) {
                assetEventIngredientDataArr.push({
                    assetEventIngredientId: $(currentRow).find("td:eq(0) input[type='hidden']").val(),
                    ingredientId: $(currentRow).find("td:eq(1) input[type='hidden']").val(),
                    stockQty: tds[3].textContent,
                    eventQty: tds[4].children[0].textContent,
                    costPrice: tds[5].textContent,
                    totalAmount: tds[6].innerHTML,
                    returnQty: tds[4].children[0].textContent,
                    actualQty: 0.0
                });
            }
            else {
                assetEventIngredientDataArr.push({
                    assetEventIngredientId: $(currentRow).find("td:eq(0) input[type='hidden']").val(),
                    ingredientId: $(currentRow).find("td:eq(1) input[type='hidden']").val(),
                    stockQty: tds[3].textContent,
                    eventQty: tds[4].children[0].textContent,
                    costPrice: tds[5].textContent,
                    totalAmount: tds[6].innerHTML,
                    returnQty: $(currentRow).find("td:eq(7) input[type='number']").val(),
                    actualQty: $(currentRow).find("td:eq(8) input[type='number']").val()
                });
            }
        }
    });
}

$(function () {
    $('#saveOrder').click(function () {
        var message = validation(2);

        var status = $("#Status").val();
        if (status == 0) { status = 1; }

        //     GetAssetItemArray();
        //     GetAssetIngredientArray();

        if (message == '') {
            $("#assetEventForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    EventType: 1,
                    EventName: $("#EventName").val(),
                    EventDatetime: $("#EventDatetime").val(),
                    EventPlace: $("#EventPlace").val(),
                    ContactPersonName: $("#ContactPersonName").val(),
                    ContactPersonNumber: $("#ContactPersonNumber").val(),
                    FoodGrossAmount: $("#FoodGrossAmount").val(),
                    FoodDiscountAmount: $("#FoodDiscountAmount").val(),
                    FoodNetAmount: $("#FoodNetAmount").val(),
                    FoodVatAmount: $("#FoodVatAmount").val(),
                    FoodTaxAmount: $("#FoodTaxAmount").val(),
                    MissingTotalAmount: $("#MissingTotalAmount").val(),
                    IngredientNetAmount: $("#IngredientNetAmount").val(),
                    AssetItemNetAmount: $("#AssetItemNetAmount").val(),
                    Status: status,
                    assetEventItemModels: assetEventItemDataArr,
                    assetEventFoodmenuModels: assetEventFoodmenuDataArr,
                    assetEventIngredientModels: assetEventIngredientDataArr,
                    AssetEventItemDeletedId: assetEventItemDeletedId,
                    AssetEventFoodmenuDeletedId: assetEventfoodMenuDeletedId,
                    AssetEventIngredientDeletedId: assetEventIngredientDeletedId
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
    window.location.href = "/AssetEvent/Index";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

function validation(id) {
    var message = '';
    if ($("#EventName").val() == '') {
        message = "Enter Catering Name"
        return message;
    }

    if (id == 1) {
        if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == '0') {
            message = "Select Food Menu"
            return message;
        }
        else if ($("#Quantity").val() == '' || $("#Quantity").val() == 0) {
            message = "Enter Quantity"
            return message;
        }
        else if ($("#SalesPrice").val() == '' || $("#SalesPrice").val() == 0) {
            message = "Enter Sales Price"
            return message;
        }

        for (var i = 0; i < assetEventFoodmenuDataArr.length; i++) {
            if ($("#FoodMenuId").val() == assetEventFoodmenuDataArr[i].foodMenuId) {
                message = "FoodMenu already selected!"
                break;
            }
        }
    }

    if (id == 2) {
        var entry = 0;
        if (AssetEventItem.data().any()) {
            entry = 1;
        }

        if (AssetEventFoodmenu.data().any()) {
            entry = 1;
        }

        if (AssetIngredientItem.data().any()) {
            entry = 1;
        }

        if (entry == 0) {
            var message = 'At least one entry from Asset Detail / Foodmenu Detail / Stock Detail should be entered'
            return message;
        }

    }

    if (id == 3) {
        if ($("#AssetItemId").val() == '' || $("#AssetItemId").val() == '0') {
            message = "Select Asset Item"
            return message;
        }
        else if ($("#EventQty").val() == '' || $("#EventQty").val() == 0) {
            message = "Enter Event Quantity"
            return message;
        }
        else if ($("#AssetItemCostPrice").val() == '' || $("#AssetItemCostPrice").val() == 0) {
            message = "Enter Cost Price"
            return message;
        }

        for (var i = 0; i < assetEventItemDataArr.length; i++) {
            if ($("#AssetItemId").val() == assetEventItemDataArr[i].assetItemId) {
                message = "AssetItem already selected!"
                break;
            }
        }
    }

    if (id == 4) {
        if ($("#IngredientId").val() == '' || $("#IngredientId").val() == '0') {
            message = "Select Ingredient"
            return message;
        }
        else if ($("#IngredientEventQty").val() == '' || $("#IngredientEventQty").val() == 0) {
            message = "Enter Ingredient Quantity"
            return message;
        }
        else if ($("#AssetIngredientCostPrice").val() == '' || $("#AssetIngredientCostPrice").val() == 0) {
            message = "Enter Ingredient Sales Price"
            return message;
        }

        for (var i = 0; i < assetEventIngredientDataArr.length; i++) {
            if ($("#IngredientId").val() == assetEventIngredientDataArr[i].foodMenuId) {
                message = "Ingredient already selected!"
                break;
            }
        }
    }
    return message;
}

function calculateColumn(index) {
    var total = 0;
    $('#AssetEventFoodmenu tbody tr').each(function () {
        var value = parseFloat($('td', this).eq(index).text());
        if (!isNaN(value)) {
            total += value;
        }
    });
    return total;
}

function calculateAssetItemColumn(index) {
    var total = 0;
    $('#AssetEventItem tbody tr').each(function () {
        var value = parseFloat($('td', this).eq(index).text());
        if (!isNaN(value)) {
            total += value;
        }
    });
    return total;
}

function calculateIngredientColumn(index) {
    var total = 0;
    $('#AssetIngredientItem tbody tr').each(function () {
        var value = parseFloat($('td', this).eq(index).text());
        if (!isNaN(value)) {
            total += value;
        }
    });
    return total;
}

function calculateGross() {
    var total = 0;
    $('#AssetEventFoodmenu tbody tr').each(function () {
        var qty = parseFloat($('td', this).eq(2).text());
        var Price = parseFloat($('td', this).eq(3).text());
        if (!isNaN(Price)) {
            total += (qty * Price);
        }
    });
    return total;
}

$("#FoodDiscountAmount").change(function () {
    var FoodNetAmount;
    FoodNetAmount = parseFloat($("#FoodGrossAmount").val()) - parseFloat($("#FoodDiscountAmount").val());
    $("#FoodNetAmount").val(parseFloat(FoodNetAmount).toFixed(2));
});

$("#FoodDiscountAmount").keyup(function () {
    var FoodNetAmount;
    FoodNetAmount = parseFloat($("#FoodGrossAmount").val()) - parseFloat($("#FoodDiscountAmount").val());
    $("#FoodNetAmount").val(parseFloat(FoodNetAmount).toFixed(2));
});

//Allocate
$(function () {
    $('#allocateOrder').click(function () {
        var message = validation(2);

        var status = $("#Status").val();
        if (status <= 1) { status = 2; }

        if (message == '') {
            $("#assetEventForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    EventType: 1,
                    EventName: $("#EventName").val(),
                    EventDatetime: $("#EventDatetime").val(),
                    EventPlace: $("#EventPlace").val(),
                    ContactPersonName: $("#ContactPersonName").val(),
                    ContactPersonNumber: $("#ContactPersonNumber").val(),
                    FoodGrossAmount: $("#FoodGrossAmount").val(),
                    FoodDiscountAmount: $("#FoodDiscountAmount").val(),
                    FoodNetAmount: $("#FoodNetAmount").val(),
                    FoodVatAmount: $("#FoodVatAmount").val(),
                    FoodTaxAmount: $("#FoodTaxAmount").val(),
                    MissingTotalAmount: $("#MissingTotalAmount").val(),
                    IngredientNetAmount: $("#IngredientNetAmount").val(),
                    AssetItemNetAmount: $("#AssetItemNetAmount").val(),
                    Status: status,
                    assetEventItemModels: assetEventItemDataArr,
                    assetEventFoodmenuModels: assetEventFoodmenuDataArr,
                    assetEventIngredientModels: assetEventIngredientDataArr,
                    AssetEventItemDeletedId: assetEventItemDeletedId,
                    AssetEventFoodmenuDeletedId: assetEventfoodMenuDeletedId,
                    AssetEventIngredientDeletedId: assetEventIngredientDeletedId
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


//Returned
$(function () {
    $('#returnOrder').click(function () {
        var message = validation(2);

        var status = $("#Status").val();
        if (status <= 2) { status = 3; }

        if (message == '') {
            $("#assetEventForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    EventType: 1,
                    EventName: $("#EventName").val(),
                    EventDatetime: $("#EventDatetime").val(),
                    EventPlace: $("#EventPlace").val(),
                    ContactPersonName: $("#ContactPersonName").val(),
                    ContactPersonNumber: $("#ContactPersonNumber").val(),
                    FoodGrossAmount: $("#FoodGrossAmount").val(),
                    FoodDiscountAmount: $("#FoodDiscountAmount").val(),
                    FoodNetAmount: $("#FoodNetAmount").val(),
                    FoodVatAmount: $("#FoodVatAmount").val(),
                    FoodTaxAmount: $("#FoodTaxAmount").val(),
                    MissingTotalAmount: $("#MissingTotalAmount").val(),
                    IngredientNetAmount: $("#IngredientNetAmount").val(),
                    AssetItemNetAmount: $("#AssetItemNetAmount").val(),
                    Status: status,
                    assetEventItemModels: assetEventItemDataArr,
                    assetEventFoodmenuModels: assetEventFoodmenuDataArr,
                    assetEventIngredientModels: assetEventIngredientDataArr,
                    AssetEventItemDeletedId: assetEventItemDeletedId,
                    AssetEventFoodmenuDeletedId: assetEventfoodMenuDeletedId,
                    AssetEventIngredientDeletedId: assetEventIngredientDeletedId
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


//Closed
$(function () {
    $('#closeOrder').click(function () {
        var message = validation(2);

        var status = $("#Status").val();
        if (status <= 3) { status = 4; }

        if (message == '') {
            $("#assetEventForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    EventType: 1,
                    EventName: $("#EventName").val(),
                    EventDatetime: $("#EventDatetime").val(),
                    EventPlace: $("#EventPlace").val(),
                    ContactPersonName: $("#ContactPersonName").val(),
                    ContactPersonNumber: $("#ContactPersonNumber").val(),
                    FoodGrossAmount: $("#FoodGrossAmount").val(),
                    FoodDiscountAmount: $("#FoodDiscountAmount").val(),
                    FoodNetAmount: $("#FoodNetAmount").val(),
                    FoodVatAmount: $("#FoodVatAmount").val(),
                    FoodTaxAmount: $("#FoodTaxAmount").val(),
                    MissingTotalAmount: $("#MissingTotalAmount").val(),
                    IngredientNetAmount: $("#IngredientNetAmount").val(),
                    AssetItemNetAmount: $("#AssetItemNetAmount").val(),
                    Status: status,
                    assetEventItemModels: assetEventItemDataArr,
                    assetEventFoodmenuModels: assetEventFoodmenuDataArr,
                    assetEventIngredientModels: assetEventIngredientDataArr,
                    AssetEventItemDeletedId: assetEventItemDeletedId,
                    AssetEventFoodmenuDeletedId: assetEventfoodMenuDeletedId,
                    AssetEventIngredientDeletedId: assetEventIngredientDeletedId
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
        },
        error: function (data) {
            alert(data);
        }
    });
}


function GetUnitNameByAssetItemId() {
    $.ajax({
        url: "/AssetEvent/GetAssetItemUnitName?id=" + $("#AssetItemId").val(),
        data: {},
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            $("#AssetItemUnitName").text(obj.assetItemUnitName);
        },
        error: function (data) {
            alert(data);
        }
    });
}

$(document).on('change', 'input[name]', function () {

    str = this.id;
    str2 = "allocatedQty";
    str3 = "returnQty";
    str4 = "missingQty";
    str5 = "IngredientRQty";
    str6 = "IngredientAQty";

    //Asset Item Allocated
    if (str.indexOf(str2) != -1) {
        id = str.substring(12, 100);
        returnId = "#returnQty" + id.toString();

        //alert(this.id + " focus out");
        var qty = $(this).val();
        // alert(qty);
        $(returnId).val(qty);

        var MissingQty = parseFloat((parseFloat(AllocatedQty) - parseFloat(ReturnQty))).toFixed(2);

        for (var i = 0; i < assetEventItemDataArr.length; i++) {
            if (assetEventItemDataArr[i].assetItemId == id) {
                assetEventItemDataArr[i].allocatedQty = qty;
                assetEventItemDataArr[i].returnQty = qty;
            }
        }
    }

    //Asset Item Returned
    if (str.indexOf(str3) != -1) {

        id = str.substring(9, 100);
        missingId = "#missingQty" + id.toString();
        allocatedId = "#allocatedQty" + id.toString();

        // alert(this.id + " focus out");

        var returnQty = $(this).val();
        var allocatedQty = $(allocatedId).val();
        missingQ = parseFloat((parseFloat(allocatedQty) - parseFloat(returnQty))).toFixed(2);

        // alert(missingId);
        //  alert(missingQ);

        $(missingId).val(missingQ);

        $("#MissingTotalAmount").val(calculateMissingTotal());

        for (var i = 0; i < assetEventItemDataArr.length; i++) {
            if (assetEventItemDataArr[i].assetItemId == id) {
                assetEventItemDataArr[i].returnQty = returnQty;
                assetEventItemDataArr[i].missingQty = missingQ;
            }
        }
    }

    //Missing Total
    if (str.indexOf(str4) != -1) {
        $("#MissingTotalAmount").val(calculateMissingTotal());
    }

    //Stock Ingredient Item Return
    if (str.indexOf(str5) != -1) {
        id = str.substring(14, 100);
        returnId = "#IngredientRQty" + id.toString();

        // alert(id + " focus out");
        var qty = $(this).val();
        // alert(qty);
        $(returnId).val(qty);

        for (var i = 0; i < assetEventIngredientDataArr.length; i++) {
            if (assetEventIngredientDataArr[i].ingredientId == id) {
                assetEventIngredientDataArr[i].returnQty = qty;
            }
        }
    }

    //Stock Ingredient Item Actual
    if (str.indexOf(str6) != -1) {
        id = str.substring(14, 100);
        returnId = "#IngredientAQty" + id.toString();

        // alert(id + " focus out");
        var qty = $(this).val();
        // alert(qty);
        $(returnId).val(qty);

        for (var i = 0; i < assetEventIngredientDataArr.length; i++) {
            if (assetEventIngredientDataArr[i].ingredientId == id) {
                assetEventIngredientDataArr[i].actualQty = qty;
            }
        }
    }


});




$(document).on('onchange', 'select[name]', function () {
    alert('IN');
});

$("select").change(function () { // any select that changes.
    str = this.id;
    str2 = "MissingNote";

    //Allocated
    if (str.indexOf(str2) != -1) {
        id = str.substring(11, 100);
        MissingNoteText = $(this).val();

        for (var i = 0; i < assetEventItemDataArr.length; i++) {
            if (assetEventItemDataArr[i].assetItemId == id) {
                assetEventItemDataArr[i].missingNote = MissingNoteText;
            }
        }
    }
    alert(MissingNoteText);
})


//Stock Update
$('#selectAll').click(function (e) {
    $(this).closest('table').find('td input:checkbox').prop('checked', this.checked);
});

$('#btnStockOut').click(function () {
    var assetEventId = [];
    $('.row input:checked').each(function () {
        var chkId = $(this)[0].id;
        if (chkId != "selectAll") {
            assetEventId.push($(this)[0].id);
        }
    });

    $.ajax({
        url: "/AssetEvent/UpdateStockById",
        data: { 'ids': assetEventId },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.status == "200") {
                $(".bmodal-body").text(data.message);
                $("#bsave").show();
                $("#bok").hide();
                jQuery.noConflict();
                $("#bModal").modal('show');
            }
            else {
                $(".bmodal-body").text(data.message);
                $("#bok").show();
                $("#bsave").hide();
                jQuery.noConflict();
                $("#bModal").modal('show');
            }
        },
        error: function (data) {
            alert(data);
        }
    });
});

$("#bsave").click(function () {
    window.location.href = "";
});

$('#bok').click(function () {
    $("#bModal").modal('hide');
});


function calculateMissingTotal() {
    var total = 0;
    $('#AssetEventItem tbody tr').each(function () {
        var qty = parseFloat($(this).children("td:eq(7)").find("input").val());
        var Price = parseFloat($('td', this).eq(3).text()); 
        if (!isNaN(Price)) {
            total += (qty * Price);
        }
    });
    return total;
}