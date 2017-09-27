var slideIndex = 0;
var slidesCount = 0;
var timerId = 0
function onMouseEnter() {
    clearInterval(timerId);
    console.log("clear "+timerId)
}
function onMouseLeave() {
    timerId = setInterval(SlideUp, 5000);
    console.log("set " + timerId)
}
function onSlideshowInit() {
    $(".slide").each(function (index) {
        slidesCount++;
    });
    slideshow.onmouseenter = onMouseEnter;
    slideshow.onmouseleave = onMouseLeave;
}
document.addEventListener("DOMContentLoaded", onSlideshowInit);
function onSlideUpdate() {
    
    $(".slide").each(function (index) {
        if (index == slideIndex) {
            $(this).css("display", "block");
        }
        else {
            $(this).css("display", "none");
        }
    });
    
}
document.addEventListener("DOMContentLoaded", onSlideUpdate);
timerId = setInterval(SlideUp, 5000);
console.log("set " + timerId)
function SlideUp() {
    if (slideIndex + 1 >= slidesCount) {
        slideIndex = 0;
    }
    else {
        slideIndex++;
    }
    onSlideUpdate();
}
function SlideDown() {
    console.log(slideIndex);
    if (slideIndex <= 0) {
        slideIndex = slidesCount - 1;
    }
    else {
        slideIndex--;
    }
    onSlideUpdate();
}
