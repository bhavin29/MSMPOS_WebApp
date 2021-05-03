var SalesDeliveryDatatable;
var editDataArr = [];
var deletedId = [];
var DisPerTotal = 0;
var DisAmtTotal = 0;
var TaxPerTotal = 0;
var TaxAmount = 0;
var GTotal = 0;
var GrossAmount = 0;
var TaxAmountTotal = 0;
var TotalAmount = 0
var Status = 0;

$(document).ready(function () {
    GetFoodMenuByItemType();
    //var supId = $("#SupplierId").val();

    //if (supId != null && supId != 0) {
    //    GetSupplierDetailsById(supId);
    //}

    $("#salesDelivery").validate();

    SalesDeliveryDatatable = $('#salesDeliveryDetails').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "oLanguage": {
            "EmptyTable": "No data available"
        },
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [9],
                "Data": "", "name": "Action", "defaultContent": '<a href="#" class="deleteItem">Delete Order</a>', "autoWidth": true
            }
            ,
            {
                "targets": [5, 7, 10, 11, 13, 14, 15],
                "visible": false,
                "searchable": false
            }
        ]
    });

    $('#TaxType').hide();
    $("#StoreId").focus();
    $("#StoreId").select2();
    $("#CustomerId").select2();
    $("#FoodMenuId").select2();
});

$('#cancel').on('click', function (e) {
    e.preventDefault();
    TotalAmount += editDataArr[0].total;
    $("#TotalAmount").val(parseFloat(TotalAmount).toFixed(2));
    DueAmount();
    clearItem();
    dataArr.push(editDataArr[0]);
    editDataArr = [];
});

$('#addRow').on('click', function (e) {
    e.preventDefault();
    var message = validation(0);
    var DiscountPercentage = 0;
    var DiscountAmount = 0;
    var TaxPercentage = 0;
    var TaxAmount = 0;
    var Vatable = 0;
    var Nonvatable = 0;
    var VatableTotal = 0;
    var NonvatableTotal = 0;
    var foodMenuId = $("#FoodMenuId").val();
    var itemType = $("#ItemType").val();
    var recordid = $("#ItemType").val() + 'rowId' + $("#FoodMenuId").val();
    var taxType = $("#TaxType").val();

    $.ajax({
        url: "/SalesDelivery/GetTaxByFoodMenuId",
        data: { "foodMenuId": foodMenuId, "itemType": itemType },
        async: false,
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            TaxPercentage = obj.taxPercentage;
            TaxPercentage = parseFloat(TaxPercentage).toFixed(2);
        }
    });

    var POQty = parseFloat($("#POQty").val()).toFixed(2);
    var GRNQty = parseFloat($("#GRNQty").val()).toFixed(2);
    var UnitPrice = parseFloat($("#UnitPrice").val()).toFixed(2);
    var Total = parseFloat($("#UnitPrice").val() * $("#GRNQty").val()).toFixed(2);
    DiscountPercentage = parseFloat($("#DiscountPercentage").val()).toFixed(2);

    POQty = getNum(POQty);
    GRNQty = getNum(GRNQty);
    DiscountPercentage = getNum(DiscountPercentage);
    TaxPercentage = getNum(TaxPercentage);
    UnitPrice = getNum(UnitPrice);
    Total = getNum(Total);

    if (DiscountPercentage > 0) {
        DiscountAmount = ((parseFloat(Total) * parseFloat(DiscountPercentage)) / 100).toFixed(2);
        DisAmtTotal = parseFloat(parseFloat(DiscountAmount) + parseFloat($("#DiscountAmount").val())).toFixed(2);
        Total = (parseFloat(Total) - parseFloat(DiscountAmount)).toFixed(2);
    }
    else {
        DiscountAmount = parseFloat(0).toFixed(2);
    }

    var TaxTypeVaule = +$('#TaxType').is(':checked')
    if (TaxPercentage > 0) {
        if (TaxTypeVaule == 1) {
            TaxAmount = (parseFloat(Total) - ((parseFloat(Total) / (100 + parseFloat(TaxPercentage))) * 100)).toFixed(2);
        }
        else {
            TaxAmount = ((parseFloat(Total) * parseFloat(TaxPercentage)) / 100).toFixed(2);
            Total = (parseFloat(Total) + parseFloat(TaxAmount)).toFixed(2);
        }
    }

    if (TaxAmount > 0) {
        Vatable = (parseFloat(Total) - parseFloat(TaxAmount)).toFixed(2);
    }
    else {
        Nonvatable = parseFloat(Total).toFixed(2);
    }

    if (message == '') {
        SalesDeliveryDatatable.row('.active').remove().draw(false);

        var rowNode = SalesDeliveryDatatable.row.add([
            '<td class="text-right">' + $("#FoodMenuId").val() + ' </td>',
            $('#FoodMenuId').children("option:selected").text(),
            '<td class="text-right">' + POQty + ' </td>',
            '<td class="text-right">' + GRNQty + ' </td>',
            '<td class="text-right">' + UnitPrice + ' </td>',
            '<td class="text-right">' + DiscountPercentage + ' </td>',
            '<td class="text-right">' + DiscountAmount + ' </td>',
            '<td class="text-right">' + TaxPercentage + ' </td>',
            '<td class="text-right">' + TaxAmount + ' </td>',
            '<td class="text-right">' + Total + ' </td>',
            '<td class="text-right">' + Vatable + ' </td>',
            '<td class="text-right">' + Nonvatable + ' </td>',
            '<td><div class="form-button-action"><a href="#" data-itemId="' + recordid + '"" ></a><a href="#" data-toggle="modal" data-target="#myModal' + + $("#ItemType").val() + '' + $("#FoodMenuId").val() + '">Delete</a></div></td > ' +
            '<div class="modal fade" id=myModal' + $("#ItemType").val() + '' + $("#FoodMenuId").val() + ' tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteOrder(\'' + recordid + '\')" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
            $("#SalesDeliveryId").val(),
            $("#ItemType").val(),
            recordid
        ]).node().id = recordid;
        SalesDeliveryDatatable.draw(false);

        dataArr.push({
            foodMenuId: $("#FoodMenuId").val(),
            soQty: POQty,
            deliveryQTY: GRNQty,
            unitPrice: UnitPrice,
            discountAmount: DiscountAmount,
            discountPercentage: DiscountPercentage,
            grossAmount: GRNQty * UnitPrice,
            taxAmount: TaxAmount,
            taxPercentage: TaxPercentage,
            totalAmount: Total,
            vatableAmount: Vatable,
            nonVatableAmount: Nonvatable,
            salesDeliveryId: $("#SalesDeliveryId").val(),
            itemType: $("#ItemType").val()
        });

        var attRowNode = '#' + recordid;

        $(attRowNode).find('td').eq(1).addClass('text-right');
        $(attRowNode).find('td').eq(2).addClass('text-right');
        $(attRowNode).find('td').eq(3).addClass('text-right');
        $(attRowNode).find('td').eq(4).addClass('text-right');
        $(attRowNode).find('td').eq(5).addClass('text-right');
        $(attRowNode).find('td').eq(6).addClass('text-right');

        DisAmtTotal = calculateDiscountColumn(4);
        TaxAmountTotal = CalculateArray('TaxAmount');
        VatableTotal = CalculateArray('VatableAmount');
        NonvatableTotal = CalculateArray('NonvatableAmount');
        TotalAmount = CalculateArray('Total');
        GrossAmount = TotalAmount - TaxAmountTotal;

        $("#GrossAmount").val(parseFloat(GrossAmount).toFixed(2));
        $("#TaxAmount").val(parseFloat(TaxAmountTotal).toFixed(2));
        $("#TotalAmount").val(parseFloat(TotalAmount).toFixed(2));
        $("#VatableAmount").val(parseFloat(VatableTotal).toFixed(2));
        $("#NonVatableAmount").val(parseFloat(NonvatableTotal).toFixed(2));

        DueAmount();
        clearItem();
        $("#ItemType").focus();
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
        url: 'SalesDeliveryFoodMenu',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        var message = validation(1);
        if (message == '') {
            $("#salesDelivery").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    SalesId: $("#SalesId").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    StoreId: $("#StoreId").val(),
                    CustomerId: $("#CustomerId").val(),
                    SupplierEmail: '',
                    IsSendEmail: "false",
                    GrossAmount: $("#GrossAmount").val(),
                    TaxAmount: $("#TaxAmount").val(),
                    TotalAmount: $("#TotalAmount").val(),
                    VatableAmount: $("#VatableAmount").val(),
                    NonVatableAmount: $("#NonVatableAmount").val(),
                    DeliveryNoteNumber: $("#DeliveryNoteNumber").val(),
                    DeliveryDate: $("#DeliveryDate").val(),
                    SalesDeliveryDate: $("#SalesDeliveryDate").val(),
                    DriverName: $("#DriverName").val(),
                    VehicleNumber: $("#VehicleNumber").val(),
                    Notes: $("#Notes").val(),
                    SupplierList: [],
                    FoodMenuList: [],
                    salesDeliveryDetails: dataArr,
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

$(function () {
    $('#saveAndApprovedOrder').click(function () {
        var message = validation(1);
        if (message == '') {
            $("#salesDelivery").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    CustomerId: $("#CustomerId").val(),
                    //SupplierEmail: $("#SupplierEmail").val(),
                    //IsSendEmail: $("#IsSendEmail").is(":checked") ? "true" : "false",
                    StoreId: $("#StoreId").val(),
                    EmployeeId: $("#EmployeeId").val(),
                    Date: $("#SalesDeliveryDate").val(),
                    GrossTotal: $("#GrossTotal").val(),
                    TaxAmountTotal: $("#TaxAmount").val(),
                    DiscountAmount: $("#DiscountAmount").val(),
                    TotalAmount: $("#TotalAmount").val(),
                    Due: $("#DueAmount").val(),
                    Paid: $("#PaidAmount").val(),
                    Notes: $("#Notes").val(),
                    Status: 2,
                    SupplierList: [],
                    FoodMenuList: [],
                    salesDeliveryDetails: dataArr,
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
    })
});

$("#save").click(function () {
    window.location.href = "/SalesDelivery/SalesDeliveryFoodMenuList";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

function deleteOrder(rowId) {
    var id;
    var DisAmtTotal = 0;
    var TaxAmountTotal = 0;
    var VatableTotal = 0;
    var NonvatableTotal = 0;
    var TotalAmount = 0;
    var GrossAmount = 0;

    for (var i = 0; i < dataArr.length; i++) {

        id = dataArr[i].itemType + 'rowId' + dataArr[i].foodMenuId;

        if (id == rowId) {
            deletedId.push(dataArr[i].salesDeliveryId);
            dataArr.splice(i, 1);
            SalesDeliveryDatatable.row('#' + rowId).remove().draw(false);
            jQuery.noConflict();
        }
    }

    DisAmtTotal = calculateDiscountColumn(4);
    TaxAmountTotal = CalculateArray('TaxAmount');
    VatableTotal = CalculateArray('VatableAmount');
    NonvatableTotal = CalculateArray('NonvatableAmount');
    TotalAmount = CalculateArray('Total');
    GrossAmount = TotalAmount - TaxAmountTotal;

    $("#GrossAmount").val(parseFloat(GrossAmount).toFixed(2));
    $("#TaxAmount").val(parseFloat(TaxAmountTotal).toFixed(2));
    $("#TotalAmount").val(parseFloat(TotalAmount).toFixed(2));
    $("#VatableAmount").val(parseFloat(VatableTotal).toFixed(2));
    $("#NonVatableAmount").val(parseFloat(NonvatableTotal).toFixed(2));

    $("#myModal" + salesDeliveryId).modal('hide');
};

$(document).on('click', 'a.editItem', function (e) {
    if (!SalesDeliveryDatatable.data().any() || SalesDeliveryDatatable.data().row == null) {
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
            if (dataArr[i].foodMenuId == id) {
                $("#FoodMenuId").val(dataArr[i].foodMenuId),
                    $("#POQty").val(dataArr[i].quantity),
                    $("#GRNQty").val(dataArr[i].quantity),
                    $("#UnitPrice").val(dataArr[i].unitPrice),
                    $("#DiscountPercentage").val(dataArr[i].DiscountPercentage),
                    $("#DiscountAmounts").val(dataArr[i].Discount),
                    $("#SalesDeliveryId").val(dataArr[i].salesDeliveryId),
                    $("#ItemType").val(dataArr[i].itemType)
                GrandTotal = $("#GrossAmount").val();
                TotalAmount = dataArr[i].total;
                GrandTotal -= TotalAmount;
                $("#GrossAmount").val(GrandTotal);
                DueAmount();
                editDataArr = dataArr.splice(i, 1);
            }
        }
    }
});

$("#PaidAmount").blur(function () {
    DueAmount();
});

function DueAmount() {
    var Due = 0
    Due = (GrandTotal - parseFloat($("#PaidAmount").val()));
    $("#DueAmount").val(Due);
}

function validation(id) {
    var message = '';
    if ($("#CustomerId").val() == '' || $("#CustomerId").val() == '0') {
        message = "Select Customer"
        return message;
    }

    if ($("#StoreId").val() == '' || $("#StoreId").val() == '0') {
        message = "Select Store"
        return message;
    }

    if ($("#EmployeeId").val() == '' || $("#EmployeeId").val() == '0') {
        message = "Select Employee"
        return message;
    }
    if (id == 1) {
        if (!SalesDeliveryDatatable.data().any() || SalesDeliveryDatatable.data().row == null) {
            var message = 'At least one order should be entered'
            return message;
        }
    }
    else {
        if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == '0') {
            message = "Select Product"
            return message;
        }
        else if ($("#UnitPrice").val() == '' || $("#UnitPrice").val() == 0) {
            message = "Enter unit price"
            return message;
        }
        else if ($("#POQty").val() == '' || $("#POQty").val() == 0) {
            message = "Enter quantity"
            return message;
        }
        else if ($("#GRNQty").val() == '' || $("#GRNQty").val() == 0) {
            message = "Enter quantity"
            return message;
        }
        for (var i = 0; i < dataArr.length; i++) {
            if ($("#FoodMenuId").val() == dataArr[i].foodMenuId && $("#ItemType").val() == dataArr[i].itemType) {
                message = "Product already selected!"
                break;
            }
        }
    }
    return message;
}

function clearItem() {
    $("#UnitPrice").val(''),
        $("#POQty").val('1'),
        $("#GRNQty").val('1'),
        $("#DiscountPercentage").val(''),
        $("#SalesDeliveryId").val('0'),
        //     $("#ItemType").val('0'),
        $('#FoodMenuId').val(0).trigger('change')
    // GetFoodMenuByItemType();
}

function calculateDiscountColumn(index) {
    var totaldis = 0.00, discount = 0;
    $('#salesDeliveryDetails tbody tr').each(function () {
        var value = parseFloat($('td', this).eq(4).text());// disc%
        var value2 = parseFloat($('td', this).eq(2).text());//qty
        var value3 = parseFloat($('td', this).eq(3).text());//price

        if (!isNaN(value)) {
            discount = (((parseFloat(value2) * parseFloat(value3)) * parseFloat(value)) / 100).toFixed(2);
            totaldis = (parseFloat(totaldis) + parseFloat(discount));
        }
    });
    return totaldis;
}


function getNum(val) {
    val = +val || 0
    return val.toFixed(2);
}

function calculateColumn(index) {
    var total = 0;
    $('#salesDeliveryDetails tbody tr').each(function () {
        var value = parseFloat($('td', this).eq(index).text());
        if (!isNaN(value)) {
            total += value;
        }
    });
    return total;
}

function calculateGross() {
    var total = 0;
    $('#salesDeliveryDetails tbody tr').each(function () {
        var Grnqty = parseFloat($('td', this).eq(2).text());
        var Price = parseFloat($('td', this).eq(3).text());
        if (!isNaN(Price)) {
            total += (Grnqty * Price);
        }


    });
    return total;
}

function GetFoodMenuLastPrice(foodMenuId) {
    var itemType = $("#ItemType").val();

    $.ajax({
        url: "/Sales/GetFoodMenuLastPrice",
        data: { "itemType": itemType, "foodMenuId": foodMenuId.value },
        type: "GET",
        dataType: "text",
        success: function (data) {
            $("#UnitPrice").val('');
            var obj = JSON.parse(data);
            $("#UnitPrice").val(parseFloat(obj.unitPrice));
        },
        error: function (data) {
            alert(data);
        }
    });
}


function GetPurchaseGRNbyPO(poReference) {
    poReference = $("#POReference").val();
    $.ajax({
        url: "/SalesDelivery/GetPurchaseIdByPOReference",
        data: { "poReference": poReference },
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            if (obj.purchaseId > 0) {
                window.location.href = "/SalesDelivery/SalesDeliveryFoodMenu?purchaseId=" + obj.purchaseId;
            }
            else {
                alert("Reference Number Not Found!");
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}

function GetFoodMenuByItemType() {

    var itemType = $("#ItemType").val();
    if (itemType == 0) {
        $.ajax({
            url: "/Sales/GetFoodMenuList",
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
    if (itemType == 1) {
        $.ajax({
            url: "/Sales/GetIngredientList",
            data: {},
            type: "GET",
            dataType: "text",
            success: function (data) {
                $("#FoodMenuId").empty();
                var obj = JSON.parse(data);
                for (var i = 0; i < obj.ingredientList.length; ++i) {
                    $("#FoodMenuId").append('<option value="' + obj.ingredientList[i].value + '">' + obj.ingredientList[i].text + '</option>');
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    }
    if (itemType == 2) {
        $.ajax({
            url: "/Sales/GetAssetItemList",
            data: {},
            type: "GET",
            dataType: "text",
            success: function (data) {
                $("#FoodMenuId").empty();
                var obj = JSON.parse(data);
                for (var i = 0; i < obj.assetItemList.length; ++i) {
                    $("#FoodMenuId").append('<option value="' + obj.assetItemList[i].value + '">' + obj.assetItemList[i].text + '</option>');
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    }
}

function CalculateArray(columnname) {
    var Total = 0;
    if (columnname == "VatableAmount") {
        for (var i = 0; i < dataArr.length; i++) {
            Total = (parseFloat(Total) + parseFloat(dataArr[i].vatableAmount)).toFixed(2);
        }
    }
    else if (columnname == "NonvatableAmount") {
        for (var i = 0; i < dataArr.length; i++) {
            Total = (parseFloat(Total) + parseFloat(dataArr[i].nonVatableAmount)).toFixed(2);
        }
    }
    else if (columnname == "TaxAmount") {
        for (var i = 0; i < dataArr.length; i++) {
            Total = (parseFloat(Total) + parseFloat(dataArr[i].taxAmount)).toFixed(2);
        }
    }
    else if (columnname == "Total") {
        for (var i = 0; i < dataArr.length; i++) {
            Total = (parseFloat(Total) + parseFloat(dataArr[i].totalAmount)).toFixed(2);
        }
    }

    return Total;
}
