document.addEventListener("DOMContentLoaded", function () {
    const mainElement = document.querySelector("main");
    const mainWidth = mainElement.offsetWidth;
    document.documentElement.style.setProperty('--main-width', `${mainWidth}px`);
});