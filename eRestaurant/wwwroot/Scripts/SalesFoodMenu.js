var SalesDatatable;
var editDataArr = [];
var deletedId = [];
var DisPerTotal = 0;
var DisAmtTotal = 0;
var TaxPerTotal = 0;
var TaxAmtTotal = 0;
var GTotal = 0;
var GrossAmount = 0;
var TaxAmountTotal = 0;
var TotalAmount = 0
var Status = 0;
var RowNumber = 0;

$(document).ready(function () {
    GetFoodMenuByItemType();
    //var supId = $("#CustomerId").val();
    //if (supId != null && supId != 0) {
    //    GetSupplierDetailsById(supId);
    //}
    $("#sales").validate();
    SalesDatatable = $('#SalesDetails').DataTable({
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
                "targets": [4, 6, 9, 10, 12, 13, 14],
                "visible": false,
                "searchable": false
            }
        ]
    });

    RowNumber = dataArr.length;

    $('#TaxType').hide();
    $("#CustomerId").select2();
    $("#FoodMenuId").select2();
    $("#StoreId").select2();
    $("#StoreId").focus();
});

$('#cancel').on('click', function (e) {
    e.preventDefault();
    GrandTotal += editDataArr[0].total;
    $("#GrandTotal").val(parseFloat(GrandTotal).toFixed(2));
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
    var itemType = $("#ItemType").val();
    var foodMenuId = $("#FoodMenuId").val();
    var foodMenuIdSelect = $("#FoodMenuId").val();
    var recordid = $("#ItemType").val() + 'rowId' + $("#FoodMenuId").val();

    $.ajax({
        url: "/Sales/GetTaxByFoodMenuId",
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

    var Qty = parseFloat($("#Quantity").val()).toFixed(2);
    var UnitPrice = parseFloat($("#UnitPrice").val()).toFixed(2);
    var Total = parseFloat($("#UnitPrice").val() * $("#Quantity").val()).toFixed(2);
    DiscountPercentage = parseFloat($("#Discount").val()).toFixed(2);

    Qty = getNum(Qty);
    DiscountPercentage = getNum(DiscountPercentage);
    TaxPercentage = getNum(TaxPercentage);
    UnitPrice = getNum(UnitPrice);
    Total = getNum(Total);
    debugger;
    if (DiscountPercentage > 0) {
        DiscountAmount = ((parseFloat(Total) * parseFloat(DiscountPercentage)) / 100).toFixed(2);
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
        SalesDatatable.row('.active').remove().draw(false);
        var rowNode = SalesDatatable.row.add([
            '<td class="text-right">' + $("#FoodMenuId").val() + ' </td>',
            $('#FoodMenuId').children("option:selected").text(),
            '<td class="text-right">' + Qty + ' </td>',
            '<td class="text-right">' + UnitPrice + ' </td>',
            '<td class="text-right">' + DiscountPercentage + ' </td>',
            '<td class="text-right">' + DiscountAmount + ' </td>',
            '<td class="text-right">' + TaxPercentage + ' </td>',
            '<td class="text-right">' + TaxAmount + ' </td>',
            '<td class="text-right">' + Total + ' </td>',
            '<td class="text-right">' + Vatable + ' </td>',
            '<td class="text-right">' + Nonvatable + ' </td>',
            '<td><div class="form-button-action"><a href="#" data-itemId="' + recordid + '></a><a href="#" data-toggle="modal" data-target="#myModal' + $("#ItemType").val() + '' + $("#FoodMenuId").val() + '">Delete</a></div></td > ' +
            '<div class="modal fade" id="myModal' + $("#ItemType").val() + '' + $("#FoodMenuId").val() + '" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteOrder(\'' + recordid + '\')" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
            $("#SalesId").val(),
            $("#ItemType").val(),
            recordid
        ]).node().id = recordid;
        SalesDatatable.draw(false);

        dataArr.push({
            foodMenuId: $("#FoodMenuId").val(),
            unitPrice: UnitPrice,
            quantity: Qty,
            discountAmount: DiscountAmount,
            discountPercentage: DiscountPercentage,
            taxAmount: TaxAmount,
            taxPercentage: TaxPercentage,
            total: Total,
            vatableAmount: Vatable,
            nonVatableAmount: Nonvatable,
            salesId: $("#SalesId").val(),
            itemType: $("#ItemType").val(),
            rowNumber: RowNumber
        });

        var attRowNode = '#' + recordid;

        $(attRowNode).find('td').eq(1).addClass('text-right');
        $(attRowNode).find('td').eq(2).addClass('text-right');
        $(attRowNode).find('td').eq(3).addClass('text-right');
        $(attRowNode).find('td').eq(4).addClass('text-right');
        $(attRowNode).find('td').eq(5).addClass('text-right');


        DisAmtTotal = calculateDiscountColumn(4);
        TaxAmountTotal = CalculateArray('TaxAmount');
        VatableTotal = CalculateArray('VatableAmount');
        NonvatableTotal = CalculateArray('NonvatableAmount');
        TotalAmount = CalculateArray('Total');
        GrossAmount = TotalAmount - TaxAmountTotal;

        $("#GrossAmount").val(parseFloat(GrossAmount).toFixed(2));
        $("#TaxAmount").val(parseFloat(TaxAmountTotal).toFixed(2));
        $("#GrandTotal").val(parseFloat(TotalAmount).toFixed(2));
        $("#VatableAmount").val(parseFloat(VatableTotal).toFixed(2));
        $("#NonVatableAmount").val(parseFloat(NonvatableTotal).toFixed(2));

        DueAmount();
        clearItem();
        $("#ItemType").focus();
        RowNumber += 1;
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
        url: 'SalesFoodMenu',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        var message = validation(1);
        if (message == '') {
            $("#sales").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    CustomerId: $("#CustomerId").val(),
                    SupplierEmail: '',
                    IsSendEmail: "false",
                    StoreId: $("#StoreId").val(),
                    EmployeeId: $("#EmployeeId").val(),
                    Date: $("#Date").val(),
                    GrandTotal: $("#GrandTotal").val(),
                    TaxAmount: $("#TaxAmount").val(),
                    DiscountAmount: $("#DiscountAmount").val(),
                    VatableAmount: $("#VatableAmount").val(),
                    NonVatableAmount: $("#NonVatableAmount").val(),
                    Due: $("#Due").val(),
                    Paid: $("#Paid").val(),
                    Notes: $("#Notes").val(),
                    Status: 1,
                    SupplierList: [],
                    FoodMenuList: [],
                    SalesDetails: dataArr,
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
            $("#sales").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    CustomerId: $("#CustomerId").val(),
                    SupplierEmail: '',
                    IsSendEmail: "false",
                    StoreId: $("#StoreId").val(),
                    EmployeeId: $("#EmployeeId").val(),
                    Date: $("#Date").val(),
                    GrandTotal: $("#GrandTotal").val(),
                    TaxAmount: $("#TaxAmount").val(),
                    DiscountAmount: $("#DiscountAmount").val(),
                    Due: $("#Due").val(),
                    Paid: $("#Paid").val(),
                    Notes: $("#Notes").val(),
                    Status: 2,
                    SupplierList: [],
                    FoodMenuList: [],
                    SalesDetails: dataArr,
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
    window.location.href = "/Sales/SalesFoodMenuList";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

$(document).on('click', 'a.editItem', function (e) {
    if (!SalesDatatable.data().any() || SalesDatatable.data().row == null) {
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
                    $("#UnitPrice").val(dataArr[i].unitPrice),
                    $("#Quantity").val(dataArr[i].quantity),
                    $("#Discount").val(dataArr[i].Discount),
                    $("#SalesId").val(dataArr[i].salesId),
                    $("#ItemType").val(dataArr[i].itemType),
                    $("#VatableAmount").val(dataArr[i].VatableAmount),
                    $("#NonVatableAmount").val(dataArr[i].NonVatableAmount),

                    GrandTotal = $("#GrandTotal").val();
                TotalAmount = dataArr[i].total;
                GrandTotal -= TotalAmount;
                $("#GrandTotal").val(GrandTotal);
                DueAmount();
                editDataArr = dataArr.splice(i, 1);
            }
        }
    }
});

$("#Paid").blur(function () {
    DueAmount();
});

function DueAmount() {
    var Due = 0
    Due = (GrandTotal - parseFloat($("#Paid").val()));
    $("#Due").val(Due);
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
        if (!SalesDatatable.data().any() || SalesDatatable.data().row == null) {
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
            message = "Enter price"
            return message;
        }
        else if ($("#Quantity").val() == '' || $("#Quantity").val() == 0) {
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
        $("#Quantity").val('1'),
        $("#Discount").val(''),
        $("#Amount").val(''),
        $("#SalesId").val('0'),
        $('#FoodMenuId').val(0).trigger('change');
}

function getNum(val) {
    val = +val || 0
    return val.toFixed(2);
}

function calculateColumn(index) {
    var total = 0;
    $('#SalesDetails tbody tr').each(function () {
        var value = parseFloat($('td', this).eq(index).text());
        if (!isNaN(value)) {
            total += value;
        }
    });
    return total;
}

function calculateDiscountColumn(index) {
    var totaldis = 0.00, discount = 0;
    $('#SalesDetails tbody tr').each(function () {
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
            DueAmount();
            deletedId.push(dataArr[i].salesId);
            dataArr.splice(i, 1);
            SalesDatatable.row('#' + rowId).remove().draw(false);
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
    $("#GrandTotal").val(parseFloat(TotalAmount).toFixed(2));
    $("#VatableAmount").val(parseFloat(VatableTotal).toFixed(2));
    $("#NonVatableAmount").val(parseFloat(NonvatableTotal).toFixed(2));

    $("#myModal" + salesId).modal('hide');

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
            Total = (parseFloat(Total) + parseFloat(dataArr[i].total)).toFixed(2);
        }
    }

    return Total;
}
