var slideIndex = -1;
var slidesCount = 0;
function onSlideshowInit() {
    $(".slide").each(function (index) {
        slidesCount++;
    });
}
document.addEventListener("DOMContentLoaded", onSlideshowInit);
function onSlideshowTick() {
    if (slideIndex + 1 >= slidesCount) {
        slideIndex = 0;
    }
    else {
        slideIndex++;
    }
    console.log(slideIndex)
    $(".slide").each(function (index) {
        if (index == slideIndex) {
            $(this).css("display", "block");
        }
        else {
            $(this).css("display", "none");
        }
    });
    
}
document.addEventListener("DOMContentLoaded", onSlideshowTick);
//setInterval(onSlideshowTick, 3000);
