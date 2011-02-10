var Tools;
if(!Tools)
    Tools = {};


Tools.selectById = function (id) {
    return $("#" + id);
}

Tools.emptyList = function (list) {
    list.each(function (n, item) {
        $(item).remove();
    });
}
