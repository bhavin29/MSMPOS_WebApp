﻿var PurchaseDatatable;
var editDataArr = [];
var deletedId = [];
var DisPerTotal = 0;
var DisAmtTotal = 0;
var TaxPerTotal = 0;
var TaxAmtTotal = 0;
var GTotal = 0;
var Status = 0;


$(document).ready(function () {
    var supId = $("#SupplierId").val();
    if (supId != null && supId != 0) {
        GetSupplierDetailsById(supId);
    }
    $("#purchaseInvoice").validate();
    PurchaseDatatable = $('#PurchaseInvoiceDetails').DataTable({
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
                "targets": [5],
                "visible": false,
                "searchable": false
            }
        ]
    });
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
    var Discount = 0;
    var DiscountAmount = 0;
    var TaxPercentage = 0;
    var TaxAmount = 0;
    var foodMenuId = $("#FoodMenuId").val();
    $.ajax({
        url: "/PurchaseInvoiceFoodMenu/GetTaxByFoodMenuId",
        data: { "foodMenuId": foodMenuId },
        async: false,
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            TaxPercentage = obj.taxPercentage;
            TaxPercentage = parseFloat(TaxPercentage).toFixed(2);
        }
    });
    debugger;
    var POQty = parseFloat($("#POQty").val()).toFixed(2);
    var GRNQty = parseFloat($("#GRNQty").val()).toFixed(2);
    DiscountPer = parseFloat($("#DiscountPercentage").val()).toFixed(2);
    var UnitPrice = parseFloat($("#UnitPrice").val()).toFixed(2);
    var Total = parseFloat($("#UnitPrice").val() * $("#GRNQty").val()).toFixed(2);
    
    POQty = getNum(POQty);
    GRNQty = getNum(GRNQty);
    DiscountPer = getNum(DiscountPer);
    TaxPercentage = getNum(TaxPercentage);
    UnitPrice = getNum(UnitPrice);
    Total = getNum(Total);

    if (DiscountPer > 0) {
        DiscountAmount = ((parseFloat(Total) * parseFloat(DiscountPer)) / 100).toFixed(2);
        DisAmtTotal = parseFloat(parseFloat(DiscountAmount)+parseFloat($("#DiscountAmount").val())).toFixed(2);
        Total = (parseFloat(Total) - parseFloat(DiscountAmount)).toFixed(2);
    }
    if (TaxPercentage > 0) {
        TaxAmount = ((parseFloat(Total) * parseFloat(TaxPercentage)) / 100).toFixed(2);
        Total = (parseFloat(Total) + parseFloat(TaxAmount)).toFixed(2);
    }

    if (message == '') {
        PurchaseDatatable.row('.active').remove().draw(false);
        var rowNode = PurchaseDatatable.row.add([
            '<td class="text-right">' + $("#FoodMenuId").val() + ' </td>',
            $('#FoodMenuId').children("option:selected").text(),
            '<td class="text-right">' + POQty + ' </td>',
            '<td class="text-right">' + GRNQty + ' </td>',
            '<td class="text-right">' + UnitPrice + ' </td>',
            '<td class="text-right">' + DiscountPercentage + ' </td>',
            '<td class="text-right">' + DiscountAmount + ' </td>',
            //'<td class="text-right">' + TaxPercentage + ' </td>',
            '<td class="text-right">' + TaxAmount + ' </td>',
            '<td class="text-right">' + Total + ' </td>',
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#FoodMenuId").val() + '" class="btn btn-link editItem"></a><a href="#" data-toggle="modal" data-target="#myModal0">Delete</a></div></td > ' +
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteOrder(0, ' + $("#FoodMenuId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
            $("#PurchaseInvoiceId").val()
        ]).draw(false).nodes();
        dataArr.push({
            foodMenuId: $("#FoodMenuId").val(),
            POquantity: POQty,
            GRNquantity: GRNQty,
            unitPrice: UnitPrice,
            discountAmount: DiscountAmount,
            discountPercentage: DiscountPercentage,
            taxAmount: TaxAmount,
            taxPercentage: TaxPercentage,
            total: Total,
            purchaseId: $("#PurchaseInvoiceId").val()
        });
        $(rowNode).find('td').eq(1).addClass('text-right');
        $(rowNode).find('td').eq(2).addClass('text-right');
        $(rowNode).find('td').eq(3).addClass('text-right');
        $(rowNode).find('td').eq(4).addClass('text-right');
        $(rowNode).find('td').eq(5).addClass('text-right');
        $(rowNode).find('td').eq(6).addClass('text-right');
        $(rowNode).find('td').eq(6).addClass('text-right');
        //GrandTotal += $("#UnitPrice").val() * $("#Quantity").val();
        //GrandTotal += Total;
        debugger;
        //DisPerTotal = calculateColumn(3);
        DisAmtTotal = calculateColumn(4);
        //TaxPerTotal = calculateColumn(5);
        //TaxAmtTotal = calculateColumn(4);
        GTotal = calculateColumn(5);
        GrandTotal = GTotal;
        $("#DiscountAmount").val(parseFloat(DisAmtTotal).toFixed(2));
        $("#TaxAmount").val(parseFloat(TaxAmtTotal).toFixed(2));
        $("#GrandTotal").val(parseFloat(GrandTotal).toFixed(2));
        DueAmount();
        clearItem();
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

function saveOrder(data) {
    return $.ajax({
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        type: 'POST',
        beforeSend: function (xhr) { xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val()); },
        url: 'PurchaseInvoiceFoodMenu\PurchaseInvoiceFoodMenuList',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        debugger;
        var message = validation(1);
        if (message == '') {
            $("#purchaseInvoiceform").on("submit", function (e) {
                   debugger;
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    SupplierId: $("#SupplierId").val(),
                    //SupplierEmail: $("#SupplierEmail").val(),
                    //IsSendEmail: $("#IsSendEmail").is(":checked") ? "true" : "false",
                    StoreId: $("#StoreId").val(),
                    EmployeeId: $("#EmployeeId").val(),
                    Date: $("#Date").val(),
                    GrandTotal: $("#GrandTotal").val(),
                    TaxAmount: $("#TaxAmount").val(),
                    DiscountAmount: $("#DiscountAmount").val(),
                    Due: $("#DueAmount").val(),
                    Paid: $("#PaidAmount").val(),
                    Notes: $("#Notes").val(),
                    Status: 1,
                    SupplierList: [],
                    FoodMenuList: [],
                    PurchaseDetails: dataArr,
                    DeletedId: deletedId
                });
                $.when(saveOrder(data)).then(function (response) {
                    debugger;
                    if (response.status == "200") {
                        debugger;
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

$(function () {
    $('#saveAndApprovedOrder').click(function () {
        var message = validation(1);
        if (message == '') {
            $("#purchaseInvoice").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    SupplierId: $("#SupplierId").val(),
                    //SupplierEmail: $("#SupplierEmail").val(),
                    //IsSendEmail: $("#IsSendEmail").is(":checked") ? "true" : "false",
                    StoreId: $("#StoreId").val(),
                    EmployeeId: $("#EmployeeId").val(),
                    Date: $("#PurchaseInvoiceDate").val(),
                    GrandTotal: $("#GrossTotal").val(),
                    TaxAmount: $("#TaxAmount").val(),
                    DiscountAmount: $("#DiscountAmount").val(),
                    Due: $("#DueAmount").val(),
                    Paid: $("#PaidAmount").val(),
                    Notes: $("#Notes").val(),
                    Status: 2,
                    SupplierList: [],
                    FoodMenuList: [],
                    PurchaseDetails: dataArr,
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
    window.location.href = "/PurchaseInvoiceFoodMenu/PurchaseInvoiceFoodMenuList";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

function deleteOrder(purchaseId, foodMenuId, rowId) {
    var id = foodMenuId;
    for (var i = 0; i < dataArr.length; i++) {
        if (dataArr[i].foodMenuId == id) {
            TotalAmount = dataArr[i].total;
            GrandTotal -= TotalAmount;
            $("#GrossAmount").val(GrandTotal);
            DueAmount();
            deletedId.push(dataArr[i].purchaseId);
            dataArr.splice(i, 1);
            PurchaseDatatable.row(rowId).remove().draw(false);
            jQuery.noConflict();
            $("#myModal" + purchaseId).modal('hide');
        }
    }
};

$(document).on('click', 'a.editItem', function (e) {
    if (!PurchaseDatatable.data().any() || PurchaseDatatable.data().row == null) {
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
                    $("#Discount").val(dataArr[i].Discount),
                    $("#PurchaseInvoiceId").val(dataArr[i].purchaseId)
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
    if ($("#SupplierId").val() == '' || $("#SupplierId").val() == '0') {
        message = "Select Supplier"
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
        if (!PurchaseDatatable.data().any() || PurchaseDatatable.data().row == null) {
            var message = 'At least one order should be entered'
            return message;
        }
    }
    else {
        if ($("#FoodMenuId").val() == '' || $("#FoodMenuId").val() == '0') {
            message = "Select foodMenu"
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
            if ($("#FoodMenuId").val() == dataArr[i].foodMenuId) {
                message = "FoodMenu already selected!"
                break;
            }
        }
    }
    return message;
}

function clearItem() {
    $("#FoodMenuId").val('0'),
        $("#UnitPrice").val(''),
        $("#POQty").val('1'),
        $("#GRNQty").val('1'),
        $("#Discount").val(''),
        $("#GrossAmount").val(''),
        $("#PurchaseInvoiceId").val('0')
}

function GetSupplierDetails(supplierId) {
    $.ajax({
        url: "/PurchaseInvoiceFoodMenu/GetSupplierDetail",
        data: { "supplierId": supplierId.value },
        type: "GET",
        dataType: "text",
        success: function (data) {
            $("#FoodMenuId").empty();
            var obj = JSON.parse(data);
            for (var i = 0; i < obj.foodMenuList.length; ++i) {
                $("#FoodMenuId").append('<option value="' + obj.foodMenuList[i].value + '">' + obj.foodMenuList[i].text + '</option>');
            }
            $("#SupplierEmail").val(obj.email);
        },
        error: function (data) {
            alert(data);
        }
    });
}

function GetSupplierDetailsById(supplierId) {
    $.ajax({
        url: "/PurchaseInvoiceFoodMenu/GetSupplierDetail",
        data: { "supplierId": supplierId },
        type: "GET",
        dataType: "text",
        success: function (data) {
            $("#FoodMenuId").empty();
            var obj = JSON.parse(data);
            for (var i = 0; i < obj.foodMenuList.length; ++i) {
                $("#FoodMenuId").append('<option value="' + obj.foodMenuList[i].value + '">' + obj.foodMenuList[i].text + '</option>');
            }
            $("#SupplierEmail").val(obj.email);
        },
        error: function (data) {
            alert(data);
        }
    });
}

$('#chkAllFoodMenu').change(function () {
    if ($(this).is(":checked")) {
        $.ajax({
            url: "/PurchaseInvoiceFoodMenu/GetFoodMenuList",
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
});

function getNum(val) {
    val = +val || 0
    return val.toFixed(2);
}

function calculateColumn(index) {
    var total = 0;
    $('#PurchaseInvoiceDetails tbody tr').each(function () {
        var value = parseFloat($('td', this).eq(index).text());
        if (!isNaN(value)) {
            total += value;
        }
    });
    return total;
}

function GetFoodMenuLastPrice(foodMenuId) {
    $.ajax({
        url: "/PurchaseInvoiceFoodMenu/GetFoodMenuLastPrice",
        data: { "foodMenuId": foodMenuId.value },
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
