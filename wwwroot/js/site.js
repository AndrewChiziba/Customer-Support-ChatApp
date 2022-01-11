// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// When the user scrolls the page, execute myFunction for floating search
//window.onscroll = function () { myFunction() };

// Get the navbar
//var Searchbar = document.getElementById("Searchbar");

// Get the offset position of the navbar
//var sticky = Searchbar.offsetTop;

// Add the sticky class to the navbar when you reach its scroll position. Remove "sticky" when you leave the scroll position
//function myFunction() {
//    if (window.pageYOffset >= sticky) {
//        Searchbar.classList.add("sticky")
//    } else {
//        Searchbar.classList.remove("sticky");
//    }
//}

function MessagesStatus() {

    var Envelope = document.getElementById("envelope");

    $.ajax({
        url: '/Chat/MessageStatus',
        type: 'POST',
        async: true,
        // passing JSON objects as comma(,) separated values
        success: function (data) {
            // response from PHP back-end PHP server
            $("#data").text(data);
            if (data != "0") Envelope.innerHTML += "<strong style='color: red;   position: relative;  bottom: 1ex; font-size: 80%;'>" + data + "</strong>";

        },
        error: function () {
            console.log("There was an error.");
        }
    })
    // jQuery Ajax Post Request using $.ajax()
}


function toBasket(chk, productId) {
    var ProductId = productId;
    console.log(ProductId);

    chk.style.color = "#e2264d";

    $.ajax({
        url: '/MyAccount/ToBasket',
        type: 'POST',
        async: true,
        // passing JSON objects as comma(,) separated values
        data: {
            ProductId
        },
        success: (response) => {
            // response from PHP back-end PHP server
            $("#resultID").show().html(response);
        }
    })
    // jQuery Ajax Post Request using $.ajax()
}
///////////////////////////////////////////////////////////////////////
function deleteFromBasket(chk, productId) {
    var ProductId = productId;

    chk.style.color = "transparent";
    chk.style.textShadow = "0 0 0 #808080";

    $.ajax({
        url: '/MyAccount/DeleteFromBasket',
        type: 'POST',
        async: true,
        // passing JSON objects as comma(,) separated values
        data: {
            ProductId
        },
        success: (response) => {
            // response from PHP back-end PHP server
            $("#resultID").show().html(response);
        }
    })
    // jQuery Ajax Post Request using $.ajax()
}


function CreateImgCookieOnce() {
    if (!document.cookie.split('; ').find(row => row.startsWith('ImgCookie'))) {
        // Note that we are setting `SameSite=None;` in this example because the example
        // needs to work cross-origin.
        // It is more common not to set the `SameSite` attribute, which results in the default,
        // and more secure, value of `SameSite=Lax;`
        document.cookie = "ImgCookie=true; expires=Fri, 31 Dec 9999 23:59:59 GMT; SameSite=Lax; Secure";
    }
}


window.onload = function () {

    var SwitchBtn = document.getElementById("imgbtn");
    //create once off cookie for images on or off
    CreateImgCookieOnce();
    //determine whether switchbtn will be on or off
    if (document.cookie.split(';').some((item) => item.includes('ImgCookie=false'))) {

    }
    else {
        SwitchBtn.checked = true;
    }

    MessagesStatus();
}

function ImgStatus(SwitchBtn) {
    if (document.cookie.split(';').some((item) => item.includes('ImgCookie=false'))) {
        document.cookie = "ImgCookie=true; expires=Fri, 31 Dec 9999 23:59:59 GMT; SameSite=Lax; Secure";
    }
    else {
        document.cookie = "ImgCookie=false; expires=Fri, 31 Dec 9999 23:59:59 GMT; SameSite=Lax; Secure";
    }
    window.location.reload();
}

function SwitchBtnstatus() {
    var SwitchBtn = document.getElementById("imgbtn");
    if (SwitchBtn.checked == true) return true;
    else return false;
}





