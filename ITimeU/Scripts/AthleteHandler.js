var AthleteHandler;
if (!AthleteHandler)
    AthleteHandler = {};

function AthleteHandler() { }

var clubId;
var athleteId;

AthleteHandler.prototype.getAllAthletes = function (_btnChangeClub, _ddAthlete) {

    _btnChangeClub.bind("click", function () {
        resetAllFields();

        url = "/Athlete/GetAllAthletes/?Id=" + clubId;
        $.post(url, function (data) {
            _ddAthlete.html("<option value=0> </option>" + data);
        });
        $("#StatusMessageUpdate").hide();
        $("#StatusMessageDelete").hide(); 
    });
}

AthleteHandler.prototype.getAthleteDetails = function (_btnChangeAthlete, _tbFirstName,
            _tbLastName, _tbEmail, _tbAddress, _tbPostCode, _tbCity, _tbPhoneNum,
            _ddGender, _ddBirthday, _tbStartNum, _ddClass) {

    _btnChangeAthlete.bind("click", function () {

        $("#StatusMessageUpdate").hide();
        $("#StatusMessageDelete").hide(); 
        url = "/Athlete/GetAthlete/?Id=" + athleteId;
        $.post(url, function (data) {
            var athlete = [];
            athlete = data.split(' //// ');
            _tbFirstName.val(athlete[1]);
            _tbLastName.val(athlete[2]);
            _tbAddress.val(athlete[4]);
            _tbEmail.val(athlete[7]);
            _tbPostCode.val(athlete[5]);
            _tbCity.val(athlete[6]);
            _tbPhoneNum.val(athlete[9]);
            _tbStartNum.val(athlete[13]);

            var birthvalue = $("#ddBirthDate option:contains('" + athlete[10] + "')").val();
            _ddBirthday.val(birthvalue);
            var gendervalue = $("#ddGender option:contains('" + athlete[8] + "')").val();
            _ddGender.val(gendervalue);
            var classvalue = $("#ddClass option:contains('" + athlete[12] + "')").val();
            _ddClass.val(classvalue);

        });
    });
}

AthleteHandler.prototype.setChangeActionClub = function (_ddClub) {   

    _ddClub.bind("change", function () {        
        $("#ddClub :selected").each(function () {
            clubId = $(this).val();           
        });        
      });
}

AthleteHandler.prototype.setChangeActionAthlete = function (_ddAthlete) {

    _ddAthlete.bind("change", function () {
        $("#ddAthlete :selected").each(function () {
            athleteId = $(this).val();
        });
    });
}

function resetAllFields() {    
    $("#ddBirthDate").val(0);
    $("#ddGender").val(0);
    $("#ddClass").val(0);    

    $("#txtFirstName").val('');
    $("#txtLastName").val('');
    $("#txtEmail").val('');
    $("#txtPostalAddress").val('');
    $("#txtPostalCode").val('');
    $("#txtCity").val('');
    $("#txtPhoneNumber").val('');
    $("#txtStartNumber").val('');
}

