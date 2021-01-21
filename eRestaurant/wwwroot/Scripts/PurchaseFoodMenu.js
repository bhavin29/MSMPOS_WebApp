var PurchaseDatatable;
var editDataArr = [];
var deletedId = [];

$(document).ready(function () {
    $("#purchase").validate();
    PurchaseDatatable = $('#PurchaseOrderDetails').DataTable({
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
                "targets": [5],
                "Data": "", "name": "Action", "defaultContent": '<a href="#" class="deleteItem">Delete Order</a>', "autoWidth": true
            }
            ,
            {
                "targets": [6],
                "visible": false,
                "searchable": false
            }
        ]
    });
});

$('#cancel').on('click', function (e) {
    debugger;
    e.preventDefault();
    GrandTotal += editDataArr[0].total;
    $("#GrandTotal").val(parseFloat(GrandTotal).toFixed(4));
    DueAmount();
    clearItem();
    dataArr.push(editDataArr[0]);
    editDataArr = [];
});

$('#addRow').on('click', function (e) {
    e.preventDefault();
    var message = validation(0);

    var Qty = parseFloat($("#Quantity").val()).toFixed(4);
    var UnitPrice = parseFloat($("#UnitPrice").val()).toFixed(4);
    var Total = parseFloat($("#UnitPrice").val() * $("#Quantity").val()).toFixed(4);

    if (message == '') {
        PurchaseDatatable.row('.active').remove().draw(false);
        var rowNode = PurchaseDatatable.row.add([
            '<td class="text-right">' + $("#FoodMenuId").val() + ' </td>',
            $('#FoodMenuId').children("option:selected").text(),
            '<td class="text-right">' + UnitPrice + ' </td>',
            '<td class="text-right">' + Qty + ' </td>',
            '<td class="text-right">' + Total + ' </td>',
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#FoodMenuId").val() + '" class="btn btn-link editItem"><i class="fa fa-edit"></i></a><a href="#" class="btn btn-link btn-danger" data-toggle="modal" data-target="#myModal0"><i class="fa fa-times"></i></a></div></td > ' +
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#FoodMenuId").val() + '" onclick="deleteOrder(0, ' + $("#FoodMenuId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-default" data-dismiss="modal">Close</button></div></div></div ></div >',
            $("#PurchaseId").val()
        ]).draw(false).nodes();
        dataArr.push({
            foodMenuId: $("#FoodMenuId").val(),
            unitPrice: UnitPrice,
            quantity: Qty,
            total: Total,
            purchaseId: $("#PurchaseId").val()
        });
        $(rowNode).find('td').eq(1).addClass('text-right');
        $(rowNode).find('td').eq(2).addClass('text-right');
        $(rowNode).find('td').eq(3).addClass('text-right');
        GrandTotal += $("#UnitPrice").val() * $("#Quantity").val();
        $("#GrandTotal").val(parseFloat(GrandTotal).toFixed(4));
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
        url: 'PurchaseFoodMenu',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        var message = validation(1);
        if (message == '') {
            $("#purchase").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    SupplierId: $("#SupplierId").val(),
                    StoreId: $("#StoreId").val(),
                    EmployeeId: $("#EmployeeId").val(),
                    Date: $("#Date").val(),
                    GrandTotal: $("#GrandTotal").val(),
                    Due: $("#Due").val(),
                    Paid: $("#Paid").val(),
                    Notes: $("#Notes").val(),
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
    window.location.href = "/PurchaseFoodMenu/PurchaseFoodMenuList";
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
            $("#GrandTotal").val(GrandTotal);
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
                    $("#UnitPrice").val(dataArr[i].unitPrice),
                    $("#Quantity").val(dataArr[i].quantity),
                    $("#PurchaseId").val(dataArr[i].purchaseId)
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
        else if ($("#Quantity").val() == '' || $("#Quantity").val() == 0) {
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
        $("#Quantity").val(''),
        $("#Amount").val(''),
        $("#PurchaseId").val('0')
}