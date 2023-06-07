$(document).ready(function() {
	$("#triger-menu").on('click', function (event) {

        $("#aass").switchClass('invis','show', 0, "easeInOutQuad").queue(function (next) {
            $("#menu-cont").switchClass("iin", "out", 50, "easeInOutQuad");
            $("#menu-content-neo").switchClass("iin", "out", 50, "easeInOutQuad");
            //.removeClass('in').delay( 2000 ).addClass('out');
            next();
        });
    });
    $("#menu-container-neo").click(function () {
        $("#menu-content-neo").switchClass("out", "iin", 100, "easeInOutQuad").queue(function (next) {
            //$("#menu-content-neo").delay( 1000 ).removeClass('out').addClass('in');
            $("#menu-cont").switchClass('out', 'iin', 100, "easeInOutQuad");
            $("#aass").switchClass('show', 'invis', 0, "easeInOutQuad");
            next();
        });
    });
});