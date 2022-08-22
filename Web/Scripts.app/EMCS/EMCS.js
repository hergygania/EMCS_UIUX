(() => {
  // node_modules/perfect-scrollbar/dist/perfect-scrollbar.esm.js
  function get(element) {
    return getComputedStyle(element);
  }
  function set(element, obj) {
    for (var key in obj) {
      var val = obj[key];
      if (typeof val === "number") {
        val = val + "px";
      }
      element.style[key] = val;
    }
    return element;
  }
  function div(className) {
    var div2 = document.createElement("div");
    div2.className = className;
    return div2;
  }
  var elMatches = typeof Element !== "undefined" && (Element.prototype.matches || Element.prototype.webkitMatchesSelector || Element.prototype.mozMatchesSelector || Element.prototype.msMatchesSelector);
  function matches(element, query) {
    if (!elMatches) {
      throw new Error("No element matching method supported");
    }
    return elMatches.call(element, query);
  }
  function remove(element) {
    if (element.remove) {
      element.remove();
    } else {
      if (element.parentNode) {
        element.parentNode.removeChild(element);
      }
    }
  }
  function queryChildren(element, selector) {
    return Array.prototype.filter.call(
      element.children,
      function(child) {
        return matches(child, selector);
      }
    );
  }
  var cls = {
    main: "ps",
    rtl: "ps__rtl",
    element: {
      thumb: function(x) {
        return "ps__thumb-" + x;
      },
      rail: function(x) {
        return "ps__rail-" + x;
      },
      consuming: "ps__child--consume"
    },
    state: {
      focus: "ps--focus",
      clicking: "ps--clicking",
      active: function(x) {
        return "ps--active-" + x;
      },
      scrolling: function(x) {
        return "ps--scrolling-" + x;
      }
    }
  };
  var scrollingClassTimeout = { x: null, y: null };
  function addScrollingClass(i, x) {
    var classList = i.element.classList;
    var className = cls.state.scrolling(x);
    if (classList.contains(className)) {
      clearTimeout(scrollingClassTimeout[x]);
    } else {
      classList.add(className);
    }
  }
  function removeScrollingClass(i, x) {
    scrollingClassTimeout[x] = setTimeout(
      function() {
        return i.isAlive && i.element.classList.remove(cls.state.scrolling(x));
      },
      i.settings.scrollingThreshold
    );
  }
  function setScrollingClassInstantly(i, x) {
    addScrollingClass(i, x);
    removeScrollingClass(i, x);
  }
  var EventElement = function EventElement2(element) {
    this.element = element;
    this.handlers = {};
  };
  var prototypeAccessors = { isEmpty: { configurable: true } };
  EventElement.prototype.bind = function bind(eventName, handler) {
    if (typeof this.handlers[eventName] === "undefined") {
      this.handlers[eventName] = [];
    }
    this.handlers[eventName].push(handler);
    this.element.addEventListener(eventName, handler, false);
  };
  EventElement.prototype.unbind = function unbind(eventName, target) {
    var this$1 = this;
    this.handlers[eventName] = this.handlers[eventName].filter(function(handler) {
      if (target && handler !== target) {
        return true;
      }
      this$1.element.removeEventListener(eventName, handler, false);
      return false;
    });
  };
  EventElement.prototype.unbindAll = function unbindAll() {
    for (var name in this.handlers) {
      this.unbind(name);
    }
  };
  prototypeAccessors.isEmpty.get = function() {
    var this$1 = this;
    return Object.keys(this.handlers).every(
      function(key) {
        return this$1.handlers[key].length === 0;
      }
    );
  };
  Object.defineProperties(EventElement.prototype, prototypeAccessors);
  var EventManager = function EventManager2() {
    this.eventElements = [];
  };
  EventManager.prototype.eventElement = function eventElement(element) {
    var ee = this.eventElements.filter(function(ee2) {
      return ee2.element === element;
    })[0];
    if (!ee) {
      ee = new EventElement(element);
      this.eventElements.push(ee);
    }
    return ee;
  };
  EventManager.prototype.bind = function bind2(element, eventName, handler) {
    this.eventElement(element).bind(eventName, handler);
  };
  EventManager.prototype.unbind = function unbind2(element, eventName, handler) {
    var ee = this.eventElement(element);
    ee.unbind(eventName, handler);
    if (ee.isEmpty) {
      this.eventElements.splice(this.eventElements.indexOf(ee), 1);
    }
  };
  EventManager.prototype.unbindAll = function unbindAll2() {
    this.eventElements.forEach(function(e) {
      return e.unbindAll();
    });
    this.eventElements = [];
  };
  EventManager.prototype.once = function once(element, eventName, handler) {
    var ee = this.eventElement(element);
    var onceHandler = function(evt) {
      ee.unbind(eventName, onceHandler);
      handler(evt);
    };
    ee.bind(eventName, onceHandler);
  };
  function createEvent(name) {
    if (typeof window.CustomEvent === "function") {
      return new CustomEvent(name);
    } else {
      var evt = document.createEvent("CustomEvent");
      evt.initCustomEvent(name, false, false, void 0);
      return evt;
    }
  }
  function processScrollDiff(i, axis, diff, useScrollingClass, forceFireReachEvent) {
    if (useScrollingClass === void 0)
      useScrollingClass = true;
    if (forceFireReachEvent === void 0)
      forceFireReachEvent = false;
    var fields;
    if (axis === "top") {
      fields = [
        "contentHeight",
        "containerHeight",
        "scrollTop",
        "y",
        "up",
        "down"
      ];
    } else if (axis === "left") {
      fields = [
        "contentWidth",
        "containerWidth",
        "scrollLeft",
        "x",
        "left",
        "right"
      ];
    } else {
      throw new Error("A proper axis should be provided");
    }
    processScrollDiff$1(i, diff, fields, useScrollingClass, forceFireReachEvent);
  }
  function processScrollDiff$1(i, diff, ref, useScrollingClass, forceFireReachEvent) {
    var contentHeight = ref[0];
    var containerHeight = ref[1];
    var scrollTop = ref[2];
    var y = ref[3];
    var up = ref[4];
    var down = ref[5];
    if (useScrollingClass === void 0)
      useScrollingClass = true;
    if (forceFireReachEvent === void 0)
      forceFireReachEvent = false;
    var element = i.element;
    i.reach[y] = null;
    if (element[scrollTop] < 1) {
      i.reach[y] = "start";
    }
    if (element[scrollTop] > i[contentHeight] - i[containerHeight] - 1) {
      i.reach[y] = "end";
    }
    if (diff) {
      element.dispatchEvent(createEvent("ps-scroll-" + y));
      if (diff < 0) {
        element.dispatchEvent(createEvent("ps-scroll-" + up));
      } else if (diff > 0) {
        element.dispatchEvent(createEvent("ps-scroll-" + down));
      }
      if (useScrollingClass) {
        setScrollingClassInstantly(i, y);
      }
    }
    if (i.reach[y] && (diff || forceFireReachEvent)) {
      element.dispatchEvent(createEvent("ps-" + y + "-reach-" + i.reach[y]));
    }
  }
  function toInt(x) {
    return parseInt(x, 10) || 0;
  }
  function isEditable(el) {
    return matches(el, "input,[contenteditable]") || matches(el, "select,[contenteditable]") || matches(el, "textarea,[contenteditable]") || matches(el, "button,[contenteditable]");
  }
  function outerWidth(element) {
    var styles = get(element);
    return toInt(styles.width) + toInt(styles.paddingLeft) + toInt(styles.paddingRight) + toInt(styles.borderLeftWidth) + toInt(styles.borderRightWidth);
  }
  var env = {
    isWebKit: typeof document !== "undefined" && "WebkitAppearance" in document.documentElement.style,
    supportsTouch: typeof window !== "undefined" && ("ontouchstart" in window || "maxTouchPoints" in window.navigator && window.navigator.maxTouchPoints > 0 || window.DocumentTouch && document instanceof window.DocumentTouch),
    supportsIePointer: typeof navigator !== "undefined" && navigator.msMaxTouchPoints,
    isChrome: typeof navigator !== "undefined" && /Chrome/i.test(navigator && navigator.userAgent)
  };
  function updateGeometry(i) {
    var element = i.element;
    var roundedScrollTop = Math.floor(element.scrollTop);
    var rect = element.getBoundingClientRect();
    i.containerWidth = Math.round(rect.width);
    i.containerHeight = Math.round(rect.height);
    i.contentWidth = element.scrollWidth;
    i.contentHeight = element.scrollHeight;
    if (!element.contains(i.scrollbarXRail)) {
      queryChildren(element, cls.element.rail("x")).forEach(
        function(el) {
          return remove(el);
        }
      );
      element.appendChild(i.scrollbarXRail);
    }
    if (!element.contains(i.scrollbarYRail)) {
      queryChildren(element, cls.element.rail("y")).forEach(
        function(el) {
          return remove(el);
        }
      );
      element.appendChild(i.scrollbarYRail);
    }
    if (!i.settings.suppressScrollX && i.containerWidth + i.settings.scrollXMarginOffset < i.contentWidth) {
      i.scrollbarXActive = true;
      i.railXWidth = i.containerWidth - i.railXMarginWidth;
      i.railXRatio = i.containerWidth / i.railXWidth;
      i.scrollbarXWidth = getThumbSize(
        i,
        toInt(i.railXWidth * i.containerWidth / i.contentWidth)
      );
      i.scrollbarXLeft = toInt(
        (i.negativeScrollAdjustment + element.scrollLeft) * (i.railXWidth - i.scrollbarXWidth) / (i.contentWidth - i.containerWidth)
      );
    } else {
      i.scrollbarXActive = false;
    }
    if (!i.settings.suppressScrollY && i.containerHeight + i.settings.scrollYMarginOffset < i.contentHeight) {
      i.scrollbarYActive = true;
      i.railYHeight = i.containerHeight - i.railYMarginHeight;
      i.railYRatio = i.containerHeight / i.railYHeight;
      i.scrollbarYHeight = getThumbSize(
        i,
        toInt(i.railYHeight * i.containerHeight / i.contentHeight)
      );
      i.scrollbarYTop = toInt(
        roundedScrollTop * (i.railYHeight - i.scrollbarYHeight) / (i.contentHeight - i.containerHeight)
      );
    } else {
      i.scrollbarYActive = false;
    }
    if (i.scrollbarXLeft >= i.railXWidth - i.scrollbarXWidth) {
      i.scrollbarXLeft = i.railXWidth - i.scrollbarXWidth;
    }
    if (i.scrollbarYTop >= i.railYHeight - i.scrollbarYHeight) {
      i.scrollbarYTop = i.railYHeight - i.scrollbarYHeight;
    }
    updateCss(element, i);
    if (i.scrollbarXActive) {
      element.classList.add(cls.state.active("x"));
    } else {
      element.classList.remove(cls.state.active("x"));
      i.scrollbarXWidth = 0;
      i.scrollbarXLeft = 0;
      element.scrollLeft = i.isRtl === true ? i.contentWidth : 0;
    }
    if (i.scrollbarYActive) {
      element.classList.add(cls.state.active("y"));
    } else {
      element.classList.remove(cls.state.active("y"));
      i.scrollbarYHeight = 0;
      i.scrollbarYTop = 0;
      element.scrollTop = 0;
    }
  }
  function getThumbSize(i, thumbSize) {
    if (i.settings.minScrollbarLength) {
      thumbSize = Math.max(thumbSize, i.settings.minScrollbarLength);
    }
    if (i.settings.maxScrollbarLength) {
      thumbSize = Math.min(thumbSize, i.settings.maxScrollbarLength);
    }
    return thumbSize;
  }
  function updateCss(element, i) {
    var xRailOffset = { width: i.railXWidth };
    var roundedScrollTop = Math.floor(element.scrollTop);
    if (i.isRtl) {
      xRailOffset.left = i.negativeScrollAdjustment + element.scrollLeft + i.containerWidth - i.contentWidth;
    } else {
      xRailOffset.left = element.scrollLeft;
    }
    if (i.isScrollbarXUsingBottom) {
      xRailOffset.bottom = i.scrollbarXBottom - roundedScrollTop;
    } else {
      xRailOffset.top = i.scrollbarXTop + roundedScrollTop;
    }
    set(i.scrollbarXRail, xRailOffset);
    var yRailOffset = { top: roundedScrollTop, height: i.railYHeight };
    if (i.isScrollbarYUsingRight) {
      if (i.isRtl) {
        yRailOffset.right = i.contentWidth - (i.negativeScrollAdjustment + element.scrollLeft) - i.scrollbarYRight - i.scrollbarYOuterWidth - 9;
      } else {
        yRailOffset.right = i.scrollbarYRight - element.scrollLeft;
      }
    } else {
      if (i.isRtl) {
        yRailOffset.left = i.negativeScrollAdjustment + element.scrollLeft + i.containerWidth * 2 - i.contentWidth - i.scrollbarYLeft - i.scrollbarYOuterWidth;
      } else {
        yRailOffset.left = i.scrollbarYLeft + element.scrollLeft;
      }
    }
    set(i.scrollbarYRail, yRailOffset);
    set(i.scrollbarX, {
      left: i.scrollbarXLeft,
      width: i.scrollbarXWidth - i.railBorderXWidth
    });
    set(i.scrollbarY, {
      top: i.scrollbarYTop,
      height: i.scrollbarYHeight - i.railBorderYWidth
    });
  }
  function clickRail(i) {
    var element = i.element;
    i.event.bind(i.scrollbarY, "mousedown", function(e) {
      return e.stopPropagation();
    });
    i.event.bind(i.scrollbarYRail, "mousedown", function(e) {
      var positionTop = e.pageY - window.pageYOffset - i.scrollbarYRail.getBoundingClientRect().top;
      var direction = positionTop > i.scrollbarYTop ? 1 : -1;
      i.element.scrollTop += direction * i.containerHeight;
      updateGeometry(i);
      e.stopPropagation();
    });
    i.event.bind(i.scrollbarX, "mousedown", function(e) {
      return e.stopPropagation();
    });
    i.event.bind(i.scrollbarXRail, "mousedown", function(e) {
      var positionLeft = e.pageX - window.pageXOffset - i.scrollbarXRail.getBoundingClientRect().left;
      var direction = positionLeft > i.scrollbarXLeft ? 1 : -1;
      i.element.scrollLeft += direction * i.containerWidth;
      updateGeometry(i);
      e.stopPropagation();
    });
  }
  function dragThumb(i) {
    bindMouseScrollHandler(i, [
      "containerWidth",
      "contentWidth",
      "pageX",
      "railXWidth",
      "scrollbarX",
      "scrollbarXWidth",
      "scrollLeft",
      "x",
      "scrollbarXRail"
    ]);
    bindMouseScrollHandler(i, [
      "containerHeight",
      "contentHeight",
      "pageY",
      "railYHeight",
      "scrollbarY",
      "scrollbarYHeight",
      "scrollTop",
      "y",
      "scrollbarYRail"
    ]);
  }
  function bindMouseScrollHandler(i, ref) {
    var containerHeight = ref[0];
    var contentHeight = ref[1];
    var pageY = ref[2];
    var railYHeight = ref[3];
    var scrollbarY = ref[4];
    var scrollbarYHeight = ref[5];
    var scrollTop = ref[6];
    var y = ref[7];
    var scrollbarYRail = ref[8];
    var element = i.element;
    var startingScrollTop = null;
    var startingMousePageY = null;
    var scrollBy = null;
    function mouseMoveHandler(e) {
      if (e.touches && e.touches[0]) {
        e[pageY] = e.touches[0].pageY;
      }
      element[scrollTop] = startingScrollTop + scrollBy * (e[pageY] - startingMousePageY);
      addScrollingClass(i, y);
      updateGeometry(i);
      e.stopPropagation();
      if (e.type.startsWith("touch") && e.changedTouches.length > 1) {
        e.preventDefault();
      }
    }
    function mouseUpHandler() {
      removeScrollingClass(i, y);
      i[scrollbarYRail].classList.remove(cls.state.clicking);
      i.event.unbind(i.ownerDocument, "mousemove", mouseMoveHandler);
    }
    function bindMoves(e, touchMode) {
      startingScrollTop = element[scrollTop];
      if (touchMode && e.touches) {
        e[pageY] = e.touches[0].pageY;
      }
      startingMousePageY = e[pageY];
      scrollBy = (i[contentHeight] - i[containerHeight]) / (i[railYHeight] - i[scrollbarYHeight]);
      if (!touchMode) {
        i.event.bind(i.ownerDocument, "mousemove", mouseMoveHandler);
        i.event.once(i.ownerDocument, "mouseup", mouseUpHandler);
        e.preventDefault();
      } else {
        i.event.bind(i.ownerDocument, "touchmove", mouseMoveHandler);
      }
      i[scrollbarYRail].classList.add(cls.state.clicking);
      e.stopPropagation();
    }
    i.event.bind(i[scrollbarY], "mousedown", function(e) {
      bindMoves(e);
    });
    i.event.bind(i[scrollbarY], "touchstart", function(e) {
      bindMoves(e, true);
    });
  }
  function keyboard(i) {
    var element = i.element;
    var elementHovered = function() {
      return matches(element, ":hover");
    };
    var scrollbarFocused = function() {
      return matches(i.scrollbarX, ":focus") || matches(i.scrollbarY, ":focus");
    };
    function shouldPreventDefault(deltaX, deltaY) {
      var scrollTop = Math.floor(element.scrollTop);
      if (deltaX === 0) {
        if (!i.scrollbarYActive) {
          return false;
        }
        if (scrollTop === 0 && deltaY > 0 || scrollTop >= i.contentHeight - i.containerHeight && deltaY < 0) {
          return !i.settings.wheelPropagation;
        }
      }
      var scrollLeft = element.scrollLeft;
      if (deltaY === 0) {
        if (!i.scrollbarXActive) {
          return false;
        }
        if (scrollLeft === 0 && deltaX < 0 || scrollLeft >= i.contentWidth - i.containerWidth && deltaX > 0) {
          return !i.settings.wheelPropagation;
        }
      }
      return true;
    }
    i.event.bind(i.ownerDocument, "keydown", function(e) {
      if (e.isDefaultPrevented && e.isDefaultPrevented() || e.defaultPrevented) {
        return;
      }
      if (!elementHovered() && !scrollbarFocused()) {
        return;
      }
      var activeElement = document.activeElement ? document.activeElement : i.ownerDocument.activeElement;
      if (activeElement) {
        if (activeElement.tagName === "IFRAME") {
          activeElement = activeElement.contentDocument.activeElement;
        } else {
          while (activeElement.shadowRoot) {
            activeElement = activeElement.shadowRoot.activeElement;
          }
        }
        if (isEditable(activeElement)) {
          return;
        }
      }
      var deltaX = 0;
      var deltaY = 0;
      switch (e.which) {
        case 37:
          if (e.metaKey) {
            deltaX = -i.contentWidth;
          } else if (e.altKey) {
            deltaX = -i.containerWidth;
          } else {
            deltaX = -30;
          }
          break;
        case 38:
          if (e.metaKey) {
            deltaY = i.contentHeight;
          } else if (e.altKey) {
            deltaY = i.containerHeight;
          } else {
            deltaY = 30;
          }
          break;
        case 39:
          if (e.metaKey) {
            deltaX = i.contentWidth;
          } else if (e.altKey) {
            deltaX = i.containerWidth;
          } else {
            deltaX = 30;
          }
          break;
        case 40:
          if (e.metaKey) {
            deltaY = -i.contentHeight;
          } else if (e.altKey) {
            deltaY = -i.containerHeight;
          } else {
            deltaY = -30;
          }
          break;
        case 32:
          if (e.shiftKey) {
            deltaY = i.containerHeight;
          } else {
            deltaY = -i.containerHeight;
          }
          break;
        case 33:
          deltaY = i.containerHeight;
          break;
        case 34:
          deltaY = -i.containerHeight;
          break;
        case 36:
          deltaY = i.contentHeight;
          break;
        case 35:
          deltaY = -i.contentHeight;
          break;
        default:
          return;
      }
      if (i.settings.suppressScrollX && deltaX !== 0) {
        return;
      }
      if (i.settings.suppressScrollY && deltaY !== 0) {
        return;
      }
      element.scrollTop -= deltaY;
      element.scrollLeft += deltaX;
      updateGeometry(i);
      if (shouldPreventDefault(deltaX, deltaY)) {
        e.preventDefault();
      }
    });
  }
  function wheel(i) {
    var element = i.element;
    function shouldPreventDefault(deltaX, deltaY) {
      var roundedScrollTop = Math.floor(element.scrollTop);
      var isTop = element.scrollTop === 0;
      var isBottom = roundedScrollTop + element.offsetHeight === element.scrollHeight;
      var isLeft = element.scrollLeft === 0;
      var isRight = element.scrollLeft + element.offsetWidth === element.scrollWidth;
      var hitsBound;
      if (Math.abs(deltaY) > Math.abs(deltaX)) {
        hitsBound = isTop || isBottom;
      } else {
        hitsBound = isLeft || isRight;
      }
      return hitsBound ? !i.settings.wheelPropagation : true;
    }
    function getDeltaFromEvent(e) {
      var deltaX = e.deltaX;
      var deltaY = -1 * e.deltaY;
      if (typeof deltaX === "undefined" || typeof deltaY === "undefined") {
        deltaX = -1 * e.wheelDeltaX / 6;
        deltaY = e.wheelDeltaY / 6;
      }
      if (e.deltaMode && e.deltaMode === 1) {
        deltaX *= 10;
        deltaY *= 10;
      }
      if (deltaX !== deltaX && deltaY !== deltaY) {
        deltaX = 0;
        deltaY = e.wheelDelta;
      }
      if (e.shiftKey) {
        return [-deltaY, -deltaX];
      }
      return [deltaX, deltaY];
    }
    function shouldBeConsumedByChild(target, deltaX, deltaY) {
      if (!env.isWebKit && element.querySelector("select:focus")) {
        return true;
      }
      if (!element.contains(target)) {
        return false;
      }
      var cursor = target;
      while (cursor && cursor !== element) {
        if (cursor.classList.contains(cls.element.consuming)) {
          return true;
        }
        var style = get(cursor);
        if (deltaY && style.overflowY.match(/(scroll|auto)/)) {
          var maxScrollTop = cursor.scrollHeight - cursor.clientHeight;
          if (maxScrollTop > 0) {
            if (cursor.scrollTop > 0 && deltaY < 0 || cursor.scrollTop < maxScrollTop && deltaY > 0) {
              return true;
            }
          }
        }
        if (deltaX && style.overflowX.match(/(scroll|auto)/)) {
          var maxScrollLeft = cursor.scrollWidth - cursor.clientWidth;
          if (maxScrollLeft > 0) {
            if (cursor.scrollLeft > 0 && deltaX < 0 || cursor.scrollLeft < maxScrollLeft && deltaX > 0) {
              return true;
            }
          }
        }
        cursor = cursor.parentNode;
      }
      return false;
    }
    function mousewheelHandler(e) {
      var ref = getDeltaFromEvent(e);
      var deltaX = ref[0];
      var deltaY = ref[1];
      if (shouldBeConsumedByChild(e.target, deltaX, deltaY)) {
        return;
      }
      var shouldPrevent = false;
      if (!i.settings.useBothWheelAxes) {
        element.scrollTop -= deltaY * i.settings.wheelSpeed;
        element.scrollLeft += deltaX * i.settings.wheelSpeed;
      } else if (i.scrollbarYActive && !i.scrollbarXActive) {
        if (deltaY) {
          element.scrollTop -= deltaY * i.settings.wheelSpeed;
        } else {
          element.scrollTop += deltaX * i.settings.wheelSpeed;
        }
        shouldPrevent = true;
      } else if (i.scrollbarXActive && !i.scrollbarYActive) {
        if (deltaX) {
          element.scrollLeft += deltaX * i.settings.wheelSpeed;
        } else {
          element.scrollLeft -= deltaY * i.settings.wheelSpeed;
        }
        shouldPrevent = true;
      }
      updateGeometry(i);
      shouldPrevent = shouldPrevent || shouldPreventDefault(deltaX, deltaY);
      if (shouldPrevent && !e.ctrlKey) {
        e.stopPropagation();
        e.preventDefault();
      }
    }
    if (typeof window.onwheel !== "undefined") {
      i.event.bind(element, "wheel", mousewheelHandler);
    } else if (typeof window.onmousewheel !== "undefined") {
      i.event.bind(element, "mousewheel", mousewheelHandler);
    }
  }
  function touch(i) {
    if (!env.supportsTouch && !env.supportsIePointer) {
      return;
    }
    var element = i.element;
    function shouldPrevent(deltaX, deltaY) {
      var scrollTop = Math.floor(element.scrollTop);
      var scrollLeft = element.scrollLeft;
      var magnitudeX = Math.abs(deltaX);
      var magnitudeY = Math.abs(deltaY);
      if (magnitudeY > magnitudeX) {
        if (deltaY < 0 && scrollTop === i.contentHeight - i.containerHeight || deltaY > 0 && scrollTop === 0) {
          return window.scrollY === 0 && deltaY > 0 && env.isChrome;
        }
      } else if (magnitudeX > magnitudeY) {
        if (deltaX < 0 && scrollLeft === i.contentWidth - i.containerWidth || deltaX > 0 && scrollLeft === 0) {
          return true;
        }
      }
      return true;
    }
    function applyTouchMove(differenceX, differenceY) {
      element.scrollTop -= differenceY;
      element.scrollLeft -= differenceX;
      updateGeometry(i);
    }
    var startOffset = {};
    var startTime = 0;
    var speed = {};
    var easingLoop = null;
    function getTouch(e) {
      if (e.targetTouches) {
        return e.targetTouches[0];
      } else {
        return e;
      }
    }
    function shouldHandle(e) {
      if (e.pointerType && e.pointerType === "pen" && e.buttons === 0) {
        return false;
      }
      if (e.targetTouches && e.targetTouches.length === 1) {
        return true;
      }
      if (e.pointerType && e.pointerType !== "mouse" && e.pointerType !== e.MSPOINTER_TYPE_MOUSE) {
        return true;
      }
      return false;
    }
    function touchStart(e) {
      if (!shouldHandle(e)) {
        return;
      }
      var touch2 = getTouch(e);
      startOffset.pageX = touch2.pageX;
      startOffset.pageY = touch2.pageY;
      startTime = new Date().getTime();
      if (easingLoop !== null) {
        clearInterval(easingLoop);
      }
    }
    function shouldBeConsumedByChild(target, deltaX, deltaY) {
      if (!element.contains(target)) {
        return false;
      }
      var cursor = target;
      while (cursor && cursor !== element) {
        if (cursor.classList.contains(cls.element.consuming)) {
          return true;
        }
        var style = get(cursor);
        if (deltaY && style.overflowY.match(/(scroll|auto)/)) {
          var maxScrollTop = cursor.scrollHeight - cursor.clientHeight;
          if (maxScrollTop > 0) {
            if (cursor.scrollTop > 0 && deltaY < 0 || cursor.scrollTop < maxScrollTop && deltaY > 0) {
              return true;
            }
          }
        }
        if (deltaX && style.overflowX.match(/(scroll|auto)/)) {
          var maxScrollLeft = cursor.scrollWidth - cursor.clientWidth;
          if (maxScrollLeft > 0) {
            if (cursor.scrollLeft > 0 && deltaX < 0 || cursor.scrollLeft < maxScrollLeft && deltaX > 0) {
              return true;
            }
          }
        }
        cursor = cursor.parentNode;
      }
      return false;
    }
    function touchMove(e) {
      if (shouldHandle(e)) {
        var touch2 = getTouch(e);
        var currentOffset = { pageX: touch2.pageX, pageY: touch2.pageY };
        var differenceX = currentOffset.pageX - startOffset.pageX;
        var differenceY = currentOffset.pageY - startOffset.pageY;
        if (shouldBeConsumedByChild(e.target, differenceX, differenceY)) {
          return;
        }
        applyTouchMove(differenceX, differenceY);
        startOffset = currentOffset;
        var currentTime = new Date().getTime();
        var timeGap = currentTime - startTime;
        if (timeGap > 0) {
          speed.x = differenceX / timeGap;
          speed.y = differenceY / timeGap;
          startTime = currentTime;
        }
        if (shouldPrevent(differenceX, differenceY)) {
          e.preventDefault();
        }
      }
    }
    function touchEnd() {
      if (i.settings.swipeEasing) {
        clearInterval(easingLoop);
        easingLoop = setInterval(function() {
          if (i.isInitialized) {
            clearInterval(easingLoop);
            return;
          }
          if (!speed.x && !speed.y) {
            clearInterval(easingLoop);
            return;
          }
          if (Math.abs(speed.x) < 0.01 && Math.abs(speed.y) < 0.01) {
            clearInterval(easingLoop);
            return;
          }
          if (!i.element) {
            clearInterval(easingLoop);
            return;
          }
          applyTouchMove(speed.x * 30, speed.y * 30);
          speed.x *= 0.8;
          speed.y *= 0.8;
        }, 10);
      }
    }
    if (env.supportsTouch) {
      i.event.bind(element, "touchstart", touchStart);
      i.event.bind(element, "touchmove", touchMove);
      i.event.bind(element, "touchend", touchEnd);
    } else if (env.supportsIePointer) {
      if (window.PointerEvent) {
        i.event.bind(element, "pointerdown", touchStart);
        i.event.bind(element, "pointermove", touchMove);
        i.event.bind(element, "pointerup", touchEnd);
      } else if (window.MSPointerEvent) {
        i.event.bind(element, "MSPointerDown", touchStart);
        i.event.bind(element, "MSPointerMove", touchMove);
        i.event.bind(element, "MSPointerUp", touchEnd);
      }
    }
  }
  var defaultSettings = function() {
    return {
      handlers: ["click-rail", "drag-thumb", "keyboard", "wheel", "touch"],
      maxScrollbarLength: null,
      minScrollbarLength: null,
      scrollingThreshold: 1e3,
      scrollXMarginOffset: 0,
      scrollYMarginOffset: 0,
      suppressScrollX: false,
      suppressScrollY: false,
      swipeEasing: true,
      useBothWheelAxes: false,
      wheelPropagation: true,
      wheelSpeed: 1
    };
  };
  var handlers = {
    "click-rail": clickRail,
    "drag-thumb": dragThumb,
    keyboard,
    wheel,
    touch
  };
  var PerfectScrollbar = function PerfectScrollbar2(element, userSettings) {
    var this$1 = this;
    if (userSettings === void 0)
      userSettings = {};
    if (typeof element === "string") {
      element = document.querySelector(element);
    }
    if (!element || !element.nodeName) {
      throw new Error("no element is specified to initialize PerfectScrollbar");
    }
    this.element = element;
    element.classList.add(cls.main);
    this.settings = defaultSettings();
    for (var key in userSettings) {
      this.settings[key] = userSettings[key];
    }
    this.containerWidth = null;
    this.containerHeight = null;
    this.contentWidth = null;
    this.contentHeight = null;
    var focus = function() {
      return element.classList.add(cls.state.focus);
    };
    var blur = function() {
      return element.classList.remove(cls.state.focus);
    };
    this.isRtl = get(element).direction === "rtl";
    if (this.isRtl === true) {
      element.classList.add(cls.rtl);
    }
    this.isNegativeScroll = function() {
      var originalScrollLeft = element.scrollLeft;
      var result = null;
      element.scrollLeft = -1;
      result = element.scrollLeft < 0;
      element.scrollLeft = originalScrollLeft;
      return result;
    }();
    this.negativeScrollAdjustment = this.isNegativeScroll ? element.scrollWidth - element.clientWidth : 0;
    this.event = new EventManager();
    this.ownerDocument = element.ownerDocument || document;
    this.scrollbarXRail = div(cls.element.rail("x"));
    element.appendChild(this.scrollbarXRail);
    this.scrollbarX = div(cls.element.thumb("x"));
    this.scrollbarXRail.appendChild(this.scrollbarX);
    this.scrollbarX.setAttribute("tabindex", 0);
    this.event.bind(this.scrollbarX, "focus", focus);
    this.event.bind(this.scrollbarX, "blur", blur);
    this.scrollbarXActive = null;
    this.scrollbarXWidth = null;
    this.scrollbarXLeft = null;
    var railXStyle = get(this.scrollbarXRail);
    this.scrollbarXBottom = parseInt(railXStyle.bottom, 10);
    if (isNaN(this.scrollbarXBottom)) {
      this.isScrollbarXUsingBottom = false;
      this.scrollbarXTop = toInt(railXStyle.top);
    } else {
      this.isScrollbarXUsingBottom = true;
    }
    this.railBorderXWidth = toInt(railXStyle.borderLeftWidth) + toInt(railXStyle.borderRightWidth);
    set(this.scrollbarXRail, { display: "block" });
    this.railXMarginWidth = toInt(railXStyle.marginLeft) + toInt(railXStyle.marginRight);
    set(this.scrollbarXRail, { display: "" });
    this.railXWidth = null;
    this.railXRatio = null;
    this.scrollbarYRail = div(cls.element.rail("y"));
    element.appendChild(this.scrollbarYRail);
    this.scrollbarY = div(cls.element.thumb("y"));
    this.scrollbarYRail.appendChild(this.scrollbarY);
    this.scrollbarY.setAttribute("tabindex", 0);
    this.event.bind(this.scrollbarY, "focus", focus);
    this.event.bind(this.scrollbarY, "blur", blur);
    this.scrollbarYActive = null;
    this.scrollbarYHeight = null;
    this.scrollbarYTop = null;
    var railYStyle = get(this.scrollbarYRail);
    this.scrollbarYRight = parseInt(railYStyle.right, 10);
    if (isNaN(this.scrollbarYRight)) {
      this.isScrollbarYUsingRight = false;
      this.scrollbarYLeft = toInt(railYStyle.left);
    } else {
      this.isScrollbarYUsingRight = true;
    }
    this.scrollbarYOuterWidth = this.isRtl ? outerWidth(this.scrollbarY) : null;
    this.railBorderYWidth = toInt(railYStyle.borderTopWidth) + toInt(railYStyle.borderBottomWidth);
    set(this.scrollbarYRail, { display: "block" });
    this.railYMarginHeight = toInt(railYStyle.marginTop) + toInt(railYStyle.marginBottom);
    set(this.scrollbarYRail, { display: "" });
    this.railYHeight = null;
    this.railYRatio = null;
    this.reach = {
      x: element.scrollLeft <= 0 ? "start" : element.scrollLeft >= this.contentWidth - this.containerWidth ? "end" : null,
      y: element.scrollTop <= 0 ? "start" : element.scrollTop >= this.contentHeight - this.containerHeight ? "end" : null
    };
    this.isAlive = true;
    this.settings.handlers.forEach(function(handlerName) {
      return handlers[handlerName](this$1);
    });
    this.lastScrollTop = Math.floor(element.scrollTop);
    this.lastScrollLeft = element.scrollLeft;
    this.event.bind(this.element, "scroll", function(e) {
      return this$1.onScroll(e);
    });
    updateGeometry(this);
  };
  PerfectScrollbar.prototype.update = function update() {
    if (!this.isAlive) {
      return;
    }
    this.negativeScrollAdjustment = this.isNegativeScroll ? this.element.scrollWidth - this.element.clientWidth : 0;
    set(this.scrollbarXRail, { display: "block" });
    set(this.scrollbarYRail, { display: "block" });
    this.railXMarginWidth = toInt(get(this.scrollbarXRail).marginLeft) + toInt(get(this.scrollbarXRail).marginRight);
    this.railYMarginHeight = toInt(get(this.scrollbarYRail).marginTop) + toInt(get(this.scrollbarYRail).marginBottom);
    set(this.scrollbarXRail, { display: "none" });
    set(this.scrollbarYRail, { display: "none" });
    updateGeometry(this);
    processScrollDiff(this, "top", 0, false, true);
    processScrollDiff(this, "left", 0, false, true);
    set(this.scrollbarXRail, { display: "" });
    set(this.scrollbarYRail, { display: "" });
  };
  PerfectScrollbar.prototype.onScroll = function onScroll(e) {
    if (!this.isAlive) {
      return;
    }
    updateGeometry(this);
    processScrollDiff(this, "top", this.element.scrollTop - this.lastScrollTop);
    processScrollDiff(
      this,
      "left",
      this.element.scrollLeft - this.lastScrollLeft
    );
    this.lastScrollTop = Math.floor(this.element.scrollTop);
    this.lastScrollLeft = this.element.scrollLeft;
  };
  PerfectScrollbar.prototype.destroy = function destroy() {
    if (!this.isAlive) {
      return;
    }
    this.event.unbindAll();
    remove(this.scrollbarX);
    remove(this.scrollbarY);
    remove(this.scrollbarXRail);
    remove(this.scrollbarYRail);
    this.removePsClasses();
    this.element = null;
    this.scrollbarX = null;
    this.scrollbarY = null;
    this.scrollbarXRail = null;
    this.scrollbarYRail = null;
    this.isAlive = false;
  };
  PerfectScrollbar.prototype.removePsClasses = function removePsClasses() {
    this.element.className = this.element.className.split(" ").filter(function(name) {
      return !name.match(/^ps([-_].+|)$/);
    }).join(" ");
  };
  var perfect_scrollbar_esm_default = PerfectScrollbar;

  // node_modules/flowbite/dist/flowbite.js
  (() => {
    "use strict";
    var __webpack_exports__ = {};
    ;
    function _toConsumableArray(arr) {
      return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _unsupportedIterableToArray(arr) || _nonIterableSpread();
    }
    function _nonIterableSpread() {
      throw new TypeError("Invalid attempt to spread non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.");
    }
    function _unsupportedIterableToArray(o, minLen) {
      if (!o)
        return;
      if (typeof o === "string")
        return _arrayLikeToArray(o, minLen);
      var n = Object.prototype.toString.call(o).slice(8, -1);
      if (n === "Object" && o.constructor)
        n = o.constructor.name;
      if (n === "Map" || n === "Set")
        return Array.from(o);
      if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n))
        return _arrayLikeToArray(o, minLen);
    }
    function _iterableToArray(iter) {
      if (typeof Symbol !== "undefined" && iter[Symbol.iterator] != null || iter["@@iterator"] != null)
        return Array.from(iter);
    }
    function _arrayWithoutHoles(arr) {
      if (Array.isArray(arr))
        return _arrayLikeToArray(arr);
    }
    function _arrayLikeToArray(arr, len) {
      if (len == null || len > arr.length)
        len = arr.length;
      for (var i = 0, arr2 = new Array(len); i < len; i++) {
        arr2[i] = arr[i];
      }
      return arr2;
    }
    function ownKeys(object, enumerableOnly) {
      var keys = Object.keys(object);
      if (Object.getOwnPropertySymbols) {
        var symbols = Object.getOwnPropertySymbols(object);
        enumerableOnly && (symbols = symbols.filter(function(sym) {
          return Object.getOwnPropertyDescriptor(object, sym).enumerable;
        })), keys.push.apply(keys, symbols);
      }
      return keys;
    }
    function _objectSpread(target) {
      for (var i = 1; i < arguments.length; i++) {
        var source = null != arguments[i] ? arguments[i] : {};
        i % 2 ? ownKeys(Object(source), true).forEach(function(key) {
          _defineProperty(target, key, source[key]);
        }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)) : ownKeys(Object(source)).forEach(function(key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
      return target;
    }
    function _defineProperty(obj, key, value) {
      if (key in obj) {
        Object.defineProperty(obj, key, { value, enumerable: true, configurable: true, writable: true });
      } else {
        obj[key] = value;
      }
      return obj;
    }
    function _classCallCheck(instance, Constructor) {
      if (!(instance instanceof Constructor)) {
        throw new TypeError("Cannot call a class as a function");
      }
    }
    function _defineProperties(target, props) {
      for (var i = 0; i < props.length; i++) {
        var descriptor = props[i];
        descriptor.enumerable = descriptor.enumerable || false;
        descriptor.configurable = true;
        if ("value" in descriptor)
          descriptor.writable = true;
        Object.defineProperty(target, descriptor.key, descriptor);
      }
    }
    function _createClass(Constructor, protoProps, staticProps) {
      if (protoProps)
        _defineProperties(Constructor.prototype, protoProps);
      if (staticProps)
        _defineProperties(Constructor, staticProps);
      Object.defineProperty(Constructor, "prototype", { writable: false });
      return Constructor;
    }
    var Default = {
      alwaysOpen: false,
      activeClasses: "bg-gray-100 dark:bg-gray-800 text-gray-900 dark:text-white",
      inactiveClasses: "text-gray-500 dark:text-gray-400",
      onOpen: function onOpen() {
      },
      onClose: function onClose() {
      },
      onToggle: function onToggle() {
      }
    };
    var Accordion = /* @__PURE__ */ function() {
      function Accordion2() {
        var items = arguments.length > 0 && arguments[0] !== void 0 ? arguments[0] : [];
        var options = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : {};
        _classCallCheck(this, Accordion2);
        this._items = items;
        this._options = _objectSpread(_objectSpread({}, Default), options);
        this._init();
      }
      _createClass(Accordion2, [{
        key: "_init",
        value: function _init() {
          var _this = this;
          if (this._items.length) {
            this._items.map(function(item) {
              if (item.active) {
                _this.open(item.id);
              }
              item.triggerEl.addEventListener("click", function() {
                _this.toggle(item.id);
              });
            });
          }
        }
      }, {
        key: "getItem",
        value: function getItem(id) {
          return this._items.filter(function(item) {
            return item.id === id;
          })[0];
        }
      }, {
        key: "open",
        value: function open(id) {
          var _this2 = this, _item$triggerEl$class, _item$triggerEl$class2;
          var item = this.getItem(id);
          if (!this._options.alwaysOpen) {
            this._items.map(function(i) {
              if (i !== item) {
                var _i$triggerEl$classLis, _i$triggerEl$classLis2;
                (_i$triggerEl$classLis = i.triggerEl.classList).remove.apply(_i$triggerEl$classLis, _toConsumableArray(_this2._options.activeClasses.split(" ")));
                (_i$triggerEl$classLis2 = i.triggerEl.classList).add.apply(_i$triggerEl$classLis2, _toConsumableArray(_this2._options.inactiveClasses.split(" ")));
                i.targetEl.classList.add("hidden");
                i.triggerEl.setAttribute("aria-expanded", false);
                i.active = false;
                if (i.iconEl) {
                  i.iconEl.classList.remove("rotate-180");
                }
              }
            });
          }
          (_item$triggerEl$class = item.triggerEl.classList).add.apply(_item$triggerEl$class, _toConsumableArray(this._options.activeClasses.split(" ")));
          (_item$triggerEl$class2 = item.triggerEl.classList).remove.apply(_item$triggerEl$class2, _toConsumableArray(this._options.inactiveClasses.split(" ")));
          item.triggerEl.setAttribute("aria-expanded", true);
          item.targetEl.classList.remove("hidden");
          item.active = true;
          if (item.iconEl) {
            item.iconEl.classList.add("rotate-180");
          }
          this._options.onOpen(this, item);
        }
      }, {
        key: "toggle",
        value: function toggle(id) {
          var item = this.getItem(id);
          if (item.active) {
            this.close(id);
          } else {
            this.open(id);
          }
          this._options.onToggle(this, item);
        }
      }, {
        key: "close",
        value: function close(id) {
          var _item$triggerEl$class3, _item$triggerEl$class4;
          var item = this.getItem(id);
          (_item$triggerEl$class3 = item.triggerEl.classList).remove.apply(_item$triggerEl$class3, _toConsumableArray(this._options.activeClasses.split(" ")));
          (_item$triggerEl$class4 = item.triggerEl.classList).add.apply(_item$triggerEl$class4, _toConsumableArray(this._options.inactiveClasses.split(" ")));
          item.targetEl.classList.add("hidden");
          item.triggerEl.setAttribute("aria-expanded", false);
          item.active = false;
          if (item.iconEl) {
            item.iconEl.classList.remove("rotate-180");
          }
          this._options.onClose(this, item);
        }
      }]);
      return Accordion2;
    }();
    window.Accordion = Accordion;
    function initAccordion() {
      document.querySelectorAll("[data-accordion]").forEach(function(accordionEl) {
        var alwaysOpen = accordionEl.getAttribute("data-accordion");
        var activeClasses = accordionEl.getAttribute("data-active-classes");
        var inactiveClasses = accordionEl.getAttribute("data-inactive-classes");
        var items = [];
        accordionEl.querySelectorAll("[data-accordion-target]").forEach(function(el) {
          var item = {
            id: el.getAttribute("data-accordion-target"),
            triggerEl: el,
            targetEl: document.querySelector(el.getAttribute("data-accordion-target")),
            iconEl: el.querySelector("[data-accordion-icon]"),
            active: el.getAttribute("aria-expanded") === "true" ? true : false
          };
          items.push(item);
        });
        new Accordion(items, {
          alwaysOpen: alwaysOpen === "open" ? true : false,
          activeClasses: activeClasses ? activeClasses : Default.activeClasses,
          inactiveClasses: inactiveClasses ? inactiveClasses : Default.inactiveClasses
        });
      });
    }
    if (document.readyState !== "loading") {
      initAccordion();
    } else {
      document.addEventListener("DOMContentLoaded", initAccordion);
    }
    const accordion = Accordion;
    ;
    function collapse_ownKeys(object, enumerableOnly) {
      var keys = Object.keys(object);
      if (Object.getOwnPropertySymbols) {
        var symbols = Object.getOwnPropertySymbols(object);
        enumerableOnly && (symbols = symbols.filter(function(sym) {
          return Object.getOwnPropertyDescriptor(object, sym).enumerable;
        })), keys.push.apply(keys, symbols);
      }
      return keys;
    }
    function collapse_objectSpread(target) {
      for (var i = 1; i < arguments.length; i++) {
        var source = null != arguments[i] ? arguments[i] : {};
        i % 2 ? collapse_ownKeys(Object(source), true).forEach(function(key) {
          collapse_defineProperty(target, key, source[key]);
        }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)) : collapse_ownKeys(Object(source)).forEach(function(key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
      return target;
    }
    function collapse_defineProperty(obj, key, value) {
      if (key in obj) {
        Object.defineProperty(obj, key, { value, enumerable: true, configurable: true, writable: true });
      } else {
        obj[key] = value;
      }
      return obj;
    }
    function collapse_classCallCheck(instance, Constructor) {
      if (!(instance instanceof Constructor)) {
        throw new TypeError("Cannot call a class as a function");
      }
    }
    function collapse_defineProperties(target, props) {
      for (var i = 0; i < props.length; i++) {
        var descriptor = props[i];
        descriptor.enumerable = descriptor.enumerable || false;
        descriptor.configurable = true;
        if ("value" in descriptor)
          descriptor.writable = true;
        Object.defineProperty(target, descriptor.key, descriptor);
      }
    }
    function collapse_createClass(Constructor, protoProps, staticProps) {
      if (protoProps)
        collapse_defineProperties(Constructor.prototype, protoProps);
      if (staticProps)
        collapse_defineProperties(Constructor, staticProps);
      Object.defineProperty(Constructor, "prototype", { writable: false });
      return Constructor;
    }
    var collapse_Default = {
      triggerEl: null,
      onCollapse: function onCollapse() {
      },
      onExpand: function onExpand() {
      },
      onToggle: function onToggle() {
      }
    };
    var Collapse = /* @__PURE__ */ function() {
      function Collapse2() {
        var targetEl = arguments.length > 0 && arguments[0] !== void 0 ? arguments[0] : null;
        var options = arguments.length > 1 ? arguments[1] : void 0;
        collapse_classCallCheck(this, Collapse2);
        this._targetEl = targetEl;
        this._triggerEl = options ? options.triggerEl : collapse_Default.triggerEl;
        this._options = collapse_objectSpread(collapse_objectSpread({}, collapse_Default), options);
        this._visible = false;
        this._init();
      }
      collapse_createClass(Collapse2, [{
        key: "_init",
        value: function _init() {
          var _this = this;
          if (this._triggerEl) {
            if (this._triggerEl.hasAttribute("aria-expanded")) {
              this._visible = this._triggerEl.getAttribute("aria-expanded") === "true" ? true : false;
            } else {
              this._visible = this._targetEl.classList.contains("hidden") ? false : true;
            }
            this._triggerEl.addEventListener("click", function() {
              _this._visible ? _this.collapse() : _this.expand();
            });
          }
        }
      }, {
        key: "collapse",
        value: function collapse2() {
          this._targetEl.classList.add("hidden");
          if (this._triggerEl) {
            this._triggerEl.setAttribute("aria-expanded", "false");
          }
          this._visible = false;
          this._options.onCollapse(this);
        }
      }, {
        key: "expand",
        value: function expand() {
          this._targetEl.classList.remove("hidden");
          if (this._triggerEl) {
            this._triggerEl.setAttribute("aria-expanded", "true");
          }
          this._visible = true;
          this._options.onExpand(this);
        }
      }, {
        key: "toggle",
        value: function toggle() {
          if (this._visible) {
            this.collapse();
          } else {
            this.expand();
          }
        }
      }]);
      return Collapse2;
    }();
    window.Collapse = Collapse;
    function initCollapse() {
      document.querySelectorAll("[data-collapse-toggle]").forEach(function(triggerEl) {
        var targetEl = document.getElementById(triggerEl.getAttribute("data-collapse-toggle"));
        new Collapse(targetEl, {
          triggerEl
        });
      });
    }
    if (document.readyState !== "loading") {
      initCollapse();
    } else {
      document.addEventListener("DOMContentLoaded", initCollapse);
    }
    const collapse = Collapse;
    ;
    function carousel_toConsumableArray(arr) {
      return carousel_arrayWithoutHoles(arr) || carousel_iterableToArray(arr) || carousel_unsupportedIterableToArray(arr) || carousel_nonIterableSpread();
    }
    function carousel_nonIterableSpread() {
      throw new TypeError("Invalid attempt to spread non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.");
    }
    function carousel_unsupportedIterableToArray(o, minLen) {
      if (!o)
        return;
      if (typeof o === "string")
        return carousel_arrayLikeToArray(o, minLen);
      var n = Object.prototype.toString.call(o).slice(8, -1);
      if (n === "Object" && o.constructor)
        n = o.constructor.name;
      if (n === "Map" || n === "Set")
        return Array.from(o);
      if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n))
        return carousel_arrayLikeToArray(o, minLen);
    }
    function carousel_iterableToArray(iter) {
      if (typeof Symbol !== "undefined" && iter[Symbol.iterator] != null || iter["@@iterator"] != null)
        return Array.from(iter);
    }
    function carousel_arrayWithoutHoles(arr) {
      if (Array.isArray(arr))
        return carousel_arrayLikeToArray(arr);
    }
    function carousel_arrayLikeToArray(arr, len) {
      if (len == null || len > arr.length)
        len = arr.length;
      for (var i = 0, arr2 = new Array(len); i < len; i++) {
        arr2[i] = arr[i];
      }
      return arr2;
    }
    function carousel_ownKeys(object, enumerableOnly) {
      var keys = Object.keys(object);
      if (Object.getOwnPropertySymbols) {
        var symbols = Object.getOwnPropertySymbols(object);
        enumerableOnly && (symbols = symbols.filter(function(sym) {
          return Object.getOwnPropertyDescriptor(object, sym).enumerable;
        })), keys.push.apply(keys, symbols);
      }
      return keys;
    }
    function carousel_objectSpread(target) {
      for (var i = 1; i < arguments.length; i++) {
        var source = null != arguments[i] ? arguments[i] : {};
        i % 2 ? carousel_ownKeys(Object(source), true).forEach(function(key) {
          carousel_defineProperty(target, key, source[key]);
        }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)) : carousel_ownKeys(Object(source)).forEach(function(key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
      return target;
    }
    function carousel_defineProperty(obj, key, value) {
      if (key in obj) {
        Object.defineProperty(obj, key, { value, enumerable: true, configurable: true, writable: true });
      } else {
        obj[key] = value;
      }
      return obj;
    }
    function carousel_classCallCheck(instance, Constructor) {
      if (!(instance instanceof Constructor)) {
        throw new TypeError("Cannot call a class as a function");
      }
    }
    function carousel_defineProperties(target, props) {
      for (var i = 0; i < props.length; i++) {
        var descriptor = props[i];
        descriptor.enumerable = descriptor.enumerable || false;
        descriptor.configurable = true;
        if ("value" in descriptor)
          descriptor.writable = true;
        Object.defineProperty(target, descriptor.key, descriptor);
      }
    }
    function carousel_createClass(Constructor, protoProps, staticProps) {
      if (protoProps)
        carousel_defineProperties(Constructor.prototype, protoProps);
      if (staticProps)
        carousel_defineProperties(Constructor, staticProps);
      Object.defineProperty(Constructor, "prototype", { writable: false });
      return Constructor;
    }
    var carousel_Default = {
      defaultPosition: 0,
      indicators: {
        items: [],
        activeClasses: "bg-white dark:bg-gray-800",
        inactiveClasses: "bg-white/50 dark:bg-gray-800/50 hover:bg-white dark:hover:bg-gray-800"
      },
      interval: 3e3,
      onNext: function onNext() {
      },
      onPrev: function onPrev() {
      },
      onChange: function onChange() {
      }
    };
    var Carousel = /* @__PURE__ */ function() {
      function Carousel2() {
        var items = arguments.length > 0 && arguments[0] !== void 0 ? arguments[0] : [];
        var options = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : {};
        carousel_classCallCheck(this, Carousel2);
        this._items = items;
        this._options = carousel_objectSpread(carousel_objectSpread(carousel_objectSpread({}, carousel_Default), options), {}, {
          indicators: carousel_objectSpread(carousel_objectSpread({}, carousel_Default.indicators), options.indicators)
        });
        this._activeItem = this.getItem(this._options.defaultPosition);
        this._indicators = this._options.indicators.items;
        this._interval = null;
        this._init();
      }
      carousel_createClass(Carousel2, [{
        key: "_init",
        value: function _init() {
          var _this = this;
          this._items.map(function(item) {
            item.el.classList.add("absolute", "inset-0", "transition-all", "transform");
          });
          if (this._getActiveItem()) {
            this.slideTo(this._getActiveItem().position);
          } else {
            this.slideTo(0);
          }
          this._indicators.map(function(indicator, position) {
            indicator.el.addEventListener("click", function() {
              _this.slideTo(position);
            });
          });
        }
      }, {
        key: "getItem",
        value: function getItem(position) {
          return this._items[position];
        }
      }, {
        key: "slideTo",
        value: function slideTo(position) {
          var nextItem = this._items[position];
          var rotationItems = {
            "left": nextItem.position === 0 ? this._items[this._items.length - 1] : this._items[nextItem.position - 1],
            "middle": nextItem,
            "right": nextItem.position === this._items.length - 1 ? this._items[0] : this._items[nextItem.position + 1]
          };
          this._rotate(rotationItems);
          this._setActiveItem(nextItem.position);
          if (this._interval) {
            this.pause();
            this.cycle();
          }
          this._options.onChange(this);
        }
      }, {
        key: "next",
        value: function next() {
          var activeItem = this._getActiveItem();
          var nextItem = null;
          if (activeItem.position === this._items.length - 1) {
            nextItem = this._items[0];
          } else {
            nextItem = this._items[activeItem.position + 1];
          }
          this.slideTo(nextItem.position);
          this._options.onNext(this);
        }
      }, {
        key: "prev",
        value: function prev() {
          var activeItem = this._getActiveItem();
          var prevItem = null;
          if (activeItem.position === 0) {
            prevItem = this._items[this._items.length - 1];
          } else {
            prevItem = this._items[activeItem.position - 1];
          }
          this.slideTo(prevItem.position);
          this._options.onPrev(this);
        }
      }, {
        key: "_rotate",
        value: function _rotate(rotationItems) {
          this._items.map(function(item) {
            item.el.classList.add("hidden");
          });
          rotationItems.left.el.classList.remove("-translate-x-full", "translate-x-full", "translate-x-0", "hidden", "z-20");
          rotationItems.left.el.classList.add("-translate-x-full", "z-10");
          rotationItems.middle.el.classList.remove("-translate-x-full", "translate-x-full", "translate-x-0", "hidden", "z-10");
          rotationItems.middle.el.classList.add("translate-x-0", "z-20");
          rotationItems.right.el.classList.remove("-translate-x-full", "translate-x-full", "translate-x-0", "hidden", "z-20");
          rotationItems.right.el.classList.add("translate-x-full", "z-10");
        }
      }, {
        key: "cycle",
        value: function cycle() {
          var _this2 = this;
          this._interval = setInterval(function() {
            _this2.next();
          }, this._options.interval);
        }
      }, {
        key: "pause",
        value: function pause() {
          clearInterval(this._interval);
        }
      }, {
        key: "_getActiveItem",
        value: function _getActiveItem() {
          return this._activeItem;
        }
      }, {
        key: "_setActiveItem",
        value: function _setActiveItem(position) {
          var _this3 = this;
          this._activeItem = this._items[position];
          if (this._indicators.length) {
            var _this$_indicators$pos, _this$_indicators$pos2;
            this._indicators.map(function(indicator) {
              var _indicator$el$classLi, _indicator$el$classLi2;
              indicator.el.setAttribute("aria-current", "false");
              (_indicator$el$classLi = indicator.el.classList).remove.apply(_indicator$el$classLi, carousel_toConsumableArray(_this3._options.indicators.activeClasses.split(" ")));
              (_indicator$el$classLi2 = indicator.el.classList).add.apply(_indicator$el$classLi2, carousel_toConsumableArray(_this3._options.indicators.inactiveClasses.split(" ")));
            });
            (_this$_indicators$pos = this._indicators[position].el.classList).add.apply(_this$_indicators$pos, carousel_toConsumableArray(this._options.indicators.activeClasses.split(" ")));
            (_this$_indicators$pos2 = this._indicators[position].el.classList).remove.apply(_this$_indicators$pos2, carousel_toConsumableArray(this._options.indicators.inactiveClasses.split(" ")));
            this._indicators[position].el.setAttribute("aria-current", "true");
          }
        }
      }]);
      return Carousel2;
    }();
    window.Carousel = Carousel;
    function initCarousel() {
      document.querySelectorAll("[data-carousel]").forEach(function(carouselEl) {
        var interval = carouselEl.getAttribute("data-carousel-interval");
        var slide = carouselEl.getAttribute("data-carousel") === "slide" ? true : false;
        var items = [];
        var defaultPosition = 0;
        if (carouselEl.querySelectorAll("[data-carousel-item]").length) {
          carousel_toConsumableArray(carouselEl.querySelectorAll("[data-carousel-item]")).map(function(carouselItemEl, position) {
            items.push({
              position,
              el: carouselItemEl
            });
            if (carouselItemEl.getAttribute("data-carousel-item") === "active") {
              defaultPosition = position;
            }
          });
        }
        var indicators = [];
        if (carouselEl.querySelectorAll("[data-carousel-slide-to]").length) {
          carousel_toConsumableArray(carouselEl.querySelectorAll("[data-carousel-slide-to]")).map(function(indicatorEl) {
            indicators.push({
              position: indicatorEl.getAttribute("data-carousel-slide-to"),
              el: indicatorEl
            });
          });
        }
        var carousel2 = new Carousel(items, {
          defaultPosition,
          indicators: {
            items: indicators
          },
          interval: interval ? interval : carousel_Default.interval
        });
        if (slide) {
          carousel2.cycle();
        }
        var carouselNextEl = carouselEl.querySelector("[data-carousel-next]");
        var carouselPrevEl = carouselEl.querySelector("[data-carousel-prev]");
        if (carouselNextEl) {
          carouselNextEl.addEventListener("click", function() {
            carousel2.next();
          });
        }
        if (carouselPrevEl) {
          carouselPrevEl.addEventListener("click", function() {
            carousel2.prev();
          });
        }
      });
    }
    if (document.readyState !== "loading") {
      initCarousel();
    } else {
      document.addEventListener("DOMContentLoaded", initCarousel);
    }
    const carousel = Carousel;
    ;
    function dismiss_ownKeys(object, enumerableOnly) {
      var keys = Object.keys(object);
      if (Object.getOwnPropertySymbols) {
        var symbols = Object.getOwnPropertySymbols(object);
        enumerableOnly && (symbols = symbols.filter(function(sym) {
          return Object.getOwnPropertyDescriptor(object, sym).enumerable;
        })), keys.push.apply(keys, symbols);
      }
      return keys;
    }
    function dismiss_objectSpread(target) {
      for (var i = 1; i < arguments.length; i++) {
        var source = null != arguments[i] ? arguments[i] : {};
        i % 2 ? dismiss_ownKeys(Object(source), true).forEach(function(key) {
          dismiss_defineProperty(target, key, source[key]);
        }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)) : dismiss_ownKeys(Object(source)).forEach(function(key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
      return target;
    }
    function dismiss_defineProperty(obj, key, value) {
      if (key in obj) {
        Object.defineProperty(obj, key, { value, enumerable: true, configurable: true, writable: true });
      } else {
        obj[key] = value;
      }
      return obj;
    }
    function dismiss_classCallCheck(instance, Constructor) {
      if (!(instance instanceof Constructor)) {
        throw new TypeError("Cannot call a class as a function");
      }
    }
    function dismiss_defineProperties(target, props) {
      for (var i = 0; i < props.length; i++) {
        var descriptor = props[i];
        descriptor.enumerable = descriptor.enumerable || false;
        descriptor.configurable = true;
        if ("value" in descriptor)
          descriptor.writable = true;
        Object.defineProperty(target, descriptor.key, descriptor);
      }
    }
    function dismiss_createClass(Constructor, protoProps, staticProps) {
      if (protoProps)
        dismiss_defineProperties(Constructor.prototype, protoProps);
      if (staticProps)
        dismiss_defineProperties(Constructor, staticProps);
      Object.defineProperty(Constructor, "prototype", { writable: false });
      return Constructor;
    }
    var dismiss_Default = {
      triggerEl: null,
      transition: "transition-opacity",
      duration: 300,
      timing: "ease-out",
      onHide: function onHide() {
      }
    };
    var Dismiss = /* @__PURE__ */ function() {
      function Dismiss2() {
        var targetEl = arguments.length > 0 && arguments[0] !== void 0 ? arguments[0] : null;
        var options = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : {};
        dismiss_classCallCheck(this, Dismiss2);
        this._targetEl = targetEl;
        this._triggerEl = options ? options.triggerEl : dismiss_Default.triggerEl;
        this._options = dismiss_objectSpread(dismiss_objectSpread({}, dismiss_Default), options);
        this._init();
      }
      dismiss_createClass(Dismiss2, [{
        key: "_init",
        value: function _init() {
          var _this = this;
          if (this._triggerEl) {
            this._triggerEl.addEventListener("click", function() {
              _this.hide();
            });
          }
        }
      }, {
        key: "hide",
        value: function hide2() {
          var _this2 = this;
          this._targetEl.classList.add(this._options.transition, "duration-".concat(this._options.duration), this._options.timing, "opacity-0");
          setTimeout(function() {
            _this2._targetEl.classList.add("hidden");
          }, this._options.duration);
          this._options.onHide(this, this._targetEl);
        }
      }]);
      return Dismiss2;
    }();
    window.Dismiss = Dismiss;
    function initDismiss() {
      document.querySelectorAll("[data-dismiss-target]").forEach(function(triggerEl) {
        var targetEl = document.querySelector(triggerEl.getAttribute("data-dismiss-target"));
        new Dismiss(targetEl, {
          triggerEl
        });
      });
    }
    if (document.readyState !== "loading") {
      initDismiss();
    } else {
      document.addEventListener("DOMContentLoaded", initDismiss);
    }
    const dismiss = Dismiss;
    ;
    function getWindow(node) {
      if (node == null) {
        return window;
      }
      if (node.toString() !== "[object Window]") {
        var ownerDocument = node.ownerDocument;
        return ownerDocument ? ownerDocument.defaultView || window : window;
      }
      return node;
    }
    ;
    function isElement(node) {
      var OwnElement = getWindow(node).Element;
      return node instanceof OwnElement || node instanceof Element;
    }
    function isHTMLElement(node) {
      var OwnElement = getWindow(node).HTMLElement;
      return node instanceof OwnElement || node instanceof HTMLElement;
    }
    function isShadowRoot(node) {
      if (typeof ShadowRoot === "undefined") {
        return false;
      }
      var OwnElement = getWindow(node).ShadowRoot;
      return node instanceof OwnElement || node instanceof ShadowRoot;
    }
    ;
    var math_max = Math.max;
    var math_min = Math.min;
    var round = Math.round;
    ;
    function getBoundingClientRect(element, includeScale) {
      if (includeScale === void 0) {
        includeScale = false;
      }
      var rect = element.getBoundingClientRect();
      var scaleX = 1;
      var scaleY = 1;
      if (isHTMLElement(element) && includeScale) {
        var offsetHeight = element.offsetHeight;
        var offsetWidth = element.offsetWidth;
        if (offsetWidth > 0) {
          scaleX = round(rect.width) / offsetWidth || 1;
        }
        if (offsetHeight > 0) {
          scaleY = round(rect.height) / offsetHeight || 1;
        }
      }
      return {
        width: rect.width / scaleX,
        height: rect.height / scaleY,
        top: rect.top / scaleY,
        right: rect.right / scaleX,
        bottom: rect.bottom / scaleY,
        left: rect.left / scaleX,
        x: rect.left / scaleX,
        y: rect.top / scaleY
      };
    }
    ;
    function getWindowScroll(node) {
      var win = getWindow(node);
      var scrollLeft = win.pageXOffset;
      var scrollTop = win.pageYOffset;
      return {
        scrollLeft,
        scrollTop
      };
    }
    ;
    function getHTMLElementScroll(element) {
      return {
        scrollLeft: element.scrollLeft,
        scrollTop: element.scrollTop
      };
    }
    ;
    function getNodeScroll(node) {
      if (node === getWindow(node) || !isHTMLElement(node)) {
        return getWindowScroll(node);
      } else {
        return getHTMLElementScroll(node);
      }
    }
    ;
    function getNodeName(element) {
      return element ? (element.nodeName || "").toLowerCase() : null;
    }
    ;
    function getDocumentElement(element) {
      return ((isElement(element) ? element.ownerDocument : element.document) || window.document).documentElement;
    }
    ;
    function getWindowScrollBarX(element) {
      return getBoundingClientRect(getDocumentElement(element)).left + getWindowScroll(element).scrollLeft;
    }
    ;
    function getComputedStyle2(element) {
      return getWindow(element).getComputedStyle(element);
    }
    ;
    function isScrollParent(element) {
      var _getComputedStyle = getComputedStyle2(element), overflow = _getComputedStyle.overflow, overflowX = _getComputedStyle.overflowX, overflowY = _getComputedStyle.overflowY;
      return /auto|scroll|overlay|hidden/.test(overflow + overflowY + overflowX);
    }
    ;
    function isElementScaled(element) {
      var rect = element.getBoundingClientRect();
      var scaleX = round(rect.width) / element.offsetWidth || 1;
      var scaleY = round(rect.height) / element.offsetHeight || 1;
      return scaleX !== 1 || scaleY !== 1;
    }
    function getCompositeRect(elementOrVirtualElement, offsetParent, isFixed) {
      if (isFixed === void 0) {
        isFixed = false;
      }
      var isOffsetParentAnElement = isHTMLElement(offsetParent);
      var offsetParentIsScaled = isHTMLElement(offsetParent) && isElementScaled(offsetParent);
      var documentElement = getDocumentElement(offsetParent);
      var rect = getBoundingClientRect(elementOrVirtualElement, offsetParentIsScaled);
      var scroll = {
        scrollLeft: 0,
        scrollTop: 0
      };
      var offsets = {
        x: 0,
        y: 0
      };
      if (isOffsetParentAnElement || !isOffsetParentAnElement && !isFixed) {
        if (getNodeName(offsetParent) !== "body" || isScrollParent(documentElement)) {
          scroll = getNodeScroll(offsetParent);
        }
        if (isHTMLElement(offsetParent)) {
          offsets = getBoundingClientRect(offsetParent, true);
          offsets.x += offsetParent.clientLeft;
          offsets.y += offsetParent.clientTop;
        } else if (documentElement) {
          offsets.x = getWindowScrollBarX(documentElement);
        }
      }
      return {
        x: rect.left + scroll.scrollLeft - offsets.x,
        y: rect.top + scroll.scrollTop - offsets.y,
        width: rect.width,
        height: rect.height
      };
    }
    ;
    function getLayoutRect(element) {
      var clientRect = getBoundingClientRect(element);
      var width = element.offsetWidth;
      var height = element.offsetHeight;
      if (Math.abs(clientRect.width - width) <= 1) {
        width = clientRect.width;
      }
      if (Math.abs(clientRect.height - height) <= 1) {
        height = clientRect.height;
      }
      return {
        x: element.offsetLeft,
        y: element.offsetTop,
        width,
        height
      };
    }
    ;
    function getParentNode(element) {
      if (getNodeName(element) === "html") {
        return element;
      }
      return element.assignedSlot || element.parentNode || (isShadowRoot(element) ? element.host : null) || getDocumentElement(element);
    }
    ;
    function getScrollParent(node) {
      if (["html", "body", "#document"].indexOf(getNodeName(node)) >= 0) {
        return node.ownerDocument.body;
      }
      if (isHTMLElement(node) && isScrollParent(node)) {
        return node;
      }
      return getScrollParent(getParentNode(node));
    }
    ;
    function listScrollParents(element, list) {
      var _element$ownerDocumen;
      if (list === void 0) {
        list = [];
      }
      var scrollParent = getScrollParent(element);
      var isBody = scrollParent === ((_element$ownerDocumen = element.ownerDocument) == null ? void 0 : _element$ownerDocumen.body);
      var win = getWindow(scrollParent);
      var target = isBody ? [win].concat(win.visualViewport || [], isScrollParent(scrollParent) ? scrollParent : []) : scrollParent;
      var updatedList = list.concat(target);
      return isBody ? updatedList : updatedList.concat(listScrollParents(getParentNode(target)));
    }
    ;
    function isTableElement(element) {
      return ["table", "td", "th"].indexOf(getNodeName(element)) >= 0;
    }
    ;
    function getTrueOffsetParent(element) {
      if (!isHTMLElement(element) || getComputedStyle2(element).position === "fixed") {
        return null;
      }
      return element.offsetParent;
    }
    function getContainingBlock(element) {
      var isFirefox = navigator.userAgent.toLowerCase().indexOf("firefox") !== -1;
      var isIE = navigator.userAgent.indexOf("Trident") !== -1;
      if (isIE && isHTMLElement(element)) {
        var elementCss = getComputedStyle2(element);
        if (elementCss.position === "fixed") {
          return null;
        }
      }
      var currentNode = getParentNode(element);
      while (isHTMLElement(currentNode) && ["html", "body"].indexOf(getNodeName(currentNode)) < 0) {
        var css = getComputedStyle2(currentNode);
        if (css.transform !== "none" || css.perspective !== "none" || css.contain === "paint" || ["transform", "perspective"].indexOf(css.willChange) !== -1 || isFirefox && css.willChange === "filter" || isFirefox && css.filter && css.filter !== "none") {
          return currentNode;
        } else {
          currentNode = currentNode.parentNode;
        }
      }
      return null;
    }
    function getOffsetParent(element) {
      var window2 = getWindow(element);
      var offsetParent = getTrueOffsetParent(element);
      while (offsetParent && isTableElement(offsetParent) && getComputedStyle2(offsetParent).position === "static") {
        offsetParent = getTrueOffsetParent(offsetParent);
      }
      if (offsetParent && (getNodeName(offsetParent) === "html" || getNodeName(offsetParent) === "body" && getComputedStyle2(offsetParent).position === "static")) {
        return window2;
      }
      return offsetParent || getContainingBlock(element) || window2;
    }
    ;
    var enums_top = "top";
    var bottom = "bottom";
    var right = "right";
    var left = "left";
    var auto = "auto";
    var basePlacements = [enums_top, bottom, right, left];
    var start = "start";
    var end = "end";
    var clippingParents = "clippingParents";
    var viewport = "viewport";
    var popper = "popper";
    var reference = "reference";
    var variationPlacements = /* @__PURE__ */ basePlacements.reduce(function(acc, placement) {
      return acc.concat([placement + "-" + start, placement + "-" + end]);
    }, []);
    var enums_placements = /* @__PURE__ */ [].concat(basePlacements, [auto]).reduce(function(acc, placement) {
      return acc.concat([placement, placement + "-" + start, placement + "-" + end]);
    }, []);
    var beforeRead = "beforeRead";
    var read = "read";
    var afterRead = "afterRead";
    var beforeMain = "beforeMain";
    var main = "main";
    var afterMain = "afterMain";
    var beforeWrite = "beforeWrite";
    var write = "write";
    var afterWrite = "afterWrite";
    var modifierPhases = [beforeRead, read, afterRead, beforeMain, main, afterMain, beforeWrite, write, afterWrite];
    ;
    function order(modifiers) {
      var map = /* @__PURE__ */ new Map();
      var visited = /* @__PURE__ */ new Set();
      var result = [];
      modifiers.forEach(function(modifier) {
        map.set(modifier.name, modifier);
      });
      function sort(modifier) {
        visited.add(modifier.name);
        var requires = [].concat(modifier.requires || [], modifier.requiresIfExists || []);
        requires.forEach(function(dep) {
          if (!visited.has(dep)) {
            var depModifier = map.get(dep);
            if (depModifier) {
              sort(depModifier);
            }
          }
        });
        result.push(modifier);
      }
      modifiers.forEach(function(modifier) {
        if (!visited.has(modifier.name)) {
          sort(modifier);
        }
      });
      return result;
    }
    function orderModifiers(modifiers) {
      var orderedModifiers = order(modifiers);
      return modifierPhases.reduce(function(acc, phase) {
        return acc.concat(orderedModifiers.filter(function(modifier) {
          return modifier.phase === phase;
        }));
      }, []);
    }
    ;
    function debounce(fn) {
      var pending;
      return function() {
        if (!pending) {
          pending = new Promise(function(resolve) {
            Promise.resolve().then(function() {
              pending = void 0;
              resolve(fn());
            });
          });
        }
        return pending;
      };
    }
    ;
    function mergeByName(modifiers) {
      var merged = modifiers.reduce(function(merged2, current) {
        var existing = merged2[current.name];
        merged2[current.name] = existing ? Object.assign({}, existing, current, {
          options: Object.assign({}, existing.options, current.options),
          data: Object.assign({}, existing.data, current.data)
        }) : current;
        return merged2;
      }, {});
      return Object.keys(merged).map(function(key) {
        return merged[key];
      });
    }
    ;
    var INVALID_ELEMENT_ERROR = "Popper: Invalid reference or popper argument provided. They must be either a DOM element or virtual element.";
    var INFINITE_LOOP_ERROR = "Popper: An infinite loop in the modifiers cycle has been detected! The cycle has been interrupted to prevent a browser crash.";
    var DEFAULT_OPTIONS = {
      placement: "bottom",
      modifiers: [],
      strategy: "absolute"
    };
    function areValidElements() {
      for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
        args[_key] = arguments[_key];
      }
      return !args.some(function(element) {
        return !(element && typeof element.getBoundingClientRect === "function");
      });
    }
    function popperGenerator(generatorOptions) {
      if (generatorOptions === void 0) {
        generatorOptions = {};
      }
      var _generatorOptions = generatorOptions, _generatorOptions$def = _generatorOptions.defaultModifiers, defaultModifiers2 = _generatorOptions$def === void 0 ? [] : _generatorOptions$def, _generatorOptions$def2 = _generatorOptions.defaultOptions, defaultOptions = _generatorOptions$def2 === void 0 ? DEFAULT_OPTIONS : _generatorOptions$def2;
      return function createPopper2(reference2, popper2, options) {
        if (options === void 0) {
          options = defaultOptions;
        }
        var state = {
          placement: "bottom",
          orderedModifiers: [],
          options: Object.assign({}, DEFAULT_OPTIONS, defaultOptions),
          modifiersData: {},
          elements: {
            reference: reference2,
            popper: popper2
          },
          attributes: {},
          styles: {}
        };
        var effectCleanupFns = [];
        var isDestroyed = false;
        var instance = {
          state,
          setOptions: function setOptions(setOptionsAction) {
            var options2 = typeof setOptionsAction === "function" ? setOptionsAction(state.options) : setOptionsAction;
            cleanupModifierEffects();
            state.options = Object.assign({}, defaultOptions, state.options, options2);
            state.scrollParents = {
              reference: isElement(reference2) ? listScrollParents(reference2) : reference2.contextElement ? listScrollParents(reference2.contextElement) : [],
              popper: listScrollParents(popper2)
            };
            var orderedModifiers = orderModifiers(mergeByName([].concat(defaultModifiers2, state.options.modifiers)));
            state.orderedModifiers = orderedModifiers.filter(function(m) {
              return m.enabled;
            });
            if (false) {
              var _getComputedStyle, marginTop, marginRight, marginBottom, marginLeft, flipModifier, modifiers;
            }
            runModifierEffects();
            return instance.update();
          },
          forceUpdate: function forceUpdate() {
            if (isDestroyed) {
              return;
            }
            var _state$elements = state.elements, reference3 = _state$elements.reference, popper3 = _state$elements.popper;
            if (!areValidElements(reference3, popper3)) {
              if (false) {
              }
              return;
            }
            state.rects = {
              reference: getCompositeRect(reference3, getOffsetParent(popper3), state.options.strategy === "fixed"),
              popper: getLayoutRect(popper3)
            };
            state.reset = false;
            state.placement = state.options.placement;
            state.orderedModifiers.forEach(function(modifier) {
              return state.modifiersData[modifier.name] = Object.assign({}, modifier.data);
            });
            var __debug_loops__ = 0;
            for (var index = 0; index < state.orderedModifiers.length; index++) {
              if (false) {
              }
              if (state.reset === true) {
                state.reset = false;
                index = -1;
                continue;
              }
              var _state$orderedModifie = state.orderedModifiers[index], fn = _state$orderedModifie.fn, _state$orderedModifie2 = _state$orderedModifie.options, _options = _state$orderedModifie2 === void 0 ? {} : _state$orderedModifie2, name = _state$orderedModifie.name;
              if (typeof fn === "function") {
                state = fn({
                  state,
                  options: _options,
                  name,
                  instance
                }) || state;
              }
            }
          },
          update: debounce(function() {
            return new Promise(function(resolve) {
              instance.forceUpdate();
              resolve(state);
            });
          }),
          destroy: function destroy2() {
            cleanupModifierEffects();
            isDestroyed = true;
          }
        };
        if (!areValidElements(reference2, popper2)) {
          if (false) {
          }
          return instance;
        }
        instance.setOptions(options).then(function(state2) {
          if (!isDestroyed && options.onFirstUpdate) {
            options.onFirstUpdate(state2);
          }
        });
        function runModifierEffects() {
          state.orderedModifiers.forEach(function(_ref3) {
            var name = _ref3.name, _ref3$options = _ref3.options, options2 = _ref3$options === void 0 ? {} : _ref3$options, effect2 = _ref3.effect;
            if (typeof effect2 === "function") {
              var cleanupFn = effect2({
                state,
                name,
                instance,
                options: options2
              });
              var noopFn = function noopFn2() {
              };
              effectCleanupFns.push(cleanupFn || noopFn);
            }
          });
        }
        function cleanupModifierEffects() {
          effectCleanupFns.forEach(function(fn) {
            return fn();
          });
          effectCleanupFns = [];
        }
        return instance;
      };
    }
    var createPopper = null;
    ;
    var passive = {
      passive: true
    };
    function effect(_ref) {
      var state = _ref.state, instance = _ref.instance, options = _ref.options;
      var _options$scroll = options.scroll, scroll = _options$scroll === void 0 ? true : _options$scroll, _options$resize = options.resize, resize = _options$resize === void 0 ? true : _options$resize;
      var window2 = getWindow(state.elements.popper);
      var scrollParents = [].concat(state.scrollParents.reference, state.scrollParents.popper);
      if (scroll) {
        scrollParents.forEach(function(scrollParent) {
          scrollParent.addEventListener("scroll", instance.update, passive);
        });
      }
      if (resize) {
        window2.addEventListener("resize", instance.update, passive);
      }
      return function() {
        if (scroll) {
          scrollParents.forEach(function(scrollParent) {
            scrollParent.removeEventListener("scroll", instance.update, passive);
          });
        }
        if (resize) {
          window2.removeEventListener("resize", instance.update, passive);
        }
      };
    }
    const eventListeners = {
      name: "eventListeners",
      enabled: true,
      phase: "write",
      fn: function fn() {
      },
      effect,
      data: {}
    };
    ;
    function getBasePlacement(placement) {
      return placement.split("-")[0];
    }
    ;
    function getVariation(placement) {
      return placement.split("-")[1];
    }
    ;
    function getMainAxisFromPlacement(placement) {
      return ["top", "bottom"].indexOf(placement) >= 0 ? "x" : "y";
    }
    ;
    function computeOffsets(_ref) {
      var reference2 = _ref.reference, element = _ref.element, placement = _ref.placement;
      var basePlacement = placement ? getBasePlacement(placement) : null;
      var variation = placement ? getVariation(placement) : null;
      var commonX = reference2.x + reference2.width / 2 - element.width / 2;
      var commonY = reference2.y + reference2.height / 2 - element.height / 2;
      var offsets;
      switch (basePlacement) {
        case enums_top:
          offsets = {
            x: commonX,
            y: reference2.y - element.height
          };
          break;
        case bottom:
          offsets = {
            x: commonX,
            y: reference2.y + reference2.height
          };
          break;
        case right:
          offsets = {
            x: reference2.x + reference2.width,
            y: commonY
          };
          break;
        case left:
          offsets = {
            x: reference2.x - element.width,
            y: commonY
          };
          break;
        default:
          offsets = {
            x: reference2.x,
            y: reference2.y
          };
      }
      var mainAxis = basePlacement ? getMainAxisFromPlacement(basePlacement) : null;
      if (mainAxis != null) {
        var len = mainAxis === "y" ? "height" : "width";
        switch (variation) {
          case start:
            offsets[mainAxis] = offsets[mainAxis] - (reference2[len] / 2 - element[len] / 2);
            break;
          case end:
            offsets[mainAxis] = offsets[mainAxis] + (reference2[len] / 2 - element[len] / 2);
            break;
          default:
        }
      }
      return offsets;
    }
    ;
    function popperOffsets(_ref) {
      var state = _ref.state, name = _ref.name;
      state.modifiersData[name] = computeOffsets({
        reference: state.rects.reference,
        element: state.rects.popper,
        strategy: "absolute",
        placement: state.placement
      });
    }
    const modifiers_popperOffsets = {
      name: "popperOffsets",
      enabled: true,
      phase: "read",
      fn: popperOffsets,
      data: {}
    };
    ;
    var unsetSides = {
      top: "auto",
      right: "auto",
      bottom: "auto",
      left: "auto"
    };
    function roundOffsetsByDPR(_ref) {
      var x = _ref.x, y = _ref.y;
      var win = window;
      var dpr = win.devicePixelRatio || 1;
      return {
        x: round(x * dpr) / dpr || 0,
        y: round(y * dpr) / dpr || 0
      };
    }
    function mapToStyles(_ref2) {
      var _Object$assign2;
      var popper2 = _ref2.popper, popperRect = _ref2.popperRect, placement = _ref2.placement, variation = _ref2.variation, offsets = _ref2.offsets, position = _ref2.position, gpuAcceleration = _ref2.gpuAcceleration, adaptive = _ref2.adaptive, roundOffsets = _ref2.roundOffsets, isFixed = _ref2.isFixed;
      var _offsets$x = offsets.x, x = _offsets$x === void 0 ? 0 : _offsets$x, _offsets$y = offsets.y, y = _offsets$y === void 0 ? 0 : _offsets$y;
      var _ref3 = typeof roundOffsets === "function" ? roundOffsets({
        x,
        y
      }) : {
        x,
        y
      };
      x = _ref3.x;
      y = _ref3.y;
      var hasX = offsets.hasOwnProperty("x");
      var hasY = offsets.hasOwnProperty("y");
      var sideX = left;
      var sideY = enums_top;
      var win = window;
      if (adaptive) {
        var offsetParent = getOffsetParent(popper2);
        var heightProp = "clientHeight";
        var widthProp = "clientWidth";
        if (offsetParent === getWindow(popper2)) {
          offsetParent = getDocumentElement(popper2);
          if (getComputedStyle2(offsetParent).position !== "static" && position === "absolute") {
            heightProp = "scrollHeight";
            widthProp = "scrollWidth";
          }
        }
        offsetParent = offsetParent;
        if (placement === enums_top || (placement === left || placement === right) && variation === end) {
          sideY = bottom;
          var offsetY = isFixed && win.visualViewport ? win.visualViewport.height : offsetParent[heightProp];
          y -= offsetY - popperRect.height;
          y *= gpuAcceleration ? 1 : -1;
        }
        if (placement === left || (placement === enums_top || placement === bottom) && variation === end) {
          sideX = right;
          var offsetX = isFixed && win.visualViewport ? win.visualViewport.width : offsetParent[widthProp];
          x -= offsetX - popperRect.width;
          x *= gpuAcceleration ? 1 : -1;
        }
      }
      var commonStyles = Object.assign({
        position
      }, adaptive && unsetSides);
      var _ref4 = roundOffsets === true ? roundOffsetsByDPR({
        x,
        y
      }) : {
        x,
        y
      };
      x = _ref4.x;
      y = _ref4.y;
      if (gpuAcceleration) {
        var _Object$assign;
        return Object.assign({}, commonStyles, (_Object$assign = {}, _Object$assign[sideY] = hasY ? "0" : "", _Object$assign[sideX] = hasX ? "0" : "", _Object$assign.transform = (win.devicePixelRatio || 1) <= 1 ? "translate(" + x + "px, " + y + "px)" : "translate3d(" + x + "px, " + y + "px, 0)", _Object$assign));
      }
      return Object.assign({}, commonStyles, (_Object$assign2 = {}, _Object$assign2[sideY] = hasY ? y + "px" : "", _Object$assign2[sideX] = hasX ? x + "px" : "", _Object$assign2.transform = "", _Object$assign2));
    }
    function computeStyles(_ref5) {
      var state = _ref5.state, options = _ref5.options;
      var _options$gpuAccelerat = options.gpuAcceleration, gpuAcceleration = _options$gpuAccelerat === void 0 ? true : _options$gpuAccelerat, _options$adaptive = options.adaptive, adaptive = _options$adaptive === void 0 ? true : _options$adaptive, _options$roundOffsets = options.roundOffsets, roundOffsets = _options$roundOffsets === void 0 ? true : _options$roundOffsets;
      if (false) {
        var transitionProperty;
      }
      var commonStyles = {
        placement: getBasePlacement(state.placement),
        variation: getVariation(state.placement),
        popper: state.elements.popper,
        popperRect: state.rects.popper,
        gpuAcceleration,
        isFixed: state.options.strategy === "fixed"
      };
      if (state.modifiersData.popperOffsets != null) {
        state.styles.popper = Object.assign({}, state.styles.popper, mapToStyles(Object.assign({}, commonStyles, {
          offsets: state.modifiersData.popperOffsets,
          position: state.options.strategy,
          adaptive,
          roundOffsets
        })));
      }
      if (state.modifiersData.arrow != null) {
        state.styles.arrow = Object.assign({}, state.styles.arrow, mapToStyles(Object.assign({}, commonStyles, {
          offsets: state.modifiersData.arrow,
          position: "absolute",
          adaptive: false,
          roundOffsets
        })));
      }
      state.attributes.popper = Object.assign({}, state.attributes.popper, {
        "data-popper-placement": state.placement
      });
    }
    const modifiers_computeStyles = {
      name: "computeStyles",
      enabled: true,
      phase: "beforeWrite",
      fn: computeStyles,
      data: {}
    };
    ;
    function applyStyles(_ref) {
      var state = _ref.state;
      Object.keys(state.elements).forEach(function(name) {
        var style = state.styles[name] || {};
        var attributes = state.attributes[name] || {};
        var element = state.elements[name];
        if (!isHTMLElement(element) || !getNodeName(element)) {
          return;
        }
        Object.assign(element.style, style);
        Object.keys(attributes).forEach(function(name2) {
          var value = attributes[name2];
          if (value === false) {
            element.removeAttribute(name2);
          } else {
            element.setAttribute(name2, value === true ? "" : value);
          }
        });
      });
    }
    function applyStyles_effect(_ref2) {
      var state = _ref2.state;
      var initialStyles = {
        popper: {
          position: state.options.strategy,
          left: "0",
          top: "0",
          margin: "0"
        },
        arrow: {
          position: "absolute"
        },
        reference: {}
      };
      Object.assign(state.elements.popper.style, initialStyles.popper);
      state.styles = initialStyles;
      if (state.elements.arrow) {
        Object.assign(state.elements.arrow.style, initialStyles.arrow);
      }
      return function() {
        Object.keys(state.elements).forEach(function(name) {
          var element = state.elements[name];
          var attributes = state.attributes[name] || {};
          var styleProperties = Object.keys(state.styles.hasOwnProperty(name) ? state.styles[name] : initialStyles[name]);
          var style = styleProperties.reduce(function(style2, property) {
            style2[property] = "";
            return style2;
          }, {});
          if (!isHTMLElement(element) || !getNodeName(element)) {
            return;
          }
          Object.assign(element.style, style);
          Object.keys(attributes).forEach(function(attribute) {
            element.removeAttribute(attribute);
          });
        });
      };
    }
    const modifiers_applyStyles = {
      name: "applyStyles",
      enabled: true,
      phase: "write",
      fn: applyStyles,
      effect: applyStyles_effect,
      requires: ["computeStyles"]
    };
    ;
    function distanceAndSkiddingToXY(placement, rects, offset2) {
      var basePlacement = getBasePlacement(placement);
      var invertDistance = [left, enums_top].indexOf(basePlacement) >= 0 ? -1 : 1;
      var _ref = typeof offset2 === "function" ? offset2(Object.assign({}, rects, {
        placement
      })) : offset2, skidding = _ref[0], distance = _ref[1];
      skidding = skidding || 0;
      distance = (distance || 0) * invertDistance;
      return [left, right].indexOf(basePlacement) >= 0 ? {
        x: distance,
        y: skidding
      } : {
        x: skidding,
        y: distance
      };
    }
    function offset(_ref2) {
      var state = _ref2.state, options = _ref2.options, name = _ref2.name;
      var _options$offset = options.offset, offset2 = _options$offset === void 0 ? [0, 0] : _options$offset;
      var data = enums_placements.reduce(function(acc, placement) {
        acc[placement] = distanceAndSkiddingToXY(placement, state.rects, offset2);
        return acc;
      }, {});
      var _data$state$placement = data[state.placement], x = _data$state$placement.x, y = _data$state$placement.y;
      if (state.modifiersData.popperOffsets != null) {
        state.modifiersData.popperOffsets.x += x;
        state.modifiersData.popperOffsets.y += y;
      }
      state.modifiersData[name] = data;
    }
    const modifiers_offset = {
      name: "offset",
      enabled: true,
      phase: "main",
      requires: ["popperOffsets"],
      fn: offset
    };
    ;
    var hash = {
      left: "right",
      right: "left",
      bottom: "top",
      top: "bottom"
    };
    function getOppositePlacement(placement) {
      return placement.replace(/left|right|bottom|top/g, function(matched) {
        return hash[matched];
      });
    }
    ;
    var getOppositeVariationPlacement_hash = {
      start: "end",
      end: "start"
    };
    function getOppositeVariationPlacement(placement) {
      return placement.replace(/start|end/g, function(matched) {
        return getOppositeVariationPlacement_hash[matched];
      });
    }
    ;
    function getViewportRect(element) {
      var win = getWindow(element);
      var html = getDocumentElement(element);
      var visualViewport = win.visualViewport;
      var width = html.clientWidth;
      var height = html.clientHeight;
      var x = 0;
      var y = 0;
      if (visualViewport) {
        width = visualViewport.width;
        height = visualViewport.height;
        if (!/^((?!chrome|android).)*safari/i.test(navigator.userAgent)) {
          x = visualViewport.offsetLeft;
          y = visualViewport.offsetTop;
        }
      }
      return {
        width,
        height,
        x: x + getWindowScrollBarX(element),
        y
      };
    }
    ;
    function getDocumentRect(element) {
      var _element$ownerDocumen;
      var html = getDocumentElement(element);
      var winScroll = getWindowScroll(element);
      var body = (_element$ownerDocumen = element.ownerDocument) == null ? void 0 : _element$ownerDocumen.body;
      var width = math_max(html.scrollWidth, html.clientWidth, body ? body.scrollWidth : 0, body ? body.clientWidth : 0);
      var height = math_max(html.scrollHeight, html.clientHeight, body ? body.scrollHeight : 0, body ? body.clientHeight : 0);
      var x = -winScroll.scrollLeft + getWindowScrollBarX(element);
      var y = -winScroll.scrollTop;
      if (getComputedStyle2(body || html).direction === "rtl") {
        x += math_max(html.clientWidth, body ? body.clientWidth : 0) - width;
      }
      return {
        width,
        height,
        x,
        y
      };
    }
    ;
    function contains(parent, child) {
      var rootNode = child.getRootNode && child.getRootNode();
      if (parent.contains(child)) {
        return true;
      } else if (rootNode && isShadowRoot(rootNode)) {
        var next = child;
        do {
          if (next && parent.isSameNode(next)) {
            return true;
          }
          next = next.parentNode || next.host;
        } while (next);
      }
      return false;
    }
    ;
    function rectToClientRect(rect) {
      return Object.assign({}, rect, {
        left: rect.x,
        top: rect.y,
        right: rect.x + rect.width,
        bottom: rect.y + rect.height
      });
    }
    ;
    function getInnerBoundingClientRect(element) {
      var rect = getBoundingClientRect(element);
      rect.top = rect.top + element.clientTop;
      rect.left = rect.left + element.clientLeft;
      rect.bottom = rect.top + element.clientHeight;
      rect.right = rect.left + element.clientWidth;
      rect.width = element.clientWidth;
      rect.height = element.clientHeight;
      rect.x = rect.left;
      rect.y = rect.top;
      return rect;
    }
    function getClientRectFromMixedType(element, clippingParent) {
      return clippingParent === viewport ? rectToClientRect(getViewportRect(element)) : isElement(clippingParent) ? getInnerBoundingClientRect(clippingParent) : rectToClientRect(getDocumentRect(getDocumentElement(element)));
    }
    function getClippingParents(element) {
      var clippingParents2 = listScrollParents(getParentNode(element));
      var canEscapeClipping = ["absolute", "fixed"].indexOf(getComputedStyle2(element).position) >= 0;
      var clipperElement = canEscapeClipping && isHTMLElement(element) ? getOffsetParent(element) : element;
      if (!isElement(clipperElement)) {
        return [];
      }
      return clippingParents2.filter(function(clippingParent) {
        return isElement(clippingParent) && contains(clippingParent, clipperElement) && getNodeName(clippingParent) !== "body";
      });
    }
    function getClippingRect(element, boundary, rootBoundary) {
      var mainClippingParents = boundary === "clippingParents" ? getClippingParents(element) : [].concat(boundary);
      var clippingParents2 = [].concat(mainClippingParents, [rootBoundary]);
      var firstClippingParent = clippingParents2[0];
      var clippingRect = clippingParents2.reduce(function(accRect, clippingParent) {
        var rect = getClientRectFromMixedType(element, clippingParent);
        accRect.top = math_max(rect.top, accRect.top);
        accRect.right = math_min(rect.right, accRect.right);
        accRect.bottom = math_min(rect.bottom, accRect.bottom);
        accRect.left = math_max(rect.left, accRect.left);
        return accRect;
      }, getClientRectFromMixedType(element, firstClippingParent));
      clippingRect.width = clippingRect.right - clippingRect.left;
      clippingRect.height = clippingRect.bottom - clippingRect.top;
      clippingRect.x = clippingRect.left;
      clippingRect.y = clippingRect.top;
      return clippingRect;
    }
    ;
    function getFreshSideObject() {
      return {
        top: 0,
        right: 0,
        bottom: 0,
        left: 0
      };
    }
    ;
    function mergePaddingObject(paddingObject) {
      return Object.assign({}, getFreshSideObject(), paddingObject);
    }
    ;
    function expandToHashMap(value, keys) {
      return keys.reduce(function(hashMap, key) {
        hashMap[key] = value;
        return hashMap;
      }, {});
    }
    ;
    function detectOverflow(state, options) {
      if (options === void 0) {
        options = {};
      }
      var _options = options, _options$placement = _options.placement, placement = _options$placement === void 0 ? state.placement : _options$placement, _options$boundary = _options.boundary, boundary = _options$boundary === void 0 ? clippingParents : _options$boundary, _options$rootBoundary = _options.rootBoundary, rootBoundary = _options$rootBoundary === void 0 ? viewport : _options$rootBoundary, _options$elementConte = _options.elementContext, elementContext = _options$elementConte === void 0 ? popper : _options$elementConte, _options$altBoundary = _options.altBoundary, altBoundary = _options$altBoundary === void 0 ? false : _options$altBoundary, _options$padding = _options.padding, padding = _options$padding === void 0 ? 0 : _options$padding;
      var paddingObject = mergePaddingObject(typeof padding !== "number" ? padding : expandToHashMap(padding, basePlacements));
      var altContext = elementContext === popper ? reference : popper;
      var popperRect = state.rects.popper;
      var element = state.elements[altBoundary ? altContext : elementContext];
      var clippingClientRect = getClippingRect(isElement(element) ? element : element.contextElement || getDocumentElement(state.elements.popper), boundary, rootBoundary);
      var referenceClientRect = getBoundingClientRect(state.elements.reference);
      var popperOffsets2 = computeOffsets({
        reference: referenceClientRect,
        element: popperRect,
        strategy: "absolute",
        placement
      });
      var popperClientRect = rectToClientRect(Object.assign({}, popperRect, popperOffsets2));
      var elementClientRect = elementContext === popper ? popperClientRect : referenceClientRect;
      var overflowOffsets = {
        top: clippingClientRect.top - elementClientRect.top + paddingObject.top,
        bottom: elementClientRect.bottom - clippingClientRect.bottom + paddingObject.bottom,
        left: clippingClientRect.left - elementClientRect.left + paddingObject.left,
        right: elementClientRect.right - clippingClientRect.right + paddingObject.right
      };
      var offsetData = state.modifiersData.offset;
      if (elementContext === popper && offsetData) {
        var offset2 = offsetData[placement];
        Object.keys(overflowOffsets).forEach(function(key) {
          var multiply = [right, bottom].indexOf(key) >= 0 ? 1 : -1;
          var axis = [enums_top, bottom].indexOf(key) >= 0 ? "y" : "x";
          overflowOffsets[key] += offset2[axis] * multiply;
        });
      }
      return overflowOffsets;
    }
    ;
    function computeAutoPlacement(state, options) {
      if (options === void 0) {
        options = {};
      }
      var _options = options, placement = _options.placement, boundary = _options.boundary, rootBoundary = _options.rootBoundary, padding = _options.padding, flipVariations = _options.flipVariations, _options$allowedAutoP = _options.allowedAutoPlacements, allowedAutoPlacements = _options$allowedAutoP === void 0 ? enums_placements : _options$allowedAutoP;
      var variation = getVariation(placement);
      var placements = variation ? flipVariations ? variationPlacements : variationPlacements.filter(function(placement2) {
        return getVariation(placement2) === variation;
      }) : basePlacements;
      var allowedPlacements = placements.filter(function(placement2) {
        return allowedAutoPlacements.indexOf(placement2) >= 0;
      });
      if (allowedPlacements.length === 0) {
        allowedPlacements = placements;
        if (false) {
        }
      }
      var overflows = allowedPlacements.reduce(function(acc, placement2) {
        acc[placement2] = detectOverflow(state, {
          placement: placement2,
          boundary,
          rootBoundary,
          padding
        })[getBasePlacement(placement2)];
        return acc;
      }, {});
      return Object.keys(overflows).sort(function(a, b) {
        return overflows[a] - overflows[b];
      });
    }
    ;
    function getExpandedFallbackPlacements(placement) {
      if (getBasePlacement(placement) === auto) {
        return [];
      }
      var oppositePlacement = getOppositePlacement(placement);
      return [getOppositeVariationPlacement(placement), oppositePlacement, getOppositeVariationPlacement(oppositePlacement)];
    }
    function flip(_ref) {
      var state = _ref.state, options = _ref.options, name = _ref.name;
      if (state.modifiersData[name]._skip) {
        return;
      }
      var _options$mainAxis = options.mainAxis, checkMainAxis = _options$mainAxis === void 0 ? true : _options$mainAxis, _options$altAxis = options.altAxis, checkAltAxis = _options$altAxis === void 0 ? true : _options$altAxis, specifiedFallbackPlacements = options.fallbackPlacements, padding = options.padding, boundary = options.boundary, rootBoundary = options.rootBoundary, altBoundary = options.altBoundary, _options$flipVariatio = options.flipVariations, flipVariations = _options$flipVariatio === void 0 ? true : _options$flipVariatio, allowedAutoPlacements = options.allowedAutoPlacements;
      var preferredPlacement = state.options.placement;
      var basePlacement = getBasePlacement(preferredPlacement);
      var isBasePlacement = basePlacement === preferredPlacement;
      var fallbackPlacements = specifiedFallbackPlacements || (isBasePlacement || !flipVariations ? [getOppositePlacement(preferredPlacement)] : getExpandedFallbackPlacements(preferredPlacement));
      var placements = [preferredPlacement].concat(fallbackPlacements).reduce(function(acc, placement2) {
        return acc.concat(getBasePlacement(placement2) === auto ? computeAutoPlacement(state, {
          placement: placement2,
          boundary,
          rootBoundary,
          padding,
          flipVariations,
          allowedAutoPlacements
        }) : placement2);
      }, []);
      var referenceRect = state.rects.reference;
      var popperRect = state.rects.popper;
      var checksMap = /* @__PURE__ */ new Map();
      var makeFallbackChecks = true;
      var firstFittingPlacement = placements[0];
      for (var i = 0; i < placements.length; i++) {
        var placement = placements[i];
        var _basePlacement = getBasePlacement(placement);
        var isStartVariation = getVariation(placement) === start;
        var isVertical = [enums_top, bottom].indexOf(_basePlacement) >= 0;
        var len = isVertical ? "width" : "height";
        var overflow = detectOverflow(state, {
          placement,
          boundary,
          rootBoundary,
          altBoundary,
          padding
        });
        var mainVariationSide = isVertical ? isStartVariation ? right : left : isStartVariation ? bottom : enums_top;
        if (referenceRect[len] > popperRect[len]) {
          mainVariationSide = getOppositePlacement(mainVariationSide);
        }
        var altVariationSide = getOppositePlacement(mainVariationSide);
        var checks = [];
        if (checkMainAxis) {
          checks.push(overflow[_basePlacement] <= 0);
        }
        if (checkAltAxis) {
          checks.push(overflow[mainVariationSide] <= 0, overflow[altVariationSide] <= 0);
        }
        if (checks.every(function(check) {
          return check;
        })) {
          firstFittingPlacement = placement;
          makeFallbackChecks = false;
          break;
        }
        checksMap.set(placement, checks);
      }
      if (makeFallbackChecks) {
        var numberOfChecks = flipVariations ? 3 : 1;
        var _loop = function _loop2(_i2) {
          var fittingPlacement = placements.find(function(placement2) {
            var checks2 = checksMap.get(placement2);
            if (checks2) {
              return checks2.slice(0, _i2).every(function(check) {
                return check;
              });
            }
          });
          if (fittingPlacement) {
            firstFittingPlacement = fittingPlacement;
            return "break";
          }
        };
        for (var _i = numberOfChecks; _i > 0; _i--) {
          var _ret = _loop(_i);
          if (_ret === "break")
            break;
        }
      }
      if (state.placement !== firstFittingPlacement) {
        state.modifiersData[name]._skip = true;
        state.placement = firstFittingPlacement;
        state.reset = true;
      }
    }
    const modifiers_flip = {
      name: "flip",
      enabled: true,
      phase: "main",
      fn: flip,
      requiresIfExists: ["offset"],
      data: {
        _skip: false
      }
    };
    ;
    function getAltAxis(axis) {
      return axis === "x" ? "y" : "x";
    }
    ;
    function within(min, value, max) {
      return math_max(min, math_min(value, max));
    }
    function withinMaxClamp(min, value, max) {
      var v = within(min, value, max);
      return v > max ? max : v;
    }
    ;
    function preventOverflow(_ref) {
      var state = _ref.state, options = _ref.options, name = _ref.name;
      var _options$mainAxis = options.mainAxis, checkMainAxis = _options$mainAxis === void 0 ? true : _options$mainAxis, _options$altAxis = options.altAxis, checkAltAxis = _options$altAxis === void 0 ? false : _options$altAxis, boundary = options.boundary, rootBoundary = options.rootBoundary, altBoundary = options.altBoundary, padding = options.padding, _options$tether = options.tether, tether = _options$tether === void 0 ? true : _options$tether, _options$tetherOffset = options.tetherOffset, tetherOffset = _options$tetherOffset === void 0 ? 0 : _options$tetherOffset;
      var overflow = detectOverflow(state, {
        boundary,
        rootBoundary,
        padding,
        altBoundary
      });
      var basePlacement = getBasePlacement(state.placement);
      var variation = getVariation(state.placement);
      var isBasePlacement = !variation;
      var mainAxis = getMainAxisFromPlacement(basePlacement);
      var altAxis = getAltAxis(mainAxis);
      var popperOffsets2 = state.modifiersData.popperOffsets;
      var referenceRect = state.rects.reference;
      var popperRect = state.rects.popper;
      var tetherOffsetValue = typeof tetherOffset === "function" ? tetherOffset(Object.assign({}, state.rects, {
        placement: state.placement
      })) : tetherOffset;
      var normalizedTetherOffsetValue = typeof tetherOffsetValue === "number" ? {
        mainAxis: tetherOffsetValue,
        altAxis: tetherOffsetValue
      } : Object.assign({
        mainAxis: 0,
        altAxis: 0
      }, tetherOffsetValue);
      var offsetModifierState = state.modifiersData.offset ? state.modifiersData.offset[state.placement] : null;
      var data = {
        x: 0,
        y: 0
      };
      if (!popperOffsets2) {
        return;
      }
      if (checkMainAxis) {
        var _offsetModifierState$;
        var mainSide = mainAxis === "y" ? enums_top : left;
        var altSide = mainAxis === "y" ? bottom : right;
        var len = mainAxis === "y" ? "height" : "width";
        var offset2 = popperOffsets2[mainAxis];
        var min = offset2 + overflow[mainSide];
        var max = offset2 - overflow[altSide];
        var additive = tether ? -popperRect[len] / 2 : 0;
        var minLen = variation === start ? referenceRect[len] : popperRect[len];
        var maxLen = variation === start ? -popperRect[len] : -referenceRect[len];
        var arrowElement = state.elements.arrow;
        var arrowRect = tether && arrowElement ? getLayoutRect(arrowElement) : {
          width: 0,
          height: 0
        };
        var arrowPaddingObject = state.modifiersData["arrow#persistent"] ? state.modifiersData["arrow#persistent"].padding : getFreshSideObject();
        var arrowPaddingMin = arrowPaddingObject[mainSide];
        var arrowPaddingMax = arrowPaddingObject[altSide];
        var arrowLen = within(0, referenceRect[len], arrowRect[len]);
        var minOffset = isBasePlacement ? referenceRect[len] / 2 - additive - arrowLen - arrowPaddingMin - normalizedTetherOffsetValue.mainAxis : minLen - arrowLen - arrowPaddingMin - normalizedTetherOffsetValue.mainAxis;
        var maxOffset = isBasePlacement ? -referenceRect[len] / 2 + additive + arrowLen + arrowPaddingMax + normalizedTetherOffsetValue.mainAxis : maxLen + arrowLen + arrowPaddingMax + normalizedTetherOffsetValue.mainAxis;
        var arrowOffsetParent = state.elements.arrow && getOffsetParent(state.elements.arrow);
        var clientOffset = arrowOffsetParent ? mainAxis === "y" ? arrowOffsetParent.clientTop || 0 : arrowOffsetParent.clientLeft || 0 : 0;
        var offsetModifierValue = (_offsetModifierState$ = offsetModifierState == null ? void 0 : offsetModifierState[mainAxis]) != null ? _offsetModifierState$ : 0;
        var tetherMin = offset2 + minOffset - offsetModifierValue - clientOffset;
        var tetherMax = offset2 + maxOffset - offsetModifierValue;
        var preventedOffset = within(tether ? math_min(min, tetherMin) : min, offset2, tether ? math_max(max, tetherMax) : max);
        popperOffsets2[mainAxis] = preventedOffset;
        data[mainAxis] = preventedOffset - offset2;
      }
      if (checkAltAxis) {
        var _offsetModifierState$2;
        var _mainSide = mainAxis === "x" ? enums_top : left;
        var _altSide = mainAxis === "x" ? bottom : right;
        var _offset = popperOffsets2[altAxis];
        var _len = altAxis === "y" ? "height" : "width";
        var _min = _offset + overflow[_mainSide];
        var _max = _offset - overflow[_altSide];
        var isOriginSide = [enums_top, left].indexOf(basePlacement) !== -1;
        var _offsetModifierValue = (_offsetModifierState$2 = offsetModifierState == null ? void 0 : offsetModifierState[altAxis]) != null ? _offsetModifierState$2 : 0;
        var _tetherMin = isOriginSide ? _min : _offset - referenceRect[_len] - popperRect[_len] - _offsetModifierValue + normalizedTetherOffsetValue.altAxis;
        var _tetherMax = isOriginSide ? _offset + referenceRect[_len] + popperRect[_len] - _offsetModifierValue - normalizedTetherOffsetValue.altAxis : _max;
        var _preventedOffset = tether && isOriginSide ? withinMaxClamp(_tetherMin, _offset, _tetherMax) : within(tether ? _tetherMin : _min, _offset, tether ? _tetherMax : _max);
        popperOffsets2[altAxis] = _preventedOffset;
        data[altAxis] = _preventedOffset - _offset;
      }
      state.modifiersData[name] = data;
    }
    const modifiers_preventOverflow = {
      name: "preventOverflow",
      enabled: true,
      phase: "main",
      fn: preventOverflow,
      requiresIfExists: ["offset"]
    };
    ;
    var toPaddingObject = function toPaddingObject2(padding, state) {
      padding = typeof padding === "function" ? padding(Object.assign({}, state.rects, {
        placement: state.placement
      })) : padding;
      return mergePaddingObject(typeof padding !== "number" ? padding : expandToHashMap(padding, basePlacements));
    };
    function arrow(_ref) {
      var _state$modifiersData$;
      var state = _ref.state, name = _ref.name, options = _ref.options;
      var arrowElement = state.elements.arrow;
      var popperOffsets2 = state.modifiersData.popperOffsets;
      var basePlacement = getBasePlacement(state.placement);
      var axis = getMainAxisFromPlacement(basePlacement);
      var isVertical = [left, right].indexOf(basePlacement) >= 0;
      var len = isVertical ? "height" : "width";
      if (!arrowElement || !popperOffsets2) {
        return;
      }
      var paddingObject = toPaddingObject(options.padding, state);
      var arrowRect = getLayoutRect(arrowElement);
      var minProp = axis === "y" ? enums_top : left;
      var maxProp = axis === "y" ? bottom : right;
      var endDiff = state.rects.reference[len] + state.rects.reference[axis] - popperOffsets2[axis] - state.rects.popper[len];
      var startDiff = popperOffsets2[axis] - state.rects.reference[axis];
      var arrowOffsetParent = getOffsetParent(arrowElement);
      var clientSize = arrowOffsetParent ? axis === "y" ? arrowOffsetParent.clientHeight || 0 : arrowOffsetParent.clientWidth || 0 : 0;
      var centerToReference = endDiff / 2 - startDiff / 2;
      var min = paddingObject[minProp];
      var max = clientSize - arrowRect[len] - paddingObject[maxProp];
      var center = clientSize / 2 - arrowRect[len] / 2 + centerToReference;
      var offset2 = within(min, center, max);
      var axisProp = axis;
      state.modifiersData[name] = (_state$modifiersData$ = {}, _state$modifiersData$[axisProp] = offset2, _state$modifiersData$.centerOffset = offset2 - center, _state$modifiersData$);
    }
    function arrow_effect(_ref2) {
      var state = _ref2.state, options = _ref2.options;
      var _options$element = options.element, arrowElement = _options$element === void 0 ? "[data-popper-arrow]" : _options$element;
      if (arrowElement == null) {
        return;
      }
      if (typeof arrowElement === "string") {
        arrowElement = state.elements.popper.querySelector(arrowElement);
        if (!arrowElement) {
          return;
        }
      }
      if (false) {
      }
      if (!contains(state.elements.popper, arrowElement)) {
        if (false) {
        }
        return;
      }
      state.elements.arrow = arrowElement;
    }
    const modifiers_arrow = {
      name: "arrow",
      enabled: true,
      phase: "main",
      fn: arrow,
      effect: arrow_effect,
      requires: ["popperOffsets"],
      requiresIfExists: ["preventOverflow"]
    };
    ;
    function getSideOffsets(overflow, rect, preventedOffsets) {
      if (preventedOffsets === void 0) {
        preventedOffsets = {
          x: 0,
          y: 0
        };
      }
      return {
        top: overflow.top - rect.height - preventedOffsets.y,
        right: overflow.right - rect.width + preventedOffsets.x,
        bottom: overflow.bottom - rect.height + preventedOffsets.y,
        left: overflow.left - rect.width - preventedOffsets.x
      };
    }
    function isAnySideFullyClipped(overflow) {
      return [enums_top, right, bottom, left].some(function(side) {
        return overflow[side] >= 0;
      });
    }
    function hide(_ref) {
      var state = _ref.state, name = _ref.name;
      var referenceRect = state.rects.reference;
      var popperRect = state.rects.popper;
      var preventedOffsets = state.modifiersData.preventOverflow;
      var referenceOverflow = detectOverflow(state, {
        elementContext: "reference"
      });
      var popperAltOverflow = detectOverflow(state, {
        altBoundary: true
      });
      var referenceClippingOffsets = getSideOffsets(referenceOverflow, referenceRect);
      var popperEscapeOffsets = getSideOffsets(popperAltOverflow, popperRect, preventedOffsets);
      var isReferenceHidden = isAnySideFullyClipped(referenceClippingOffsets);
      var hasPopperEscaped = isAnySideFullyClipped(popperEscapeOffsets);
      state.modifiersData[name] = {
        referenceClippingOffsets,
        popperEscapeOffsets,
        isReferenceHidden,
        hasPopperEscaped
      };
      state.attributes.popper = Object.assign({}, state.attributes.popper, {
        "data-popper-reference-hidden": isReferenceHidden,
        "data-popper-escaped": hasPopperEscaped
      });
    }
    const modifiers_hide = {
      name: "hide",
      enabled: true,
      phase: "main",
      requiresIfExists: ["preventOverflow"],
      fn: hide
    };
    ;
    var defaultModifiers = [eventListeners, modifiers_popperOffsets, modifiers_computeStyles, modifiers_applyStyles, modifiers_offset, modifiers_flip, modifiers_preventOverflow, modifiers_arrow, modifiers_hide];
    var popper_createPopper = /* @__PURE__ */ popperGenerator({
      defaultModifiers
    });
    ;
    function dropdown_toConsumableArray(arr) {
      return dropdown_arrayWithoutHoles(arr) || dropdown_iterableToArray(arr) || dropdown_unsupportedIterableToArray(arr) || dropdown_nonIterableSpread();
    }
    function dropdown_nonIterableSpread() {
      throw new TypeError("Invalid attempt to spread non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.");
    }
    function dropdown_unsupportedIterableToArray(o, minLen) {
      if (!o)
        return;
      if (typeof o === "string")
        return dropdown_arrayLikeToArray(o, minLen);
      var n = Object.prototype.toString.call(o).slice(8, -1);
      if (n === "Object" && o.constructor)
        n = o.constructor.name;
      if (n === "Map" || n === "Set")
        return Array.from(o);
      if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n))
        return dropdown_arrayLikeToArray(o, minLen);
    }
    function dropdown_iterableToArray(iter) {
      if (typeof Symbol !== "undefined" && iter[Symbol.iterator] != null || iter["@@iterator"] != null)
        return Array.from(iter);
    }
    function dropdown_arrayWithoutHoles(arr) {
      if (Array.isArray(arr))
        return dropdown_arrayLikeToArray(arr);
    }
    function dropdown_arrayLikeToArray(arr, len) {
      if (len == null || len > arr.length)
        len = arr.length;
      for (var i = 0, arr2 = new Array(len); i < len; i++) {
        arr2[i] = arr[i];
      }
      return arr2;
    }
    function dropdown_ownKeys(object, enumerableOnly) {
      var keys = Object.keys(object);
      if (Object.getOwnPropertySymbols) {
        var symbols = Object.getOwnPropertySymbols(object);
        enumerableOnly && (symbols = symbols.filter(function(sym) {
          return Object.getOwnPropertyDescriptor(object, sym).enumerable;
        })), keys.push.apply(keys, symbols);
      }
      return keys;
    }
    function dropdown_objectSpread(target) {
      for (var i = 1; i < arguments.length; i++) {
        var source = null != arguments[i] ? arguments[i] : {};
        i % 2 ? dropdown_ownKeys(Object(source), true).forEach(function(key) {
          dropdown_defineProperty(target, key, source[key]);
        }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)) : dropdown_ownKeys(Object(source)).forEach(function(key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
      return target;
    }
    function dropdown_defineProperty(obj, key, value) {
      if (key in obj) {
        Object.defineProperty(obj, key, { value, enumerable: true, configurable: true, writable: true });
      } else {
        obj[key] = value;
      }
      return obj;
    }
    function dropdown_classCallCheck(instance, Constructor) {
      if (!(instance instanceof Constructor)) {
        throw new TypeError("Cannot call a class as a function");
      }
    }
    function dropdown_defineProperties(target, props) {
      for (var i = 0; i < props.length; i++) {
        var descriptor = props[i];
        descriptor.enumerable = descriptor.enumerable || false;
        descriptor.configurable = true;
        if ("value" in descriptor)
          descriptor.writable = true;
        Object.defineProperty(target, descriptor.key, descriptor);
      }
    }
    function dropdown_createClass(Constructor, protoProps, staticProps) {
      if (protoProps)
        dropdown_defineProperties(Constructor.prototype, protoProps);
      if (staticProps)
        dropdown_defineProperties(Constructor, staticProps);
      Object.defineProperty(Constructor, "prototype", { writable: false });
      return Constructor;
    }
    var dropdown_Default = {
      placement: "bottom",
      triggerType: "click",
      onShow: function onShow() {
      },
      onHide: function onHide() {
      }
    };
    var Dropdown = /* @__PURE__ */ function() {
      function Dropdown2() {
        var targetElement = arguments.length > 0 && arguments[0] !== void 0 ? arguments[0] : null;
        var triggerElement = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : null;
        var options = arguments.length > 2 && arguments[2] !== void 0 ? arguments[2] : {};
        dropdown_classCallCheck(this, Dropdown2);
        this._targetEl = targetElement;
        this._triggerEl = triggerElement;
        this._options = dropdown_objectSpread(dropdown_objectSpread({}, dropdown_Default), options);
        this._popperInstance = this._createPopperInstace();
        this._visible = false;
        this._init();
      }
      dropdown_createClass(Dropdown2, [{
        key: "_init",
        value: function _init() {
          var _this = this;
          if (this._triggerEl) {
            this._triggerEl.addEventListener("click", function() {
              _this.toggle();
            });
          }
        }
      }, {
        key: "_createPopperInstace",
        value: function _createPopperInstace() {
          return popper_createPopper(this._triggerEl, this._targetEl, {
            placement: this._options.placement,
            modifiers: [{
              name: "offset",
              options: {
                offset: [0, 10]
              }
            }]
          });
        }
      }, {
        key: "_handleClickOutside",
        value: function _handleClickOutside(ev, targetEl) {
          var clickedEl = ev.target;
          if (clickedEl !== targetEl && !targetEl.contains(clickedEl) && !this._triggerEl.contains(clickedEl) && this._visible) {
            this.hide();
          }
          document.body.removeEventListener("click", this._handleClickOutside, true);
        }
      }, {
        key: "toggle",
        value: function toggle() {
          if (this._visible) {
            this.hide();
            document.body.removeEventListener("click", this._handleClickOutside, true);
          } else {
            this.show();
          }
        }
      }, {
        key: "show",
        value: function show() {
          var _this2 = this;
          this._targetEl.classList.remove("hidden");
          this._targetEl.classList.add("block");
          this._popperInstance.setOptions(function(options) {
            return dropdown_objectSpread(dropdown_objectSpread({}, options), {}, {
              modifiers: [].concat(dropdown_toConsumableArray(options.modifiers), [{
                name: "eventListeners",
                enabled: true
              }])
            });
          });
          document.body.addEventListener("click", function(ev) {
            _this2._handleClickOutside(ev, _this2._targetEl);
          }, true);
          this._popperInstance.update();
          this._visible = true;
          this._options.onShow(this);
        }
      }, {
        key: "hide",
        value: function hide2() {
          this._targetEl.classList.remove("block");
          this._targetEl.classList.add("hidden");
          this._popperInstance.setOptions(function(options) {
            return dropdown_objectSpread(dropdown_objectSpread({}, options), {}, {
              modifiers: [].concat(dropdown_toConsumableArray(options.modifiers), [{
                name: "eventListeners",
                enabled: false
              }])
            });
          });
          this._visible = false;
          this._options.onHide(this);
        }
      }]);
      return Dropdown2;
    }();
    window.Dropdown = Dropdown;
    function initDropdown() {
      document.querySelectorAll("[data-dropdown-toggle]").forEach(function(triggerEl) {
        var targetEl = document.getElementById(triggerEl.getAttribute("data-dropdown-toggle"));
        var placement = triggerEl.getAttribute("data-dropdown-placement");
        new Dropdown(targetEl, triggerEl, {
          placement: placement ? placement : dropdown_Default.placement
        });
      });
    }
    if (document.readyState !== "loading") {
      initDropdown();
    } else {
      document.addEventListener("DOMContentLoaded", initDropdown);
    }
    const dropdown = Dropdown;
    ;
    function modal_toConsumableArray(arr) {
      return modal_arrayWithoutHoles(arr) || modal_iterableToArray(arr) || modal_unsupportedIterableToArray(arr) || modal_nonIterableSpread();
    }
    function modal_nonIterableSpread() {
      throw new TypeError("Invalid attempt to spread non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.");
    }
    function modal_unsupportedIterableToArray(o, minLen) {
      if (!o)
        return;
      if (typeof o === "string")
        return modal_arrayLikeToArray(o, minLen);
      var n = Object.prototype.toString.call(o).slice(8, -1);
      if (n === "Object" && o.constructor)
        n = o.constructor.name;
      if (n === "Map" || n === "Set")
        return Array.from(o);
      if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n))
        return modal_arrayLikeToArray(o, minLen);
    }
    function modal_iterableToArray(iter) {
      if (typeof Symbol !== "undefined" && iter[Symbol.iterator] != null || iter["@@iterator"] != null)
        return Array.from(iter);
    }
    function modal_arrayWithoutHoles(arr) {
      if (Array.isArray(arr))
        return modal_arrayLikeToArray(arr);
    }
    function modal_arrayLikeToArray(arr, len) {
      if (len == null || len > arr.length)
        len = arr.length;
      for (var i = 0, arr2 = new Array(len); i < len; i++) {
        arr2[i] = arr[i];
      }
      return arr2;
    }
    function modal_ownKeys(object, enumerableOnly) {
      var keys = Object.keys(object);
      if (Object.getOwnPropertySymbols) {
        var symbols = Object.getOwnPropertySymbols(object);
        enumerableOnly && (symbols = symbols.filter(function(sym) {
          return Object.getOwnPropertyDescriptor(object, sym).enumerable;
        })), keys.push.apply(keys, symbols);
      }
      return keys;
    }
    function modal_objectSpread(target) {
      for (var i = 1; i < arguments.length; i++) {
        var source = null != arguments[i] ? arguments[i] : {};
        i % 2 ? modal_ownKeys(Object(source), true).forEach(function(key) {
          modal_defineProperty(target, key, source[key]);
        }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)) : modal_ownKeys(Object(source)).forEach(function(key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
      return target;
    }
    function modal_defineProperty(obj, key, value) {
      if (key in obj) {
        Object.defineProperty(obj, key, { value, enumerable: true, configurable: true, writable: true });
      } else {
        obj[key] = value;
      }
      return obj;
    }
    function modal_classCallCheck(instance, Constructor) {
      if (!(instance instanceof Constructor)) {
        throw new TypeError("Cannot call a class as a function");
      }
    }
    function modal_defineProperties(target, props) {
      for (var i = 0; i < props.length; i++) {
        var descriptor = props[i];
        descriptor.enumerable = descriptor.enumerable || false;
        descriptor.configurable = true;
        if ("value" in descriptor)
          descriptor.writable = true;
        Object.defineProperty(target, descriptor.key, descriptor);
      }
    }
    function modal_createClass(Constructor, protoProps, staticProps) {
      if (protoProps)
        modal_defineProperties(Constructor.prototype, protoProps);
      if (staticProps)
        modal_defineProperties(Constructor, staticProps);
      Object.defineProperty(Constructor, "prototype", { writable: false });
      return Constructor;
    }
    var modal_Default = {
      placement: "center",
      backdropClasses: "bg-gray-900 bg-opacity-50 dark:bg-opacity-80 fixed inset-0 z-40",
      onHide: function onHide() {
      },
      onShow: function onShow() {
      },
      onToggle: function onToggle() {
      }
    };
    var Modal = /* @__PURE__ */ function() {
      function Modal2() {
        var targetEl = arguments.length > 0 && arguments[0] !== void 0 ? arguments[0] : null;
        var options = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : {};
        modal_classCallCheck(this, Modal2);
        this._targetEl = targetEl;
        this._options = modal_objectSpread(modal_objectSpread({}, modal_Default), options);
        this._isHidden = true;
        this._init();
      }
      modal_createClass(Modal2, [{
        key: "_init",
        value: function _init() {
          var _this = this;
          this._getPlacementClasses().map(function(c) {
            _this._targetEl.classList.add(c);
          });
        }
      }, {
        key: "_createBackdrop",
        value: function _createBackdrop() {
          if (this._isHidden) {
            var _backdropEl$classList;
            var backdropEl = document.createElement("div");
            backdropEl.setAttribute("modal-backdrop", "");
            (_backdropEl$classList = backdropEl.classList).add.apply(_backdropEl$classList, modal_toConsumableArray(this._options.backdropClasses.split(" ")));
            document.querySelector("body").append(backdropEl);
          }
        }
      }, {
        key: "_destroyBackdropEl",
        value: function _destroyBackdropEl() {
          if (!this._isHidden) {
            document.querySelector("[modal-backdrop]").remove();
          }
        }
      }, {
        key: "_getPlacementClasses",
        value: function _getPlacementClasses() {
          switch (this._options.placement) {
            case "top-left":
              return ["justify-start", "items-start"];
            case "top-center":
              return ["justify-center", "items-start"];
            case "top-right":
              return ["justify-end", "items-start"];
            case "center-left":
              return ["justify-start", "items-center"];
            case "center":
              return ["justify-center", "items-center"];
            case "center-right":
              return ["justify-end", "items-center"];
            case "bottom-left":
              return ["justify-start", "items-end"];
            case "bottom-center":
              return ["justify-center", "items-end"];
            case "bottom-right":
              return ["justify-end", "items-end"];
            default:
              return ["justify-center", "items-center"];
          }
        }
      }, {
        key: "toggle",
        value: function toggle() {
          if (this._isHidden) {
            this.show();
          } else {
            this.hide();
          }
          this._options.onToggle(this);
        }
      }, {
        key: "show",
        value: function show() {
          this._targetEl.classList.add("flex");
          this._targetEl.classList.remove("hidden");
          this._targetEl.setAttribute("aria-modal", "true");
          this._targetEl.setAttribute("role", "dialog");
          this._targetEl.removeAttribute("aria-hidden");
          this._createBackdrop();
          this._isHidden = false;
          this._options.onShow(this);
        }
      }, {
        key: "hide",
        value: function hide2() {
          this._targetEl.classList.add("hidden");
          this._targetEl.classList.remove("flex");
          this._targetEl.setAttribute("aria-hidden", "true");
          this._targetEl.removeAttribute("aria-modal");
          this._targetEl.removeAttribute("role");
          this._destroyBackdropEl();
          this._isHidden = true;
          this._options.onHide(this);
        }
      }]);
      return Modal2;
    }();
    window.Modal = Modal;
    var getModalInstance = function getModalInstance2(id, instances) {
      if (instances.some(function(modalInstance) {
        return modalInstance.id === id;
      })) {
        return instances.find(function(modalInstance) {
          return modalInstance.id === id;
        });
      }
      return false;
    };
    function initModal() {
      var modalInstances = [];
      document.querySelectorAll("[data-modal-toggle]").forEach(function(el) {
        var modalId = el.getAttribute("data-modal-toggle");
        var modalEl = document.getElementById(modalId);
        var placement = modalEl.getAttribute("data-modal-placement");
        if (modalEl) {
          if (!modalEl.hasAttribute("aria-hidden") && !modalEl.hasAttribute("aria-modal")) {
            modalEl.setAttribute("aria-hidden", "true");
          }
        }
        var modal2 = null;
        if (getModalInstance(modalId, modalInstances)) {
          modal2 = getModalInstance(modalId, modalInstances);
          modal2 = modal2.object;
        } else {
          modal2 = new Modal(modalEl, {
            placement: placement ? placement : modal_Default.placement
          });
          modalInstances.push({
            id: modalId,
            object: modal2
          });
        }
        if (modalEl.hasAttribute("data-modal-show") && modalEl.getAttribute("data-modal-show") === "true") {
          modal2.show();
        }
        el.addEventListener("click", function() {
          modal2.toggle();
        });
      });
    }
    if (document.readyState !== "loading") {
      initModal();
    } else {
      document.addEventListener("DOMContentLoaded", initModal);
    }
    const modal = Modal;
    ;
    function drawer_toConsumableArray(arr) {
      return drawer_arrayWithoutHoles(arr) || drawer_iterableToArray(arr) || drawer_unsupportedIterableToArray(arr) || drawer_nonIterableSpread();
    }
    function drawer_nonIterableSpread() {
      throw new TypeError("Invalid attempt to spread non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.");
    }
    function drawer_unsupportedIterableToArray(o, minLen) {
      if (!o)
        return;
      if (typeof o === "string")
        return drawer_arrayLikeToArray(o, minLen);
      var n = Object.prototype.toString.call(o).slice(8, -1);
      if (n === "Object" && o.constructor)
        n = o.constructor.name;
      if (n === "Map" || n === "Set")
        return Array.from(o);
      if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n))
        return drawer_arrayLikeToArray(o, minLen);
    }
    function drawer_iterableToArray(iter) {
      if (typeof Symbol !== "undefined" && iter[Symbol.iterator] != null || iter["@@iterator"] != null)
        return Array.from(iter);
    }
    function drawer_arrayWithoutHoles(arr) {
      if (Array.isArray(arr))
        return drawer_arrayLikeToArray(arr);
    }
    function drawer_arrayLikeToArray(arr, len) {
      if (len == null || len > arr.length)
        len = arr.length;
      for (var i = 0, arr2 = new Array(len); i < len; i++) {
        arr2[i] = arr[i];
      }
      return arr2;
    }
    function drawer_ownKeys(object, enumerableOnly) {
      var keys = Object.keys(object);
      if (Object.getOwnPropertySymbols) {
        var symbols = Object.getOwnPropertySymbols(object);
        enumerableOnly && (symbols = symbols.filter(function(sym) {
          return Object.getOwnPropertyDescriptor(object, sym).enumerable;
        })), keys.push.apply(keys, symbols);
      }
      return keys;
    }
    function drawer_objectSpread(target) {
      for (var i = 1; i < arguments.length; i++) {
        var source = null != arguments[i] ? arguments[i] : {};
        i % 2 ? drawer_ownKeys(Object(source), true).forEach(function(key) {
          drawer_defineProperty(target, key, source[key]);
        }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)) : drawer_ownKeys(Object(source)).forEach(function(key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
      return target;
    }
    function drawer_defineProperty(obj, key, value) {
      if (key in obj) {
        Object.defineProperty(obj, key, { value, enumerable: true, configurable: true, writable: true });
      } else {
        obj[key] = value;
      }
      return obj;
    }
    function drawer_classCallCheck(instance, Constructor) {
      if (!(instance instanceof Constructor)) {
        throw new TypeError("Cannot call a class as a function");
      }
    }
    function drawer_defineProperties(target, props) {
      for (var i = 0; i < props.length; i++) {
        var descriptor = props[i];
        descriptor.enumerable = descriptor.enumerable || false;
        descriptor.configurable = true;
        if ("value" in descriptor)
          descriptor.writable = true;
        Object.defineProperty(target, descriptor.key, descriptor);
      }
    }
    function drawer_createClass(Constructor, protoProps, staticProps) {
      if (protoProps)
        drawer_defineProperties(Constructor.prototype, protoProps);
      if (staticProps)
        drawer_defineProperties(Constructor, staticProps);
      Object.defineProperty(Constructor, "prototype", { writable: false });
      return Constructor;
    }
    var drawer_Default = {
      placement: "left",
      bodyScrolling: false,
      backdrop: true,
      edge: false,
      edgeOffset: "bottom-[60px]",
      backdropClasses: "bg-gray-900 bg-opacity-50 dark:bg-opacity-80 fixed inset-0 z-30",
      onShow: function onShow() {
      },
      onHide: function onHide() {
      },
      onToggle: function onToggle() {
      }
    };
    var Drawer = /* @__PURE__ */ function() {
      function Drawer2() {
        var targetEl = arguments.length > 0 && arguments[0] !== void 0 ? arguments[0] : null;
        var options = arguments.length > 1 ? arguments[1] : void 0;
        drawer_classCallCheck(this, Drawer2);
        this._targetEl = targetEl;
        this._options = drawer_objectSpread(drawer_objectSpread({}, drawer_Default), options);
        this._visible = false;
        this._init();
      }
      drawer_createClass(Drawer2, [{
        key: "_init",
        value: function _init() {
          var _this = this;
          if (this._targetEl) {
            this._targetEl.setAttribute("aria-hidden", "true");
            this._targetEl.classList.add("transition-transform");
          }
          this._getPlacementClasses(this._options.placement).base.map(function(c) {
            _this._targetEl.classList.add(c);
          });
          this.hide();
        }
      }, {
        key: "isVisible",
        value: function isVisible() {
          return this._visible;
        }
      }, {
        key: "hide",
        value: function hide2() {
          var _this2 = this;
          if (this._options.edge) {
            this._getPlacementClasses(this._options.placement + "-edge").active.map(function(c) {
              _this2._targetEl.classList.remove(c);
            });
            this._getPlacementClasses(this._options.placement + "-edge").inactive.map(function(c) {
              _this2._targetEl.classList.add(c);
            });
          } else {
            this._getPlacementClasses(this._options.placement).active.map(function(c) {
              _this2._targetEl.classList.remove(c);
            });
            this._getPlacementClasses(this._options.placement).inactive.map(function(c) {
              _this2._targetEl.classList.add(c);
            });
          }
          this._targetEl.setAttribute("aria-hidden", "true");
          this._targetEl.removeAttribute("aria-modal");
          this._targetEl.removeAttribute("role");
          if (!this._options.bodyScrolling) {
            document.body.classList.remove("overflow-hidden");
          }
          if (this._options.backdrop) {
            this._destroyBackdropEl();
          }
          this._visible = false;
          this._options.onHide(this);
        }
      }, {
        key: "show",
        value: function show() {
          var _this3 = this;
          if (this._options.edge) {
            this._getPlacementClasses(this._options.placement + "-edge").active.map(function(c) {
              _this3._targetEl.classList.add(c);
            });
            this._getPlacementClasses(this._options.placement + "-edge").inactive.map(function(c) {
              _this3._targetEl.classList.remove(c);
            });
          } else {
            this._getPlacementClasses(this._options.placement).active.map(function(c) {
              _this3._targetEl.classList.add(c);
            });
            this._getPlacementClasses(this._options.placement).inactive.map(function(c) {
              _this3._targetEl.classList.remove(c);
            });
          }
          this._targetEl.setAttribute("aria-modal", "true");
          this._targetEl.setAttribute("role", "dialog");
          this._targetEl.removeAttribute("aria-hidden");
          if (!this._options.bodyScrolling) {
            document.body.classList.add("overflow-hidden");
          }
          if (this._options.backdrop) {
            this._createBackdrop();
          }
          this._visible = true;
          this._options.onShow(this);
        }
      }, {
        key: "toggle",
        value: function toggle() {
          if (this.isVisible()) {
            this.hide();
          } else {
            this.show();
          }
        }
      }, {
        key: "_createBackdrop",
        value: function _createBackdrop() {
          var _this4 = this;
          if (!this._visible) {
            var _backdropEl$classList;
            var backdropEl = document.createElement("div");
            backdropEl.setAttribute("drawer-backdrop", "");
            (_backdropEl$classList = backdropEl.classList).add.apply(_backdropEl$classList, drawer_toConsumableArray(this._options.backdropClasses.split(" ")));
            document.querySelector("body").append(backdropEl);
            backdropEl.addEventListener("click", function() {
              _this4.hide();
            });
          }
        }
      }, {
        key: "_destroyBackdropEl",
        value: function _destroyBackdropEl() {
          if (this._visible) {
            document.querySelector("[drawer-backdrop]").remove();
          }
        }
      }, {
        key: "_getPlacementClasses",
        value: function _getPlacementClasses(placement) {
          switch (placement) {
            case "top":
              return {
                base: ["top-0", "left-0", "right-0"],
                active: ["transform-none"],
                inactive: ["-translate-y-full"]
              };
            case "right":
              return {
                base: ["right-0", "top-0"],
                active: ["transform-none"],
                inactive: ["translate-x-full"]
              };
            case "bottom":
              return {
                base: ["bottom-0", "left-0", "right-0"],
                active: ["transform-none"],
                inactive: ["translate-y-full"]
              };
            case "left":
              return {
                base: ["left-0", "top-0"],
                active: ["transform-none"],
                inactive: ["-translate-x-full"]
              };
            case "bottom-edge":
              return {
                base: ["left-0", "top-0"],
                active: ["transform-none"],
                inactive: ["translate-y-full", this._options.edgeOffset]
              };
            default:
              return {
                base: ["left-0", "top-0"],
                active: ["transform-none"],
                inactive: ["-translate-x-full"]
              };
          }
        }
      }]);
      return Drawer2;
    }();
    window.Drawer = Drawer;
    var getDrawerInstance = function getDrawerInstance2(id, instances) {
      if (instances.some(function(drawerInstance) {
        return drawerInstance.id === id;
      })) {
        return instances.find(function(drawerInstance) {
          return drawerInstance.id === id;
        });
      }
      return false;
    };
    function initDrawer() {
      var drawerInstances = [];
      document.querySelectorAll("[data-drawer-target]").forEach(function(triggerEl) {
        var targetEl = document.getElementById(triggerEl.getAttribute("data-drawer-target"));
        var drawerId = targetEl.id;
        var placement = triggerEl.getAttribute("data-drawer-placement");
        var bodyScrolling = triggerEl.getAttribute("data-drawer-body-scrolling");
        var backdrop = triggerEl.getAttribute("data-drawer-backdrop");
        var edge = triggerEl.getAttribute("data-drawer-edge");
        var edgeOffset = triggerEl.getAttribute("data-drawer-edge-offset");
        var drawer2 = null;
        if (getDrawerInstance(drawerId, drawerInstances)) {
          drawer2 = getDrawerInstance(drawerId, drawerInstances);
          drawer2 = drawer2.object;
        } else {
          drawer2 = new Drawer(targetEl, {
            placement: placement ? placement : drawer_Default.placement,
            bodyScrolling: bodyScrolling ? bodyScrolling === "true" ? true : false : drawer_Default.bodyScrolling,
            backdrop: backdrop ? backdrop === "true" ? true : false : drawer_Default.backdrop,
            edge: edge ? edge === "true" ? true : false : drawer_Default.edge,
            edgeOffset: edgeOffset ? edgeOffset : drawer_Default.edgeOffset
          });
          drawerInstances.push({
            id: drawerId,
            object: drawer2
          });
        }
      });
      document.querySelectorAll("[data-drawer-toggle]").forEach(function(triggerEl) {
        var targetEl = document.getElementById(triggerEl.getAttribute("data-drawer-toggle"));
        var drawerId = targetEl.id;
        var drawer2 = getDrawerInstance(drawerId, drawerInstances);
        triggerEl.addEventListener("click", function() {
          if (drawer2.object.isVisible()) {
            drawer2.object.hide();
          } else {
            drawer2.object.show();
          }
        });
      });
      document.querySelectorAll("[data-drawer-dismiss]").forEach(function(triggerEl) {
        var targetEl = document.getElementById(triggerEl.getAttribute("data-drawer-dismiss"));
        var drawerId = targetEl.id;
        var drawer2 = getDrawerInstance(drawerId, drawerInstances);
        triggerEl.addEventListener("click", function() {
          drawer2.object.hide();
        });
      });
      document.querySelectorAll("[data-drawer-show]").forEach(function(triggerEl) {
        var targetEl = document.getElementById(triggerEl.getAttribute("data-drawer-show"));
        var drawerId = targetEl.id;
        var drawer2 = getDrawerInstance(drawerId, drawerInstances);
        triggerEl.addEventListener("click", function() {
          drawer2.object.show();
        });
      });
    }
    if (document.readyState !== "loading") {
      initDrawer();
    } else {
      document.addEventListener("DOMContentLoaded", initDrawer);
    }
    const drawer = Drawer;
    ;
    function tabs_toConsumableArray(arr) {
      return tabs_arrayWithoutHoles(arr) || tabs_iterableToArray(arr) || tabs_unsupportedIterableToArray(arr) || tabs_nonIterableSpread();
    }
    function tabs_nonIterableSpread() {
      throw new TypeError("Invalid attempt to spread non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.");
    }
    function tabs_unsupportedIterableToArray(o, minLen) {
      if (!o)
        return;
      if (typeof o === "string")
        return tabs_arrayLikeToArray(o, minLen);
      var n = Object.prototype.toString.call(o).slice(8, -1);
      if (n === "Object" && o.constructor)
        n = o.constructor.name;
      if (n === "Map" || n === "Set")
        return Array.from(o);
      if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n))
        return tabs_arrayLikeToArray(o, minLen);
    }
    function tabs_iterableToArray(iter) {
      if (typeof Symbol !== "undefined" && iter[Symbol.iterator] != null || iter["@@iterator"] != null)
        return Array.from(iter);
    }
    function tabs_arrayWithoutHoles(arr) {
      if (Array.isArray(arr))
        return tabs_arrayLikeToArray(arr);
    }
    function tabs_arrayLikeToArray(arr, len) {
      if (len == null || len > arr.length)
        len = arr.length;
      for (var i = 0, arr2 = new Array(len); i < len; i++) {
        arr2[i] = arr[i];
      }
      return arr2;
    }
    function tabs_ownKeys(object, enumerableOnly) {
      var keys = Object.keys(object);
      if (Object.getOwnPropertySymbols) {
        var symbols = Object.getOwnPropertySymbols(object);
        enumerableOnly && (symbols = symbols.filter(function(sym) {
          return Object.getOwnPropertyDescriptor(object, sym).enumerable;
        })), keys.push.apply(keys, symbols);
      }
      return keys;
    }
    function tabs_objectSpread(target) {
      for (var i = 1; i < arguments.length; i++) {
        var source = null != arguments[i] ? arguments[i] : {};
        i % 2 ? tabs_ownKeys(Object(source), true).forEach(function(key) {
          tabs_defineProperty(target, key, source[key]);
        }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)) : tabs_ownKeys(Object(source)).forEach(function(key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
      return target;
    }
    function tabs_defineProperty(obj, key, value) {
      if (key in obj) {
        Object.defineProperty(obj, key, { value, enumerable: true, configurable: true, writable: true });
      } else {
        obj[key] = value;
      }
      return obj;
    }
    function tabs_classCallCheck(instance, Constructor) {
      if (!(instance instanceof Constructor)) {
        throw new TypeError("Cannot call a class as a function");
      }
    }
    function tabs_defineProperties(target, props) {
      for (var i = 0; i < props.length; i++) {
        var descriptor = props[i];
        descriptor.enumerable = descriptor.enumerable || false;
        descriptor.configurable = true;
        if ("value" in descriptor)
          descriptor.writable = true;
        Object.defineProperty(target, descriptor.key, descriptor);
      }
    }
    function tabs_createClass(Constructor, protoProps, staticProps) {
      if (protoProps)
        tabs_defineProperties(Constructor.prototype, protoProps);
      if (staticProps)
        tabs_defineProperties(Constructor, staticProps);
      Object.defineProperty(Constructor, "prototype", { writable: false });
      return Constructor;
    }
    var tabs_Default = {
      defaultTabId: null,
      activeClasses: "text-blue-600 hover:text-blue-600 dark:text-blue-500 dark:hover:text-blue-500 border-blue-600 dark:border-blue-500",
      inactiveClasses: "dark:border-transparent text-gray-500 hover:text-gray-600 dark:text-gray-400 border-gray-100 hover:border-gray-300 dark:border-gray-700 dark:hover:text-gray-300",
      onShow: function onShow() {
      }
    };
    var Tabs = /* @__PURE__ */ function() {
      function Tabs2() {
        var items = arguments.length > 0 && arguments[0] !== void 0 ? arguments[0] : [];
        var options = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : {};
        tabs_classCallCheck(this, Tabs2);
        this._items = items;
        this._activeTab = options ? this.getTab(options.defaultTabId) : null;
        this._options = tabs_objectSpread(tabs_objectSpread({}, tabs_Default), options);
        this._init();
      }
      tabs_createClass(Tabs2, [{
        key: "_init",
        value: function _init() {
          var _this = this;
          if (this._items.length) {
            if (!this._activeTab) {
              this._setActiveTab(this._items[0]);
            }
            this.show(this._activeTab.id, true);
            this._items.map(function(tab) {
              tab.triggerEl.addEventListener("click", function() {
                _this.show(tab.id);
              });
            });
          }
        }
      }, {
        key: "getActiveTab",
        value: function getActiveTab() {
          return this._activeTab;
        }
      }, {
        key: "_setActiveTab",
        value: function _setActiveTab(tab) {
          this._activeTab = tab;
        }
      }, {
        key: "getTab",
        value: function getTab(id) {
          return this._items.filter(function(t) {
            return t.id === id;
          })[0];
        }
      }, {
        key: "show",
        value: function show(id) {
          var _this2 = this, _tab$triggerEl$classL, _tab$triggerEl$classL2;
          var forceShow = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : false;
          var tab = this.getTab(id);
          if (tab === this._activeTab && !forceShow) {
            return;
          }
          this._items.map(function(t) {
            if (t !== tab) {
              var _t$triggerEl$classLis, _t$triggerEl$classLis2;
              (_t$triggerEl$classLis = t.triggerEl.classList).remove.apply(_t$triggerEl$classLis, tabs_toConsumableArray(_this2._options.activeClasses.split(" ")));
              (_t$triggerEl$classLis2 = t.triggerEl.classList).add.apply(_t$triggerEl$classLis2, tabs_toConsumableArray(_this2._options.inactiveClasses.split(" ")));
              t.targetEl.classList.add("hidden");
              t.triggerEl.setAttribute("aria-selected", false);
            }
          });
          (_tab$triggerEl$classL = tab.triggerEl.classList).add.apply(_tab$triggerEl$classL, tabs_toConsumableArray(this._options.activeClasses.split(" ")));
          (_tab$triggerEl$classL2 = tab.triggerEl.classList).remove.apply(_tab$triggerEl$classL2, tabs_toConsumableArray(this._options.inactiveClasses.split(" ")));
          tab.triggerEl.setAttribute("aria-selected", true);
          tab.targetEl.classList.remove("hidden");
          this._setActiveTab(tab);
          this._options.onShow(this, tab);
        }
      }]);
      return Tabs2;
    }();
    window.Tabs = Tabs;
    function initTabs() {
      document.querySelectorAll("[data-tabs-toggle]").forEach(function(triggerEl) {
        var tabElements = [];
        var defaultTabId = null;
        triggerEl.querySelectorAll('[role="tab"]').forEach(function(el) {
          var isActive = el.getAttribute("aria-selected") === "true";
          var tab = {
            id: el.getAttribute("data-tabs-target"),
            triggerEl: el,
            targetEl: document.querySelector(el.getAttribute("data-tabs-target"))
          };
          tabElements.push(tab);
          if (isActive) {
            defaultTabId = tab.id;
          }
        });
        new Tabs(tabElements, {
          defaultTabId
        });
      });
    }
    if (document.readyState !== "loading") {
      initTabs();
    } else {
      document.addEventListener("DOMContentLoaded", initTabs);
    }
    const tabs = Tabs;
    ;
    function tooltip_toConsumableArray(arr) {
      return tooltip_arrayWithoutHoles(arr) || tooltip_iterableToArray(arr) || tooltip_unsupportedIterableToArray(arr) || tooltip_nonIterableSpread();
    }
    function tooltip_nonIterableSpread() {
      throw new TypeError("Invalid attempt to spread non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.");
    }
    function tooltip_unsupportedIterableToArray(o, minLen) {
      if (!o)
        return;
      if (typeof o === "string")
        return tooltip_arrayLikeToArray(o, minLen);
      var n = Object.prototype.toString.call(o).slice(8, -1);
      if (n === "Object" && o.constructor)
        n = o.constructor.name;
      if (n === "Map" || n === "Set")
        return Array.from(o);
      if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n))
        return tooltip_arrayLikeToArray(o, minLen);
    }
    function tooltip_iterableToArray(iter) {
      if (typeof Symbol !== "undefined" && iter[Symbol.iterator] != null || iter["@@iterator"] != null)
        return Array.from(iter);
    }
    function tooltip_arrayWithoutHoles(arr) {
      if (Array.isArray(arr))
        return tooltip_arrayLikeToArray(arr);
    }
    function tooltip_arrayLikeToArray(arr, len) {
      if (len == null || len > arr.length)
        len = arr.length;
      for (var i = 0, arr2 = new Array(len); i < len; i++) {
        arr2[i] = arr[i];
      }
      return arr2;
    }
    function tooltip_ownKeys(object, enumerableOnly) {
      var keys = Object.keys(object);
      if (Object.getOwnPropertySymbols) {
        var symbols = Object.getOwnPropertySymbols(object);
        enumerableOnly && (symbols = symbols.filter(function(sym) {
          return Object.getOwnPropertyDescriptor(object, sym).enumerable;
        })), keys.push.apply(keys, symbols);
      }
      return keys;
    }
    function tooltip_objectSpread(target) {
      for (var i = 1; i < arguments.length; i++) {
        var source = null != arguments[i] ? arguments[i] : {};
        i % 2 ? tooltip_ownKeys(Object(source), true).forEach(function(key) {
          tooltip_defineProperty(target, key, source[key]);
        }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)) : tooltip_ownKeys(Object(source)).forEach(function(key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
      return target;
    }
    function tooltip_defineProperty(obj, key, value) {
      if (key in obj) {
        Object.defineProperty(obj, key, { value, enumerable: true, configurable: true, writable: true });
      } else {
        obj[key] = value;
      }
      return obj;
    }
    function tooltip_classCallCheck(instance, Constructor) {
      if (!(instance instanceof Constructor)) {
        throw new TypeError("Cannot call a class as a function");
      }
    }
    function tooltip_defineProperties(target, props) {
      for (var i = 0; i < props.length; i++) {
        var descriptor = props[i];
        descriptor.enumerable = descriptor.enumerable || false;
        descriptor.configurable = true;
        if ("value" in descriptor)
          descriptor.writable = true;
        Object.defineProperty(target, descriptor.key, descriptor);
      }
    }
    function tooltip_createClass(Constructor, protoProps, staticProps) {
      if (protoProps)
        tooltip_defineProperties(Constructor.prototype, protoProps);
      if (staticProps)
        tooltip_defineProperties(Constructor, staticProps);
      Object.defineProperty(Constructor, "prototype", { writable: false });
      return Constructor;
    }
    var tooltip_Default = {
      placement: "top",
      triggerType: "hover",
      onShow: function onShow() {
      },
      onHide: function onHide() {
      }
    };
    var Tooltip = /* @__PURE__ */ function() {
      function Tooltip2() {
        var targetEl = arguments.length > 0 && arguments[0] !== void 0 ? arguments[0] : null;
        var triggerEl = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : null;
        var options = arguments.length > 2 && arguments[2] !== void 0 ? arguments[2] : {};
        tooltip_classCallCheck(this, Tooltip2);
        this._targetEl = targetEl;
        this._triggerEl = triggerEl;
        this._options = tooltip_objectSpread(tooltip_objectSpread({}, tooltip_Default), options);
        this._popperInstance = this._createPopperInstace();
        this._init();
      }
      tooltip_createClass(Tooltip2, [{
        key: "_init",
        value: function _init() {
          var _this = this;
          if (this._triggerEl) {
            var triggerEvents = this._getTriggerEvents();
            triggerEvents.showEvents.forEach(function(ev) {
              _this._triggerEl.addEventListener(ev, function() {
                _this.show();
              });
            });
            triggerEvents.hideEvents.forEach(function(ev) {
              _this._triggerEl.addEventListener(ev, function() {
                _this.hide();
              });
            });
          }
        }
      }, {
        key: "_createPopperInstace",
        value: function _createPopperInstace() {
          return popper_createPopper(this._triggerEl, this._targetEl, {
            placement: this._options.placement,
            modifiers: [{
              name: "offset",
              options: {
                offset: [0, 8]
              }
            }]
          });
        }
      }, {
        key: "_getTriggerEvents",
        value: function _getTriggerEvents() {
          switch (this._options.triggerType) {
            case "hover":
              return {
                showEvents: ["mouseenter", "focus"],
                hideEvents: ["mouseleave", "blur"]
              };
            case "click":
              return {
                showEvents: ["click", "focus"],
                hideEvents: ["focusout", "blur"]
              };
            default:
              return {
                showEvents: ["mouseenter", "focus"],
                hideEvents: ["mouseleave", "blur"]
              };
          }
        }
      }, {
        key: "show",
        value: function show() {
          this._targetEl.classList.remove("opacity-0", "invisible");
          this._targetEl.classList.add("opacity-100", "visible");
          this._popperInstance.setOptions(function(options) {
            return tooltip_objectSpread(tooltip_objectSpread({}, options), {}, {
              modifiers: [].concat(tooltip_toConsumableArray(options.modifiers), [{
                name: "eventListeners",
                enabled: true
              }])
            });
          });
          this._popperInstance.update();
          this._options.onShow(this);
        }
      }, {
        key: "hide",
        value: function hide2() {
          this._targetEl.classList.remove("opacity-100", "visible");
          this._targetEl.classList.add("opacity-0", "invisible");
          this._popperInstance.setOptions(function(options) {
            return tooltip_objectSpread(tooltip_objectSpread({}, options), {}, {
              modifiers: [].concat(tooltip_toConsumableArray(options.modifiers), [{
                name: "eventListeners",
                enabled: false
              }])
            });
          });
          this._options.onHide(this);
        }
      }]);
      return Tooltip2;
    }();
    window.Tooltip = Tooltip;
    function initTooltip() {
      document.querySelectorAll("[data-tooltip-target]").forEach(function(triggerEl) {
        var targetEl = document.getElementById(triggerEl.getAttribute("data-tooltip-target"));
        var triggerType = triggerEl.getAttribute("data-tooltip-trigger");
        var placement = triggerEl.getAttribute("data-tooltip-placement");
        new Tooltip(targetEl, triggerEl, {
          placement: placement ? placement : tooltip_Default.placement,
          triggerType: triggerType ? triggerType : tooltip_Default.triggerType
        });
      });
    }
    if (document.readyState !== "loading") {
      initTooltip();
    } else {
      document.addEventListener("DOMContentLoaded", initTooltip);
    }
    const tooltip = Tooltip;
    ;
    function popover_toConsumableArray(arr) {
      return popover_arrayWithoutHoles(arr) || popover_iterableToArray(arr) || popover_unsupportedIterableToArray(arr) || popover_nonIterableSpread();
    }
    function popover_nonIterableSpread() {
      throw new TypeError("Invalid attempt to spread non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.");
    }
    function popover_unsupportedIterableToArray(o, minLen) {
      if (!o)
        return;
      if (typeof o === "string")
        return popover_arrayLikeToArray(o, minLen);
      var n = Object.prototype.toString.call(o).slice(8, -1);
      if (n === "Object" && o.constructor)
        n = o.constructor.name;
      if (n === "Map" || n === "Set")
        return Array.from(o);
      if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n))
        return popover_arrayLikeToArray(o, minLen);
    }
    function popover_iterableToArray(iter) {
      if (typeof Symbol !== "undefined" && iter[Symbol.iterator] != null || iter["@@iterator"] != null)
        return Array.from(iter);
    }
    function popover_arrayWithoutHoles(arr) {
      if (Array.isArray(arr))
        return popover_arrayLikeToArray(arr);
    }
    function popover_arrayLikeToArray(arr, len) {
      if (len == null || len > arr.length)
        len = arr.length;
      for (var i = 0, arr2 = new Array(len); i < len; i++) {
        arr2[i] = arr[i];
      }
      return arr2;
    }
    function popover_ownKeys(object, enumerableOnly) {
      var keys = Object.keys(object);
      if (Object.getOwnPropertySymbols) {
        var symbols = Object.getOwnPropertySymbols(object);
        enumerableOnly && (symbols = symbols.filter(function(sym) {
          return Object.getOwnPropertyDescriptor(object, sym).enumerable;
        })), keys.push.apply(keys, symbols);
      }
      return keys;
    }
    function popover_objectSpread(target) {
      for (var i = 1; i < arguments.length; i++) {
        var source = null != arguments[i] ? arguments[i] : {};
        i % 2 ? popover_ownKeys(Object(source), true).forEach(function(key) {
          popover_defineProperty(target, key, source[key]);
        }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)) : popover_ownKeys(Object(source)).forEach(function(key) {
          Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key));
        });
      }
      return target;
    }
    function popover_defineProperty(obj, key, value) {
      if (key in obj) {
        Object.defineProperty(obj, key, { value, enumerable: true, configurable: true, writable: true });
      } else {
        obj[key] = value;
      }
      return obj;
    }
    function popover_classCallCheck(instance, Constructor) {
      if (!(instance instanceof Constructor)) {
        throw new TypeError("Cannot call a class as a function");
      }
    }
    function popover_defineProperties(target, props) {
      for (var i = 0; i < props.length; i++) {
        var descriptor = props[i];
        descriptor.enumerable = descriptor.enumerable || false;
        descriptor.configurable = true;
        if ("value" in descriptor)
          descriptor.writable = true;
        Object.defineProperty(target, descriptor.key, descriptor);
      }
    }
    function popover_createClass(Constructor, protoProps, staticProps) {
      if (protoProps)
        popover_defineProperties(Constructor.prototype, protoProps);
      if (staticProps)
        popover_defineProperties(Constructor, staticProps);
      Object.defineProperty(Constructor, "prototype", { writable: false });
      return Constructor;
    }
    var popover_Default = {
      placement: "top",
      offset: 10,
      triggerType: "hover",
      onShow: function onShow() {
      },
      onHide: function onHide() {
      }
    };
    var Popover = /* @__PURE__ */ function() {
      function Popover2() {
        var targetEl = arguments.length > 0 && arguments[0] !== void 0 ? arguments[0] : null;
        var triggerEl = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : null;
        var options = arguments.length > 2 && arguments[2] !== void 0 ? arguments[2] : {};
        popover_classCallCheck(this, Popover2);
        this._targetEl = targetEl;
        this._triggerEl = triggerEl;
        this._options = popover_objectSpread(popover_objectSpread({}, popover_Default), options);
        this._popperInstance = this._createPopperInstace();
        this._init();
      }
      popover_createClass(Popover2, [{
        key: "_init",
        value: function _init() {
          var _this = this;
          if (this._triggerEl) {
            var triggerEvents = this._getTriggerEvents();
            triggerEvents.showEvents.forEach(function(ev) {
              _this._triggerEl.addEventListener(ev, function() {
                _this.show();
              });
              _this._targetEl.addEventListener(ev, function() {
                _this.show();
              });
            });
            triggerEvents.hideEvents.forEach(function(ev) {
              _this._triggerEl.addEventListener(ev, function() {
                setTimeout(function() {
                  if (!_this._targetEl.matches(":hover")) {
                    _this.hide();
                  }
                }, 100);
              });
              _this._targetEl.addEventListener(ev, function() {
                setTimeout(function() {
                  if (!_this._triggerEl.matches(":hover")) {
                    _this.hide();
                  }
                }, 100);
              });
            });
          }
        }
      }, {
        key: "_createPopperInstace",
        value: function _createPopperInstace() {
          return popper_createPopper(this._triggerEl, this._targetEl, {
            placement: this._options.placement,
            modifiers: [{
              name: "offset",
              options: {
                offset: [0, this._options.offset]
              }
            }]
          });
        }
      }, {
        key: "_getTriggerEvents",
        value: function _getTriggerEvents() {
          switch (this._options.triggerType) {
            case "hover":
              return {
                showEvents: ["mouseenter", "focus"],
                hideEvents: ["mouseleave", "blur"]
              };
            case "click":
              return {
                showEvents: ["click", "focus"],
                hideEvents: ["focusout", "blur"]
              };
            default:
              return {
                showEvents: ["mouseenter", "focus"],
                hideEvents: ["mouseleave", "blur"]
              };
          }
        }
      }, {
        key: "show",
        value: function show() {
          this._targetEl.classList.remove("opacity-0", "invisible");
          this._targetEl.classList.add("opacity-100", "visible");
          this._popperInstance.setOptions(function(options) {
            return popover_objectSpread(popover_objectSpread({}, options), {}, {
              modifiers: [].concat(popover_toConsumableArray(options.modifiers), [{
                name: "eventListeners",
                enabled: true
              }])
            });
          });
          this._popperInstance.update();
          this._options.onShow(this);
        }
      }, {
        key: "hide",
        value: function hide2() {
          this._targetEl.classList.remove("opacity-100", "visible");
          this._targetEl.classList.add("opacity-0", "invisible");
          this._popperInstance.setOptions(function(options) {
            return popover_objectSpread(popover_objectSpread({}, options), {}, {
              modifiers: [].concat(popover_toConsumableArray(options.modifiers), [{
                name: "eventListeners",
                enabled: false
              }])
            });
          });
          this._options.onHide(this);
        }
      }]);
      return Popover2;
    }();
    window.Popover = Popover;
    function initPopover() {
      document.querySelectorAll("[data-popover-target]").forEach(function(triggerEl) {
        var targetEl = document.getElementById(triggerEl.getAttribute("data-popover-target"));
        var triggerType = triggerEl.getAttribute("data-popover-trigger");
        var placement = triggerEl.getAttribute("data-popover-placement");
        var offset2 = triggerEl.getAttribute("data-popover-offset");
        new Popover(targetEl, triggerEl, {
          placement: placement ? placement : popover_Default.placement,
          offset: offset2 ? parseInt(offset2) : popover_Default.offset,
          triggerType: triggerType ? triggerType : popover_Default.triggerType
        });
      });
    }
    if (document.readyState !== "loading") {
      initPopover();
    } else {
      document.addEventListener("DOMContentLoaded", initPopover);
    }
    const popover = Popover;
    ;
    const flowbite = {
      Accordion: accordion,
      Collapse: collapse,
      Carousel: carousel,
      Dismiss: dismiss,
      Dropdown: dropdown,
      Modal: modal,
      Drawer: drawer,
      Tabs: tabs,
      Tooltip: tooltip,
      Popover: popover
    };
  })();

  // Web/Scripts.app/EMCS/src/EMCS.js
  var sidebarNavScrollbar = new perfect_scrollbar_esm_default(".sidebar-nav--inner", {
    wheelSpeed: 1,
    wheelPropagation: true,
    minScrollbarLength: 20
  });
  var mainContentScrollbar = new perfect_scrollbar_esm_default(".main-content", {
    wheelSpeed: 1,
    wheelPropagation: true,
    minScrollbarLength: 20
  });
  var themeToggleDarkIcon = document.getElementById("theme-toggle-dark-icon");
  var themeToggleLightIcon = document.getElementById("theme-toggle-light-icon");
  if (localStorage.getItem("color-theme") === "dark" || !("color-theme" in localStorage) && window.matchMedia("(prefers-color-scheme: dark)").matches) {
    themeToggleLightIcon.classList.remove("hidden");
  } else {
    themeToggleDarkIcon.classList.remove("hidden");
  }
  var themeToggleBtn = document.getElementById("theme-toggle");
  themeToggleBtn.addEventListener("click", function() {
    themeToggleDarkIcon.classList.toggle("hidden");
    themeToggleLightIcon.classList.toggle("hidden");
    if (localStorage.getItem("color-theme")) {
      if (localStorage.getItem("color-theme") === "light") {
        document.documentElement.classList.add("dark");
        localStorage.setItem("color-theme", "dark");
      } else {
        document.documentElement.classList.remove("dark");
        localStorage.setItem("color-theme", "light");
      }
    } else {
      if (document.documentElement.classList.contains("dark")) {
        document.documentElement.classList.remove("dark");
        localStorage.setItem("color-theme", "light");
      } else {
        document.documentElement.classList.add("dark");
        localStorage.setItem("color-theme", "dark");
      }
    }
  });
})();
/*!
 * perfect-scrollbar v1.5.3
 * Copyright 2021 Hyunje Jun, MDBootstrap and Contributors
 * Licensed under MIT
 */
