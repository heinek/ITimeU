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
var btnUpHour;
var btnUpMin;
var btnUpSek;
var btnUpMS;
var btnDownHour;
var btnDownMin;
var btnDownSek;
var btnDownMS;
var btnIntermediate;
var btnCheckpoint;
var initialid;
var runtimeid;
var startBtnText = "Start";
var stopBtnText = "Stop";
var restartBtnText = "Restart";
var tbEditHour;
var tbEditMin;
var tbEditSek;
var tbEditMSek;
var ddlCheckpoints
var checkpointid = 0;


// Initialises a Stopwatch instance that displays its time nicely formatted.
TimerHandler.prototype.showTimer = function (lblTimer) {
    timer = new Stopwatch(function (runtime) {
        var displayText = timeFormatFactory.MSSDFormat(runtime);
        lblTimer.html(displayText);
    });
    timer.doDisplay();
}

TimerHandler.prototype.setIntermediateAction = function (_btnIntermediate, _listIntermediate, _ddlCheckpoints) {
    btnIntermediate = _btnIntermediate;
    ddlCheckpoints = _ddlCheckpoints;
    Tools.disable(btnIntermediate);
    btnIntermediate.click(function () {
        timer.addIntermediate(function (runtime) {
            url = "/Timer/SaveRuntime/?runtime=" + runtime + "&checkpointid=" + ddlCheckpoints.val();
            $.get(url, function (data) {
                _listIntermediate.html(data);
            });
        });
    });
}

TimerHandler.prototype.setEditAction = function (_listIntermediates, _btnEdit, _tbEditHour, _tbEditMin, _tbEditSek, _tbEditMSek) {
    btnEdit = _btnEdit;
    tbEditHour = _tbEditHour;
    tbEditMin = _tbEditMin;
    tbEditSek = _tbEditSek;
    tbEditMSek = _tbEditMSek;
    btnEdit.bind("click", function () {
        url = "/Timer/EditRuntime/?orginalruntimeid=" + initialid + "&hour=" + tbEditHour.val() + "&min=" + tbEditMin.val() + "&sek=" + tbEditSek.val() + "&msek=" + tbEditMSek.val();
        $.get(url, function (data) {
            _listIntermediates.html(data);
        });
        tbEditHour.val("");
        tbEditMin.val("");
        tbEditSek.val("");
        tbEditMSek.val("");
        Tools.disable(btnEdit);
        Tools.disable(btnDelete);
        Tools.disable(btnUpHour);
        Tools.disable(btnUpMin);
        Tools.disable(btnUpSek);
        Tools.disable(btnUpMS);
        Tools.disable(btnDownHour);
        Tools.disable(btnDownMin);
        Tools.disable(btnDownSek);
        Tools.disable(btnDownMS);
    });
}

TimerHandler.prototype.setDeleteAction = function (_listIntermediates, _btnDelete) {
    btnDelete = _btnDelete;
    btnDelete.bind("click", function () {
        url = "/Timer/DeleteRuntime/?runtimeid=" + initialid;
        $.get(url, function (data) {
            _listIntermediates.html(data);
        });
        tbEditHour.val("");
        Tools.disable(btnEdit);
        Tools.disable(btnDelete);
        Tools.disable(btnUpHour);
        Tools.disable(btnUpMin);
        Tools.disable(btnUpSek);
        Tools.disable(btnUpMS);
        Tools.disable(btnDownHour);
        Tools.disable(btnDownMin);
        Tools.disable(btnDownSek);
        Tools.disable(btnDownMS);
    });
}

TimerHandler.prototype.setChangeAction = function (_listIntermediates, _tbEditHour, _tbEditMin, _tbEditSek, _tbEditMSek, _btnDownHour, _btnDownMin, _btnDownSek, _btnDownMS, _btnUpHour, _btnUpMin, _btnUpSek, _btnUpMS) {
    tbEditHour = _tbEditHour;
    tbEditMin = _tbEditMin;
    tbEditSek = _tbEditSek;
    tbEditMSek = _tbEditMSek;
    btnDownHour = _btnDownHour;
    btnDownMin = _btnDownMin;
    btnDownSek = _btnDownSek;
    btnDownMS = _btnDownMS;
    btnUpHour = _btnUpHour;
    btnUpMin = _btnUpMin;
    btnUpSek = _btnUpSek;
    btnUpMS = _btnUpMS;
    _listIntermediates.bind("change", function () {
        var strh = "";
        var strm = "";
        var strs = "";
        var strms = "";
        var strid = "";
        $("#listIntermediate :selected").each(function () {
            strid = $(this).val();
            strh = $(this).text().substring(0, 1);
            strm = $(this).text().substring(2, 4);
            strs = $(this).text().substring(5, 7);
            strms = $(this).text().substring(8, 9);
        });
        initialid = strid;
        tbEditHour.val(strh);
        tbEditMin.val(strm);
        tbEditSek.val(strs);
        tbEditMSek.val(strms);
        Tools.enable(btnEdit);
        Tools.enable(btnDelete);
        Tools.enable(btnDownHour);
        Tools.enable(btnDownMin);
        Tools.enable(btnDownSek);
        Tools.enable(btnDownMS);
        Tools.enable(btnUpHour);
        Tools.enable(btnUpMin);
        Tools.enable(btnUpSek);
        Tools.enable(btnUpMS);
    });
}

TimerHandler.prototype.setIncrementHourAction = function (_btnUpHour, _tbEditHour) {
    btnUpHour = _btnUpHour;
    tbEditHour = _tbEditHour;
    btnUpHour.bind("click", function () {
        var time = 0;
        time = parseInt(tbEditHour.val());
        time = time + 1;
        time = time % 24;
        tbEditHour.val(time);
    });
}

TimerHandler.prototype.setIncrementMinAction = function (_btnUpMin, _tbEditMin) {
    btnUpMin = _btnUpMin;
    tbEditMin = _tbEditMin;
    btnUpMin.bind("click", function () {
        var time = parseInt(tbEditMin.val());
        time = time + 1;
        time = time % 60;
        //        if (timermod < 10)
        //            str = "0" + timermod;
        //        else
        //            str = timermod;
        tbEditMin.val(time);
    });
}

TimerHandler.prototype.setIncrementSekAction = function (_btnUpSek, _tbEditSek) {
    btnUpSek = _btnUpSek;
    tbEditSek = _tbEditSek;
    btnUpSek.bind("click", function () {
        var time = 0;
        time = parseInt(tbEditSek.val());
        time = time + 1;
        time = time % 60;
        //tbEditSek.val((time < 10 ? "0" : "") + time);
        tbEditSek.val(time);
    });
}

TimerHandler.prototype.setIncrementMSAction = function (_btnUpMS, _tbEditMS) {
    btnUpMS = _btnUpMS;
    tbEditMS = _tbEditMS;
    btnUpMS.bind("click", function () {
        var time = 0;
        time = parseInt(tbEditMS.val());
        time = time + 1;
        time = time % 10;
        tbEditMS.val(time);
    });
}

TimerHandler.prototype.setDecrementHourAction = function (_btnUpHour, _tbEditHour) {
    btnUpHour = _btnUpHour;
    tbEditHour = _tbEditHour;
    btnUpHour.bind("click", function () {
        var time = 0;
        time = parseInt(tbEditHour.val());
        time = time - 1;
        if (time == -1) {
            time = 23;
        }
        tbEditHour.val(time);
    });
}

TimerHandler.prototype.setDecrementMinAction = function (_btnUpMin, _tbEditMin) {
    btnUpMin = _btnUpMin;
    tbEditMin = _tbEditMin;
    btnUpMin.bind("click", function () {
        var time = 0;
        time = parseInt(tbEditMin.val());
        time = time - 1;
        if (time == -1) {
            time = 59;
        }
        //tbEditMin.val((time < 10 ? "0" : "") + time);
        tbEditMin.val(time);
    });
}

TimerHandler.prototype.setDecrementSekAction = function (_btnUpSek, _tbEditSek) {
    btnUpSek = _btnUpSek;
    tbEditSek = _tbEditSek;
    btnUpSek.bind("click", function () {
        var time = 0;
        time = parseInt(tbEditSek.val());
        time = time - 1;
        if (time == -1) {
            time = 59;
        }
        //tbEditSek.val((time < 10 ? "0" : "") + time);
        tbEditSek.val(time);
    });
}

TimerHandler.prototype.setDecrementMSAction = function (_btnUpMS, _tbEditMS) {
    btnUpMS = _btnUpMS;
    tbEditMS = _tbEditMS;
    btnUpMS.bind("click", function () {
        var time = 0;
        time = parseInt(tbEditMS.val());
        time = time - 1;
        if (time == -1) {
            time = 9;
        }
        tbEditMS.val(time);
    });
}

TimerHandler.prototype.setChangeCheckpoint = function (_btnCheckpoint, _listIntermediate, _ddlCheckpoint) {
    _btnCheckpoint.bind("click", function () {
        checkpointid = _ddlCheckpoint.val();
        url = "/Timer/ChangeCheckpoint/?checkpointid=" + checkpointid;
        $.get(url, function (data) {
            _listIntermediate.html(data);
        });
    });
}

TimerHandler.prototype.setStartStopActions = function (_btnStartStop, startFunction, stopFunction) {
    btnStartStop = _btnStartStop;
    btnStartStop.bind("click", function () {
        timer.startStop();
        if (btnStartStop.val() == startBtnText || btnStartStop.val() == restartBtnText) {
            btnStartStop.val(stopBtnText);
            if (btnIntermediate) {
                Tools.enable(btnIntermediate);
            }
            startFunction();
        } else {
            btnStartStop.val(restartBtnText);

            if (btnIntermediate) {
                Tools.disable(btnIntermediate);
            }
            stopFunction();
        }
    });
}

