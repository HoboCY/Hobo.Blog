var onResize = function () {
    $("body").css("padding-top", $(".navbar").height() + 25);
};

$(window).resize(onResize);

$(function () {
    onResize();
});