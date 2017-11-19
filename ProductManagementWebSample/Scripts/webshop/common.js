
function switchStorage() {
    $('#db-storage-btn').toggleClass('active');
    $('#memory-storage-btn').toggleClass('active');    

    if ($('#db-storage-btn').hasClass('active')) {
        $('#selectedDataStorage').val('db');
    } else {
        $('#selectedDataStorage').val('memory');
    }
}