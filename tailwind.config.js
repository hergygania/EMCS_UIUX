/** @type {import('tailwindcss').Config} */
const defaultTheme = require("tailwindcss/defaultTheme");
module.exports = {
  content: [
    "./Web/Views/EMCS/**/*.cshtml",
    "./Web/Scripts.app/EMCS/**/*.js",
    "./node_modules/flowbite/**/*.js",
  ],
  theme: {
    extend: {
      fontFamily: {
        sans: ["Inter", ...defaultTheme.fontFamily.sans],
      },
      colors: {
        primary: "#FE9D01",
      },
    },
  },
  darkMode: "class",
  plugins: [require("flowbite/plugin")],
};
