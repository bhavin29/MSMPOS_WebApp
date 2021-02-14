var editAssetEventFoodmenuDataArr = [];
var assetEventfoodMenuDeletedId = [];
var editAssetEventItemDataArr = [];
var assetEventItemDeletedId = [];
var GrossAmount = 0;
var NetAmountTotal = 0;
$(document).ready(function () {
    $("#assetEventForm").validate();

    AssetEventItem = $('#AssetEventItem').DataTable({
        columnDefs: [
            { targets: [0, 1], orderable: false },
            { targets: [3, 4], orderable: false, class: "text-right" },
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

    AssetEventFoodmenu = $('#AssetEventFoodmenu').DataTable({
        columnDefs: [
            { targets: [0, 1], orderable: false, visible:false},
            { targets: [3, 4], orderable: false, class: "text-right" }
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
    $("#EventType").focus();

});

//EventItem


$('#addRowItem').on('click', function (e) {
    e.preventDefault();
    var message = validation(3);
    var rowId = "rowId" + $("#AssetItemId").val();
    var EventQty =parseFloat($("#EventQty").val()).toFixed(2);
    var StockQty = parseFloat($("#StockQty").val()).toFixed(2);
    var AllocatedQty = parseFloat($("#AllocatedQty").val()).toFixed(2);
    var ReturnQty = parseFloat($("#ReturnQty").val()).toFixed(2);
    var MissingQty = parseFloat($("#MissingQty").val()).toFixed(2);
    var MissingNote = $("#MissingNote").val();
    if (message == '') {
        AssetEventItem.row('.active').remove().draw(false);
        var rowNode = AssetEventItem.row.add([
            '<td class="text-right"><input class="form-control" type="hidden" value="' + $("#AssetEventItemId").val() + '" /> </td>',
            '<td class="text-right"><input class="form-control" type="hidden" value="' + $("#AssetItemId").val() + '" /> </td>',
            $('#AssetItemId').children("option:selected").text(),
            '<td class="text-right"><input type="number" id="stockQty" class="form-control col-sm-11 text-right" min="0" max="99999" value="' + StockQty + '" name="item.StockQty"> </td>',
            '<td class="text-right">' + EventQty + ' </td>',
            '<td class="text-right"><input type="number" id="allocatedQty" class="form-control col-sm-11 text-right" min="0" max="99999" value="' + AllocatedQty + '" name="item.AllocatedQty"> </td>',
            '<td class="text-right"><input type="number" id="returnQty" class="form-control col-sm-11 text-right" min="0" max="99999" value="' + ReturnQty + '" name="item.ReturnQty"> </td>',
            '<td class="text-right"><input type="number" id="missingQty" class="form-control col-sm-11 text-right" min="0" max="99999" value="' + MissingQty + '" name="item.MissingQty"> </td>',
            '<td class="text-right"><input type="text" id="missingNote" class="form-control col-sm-11" value="' + MissingNote + '" name="item.MissingNote"> </td>',
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
            missingNote: MissingNote,
            assetEventItemId: $("#AssetEventItemId").val()
        });

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
                $("#MissingNote").val(assetEventItemDataArr[i].missingNote);
                editAssetEventItemDataArr = assetEventItemDataArr.splice(i, 1);
            }
        }
    }
});

//FoodMenu
$('#addRowFood').on('click', function (e) {
    e.preventDefault();
    var message = validation(1);
    var rowId = "rowId" + $("#FoodMenuId").val();
    var Price;
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
    var TotalPrice = 0;
    Qty = parseFloat(Qty).toFixed(2);
    TotalPrice = parseFloat(Qty * Price).toFixed(2);
    if (message == '') {
        AssetEventFoodmenu.row('.active').remove().draw(false);
        var rowNode = AssetEventFoodmenu.row.add([
            $("#AssetEventFoodmenuId").val(),
            '<td class="text-right">' + $("#FoodMenuId").val() + ' </td>',
            $('#FoodMenuId').children("option:selected").text(),
            '<td class="text-right">' + Price + ' </td>',
            '<td class="text-right">' + Qty + ' </td>',
            '<td class="text-right">' + TotalPrice + ' </td>',
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#FoodMenuId").val() + '" class=" editFoodMenuItem">Edit</a></a> / <a href="#" data-toggle="modal" data-target="#myModal0">Delete</a></div></td > ' +
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteFoodMenuOrder(0, ' + $("#FoodMenuId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
        ]).draw(false).nodes();
        assetEventFoodmenuDataArr.push({
            foodMenuId: $("#FoodMenuId").val(),
            qunatity: Qty,
            salesPrice: Price,
            totalPrice: TotalPrice,
            assetEventFoodmenuId: $("#AssetEventFoodmenuId").val()
        });
        $(rowNode).find('td').eq(2).addClass('text-right');
        $(rowNode).find('td').eq(3).addClass('text-right');
        $(rowNode).find('td').eq(4).addClass('text-right');

        GrossAmount = calculateColumn(3);

        $("#FoodGrossAmount").val(parseFloat(GrossAmount).toFixed(2));
        $("#FoodNetAmount").val(parseFloat(GrossAmount).toFixed(2));
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
                editAssetEventFoodmenuDataArr = assetEventFoodmenuDataArr.splice(i, 1);
            }
        }
    }
});

function saveOrder(data) {
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

        assetEventItemDataArr.push({
            assetEventItemId: $(currentRow).find("td:eq(0) input[type='hidden']").val(),
            assetItemId: $(currentRow).find("td:eq(1) input[type='hidden']").val(),
            stockQty: $(currentRow).find("td:eq(3) input[type='number']").val(),
            eventQty: tds[4].textContent,
            allocatedQty: $(currentRow).find("td:eq(5) input[type='number']").val(),
            returnQty: $(currentRow).find("td:eq(6) input[type='number']").val(),
            missingQty: $(currentRow).find("td:eq(7) input[type='number']").val(),
            missingNote: $(currentRow).find("td:eq(8) input[type='text']").val()
        });
    });
}

$(function () {
    $('#saveOrder').click(function () {
        var message = validation(2);
        
        GetAssetItemArray();

        if (message == '') {
            $("#assetEventForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    EventType: $("#EventType").val(),
                    EventName: $("#EventName").val(),
                    EventDatetime: $("#EventDatetime").val(),
                    EventPlace: $("#EventPlace").val(),
                    ContactPersonName: $("#ContactPersonName").val(),
                    ContactPersonNumber: $("#ContactPersonNumber").val(),
                    FoodGrossAmount: $("#FoodGrossAmount").val(),
                    FoodDiscountAmount: $("#FoodDiscountAmount").val(),
                    FoodNetAmount: $("#FoodNetAmount").val(),
                    Status: 1,
                    assetEventItemModels: assetEventItemDataArr,
                    assetEventFoodmenuModels: assetEventFoodmenuDataArr,
                    AssetEventItemDeletedId: assetEventItemDeletedId,
                    AssetEventFoodmenuDeletedId: assetEventfoodMenuDeletedId
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

    if (id==1) {
        if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == '0') {
            message = "Select Food Menu"
            return message;
        }
        else if ($("#Quantity").val() == '' || $("#Quantity").val() == 0) {
            message = "Enter Quantity"
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
        if (!AssetEventItem.data().any() || AssetEventItem.data().row == null) {
            var message = 'At least one order should be entered'
            return message;
        }

        if (!AssetEventFoodmenu.data().any() || AssetEventFoodmenu.data().row == null) {
            var message = 'At least one order should be entered'
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

        for (var i = 0; i < assetEventItemDataArr.length; i++) {
            if ($("#AssetItemId").val() == assetEventItemDataArr[i].assetItemId) {
                message = "AssetItem already selected!"
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
        GetAssetItemArray();

        if (message == '') {
            $("#assetEventForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    EventType: $("#EventType").val(),
                    EventName: $("#EventName").val(),
                    EventDatetime: $("#EventDatetime").val(),
                    EventPlace: $("#EventPlace").val(),
                    ContactPersonName: $("#ContactPersonName").val(),
                    ContactPersonNumber: $("#ContactPersonNumber").val(),
                    FoodGrossAmount: $("#FoodGrossAmount").val(),
                    FoodDiscountAmount: $("#FoodDiscountAmount").val(),
                    FoodNetAmount: $("#FoodNetAmount").val(),
                    Status: 2,
                    assetEventItemModels: assetEventItemDataArr,
                    assetEventFoodmenuModels: assetEventFoodmenuDataArr,
                    AssetEventItemDeletedId: assetEventItemDeletedId,
                    AssetEventFoodmenuDeletedId: assetEventfoodMenuDeletedId
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
        GetAssetItemArray();

        if (message == '') {
            $("#assetEventForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    EventType: $("#EventType").val(),
                    EventName: $("#EventName").val(),
                    EventDatetime: $("#EventDatetime").val(),
                    EventPlace: $("#EventPlace").val(),
                    ContactPersonName: $("#ContactPersonName").val(),
                    ContactPersonNumber: $("#ContactPersonNumber").val(),
                    FoodGrossAmount: $("#FoodGrossAmount").val(),
                    FoodDiscountAmount: $("#FoodDiscountAmount").val(),
                    FoodNetAmount: $("#FoodNetAmount").val(),
                    Status: 3,
                    assetEventItemModels: assetEventItemDataArr,
                    assetEventFoodmenuModels: assetEventFoodmenuDataArr,
                    AssetEventItemDeletedId: assetEventItemDeletedId,
                    AssetEventFoodmenuDeletedId: assetEventfoodMenuDeletedId
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
        GetAssetItemArray();

        if (message == '') {
            $("#assetEventForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    EventType: $("#EventType").val(),
                    EventName: $("#EventName").val(),
                    EventDatetime: $("#EventDatetime").val(),
                    EventPlace: $("#EventPlace").val(),
                    ContactPersonName: $("#ContactPersonName").val(),
                    ContactPersonNumber: $("#ContactPersonNumber").val(),
                    FoodGrossAmount: $("#FoodGrossAmount").val(),
                    FoodDiscountAmount: $("#FoodDiscountAmount").val(),
                    FoodNetAmount: $("#FoodNetAmount").val(),
                    Status: 4,
                    assetEventItemModels: assetEventItemDataArr,
                    assetEventFoodmenuModels: assetEventFoodmenuDataArr,
                    AssetEventItemDeletedId: assetEventItemDeletedId,
                    AssetEventFoodmenuDeletedId: assetEventfoodMenuDeletedId
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