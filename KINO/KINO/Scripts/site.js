function setMenuItemActive() {
    jQuery("li>a").each(function () {
        if (window.location.pathname == jQuery(this).attr("href")) {
            jQuery(this).addClass("menu-item-active");
        }
    });
}
document.addEventListener("DOMContentLoaded", setMenuItemActive);

var seats = []
function addClickHandlers() {
    jQuery(".seat").click(function () {
        jQuery(this).addClass("seat-active");
        var row = jQuery(this).attr("row");
        var number = jQuery(this).attr("number");

        jQuery(".finalize-block").
    });
}
document.addEventListener("DOMContentLoaded", addClickHandlers);

