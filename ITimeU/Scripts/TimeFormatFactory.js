var TimeFormatFactory;
if (!TimeFormatFactory)
    TimeFormatFactory = {};

function TimeFormatFactory() { }

TimeFormatFactory.prototype.MSSDFormat = function (milliSecondsRun) {
    // format time as m:ss.d
    var minutes = Math.floor(milliSecondsRun / 60000);
    var seconds = Math.floor(milliSecondsRun % 60000 / 1000);
    var decimals = Math.floor(milliSecondsRun % 1000 / 100);
    var displayText = minutes + ":" + (seconds < 10 ? "0" : "") + seconds + "." + decimals;
    return displayText;
}

TimeFormatFactory.prototype.Milliseconds = function (milliSecondsRun) {
    return milliSecondsRun;
}
