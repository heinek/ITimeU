var TimerHandler;
if (!TimerHandler)
    TimerHandler = {};

function TimerHandler() { }

/** 
* This file uses a Stopwatch instance to handle DOM events.
* It assumes that the document has elements with the IDs passed to initTimer function
*/

var timeFormatFactory = new TimeFormatFactory();

// Initialises a Stopwatch instance that displays its time nicely formatted.
TimerHandler.prototype.initTimer = function (lblTimer, btnStartStop, btnReset) {

    var timer = new Stopwatch(function (runtime) {
        var displayText = timeFormatFactory.MSSDFormat(runtime);
        lblTimer.html(displayText);
    });

    btnStartStop.bind("click", function () { timer.startStop(); });
    btnReset.bind("click", function () { timer.resetLap(); });
    timer.doDisplay();
}


