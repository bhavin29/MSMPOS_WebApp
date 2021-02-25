var PurchaseDatatable;
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
    var supId = $("#SupplierId").val();

    if (supId != null && supId != 0) {
        GetSupplierDetailsById(supId);
    }
    debugger;
    $("#purchaseInvoice").validate();

    PurchaseDatatable = $('#purchaseOrderDetails').DataTable({
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
                "targets": [6, 7,11],
                "visible": false,
                "searchable": false
            }
        ]
    });

    $("#StoreId").focus();

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
    debugger;
    e.preventDefault();
    var message = validation(0);
    var DiscountPercentage = 0;
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
    var POQty = parseFloat($("#POQty").val()).toFixed(2);
    var InvoiceQty = parseFloat($("#InvoiceQty").val()).toFixed(2);
    DiscountPercentage = parseFloat($("#DiscountPercentage").val()).toFixed(2);

    var UnitPrice = parseFloat($("#UnitPrice").val()).toFixed(2);
    var Total = parseFloat($("#UnitPrice").val() * $("#InvoiceQty").val()).toFixed(2);
    debugger;
    POQty = getNum(POQty);
    InvoiceQty = getNum(InvoiceQty);
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

    if (TaxPercentage > 0) {
        TaxAmount = ((parseFloat(Total) * parseFloat(TaxPercentage)) / 100).toFixed(2);
        // Total = (parseFloat(Total) + parseFloat(TaxAmount)).toFixed(2);
    }
    debugger;
    if (message == '') {
        PurchaseDatatable.row('.active').remove().draw(false);

        var rowNode = PurchaseDatatable.row.add([
            '<td class="text-right">' + $("#FoodMenuId").val() + ' </td>',
            $('#FoodMenuId').children("option:selected").text(),
            '<td class="text-right">' + POQty + ' </td>',
            '<td class="text-right">' + InvoiceQty + ' </td>',
            '<td class="text-right">' + UnitPrice + ' </td>',
            '<td class="text-right">' + DiscountPercentage + ' </td>',
            '<td class="text-right">' + DiscountAmount + ' </td>',
            '<td class="text-right">' + TaxPercentage + ' </td>',
            '<td class="text-right">' + TaxAmount + ' </td>',
            '<td class="text-right">' + Total + ' </td>',
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#FoodMenuId").val() + '"" ></a><a href="#" data-toggle="modal" data-target="#myModal0">Delete</a></div></td > ' +
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteOrder(0, ' + $("#FoodMenuId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
            $("#PurchaseInvoiceId").val()
        ]).draw(false).nodes();
        dataArr.push({
            foodMenuId: $("#FoodMenuId").val(),
            poQty: POQty,
            invoiceQty: InvoiceQty,
            unitPrice: UnitPrice,
            discountAmount: DiscountAmount,
            discountPercentage: DiscountPercentage,
            grossAmount: InvoiceQty * UnitPrice,
            taxAmount: TaxAmount,
            taxPercentage: TaxPercentage,
            totalAmount: Total,
            purchaseInvoiceId: $("#PurchaseInvoiceId").val(),
            purchaseStatus: $("#PurchaseStatus").val()
        });
        $(rowNode).find('td').eq(1).addClass('text-right');
        $(rowNode).find('td').eq(2).addClass('text-right');
        $(rowNode).find('td').eq(3).addClass('text-right');
        $(rowNode).find('td').eq(4).addClass('text-right');
        $(rowNode).find('td').eq(5).addClass('text-right');
        $(rowNode).find('td').eq(6).addClass('text-right');

        DisAmtTotal = calculateColumn(4);
        TaxAmountTotal = calculateColumn(5);
        GrossAmount = calculateGross();
        TotalAmount = GrossAmount - DisAmtTotal;

        $("#GrossAmount").val(parseFloat(GrossAmount).toFixed(2));
        $("#TaxAmount").val(parseFloat(TaxAmountTotal).toFixed(2));
        $("#TotalAmount").val(parseFloat(TotalAmount).toFixed(2));

        DueAmount();
        clearItem();
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

function saveOrder(data) {
    return $.ajax({
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        type: 'POST',
        beforeSend: function (xhr) { xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val()); },
        url: 'PurchaseInvoiceFoodMenu',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {

        debugger;
        var message = validation(1);
        if (message == '') {
            $("#purchaseInvoice").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    purchaseId: $("#PurchaseId").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    StoreId: $("#StoreId").val(),
                    SupplierId: $("#SupplierId").val(),
                    SupplierEmail: $("#SupplierEmail").val(),
                    IsSendEmail: $("#IsSendEmail").is(":checked") ? "true" : "false",
                    GrossAmount: $("#GrossAmount").val(),
                    TaxAmount: $("#TaxAmount").val(),
                    TotalAmount: $("#TotalAmount").val(),
                    DeliveryNoteNumber: $("#DeliveryNoteNumber").val(),
                    DeliveryDate: $("#DeliveryDate").val(),
                    PurchaseInvoiceDate: $("#PurchaseInvoiceDate").val(),
                    DriverName: $("#DriverName").val(),
                    VehicleNumber: $("#VehicleNumber").val(),
                    Notes: $("#Notes").val(),
                    purchaseStatus: $("#PurchaseStatus").val(),
                    SupplierList: [],
                    FoodMenuList: [],
                    PurchaseInvoiceDetails: dataArr,
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
            $("#purchaseInvoice").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    purchaseId: $("#PurchaseId").val(),
                    SupplierId: $("#SupplierId").val(),
                    //SupplierEmail: $("#SupplierEmail").val(),
                    //IsSendEmail: $("#IsSendEmail").is(":checked") ? "true" : "false",
                    StoreId: $("#StoreId").val(),
                    EmployeeId: $("#EmployeeId").val(),
                    Date: $("#PurchaseInvoiceDate").val(),
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

    GrossAmount = $("#GrossAmount").val();
    TaxAmountTotal = $("#TaxAmount").val();
    TotalAmount = $("#TotalAmount").val();


    for (var i = 0; i < dataArr.length; i++) {
        if (dataArr[i].foodMenuId == id) {
            Gross = dataArr[i].unitPrice * dataArr[i].Invoiceqty;
            Tax = dataArr[i].taxAmount;
            Total = dataArr[i].totalAmount;
            debugger;
            GrossAmount -= Gross;
            TaxAmountTotal -= Tax;
            TotalAmount -= Total;

            $("#GrossAmount").val(parseFloat(GrossAmount).toFixed(2));
            $("#TaxAmount").val(parseFloat(TaxAmountTotal).toFixed(2));
            $("#TotalAmount").val(parseFloat(TotalAmount).toFixed(2));

            deletedId.push(dataArr[i].purchaseInvoiceId);
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
                    $("#InvoiceQty").val(dataArr[i].quantity),
                    $("#UnitPrice").val(dataArr[i].unitPrice),
                    $("#DiscountPercentage").val(dataArr[i].DiscountPercentage),
                    $("#DiscountAmounts").val(dataArr[i].Discount),
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
        else if ($("#InvoiceQty").val() == '' || $("#InvoiceQty").val() == 0) {
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
        $("#InvoiceQty").val('1'),
        $("#DiscountPercentage").val(''),
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
    $('#purchaseOrderDetails tbody tr').each(function () {
        var value = parseFloat($('td', this).eq(index).text());
        if (!isNaN(value)) {
            total += value;
        }
    });
    return total;
}

function calculateGross() {
    var total = 0;
    $('#purchaseOrderDetails tbody tr').each(function () {
        var Invoiceqty = parseFloat($('td', this).eq(2).text());
        var Price = parseFloat($('td', this).eq(3).text());
        if (!isNaN(Price)) {
            total += (Invoiceqty * Price);
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


function GetPurchaseInvoicebyPO(poReference) {
    poReference = $("#POReference").val();
    $.ajax({
        url: "/PurchaseInvoiceFoodMenu/GetPurchaseIdByPOReference",
        data: { "poReference": poReference },
        type: "GET",
        dataType: "text",
        success: function (data) {
            var obj = JSON.parse(data);
            if (obj.purchaseId > 0) {
                window.location.href = "/PurchaseInvoiceFoodMenu/PurchaseInvoiceFoodMenu?purchaseId=" + obj.purchaseId;
            }
            else {
                alert("Reference Number Not Found!");
            }
        },
        error: function (data) {
            alert(data);
        }
    });
    debugger;
}
