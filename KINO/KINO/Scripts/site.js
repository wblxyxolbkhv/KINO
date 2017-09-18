function setMenuItemActive() {
    jQuery("li>a").each(function () {
        console.log(window.location.pathname);
        console.log(jQuery(this).attr("href"));
        if (window.location.pathname == jQuery(this).attr("href")) {
            jQuery(this).addClass("menu-item-active");
        }
    });
}
document.addEventListener("DOMContentLoaded", setMenuItemActive);