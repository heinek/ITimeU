/** 
* This file uses a Stopwatch instance to handle DOM events.
* It assumes that the document has elements with the following IDs:
* time: placeholder to display time
* start: source of start/stop event
* reset: source of reset/lap event
*/

// A Stopwatch instance that displays its time nicely formatted.
var s = new Stopwatch(function (runtime) {
    // format time as m:ss.d
    var minutes = Math.floor(runtime / 60000);
    var seconds = Math.floor(runtime % 60000 / 1000);
    var decimals = Math.floor(runtime % 1000 / 100);
    var displayText = minutes + ":" + (seconds < 10 ? "0" : "") + seconds + "." + decimals;
    $("#time1").html(displayText);
});

// A Stopwatch instance that displays its raw time in milliseconds.
var t = new Stopwatch(function (runtime) {
    // display time in milliseconds
    $("#time2").html("" + runtime);
});

// Code to create instances and wire everything together.
$(document).ready(function () {
    // DOM Level 2 event model allows us to attach more than one event listener to a source!
    // jQuery's bind method hides browser-specific differences
    // first stopwatch
    $("#start1").bind("click", function () { s.startStop(); });
    $("#reset1").bind("click", function () { s.resetLap(); });
    s.doDisplay();
    // second stopwatch
    $("#start2").bind("click", function () { t.startStop(); });
    $("#reset2").bind("click", function () { t.resetLap(); });
    t.doDisplay();
});
