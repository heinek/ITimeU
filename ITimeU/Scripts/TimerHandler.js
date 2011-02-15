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
var listIntermediate;

var startBtnText = "Start";
var stopBtnText = "Stop";

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
            var displayText = timeFormatFactory.MSSDFormat(runtime);
            listIntermediate.append('<li class="liIntermediate">' + displayText + '</li>');
            // Save runtime to database...
            url = "/Runtime/Save/?runtime=" + runtime;
            $.get(url);
        });
    });
}

TimerHandler.prototype.setResetAction = function (_btnReset, resetFunction) {
    btnReset = _btnReset;
    Tools.disable(btnReset);
    btnReset.bind("click", function () {
        timer.resetLap();
        resetFunction();
        if (listIntermediate) {
            Tools.emptyList(listIntermediate);
        }
        Tools.disable(btnReset);
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


