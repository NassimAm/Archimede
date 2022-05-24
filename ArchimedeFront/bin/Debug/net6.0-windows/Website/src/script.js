const navlinks = document.getElementById("navlinks");
const menu = document.getElementById("menu");

function showmenu() {
    navlinks.style.right = "0";
    menu.style.display = "none";
}
function hidemenu() {
    navlinks.style.right = "-200px";
    menu.style.display = "block";
}