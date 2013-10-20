//All pages

$(document).ready(function() {
    // Monitor dropdown selections for a change
    // ------------------------------------------------------------------------
    $('.playerkey').click(function(e){
        playerkey = $(this).attr("data-playerkey");
        network = $(this).attr("data-network");
        playerName = $(this).attr("data-playerName");
        switchAlias(playerkey, network, playerName);
    });

    $('.currencySwitch').click(function(e){
        currencySelected = $(this).attr('value');
        switchCurrency(currencySelected);
    });

    // Prevent default click behaviour on poker screen name add button
    $('.nameAdd').click(function(e){
        e.preventDefault();
    });


    //adjust the css for the drop down to make it line up with the size of the button/users playername
    var playerNameSize = $('.nameSwitch').outerWidth();
    $('.pdNameSwitch .dropdown-menu').width(playerNameSize-2);

    //function for settings add a handler for adding of the network name
    $('.networkSelect li').click(function(){
        var selectedNetwork = $(this).html();
        var selectedValue = $(this).attr('value');
        $('.networkSelect .btn-default p').html(selectedNetwork);
        $('.networkSelect .btn-default p').attr('value', selectedValue);
        $('#network').attr('value',selectedValue);
    });

    // The feedback dialog 
    function feedback() {
        var feedbackDialog = $('.feedback')
        .dialog({
            autoOpen:   false,
            modal:      false,
            resizable:  false,
            dialogClass: 'feedbackDialog',
            width: 600,
            position: {  
                my: 'center',
                at: 'top+300',
                of: '.box'
            }
        });
        $('.feedback').dialog('open');
    }

    function resizeFeedback() {

        $( '.feedback' ).dialog( 
            "option",
            "position",{ 
                my: 'center',
                at: 'top+300',
                of: '.box'
        });

    }

    $('.feedback .closeBtn').click(function(){
        $('.feedback').dialog('close');
    });

    // bind a click function to the function in the menu button
    $('.feedbackBTN').click(function(){
        feedback();
    });

    // Alert message fade outs
    var hide_delay = 2500;  // starting timeout before first message is hidden
    var hide_next = 800;   // time in mS to wait before hiding next message
    $('.alert-message').slideDown().each( function(index,el) {
        window.setTimeout( function(){
            $(el).slideUp();  // hide the message
        }, hide_delay + hide_next*index);
    });

    // Function for feedback select
    $('.feedbackSelect li').click(function(){
        var feedType = $(this).html();
        var feedValue = $(this).attr('value');
        $('.feedbackSelect .btn-default p').html(feedType);
        $('#feedback_type').attr('value', feedValue);
    });

    $(window).resize(function(){
        $(document).ready(function(){
            var feedOpen = $('.feedback').css('display');
            if(feedOpen == 'block')
                resizeFeedback();
        });
    });

    //Ajax required for the feedback form
    $("#feedback_submit_button").click(function(){

    $.ajax({
        url: window.location.protocol + "//" + window.location.host + $("#feedback_form").attr("action"),
        type: "POST",
        data:  $("#feedback_form").serialize(),
        dataType: 'json',
        beforeSend: function () {
            // Hide submit button, show sending button
            $('#feedback_text').attr('readonly', true);
            $('#feedback_submit_button').css( {display: 'none'} );
            $('#feedback_sending_button').css( {display: 'block'} );
        }
        }).error(function ( data ) {
            // If http status code was anything but a 200
            $('#feedback_sending_button').css( {display: 'none'} );
            $('#feedback_error').css( {display: 'block'} );

        }).success(function ( data ) {
        // Check if server confirmed msg was sent, else show an error
            if(data['success'] == true){
                $('#feedback_sending_button').css( {display: 'none'} );
                $('#feedback_success').css( {display: 'block'} );
                $('#feedback_form').hide('slow');
            }
            else{
                $('#feedback_sending_button').css( {display: 'none'} );
                $('#feedback_error').css( {display: 'block'} );
            }
        });
    });
});

//The Nav Bar
var menuOpen = false;
$('#loginBtn').click( function () {
        if (menuOpen == false){
            $('#loginBox').slideDown();
            menuOpen = true;
        }
        else if( menuOpen == true){
            $('#loginBox').slideUp();
            menuOpen = false;
        }
});

// ---
// Functions for sliding in-and-out the winning hand on
// session details pg -> hand list
function showWinningHand(el){
    var div = $(el).next('.winning_hand');
    div.stop(true, true).animate({
        left: "+=105"
    }, 'fast');
}
function hideWinningHand(el){
    var div = $(el).next('.winning_hand');
    div.stop(true, true).animate({
        left: "-=105"
    }, 'fast');
}
// ---

// Switches the currently selected alias and reloads the page
// ------------------------------------------------------------------------
function switchAlias(playerkey, network, playerName){
    // Reload the page with the option chosen by the user
    url = window.location.protocol + "//" + window.location.host + window.location.pathname + window.location.search
    url = addOrUpdateUrlParameter(url, 'playerkey', playerkey);
    url = addOrUpdateUrlParameter(url, 'network', network);
    url = addOrUpdateUrlParameter(url, 'playername', playerName);
    window.location.href = url;
}

// Switches currencies and reloads the page
// ------------------------------------------------------------------------
function switchCurrency(currencySelected){
    // Reload the page with the option chosen by the user
    url = window.location.protocol + "//" + window.location.host + window.location.pathname + window.location.search
    url = addOrUpdateUrlParameter(url, 'currency', currencySelected);
    window.location.href = url
}

// URL parameter Add/Edit/Delete functions
// a = url, b = paramName, c = paramValue
function getUrlParameter(a, b) { var searchString = a.substring(1), i, val, params = searchString.split("&"); for (i=0;i<params.length;i++) { val = params[i].split("="); if (val[0] == b) { return decodeURIComponent(val[1]); } } return null; }
function addOrUpdateUrlParameter(a, b, c) { if (b.trim() == "") { /* alert("Parameter name should not be empty."); */ return a } if (c.trim() == "") { /* alert("Parameter value should not be empty."); */ return a } if (a.indexOf("?") == -1) { return a + "?" + b + "=" + c } var d = ""; var e = false; var f = false; if (a.indexOf("?") == -1) { /* alert("No Url Parameters Found!"); */ return a } var g = a.split("?"); if (g.length >= 2) { d = d + g[0] + "?"; var h = g[1].split(/[&;]/g); for (var i = 0; i < h.length; i++) { var j = h[i]; var k = j.split("="); if (k.length >= 2) { if (k[0] == b) { f = true; k[1] = c; d = d + b + "=" + c + "&" } else { d = d + j + "&" } } } if (f == false) { /* alert("Param not found to remove, so adding it"); */ d = a + "&" + b + "=" + c; } if (d.charAt(d.length -1) == '&') { d = d.slice(0, d.length - 1) } return d } }
function removeUrlParameter(a, b) { var c = ""; var d = false; var e = false; if (a.indexOf("?") == -1) { /* alert("No Url Parameters Found!");*/ return a } var f = a.split("?"); if (f.length >= 2) { c = c + f[0] + "?"; var g = f[1].split(/[&;]/g); for (var h = 0; h < g.length; h++) { var i = g[h]; var j = i.split("="); if (j.length >= 2) { if (j[0] != b) { c = c + i + "&"; d = true } else { e = true } } } if (e == false) { /* alert("Requested query string not found to remove");*/ return a } var k = c.split("?"); if (k.length >= 2) { if (k[1].trim() == "") { return k[0] } } if (d == true) { c = c.slice(0, c.length - 1) } return c } }


// -- Player Switching (coaching)
$('a.coach_player').click(function(e){
    // Prevent default click behaviour
    e.preventDefault();

    // Get player who was selected/clicked on
    var player = $(this).attr('value');

    // If path is neither dashboard or sessions-list, set to sessions-list, path js vars are set in the base template
    if ( (current_path != dashboard_path) && (current_path != sessions_path) ) {
        path = sessions_path;
    } else {
        path = current_path;
    }

    // Get the current url
    url = window.location.protocol + "//" + window.location.host + path + window.location.search;

    // Check if player value has changed from page load
    if(getUrlParameter(url, 'player') != player ){

        // On player switches remove currency and player preferences
        url = removeUrlParameter(url, 'currency');
        url = removeUrlParameter(url, 'playerkey');
        url = removeUrlParameter(url, 'sessionid');

        // If player selected is 'off', remove player and playerkey from the param list
        if (player == 'off'){
            url = removeUrlParameter(url, 'player');
        } else {
            url = addOrUpdateUrlParameter(url, 'player', player);
        }

        window.location.href = url;
    }
});
