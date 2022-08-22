import PerfectScrollbar from "perfect-scrollbar";
import "flowbite";

/*
 * Perfect Scrollbar
 * https://perfectscrollbar.com/
 */
const sidebarNavScrollbar = new PerfectScrollbar(".sidebar-nav--inner", {
  wheelSpeed: 1,
  wheelPropagation: true,
  minScrollbarLength: 20,
});
const mainContentScrollbar = new PerfectScrollbar(".main-content", {
  wheelSpeed: 1,
  wheelPropagation: true,
  minScrollbarLength: 20,
});
// const tableScrollbar = new PerfectScrollbar(
//   "#tab-task-cargo .fixed-table-body",
//   {
//     wheelSpeed: 1,
//     wheelPropagation: true,
//     minScrollbarLength: 20,
//   }
// );

/*
 * Toggle Dark Mode
 * https://flowbite.com/docs/customize/dark-mode/
 */
var themeToggleDarkIcon = document.getElementById("theme-toggle-dark-icon");
var themeToggleLightIcon = document.getElementById("theme-toggle-light-icon");

// Change the icons inside the button based on previous settings
if (
  localStorage.getItem("color-theme") === "dark" ||
  (!("color-theme" in localStorage) &&
    window.matchMedia("(prefers-color-scheme: dark)").matches)
) {
  themeToggleLightIcon.classList.remove("hidden");
} else {
  themeToggleDarkIcon.classList.remove("hidden");
}

var themeToggleBtn = document.getElementById("theme-toggle");

themeToggleBtn.addEventListener("click", function () {
  // toggle icons inside button
  themeToggleDarkIcon.classList.toggle("hidden");
  themeToggleLightIcon.classList.toggle("hidden");

  // if set via local storage previously
  if (localStorage.getItem("color-theme")) {
    if (localStorage.getItem("color-theme") === "light") {
      document.documentElement.classList.add("dark");
      localStorage.setItem("color-theme", "dark");
    } else {
      document.documentElement.classList.remove("dark");
      localStorage.setItem("color-theme", "light");
    }

    // if NOT set via local storage previously
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

/*
 * App jQuery scripts
 */
// $(function () {
//   /*
//    * Sidebar navigation
//    */
//   $("#main-container").toggleClass(localStorage.sidebarCollapsed);

//   $("#sidebar-toggle").on("click", function (e) {
//     e.preventDefault();
//     $("#main-container").toggleClass("sidebar-collapsed");
//     if (localStorage.sidebarCollapsed != "sidebar-collapsed") {
//       $("#main-container").toggleClass("sidebar-collapsed", true);
//       localStorage.sidebarCollapsed = "sidebar-collapsed";
//     } else {
//       $("#main-container").toggleClass("sidebar-collapsed", false);
//       localStorage.sidebarCollapsed = "";
//     }
//   });

//   $(".nav-collapse").on("click", function (e) {
//     e.preventDefault();
//     $(this).toggleClass("nav-collapse--active");
//     $(this).next().slideToggle();
//   });

//   $(document).on(
//     "mouseover mouseout",
//     ".sidebar-collapsed #sidebar-nav",
//     function () {
//       $(".sidebar-collapsed").toggleClass("sidebar-hovered");
//     }
//   );
// });
