var _SlideshowTransitions = [
    { $Duration: 400, y: 1, $Delay: 50, $Cols: 12, $Formation: $JssorSlideshowFormations$.$FormationStraight, $Assembly: 513, $Easing: { $Top: $Jease$.$InCubic, $Opacity: $Jease$.$OutQuad }, $Opacity: 2 },
    { $Duration: 400, $Delay: 50, $Cols: 10, $Clip: 2, $Formation: $JssorSlideshowFormations$.$FormationStraight },
    { $Duration: 400, $Delay: 40, $Cols: 16, $Formation: $JssorSlideshowFormations$.$FormationStraight, $Opacity: 2, $Assembly: 260 },
    { $Duration: 400, $Delay: 40, $Rows: 10, $Clip: 8, $Move: true, $Formation: $JssorSlideshowFormations$.$FormationCircle, $Assembly: 264, $Easing: $Jease$.$InBounce },
    { $Duration: 400, $Delay: 40, $Cols: 16, $Clip: 1, $Move: true, $Formation: $JssorSlideshowFormations$.$FormationCircle, $Assembly: 264, $Easing: $Jease$.$InBounce }
];

var options = {
    $AutoPlay: 1,
    $Loop: 0,
    $DragOrientation: 3,                        //[Optional] Orientation to drag slide, 0 no drag, 1 horizental, 2 vertical, 3 either, default value is 1 (Note that the $DragOrientation should be the same as $PlayOrientation when $Cols is greater than 1, or parking position is not 0)
    $SlideDuration: 500,                        //[Optional] Specifies default duration (swipe) for slide in milliseconds, default value is 500
    $SlideshowOptions: {
        $Class: $JssorSlideshowRunner$,
        $Transitions: _SlideshowTransitions,
        $TransitionsOrder: 1,
        $ShowLink: true
    },
    $ArrowNavigatorOptions: {                   //[Optional] Options to specify and enable arrow navigator or not
        $Class: $JssorArrowNavigator$,          //[Requried] Class to create arrow navigator instance
        $ChanceToShow: 1,                       //[Required] 0 Never, 1 Mouse Over, 2 Always
        $AutoCenter: 2,                         //[Optional] Auto center arrows in parent container, 0 No, 1 Horizontal, 2 Vertical, 3 Both, default value is 0
        $Steps: 1                               //[Optional] Steps to go for each navigation request, default value is 1
    },
    $BulletNavigatorOptions: {                  //[Optional] Options to specify and enable navigator or not
        $Class: $JssorBulletNavigator$,         //[Required] Class to create navigator instance
        $ChanceToShow: 2,                       //[Required] 0 Never, 1 Mouse Over, 2 Always
        $AutoCenter: 1,                         //[Optional] Auto center navigator in parent container, 0 None, 1 Horizontal, 2 Vertical, 3 Both, default value is 0
        $Steps: 1,                              //[Optional] Steps to go for each navigation request, default value is 1
        $Rows: 1,                               //[Optional] Specify lanes to arrange items, default value is 1
        $SpacingX: 10,                          //[Optional] Horizontal space between each item in pixel, default value is 0
        $SpacingY: 10,                          //[Optional] Vertical space between each item in pixel, default value is 0
        $Orientation: 1                         //[Optional] The orientation of the navigator, 1 horizontal, 2 vertical, default value is 1
    }
};

var jssor1_slider = new $JssorSlider$("banner_1", options);

function ScaleSlider() {
    var parentWidth = $('#banner_1').parent().width();
    if (parentWidth) {
        jssor1_slider.$ScaleWidth(parentWidth);
    }
    else
        window.setTimeout(ScaleSlider, 30);
}
//Scale slider after document ready
ScaleSlider();

//Scale slider while window load/resize/orientationchange.
$(window).bind("load", ScaleSlider);
$(window).bind("resize", ScaleSlider);
$(window).bind("orientationchange", ScaleSlider);
//responsive code end

$("#touch-me-button").on("click", function () {
    $("#div-btn-show-hide").toggle("2000");
    var data = $("#div-btn-show-hide").position();
    $('html, body').animate({
        scrollTop: parseInt(data.top)
    }, 2000);
})