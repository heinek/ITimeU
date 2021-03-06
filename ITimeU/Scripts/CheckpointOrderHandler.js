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
var divErrorMessage;
var btnChangeCP

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
        var str = "";
        var strid = "";
        $("#lstCheckpointOrders :selected").each(function () {
            strid = $(this).val();
            str = $(this).text();
        });

        initialid = strid;

        urlDelete = "/CheckpointOrder/DeleteCheckpointOrder/?id=" + initialid + "&checkpointId=" + $("#ddCheckpoint").val();
        $.get(urlDelete, function (data) {
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
        url = "/CheckpointOrder/MoveCheckpointUp/?checkpointID=" + $("#ddCheckpoint").val() + "&startingNumber=" + tbedit.val() + "&checkpointOrderId=" + initialid;
        $.get(url, function (data) {
            listCheckpointOrders.html(data);
            //listCheckpointOrders.options[3].selected = true;            
        });
    });
}

CheckpointOrderHandler.prototype.setMoveDownAction = function (_listCheckpointOrders, _btnDown) {
    listCheckpointOrders = _listCheckpointOrders;
    btnDown = _btnDown;

    btnDown.bind("click", function () {
        url = "/CheckpointOrder/MoveCheckpointDown/?checkpointID=" + $("#ddCheckpoint").val() + "&startingNumber=" + tbedit.val() + "&checkpointOrderId=" + initialid;
        $.get(url, function (data) {
            listCheckpointOrders.html(data);
        });
    });
}

CheckpointOrderHandler.prototype.setddCheckpointChangeAction = function (_ddCheckpoints, _btnChangeCP) {
    ddCheckpoints = _ddCheckpoints;
    btnChangeCP = _btnChangeCP;
    btnChangeCP.bind("click", function () {
        ddCheckpointId = ddCheckpoints.val();
        url = "/CheckpointOrder/GetStartingNumbersForCheckpoint/?checkpointID=" + ddCheckpointId;
        $.post(url, function (data) {
            listCheckpointOrders.html(data);
        });
    });
}

CheckpointOrderHandler.prototype.setInsertAction = function (_listCheckpointOrders, _tbedit, _divErrorMessage) {
    listCheckpointOrders = _listCheckpointOrders;
    tbedit = _tbedit;
    divErrorMessage = _divErrorMessage;
}

function SetCheckpointId(ddlCPId) {
    ddCheckpointId = ddlCPId;
}

// Verify startNumber is Number 0-9 and not empty
// If Number return True else False
function IsNumber(startNum) {
    var check = true;
    if (startNum.length == 0) {
        check = false;
        return check;
    }
    for (var i = 0; i < startNum.length; i++) {
        if (String.fromCharCode(startNum.charAt(i).charCodeAt(0)).match(/[^0-9]/g))
            check = false;        
    }    
    return check;
}

//Verfiy starting number already exist in List
//If exist return true else return false
function IsDuplicate(startNum) {
    var itemExists = false;
    $("#lstCheckpointOrders option").each(function () {
        if ($(this).text() == startNum) {            
            itemExists = true;            
        }
    });
    return itemExists;
 }