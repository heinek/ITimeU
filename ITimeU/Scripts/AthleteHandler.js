var AthleteHandler;
if (!AthleteHandler)
    AthleteHandler = {};

function AthleteHandler() { }

var clubId = 0;
var athleteId = 0;

AthleteHandler.prototype.getAllAthletes = function (_btnChangeClub, _ddAthlete) {

    _btnChangeClub.bind("click", function () {
        if (clubId > 0) {
            resetAllFields();

            url = "/Athlete/GetAllAthletes/?clubId=" + clubId;
            $.post(url, function (data) {
                _ddAthlete.html("<option value=0>Velg Athlete...</option>" + data);
            });
            $("#StatusMessageUpdate").hide();
            $("#StatusMessageDelete").hide();
            $("#StatusErrorMessage").hide();
        }
    });
}

AthleteHandler.prototype.getAthleteDetails = function (_btnChangeAthlete, _tbFirstName,
            _tbLastName, _tbEmail, _tbAddress, _tbPostCode, _tbCity, _tbPhoneNum,
            _ddGender, _ddBirthday, _tbStartNum, _ddClass) {

    _btnChangeAthlete.bind("click", function () {
        if (athleteId > 0) {
            $("#StatusMessageUpdate").hide();
            $("#StatusMessageDelete").hide();
            $("#StatusErrorMessage").hide();
            url = "/Athlete/GetAthleteDetails/?athleteId=" + athleteId;
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
        }
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
AthleteHandler.prototype.setEditActionAthlete = function (_btnEditAthlete, _tbFirstName,
            _tbLastName, _tbEmail, _tbAddress, _tbPostCode, _tbCity, _tbPhoneNum,
            _ddGender, _ddBirthday, _tbStartNum, _ddClass) {

    _btnEditAthlete.bind("click", function () {
        $("#StatusMessageUpdate").hide();
        $("#StatusMessageDelete").hide();
        $("#StatusErrorMessage").hide();

        url = "/Athlete/EditAthleteDetails/?txtathleteId=" + athleteId + "&txtfirstName=" + _tbFirstName.val() +
        "&txtlastName=" + _tbLastName.val() + "&txtemail=" + _tbEmail.val() + "&txtpostalAddress=" + _tbAddress.val() +
        "&txtpostalCode=" + _tbPostCode.val() + "&txtcity=" + _tbCity.val() + "&txtphoneNumber=" + _tbPhoneNum.val() +
        "&txtgender=" + _ddGender.val() + "&txtbirthDate=" + _ddBirthday.val() +
        "&txtstartNumber=" + _tbStartNum.val() + "&txtathleteClass=" + _ddClass.val();

        $.post(url, function (data) {            
            if (data == "invalid") {
                $("#StatusErrorMessage").show().html('First Name, Last Name, Email and StartNumber must not be empty and invalid.');
            }
            else if (data == "updated") {
                $("#StatusMessageUpdate").show().html('Athlete has been updated successfully!');
            }
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

    athleteId = 0;
}

