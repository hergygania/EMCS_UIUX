﻿/*
 Highcharts JS v7.1.3 (2019-08-14)

 Solid angular gauge module

 (c) 2010-2019 Torstein Honsi

 License: www.highcharts.com/license
*/
(function (b) { "object" === typeof module && module.exports ? (b["default"] = b, module.exports = b) : "function" === typeof define && define.amd ? define("highcharts/modules/solid-gauge", ["highcharts", "highcharts/highcharts-more"], function (e) { b(e); b.Highcharts = e; return b }) : b("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (b) {
    function e(l, b, e, r) { l.hasOwnProperty(b) || (l[b] = r.apply(null, e)) } b = b ? b._modules : {}; e(b, "modules/solid-gauge.src.js", [b["parts/Globals.js"], b["parts/Utilities.js"]], function (b, e) {
        var l =
            e.isNumber, r = e.pInt, t = b.pick; e = b.wrap; e(b.Renderer.prototype.symbols, "arc", function (a, b, f, d, g, c) { a = a(b, f, d, g, c); c.rounded && (d = ((c.r || d) - c.innerR) / 2, c = ["A", d, d, 0, 1, 1, a[12], a[13]], a.splice.apply(a, [a.length - 1, 0].concat(["A", d, d, 0, 1, 1, a[1], a[2]])), a.splice.apply(a, [11, 3].concat(c))); return a }); var v = {
                initDataClasses: function (a) {
                    var h = this.chart, f, d = 0, g = this.options; this.dataClasses = f = []; a.dataClasses.forEach(function (c, k) {
                        c = b.merge(c); f.push(c); c.color || ("category" === g.dataClassColor ? (k = h.options.colors,
                            c.color = k[d++], d === k.length && (d = 0)) : c.color = b.color(g.minColor).tweenTo(b.color(g.maxColor), k / (a.dataClasses.length - 1)))
                    })
                }, initStops: function (a) { this.stops = a.stops || [[0, this.options.minColor], [1, this.options.maxColor]]; this.stops.forEach(function (a) { a.color = b.color(a[1]) }) }, toColor: function (a, b) {
                    var f = this.stops, d = this.dataClasses, g; if (d) for (g = d.length; g--;) { var c = d[g]; var k = c.from; f = c.to; if ((void 0 === k || a >= k) && (void 0 === f || a <= f)) { var h = c.color; b && (b.dataClass = g); break } } else {
                    this.isLog && (a = this.val2lin(a));
                        a = 1 - (this.max - a) / (this.max - this.min); for (g = f.length; g-- && !(a > f[g][0]);); k = f[g] || f[g + 1]; f = f[g + 1] || k; a = 1 - (f[0] - a) / (f[0] - k[0] || 1); h = k.color.tweenTo(f.color, a)
                    } return h
                }
            }; b.seriesType("solidgauge", "gauge", { colorByPoint: !0, dataLabels: { y: 0 } }, {
                drawLegendSymbol: b.LegendSymbolMixin.drawRectangle, translate: function () { var a = this.yAxis; b.extend(a, v); !a.dataClasses && a.options.dataClasses && a.initDataClasses(a.options); a.initStops(a.options); b.seriesTypes.gauge.prototype.translate.call(this) }, drawPoints: function () {
                    var a =
                        this, h = a.yAxis, f = h.center, d = a.options, g = a.chart.renderer, c = d.overshoot, k = l(c) ? c / 180 * Math.PI : 0, e; l(d.threshold) && (e = h.startAngleRad + h.translate(d.threshold, null, null, null, !0)); this.thresholdAngleRad = t(e, h.startAngleRad); a.points.forEach(function (c) {
                            if (!c.isNull) {
                                var e = c.graphic, m = h.startAngleRad + h.translate(c.y, null, null, null, !0), l = r(t(c.options.radius, d.radius, 100)) * f[2] / 200, n = r(t(c.options.innerRadius, d.innerRadius, 60)) * f[2] / 200, p = h.toColor(c.y, c), q = Math.min(h.startAngleRad, h.endAngleRad), u = Math.max(h.startAngleRad,
                                    h.endAngleRad); "none" === p && (p = c.color || a.color || "none"); "none" !== p && (c.color = p); m = Math.max(q - k, Math.min(u + k, m)); !1 === d.wrap && (m = Math.max(q, Math.min(u, m))); q = Math.min(m, a.thresholdAngleRad); m = Math.max(m, a.thresholdAngleRad); m - q > 2 * Math.PI && (m = q + 2 * Math.PI); c.shapeArgs = n = { x: f[0], y: f[1], r: l, innerR: n, start: q, end: m, rounded: d.rounded }; c.startR = l; e ? (l = n.d, e.animate(b.extend({ fill: p }, n)), l && (n.d = l)) : (c.graphic = e = g.arc(n).attr({ fill: p, "sweep-flag": 0 }).add(a.group), a.chart.styledMode || ("square" !== d.linecap &&
                                        e.attr({ "stroke-linecap": "round", "stroke-linejoin": "round" }), e.attr({ stroke: d.borderColor || "none", "stroke-width": d.borderWidth || 0 }))); e && e.addClass(c.getClassName(), !0)
                            }
                        })
                }, animate: function (a) { a || (this.startAngleRad = this.thresholdAngleRad, b.seriesTypes.pie.prototype.animate.call(this, a)) }
            })
    }); e(b, "masters/modules/solid-gauge.src.js", [], function () { })
});
//# sourceMappingURL=solid-gauge.js.map