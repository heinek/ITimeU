var RaceHandler;
if (!RaceHandler)
    RaceHandler = {};

function RaceHandler() { }

var tbName;
var tbDistance;
var tbStartDate;
var btnCreate;
var divErrorMessage;

RaceHandler.prototype.setInsertAction = function (_btnCreate, _tbName, _tbDistance, _tbStartDate, _divErrorMessage) {
    btnCreate = _btnCreate;
    tbName = _tbName;
    tbDistance = _tbDistance;
    tbStartDate = _tbStartDate;
    divErrorMessage = _divErrorMessage;

    btnCreate.bind("click", function () {
//        if (tbName.val().length == 0 || tbDistance.val().length == 0 || tbStartDate.val().length == 0) {
//            divErrorMessage.show().html('Input fields must not be empty');
//        }
//        else {
//            divErrorMessage.hide();
            url = "/Race/Create/?name=" + tbName.val() + "&distance=" + tbDistance.val() + "&startDate=" + tbStartDate.val();
            $.post(url);
            
            tbName.val("");
            tbDistance.val("");
            tbStartDate.val("");
        //}
    });


}