var editFoodMenuDataArr = [];
var foodMenuDeletedId = [];
var editIngredientDataArr = [];
var ingredientDeletedId = [];
$(document).ready(function () {
    $("#productionEntryForm").validate();
    $(".AllocationOutput").on('keyup change', calculateSum);
    EntryIngredient = $('#EntryIngredient').DataTable({
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



    //if ($("#FoodmenuType").val() == 2) {
    //    $("#onthespot").prop('checked', true);
    //}
    //else {
    //    $("#onthespot").prop('checked', false);
    //}

});

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
                            return '<input type="number" onkeyup="return calculateSum();" class="form-control col-sm-6 AllocationOutput" min="0" max="99999" value="' + row.allocationOutput + '"/>';
                        },
                    },
                    {
                        data: null,
                        mRender: function (data, type, row) {
                            return '<input type="number" class="form-control col-sm-6" min="0" max="99999" value="' + row.actualOutput + '"/>';
                        },
                    },
                    {
                        data: null,
                        mRender: function (data, type, row) {
                            return '<div class="form-button-action"><a href="#" data-itemid="' + row.foodMenuId+'" class="btn btn-link  editFoodMenuItem">Edit</a></div>';
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

$("#CurrentBatchSize").keyup(function () {
    var entryIngredient = document.getElementById('EntryIngredient');
    var batchSize = $("#BatchSize").val();
    var currentBatchSize = $("#CurrentBatchSize").val();
    var ingredientQty=0;
    for (var i = 1; i < entryIngredient.rows.length; i++) {
        ingredientQty = entryIngredient.rows[i].cells[1].innerText;
        actualIngredientQty = ingredientQty * currentBatchSize * batchSize;
        entryIngredient.rows[i].cells[2].innerHTML = actualIngredientQty;
    }
    
    //iterate through each row in the table
    //$('tr > td').each(function () {
    //    //the value of sum needs to be reset for each row, so it has to be set inside the row loop
    //    var sum = 0
    //    $(this)[0].children[3].text('1');
    //    //find the combat elements in the current row and sum it 
    //    $(this).find('.combat').each(function () {
    //        //var combat = $(this).text();
    //        $(this).text('1');
    //        //if (!isNaN(combat) && combat.length !== 0) {
    //        //    sum += parseFloat(combat);
    //        //}
    //    });
    //    //set the value of currents rows sum to the total-combat element in the current row
    //});
});

function calculateSum() {
    var $input = $(this);
    var $row = $input.closest('td');
    var sum = 0;

    $row.find(".AllocationOutput").each(function () {
        sum += parseFloat(this.value) || 0;
    });

    $("#varition").text(sum);
}