function setMenuItemActive() {
    jQuery("li>a").each(function () {
        if (window.location.pathname == jQuery(this).attr("href")) {
            jQuery(this).addClass("menu-item-active");
        }
    });
}
document.addEventListener("DOMContentLoaded", setMenuItemActive);

function SetNewEntry(files) {
    var img = document.getElementById('filmimage');
    img.src = window.URL.createObjectURL(files[0]);
    //img.onload = function () {
        //window.URL.revokeObjectURL(this.src);
    //}
}