/**
* This file defines the Stopwatch class.
* Note that it knows nothing about instances and how those instances are used.
*/
var Stopwatch;
if (!Stopwatch)
    Stopwatch = {};

/**
* Constructs a new Stopwatch instance.
* @param {Object} displayTime a function for displaying the time
*/
function Stopwatch(displayTime) {
    this.runtime = 0; // milliseconds
    this.timer = null; // non-null if running
    this.displayTime = displayTime; // not showing runtime anywhere
}

/**
* The increment in milliseconds.
* (This is a class variable shared by all Stopwatch instances.)
*/
Stopwatch.INCREMENT = 100

/**
* Displays the time using the appropriate display strategy.
*/
Stopwatch.prototype.doDisplay = function () {
    if (!this.laptime)
        this.displayTime(this.runtime);
    else
        this.displayTime(this.laptime);
}

Stopwatch.prototype.addIntermediate = function (updateList) {
    updateList(this.runtime);
}

/**
* Handles an incoming start/stop event.
*/
Stopwatch.prototype.startStop = function () {
    if (!this.timer) {
        var instance = this;
        this.runtime = 0;
        this.timer = window.setInterval(function () {
            instance.runtime += Stopwatch.INCREMENT;
            instance.doDisplay();
        }, Stopwatch.INCREMENT);
    }
    else {
        window.clearInterval(this.timer);
        this.timer = null;
        this.doDisplay();
    }
}

/**
* Handles an incoming reset/lap event.
*/
Stopwatch.prototype.resetLap = function () {
    if (!this.laptime) {
        if (this.timer) {
            this.laptime = this.runtime;
        }
        else {
            this.runtime = 0;
        }
    } else {
        delete this.laptime;
    }
    this.doDisplay();
}
