$(document).ready(function () {
    // Store the scroll position in sessionStorage
    $(window).scroll(function () {
        sessionStorage.setItem('scrollPosition', $(window).scrollTop());
    });

    // Restore the scroll position after the action is completed
    if (sessionStorage.getItem('scrollPosition') !== null) {
        $(window).scrollTop(sessionStorage.getItem('scrollPosition'));
    }
});
