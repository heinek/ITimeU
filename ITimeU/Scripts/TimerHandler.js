///<reference path="jquery-1.4.4.js"/>

var TimerHandler;
if (!TimerHandler)
    TimerHandler = {};

function TimerHandler() { }

/** 
* This file uses a Stopwatch instance to handle DOM events.
* It assumes that the document has elements with the IDs passed to initTimer function
*/

var timeFormatFactory = new TimeFormatFactory();
var timer;

var btnStartStop;
var btnIntermediate;
var btnReset;
var btnEdit;
var btnDelete;
var listIntermediate;
var initialid;
var runtimeid;
var startBtnText = "Start";
var stopBtnText = "Stop";
var tbedit;
// Initialises a Stopwatch instance that displays its time nicely formatted.
TimerHandler.prototype.initTimer = function (lblTimer) {

    timer = new Stopwatch(function (runtime) {
        var displayText = timeFormatFactory.MSSDFormat(runtime);
        lblTimer.html(displayText);
    });
    timer.doDisplay();
}

TimerHandler.prototype.setIntermediateAction = function (_btnIntermediate, _listIntermediate) {
    btnIntermediate = _btnIntermediate;
    listIntermediate = _listIntermediate;
    Tools.disable(btnIntermediate);
    btnIntermediate.click(function () {
        timer.addIntermediate(function (runtime) {
            url = "/Timer/SaveRuntime/?runtime=" + runtime;
            $.get(url, function (data) {
                listIntermediate.html(data);
            });
        });
    });
}

TimerHandler.prototype.setResetAction = function (_btnReset, resetFunction) {
    btnReset = _btnReset;
    Tools.disable(btnReset);
    btnReset.bind("click", function () {
        timer.resetLap();
        resetFunction();
        if (tbruntime) {
            tbruntime.val("");
        }
        if (listIntermediate) {
            Tools.emptyList(listIntermediate);
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
}

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

}


