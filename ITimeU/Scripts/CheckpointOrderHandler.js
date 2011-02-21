var CheckpointOrderHandler;
if (!CheckpointOrderHandler)
    CheckpointOrderHandler = {};

function CheckpointOrderHandler() { }

/** 
* Comment
*/

CheckpointOrderHandler.prototype.setIntermediateAction = function (_listIntermediate) {
    listIntermediate = _listIntermediate;
    Tools.disable(btnIntermediate);
    btnIntermediate.click(function () {
        timer.addIntermediate(function (runtime) {
            var displayText = timeFormatFactory.MSSDFormat(runtime);
            listIntermediate.append('<li class="liIntermediate">' + displayText + '</li>');

        });
    });
}
