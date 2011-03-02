var CheckpointOrderHandler;
if (!CheckpointOrderHandler)
    CheckpointOrderHandler = {};

function CheckpointOrderHandler() { }

/** 
* Comment
*/

var btnChange;
var btnDelete;
var listCheckpointOrders;
var initialid;
var tbedit;
var lblStatus;

CheckpointOrderHandler.prototype.setEditAction = function (_listCheckpointOrders, _btnEdit, _tbedit) {
    listCheckpointOrders = _listCheckpointOrders;
    btnChange = _btnEdit;
    tbedit = _tbedit;
    btnChange.bind("click", function () {
        url = "/CheckpointOrder/UpdateCheckpointOrder/?ID=" + initialid + "&startingNumber=" + tbedit.val();
        $.get(url, function (data) {
            listCheckpointOrders.html(data);
        });
        tbedit.val("");
        Tools.disable(btnChange);
        Tools.disable(btnDelete);
    });
}

CheckpointOrderHandler.prototype.setChangeAction = function (_listCheckpointOrders, _tbedit) {
    listCheckpointOrders = _listCheckpointOrders;
    tbedit = _tbedit;

    listCheckpointOrders.bind("change", function () {
        var str = "";
        var strid = "";
        $("#lstCheckpointOrders :selected").each(function () {
            strid += $(this).val();
            str += $(this).text();
        });
        initialid = strid;
        tbedit.val(str);

        Tools.enable(btnChange);
        Tools.enable(btnDelete);
    });
}

CheckpointOrderHandler.prototype.setDeleteAction = function (_listCheckpointOrders, _btnDelete) {
    listCheckpointOrders = _listCheckpointOrders;
    btnDelete = _btnDelete;
    btnDelete.bind("click", function () {
        url = "/CheckpointOrder/DeleteCheckpointOrder/?id=" + initialid;
        $.get(url, function (data) {
            listCheckpointOrders.html(data);
        });
        tbedit.val("");
        Tools.disable(btnChange);
        Tools.disable(btnDelete);
    });
}

//CheckpointOrderHandler.prototype.setEdit2Action = function (_listCheckpointOrders, _listCheckpoints) {
//    listCheckpoints = _listCheckpoints;

//    listCheckpoints.bind("change", function () {
//        listCheckpointOrders.html("");
//        url = "/CheckpointOrder/GetStartingNumbersForCheckpoint/?id=" + initialid;
//        $.get(url, function (data) {
//            listCheckpointOrders.html(data);
//        });
//        tbedit.val("");
//        Tools.disable(btnChange);
//        Tools.disable(btnDelete);
//    });
//}






