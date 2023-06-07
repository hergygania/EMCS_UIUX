let step = 'step1';

const step1 = document.getElementById('step1');
const step2 = document.getElementById('step2');
const step3 = document.getElementById('step3');
const step4 = document.getElementById('step4');

function next() {
    if (step === 'step1') {
        step = 'step2';
        step1.classList.remove("is-active");
        step1.classList.add("is-complete");
        step2.classList.add("is-active");

    } else if (step === 'step2') {
        step = 'step3';
        step2.classList.remove("is-active");
        step2.classList.add("is-complete");
        step3.classList.add("is-active");

    } else if (step === 'step3') {
        step = 'step4d';
        step3.classList.remove("is-active");
        step3.classList.add("is-complete");
        step4.classList.add("is-active");

    } else if (step === 'step4d') {
        step = 'complete';
        step4.classList.remove("is-active");
        step4.classList.add("is-complete");

    } else if (step === 'complete') {
        step = 'step1';
        step4.classList.remove("is-complete");
        step3.classList.remove("is-complete");
        step2.classList.remove("is-complete");
        step1.classList.remove("is-complete");
        step1.classList.add("is-active");
    }
}

(function () {
    var $point_arr, $points, $progress, $trigger, active, max, tracker, val;

    $trigger = $('.trigger').first();
    $points = $('.progress-points').first();
    $point_arr = $('.progress-point');
    $progress = $('.progress').first();

    val = +$points.data('current') - 1;
    max = $point_arr.length - 1;
    tracker = active = 0;

    function activate(index) {
        if (index !== active) {
            active = index;
            var $Active = $point_arr.eq(active);

            $point_arr
                .removeClass('completed active')
                .slice(0, active).addClass('completed');

            $Active.addClass('active');

            return $progress.css('width', (index / max * 100) + "%");
        }
        return $progress.css('width', (index / max * 100) + "%");
    };

    $points.on('click', 'li', function () {
        var _index;
        _index = $point_arr.index(this);
        tracker = _index === 0 ? 1 : _index === val ? 0 : tracker;

        return activate(_index);
    });

    $trigger.on('click', function () {
        return activate(tracker++ % 2 === 0 ? 0 : val);
    });

    setTimeout((function () {
        return activate(val);
    }), 1000);

// ReSharper disable once ThisInGlobalContext
}).call(this);