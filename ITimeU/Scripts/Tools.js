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

Tools.increaseHour = function (tb) {
    var time = Tools.parse(tb.val(), 24, true);
    tb.val(time);
}

Tools.decreaseHour = function (tb) {
    var time = Tools.parse(tb.val(), 24, false);
    tb.val(time);
}

Tools.increaseMinutes = function (tb) {
    var time = Tools.parse(tb.val(), 60, true);
    tb.val(time);
}

Tools.decreaseMinutes = function (tb) {
    var time = Tools.parse(tb.val(), 60, false);
    tb.val(time);
}

Tools.increaseSeconds = function (tb) {
    var time = Tools.parse(tb.val(), 60, true);
    tb.val(time);
}

Tools.decreaseSeconds = function (tb) {
    var time = Tools.parse(tb.val(), 60, false);
    tb.val(time);
}

Tools.increaseMilliseconds = function (tb) {
    var time = Tools.parse(tb.val(), 10, true);
    tb.val(time);
}

Tools.decreaseMilliseconds = function (tb) {
    var time = Tools.parse(tb.val(), 10, false);
    tb.val(time);
}

Tools.deleteFromList = function (list, url) {
    $.get(url, function (data) {
        list.html(data);
    });
}

Tools.postUpdate = function (control, url) {
    $.post(url, function (data) {
        control.html(data);
    });
}

Tools.DisableControls = function () {
    $("#btnEdit").attr('disabled', 'disabled');
    $("#btnDelete").attr('disabled', 'disabled');
    $("#btnUpHour").attr('disabled', 'disabled');
    $("#btnUpMin").attr('disabled', 'disabled');
    $("#btnUpSek").attr('disabled', 'disabled');
    $("#btnUpMS").attr('disabled', 'disabled');
    $("#btnDownHour").attr('disabled', 'disabled');
    $("#btnDownMin").attr('disabled', 'disabled');
    $("#btnDownSek").attr('disabled', 'disabled');
    $("#btnDownMS").attr('disabled', 'disabled');
}

Tools.EnableControls = function () {
    $("#btnEdit").removeAttr('disabled');
    $("#btnDelete").removeAttr('disabled');
    $("#btnUpHour").removeAttr('disabled');
    $("#btnUpMin").removeAttr('disabled');
    $("#btnUpSek").removeAttr('disabled');
    $("#btnUpMS").removeAttr('disabled');
    $("#btnDownHour").removeAttr('disabled');
    $("#btnDownMin").removeAttr('disabled');
    $("#btnDownSek").removeAttr('disabled');
    $("#btnDownMS").removeAttr('disabled');
}

Tools.ResetEditFields = function () {
    $("#tbEditHour").val("");
    $("#tbEditMin").val("");
    $("#tbEditSek").val("");
    $("#tbEditMSek").val("");
}