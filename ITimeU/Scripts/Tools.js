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

Tools.parse = function (integer, modulus, add) {
    var time = parseInt(integer, 10);
    if (add) {
        time = time + 1;
        time = time % modulus;
    }
    else {
        time = time - 1;
        if (time == -1) {
            time = modulus - 1;
        }
    }
    if (time < 10 && modulus != 10 && modulus != 24)
        time = "0" + time;
    return time;
}