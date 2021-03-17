var editFoodMenuDataArr = [];
var foodMenuDeletedId = [];
var editIngredientDataArr = [];
var ingredientDeletedId = [];
$(document).ready(function () {
    $("#productionEntryForm").validate();
    EntryIngredient = $('#EntryIngredient').DataTable({
        columnDefs: [
            { targets: [0], orderable: false, visible: false },
            { targets: [1,3], orderable: false, visible: false },
            { targets: [ 4], orderable: false, class: "text-right" },
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

    EntryFoodMenu = $('#EntryFoodMenu').DataTable({
        columnDefs: [
            { targets: [0], orderable: false, visible: false },
            { targets: [1,3], orderable: false, visible: false },
            { targets: [4], orderable: false, class: "text-right" }
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

    if ($("#FoodmenuType").val() == 2) {
        $("#onthespot").prop('checked', true);
    }
    else {
        $("#onthespot").prop('checked', false);
    }

    if ($("#AssetEventId").val() > 0) {
        document.getElementById("headerColorChange").style.backgroundColor = "#7FFF00";
    }

   // calculateSum();
    $("#StoreId").select2();
    $("#ProductionFormulaId").focus();
    $("#ProductionFormulaId").select2();
});

function loadProductionFormula() {
    window.location.href = "/ProductionEntry/ProductionEntry?productionFormulaId=" + $("#ProductionFormulaId").val() + "&foodMenuType=" + $("#FoodmenuType").val();
    $("#ProductionFormulaId").focus();
}

$("#ActualBatchSize").keyup(function () {

    var batchSize = $("#BatchSize").val();
    var actualBatchSize = $("#ActualBatchSize").val();
    var ingredientQty = 0;

    var i = 0;
    $("#EntryIngredient tbody tr").each(function () {
        var tds = $(this).find("td");
        var currentRow = $(this).closest("tr");
        var dataline = $('#EntryIngredient').DataTable().row(currentRow).data();

        var values = currentRow.find(":input").map(function () {
            return $(this).val()
        });

        ingredientQty = parseFloat(dataline[3]);

        actualIngredientQty = (ingredientQty * actualBatchSize) / batchSize;
        actualIngredientQty = parseFloat(actualIngredientQty).toFixed(2);

        var ingtextbox = '#i' + i;
        var ingAllotextbox = '#ia' + i;
        $(ingAllotextbox).val(actualIngredientQty);
        $(ingtextbox).val(actualIngredientQty);
        i = i + 1;
    });

    i = 0;
    $("#EntryFoodMenu tbody tr").each(function () {
        var tds = $(this).find("td");
        var currentRow = $(this).closest("tr");
        var datatrline = $('#EntryFoodMenu').DataTable().row(currentRow).data();
        var values = currentRow.find(":input").map(function () {
            return $(this).val()
        });

        foodmenuQty = parseFloat(datatrline[3]);
        actualfoodmenuQty = (foodmenuQty * actualBatchSize) / batchSize;
        actualfoodmenuQty = parseFloat(actualfoodmenuQty).toFixed(2);
        var foodtextbox = '#f' + i;
        var foodAllotextbox = '#fa' + i;

        $(foodAllotextbox).val(actualfoodmenuQty);
        $(foodtextbox).val(actualfoodmenuQty);
        i = i + 1;
   });

});

function calculateSum() {
    var subtotal = 0;
    $('.AllocationOutput').each(function () {
        var $this = $(this);
        var quantity = parseInt($this.val());
        subtotal += quantity;
    });

    var variation = "";
    if ($("#ActualBatchSize").val() != subtotal) {
        variation = "Actual Batch size is " + $("#ActualBatchSize").val() + " " + $("#BatchSizeUnitName").text() + " and allocation with " + subtotal + " " + $("#BatchSizeUnitName").text();
    }

    $("#VariationNotes").val(variation);
}

function saveOrder(data) {
    console.log(data);
    return $.ajax({
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        type: 'POST',
        beforeSend: function (xhr) { xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val()); },
        url: 'ProductionEntry',
        data: data,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
    });
};

$(function () {
    $('#saveOrder').click(function () {
        var message = validation(1);
        if (message == '') {

            $("#EntryIngredient tbody tr").each(function () {
                var tds = $(this).find("td");
                var currentRow = $(this).closest("tr");
                var dataline = $('#EntryIngredient').DataTable().row(currentRow).data();

                var values = currentRow.find(":input").map(function () {
                    return $(this).val()
                });

                ingredientDataArr.push({
                    peIngredientId: dataline[0],
                    ingredientId: dataline[1],
                    ingredientQty: dataline[3],
                    allocationIngredientQty: values[0],
                    actualIngredientQty: values[1]
                });
            });

            $("#EntryFoodMenu tbody tr").each(function () {
                var tds = $(this).find("td");
                var currentRow = $(this).closest("tr");
                var datatrline = $('#EntryFoodMenu').DataTable().row(currentRow).data();
                var values = currentRow.find(":input").map(function () {
                    return $(this).val()
                });

                foodMenuDataArr.push({
                    peFoodMenuId: datatrline[0],
                    foodMenuId: datatrline[1],
                    expectedOutput: datatrline[3],
                    allocationOutput: values[0],//$(currentRow).find("#allocateOut").val(),
                    actualOutput: values[1]
                });
            });

            $("#productionEntryForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    FoodmenuType: $("#FoodmenuType").val(),
                    ProductionFormulaId: $("#ProductionFormulaId").val(),
                    StoreId: $("#StoreId").val(),
                    ActualBatchSize: $("#ActualBatchSize").val(),
                    ProductionDate: $("#ProductionDate").val(),
                    Status: 1,
                    VariationNotes: $("#VariationNotes").val(),
                    Notes: $("#Notes").val(),
                    productionEntryFoodMenuModels: foodMenuDataArr,
                    productionEntryIngredientModels: ingredientDataArr
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
    $('#inProgress').click(function () {

        var message = validation(1);

        $("#EntryIngredient tbody tr").each(function () {
            var tds = $(this).find("td");
            var currentRow = $(this).closest("tr");
            var dataline = $('#EntryIngredient').DataTable().row(currentRow).data();

            var values = currentRow.find(":input").map(function () {
                return $(this).val()
            });

            ingredientDataArr.push({
                peIngredientId: dataline[0],
                ingredientId: dataline[1],
                ingredientQty: dataline[3],
                allocationIngredientQty: values[0],
                actualIngredientQty: values[1]
            });
        });

        $("#EntryFoodMenu tbody tr").each(function () {
            var tds = $(this).find("td");
            var currentRow = $(this).closest("tr");
            var datatrline = $('#EntryFoodMenu').DataTable().row(currentRow).data();
            var values = currentRow.find(":input").map(function () {
                return $(this).val()
            });

            foodMenuDataArr.push({
                peFoodMenuId: datatrline[0],
                foodMenuId: datatrline[1],
                expectedOutput: datatrline[3],
                allocationOutput: values[0],//$(currentRow).find("#allocateOut").val(),
                actualOutput: values[1]
            });
        });

        if (message == '') {
            $("#productionEntryForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    FoodmenuType: $("#FoodmenuType").val(),
                    ProductionFormulaId: $("#ProductionFormulaId").val(),
                    StoreId: $("#StoreId").val(),
                    ActualBatchSize: $("#ActualBatchSize").val(),
                    ProductionDate: $("#ProductionDate").val(),
                    Status: 2,
                    VariationNotes: $("#VariationNotes").val(),
                    Notes: $("#Notes").val(),
                    productionEntryFoodMenuModels: foodMenuDataArr,
                    productionEntryIngredientModels: ingredientDataArr
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
    $('#completed').click(function () {

        var message = validation(1);

        $("#EntryIngredient tbody tr").each(function () {
            var tds = $(this).find("td");
            var currentRow = $(this).closest("tr");
            var dataline = $('#EntryIngredient').DataTable().row(currentRow).data();

            var values = currentRow.find(":input").map(function () {
                return $(this).val()
            });

            ingredientDataArr.push({
                peIngredientId: dataline[0],
                ingredientId: dataline[1],
                ingredientQty: dataline[3],
                allocationIngredientQty: values[0],
                actualIngredientQty: values[1]
            });
        });

        $("#EntryFoodMenu tbody tr").each(function () {
            var tds = $(this).find("td");
            var currentRow = $(this).closest("tr");
            var datatrline = $('#EntryFoodMenu').DataTable().row(currentRow).data();
            var values = currentRow.find(":input").map(function () {
                return $(this).val()
            });

            foodMenuDataArr.push({
                peFoodMenuId: datatrline[0],
                foodMenuId: datatrline[1],
                expectedOutput: datatrline[3],
                allocationOutput: values[0],//$(currentRow).find("#allocateOut").val(),
                actualOutput: values[1]
            });
        });

        if (message == '') {
            $("#productionEntryForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    FoodmenuType: $("#FoodmenuType").val(),
                    ProductionFormulaId: $("#ProductionFormulaId").val(),
                    StoreId: $("#StoreId").val(),
                    ActualBatchSize: $("#ActualBatchSize").val(),
                    ProductionDate: $("#ProductionDate").val(),
                    Status: 3,
                    VariationNotes: $("#VariationNotes").val(),
                    Notes: $("#Notes").val(),
                    productionEntryFoodMenuModels: foodMenuDataArr,
                    productionEntryIngredientModels: ingredientDataArr
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
    window.location.href = "/ProductionEntry/Index";
});

$('#ok').click(function () {
    $("#aModal").modal('hide');
});

function validation(id) {
    var message = '';
    if (id == 1) {
        if ($("#StoreId").val() == '' || $("#StoreId").val() == '0') {
            message = "Select store"
        }
        else if ($("#ActualBatchSize").val() == '' || $("#ActualBatchSize").val() == '0') {
            message = "Enter value for the actual batch size"
        }
        else if ($("#ProductionFormulaId").val() == '' || $("#ProductionFormulaId").val() == '0') {
            message = "Select Recipe Name"
        }
    }
    return message;
}
