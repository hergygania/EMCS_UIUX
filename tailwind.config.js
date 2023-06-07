/** @type {import('tailwindcss').Config} */
const defaultTheme = require("tailwindcss/defaultTheme");
module.exports = {
  content: ["./Web/Views/EMCS/**/*.cshtml", "./Web/Scripts.app/EMCS/**/*.js", "./node_modules/flowbite/**/*.js"],
  safelist: ["blur-none", "blur-sm", "filter-none"],
  theme: {
    extend: {
      fontFamily: {
        sans: ["Inter", ...defaultTheme.fontFamily.sans],
      },
      colors: {
        primary: "#FE9D01",
        //primary: "#FF7A00",
        brand: {
          primary: "#FF7A00",
          secondary: "#F6B221",
          orange: {
            light: "#FFCA8A",
            DEFAULT: "#FF7A00",
          },
          yellow: {
            light: "#FFF1CB",
            DEFAULT: "#F6B221",
          },
          black: {
            DEFAULT: "#0F1821",
            medium: "#353535",
            light: "#898989",
            lighter: "#C9C9C9",
          },
        },
      },
      animation: {
        "spin-slow": "spin 3s linear infinite",
      },
    },
  },
  darkMode: "class",
  plugins: [require("flowbite/plugin")],
};
