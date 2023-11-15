// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/**
 * Set the value of a cookie and its expiration date in days
 */
function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}

/**
 * Retreive a cookie by name
 */
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

/** erase a cookie */
function eraseCookie(name) {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}

/** function to change the theme of the application
 * Stores theme preference in a cookie.
 */
function changeTheme() {

    var value = getCookie("Theme");

    if (value !== null) {
        var color = value === "Dark" ? "Light" : "Dark";
        setCookie("Theme", color, 1);
    }
    else {
        setCookie("Theme", "Dark");
    }

    location.reload();

}


function updateWager(val) {

    $.ajax({
        type: "POST",
        url: 'Bets/updateWagerBet',
        data: { "bet": val },
        success: function (data) {
            location.reload();
        },
        error: function (error) {
            alert(JSON.stringify(error));
            cache: true;
        }
    });

}