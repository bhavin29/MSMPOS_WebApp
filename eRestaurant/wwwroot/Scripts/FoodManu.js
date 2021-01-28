var FoodManuDetailsDatatable;
var editDataArr = [];
var deletedId = [];
$(document).ready(function () {
    $("#FoodMenu").validate();
    FoodManuDetailsDatatable = $('#FoodManuDetails').DataTable({
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
                "targets": [3],
                "Data": "", "name": "Action", "defaultContent": '<a href="#" class="deleteItem">Delete Record</a>', "autoWidth": true
            },
            {
                "targets": [4],
                "visible": false,
                "searchable": false
            }

        ]
    });
});

$('#cancel').on('click', function (e) {
    e.preventDefault();
    clearItem();
    dataArr.push(editDataArr[0]);
    editDataArr = [];
});

$('#addRow').on('click', function (e) {
    e.preventDefault();
    var message = validation(0);
    var rowId = "rowId" + $("#IngredientId").val();
    var Consumption = $("#Consumption").val();
    Consumption = parseFloat(Consumption).toFixed(4);
    if (message == '') {
        FoodManuDetailsDatatable.row('.active').remove().draw(false);
        var rowNode = FoodManuDetailsDatatable.row.add([
            $("#IngredientId").val(),
            $('#IngredientId').children("option:selected").text(),
            Consumption,
            '<td><div class="form-button-action"><a href="#" data-itemId="' + $("#IngredientId").val() + '" class="btn btn-link editItem"><i class="fa fa-edit"></i></a><a href="#" class="btn btn-link btn-danger" data-toggle="modal" data-target="#myModal"' + $("#IngredientId").val() + '"><i class="fa fa-times"></i></a></div></td > ' +
            '<div class="modal fade" id=myModal"' + $("#IngredientId").val() + '" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class= "modal-dialog" > <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="myModalLabel">Delete Confirmation</h4></div><div class="modal-body">' +
            'Are you want to delete this?</div><div class="modal-footer"><a id="deleteBtn" data-itemId="' + $("#IngredientId").val() + '" onclick="deleteOrder(' + $("#IngredientId").val() + ',' + rowId + ')" class="btn bg-danger mr-1" data-dismiss="modal">Delete</a><button type="button" class="btn btn-primary" data-dismiss="modal">Close</button></div></div></div ></div >',
            $("#FoodMenuId").val()
        ]).node().id = rowId;
        FoodManuDetailsDatatable.draw(false);
        dataArr.push({
            ingredientId: $("#IngredientId").val(),
            Consumption: $("#Consumption").val(),
            ingredientName: $('#IngredientId').children("option:selected").text(),
            foodMenuId: $("#FoodMenuId").val()
        });
        $(rowNode).find('td').eq(1).addClass('text-right');
        $(rowNode).find('td').eq(2).addClass('text-right');
        clearItem();
        editDataArr = [];
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

function saveOrder(data) {
    console.log(data);
    debugger;
    return $.ajax({
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        type: 'POST',
        beforeSend: function (xhr) { xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val()); },
        url: 'FoodMenu',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        debugger;
        var message = validation(0);
        if (message == '') {
            $("#foodMenu").on("submit", function (e) {
                debugger;
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    FoodCategoryId: $("#FoodCategoryId").val(),
                    FoodMenuName: $("#FoodMenuName").val(),
                    FoodMenuCode: $("#FoodMenuCode").val(),
                    ColourCode: $("#ColourCode").val(),
                    BigThumb: $("#BigThumb").val(),
                    MediumThumb: $("#MediumThumb").val(),
                    SmallThumb: $("#SmallThumb").val(),
                    SalesPrice: $("#SalesPrice").val(),
                    Notes: $("#Notes").val(),
                    IsVegItem: $("#IsVegItem").val(),
                    IsBeverages: $("#IsBeverages").val(),
                    FoodVat: $("#FoodVat").val(),
                    Foodcess: $("#Foodcess").val(),
                    OfferIsAvailable: $("#OfferIsAvailable").val(),
                    Position: $("#Position").val(),
                    OutletId: $("#OutletId").val(),
                    IsActive: $("#IsActive").val(),
                    Consumption: $("#Consumption").val(),
                    IngredientId: $("#IngredientId").val(),
                    IngredientList: [],
                    FoodCategoryList: [],
                    FoodMenuDetails: dataArr,
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

$('#save').click(function () {
    window.location.href = "Index";
});
$('#OK').click(function () {
    $("#FoodMenu").prev().focus();
    $("#vModal").modal('hide');
});

function deleteOrder(id, rowId) {
    for (var i = 0; i < dataArr.length; i++) {
        if (dataArr[i].ingredientId == id) {
            deletedId.push(dataArr[i].foodManuId);
            dataArr.splice(i, 1);
            FoodManuDetailsDatatable.row(rowId).remove().draw(false);
            jQuery.noConflict();
            $("#myModal" + id).modal('hide');
        }
    }
};

$(document).on('click', 'a.editItem', function (e) {
    if (!FoodManuDetailsDatatable.data().any() || FoodManuDetailsDatatable.data().row == null) {
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
                    $("#Consumption").val(dataArr[i].consumption),
                    $("#FoodMenuId").val(dataArr[i].foodMenuId);
                editDataArr = dataArr.splice(i, 1);
            }
        }
    }
});

function validation(id) {
    var message = '';

   
    if (id == 0) {
        if ($("#IngredientId").val() == '' || $("#IngredientId").val() == '0') {
            message = "Select ingredient"
        }
        else if ($("#Consumption").val() == '' || $("#Consumption").val() == 0) {
            message = "Enter Consumption"
        }

        for (var i = 0; i < dataArr.length; i++) {
            if ($("#IngredientId").val() == dataArr[i].ingredientId) {
                message = "Ingredient already selected!"
                break;
            }
        }
    }
    return message;
}

function clearItem() {
    $("#IngredientId").val('0'),
        $("#Consumption").val(''),
        $("#FoodMenuId").val('0')
}