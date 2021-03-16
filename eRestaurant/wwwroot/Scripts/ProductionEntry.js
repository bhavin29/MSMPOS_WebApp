var editFoodMenuDataArr = [];
var foodMenuDeletedId = [];
var editIngredientDataArr = [];
var ingredientDeletedId = [];
$(document).ready(function () {
    $("#productionEntryForm").validate();
    EntryIngredient = $('#EntryIngredient').DataTable({
        columnDefs: [
            { targets: [0], orderable: false, visible: false },
            { targets: [1], orderable: false, visible: false },
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

    EntryFoodMenu = $('#EntryFoodMenu').DataTable({
        columnDefs: [
            { targets: [0], orderable: false, visible: false },
            { targets: [1], orderable: false, visible: false },
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

    if ($("#FoodmenuType").val() == 2) {
        $("#onthespot").prop('checked', true);
    }
    else {
        $("#onthespot").prop('checked', false);
    }

    if ($("#AssetEventId").val() > 0) {
        document.getElementById("headerColorChange").style.backgroundColor = "#7FFF00";
    }
    calculateSum();
    $("#ProductionFormulaId").focus();
    $("#ProductionFormulaId").select2();
});

function loadProductionFormula() {
    window.location.href = "/ProductionEntry/ProductionEntry?productionFormulaId=" + $("#ProductionFormulaId").val() + "&foodMenuType=" + $("#FoodmenuType").val();
    $("#ProductionFormulaId").focus();
}
/*
function loadProductionFormulaById() {
    $.ajaxSetup({ cache: false });
    $.ajax({
        url: "/ProductionEntry/GetProductionFormulaById/" + $("#ProductionFormulaId").val(),
        dataType: "json",
        success: function (data) {
            if (EntryFoodMenu != null) {
                EntryFoodMenu.clear();
                EntryFoodMenu.destroy();
            }
            if (EntryIngredient != null) {
                EntryIngredient.clear();
                EntryIngredient.destroy();
            }

            $("#BatchSize").val(data.productionEntryModel.batchSize);
            $("#BatchSizeUnitName").text(data.productionEntryModel.unitName);
            $("#CurrentBatchSizeUnitName").text(data.productionEntryModel.unitName);
            $("#ProductionDate").val(data.productionEntryModel.productionDate);

            EntryIngredient = $('#EntryIngredient').DataTable({
                "data": data.productionEntryModel.productionEntryIngredientModels,
                "columns": [
                    { "data": "peIngredientId", "name": "PEIngredientId", "autoWidth": true },
                    { "data": "ingredientId", "name": "IngredientId", "autoWidth": true },
                    { "data": "ingredientName", "name": "IngredientName", "autoWidth": true },
                    { "data": "ingredientQty", "name": "IngredientQty", "autoWidth": true },
                    { "data": "actualIngredientQty", "name": "ActualIngredientQty", "autoWidth": true }
                ],
                columnDefs: [
                    { targets: [0], orderable: false, visible: false },
                    { targets: [1], orderable: false, visible: false },
                    { targets: [3,4], orderable: false, class: "text-right" },
                ],
                "paging": false,
                "bLengthChange": true,
                "bInfo": true,
                "bFilter": true,
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
                "data": data.productionEntryModel.productionEntryFoodMenuModels,
                "columns": [
                    { "data": "peFoodMenuId", "name": "PEFoodMenuId", "autoWidth": true },
                    { "data": "foodMenuId", "name": "FoodMenuId", "autoWidth": true },
                    { "data": "foodMenuName", "name": "FoodMenuName", "autoWidth": true },
                    { "data": "expectedOutput", "name": "ExpectedOutput", "autoWidth": true },
                    {
                        data: null,
                        mRender: function (data, type, row) {
                            return '<input type="number" id="allocateOut"  onkeyup="return calculateSum();" class="form-control col-sm-6 AllocationOutput" min="0" max="99999" value="' + row.allocationOutput + '"/>';
                        },
                    },
                    {
                        data: null,
                        mRender: function (data, type, row) {
                            return '<input type="number" id="actualOut" class="form-control col-sm-6" min="0" max="99999" value="' + row.actualOutput + '"/>';
                        },
                    }
                ],
                columnDefs: [
                    { targets: [0], orderable: false, visible: false },
                    { targets: [1], orderable: false, visible: false },
                    { targets: [3,4], orderable: false, class: "text-right" }
                ],
                "paging": false,
                "bLengthChange": true,
                "bInfo": true,
                "bFilter": true,
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
        },
        error: function (err) {

        }
    });
}
*/
$("#ActualBatchSize").keyup(function () {
    var entryIngredient = document.getElementById('EntryIngredient');
    var batchSize = $("#BatchSize").val();
    var actualBatchSize = $("#ActualBatchSize").val();
    var ingredientQty = 0;

    for (var i = 1; i < entryIngredient.rows.length; i++) {
        var value = entryIngredient.rows[i].cells[1].innerText;
        ingredientQty = value.split(' ')[0].replace(',','');
        ingredientQty = parseFloat(ingredientQty);
        actualIngredientQty = (ingredientQty * actualBatchSize) / batchSize;
        actualIngredientQty = parseFloat(actualIngredientQty).toFixed(2);
        var ingtextbox = '#i' + i;
        $(ingtextbox).val(actualIngredientQty);
    }

    var entryFoodmenu = document.getElementById('EntryFoodMenu');
    var foodmenuQty = 0;

    for (var i = 1; i < entryFoodmenu.rows.length; i++) {
        var value = entryFoodmenu.rows[i].cells[1].innerText;
        foodmenuQty = value.split(' ')[0].replace(',', '');
        foodmenuQty = parseFloat(foodmenuQty);
        actualfoodmenuQty = (foodmenuQty * actualBatchSize) / batchSize;
        actualfoodmenuQty = parseFloat(actualfoodmenuQty).toFixed(2);
        var foodtextbox = '#f' + i;
        $(foodtextbox).val(actualfoodmenuQty);
    }
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
                ingredientQty: dataline[3].split(' ')[0].replace(',', ''),
                actualIngredientQty: values[0]//tds[2].textContent
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
                expectedOutput: datatrline[3].split(' ')[0].replace(',', ''),
                allocationOutput: $(currentRow).find("#allocateOut").val(),
                actualOutput: values[0]
            });
        });

        if (message == '') {
            $("#productionEntryForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    FoodmenuType: $("#FoodmenuType").val(),
                    ProductionFormulaId: $("#ProductionFormulaId").val(),
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

            ingredientDataArr.push({
                peIngredientId: dataline[0],
                ingredientId: dataline[1],
                ingredientQty: dataline[3],
                actualIngredientQty: tds[2].textContent
            });
        });

        $("#EntryFoodMenu tbody tr").each(function () {
            var tds = $(this).find("td");
            var currentRow = $(this).closest("tr");
            var datatrline = $('#EntryFoodMenu').DataTable().row(currentRow).data();

            foodMenuDataArr.push({
                peFoodMenuId: datatrline[0],
                foodMenuId: datatrline[1],
                expectedOutput: datatrline[3],
                allocationOutput: $(currentRow).find("#allocateOut").val(),
                actualOutput: $(currentRow).find("#actualOut").val()
            });
        });

        if (message == '') {
            $("#productionEntryForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    FoodmenuType: $("#FoodmenuType").val(),
                    ProductionFormulaId: $("#ProductionFormulaId").val(),
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

            ingredientDataArr.push({
                peIngredientId: dataline[0],
                ingredientId: dataline[1],
                ingredientQty: dataline[3],
                actualIngredientQty: tds[2].textContent
            });
        });

        $("#EntryFoodMenu tbody tr").each(function () {
            var tds = $(this).find("td");
            var currentRow = $(this).closest("tr");
            var datatrline = $('#EntryFoodMenu').DataTable().row(currentRow).data();

            foodMenuDataArr.push({
                peFoodMenuId: datatrline[0],
                foodMenuId: datatrline[1],
                expectedOutput: datatrline[3],
                allocationOutput: $(currentRow).find("#allocateOut").val(),
                actualOutput: $(currentRow).find("#actualOut").val()
            });
        });

        if (message == '') {
            $("#productionEntryForm").on("submit", function (e) {
                e.preventDefault();
                var data = ({
                    Id: $("#Id").val(),
                    FoodmenuType: $("#FoodmenuType").val(),
                    ProductionFormulaId: $("#ProductionFormulaId").val(),
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
        if ($("#ActualBatchSize").val() == '' || $("#ActualBatchSize").val() == '0') {
            message = "Enter value for the actual batch size"
        }

        if ($("#ProductionFormulaId").val() == '' || $("#ProductionFormulaId").val() == '0') {
            message = "Select Recipe Name"
        }
    }
    return message;
}
