/** @type {import('tailwindcss').Config} */
module.exports = {

  content: ["./Pages/**/*.razor",
    "./Shared/**/*.razor",
    "./wwwroot/**/*.html"],
  theme: {
    extend: {},
  },
  variants:{
    extend:{}
  },
  plugins: [
    require('@tailwindcss/forms'),
  ],
}

