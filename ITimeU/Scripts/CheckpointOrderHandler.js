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
var ddCheckpointId;
var initialid;
var tbedit;
var lblStatus;
var btnUp;
var btnDown;
var ddCheckpoints;

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

CheckpointOrderHandler.prototype.setChangeAction = function (_listCheckpointOrders, _tbedit, _btnUp, _btnDown) {
    listCheckpointOrders = _listCheckpointOrders;
    tbedit = _tbedit;
    btnUp = _btnUp;
    btnDown = _btnDown;

    listCheckpointOrders.bind("change", function () {
        var str = "";
        var strid = "";
        $("#lstCheckpointOrders :selected").each(function () {
            strid = $(this).val();
            str = $(this).text();
        });
    
        initialid = strid;
        tbedit.val(str);

        Tools.enable(btnChange);
        Tools.enable(btnDelete);
        Tools.enable(btnUp);
        Tools.enable(btnDown);
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

CheckpointOrderHandler.prototype.setMoveUpAction = function (_listCheckpointOrders, _btnUp) {
    listCheckpointOrders = _listCheckpointOrders;
    btnUp = _btnUp;    
    btnUp.bind("click", function () {
        url = "/CheckpointOrder/MoveCheckpointUp/?checkpointID=" + ddCheckpointId + "&startingNumber=" + tbedit.val() + "&checkpointOrderId=" + initialid;
        $.get(url, function (data) {
            listCheckpointOrders.html(data);
        });
    });
}

CheckpointOrderHandler.prototype.setMoveDownAction = function (_listCheckpointOrders, _btnDown) {
    listCheckpointOrders = _listCheckpointOrders;
    btnDown = _btnDown;

    btnDown.bind("click", function () {       
        url = "/CheckpointOrder/MoveCheckpointDown/?checkpointID=" + ddCheckpointId + "&startingNumber=" + tbedit.val() + "&checkpointOrderId=" + initialid;
        $.get(url, function (data) {
            listCheckpointOrders.html(data);
        });
    });
}

CheckpointOrderHandler.prototype.setddCheckpointChangeAction = function (_ddCheckpoints) {
    ddCheckpoints = _ddCheckpoints;
    ddCheckpoints.bind("change", function () {
        $("#ddCheckpoint :selected").each(function () {
            ddCheckpointId = $(this).val();
        });
        url = "/CheckpointOrder/GetStartingNumbersForCheckpoint/?checkpointID=" + ddCheckpointId;
        $.get(url, function (data) {
            listCheckpointOrders.html(data);            
        });
    });
}

