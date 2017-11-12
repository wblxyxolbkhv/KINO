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




var seatsIds = []
function setSeatsClickable() {
    jQuery(".seat").click(function () {
        var element = jQuery(this);
        if (!element.hasClass("booked")) {
            if (!element.hasClass("active-seat")) {
                element.addClass("active-seat");
                var row = element.attr("row");
                var number = element.attr("number");
                onClickSeat(row, number);
            }
            else {
                element.removeClass("active-seat");
                var row = element.attr("row");
                var number = element.attr("number");
                onDeactivateSeat(row, number);
            }
        }
    });
}
function onClickSeat(row, number) {
    var cost = jQuery(".session-cost").attr("value");
    console.log(cost);
    var content = "<p>" + row + " ряд</p>";
    content += "<p>" + number + " место</p>";
    content += "<p>" + cost + "р</p>";
    var id = "ticket-row" + row + "-number" + number;
    var value = row * 1000 + Number.parseInt(number);
    jQuery(".make-order-button").before("<div class='ticket' id='" + id + "'>" + content + "<input type='hidden' name='"+id+"' value=" + value + "></div>");
    seatsIds.push(id);
}
function onDeactivateSeat(row, number) {
    var id = "ticket-row" + row + "-number" + number;
    var index = seatsIds.indexOf(id);
    id = "#" + id;
    jQuery(id).detach();
    seatsIds.splice(index);
}
document.addEventListener("DOMContentLoaded", setSeatsClickable);
