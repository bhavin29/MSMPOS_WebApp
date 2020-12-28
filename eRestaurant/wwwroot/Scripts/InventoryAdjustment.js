var InventoryAdjustmentDatatable;


$(document).ready(function () {

    $("#InventoryAdjustment").validate();
    InventoryAdjustmentDatatable = $('#InventoryAdjustmentOrderDetail').DataTable({
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
                "Data": "", "name": "Action", "defaultContent": '<a href="#" class="deleteItem">Delete Record</a>', "autoWidth": true
            }
           ]
    });
});

$('#addRow').on('click', function (e) {
  
    e.preventDefault();
    var message = validation();
    if (message == '') {
        
        InventoryAdjustmentDatatable.row('.active').remove().draw(false);
        InventoryAdjustmentDatatable.row.add([
            $("#IngredientId").val(),
            $('#IngredientId').children("option:selected").text(),
            $("#Quantity").val(),
            $("#ConsumpationStatus").val(),
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#IngredientId").val() + '" class="btn btn-link editItem"><i class="fa fa-edit"></i></a><a href="#" class="btn btn-link btn-danger" data-toggle="modal" data-target="#myModal0"><i class="fa fa-times"></i></a></div></td > ' +
            '<div class="modal fade" id=myModal0 tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#IngredientId").val() + '" onclick="deleteOrder(0, ' + $("#IngredientId").val() + ',0)" data-dismiss="modal" class="btn bg-danger mr-1">Delete</a><button type="button" class="btn btn-default" data-dismiss="modal">Close</button></div></div></div ></div >',
            $("#InventoryAdjustmentId").val()
        ]).draw(false);
       
        dataArr.push({
            ingredientId: $("#IngredientId").val(),
            quantity: $("#Quantity").val(),
            ConsumpationStatus: $("#ConsumpationStatus").val(),
            InventoryAdjustmentId: $("#InventoryAdjustmentId").val()
        });
        clearItem();
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
        url: 'InventoryAdjustment',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        if (!InventoryAdjustment.data().any() || InventoryAdjustment.data().row == null) {
            var message = 'No data available!'
            $(".modal-body").text(message);
            $("#save").hide();
            jQuery.noConflict();
            $("#aModal").modal('show');
        }
        else {
            $("#InventoryAdjustment").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    ReferenceNo: $("#ReferenceNo").val(),
                    StoreId: $("#StoreId").val(),
                    EmployeeId: $("#EmployeeId").val(),
                    Date: $("#Date").val(),
                    Notes: $("#Notes").val(),
                    SupplierList: [],
                    EmployeeList: [],
                    IngredientList: [],
                    InventoryAdjustmentDetails: dataArr
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
    window.location.href = "/InventoryAdjustment/InventoryAdjustmentList";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

function deleteOrderItem(id) {
    debugger;
    return $.ajax({
        dataType: "json",
        //contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        type: "GET",
        // beforeSend: function (xhr) { xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val()); },
        url: "/InventoryAdjustment/DeleteInventoryAdjustmentDetails",
        data: "InventoryAdjustmentId=" + id
        //headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
}

function deleteOrder(purchaseId, ingredientId, rowId) {
    var id = ingredientId;
    if (purchaseId == "0") {
        for (var i = 0; i < dataArr.length; i++) {
            if (dataArr[i].ingredientId == id) {
                dataArr.splice(i, 1);
                InventoryAdjustmentDatatable.row(rowId).remove().draw(false);
                jQuery.noConflict();
                $("#myModal0").modal('hide');
            }
        }
    }
    else {
        $.when(deleteOrderItem(InventoryAdjustmentId).then(function (res) {
            console.log(res)
            if (res.status == 200) {
                debugger;
                for (var i = 0; i < dataArr.length; i++) {
                    if (dataArr[i].ingredientId == id) {
                        dataArr.splice(i, 1);
                        InventoryAdjustmentDatatable.row(rowId).remove().draw(false);
                        jQuery.noConflict();
                        $("#myModal2" + rowId).modal('hide');
                    }
                }
                console.log(res);
            }
        }).fail(function (err) {
            console.log(err);
        }));
    }
};

$(document).on('click', 'a.editItem', function (e) {
    debugger;
    if (!InventoryAdjustmentDatatable.data().any() || InventoryAdjustmentDatatable.data().row == null) {
        var message = 'No data available!'
        $(".modal-body").text(message);
        $("#save").hide();
        jQuery.noConflict();
        $("#aModal").modal('show');
    }
    else {
        e.preventDefault();
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
                $("#IngredientId").val(dataArr[i].ingredientId),
                    $("#Quantity").val(dataArr[i].quantity),
                    $("#ConsumpationStatus").val(dataArr[i].ConsumpationStatus),
                    $("#InventoryAdjustmentId").val(dataArr[i].InventoryAdjustmentId);
                dataArr.splice(i, 1);
                $(this).remove();
            }
        }
    }
});

function validation() {
    var message = '';

    if ($("#IngredientId").val() == '' || $("#IngredientId").val() == '0') {
        message = "Select ingredient"
    }

    else if ($("#ConsumpationStatus").val() == '' || $("#ConsumpationStatus").val() == 0) {
        message = "Select Comsumption Status"
    }
    else if ($("#Quantity").val() == '' || $("#Quantity").val() == 0) {
        message = "Enter quantity"
    }

    for (var i = 0; i < dataArr.length; i++) {
        if ($("#IngredientId").val() == dataArr[i].ingredientId) {
            message = "Ingredient already selected!"
            break;
        }
     }
    return message;
}

function clearItem() {
    $("#IngredientId").val(''),
        $("#ConsumpationStatus").val(''),
        $("#Quantity").val(''),
        $("#InventoryAdjustmentId").val('0')
}