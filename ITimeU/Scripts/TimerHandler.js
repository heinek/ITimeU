///<reference path="jquery-1.4.4.js"/>

/** 
* This file uses a Stopwatch instance to handle DOM events.
* It assumes that the document has elements with the IDs passed to initTimer function.
*/

function TimerHandler() { }

var timeFormatFactory = new TimeFormatFactory();
var timer;
var btnStartStop;
var btnEdit;
var btnDelete;
var initialid;
var runtimeid;
var startBtnText = "Start";
var stopBtnText = "Stop";
var restartBtnText = "Restart";
var tbedit;
var ddlCheckpoints

var btnIntermediate;
var listIntermediate;
// Initialises a Stopwatch instance that displays its time nicely formatted.
TimerHandler.prototype.showTimer = function (lblTimer) {
    timer = new Stopwatch(function (runtime) {
        var displayText = timeFormatFactory.MSSDFormat(runtime);
        lblTimer.html(displayText);
    });
    timer.doDisplay();
}

TimerHandler.prototype.setIntermediateAction = function (_btnIntermediate, _listIntermediate, _ddlCheckpoints) {
/*
 * Start/Stop/Restart button functionality
*/
    ddlCheckpoints = _ddlCheckpoints;
TimerHandler.prototype.setStartStopActions = function (
    _btnStartStop, startFunction, stopFunction, restartFunction)
{
    btnStartStop = _btnStartStop;

    btnStartStop.bind("click", function () {
        timer.startStop();

        if (btnStartStop.val() == startBtnText) {
            // Start is clicked
            if (btnIntermediate) {
                Tools.enable(btnIntermediate);
            }

            btnStartStop.val(stopBtnText);

            startFunction();
        } else if (btnStartStop.val() == stopBtnText) {
            // Stop is clicked

            if (btnIntermediate) {
                Tools.disable(btnIntermediate);
            url = "/Timer/SaveRuntime/?runtime=" + runtime + "&checkpointid=" + ddlCheckpoints.val();
            }

            btnStartStop.val(restartBtnText);

            stopFunction();
        } else if (btnStartStop.val() == restartBtnText) {
            // Restart is clicked

            if (btnIntermediate) {
                Tools.enable(btnIntermediate);
            }
        listIntermediate.html("");
            if (listIntermediate) {
                Tools.emptyList(listIntermediate);
            }

            timer.resetLap();
            btnStartStop.val(stopBtnText);

            restartFunction();
        }
        Tools.disable(btnReset);
    });


}

TimerHandler.prototype.setEditAction = function (_listIntermediates, _btnEdit, _tbedit) {
    listIntermediate = _listIntermediates;
    btnEdit = _btnEdit;
    tbedit = _tbedit;
    btnEdit.bind("click", function () {
        url = "/Timer/EditRuntime/?orginalruntimeid=" + initialid + "&newruntime=" + tbedit.val();
        $.get(url, function (data) {
            listIntermediate.html(data);
        });
        tbedit.val("");
        Tools.disable(btnEdit);
        Tools.disable(btnDelete);
    });
}

TimerHandler.prototype.setDeleteAction = function (_listIntermediates, _btnDelete) {
    listIntermediate = _listIntermediates;
    btnDelete = _btnDelete;
    btnDelete.bind("click", function () {
        url = "/Timer/DeleteRuntime/?runtimeid=" + initialid;
        $.get(url, function (data) {
            listIntermediate.html(data);
        });
        tbedit.val("");
        Tools.disable(btnEdit);
        Tools.disable(btnDelete);
    });
}

TimerHandler.prototype.setChangeAction = function (_listIntermediates, _tbedit) {
    listIntermediate = _listIntermediates;
    tbedit = _tbedit;
    listIntermediate.bind("change", function () {
        var str = "";
        var strid = "";
        $("#lstIntermediates :selected").each(function () {
            strid += $(this).val();
            str += $(this).text();
        });
        initialid = strid;
        tbedit.val(str);
        Tools.enable(btnEdit);
        Tools.enable(btnDelete);
    });

TimerHandler.prototype.setStartStopActions = function (_btnStartStop, startFunction, stopFunction) {
    btnStartStop = _btnStartStop;
    btnStartStop.bind("click", function () {
        timer.startStop();
        if (btnStartStop.val() == startBtnText) {
            btnStartStop.val(stopBtnText);
            if (btnIntermediate) {
                Tools.enable(btnIntermediate);
            }
            if (btnReset) {
                Tools.disable(btnReset);
            }
            startFunction();
        }
        else {
            btnStartStop.val(startBtnText);
            if (btnReset) {
                Tools.enable(btnReset);
            }
            if (btnIntermediate) {
                Tools.disable(btnIntermediate);
            }
            stopFunction();
        }
    });

        });
    });
}
