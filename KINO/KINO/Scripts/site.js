function setMenuItemActive() {
    jQuery("li>a").each(function () {
        if (window.location.pathname == jQuery(this).attr("href")) {
            jQuery(this).addClass("menu-item-active");
        }
    });
}
document.addEventListener("DOMContentLoaded", setMenuItemActive);