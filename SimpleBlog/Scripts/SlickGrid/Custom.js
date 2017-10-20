var grid;
var columns = [
    { id: "ID", name: "ID", field: "ID" },
    { id: "CategoryId", name: "CategoryId", field: "CategoryId" },
    { id: "Subject", name: "Subject", field: "Subject", minWidth: 70 },
    { id: "Body", name: "Body", field: "Body" },
    { id: "DatePosted", name: "DatePosted", field: "DatePosted" },
    { id: "Type", name: "Type", field: "Type" },
    { id: "OtherType", name: "OtherType", field: "OtherType" },
    { id: "Category", name: "Category", field: "Category" },
];

var options = {
    enableCellNavigation: true,
    enableColumnReorder: false,
    multiColumnSort: true,
    asyncEditorLoading: true,
    forceFitColumns: true
};

$(function () {
    var myData = [];
    $.getJSON('/SlickGrid/getListData', function (data) {
        myData = data;
        console.log(myData);
        grid = new Slick.Grid("#myGrid", myData, columns, options);
    });
});