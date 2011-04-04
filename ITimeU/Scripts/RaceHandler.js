var RaceHandler;
if (!RaceHandler)
    RaceHandler = {};

function RaceHandler() { }

var tbName;
var tbDistance;
var tbStartDate;
var btnCreate;
var divErrorMessage;
var divStatusMessage;

RaceHandler.prototype.setInsertAction = function (_btnCreate, _tbName, _tbDistance, _tbStartDate, _divErrorMessage, _divStatusMessage) {
    btnCreate = _btnCreate;
    tbName = _tbName;
    tbDistance = _tbDistance;
    tbStartDate = _tbStartDate;
    divErrorMessage = _divErrorMessage;
    divStatusMessage = _divStatusMessage;

    tbStartDate.bind("focus", function () {
        tbStartDate.blur();
    });

    btnCreate.bind("click", function () {
        if (tbName.val().length == 0 || tbStartDate.val().length == 0) {
            divStatusMessage.hide();
            divErrorMessage.show().html('Race name and start date must not be empty');
        }
        else if (!IsNumber(tbDistance.val())) {
            divStatusMessage.hide();
            divErrorMessage.show().html('Distance must be number');
        }
        else {
            divErrorMessage.hide();
            url = "/Race/Create/?name=" + tbName.val() + "&distance=" + tbDistance.val() + "&startDate=" + tbStartDate.val();
            $.post(url);
            divStatusMessage.show().html('Race has been saved successfully!');

            tbName.val("");
            tbDistance.val("");
            tbStartDate.val("");
        }
    });
}

function IsNumber(startNum) {
    var check = true;    
    for (var i = 0; i < startNum.length; i++) {
        if (String.fromCharCode(startNum.charAt(i).charCodeAt(0)).match(/[^0-9]/g))
            check = false;
    }
    return check;
}