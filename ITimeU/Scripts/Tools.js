var Tools;
if(!Tools)
    Tools = {};


Tools.selectElementById = function (id) {
    return $('#' + id);
}

Tools.emptyList = function (list) {
    list.children().remove();
}

Tools.enable = function (element) {
    element.attr("disabled", false);
}

Tools.disable = function (element) {
    element.attr("disabled", true);
}
