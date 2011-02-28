var CheckpointOrderHandler;
if (!CheckpointOrderHandler)
    CheckpointOrderHandler = {};

function CheckpointOrderHandler() { }

/** 
* Comment
*/

var btnEdit;
var btnDelete;
var listCheckpointOrders;
var initialid;
var tbedit;
var lblStatus;

CheckpointOrderHandler.prototype.setEditAction = function (_listCheckpointOrders, _btnEdit, _tbedit) {
    listCheckpointOrders = _listCheckpointOrders;
    btnEdit = _btnEdit;
    tbedit = _tbedit;
    btnEdit.bind("click", function () {
        url = "/CheckpointOrder/UpdateCheckpointOrder/?ID=" + initialid + "&startingNumber=" + tbedit.val();
        $.get(url, function (data) {
            listCheckpointOrders.html(data);
        });
        tbedit.val("");
        Tools.disable(btnEdit);
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
        Tools.enable(btnEdit);
        Tools.enable(btnDelete);
    });
}

CheckpointOrderHandler.prototype.setDeleteAction = function (_listCheckpointOrders, _btnDelete) {
    listCheckpointOrders = _listCheckpointOrders;
    btnDelete = _btnDelete;
    btnDelete.bind("click", function () {
        url = "/CheckpointOrder/Delete/?id=" + initialid;
        $.get(url, function (data) {
            listCheckpoints.html(data);
        });
        tbedit.val("");
        Tools.disable(btnEdit);
        Tools.disable(btnDelete);
    });
}




